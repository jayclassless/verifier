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
using System.Xml.Serialization;

namespace Classless.Verifier {
	/// <summary>A class that reads in SFV files.</summary>
	public class VerifyXMLFileListReader : FileListReader {
		/// <summary>Read in a file list.</summary>
		/// <param name="filename">The file list to read in.</param>
		override public void Read(string filename) {
			// Open the file.
			StreamReader reader = new StreamReader(filename);

			// Deserialize the XML.
			XmlSerializer serializer = new XmlSerializer(typeof(FileList));
			FileList = (FileList)serializer.Deserialize(reader);

			// Finish up.
			FileList.sourceFile = filename;
			FileList.type = FileListType.VERIFY;
			reader.Close();
		}
	}
}
