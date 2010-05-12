//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;
using Ict.Petra.Client.MReporting.Logic;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Printing;
using Ict.Tools.CodeGeneration;
using Ict.Petra.Shared.MReporting;
using Ict.Testing.NUnitPetraClient;

namespace Tests.Reporting
{
    /// This is a test for the reports.
    /// It runs as a NUnit test, and the login is defined in the config file.
    [TestFixture]
    public class TReportingTest : System.Object
    {
        private CultureInfo OrigCulture;

        /// the System.Object that is able to deal with all the parameters, and can calculate a report
        public TRptCalculator FCalculator;
        public String PathToTestData;
        public String PathToSettingsData;

        [SetUp]
        public void Init()
        {
            new TLogging("test.log");

            // todo: what about different cultures?
            OrigCulture = new CultureInfo("en-GB", false);
            Thread.CurrentThread.CurrentCulture = OrigCulture;
            TPetraConnector.Connect("Tests.Reporting.dll.config");
            FCalculator = new TRptCalculator();
            PathToTestData = "../../Reporting/TestData/".Replace("/", System.IO.Path.DirectorySeparatorChar.ToString());
            PathToSettingsData = "../../../XMLReports/Settings/".Replace("/", System.IO.Path.DirectorySeparatorChar.ToString());
        }

        [TearDown]
        public void TearDown()
        {
            TPetraConnector.Disconnect();
        }

        public static void PrintTxt(TResultList results, TParameterList parameters, string output)
        {
            TReportPrinterLayout reportTxtPrinter;
            TTxtPrinter txtPrinter;

            txtPrinter = new TTxtPrinter();
            reportTxtPrinter = new TReportPrinterLayout(results, parameters, txtPrinter);
            reportTxtPrinter.PrintReport();
            txtPrinter.WriteToFile(output);
        }

        public void TestDetailReport(String ATestPath, String ASettingsPath)
        {
            String detailReportCSV;
            String action;
            String query;
            String paramName;
            String paramValue;
            int SelectedRow;
            int columnCounter;

            detailReportCSV = FCalculator.GetParameters().Get("param_detail_report_0").ToString();
            TLogging.Log("detail report: " + StringHelper.GetNextCSV(ref detailReportCSV, ","));
            action = StringHelper.GetNextCSV(ref detailReportCSV, ",");

            if (action == "PartnerEditScreen")
            {
                /* get the partner key from the parameter */
                SelectedRow = 1;

                if (FCalculator.GetResults().GetResults().Count > 0)
                {
                    TResult row = (TResult)FCalculator.GetResults().GetResults()[SelectedRow];
                    Console.WriteLine("detailReportCSV " + detailReportCSV.ToString());
                    Console.WriteLine(FCalculator.GetResults().GetResults().Count.ToString());

                    if (row.column.Length > 0)
                    {
                        TLogging.Log("Open Partner Edit Screen for partner " + row.column[Convert.ToInt32(detailReportCSV)].ToString());
                    }
                }
            }
            else if (action.IndexOf(".xml") != -1)
            {
                query = StringHelper.GetNextCSV(ref detailReportCSV, ",");
                FCalculator.GetParameters().Add("param_whereSQL", query);

                /* get the parameter names and values */
                while (detailReportCSV.Length > 0)
                {
                    paramName = StringHelper.GetNextCSV(ref detailReportCSV, ",");
                    paramValue = StringHelper.GetNextCSV(ref detailReportCSV, ",");
                    FCalculator.GetParameters().Add(paramName, paramValue);
                }

                /* add the values of the selected column (in this example the first one) */
                SelectedRow = 1;

                for (columnCounter = 0; columnCounter <= FCalculator.GetParameters().Get("MaxDisplayColumns").ToInt32() - 1; columnCounter += 1)
                {
                    FCalculator.GetParameters().Add("param_" +
                        FCalculator.GetParameters().Get("param_calculation",
                            columnCounter).ToString(), ((TResult)FCalculator.GetResults().GetResults()[SelectedRow]).column[columnCounter]);
                }

                /* action is a link to a settings file; it contains e.g. xmlfiles, currentReport, and column settings */
                /* TParameterList.Load adds the new parameters to the existing parameters */
                FCalculator.GetParameters().Load(ASettingsPath + '/' + action);

                if (FCalculator.GenerateResultRemoteClient())
                {
                    FCalculator.GetParameters().Save(ATestPath + "LogParametersAfterCalcDetail.log", true);
                    FCalculator.GetResults().WriteCSV(FCalculator.GetParameters(), ATestPath + "LogResultsDetail.log", "FIND_BEST_SEPARATOR", true);

                    /* test if there is a detail report */
                    if (FCalculator.GetParameters().Exists("param_detail_report_0") == true)
                    {
                        TestDetailReport(ATestPath, ASettingsPath);
                    }
                }
            }
        }

        /** common procedure that will load all settings in the given directory, and run a report and compare the result with results from previous, using the csv and the txt output
         */
        public void TestReport(String ASettingsDirectory)
        {
            String[] fileEntries;
            string fileName;

            if (!Directory.Exists(".." + System.IO.Path.DirectorySeparatorChar + ".." + System.IO.Path.DirectorySeparatorChar + "Reporting" +
                    System.IO.Path.DirectorySeparatorChar + "TestData" + System.IO.Path.DirectorySeparatorChar + ASettingsDirectory))
            {
                TLogging.Log("Test for " + ASettingsDirectory + " does not exist yet!");
                return;
            }

            try
            {
                /* get all xml files in the given directory (assume we are starting it from testing\_bin\debug */
                fileEntries = Directory.GetFiles(PathToTestData + ASettingsDirectory, "*.xml");

                foreach (string s in fileEntries)
                {
                    fileName = s.Substring(0, s.IndexOf(".xml"));
                    System.Console.Write(Path.GetFileName(fileName) + ' ');
                    FCalculator.ResetParameters();
                    FCalculator.GetParameters().Load(fileName + ".xml");

                    if (FCalculator.GenerateResultRemoteClient())
                    {
                        FCalculator.GetParameters().Save(
                            PathToTestData + ASettingsDirectory + System.IO.Path.DirectorySeparatorChar + "LogParametersAfterCalc.log",
                            true);
                        FCalculator.GetResults().WriteCSV(
                            FCalculator.GetParameters(), PathToTestData + ASettingsDirectory + System.IO.Path.DirectorySeparatorChar +
                            "LogResults.log",
                            "FIND_BEST_SEPARATOR", true);

                        if (!System.IO.File.Exists(fileName + ".csv"))
                        {
                            /* create a new reference file */
                            FCalculator.GetResults().WriteCSV(FCalculator.GetParameters(), fileName + ".csv");
                        }
                        else
                        {
                            FCalculator.GetResults().WriteCSV(FCalculator.GetParameters(), fileName + ".csv.new");
                        }

                        if (!System.IO.File.Exists(fileName + ".txt"))
                        {
                            /* create a new reference file */
                            PrintTxt(FCalculator.GetResults(), FCalculator.GetParameters(), fileName + ".txt");
                        }
                        else
                        {
                            PrintTxt(FCalculator.GetResults(), FCalculator.GetParameters(), fileName + ".txt.new");
                        }

                        if (System.IO.File.Exists(fileName + ".csv.new"))
                        {
                            /* compare the files */
                            Assert.AreEqual(true, TTextFile.SameContent(fileName + ".csv",
                                    fileName + ".csv.new"), "the csv files should be the same: " + fileName);
                            System.IO.File.Delete(fileName + ".csv.new");
                        }

                        if (System.IO.File.Exists(fileName + ".txt.new"))
                        {
                            /* compare the files */
                            /* requires compilation with directive TESTMODE being set, so that the date of the report printout is constant */
                            Assert.AreEqual(true, TTextFile.SameContent(fileName + ".txt",
                                    fileName + ".txt.new"), "the txt files should be the same: " + fileName);
                            System.IO.File.Delete(fileName + ".txt.new");
                        }

                        /* todo: test if something was written to the log file (e.g. parameter not found, etc); */
                        /* make a copy of the file before running the report, and compare with the new version */
                        /* this proves difficult, because it runs against the server */
                        /* once the progress of the report is fed back, we can compare the two output files */
                        /* test if there is a detail report */
                        if (FCalculator.GetParameters().Exists("param_detail_report_0") == true)
                        {
                            TestDetailReport(PathToTestData + ASettingsDirectory + System.IO.Path.DirectorySeparatorChar, PathToSettingsData);
                        }
                    }
                    else
                    {
                        Assert.Fail("Report calculation did not finish, was cancelled on the server");
                    }
                }
            }
            catch (Exception e)
            {
                if (e.GetType() != typeof(NUnit.Framework.AssertionException))
                {
                    System.Console.WriteLine(e.Message);
                    System.Console.WriteLine(e.StackTrace);
                }

                throw e;
            }
        }

        [Test]
        public void TestFDDonorsPerRecipient()
        {
            TestReport("FDDonorsPerRecipient");
        }

        [Test]
        public void TestPassportExpiry()
        {
            TestReport("Passport Expiry");
        }

        [Test]
        public void TestAccountDetail()
        {
            TestReport("Account Detail");
        }

        [Test]
        public void TestAccountDetailAnalysisAttr()
        {
            TestReport("AccountDetailAnalysisAttr");
        }

        [Test]
        public void TestAPPaymentExport()
        {
            TestReport("APPaymentExport");
        }

        [Test]
        public void TestBalSheet()
        {
            TestReport("BalanceSheet");
        }

        [Test]
        public void TestBalSheetMultiLedger()
        {
            TestReport("BalSheet MultiLedger");
        }

        [Test]
        public void TestBirthdayList()
        {
            TestReport("Birthday List");
        }

        [Test]
        public void TestFDIncomeByFund()
        {
            TestReport("FDIncomeByFund");
        }

        [Test]
        public void TestGiftBatchExport()
        {
            TestReport("GiftBatchExport");
        }

        [Test]
        public void TestIncExpMultiLedger()
        {
            TestReport("IncExp MultiLedger");
        }

        [Test]
        public void TestIncExpMultiPeriod()
        {
            TestReport("IncExp MultiPeriod");
        }

        [Test]
        public void TestIncExpStatement()
        {
            TestReport("Income Expense Statement");
        }

        [Test]
        public void TestSurplusDeficit()
        {
            TestReport("SurplusDeficit");
        }

        [Test]
        public void TestTrialBalance()
        {
            TestReport("TrialBalance");
        }

        [Test]
        public void TestPartnerFindByEmail()
        {
            TestReport("PartnerFindByEmail");
        }

        [Test]
        public void TestGiftExportByMotivation()
        {
            TestReport("GiftExportByMotivation");
        }

        [Test]
        public void TestFDIncomeLocalSplit()
        {
            TestReport("FDIncomeLocalSplit");
        }

        [Test]
        public void TestTotalGiftsPerDonor()
        {
            TestReport("TotalGiftsPerDonor");
        }

        [Test]
        public void TestGiftMethodGiving()
        {
            TestReport("GiftMethodGiving");
        }

        [Test]
        public void TestAddressesOfRelationships()
        {
            TestReport("AddressesOfRelationships");
        }

        [Test]
        public void TestGiftDataExport()
        {
            TestReport("GiftDataExport");
        }

        /** for testing csv files and printouts
         */
        public void RunLocalizedReport(String OutputCSVFile)
        {
            if (!System.IO.File.Exists(OutputCSVFile + ".csv"))
            {
                /* create a new reference file */
                FCalculator.GetResults().WriteCSV(FCalculator.GetParameters(), OutputCSVFile + ".csv");
            }
            else
            {
                FCalculator.GetResults().WriteCSV(FCalculator.GetParameters(), OutputCSVFile + ".csv.new");
            }

            if (!System.IO.File.Exists(OutputCSVFile + ".txt"))
            {
                /* create a new reference file */
                PrintTxt(FCalculator.GetResults(), FCalculator.GetParameters(), OutputCSVFile + ".txt");
            }
            else
            {
                PrintTxt(FCalculator.GetResults(), FCalculator.GetParameters(), OutputCSVFile + ".txt.new");
            }

            if (System.IO.File.Exists(OutputCSVFile + ".csv.new"))
            {
                Assert.AreEqual(true, TTextFile.SameContent(OutputCSVFile + ".csv",
                        OutputCSVFile + ".csv.new"), "the csv files should be the same: " + OutputCSVFile);
                System.IO.File.Delete(OutputCSVFile + ".csv.new");
            }

            if (System.IO.File.Exists(OutputCSVFile + ".txt.new"))
            {
                Assert.AreEqual(true, TTextFile.SameContent(OutputCSVFile + ".txt",
                        OutputCSVFile + ".txt.new"), "the txt files should be the same: " + OutputCSVFile);
                System.IO.File.Delete(OutputCSVFile + ".txt.new");
            }
        }

        /** for testing csv files and printouts
         */
        public void ExportLocalizationReports(String Prefix)
        {
            /* test which csv separator is used, and how dates and currency values are formatted */
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB", false);
            FCalculator.GetParameters().Add("param_currency_format", "CurrencyWithoutDecimals");
            RunLocalizedReport(
                PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + Thread.CurrentThread.CurrentCulture.Name + Prefix +
                "WODecimals");
            FCalculator.GetParameters().Add("param_currency_format", "CurrencyThousands");
            FCalculator.GetParameters().Add("ControlSource", new TVariant("Currency: EUR (in Thousands)"), ReportingConsts.HEADERDESCR2);
            RunLocalizedReport(
                PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + Thread.CurrentThread.CurrentCulture.Name + Prefix +
                "Thousands");
            FCalculator.GetParameters().Add("ControlSource", new TVariant("Currency: EUR"), ReportingConsts.HEADERDESCR2);
            FCalculator.GetParameters().RemoveVariable("param_currency_format");
            RunLocalizedReport(
                PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + Thread.CurrentThread.CurrentCulture.Name + Prefix +
                "Standard");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE", false);
            FCalculator.GetParameters().Add("param_currency_format", "CurrencyWithoutDecimals");
            RunLocalizedReport(
                PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + Thread.CurrentThread.CurrentCulture.Name + Prefix +
                "WODecimals");
            FCalculator.GetParameters().Add("param_currency_format", "CurrencyThousands");
            FCalculator.GetParameters().Add("ControlSource", new TVariant("Currency: EUR (in Thousands)"), ReportingConsts.HEADERDESCR2);
            RunLocalizedReport(
                PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + Thread.CurrentThread.CurrentCulture.Name + Prefix +
                "Thousands");
            FCalculator.GetParameters().Add("ControlSource", new TVariant("Currency: EUR"), ReportingConsts.HEADERDESCR2);
            FCalculator.GetParameters().RemoveVariable("param_currency_format");
            RunLocalizedReport(
                PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + Thread.CurrentThread.CurrentCulture.Name + Prefix +
                "Standard");
            Thread.CurrentThread.CurrentCulture = OrigCulture;
        }

        [Test]
        public void TestLocalizationCurrency()
        {
            String XMLFile;

            FCalculator.ResetParameters();
            XMLFile = PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + "TestStandard_Bal.xml";
            FCalculator.GetParameters().Load(XMLFile);

            if (FCalculator.GenerateResultRemoteClient())
            {
                FCalculator.GetParameters().Save(
                    PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + "LogParametersAfterCalc.log",
                    true);
                FCalculator.GetResults().WriteCSV(
                    FCalculator.GetParameters(), PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + "LogResults.log",
                    "FIND_BEST_SEPARATOR", true);
                ExportLocalizationReports("Bal");
            }
        }

        [Test]
        public void TestLocalizationDates()
        {
            String XMLFile;

            FCalculator.ResetParameters();
            XMLFile = PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + "TestStandard_Acc.xml";
            FCalculator.GetParameters().Load(XMLFile);

            if (FCalculator.GenerateResultRemoteClient())
            {
                FCalculator.GetParameters().Save(
                    PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + "LogParametersAfterCalc.log",
                    true);
                FCalculator.GetResults().WriteCSV(
                    FCalculator.GetParameters(), PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + "LogResults.log",
                    "FIND_BEST_SEPARATOR", true);
                ExportLocalizationReports("Acc");
            }
        }

        [Test]
        public void CheckConfigFile()
        {
            // see also http://nunit.com/blogs/?p=9, How NUnit Finds Config Files
            Assert.AreEqual("demo", System.Configuration.ConfigurationManager.AppSettings["AutoLogin"]);
        }
    }
}