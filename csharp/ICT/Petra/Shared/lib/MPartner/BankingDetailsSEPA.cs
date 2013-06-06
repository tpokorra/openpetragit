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
using System.IO;
using System.Collections.Generic;
using Ict.Common;

namespace Ict.Petra.Shared.MPartner
{
    /// <summary>
    /// useful functions for dealing with bank account numbers (IBAN and BIC) for SEPA.
    /// at the moment, this is only implemented for Germany. Other european countries might be easy to implement as well
    /// </summary>
    public class TBankingDetailsSEPA
    {
        static private SortedList <String, String>BLZToBIC = null;

        /// <summary>
        /// for Germany, download the latest file from http://www.bundesbank.de/Redaktion/DE/Standardartikel/Kerngeschaeftsfelder/Unbarer_Zahlungsverkehr/bankleitzahlen_download.html
        /// see also sample file in csharp/ICT/Testing/lib/MPartner/SampleData/BLZ_sample.txt
        /// </summary>
        public bool InitBLZToBIC(string ABLZFilename)
        {
            BLZToBIC = new SortedList <string, string>();
            List <string>ReplaceLastDigits = new List <string>();

            try
            {
                using (StreamReader sr = new StreamReader(ABLZFilename))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        string BLZ = line.Substring(0, 8);
                        string BIC = line.Substring(139, 11);
                        string MethodCheckAccountNumber = line.Substring(150, 2);

                        if (BIC.Trim().Length == 0)
                        {
                            continue;
                        }

                        if (BLZToBIC.ContainsKey(BLZ))
                        {
                            if (BIC != BLZToBIC[BLZ])
                            {
                                ReplaceLastDigits.Add(BLZ);
                            }
                        }
                        else
                        {
                            BLZToBIC.Add(BLZ, BIC);
                        }
                    }

                    sr.Close();
                }

                // see http://de.wikipedia.org/wiki/ISO_9362#Aufbau
                // only 8 digits are really important, the last 3 identify the branch of the bank
                foreach (string s in ReplaceLastDigits)
                {
                    string BIC = BLZToBIC[s];
                    BLZToBIC[s] = BIC.Substring(0, BIC.Length - 3) + "XXX";
                }

                return true;
            }
            catch (Exception e)
            {
                TLogging.Log("problems opening BLZ file: " + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// convert a old bank sort code to the correct BIC code
        /// </summary>
        public string ConvertToBIC(string ACountryCode, string ABankSortCode)
        {
            if (ACountryCode != "DE")
            {
                return "NOTSUPPORTED";
            }

            if (BLZToBIC == null)
            {
                throw new Exception("need to load BLZ file first!");
            }

            if (!BLZToBIC.ContainsKey(ABankSortCode))
            {
                return "UNKNOWNBANK";
            }

            return BLZToBIC[ABankSortCode];
        }

        /// <summary>
        /// convert the old bank account number to the new IBAN code
        /// </summary>
        public string ConvertToIBAN(string ACountryCode, string ABankSortCode, string AAccountNumber)
        {
            if (ACountryCode != "DE")
            {
                return "NOTSUPPORTED";
            }

            // TODO validate the account number with check digit
            // see also http://www.bundesbank.de/Navigation/DE/Kerngeschaeftsfelder/Unbarer_Zahlungsverkehr/Pruefzifferberechnung/pruefzifferberechnung.html

            // see http://www.pruefziffernberechnung.de/Originaldokumente/IBAN/Prufziffer_07.00.pdf

            Int64 SortCode = Convert.ToInt64(ABankSortCode);
            Int64 AccountNumber = Convert.ToInt64(AAccountNumber);

            Int32 checkDigits = 98 - Convert.ToInt32((Convert.ToDecimal(SortCode.ToString("00000000") +
                                                          AccountNumber.ToString("0000000000") + "131400") % 97));

            return "DE" + checkDigits.ToString("00") + SortCode.ToString("00000000") +
                   AccountNumber.ToString("0000000000");
        }
    }
}