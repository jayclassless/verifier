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
using System.Xml;
using System.Configuration;
using System.Reflection;

namespace Classless.Verifier {
	/// <summary>A class for managing persistent application settings.</summary>
	public class AppConfig : AppSettingsReader {
		private string docName = string.Empty;


		// Singleton fun.
		private static AppConfig instance;
		static AppConfig() { instance = new AppConfig(); }
		private AppConfig() : base() {
			docName = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), Assembly.GetEntryAssembly().GetName().Name + ".exe.config");
		}


		/// <summary>Gets the current AppConfig instance.</summary>
		static public AppConfig Instance {
			get { return instance; }
		}


		/// <summary>Gets the value for a specified key from the AppSettings property and returns an object
		/// of the specified type containing the value from the .config file.</summary>
		/// <param name="key">The key for which to get the value.</param>
		/// <param name="type">The type of the object to return.</param>
		/// <param name="defaultValue">The value to return if the specified key cannot be found.</param>
		/// <returns>The value of the specified key.</returns>
		/// <remarks>If the key is not found, then the value provided in defaultValue is returned.</remarks>
		public object GetValue(string key, Type type, object defaultValue) {
			object temp = defaultValue;

			try {
				temp = this.GetValue(key, type);
			} catch (InvalidOperationException) { }

			return temp;
		}


		/// <summary>Sets the value for a specified key in the .config file.</summary>
		/// <param name="key">The key for which to set the value.</param>
		/// <param name="value">The value to save to the AppConfig.</param>
		/// <returns>Whether or not the set operation was successful.</returns>
		public bool SetValue(string key, string value) {
			XmlDocument cfgDoc = new XmlDocument();
			loadConfigDoc(cfgDoc);

			// Get the appSettings node.
			XmlNode node = cfgDoc.SelectSingleNode("//appSettings");
			if (node == null) {
				throw new System.InvalidOperationException("appSettings section not found");
			}

			try {
				// Get the element we want to update.
				XmlElement addElem = (XmlElement)node.SelectSingleNode("//add[@key='" + key + "']");

				if (addElem != null) {
					// Update the element.
					addElem.SetAttribute("value", value);
				} else {
					// The element wasn't there -- create it.
					XmlElement entry = cfgDoc.CreateElement("add");
					entry.SetAttribute("key", key);
					entry.SetAttribute("value", value);
					node.AppendChild(entry);
				}

				// Save it.
				saveConfigDoc(cfgDoc, docName);
			} catch {
				return false;
			}

			return true;
		}


		/// <summary>Remove the specified key from the .config file.</summary>
		/// <param name="key">The key to remove.</param>
		/// <returns>Whether or not the remove operation was successful.</returns>
		public bool RemoveElement(string key) {
			try {
				XmlDocument cfgDoc = new XmlDocument();
				loadConfigDoc(cfgDoc);

				// Get the appSettings node.
				XmlNode node = cfgDoc.SelectSingleNode("//appSettings");
				if (node == null) {
					throw new System.InvalidOperationException("appSettings section not found");
				}

				// Remove the element.
				node.RemoveChild(node.SelectSingleNode("//add[@key='" + key + "']"));
   
				// Save it.
				saveConfigDoc(cfgDoc, docName);
			} catch {
				return false;
			}

			return true;
		}


		// Load the config file.
		private XmlDocument loadConfigDoc(XmlDocument cfgDoc) {
			cfgDoc.Load(docName);
			return cfgDoc;
		}


		// Save the config file.
		private void saveConfigDoc(XmlDocument cfgDoc, string cfgDocPath) {
			XmlTextWriter writer = new XmlTextWriter(cfgDocPath, null);
			writer.Formatting = Formatting.Indented;
			cfgDoc.WriteTo(writer);
			writer.Flush();
			writer.Close();
		}
	}
}
