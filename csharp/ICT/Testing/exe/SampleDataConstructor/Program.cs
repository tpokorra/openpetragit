//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       thomass
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
using System.Collections.Generic;
using System.Collections.Specialized;

using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace SampleDataConstructor
{
/// <summary>
/// This class creates sample data (partners, organisations, gifts) and imports them into OpenPetra.
/// </summary>
/// <remarks>
/// <para>
/// The class requires raw data to have been created already by benerator, reads this data, enhances
/// and compiles it (using the literal meaning of "compile", i.e. putting together People, Addresses,
/// Phonenumbers to create partners), and them imports this data to the OpenPetra Server.
/// </para>
/// <para>
/// Generally, the Sample Data creator DOES NOT use the Petra Model internally,
/// although it tries to stay close to it ( e.g. Naming Convention).
/// This is so it can run a simple simulation for creating events (marriages resulting in same location, children, gift entries).
/// These can then be saved in Petra.
/// </para>
/// <para>
/// Classes starting in "R" (RPartner) are raw data for later use.
/// Classes starting in "???" are the actual Petra TDS Records.
/// </para>
/// <para>
/// Goal for now:
/// just create many Partners with Addresses and then import.
/// This can perhaps be done with the original Petra Format.
///
/// For now, creation of
/// - raw data
/// - then putting together Petra data (no simulation)
/// </para>
///
/// TODO: Check comment from Timo: This is actually rather a tool than a test 
/// - so one could change it's location.
/// </remarks>
class TSampleDataConstructor
{
    /// <summary>
    /// data directory containing the raw data files created by benerator
    /// </summary>
    /// <remarks>Please forgive me: dd = dataDirectory</remarks>
    /// TODO (before shipping this testing to trunk): get this path from nant (or sth.) and not set it statically
    const string dd = "../../tmp/Tests-exe.SampleDataConstructor/";

    const string filePeople = "people.csv";
    const string fileOrganisations = "organisations.csv";
    const string fileAddresses = "addresses.csv";

    public static void doReport(ExecutionReport report)
    {
        Console.WriteLine(report);
    }

    /// <summary>
    /// Creates Sample Data using the raw data provided and exports this to the Petra Server
    /// </summary>
    /// <remarks>
    /// TODO: use standard OpenPetra logging instead of the prelimanary "Report"
    /// </remarks>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        // SampleDataConstructor
        Console.WriteLine("Reading Raw Data Files...");


        //// new TAppSettingsManager(false);
        //// string csvInputFileName = TAppSettingsManager.GetValue("file", true);

        
        ExecutionReport report;
        try
        {
            Console.WriteLine("Reading Raw Data Files...");
            RawData rawData = new RawData();
            rawData.readRawDataFromFile(filePeople, RawData.filetypes.people, dd);
            rawData.readRawDataFromFile(fileOrganisations, RawData.filetypes.organizations, dd);
            rawData.readRawDataFromFile(fileAddresses, RawData.filetypes.addresses, dd);
            Console.WriteLine("People:        " + rawData.People.Count);
            Console.WriteLine("Organizations: " + rawData.Organizations.Count);
            Console.WriteLine("Locations:     " + rawData.Locations.Count);
            Console.WriteLine("Mobile Phones: " + rawData.Mobilephones.Count);
            Console.WriteLine("Countries:     " + rawData.Countries.Count);

            Console.WriteLine("Creating TDS from Raw Data...");
            var dataTDS = new SampleDataConstructorTDS();
            var constructionStats = new ConstructionStats();
            var unusedLocations = new Stack <PLocationRow>();

            DataBuilder.insertPeople(dataTDS, rawData, out report); doReport(report);
            DataBuilder.insertOrganisations(dataTDS, rawData, out report); doReport(report);
            // Units? No units for now (could be copied from ImportExportYML)
            //
            // Subscriptions? No Subscriptions for now
            //

            DataBuilder.insertLocations(dataTDS, unusedLocations, rawData, out report); doReport(report);

            /*
             * DataBuilder.initMobilePhones(dataTDS,rawData, out report);
             * DataBuilder.initCountries(dataTDS,rawData, out report);
             */
            DataBuilder.AssignHomesToPartners(
                dataTDS, unusedLocations,
                constructionStats.PeopleWithHomeKnown,
                out report); doReport(report);

           	/*
             *  DataBuilder.assignSpecialTypesToPeople(dataTDS,rawData,supporterStats,out report);
             */
        
            
            // Save data to Server
            /*
			TVerificationResultCollection VerificationResult;
			
            if (!TRemote.MSysMan.ImportExport.WebConnectors.SaveTDS(dataTDS, out VerificationResult))
            	throw new Exception("Error saving to Database. No more information for now.");
            */
            Console.WriteLine("Completed.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
    }
}
}