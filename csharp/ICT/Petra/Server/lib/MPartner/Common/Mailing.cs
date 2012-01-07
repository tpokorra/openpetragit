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
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Collections.Generic;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.DataAggregates;

namespace Ict.Petra.Server.MPartner.Common
{
    /// <summary>
    /// Contains Partner Module Partner (Mailing) - Subnamespace Business Logic.
    ///
    /// 'Business Logic' refers to any logic that retrieves data in a specific way,
    /// checks the validity of modifications of data, or perform certain changes on
    /// data in a specific way.
    /// </summary>
    public static class TMailing
    {
        /// <summary>
        /// Creates a list of locations for a given Partner.
        /// </summary>
        /// <remarks>Corresponds in parts with with Progress 4GL Method
        /// 'GetPartnerLocations' in partner/maillib.p.</remarks>
        /// <param name="APartnerKey">PartnerKey of the Partner being processed.</param>
        /// <param name="AMailingAddressesOnly">If true: only include addresses with mailing flag set.</param>
        /// <param name="AIncludeCurrentAddresses">If true: include current addresses.</param>
        /// <param name="AIncludeFutureAddresses">If true: include future addresses.</param>
        /// <param name="AIncludeExpiredAddresses">If true: include expired addresses.</param>
        /// <param name="APartnerLocations">The Locations of the Partner being processed.</param>
        /// <returns>False if an invalid PartnerKey was passed in or if Petra Security
        /// denied access to the Partner, otherwise true.</returns>
        public static bool GetPartnerLocations(Int64 APartnerKey,
            bool AMailingAddressesOnly,
            bool AIncludeCurrentAddresses,
            bool AIncludeFutureAddresses,
            bool AIncludeExpiredAddresses,
            out PPartnerLocationTable APartnerLocations)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;

            String SelectSQL;
            DataSet FillDataSet;

            OdbcParameter param;

            // Initialise out Argument
            APartnerLocations = null;

            if (APartnerKey > 0)
            {
#if DEBUGMODE
                if (TLogging.DL >= 8)
                {
                    Console.WriteLine("TMailing.GetPartnerLocations: Checking access to Partner.");
                }
#endif

                if (TSecurity.CanAccessPartnerByKey(APartnerKey, false) ==
                    TPartnerAccessLevelEnum.palGranted)
                {
                    ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                        IsolationLevel.ReadCommitted,
                        TEnforceIsolationLevel.eilMinimum,
                        out NewTransaction);

                    // Load Partner Locations, taking passed in restrictions into account.
                    try
                    {
                        SelectSQL =

//                            "SELECT *" +
//                            "  FROM PUB_" + PPartnerLocationTable.GetTableDBName() +
//                            " WHERE " + PPartnerLocationTable.GetPartnerKeyDBName() + " = ?" +
//                            "   AND (NOT ? = true OR (? = true AND " + PPartnerLocationTable.GetSendMailDBName() + " = true))";
                            "SELECT *" +
                            "  FROM PUB_" + PPartnerLocationTable.GetTableDBName() +
                            " WHERE " + PPartnerLocationTable.GetPartnerKeyDBName() + " = ?" +
                            "   AND (NOT ? = true OR (? = true AND " + PPartnerLocationTable.GetSendMailDBName() + " = true))" +
                            "   AND ((? = true AND (" + PPartnerLocationTable.GetDateEffectiveDBName() + " <= ?" +
                            "     AND (" + PPartnerLocationTable.GetDateGoodUntilDBName() + " >= ?" +
                            "          OR " + PPartnerLocationTable.GetDateGoodUntilDBName() + " IS NULL)))" +
                            "     OR (? = true AND " + PPartnerLocationTable.GetDateEffectiveDBName() + " > ?)" +
                            "     OR (? = true AND " + PPartnerLocationTable.GetDateGoodUntilDBName() + " < ?))";

                        List <OdbcParameter>parameters = new List <OdbcParameter>();
                        param = new OdbcParameter("PartnerKey", OdbcType.Decimal, 10);
                        param.Value = APartnerKey;
                        parameters.Add(param);
                        param = new OdbcParameter("MailingAddressOnly1", OdbcType.Bit);
                        param.Value = AMailingAddressesOnly;
                        parameters.Add(param);
                        param = new OdbcParameter("MailingAddressOnly2", OdbcType.Bit);
                        param.Value = AMailingAddressesOnly;
                        parameters.Add(param);
                        param = new OdbcParameter("IncludeCurrentAddresses", OdbcType.Bit);
                        param.Value = AIncludeCurrentAddresses;
                        parameters.Add(param);
                        param = new OdbcParameter("TodaysDate1", OdbcType.Date);
                        param.Value = DateTime.Now;
                        parameters.Add(param);
                        param = new OdbcParameter("TodaysDate2", OdbcType.Date);
                        param.Value = DateTime.Now;
                        parameters.Add(param);
                        param = new OdbcParameter("IncludeFutureAddresses", OdbcType.Bit);
                        param.Value = AIncludeFutureAddresses;
                        parameters.Add(param);
                        param = new OdbcParameter("TodaysDate3", OdbcType.Date);
                        param.Value = DateTime.Now;
                        parameters.Add(param);
                        param = new OdbcParameter("IncludeExpiredAddresses", OdbcType.Bit);
                        param.Value = AIncludeExpiredAddresses;
                        parameters.Add(param);
                        param = new OdbcParameter("TodaysDate4", OdbcType.Date);
                        param.Value = DateTime.Now;
                        parameters.Add(param);

                        /*
                         * Our out Argument 'APartnerLocations' is a Typed DataTable, but SelectDT
                         * returns an untyped DataTable, therefore we need to create a Typed DataTable
                         * that contains the data of the returned untyped DataTable!
                         */
                        FillDataSet = new DataSet();
                        APartnerLocations = new PPartnerLocationTable(PPartnerLocationTable.GetTableDBName());
                        FillDataSet.Tables.Add(APartnerLocations);

                        DBAccess.GDBAccessObj.Select(FillDataSet, SelectSQL,
                            PPartnerLocationTable.GetTableDBName(),
                            ReadTransaction,
                            parameters.ToArray());

#if DEBUGMODE
                        if (TLogging.DL >= 7)
                        {
                            Console.WriteLine("TMailing.GetPartnerLocations:  FillDataSet.Tables.Count: " + FillDataSet.Tables.Count.ToString());
                        }
#endif
                        FillDataSet.Tables.Remove(APartnerLocations);

                        if (APartnerLocations.Rows.Count > 0)
                        {
#if DEBUGMODE
                            if (TLogging.DL >= 7)
                            {
                                Console.WriteLine(
                                    "TMailing.GetPartnerLocations: Found " + APartnerLocations.Rows.Count.ToString() +
                                    " PartnerLocations found for Partner " + APartnerKey.ToString() + ".");
                            }
#endif
                        }
                        else
                        {
                            /*
                             * /* No Rows returned = no PartnerLocations for Partner.
                             * That shouldn't happen with existing Partners, but if it does (eg. non-existing
                             * PartnerKey passed in) we return an empty Typed DataTable.
                             */
#if DEBUGMODE
                            if (TLogging.DL >= 7)
                            {
                                Console.WriteLine(
                                    "TMailing.GetPartnerLocations: No PartnerLocations found for Partner " + APartnerKey.ToString() + "!");
                            }
#endif
                            APartnerLocations = new PPartnerLocationTable();
                        }
                    }
                    finally
                    {
                        if (NewTransaction)
                        {
                            DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                            if (TLogging.DL >= 7)
                            {
                                Console.WriteLine("TMailing.GetPartnerLocations: committed own transaction.");
                            }
#endif
                        }
                    }

                    return true;
                }
                else
                {
#if DEBUGMODE
                    if (TLogging.DL >= 8)
                    {
                        Console.WriteLine("TMailing.GetPartnerLocations: Access to Partner DENIED!");
                    }
#endif

                    // Petra Security prevents us from accessing this Partner -> return false;
                    return false;
                }
            }
            else
            {
                // Invalid PartnerKey -> return false;
                return false;
            }
        }

        /// <summary>
        /// Returns the Primary Key of the Location and the Location and PartnerLocation DataRows
        /// of the 'Best Address' of a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner for which the 'Best Address'
        /// should be loaded for.</param>
        /// <param name="ABestAddressPK">Primary Key of the 'Best Address' Location</param>
        /// <param name="ALocationDR">DataRow containing the 'Best Address' Location</param>
        /// <param name="APartnerLocationDR">DataRow containing the 'Best Address' PartnerLocation</param>
        /// <param name="AVerificationResult">Contains DB call exceptions, if there are any.</param>
        /// <returns>False if an invalid PartnerKey was passed in or if Petra Security
        /// denied access to the Partner or if Location/PartnerLocation Data could not be loaded for the
        /// Partner, otherwise true.</returns>
        public static bool GetPartnersBestLocationData(Int64 APartnerKey,
            out TLocationPK ABestAddressPK,
            out PLocationRow ALocationDR, out PPartnerLocationRow APartnerLocationDR,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            PPartnerLocationTable PartnerLocationDT;
            PLocationTable LocationDT;

            ALocationDR = null;
            APartnerLocationDR = null;
            AVerificationResult = null;
            ABestAddressPK = null;

            if (APartnerKey > 0)
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                try
                {
                    if (TMailing.GetPartnerLocations(APartnerKey, false,
                            true, true, true, out PartnerLocationDT))
                    {
#if DEBUGMODE
                        if (TLogging.DL >= 8)
                        {
                            Console.WriteLine(
                                "TMailing.GetPartnersBestLocationData: processing " + PartnerLocationDT.Rows.Count.ToString() + " Locations...");
                        }
#endif

                        if (PartnerLocationDT.Rows.Count > 1)
                        {
                            Calculations.DeterminePartnerLocationsDateStatus(PartnerLocationDT, DateTime.Today);
                            ABestAddressPK = Calculations.DetermineBestAddress(PartnerLocationDT);
                        }
                        else if (PartnerLocationDT.Rows.Count == 1)
                        {
                            ABestAddressPK = new TLocationPK(PartnerLocationDT[0].SiteKey, PartnerLocationDT[0].LocationKey);
                        }
                        else
                        {
                            return false;
                        }

#if DEBUGMODE
                        if (TLogging.DL >= 8)
                        {
                            Console.WriteLine(
                                "TMailing.GetPartnersBestLocationData: BestAddressPK: " + ABestAddressPK.SiteKey.ToString() + ", " +
                                ABestAddressPK.LocationKey.ToString());
                        }
#endif
                        APartnerLocationDR = (PPartnerLocationRow)PartnerLocationDT.Rows.Find(
                            new object[] { APartnerKey, ABestAddressPK.SiteKey, ABestAddressPK.LocationKey });

                        LocationDT = TPPartnerAddressAggregate.LoadByPrimaryKey(
                            ABestAddressPK.SiteKey, ABestAddressPK.LocationKey, ReadTransaction);

                        if (LocationDT != null)
                        {
                            ALocationDR = LocationDT[0];
                        }
                        else
                        {
                            return false;
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TLogging.DL >= 7)
                        {
                            Console.WriteLine("TMailing.GetPartnersBestLocationData: committed own transaction.");
                        }
#endif
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the Primary Key of the Location of the 'Best Address' of a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartneKey of the Partner for which the 'Best Address'
        /// should be loaded for.</param>
        /// <param name="AVerificationResult">Contains DB call exceptions, if there are any.</param>
        /// <returns>TLocationPK(-1,-1) if an invalid PartnerKey was passed in or if Petra Security
        /// denied access to the Partner or if Location/PartnerLocation Data could not be loaded for the
        /// Partner, otherwise the Primary Key of the Location of the 'Best Address' of the Partner.</returns>
        public static TLocationPK GetPartnersBestLocation(Int64 APartnerKey,
            out TVerificationResultCollection AVerificationResult)
        {
            TLocationPK ReturnValue = new TLocationPK(-1, -1);
            TLocationPK LocationPK;

            PLocationRow LocationDR;
            PPartnerLocationRow PartnerLocationDR;

            if (GetPartnersBestLocationData(APartnerKey, out LocationPK, out LocationDR,
                    out PartnerLocationDR, out AVerificationResult))
            {
                ReturnValue = LocationPK;
            }

            return ReturnValue;
        }
    }
}