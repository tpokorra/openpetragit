//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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
using System;
using System.Drawing;
using System.Windows.Forms;
using Ict.Common;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Description of GLRevaluation.
    /// </summary>
    public partial class TGLRevaluation : Form
    {
        /// <summary>
        /// Runs the revalation ...
        /// </summary>
        /// <param name="AParentFormHandle"></param>
        public TGLRevaluation(IntPtr AParentFormHandle) : base()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblName.Text = Catalog.GetString("Select cost center:");
            this.btnRevaluate.Text = Catalog.GetString("Revaluate");
            this.btnCancel.Text = Catalog.GetString("Cancel");
            this.Text = Catalog.GetString("Revaluation ...");
            #endregion


            this.btnCancel.Click += new EventHandler(CancelRevaluation);
            this.btnRevaluate.Click += new EventHandler(RunRevaluation);
        }
    }
}