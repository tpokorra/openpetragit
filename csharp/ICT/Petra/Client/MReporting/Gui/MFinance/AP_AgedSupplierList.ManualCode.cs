//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmAP_AgedSupplierList
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                lblLedger.Text = Catalog.GetString("Ledger: ") + FLedgerNumber.ToString();
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_currency", "Base");
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_date_selection30", dtpDateSelection.Date.Value.AddDays(30));
            ACalc.AddParameter("param_date_selectionSub30", dtpDateSelection.Date.Value.AddDays(-30));
            ACalc.AddParameter("param_date_selection60", dtpDateSelection.Date.Value.AddDays(60));
            ACalc.AddParameter("DueDate", DateTime.Today);

            int ColumnCounter = 0;

            if (chkInvoice.Checked)
            {
                ACalc.AddParameter("param_calculation", "Document Code", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
                ACalc.AddParameter("param_calculation", "Reference", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);
                ACalc.AddParameter("param_calculation", "Discount", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)0.5, ColumnCounter++);
            }
            else
            {
                ACalc.AddParameter("param_calculation", "Supplier", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)5.0, ColumnCounter++);
            }

            ACalc.AddParameter("param_calculation", "Overdue 30+", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "Overdue", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "Due", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "Due 30+", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "Due 60+", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "Total Due", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);

            ACalc.AddParameter("MaxDisplayColumns", ColumnCounter);
        }
    }
}