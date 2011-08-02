// auto generated with nant generateORM
// Do not modify this file manually!
//
{#GPLFILEHEADER}

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Petra.Shared;
using System;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
{#USINGNAMESPACES}

namespace {#NAMESPACE}.Access
{
    {#CONTENTDATASETSANDTABLESANDROWS}
}

{##TYPEDDATASET}
 /// auto generated
[Serializable()]
public class {#DATASETNAME}Access
{
    {#SUBMITCHANGESFUNCTION}
}

{##SUBMITCHANGESFUNCTION}

/// auto generated
static public TSubmitChangesResult SubmitChanges({#DATASETNAME} AInspectDS, out TVerificationResultCollection AVerificationResult)
{
    AVerificationResult = new TVerificationResultCollection();

    if (AInspectDS == null)
    {
        return TSubmitChangesResult.scrOK;
    }

    TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
    TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

    try
    {
        SubmissionResult = TSubmitChangesResult.scrOK;
        
        {#SUBMITCHANGESDELETE}
        {#SUBMITCHANGESINSERT}
        {#SUBMITCHANGESUPDATE}
        
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
        TLogging.Log("exception during saving dataset {#DATASETNAME}:" + e.Message);

        DBAccess.GDBAccessObj.RollbackTransaction();

        throw new Exception(e.ToString() + " " + e.Message);
    }

    return SubmissionResult;
}

{##SUBMITCHANGES}
{#IFNDEF UPDATESEQUENCEINOTHERTABLES}
if (SubmissionResult == TSubmitChangesResult.scrOK
    && !TTypedDataAccess.SubmitChanges(AInspectDS.{#TABLEVARIABLENAME}, SubmitChangesTransaction,
            TTypedDataAccess.eSubmitChangesOperations.{#SQLOPERATION},
            out AVerificationResult,
            UserInfo.GUserInfo.UserID{#SEQUENCENAMEANDFIELD}))
{
    SubmissionResult = TSubmitChangesResult.scrError;
}
{#ENDIFN UPDATESEQUENCEINOTHERTABLES}
{#IFDEF UPDATESEQUENCEINOTHERTABLES}
if (SubmissionResult == TSubmitChangesResult.scrOK && AInspectDS.{#TABLEVARIABLENAME} != null)
{
    SortedList<Int64, Int32> OldSequenceValuesRow = new SortedList<Int64, Int32>();
    Int32 rowIndex = 0;
    foreach ({#TABLEROWTYPE} origRow in AInspectDS.{#TABLEVARIABLENAME}.Rows)
    {
        if (origRow.RowState != DataRowState.Deleted && origRow.{#SEQUENCEDCOLUMNNAME} < 0)
        {
            OldSequenceValuesRow.Add(origRow.{#SEQUENCEDCOLUMNNAME}, rowIndex);
        }

        rowIndex++;
    }
    if (!TTypedDataAccess.SubmitChanges(AInspectDS.{#TABLEVARIABLENAME}, SubmitChangesTransaction,
            TTypedDataAccess.eSubmitChangesOperations.{#SQLOPERATION},
            out AVerificationResult,
            UserInfo.GUserInfo.UserID{#SEQUENCENAMEANDFIELD}))
    {
        SubmissionResult = TSubmitChangesResult.scrError;
    }
    else
    {
        {#UPDATESEQUENCEINOTHERTABLES}
    }
}
{#ENDIF UPDATESEQUENCEINOTHERTABLES}

{##UPDATESEQUENCEINOTHERTABLES}
if (AInspectDS.{#REFERENCINGTABLENAME} != null)
{
    foreach ({#REFERENCINGTABLEROWTYPE} otherRow in AInspectDS.{#REFERENCINGTABLENAME}.Rows)
    {
        if ((otherRow.RowState != DataRowState.Deleted) && {#TESTFORNULL}otherRow.{#REFCOLUMNNAME} < 0)
        {
            otherRow.{#REFCOLUMNNAME} = AInspectDS.{#TABLEVARIABLENAME}[OldSequenceValuesRow[otherRow.{#REFCOLUMNNAME}]].{#SEQUENCEDCOLUMNNAME};
        }
    }
}