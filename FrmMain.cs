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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using AMS.Profile;

namespace Classless.Verifier {
	/// <summary>The main window of the application.</summary>
	public class FrmMain : Form {
		#region Control Declarations
		private ProgressStatusBar barStatus;

		private System.Windows.Forms.OpenFileDialog dialogOpen;
		private System.Windows.Forms.MainMenu mnuMain;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuFileOpen;
		private System.Windows.Forms.MenuItem mnuFileVerify;
		private System.Windows.Forms.MenuItem mnuFileViewLog;
		private System.Windows.Forms.MenuItem mnuFileExit;
		private System.Windows.Forms.MenuItem mnuTools;
		private System.Windows.Forms.MenuItem mnuToolsCalculator;
		private System.Windows.Forms.MenuItem mnuToolsCreateFileList;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuHelpHomePage;
		private System.Windows.Forms.MenuItem mnuHelpXMLHomePage;
		private System.Windows.Forms.MenuItem mnuHelpClassless;
		private System.Windows.Forms.MenuItem mnuHelpMPL;
		private System.Windows.Forms.MenuItem mnuHelpAbout;
		private System.Windows.Forms.ContextMenu mnuFiles;
		private System.Windows.Forms.MenuItem mnuFilesIgnoreProcessing;
		private System.Windows.Forms.MenuItem mnuFilesClearResults;
		private System.Windows.Forms.MenuItem mnuFilesUnIgnoreAll;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.StatusBarPanel panelStatus;
		private System.Windows.Forms.StatusBarPanel panelProgress;
		private System.Windows.Forms.ListView listFiles;
		private System.Windows.Forms.ColumnHeader colFilesFile;
		private System.Windows.Forms.ColumnHeader colFilesHash;
		private System.Windows.Forms.ColumnHeader colFilesType;
		private System.Windows.Forms.Button btnVerify;
		private System.Windows.Forms.ImageList ilFileIcons;
		private System.ComponentModel.IContainer components;
		#endregion


		// The currently open file list.
		private FileList fileList;

		// The current program options.
		private Config cfg;

		// Tallies of the file list's processing.
		private int fileListGood;
		private int fileListBad;
		private int fileListMissing;
		private int fileListIgnored;

		// A flag so we can tell when this window has loaded.
		private bool firstLoad;

		// File list processor objects.
		private FileListProcessor listProcessor;
		private System.Windows.Forms.MenuItem mnuToolsOptions;
		private Thread listProcessorThread;


		/// <summary>Gets or sets the current file list.</summary>
		public FileList FileList {
			get { return fileList; }
			set {
				fileList = value;

				// Clear out the file list.
				listFiles.Items.Clear();

				// Load the file list with the new files (if given any).
				if (fileList != null) {
					ListViewItem lvi;
					foreach(FileListFile f in fileList.filelist) {
						lvi = new ListViewItem(f.name);
						lvi.SubItems.Add(Algorithm.GetShortNameFromType(f.type));
						lvi.SubItems.Add(f.Value.ToUpper(System.Globalization.CultureInfo.InvariantCulture));
						lvi.Tag = f;
						lvi.ImageIndex = 0;
						listFiles.Items.Add(lvi);
					}

					panelStatus.Text = listFiles.Items.Count.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + " Files";
				} else {
					panelStatus.Text = string.Empty;
				}
			}
		}


		/// <summary>Initializes an instance of FrmMain.</summary>
		public FrmMain() {
			// Initialize some variables.
			firstLoad = true;
			cfg = new Config();

			// Build the form.
			InitializeComponent();
			StartEventProcessing();

			// Establish last window settings.
			this.Size = new Size(
				cfg.GetValue("MainWindow", "Width", this.Size.Width),
				cfg.GetValue("MainWindow", "Height", this.Size.Height)
			);
			this.Location = new Point(
				cfg.GetValue("MainWindow", "X", ((Screen.PrimaryScreen.Bounds.Width - this.Size.Width) / 2)),
				cfg.GetValue("MainWindow", "Y", ((Screen.PrimaryScreen.Bounds.Height - this.Size.Height) / 2))
			);
			this.colFilesFile.Width = cfg.GetValue("MainWindow", "ListFileWidth", this.colFilesFile.Width);
			this.colFilesType.Width = cfg.GetValue("MainWindow", "ListTypeWidth", this.colFilesType.Width);
			this.colFilesHash.Width = cfg.GetValue("MainWindow", "ListHashWidth", this.colFilesHash.Width);

			// Establish last window settings for the log.
			FrmLog.Instance.Size = new Size(
				cfg.GetValue("LogWindow", "Width", FrmLog.Instance.Size.Width),
				cfg.GetValue("LogWindow", "Height", FrmLog.Instance.Size.Height)
			);
			FrmLog.Instance.Location = new Point(
				cfg.GetValue("LogWindow", "X", 0),
				cfg.GetValue("LogWindow", "Y", 0)
			);
		}


		/// <summary>Clean up any resources being used.</summary>
		protected override void Dispose(bool disposing) {
			if ((disposing) && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}


		// Opens a URL in the default browser.
		private void OpenURL(string url) {
			System.Diagnostics.Process.Start(url);
		}


		// Lock the GUI so the user can't do anything harmful while we're processing.
		private void LockGUI() {
			btnVerify.Text = "Processing list. Press again to stop.";
			mnuFileOpen.Enabled = false;
			mnuFileVerify.Enabled = false;
			mnuFileExit.Enabled = false;
			this.Cursor = Cursors.AppStarting;
			listFiles.Cursor = Cursors.AppStarting;
			Application.DoEvents();
		}


		// Unlock the GUI.
		private void UnlockGUI() {
			btnVerify.Text = "&Verify Files";
			mnuFileOpen.Enabled = true;
			mnuFileVerify.Enabled = true;
			mnuFileExit.Enabled = true;
			this.Cursor = Cursors.Default;
			listFiles.Cursor = Cursors.Default;
			this.Text = "Verifier";
			Application.DoEvents();
		}


		// Save information about the windows' size, etc.
		private void SaveSettings() {
			if (cfg.GetValue("Program", "RememberWindowSettings", true)) {
				// Main window.
				if (this.WindowState == FormWindowState.Normal) {
					cfg.SetValue("MainWindow", "Width", this.Size.Width);
					cfg.SetValue("MainWindow", "Height", this.Size.Height);
					cfg.SetValue("MainWindow", "X", this.Location.X);
					cfg.SetValue("MainWindow", "Y", this.Location.Y);
					cfg.SetValue("MainWindow", "ListFileWidth", this.colFilesFile.Width);
					cfg.SetValue("MainWindow", "ListTypeWidth", this.colFilesType.Width);
					cfg.SetValue("MainWindow", "ListHashWidth", this.colFilesHash.Width);
				}

				// Log window.
				if (FrmLog.Instance.WindowState == FormWindowState.Normal) {
					cfg.SetValue("LogWindow", "Width", FrmLog.Instance.Size.Width);
					cfg.SetValue("LogWindow", "Height", FrmLog.Instance.Size.Height);
					cfg.SetValue("LogWindow", "X", FrmLog.Instance.Location.X);
					cfg.SetValue("LogWindow", "Y", FrmLog.Instance.Location.Y);
				}
			} else {
				cfg.RemoveSection("MainWindow");
				cfg.RemoveSection("LogWindow");
			}
		}


		// Open a file list.
		private void OpenFileList(string filename) {
			try {
				FileList = new FileList(filename);
			} catch (FileTypeException) {
				// Couldn't figure it out, ask the user what they want to do.
				FrmChooseType fct = new FrmChooseType();
				if (fct.ShowDialog(this) == DialogResult.Cancel) { return; }

				try {
					FileList = new FileList(filename, fct.FileListType);
				} catch (FileTypeException) {
					// Unknown format.
					MessageBox.Show(filename + " is not a supported file verification list format.",
						"Verifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
				} catch (Exception ex) {
					throw ex;
				}
			} catch (Exception ex) {
				// Bad error.
				MessageBox.Show("Could not open file verification list:" +
					Environment.NewLine + Environment.NewLine +
					filename +
					Environment.NewLine + Environment.NewLine +
					ex.Message,
					"Verifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			// In case nothing shows up, let the user know that we tried, but didn't find anything.
			if (FileList.filelist.Count == 0) {
				MessageBox.Show("Could not find any entries in " +
					filename,
					"Verifier", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}


		// Clear the results of a previous processing.
		private void ClearResults() {
			panelStatus.Text = string.Empty;
			barStatus.ProgressBar.Value = 0;
			foreach (ListViewItem lvi in listFiles.Items) {
				lvi.ImageIndex = 0;
			}
			Application.DoEvents();
			fileListGood = fileListBad = fileListMissing = fileListIgnored = 0;
			FrmLog.Instance.ClearLines();
		}


		#region Events
		// Hook into the event processing.
		private void StartEventProcessing() {
			this.Activated += new System.EventHandler(this.FrmMain_Activated);
			this.Closing += new CancelEventHandler(this.FrmMain_Closing);
			listFiles.DragEnter += new DragEventHandler(this.listFiles_DragEnter);
			listFiles.DragDrop += new DragEventHandler(this.listFiles_DragDrop);
			mnuFilesIgnoreProcessing.Click += new System.EventHandler(this.mnuFilesIgnoreProcessing_Click);
			mnuFilesUnIgnoreAll.Click += new System.EventHandler(this.mnuFilesUnIgnoreAll_Click);
			mnuFilesClearResults.Click += new System.EventHandler(this.mnuFilesClearResults_Click);
			btnVerify.Click += new System.EventHandler(this.initiateVerify);
			mnuFileOpen.Click += new System.EventHandler(this.mnuFileOpen_Click);
			mnuFileVerify.Click += new System.EventHandler(this.initiateVerify);
			mnuFileViewLog.Click += new System.EventHandler(this.mnuFileViewLog_Click);
			mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
			mnuToolsCalculator.Click += new System.EventHandler(this.mnuToolsCalculator_Click);
			mnuToolsOptions.Click += new System.EventHandler(this.mnuToolsOptions_Click);
			mnuHelpHomePage.Click += new System.EventHandler(this.mnuHelpHomePage_Click);
			mnuHelpXMLHomePage.Click += new System.EventHandler(this.mnuHelpXMLHomePage_Click);
			mnuHelpClassless.Click += new System.EventHandler(this.mnuHelpClassless_Click);
			mnuHelpMPL.Click += new System.EventHandler(this.mnuHelpMPL_Click);
			mnuHelpAbout.Click += new System.EventHandler(this.mnuHelpAbout_Click);
		}

		// The window has been activated.
		private void FrmMain_Activated(object sender, System.EventArgs args) {
			if (firstLoad) {
				firstLoad = false;
				if ((FileList != null) && (cfg.GetValue("Program", "ProcessListOnApplicationLoad", true))) {
					initiateVerify(this, args);
				}
			}
		}

		// The user is trying to exit the application.
		private void FrmMain_Closing(object sender, CancelEventArgs args) {
			// See if they're trying to abort.
			if (listProcessorThread != null) {
				if (cfg.GetValue("Program", "ConfirmApplicationExit", true)) {
					if (MessageBox.Show("Stop processing the current list?", "Verify", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No) {

						// We have to check if it's still processing, in case they took too long with the messagebox.
						if (listProcessorThread != null) {
							listProcessorThread.Abort();
							processorCompletion(sender, new CompletionEventArgs());
						}
					} else {
						args.Cancel = true;
						return;
					}
				} else {
					listProcessorThread.Abort();
				}
			}

			SaveSettings();
		}

		// User wants to drag and drop a filelist into the application.
		private void listFiles_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true) {
				e.Effect = DragDropEffects.All;
			}
		}

		// User has dropped the file into the list.
		private void listFiles_DragDrop(object sender, DragEventArgs args) {
			OpenFileList(((string[])args.Data.GetData(DataFormats.FileDrop))[0]);
		}

		// User wants to toggle the ignore status of some files.
		private void mnuFilesIgnoreProcessing_Click(object sender, EventArgs args) {
			foreach (ListViewItem lvi in listFiles.SelectedItems) {
				if (((FileListFile)lvi.Tag).Ignore) {
					lvi.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
					((FileListFile)lvi.Tag).Ignore = false;
				} else {
					lvi.ForeColor = Color.FromKnownColor(KnownColor.InactiveCaptionText);
					((FileListFile)lvi.Tag).Ignore = true;
				}
			}
		}

		// User wants to un-ignore all files.
		private void mnuFilesUnIgnoreAll_Click(object sender, EventArgs args) {
			foreach (ListViewItem lvi in listFiles.Items) {
				if (((FileListFile)lvi.Tag).Ignore) {
					lvi.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
					((FileListFile)lvi.Tag).Ignore = false;
				}
			}
		}

		// User wants to clear the result icons.
		private void mnuFilesClearResults_Click(object sender, EventArgs args) {
			ClearResults();
		}

		// User wants to verify the current file list.
		private void initiateVerify(object sender, System.EventArgs args) {
			// Make sure there's a list to work with.
			if (FileList == null) {
				MessageBox.Show("No file verification list has been opened.", "Verify", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// See if they're trying to abort.
			if (listProcessorThread != null) {
				if (MessageBox.Show("Stop processing the current list?", "Verify", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

				// We have to check if it's still processing, in case they took too long with the messagebox.
				if (listProcessorThread != null) {
					listProcessorThread.Abort();
				}
				return;
			}

			// Clear things up in preparation.
			ClearResults();

			// Set up the processor.
			listProcessor = new FileListProcessor(FileList);
			listProcessor.Progress += new FileListProcessor.ProgressEventHandler(this.processorProgress);
			listProcessor.Completion += new FileListProcessor.CompletionEventHandler(this.processorCompletion);

			// Start the log.
			FrmLog.Instance.AddHeader();
			FrmLog.Instance.AddLine("      File List: " + FileList.sourceFile);
			FrmLog.Instance.AddLine("      List Type: " + FileList.type.ToString());
			FrmLog.Instance.AddLine("          Start: " + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz"));
			FrmLog.Instance.AddLine();

			// Lock up the GUI.
			LockGUI();

			// Start the processor.
			listProcessorThread = new Thread(new ThreadStart(listProcessor.Start));
			listProcessorThread.IsBackground = true;
			listProcessorThread.Priority = (ThreadPriority)Enum.Parse(typeof(ThreadPriority), cfg.GetValue("Program", "ProcessingThreadPriority", "Normal"));
			listProcessorThread.Start();
		}

		// The FileListProcessor has made some progress.
		private void processorProgress(object sender, ProgressEventArgs args) {
			foreach(ListViewItem lvi in listFiles.Items) {
				if (lvi.Tag == args.File) {
					// Show the status.
					switch (args.Status) {
						case FileListProcessorStatus.Good: lvi.ImageIndex = 1; fileListGood++; break;
						case FileListProcessorStatus.Bad: lvi.ImageIndex = 2; fileListBad++; break;
						case FileListProcessorStatus.NotFound: lvi.ImageIndex = 3; fileListMissing++; break;
						case FileListProcessorStatus.Error: lvi.ImageIndex = 2; fileListBad++; break;
						case FileListProcessorStatus.InProcess: lvi.ImageIndex = 4; break;
						case FileListProcessorStatus.WrongSize: lvi.ImageIndex = 2; fileListBad++; break;
						case FileListProcessorStatus.Ignored: fileListIgnored++; break;
					}

					// Scroll if needed.
					lvi.EnsureVisible();

					// Show the overall progress.
					this.Text = "Verifier - Processing (" + args.PercentCompleteTotal.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + "%)";
					barStatus.ProgressBar.Value = args.PercentCompleteTotal;
					panelStatus.Text = listFiles.Items.Count.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + " Files: " +
						fileListGood.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + " Good / " +
						fileListBad.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + " Bad / " +
						fileListMissing.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + " Missing";

					return;
				}
			}
		}

		// The FileListProcessor completed.
		private void processorCompletion(object sender, CompletionEventArgs args) {
			listProcessor = null;
			listProcessorThread = null;

			// Open the GUI.
			UnlockGUI();
			barStatus.ProgressBar.Value = 0;

			// Clear out the icon of the file that was marked as in-process.
			for (int i = 0; i < listFiles.Items.Count; i++) {
				if (listFiles.Items[i].ImageIndex == 4) {
					listFiles.Items[i].ImageIndex = 0;
					break;
				}
			}

			// If there's an error, show it to the user.
			if (args.Exception != null) {
				FrmLog.Instance.AddLine();

				if (args.Exception is ThreadAbortException) {
					FrmLog.Instance.AddLine("--Processing Aborted by User--");
				} else {
					FrmLog.Instance.AddLine("--Error: " + args.Exception.Message + "--");
					MessageBox.Show("There was an error while processing the files:" +
						Environment.NewLine + Environment.NewLine +
						args.Exception.Message,
						"Verify", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			// Put final results into the log.
			FrmLog.Instance.AddLine();
			FrmLog.Instance.AddLine("    Total Files: " + listFiles.Items.Count.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
			FrmLog.Instance.AddLine("Files Processed: " + (fileListGood + fileListBad + fileListMissing + fileListIgnored).ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
			FrmLog.Instance.AddLine("     Good Files: " + fileListGood.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
			FrmLog.Instance.AddLine("      Bad Files: " + fileListBad.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
			FrmLog.Instance.AddLine("  Missing Files: " + fileListMissing.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
			FrmLog.Instance.AddLine("       Finished: " + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz"));

			// Save the log.
			if (cfg.GetValue("Program", "AutomaticallySaveLog", false)) {
				FrmLog.Instance.Save(Path.ChangeExtension(FileList.sourceFile, ".log"));
			}
		}

		// User wants to open a file verification list.
		private void mnuFileOpen_Click(object sender, System.EventArgs args) {
			DialogResult dr = dialogOpen.ShowDialog(this);

			if (dr == DialogResult.OK) {
				OpenFileList(dialogOpen.FileName);
			}
		}

		// User wants to view the processing log.
		private void mnuFileViewLog_Click(object sender, System.EventArgs args) {
			FrmLog.Instance.Show();
			FrmLog.Instance.Activate();
		}

		// User wants to quit.
		private void mnuFileExit_Click(object sender, System.EventArgs args) {
			Application.Exit();
		}

		// User wants to use the hash calculator.
		private void mnuToolsCalculator_Click(object sender, System.EventArgs args) {
			FrmCalculator fc = new FrmCalculator();
			fc.Show();
		}

		// User wants manage the program options.
		private void mnuToolsOptions_Click(object sender, System.EventArgs args) {
			FrmOptions fo = new FrmOptions();
			fo.ShowDialog(this);
		}

		// User wants to view the Verifier home page.
		private void mnuHelpHomePage_Click(object sender, System.EventArgs args) {
			OpenURL(Verifier.GetHomePage());
		}

		// User wants to view the VerifyXML home page.
		private void mnuHelpXMLHomePage_Click(object sender, System.EventArgs args) {
			OpenURL("http://www.classless.net/projects/verifyxml/");
		}

		// User wants to view the Classless.net home page.
		private void mnuHelpClassless_Click(object sender, System.EventArgs args) {
			OpenURL("http://www.classless.net/");
		}

		// User wants to view the MPL.
		private void mnuHelpMPL_Click(object sender, System.EventArgs args) {
			OpenURL("http://www.mozilla.org/MPL/MPL-1.1.html");
		}

		// User wants to view the about window.
		private void mnuHelpAbout_Click(object sender, System.EventArgs args) {
			FrmAbout fa = new FrmAbout();
			fa.ShowDialog(this);
		}
		#endregion


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmMain));
			this.mnuMain = new System.Windows.Forms.MainMenu();
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuFileOpen = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mnuFileVerify = new System.Windows.Forms.MenuItem();
			this.mnuFileViewLog = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.mnuFileExit = new System.Windows.Forms.MenuItem();
			this.mnuTools = new System.Windows.Forms.MenuItem();
			this.mnuToolsCalculator = new System.Windows.Forms.MenuItem();
			this.mnuToolsCreateFileList = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuHelpHomePage = new System.Windows.Forms.MenuItem();
			this.mnuHelpXMLHomePage = new System.Windows.Forms.MenuItem();
			this.mnuHelpClassless = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.mnuHelpMPL = new System.Windows.Forms.MenuItem();
			this.mnuHelpAbout = new System.Windows.Forms.MenuItem();
			this.barStatus = new Classless.Verifier.ProgressStatusBar();
			this.panelStatus = new System.Windows.Forms.StatusBarPanel();
			this.panelProgress = new System.Windows.Forms.StatusBarPanel();
			this.dialogOpen = new System.Windows.Forms.OpenFileDialog();
			this.listFiles = new System.Windows.Forms.ListView();
			this.colFilesFile = new System.Windows.Forms.ColumnHeader();
			this.colFilesType = new System.Windows.Forms.ColumnHeader();
			this.colFilesHash = new System.Windows.Forms.ColumnHeader();
			this.mnuFiles = new System.Windows.Forms.ContextMenu();
			this.mnuFilesIgnoreProcessing = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mnuFilesUnIgnoreAll = new System.Windows.Forms.MenuItem();
			this.mnuFilesClearResults = new System.Windows.Forms.MenuItem();
			this.ilFileIcons = new System.Windows.Forms.ImageList(this.components);
			this.btnVerify = new System.Windows.Forms.Button();
			this.mnuToolsOptions = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.panelStatus)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelProgress)).BeginInit();
			this.SuspendLayout();
			// 
			// mnuMain
			// 
			this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFile,
																					this.mnuTools,
																					this.mnuHelp});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFileOpen,
																					this.menuItem4,
																					this.mnuFileVerify,
																					this.mnuFileViewLog,
																					this.menuItem7,
																					this.mnuFileExit});
			this.mnuFile.Text = "&File";
			// 
			// mnuFileOpen
			// 
			this.mnuFileOpen.Index = 0;
			this.mnuFileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.mnuFileOpen.Text = "&Open...";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.Text = "-";
			// 
			// mnuFileVerify
			// 
			this.mnuFileVerify.Index = 2;
			this.mnuFileVerify.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.mnuFileVerify.Text = "&Verify List";
			// 
			// mnuFileViewLog
			// 
			this.mnuFileViewLog.Index = 3;
			this.mnuFileViewLog.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
			this.mnuFileViewLog.Text = "View &Log";
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 4;
			this.menuItem7.Text = "-";
			// 
			// mnuFileExit
			// 
			this.mnuFileExit.Index = 5;
			this.mnuFileExit.Shortcut = System.Windows.Forms.Shortcut.AltF4;
			this.mnuFileExit.Text = "E&xit";
			// 
			// mnuTools
			// 
			this.mnuTools.Index = 1;
			this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuToolsCalculator,
																					 this.mnuToolsCreateFileList,
																					 this.mnuToolsOptions});
			this.mnuTools.Text = "&Tools";
			// 
			// mnuToolsCalculator
			// 
			this.mnuToolsCalculator.Index = 0;
			this.mnuToolsCalculator.Text = "&Calculator";
			// 
			// mnuToolsCreateFileList
			// 
			this.mnuToolsCreateFileList.Enabled = false;
			this.mnuToolsCreateFileList.Index = 1;
			this.mnuToolsCreateFileList.Text = "Create File &List";
			// 
			// mnuHelp
			// 
			this.mnuHelp.Index = 2;
			this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuHelpHomePage,
																					this.mnuHelpXMLHomePage,
																					this.mnuHelpClassless,
																					this.menuItem5,
																					this.mnuHelpMPL,
																					this.mnuHelpAbout});
			this.mnuHelp.Text = "&Help";
			// 
			// mnuHelpHomePage
			// 
			this.mnuHelpHomePage.Index = 0;
			this.mnuHelpHomePage.Text = "&Verifier Home Page";
			// 
			// mnuHelpXMLHomePage
			// 
			this.mnuHelpXMLHomePage.Index = 1;
			this.mnuHelpXMLHomePage.Text = "Verify&XML Home Page";
			// 
			// mnuHelpClassless
			// 
			this.mnuHelpClassless.Index = 2;
			this.mnuHelpClassless.Text = "&Classless.net";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 3;
			this.menuItem5.Text = "-";
			// 
			// mnuHelpMPL
			// 
			this.mnuHelpMPL.Index = 4;
			this.mnuHelpMPL.Text = "&Mozilla Public License";
			// 
			// mnuHelpAbout
			// 
			this.mnuHelpAbout.Index = 5;
			this.mnuHelpAbout.Text = "&About";
			// 
			// barStatus
			// 
			this.barStatus.Location = new System.Drawing.Point(0, 233);
			this.barStatus.Name = "barStatus";
			this.barStatus.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.panelStatus,
																						 this.panelProgress});
			this.barStatus.ProgressBarPanel = 1;
			this.barStatus.ShowPanels = true;
			this.barStatus.Size = new System.Drawing.Size(552, 20);
			this.barStatus.TabIndex = 0;
			// 
			// panelStatus
			// 
			this.panelStatus.MinWidth = 230;
			this.panelStatus.Width = 230;
			// 
			// panelProgress
			// 
			this.panelProgress.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.panelProgress.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.panelProgress.Width = 306;
			// 
			// dialogOpen
			// 
			this.dialogOpen.Filter = "All Supported (*.verify;*.vfy;*.sfv;*.md5;*.md5sum)|*.verify;*.vfy;*.sfv;*.md5;*." +
				"md5sum|VerifyXML File (*.verify;*.vfy)|*.verify;*.vfy|SFV File (*.sfv)|*.sfv|MD5" +
				" Checksums (*.md5;*.md5sum)|*.md5;*.md5sum|All Files (*.*)|*.*";
			this.dialogOpen.Title = "Open File Verification List";
			// 
			// listFiles
			// 
			this.listFiles.AllowDrop = true;
			this.listFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.colFilesFile,
																						this.colFilesType,
																						this.colFilesHash});
			this.listFiles.ContextMenu = this.mnuFiles;
			this.listFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listFiles.FullRowSelect = true;
			this.listFiles.GridLines = true;
			this.listFiles.Location = new System.Drawing.Point(0, 0);
			this.listFiles.Name = "listFiles";
			this.listFiles.Size = new System.Drawing.Size(552, 210);
			this.listFiles.SmallImageList = this.ilFileIcons;
			this.listFiles.TabIndex = 1;
			this.listFiles.View = System.Windows.Forms.View.Details;
			// 
			// colFilesFile
			// 
			this.colFilesFile.Text = "File";
			this.colFilesFile.Width = 240;
			// 
			// colFilesType
			// 
			this.colFilesType.Text = "Type";
			// 
			// colFilesHash
			// 
			this.colFilesHash.Text = "Hash";
			this.colFilesHash.Width = 230;
			// 
			// mnuFiles
			// 
			this.mnuFiles.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuFilesIgnoreProcessing,
																					 this.menuItem2,
																					 this.mnuFilesUnIgnoreAll,
																					 this.mnuFilesClearResults});
			// 
			// mnuFilesIgnoreProcessing
			// 
			this.mnuFilesIgnoreProcessing.Index = 0;
			this.mnuFilesIgnoreProcessing.Text = "Ignore Processing (Toggle)";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "-";
			// 
			// mnuFilesUnIgnoreAll
			// 
			this.mnuFilesUnIgnoreAll.Index = 2;
			this.mnuFilesUnIgnoreAll.Text = "Un-Ignore All";
			// 
			// mnuFilesClearResults
			// 
			this.mnuFilesClearResults.Index = 3;
			this.mnuFilesClearResults.Text = "Clear Results";
			// 
			// ilFileIcons
			// 
			this.ilFileIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ilFileIcons.ImageSize = new System.Drawing.Size(16, 16);
			this.ilFileIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilFileIcons.ImageStream")));
			this.ilFileIcons.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// btnVerify
			// 
			this.btnVerify.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnVerify.Location = new System.Drawing.Point(0, 210);
			this.btnVerify.Name = "btnVerify";
			this.btnVerify.Size = new System.Drawing.Size(552, 23);
			this.btnVerify.TabIndex = 2;
			this.btnVerify.Text = "&Verify Files";
			// 
			// mnuToolsOptions
			// 
			this.mnuToolsOptions.Index = 2;
			this.mnuToolsOptions.Text = "&Options";
			// 
			// FrmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(552, 253);
			this.Controls.Add(this.listFiles);
			this.Controls.Add(this.btnVerify);
			this.Controls.Add(this.barStatus);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mnuMain;
			this.Name = "FrmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Verifier";
			((System.ComponentModel.ISupportInitialize)(this.panelStatus)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelProgress)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
