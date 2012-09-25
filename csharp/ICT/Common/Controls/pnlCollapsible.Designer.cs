﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
namespace Ict.Common.Controls
{
    partial class TPnlCollapsible
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the control.
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPnlCollapsible));
            this.pnlTitle = new Ict.Common.Controls.TPnlGradient();
            this.pnlTitleText = new Ict.Common.Controls.TPnlGradient();
            this.lblDetailHeading = new System.Windows.Forms.Label();
            this.btnToggle = new System.Windows.Forms.Button();
            this.imlUpDownArrows = new System.Windows.Forms.ImageList(this.components);
            this.pnlContent = new Ict.Common.Controls.TPnlGradient();
            this.tipCollapseExpandHints = new System.Windows.Forms.ToolTip(this.components);
            this.pnlTitle.SuspendLayout();
            this.pnlTitleText.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTitle
            //
            this.pnlTitle.AutoSize = true;
            this.pnlTitle.Controls.Add(this.pnlTitleText);
            this.pnlTitle.Controls.Add(this.pnlContent);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(424, 150);
            this.pnlTitle.TabIndex = 2;
            //
            // pnlTitleText
            //
            this.pnlTitleText.AutoSize = true;
            this.pnlTitleText.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;           
            this.pnlTitleText.BackColor = System.Drawing.Color.Transparent;
            this.pnlTitleText.Controls.Add(this.btnToggle);
            this.pnlTitleText.Controls.Add(this.lblDetailHeading);            
            this.pnlTitleText.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleText.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleText.Name = "pnlTitleText";
            this.pnlTitleText.Size = new System.Drawing.Size(424, 24);
            this.pnlTitleText.TabIndex = 1;
            this.pnlTitleText.Parent = pnlTitle;
            this.tipCollapseExpandHints.SetToolTip(this.pnlTitleText, "Click here to expand / collapse the panel");
            this.pnlTitleText.MouseLeave += new System.EventHandler(this.BtnToggleMouseLeave);
            this.pnlTitleText.Click += new System.EventHandler(this.BtnToggleClick);
            this.pnlTitleText.MouseEnter += new System.EventHandler(this.BtnToggleMouseEnter);
            //
            // lblDetailHeading
            //
            this.lblDetailHeading.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.lblDetailHeading.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailHeading.ForeColor = System.Drawing.Color.MediumBlue;
            this.lblDetailHeading.Parent = pnlTitleText;
            this.lblDetailHeading.Location = new System.Drawing.Point(2, 2);
            this.lblDetailHeading.Name = "lblDetailHeading";
            this.lblDetailHeading.Size = new System.Drawing.Size(395, 18);
            this.lblDetailHeading.TabIndex = 1;
            this.lblDetailHeading.Text = "Collapsible Panel";            
            this.lblDetailHeading.AutoEllipsis = true;
            this.tipCollapseExpandHints.SetToolTip(this.lblDetailHeading, "Click here to expand / collapse the panel");
            this.lblDetailHeading.Click += new System.EventHandler(this.BtnToggleClick);
            //
            // btnToggle
            //
            this.btnToggle.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggle.BackColor = System.Drawing.Color.Transparent;
            this.btnToggle.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggle.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggle.ImageIndex = 0;
            this.btnToggle.ImageList = this.imlUpDownArrows;
            this.btnToggle.Location = new System.Drawing.Point(408, 5);
            this.btnToggle.Name = "btnToggle";
            this.btnToggle.Parent = pnlTitleText;
            this.btnToggle.Size = new System.Drawing.Size(18, 18);
            this.btnToggle.TabIndex = 0;
            this.btnToggle.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.tipCollapseExpandHints.SetToolTip(this.btnToggle, "Expands / collapses the panel");
            this.btnToggle.UseVisualStyleBackColor = false;
            this.btnToggle.MouseLeave += new System.EventHandler(this.BtnToggleMouseLeave);
            this.btnToggle.Click += new System.EventHandler(this.BtnToggleClick);
            this.btnToggle.MouseEnter += new System.EventHandler(this.BtnToggleMouseEnter);
            //
            // imlUpDownArrows
            //
            this.imlUpDownArrows.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlUpDownArrows.ImageStream")));
            this.imlUpDownArrows.TransparentColor = System.Drawing.Color.Transparent;
            this.imlUpDownArrows.Images.SetKeyName(0, "CollapseDown.ico");
            this.imlUpDownArrows.Images.SetKeyName(1, "CollapseDown_Hover.ico");
            this.imlUpDownArrows.Images.SetKeyName(2, "CollapseLeft.ico");
            this.imlUpDownArrows.Images.SetKeyName(3, "CollapseLeft_Hover.ico");
            this.imlUpDownArrows.Images.SetKeyName(4, "CollapseRight.ico");
            this.imlUpDownArrows.Images.SetKeyName(5, "CollapseRight_Hover.ico");
            this.imlUpDownArrows.Images.SetKeyName(6, "CollapseUp.ico");
            this.imlUpDownArrows.Images.SetKeyName(7, "CollapseUp_Hover.ico");
            this.imlUpDownArrows.Images.SetKeyName(8, "TaskPanel_CollapseDown.ico");
            this.imlUpDownArrows.Images.SetKeyName(9, "TaskPanel_CollapseDown_Hover.ico");
            this.imlUpDownArrows.Images.SetKeyName(10, "TaskPanel_CollapseUp.ico");
            this.imlUpDownArrows.Images.SetKeyName(11, "TaskPanel_CollapseUp_Hover.ico");
            //
            // pnlContent
            //
            this.pnlContent.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContent.BackColor = System.Drawing.SystemColors.Info;
            this.pnlContent.Font = new System.Drawing.Font("Verdana", 6.75F);
            this.pnlContent.Location = new System.Drawing.Point(0, 28);
            this.pnlContent.Margin = new System.Windows.Forms.Padding(0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(424, 126);
            this.pnlContent.TabIndex = 2;
            //
            // TPnlCollapsible
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.pnlTitle);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TPnlCollapsible";
            this.Size = new System.Drawing.Size(424, 150);
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitleText.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        
        private System.Windows.Forms.ToolTip tipCollapseExpandHints;
        private System.Windows.Forms.ImageList imlUpDownArrows;
        private System.Windows.Forms.Button btnToggle;
        private System.Windows.Forms.Label lblDetailHeading;
        private Ict.Common.Controls.TPnlGradient pnlTitleText;
        private Ict.Common.Controls.TPnlGradient pnlTitle;
        private Ict.Common.Controls.TPnlGradient pnlContent;
    }
}
