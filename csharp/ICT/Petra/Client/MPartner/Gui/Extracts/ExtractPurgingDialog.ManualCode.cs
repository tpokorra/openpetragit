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
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.Interfaces.MPartner.Extracts.WebConnectors;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    /// manual methods for the generated window
    public partial class TFrmExtractPurgingDialog : System.Windows.Forms.Form
    {
        Boolean FPurgingSuccessful;

        private void InitializeManualCode()
        {
            // set the number of days to default according to settings used in System Manager
            String NumberOfDays = TSystemDefaults.GetSystemDefault
                                      (SharedConstants.SYSDEFAULT_PURGEEXTRACTS, "no,365").Split(',')[1];

            txtNumberOfDays.NumberValueInt = Convert.ToInt32(NumberOfDays);

            FPurgingSuccessful = false;
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

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            // delete extracts on the server
            if (TRemote.MPartner.Partner.WebConnectors.PurgeExtracts(Convert.ToInt32(txtNumberOfDays.Text),
                    chkAllUsers.Checked,
                    cmbUser.GetSelectedString()))
            {
                MessageBox.Show(Catalog.GetString("Purging of extract was successful"),
                    Catalog.GetString("Purge Extracts"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                FPurgingSuccessful = true;
            }
            else
            {
                MessageBox.Show(Catalog.GetString("Purging of extracts failed. Please see server log file for more details."),
                    Catalog.GetString("Purge Extracts"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);

                FPurgingSuccessful = false;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the purging result value
        /// </summary>
        /// <param name="APurgingSuccessful"></param>
        /// <returns></returns>
        public void GetReturnedParameters(out Boolean APurgingSuccessful)
        {
            APurgingSuccessful = FPurgingSuccessful;
        }
    }
}