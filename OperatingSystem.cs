// $Id$

/*
 * Classless.Utilities.OperatingSystem
 * An Operating System Identifier for C#/.NET
 * http://www.classless.net
 * 
 * Version 1.0, 7/5/2004
 * 
 * */

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
 * The Original Code is Classless.Utilities.OperatingSystem.
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

namespace Classless.Utilities {
	/// <summary></summary>
	sealed public class OperatingSystem {
		/// <summary>Gets the operating sytem version.</summary>
		static public Version Version {
			get { return System.Environment.OSVersion.Version; }
		}


		/// <summary>Gets the platform ID.</summary>
		static public PlatformID Platform {
			get { return System.Environment.OSVersion.Platform; }
		}


		/// <summary>Gets the human-readable operating system name.</summary>
		static public string Name {
			get {
				string temp = "Unknown";

				switch (Platform) {
					case PlatformID.Win32NT:
						switch (Version.Major) {
							case 3:	temp = "Windows NT 3.51"; break;
							case 4:	temp = "Windows NT 4.0"; break;
							case 5:
								switch (Version.Minor) {
									case 0: temp = "Windows 2000"; break;
									case 1: temp = "Windows XP"; break;
									case 2: temp = "Windows 2003"; break;
								}
								break;
							case 6: temp = "Windows Longhorn"; break;
						}
						break;

					case PlatformID.Win32Windows:
						switch (Version.Minor) {
							case 0: temp = "Windows 95"; break;
							case 10: temp = "Windows 98"; break;
							case 90: temp = "Windows Me"; break;
						}
						break;

					case PlatformID.Win32S:
						temp = "Windows";
						break;

					default:
						temp = Platform.ToString();
						break;
				}

				return temp;
			}
		}

		// Default private constructor.
		private OperatingSystem() {}
	}
}
