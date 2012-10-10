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
using System.Configuration;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Tools.MFinance.Server.GDPdUExport
{
    /// This will export the accounts and costcentres involved
    public class TGDPdUExportAccountsAndCostCentres
    {
        private static ACostCentreRow GetDepartmentCostCentre(ACostCentreTable ACostCentres, ACostCentreRow ACostCentreToInvestigate, StringCollection ADepartmentCodes)
        {
            if (ADepartmentCodes.Contains(ACostCentreToInvestigate.CostCentreCode))
            {
                return ACostCentreToInvestigate;
            }
            
            ACostCentreRow row = (ACostCentreRow)ACostCentres.DefaultView.FindRows(ACostCentreToInvestigate.CostCentreToReportTo)[0].Row;
            
            return GetDepartmentCostCentre(ACostCentres, row, ADepartmentCodes);
        }
        
        /// <summary>
        /// Export the cost centres
        /// </summary>
        public static void ExportCostCentres(string AOutputPath,
            char ACSVSeparator,
            string ANewLine,
            Int32 ALedgerNumber,
            List <string>ACostCentres)
        {
            string filename = Path.GetFullPath(Path.Combine(AOutputPath, "costcentre.csv"));

            Console.WriteLine("Writing file: " + filename);

            StringBuilder sb = new StringBuilder();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            string sql =
                String.Format("SELECT * from PUB_{0} WHERE {1} = {2} ORDER BY {3}",
                    ACostCentreTable.GetTableDBName(),
                    ACostCentreTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    ACostCentreTable.GetCostCentreCodeDBName());

            ACostCentreTable costcentres = new ACostCentreTable();
            DBAccess.GDBAccessObj.SelectDT(costcentres, sql, Transaction, null, 0, 0);

            DBAccess.GDBAccessObj.RollbackTransaction();
            
            costcentres.DefaultView.Sort = ACostCentreTable.GetCostCentreCodeDBName();

            foreach (ACostCentreRow row in costcentres.Rows)
            {
                if (ACostCentres.Contains(row.CostCentreCode))
                {
                    ACostCentreRow departmentRow = GetDepartmentCostCentre(costcentres,
                                                                        row,
                                                                        StringHelper.StrSplit(TAppSettingsManager.GetValue("SummaryCostCentres", "4300S"), ","));
                    
                    sb.Append(StringHelper.StrMerge(new string[] { row.CostCentreCode, row.CostCentreName, departmentRow.CostCentreName }, ACSVSeparator));
                    sb.Append(ANewLine);
                }
            }

            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252));
            sw.Write(sb.ToString());
            sw.Close();
        }

        /// <summary>
        /// Export the accounts
        /// </summary>
        public static void ExportAccounts(string AOutputPath,
            char ACSVSeparator,
            string ANewLine,
            Int32 ALedgerNumber,
            List <string>AAccounts)
        {
            string filename = Path.GetFullPath(Path.Combine(AOutputPath, "account.csv"));

            Console.WriteLine("Writing file: " + filename);

            StringBuilder sb = new StringBuilder();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            // only export accounts that are actually used with these cost centres
            string sql =
                String.Format("SELECT {0}, {1}, {2} from PUB_{3} WHERE {4} = {5} AND " +
                    " {6}=true ORDER BY {0}",
                    AAccountTable.GetAccountCodeDBName(),
                    AAccountTable.GetAccountCodeLongDescDBName(),
                    AAccountTable.GetDebitCreditIndicatorDBName(),
                    AAccountTable.GetTableDBName(),
                    AAccountTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    AAccountTable.GetPostingStatusDBName());

            DataTable accounts = DBAccess.GDBAccessObj.SelectDT(sql, "accounts", Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            foreach (DataRow row in accounts.Rows)
            {
                if (AAccounts.Contains(row[0].ToString()))
                {
                    sb.Append(StringHelper.StrMerge(new string[] { row[0].ToString(), row[1].ToString(),
                                                                   Convert.ToBoolean(row[2]) ? "Soll" : "Haben" }, ACSVSeparator));
                    sb.Append(ANewLine);
                }
            }

            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252));
            sw.Write(sb.ToString());
            sw.Close();
        }
    }
}