//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;

namespace Ict.Common.Controls
{
    partial class TPnlAccordion
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            new System.ComponentModel.ComponentResourceManager(typeof(TRbtNavigationButton));

            this.lblCaption = new System.Windows.Forms.Label();
            this.pbxIcon = new System.Windows.Forms.PictureBox();

            this.SuspendLayout();

            //
            // lblCaption
            //
            this.lblCaption.BackColor = System.Drawing.Color.Transparent;
            this.lblCaption.Location = new System.Drawing.Point(31, 7);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.lblCaption.Size = new System.Drawing.Size(168, 22);

            //
            // pbxIcon
            //
            this.pbxIcon.BackColor = System.Drawing.Color.Transparent;
            this.pbxIcon.Location = new System.Drawing.Point(8, 5);
            this.pbxIcon.Name = "pbxIcon";
            this.pbxIcon.Size = new System.Drawing.Size(32, 32);

            //
            // TRbtNavigationButton
            //
            this.Size = new System.Drawing.Size(184, 28);
            this.Controls.Add(lblCaption);
            this.Controls.Add(pbxIcon);
            this.Name = "TRbtNavigationButton";

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.PictureBox pbxIcon;
    }
}