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
using System.Reflection;

namespace Classless.Verifier {
	/// <summary>The application "About" window.</summary>
	public class FrmAbout : Form {
		#region Control Declarations
		private System.Windows.Forms.Label lblVerifierNameVersion;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblVerifierCopyright;
		private System.Windows.Forms.Label lblHasherNameVersion;
		private System.Windows.Forms.Label lblHasherCopyright;
		private System.Windows.Forms.Label lblNETNameVersion;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblVerifierDescription;
		private System.ComponentModel.Container components = null;
		#endregion


		/// <summary>Initializes an instance of FrmAbout.</summary>
		public FrmAbout() {
			InitializeComponent();
			StartEventProcessing();

			Version version;
			AssemblyTitleAttribute title;
			AssemblyCopyrightAttribute copyright;
			AssemblyDescriptionAttribute description;

			// Display the Verifier information.
			copyright = (AssemblyCopyrightAttribute)AssemblyCopyrightAttribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute));
			description = (AssemblyDescriptionAttribute)AssemblyDescriptionAttribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyDescriptionAttribute));
			lblVerifierDescription.Text = description.Description;
			lblVerifierCopyright.Text = "Copyright " + copyright.Copyright;
			lblVerifierNameVersion.Text = Verifier.GetFullTitle(true);

			// Display the Classless.Hasher information.
			version = Assembly.GetAssembly(typeof(Classless.Hasher.MD5)).GetName().Version;
			title = (AssemblyTitleAttribute)AssemblyTitleAttribute.GetCustomAttribute(Assembly.GetAssembly(typeof(Classless.Hasher.MD5)), typeof(AssemblyTitleAttribute));
			copyright = (AssemblyCopyrightAttribute)AssemblyCopyrightAttribute.GetCustomAttribute(Assembly.GetAssembly(typeof(Classless.Hasher.MD5)), typeof(AssemblyCopyrightAttribute));
			lblHasherCopyright.Text = "Copyright " + copyright.Copyright;
			lblHasherNameVersion.Text = title.Title + " v" +
				version.Major.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + "." +
				version.Minor.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

			// Display the .NET information.
			version = Environment.Version;
			lblNETNameVersion.Text += " v" +
				version.Major.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + "." +
				version.Minor.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + "." +
				version.Build.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
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
			btnOK.Click += new System.EventHandler(this.btnOK_Click);
		}

		// User wants to close this window.
		private void btnOK_Click(object sender, EventArgs e) {
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
			this.lblVerifierNameVersion = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblVerifierCopyright = new System.Windows.Forms.Label();
			this.lblHasherNameVersion = new System.Windows.Forms.Label();
			this.lblHasherCopyright = new System.Windows.Forms.Label();
			this.lblNETNameVersion = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblVerifierDescription = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblVerifierNameVersion
			// 
			this.lblVerifierNameVersion.Location = new System.Drawing.Point(8, 12);
			this.lblVerifierNameVersion.Name = "lblVerifierNameVersion";
			this.lblVerifierNameVersion.Size = new System.Drawing.Size(212, 16);
			this.lblVerifierNameVersion.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 68);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Based Upon:";
			// 
			// lblVerifierCopyright
			// 
			this.lblVerifierCopyright.Location = new System.Drawing.Point(8, 44);
			this.lblVerifierCopyright.Name = "lblVerifierCopyright";
			this.lblVerifierCopyright.Size = new System.Drawing.Size(212, 16);
			this.lblVerifierCopyright.TabIndex = 2;
			// 
			// lblHasherNameVersion
			// 
			this.lblHasherNameVersion.Location = new System.Drawing.Point(16, 88);
			this.lblHasherNameVersion.Name = "lblHasherNameVersion";
			this.lblHasherNameVersion.Size = new System.Drawing.Size(212, 16);
			this.lblHasherNameVersion.TabIndex = 3;
			// 
			// lblHasherCopyright
			// 
			this.lblHasherCopyright.Location = new System.Drawing.Point(16, 104);
			this.lblHasherCopyright.Name = "lblHasherCopyright";
			this.lblHasherCopyright.Size = new System.Drawing.Size(212, 16);
			this.lblHasherCopyright.TabIndex = 4;
			// 
			// lblNETNameVersion
			// 
			this.lblNETNameVersion.Location = new System.Drawing.Point(16, 128);
			this.lblNETNameVersion.Name = "lblNETNameVersion";
			this.lblNETNameVersion.Size = new System.Drawing.Size(212, 16);
			this.lblNETNameVersion.TabIndex = 5;
			this.lblNETNameVersion.Text = "Microsoft .NET Framework";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(83, 152);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 23);
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "&OK";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblVerifierDescription);
			this.groupBox1.Controls.Add(this.lblHasherNameVersion);
			this.groupBox1.Controls.Add(this.lblHasherCopyright);
			this.groupBox1.Controls.Add(this.lblVerifierNameVersion);
			this.groupBox1.Controls.Add(this.lblNETNameVersion);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.lblVerifierCopyright);
			this.groupBox1.Location = new System.Drawing.Point(4, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(236, 148);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			// 
			// lblVerifierDescription
			// 
			this.lblVerifierDescription.Location = new System.Drawing.Point(8, 28);
			this.lblVerifierDescription.Name = "lblVerifierDescription";
			this.lblVerifierDescription.Size = new System.Drawing.Size(212, 16);
			this.lblVerifierDescription.TabIndex = 7;
			// 
			// FrmAbout
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(246, 179);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmAbout";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About Verifier";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
