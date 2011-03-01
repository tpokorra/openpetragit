﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Shared.MConference;

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    /// <summary>
    /// Description of TFrmSendingFieldReports.ManualCode.
    /// </summary>
    public class TFrmSendingFieldReport : TFrmFieldReports
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AParentFormHandle"></param>
        public TFrmSendingFieldReport(IntPtr AParentFormHandle) : base(AParentFormHandle)
        {
            this.Text = Catalog.GetString("Sending Field Report");
            BaseGrpChargedFields.Visible = false;

            SetReportParameters("Conference\\\\sendingfieldreport.xml,Conference\\\\conference.xml",
                "Sending Field Report");

            grdFields_InitialiseData(TUnitTypeEnum.utSendingFields);
        }
    }
}