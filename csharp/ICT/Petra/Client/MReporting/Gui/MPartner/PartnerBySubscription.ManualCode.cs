﻿//
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
using System.Data;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    public partial class TFrmPartnerBySubscription
    {
        private void InitializeManualCode()
        {
            string CheckedMember = "CHECKED";
            string DisplayMember = PPublicationTable.GetPublicationDescriptionDBName();
            string ValueMember = PPublicationTable.GetPublicationCodeDBName();

            DataTable Table = TDataCache.TMPartner.GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum.PublicationList);
            DataView view = new DataView(Table);

            // TODO view.RowFilter = only active publications?
            view.Sort = ValueMember;

            DataTable NewTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });
            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbIncludePublication.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            //clbIncludePublication.SelectionMode = SourceGrid.GridSelectionMode.Row;
            clbIncludePublication.Columns.Clear();
            clbIncludePublication.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            clbIncludePublication.AddTextColumn(Catalog.GetString("Publication Code"), NewTable.Columns[ValueMember], 100);
            clbIncludePublication.AddTextColumn(Catalog.GetString("Publication Description"), NewTable.Columns[DisplayMember], 200);
            clbIncludePublication.DataBindGrid(NewTable, ValueMember, CheckedMember, ValueMember, DisplayMember, false, true, false);

            dtpDateOfSendingCopy.Date = DateTime.Now;
        }

        /// <summary>
        /// only run this code once during activation
        /// </summary>
        private void RunOnceOnActivationManual()
        {
            // no columns tab needed if called from extracts
            if (CalledFromExtracts)
            {
                tabReportSettings.Controls.Remove(tpgColumns);
                tabReportSettings.Controls.Remove(tpgReportSorting);
            }

            // enable autofind in list for first character (so the user can press character to find list entry)
            this.clbIncludePublication.AutoFindColumn = ((Int16)(1));
            this.clbIncludePublication.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;
        }
        
        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (clbIncludePublication.GetCheckedStringList().Length == 0)
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("Select at least one subscription"),
                    Catalog.GetString("Please select at least one subscription, to avoid listing the whole database!"),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }
    }
}