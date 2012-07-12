//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2012 by OM International
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
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MPersonnel.Units.Data.Access;
using System.Windows.Forms;
using System.Collections;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Shared.MPartner.Partner.Data;
using System.Data;
using Ict.Petra.Shared.MPersonnel;

namespace Ict.Petra.Server.MPersonnel.WebConnectors
{
    /// <summary>
    /// Description of Personnel.
    /// </summary>
    public partial class TPersonnelWebConnector
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PERSONNEL")]
        public static ArrayList GetUnitHeirarchy()
        {
            const Int64 THE_ORGANISATION = 1000000;

            ArrayList Ret = new ArrayList();
            try
            {
                TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

                PUnitTable UnitTbl = PUnitAccess.LoadAll(Transaction);
                UmUnitStructureTable HierarchyTbl = UmUnitStructureAccess.LoadAll(Transaction);
                HierarchyTbl.DefaultView.Sort = UmUnitStructureTable.GetChildUnitKeyDBName();
                UnitTbl.DefaultView.Sort = PUnitTable.GetPartnerKeyDBName();
                UnitHierarchyNode RootNode = new UnitHierarchyNode();

                RootNode.MyUnitKey = THE_ORGANISATION;
                RootNode.ParentUnitKey = THE_ORGANISATION;
                RootNode.Description = "The Organisation";

                Int32 RootUnitIdx = UnitTbl.DefaultView.Find(THE_ORGANISATION);

                if (RootUnitIdx >= 0)
                {
                    RootNode.Description = ((PUnitRow)UnitTbl.DefaultView[RootUnitIdx].Row).UnitName;
                    UnitTbl.DefaultView.Delete(RootUnitIdx);
                }
                Ret.Add(RootNode);

                foreach (DataRowView rv in UnitTbl.DefaultView)
                {
                    PUnitRow UnitRow = (PUnitRow)rv.Row;
                    UnitHierarchyNode Node = new UnitHierarchyNode();
                    Node.Description = UnitRow.UnitName;
                    Node.MyUnitKey = UnitRow.PartnerKey;

                    Int32 HierarchyTblIdx = HierarchyTbl.DefaultView.Find(Node.MyUnitKey);
                    if (HierarchyTblIdx >= 0)
                    {
                        Node.ParentUnitKey = ((UmUnitStructureRow)HierarchyTbl.DefaultView[HierarchyTblIdx].Row).ParentUnitKey;
                    }
                    else
                    {
                        Node.ParentUnitKey = THE_ORGANISATION;
                    }
                    Ret.Add(Node);
                }
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
            return Ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Nodes"></param>
        /// <returns></returns>
        [RequireModulePermission("PERSONNEL")]
        public static bool SaveUnitHierarchy(ArrayList Nodes)
        {
            UmUnitStructureTable NewTable = new UmUnitStructureTable();

            foreach (UnitHierarchyNode Node in Nodes)
            {
                NewTable.Rows.Add(Node.ParentUnitKey, Node.MyUnitKey);
            }

            // This new table I've constructed COMPLETELY REPLACES
            // the existing UmUnitStructure table. 
            // I'll delete the whole content before calling SubmitChanges with my new data.

            Boolean CommitOK = false;
            try
            {
                TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_um_unit_structure", Transaction);

                TVerificationResultCollection Results;
                CommitOK = UmUnitStructureAccess.SubmitChanges(NewTable, Transaction, out Results);
            }
            finally
            {
                if (CommitOK)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            return true;
        }
    }
}