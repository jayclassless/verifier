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
	/// <summary>A status bar control with a built-in progress bar.</summary>
	public class ProgressStatusBar : StatusBar {
		private ProgressBar progressBar;
		private int progressBarPanel;


		/// <summary>Gets the progress bar that is built into the status bar.</summary>
		public ProgressBar ProgressBar {
			get { return progressBar; }
		}

		/// <summary>Gets or sets the panel index that the progress bar is located in.</summary>
		public int ProgressBarPanel {
			get { return progressBarPanel; }
			set {
				progressBarPanel = value;
				this.Panels[progressBarPanel].Style = StatusBarPanelStyle.OwnerDraw;
			}
		}


		/// <summary>Initializes an instance of ProgressStatusBar.</summary>
		public ProgressStatusBar() {
			progressBar = new ProgressBar();
			progressBar.Hide();
			progressBarPanel = -1;

			this.Controls.Add(progressBar);    
			this.DrawItem += new System.Windows.Forms.StatusBarDrawItemEventHandler(this.progressBar_DrawItem);
		}


		// The status bar is being redrawn.
		private void progressBar_DrawItem(object sender, StatusBarDrawItemEventArgs sbdevent) {
			if (progressBarPanel >= 0) {
				progressBar.Location = new System.Drawing.Point((sbdevent.Bounds.X - 1), (sbdevent.Bounds.Y - 1));
				progressBar.Size = new System.Drawing.Size((sbdevent.Bounds.Width + 2), (sbdevent.Bounds.Height + 2));
				progressBar.Show();
			}
		}
	}
}
