//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Reflection;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmGLSettings
    {
        private Int32 FLedgerNumber;
        private Boolean FCurrencyChangeAllowed;
        private Boolean FWarnings = false;
        private Int32 FCurrentForwardPostingPeriods;

        /// <summary>
        /// Maintain settings for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                FMainDS.Clear();
                FMainDS.Merge(TRemote.MFinance.Setup.WebConnectors.LoadLedgerSettings(FLedgerNumber, out FCurrencyChangeAllowed));
                FCurrentForwardPostingPeriods = ((ALedgerRow)(FMainDS.ALedger.Rows[0])).NumberFwdPostingPeriods;
                ShowData(FMainDS);
            }
        }

        private void InitializeManualCode()
        {
            // Panels in group boxes add unwanted extra height -> reduce height manually

            int DefaultHeight = nudActualsDataRetention.Height;
            int HeightDifference = pnlSuspenseAccounts.Height - DefaultHeight;

            // correct heights of controls
            pnlSuspenseAccounts.Height = DefaultHeight;
            pnlBudget.Height = DefaultHeight;
            pnlFwdPosting.Height = DefaultHeight;
            pnlBranchProcess.Height = DefaultHeight;

            grpMiscellaneousFlags.Height -= (int)(3.5 * HeightDifference);

            // move controls into the right place after correcting heights
            chkSuspenseAccountFlag.Location =
                new System.Drawing.Point(chkSuspenseAccountFlag.Location.X, chkSuspenseAccountFlag.Location.Y - (int)(0.5 * HeightDifference));
            pnlBudget.Location = new System.Drawing.Point(pnlBudget.Location.X, pnlBudget.Location.Y - (int)(1 * HeightDifference));
            chkBudgetControlFlag.Location =
                new System.Drawing.Point(chkBudgetControlFlag.Location.X, chkBudgetControlFlag.Location.Y - (int)(0.5 * HeightDifference));
            pnlFwdPosting.Location = new System.Drawing.Point(pnlFwdPosting.Location.X, pnlFwdPosting.Location.Y - (int)(2 * HeightDifference));
            chkUseDefaultFwdPostingPeriods.Location = new System.Drawing.Point(chkUseDefaultFwdPostingPeriods.Location.X,
                chkUseDefaultFwdPostingPeriods.Location.Y - (int)(0.5 * HeightDifference));
            lblNumberFwdPostingPeriods.Location = new System.Drawing.Point(lblNumberFwdPostingPeriods.Location.X,
                lblNumberFwdPostingPeriods.Location.Y - (int)(0.5 * HeightDifference));
            nudNumberFwdPostingPeriods.Location = new System.Drawing.Point(nudNumberFwdPostingPeriods.Location.X,
                nudNumberFwdPostingPeriods.Location.Y - (int)(0.5 * HeightDifference));
            pnlBranchProcess.Location =
                new System.Drawing.Point(pnlBranchProcess.Location.X, pnlBranchProcess.Location.Y - (int)(3 * HeightDifference));
            chkBranchProcessing.Location =
                new System.Drawing.Point(chkBranchProcessing.Location.X, chkBranchProcessing.Location.Y - (int)(0.5 * HeightDifference));

            grpDataRetention.Location =
                new System.Drawing.Point(grpDataRetention.Location.X, grpDataRetention.Location.Y - (int)(3.5 * HeightDifference));

            if (!FCurrencyChangeAllowed)
            {
                cmbBaseCurrency.Enabled = false;
                cmbIntlCurrency.Enabled = false;
            }
        }

        /// <summary>
        /// Show ledger settings data from given data set
        /// </summary>
        /// <param name="ADataSet">The data set for which details will be shown</param>
        private void ShowData(GLSetupTDS ADataSet)
        {
            ALedgerRow LedgerRow;
            AAccountingSystemParameterRow ParameterRow;

            if ((ADataSet == null)
                || (ADataSet.ALedger.Count == 0))
            {
                return;
            }

            FPetraUtilsObject.DisableDataChangedEvent();

            // set limits to number boxes
            if (ADataSet.AAccountingSystemParameter.Count > 0)
            {
                ParameterRow = (AAccountingSystemParameterRow)ADataSet.AAccountingSystemParameter.Rows[0];

                nudNumberFwdPostingPeriods.Maximum = MFinanceConstants.MAX_PERIODS - ((ALedgerRow)ADataSet.ALedger.Rows[0]).NumberOfAccountingPeriods;
                nudActualsDataRetention.Maximum = ParameterRow.ActualsDataRetention;
                nudActualsDataRetention.Minimum = 1;
                nudBudgetDataRetention.Maximum = ParameterRow.BudgetDataRetention;
                nudBudgetDataRetention.Minimum = 1;
                nudGiftDataRetention.Maximum = ParameterRow.GiftDataRetention;
                nudGiftDataRetention.Minimum = 1;
            }

            LedgerRow = (ALedgerRow)ADataSet.ALedger.Rows[0];

            if (LedgerRow != null)
            {
                cmbBaseCurrency.SetSelectedString(LedgerRow.BaseCurrency, -1);

                cmbIntlCurrency.SetSelectedString(LedgerRow.IntlCurrency, -1);

                chkSuspenseAccountFlag.Checked = LedgerRow.SuspenseAccountFlag;

                if (LedgerRow.IsBudgetControlFlagNull())
                {
                    chkBudgetControlFlag.Checked = false;
                }
                else
                {
                    chkBudgetControlFlag.Checked = LedgerRow.BudgetControlFlag;
                }

                if (LedgerRow.IsNumberFwdPostingPeriodsNull())
                {
                    nudNumberFwdPostingPeriods.Value = 0;
                }
                else
                {
                    if (LedgerRow.NumberFwdPostingPeriods > nudNumberFwdPostingPeriods.Maximum)
                    {
                        nudNumberFwdPostingPeriods.Value = nudNumberFwdPostingPeriods.Maximum;
                    }
                    else
                    {
                        nudNumberFwdPostingPeriods.Value = LedgerRow.NumberFwdPostingPeriods;
                    }
                }

                if (LedgerRow.IsBranchProcessingNull())
                {
                    chkBranchProcessing.Checked = false;
                }
                else
                {
                    chkBranchProcessing.Checked = LedgerRow.BranchProcessing;
                }

                if (LedgerRow.IsActualsDataRetentionNull())
                {
                    nudActualsDataRetention.Value = 1;
                }
                else
                {
                    if (LedgerRow.ActualsDataRetention > nudActualsDataRetention.Maximum)
                    {
                        nudActualsDataRetention.Value = nudActualsDataRetention.Maximum;
                    }
                    else
                    {
                        nudActualsDataRetention.Value = LedgerRow.ActualsDataRetention;
                    }
                }

                if (LedgerRow.IsBudgetDataRetentionNull())
                {
                    nudBudgetDataRetention.Value = 1;
                }
                else
                {
                    if (LedgerRow.BudgetDataRetention > nudBudgetDataRetention.Maximum)
                    {
                        nudBudgetDataRetention.Value = nudBudgetDataRetention.Maximum;
                    }
                    else
                    {
                        nudBudgetDataRetention.Value = LedgerRow.BudgetDataRetention;
                    }
                }

                if (LedgerRow.IsGiftDataRetentionNull())
                {
                    nudGiftDataRetention.Value = 1;
                }
                else
                {
                    if (LedgerRow.GiftDataRetention > nudGiftDataRetention.Maximum)
                    {
                        nudGiftDataRetention.Value = nudGiftDataRetention.Maximum;
                    }
                    else
                    {
                        nudGiftDataRetention.Value = LedgerRow.GiftDataRetention;
                    }
                }
            }

            FPetraUtilsObject.EnableDataChangedEvent();
        }

        private TSubmitChangesResult StoreManualCode(ref GLSetupTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            if (FWarnings)
            {
                if (MessageBox.Show(Catalog.GetString("Do you really want to save despite the earlier warnings?"),
                        Catalog.GetString("Save Ledger Settings"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    AVerificationResult = new TVerificationResultCollection();
                    return TSubmitChangesResult.scrError;
                }
            }

            // save ledger settings now
            // (a_ledger_init_flag records are automatically added/removed on server side)
            return TRemote.MFinance.Setup.WebConnectors.SaveLedgerSettings(FLedgerNumber, ref ASubmitChanges, out AVerificationResult);
        }

        private void ValidateDataManual(ALedgerRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;

            // check that there no suspense accounts for this ledger if box is unticked
            ValidationColumn = ARow.Table.Columns[ALedgerTable.ColumnSuspenseAccountFlagId];

            if (FPetraUtilsObject.ValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.SuspenseAccountFlag)
                {
                    if (!TRemote.MFinance.Common.ServerLookups.WebConnectors.HasSuspenseAccounts(FLedgerNumber))
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(this,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_NO_SUSPENSE_ACCOUNTS_ALLOWED)),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                    else
                    {
                        VerificationResult = null;
                    }

                    // Handle addition/removal to/from TVerificationResultCollection
                    VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn);
                }
            }

            // check that the number of forwarding periods is not less than the already used ones
            ValidationColumn = ARow.Table.Columns[ALedgerTable.ColumnNumberFwdPostingPeriodsId];

            if (FPetraUtilsObject.ValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (ARow.NumberFwdPostingPeriods <= FCurrentForwardPostingPeriods)
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(this,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_NUMBER_FWD_PERIODS_TOO_SMALL,
                                new string[] { FCurrentForwardPostingPeriods.ToString(), FCurrentForwardPostingPeriods.ToString() })),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }
                else
                {
                    VerificationResult = null;
                }

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn);
            }
        }

        private void UseDefaultFwdPostingPeriodsChanged(Object sender, EventArgs e)
        {
            nudNumberFwdPostingPeriods.Enabled = !chkUseDefaultFwdPostingPeriods.Checked;
            lblNumberFwdPostingPeriods.Enabled = !chkUseDefaultFwdPostingPeriods.Checked;

            if (chkUseDefaultFwdPostingPeriods.Checked)
            {
                if ((FMainDS.AAccountingSystemParameter != null)
                    && (FMainDS.AAccountingSystemParameter.Count > 0))
                {
                    nudNumberFwdPostingPeriods.Value =
                        ((AAccountingSystemParameterRow)FMainDS.AAccountingSystemParameter.Rows[0]).NumberFwdPostingPeriods;
                }
                else
                {
                    nudNumberFwdPostingPeriods.Value = 2;
                }
            }
        }
    }
}