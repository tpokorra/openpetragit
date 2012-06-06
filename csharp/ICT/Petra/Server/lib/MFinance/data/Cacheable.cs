// auto generated with nant generateORM
// Do not modify this file manually!
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using Ict.Common.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Server.App.Core;

#region ManualCode
using System.Collections.Generic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.DataAggregates;
using Ict.Petra.Shared.MFinance.Account.Validation;
using Ict.Petra.Shared.MFinance.Gift.Validation;
using Ict.Petra.Shared.MFinance.AP.Validation;
#endregion ManualCode
namespace Ict.Petra.Server.MFinance.Cacheable
{
    /// <summary>
    /// Returns cacheable DataTables for DB tables in the MFinance sub-namespace
    /// that can be cached on the Client side.
    ///
    /// Examples of such tables are tables that form entries of ComboBoxes or Lists
    /// and which would be retrieved numerous times from the Server as UI windows
    /// are opened.
    /// </summary>
    public partial class TCacheable : TCacheableTablesLoader
    {
        /// time when this object was instantiated
        private DateTime FStartTime;

        /// <summary>
        /// constructor
        /// </summary>
        public TCacheable() : base()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }
#endif
            FStartTime = DateTime.Now;
            FCacheableTablesManager = TCacheableTablesManager.GCacheableTablesManager;
        }

#if DEBUGMODE
        /// destructor
        ~TCacheable()
        {
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }
        }
#endif

#region ManualCode
        /// <summary>
        /// Returns a certain cachable DataTable that contains all columns and all
        /// rows of a specified table.
        ///
        /// @comment Wrapper for other GetCacheableTable method
        /// </summary>
        ///
        /// <param name="ACacheableTable">Tells what cacheable DataTable should be returned.</param>
        /// <returns>DataTable</returns>
        public DataTable GetCacheableTable(TCacheableFinanceTablesEnum ACacheableTable)
        {
            System.Type TmpType;
            return GetCacheableTable(ACacheableTable, "", false, out TmpType);
        }
#endregion ManualCode
        /// <summary>
        /// Returns a certain cachable DataTable that contains all columns and all
        /// rows of a specified table.
        ///
        /// @comment Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
        /// once its contents got retrieved from the DB. It returns the cached
        /// DataTable from it on subsequent calls, therefore making more no further DB
        /// queries!
        ///
        /// @comment All DataTables are retrieved as Typed DataTables, but are passed
        /// out as a normal DataTable. However, this DataTable can be cast by the
        /// caller to the appropriate TypedDataTable to have access to the features of
        /// a Typed DataTable!
        /// </summary>
        ///
        /// <param name="ACacheableTable">Tells what cacheable DataTable should be returned.</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable that the caller has. '' can
        /// be specified to always get a DataTable back (see @return)</param>
        /// <param name="ARefreshFromDB">Set to true to reload the cached DataTable from the
        /// DB and through that refresh the Table in the Cache with what is now in the
        /// DB (this would be done when it is known that the DB Table has changed).
        /// The CacheableTablesManager will notify other Clients that they need to
        /// retrieve this Cacheable DataTable anew from the PetraServer the next time
        /// the Client accesses the Cacheable DataTable. Otherwise set to false.</param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <returns>
        /// DataTable If the Hash that got passed in AHashCode doesn't fit the
        /// Hash that the CacheableTablesManager has for this cacheable DataTable, the
        /// specified DataTable is returned, otherwise nil.
        /// </returns>
        public DataTable GetCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            String TableName = Enum.GetName(typeof(TCacheableFinanceTablesEnum), ACacheableTable);

#if DEBUGMODE
            if (TLogging.DL >= 7)
            {
                Console.WriteLine(this.GetType().FullName + ".GetCacheableTable called for table '" + TableName + "'.");
            }
#endif

            if ((ARefreshFromDB) || ((!FCacheableTablesManager.IsTableCached(TableName))))
            {
                Boolean NewTransaction;
                TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    Ict.Petra.Server.MCommon.MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                try
                {

                    switch(ACacheableTable)
                    {
                        case TCacheableFinanceTablesEnum.AnalysisTypeList:
                        {
                            DataTable TmpTable = AAnalysisTypeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.FreeformAnalysisList:
                        {
                            DataTable TmpTable = AFreeformAnalysisAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.AnalysisAttributeList:
                        {
                            DataTable TmpTable = AAnalysisAttributeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.BudgetTypeList:
                        {
                            DataTable TmpTable = ABudgetTypeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.CostCentreTypesList:
                        {
                            DataTable TmpTable = ACostCentreTypesAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.EmailDestinationList:
                        {
                            DataTable TmpTable = AEmailDestinationAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.MethodOfGivingList:
                        {
                            DataTable TmpTable = AMethodOfGivingAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.MethodOfPaymentList:
                        {
                            DataTable TmpTable = AMethodOfPaymentAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.LedgerNameList:
                        {
                            DataTable TmpTable = GetLedgerNameListTable(ReadTransaction, TableName);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }

                        default:
                            // Unknown Standard Cacheable DataTable
                            throw new ECachedDataTableNotImplementedException("Requested Cacheable DataTable '" +
                                TableName + "' is not available as a Standard Cacheable Table");
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TLogging.DL >= 7)
                        {
                            Console.WriteLine(this.GetType().FullName + ".GetCacheableTable: commited own transaction.");
                        }
#endif
                    }
                }
            }

            // Return the DataTable from the Cache only if the Hash is not the same
            return ResultingCachedDataTable(TableName, AHashCode, out AType);
        }

        /// <summary>
        /// Returns a certain cachable DataTable that contains all columns and rows
        /// of a specified table that match a specified Ledger Number.
        ///
        /// @comment Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
        /// once its contents got retrieved from the DB. It returns the cached
        /// DataTable from it on subsequent calls, therefore making more no further DB
        /// queries!
        ///
        /// @comment All DataTables are retrieved as Typed DataTables, but are passed
        /// out as a normal DataTable. However, this DataTable can be cast by the
        /// caller to the appropriate TypedDataTable to have access to the features of
        /// a Typed DataTable!
        ///
        /// </summary>
        /// <param name="ACacheableTable">Tells what cachable DataTable should be returned.</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable that the caller has. '' can be
        /// specified to always get a DataTable back (see @return)</param>
        /// <param name="ARefreshFromDB">Set to true to reload the cached DataTable from the
        /// DB and through that refresh the Table in the Cache with what is now in the
        /// DB (this would be done when it is known that the DB Table has changed).
        /// The CacheableTablesManager will notify other Clients that they need to
        /// retrieve this Cacheable DataTable anew from the PetraServer the next time
        /// the Client accesses the Cacheable DataTable. Otherwise set to false.</param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <param name="ALedgerNumber">The LedgerNumber that the rows that should be stored in
        /// the Cache need to match.</param>
        /// <returns>)
        /// DataTable If the Hash that got passed in AHashCode doesn't fit the
        /// Hash that the CacheableTablesManager has for this cacheable DataTable, the
        /// specified DataTable is returned, otherwise nil.
        /// </returns>
        public DataTable GetCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            System.Int32 ALedgerNumber,
            out System.Type AType)
        {
            string TableName = Enum.GetName(typeof(TCacheableFinanceTablesEnum), ACacheableTable);
        #if DEBUGMODE
            if (TLogging.DL >= 7)
            {
                Console.WriteLine(
                    this.GetType().FullName + ".GetCacheableTable called with ATableName='" + TableName + "' and ALedgerNumber=" +
                    ALedgerNumber.ToString() + '.');
            }
        #endif
        #if DEBUGMODE
            if (FCacheableTablesManager.IsTableCached(TableName))
            {
                if (TLogging.DL >= 7)
                {
                    Console.WriteLine("Cached DataTable has currently " +
                        FCacheableTablesManager.GetCachedDataTable(TableName, out AType).Rows.Count.ToString() + " rows in total.");
                    Console.WriteLine("Cached DataTable has currently " +
                        Convert.ToString(FCacheableTablesManager.GetCachedDataTable(TableName,
                                out AType).Select(
                                ALedgerTable.GetLedgerNumberDBName() + " = " +
                                ALedgerNumber.ToString()).Length) + " rows with ALedgerNumber=" + ALedgerNumber.ToString() + '.');
                }
            }
        #endif

            if ((ARefreshFromDB) || ((!FCacheableTablesManager.IsTableCached(TableName)))
                || ((FCacheableTablesManager.IsTableCached(TableName))
                    && (FCacheableTablesManager.GetCachedDataTable(TableName,
                            out AType).Select(ALedgerTable.GetLedgerNumberDBName() + " = " +
                            ALedgerNumber.ToString()).Length == 0)))
            {
                Boolean NewTransaction;
                TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                try
                {
                    switch (ACacheableTable)
                    {
                        case TCacheableFinanceTablesEnum.MotivationGroupList:
                        {
                            DataTable TmpTable = AMotivationGroupAccess.LoadViaALedger(ALedgerNumber, ReadTransaction);
                            FCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpTable, DomainManager.GClientID, (object)ALedgerNumber);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.MotivationList:
                        {
                            DataTable TmpTable = AMotivationDetailAccess.LoadViaALedger(ALedgerNumber, ReadTransaction);
                            FCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpTable, DomainManager.GClientID, (object)ALedgerNumber);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.FeesPayableList:
                        {
                            DataTable TmpTable = AFeesPayableAccess.LoadViaALedger(ALedgerNumber, ReadTransaction);
                            FCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpTable, DomainManager.GClientID, (object)ALedgerNumber);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.FeesReceivableList:
                        {
                            DataTable TmpTable = AFeesReceivableAccess.LoadViaALedger(ALedgerNumber, ReadTransaction);
                            FCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpTable, DomainManager.GClientID, (object)ALedgerNumber);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.AccountingPeriodList:
                        {
                            DataTable TmpTable = GetAccountingPeriodListTable(ReadTransaction, ALedgerNumber, TableName);
                            FCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpTable, DomainManager.GClientID, (object)ALedgerNumber);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.LedgerDetails:
                        {
                            DataTable TmpTable = GetLedgerDetailsTable(ReadTransaction, ALedgerNumber, TableName);
                            FCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpTable, DomainManager.GClientID, (object)ALedgerNumber);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.CostCentreList:
                        {
                            DataTable TmpTable = GetCostCentreListTable(ReadTransaction, ALedgerNumber, TableName);
                            FCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpTable, DomainManager.GClientID, (object)ALedgerNumber);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.AccountList:
                        {
                            DataTable TmpTable = GetAccountListTable(ReadTransaction, ALedgerNumber, TableName);
                            FCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpTable, DomainManager.GClientID, (object)ALedgerNumber);
                            break;
                        }
                        case TCacheableFinanceTablesEnum.AccountHierarchyList:
                        {
                            DataTable TmpTable = GetAccountHierarchyListTable(ReadTransaction, ALedgerNumber, TableName);
                            FCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpTable, DomainManager.GClientID, (object)ALedgerNumber);
                            break;
                        }

                        default:

                            // Unknown Standard Cacheable DataTable
                            throw new ECachedDataTableNotImplementedException("Requested Cacheable DataTable '" +
                            Enum.GetName(typeof(TCacheableFinanceTablesEnum),
                                ACacheableTable) + "' is not available as a Standard Cacheable Table (with ALedgerNumber as an Argument)");
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
        #if DEBUGMODE
                        if (TLogging.DL >= 7)
                        {
                            Console.WriteLine(this.GetType().FullName + ".GetCacheableTable: commited own transaction.");
                        }
        #endif
                    }
                }
            }

            DataView TmpView = new DataView(FCacheableTablesManager.GetCachedDataTable(TableName,
                    out AType), ALedgerTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString(), "", DataViewRowState.CurrentRows);

            // Return the DataTable from the Cache only if the Hash is not the same
            return ResultingCachedDataTable(TableName, AHashCode, TmpView, out AType);
        }

        /// <summary>
        /// Saves a specific Cachable DataTable. The whole DataTable needs to be submitted,
        /// not just changes to it!
        /// </summary>
        /// <remarks>
        /// Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
        /// once its saved successfully to the DB, which in turn tells all other Clients
        /// that they need to reload this Cacheable DataTable the next time something in the
        /// Client accesses it.
        /// </remarks>
        /// <param name="ACacheableTable">Name of the Cacheable DataTable with changes.</param>
        /// <param name="ASubmitTable">Cacheable DataTable with changes. The whole DataTable needs
        /// to be submitted, not just changes to it!</param>
        /// <param name="AVerificationResult">Will be filled with any
        /// VerificationResults if errors occur.</param>
        /// <returns>Status of the operation.</returns>
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
            ref TTypedDataTable ASubmitTable,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TVerificationResultCollection SingleVerificationResultCollection;
            TValidationControlsDict ValidationControlsDict = new TValidationControlsDict();
            string CacheableDTName = Enum.GetName(typeof(TCacheableFinanceTablesEnum), ACacheableTable);

            // Console.WriteLine("Entering Finance.SaveChangedStandardCacheableTable...");
            AVerificationResult = null;

            // TODO: check write permissions

            if (ASubmitTable != null)
            {
                AVerificationResult = new TVerificationResultCollection();
                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    switch (ACacheableTable)
                    {
                        case TCacheableFinanceTablesEnum.AnalysisTypeList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                AAnalysisTypeValidation.Validate(ASubmitTable, ref AVerificationResult, ValidationControlsDict);
                                ValidateAnalysisTypeListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (AAnalysisTypeAccess.SubmitChanges((AAnalysisTypeTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableFinanceTablesEnum.FreeformAnalysisList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                AFreeformAnalysisValidation.Validate(ASubmitTable, ref AVerificationResult, ValidationControlsDict);
                                ValidateFreeformAnalysisListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (AFreeformAnalysisAccess.SubmitChanges((AFreeformAnalysisTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableFinanceTablesEnum.AnalysisAttributeList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                AAnalysisAttributeValidation.Validate(ASubmitTable, ref AVerificationResult, ValidationControlsDict);
                                ValidateAnalysisAttributeListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (AAnalysisAttributeAccess.SubmitChanges((AAnalysisAttributeTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableFinanceTablesEnum.BudgetTypeList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                ABudgetTypeValidation.Validate(ASubmitTable, ref AVerificationResult, ValidationControlsDict);
                                ValidateBudgetTypeListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (ABudgetTypeAccess.SubmitChanges((ABudgetTypeTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableFinanceTablesEnum.CostCentreTypesList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                ACostCentreTypesValidation.Validate(ASubmitTable, ref AVerificationResult, ValidationControlsDict);
                                ValidateCostCentreTypesListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (ACostCentreTypesAccess.SubmitChanges((ACostCentreTypesTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableFinanceTablesEnum.EmailDestinationList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                AEmailDestinationValidation.Validate(ASubmitTable, ref AVerificationResult, ValidationControlsDict);
                                ValidateEmailDestinationListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (AEmailDestinationAccess.SubmitChanges((AEmailDestinationTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableFinanceTablesEnum.MethodOfGivingList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                AMethodOfGivingValidation.Validate(ASubmitTable, ref AVerificationResult, ValidationControlsDict);
                                ValidateMethodOfGivingListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (AMethodOfGivingAccess.SubmitChanges((AMethodOfGivingTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableFinanceTablesEnum.MethodOfPaymentList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                AMethodOfPaymentValidation.Validate(ASubmitTable, ref AVerificationResult, ValidationControlsDict);
                                ValidateMethodOfPaymentListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (AMethodOfPaymentAccess.SubmitChanges((AMethodOfPaymentTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;

                        default:

                            throw new Exception(
                            "TCacheable.SaveChangedStandardCacheableTable: unsupported Cacheabled DataTable '" + CacheableDTName + "'");
                    }

                    if (SubmissionResult == TSubmitChangesResult.scrOK)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
                catch (Exception e)
                {
                    TLogging.Log(
                        "TCacheable.SaveChangedStandardCacheableTable: after SubmitChanges call for Cacheabled DataTable '" +
                        CacheableDTName +
                        "':  Exception " + e.ToString());

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw new Exception(e.ToString() + " " + e.Message);
                }
            }

            // If saving of the DataTable was successful, update the Cacheable DataTable in the Servers'
            // Cache and inform all other Clients that they need to reload this Cacheable DataTable
            // the next time something in the Client accesses it.
            if (SubmissionResult == TSubmitChangesResult.scrOK)
            {
                Type TmpType;
                GetCacheableTable(ACacheableTable, String.Empty, true, out TmpType);
            }

            if (AVerificationResult.Count > 0)
            {
                // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
                // Serialisation (needed for .NET Remoting).
                TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
            }

            return SubmissionResult;
        }

#region Data Validation

        partial void ValidateAnalysisTypeListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateFreeformAnalysisListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateAnalysisAttributeListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateBudgetTypeListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateCostCentreTypesListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateEmailDestinationListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateMethodOfGivingListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateMethodOfPaymentListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

#endregion Data Validation

        /// <summary>
        /// Saves a specific Cachable DataTable. The whole DataTable needs to be submitted,
        /// not just changes to it!
        /// </summary>
        /// <remarks>
        /// Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
        /// once its saved successfully to the DB, which in turn tells all other Clients
        /// that they need to reload this Cacheable DataTable the next time something in the
        /// Client accesses it.
        /// </remarks>
        /// <param name="ACacheableTable">Name of the Cacheable DataTable with changes.</param>
        /// <param name="ASubmitTable">Cacheable DataTable with changes. The whole DataTable needs
        /// to be submitted, not just changes to it!</param>
        /// <param name="ALedgerNumber">The LedgerNumber that the rows that should be stored in
        /// the Cache need to match.</param>
        /// <param name="AVerificationResult">Will be filled with any
        /// VerificationResults if errors occur.</param>
        /// <returns>Status of the operation.</returns>
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
            ref TTypedDataTable ASubmitTable,
            int ALedgerNumber,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TVerificationResultCollection SingleVerificationResultCollection;
            TValidationControlsDict ValidationControlsDict = new TValidationControlsDict();
            string CacheableDTName = Enum.GetName(typeof(TCacheableFinanceTablesEnum), ACacheableTable);
            Type TmpType;

            // Console.WriteLine("Entering Finance.SaveChangedStandardCacheableTable...");
            AVerificationResult = null;

            // TODO: check write permissions

            if (ASubmitTable != null)
            {
                AVerificationResult = new TVerificationResultCollection();
                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    switch (ACacheableTable)
                    {
                        case TCacheableFinanceTablesEnum.MotivationGroupList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                AMotivationGroupValidation.Validate(ASubmitTable, ref AVerificationResult, ValidationControlsDict);
                                ValidateMotivationGroupListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (AMotivationGroupAccess.SubmitChanges((AMotivationGroupTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableFinanceTablesEnum.MotivationList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                AMotivationDetailValidation.Validate(ASubmitTable, ref AVerificationResult, ValidationControlsDict);
                                ValidateMotivationListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (AMotivationDetailAccess.SubmitChanges((AMotivationDetailTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableFinanceTablesEnum.FeesPayableList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                AFeesPayableValidation.Validate(ASubmitTable, ref AVerificationResult, ValidationControlsDict);
                                ValidateFeesPayableListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (AFeesPayableAccess.SubmitChanges((AFeesPayableTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableFinanceTablesEnum.FeesReceivableList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                AFeesReceivableValidation.Validate(ASubmitTable, ref AVerificationResult, ValidationControlsDict);
                                ValidateFeesReceivableListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (AFeesReceivableAccess.SubmitChanges((AFeesReceivableTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;

                        default:

                            throw new Exception(
                            "TFinanceCacheable.SaveChangedStandardCacheableTable: unsupported Cacheabled DataTable '" + CacheableDTName + "'");
                    }

                    if (SubmissionResult == TSubmitChangesResult.scrOK)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
                catch (Exception e)
                {
                    TLogging.Log(
                        "TFinanceCacheable.SaveChangedStandardCacheableTable: after SubmitChanges call for Cacheabled DataTable '" +
                        CacheableDTName +
                        "':  Exception " + e.ToString());

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw new Exception(e.ToString() + " " + e.Message);
                }
            }

            // If saving of the DataTable was successful, update the Cacheable DataTable in the Servers'
            // Cache and inform all other Clients that they need to reload this Cacheable DataTable
            // the next time something in the Client accesses it.
            if (SubmissionResult == TSubmitChangesResult.scrOK)
            {
                //FCacheableTablesManager.AddOrRefreshCachedTable(ATableName, ASubmitTable, DomainManager.GClientID);
                GetCacheableTable(ACacheableTable, String.Empty, true, ALedgerNumber, out TmpType);
            }

            if (AVerificationResult.Count > 0)
            {
                // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
                // Serialisation (needed for .NET Remoting).
                TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
            }

            return SubmissionResult;
        }

        #region Data Validation

                partial void ValidateMotivationGroupListManual(TValidationControlsDict ValidationControlsDict,
                    ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
                partial void ValidateMotivationListManual(TValidationControlsDict ValidationControlsDict,
                    ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
                partial void ValidateFeesPayableListManual(TValidationControlsDict ValidationControlsDict,
                    ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
                partial void ValidateFeesReceivableListManual(TValidationControlsDict ValidationControlsDict,
                    ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

        #endregion Data Validation

        private DataTable GetAccountingPeriodListTable(TDBTransaction AReadTransaction, System.Int32 ALedgerNumber, string ATableName)
        {
#region ManualCode
            StringCollection FieldList = new StringCollection();
            FieldList.Add(AAccountingPeriodTable.GetLedgerNumberDBName());
            FieldList.Add(AAccountingPeriodTable.GetAccountingPeriodNumberDBName());
            FieldList.Add(AAccountingPeriodTable.GetAccountingPeriodDescDBName());
            FieldList.Add(AAccountingPeriodTable.GetPeriodStartDateDBName());
            FieldList.Add(AAccountingPeriodTable.GetPeriodEndDateDBName());
            return AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, FieldList, AReadTransaction);
#endregion ManualCode
        }

        private DataTable GetLedgerNameListTable(TDBTransaction AReadTransaction, string ATableName)
        {
#region ManualCode
            return TALedgerNameAggregate.GetData(ATableName, AReadTransaction);
#endregion ManualCode
        }

        private DataTable GetLedgerDetailsTable(TDBTransaction AReadTransaction, System.Int32 ALedgerNumber, string ATableName)
        {
#region ManualCode
//            StringCollection FieldList = new StringCollection();
//            FieldList.Add(ALedgerTable.GetLedgerNumberDBName());
//            FieldList.Add(ALedgerTable.GetNumberFwdPostingPeriodsDBName());
//            FieldList.Add(ALedgerTable.GetNumberOfAccountingPeriodsDBName());
//            FieldList.Add(ALedgerTable.GetCurrentPeriodDBName());
//            FieldList.Add(ALedgerTable.GetCurrentFinancialYearDBName());
//            FieldList.Add(ALedgerTable.GetBranchProcessingDBName());
//            FieldList.Add(ALedgerTable.GetBaseCurrencyDBName());
//            FieldList.Add(ALedgerTable.GetIntlCurrencyDBName());
            return ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, AReadTransaction);
#endregion ManualCode
        }

        private DataTable GetCostCentreListTable(TDBTransaction AReadTransaction, System.Int32 ALedgerNumber, string ATableName)
        {
#region ManualCode
            StringCollection FieldList = new StringCollection();
            FieldList.Add(ACostCentreTable.GetLedgerNumberDBName());
            FieldList.Add(ACostCentreTable.GetCostCentreCodeDBName());
            FieldList.Add(ACostCentreTable.GetCostCentreNameDBName());
            FieldList.Add(ACostCentreTable.GetCostCentreToReportToDBName());
            FieldList.Add(ACostCentreTable.GetPostingCostCentreFlagDBName());
            FieldList.Add(ACostCentreTable.GetCostCentreActiveFlagDBName());
            FieldList.Add(ACostCentreTable.GetCostCentreTypeDBName());
            return ACostCentreAccess.LoadViaALedger(ALedgerNumber, FieldList, AReadTransaction);
#endregion ManualCode
        }

        private DataTable GetAccountListTable(TDBTransaction AReadTransaction, System.Int32 ALedgerNumber, string ATableName)
        {
#region ManualCode
            StringCollection FieldList = new StringCollection();
            FieldList.Add(AAccountTable.GetLedgerNumberDBName());
            FieldList.Add(AAccountTable.GetAccountCodeDBName());
            FieldList.Add(AAccountTable.GetAccountCodeShortDescDBName());
            FieldList.Add(AAccountTable.GetAccountActiveFlagDBName());
            FieldList.Add(AAccountTable.GetPostingStatusDBName());
            FieldList.Add(AAccountTable.GetForeignCurrencyFlagDBName());
            FieldList.Add(AAccountTable.GetForeignCurrencyCodeDBName());
            GLSetupTDS TempDS = new GLSetupTDS();
            AAccountAccess.LoadViaALedger(TempDS, ALedgerNumber, FieldList, AReadTransaction);

            // load AAccountProperty and set the BankAccountFlag
            AAccountPropertyAccess.LoadViaALedger(TempDS, ALedgerNumber, AReadTransaction);

            foreach (AAccountPropertyRow accProp in TempDS.AAccountProperty.Rows)
            {
                if ((accProp.PropertyCode == MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT) && (accProp.PropertyValue == "true"))
                {
                    TempDS.AAccount.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        AAccountTable.GetAccountCodeDBName(),
                        accProp.AccountCode);
                    GLSetupTDSAAccountRow acc = (GLSetupTDSAAccountRow)TempDS.AAccount.DefaultView[0].Row;
                    acc.BankAccountFlag = true;
                    TempDS.AAccount.DefaultView.RowFilter = "";
                }
            }

            // load AAccountHierarchyDetails and check if this account reports to the CASH account
            AAccountHierarchyDetailAccess.LoadViaAAccountHierarchy(TempDS, ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, AReadTransaction);

            TLedgerInfo ledgerInfo = new TLedgerInfo(ALedgerNumber);
            TGetAccountHierarchyDetailInfo accountHierarchyTools = new TGetAccountHierarchyDetailInfo(ledgerInfo);
            List<string> children = accountHierarchyTools.GetChildren(MFinanceConstants.CASH_ACCT);

            foreach (GLSetupTDSAAccountRow account in TempDS.AAccount.Rows)
            {
                if (children.Contains(account.AccountCode))
                {
                    account.CashAccountFlag = true;
                }
            }

            return TempDS.AAccount;
#endregion ManualCode
        }

        private DataTable GetAccountHierarchyListTable(TDBTransaction AReadTransaction, System.Int32 ALedgerNumber, string ATableName)
        {
#region ManualCode
            StringCollection FieldList = new StringCollection();
            FieldList.Add(AAccountHierarchyTable.GetLedgerNumberDBName());
            FieldList.Add(AAccountHierarchyTable.GetAccountHierarchyCodeDBName());
            return AAccountHierarchyAccess.LoadViaALedger(ALedgerNumber, FieldList, AReadTransaction);
#endregion ManualCode
        }
    }
}
