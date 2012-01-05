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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Globalization;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MSysMan.ImportExport.WebConnectors
{
    /// <summary>
    /// import and export of all data in this database
    /// </summary>
    public class TImportExportWebConnector
    {
        /// <summary>
        /// return a compressed XmlDocument with all data in this database;
        /// this is useful to convert data between different database systems, etc
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("SYSMAN")]
        public static string ExportAllTables()
        {
            XmlDocument OpenPetraData = TYml2Xml.CreateXmlDocument();

            XmlNode rootNode = OpenPetraData.FirstChild.NextSibling;

            Assembly TypedTablesAssembly = Assembly.LoadFrom("Ict.Petra.Shared.lib.data.dll");

            ExportTables(rootNode, "MSysMan", "", TypedTablesAssembly);
            ExportTables(rootNode, "MCommon", "", TypedTablesAssembly);
            ExportTables(rootNode, "MPartner", "Partner", TypedTablesAssembly);
            ExportTables(rootNode, "MPartner", "Mailroom", TypedTablesAssembly);
            ExportTables(rootNode, "MFinance", "Account", TypedTablesAssembly);
            ExportTables(rootNode, "MFinance", "Gift", TypedTablesAssembly);
            ExportTables(rootNode, "MFinance", "AP", TypedTablesAssembly);
            ExportTables(rootNode, "MFinance", "AR", TypedTablesAssembly);
            ExportTables(rootNode, "MPersonnel", "Personnel", TypedTablesAssembly);
            ExportTables(rootNode, "MPersonnel", "Units", TypedTablesAssembly);
            ExportTables(rootNode, "MConference", "", TypedTablesAssembly);
            ExportTables(rootNode, "MHospitality", "", TypedTablesAssembly);
            ExportSequences(rootNode);
            return TYml2Xml.Xml2YmlGz(OpenPetraData);
        }

        /// <summary>
        /// export one module at the time
        /// </summary>
        /// <param name="ARootNode"></param>
        /// <param name="AModuleName"></param>
        /// <param name="ASubModuleName">can be empty if there is no submodule</param>
        /// <param name="ATypedTablesAssembly">the assembly with all types of the tables in the database</param>
        private static void ExportTables(XmlNode ARootNode, string AModuleName, string ASubModuleName, Assembly ATypedTablesAssembly)
        {
            XmlElement moduleNode = ARootNode.OwnerDocument.CreateElement(AModuleName + ASubModuleName);

            ARootNode.AppendChild(moduleNode);

            string namespaceName = "Ict.Petra.Shared." + AModuleName;

            if (ASubModuleName.Length > 0)
            {
                namespaceName += "." + ASubModuleName;
            }

            namespaceName += ".Data";

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();

            SortedList <string, Type>SortedTypes = new SortedList <string, Type>();

            foreach (Type type in ATypedTablesAssembly.GetTypes())
            {
                if ((type.Namespace == namespaceName) && type.Name.EndsWith("Table"))
                {
                    SortedTypes.Add(type.Name, type);
                }
            }

            // export the tables, ordered by name
            foreach (string typeName in SortedTypes.Keys)
            {
                ExportTable(moduleNode, ATypedTablesAssembly, SortedTypes[typeName], Transaction);
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
        }

        private static void ExportTable(XmlNode AModuleNode, Assembly AAsm, Type ATableType, TDBTransaction ATransaction)
        {
            if (ATableType.Name.Contains("TDS"))
            {
                // this is probably a table in a typed dataset
                return;
            }

            MethodInfo mi = ATableType.GetMethod("GetTableDBName", BindingFlags.Static | BindingFlags.Public);
            string TableDBName = mi.Invoke(null, null).ToString();
            DataTable table = DBAccess.GDBAccessObj.SelectDT("Select * from " + TableDBName, TableDBName, ATransaction);

            XmlElement tableNode = AModuleNode.OwnerDocument.CreateElement(ATableType.Name);

            AModuleNode.AppendChild(tableNode);

            // for SQLite the table is not sorted by primary key. Therefore do it manually
            DataView v = table.DefaultView;
            DataTable t = (DataTable)AAsm.CreateInstance(ATableType.Namespace + "." + ATableType.Name);
            string sortOrderPrimaryKey = String.Empty;

            foreach (DataColumn c in t.PrimaryKey)
            {
                if (sortOrderPrimaryKey.Length > 0)
                {
                    sortOrderPrimaryKey += ",";
                }

                sortOrderPrimaryKey += c.ColumnName;
            }

            v.Sort = sortOrderPrimaryKey;

            Int32 RowCounter = 0;

            ConvertColumnNames(table.Columns);

            // TODO: automatically filter column values that are the same and group the data?
            foreach (DataRowView rv in v)
            {
                DataRow row = rv.Row;
                RowCounter++;
                XmlElement rowNode = AModuleNode.OwnerDocument.CreateElement("Row" + RowCounter.ToString());
                tableNode.AppendChild(rowNode);

                foreach (DataColumn col in table.Columns)
                {
                    if (row[col].GetType() != typeof(DBNull))
                    {
                        if (col.DataType == typeof(DateTime))
                        {
                            DateTime d = Convert.ToDateTime(row[col]);

                            if (d.TimeOfDay == TimeSpan.Zero)
                            {
                                rowNode.SetAttribute(col.ColumnName, d.ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                rowNode.SetAttribute(col.ColumnName, d.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                        }
                        else if ((col.DataType == typeof(double)) || (col.DataType == typeof(decimal)))
                        {
                            // store decimals always with decimal point, no thousands separator
                            decimal dval = Convert.ToDecimal(row[col]);
                            rowNode.SetAttribute(col.ColumnName, dval.ToString("0.########", CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            rowNode.SetAttribute(col.ColumnName, row[col].ToString().Replace("\"", "&quot;"));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// export the sequences
        /// </summary>
        /// <param name="ARootNode"></param>
        private static void ExportSequences(XmlNode ARootNode)
        {
            XmlElement sequencesNode = ARootNode.OwnerDocument.CreateElement("Sequences");

            ARootNode.AppendChild(sequencesNode);

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();

            foreach (string seq in TTableList.GetDBSequenceNames())
            {
                XmlElement sequenceNode = ARootNode.OwnerDocument.CreateElement(StringHelper.UpperCamelCase(seq, false, false));
                sequencesNode.AppendChild(sequenceNode);

                sequenceNode.SetAttribute("value", DBAccess.GDBAccessObj.GetCurrentSequenceValue(seq, Transaction).ToString());
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
        }

        /// <summary>
        /// this will reset the current database, and load the data from the given XmlDocument
        /// </summary>
        /// <param name="AZippedNewDatabaseData">zipped YML</param>
        /// <returns></returns>
        [RequireModulePermission("SYSMAN")]
        public static bool ResetDatabase(string AZippedNewDatabaseData)
        {
            List <string>tables = TTableList.GetDBNames();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                tables.Reverse();

                foreach (string table in tables)
                {
                    DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM pub_" + table, Transaction, false);
                }

                TYml2Xml ymlParser = new TYml2Xml(PackTools.UnzipString(AZippedNewDatabaseData).Replace("\r", "").Split(new char[] { '\n' }));
                XmlDocument doc = ymlParser.ParseYML2XML();

                tables.Reverse();

                // one transaction to import the user table and user permissions. otherwise logging in will not be possible if other import fails?
                bool success = true;
                success = success && LoadTable("s_user", doc, Transaction);
                success = success && LoadTable("s_module", doc, Transaction);
                success = success && LoadTable("s_user_module_access_permission", doc, Transaction);
                success = success && LoadTable("s_system_defaults", doc, Transaction);
                success = success && LoadTable("s_system_status", doc, Transaction);

                if (!success)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    return false;
                }

                DBAccess.GDBAccessObj.CommitTransaction();
                tables.Remove("s_user");
                tables.Remove("s_module");
                tables.Remove("s_user_module_access_permission");
                tables.Remove("s_system_defaults");
                tables.Remove("s_system_status");

                Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                foreach (string table in tables)
                {
                    LoadTable(table, doc, Transaction);
                }

                // set sequences appropriately, not lagging behind the imported data
                foreach (string seq in TTableList.GetDBSequenceNames())
                {
                    LoadSequence(seq, doc, Transaction);
                }

                // make sure we have the correct database version
                TFileVersionInfo serverExeInfo = new TFileVersionInfo(TSrvSetting.ApplicationVersion);
                DBAccess.GDBAccessObj.ExecuteNonQuery(String.Format(
                        "UPDATE PUB_s_system_defaults SET s_default_value_c = '{0}' WHERE s_default_code_c = 'CurrentDatabaseVersion'",
                        serverExeInfo.ToString()), Transaction, false);

                DBAccess.GDBAccessObj.CommitTransaction();

                // reset all cached tables
                TCacheableTablesManager.GCacheableTablesManager.MarkAllCachedTableNeedsRefreshing();
            }
            catch (Exception e)
            {
                TLogging.Log("Problem in ResetDatabase: " + e.Message);
                TLogging.Log(e.StackTrace);
                DBAccess.GDBAccessObj.RollbackTransaction();
                return false;
            }

            return true;
        }

        private static XmlNode FindNode(XmlDocument ADoc, string ATableName)
        {
            XmlNode rootNode = ADoc.FirstChild.NextSibling;

            foreach (XmlNode ModuleNode in rootNode.ChildNodes)
            {
                foreach (XmlNode TableNode in ModuleNode.ChildNodes)
                {
                    if (TableNode.Name == ATableName)
                    {
                        return TableNode;
                    }
                }
            }

            return null;
        }

        private static bool LoadSequence(string ASequenceName, XmlDocument ADoc, TDBTransaction ATransaction)
        {
            XmlNode SequenceNode = FindNode(ADoc, StringHelper.UpperCamelCase(ASequenceName, false, false));

            if (SequenceNode == null)
            {
                TLogging.Log("SequenceNode null: " + ASequenceName);
                return false;
            }

            DBAccess.GDBAccessObj.RestartSequence(ASequenceName, ATransaction, Convert.ToInt64(TYml2Xml.GetAttribute(SequenceNode, "value")));

            return true;
        }

        private static bool LoadTable(string ATableName, XmlDocument ADoc, TDBTransaction ATransaction)
        {
            XmlNode TableNode = FindNode(ADoc, StringHelper.UpperCamelCase(ATableName, false, false) + "Table");

            if (TableNode == null)
            {
                // TLogging.Log("tablenode null: " + ATableName);
                return false;
            }

            if (TableNode.ChildNodes.Count == 0)
            {
                // TLogging.Log("no children: " + ATableName);
                return false;
            }

            DataTable table = DBAccess.GDBAccessObj.SelectDT("Select * from " + ATableName, ATableName, ATransaction);
            List <OdbcParameter>Parameters = new List <OdbcParameter>();

            string InsertStatement = "INSERT INTO pub_" + ATableName + "() VALUES ";

            string OrigInsertStatement = InsertStatement;

            ConvertColumnNames(table.Columns);

            bool firstRow = true;

            foreach (XmlNode RowNode in TableNode.ChildNodes)
            {
                if (!firstRow)
                {
                    if (CommonTypes.ParseDBType(DBAccess.GDBAccessObj.DBType) == TDBType.SQLite)
                    {
                        // SQLite does not support INSERT of several rows at the same time
                        try
                        {
                            DBAccess.GDBAccessObj.ExecuteNonQuery(InsertStatement, ATransaction, false, Parameters.ToArray());
                        }
                        catch (Exception e)
                        {
                            TLogging.Log("error in ResetDatabase, LoadTable " + ATableName + ":" + e.Message);
                            throw e;
                        }

                        InsertStatement = OrigInsertStatement;
                        Parameters = new List <OdbcParameter>();
                    }
                    else
                    {
                        InsertStatement += ",";
                    }
                }

                firstRow = false;

                InsertStatement += "(";

                bool firstColumn = true;

                foreach (DataColumn col in table.Columns)
                {
                    if (!firstColumn)
                    {
                        InsertStatement += ",";
                    }

                    firstColumn = false;

                    if (TYml2Xml.HasAttribute(RowNode, col.ColumnName))
                    {
                        string strValue = TYml2Xml.GetAttribute(RowNode, col.ColumnName);

                        if (col.DataType == typeof(DateTime))
                        {
                            OdbcParameter p;

                            if (strValue.Length == "yyyy-MM-dd".Length)
                            {
                                p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.Date);
                                p.Value = DateTime.ParseExact(strValue, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.DateTime);
                                p.Value = DateTime.ParseExact(strValue, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            }

                            Parameters.Add(p);
                        }
                        else if (col.DataType == typeof(String))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.VarChar);
                            p.Value = strValue.Replace("&quot;", "\"");
                            Parameters.Add(p);
                        }
                        else if (col.DataType == typeof(Int32))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.Int);
                            p.Value = Convert.ToInt32(strValue);
                            Parameters.Add(p);
                        }
                        else if (col.DataType == typeof(Int64))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.Decimal);
                            p.Value = Convert.ToInt64(strValue);
                            Parameters.Add(p);
                        }
                        else if (col.DataType == typeof(double))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.Decimal);
                            p.Value = Convert.ToDouble(strValue, CultureInfo.InvariantCulture);
                            Parameters.Add(p);
                        }
                        else if (col.DataType == typeof(bool))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.Bit);
                            p.Value = Convert.ToBoolean(strValue);
                            Parameters.Add(p);
                        }
                        else if (col.DataType == typeof(Decimal))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.Decimal);
                            p.Value = Convert.ToDecimal(strValue, CultureInfo.InvariantCulture);
                            Parameters.Add(p);
                        }
                        else
                        {
                            // should not get here?
                            throw new Exception("error in ResetDatabase, LoadTable: " + col.DataType.ToString() + " has not yet been implemented");
                        }

                        InsertStatement += "?";
                    }
                    else
                    {
                        InsertStatement += "NULL"; // DEFAULT
                    }
                }

                InsertStatement += ")";
            }

            try
            {
                DBAccess.GDBAccessObj.ExecuteNonQuery(InsertStatement, ATransaction, false, Parameters.ToArray());
            }
            catch (Exception e)
            {
                TLogging.Log("error in ResetDatabase, LoadTable " + ATableName + ":" + e.Message);
                throw e;
            }
            return true;
        }

        private static void ConvertColumnNames(DataColumnCollection AColumns)
        {
            foreach (DataColumn col in AColumns)
            {
                string colName = StringHelper.UpperCamelCase(col.ColumnName, true, true);

                if (AColumns.Contains(colName))
                {
                    // this column is not unique. happens in p_recent_partner, columns p_when_d and p_when_t
                    colName = StringHelper.UpperCamelCase(col.ColumnName, true, false);
                }

                col.ColumnName = colName;
            }
        }

        /// <summary>
        /// Commits the given SampleDataConstructorTDS TDS to the database
        /// </summary>
        /// <remarks>
        /// Why did I have to spend _several hours_ trying to get into how OpenPetra does things
        /// just in order to add this one line?
        /// </remarks>
        /// <param name="dataTDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("SYSMAN")]
        public static bool SaveTDS(SampleDataConstructorTDS dataTDS, out TVerificationResultCollection AVerificationResult)
        {
            return SampleDataConstructorTDSAccess.SubmitChanges(dataTDS, out AVerificationResult) == TSubmitChangesResult.scrOK;
        }
    }
}