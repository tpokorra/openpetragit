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
//using System.Drawing;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
//using Ict.Petra.Shared.MConference.Data;
//using Ict.Petra.Shared.MReporting;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
//using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    /// <summary>
    /// manual code for TFrmAgeSummaryReport class
    /// </summary>
    public partial class TFrmAgeSummaryReport
    {
        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            int ColumnCounter = 0;

            ACalc.AddParameter("param_calculation", "Age", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);

            ACalc.AddParameter("param_calculation", "Total", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);

            ACalc.AddParameter("param_calculation", "Female", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);

            ACalc.AddParameter("param_calculation", "Male", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);

            ACalc.AddParameter("param_calculation", "Other", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);

            ACalc.AddParameter("MaxDisplayColumns", ColumnCounter);
        }
    }
}