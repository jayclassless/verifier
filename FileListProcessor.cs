// $Id$

#region License
/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Verifier.
 *
 * The Initial Developer of the Original Code is Classless.net.
 * Portions created by the Initial Developer are Copyright (C) 2004 the Initial
 * Developer. All Rights Reserved.
 *
 * Contributor(s):
 *		Jason Simeone (jay@classless.net)
 * 
 * ***** END LICENSE BLOCK ***** */
#endregion

using System;
using System.IO;
using System.Threading;

namespace Classless.Verifier {
	/// <summary>A class that controls the processing of a file verification list.</summary>
	public class FileListProcessor {
		private FileList fileList;
		private FileListFile currentFile;
		private int numFilesProcessed;
		private bool isProcessing;


		/// <summary>Whether or not the FileListProcessor is currently processing.</summary>
		public bool IsProcessing {
			get { return isProcessing; }
		}


		/// <summary>Represents the method that will handle the progress notification event.</summary>
		public delegate void ProgressEventHandler(object sender, ProgressEventArgs args);

		/// <summary>Represents the method that will handle the completion notification event.</summary>
		public delegate void CompletionEventHandler(object sender, CompletionEventArgs args);

		/// <summary>Occurs when a progress notification interval has been reached.</summary>
		public event ProgressEventHandler Progress;

		/// <summary>Occurs when the FileListProcessor has finished all processing.</summary>
		public event CompletionEventHandler Completion;


		/// <summary>Initializes an instances of FileListProcessor.</summary>
		/// <param name="fileList">The FileList that will be processed.</param>
		public FileListProcessor(FileList fileList) {
			this.fileList = fileList;
			isProcessing = false;
		}


		/// <summary>Start the processor.</summary>
 		public void Start() {
			if (IsProcessing) { return; }

			// Move to the directory of the file verification list.
			try {
				Environment.CurrentDirectory = new FileInfo(fileList.sourceFile).Directory.FullName;
			} catch (Exception ex) {
				OnCompletion(ex);
				return;
			}

			numFilesProcessed = 0;

			ProgressStream fs = null;
			string hash;
			System.Security.Cryptography.HashAlgorithm hasher;
			try {
				foreach(FileListFile f in fileList.filelist) {
					// Start logging the results.
					FrmLog.Instance.AddLine("           File: " + f.name);
					if (f.sizeSpecified) {
						FrmLog.Instance.AddLine("Documented Size: " + f.size);
					}
					FrmLog.Instance.AddLine("Documented Hash: " + f.Value.ToUpper(System.Globalization.CultureInfo.InvariantCulture));

					// See if we're supposed to ignore this file.
					if (f.Ignore) {
						FinishedFile(f, FileListProcessorStatus.Ignored);
						continue;
					}

					// Announce that we're starting on this file.
					OnProgress(f, FileListProcessorStatus.InProcess, 0, CalculatePercentage(0));
					currentFile = f;

					// Open the file.
					try {
						fs = new ProgressStream(File.OpenRead(f.name), 5);

						// If the file isn't the same size, it shouldn't be considered valid.
						FrmLog.Instance.AddLine("    Actual Size: " + fs.Length.ToString());
						if ((currentFile.sizeSpecified) && (currentFile.size != (ulong)fs.Length)) {
							fs.Close();
							FinishedFile(f, FileListProcessorStatus.WrongSize);
							continue;
						}

						fs.ProgressUpdate += new ProgressStream.ProgressUpdateEventHandler(this.fs_ProgressUpdate);
					} catch (FileNotFoundException) {
						FinishedFile(f, FileListProcessorStatus.NotFound);
						continue;
					} catch (Exception ex) {
						if (ex is ThreadAbortException) { throw; }
						FinishedFile(f, FileListProcessorStatus.Error);
						continue;
					}

					// Hash the file.
					try {
						hasher = Algorithm.GetHasherFromType(f.type);
						FrmLog.Instance.AddLine(" Hash Algorithm: " + f.type.ToString());
						hash = Classless.Hasher.Utilities.ByteToHexadecimal(hasher.ComputeHash(fs)).ToUpper(System.Globalization.CultureInfo.InvariantCulture);
						FrmLog.Instance.AddLine("Calculated Hash: " + hash);
					} catch (Exception ex) {
						if (ex is ThreadAbortException) { throw; }
						FinishedFile(f, FileListProcessorStatus.Error);
						continue;
					} finally {
						fs.Close();
					}

					// Check the hash.
					if (f.Value.ToUpper(System.Globalization.CultureInfo.InvariantCulture) == hash) {
						FinishedFile(f, FileListProcessorStatus.Good);
					} else {
						FinishedFile(f, FileListProcessorStatus.Bad);
					}
				}
			} catch (Exception e) {
				if (fs != null) { fs.Close(); }
				OnCompletion(e);
			} finally {
				isProcessing = false;
			}

			OnCompletion(null);
		}


		// Figures out the current total percentage.
		private int CalculatePercentage(int filePercentage) {
			int temp = (int)Math.Floor(((double)numFilesProcessed / fileList.filelist.Count) * 100.0);
			temp += (int)Math.Floor((1.0 / (double)fileList.filelist.Count) * (double)filePercentage);
			return temp;
		}


		// We've finished processing a file.
		private void FinishedFile(FileListFile file, FileListProcessorStatus status) {
			// Log the result.
			FrmLog.Instance.AddLine("         Status: " + status.ToString());
			FrmLog.Instance.AddLine();

			// Notify the GUI.
			OnProgress(file, status, 100, CalculatePercentage(100));
			numFilesProcessed++;
		}


		// Fires the Progress event.
		private void OnProgress(FileListFile file, FileListProcessorStatus status, int percentCompleteFile, int percentCompleteTotal) {
			if (Progress != null) {
				// Package our event arguments.
				ProgressEventArgs e = new ProgressEventArgs();
				e.File = file;
				e.Status = status;
				e.PercentCompleteFile = percentCompleteFile;
				e.PercentCompleteTotal = percentCompleteTotal;

				Progress(this, e);
			}
		}


		// Fires the Completion event.
		private void OnCompletion(Exception ex) {
			if (Completion != null) {
				// Package our event arguments.
				CompletionEventArgs e = new CompletionEventArgs();
				e.Exception = ex;

				Completion(this, e);
			}
		}

		// Got an update from the file stream reading.
		private void fs_ProgressUpdate(object sender, ProgressUpdateEventArgs args) {
			OnProgress(currentFile, FileListProcessorStatus.InProcess, args.PercentDone, CalculatePercentage(args.PercentDone));
		}
	}
}
