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
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core;

namespace Ict.Testing.SampleDataConstructor
{
    /// <summary>
    /// tools for generating organisation partner records
    /// </summary>
    public class SampleDataOrganisations
    {
        /// <summary>
        /// generate the partners from a text file that was generated with Benerator
        /// </summary>
        /// <param name="AInputBeneratorFile"></param>
        public static void GenerateOrganisationPartners(string AInputBeneratorFile)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();

            XmlDocument doc = TCsv2Xml.ParseCSV2Xml(AInputBeneratorFile, ",", Encoding.UTF8);

            XmlNode RecordNode = doc.FirstChild.NextSibling.FirstChild;

            while (RecordNode != null)
            {
                long OrgPartnerKey = TNewPartnerKey.GetNewPartnerKey(-1);

                if (!TNewPartnerKey.SubmitNewPartnerKey(DomainManager.GSiteKey,
                        OrgPartnerKey, ref OrgPartnerKey))
                {
                    throw new Exception("create organisation: problems getting a new partner key");
                }

                POrganisationRow organisationRecord = MainDS.POrganisation.NewRowTyped();
                organisationRecord.PartnerKey = OrgPartnerKey;
                organisationRecord.OrganisationName = TXMLParser.GetAttribute(RecordNode, "OrganisationName");
                MainDS.POrganisation.Rows.Add(organisationRecord);

                PPartnerRow PartnerRow = MainDS.PPartner.NewRowTyped();
                PartnerRow.PartnerKey = organisationRecord.PartnerKey;
                PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_ORGANISATION;
                PartnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;
                PartnerRow.PartnerShortName = organisationRecord.OrganisationName;
                PartnerRow.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_ORGANISATION;
                MainDS.PPartner.Rows.Add(PartnerRow);

                PLocationRow locationRow = MainDS.PLocation.NewRowTyped();

                locationRow.SiteKey = DomainManager.GSiteKey;
                locationRow.LocationKey = (MainDS.PLocation.Count + 1) * -1;
                locationRow.StreetName = TXMLParser.GetAttribute(RecordNode, "Addr2");
                locationRow.PostalCode = TXMLParser.GetAttribute(RecordNode, "PostCode");
                locationRow.City = TXMLParser.GetAttribute(RecordNode, "City");
                locationRow.County = TXMLParser.GetAttribute(RecordNode, "Province");
                locationRow.CountryCode = TXMLParser.GetAttribute(RecordNode, "CountryCode");

                MainDS.PLocation.Rows.Add(locationRow);

                PPartnerLocationRow organisationLocationRow = MainDS.PPartnerLocation.NewRowTyped();

                organisationLocationRow.PartnerKey = PartnerRow.PartnerKey;
                organisationLocationRow.LocationKey = locationRow.LocationKey;
                organisationLocationRow.SiteKey = locationRow.SiteKey;
                organisationLocationRow.LocationType = MPartnerConstants.LOCATIONTYPE_BUSINESS;
                organisationLocationRow.SendMail = true;

                //organisationLocationRow.EmailAddress = TXMLParser.GetAttribute(RecordNode, "Email");

                MainDS.PPartnerLocation.Rows.Add(organisationLocationRow);

                RecordNode = RecordNode.NextSibling;
            }

            TVerificationResultCollection VerificationResult;
            PartnerEditTDSAccess.SubmitChanges(MainDS, out VerificationResult);

            if (VerificationResult.HasCriticalOrNonCriticalErrors)
            {
                throw new Exception(VerificationResult.BuildVerificationResultString());
            }
        }
    }
}