//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MPartner.Import;
using Ict.Petra.Server.MPartner.Partner;
using Ict.Petra.Server.MPartner.Partner.ServerLookups;

namespace Ict.Petra.Server.MPartner.ImportExport.WebConnectors
{
    /// <summary>
    /// import and export partner data
    /// </summary>
    public class TImportExportWebConnector
    {
        static Int64 NewPartnerKey = -1;

        private static void ParsePartners(ref PartnerImportExportTDS AMainDS, XmlNode ACurNode)
        {
            XmlNode LocalNode = ACurNode;

            while (LocalNode != null)
            {
                if (LocalNode.Name.StartsWith("PartnerGroup"))
                {
                    ParsePartners(ref AMainDS, LocalNode.FirstChild);
                }
                else if (LocalNode.Name.StartsWith("Partner"))
                {
                    PPartnerRow newPartner = AMainDS.PPartner.NewRowTyped();

                    if (!TYml2Xml.HasAttributeRecursive(LocalNode, "SiteKey"))
                    {
                        throw new Exception(Catalog.GetString("Missing SiteKey Attribute"));
                    }

                    if (!TYml2Xml.HasAttributeRecursive(LocalNode, "status"))
                    {
                        throw new Exception(Catalog.GetString("Missing status Attribute"));
                    }

                    // get a new partner key
                    newPartner.PartnerKey = TImportExportWebConnector.NewPartnerKey;
                    TImportExportWebConnector.NewPartnerKey--;

                    if (TYml2Xml.GetAttributeRecursive(LocalNode, "class") == MPartnerConstants.PARTNERCLASS_FAMILY)
                    {
                        PFamilyRow newFamily = AMainDS.PFamily.NewRowTyped();
                        newFamily.PartnerKey = newPartner.PartnerKey;
                        newFamily.FamilyName = TYml2Xml.GetAttributeRecursive(LocalNode, "LastName");
                        newFamily.FirstName = TYml2Xml.GetAttribute(LocalNode, "FirstName");
                        newFamily.Title = TYml2Xml.GetAttribute(LocalNode, "Title");

                        if (TYml2Xml.HasAttribute(LocalNode, "CreatedAt"))
                        {
                            newFamily.DateCreated = Convert.ToDateTime(TYml2Xml.GetAttribute(LocalNode, "CreatedAt"));
                        }

                        AMainDS.PFamily.Rows.Add(newFamily);

                        newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
                        newPartner.AddresseeTypeCode = MPartnerConstants.PARTNERCLASS_FAMILY;

                        newPartner.PartnerShortName =
                            Calculations.DeterminePartnerShortName(newFamily.FamilyName, newFamily.Title, newFamily.FirstName);
                    }

                    if (TYml2Xml.GetAttributeRecursive(LocalNode, "class") == MPartnerConstants.PARTNERCLASS_PERSON)
                    {
                        if (TAppSettingsManager.GetValue("AllowCreationPersonRecords", "false", false).ToLower() != "true")
                        {
                            throw new Exception(
                                "We are currently not supporting import of PERSON records, until we have resolved the issues with household/family. "
                                +
                                "Please add configuration parameter AllowCreationPersonRecords with value true if you want to use PERSON records");
                        }

                        // TODO
                    }
                    else if (TYml2Xml.GetAttributeRecursive(LocalNode, "class") == MPartnerConstants.PARTNERCLASS_ORGANISATION)
                    {
                        // TODO
                    }
                    else
                    {
                        // TODO AVerificationResult add failing problem: unknown partner class
                    }

                    newPartner.StatusCode = TYml2Xml.GetAttributeRecursive(LocalNode, "status");
                    AMainDS.PPartner.Rows.Add(newPartner);

                    // import special types
                    StringCollection SpecialTypes = StringHelper.StrSplit(TYml2Xml.GetAttributeRecursive(LocalNode, "SpecialTypes"), ",");

                    foreach (string SpecialType in SpecialTypes)
                    {
                        PPartnerTypeRow partnertype = AMainDS.PPartnerType.NewRowTyped();
                        partnertype.PartnerKey = newPartner.PartnerKey;
                        partnertype.TypeCode = SpecialType.Trim();
                        AMainDS.PPartnerType.Rows.Add(partnertype);

                        // TODO: check if special type does not exist yet, and create it
                    }

                    // import subscriptions
                    StringCollection Subscriptions = StringHelper.StrSplit(TYml2Xml.GetAttributeRecursive(LocalNode, "Subscriptions"), ",");

                    foreach (string publicationCode in Subscriptions)
                    {
                        PSubscriptionRow subscription = AMainDS.PSubscription.NewRowTyped();
                        subscription.PartnerKey = newPartner.PartnerKey;
                        subscription.PublicationCode = publicationCode.Trim();
                        subscription.ReasonSubsGivenCode = "FREE";
                        AMainDS.PSubscription.Rows.Add(subscription);
                    }

                    // import address
                    XmlNode addressNode = TYml2Xml.GetChild(LocalNode, "Address");

                    if ((addressNode == null) || (TYml2Xml.GetAttributeRecursive(addressNode, "Street").Length == 0))
                    {
                        // add the empty location
                        PPartnerLocationRow partnerlocation = AMainDS.PPartnerLocation.NewRowTyped(true);
                        partnerlocation.SiteKey = 0;
                        partnerlocation.PartnerKey = newPartner.PartnerKey;
                        partnerlocation.DateEffective = DateTime.Now;
                        partnerlocation.LocationType = "HOME";
                        partnerlocation.SendMail = false;
                        partnerlocation.EmailAddress = TYml2Xml.GetAttributeRecursive(addressNode, "Email");
                        partnerlocation.TelephoneNumber = TYml2Xml.GetAttributeRecursive(addressNode, "Phone");
                        partnerlocation.MobileNumber = TYml2Xml.GetAttributeRecursive(addressNode, "MobilePhone");
                        AMainDS.PPartnerLocation.Rows.Add(partnerlocation);
                    }
                    else
                    {
                        // TODO: avoid duplicate addresses, reuse existing locations
                        PLocationRow location = AMainDS.PLocation.NewRowTyped(true);
                        location.LocationKey = (AMainDS.PLocation.Rows.Count + 1) * -1;
                        location.SiteKey = 0;

                        if (!TYml2Xml.HasAttributeRecursive(LocalNode, "Country"))
                        {
                            throw new Exception(Catalog.GetString("Missing Country Attribute"));
                        }

                        location.CountryCode = TYml2Xml.GetAttributeRecursive(addressNode, "Country");
                        location.StreetName = TYml2Xml.GetAttributeRecursive(addressNode, "Street");
                        location.City = TYml2Xml.GetAttributeRecursive(addressNode, "City");
                        location.PostalCode = TYml2Xml.GetAttributeRecursive(addressNode, "PostCode");
                        AMainDS.PLocation.Rows.Add(location);

                        PPartnerLocationRow partnerlocation = AMainDS.PPartnerLocation.NewRowTyped(true);
                        partnerlocation.SiteKey = 0;
                        partnerlocation.LocationKey = location.LocationKey;
                        partnerlocation.PartnerKey = newPartner.PartnerKey;
                        partnerlocation.SendMail = true;
                        partnerlocation.DateEffective = DateTime.Now;
                        partnerlocation.LocationType = "HOME";
                        partnerlocation.EmailAddress = TYml2Xml.GetAttributeRecursive(addressNode, "Email");
                        partnerlocation.TelephoneNumber = TYml2Xml.GetAttributeRecursive(addressNode, "Phone");
                        partnerlocation.MobileNumber = TYml2Xml.GetAttributeRecursive(addressNode, "MobilePhone");
                        AMainDS.PPartnerLocation.Rows.Add(partnerlocation);
                    }
                }

                LocalNode = LocalNode.NextSibling;
            }
        }

        /// <summary>
        /// imports partner data from file
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerImportExportTDS ImportPartnersFromYml(string AXmlPartnerData, out TVerificationResultCollection AVerificationResult)
        {
            return TImportExportYml.ImportPartners(AXmlPartnerData, out AVerificationResult);
        }

        /// <summary>
        /// This imports partners from a CSV file
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerImportExportTDS ImportFromCSVFile(string AXmlPartnerData, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(AXmlPartnerData);

            XmlNode root = doc.FirstChild.NextSibling.FirstChild;

            PartnerImportExportTDS MainDS = TPartnerImportCSV.ImportData(root);

            // TODO: check for updated partners, matching addresses etc.

            return MainDS;
        }

        /// <summary>
        /// This imports partners from a partner extract which is a text file format used already with Petra 2.x
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerImportExportTDS ImportFromPartnerExtract(string[] ATextFileLines, out TVerificationResultCollection AVerificationResult)
        {
            TPartnerFileImport Importer = new TPartnerFileImport();
            PartnerImportExportTDS MainDS = Importer.ImportAllData(ATextFileLines, string.Empty, false, out AVerificationResult);

            return MainDS;
        }
        
        /// <summary>
        /// Web connector for commit changes after importing a partner
        /// </summary>
        /// <param name="MainDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static Boolean CommitChanges (PartnerImportExportTDS MainDS,  out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult Res = PartnerImportExportTDSAccess.SubmitChanges(MainDS, out AVerificationResult);
            return (TSubmitChangesResult.scrOK == Res);
        }

        /// <summary>
        /// return an XmlDocument with all partner info;
        /// the partners are grouped by class, country, status, and sitekey
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static string ExportPartners()
        {
            return TImportExportYml.ExportPartners();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>A string that will form the first two lines of a .ext file</returns>
        [RequireModulePermission("PTNRUSER")]
        public static string GetExtFileHeader ()
        { 
            TPartnerFileExport Exporter = new TPartnerFileExport();
            return Exporter.ExtFileHeader();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>A string that will form the final line of a .ext file</returns>
        [RequireModulePermission("PTNRUSER")]
        public static string GetExtFileFooter ()
        {
            return "0  \"FINISH\"\n";
        }
        
       /// <summary>
       /// Format a partner as ext (Petra 2.x format)
       /// If I've been asked to export a PERSON, I can also export the FAMILY record first.
       /// </summary>
       /// <param name="APartnerKey">Partner key</param>
       /// <param name="ASiteKey">Partner's site key</param>
       /// <param name="ALocationKey">Partner's primary location key</param>
       /// <param name="ANoFamily">Set this flag for a PERSON, to prevent the FAMILY being exported too.</param>
       /// <param name="ASpecificBuildingInfo">Only include these buildings (null for all)</param>
       /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static string ExportPartnerExt(Int64 APartnerKey, Int32 ASiteKey, Int32 ALocationKey, Boolean ANoFamily, StringCollection ASpecificBuildingInfo)
        {
            String extRecord = "";
            //
            // First I'm going to check that I can access this partner OK..
            Boolean PartnerAccessOk = false;
            String ShortName;
            TPartnerClass PartnerClass;
            Boolean IsMergedPartner = false;
            Boolean UserCanAccessPartner = false;
            
            if (APartnerKey != 0)
            {
                PartnerAccessOk = TPartnerServerLookups.VerifyPartner(APartnerKey,
                        out ShortName, out PartnerClass, 
                        out IsMergedPartner, out UserCanAccessPartner);
            }
            if (!PartnerAccessOk || !UserCanAccessPartner)
            {
                return extRecord;  // This is empty - TODO: I'm not returning any error code here.
            }
            
            TPartnerFileExport Exporter = new TPartnerFileExport();
            PartnerImportExportTDS AMainDS = TExportAllPartnerData.ExportPartner(APartnerKey);
            
            if (!ANoFamily)  // I'll check whether there's a FAMILY to go with this Partner.
            {
                PPartnerRow PartnerRow = AMainDS.PPartner[0];
                if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                {
                    PPersonRow PersonRow = AMainDS.PPerson[0];
                    long FamilyKey = PersonRow.FamilyKey;
                    PartnerAccessOk = TPartnerServerLookups.VerifyPartner(FamilyKey,
                            out ShortName, out PartnerClass, 
                            out IsMergedPartner, out UserCanAccessPartner);
                    if ((FamilyKey > 0) && PartnerAccessOk  && UserCanAccessPartner)
                    {
                        PartnerImportExportTDS FamilyDS = TExportAllPartnerData.ExportPartner(FamilyKey);
                        extRecord += Exporter.ExportPartnerExt(FamilyDS, ASiteKey, ALocationKey, ASpecificBuildingInfo);
                    }
                    // TODO: If I couldn't access the FAMILY for a PERSON, I should perhaps stop exporting?
                }
            }
            
            extRecord += Exporter.ExportPartnerExt( AMainDS, ASiteKey, ALocationKey, ASpecificBuildingInfo);
            return extRecord;
        }

        /// <summary>
        /// Unpack this (ext formatted) string into the database
        /// </summary>
        /// <param name="ALinesToImport"></param>
        /// <param name="ALimitToOption"></param>
        /// <param name="ADoNotOverwrite"></param>
        /// <param name="AResultList"></param>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean ImportDataExt(string[] ALinesToImport,
                    string ALimitToOption,
                    bool ADoNotOverwrite,
                    out TVerificationResultCollection AResultList)
        {
            TPartnerFileImport Importer = new TPartnerFileImport();
            Importer.ImportAllData(ALinesToImport,
                    ALimitToOption, ADoNotOverwrite,  out AResultList);
         return true;   
        }

    }
}