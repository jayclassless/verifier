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
	/// <summary>Represents a file list reading class.</summary>
	abstract public class FileListReader {
		/// <summary>The file list that was read in.</summary>
		public FileList FileList = null;


		/// <summary>Read in a file list.</summary>
		/// <param name="filename">The file list to read in.</param>
		abstract public void Read(string filename);


		/// <summary>Retrieves the appropriate FileListReader given a specific file type.</summary>
		/// <param name="fileExtension">The filename extension of the FileListReader wanted.</param>
		/// <returns>The appropriate FileListReader.</returns>
		static public FileListReader GetReader(string fileExtension) {
			FileListReader flr;

			// Tidy up the extension we were given.
			fileExtension = fileExtension.ToLower(System.Globalization.CultureInfo.InvariantCulture);
			if (fileExtension.Substring(0, 1) == ".") {
				fileExtension = fileExtension.Substring(1);
			}

			// Figure out which reader to return.
			switch (fileExtension) {
				case "verify":	flr = new VerifyXMLFileListReader(); break;
				case "vfy":		flr = new VerifyXMLFileListReader(); break;
				case "sfv":		flr = new SFVFileListReader(); break;
				case "md5":		flr = new MD5FileListReader(); break;
				case "md5sum":	flr = new MD5FileListReader(); break;
				default:		throw new FileTypeException();
			}

			return flr;
		}


		/// <summary>Retrieves the appropriate FileListReader given a specific file type.</summary>
		/// <param name="type">The FileListType of the FileListReader wanted.</param>
		/// <returns>The appropriate FileListReader.</returns>
		static public FileListReader GetReader(FileListType type) {
			FileListReader flr;

			switch (type) {
				case FileListType.BSDMD5:	flr = new MD5FileListReader(); break;
				case FileListType.MD5SUM:	flr = new MD5FileListReader(); break;
				case FileListType.SFV:		flr = new SFVFileListReader(); break;
				case FileListType.VERIFYXML: flr = new VerifyXMLFileListReader(); break;
				default:					throw new FileTypeException();
			}

			return flr;
		}
	}
}
