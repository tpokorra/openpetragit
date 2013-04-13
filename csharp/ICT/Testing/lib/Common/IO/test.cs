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
using System.Runtime.InteropServices;
using NUnit.Framework;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Xml;
using System.Text;
using System.IO;
using Ict.Common;
using Ict.Common.IO;
using Jayrock.Json;
using OfficeOpenXml;
using ICSharpCode.SharpZipLib.Zip;

namespace Ict.Common.IO.Testing
{
    ///  This is a testing program for Ict.Common.IO.dll
    [TestFixture]
    public class TTestCommonIO
    {
        string PathToTestData;

        /// init
        [SetUp]
        public void Init()
        {
            new TLogging("test.log");
            new TAppSettingsManager("../../etc/TestClient.config");
            TLogging.DebugLevel = TAppSettingsManager.GetInt16("Client.DebugLevel", 0);

            PathToTestData = "../../csharp/ICT/Testing/lib/Common/IO/TestData/".Replace("/", System.IO.Path.DirectorySeparatorChar.ToString());
        }

        private XmlDocument CreateTestDoc()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);

            doc.InsertBefore(xmlDeclaration, doc.DocumentElement);

            // it is necessary to have this root node without attributes, to comply with the xml documents generated by TYML2XML
            XmlElement rootNode = doc.CreateElement(TYml2Xml.ROOTNODEINTERNAL);
            doc.AppendChild(rootNode);

            XmlElement childNode = doc.CreateElement(TYml2Xml.XMLELEMENT);
            childNode.SetAttribute("name", "testname");
            childNode.SetAttribute("active", true.ToString());
            rootNode.AppendChild(childNode);

            XmlElement anotherChildNode = doc.CreateElement(TYml2Xml.XMLELEMENT);
            anotherChildNode.SetAttribute("name", "testname2");
            anotherChildNode.SetAttribute("active", true.ToString());
            rootNode.AppendChild(anotherChildNode);

            XmlElement grandChildNode = doc.CreateElement(TYml2Xml.XMLELEMENT);
            grandChildNode.SetAttribute("name", "grandchild1");
            grandChildNode.SetAttribute("active", true.ToString());
            childNode.AppendChild(grandChildNode);

            XmlElement grandChild2Node = doc.CreateElement(TYml2Xml.XMLELEMENT);
            grandChild2Node.SetAttribute("name", "grandchild2");
            grandChild2Node.SetAttribute("active", false.ToString());
            childNode.AppendChild(grandChild2Node);

            return doc;
        }

        /// test the xml writer
        [Test]
        public void TestXmlWriter()
        {
            XmlDocument doc = CreateTestDoc();

            // first see if the xml file is still the same
            string filename = PathToTestData + "test.xml";
            StreamWriter sw = new StreamWriter(filename + ".new");

            sw.Write(TXMLParser.XmlToString(doc));
            sw.Close();
            Assert.AreEqual(true, TTextFile.SameContent(filename,
                    filename + ".new"), "the files should be the same: " + filename);
            System.IO.File.Delete(filename + ".new");
        }

        /// test the yml writer
        [Test]
        public void TestYmlWriter()
        {
            XmlDocument doc = CreateTestDoc();

            // now test the yml file
            string filename = PathToTestData + "test.yml";

            TYml2Xml.Xml2Yml(doc, filename + ".new");
            Assert.AreEqual(true, TTextFile.SameContent(filename,
                    filename + ".new"), "the files should be the same: " + filename);
            System.IO.File.Delete(filename + ".new");
        }

        /// <summary>
        /// test reading and writing a yml file
        /// </summary>
        [Test]
        public void TestReadAndWriteYaml()
        {
            // parse a winforms yaml file, write it again, check if the same

            string filename = PathToTestData + "testReadWrite.yml";
            TYml2Xml converterYml = new TYml2Xml(filename);
            XmlDocument docFromYML = converterYml.ParseYML2XML();

            TYml2Xml.Xml2Yml(docFromYML, filename + ".new");
            Assert.AreEqual(true, TTextFile.SameContent(filename,
                    filename + ".new"), "the files should be the same: " + filename);
            System.IO.File.Delete(filename + ".new");
        }

        /// test the csv writer
        [Test]
        public void TestCSVWriter()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            XmlDocument doc = CreateTestDoc();
            // now test the csv file
            string filename = PathToTestData + "test.csv";

            TCsv2Xml.Xml2Csv(doc, filename + ".new");
            Assert.AreEqual(true, TTextFile.SameContent(filename,
                    filename + ".new"), "the files should be the same: " + filename);
            System.IO.File.Delete(filename + ".new");
        }

        /// test the csv parser
        [Test]
        public void TestCSVParser()
        {
            CreateTestDoc();

            // load from csv, is it the same xml code?
            string filename = PathToTestData + "test.csv";
            XmlDocument docFromCSV = TCsv2Xml.ParseCSV2Xml(filename);

            filename = PathToTestData + "test.xml";
            StreamWriter sw = new StreamWriter(filename + ".new");
            sw.Write(TXMLParser.XmlToString(docFromCSV, true));
            sw.Close();
            Assert.AreEqual(true, TTextFile.SameContent(filename,
                    filename + ".new"), "after importing from csv: the files should be the same: " + filename);
            System.IO.File.Delete(filename + ".new");
        }

        /// test the yml parser
        [Test]
        public void TestYMLParser()
        {
            CreateTestDoc();
            // load from yml, is it the same xml code?
            string filename = PathToTestData + "test.yml";
            TYml2Xml converterYml = new TYml2Xml(filename);
            XmlDocument docFromYML = converterYml.ParseYML2XML();

            filename = PathToTestData + "testWithInheritedAttributes.xml";
            StreamWriter sw = new StreamWriter(filename + ".new");
            sw.Write(TXMLParser.XmlToString(docFromYML, true));
            sw.Close();
            Assert.AreEqual(true, TTextFile.SameContent(filename,
                    filename + ".new"), "after importing from yml: the files should be the same: " + filename);
            System.IO.File.Delete(filename + ".new");
        }

        /// tests for yml with back slashes
        [Test]
        public void TestYMLBackSlashValue()
        {
            string filename = PathToTestData + "testBackslash.yml";
            string backslashValue = "data\\Kontoauszuege\\test.csv";
            TYml2Xml converterYml = new TYml2Xml(filename);
            XmlDocument docFromYML = converterYml.ParseYML2XML();

            XmlNode node = TXMLParser.FindNodeRecursive(docFromYML.DocumentElement, "testname");

            Assert.AreEqual(backslashValue, TXMLParser.GetAttribute(node, "Filename"));

            // does writing of the backslash value work as well?
            TXMLParser.SetAttribute(node, "Filename", backslashValue);
            TYml2Xml.Xml2Yml(docFromYML, filename + ".new");

            Assert.AreEqual(true, TTextFile.SameContent(filename,
                    filename + ".new"), "the files should be the same: " + filename);
            System.IO.File.Delete(filename + ".new");
        }

        /// test zipping strings
        [Test]
        public void TestCompressingString()
        {
            string testText = "<test>blablablablabla</test>";

            string compressed = PackTools.ZipString(testText);

            Assert.AreEqual(testText, PackTools.UnzipString(compressed)[0], "compressing a string");
        }

        /// test the template engine
        [Test]
        public void TestTemplateEngine()
        {
            ProcessTemplate template = new ProcessTemplate();

            template.FTemplateCode = new StringBuilder("my test" + Environment.NewLine +
                "{#IFDEF TEST1}" + Environment.NewLine +
                "test1" + Environment.NewLine +
                "{#ENDIF TEST1}" + Environment.NewLine);

            template.SetCodelet("TEST1", "test");

            Assert.AreEqual("my test" + Environment.NewLine + "test1" + Environment.NewLine,
                template.FinishWriting(true),
                "TEST1");

            template = new ProcessTemplate();
            template.FTemplateCode = new StringBuilder("my test" + Environment.NewLine +
                "{#IFNDEF TEST2}" + Environment.NewLine +
                "test2" + Environment.NewLine +
                "{#ENDIFN TEST2}" + Environment.NewLine);

            template.SetCodelet("TEST2", "test");

            Assert.AreEqual("my test" + Environment.NewLine,
                template.FinishWriting(true),
                "TEST2");

            template = new ProcessTemplate();
            template.FTemplateCode = new StringBuilder("my test" + Environment.NewLine +
                "{#IFNDEF TEST3}" + Environment.NewLine +
                "test3" + Environment.NewLine +
                "{#ENDIFN TEST3}" + Environment.NewLine +
                "hallo" + Environment.NewLine);

            Assert.AreEqual("my test" + Environment.NewLine +
                "test3" + Environment.NewLine +
                "hallo" + Environment.NewLine,
                template.FinishWriting(true),
                "TEST3");

            template = new ProcessTemplate();
            template.FTemplateCode = new StringBuilder("my test" + Environment.NewLine +
                "{#IFDEF TEST1}" + Environment.NewLine +
                "test1" + Environment.NewLine +
                "{#ENDIF TEST1}" + Environment.NewLine +
                "{#IFNDEF TEST2}" + Environment.NewLine +
                "test2" + Environment.NewLine +
                "{#ENDIFN TEST2}" + Environment.NewLine +
                "{#IFNDEF TEST3}" + Environment.NewLine +
                "test3" + Environment.NewLine +
                "{#ENDIFN TEST3}" + Environment.NewLine +
                "{#IFDEF TEST4}" + Environment.NewLine +
                "test4" + Environment.NewLine +
                "{#ENDIF TEST4}" + Environment.NewLine +
                "hallo" + Environment.NewLine);

            template.SetCodelet("TEST1", "test");
            template.SetCodelet("TEST3", "test");

            Assert.AreEqual("my test" + Environment.NewLine +
                "test1" + Environment.NewLine +
                "test2" + Environment.NewLine +
                "hallo" + Environment.NewLine,
                template.FinishWriting(true),
                "TEST4");

            template = new ProcessTemplate();
            template.FTemplateCode = new StringBuilder("my test" + Environment.NewLine +
                "{#IFDEF TEST5 OR TEST1}" + Environment.NewLine +
                "test5" + Environment.NewLine +
                "{#ENDIF TEST5 OR TEST1}" + Environment.NewLine);

            template.SetCodelet("TEST5", "test");

            Assert.AreEqual("my test" + Environment.NewLine + "test5" + Environment.NewLine,
                template.FinishWriting(true),
                "TEST5");

            template = new ProcessTemplate();
            template.FTemplateCode = new StringBuilder("my test" + Environment.NewLine +
                "{#IFDEF TEST5 AND TEST6}" + Environment.NewLine +
                "test6" + Environment.NewLine +
                "{#ENDIF TEST5 AND TEST6}" + Environment.NewLine);

            template.SetCodelet("TEST6", "test");

            Assert.AreEqual("my test" + Environment.NewLine,
                template.FinishWriting(true),
                "TEST6");

            template = new ProcessTemplate();
            template.FTemplateCode = new StringBuilder("my test" + Environment.NewLine +
                "{#IFDEF TEST6 AND TEST7}" + Environment.NewLine +
                "test7" + Environment.NewLine +
                "{#ENDIF TEST6 AND TEST7}" + Environment.NewLine);

            template.SetCodelet("TEST6", "test");
            template.SetCodelet("TEST7", "test");

            Assert.AreEqual("my test" + Environment.NewLine + "test7" + Environment.NewLine,
                template.FinishWriting(true),
                "TEST7");

            template = new ProcessTemplate();
            template.FTemplateCode = new StringBuilder("{#IFDEF TEST8}" + Environment.NewLine +
                "test8" + Environment.NewLine +
                "{#ENDIF TEST8}" + Environment.NewLine +
                "test8" + Environment.NewLine +
                "{#IFDEF TEST8}" + Environment.NewLine +
                "test8" + Environment.NewLine +
                "{#ENDIF TEST8}" + Environment.NewLine);


            template.SetCodelet("TEST8", "test");

            Assert.AreEqual("test8" + Environment.NewLine + "test8" + Environment.NewLine + "test8" + Environment.NewLine,
                template.FinishWriting(true),
                "TEST8");

            template = new ProcessTemplate();
            template.FTemplateCode = new StringBuilder("{#IFNDEF TEST9}" + Environment.NewLine +
                "test8" + Environment.NewLine +
                "{#ENDIFN TEST9}" + Environment.NewLine +
                "test9" + Environment.NewLine +
                "{#IFNDEF TEST9}" + Environment.NewLine +
                "test8" + Environment.NewLine +
                "{#ENDIFN TEST9}" + Environment.NewLine);


            template.SetCodelet("TEST9", "test");

            Assert.AreEqual("test9" + Environment.NewLine,
                template.FinishWriting(true),
                "TEST9");

            template = new ProcessTemplate();
            template.FTemplateCode = new StringBuilder("{#IFNDEF TEST10}" + Environment.NewLine +
                "test8" + Environment.NewLine +
                "{#ENDIFN TEST10}" + Environment.NewLine +
                "test9" + Environment.NewLine +
                "{#IFDEF TEST10}" + Environment.NewLine +
                "test10" + Environment.NewLine +
                "{#ENDIF TEST10}" + Environment.NewLine);


            template.SetCodelet("TEST10", "test");

            Assert.AreEqual("test9" + Environment.NewLine + "test10" + Environment.NewLine,
                template.FinishWriting(true),
                "TEST10");

            template = new ProcessTemplate();
            template.FTemplateCode = new StringBuilder("{#IFDEF TEST10}" + Environment.NewLine +
                "{#IFDEF TEST11}" + Environment.NewLine +
                "{#IFDEF TEST11}" + Environment.NewLine +
                "test11" + Environment.NewLine +
                "{#ENDIF TEST11}" + Environment.NewLine +
                "{#ENDIF TEST11}" + Environment.NewLine +
                "{#IFNDEF TEST11}" + Environment.NewLine +
                "test10" + Environment.NewLine +
                "{#ENDIFN TEST11}" + Environment.NewLine +
                "{#ENDIF TEST10}" + Environment.NewLine +
                "{#IFNDEF TEST10}" + Environment.NewLine +
                "{#IFNDEF TEST11}" + Environment.NewLine +
                "{#IFDEF TEST11}" + Environment.NewLine +
                "test13" + Environment.NewLine +
                "{#ENDIF TEST11}" + Environment.NewLine +
                "{#ENDIFN TEST11}" + Environment.NewLine +
                "{#ENDIFN TEST10}" + Environment.NewLine +
                "test1" + Environment.NewLine);

            template.SetCodelet("TEST11", "test_11");
            template.SetCodelet("TEST10", "test_10");

            Assert.AreEqual("test11" + Environment.NewLine + "test1" + Environment.NewLine,
                template.FinishWriting(true),
                "TEST11");
        }

        /// test json parser
        [Test]
        public void TestJsonParser()
        {
            string JSONFormData = "{'FirstName':'#FIRSTNAME','LastName':'#LASTNAME','Street':'#STREET'}";

            JSONFormData = JSONFormData.Replace("'", "\"");

            string problem = "My \"Lastname\" with quotes";
            string test = JSONFormData.Replace("#LASTNAME", problem);
            JsonObject obj = TJsonTools.ParseValues(test);
            Assert.AreEqual(problem.Replace("\"", "&quot;"), obj["LastName"].ToString());

            problem = "My \"Lastname\", with comma";
            test = JSONFormData.Replace("#LASTNAME", problem);
            obj = TJsonTools.ParseValues(test);
            Assert.AreEqual(problem.Replace("\"", "&quot;"), obj["LastName"].ToString());

            problem = "My \"Lastname\": with colon";
            test = JSONFormData.Replace("#LASTNAME", problem);
            obj = TJsonTools.ParseValues(test);
            Assert.AreEqual(problem.Replace("\"", "&quot;"), obj["LastName"].ToString());

            problem = "My \"Lastname\",\" with\" quote comma quote";
            test = JSONFormData.Replace("#LASTNAME", problem);
            obj = TJsonTools.ParseValues(test);
            Assert.AreEqual(problem.Replace("\"", "&quot;"), obj["LastName"].ToString());

            // test with more curly brackets, nested lists
            JSONFormData = "{'card-0':{'FirstName':'#FIRSTNAME','LastName':'#LASTNAME'},'card-1':{'Street':'#STREET'}}";
            JSONFormData = JSONFormData.Replace("'", "\"");
            problem = "My \"Lastname\": with nested values and colon";
            test = JSONFormData.Replace("#LASTNAME", problem);
            obj = TJsonTools.ParseValues(test);
            Assert.AreEqual(problem.Replace("\"", "&quot;"), obj["LastName"].ToString());
        }

        /// <summary>
        /// test writing to an Excel file.
        /// currently fails on Windows and on Linux on Dispose: System.NotSupportedException : Stream does not support writing.
        /// solution for the moment: use saving via stream, see TestExcelExportStream
        /// </summary>
        [Test]
        [Ignore("does not work on Windows or Mono. better use TestExcelExportStream")]
        public void TestExcelExportFile()
        {
            string filename = PathToTestData + "test.xlsx";

            // display error messages in english
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("test");

                worksheet.Cells["A1"].Value = "test1";
                worksheet.Cells["B3"].Value = "test2";
                worksheet.Cells["B7"].Value = "test2";

                TLogging.Log("writing to " + filename);

                pck.SaveAs(new FileInfo(filename));
            }

            new ExcelPackage(new FileInfo(filename));
            PackTools.Unzip(PathToTestData + "testUnzip", filename);

            FileInfo f = new FileInfo(PathToTestData + "testUnzip/xl/sharedStrings.xml");
            Assert.AreNotEqual(0, f.Length, "file sharedStrings.xml should not be empty");

            Assert.IsInstanceOf(typeof(ExcelPackage), new ExcelPackage(new FileInfo(filename)), "cannot open excel file");
        }

        /// <summary>
        /// test writing to an Excel file
        /// </summary>
        [Test]
        public void TestExcelExportStream()
        {
            string filename = PathToTestData + "test.xlsx";

            // display error messages in english
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (StreamWriter sw = new StreamWriter(filename))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    using (ExcelPackage pck = new ExcelPackage(m))
                    {
                        ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("test");

                        worksheet.Cells["A1"].Value = "test1";
                        worksheet.Cells["B3"].Value = "test2";
                        worksheet.Cells["B7"].Value = "test2";
                        
                        pck.SaveAs(m);
                    }

                    TLogging.Log("writing to " + filename);

                    m.WriteTo(sw.BaseStream);
                    m.Close();
                    sw.Close();
                }
            }

            new ExcelPackage(new FileInfo(filename));
            PackTools.Unzip(PathToTestData + "testUnzip", filename);

            FileInfo f = new FileInfo(PathToTestData + "testUnzip/xl/sharedStrings.xml");
            Assert.AreNotEqual(0, f.Length, "file sharedStrings.xml should not be empty");

            Assert.IsInstanceOf(typeof(ExcelPackage), new ExcelPackage(new FileInfo(filename)), "cannot open excel file");
        }
    }
}