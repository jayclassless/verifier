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
	/// <summary>Represents the status of a file's processing.</summary>
	public enum FileListProcessorStatus {
		/// <summary>No status.</summary>
		None,

		/// <summary>The file is in process of being verified.</summary>
		InProcess,

		/// <summary>The file has passed verification.</summary>
		Good,

		/// <summary>The file has failed verification.</summary>
		Bad,

		/// <summary>There was an error while trying to verify the file.</summary>
		Error,

		/// <summary>The file could not be found.</summary>
		NotFound,

		/// <summary>The file is the wrong size.</summary>
		WrongSize
	}
}
