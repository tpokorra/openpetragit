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
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using System.Data;


namespace Ict.Petra.Server.App.Core.Security
{
    /// <summary>
    /// The TGroupManager class provides functions to work with the Security Groups
    /// and Users' Security Groups of a Petra DB.
    /// </summary>
    public class TGroupManager
    {
        /// <summary>
        /// load the groups of the given user
        /// </summary>
        /// <param name="AUserID"></param>
        /// <returns></returns>
        public static SUserGroupTable LoadUserGroups(String AUserID)
        {
            SUserGroupTable ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

                if (SUserGroupAccess.CountViaSUser(AUserID, ReadTransaction) > 0)
                {
                    ReturnValue = SUserGroupAccess.LoadViaSUser(AUserID, ReadTransaction);
                }
                else
                {
                    ReturnValue = new SUserGroupTable();
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                    if (TLogging.DL >= 8)
                    {
                        Console.WriteLine("TGroupManager.LoadUserGroups: committed own transaction.");
                    }
#endif
                }
            }
            return ReturnValue;
        }
    }
}