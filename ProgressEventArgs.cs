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

namespace Classless.Verifier {
	/// <summary>Event data for the FileListProcessor.Progress event.</summary>
	public class ProgressEventArgs : EventArgs {
		/// <summary>The file that this notification is for.</summary>
		public FileListFile File = null;

		/// <summary>The current processing status of the file.</summary>
		public FileListProcessorStatus Status = FileListProcessorStatus.InProcess;

		/// <summary>The percent complete of the file.</summary>
		public int PercentCompleteFile = 0;

		/// <summary>The percent complete of the entire file list.</summary>
		public int PercentCompleteTotal = 0;
	}
}
