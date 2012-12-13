//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Validation;


namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLTransactions
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private Int32 FJournalNumber = -1;
        private string FTransactionCurrency = string.Empty;
        private string FBatchStatus = string.Empty;
        private string FJournalStatus = string.Empty;

        private ABatchRow FBatchRow = null;

        /// <summary>
        /// load the transactions into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="AForeignCurrencyName"></param>
        /// <param name="ABatchStatus"></param>
        /// <param name="AJournalStatus"></param>
        public void LoadTransactions(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            string AForeignCurrencyName,
            string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED,
            string AJournalStatus = MFinanceConstants.BATCH_UNPOSTED)
        {
            FBatchRow = GetBatchRow();

            //Check if the same batch is selected, so no need to apply filter
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber) && (FJournalNumber == AJournalNumber)
                && (FTransactionCurrency == AForeignCurrencyName) && (FBatchStatus == ABatchStatus) && (FJournalStatus == AJournalStatus)
                && (FMainDS.ATransaction.DefaultView.Count > 0))
            {
                //Same as previously selected
                if ((FBatchRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED) && (grdDetails.SelectedRowIndex() > 0))
                {
                    GetDetailsFromControls(GetSelectedDetailRow());
                }

                return;
            }

            bool requireControlSetup = (FLedgerNumber == -1) || (FTransactionCurrency != AForeignCurrencyName);

            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FJournalNumber = AJournalNumber;
            FTransactionCurrency = AForeignCurrencyName;
            FBatchStatus = ABatchStatus;
            FJournalStatus = AJournalStatus;

            FPreviouslySelectedDetailRow = null;

            // only load from server if there are no transactions loaded yet for this journal
            // otherwise we would overwrite transactions that have already been modified
            FMainDS.ATransaction.DefaultView.RowFilter = string.Empty;

            if (FMainDS.ATransaction.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionWithAttributes(ALedgerNumber, ABatchNumber, AJournalNumber));
            }
            else
            {
                FMainDS.ATransaction.DefaultView.Sort = String.Format("{0} ASC, {1} ASC, {2} ASC",
                    ATransactionTable.GetLedgerNumberDBName(),
                    ATransactionTable.GetBatchNumberDBName(),
                    ATransactionTable.GetJournalNumberDBName()
                    );

                if (FMainDS.ATransaction.DefaultView.Find(new object[] { FLedgerNumber, FBatchNumber, FJournalNumber }) == -1)
                {
                    FMainDS.ATransaction.Clear();
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionWithAttributes(ALedgerNumber, ABatchNumber, AJournalNumber));
                }
            }

            FMainDS.ATransaction.DefaultView.RowFilter = String.Format("{0}={1} and {2}={3}",
                ATransactionTable.GetBatchNumberDBName(),
                FBatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                FJournalNumber);

            FMainDS.ATransaction.DefaultView.Sort = String.Format("{0} ASC",
                ATransactionTable.GetTransactionNumberDBName()
                );

            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ATransaction.DefaultView);

            // if this form is readonly, then we need all account and cost centre codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;

            if (requireControlSetup)
            {
                TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                    true, false, ActiveOnly, false, AForeignCurrencyName);
                TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, ActiveOnly, false);
            }

            ShowData();
            ShowDetails();

            btnNew.Enabled = !FPetraUtilsObject.DetailProtectedMode && FJournalStatus == MFinanceConstants.BATCH_UNPOSTED;
            btnRemove.Enabled = !FPetraUtilsObject.DetailProtectedMode && FJournalStatus == MFinanceConstants.BATCH_UNPOSTED;

            //This will update Batch and journal totals
            UpdateTotals();

            if (grdDetails.Rows.Count < 2)
            {
                ClearControls();
                pnlDetails.Enabled = false;
            }
        }

        /// <summary>
        /// Unload the currently loaded transactions
        /// </summary>
        public void UnloadTransactions()
        {
            if (FMainDS.ATransaction.DefaultView.Count > 0)
            {
                FPreviouslySelectedDetailRow = null;
                FMainDS.ATransaction.Clear();
                //ClearControls();
            }
        }

        /// <summary>
        /// Update the effective date from outside
        /// </summary>
        /// <param name="AEffectiveDate"></param>
        public void UpdateEffectiveDateForCurrentRow(DateTime AEffectiveDate)
        {
            if ((GetSelectedDetailRow() != null) && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                GetSelectedDetailRow().TransactionDate = AEffectiveDate;
                dtpDetailTransactionDate.Date = AEffectiveDate;
                GetDetailsFromControls(GetSelectedDetailRow());
            }
        }

        /// <summary>
        /// Return the active transaction number and sets the Journal number
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        public Int32 ActiveTransactionNumber(Int32 ALedgerNumber, Int32 ABatchNumber, ref Int32 AJournalNumber)
        {
            Int32 activeTrans = 0;

            if (FPreviouslySelectedDetailRow != null)
            {
                activeTrans = FPreviouslySelectedDetailRow.TransactionNumber;
                AJournalNumber = FPreviouslySelectedDetailRow.JournalNumber;
            }

            return activeTrans;
        }

//            Int32 ledgerNumber;
//            Int32 batchNumber;
//            Int32 journalNumber;
//            DateTime batchEffectiveDate;
//
//            ledgerNumber = AGLBatchRow.LedgerNumber;
//            batchNumber = AGLBatchRow.BatchNumber;
//            journalNumber = AGLJournalRow.JournalNumber;
//            batchEffectiveDate = AGLBatchRow.DateEffective;
//
//            if (FMainDS.ATransaction.Rows.Count == 0)
//            {
//                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionWithAttributes(ledgerNumber, batchNumber, journalNumber));
//            }
//            else if ((FLedgerNumber == ledgerNumber) || (FBatchNumber == batchNumber && FJournalNumber == journalNumber))
//            {
//                FGLEffectivePeriodChanged = true;
//                //Rows already active in transaction tab. Need to set current row ac code below will not update selected row
//                GetSelectedDetailRow().TransactionDate = batchEffectiveDate;
//            }
//
//            //Update all transactions
//            foreach (ATransactionRow transRow in FMainDS.ATransaction.Rows)
//            {
//              if (transRow.BatchNumber.Equals(batchNumber) && transRow.JournalNumber.Equals(journalNumber) && transRow.LedgerNumber.Equals(ledgerNumber))
//                {
//                    transRow.TransactionDate = batchEffectiveDate;
//                }
//            }
//
//            if (FGLEffectivePeriodChanged)
//            {
//                ShowDetails();
//            }

        /// <summary>
        /// get the details of the current journal
        /// </summary>
        /// <returns></returns>
        private GLBatchTDSAJournalRow GetJournalRow()
        {
            return ((TFrmGLBatch)ParentForm).GetJournalsControl().GetSelectedDetailRow();
        }

        private ABatchRow GetBatchRow()
        {
            return ((TFrmGLBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();
        }

        /// <summary>
        /// Cancel any changes made to this form
        /// </summary>
        public void CancelChangesToFixedBatches()
        {
            if ((GetBatchRow() != null) && (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                FMainDS.ATransaction.RejectChanges();
            }
        }

        /// <summary>
        /// add a new transactions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges && !((TFrmGLBatch) this.ParentForm).SaveChanges())
            {
                return;
            }

            this.CreateNewATransaction();
            ProcessAnalysisAttributes();

            if (pnlDetails.Enabled == false)
            {
                pnlDetails.Enabled = true;
            }

            ((TFrmGLBatch) this.ParentForm).EnableAttributes();

            cmbDetailCostCentreCode.Focus();
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the journal.lastTransactionNumber is updated;
        /// will use the currently selected journal
        /// </summary>
        public void NewRowManual(ref GLBatchTDSATransactionRow ANewRow)
        {
            NewRowManual(ref ANewRow, null);
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the journal.lastTransactionNumber is updated
        /// </summary>
        /// <param name="ANewRow">returns the modified new transaction row</param>
        /// <param name="ARefJournalRow">this can be null; otherwise this is the journal that the transaction should belong to</param>
        public void NewRowManual(ref GLBatchTDSATransactionRow ANewRow, AJournalRow ARefJournalRow)
        {
            //GLBatchTDSATransactionRow prevRow = GetSelectedDetailRow();

            if (ARefJournalRow == null)
            {
                ARefJournalRow = GetJournalRow();
            }

            ANewRow.LedgerNumber = ARefJournalRow.LedgerNumber;
            ANewRow.BatchNumber = ARefJournalRow.BatchNumber;
            ANewRow.JournalNumber = ARefJournalRow.JournalNumber;
            ANewRow.TransactionNumber = ARefJournalRow.LastTransactionNumber + 1;
            ANewRow.TransactionDate = GetBatchRow().DateEffective;
            ARefJournalRow.LastTransactionNumber++;

            if (FPreviouslySelectedDetailRow != null)
            {
                ANewRow.AccountCode = FPreviouslySelectedDetailRow.AccountCode;
                ANewRow.CostCentreCode = FPreviouslySelectedDetailRow.CostCentreCode;

                if (ARefJournalRow.JournalCreditTotal != ARefJournalRow.JournalDebitTotal)
                {
                    ANewRow.Reference = FPreviouslySelectedDetailRow.Reference;
                    ANewRow.Narrative = FPreviouslySelectedDetailRow.Narrative;
                    ANewRow.TransactionDate = FPreviouslySelectedDetailRow.TransactionDate;
                    decimal Difference = ARefJournalRow.JournalDebitTotal - ARefJournalRow.JournalCreditTotal;
                    ANewRow.TransactionAmount = Math.Abs(Difference);
                    ANewRow.DebitCreditIndicator = Difference < 0;
                }
            }

            //If first row added
            if (grdDetails.Rows.Count == 2)
            {
                if (cmbDetailCostCentreCode.Count > 1)
                {
                    cmbDetailCostCentreCode.SelectedIndex = 1;
                }
                else
                {
                    cmbDetailCostCentreCode.SelectedIndex = -1;
                }

                if (cmbDetailAccountCode.Count > 1)
                {
                    cmbDetailAccountCode.SelectedIndex = 1;
                }
                else
                {
                    cmbDetailAccountCode.SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// show ledger, batch and journal number
        /// </summary>
        private void ShowDataManual()
        {
            if (FLedgerNumber != -1)
            {
                txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
                txtBatchNumber.Text = FBatchNumber.ToString();
                txtJournalNumber.Text = FJournalNumber.ToString();

                string TransactionCurrency = GetJournalRow().TransactionCurrency;
                string BaseCurrency = FMainDS.ALedger[0].BaseCurrency;
                lblBaseCurrency.Text = String.Format(Catalog.GetString("{0} (Base Currency)"), BaseCurrency);
                lblTransactionCurrency.Text = String.Format(Catalog.GetString("{0} (Transaction Currency)"), TransactionCurrency);
                txtDebitAmountBase.CurrencySymbol = BaseCurrency;
                txtCreditAmountBase.CurrencySymbol = BaseCurrency;
                txtDebitAmount.CurrencySymbol = TransactionCurrency;
                txtCreditAmount.CurrencySymbol = TransactionCurrency;
                txtCreditTotalAmountBase.CurrencySymbol = BaseCurrency;
                txtDebitTotalAmountBase.CurrencySymbol = BaseCurrency;
                txtCreditTotalAmount.CurrencySymbol = TransactionCurrency;
                txtDebitTotalAmount.CurrencySymbol = TransactionCurrency;

                // foreign currency accounts only get transactions in that currency
                if (FTransactionCurrency != TransactionCurrency)
                {
                    string SelectedAccount = cmbDetailAccountCode.GetSelectedString();

                    // if this form is readonly, then we need all account and cost centre codes, because old codes might have been used
                    bool ActiveOnly = this.Enabled;

                    TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                        true, false, ActiveOnly, false, TransactionCurrency);

                    cmbDetailAccountCode.SetSelectedString(SelectedAccount);

                    if ((GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED) && FPetraUtilsObject.HasChanges)
                    {
                        FPetraUtilsObject.DisableSaveButton();
                    }

                    FTransactionCurrency = TransactionCurrency;
                }
            }

            UpdateChangeableStatus();
        }

        private void ShowDetailsManual(ATransactionRow ARow)
        {
            txtJournalNumber.Text = FJournalNumber.ToString();

            if (ARow == null)
            {
                ((TFrmGLBatch)ParentForm).DisableAttributes();
                return;
            }

            if (ARow.DebitCreditIndicator)
            {
                txtDebitAmountBase.NumberValueDecimal = ARow.AmountInBaseCurrency;
                txtCreditAmountBase.NumberValueDecimal = 0;
                txtDebitAmount.NumberValueDecimal = ARow.TransactionAmount;
                txtCreditAmount.NumberValueDecimal = 0;
            }
            else
            {
                txtDebitAmountBase.NumberValueDecimal = 0;
                txtCreditAmountBase.NumberValueDecimal = ARow.AmountInBaseCurrency;
                txtDebitAmount.NumberValueDecimal = 0;
                txtCreditAmount.NumberValueDecimal = ARow.TransactionAmount;
            }

            if (FPetraUtilsObject.HasChanges && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                UpdateTotals();
            }
            else if (FPetraUtilsObject.HasChanges && (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                FPetraUtilsObject.DisableSaveButton();
            }
        }

        private void GetDetailDataFromControlsManual(ATransactionRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            Decimal oldTransactionAmount = ARow.TransactionAmount;
            bool oldDebitCreditIndicator = ARow.DebitCreditIndicator;

            if (txtDebitAmount.Text.Length == 0)
            {
                txtDebitAmount.NumberValueDecimal = 0;
            }

            if (txtCreditAmount.Text.Length == 0)
            {
                txtCreditAmount.NumberValueDecimal = 0;
            }

            ARow.DebitCreditIndicator = (txtDebitAmount.NumberValueDecimal.Value > 0);

            if (ARow.DebitCreditIndicator)
            {
                ARow.TransactionAmount = Math.Abs(txtDebitAmount.NumberValueDecimal.Value);

                if (txtCreditAmount.NumberValueDecimal.Value != 0)
                {
                    txtCreditAmount.NumberValueDecimal = 0;
                }
            }
            else
            {
                ARow.TransactionAmount = Math.Abs(txtCreditAmount.NumberValueDecimal.Value);
            }

            if ((oldTransactionAmount != Convert.ToDecimal(ARow.TransactionAmount))
                || (oldDebitCreditIndicator != ARow.DebitCreditIndicator))
            {
                UpdateTotals();
            }
        }

        private void TransDateChanged(object sender, EventArgs e)
        {
            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            try
            {
                DateTime dateValue;

                string aDate = dtpDetailTransactionDate.Date.ToString();

                if (!DateTime.TryParse(aDate, out dateValue))
                {
                    dtpDetailTransactionDate.Date = GetBatchRow().DateEffective;
                }
            }
            catch
            {
                //Do nothing
            }
        }

        /// <summary>
        /// update amount in other currencies (optional) and recalculate all totals for current batch and journal
        /// </summary>
        public void UpdateTotals()
        {
            bool alreadyChanged;

            if ((FJournalNumber != -1))         // && !pnlDetailsProtected)
            {
                GLBatchTDSAJournalRow journal = GetJournalRow();

                GLRoutines.UpdateTotalsOfJournal(ref FMainDS, journal);

                alreadyChanged = FPetraUtilsObject.HasChanges;

                if (!alreadyChanged)
                {
                    FPetraUtilsObject.DisableDataChangedEvent();
                }

                txtCreditTotalAmount.NumberValueDecimal = journal.JournalCreditTotal;
                txtDebitTotalAmount.NumberValueDecimal = journal.JournalDebitTotal;
                txtCreditTotalAmountBase.NumberValueDecimal = journal.JournalCreditTotalBase;
                txtDebitTotalAmountBase.NumberValueDecimal = journal.JournalDebitTotalBase;

                if (FPreviouslySelectedDetailRow != null)
                {
                    if (FPreviouslySelectedDetailRow.DebitCreditIndicator)
                    {
                        txtDebitAmountBase.NumberValueDecimal = FPreviouslySelectedDetailRow.AmountInBaseCurrency;
                        txtCreditAmountBase.NumberValueDecimal = 0;
                        txtDebitAmount.NumberValueDecimal = FPreviouslySelectedDetailRow.TransactionAmount;
                        txtCreditAmount.NumberValueDecimal = 0;
                    }
                    else
                    {
                        txtDebitAmountBase.NumberValueDecimal = 0;
                        txtCreditAmountBase.NumberValueDecimal = FPreviouslySelectedDetailRow.AmountInBaseCurrency;
                        txtDebitAmount.NumberValueDecimal = 0;
                        txtCreditAmount.NumberValueDecimal = FPreviouslySelectedDetailRow.TransactionAmount;
                    }
                }

                if (!alreadyChanged)
                {
                    FPetraUtilsObject.EnableDataChangedEvent();
                }

                // refresh the currency symbols
                ShowDataManual();

                if (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
                {
                    ((TFrmGLBatch)ParentForm).GetJournalsControl().UpdateTotals(GetBatchRow());
                    ((TFrmGLBatch)ParentForm).GetBatchControl().UpdateTotals();
                }

                if (!alreadyChanged && FPetraUtilsObject.HasChanges)
                {
                    FPetraUtilsObject.DisableSaveButton();
                }
            }
        }

        /// <summary>
        /// WorkAroundInitialization
        /// </summary>
        public void WorkAroundInitialization()
        {
            txtCreditAmount.Validated += new EventHandler(ControlHasChanged);
            txtDebitAmount.Validated += new EventHandler(ControlHasChanged);
            cmbDetailCostCentreCode.Validated += new EventHandler(ControlHasChanged);
            cmbDetailAccountCode.Validated += new EventHandler(ControlHasChanged);
            cmbDetailKeyMinistryKey.Validated += new EventHandler(ControlHasChanged);
            txtDetailNarrative.Validated += new EventHandler(ControlHasChanged);
            txtDetailReference.Validated += new EventHandler(ControlHasChanged);
            dtpDetailTransactionDate.Validated += new EventHandler(ControlHasChanged);
        }

        private void ControlHasChanged(System.Object sender, EventArgs e)
        {
            //TODO: Find out why these were put here as they stop the field updates from working
            //SourceGrid.RowEventArgs egrid = new SourceGrid.RowEventArgs(-10);
            //FocusedRowChanged(sender, egrid);
        }

        /// <summary>
        /// enable or disable the buttons
        /// </summary>
        public void UpdateChangeableStatus()
        {
            Boolean changeable = !FPetraUtilsObject.DetailProtectedMode
                                 && (GetBatchRow() != null)
                                 && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
                                 && (GetJournalRow().JournalStatus == MFinanceConstants.BATCH_UNPOSTED);

            // pnlDetailsProtected must be changed first: when the enabled property of the control is changed, the focus changes, which triggers validation
            pnlDetailsProtected = !changeable;
            this.btnRemove.Enabled = changeable;
            this.btnNew.Enabled = changeable;
            pnlDetails.Enabled = changeable;
        }

        /// <summary>
        /// remove transactions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RemoveRow(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                || (MessageBox.Show(String.Format(Catalog.GetString(
                                "You have chosen to delete this transaction ({0}).\n\nDo you really want to delete it?"),
                            FPreviouslySelectedDetailRow.TransactionNumber), Catalog.GetString("Confirm Delete"),
                        MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                int rowIndex = grdDetails.SelectedRowIndex();

                ((TFrmGLBatch)ParentForm).GetAttributesControl().DeleteTransactionAttributes(FPreviouslySelectedDetailRow);
                FPreviouslySelectedDetailRow.Delete();

                FPreviouslySelectedDetailRow = null;

                FPetraUtilsObject.SetChangedFlag();

                SelectRowInGrid(rowIndex);

                if (grdDetails.Rows.Count < 2)
                {
                    ClearControls();
                    ((TFrmGLBatch)ParentForm).DisableAttributes();
                    pnlDetails.Enabled = false;
                }

                UpdateTotals();
            }
        }

        /// <summary>
        /// clear the current selection
        /// </summary>
        public void ClearCurrentSelection()
        {
            this.FPreviouslySelectedDetailRow = null;
        }

        private void ClearControls()
        {
            //Stop data change detection
            FPetraUtilsObject.DisableDataChangedEvent();

            //Clear combos
            cmbDetailAccountCode.SelectedIndex = -1;
            cmbDetailCostCentreCode.SelectedIndex = -1;
            cmbDetailKeyMinistryKey.SelectedIndex = -1;
            //Clear Textboxes
            txtDetailNarrative.Clear();
            txtDetailReference.Clear();
            //Clear Numeric Textboxes
            txtDebitAmount.NumberValueDecimal = 0;
            txtDebitAmountBase.NumberValueDecimal = 0;
            txtCreditAmount.NumberValueDecimal = 0;
            txtCreditAmountBase.NumberValueDecimal = 0;

            //Enable data change detection
            FPetraUtilsObject.EnableDataChangedEvent();
        }

        /// <summary>
        /// if the account code changes, analysis types/attributes  have to be updated
        /// </summary>
        private void AccountCodeDetailChanged(object sender, EventArgs e)
        {
            ProcessAnalysisAttributes();
        }

        private void ProcessAnalysisAttributes()
        {
            ((TFrmGLBatch)ParentForm).GetAttributesControl().CheckAnalysisAttributes(cmbDetailAccountCode.GetSelectedString());
        }

        private void ValidateDataDetailsManual(ATransactionRow ARow)
        {
            if ((ARow == null) || (GetBatchRow() == null) || (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            //Local validation
            if ((txtDebitAmount.NumberValueDecimal == 0) && (txtCreditAmount.NumberValueDecimal == 0))
            {
                TSharedFinanceValidation_GL.ValidateGLDetailManual(this, FBatchRow, ARow, txtDebitAmount, ref VerificationResultCollection,
                    FValidationControlsDict);
            }
            else
            {
                TSharedFinanceValidation_GL.ValidateGLDetailManual(this, FBatchRow, ARow, null, ref VerificationResultCollection,
                    FValidationControlsDict);
            }
        }

        /// <summary>
        /// Set focus to the gid controltab
        /// </summary>
        public void FocusGrid()
        {
            if ((grdDetails != null) && grdDetails.Enabled && grdDetails.TabStop)
            {
                grdDetails.Focus();
            }
        }
    }
}