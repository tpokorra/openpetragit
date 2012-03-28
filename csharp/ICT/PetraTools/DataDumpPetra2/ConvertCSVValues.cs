﻿//
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
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Threading;
using Ict.Common;
using ICSharpCode.SharpZipLib.GZip;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// parse the dump file from Progress which is basically a CSV file
    /// </summary>
    public class TParseProgressCSV
    {
        private static Encoding ProgressFileEncoding;

        /// <summary>
        /// init the codepage/encoding for the Progress CSV files
        /// </summary>
        public static void InitProgressCodePage()
        {
            string ProgressCodepage =
                TAppSettingsManager.GetValue("CodePage", Environment.GetEnvironmentVariable("PROGRESS_CP"));

            try
            {
                ProgressFileEncoding = Encoding.GetEncoding(Convert.ToInt32(ProgressCodepage));
            }
            catch
            {
                ProgressFileEncoding = Encoding.GetEncoding(ProgressCodepage);
            }
        }

        /// <summary>
        /// parse a CSV file that was dumped by Progress.
        /// that is the fastest way of dumping the data, but it is not ready for being imported into PostgreSQL.
        /// </summary>
        /// <param name="AInputFileDGz">the path to a gzipped csv file</param>
        /// <param name="AColumnCount">for checking the number of columns</param>
        public static List <string[]>ParseFile(string AInputFileDGz, int AColumnCount)
        {
            System.IO.Stream fs = new FileStream(AInputFileDGz, FileMode.Open, FileAccess.Read);
            GZipInputStream gzipStream = new GZipInputStream(fs);
            StreamReader MyReader = new StreamReader(gzipStream, ProgressFileEncoding);

            List <string[]>Result = new List <string[]>();

            int RealLineCounter = 0;

            string OrigLine = string.Empty;

            try
            {
                while (!MyReader.EndOfStream)
                {
                    OrigLine = MyReader.ReadLine();

                    if (OrigLine == ".")
                    {
                        // we have parsed all the data
                        MyReader.ReadToEnd();

                        break;
                    }

                    StringBuilder line = new StringBuilder(OrigLine);

                    int ColumnCounter = 0;

                    if (TLogging.DebugLevel == 10)
                    {
                        TLogging.Log("parsing line: " + line.ToString());
                    }

                    RealLineCounter++;

                    string[] NewLine = new string[AColumnCount];

                    while (line.Length > 0)
                    {
                        string val = string.Empty;

                        if (line[0] == '"')
                        {
                            bool AcrossSeveralLines = false;

                            do
                            {
                                if (AcrossSeveralLines)
                                {
                                    line.Append("\n").Append(MyReader.ReadLine());

                                    if (TLogging.DebugLevel == 10)
                                    {
                                        TLogging.Log("adding next line: " + line.ToString());
                                    }

                                    RealLineCounter++;
                                }

                                try
                                {
                                    val = StringHelper.GetNextCSV(ref line, ' ');
                                    AcrossSeveralLines = false;
                                }
                                catch (System.IndexOutOfRangeException)
                                {
                                    // the current data row is across several lines
                                    AcrossSeveralLines = true;
                                }
                            } while (AcrossSeveralLines);

                            // double quotes have been escaped by two double quotes
                            val = val.Replace("\"\"", "\"");

                            val = val.Replace("\n", "\\n").Replace("\r", "\\r");
                        }
                        else
                        {
                            val = StringHelper.GetNextCSV(ref line, ' ');

                            if (val == "?")
                            {
                                // NULL
                                val = "\\N";
                            }
                        }

                        if (TLogging.DebugLevel == 10)
                        {
                            TLogging.Log("Parsed value: " + val);
                        }

                        if (ColumnCounter >= AColumnCount)
                        {
                            TLogging.Log(OrigLine);
                            throw new Exception(
                                String.Format("Line {0}: Invalid number of columns, should be only {1} but there are more columns.",
                                    RealLineCounter,
                                    AColumnCount));
                        }

                        NewLine[ColumnCounter] = val;
                        ColumnCounter++;
                    }

                    if (ColumnCounter != AColumnCount)
                    {
                        TLogging.Log(OrigLine);
                        throw new Exception(
                            String.Format("Line {0}: Invalid number of columns, should be {1} but there are only {2} columns.",
                                RealLineCounter,
                                AColumnCount,
                                ColumnCounter));
                    }

                    Result.Add(NewLine);

                    if ((TLogging.DebugLevel > 0) && (RealLineCounter % 500000 == 0))
                    {
                        TLogging.Log(RealLineCounter.ToString() + " " + (GC.GetTotalMemory(false) / 1024 / 1024).ToString() + " MB");
                    }
                }
            }
            catch (Exception e)
            {
                TLogging.Log(OrigLine);
                TLogging.Log("Problem parsing file, in line " + RealLineCounter.ToString());
                throw e;
            }

            return Result;
        }

        private static string ReplaceKommaQuotes(string InputString)
        {
            int startCopyIndex = 0;
            string lineWithoutDoubleQuotes = "";

            int StringLength = InputString.Length - 1;
            bool IsString = false;

            for (int Counter = 0; Counter < StringLength; ++Counter)
            {
                if (InputString[Counter] == '\"')
                {
                    if (!IsString)
                    {
                        IsString = true;

                        if ((Counter > 0)
                            && (InputString[Counter - 1] == ','))
                        {
                            // We have ," replace it with KOMMAQUOTES%%
                            lineWithoutDoubleQuotes = lineWithoutDoubleQuotes +
                                                      InputString.Substring(startCopyIndex, Counter - 1 - startCopyIndex) +
                                                      "KOMMAQUOTES%%";
                            startCopyIndex = Counter + 1;
                        }
                    }
                    else
                    {
                        // check if " is end of string or part of string
                        if (InputString[Counter + 1] == '\"')
                        {
                            // We have a double quote as part of the string
                            Counter++;
                        }
                        else
                        {
                            IsString = false;
                        }
                    }
                }
            }

            lineWithoutDoubleQuotes = lineWithoutDoubleQuotes +
                                      InputString.Substring(startCopyIndex, InputString.Length - startCopyIndex);

            return lineWithoutDoubleQuotes;
        }
    }
}