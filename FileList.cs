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
		public FileListType type = FileListType.VERIFY;

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
			string extension = Path.GetExtension(filename).ToLower(System.Globalization.CultureInfo.InvariantCulture);

			// Open the file.
			StreamReader reader = new StreamReader(filename);

			// Figure out what type of file it is.
			if ((extension == ".verify") || (extension == ".vfy")) {
				try {
					// Deserialize the XML.
					XmlSerializer serializer = new XmlSerializer(typeof(FileList));
					FileList temp = (FileList)serializer.Deserialize(reader);

					// Copy it into this object.
					this.filelist = temp.filelist;
					this.information = temp.information;
					this.type = FileListType.VERIFY;
					this.version = temp.version;
				} catch {
					try { reader.Close(); } catch {}
					throw;
				}
			} else {
				switch (extension) {
					case ".sfv":	this.type = FileListType.SFV;		break;
					case ".md5":	this.type = FileListType.MD5;		break;
					case ".md5sum":	this.type = FileListType.MD5SUM;	break;
					default:
						try { reader.Close(); } catch {}
						throw new FileTypeException();
				}

				// Read it in.
				string line;
				Match m;
				while ((line = reader.ReadLine()) != null) {
					line = line.Trim();

					switch (this.type) {
						case FileListType.SFV:
							// Format:   filename.ext ABCD1234
							m = Regex.Match(line, @"^(?<Name>[^;].+) (?<Checksum>[0-9a-fA-F]{8})$");
							if (m.Success) {
								filelist.Add(new FileListFile(m.Groups["Name"].Value.Trim(), m.Groups["Checksum"].Value.Trim(), AlgorithmType.CRC32REVERSED));
							}
							break;

						case FileListType.MD5:
							// Format:   MD5 (filename.ext) = 0123456789ABCDEF0123456789ABCDEF
							m = Regex.Match(line, @"^MD5 \((?<Name>.+)\) = (?<Checksum>[0-9a-fA-F]{32})$");
							if (m.Success) {
								filelist.Add(new FileListFile(m.Groups["Name"].Value.Trim(), m.Groups["Checksum"].Value.Trim(), AlgorithmType.MD5));
							}
							break;

						case FileListType.MD5SUM:
							// Format:   0123456789ABCDEF0123456789ABCDEF filename.ext
							m = Regex.Match(line, @"^(?<Checksum>[0-9a-fA-F]{32})\s+(?<Name>.+)$");
							if (m.Success) {
								filelist.Add(new FileListFile(m.Groups["Name"].Value.Trim(), m.Groups["Checksum"].Value.Trim(), AlgorithmType.MD5));
							}
							break;
					}
				}
			}

			this.sourceFile = filename;
			reader.Close();
		}
	}
}
