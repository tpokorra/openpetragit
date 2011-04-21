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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// functions for creating new partners and to edit partners
    /// </summary>
    public class TSimplePartnerEditWebConnector
    {
        /// <summary>
        /// get a partner key for a new partner
        /// </summary>
        /// <param name="AFieldPartnerKey">can be -1, then the default site key is used</param>
        [RequireModulePermission("PTNRUSER")]
        public static Int64 NewPartnerKey(Int64 AFieldPartnerKey)
        {
            Int64 NewPartnerKey = TNewPartnerKey.GetNewPartnerKey(AFieldPartnerKey);

            TNewPartnerKey.SubmitNewPartnerKey(NewPartnerKey - NewPartnerKey % 1000000, NewPartnerKey, ref NewPartnerKey);
            return NewPartnerKey;
        }

        /// <summary>
        /// return the existing data of a partner
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerEditTDS GetPartnerDetails(Int64 APartnerKey, bool AWithAddressDetails, bool AWithSubscriptions, bool AWithRelationships)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            PPartnerAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);

            if (MainDS.PPartner.Rows.Count == 0)
            {
                return null;
            }

            switch (MainDS.PPartner[0].PartnerClass)
            {
                case MPartnerConstants.PARTNERCLASS_FAMILY:
                    PFamilyAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                    break;

                case MPartnerConstants.PARTNERCLASS_PERSON:
                    PPersonAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                    break;

                case MPartnerConstants.PARTNERCLASS_CHURCH:
                    PChurchAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                    break;

                case MPartnerConstants.PARTNERCLASS_ORGANISATION:
                    POrganisationAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                    break;
            }

            if (AWithAddressDetails)
            {
                PPartnerLocationAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                PLocationAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
            }

            if (AWithRelationships)
            {
                PPartnerRelationshipAccess.LoadViaPPartnerPartnerKey(MainDS, APartnerKey, Transaction);
            }

            if (AWithSubscriptions)
            {
                PSubscriptionAccess.LoadViaPPartnerPartnerKey(MainDS, APartnerKey, Transaction);
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            return null;
        }

        /// <summary>
        /// store the currently edited partner
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool SavePartner(PartnerEditTDS AMainDS, out TVerificationResultCollection AVerificationResult)
        {
            if (!PAcquisitionAccess.Exists(MPartnerConstants.PARTNERIMPORT_AQUISITION_DEFAULT, null))
            {
                PAcquisitionTable AcqTable = new PAcquisitionTable();
                PAcquisitionRow row = AcqTable.NewRowTyped();
                row.AcquisitionCode = MPartnerConstants.PARTNERIMPORT_AQUISITION_DEFAULT;
                row.AcquisitionDescription = Catalog.GetString("Imported Data");
                AcqTable.Rows.Add(row);

                TVerificationResultCollection VerificationResult;
                TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                PAcquisitionAccess.SubmitChanges(AcqTable, Transaction, out VerificationResult);

                DBAccess.GDBAccessObj.CommitTransaction();
            }

            return PartnerEditTDSAccess.SubmitChanges(AMainDS, out AVerificationResult) == TSubmitChangesResult.scrOK;
        }
    }
}