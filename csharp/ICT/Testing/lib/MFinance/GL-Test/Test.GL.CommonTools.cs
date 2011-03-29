﻿//
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
using Ict.Petra.Server.MFinance.GL;


using Ict.Common;


namespace Ict.Testing.Petra.Server.MFinance.GL
{
    [TestFixture]
    public partial class TestGLCommonTools : CommonNUnitFunctions
    {
        [Test]
        public void Test_01_TLedgerInitFlagHandler()
        {
        	bool blnOld = new TLedgerInitFlagHandler(43,TLedgerInitFlagHandler.REVALUATION).Flag;
        	 blnOld = new TLedgerInitFlagHandler(43,TLedgerInitFlagHandler.REVALUATION).Flag;
        	 blnOld = new TLedgerInitFlagHandler(43,TLedgerInitFlagHandler.REVALUATION).Flag;
        	System.Diagnostics.Debug.WriteLine("--------------------");
        	//new TLedgerInitFlagHandler(43,TLedgerInitFlagHandler.REVALUATION).Flag = false;
        	new TLedgerInitFlagHandler(43,TLedgerInitFlagHandler.REVALUATION).Flag = true;
        	System.Diagnostics.Debug.WriteLine("--------------------");
        	Assert.IsTrue(new TLedgerInitFlagHandler(
        		43,TLedgerInitFlagHandler.REVALUATION).Flag, "Flag was set a line before");
        	System.Diagnostics.Debug.WriteLine("--------------------");
        	new TLedgerInitFlagHandler(43,TLedgerInitFlagHandler.REVALUATION).Flag = false;
        	System.Diagnostics.Debug.WriteLine("--------------------");
        	Assert.IsFalse(new TLedgerInitFlagHandler(
        		43,TLedgerInitFlagHandler.REVALUATION).Flag, "Flag was reset a line before");
        	System.Diagnostics.Debug.WriteLine("--------------------");
        	new TLedgerInitFlagHandler(43,TLedgerInitFlagHandler.REVALUATION).Flag = blnOld;
        	System.Diagnostics.Debug.WriteLine("--------------------");
        }

        [TestFixtureSetUp]
        public void Init()
        {
            InitServerConnection();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            DisconnectServerConnection();
        }
    }
}