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
using System.Text;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;

namespace Ict.Petra.Tools.MFinance.Server.GDPdUExport
{
    /// This will export the finance data for the tax office, according to GDPdU
    public class TGDPdUExportBalances
    {
        /// <summary>
        /// export all GL Balances in the given year, towards the specified cost centres
        /// </summary>
        public static void ExportGLBalances(string AOutputPath,
                                          char ACSVSeparator,
                                          string ANewLine,
                                          Int32 ALedgerNumber,
                                          Int32 AFinancialYear,
                                          string ACostCentres)
        {
            string filename = Path.GetFullPath(Path.Combine(AOutputPath, "balance.csv"));
            Console.WriteLine("Writing file: " + filename);

            StringBuilder sb = new StringBuilder();
            
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            
            string sql = 
                String.Format("SELECT {1}, GLM.{0}, {2}, {3} FROM PUB_{4} AS GLM, PUB_{10} AS A " +
                              "WHERE GLM.{5} = {6} AND {7} = {8} AND GLM.{1} IN ({9}) " +
                              "AND A.{5} = GLM.{5} AND A.{0} = GLM.{0} AND " +
                              "A.{11}=true " + 
                              "ORDER BY {1}, {0}",
                              AGeneralLedgerMasterTable.GetAccountCodeDBName(),
                              AGeneralLedgerMasterTable.GetCostCentreCodeDBName(),
                              AGeneralLedgerMasterTable.GetYtdActualBaseDBName(),
                              AGeneralLedgerMasterTable.GetStartBalanceBaseDBName(),
                              AGeneralLedgerMasterTable.GetTableDBName(),
                              AGeneralLedgerMasterTable.GetLedgerNumberDBName(),
                              ALedgerNumber,
                              AGeneralLedgerMasterTable.GetYearDBName(),
                              AFinancialYear,
                              "'" + ACostCentres.Replace(",", "','") + "'",
                              AAccountTable.GetTableDBName(),
                              AAccountTable.GetPostingStatusDBName());
            
            DataTable balances = DBAccess.GDBAccessObj.SelectDT(sql, "balances", Transaction);
            
            DBAccess.GDBAccessObj.RollbackTransaction();

            foreach (DataRow row in balances.Rows)
            {
                StringBuilder attributes = new StringBuilder();

                sb.Append(StringHelper.StrMerge(
                    new string[]{
                        row[0].ToString(),
                        row[1].ToString(),
                        String.Format("{0:N}", Convert.ToDecimal(row[2])),
                        String.Format("{0:N}", Convert.ToDecimal(row[3]))}, ACSVSeparator));
                
                sb.Append(ANewLine);
            }

            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252));
            sw.Write(sb.ToString());
            sw.Close();
        }
    }
}