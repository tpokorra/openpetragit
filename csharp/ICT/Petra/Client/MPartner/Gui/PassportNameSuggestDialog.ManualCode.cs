﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// manual methods for the generated window
    public partial class TFrmPassportNameSuggestDialog : System.Windows.Forms.Form
    {
		/// <summary>
		/// set the initial value for passport name in the dialog
		/// </summary>
		/// <param name="APassportName"></param>
    	public void SetPassportName (String APassportName)
        {
	        txtPassportName.Text = APassportName;
        }
        
        private void InitializeManualCode()
        {
            lblExplanation.Text = Catalog.GetString(
                "To use the short name listed below simply select 'OK'." + "\r\n" +
                "You may change the name and then select 'OK'." + "\r\n" +
                "Select 'Cancel' to ignore any changes you have made." + "\r\n" +
                "The Family Name must be put in brackets." + "\r\n" +
                "For example: Mike (Miller)"+ "\r\n");
        }

        private void CustomClosingHandler(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CanClose())
            {
                // MessageBox.Show('TFrmReportingPeriodSelectionDialog.TFormPetra_Closing: e.Cancel := true');
                e.Cancel = true;
            }
            else
            {
                //TODO UnRegisterUIConnector();

                // Needs to be set to false because it got set to true in ancestor Form!
                e.Cancel = false;

                // Need to call the following method in the Base Form to remove this Form from the Open Forms List
                FPetraUtilsObject.TFrmPetra_Closing(this, null);
            }
        }

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the values of Fields
        /// on the screen.
        ///
        /// </summary>
        /// <param name="AReasonEnded">Text that gives the reason for ending the Subscriptions</param>
        /// <param name="ADateEnded">Date when the Subscriptions should end (can be empty)</param>
        /// <returns>false if the Dialog is still uninitialised, otherwise true.
        /// </returns>
        public Boolean GetReturnedParameters(out String APassportName)
        {
            Boolean ReturnValue = true;

            APassportName = txtPassportName.Text;

            return ReturnValue;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}