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

namespace Classless.Verifier {
	/// <summary>Wraps a FileStream providing a progress update regarding how much has been read.</summary>
	public class ProgressStream : Stream {
		private FileStream fs;
		private int currentProgress;
		private int lastNotifyProgress;
		private int notifyInterval;


		/// <summary>Represents the method that will handle the progress update notifications.</summary>
		public delegate void ProgressUpdateEventHandler(object sender, ProgressUpdateEventArgs args);

		/// <summary>Occurs when the notification interval has been reached.</summary>
		public event ProgressUpdateEventHandler ProgressUpdate;


		/// <summary>Gets a value indicating whether the current stream supports reading.</summary>
		public override bool CanRead {
			get { return fs.CanRead; }
		}

		/// <summary>Gets a value indicating whether the current stream supports seeking.</summary>
		public override bool CanSeek {
			get { return fs.CanSeek; }
		}

		/// <summary>Gets a value indicating whether the current stream supports writing.</summary>
		public override bool CanWrite {
			get { return fs.CanWrite; }
		}

		/// <summary>Gets the length in bytes of the stream.</summary>
		public override long Length {
			get { return fs.Length; }
		}

		/// <summary>Gets or sets the current position of this stream.</summary>
		public override long Position {
			get { return fs.Position; }
			set { fs.Position = value; }
		}

		/// <summary>The current progress reached on the stream.</summary>
		public int CurrentProgress {
			get { return currentProgress; }
		}

		/// <summary>Gets or sets the update interval.</summary>
		public int NotifyInterval {
			get { return notifyInterval; }
			set {
				if ((value > 100) || (value < 1)) {
					throw new ArgumentOutOfRangeException("NotifyInterval", value, "The update interval must be between 1 and 100 (inclusive).");
				} else {
					notifyInterval = value;
				}
			}
		}


		/// <summary>Constructor.</summary>
		/// <param name="fs">File stream to measure progress from.</param>
		/// <remarks>Update triggering occurs every 5 percent.</remarks>
		public ProgressStream(FileStream fs) {
			this.fs = fs;
			currentProgress = lastNotifyProgress = 0;
			notifyInterval = 5;
		}

		/// <summary>Constructor.</summary>
		/// <param name="fs">File stream to measure progress from.</param>
		/// <param name="notifyInterval">At what percentage point intervals to trigger an update.</param>
		public ProgressStream(FileStream fs, int notifyInterval) {
			this.fs = fs;
			currentProgress = lastNotifyProgress = 0;
			NotifyInterval = notifyInterval;
		}


		/// <summary>Clears all buffers for this stream and causes any unbuffered data to be written to the underlying device.</summary>
		public override void Flush() {
			fs.Flush();
		}


		/// <summary>Reads a block of bytes from the stream and writes the data in a given buffer.</summary>
		/// <param name="buffer">When this method returns, contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
		/// <param name="offset">The byte offset in array at which to begin writing.</param>
		/// <param name="count">The maximum number of bytes to read.</param>
		/// <returns>The total number of bytes read into the buffer. This may be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
		public override int Read(byte[] buffer, int offset, int count) {
			int temp;

			temp = fs.Read(buffer, offset, count);
			currentProgress = (int)(((float)Position / (float)Length) * 100.0);

			if ((currentProgress - lastNotifyProgress) >= notifyInterval) {
				// Update anyone who's listening.
				if (ProgressUpdate != null) {
					// Package the event arguments.
					ProgressUpdateEventArgs e = new ProgressUpdateEventArgs();
					e.PercentDone = CurrentProgress;
					e.CurrentPosition = Position;

					ProgressUpdate(this, e);
				}

				lastNotifyProgress = CurrentProgress;
			}

			return temp;
		}


		/// <summary>Writes a block of bytes to this stream using data from a buffer.</summary>
		/// <param name="buffer">The array to read.</param>
		/// <param name="offset">The byte offset in array at which to begin reading.</param>
		/// <param name="count">The maximum number of bytes to write.</param>
		public override void Write(byte[] buffer, int offset, int count) {
			fs.Write(buffer, offset, count);
		}


		/// <summary>Sets the current position of this stream to the given value.</summary>
		/// <param name="offset">The point relative to origin from which to begin seeking.</param>
		/// <param name="origin">Specifies the beginning, the end, or the current position as a reference point for origin.</param>
		/// <returns>The new position within the current stream.</returns>
		public override long Seek(long offset, SeekOrigin origin) {
			return fs.Seek(offset, origin);
		}


		/// <summary>Sets the length of this stream to the given value.</summary>
		/// <param name="value">The new length of the stream.</param>
		public override void SetLength(long value) {
			fs.SetLength(value);
		}
	} 
}
