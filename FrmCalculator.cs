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
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Classless.Verifier {
	/// <summary>The hash calculator window.</summary>
	public class FrmCalculator : Form {
		#region Control Declarations
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox txtInputFile;
		private System.Windows.Forms.TextBox txtInputText;
		private System.Windows.Forms.RadioButton optInputFile;
		private System.Windows.Forms.RadioButton optInputText;
		private System.Windows.Forms.ListBox listAlgorithms;
		private System.Windows.Forms.Button btnCalculate;
		private System.Windows.Forms.TextBox txtOutput;
		private System.Windows.Forms.ContextMenu mnuAlgorithms;
		private System.Windows.Forms.MenuItem mnuAlgorithmsSelectAll;
		private System.Windows.Forms.MenuItem mnuAlgorithmsClearAll;
		private System.Windows.Forms.OpenFileDialog dialogOpen;
		private System.Windows.Forms.SaveFileDialog dialogSave;
		private System.Windows.Forms.ContextMenu mnuOutput;
		private System.Windows.Forms.MenuItem mnuOutputSave;
		private System.Windows.Forms.MenuItem mnuOutputCopy;
		private System.ComponentModel.Container components = null;
		#endregion


		/// <summary>Initializes an instances of FrmCalculator.</summary>
		public FrmCalculator() {
			InitializeComponent();
			StartEventProcessing();

			// Add the algorithms to the list.
			foreach(AlgorithmName an in Algorithm.Names) {
				listAlgorithms.Items.Add(an.Name);
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
			optInputText.Click += new System.EventHandler(this.optInputText_Click);
			optInputFile.Click += new System.EventHandler(this.optInputFile_Click);
			btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
			mnuAlgorithmsClearAll.Click += new System.EventHandler(this.mnuAlgorithmsClearAll_Click);
			mnuAlgorithmsSelectAll.Click += new System.EventHandler(this.mnuAlgorithmsSelectAll_Click);
			mnuOutputCopy.Click += new System.EventHandler(this.mnuOutputCopy_Click);
			mnuOutputSave.Click += new System.EventHandler(this.mnuOutputSave_Click);
		}

		// User chooses to input a string directly into the calculator.
		private void optInputText_Click(object sender, System.EventArgs e) {
			txtInputText.Enabled = true;
			txtInputFile.Enabled = false;
			btnBrowse.Enabled = false;
		}

		// User chooses to use a file as input to the calculator.
		private void optInputFile_Click(object sender, System.EventArgs e) {
			txtInputText.Enabled = false;
			txtInputFile.Enabled = true;
			btnBrowse.Enabled = true;
		}

		// User wants to browser for a file to use for input to the calculator.
		private void btnBrowse_Click(object sender, System.EventArgs e) {
			DialogResult dr = dialogOpen.ShowDialog();

			if (dr == DialogResult.OK) {
				txtInputFile.Text = dialogOpen.FileName;
			}
		}

		// User wants to calculate the hashes/checksums for the input given.
		private void btnCalculate_Click(object sender, System.EventArgs e) {
			int padding = 0;
			DateTime start = new DateTime(DateTime.Now.Ticks);

			// Validate input.
			if (listAlgorithms.SelectedItems.Count <= 0) {
				MessageBox.Show("You must select at least one algorithm to calculate with.",
					"Hash Calculator", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if ((txtInputFile.Text.Length == 0) && (optInputFile.Checked)) {
				MessageBox.Show("You must specify a file to process.",
					"Hash Calculator", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// Make them wait.
			btnCalculate.Text = "Please wait...";
			btnCalculate.Enabled = false;
			Cursor.Current = Cursors.WaitCursor;

			// Build the calculator.
			HashCalculator c = new HashCalculator();
			foreach(object a in listAlgorithms.SelectedItems) {
				if (a.ToString().Length > padding) { padding = a.ToString().Length; }
				c.AddAlgorithm(Algorithm.GetTypeFromName(a.ToString()));
			}

			// Calculate the hashes/checksums.
			if (optInputText.Checked) {
				c.ComputeHash(txtInputText.Text);
			} else {
				try {
					FileStream fs = File.OpenRead(txtInputFile.Text);
					c.ComputeHash(fs);
				} catch (FileNotFoundException) {
					MessageBox.Show("Could not find file.",
						"Hash Calculator", MessageBoxButtons.OK, MessageBoxIcon.Error);
					btnCalculate.Enabled = true;
					Cursor.Current = Cursors.Default;
					return;
				} catch {
					MessageBox.Show("There was an error trying to hash the file.",
						"Hash Calculator", MessageBoxButtons.OK, MessageBoxIcon.Error);
					btnCalculate.Enabled = true;
					Cursor.Current = Cursors.Default;
					return;
				}
			}

			// Display results.
			txtOutput.Clear();
			foreach(object a in listAlgorithms.SelectedItems) {
				txtOutput.Text += a.ToString().PadRight(padding, ' ') + ": " + Classless.Hasher.Utilities.ByteToHexadecimal(c.GetHash(Algorithm.GetTypeFromName(a.ToString()))) + Environment.NewLine;
			}
			TimeSpan duration = DateTime.Now - start;
			txtOutput.Text += Environment.NewLine + "Calculations completed in: " + duration.ToString();

			// Open the GUI back up.
			btnCalculate.Text = "&Calculate";
			btnCalculate.Enabled = true;
			Cursor.Current = Cursors.Default;
		}

		// User wants to clear all algorithm selections in the calculator.
		private void mnuAlgorithmsClearAll_Click(object sender, System.EventArgs e) {
			for (int i = 0; i < listAlgorithms.Items.Count; i++) {
				listAlgorithms.SetSelected(i, false);
			}
		}

		// User wants to select all algorithms in the calculator.
		private void mnuAlgorithmsSelectAll_Click(object sender, System.EventArgs e) {
			for (int i = 0; i < listAlgorithms.Items.Count; i++) {
				listAlgorithms.SetSelected(i, true);
			}
		}

		// User wants to copy the selected text to the clipboard.
		private void mnuOutputCopy_Click(object sender, System.EventArgs e) {
			Clipboard.SetDataObject(txtOutput.SelectedText, true);
		}

		// User wants to save the output of the calculations.
		private void mnuOutputSave_Click(object sender, System.EventArgs e) {
			DialogResult dr = dialogSave.ShowDialog();

			if (dr == DialogResult.OK) {
				StreamWriter writer = null;
				try {
					// Open file.
					writer = new StreamWriter(File.Open(dialogSave.FileName, FileMode.Create, FileAccess.Write));

					// Write header.
					writer.WriteLine(Verifier.GetFullTitle());
					writer.WriteLine("Hash Calculator Report");
					writer.Write("Input: ");
					if (optInputText.Checked) {
						writer.WriteLine(txtInputText.Text);
					} else {
						writer.WriteLine(txtInputFile.Text);
					}
					writer.WriteLine();

					// Write the contents of the output box.
					foreach(string s in txtOutput.Lines) {
						writer.WriteLine(s);
					}
				} catch {
					MessageBox.Show("There was an error trying to save the output to the file.",
						"Hash Calculator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				} finally {
					if (writer != null) { writer.Close(); }
				}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmCalculator));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.txtInputFile = new System.Windows.Forms.TextBox();
			this.txtInputText = new System.Windows.Forms.TextBox();
			this.optInputFile = new System.Windows.Forms.RadioButton();
			this.optInputText = new System.Windows.Forms.RadioButton();
			this.listAlgorithms = new System.Windows.Forms.ListBox();
			this.mnuAlgorithms = new System.Windows.Forms.ContextMenu();
			this.mnuAlgorithmsSelectAll = new System.Windows.Forms.MenuItem();
			this.mnuAlgorithmsClearAll = new System.Windows.Forms.MenuItem();
			this.btnCalculate = new System.Windows.Forms.Button();
			this.txtOutput = new System.Windows.Forms.TextBox();
			this.mnuOutput = new System.Windows.Forms.ContextMenu();
			this.mnuOutputCopy = new System.Windows.Forms.MenuItem();
			this.mnuOutputSave = new System.Windows.Forms.MenuItem();
			this.dialogOpen = new System.Windows.Forms.OpenFileDialog();
			this.dialogSave = new System.Windows.Forms.SaveFileDialog();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnBrowse);
			this.groupBox1.Controls.Add(this.txtInputFile);
			this.groupBox1.Controls.Add(this.txtInputText);
			this.groupBox1.Controls.Add(this.optInputFile);
			this.groupBox1.Controls.Add(this.optInputText);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(540, 76);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Input:";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Enabled = false;
			this.btnBrowse.Location = new System.Drawing.Point(456, 48);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.TabIndex = 9;
			this.btnBrowse.Text = "&Browse";
			// 
			// txtInputFile
			// 
			this.txtInputFile.Enabled = false;
			this.txtInputFile.Location = new System.Drawing.Point(56, 48);
			this.txtInputFile.Name = "txtInputFile";
			this.txtInputFile.Size = new System.Drawing.Size(396, 20);
			this.txtInputFile.TabIndex = 8;
			this.txtInputFile.Text = "";
			// 
			// txtInputText
			// 
			this.txtInputText.Location = new System.Drawing.Point(56, 20);
			this.txtInputText.Name = "txtInputText";
			this.txtInputText.Size = new System.Drawing.Size(476, 20);
			this.txtInputText.TabIndex = 7;
			this.txtInputText.Text = "";
			// 
			// optInputFile
			// 
			this.optInputFile.Location = new System.Drawing.Point(8, 48);
			this.optInputFile.Name = "optInputFile";
			this.optInputFile.Size = new System.Drawing.Size(48, 24);
			this.optInputFile.TabIndex = 6;
			this.optInputFile.Text = "F&ile:";
			// 
			// optInputText
			// 
			this.optInputText.Checked = true;
			this.optInputText.Location = new System.Drawing.Point(8, 20);
			this.optInputText.Name = "optInputText";
			this.optInputText.Size = new System.Drawing.Size(52, 24);
			this.optInputText.TabIndex = 5;
			this.optInputText.TabStop = true;
			this.optInputText.Text = "&Text:";
			// 
			// listAlgorithms
			// 
			this.listAlgorithms.ContextMenu = this.mnuAlgorithms;
			this.listAlgorithms.Dock = System.Windows.Forms.DockStyle.Top;
			this.listAlgorithms.Location = new System.Drawing.Point(0, 76);
			this.listAlgorithms.MultiColumn = true;
			this.listAlgorithms.Name = "listAlgorithms";
			this.listAlgorithms.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.listAlgorithms.Size = new System.Drawing.Size(540, 108);
			this.listAlgorithms.Sorted = true;
			this.listAlgorithms.TabIndex = 2;
			// 
			// mnuAlgorithms
			// 
			this.mnuAlgorithms.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.mnuAlgorithmsSelectAll,
																						  this.mnuAlgorithmsClearAll});
			// 
			// mnuAlgorithmsSelectAll
			// 
			this.mnuAlgorithmsSelectAll.Index = 0;
			this.mnuAlgorithmsSelectAll.Text = "Select All Algorithms";
			// 
			// mnuAlgorithmsClearAll
			// 
			this.mnuAlgorithmsClearAll.Index = 1;
			this.mnuAlgorithmsClearAll.Text = "Clear All Selections";
			// 
			// btnCalculate
			// 
			this.btnCalculate.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnCalculate.Location = new System.Drawing.Point(0, 184);
			this.btnCalculate.Name = "btnCalculate";
			this.btnCalculate.Size = new System.Drawing.Size(540, 23);
			this.btnCalculate.TabIndex = 4;
			this.btnCalculate.Text = "&Calculate";
			// 
			// txtOutput
			// 
			this.txtOutput.ContextMenu = this.mnuOutput;
			this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtOutput.Location = new System.Drawing.Point(0, 207);
			this.txtOutput.Multiline = true;
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.ReadOnly = true;
			this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtOutput.Size = new System.Drawing.Size(540, 174);
			this.txtOutput.TabIndex = 5;
			this.txtOutput.Text = "";
			this.txtOutput.WordWrap = false;
			// 
			// mnuOutput
			// 
			this.mnuOutput.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuOutputCopy,
																					  this.mnuOutputSave});
			// 
			// mnuOutputCopy
			// 
			this.mnuOutputCopy.Index = 0;
			this.mnuOutputCopy.Text = "Copy";
			// 
			// mnuOutputSave
			// 
			this.mnuOutputSave.Index = 1;
			this.mnuOutputSave.Text = "Save Output";
			// 
			// dialogOpen
			// 
			this.dialogOpen.Filter = "All Files (*.*)|*.*";
			// 
			// dialogSave
			// 
			this.dialogSave.FileName = "output.txt";
			this.dialogSave.Filter = "All Files (*.*)|*.*";
			// 
			// FrmCalculator
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(540, 381);
			this.Controls.Add(this.txtOutput);
			this.Controls.Add(this.btnCalculate);
			this.Controls.Add(this.listAlgorithms);
			this.Controls.Add(this.groupBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmCalculator";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Hash Calculator";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
