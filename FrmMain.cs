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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;

namespace Classless.Verifier {
	/// <summary>The main window of the application.</summary>
	public class FrmMain : Form {
		#region Control Declarations
		private ProgressStatusBar barStatus;

		private System.Windows.Forms.MainMenu mnuMain;
		private System.Windows.Forms.OpenFileDialog dialogOpen;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuTools;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem mnuHelpAbout;
		private System.Windows.Forms.MenuItem mnuHelpHomePage;
		private System.Windows.Forms.MenuItem mnuHelpClassless;
		private System.Windows.Forms.MenuItem mnuFileOpen;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnuFileVerify;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem mnuFileExit;
		private System.Windows.Forms.MenuItem mnuToolsCalculator;
		private System.Windows.Forms.StatusBarPanel panelStatus;
		private System.Windows.Forms.StatusBarPanel panelProgress;
		private System.Windows.Forms.ListView listFiles;
		private System.Windows.Forms.ColumnHeader colFilesFile;
		private System.Windows.Forms.ColumnHeader colFilesHash;
		private System.Windows.Forms.MenuItem mnuHelpXMLHomePage;
		private System.Windows.Forms.Button btnVerify;
		private System.Windows.Forms.ImageList ilFileIcons;
		private System.ComponentModel.IContainer components;
		#endregion


		private FileList fileList;
		private int fileListGood;
		private int fileListBad;
		private int fileListMissing;
		private FileListProcessor listProcessor;
		private System.Windows.Forms.ColumnHeader colFilesType;
		private Thread listProcessorThread;


		/// <summary>Gets or sets the current file list.</summary>
		public FileList FileList {
			get { return fileList; }
			set {
				fileList = value;

				// Clear out the file list.
				listFiles.Items.Clear();

				// Load the file list with the new files (if given any).
				if (fileList != null) {
					ListViewItem lvi;
					foreach(FileListFile f in fileList.filelist) {
						lvi = new ListViewItem(f.name);
						lvi.SubItems.Add(Algorithm.GetShortNameFromType(f.type));
						lvi.SubItems.Add(f.Value.ToUpper(System.Globalization.CultureInfo.InvariantCulture));
						lvi.Tag = f;
						lvi.ImageIndex = 0;
						listFiles.Items.Add(lvi);
					}

					panelStatus.Text = listFiles.Items.Count.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + " Files";
				} else {
					panelStatus.Text = string.Empty;
				}
			}
		}


		/// <summary>Initializes an instance of FrmMain.</summary>
		public FrmMain() {
			InitializeComponent();
			StartEventProcessing();

			// Set up the progress bar.
			barStatus.ProgressBarPanel = 1;
			barStatus.ProgressBar.Minimum = 0;
			barStatus.ProgressBar.Maximum = 100;
		}


		/// <summary>Clean up any resources being used.</summary>
		protected override void Dispose(bool disposing) {
			if ((disposing) && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}


		// Opens a URL in the default browser.
		private void OpenURL(string url) {
			System.Diagnostics.Process.Start(url);
		}


		// Lock the GUI so the user can't do anything harmful while we're processing.
		private void LockGUI() {
			btnVerify.Text = "Please wait...";
			btnVerify.Enabled = false;
			mnuFileOpen.Enabled = false;
			mnuFileVerify.Enabled = false;
			mnuFileExit.Enabled = false;
			this.Cursor = Cursors.WaitCursor;
			listFiles.Cursor = Cursors.WaitCursor;
			Application.DoEvents();
		}


		// Unlock the GUI.
		private void UnlockGUI() {
			btnVerify.Text = "&Verify Files";
			btnVerify.Enabled = true;
			mnuFileOpen.Enabled = true;
			mnuFileVerify.Enabled = true;
			mnuFileExit.Enabled = true;
			this.Cursor = Cursors.Default;
			listFiles.Cursor = Cursors.Default;
			this.Text = "Verifier";
			Application.DoEvents();
		}


		#region Events
		// Hook into the event processing.
		private void StartEventProcessing() {
			btnVerify.Click += new System.EventHandler(this.initiateVerify);
			mnuFileOpen.Click += new System.EventHandler(this.mnuFileOpen_Click);
			mnuFileVerify.Click += new System.EventHandler(this.initiateVerify);
			mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
			mnuToolsCalculator.Click += new System.EventHandler(this.mnuToolsCalculator_Click);
			mnuHelpHomePage.Click += new System.EventHandler(this.mnuHelpHomePage_Click);
			mnuHelpXMLHomePage.Click += new System.EventHandler(this.mnuHelpXMLHomePage_Click);
			mnuHelpClassless.Click += new System.EventHandler(this.mnuHelpClassless_Click);
			mnuHelpAbout.Click += new System.EventHandler(this.mnuHelpAbout_Click);
		}

		// User wants to verify the current file list.
		private void initiateVerify(object sender, System.EventArgs args) {
			// Make sure there's a list to work with.
			if (FileList == null) {
				MessageBox.Show("No file verification list has been opened.", "Verify", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// Clear things up in preparation.
			panelStatus.Text = string.Empty;
			barStatus.ProgressBar.Value = 0;
			foreach (ListViewItem lvi in listFiles.Items) {
				lvi.ImageIndex = 0;
			}
			Application.DoEvents();
			fileListGood = fileListBad = fileListMissing = 0;

			// Set up the processor.
			listProcessor = new FileListProcessor(FileList);
			listProcessor.Progress += new FileListProcessor.ProgressEventHandler(this.processorProgress);
			listProcessor.Completion += new FileListProcessor.CompletionEventHandler(this.processorCompletion);

			// Lock up the GUI.
			LockGUI();

			// Start the processor.
			listProcessorThread = new Thread(new ThreadStart(listProcessor.Start));
			listProcessorThread.IsBackground = true;
			listProcessorThread.Start();
		}

		// The FileListProcessor has made some progress.
		private void processorProgress(object sender, ProgressEventArgs args) {
			foreach(ListViewItem lvi in listFiles.Items) {
				if (lvi.Tag == args.File) {
					// Show the status.
					switch (args.Status) {
						case FileListProcessorStatus.Good:		lvi.ImageIndex = 1; fileListGood++;	break;
						case FileListProcessorStatus.Bad:		lvi.ImageIndex = 2; fileListBad++;	break;
						case FileListProcessorStatus.NotFound:	lvi.ImageIndex = 3; fileListMissing++; break;
						//case FileListProcessorStatus.Error:		lvi.ImageIndex = 4; fileListBad++;	break;
						//case FileListProcessorStatus.InProcess:	lvi.ImageIndex = 5; break;
						case FileListProcessorStatus.WrongSize:	lvi.ImageIndex = 3; fileListBad++;	break;
					}

					// Scroll if needed.
					lvi.EnsureVisible();

					// Show the overall progress.
					this.Text = "Verifier - Processing (" + args.PercentCompleteTotal.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + "%)";
					barStatus.ProgressBar.Value = args.PercentCompleteTotal;
					panelStatus.Text = listFiles.Items.Count.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + " Files: " +
						fileListGood.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + " Good / " +
						fileListBad.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + " Bad / " +
						fileListMissing.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + " Missing";

					return;
				}
			}
		}

		// The FileListProcessor completed.
		private void processorCompletion(object sender, CompletionEventArgs args) {
			listProcessor = null;

			// Open the GUI.
			UnlockGUI();
			barStatus.ProgressBar.Value = 0;

			// If there's an error, show it to the user.
			if (args.Exception != null) {
				MessageBox.Show("There was an error while processing the files:" +
					Environment.NewLine + Environment.NewLine +
					args.Exception.Message,
					"Verify", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// User wants to open a file verification list.
		private void mnuFileOpen_Click(object sender, System.EventArgs args) {
			DialogResult dr = dialogOpen.ShowDialog(this);

			if (dr == DialogResult.OK) {
				string temp = dialogOpen.FileName;

				try {
					FileList = new FileList(temp);
				} catch (FileTypeException) {
					// Unknown format.
					MessageBox.Show(temp + " does not contain a supported file verification list format.",
						"Verify", MessageBoxButtons.OK, MessageBoxIcon.Error);
				} catch (Exception ex) {
					// Bad error.
					MessageBox.Show("Could not open file verification list:" +
						Environment.NewLine + Environment.NewLine +
						temp +
						Environment.NewLine + Environment.NewLine +
						ex.Message,
						"Verify", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		// User wants to quit.
		private void mnuFileExit_Click(object sender, System.EventArgs args) {
			Application.Exit();
		}

		// User wants to use the hash calculator.
		private void mnuToolsCalculator_Click(object sender, System.EventArgs args) {
			FrmCalculator fc = new FrmCalculator();
			fc.Show();
		}

		// User wants to view the Verifier home page.
		private void mnuHelpHomePage_Click(object sender, System.EventArgs args) {
			OpenURL(Verifier.GetHomePage());
		}

		// User wants to view the VerifyXML home page.
		private void mnuHelpXMLHomePage_Click(object sender, System.EventArgs args) {
			OpenURL("http://www.classless.net/projects/verifyxml/");
		}

		// User wants to view the Classless.net home page.
		private void mnuHelpClassless_Click(object sender, System.EventArgs args) {
			OpenURL("http://www.classless.net/");
		}

		// User wants to view the about window.
		private void mnuHelpAbout_Click(object sender, System.EventArgs args) {
			FrmAbout fa = new FrmAbout();
			fa.ShowDialog(this);
		}
		#endregion


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmMain));
			this.mnuMain = new System.Windows.Forms.MainMenu();
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuFileOpen = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mnuFileVerify = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.mnuFileExit = new System.Windows.Forms.MenuItem();
			this.mnuTools = new System.Windows.Forms.MenuItem();
			this.mnuToolsCalculator = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuHelpHomePage = new System.Windows.Forms.MenuItem();
			this.mnuHelpXMLHomePage = new System.Windows.Forms.MenuItem();
			this.mnuHelpClassless = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.mnuHelpAbout = new System.Windows.Forms.MenuItem();
			this.barStatus = new Classless.Verifier.ProgressStatusBar();
			this.panelStatus = new System.Windows.Forms.StatusBarPanel();
			this.panelProgress = new System.Windows.Forms.StatusBarPanel();
			this.dialogOpen = new System.Windows.Forms.OpenFileDialog();
			this.listFiles = new System.Windows.Forms.ListView();
			this.colFilesFile = new System.Windows.Forms.ColumnHeader();
			this.colFilesType = new System.Windows.Forms.ColumnHeader();
			this.colFilesHash = new System.Windows.Forms.ColumnHeader();
			this.ilFileIcons = new System.Windows.Forms.ImageList(this.components);
			this.btnVerify = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.panelStatus)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelProgress)).BeginInit();
			this.SuspendLayout();
			// 
			// mnuMain
			// 
			this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFile,
																					this.mnuTools,
																					this.mnuHelp});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFileOpen,
																					this.menuItem4,
																					this.mnuFileVerify,
																					this.menuItem7,
																					this.mnuFileExit});
			this.mnuFile.Text = "&File";
			// 
			// mnuFileOpen
			// 
			this.mnuFileOpen.Index = 0;
			this.mnuFileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.mnuFileOpen.Text = "&Open...";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.Text = "-";
			// 
			// mnuFileVerify
			// 
			this.mnuFileVerify.Index = 2;
			this.mnuFileVerify.Text = "&Verify List";
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 3;
			this.menuItem7.Text = "-";
			// 
			// mnuFileExit
			// 
			this.mnuFileExit.Index = 4;
			this.mnuFileExit.Shortcut = System.Windows.Forms.Shortcut.AltF4;
			this.mnuFileExit.Text = "E&xit";
			// 
			// mnuTools
			// 
			this.mnuTools.Index = 1;
			this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuToolsCalculator});
			this.mnuTools.Text = "&Tools";
			// 
			// mnuToolsCalculator
			// 
			this.mnuToolsCalculator.Index = 0;
			this.mnuToolsCalculator.Text = "&Calculator";
			// 
			// mnuHelp
			// 
			this.mnuHelp.Index = 2;
			this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuHelpHomePage,
																					this.mnuHelpXMLHomePage,
																					this.mnuHelpClassless,
																					this.menuItem5,
																					this.mnuHelpAbout});
			this.mnuHelp.Text = "&Help";
			// 
			// mnuHelpHomePage
			// 
			this.mnuHelpHomePage.Index = 0;
			this.mnuHelpHomePage.Text = "&Verifier Home Page";
			// 
			// mnuHelpXMLHomePage
			// 
			this.mnuHelpXMLHomePage.Index = 1;
			this.mnuHelpXMLHomePage.Text = "Verifier&XML Home Page";
			// 
			// mnuHelpClassless
			// 
			this.mnuHelpClassless.Index = 2;
			this.mnuHelpClassless.Text = "C&lassless.net";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 3;
			this.menuItem5.Text = "-";
			// 
			// mnuHelpAbout
			// 
			this.mnuHelpAbout.Index = 4;
			this.mnuHelpAbout.Text = "&About";
			// 
			// barStatus
			// 
			this.barStatus.Location = new System.Drawing.Point(0, 233);
			this.barStatus.Name = "barStatus";
			this.barStatus.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.panelStatus,
																						 this.panelProgress});
			this.barStatus.ProgressBarPanel = 1;
			this.barStatus.ShowPanels = true;
			this.barStatus.Size = new System.Drawing.Size(552, 20);
			this.barStatus.TabIndex = 0;
			// 
			// panelStatus
			// 
			this.panelStatus.MinWidth = 230;
			this.panelStatus.Width = 230;
			// 
			// panelProgress
			// 
			this.panelProgress.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.panelProgress.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.panelProgress.Width = 306;
			// 
			// dialogOpen
			// 
			this.dialogOpen.Filter = "All Supported (*.verify;*.vfy;*.sfv;*.md5;*.md5sum)|*.verify;*.vfy;*.sfv;*.md5;*." +
				"md5sum|VerifyXML File (*.verify;*.vfy)|*.verify;*.vfy|SFV File (*.sfv)|*.sfv|MD5" +
				" Checksums (*.md5;*.md5sum)|*.md5;*.md5sum";
			this.dialogOpen.Title = "Open File Verification List";
			// 
			// listFiles
			// 
			this.listFiles.AllowDrop = true;
			this.listFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.colFilesFile,
																						this.colFilesType,
																						this.colFilesHash});
			this.listFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listFiles.FullRowSelect = true;
			this.listFiles.GridLines = true;
			this.listFiles.Location = new System.Drawing.Point(0, 0);
			this.listFiles.Name = "listFiles";
			this.listFiles.Size = new System.Drawing.Size(552, 210);
			this.listFiles.SmallImageList = this.ilFileIcons;
			this.listFiles.TabIndex = 1;
			this.listFiles.View = System.Windows.Forms.View.Details;
			// 
			// colFilesFile
			// 
			this.colFilesFile.Text = "File";
			this.colFilesFile.Width = 240;
			// 
			// colFilesType
			// 
			this.colFilesType.Text = "Type";
			// 
			// colFilesHash
			// 
			this.colFilesHash.Text = "Hash";
			this.colFilesHash.Width = 230;
			// 
			// ilFileIcons
			// 
			this.ilFileIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ilFileIcons.ImageSize = new System.Drawing.Size(16, 16);
			this.ilFileIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilFileIcons.ImageStream")));
			this.ilFileIcons.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// btnVerify
			// 
			this.btnVerify.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnVerify.Location = new System.Drawing.Point(0, 210);
			this.btnVerify.Name = "btnVerify";
			this.btnVerify.Size = new System.Drawing.Size(552, 23);
			this.btnVerify.TabIndex = 2;
			this.btnVerify.Text = "&Verify Files";
			// 
			// FrmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(552, 253);
			this.Controls.Add(this.listFiles);
			this.Controls.Add(this.btnVerify);
			this.Controls.Add(this.barStatus);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mnuMain;
			this.Name = "FrmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Verifier";
			((System.ComponentModel.ISupportInitialize)(this.panelStatus)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelProgress)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
