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
using AMS.Profile;

namespace Classless.Verifier {
	/// <summary>A window that allows the user to manage the program options.</summary>
	public class FrmOptions : Form {
		#region Control Declarations
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TabControl tabMainControl;
		private System.Windows.Forms.TabPage tabOptions;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboPriority;
		private System.Windows.Forms.CheckBox chkProcessOnLoad;
		private System.Windows.Forms.CheckBox chkAutoSaveLog;
		private System.Windows.Forms.CheckBox chkRememberWindowSettings;
		private System.Windows.Forms.CheckBox chkConfirmExit;
		private System.Windows.Forms.ListView listExtensions;
		private System.Windows.Forms.ColumnHeader colExtension;
		private System.Windows.Forms.TabPage tabAssociations;
		private System.Windows.Forms.Button btnSetAssociations;
		private System.Windows.Forms.Button btnRemoveAssociations;
		private System.ComponentModel.Container components = null;
		#endregion


		// The current program options.
		private Config cfg;


		/// <summary>Initializes a new instance of FrmOptions.</summary>
		public FrmOptions() {
			InitializeComponent();
			StartEventProcessing();

			// Load the priority options.
			foreach (string tp in Enum.GetNames(typeof(System.Threading.ThreadPriority))) {
				comboPriority.Items.Add(tp);
			}

			// Load the file extensions.
			ListViewItem lvi;
			lvi = new ListViewItem(new string[]{"SFV", "text/sfv", "Simple File Verification List", "Verifier.SFV"}); listExtensions.Items.Add(lvi);
			lvi = new ListViewItem(new string[]{"MD5", "text/md5", "MD5 Checksums", "Verifier.MD5"}); listExtensions.Items.Add(lvi);
			lvi = new ListViewItem(new string[]{"MD5SUM", "text/md5", "MD5 Checksums", "Verifier.MD5SUM"}); listExtensions.Items.Add(lvi);
			lvi = new ListViewItem(new string[]{"VFY", "text/xml+verifyxml", "VerifyXML File Verification List", "Verifier.VFY"}); listExtensions.Items.Add(lvi);
			lvi = new ListViewItem(new string[]{"VERIFY", "text/xml+verifyxml", "VerifyXML File Verification List", "Verifier.VERIFY"}); listExtensions.Items.Add(lvi);

			// Load the existing options.
			cfg = new Config();
			chkProcessOnLoad.Checked = cfg.GetValue("Program", "ProcessListOnApplicationLoad", true);
			chkConfirmExit.Checked = cfg.GetValue("Program", "ConfirmApplicationExit", true);
			chkAutoSaveLog.Checked = cfg.GetValue("Program", "AutomaticallySaveLog", false);
			comboPriority.Text = cfg.GetValue("Program", "ProcessingThreadPriority", "Normal");
			chkRememberWindowSettings.Checked = cfg.GetValue("GUI", "RememberWindowSettings", true);
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
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			this.btnSetAssociations.Click += new System.EventHandler(this.btnSetAssociations_Click);
			this.btnRemoveAssociations.Click += new System.EventHandler(this.btnRemoveAssociations_Click);
		}

		// User wants to save their choices.
		private void btnOK_Click(object sender, EventArgs args) {
			cfg.SetValue("Program", "ProcessListOnApplicationLoad", chkProcessOnLoad.Checked);
			cfg.SetValue("Program", "ConfirmApplicationExit", chkConfirmExit.Checked);
			cfg.SetValue("Program", "AutomaticallySaveLog", chkAutoSaveLog.Checked);
			cfg.SetValue("Program", "ProcessingThreadPriority", comboPriority.Text);
			cfg.SetValue("GUI", "RememberWindowSettings", chkRememberWindowSettings.Checked);
		}

		// User wants to set file associations.
		private void btnSetAssociations_Click(object sender, EventArgs args) {
			Org.Mentalis.Utilities.FileAssociation fa;

			foreach (ListViewItem lvi in listExtensions.CheckedItems) {
				fa = new Org.Mentalis.Utilities.FileAssociation();
				fa.ContentType = lvi.SubItems[1].Text;
				fa.Extension = lvi.SubItems[0].Text;
				fa.FullName = lvi.SubItems[2].Text;
				fa.IconIndex = 0;
				fa.IconPath = Application.ExecutablePath;
				fa.ProperName = lvi.SubItems[3].Text;
				fa.AddCommand("Verify", Application.ExecutablePath + " \"%1\"");
				fa.Create();
			}
		}

		// User wants to remove file associations.
		private void btnRemoveAssociations_Click(object sender, EventArgs args) {
			Org.Mentalis.Utilities.FileAssociation fa;

			foreach (ListViewItem lvi in listExtensions.CheckedItems) {
				fa = new Org.Mentalis.Utilities.FileAssociation();
				fa.Extension = lvi.SubItems[0].Text;
				fa.ProperName = lvi.SubItems[3].Text;
				fa.Remove();
			}
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.tabMainControl = new System.Windows.Forms.TabControl();
			this.tabOptions = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.comboPriority = new System.Windows.Forms.ComboBox();
			this.chkProcessOnLoad = new System.Windows.Forms.CheckBox();
			this.chkAutoSaveLog = new System.Windows.Forms.CheckBox();
			this.chkRememberWindowSettings = new System.Windows.Forms.CheckBox();
			this.chkConfirmExit = new System.Windows.Forms.CheckBox();
			this.tabAssociations = new System.Windows.Forms.TabPage();
			this.btnSetAssociations = new System.Windows.Forms.Button();
			this.btnRemoveAssociations = new System.Windows.Forms.Button();
			this.listExtensions = new System.Windows.Forms.ListView();
			this.colExtension = new System.Windows.Forms.ColumnHeader();
			this.panel1.SuspendLayout();
			this.tabMainControl.SuspendLayout();
			this.tabOptions.SuspendLayout();
			this.tabAssociations.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Controls.Add(this.btnOK);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 151);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(314, 32);
			this.panel1.TabIndex = 1;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(236, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "&Cancel";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(156, 4);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "&OK";
			// 
			// tabMainControl
			// 
			this.tabMainControl.Controls.Add(this.tabOptions);
			this.tabMainControl.Controls.Add(this.tabAssociations);
			this.tabMainControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMainControl.ItemSize = new System.Drawing.Size(48, 18);
			this.tabMainControl.Location = new System.Drawing.Point(0, 0);
			this.tabMainControl.Name = "tabMainControl";
			this.tabMainControl.SelectedIndex = 0;
			this.tabMainControl.Size = new System.Drawing.Size(314, 151);
			this.tabMainControl.TabIndex = 2;
			// 
			// tabOptions
			// 
			this.tabOptions.Controls.Add(this.label1);
			this.tabOptions.Controls.Add(this.comboPriority);
			this.tabOptions.Controls.Add(this.chkProcessOnLoad);
			this.tabOptions.Controls.Add(this.chkAutoSaveLog);
			this.tabOptions.Controls.Add(this.chkRememberWindowSettings);
			this.tabOptions.Controls.Add(this.chkConfirmExit);
			this.tabOptions.Location = new System.Drawing.Point(4, 22);
			this.tabOptions.Name = "tabOptions";
			this.tabOptions.Size = new System.Drawing.Size(306, 125);
			this.tabOptions.TabIndex = 0;
			this.tabOptions.Text = "Options";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 100);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(152, 20);
			this.label1.TabIndex = 11;
			this.label1.Text = "Process lists with a priorty of:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboPriority
			// 
			this.comboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPriority.ItemHeight = 13;
			this.comboPriority.Location = new System.Drawing.Point(160, 100);
			this.comboPriority.Name = "comboPriority";
			this.comboPriority.Size = new System.Drawing.Size(96, 21);
			this.comboPriority.TabIndex = 10;
			// 
			// chkProcessOnLoad
			// 
			this.chkProcessOnLoad.Location = new System.Drawing.Point(8, 76);
			this.chkProcessOnLoad.Name = "chkProcessOnLoad";
			this.chkProcessOnLoad.Size = new System.Drawing.Size(292, 20);
			this.chkProcessOnLoad.TabIndex = 9;
			this.chkProcessOnLoad.Text = "Immediately process lists loaded from command line";
			// 
			// chkAutoSaveLog
			// 
			this.chkAutoSaveLog.Location = new System.Drawing.Point(8, 52);
			this.chkAutoSaveLog.Name = "chkAutoSaveLog";
			this.chkAutoSaveLog.Size = new System.Drawing.Size(292, 20);
			this.chkAutoSaveLog.TabIndex = 8;
			this.chkAutoSaveLog.Text = "Automatically save processing log";
			// 
			// chkRememberWindowSettings
			// 
			this.chkRememberWindowSettings.Location = new System.Drawing.Point(8, 28);
			this.chkRememberWindowSettings.Name = "chkRememberWindowSettings";
			this.chkRememberWindowSettings.Size = new System.Drawing.Size(292, 20);
			this.chkRememberWindowSettings.TabIndex = 7;
			this.chkRememberWindowSettings.Text = "Remember window positions and sizes";
			// 
			// chkConfirmExit
			// 
			this.chkConfirmExit.Location = new System.Drawing.Point(8, 4);
			this.chkConfirmExit.Name = "chkConfirmExit";
			this.chkConfirmExit.Size = new System.Drawing.Size(292, 20);
			this.chkConfirmExit.TabIndex = 6;
			this.chkConfirmExit.Text = "Confirm application exit during processing";
			// 
			// tabAssociations
			// 
			this.tabAssociations.Controls.Add(this.btnSetAssociations);
			this.tabAssociations.Controls.Add(this.btnRemoveAssociations);
			this.tabAssociations.Controls.Add(this.listExtensions);
			this.tabAssociations.Location = new System.Drawing.Point(4, 22);
			this.tabAssociations.Name = "tabAssociations";
			this.tabAssociations.Size = new System.Drawing.Size(306, 125);
			this.tabAssociations.TabIndex = 1;
			this.tabAssociations.Text = "File Associations";
			// 
			// btnSetAssociations
			// 
			this.btnSetAssociations.Location = new System.Drawing.Point(120, 8);
			this.btnSetAssociations.Name = "btnSetAssociations";
			this.btnSetAssociations.Size = new System.Drawing.Size(124, 23);
			this.btnSetAssociations.TabIndex = 1;
			this.btnSetAssociations.Text = "Set Associations";
			// 
			// btnRemoveAssociations
			// 
			this.btnRemoveAssociations.Location = new System.Drawing.Point(120, 36);
			this.btnRemoveAssociations.Name = "btnRemoveAssociations";
			this.btnRemoveAssociations.Size = new System.Drawing.Size(124, 23);
			this.btnRemoveAssociations.TabIndex = 2;
			this.btnRemoveAssociations.Text = "Remove Associations";
			// 
			// listExtensions
			// 
			this.listExtensions.CheckBoxes = true;
			this.listExtensions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							 this.colExtension});
			this.listExtensions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listExtensions.Location = new System.Drawing.Point(8, 8);
			this.listExtensions.Name = "listExtensions";
			this.listExtensions.Size = new System.Drawing.Size(108, 112);
			this.listExtensions.TabIndex = 0;
			this.listExtensions.View = System.Windows.Forms.View.Details;
			// 
			// colExtension
			// 
			this.colExtension.Text = "Extension";
			this.colExtension.Width = 89;
			// 
			// FrmOptions
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnOK;
			this.ClientSize = new System.Drawing.Size(314, 183);
			this.Controls.Add(this.tabMainControl);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmOptions";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Program Options";
			this.panel1.ResumeLayout(false);
			this.tabMainControl.ResumeLayout(false);
			this.tabOptions.ResumeLayout(false);
			this.tabAssociations.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
