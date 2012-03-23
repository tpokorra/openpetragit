//
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

using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Server.MFinance.Gift.WebConnectors
{
    public partial class TTransactionWebConnector
    {
        //
        // Put Methods for the validation of Gift Transactions in this code file.
        //

        static partial void ValidateGiftBatchManual(TValidationControlsDict AValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable)
        {
            TValidationControlsDict ValidationControlsDict = new TValidationControlsDict();

            ValidationControlsDict.Add(ASubmitTable.Columns[AGiftBatchTable.ColumnBatchDescriptionId],
                new TValidationControlsData(null, AGiftBatchTable.GetBatchDescriptionDBName()));
            ValidationControlsDict.Add(ASubmitTable.Columns[AGiftBatchTable.ColumnExchangeRateToBaseId],
                new TValidationControlsData(null, AGiftBatchTable.GetExchangeRateToBaseDBName()));

            for (int Counter = 0; Counter < ASubmitTable.Rows.Count; Counter++)
            {
                TSharedFinanceValidation_Gift.ValidateGiftBatchManual("TTransactionWebConnector" +
                    " (Error in Row #" + Counter.ToString() + ")",  // No translation of message text since the server's messages should be all in English
                    (AGiftBatchRow)ASubmitTable.Rows[Counter], ref AVerificationResult,
                    ValidationControlsDict);
            }
        }

        static partial void ValidateGiftDetailManual(TValidationControlsDict AValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable)
        {
            TValidationControlsDict ValidationControlsDict = new TValidationControlsDict();

            ValidationControlsDict.Add(ASubmitTable.Columns[AGiftDetailTable.ColumnGiftCommentOneId],
                new TValidationControlsData(null, AGiftDetailTable.GetGiftCommentOneDBName()));

            for (int Counter = 0; Counter < ASubmitTable.Rows.Count; Counter++)
            {
                TSharedFinanceValidation_Gift.ValidateGiftDetailManual("TTransactionWebConnector" +
                    " (Error in Row #" + Counter.ToString() + ")",  // No translation of message text since the server's messages should be all in English
                    (AGiftDetailRow)ASubmitTable.Rows[Counter], ref AVerificationResult,
                    ValidationControlsDict);
            }
        }
    }
}