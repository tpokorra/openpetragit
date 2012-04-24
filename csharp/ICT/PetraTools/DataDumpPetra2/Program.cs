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
using System.IO;
using Ict.Common;

namespace Ict.Tools.DataDumpPetra2
{
/// <summary>
/// main class
/// </summary>
    public class TMain
    {
        /// <summary>
        /// main function
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            new TAppSettingsManager(false);
            new TLogging("Ict.Tools.DataDumpPetra2.log");

            if (!TAppSettingsManager.HasValue("debuglevel"))
            {
                Console.Error.WriteLine("dumps one single table or all tables from Progress Petra 2.3 into Postgresql SQL load format");
                Console.Error.WriteLine(
                    "usage: Ict.Tools.DataDumpPetra2 -debuglevel:<0..10> -table:<single table or all> -oldpetraxml:<path and filename of old petra.xml> -newpetraxml:<path and filename of petra.xml>");
                Console.Error.WriteLine("will default to processing all tables, and using petra23.xml and petra.xml from the current directory");
                Console.Error.WriteLine("");
            }

            try
            {
                TLogging.DebugLevel = TAppSettingsManager.GetInt16("debuglevel", 0);

                if (TAppSettingsManager.GetValue("operation", false) == "createProgressCode")
                {
                    TCreateFulldumpProgressCode createProgressCode = new TCreateFulldumpProgressCode();
                    createProgressCode.GenerateFulldumpCode();
                    return;
                }

                if (TAppSettingsManager.GetValue("clean", "false") == "true")
                {
                    TLogging.Log("deleting all resulting files...");

                    // delete sql.gz files, also _*.txt
                    string[] FilesToDelete = Directory.GetFiles(TAppSettingsManager.GetValue("fulldumpPath", "fulldump"), "*.sql.gz");

                    foreach (string file in FilesToDelete)
                    {
                        File.Delete(file);
                    }

                    FilesToDelete = Directory.GetFiles(TAppSettingsManager.GetValue("fulldumpPath", "fulldump"), "_*.txt");

                    foreach (string file in FilesToDelete)
                    {
                        File.Delete(file);
                    }
                }

                string table = TAppSettingsManager.GetValue("table", "");

                // the upgrade process is split into two steps, to make testing quicker

                // Step 1: dump from Progress Petra 2.3 to CSV files, write gz files to keep size of fulldump small
                // this takes about 45 minutes for the german database
                // just use the simple fulldump.p from progress/petra/compiler (slightly modified, fulldump23.p)
                if ((TAppSettingsManager.GetValue("operation", "dump23") == "dump23") && File.Exists("fulldumpPetra23.r"))
                {
                    TDumpProgressToPostgresql dumper = new TDumpProgressToPostgresql();
                    dumper.DumpTablesToCSV(table);
                }

                // Step 2: produce one or several sql load files for PostgreSQL
                // can be called independant from first step: for all tables or just one table
                // for tables merged into one: append to previous file
                if (TAppSettingsManager.GetValue("operation", "load30") == "load30")
                {
                    TDumpProgressToPostgresql dumper = new TDumpProgressToPostgresql();
                    dumper.LoadTablesToPostgresql(table);
                }

                // Step 3: concatenate all existing sql.gz files into one load sql file, gzipped. in the correct order
                if (TAppSettingsManager.GetValue("operation", "createSQL") == "createSQL")
                {
                    TDumpProgressToPostgresql dumper = new TDumpProgressToPostgresql();
                    dumper.CreateNewSQLFile(table);
                }

                // TODO: also anonymize the names of the partners (use random names from external list of names)? what about amounts?
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);

                if (e.InnerException != null)
                {
                    TLogging.Log(e.InnerException.Message);
                }

                TLogging.Log(e.StackTrace);
            }
        }
    }
}