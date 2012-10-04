﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash
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
using System.Collections;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// Description of GiftRevertAdjust_ManualCode.
    /// </summary>
    public partial class TFrmGiftRevertAdjust
    {
        private Int32 FLedgerNumber;
        private Hashtable requestParams = new Hashtable();
        private AGiftDetailRow giftDetailRow = null;
        private Boolean ok = false;
        DateTime StartDateCurrentPeriod;
        DateTime EndDateLastForwardingPeriod;

        /// <summary>
        /// Return if the revert/adjust action was Ok (then a refresh is needed; otherwise rollback was done)
        /// </summary>
        public bool Ok {
            get
            {
                return ok;
            }
        }
        /// <summary>
        /// A Gift Detail Row is injected
        /// </summary>
        public AGiftDetailRow GiftDetailRow {
            set
            {
                giftDetailRow = value;
                txtReversalCommentOne.Text = giftDetailRow.GiftCommentOne;
                txtReversalCommentTwo.Text = giftDetailRow.GiftCommentTwo;
                txtReversalCommentThree.Text = giftDetailRow.GiftCommentThree;
                cmbReversalCommentOneType.Text = giftDetailRow.CommentOneType;
                cmbReversalCommentTwoType.Text = giftDetailRow.CommentTwoType;
                cmbReversalCommentThreeType.Text = giftDetailRow.CommentThreeType;
            }
        }
        /// <summary>
        /// Ledger Number is injected
        /// </summary>
        public int LedgerNumber {
            set
            {
                DialogResult = System.Windows.Forms.DialogResult.Abort;
                FLedgerNumber = value;
                requestParams.Add("ALedgerNumber", FLedgerNumber);

                DateTime DefaultDate;
                TLedgerSelection.GetCurrentPostingRangeDates(FLedgerNumber,
                    out StartDateCurrentPeriod,
                    out EndDateLastForwardingPeriod,
                    out DefaultDate);
                lblValidDateRange.Text = String.Format(Catalog.GetString("Valid between {0} and {1}"),
                    StartDateCurrentPeriod.ToShortDateString(), EndDateLastForwardingPeriod.ToShortDateString());
            }
        }

        /// <summary>
        /// Some params for the server function are injected
        /// </summary>
        public void AddParam(String paramName, Object param)
        {
            requestParams.Remove(paramName);
            requestParams.Add(paramName, param);
        }

        private void RevertAdjust(System.Object sender, System.EventArgs e)
        {
            if (chkSelect.Checked && (FPreviouslySelectedDetailRow == null))
            {
                // nothing seleted
                MessageBox.Show(Catalog.GetString("Please select a Batch!."));
                return;
            }

            if ((giftDetailRow != null) && giftDetailRow.ModifiedDetail)
            {
                MessageBox.Show(Catalog.GetString("A Gift can only be reverted once!"));
                return;
            }

            Boolean ok;
            TVerificationResultCollection AMessages;


            AddParam("NewBatchSelected", chkSelect.Checked);

            if (chkSelect.Checked)
            {
                AddParam("NewBatchNumber", FPreviouslySelectedDetailRow.BatchNumber);
            }
            else
            {
                //check the gift batch date to use
                if (dtpEffectiveDate.Date < StartDateCurrentPeriod)
                {
                    dtpEffectiveDate.Date = StartDateCurrentPeriod;
                    MessageBox.Show(Catalog.GetString("Your Date was outside the allowed posting period."));
                    return;
                }

                if (dtpEffectiveDate.Date > EndDateLastForwardingPeriod)
                {
                    dtpEffectiveDate.Date = EndDateLastForwardingPeriod;
                    MessageBox.Show(Catalog.GetString("Your Date was outside the allowed posting period."));
                    return;
                }

                AddParam("GlEffectiveDate", dtpEffectiveDate.Date);
            }

            AddParam("BatchNumber", giftDetailRow.BatchNumber);
            AddParam("GiftNumber", giftDetailRow.GiftTransactionNumber);
            AddParam("GiftDetailNumber", giftDetailRow.DetailNumber);
            AddParam("ReversalCommentOne", txtReversalCommentOne.Text);
            AddParam("ReversalCommentTwo", txtReversalCommentTwo.Text);
            AddParam("ReversalCommentThree", txtReversalCommentThree.Text);
            AddParam("ReversalCommentOneType", cmbReversalCommentOneType.Text);
            AddParam("ReversalCommentTwoType", cmbReversalCommentTwoType.Text);
            AddParam("ReversalCommentThreeType", cmbReversalCommentThreeType.Text);


            ok = TRemote.MFinance.Gift.WebConnectors.GiftRevertAdjust(requestParams, out AMessages);

            if (ok)
            {
                String function = (String)requestParams["Function"];

                switch (function)
                {
                    case "ReverseGiftBatch":
                        MessageBox.Show(Catalog.GetString("Your batch has been sucessfully reverted"));
                        break;

                    case "ReverseGiftDetail":
                        MessageBox.Show(Catalog.GetString("Your gift detail has been sucessfully reverted"));
                        break;

                    case "ReverseGift":
                        MessageBox.Show(Catalog.GetString("Your gift has been sucessfully reverted"));
                        break;

                    case "AdjustGift":
                        MessageBox.Show(Catalog.GetString("Your gift has been sucessfully adjusted"));
                        break;
                }

                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                ShowMessages(AMessages);
                DialogResult = System.Windows.Forms.DialogResult.Abort;
                Close();
            }

            return;
        }

        private void InitializeManualCode()
        {
        	//FLedger is still zero at this point
        	FMainDS.AGiftBatch.DefaultView.RowFilter = String.Format("{0} = '{1}'",
        	                                                         AGiftBatchTable.GetBatchStatusDBName(), 
        	                                                         MFinanceConstants.BATCH_UNPOSTED
        	                                                        );
            FMainDS.AGiftBatch.DefaultView.Sort = AGiftBatchTable.GetBatchNumberDBName() + " DESC";
            
            SelectBatchChanged(null, null);
        }

        private void SelectBatchChanged(System.Object sender, EventArgs e)
        {
            grdDetails.Enabled = chkSelect.Checked;
            grdDetails.Visible = chkSelect.Checked;
            dtpEffectiveDate.Visible = !chkSelect.Checked;
            lblEffectiveDate.Visible = !chkSelect.Checked;
            lblValidDateRange.Visible = !chkSelect.Checked;
            
			//First pass FLedgerNumber = 0 so need to add Ledger to the filter when the user first checks the checkbox
			if (chkSelect.Checked && FLedgerNumber != 0 && !FMainDS.AGiftBatch.DefaultView.RowFilter.Contains(AGiftBatchTable.GetLedgerNumberDBName()))
			{
				FMainDS.AGiftBatch.DefaultView.RowFilter = String.Format("{0} = {1} AND {2} = '{3}'",
                                                         AGiftBatchTable.GetLedgerNumberDBName(),
                                                         FLedgerNumber,
                                                         AGiftBatchTable.GetBatchStatusDBName(), 
                                                         MFinanceConstants.BATCH_UNPOSTED
                                                        );
			}
            
        }

        private void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnHelpClick(object sender, EventArgs e)
        {
            // TODO
        }

        private void ShowMessages(TVerificationResultCollection AMessages)
        {
            string ErrorMessages = String.Empty;

            if (AMessages.Count > 0)
            {
                foreach (TVerificationResult message in AMessages)
                {
                    ErrorMessages += "[" + message.ResultContext + "] " + message.ResultTextCaption + ": " + message.ResultText + Environment.NewLine;
                }
            }

            if (ErrorMessages.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}