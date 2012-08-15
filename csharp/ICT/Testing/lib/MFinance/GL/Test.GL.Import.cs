//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
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

using System;
using System.Collections;
using System.IO;
using System.Data.Odbc;
using NUnit.Framework;
using Ict.Testing.NUnitTools;
using Ict.Testing.NUnitPetraServer;
using Ict.Petra.Server.MFinance.GL;
using Ict.Common;
using Ict.Common.Verification;

using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;


using Ict.Common.DB;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Testing.Petra.Server.MFinance.GL
{
    /// <summary>
    /// TestGLImport
    /// </summary>
    [TestFixture]
    public class TestGLImport
    {
        private const int intLedgerNumber = 43;


        /// <summary>
        /// Test_01_GL_Import
        /// </summary>
        [Test]
        public void Test_01_GL_Import()
        {
            Hashtable requestParams = new Hashtable();

            requestParams.Add("ALedgerNumber", intLedgerNumber);
            requestParams.Add("Delimiter", ";");
            requestParams.Add("DateFormatString", "dd/MM/yyyy");
            requestParams.Add("NumberFormat", "European");
            requestParams.Add("NewLine", Environment.NewLine);

            string strContent = CommonNUnitFunctions.LoadCSVFileToString("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\" +
                "test-csv\\glbatch-import.csv");

            // Console.WriteLine(strContent);
            TVerificationResultCollection verificationResult;

            bool importSuccess = TGLTransactionWebConnector.ImportGLBatches(requestParams, strContent, out verificationResult);

            if (verificationResult.HasCriticalErrors)
            {
                TLogging.Log(verificationResult.BuildVerificationResultString());
            }

            Assert.IsTrue(importSuccess, "Import glbatch-import.csv done well ....");
        }

        /// <summary>
        /// TestFixtureSetUp
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect();
            PrepareTestCaseData();
        }

        private void PrepareTestCaseData()
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();

            // Check if some special test data are available - otherwise load ...
            bool CostCentreTestCasesAvailable = ACostCentreAccess.Exists(intLedgerNumber, "4301", Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            if (!CostCentreTestCasesAvailable)
            {
                CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\" +
                    "test-sql\\gl-test-costcentre-data.sql");
            }
        }

        /// <summary>
        /// TearDown the test
        /// </summary>
        [TestFixtureTearDown]
        public void TearDownTest()
        {
            TPetraServerConnector.Disconnect();
        }
    }
}