//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;

namespace Ict.Petra.Shared.MFinance.Validation
{
    /// <summary>
    /// Contains helper functions for the shared validation of Finance data.
    /// </summary>
    public static class TSharedFinanceValidationHelper
    {
        /// <summary>
        /// Delegate for invoking the process of finding the valid start and end dates for the specified Ledger.
        /// </summary>
        public delegate bool TGetValidPostingDateRange(Int32 ALedgerNumber,
            out DateTime AStartDateCurrentPeriod,
            out DateTime AEndDateLastForwardingPeriod);

        /// <summary>
        /// Reference to the Delegate for invoking the verification of the existence of a Finance.
        /// </summary>
        private static TGetValidPostingDateRange FDelegateGetValidPostingDateRange;

        /// <summary>
        /// This property is used to provide a function which invokes the verification of the existence of a Finance.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TGetValidPostingDateRange GetValidPostingDateRangeDelegate
        {
            get
            {
                return FDelegateGetValidPostingDateRange;
            }

            set
            {
                FDelegateGetValidPostingDateRange = value;
            }
        }

        /// <summary>
        /// Get the valid posting date range for the specified ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AStartDateCurrentPeriod"></param>
        /// <param name="AEndDateLastForwardingPeriod"></param>
        /// <returns>true if dates are returned OK</returns>
        public static bool GetValidPostingDateRange(Int32 ALedgerNumber,
            out DateTime AStartDateCurrentPeriod,
            out DateTime AEndDateLastForwardingPeriod)
        {
            if (FDelegateGetValidPostingDateRange != null)
            {
                return FDelegateGetValidPostingDateRange(ALedgerNumber, out AStartDateCurrentPeriod, out AEndDateLastForwardingPeriod);
            }
            else
            {
                throw new InvalidOperationException("Delegate 'TGetValidPostingDateRange' must be initialised before calling this Method");
            }
        }
    }
}