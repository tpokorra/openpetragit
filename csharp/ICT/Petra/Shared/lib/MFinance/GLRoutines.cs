//
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
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Shared.MFinance
{
    /// <summary>
    /// useful routines that are used on both server and client
    /// </summary>
    public class GLRoutines
    {
        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the current journal
        /// NOTE this no longer calculates AmountInBaseCurrency
        /// </summary>
        /// <param name="AMainDS">ATransactions are filtered on current journal</param>
        /// <param name="ACurrentJournal"></param>
        public static void UpdateTotalsOfJournal(ref GLBatchTDS AMainDS,
            GLBatchTDSAJournalRow ACurrentJournal)
        {
            if (ACurrentJournal == null)
            {
                return;
            }

            if (ACurrentJournal.ExchangeRateToBase == 0.0m)
            {
                ACurrentJournal.ExchangeRateToBase = 1.0m;
//              throw new Exception(String.Format("Batch {0} Journal {1} has invalid exchange rate to base",
//                                                ACurrentJournal.BatchNumber,
//                                                ACurrentJournal.JournalNumber));
            }

            ACurrentJournal.JournalDebitTotal = 0.0M;
            ACurrentJournal.JournalDebitTotalBase = 0.0M;
            ACurrentJournal.JournalCreditTotal = 0.0M;
            ACurrentJournal.JournalCreditTotalBase = 0.0M;

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView v in AMainDS.ATransaction.DefaultView)
            {
                ATransactionRow r = (ATransactionRow)v.Row;

                // recalculate the amount in base currency
/*
                // I don't want to do this here -
                // I have "forex reval" transactions that deliberately have different amounts in Base,
                // to revalue the foreign transactions.
                //      CSharp\ICT\Petra\Server\lib\MFinance\AP\AP.EditTransaction.cs 
                //      CreateGLBatchAndTransactionsForPaying (line 1111)
                //                                                                   Tim Ingham March 2012
 
                // BUT - if this is not calculated here, it needs to be calculated by the caller prior to calling this.
 
                r.AmountInBaseCurrency = r.TransactionAmount / ACurrentJournal.ExchangeRateToBase;
*/
                if (r.DebitCreditIndicator)
                {
                    ACurrentJournal.JournalDebitTotal += r.TransactionAmount;
                    ACurrentJournal.JournalDebitTotalBase += r.AmountInBaseCurrency;
                }
                else
                {
                    ACurrentJournal.JournalCreditTotal += r.TransactionAmount;
                    ACurrentJournal.JournalCreditTotalBase += r.AmountInBaseCurrency;
                }
            }
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatch"></param>
        public static void UpdateTotalsOfBatch(ref GLBatchTDS AMainDS,
            ABatchRow ACurrentBatch)
        {
            string origTransactionFilter = AMainDS.ATransaction.DefaultView.RowFilter;
            string origJournalFilter = AMainDS.AJournal.DefaultView.RowFilter;

            ACurrentBatch.BatchDebitTotal = 0.0m;
            ACurrentBatch.BatchCreditTotal = 0.0m;

            AMainDS.AJournal.DefaultView.RowFilter =
                AJournalTable.GetBatchNumberDBName() + " = " + ACurrentBatch.BatchNumber.ToString();

            foreach (DataRowView journalview in AMainDS.AJournal.DefaultView)
            {
                GLBatchTDSAJournalRow journalrow = (GLBatchTDSAJournalRow)journalview.Row;

                AMainDS.ATransaction.DefaultView.RowFilter =
                    ATransactionTable.GetBatchNumberDBName() + " = " + journalrow.BatchNumber.ToString() + " and " +
                    ATransactionTable.GetJournalNumberDBName() + " = " + journalrow.JournalNumber.ToString();

                UpdateTotalsOfJournal(ref AMainDS, journalrow);

                ACurrentBatch.BatchDebitTotal += journalrow.JournalDebitTotal;
                ACurrentBatch.BatchCreditTotal += journalrow.JournalCreditTotal;
            }

            AMainDS.ATransaction.DefaultView.RowFilter = origTransactionFilter;
            AMainDS.AJournal.DefaultView.RowFilter = origJournalFilter;
        }
    }
}