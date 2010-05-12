//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
namespace Tests.Common.Printing
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbbPreview = new System.Windows.Forms.ToolStripButton();
            this.tbbSaveTestText = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtHTMLText = new System.Windows.Forms.TextBox();
            this.tabPreview = new System.Windows.Forms.TabControl();
            this.tbpPrintPreview = new System.Windows.Forms.TabPage();
            this.printPreviewControl1 = new System.Windows.Forms.PrintPreviewControl();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tbbPreviousPage = new System.Windows.Forms.ToolStripButton();
            this.tbbNextPage = new System.Windows.Forms.ToolStripButton();
            this.cmbZoom = new System.Windows.Forms.ToolStripComboBox();
            this.tbpHtmlView = new System.Windows.Forms.TabPage();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPreview.SuspendLayout();
            this.tbpPrintPreview.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tbpHtmlView.SuspendLayout();
            this.SuspendLayout();

            //
            // toolStrip1
            //
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.tbbPreview,
                    this.tbbSaveTestText
                });
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(635, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";

            //
            // tbbPreview
            //
            this.tbbPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbbPreview.Image = ((System.Drawing.Image)(resources.GetObject("tbbPreview.Image")));
            this.tbbPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbPreview.Name = "tbbPreview";
            this.tbbPreview.Size = new System.Drawing.Size(87, 22);
            this.tbbPreview.Text = "Update Preview";
            this.tbbPreview.Click += new System.EventHandler(this.TbbPreviewClick);

            //
            // tbbSaveTestText
            //
            this.tbbSaveTestText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbbSaveTestText.Image = ((System.Drawing.Image)(resources.GetObject("tbbSaveTestText.Image")));
            this.tbbSaveTestText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbSaveTestText.Name = "tbbSaveTestText";
            this.tbbSaveTestText.Size = new System.Drawing.Size(108, 22);
            this.tbbSaveTestText.Text = "Save HTML Testtext";
            this.tbbSaveTestText.Click += new System.EventHandler(this.TbbSaveTestTextClick);

            //
            // splitContainer1
            //
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;

            //
            // splitContainer1.Panel1
            //
            this.splitContainer1.Panel1.Controls.Add(this.txtHTMLText);

            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this.tabPreview);
            this.splitContainer1.Size = new System.Drawing.Size(635, 398);
            this.splitContainer1.SplitterDistance = 59;
            this.splitContainer1.TabIndex = 5;

            //
            // txtHTMLText
            //
            this.txtHTMLText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHTMLText.Location = new System.Drawing.Point(0, 0);
            this.txtHTMLText.Multiline = true;
            this.txtHTMLText.Name = "txtHTMLText";
            this.txtHTMLText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtHTMLText.Size = new System.Drawing.Size(635, 59);
            this.txtHTMLText.TabIndex = 5;

            //
            // tabPreview
            //
            this.tabPreview.Controls.Add(this.tbpPrintPreview);
            this.tabPreview.Controls.Add(this.tbpHtmlView);
            this.tabPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPreview.Location = new System.Drawing.Point(0, 0);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.SelectedIndex = 0;
            this.tabPreview.Size = new System.Drawing.Size(635, 335);
            this.tabPreview.TabIndex = 6;

            //
            // tbpPrintPreview
            //
            this.tbpPrintPreview.Controls.Add(this.printPreviewControl1);
            this.tbpPrintPreview.Controls.Add(this.toolStrip2);
            this.tbpPrintPreview.Location = new System.Drawing.Point(4, 22);
            this.tbpPrintPreview.Name = "tbpPrintPreview";
            this.tbpPrintPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tbpPrintPreview.Size = new System.Drawing.Size(627, 309);
            this.tbpPrintPreview.TabIndex = 0;
            this.tbpPrintPreview.Text = "Print Preview";
            this.tbpPrintPreview.UseVisualStyleBackColor = true;

            //
            // printPreviewControl1
            //
            this.printPreviewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printPreviewControl1.Location = new System.Drawing.Point(3, 28);
            this.printPreviewControl1.Name = "printPreviewControl1";
            this.printPreviewControl1.Size = new System.Drawing.Size(621, 278);
            this.printPreviewControl1.TabIndex = 3;

            //
            // toolStrip2
            //
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.tbbPreviousPage,
                    this.tbbNextPage,
                    this.cmbZoom
                });
            this.toolStrip2.Location = new System.Drawing.Point(3, 3);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(621, 25);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";

            //
            // tbbPreviousPage
            //
            this.tbbPreviousPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbbPreviousPage.Image = ((System.Drawing.Image)(resources.GetObject("tbbPreviousPage.Image")));
            this.tbbPreviousPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbPreviousPage.Name = "tbbPreviousPage";
            this.tbbPreviousPage.Size = new System.Drawing.Size(79, 22);
            this.tbbPreviousPage.Text = "Previous Page";
            this.tbbPreviousPage.Click += new System.EventHandler(this.TbbPreviousPageClick);

            //
            // tbbNextPage
            //
            this.tbbNextPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbbNextPage.Image = ((System.Drawing.Image)(resources.GetObject("tbbNextPage.Image")));
            this.tbbNextPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbNextPage.Name = "tbbNextPage";
            this.tbbNextPage.Size = new System.Drawing.Size(61, 22);
            this.tbbNextPage.Text = "Next Page";
            this.tbbNextPage.Click += new System.EventHandler(this.TbbNextPageClick);

            //
            // cmbZoom
            //
            this.cmbZoom.Items.AddRange(new object[] {
                    "100%",
                    "75%",
                    "50%",
                    "Fit Page"
                });
            this.cmbZoom.Name = "cmbZoom";
            this.cmbZoom.Size = new System.Drawing.Size(121, 25);
            this.cmbZoom.SelectedIndexChanged += new System.EventHandler(this.CmbZoomSelectedIndexChanged);

            //
            // tbpHtmlView
            //
            this.tbpHtmlView.Controls.Add(this.webBrowser1);
            this.tbpHtmlView.Location = new System.Drawing.Point(4, 22);
            this.tbpHtmlView.Name = "tbpHtmlView";
            this.tbpHtmlView.Padding = new System.Windows.Forms.Padding(3);
            this.tbpHtmlView.Size = new System.Drawing.Size(627, 309);
            this.tbpHtmlView.TabIndex = 1;
            this.tbpHtmlView.Text = "HTML View";
            this.tbpHtmlView.UseVisualStyleBackColor = true;

            //
            // webBrowser1
            //
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(3, 3);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(621, 303);
            this.webBrowser1.TabIndex = 1;

            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 423);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "Tests.Common.Printing";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabPreview.ResumeLayout(false);
            this.tbpPrintPreview.ResumeLayout(false);
            this.tbpPrintPreview.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tbpHtmlView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton tbbSaveTestText;
        private System.Windows.Forms.ToolStripComboBox cmbZoom;
        private System.Windows.Forms.ToolStripButton tbbNextPage;
        private System.Windows.Forms.ToolStripButton tbbPreviousPage;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tbbPreview;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TabPage tbpHtmlView;
        private System.Windows.Forms.PrintPreviewControl printPreviewControl1;
        private System.Windows.Forms.TabPage tbpPrintPreview;
        private System.Windows.Forms.TabControl tabPreview;
        private System.Windows.Forms.TextBox txtHTMLText;
    }
}