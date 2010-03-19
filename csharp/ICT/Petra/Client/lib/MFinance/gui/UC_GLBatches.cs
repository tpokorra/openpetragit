/* auto generated with nant generateWinforms from UC_GLBatches.yaml and template controlMaintainTable
 *
 * DO NOT edit manually, DO NOT edit with the designer
 *
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{

  /// auto generated user control
  public partial class TUC_GLBatches: System.Windows.Forms.UserControl, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    private Ict.Petra.Shared.MFinance.GL.Data.GLBatchTDS FMainDS;

    /// constructor
    public TUC_GLBatches() : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblLedgerNumber.Text = Catalog.GetString("Ledger:");
      this.rbtPosting.Text = Catalog.GetString("Posting");
      this.rbtEditing.Text = Catalog.GetString("Editing");
      this.rbtAll.Text = Catalog.GetString("All");
      this.rgrShowBatches.Text = Catalog.GetString("Show batches available for");
      this.btnNew.Text = Catalog.GetString("&Add");
      this.btnDelete.Text = Catalog.GetString("&Cancel");
      this.btnPostBatch.Text = Catalog.GetString("&Post Batch");
      this.lblDetailBatchDescription.Text = Catalog.GetString("Batch Description:");
      this.lblDetailBatchControlTotal.Text = Catalog.GetString("Batch Hash Total:");
      this.lblDetailDateEffective.Text = Catalog.GetString("Effective Date:");
      this.lblValidDateRange.Text = Catalog.GetString("Valid Date Range:");
      this.tbbPostBatch.Text = Catalog.GetString("&Post Batch");
      this.tbbImportFromSpreadSheet.Text = Catalog.GetString("Import From Spread Sheet");
      this.mniPost.Text = Catalog.GetString("&Post Batch");
      this.mniImportFromSpreadSheet.Text = Catalog.GetString("Import From Spread Sheet");
      this.mniBatch.Text = Catalog.GetString("&Batch");
      #endregion

    }

    /// helper object for the whole screen
    public TFrmPetraEditUtils PetraUtilsObject
    {
        set
        {
            FPetraUtilsObject = value;
        }
    }

    /// dataset for the whole screen
    public Ict.Petra.Shared.MFinance.GL.Data.GLBatchTDS MainDS
    {
        set
        {
            FMainDS = value;
        }
    }

    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
      FPetraUtilsObject.SetStatusBarText(txtDetailBatchDescription, Catalog.GetString("Enter a description for this general ledger batch."));
      FPetraUtilsObject.SetStatusBarText(txtDetailBatchControlTotal, Catalog.GetString("(Optional) Enter the total amount of the batch (hash total)."));
      FPetraUtilsObject.SetStatusBarText(dtpDetailDateEffective, Catalog.GetString("Enter the date for which this batch is to take effect."));
      grdDetails.Columns.Clear();
      grdDetails.AddTextColumn("Batch Number", FMainDS.ABatch.ColumnBatchNumber);
      grdDetails.AddTextColumn("Batch status", FMainDS.ABatch.ColumnBatchStatus);
      grdDetails.AddTextColumn("Effective Date", FMainDS.ABatch.ColumnDateEffective);
      grdDetails.AddTextColumn("Batch Debit Total", FMainDS.ABatch.ColumnBatchDebitTotal);
      grdDetails.AddTextColumn("Batch Credit Total", FMainDS.ABatch.ColumnBatchCreditTotal);
      grdDetails.AddTextColumn("Batch Control Total", FMainDS.ABatch.ColumnBatchControlTotal);
      grdDetails.AddTextColumn("Batch Description", FMainDS.ABatch.ColumnBatchDescription);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      DataView myDataView = FMainDS.ABatch.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
      grdDetails.AutoSizeCells();

      ShowData();
    }

    /// automatically generated, create a new record of ABatch and display on the edit screen
    public bool CreateNewABatch()
    {
        FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.CreateABatch(FLedgerNumber));

        FPetraUtilsObject.SetChangedFlag();

        grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ABatch.DefaultView);
        grdDetails.Refresh();
        SelectDetailRowByDataTableIndex(FMainDS.ABatch.Rows.Count - 1);

        return true;
    }

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.ABatch.PrimaryKey)
            {
                string value1 = FMainDS.ABatch.Rows[ARowNumberInTable][myColumn].ToString();
                string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[Counter][myColumn.Ordinal].ToString();
                if (value1 != value2)
                {
                    found = false;
                }
            }
            if (found)
            {
                RowNumberGrid = Counter + 1;
            }
        }
        grdDetails.Selection.ResetSelection(false);
        grdDetails.Selection.SelectRow(RowNumberGrid, true);
        // scroll to the row
        grdDetails.ShowCell(new SourceGrid.Position(RowNumberGrid, 0), true);

        FocusedRowChanged(this, new SourceGrid.RowEventArgs(RowNumberGrid));
    }

    /// return the selected row
    private ABatchRow GetSelectedDetailRow()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            return (ABatchRow)SelectedGridRow[0].Row;
        }

        return null;
    }

    private void ShowData()
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        ShowDataManual();
        pnlDetails.Enabled = false;
        if (FMainDS.ABatch != null)
        {
            DataView myDataView = FMainDS.ABatch.DefaultView;
            myDataView.Sort = "a_batch_number_i DESC";
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            grdDetails.AutoSizeCells();
            if (myDataView.Count > 0)
            {
                grdDetails.Selection.ResetSelection(false);
                grdDetails.Selection.SelectRow(1, true);
                FocusedRowChanged(this, new SourceGrid.RowEventArgs(1));
                pnlDetails.Enabled = true;
            }
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private void ShowDetails(ABatchRow ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        if (ARow == null)
        {
            pnlDetails.Enabled = false;
            ShowDetailsManual(ARow);
        }
        else
        {
            pnlDetails.Enabled = true;
            FPreviouslySelectedDetailRow = ARow;
            if (ARow.IsBatchDescriptionNull())
            {
                txtDetailBatchDescription.Text = String.Empty;
            }
            else
            {
                txtDetailBatchDescription.Text = ARow.BatchDescription;
            }
            if (ARow.IsBatchControlTotalNull())
            {
                txtDetailBatchControlTotal.Text = String.Empty;
            }
            else
            {
                txtDetailBatchControlTotal.Text = ARow.BatchControlTotal.ToString();
            }
            dtpDetailDateEffective.Value = ARow.DateEffective;
            ShowDetailsManual(ARow);
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private ABatchRow FPreviouslySelectedDetailRow = null;
    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
        // get the details from the previously selected row
        if (FPreviouslySelectedDetailRow != null)
        {
            GetDetailsFromControls(FPreviouslySelectedDetailRow);
        }
        // display the details of the currently selected row
        FPreviouslySelectedDetailRow = GetSelectedDetailRow();
        ShowDetails(FPreviouslySelectedDetailRow);
    }

    /// get the data from the controls and store in the currently selected detail row
    public void GetDataFromControls()
    {
        GetDetailsFromControls(FPreviouslySelectedDetailRow);
    }

    private void GetDetailsFromControls(ABatchRow ARow)
    {
        if (ARow != null)
        {
            ARow.BeginEdit();
            if (txtDetailBatchDescription.Text.Length == 0)
            {
                ARow.SetBatchDescriptionNull();
            }
            else
            {
                ARow.BatchDescription = txtDetailBatchDescription.Text;
            }
            if (txtDetailBatchControlTotal.Text.Length == 0)
            {
                ARow.SetBatchControlTotalNull();
            }
            else
            {
                ARow.BatchControlTotal = Convert.ToDouble(txtDetailBatchControlTotal.Text);
            }
            ARow.DateEffective = dtpDetailDateEffective.Value;
            ARow.EndEdit();
        }
    }

#region Implement interface functions
    /// auto generated
    public void RunOnceOnActivation()
    {
    }

    /// <summary>
    /// Adds event handlers for the appropiate onChange event to call a central procedure
    /// </summary>
    public void HookupAllControls()
    {
    }

    /// auto generated
    public void HookupAllInContainer(Control container)
    {
        FPetraUtilsObject.HookupAllInContainer(container);
    }

    /// auto generated
    public bool CanClose()
    {
        return FPetraUtilsObject.CanClose();
    }

    /// auto generated
    public TFrmPetraUtils GetPetraUtilsObject()
    {
        return (TFrmPetraUtils)FPetraUtilsObject;
    }
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actNew")
        {
            btnNew.Enabled = e.Enabled;
        }
        if (e.ActionName == "actDelete")
        {
            btnDelete.Enabled = e.Enabled;
        }
        if (e.ActionName == "actPostBatch")
        {
            btnPostBatch.Enabled = e.Enabled;
            tbbPostBatch.Enabled = e.Enabled;
            mniPost.Enabled = e.Enabled;
        }
        if (e.ActionName == "actImportFromSpreadSheet")
        {
            tbbImportFromSpreadSheet.Enabled = e.Enabled;
            mniImportFromSpreadSheet.Enabled = e.Enabled;
        }
    }

#endregion
  }
}
