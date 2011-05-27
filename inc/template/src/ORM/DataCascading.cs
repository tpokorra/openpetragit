// auto generated with nant generateORM
// Do not modify this file manually!
//
{#GPLFILEHEADER}

namespace {#NAMESPACE}
{
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.Odbc;
    using Ict.Common;
    using Ict.Common.DB;
    using Ict.Common.Verification;
    using Ict.Common.Data;
    {#USINGNAMESPACES}

    {#TABLECASCADINGLOOP}
}

{##TABLECASCADING}

/// auto generated
public class {#TABLENAME}Cascading : TTypedDataAccess
{
    /// cascading delete
    public static void DeleteByPrimaryKey({#FORMALPARAMETERSPRIMARYKEY}, TDBTransaction ATransaction, bool AWithCascDelete)
    {
{#IFDEF DELETEBYPRIMARYKEYCASCADING}
        int countRow;
        if ((AWithCascDelete == true))
        {
            {#DELETEBYPRIMARYKEYCASCADING}
        }
{#ENDIF DELETEBYPRIMARYKEYCASCADING}
        {#TABLENAME}Access.DeleteByPrimaryKey({#ACTUALPARAMETERSPRIMARYKEY}, ATransaction);
    }
    
    /// cascading delete
    public static void DeleteUsingTemplate({#TABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
    {
{#IFDEF DELETEBYTEMPLATECASCADING}
        int countRow;
        if ((AWithCascDelete == true))
        {
            {#DELETEBYTEMPLATECASCADING}
        }
{#ENDIF DELETEBYTEMPLATECASCADING}
        {#TABLENAME}Access.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
    }
}

{##DELETEBYPRIMARYKEYCASCADING}
{#OTHERTABLENAME}Table {#MYOTHERTABLENAME}Table = {#OTHERTABLENAME}Access.Load{#VIAPROCEDURENAME}({#ACTUALPARAMETERSPRIMARYKEY}, StringHelper.StrSplit("{#CSVLISTOTHERPRIMARYKEYFIELDS}", ","), ATransaction);
for (countRow = 0; (countRow != {#MYOTHERTABLENAME}Table.Rows.Count); countRow = (countRow + 1))
{
{#IFDEF OTHERTABLEALSOCASCADING}
    {#OTHERTABLENAME}Cascading.DeleteUsingTemplate({#MYOTHERTABLENAME}Table[countRow], null, ATransaction, AWithCascDelete);
{#ENDIF OTHERTABLEALSOCASCADING}
{#IFNDEF OTHERTABLEALSOCASCADING}
    {#OTHERTABLENAME}Access.DeleteUsingTemplate({#MYOTHERTABLENAME}Table[countRow], null, ATransaction);
{#ENDIFN OTHERTABLEALSOCASCADING}
}

{##DELETEBYTEMPLATECASCADING}
{#OTHERTABLENAME}Table {#MYOTHERTABLENAME}Table = {#OTHERTABLENAME}Access.Load{#VIAPROCEDURENAME}Template(ATemplateRow, StringHelper.StrSplit("{#CSVLISTOTHERPRIMARYKEYFIELDS}", ","), ATransaction);
for (countRow = 0; (countRow != {#MYOTHERTABLENAME}Table.Rows.Count); countRow = (countRow + 1))
{
{#IFDEF OTHERTABLEALSOCASCADING}
    {#OTHERTABLENAME}Cascading.DeleteUsingTemplate({#MYOTHERTABLENAME}Table[countRow], null, ATransaction, AWithCascDelete);
{#ENDIF OTHERTABLEALSOCASCADING}
{#IFNDEF OTHERTABLEALSOCASCADING}
    {#OTHERTABLENAME}Access.DeleteUsingTemplate({#MYOTHERTABLENAME}Table[countRow], null, ATransaction);
{#ENDIFN OTHERTABLEALSOCASCADING}
}