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
//

using System;
using System.Data;
using Ict.Common.DB;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Server.MFinance.Gift.WebConnectors
{
    /// <summary>
    /// Description of Class1.
    /// </summary>
    public class TGuiTools
    {
        /// <summary>
        /// The user can insert a recipient key manualy and so each key has to be checked for
        /// usability. Parallel to this request you can check the values of MotivationGroup and
        /// MotivationDetail. In normal cases the global defaul values shall be used.
        /// But in some cases the some "PartnerKey"-specific defaults shall be used.
        ///
        /// This Routine
        /// 1. Checks if PartnerKey is available in p_partner
        /// 2. If PartnerKey is not available the result PartnerKeyIsValid = false and the other
        ///    parameter are not changed.
        /// 3. If PartnerKey is available then PartnerKeyIsValid = false and:
        /// 4. The Table-Entry p_partner_class_c is checked for the value "unit"
        /// 5. If p_partner_class_c does not hold the value unit, den the routine is done, the other
        ///    parameters are not changed
        /// 6. If p_partner_class_c holds the value "unit" then the table p_unit shall be checked.
        /// 7. If p_unit.p_partner_class_c holds the value "KEYMIN" then the value of MotivationDetail
        ///    shall be changed to KEY-MIN.
        /// 8. If p_unit.p_partner_class_c does not hold the value "KEYMIN" the routine is done.
        /// </summary>
        /// <param name="partnerKey">Input: not used
        ///                               Output: True if the partnerKey is a valid number of an
        ///                                       existing partner and false if not.</param>
        /// <param name="motivationGroup">Input: Common default value for the motivation group.
        ///                               Output: Default value depending of the actual
        ///                                       values of APartnerKey. </param>
        /// <param name="motivationDetail">Input: Common default value for the motivation detail.
        ///                               Output: Default value depending of the actual
        ///                                       values of APartnerKey. </param>
        /// <returns>The result of is boolean and the value true tells that there exists an entry
        /// in the database which is represented by the parther key</returns>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean GetMotivationGroupAndDetail(Int64 partnerKey,
            ref String motivationGroup,
            ref String motivationDetail)
        {
            Boolean partnerKeyIsValid;

            if (partnerKey != 0)
            {
                Boolean newTransaction;
                TDBTransaction readTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    IsolationLevel.RepeatableRead, out newTransaction);
                PPartnerTable myPPartnerTable =
                    PPartnerAccess.LoadByPrimaryKey(partnerKey, readTransaction);

                if (myPPartnerTable.Rows.Count == 1)
                {
                    // Entry for partnerKey is valid
                    partnerKeyIsValid = true;
                    PPartnerRow partnerRow = (PPartnerRow)myPPartnerTable.Rows[0];

                    // Change motivationDetail if ColumnPartnerClass is UNIT
                    if (partnerRow.PartnerClass.Equals(MPartnerConstants.PARTNERCLASS_UNIT))
                    {
                        // AND KEY-MIN
                        PUnitTable pUnitTable =
                            PUnitAccess.LoadByPrimaryKey(partnerKey, readTransaction);

                        if (pUnitTable.Rows.Count == 1)
                        {
                            PUnitRow unitRow = (PUnitRow)pUnitTable.Rows[0];

                            if (unitRow.UnitTypeCode.Equals(MPartnerConstants.UNIT_TYPE_KEYMIN))
                            {
                                motivationDetail = MFinanceConstants.GROUP_DETAIL_KEY_MIN;
                            }
                        }
                    }
                }
                else
                {
                    // There is no valid entry for partnerKey
                    partnerKeyIsValid = false;
                }

                if (newTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            else
            {
                // For partnerKey==0 there is no valid entry at any time
                partnerKeyIsValid = false;
            }

            return partnerKeyIsValid;
        }
    }
}