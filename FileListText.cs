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
	/// <summary>A block of text.</summary>
	public class FileListText {
		/// <summary>The language that the text is in.</summary>
		[XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="language")]
		public string lang = "en";

		/// <summary>The text.</summary>
		[XmlTextAttribute()]
		public string Value;
	}
}
