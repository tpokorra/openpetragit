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
using System.Data;
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;

namespace Tests.MPartner.Server.PartnerEdit
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TPartnerEditTest
    {
        TServerManager FServerManager;

        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            new TLogging("../../log/TestServer.log");
            FServerManager = TPetraServerConnector.Connect("../../etc/TestServer.config");
        }

        /// <summary>
        /// cleaning up everything that was set up for this test
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            TPetraServerConnector.Disconnect();
        }

        private PPartnerRow CreateNewPartner(PartnerEditTDS AMainDS, TPartnerEditUIConnector AConnector)
        {
            PPartnerRow PartnerRow = AMainDS.PPartner.NewRowTyped();

            // get a new partner key
            Int64 newPartnerKey = -1;

            do
            {
                newPartnerKey = TNewPartnerKey.GetNewPartnerKey(DomainManager.GSiteKey);
                TNewPartnerKey.SubmitNewPartnerKey(DomainManager.GSiteKey, newPartnerKey, ref newPartnerKey);
                PartnerRow.PartnerKey = newPartnerKey;
            } while (newPartnerKey == -1);

            PartnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;

            AMainDS.PPartner.Rows.Add(PartnerRow);

            TLogging.Log("Creating new partner: " + PartnerRow.PartnerKey.ToString());

            return PartnerRow;
        }

        private PPartnerRow CreateNewFamilyPartner(PartnerEditTDS AMainDS, TPartnerEditUIConnector AConnector)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, AConnector);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestPartner, Mr";

            PFamilyRow FamilyRow = AMainDS.PFamily.NewRowTyped();
            FamilyRow.PartnerKey = PartnerRow.PartnerKey;
            FamilyRow.FamilyName = PartnerRow.PartnerKey.ToString();
            FamilyRow.FirstName = "TestPartner";
            FamilyRow.Title = "Mr";
            AMainDS.PFamily.Rows.Add(FamilyRow);

            return PartnerRow;
        }

        private PPersonRow CreateNewPerson(PartnerEditTDS AMainDS,
            TPartnerEditUIConnector AConnector,
            Int64 AFamilyKey,
            Int32 ALocationKey,
            string AFirstName,
            string ATitle,
            int AFamilyID)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, AConnector);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_PERSON;
            PartnerRow.PartnerShortName = AFamilyKey.ToString() + ", " + AFirstName + ", " + ATitle;

            PPersonRow PersonRow = AMainDS.PPerson.NewRowTyped();
            PersonRow.PartnerKey = PartnerRow.PartnerKey;
            PersonRow.FamilyKey = AFamilyKey;
            PersonRow.FamilyName = AFamilyKey.ToString();
            PersonRow.FirstName = AFirstName;
            PersonRow.FamilyId = AFamilyID;
            PersonRow.Title = ATitle;
            AMainDS.PPerson.Rows.Add(PersonRow);

            PPartnerLocationRow PartnerLocationRow = AMainDS.PPartnerLocation.NewRowTyped();
            PartnerLocationRow.SiteKey = DomainManager.GSiteKey;
            PartnerLocationRow.PartnerKey = PartnerRow.PartnerKey;
            PartnerLocationRow.LocationKey = ALocationKey;
            PartnerLocationRow.TelephoneNumber = PersonRow.PartnerKey.ToString();
            AMainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);

            return PersonRow;
        }

        private void CreateFamilyWithPersonRecords(PartnerEditTDS AMainDS, TPartnerEditUIConnector AConnector)
        {
            PPartnerRow PartnerRow = CreateNewFamilyPartner(AMainDS, AConnector);

            CreateNewLocation(PartnerRow.PartnerKey, AMainDS);

            PPersonRow PersonRow1 = CreateNewPerson(AMainDS,
                AConnector,
                PartnerRow.PartnerKey,
                AMainDS.PLocation[0].LocationKey,
                "Adam",
                "Mr",
                0);
            PPersonRow PersonRow2 = CreateNewPerson(AMainDS,
                AConnector,
                PartnerRow.PartnerKey,
                AMainDS.PLocation[0].LocationKey,
                "Eve",
                "Mrs",
                1);
        }

        private void CreateNewLocation(Int64 APartnerKey, PartnerEditTDS AMainDS)
        {
            // avoid duplicate addresses: StreetName contains the partner key
            PLocationRow LocationRow = AMainDS.PLocation.NewRowTyped();

            LocationRow.SiteKey = DomainManager.GSiteKey;
            LocationRow.LocationKey = -1;
            LocationRow.StreetName = APartnerKey.ToString() + " Nowhere Lane";
            LocationRow.PostalCode = "LO2 2CX";
            LocationRow.City = "London";
            LocationRow.CountryCode = "99";
            AMainDS.PLocation.Rows.Add(LocationRow);

            PPartnerLocationRow PartnerLocationRow = AMainDS.PPartnerLocation.NewRowTyped();
            PartnerLocationRow.SiteKey = LocationRow.SiteKey;
            PartnerLocationRow.PartnerKey = APartnerKey;
            PartnerLocationRow.LocationKey = LocationRow.LocationKey;
            PartnerLocationRow.TelephoneNumber = APartnerKey.ToString();
            AMainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);
        }

        /// <summary>
        /// create a new partner and save it with a new location
        /// </summary>
        [Test]
        public void TestSaveNewPartnerWithLocation()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            PPartnerRow PartnerRow = CreateNewFamilyPartner(MainDS, connector);

            CreateNewLocation(PartnerRow.PartnerKey, MainDS);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalError())
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "TPartnerEditUIConnector SubmitChanges return value");

            // check the location key for this partner. should not be negative
            Assert.AreEqual(1, MainDS.PPartnerLocation.Rows.Count, "TPartnerEditUIConnector SubmitChanges returns one location");
            Assert.Greater(MainDS.PPartnerLocation[0].LocationKey, 0, "TPartnerEditUIConnector SubmitChanges returns valid location key");
        }

        /// <summary>
        /// new partner with location 0.
        /// first save the partner with location 0, then add a new location, and save again
        /// </summary>
        [Test]
        public void TestNewPartnerWithLocation0()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            PPartnerRow PartnerRow = CreateNewFamilyPartner(MainDS, connector);

            PPartnerLocationRow PartnerLocationRow = MainDS.PPartnerLocation.NewRowTyped();

            PartnerLocationRow.SiteKey = DomainManager.GSiteKey;
            PartnerLocationRow.PartnerKey = PartnerRow.PartnerKey;
            PartnerLocationRow.LocationKey = 0;
            PartnerLocationRow.TelephoneNumber = PartnerRow.PartnerKey.ToString();
            MainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalError())
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "Create a partner with location 0");

            CreateNewLocation(PartnerRow.PartnerKey, MainDS);

            // remove location 0, same is done in csharp\ICT\Petra\Client\MCommon\logic\UC_PartnerAddresses.cs TUCPartnerAddressesLogic::AddRecord
            // Check if record with PartnerLocation.LocationKey = 0 is around > delete it
            DataRow PartnerLocationRecordZero =
                MainDS.PPartnerLocation.Rows.Find(new object[] { PartnerRow.PartnerKey, DomainManager.GSiteKey, 0 });

            if (PartnerLocationRecordZero != null)
            {
                DataRow LocationRecordZero = MainDS.PLocation.Rows.Find(new object[] { DomainManager.GSiteKey, 0 });

                if (LocationRecordZero != null)
                {
                    LocationRecordZero.Delete();
                }

                PartnerLocationRecordZero.Delete();
            }

            ResponseDS = new PartnerEditTDS();
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalError())
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "Replace location 0 of partner");

            Assert.AreEqual(1, MainDS.PPartnerLocation.Rows.Count, "the partner should only have one location in the dataset");

            // get all addresses of the partner
            PPartnerLocationTable testPartnerLocations = PPartnerLocationAccess.LoadViaPPartner(PartnerRow.PartnerKey, null);
            Assert.AreEqual(1, testPartnerLocations.Rows.Count, "the partner should only have one location");
            Assert.Greater(testPartnerLocations[0].LocationKey, 0, "TPartnerEditUIConnector SubmitChanges returns valid location key");
        }

        /// <summary>
        /// create a new partner and save it with an existing location
        /// </summary>
        [Test]
        public void TestSaveNewPartnerWithExistingLocation()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            PPartnerRow PartnerRow = CreateNewFamilyPartner(MainDS, connector);

            CreateNewLocation(PartnerRow.PartnerKey, MainDS);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalError())
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "saving the first partner with a location");

            Int32 LocationKey = MainDS.PLocation[0].LocationKey;

            MainDS = new PartnerEditTDS();

            PartnerRow = CreateNewFamilyPartner(MainDS, connector);

            PPartnerLocationRow PartnerLocationRow = MainDS.PPartnerLocation.NewRowTyped();
            PartnerLocationRow.SiteKey = DomainManager.GSiteKey;
            PartnerLocationRow.PartnerKey = PartnerRow.PartnerKey;
            PartnerLocationRow.LocationKey = LocationKey;
            PartnerLocationRow.TelephoneNumber = PartnerRow.PartnerKey.ToString();
            MainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);

            ResponseDS = new PartnerEditTDS();

            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalError())
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            PPartnerTable PartnerAtAddress = PPartnerAccess.LoadViaPLocation(
                DomainManager.GSiteKey, LocationKey, null);

            Assert.AreEqual(2, PartnerAtAddress.Rows.Count, "there should be two partners at this location");
        }

        /// <summary>
        /// add a new location for a family, and propagate this to the members of the family
        /// </summary>
        [Test]
        public void TestFamilyPropagateNewLocation()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            CreateFamilyWithPersonRecords(MainDS, connector);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalError())
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            // now change on partner location. should ask about everyone else
            // it seems, the change must be to PLocation. In Petra 2.3, changes to the PartnerLocation are not propagated
            // MainDS.PPartnerLocation[0].DateGoodUntil = new DateTime(2011, 01, 01);

            Assert.AreEqual(1, MainDS.PLocation.Rows.Count, "there should be only one address for the whole family");

            MainDS.PLocation[0].County = "different";

            ResponseDS = new PartnerEditTDS();

            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalError())
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            Assert.AreEqual(TSubmitChangesResult.scrInfoNeeded,
                result,
                "should ask if the partner locations of the other members of the family should be changed as well");
        }

        /// <summary>
        /// modify a location that is used by several partners, but only modify the location for this partner.
        /// need to create a new location
        /// </summary>
        [Test]
        public void TestModifyLocationCreateNew()
        {
            // TODO
        }

        /// <summary>
        /// delete a location
        /// </summary>
        [Test]
        public void TestRemoveLocationFromSeveralPartners()
        {
            // TODO
        }

        /// <summary>
        /// when adding a new location to a family, all the partners involved should be updated p_partner.s_date_modified_d.
        /// even if the partner is not part of the dataset yet.
        /// </summary>
        [Test]
        public void TestChangeLocationUpdatesDateModifiedOfAllPartners()
        {
            // TODO
        }

        /// <summary>
        /// check if changing the family will change the family id of all other members of the family as well
        /// </summary>
        [Test]
        public void TestChangeFamilyID()
        {
            // TODO
        }
    }
}