﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2012 by OM International
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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MCommon.Gui.Setup
{
    public partial class TFrmLocalDataFieldOrderSetup
    {
        // This is the extra dataset that we need that gives us the DataLabel information
        private class FExtraDS
        {
            public static PDataLabelTable PDataLabel;
        }

        /// <summary>
        /// The Context in which we have been launched, eg Family, Church, Personnel etc
        /// </summary>
        public string Context {
            get; set;
        }

        private void RunOnceOnActivationManual()
        {
            // Set up the window title
            if (String.Compare(Context, "Bank", true) == 0)
            {
                this.Text += Catalog.GetString(" For Bank");
            }
            else if (String.Compare(Context, "Church", true) == 0)
            {
                this.Text += Catalog.GetString(" For Church");
            }
            else if (String.Compare(Context, "Family", true) == 0)
            {
                this.Text += Catalog.GetString(" For Family");
            }
            else if (String.Compare(Context, "Organisation", true) == 0)
            {
                this.Text += Catalog.GetString(" For Organisation");
            }
            else if (String.Compare(Context, "Person", true) == 0)
            {
                this.Text += Catalog.GetString(" For Person");
            }
            else if (String.Compare(Context, "Unit", true) == 0)
            {
                this.Text += Catalog.GetString(" For Unit");
            }
            else if (String.Compare(Context, "Venue", true) == 0)
            {
                this.Text += Catalog.GetString(" For Venue");
            }
            else if (String.Compare(Context, "LongTermApp", true) == 0)
            {
                this.Text += Catalog.GetString(" For Long Term Applications");
            }
            else if (String.Compare(Context, "ShortTermApp", true) == 0)
            {
                this.Text += Catalog.GetString(" For Short Term Applications");
            }
            else if (String.Compare(Context, "Personnel", true) == 0)
            {
                this.Text += Catalog.GetString(" For Personnel");
            }

            // Load the Extra Data from DataLabel table
            Type DataTableType;
            FExtraDS.PDataLabel = new PDataLabelTable();
            DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("DataLabelList", String.Empty, null, out DataTableType);
            FExtraDS.PDataLabel.Merge(CacheDT);

            // Extend our main DataLabelUse table
            int NameOrdinal = FMainDS.PDataLabelUse.Columns.Add("Name", typeof(String)).Ordinal;
            int GroupOrdinal = FMainDS.PDataLabelUse.Columns.Add("GroupHeading", typeof(String)).Ordinal;
            int DescriptionOrdinal = FMainDS.PDataLabelUse.Columns.Add("Description", typeof(String)).Ordinal;

            // Take each row of our main dataset and populate the new columns with relevant data
            //   from the DataLabelUse table
            foreach (PDataLabelUseRow useRow in FMainDS.PDataLabelUse.Rows)
            {
                PDataLabelRow labelRow = (PDataLabelRow)FExtraDS.PDataLabel.Rows.Find(new object[] { useRow.DataLabelKey });
                useRow[NameOrdinal] = labelRow.Text;
                useRow[GroupOrdinal] = labelRow.Group;
                useRow[DescriptionOrdinal] = labelRow.Description;
            }

            // Add columns to the grid for the label details
            grdDetails.AddTextColumn("Name", FMainDS.PDataLabelUse.Columns[NameOrdinal]);
            grdDetails.AddTextColumn("Group Heading", FMainDS.PDataLabelUse.Columns[GroupOrdinal]);
            grdDetails.AddTextColumn("Description", FMainDS.PDataLabelUse.Columns[DescriptionOrdinal]);

            // Remove the first column.  We added this in the YAML so that the auto-generator had something to do
            grdDetails.Columns.Remove(0);

            // Create a view that will only show the rows applicable to our currentContext
            DataView contextView = new DataView(FMainDS.PDataLabelUse, "p_use_c='" + Context + "'", "p_idx1_i", DataViewRowState.CurrentRows);
            contextView.AllowNew = false;

            // Bind the view to our grid
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(contextView);
            grdDetails.Refresh();

            // Update the enabled state of the two buttons
            UpdateButtons((grdDetails.Rows.Count > 1) ? 1 : 0);
        }

        private PDataLabelUseRow FPreviouslySelectedDetailRow = null;
        private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
        {
            UpdateButtons(e.Row);
        }

        private void DataFieldPromote(System.Object sender, System.EventArgs e)
        {
            // Move down in the list, i.e. the current row needs a bigger number
            PDataLabelUseRow currentRow = this.GetSelectedDetailRow();
            int nSelectedRow = grdDetails.DataSourceRowToIndex2(grdDetails.SelectedDataRowsAsDataRowView[0]);

            DevAge.ComponentModel.BoundDataView dv = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView);
            PDataLabelUseRow otherRow = (PDataLabelUseRow)dv.DataView[nSelectedRow + 1].Row;

            DoSwap(currentRow, otherRow);

            // Move the selection so it tracks the current row (grid rows start at 1)
            // This will automatically fire the FocusedRowChanged event which will update our up/down buttons
            grdDetails.SelectRowInGrid(nSelectedRow + 2);
        }

        private void DataFieldDemote(System.Object sender, System.EventArgs e)
        {
            // Move up in the list, i.e. the current row needs a smaller number
            PDataLabelUseRow currentRow = this.GetSelectedDetailRow();
            int nSelectedRow = grdDetails.DataSourceRowToIndex2(grdDetails.SelectedDataRowsAsDataRowView[0]);

            DevAge.ComponentModel.BoundDataView dv = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView);
            PDataLabelUseRow otherRow = (PDataLabelUseRow)dv.DataView[nSelectedRow - 1].Row;

            DoSwap(currentRow, otherRow);

            // Move the selection so it tracks the current row (grid rows start at 1)
            // This will automatically fire the FocusedRowChanged event which will update our up/down buttons
            grdDetails.SelectRowInGrid(nSelectedRow);
        }

        private void DoSwap(PDataLabelUseRow Row1, PDataLabelUseRow Row2)
        {
            // Actually do the row updates in the table
            int i = Row1.Idx1;

            Row1.BeginEdit();
            Row1.Idx1 = Row2.Idx1;
            Row1.EndEdit();
            Row2.BeginEdit();
            Row2.Idx1 = i;
            Row2.EndEdit();

            FPetraUtilsObject.SetChangedFlag();
        }

        private void UpdateButtons(int CurrentRow)
        {
            // Set the enabled state of our two buttons
            // The grid rows start at 1 due to the one-row header
            this.btnDemote.Enabled = CurrentRow > 1;
            this.btnPromote.Enabled = CurrentRow < grdDetails.Rows.Count - 1;
        }

        // These four methods are not used because we do not have a details panel beneath the grid
        // However, they need to be here to get the auto-gen code to compile
        private void NewRowManual(ref PDataLabelUseRow ARow)
        {
        }

        private void ShowDetails()
        {
        }

        private void ShowDetails(object o)
        {
        }

        private void GetDetailsFromControls(PDataLabelUseRow ARow, bool AIsNewRow = false)
        {
        }

        private int FPrevRowChangedRow = -1;
    }
}