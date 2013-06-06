//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Shared.MPartner;

namespace Tests.MPartner.Server.SEPA
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TSEPATest
    {
        /// <summary>
        /// use automatic property to avoid compiler warning about unused variable FServerManager
        /// </summary>
        private TServerManager FServerManager {
            get; set;
        }

        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            new TLogging("../../log/TestServer.log");
            FServerManager = TPetraServerConnector.Connect("../../etc/TestServer.config");
        }

        /// <summary>
        /// cleaning up everything that was set up for this test
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            TPetraServerConnector.Disconnect();
        }

        /// <summary>
        /// test the conversion of bank account number and bank sort code to IBAN/BIC
        /// </summary>
        [Test]
        public void TestConvertBankAccountNumberToIBANBIC()
        {
            TBankingDetailsSEPA SEPAConverter = new TBankingDetailsSEPA();

            SEPAConverter.InitBLZToBIC("../../csharp/ICT/Testing/lib/MPartner/SampleData/BLZ_sample.txt");

            Assert.AreEqual("NOTSUPPORTED", SEPAConverter.ConvertToBIC("DK", "74351430"), "only supporting german banks at the moment");
            Assert.AreEqual("UNKNOWNBANK", SEPAConverter.ConvertToBIC("DE", "12345678"), "only supporting banks loaded from the BLZ file");
            Assert.AreEqual("INGDDEFFXXX", SEPAConverter.ConvertToBIC("DE", "50010517"), "ConvertToBic");
            Assert.AreEqual("DE12500105170648489890", SEPAConverter.ConvertToIBAN("DE", "50010517", "0648489890"), "ConvertToIban");
            // TODO Assert.AreEqual("INVALIDACCOUNT", SEPAConverter.ConvertToIBAN("DE", "50010517", "5648489890"), "checks the account number");

            // using the format of ZKA / DA of german banks
            string TestFile = "../../csharp/ICT/Testing/lib/MPartner/SampleData/convertIBANBIC.csv";
            using (StreamReader sr = new StreamReader(TestFile))
            {
                int lineCounter = 0;
                int NrErrorsIBAN = 0;
                int NrErrorsBIC = 0;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    lineCounter++;

                    string CountryCode = StringHelper.GetNextCSV(ref line, ";");
                    StringHelper.GetNextCSV(ref line, ";");
                    StringHelper.GetNextCSV(ref line, ";");
                    StringHelper.GetNextCSV(ref line, ";");
                    StringHelper.GetNextCSV(ref line, ";");
                    string BankSortCode = StringHelper.GetNextCSV(ref line, ";");
                    string AccountNumber = StringHelper.GetNextCSV(ref line, ";");
                    StringHelper.GetNextCSV(ref line, ";");
                    string BIC = StringHelper.GetNextCSV(ref line, ";");
                    string IBAN = StringHelper.GetNextCSV(ref line, ";");
                    StringHelper.GetNextCSV(ref line, ";");
                    string errorcode = StringHelper.GetNextCSV(ref line, ";");

                    if (errorcode == "00")
                    {
                        try
                        {
                            Assert.AreEqual(IBAN, SEPAConverter.ConvertToIBAN(CountryCode,
                                    BankSortCode,
                                    AccountNumber), "converting to IBAN, line " + lineCounter.ToString());
                        }
                        catch (Exception e)
                        {
                            TLogging.Log(e.Message);
                            NrErrorsIBAN++;
                        }

                        try
                        {
                            Assert.AreEqual(BIC, SEPAConverter.ConvertToBIC(CountryCode,
                                    BankSortCode), "converting to BIC, line " + lineCounter.ToString());
                        }
                        catch (Exception e)
                        {
                            TLogging.Log(e.Message);
                            NrErrorsBIC++;
                        }
                    }
                }

                Assert.AreEqual(0, NrErrorsBIC, "There have been conversion errors for BIC");
                Assert.AreEqual(0, NrErrorsIBAN, "There have been conversion errors for IBAN");

                sr.Close();
            }
        }
    }
}