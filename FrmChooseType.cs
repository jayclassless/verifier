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
	/// <summary>A window that allows the user to choose what type of file verification list to work with.</summary>
	public class FrmChooseType : Form {
		#region Control Declarations
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.RadioButton optVerifyXML;
		private System.Windows.Forms.RadioButton optSFV;
		private System.Windows.Forms.RadioButton optMD5SUM;
		private System.Windows.Forms.RadioButton optBSDMD5;
		private System.ComponentModel.Container components = null;
		#endregion


		/// <summary>Gets the file verification list type that the user has chosen.</summary>
		public FileListType FileListType {
			get {
				if (this.optVerifyXML.Checked) {
					return FileListType.VERIFYXML;
				} else if (this.optMD5SUM.Checked) {
					return FileListType.MD5SUM;
				} else if (this.optBSDMD5.Checked) {
					return FileListType.BSDMD5;
				} else {
					// Until we think of something better, default to SFV.
					return FileListType.SFV;
				}
			}
		}


		/// <summary>Initializes a new instance of FrmChooseType.</summary>
		public FrmChooseType() {
			InitializeComponent();
		}


		/// <summary>Clean up any resources being used.</summary>
		protected override void Dispose(bool disposing) {
			if ((disposing) && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.optBSDMD5 = new System.Windows.Forms.RadioButton();
			this.optMD5SUM = new System.Windows.Forms.RadioButton();
			this.optSFV = new System.Windows.Forms.RadioButton();
			this.optVerifyXML = new System.Windows.Forms.RadioButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.optBSDMD5);
			this.groupBox1.Controls.Add(this.optMD5SUM);
			this.groupBox1.Controls.Add(this.optSFV);
			this.groupBox1.Controls.Add(this.optVerifyXML);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(162, 123);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Type:";
			// 
			// optBSDMD5
			// 
			this.optBSDMD5.Location = new System.Drawing.Point(29, 96);
			this.optBSDMD5.Name = "optBSDMD5";
			this.optBSDMD5.Size = new System.Drawing.Size(104, 20);
			this.optBSDMD5.TabIndex = 3;
			this.optBSDMD5.Text = "MD5 (&BSD)";
			// 
			// optMD5SUM
			// 
			this.optMD5SUM.Location = new System.Drawing.Point(29, 72);
			this.optMD5SUM.Name = "optMD5SUM";
			this.optMD5SUM.Size = new System.Drawing.Size(104, 20);
			this.optMD5SUM.TabIndex = 2;
			this.optMD5SUM.Text = "&MD5";
			// 
			// optSFV
			// 
			this.optSFV.Checked = true;
			this.optSFV.Location = new System.Drawing.Point(29, 24);
			this.optSFV.Name = "optSFV";
			this.optSFV.Size = new System.Drawing.Size(104, 20);
			this.optSFV.TabIndex = 1;
			this.optSFV.TabStop = true;
			this.optSFV.Text = "&SFV";
			// 
			// optVerifyXML
			// 
			this.optVerifyXML.Location = new System.Drawing.Point(29, 48);
			this.optVerifyXML.Name = "optVerifyXML";
			this.optVerifyXML.Size = new System.Drawing.Size(104, 20);
			this.optVerifyXML.TabIndex = 0;
			this.optVerifyXML.Text = "&VerifyXML";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Controls.Add(this.btnOK);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 123);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(162, 32);
			this.panel1.TabIndex = 1;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(84, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "&Cancel";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(4, 4);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "&OK";
			// 
			// FrmChooseType
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(162, 155);
			this.ControlBox = false;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FrmChooseType";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Choose File List Type";
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
