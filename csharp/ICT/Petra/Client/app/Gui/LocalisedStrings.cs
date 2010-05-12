//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using System.Windows.Forms;

namespace Ict.Petra.Client.App.Gui
{
    /// <summary>
    /// Contains functions and procedures that return Localised Strings for the User Interface.
    /// </summary>
    public class LocalisedStrings
    {
        /// <summary>todoComment</summary>
        public const String COUNTY_DEFAULT_LABEL = "County/St&ate:";

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ALabelText"></param>
        /// <param name="AName"></param>
        public static void GetLocStrCounty(out String ALabelText, out String AName)
        {
            String LocalisedCountyLabel;

            LocalisedCountyLabel = TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_LOCALISEDCOUNTYLABEL);

            if (LocalisedCountyLabel != "")
            {
                if (!LocalisedCountyLabel.EndsWith(":"))
                {
                    LocalisedCountyLabel = LocalisedCountyLabel + ':';
                }

                ALabelText = LocalisedCountyLabel;
            }
            else
            {
                ALabelText = COUNTY_DEFAULT_LABEL;
            }

            // Remove & and : from the LabelText to get the 'Name' of the field
            AName = ALabelText.Replace("&", "");
            AName = AName.Replace(":", "");
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ALabel"></param>
        /// <param name="AToolTip"></param>
        /// <param name="AName"></param>
        public static void GetLocStrBankBranchCode(out String ALabel, out String AToolTip, out String AName)
        {
            String LocalisedBranchCodeAndLabel;

            LocalisedBranchCodeAndLabel = TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_LOCALISEDBRANCHCODEANDLABEL, "");

            if ((LocalisedBranchCodeAndLabel != "") && (LocalisedBranchCodeAndLabel != "|"))
            {
                ALabel = LocalisedBranchCodeAndLabel.Split(new Char[] { ('|') })[0];

                if (!ALabel.EndsWith(":"))
                {
                    ALabel = ALabel + ':';
                }

                AToolTip = LocalisedBranchCodeAndLabel.Split(new Char[] { ('|') })[1];
            }
            else
            {
                ALabel = "Ban&k/Branch Code:";
                AToolTip = "";
            }

            // Remove & and : from the LabelText to get the 'Name' of the field
            AName = ALabel.Replace("&", "");
            AName = AName.Replace(":", "");
        }
    }
}