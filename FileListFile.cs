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
using System.Xml.Serialization;

namespace Classless.Verifier {
	/// <summary>A file entry in the list.</summary>
	public class FileListFile {
		/// <summary>The filename.</summary>
		[XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public string name;

		/// <summary>The algorithm used to calculate the hash/checksum.</summary>
		[XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public AlgorithmType type;
 
		/// <summary>The file size in bytes.</summary>
		[XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public ulong size;

		/// <summary>Whether or not the file size was specified in the file list.</summary>
		[XmlIgnoreAttribute()]
		public bool sizeSpecified;

		/// <summary>The date/time the file was created.</summary>
		[XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public DateTime created;

		/// <summary>Whether or not the created date/time was specified in the file list.</summary>
		[XmlIgnoreAttribute()]
		public bool createdSpecified;

		/// <summary>The date/time the file was last modified.</summary>
		[XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public DateTime modified;

		/// <summary>Whether or not the modified date/time was specified in the file list.</summary>
		[XmlIgnoreAttribute()]
		public bool modifiedSpecified;

		/// <summary>Whether or not this file should be ignored during processing.</summary>
		[XmlIgnoreAttribute()]
		public bool Ignore;

		/// <summary>The hash/checksum.</summary>
		[XmlTextAttribute()]
		public string Value;


		/// <summary>Initializes an instance of FileListFile.</summary>
		/// <remarks>This default constructor exists for serialization's sake.</remarks>
		public FileListFile() {}


		/// <summary>Initializes an instance of FileListFile.</summary>
		/// <param name="name">The filename.</param>
		/// <param name="hashValue">The hexadecimal string representing the hash/checksum value.</param>
		/// <param name="hashType">The algorithm used to calculate the hash/checksum.</param>
		public FileListFile(string name, string hashValue, AlgorithmType hashType) {
			this.name = name;
			this.type = hashType;
			this.Value = hashValue;
			this.sizeSpecified = false;
			this.createdSpecified = false;
			this.modifiedSpecified = false;
			this.Ignore = false;
		}
	}
}
