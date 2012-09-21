//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, jomammele
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
using System.Threading;
using System.Diagnostics;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;
using GuidedTranslation;

namespace Ict.Tools.GuidedTranslation
{
    class Program
    {
        public static void Main(string[] args)
        {
            new TAppSettingsManager(false);

            //enable execution without nant -> debugging easier
            bool independentmode = false;

            if (independentmode)
            {
                TLogging.Log("started GuidedTranslations in independent mode");
            }

            try
            {
                //if ((TAppSettingsManager.HasValue("do") && (TAppSettingsManager.GetValue("do") == "processpot")))
                if (true)
                {
                    string poFilePath;

                    if (independentmode)
                    {
                        poFilePath = "D:\\openpetra\\bzr\\work-improvetranslations\\i18n\\template.pot";
                    }
                    else
                    {
                        poFilePath = TAppSettingsManager.GetValue("poFile");
                    }

                    // start analysing all items
                    TProcessPot.ProcessPot(poFilePath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Environment.Exit(-1);
            }
        }
    }
}