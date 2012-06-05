﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Server.MPartner.Partner.UIConnectors
{
    public partial class TPartnerEditUIConnector
    {
        //
        // Put Methods for the validation of Partner Edit screen data in this code file.
        //

        static partial void ValidatePPartnerManual(TValidationControlsDict AValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable)
        {
            TValidationControlsDict ValidationControlsDict = new TValidationControlsDict();

            ValidationControlsDict.Add(ASubmitTable.Columns[PPartnerTable.ColumnStatusCodeId],
                new TValidationControlsData(null, Catalog.GetString("Partner &Status")));           
            
            for (int Counter = 0; Counter < ASubmitTable.Rows.Count; Counter++)
            {
                TSharedPartnerValidation_Partner.ValidatePartnerManual("TPartnerEditUIConnector" +
                    " (Error in Row #" + Counter.ToString() + ")",  // No translation of message text since the server's messages should be all in English
                    (PPartnerRow)ASubmitTable.Rows[Counter], ref AVerificationResult,
                    ValidationControlsDict);
            }
        }

        static partial void ValidatePBankManual(TValidationControlsDict AValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable)
        {
            TValidationControlsDict ValidationControlsDict = new TValidationControlsDict();

            ValidationControlsDict.Add(ASubmitTable.Columns[PPartnerTable.ColumnStatusCodeId],
                new TValidationControlsData(null, Catalog.GetString("Partner &Status")));           

            ValidationControlsDict.Add(ASubmitTable.Columns[PBankTable.ColumnBranchCodeId],
                new TValidationControlsData(null, Catalog.GetString("Bank/Branch Code"),
                                            null, Catalog.GetString("BIC/SWIFT Code")));
            ValidationControlsDict.Add(ASubmitTable.Columns[PBankTable.ColumnBicId],
                new TValidationControlsData(null, Catalog.GetString("BIC/SWIFT Code")));
            
            for (int Counter = 0; Counter < ASubmitTable.Rows.Count; Counter++)
            {
                TSharedPartnerValidation_Partner.ValidatePartnerBankDetailsManual("TPartnerEditUIConnector" +
                    " (Error in Row #" + Counter.ToString() + ")",  // No translation of message text since the server's messages should be all in English
                    (PBankRow)ASubmitTable.Rows[Counter], ref AVerificationResult,
                    ValidationControlsDict);
            }
        }
    }
}