﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
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
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MSysMan.Gui
{
    /// manual methods for the generated window
    public partial class TFrmIntranetExport
    {
        private TFrmIntranetExportSettings FSettingsPage;
        private DateTime FGiftsSince;
        private Int32 FLedgerNumber;

        /// <summary>
        /// 
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
            }
        }

        private void InitializeManualCode()
        {
            FSettingsPage = new TFrmIntranetExportSettings(this);
        }

        private void UpdateDonationsLabel()
        {
            FGiftsSince = DateTime.Now.AddDays(0 - FSettingsPage.DonationDays);
            lblGiftsSince.Text = String.Format(Catalog.GetString("Donations Since {0}"), FGiftsSince.ToString("dd MMM yyyy"));
        }

        private void RunOnceOnActivationManual()
        {
            chkDonationData.Checked = TUserDefaults.GetBooleanDefault("IntranetExportDonations", false);
            chkFieldData.Checked = TUserDefaults.GetBooleanDefault("IntranetExportField", false);
            chkPersonData.Checked = TUserDefaults.GetBooleanDefault("IntranetExportPerson", false);
            lblGiftsSince.Width = 200;
            lblGiftsSince.Left = 40;
            UpdateDonationsLabel();
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            Boolean ExportDonationData = chkDonationData.Checked;
            Boolean ExportFieldData = chkFieldData.Checked;
            Boolean ExportPersonData = chkPersonData.Checked;
            TUserDefaults.SetDefault("IntranetExportDonations", ExportDonationData);
            TUserDefaults.SetDefault("IntranetExportField", ExportFieldData);
            TUserDefaults.SetDefault("IntranetExportPerson", ExportPersonData);
            String ServerStr = TRemote.MCommon.WebConnectors.ExportToFile(ExportDonationData, ExportFieldData, ExportPersonData,
                FSettingsPage.Password, FSettingsPage.DonationDays, FSettingsPage.OptionalMetadata);
            MessageBox.Show(ServerStr);
            Close();
        }

        private void BtnCancel_Click(Object Sender, EventArgs e)
        {
            Close();
        }

        private void BtnSettings_Click(Object Sender, EventArgs e)
        {
            FSettingsPage.ShowDialog();
            UpdateDonationsLabel();
        }

        private void BtnMotivations_Click(Object Sender, EventArgs e)
        {
            TFrmIntranetExportMotivations MotivationScreen = new TFrmIntranetExportMotivations(this);
            MotivationScreen.ShowDialog();
        }
    }
}