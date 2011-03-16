﻿//
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
using System;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Printing;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TFrmPartnerContacts
    {
        private void InitializeManualCode()
        {
        }

        private void Search(object sender, EventArgs e)
        {
            FMainDS.PPartnerContact.Clear();

            FMainDS.Merge(TRemote.MPartner.Partner.WebConnectors.FindContacts(
                    txtContactor.Text,
                    dtpContactDate.Date,
                    txtCommentContains.Text,
                    txtMethodOfContact.Text,
                    txtModule.Text,
                    txtMailingCode.Text));

            FMainDS.PPartnerContact.DefaultView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.PPartnerContact.DefaultView);
        }

        private void DeleteContacts(object Sender, EventArgs e)
        {
            TVerificationResultCollection VerificationResult;

            if (MessageBox.Show(
                    String.Format(Catalog.GetString("Do you really want to delete all {0} contacts?"), FMainDS.PPartnerContact.Count),
                    Catalog.GetString("Confirm deletion"),
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                foreach (PPartnerContactRow row in FMainDS.PPartnerContact.Rows)
                {
                    row.Delete();
                }

                if (!TRemote.MPartner.Partner.WebConnectors.DeleteContacts(FMainDS.PPartnerContact, out VerificationResult))
                {
                    MessageBox.Show(Catalog.GetString("There was a problem deleting the contacts"), Catalog.GetString("Failure"));
                }
                else
                {
                    // refresh the grid
                    Search(Sender, e);
                }
            }
        }
    }
}