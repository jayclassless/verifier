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
using System.Windows.Forms;

namespace Classless.Verifier {
	/// <summary>The starting point of the application.</summary>
	sealed public class Verifier {
		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		static void Main(string[] args) {
			FrmMain fm = new FrmMain();
			FileList fileList = null;

			if (args.Length > 0) {
				try {
					fileList = new FileList(args[0]);
				} catch (FileTypeException) {
					// Unknown format.
					MessageBox.Show(args[0] + " does not contain a supported file verification list format.",
						"Verify", MessageBoxButtons.OK, MessageBoxIcon.Error);
				} catch (Exception ex) {
					// Bad error.
					MessageBox.Show("Could not open file verification list:" +
						Environment.NewLine + Environment.NewLine +
						args[0] +
						Environment.NewLine + Environment.NewLine +
						ex.Message,
						"Verify", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			// Start the application.
			fm.FileList = fileList;
			Application.Run(fm);
		}


		// Default private constructor.
		private Verifier() {}


		/// <summary>Generates the full title  of this application with version number.</summary>
		/// <returns>Full title with version number.</returns>
		static public string GetFullTitle() {
			Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			System.Reflection.AssemblyTitleAttribute title = (System.Reflection.AssemblyTitleAttribute)System.Reflection.AssemblyTitleAttribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), typeof(System.Reflection.AssemblyTitleAttribute));

			return title.Title + " v" +
				version.Major.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + "." +
				version.Minor.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
		}


		/// <summary>Returns the URL of the application's home page.</summary>
		/// <returns>The application URL.</returns>
		static public string GetHomePage() {
			return "http://www.classless.net/projects/verifier/";
		}
	}
}
