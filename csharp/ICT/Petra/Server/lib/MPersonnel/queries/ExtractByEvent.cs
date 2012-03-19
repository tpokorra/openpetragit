//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2011 by OM International
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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MCommon.queries;
using Ict.Petra.Server.MPartner.Extracts;

namespace Ict.Petra.Server.MPersonnel.queries
{
    /// <summary>
    /// this report is quite simple, and should be used as an example for more complex reports and extracts
    /// </summary>
    public class QueryPartnerByEvent : Ict.Petra.Server.MCommon.queries.ExtractQueryBase
    {
        /// <summary>
        /// calculate an extract from a report: all partners living in a given city
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            string SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractPartnerByEvent.sql");

            // create a new object of this class and control extract calculation from base class
            QueryPartnerByEvent ExtractQuery = new QueryPartnerByEvent();

            return ExtractQuery.CalculateExtractInternal(AParameters, SqlStmt, AResults);
        }

        /// <summary>
        /// retrieve parameters from client sent in AParameters and build up AParameterList to run SQL query
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ASQLParameterList"></param>
        protected override void RetrieveParameters(TParameterList AParameters, ref TSelfExpandingArrayList ASQLParameterList)
        {
            // prepare list of selected events
            List <String>param_events = new List <String>();

            foreach (TVariant choice in AParameters.Get("param_events").ToComposite())
            {
                param_events.Add(choice.ToString());
            }

            if (param_events.Count == 0)
            {
                throw new NoNullAllowedException("At least one option must be checked.");
            }

            // now add parameters to sql parameter list
            ASQLParameterList.Add(TDbListParameterValue.OdbcListParameterValue("events", OdbcType.BigInt, param_events));
            ASQLParameterList.Add(new OdbcParameter("Accepted", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_status_accepted").ToBool()
                });
            ASQLParameterList.Add(new OdbcParameter("Hold", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_status_hold").ToBool()
                });
            ASQLParameterList.Add(new OdbcParameter("Enquiry", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_status_enquiry").ToBool()
                });
            ASQLParameterList.Add(new OdbcParameter("Cancelled", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_status_cancelled").ToBool()
                });
            ASQLParameterList.Add(new OdbcParameter("Rejected", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_status_rejected").ToBool()
                });
            ASQLParameterList.Add(new OdbcParameter("Active", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_active_partners").ToBool()
                });
            ASQLParameterList.Add(new OdbcParameter("Exclude_no_soliciations", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_exclude_no_solicitations").ToBool()
                });
        }
    }
}