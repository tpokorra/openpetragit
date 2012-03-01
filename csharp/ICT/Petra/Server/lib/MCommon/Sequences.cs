//
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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MCommon.WebConnectors;
using Ict.Petra.Server.App.Core.Security;
using System.Data;

namespace Ict.Petra.Server.MCommon.WebConnectors
{
    /// <summary>
    /// this connector returns the next sequence value from the database
    /// </summary>
    public class TSequenceWebConnector
    {
        /// <summary>
        /// get the next sequence value
        /// </summary>
        /// <param name="ASequence"></param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static Int64 GetNextSequence(TSequenceNames ASequence)
        {
            bool NewTransaction;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            Int64 NewSequenceValue = DBAccess.GDBAccessObj.GetNextSequenceValue(ASequence.ToString(), Transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }

            return NewSequenceValue;
        }
    }
}