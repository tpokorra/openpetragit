//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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

using System;
using NUnit.Framework;
using Ict.Testing.NUnitForms;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance.Common;

namespace Ict.Testing.Petra.Server.MFinance.GL
{
    class TestOperation : AbstractPerdiodEndOperation
    {
        int intCount;
        int intJobCount;
        int intOperationCount;

        public TestOperation(int ACount)
        {
            intCount = ACount;
            intOperationCount = 0;
        }

        public override AbstractPerdiodEndOperation GetActualizedClone()
        {
            return new TestOperation(intCount + 1);
        }

        public override int JobSize {
            get
            {
                return intJobCount;
            }
        }

        public void SetJobSize(int ASize)
        {
            intJobCount = ASize;
        }

        public int GetOperationCount()
        {
            return intOperationCount;
        }

        public override void RunEndOfPeriodOperation()
        {
            Assert.AreEqual(1, intCount);
            ++intOperationCount;
            intJobCount = 0;
        }
    }

    class TestOperations : TPerdiodEndOperations
    {
        public void Test1(TVerificationResultCollection tvr)
        {
            verificationResults = tvr;
            TestOperation testOperation = new TestOperation(1);
            testOperation.SetJobSize(12);
            testOperation.IsInInfoMode = true;
            RunPeriodEndSequence(testOperation, "Message");
            Assert.AreEqual(1, testOperation.GetOperationCount());
        }

        public void Test2(TVerificationResultCollection tvr)
        {
            verificationResults = tvr;
            TestOperation testOperation = new TestOperation(1);
            testOperation.SetJobSize(12);
            testOperation.IsInInfoMode = false;
            RunPeriodEndSequence(testOperation, "Message");
            Assert.AreEqual(1, testOperation.GetOperationCount());
        }
    }


    /// <summary>
    /// Test of the GL.PeriodEnd.Year routines ...
    /// </summary>
    [TestFixture]
    public partial class TestGLPeriodicEnd : CommonNUnitFunctions
    {
        private const int intLedgerNumber = 43;

        /// <summary>
        /// Some very basic tests of TPerdiodEndOperations and AbstractPerdiodEndOperation
        /// </summary>
        [Test]
        public void Test_AbstractPerdiodEndOperation()
        {
            TVerificationResultCollection tvr = new TVerificationResultCollection();
            TestOperations perdiodEndOperations = new TestOperations();

            perdiodEndOperations.Test1(tvr);
            perdiodEndOperations.Test2(tvr);
        }

        /// <summary>
        /// Carry Forward manages the switch form Month to Month including year end ...
        /// </summary>
        [Test]
        public void Test_TCarryForward()
        {
            ResetDatabase();
            TCarryForward carryForward;

            for (int i = 1; i < 13; ++i)  // 12 Months
            {
                carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
                Assert.AreEqual(carryForward.GetPeriodType, TCarryForwardENum.Month,
                    "Month: " + i.ToString());
                carryForward.SetNextPeriod();
            }

            carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
            Assert.AreEqual(carryForward.GetPeriodType, TCarryForwardENum.Year, "Next Year");
            carryForward.SetNextPeriod();

            carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
            Assert.AreEqual(carryForward.GetPeriodType, TCarryForwardENum.Month, "Next Month");
            carryForward.SetNextPeriod();
        }

        /// <summary>
        /// Test_TCarryForwardYear
        /// </summary>
        [Test]
        public void Test_TCarryForwardYear()
        {
            ResetDatabase();
            TCarryForward carryForward = null;
            TVerificationResultCollection tvr = new TVerificationResultCollection();

            for (int i = 1; i < 13; ++i)  // 12 Months
            {
                carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
                Assert.AreEqual(carryForward.GetPeriodType, TCarryForwardENum.Month,
                    "Month: " + i.ToString());
                carryForward.SetNextPeriod();
            }

            Assert.AreEqual(2010, carryForward.Year, "Standard");
            TAccountPeriodToNewYear accountPeriodToNewYear =
                new TAccountPeriodToNewYear(intLedgerNumber, 2010);
            accountPeriodToNewYear.IsInInfoMode = false;
            accountPeriodToNewYear.VerificationResultCollection = tvr;
            accountPeriodToNewYear.RunEndOfPeriodOperation();

            carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
            Assert.AreEqual(2010, carryForward.Year, "Non standard ...");
            carryForward.SetNextPeriod();

            carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
            carryForward.SetNextPeriod();

            TLedgerInitFlagHandler ledgerInitFlag =
                new TLedgerInitFlagHandler(intLedgerNumber, TLedgerInitFlagEnum.ActualYear);
            ledgerInitFlag.AddMarker("2010");
            Assert.IsFalse(ledgerInitFlag.Flag, "Should be deleted ...");
        }

        /// <summary>
        /// TestFixtureSetUp
        /// </summary>
        [SetUp]
        public void Init()
        {
            InitServerConnection();
            //ResetDatabase();
            System.Diagnostics.Debug.WriteLine("Init: " + this.ToString());
        }

        /// <summary>
        /// TearDown the test
        /// </summary>
        [TestFixtureTearDown]
        public void TearDownTest()
        {
            DisconnectServerConnection();
            System.Diagnostics.Debug.WriteLine("TearDown: " + this.ToString());
        }
    }
}