//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, morayh, christophert
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
using System.Data.Odbc;
using System.Collections.Generic;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.GL.Data.Access;

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// provides methods for posting a batch
    /// </summary>
    public class TGLPosting
    {
        private const int POSTING_LOGLEVEL = 1;

        /// <summary>
        /// creates the rows for the whole current year in AGeneralLedgerMaster and AGeneralLedgerMasterPeriod for an Account/CostCentre combination
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACostCentreCode"></param>
        /// <returns>the glm sequence</returns>
        private static Int32 CreateGLMYear(
            ref GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            string AAccountCode,
            string ACostCentreCode)
        {
            ALedgerRow Ledger = AMainDS.ALedger[0];

            AGeneralLedgerMasterRow GLMRow = AMainDS.AGeneralLedgerMaster.NewRowTyped();

            // row.GlmSequence will be set by SubmitChanges
            GLMRow.GlmSequence = (AMainDS.AGeneralLedgerMaster.Rows.Count * -1) - 1;
            GLMRow.LedgerNumber = ALedgerNumber;
            GLMRow.Year = Ledger.CurrentFinancialYear;
            GLMRow.AccountCode = AAccountCode;
            GLMRow.CostCentreCode = ACostCentreCode;

            AMainDS.AGeneralLedgerMaster.Rows.Add(GLMRow);

            for (int PeriodCount = 1; PeriodCount < Ledger.NumberOfAccountingPeriods + Ledger.NumberFwdPostingPeriods + 1; PeriodCount++)
            {
                AGeneralLedgerMasterPeriodRow PeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                PeriodRow.GlmSequence = GLMRow.GlmSequence;
                PeriodRow.PeriodNumber = PeriodCount;
                AMainDS.AGeneralLedgerMasterPeriod.Rows.Add(PeriodRow);
            }

            return GLMRow.GlmSequence;
        }

        /// <summary>
        /// creates the rows for the specified year in AGeneralLedgerMaster and AGeneralLedgerMasterPeriod for an Account/CostCentre combination
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AYear"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACostCentreCode"></param>
        /// <returns> The GLM Sequence</returns>
        public static Int32 CreateGLMYear(
            ref GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            int AYear,
            string AAccountCode,
            string ACostCentreCode)
        {
            ALedgerRow Ledger = AMainDS.ALedger[0];

            AGeneralLedgerMasterRow GLMRow = AMainDS.AGeneralLedgerMaster.NewRowTyped();

            // row.GlmSequence will be set by SubmitChanges
            GLMRow.GlmSequence = (AMainDS.AGeneralLedgerMaster.Rows.Count * -1) - 1;
            GLMRow.LedgerNumber = ALedgerNumber;
            GLMRow.Year = AYear;
            GLMRow.AccountCode = AAccountCode;
            GLMRow.CostCentreCode = ACostCentreCode;

            AMainDS.AGeneralLedgerMaster.Rows.Add(GLMRow);

            for (int PeriodCount = 1; PeriodCount < Ledger.NumberOfAccountingPeriods + Ledger.NumberFwdPostingPeriods + 1; PeriodCount++)
            {
                AGeneralLedgerMasterPeriodRow PeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                PeriodRow.GlmSequence = GLMRow.GlmSequence;
                PeriodRow.PeriodNumber = PeriodCount;
                AMainDS.AGeneralLedgerMasterPeriod.Rows.Add(PeriodRow);
            }

            return GLMRow.GlmSequence;
        }

        /// <summary>
        /// load the batch and all associated tables into the typed dataset
        /// </summary>
        /// <param name="ADataSet"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        /// <returns>false if batch does not exist at all</returns>
        private static bool LoadData(out GLBatchTDS ADataSet,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();
            ADataSet = new GLBatchTDS();

            bool IsMyOwnTransaction; // If I create a transaction here, then I need to rollback when I'm done.
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                             (IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out IsMyOwnTransaction);

            if (!ABatchAccess.Exists(ALedgerNumber, ABatchNumber, Transaction))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        Catalog.GetString("The batch does not exist at all."),
                        TResultSeverity.Resv_Critical));
                DBAccess.GDBAccessObj.RollbackTransaction();
                return false;
            }

            ALedgerAccess.LoadByPrimaryKey(ADataSet, ALedgerNumber, Transaction);

            ABatchAccess.LoadByPrimaryKey(ADataSet, ALedgerNumber, ABatchNumber, Transaction);

            AJournalAccess.LoadViaABatch(ADataSet, ALedgerNumber, ABatchNumber, Transaction);

            ATransactionAccess.LoadViaABatch(ADataSet, ALedgerNumber, ABatchNumber, Transaction);

            ATransAnalAttribAccess.LoadViaABatch(ADataSet, ALedgerNumber, ABatchNumber, Transaction);

            // load all accounts of ledger, because we need them later for the account hierarchy tree for summarisation
            AAccountAccess.LoadViaALedger(ADataSet, ALedgerNumber, Transaction);

            // TODO: use cached table?
            AAccountHierarchyDetailAccess.LoadViaAAccountHierarchy(ADataSet, ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, Transaction);

            // TODO: use cached table?
            ACostCentreAccess.LoadViaALedger(ADataSet, ALedgerNumber, Transaction);

            if (IsMyOwnTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return true;
        }

        /// <summary>
        /// Load all GLM and GLMPeriod records for the batch period and the following periods, since that will avoid loading them one by one during submitchanges.
        /// this is called after ValidateBatchAndTransactions, because the BatchYear and BatchPeriod are validated and recalculated there
        /// </summary>
        /// <param name="ADataSet"></param>
        /// <param name="ALedgerNumber"></param>
        private static void LoadGLMData(ref GLBatchTDS ADataSet, Int32 ALedgerNumber)
        {
            bool IsMyOwnTransaction; // If I create a transaction here, then I need to rollback when I'm done.
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                             (IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out IsMyOwnTransaction);

            AGeneralLedgerMasterRow GLMTemplateRow = ADataSet.AGeneralLedgerMaster.NewRowTyped(false);

            GLMTemplateRow.LedgerNumber = ALedgerNumber;
            GLMTemplateRow.Year = ADataSet.ABatch[0].BatchYear;
            AGeneralLedgerMasterAccess.LoadUsingTemplate(ADataSet, GLMTemplateRow, Transaction);

            string query = "SELECT PUB_a_general_ledger_master_period.* " +
                           "FROM PUB_a_general_ledger_master, PUB_a_general_ledger_master_period " +
                           "WHERE PUB_a_general_ledger_master.a_ledger_number_i = ? " +
                           "AND PUB_a_general_ledger_master.a_year_i = ? " +
                           "AND PUB_a_general_ledger_master_period.a_glm_sequence_i = PUB_a_general_ledger_master.a_glm_sequence_i " +
                           "AND PUB_a_general_ledger_master_period.a_period_number_i >= ?";

            List <OdbcParameter>parameters = new List <OdbcParameter>();

            OdbcParameter parameter = new OdbcParameter("ledgernumber", OdbcType.Int);
            parameter.Value = ALedgerNumber;
            parameters.Add(parameter);
            parameter = new OdbcParameter("year", OdbcType.Int);
            parameter.Value = ADataSet.ABatch[0].BatchYear;
            parameters.Add(parameter);
            parameter = new OdbcParameter("period", OdbcType.Int);
            parameter.Value = ADataSet.ABatch[0].BatchPeriod;
            parameters.Add(parameter);
            DBAccess.GDBAccessObj.Select(ADataSet,
                query,
                ADataSet.AGeneralLedgerMasterPeriod.TableName, Transaction, parameters.ToArray());

            if (IsMyOwnTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }

        /// <summary>
        /// runs validations on batch, journals and transactions
        /// some things are even modified, eg. batch period etc from date effective
        /// </summary>
        /// <param name="ADataSet"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        private static bool ValidateBatchAndTransactions(ref GLBatchTDS ADataSet,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();
            ABatchRow Batch = ADataSet.ABatch[0];

            if ((Batch.BatchStatus == MFinanceConstants.BATCH_CANCELLED) || (Batch.BatchStatus == MFinanceConstants.BATCH_POSTED))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It has status {0}"), Batch.BatchStatus),
                        TResultSeverity.Resv_Critical));
            }

            // Calculate the base currency amounts for each transaction, using the exchange rate from the journals.
            // erm - this is done already? I don't want to do it here, since my journal may contain forex-reval elements.

            // Calculate the credit and debit totals
            GLRoutines.UpdateTotalsOfBatch(ref ADataSet, Batch);

            if (Convert.ToDecimal(Batch.BatchCreditTotal) != Convert.ToDecimal(Batch.BatchDebitTotal))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It does not balance: Debit is {0:n2}, Credit is {1:n2}"), Batch.BatchDebitTotal,
                            Batch.BatchCreditTotal),
                        TResultSeverity.Resv_Critical));
            }
            else if (Batch.BatchCreditTotal == 0)
            {
//                AVerifications.Add(new TVerificationResult(
//                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
//                        Catalog.GetString("It has no monetary value. Please cancel it or add meaningful transactions."),
//                        TResultSeverity.Resv_Critical));
            }
            else if ((Batch.BatchControlTotal != 0) && (Convert.ToDecimal(Batch.BatchControlTotal) != Convert.ToDecimal(Batch.BatchCreditTotal)))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("The control total {0:n2} does not fit the Credit/Debit Total {1:n2}."),
                            Batch.BatchControlTotal,
                            Batch.BatchCreditTotal),
                        TResultSeverity.Resv_Critical));
            }

            Int32 DateEffectivePeriodNumber, DateEffectiveYearNumber;
            bool IsMyOwnTransaction; // If I create a transaction here, then I need to rollback when I'm done.
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                             (IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out IsMyOwnTransaction);

            if (!TFinancialYear.IsValidPostingPeriod(Batch.LedgerNumber, Batch.DateEffective, out DateEffectivePeriodNumber,
                    out DateEffectiveYearNumber,
                    Transaction))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("The Date Effective {0:d-MMM-yyyy} does not fit any open accounting period."),
                            Batch.DateEffective),
                        TResultSeverity.Resv_Critical));
            }
            else
            {
                // just make sure that the correct BatchPeriod is used
                Batch.BatchPeriod = DateEffectivePeriodNumber;
                Batch.BatchYear = DateEffectiveYearNumber;
            }

            if (IsMyOwnTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            DataView TransactionsOfJournalView = new DataView(ADataSet.ATransaction);

            foreach (AJournalRow journal in ADataSet.AJournal.Rows)
            {
                journal.DateEffective = Batch.DateEffective;
                journal.JournalPeriod = Batch.BatchPeriod;

                if (Convert.ToDecimal(journal.JournalCreditTotal) != Convert.ToDecimal(journal.JournalDebitTotal))
                {
                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                            String.Format(Catalog.GetString("The journal {0} does not balance: Debit is {1:n2}, Credit is {2:n2}"),
                                journal.JournalNumber,
                                journal.JournalDebitTotal, journal.JournalCreditTotal),
                            TResultSeverity.Resv_Critical));
                }

                TransactionsOfJournalView.RowFilter = ATransactionTable.GetJournalNumberDBName() + " = " + journal.JournalNumber.ToString();

                foreach (DataRowView TransactionViewRow in TransactionsOfJournalView)
                {
                    ATransactionRow transaction = (ATransactionRow)TransactionViewRow.Row;

                    // check that transactions on foreign currency accounts are using the correct currency
                    // (fx reval transactions are an exception because they are posted in base currency)
                    if (!((transaction.Reference == CommonAccountingTransactionTypesEnum.REVAL.ToString())
                          && (journal.TransactionTypeCode == CommonAccountingTransactionTypesEnum.REVAL.ToString())))
                    {
                        // get the account that this transaction is writing to
                        AAccountRow Account = (AAccountRow)ADataSet.AAccount.Rows.Find(new object[] { ALedgerNumber, transaction.AccountCode });

                        if (Account == null)
                        {
                            // should not get here
                            throw new Exception("ValidateBatchAndTransactions: Cannot find account " + transaction.AccountCode);
                        }

                        if (Account.ForeignCurrencyFlag && (journal.TransactionCurrency != Account.ForeignCurrencyCode))
                        {
                            AVerifications.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                    String.Format(Catalog.GetString(
                                            "Transaction {0} in Journal {1} with currency {2} does not fit the foreign currency {3} of account {4}."),
                                        transaction.TransactionNumber, transaction.JournalNumber, journal.TransactionCurrency,
                                        Account.ForeignCurrencyCode,
                                        transaction.AccountCode),
                                    TResultSeverity.Resv_Critical));
                        }
                    }

                    if ((transaction.AmountInBaseCurrency == 0) && (transaction.TransactionAmount != 0))
                    {
                        AVerifications.Add(new TVerificationResult(
                                String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                String.Format(Catalog.GetString("Transaction {0} in Journal {1} has invalid base transaction amount of 0."),
                                    transaction.TransactionNumber, transaction.JournalNumber),
                                TResultSeverity.Resv_Critical));
                    }
                }
            }

            return !AVerifications.HasCriticalError();
        }

        /// <summary>
        /// validate the attributes of the transactions
        /// some things are even modified, eg. batch period etc from date effective
        /// </summary>
        /// <param name="ADataSet"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        private static bool ValidateAnalysisAttributes(ref GLBatchTDS ADataSet,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();
            // fetch all the analysis tables

            GLSetupTDS analysisDS = new GLSetupTDS();
            AAnalysisTypeAccess.LoadAll(analysisDS, null);
            AFreeformAnalysisAccess.LoadViaALedger(analysisDS, ALedgerNumber, null);
            AAnalysisAttributeAccess.LoadViaALedger(analysisDS, ALedgerNumber, null);

            DataView TransactionsOfJournalView = new DataView(ADataSet.ATransaction);

            foreach (AJournalRow journal in ADataSet.AJournal.Rows)
            {
                if (journal.BatchNumber.Equals(ABatchNumber))
                {
                    TransactionsOfJournalView.RowFilter = ATransactionTable.GetJournalNumberDBName() + " = " + journal.JournalNumber.ToString();

                    foreach (DataRowView transRowView in TransactionsOfJournalView)
                    {
                        ATransactionRow trans = (ATransactionRow)transRowView.Row;
                        // 1. check that all atransanalattrib records are there for all analattributes entries
                        DataView ANView = analysisDS.AAnalysisAttribute.DefaultView;
                        ANView.RowFilter = String.Format("{0} = '{1}' AND {2} = true",
                            AAnalysisAttributeTable.GetAccountCodeDBName(),
                            trans.AccountCode, AAnalysisAttributeTable.GetActiveDBName());
                        int i = 0;

                        while (i < ANView.Count)
                        {
                            AAnalysisAttributeRow attributeRow = (AAnalysisAttributeRow)ANView[i].Row;


                            ATransAnalAttribRow aTransAttribRow =
                                (ATransAnalAttribRow)ADataSet.ATransAnalAttrib.Rows.Find(new object[] { ALedgerNumber, ABatchNumber,
                                                                                                        trans.JournalNumber,
                                                                                                        trans.TransactionNumber,
                                                                                                        attributeRow.AnalysisTypeCode });

                            if (aTransAttribRow == null)
                            {
                                AVerifications.Add(new TVerificationResult(
                                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                        String.Format(Catalog.GetString(
                                                "Missing attributes record for journal #{0} transaction #{1}  and TypeCode {2}"),
                                            trans.JournalNumber,
                                            trans.TransactionNumber, attributeRow.AnalysisTypeCode),
                                        TResultSeverity.Resv_Critical));
                            }
                            else
                            {
                                String v = aTransAttribRow.AnalysisAttributeValue;

                                if ((v == null) || (v.Length == 0))
                                {
                                    AVerifications.Add(new TVerificationResult(
                                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                            String.Format(Catalog.GetString("Missing values at journal #{0} transaction #{1}  and TypeCode {2}"),
                                                trans.JournalNumber, trans.TransactionNumber, attributeRow.AnalysisTypeCode),
                                            TResultSeverity.Resv_Critical));
                                }
                                else
                                {
                                    AFreeformAnalysisRow afaRow = (AFreeformAnalysisRow)analysisDS.AFreeformAnalysis.Rows.Find(
                                        new Object[] { ALedgerNumber, attributeRow.AnalysisTypeCode, v });

                                    if (afaRow == null)
                                    {
                                        // this would cause a constraint error and is only possible in a development/sqlite environment
                                        AVerifications.Add(new TVerificationResult(
                                                String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                                String.Format(Catalog.GetString("Invalid values at journal #{0} transaction #{1}  and TypeCode {2}"),
                                                    trans.JournalNumber, trans.TransactionNumber, attributeRow.AnalysisTypeCode),
                                                TResultSeverity.Resv_Critical));
                                    }
                                    else
                                    {
                                        if (!afaRow.Active)
                                        {
                                            AVerifications.Add(new TVerificationResult(
                                                    String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber,
                                                        ALedgerNumber),
                                                    String.Format(Catalog.GetString(
                                                            "Value {0} not active at journal #{1} transaction #{2}  and TypeCode {3}"), v,
                                                        trans.JournalNumber, trans.TransactionNumber, attributeRow.AnalysisTypeCode),
                                                    TResultSeverity.Resv_Critical));
                                        }
                                    }
                                }
                            }

                            i++;
                        }
                    }
                }
            }

            return !AVerifications.HasCriticalError();
        }

        /// Helper class for storing the amounts of a batch at posting level for account/costcentre combinations
        private class TAmount
        {
            /// amount in the base currency of the ledger
            public decimal baseAmount = 0.0M;

            /// amount in transaction currency; only for foreign currency accounts
            public decimal transAmount = 0.0M;

            /// generate a key for the account/costcentre combination
            public static string MakeKey(string AccountCode, string CostCentreCode)
            {
                return AccountCode + ":" + CostCentreCode;
            }

            /// get the account code from the key
            public static string GetAccountCode(string key)
            {
                return key.Split(':')[0];
            }

            /// get the cost centre code from the key
            public static string GetCostCentreCode(string key)
            {
                return key.Split(':')[1];
            }
        }

        /// Helper class for managing the account hierarchy for posting the batch
        private class TAccountTreeElement
        {
            /// Constructor
            public TAccountTreeElement(bool AInvert, bool AForeign)
            {
                Invert = AInvert;
                Foreign = AForeign;
            }

            /// is the debit credit indicator different of the reporting account to the parent account
            public bool Invert = false;

            /// is this account a foreign currency account
            public bool Foreign = false;

            /// generate a key for the reporting account/parent account combination
            public static string MakeKey(string ReportingAccountCode, string AccountCodeReportTo)
            {
                return ReportingAccountCode + ":" + AccountCodeReportTo;
            }

            /// get the reporting account code from the key
            public static string GetReportingAccountCode(string key)
            {
                return key.Split(':')[0];
            }

            /// get the parent account code from the key
            public static string GetAccountReportToCode(string key)
            {
                return key.Split(':')[1];
            }
        }

        /// <summary>
        /// mark each journal, each transaction as being posted;
        /// add sums for costcentre/account combinations
        /// </summary>
        /// <param name="MainDS">only contains the batch to post, and its journals and transactions</param>
        /// <returns>a list with the sums for each costcentre/account combination</returns>
        private static SortedList <string, TAmount>MarkAsPostedAndCollectData(ref GLBatchTDS MainDS)
        {
            SortedList <string, TAmount>PostingLevel = new SortedList <string, TAmount>();

            DataView myView = new DataView(MainDS.ATransaction);
            myView.Sort = ATransactionTable.GetJournalNumberDBName();

            foreach (AJournalRow journal in MainDS.AJournal.Rows)
            {
                foreach (DataRowView transactionview in myView.FindRows(journal.JournalNumber))
                {
                    ATransactionRow transaction = (ATransactionRow)transactionview.Row;
                    transaction.TransactionStatus = true;

                    // get the account that this transaction is writing to
                    AAccountRow Account = (AAccountRow)MainDS.AAccount.Rows.Find(new object[] { transaction.LedgerNumber, transaction.AccountCode });

                    // Set the sign of the amounts according to the debit/credit indicator
                    decimal SignBaseAmount = transaction.AmountInBaseCurrency;
                    decimal SignTransAmount = transaction.TransactionAmount;

                    if (Account.DebitCreditIndicator != transaction.DebitCreditIndicator)
                    {
                        SignBaseAmount *= -1.0M;
                        SignTransAmount *= -1.0M;
                    }

                    // TODO: do we need to check for base currency corrections?
                    // or do we get rid of these problems by not having international currency?

                    string key = TAmount.MakeKey(transaction.AccountCode, transaction.CostCentreCode);

                    if (!PostingLevel.ContainsKey(key))
                    {
                        PostingLevel.Add(key, new TAmount());
                    }

                    PostingLevel[key].baseAmount += SignBaseAmount;

                    // Only foreign currency accounts store a value in the transaction currency
                    if (Account.ForeignCurrencyFlag)
                    {
                        PostingLevel[key].transAmount += SignTransAmount;
                    }
                }

                journal.JournalStatus = MFinanceConstants.BATCH_POSTED;
            }

            MainDS.ABatch[0].BatchStatus = MFinanceConstants.BATCH_POSTED;

            return PostingLevel;
        }

        /// <summary>
        /// Calculate the summarization trees for each posting account and each
        /// posting cost centre. The result of the union of these trees,
        /// excluding the base posting/posting combination, is the set of
        /// accounts that receive the summary data.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APostingLevel"></param>
        /// <param name="AAccountTree"></param>
        /// <param name="ACostCentreTree"></param>
        /// <param name="AMainDS"></param>
        /// <returns></returns>
        private static bool CalculateTrees(
            Int32 ALedgerNumber,
            ref SortedList <string, TAmount>APostingLevel,
            out SortedList <string, TAccountTreeElement>AAccountTree,
            out SortedList <string, string>ACostCentreTree,
            GLBatchTDS AMainDS)
        {
            // get all accounts that each posting level account is directly or indirectly posting to
            AAccountTree = new SortedList <string, TAccountTreeElement>();

            foreach (string PostingLevelKey in APostingLevel.Keys)
            {
                string AccountCode = TAmount.GetAccountCode(PostingLevelKey);

                // only once for each account, even though there might be several entries for one account in APostingLevel because of different costcentres
                if (AAccountTree.ContainsKey(TAccountTreeElement.MakeKey(AccountCode, AccountCode)))
                {
                    continue;
                }

                AAccountRow Account = (AAccountRow)AMainDS.AAccount.Rows.Find(new object[] { ALedgerNumber, AccountCode });
                bool DebitCreditIndicator = Account.DebitCreditIndicator;
                AAccountTree.Add(TAccountTreeElement.MakeKey(AccountCode, AccountCode),
                    new TAccountTreeElement(false, Account.ForeignCurrencyFlag));

                AAccountHierarchyDetailRow HierarchyDetail =
                    (AAccountHierarchyDetailRow)AMainDS.AAccountHierarchyDetail.Rows.Find(
                        new object[] { ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, AccountCode });

                while (HierarchyDetail != null)
                {
                    Account = (AAccountRow)AMainDS.AAccount.Rows.Find(new object[] { ALedgerNumber, HierarchyDetail.AccountCodeToReportTo });

                    if (Account == null)
                    {
                        // current account is BAL SHT, and it reports nowhere (account with name = ledgernumber does not exist)
                        break;
                    }

                    AAccountTree.Add(TAccountTreeElement.MakeKey(AccountCode, HierarchyDetail.AccountCodeToReportTo),
                        new TAccountTreeElement(DebitCreditIndicator != Account.DebitCreditIndicator, Account.ForeignCurrencyFlag));

                    HierarchyDetail = (AAccountHierarchyDetailRow)AMainDS.AAccountHierarchyDetail.Rows.Find(
                        new object[] { ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, HierarchyDetail.AccountCodeToReportTo });
                }
            }

            ACostCentreTree = new SortedList <string, string>();

            foreach (string PostingLevelKey in APostingLevel.Keys)
            {
                string CostCentreCode = TAmount.GetCostCentreCode(PostingLevelKey);

                // only once for each cost centre
                if (ACostCentreTree.ContainsKey(CostCentreCode + ":" + CostCentreCode))
                {
                    continue;
                }

                ACostCentreTree.Add(CostCentreCode + ":" + CostCentreCode,
                    CostCentreCode + ":" + CostCentreCode);

                ACostCentreRow CostCentre = (ACostCentreRow)AMainDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, CostCentreCode });

                while (!CostCentre.IsCostCentreToReportToNull())
                {
                    ACostCentreTree.Add(CostCentreCode + ":" + CostCentre.CostCentreToReportTo,
                        CostCentreCode + ":" + CostCentre.CostCentreToReportTo);

                    CostCentre = (ACostCentreRow)AMainDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, CostCentre.CostCentreToReportTo });
                }
            }

            return true;
        }

        /// <summary>
        /// for each posting level, propagate the value upwards through both the account and the cost centre hierarchy in glm master;
        /// also propagate the value from the posting period through the following periods;
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="APostingLevel"></param>
        /// <param name="AAccountTree"></param>
        /// <param name="ACostCentreTree"></param>
        /// <returns></returns>
        private static bool SummarizeData(
            ref GLBatchTDS AMainDS,
            ref SortedList <string, TAmount>APostingLevel,
            ref SortedList <string, TAccountTreeElement>AAccountTree,
            ref SortedList <string, string>ACostCentreTree)
        {
            Int32 FromPeriod = AMainDS.ABatch[0].BatchPeriod;

            if (AMainDS.ALedger[0].ProvisionalYearEndFlag)
            {
                // If the year end close is running, then we are posting the year end
                // reallocations.  These appear as part of the final period, but
                // should only be written to the forward periods.
                // In year end, a_current_period_i = a_number_of_accounting_periods_i = a_batch_period_i.
                FromPeriod++;
            }

            DataView GLMMasterView = AMainDS.AGeneralLedgerMaster.DefaultView;
            GLMMasterView.Sort = AGeneralLedgerMasterTable.GetAccountCodeDBName() + "," + AGeneralLedgerMasterTable.GetCostCentreCodeDBName();
            DataView GLMPeriodView = AMainDS.AGeneralLedgerMasterPeriod.DefaultView;
            GLMPeriodView.Sort = AGeneralLedgerMasterPeriodTable.GetGlmSequenceDBName() + "," + AGeneralLedgerMasterPeriodTable.GetPeriodNumberDBName();

            // Loop through the posting data collected earlier.  Summarize it to a
            // temporary table, which is much faster than finding and updating records
            // in the glm tables multiple times.  WriteData will write it to the real
            // tables in a single pass.
            foreach (string PostingLevelKey in APostingLevel.Keys)
            {
                string AccountCode = TAmount.GetAccountCode(PostingLevelKey);
                string CostCentreCode = TAmount.GetCostCentreCode(PostingLevelKey);

                TAmount PostingLevelElement = APostingLevel[PostingLevelKey];

                // Combine the summarization trees for both the account and the cost centre.
                foreach (string AccountTreeKey in AAccountTree.Keys)
                {
                    if (TAccountTreeElement.GetReportingAccountCode(AccountTreeKey) == AccountCode)
                    {
                        string AccountCodeToReportTo = TAccountTreeElement.GetAccountReportToCode(AccountTreeKey);
                        TAccountTreeElement AccountTreeElement = AAccountTree[AccountTreeKey];

                        foreach (string CostCentreKey in ACostCentreTree.Keys)
                        {
                            if (CostCentreKey.StartsWith(CostCentreCode + ":"))
                            {
                                string CostCentreCodeToReportTo = CostCentreKey.Split(':')[1];
                                decimal SignBaseAmount = PostingLevelElement.baseAmount;
                                decimal SignTransAmount = PostingLevelElement.transAmount;

                                // Set the sign of the amounts according to the debit/credit indicator
                                if (AccountTreeElement.Invert)
                                {
                                    SignBaseAmount *= -1;
                                    SignTransAmount *= -1;
                                }

                                // Find the summary level, creating it if it does not already exist.
                                int GLMMasterIndex = GLMMasterView.Find(new object[] { AccountCodeToReportTo, CostCentreCodeToReportTo });
                                AGeneralLedgerMasterRow GlmRow;

                                if (GLMMasterIndex == -1)
                                {
                                    CreateGLMYear(
                                        ref AMainDS,
                                        AMainDS.ALedger[0].LedgerNumber,
                                        AccountCodeToReportTo,
                                        CostCentreCodeToReportTo);

                                    GLMMasterIndex = GLMMasterView.Find(new object[] { AccountCodeToReportTo, CostCentreCodeToReportTo });
                                }

                                GlmRow = (AGeneralLedgerMasterRow)GLMMasterView[GLMMasterIndex].Row;

                                GlmRow.YtdActualBase += SignBaseAmount;

                                if (AccountTreeElement.Foreign)
                                {
                                    if (GlmRow.IsYtdActualForeignNull())
                                    {
                                        GlmRow.YtdActualForeign = SignTransAmount;
                                    }
                                    else
                                    {
                                        GlmRow.YtdActualForeign += SignTransAmount;
                                    }
                                }

                                if (AMainDS.ALedger[0].ProvisionalYearEndFlag)
                                {
                                    GlmRow.ClosingPeriodActualBase += SignBaseAmount;
                                }

                                // Add the period data from the posting level to the summary levels
                                for (Int32 PeriodCount = FromPeriod;
                                     PeriodCount <= AMainDS.ALedger[0].NumberOfAccountingPeriods + AMainDS.ALedger[0].NumberFwdPostingPeriods;
                                     PeriodCount++)
                                {
                                    int GLMPeriodIndex = GLMPeriodView.Find(new object[] { GlmRow.GlmSequence, PeriodCount });
                                    AGeneralLedgerMasterPeriodRow GlmPeriodRow;

                                    if (GLMPeriodIndex == -1)
                                    {
                                        GlmPeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                                        GlmPeriodRow.GlmSequence = GlmRow.GlmSequence;
                                        GlmPeriodRow.PeriodNumber = PeriodCount;
                                    }
                                    else
                                    {
                                        GlmPeriodRow = (AGeneralLedgerMasterPeriodRow)GLMPeriodView[GLMPeriodIndex].Row;
                                    }

                                    GlmPeriodRow.ActualBase += SignBaseAmount;

                                    if (AccountTreeElement.Foreign)
                                    {
                                        if (GlmPeriodRow.IsActualForeignNull())
                                        {
                                            GlmPeriodRow.ActualForeign = SignTransAmount;
                                        }
                                        else
                                        {
                                            GlmPeriodRow.ActualForeign += SignTransAmount;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// on the posting level propagate the value from the posting period through the following periods;
        /// in this version of SummarizeData, there is no calculation of summary accounts/cost centres, since that can be done by the reports
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AMainDS"></param>
        /// <param name="APostingLevel"></param>
        /// <returns></returns>
        private static bool SummarizeDataSimple(
            Int32 ALedgerNumber,
            ref GLBatchTDS AMainDS,
            ref SortedList <string, TAmount>APostingLevel)
        {
            Int32 FromPeriod = AMainDS.ABatch[0].BatchPeriod;

            if (AMainDS.ALedger[0].ProvisionalYearEndFlag)
            {
                // If the year end close is running, then we are posting the year end
                // reallocations.  These appear as part of the final period, but
                // should only be written to the forward periods.
                // In year end, a_current_period_i = a_number_of_accounting_periods_i = a_batch_period_i.
                FromPeriod++;
            }

            DataView GLMMasterView = AMainDS.AGeneralLedgerMaster.DefaultView;
            GLMMasterView.Sort = AGeneralLedgerMasterTable.GetAccountCodeDBName() + "," + AGeneralLedgerMasterTable.GetCostCentreCodeDBName();
            DataView GLMPeriodView = AMainDS.AGeneralLedgerMasterPeriod.DefaultView;
            GLMPeriodView.Sort = AGeneralLedgerMasterPeriodTable.GetGlmSequenceDBName() + "," + AGeneralLedgerMasterPeriodTable.GetPeriodNumberDBName();

            // Loop through the posting data collected earlier.  Summarize it to a
            // temporary table, which is much faster than finding and updating records
            // in the glm tables multiple times.  WriteData will write it to the real
            // tables in a single pass.
            foreach (string PostingLevelKey in APostingLevel.Keys)
            {
                string AccountCode = TAmount.GetAccountCode(PostingLevelKey);
                string CostCentreCode = TAmount.GetCostCentreCode(PostingLevelKey);

                TAmount PostingLevelElement = APostingLevel[PostingLevelKey];

                // Find the posting level, creating it if it does not already exist.
                int GLMMasterIndex = GLMMasterView.Find(new object[] { AccountCode, CostCentreCode });
                AGeneralLedgerMasterRow GlmRow;

                if (GLMMasterIndex == -1)
                {
                    CreateGLMYear(
                        ref AMainDS,
                        ALedgerNumber,
                        AccountCode,
                        CostCentreCode);

                    GLMMasterIndex = GLMMasterView.Find(new object[] { AccountCode, CostCentreCode });
                }

                GlmRow = (AGeneralLedgerMasterRow)GLMMasterView[GLMMasterIndex].Row;

                GlmRow.YtdActualBase += PostingLevelElement.baseAmount;

                AAccountRow account = (AAccountRow)AMainDS.AAccount.Rows.Find(new object[] { ALedgerNumber, AccountCode });

                if (account.ForeignCurrencyFlag)
                {
                    if (GlmRow.IsYtdActualForeignNull())
                    {
                        GlmRow.YtdActualForeign = PostingLevelElement.transAmount;
                    }
                    else
                    {
                        GlmRow.YtdActualForeign += PostingLevelElement.transAmount;
                    }
                }

                if (AMainDS.ALedger[0].ProvisionalYearEndFlag)
                {
                    GlmRow.ClosingPeriodActualBase += PostingLevelElement.baseAmount;
                } // Last use of GlmRow in this routine ...

                // propagate the data through the following periods
                for (Int32 PeriodCount = FromPeriod;
                     PeriodCount <= AMainDS.ALedger[0].NumberOfAccountingPeriods + AMainDS.ALedger[0].NumberFwdPostingPeriods;
                     PeriodCount++)
                {
                    int GLMPeriodIndex = GLMPeriodView.Find(new object[] { GlmRow.GlmSequence, PeriodCount });
                    AGeneralLedgerMasterPeriodRow GlmPeriodRow;

                    if (GLMPeriodIndex == -1)
                    {
                        GlmPeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                        GlmPeriodRow.GlmSequence = GlmRow.GlmSequence;
                        GlmPeriodRow.PeriodNumber = PeriodCount;
                    }
                    else
                    {
                        GlmPeriodRow = (AGeneralLedgerMasterPeriodRow)GLMPeriodView[GLMPeriodIndex].Row;
                    }

                    GlmPeriodRow.ActualBase += PostingLevelElement.baseAmount;

                    if (account.ForeignCurrencyFlag)
                    {
                        if (GlmPeriodRow.IsActualForeignNull())
                        {
                            GlmPeriodRow.ActualForeign = PostingLevelElement.transAmount;
                        }
                        else
                        {
                            GlmPeriodRow.ActualForeign += PostingLevelElement.transAmount;
                        }
                    }
                }
            }

            GLMMasterView.Sort = "";
            GLMPeriodView.Sort = "";

            return true;
        }

        /// <summary>
        /// write all changes to the database; on failure the whole transaction is rolled back
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        private static bool SubmitChanges(GLBatchTDS AMainDS, out TVerificationResultCollection AVerifications)
        {
            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: SubmitChanges...");
            }

            GLBatchTDSAccess.SubmitChanges(AMainDS.GetChangesTyped(true), out AVerifications);

            if (AVerifications.HasCriticalError())
            {
                return false;
            }

            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: Finished...");
            }

            return true;
        }

        /// <summary>
        /// prepare posting a GL Batch, without saving to database yet.
        /// This is called by the actual PostGLBatch routine, but also by the routine for testing what would happen to the balances.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber">Batch to post</param>
        /// <param name="AVerifications"></param>
        /// <param name="AMainDS">modified dataset, but not submitted to database yet</param>
        /// <param name="ACalculatePostingTree">for testing a batch, we don't need to calculate the whole tree</param>
        /// <returns></returns>
        public static bool PostGLBatchInternal(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications,
            out GLBatchTDS AMainDS,
            bool ACalculatePostingTree)
        {
            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: LoadData...");
            }

            // get the data from the database into the MainDS
            if (!LoadData(out AMainDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                return false;
            }

            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: Validation...");
            }

            // first validate Batch, and Transactions; check credit/debit totals; check currency, etc
            if (!ValidateBatchAndTransactions(ref AMainDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                return false;
            }

            if (!ValidateAnalysisAttributes(ref AMainDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                return false;
            }

            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: Load GLM Data...");
            }

            LoadGLMData(ref AMainDS, ALedgerNumber);

            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: Mark as posted and collect data...");
            }

            // post each journal, each transaction; add sums for costcentre/account combinations
            SortedList <string, TAmount>PostingLevel = MarkAsPostedAndCollectData(ref AMainDS);

            // we need the tree, because of the cost centre tree, which is not calculated by the balance sheet and other reports.
            // for testing the balances, we don't need to calculate the whole tree
            if (ACalculatePostingTree)
            {
                if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
                {
                    TLogging.Log("Posting: CalculateTrees...");
                }

                // key is PostingAccount, the value TAccountTreeElement describes the parent account and other details of the relation
                SortedList <string, TAccountTreeElement>AccountTree;

                // key is the PostingCostCentre, the value is the parent Cost Centre
                SortedList <string, string>CostCentreTree;

                // TODO Can anything of this be done in StoredProcedures? Only SQLite here?

                // this was in Petra 2.x; takes a lot of time, which the reports could do better
                // TODO: can we just calculate the cost centre tree, since that is needed for Balance Sheet,
                // but avoid calculating the whole account tree?
                CalculateTrees(ALedgerNumber, ref PostingLevel, out AccountTree, out CostCentreTree, AMainDS);

                if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
                {
                    TLogging.Log("Posting: SummarizeData...");
                }

                SummarizeData(ref AMainDS, ref PostingLevel, ref AccountTree, ref CostCentreTree);
            }
            else
            {
                SummarizeDataSimple(ALedgerNumber, ref AMainDS, ref PostingLevel);
            }

            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: SummarizeDataSimple...");
            }

            return true;
        }

        /// <summary>
        /// post a GL Batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        public static bool PostGLBatch(Int32 ALedgerNumber, Int32 ABatchNumber, out TVerificationResultCollection AVerifications)
        {
            // TODO: get a lock on this ledger, no one else is allowed to change anything.

            GLBatchTDS MainDS;

            if (PostGLBatchInternal(ALedgerNumber, ABatchNumber, out AVerifications, out MainDS, true))
            {
                bool result = SubmitChanges(MainDS, out AVerifications);

                // TODO: release the lock

                return result;
            }

            return false;
        }

        /// <summary>
        /// cancel a GL Batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        public static bool CancelGLBatch(out GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();

            // get the data from the database into the MainDS
            if (!LoadData(out AMainDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                return false;
            }

            ABatchRow Batch = AMainDS.ABatch[0];

            if (Batch.BatchStatus == MFinanceConstants.BATCH_POSTED)
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot cancel Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It has status {0}"), Batch.BatchStatus),
                        TResultSeverity.Resv_Critical));
                return false;
            }

            if (Batch.BatchStatus == MFinanceConstants.BATCH_CANCELLED)
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot cancel Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It was already cancelled.")),
                        TResultSeverity.Resv_Critical));
                return false;
            }

            DBAccess.GDBAccessObj.RollbackTransaction();


            return true;
        }

        /// <summary>
        /// create a new batch.
        /// it is already stored to the database, to avoid problems with LastBatchNumber
        /// </summary>
        public static GLBatchTDS CreateABatch(Int32 ALedgerNumber)
        {
            bool NewTransactionStarted = false;

            GLBatchTDS MainDS = null;

            //Error handling
            string ErrorContext = "Create a Batch";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            try
            {
                MainDS = new GLBatchTDS();

                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                                 (IsolationLevel.Serializable, TEnforceIsolationLevel.eilMinimum, out NewTransactionStarted);

                ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                ABatchRow NewRow = MainDS.ABatch.NewRowTyped(true);
                NewRow.LedgerNumber = ALedgerNumber;
                MainDS.ALedger[0].LastBatchNumber++;
                NewRow.BatchNumber = MainDS.ALedger[0].LastBatchNumber;
                NewRow.BatchPeriod = MainDS.ALedger[0].CurrentPeriod;
                MainDS.ABatch.Rows.Add(NewRow);

                if (GLBatchTDSAccess.SubmitChanges(MainDS, out VerificationResult) == TSubmitChangesResult.scrOK)
                {
                    MainDS.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    String.Format(Catalog.GetString("Unknown error while creating a batch for Ledger: {0}." +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ALedgerNumber);
                ErrorType = TResultSeverity.Resv_Critical;
                VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
            }
            finally
            {
                if (NewTransactionStarted)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }

            return MainDS;
        }

        /// <summary>
        /// create a new batch.
        /// it is already stored to the database, to avoid problems with LastBatchNumber
        /// </summary>
        public static GLBatchTDS CreateABatch(
            Int32 ALedgerNumber,
            string ABatchDescription,
            decimal ABatchControlTotal,
            DateTime ADateEffective)
        {
            GLBatchTDS MainDS = CreateABatch(ALedgerNumber);
            ABatchRow NewRow = MainDS.ABatch[0];

            int FinancialYear, FinancialPeriod;

            TFinancialYear.GetLedgerDatePostingPeriod(ALedgerNumber, ref ADateEffective, out FinancialYear, out FinancialPeriod, null, false);
            NewRow.DateEffective = ADateEffective;
            NewRow.BatchPeriod = FinancialPeriod;
            NewRow.BatchYear = FinancialYear;
            NewRow.BatchDescription = ABatchDescription;
            NewRow.BatchControlTotal = ABatchControlTotal;

            return MainDS;
        }

        /// <summary>
        /// Create a new journal as per gl1120.i
        /// </summary>
        public static bool CreateAJournal(
            GLBatchTDS AMainDS,
            Int32 ALedgerNumber, Int32 ABatchNumber, Int32 ALastJournalNumber,
            string AJournalDescription, string ACurrency, decimal AXRateToBase,
            DateTime ADateEffective, Int32 APeriodNumber, out Int32 AJournalNumber)
        {
            bool CreationSuccessful = false;

            AJournalNumber = 0;

            //Error handling
            string ErrorContext = "Create a Journal";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            try
            {
                AJournalRow JournalRow = AMainDS.AJournal.NewRowTyped();
                JournalRow.LedgerNumber = ALedgerNumber;
                JournalRow.BatchNumber = ABatchNumber;
                AJournalNumber = ALastJournalNumber + 1;
                JournalRow.JournalNumber = AJournalNumber;
                JournalRow.JournalDescription = AJournalDescription;
                JournalRow.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
                JournalRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
                JournalRow.TransactionCurrency = ACurrency;
                JournalRow.ExchangeRateToBase = AXRateToBase;
                JournalRow.DateEffective = ADateEffective;
                JournalRow.JournalPeriod = APeriodNumber;
                AMainDS.AJournal.Rows.Add(JournalRow);

                //Update the Last Journal
                ABatchRow BatchRow = (ABatchRow)AMainDS.ABatch.Rows.Find(new object[] { ALedgerNumber, ABatchNumber });
                BatchRow.LastJournal = AJournalNumber;

                CreationSuccessful = true;
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    String.Format(Catalog.GetString("Unknown error while creating a batch for Ledger: {0}." +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ALedgerNumber);
                ErrorType = TResultSeverity.Resv_Critical;
                VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
            }

            return CreationSuccessful;
        }

        /// <summary>
        /// create a record for a_transaction
        /// </summary>
        public static bool CreateATransaction(
            GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            string ANarrative,
            string AAccountCode,
            string ACostCentreCode,
            decimal ATransAmount,
            DateTime ATransDate,
            bool ADebCredIndicator,
            string AReference,
            bool ASystemGenerated,
            decimal ABaseAmount,
            out int ATransactionNumber)
        {
            bool CreationSuccessful = false;

            ATransactionNumber = 0;

            //Error handling
            string ErrorContext = "Create a Transaction";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            try
            {
                AJournalRow JournalRow = (AJournalRow)AMainDS.AJournal.Rows.Find(new object[] { ALedgerNumber, ABatchNumber, AJournalNumber });

                //Increment the LastTransactionNumber
                JournalRow.LastTransactionNumber++;
                ATransactionNumber = JournalRow.LastTransactionNumber;

                ATransactionRow TransactionRow = AMainDS.ATransaction.NewRowTyped();

                TransactionRow.LedgerNumber = ALedgerNumber;
                TransactionRow.BatchNumber = ABatchNumber;
                TransactionRow.JournalNumber = AJournalNumber;
                TransactionRow.TransactionNumber = ATransactionNumber;
                TransactionRow.Narrative = ANarrative;
                TransactionRow.Reference = AReference;
                TransactionRow.AccountCode = AAccountCode;
                TransactionRow.CostCentreCode = ACostCentreCode;
                TransactionRow.DebitCreditIndicator = ADebCredIndicator;
                TransactionRow.SystemGenerated = ASystemGenerated;
                TransactionRow.AmountInBaseCurrency = ABaseAmount;
                TransactionRow.TransactionAmount = ATransAmount;
                TransactionRow.TransactionDate = ATransDate;

                AMainDS.ATransaction.Rows.Add(TransactionRow);

                CreationSuccessful = true;
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    String.Format(Catalog.GetString("Unknown error while creating a batch for Ledger: {0}." +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ALedgerNumber);
                ErrorType = TResultSeverity.Resv_Critical;
                VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
            }

            return CreationSuccessful;
        }
    }
}