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
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Classless.Verifier {
	/// <summary>A file verification list.</summary>
	[XmlRootAttribute("verify", Namespace="", IsNullable=false)]
	public class FileList {
		/// <summary>The type of file list that this class originally was when read.</summary>
		[XmlIgnoreAttribute()]
		public FileListType type = FileListType.VERIFYXML;

		/// <summary>The original source file this file list was read from.</summary>
		[XmlIgnoreAttribute()]
		public string sourceFile;

		/// <summary>The version of the VerifyXML schema that was used.</summary>
		[XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public string version = "1.0";

		/// <summary>General information about the file list.</summary>
		public FileListInformation information = new FileListInformation();

		/// <summary>The list of files.</summary>
		[XmlArrayItemAttribute(Type=typeof(FileListFile), ElementName="file", IsNullable=false)]
		public ArrayList filelist = new ArrayList();


		/// <summary>Initializes an instances of FileList.</summary>
		/// <remarks>This default constructor exists for serialization's sake.</remarks>
		public FileList() {}

		/// <summary>Initializes an instances of FileList.</summary>
		/// <param name="filename">The file verification list to build the FileList from.</param>
		public FileList(string filename) {
			FileListReader reader = FileListReader.GetReader(Path.GetExtension(filename));
			reader.Read(filename);
			this.filelist = reader.FileList.filelist;
			this.information = reader.FileList.information;
			this.sourceFile = reader.FileList.sourceFile;
			this.type = reader.FileList.type;
			this.version = reader.FileList.version;
		}
	}
}
