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
	/// <summary>Gives a name to an algorithm type.</summary>
	public class AlgorithmName {
		private AlgorithmType type;
		private string name;
		private string shortName;


		/// <summary>Gets the algorithm's type.</summary>
		public AlgorithmType Type {
			get { return type; }	
		}

		/// <summary>Gets the algorithm's name.</summary>
		public string Name {
			get { return name; }
		}

		/// <summary>Gets the algorithm's short name.</summary>
		public string ShortName {
			get { return shortName; }
		}


		/// <summary>Initializes an instance of AlgorithmName.</summary>
		/// <param name="type">The algorithm type.</param>
		/// <param name="name">The algorithm name.</param>
		/// <param name="shortName">The short algorithm name.</param>
		public AlgorithmName(AlgorithmType type, string name, string shortName) {
			this.type = type;
			this.name = name;
			this.shortName = shortName;
		}
	}
}
