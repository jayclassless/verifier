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
	/// <summary>A class that contains general information.</summary>
	public class FileListInformation {
		/// <summary>The date/time the file list was created.</summary>
		public DateTime created;

		/// <summary>Whether or not the create date/time was specified in the file list.</summary>
		[XmlIgnoreAttribute()]
		public bool createdSpecified;

		/// <summary>The author of the file list.</summary>
		public string createdby;

		/// <summary>The application used to create the file list.</summary>
		public string application;

		/// <summary>The name of the file list.</summary>
		public string listname;

		/// <summary>Miscellaneous comments about the file list.</summary>
		[XmlElementAttribute("comments")]
		public FileListText[] comments;
	}
}
