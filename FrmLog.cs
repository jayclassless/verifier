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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Classless.Verifier {
	/// <summary>The logging window.</summary>
	public class FrmLog : Form {
		#region Control Declarations
		private System.Windows.Forms.TextBox txtLog;
		private System.Windows.Forms.SaveFileDialog dialogSave;
		private System.Windows.Forms.ContextMenu mnuLog;
		private System.Windows.Forms.MenuItem mnuLogCopy;
		private System.Windows.Forms.MenuItem mnuLogSaveToFile;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mnuLogClose;
		private System.ComponentModel.Container components = null;
		#endregion


		// Singleton fun.
		private static FrmLog instance;
		static FrmLog() { instance = new FrmLog(); }
		private FrmLog() : base() {
			InitializeComponent();
			StartEventProcessing();
		}


		/// <summary>Gets the current AppConfig instance.</summary>
		static public FrmLog Instance {
			get { return instance; }
		}


		/// <summary>Add a header to the log.</summary>
		public void AddHeader() {
			string temp;
			int maxLen = 0;

			// Application header.
			temp = Verifier.GetFullTitle(true) + " -- " + Verifier.GetHomePage();
			maxLen = temp.Length;
			AddLine(temp);

			// Versions.
			temp = Classless.Utilities.OperatingSystem.Name + ", " + Verifier.GetFullTitleNET() + ", " + Verifier.GetFullTitleHasher();
			if (temp.Length > maxLen) { maxLen = temp.Length; }
			AddLine(temp);

			AddLine(new String('-', maxLen));
			AddLine();
		}


		/// <summary>Adds a blank line to the log.</summary>
		public void AddLine() { AddLine(string.Empty); }
		/// <summary>Adds a line of text to the log.</summary>
		/// <param name="line">The text to add.</param>
		public void AddLine(string line) {
			try {
				txtLog.Text += line + Environment.NewLine;
				txtLog.Select(txtLog.Text.Length, 0);
				txtLog.ScrollToCaret();
			} catch {
				// On the off chance that we put more than 32k in this log.
			}
		}


		/// <summary>Clears the contents of the log.</summary>
		public void ClearLines() {
			txtLog.Text = string.Empty;
		}


		/// <summary>Save the contents of the log to a file.</summary>
		public void Save() {
			DialogResult dr = dialogSave.ShowDialog();

			if (dr == DialogResult.OK) {
				Save(dialogSave.FileName);
			}
		}


		/// <summary>Save the contents of the log to a file.</summary>
		/// <param name="filename">The file name to use.</param>
		public void Save(string filename) {
			System.IO.StreamWriter writer = null;
			try {
				// Open file.
				writer = new System.IO.StreamWriter(System.IO.File.Open(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write));

				// Write the contents of the log.
				foreach(string s in txtLog.Lines) {
					writer.WriteLine(s);
				}
			} catch {
				MessageBox.Show("There was an error trying to save the log to the file.",
					"Verifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} finally {
				if (writer != null) { writer.Close(); }
			}
		}


		/// <summary>Clean up any resources being used.</summary>
		protected override void Dispose(bool disposing) {
			if ((disposing) && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}


		#region Events
		// Hook into the event processing.
		private void StartEventProcessing() {
			this.Closing += new CancelEventHandler(this.FrmMain_Closing);
			mnuLogCopy.Click += new System.EventHandler(this.mnuLogCopy_Click);
			mnuLogSaveToFile.Click += new System.EventHandler(this.mnuLogSaveToFile_Click);
			mnuLogClose.Click += new System.EventHandler(this.mnuLogClose_Click);
		}

		// User wants to close the window.
		private void FrmMain_Closing(object sender, CancelEventArgs args) {
			this.Hide();
			args.Cancel = true;
		}

		// User wants to copy the selected text to the clipboard.
		private void mnuLogCopy_Click(object sender, System.EventArgs args) {
			if (txtLog.SelectedText.Length == 0) {
				Clipboard.SetDataObject(txtLog.Text, true);
			} else {
				Clipboard.SetDataObject(txtLog.SelectedText, true);
			}
		}

		// User wants to save the log to a file.
		private void mnuLogSaveToFile_Click(object sender, System.EventArgs args) {
			Save();	
		}

		// User wants to close the window.
		private void mnuLogClose_Click(object sender, System.EventArgs args) {
			this.Close();
		}
		#endregion


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtLog = new System.Windows.Forms.TextBox();
			this.mnuLog = new System.Windows.Forms.ContextMenu();
			this.mnuLogCopy = new System.Windows.Forms.MenuItem();
			this.mnuLogSaveToFile = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuLogClose = new System.Windows.Forms.MenuItem();
			this.dialogSave = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// txtLog
			// 
			this.txtLog.ContextMenu = this.mnuLog;
			this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtLog.Location = new System.Drawing.Point(0, 0);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtLog.Size = new System.Drawing.Size(588, 269);
			this.txtLog.TabIndex = 0;
			this.txtLog.Text = "";
			this.txtLog.WordWrap = false;
			// 
			// mnuLog
			// 
			this.mnuLog.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				   this.mnuLogCopy,
																				   this.mnuLogSaveToFile,
																				   this.menuItem1,
																				   this.mnuLogClose});
			// 
			// mnuLogCopy
			// 
			this.mnuLogCopy.Index = 0;
			this.mnuLogCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.mnuLogCopy.Text = "Copy";
			// 
			// mnuLogSaveToFile
			// 
			this.mnuLogSaveToFile.Index = 1;
			this.mnuLogSaveToFile.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.mnuLogSaveToFile.Text = "Save To File";
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 2;
			this.menuItem1.Text = "-";
			// 
			// mnuLogClose
			// 
			this.mnuLogClose.Index = 3;
			this.mnuLogClose.Shortcut = System.Windows.Forms.Shortcut.AltF4;
			this.mnuLogClose.Text = "Close";
			// 
			// dialogSave
			// 
			this.dialogSave.DefaultExt = "log";
			this.dialogSave.Filter = "All Files (*.*)|*.*";
			this.dialogSave.Title = "Save Processing Log";
			// 
			// FrmLog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(588, 269);
			this.Controls.Add(this.txtLog);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "FrmLog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Processing Log";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
