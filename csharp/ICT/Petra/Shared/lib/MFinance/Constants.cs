//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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

namespace Ict.Petra.Shared.MFinance
{
    /// <summary>
    /// some constants used in the finance module
    /// </summary>
    public class MFinanceConstants
    {
        /// <summary>GL Batch</summary>
        public const String BATCH_UNPOSTED = "Unposted";

        /// <summary>GL Batch</summary>
        public const String BATCH_POSTED = "Posted";

        /// <summary>GL Batch</summary>
        public const String BATCH_CANCELLED = "Cancelled";

        /// <summary>GL Batch</summary>
        public const String BATCH_HAS_TRANSACTIONS = "HasTransactions";

        /// <summary>GL Batch</summary>
        public const String TRANSACTION_REVAL = "REVAL";

        /// <summary>GL Batch</summary>
        public const String TRANSACTION_FX_REVAL = "FX REVAL";

        /// <summary>GL Batch</summary>
        public const String TRANSACTION_STD = "STD";

        /// <summary>GL Batch</summary>
        public const String TRANSACTION_ALLOC = "ALLOC";

        /// <summary>GL Batch</summary>
        public const String TRANSACTION_REALLOC = "REALLOC";

        /// <summary>GL Batch</summary>
        public const String TRANSACTION_GIFT = "GR";

        /// <summary>GL Batch</summary>
        public const String TRANSACTION_AP = "INV";

        /// <summary>General Ledger</summary>
        public const String ACCOUNT_HIERARCHY_STANDARD = "STANDARD";

        /// <summary>Accounts Payable</summary>
        public const String AP_DOCUMENT_OPEN = "OPEN";

        /// <summary>Accounts Payable</summary>
        public const String AP_DOCUMENT_APPROVED = "APPROVED";

        /// <summary>Accounts Payable</summary>
        public const String AP_DOCUMENT_POSTED = "POSTED";

        /// <summary>Accounts Payable</summary>
        public const String AP_DOCUMENT_PARTIALLY_PAID = "PARTPAID";

        /// <summary>Accounts Payable</summary>
        public const String AP_DOCUMENT_PAID = "PAID";

        /// <summary>Account Property</summary>
        public const String ACCOUNT_PROPERTY_BANK_ACCOUNT = "Bank Account";

        /// <summary>Bank statements</summary>
        public const String BANK_STMT_STATUS_MATCHED = "MATCHED";

        /// <summary>Bank statements</summary>
        public const String BANK_STMT_STATUS_MATCHED_GIFT = "MATCHED-GIFT";

        /// <summary>Bank statements</summary>
        public const String BANK_STMT_STATUS_MATCHED_GL = "MATCHED-GL";

        /// <summary>Bank statements</summary>
        public const String BANK_STMT_STATUS_MATCHED_AP = "MATCHED-AP";

        /// <summary>Bank statements</summary>
        public const String BANK_STMT_STATUS_NO_MATCHING = "DONT-MATCH";

        /// <summary>Bank statements</summary>
        public const String BANK_STMT_STATUS_UNMATCHED = "UNMATCHED";

        /// <summary>Sub Systems, General Ledger</summary>
        public const String SUB_SYSTEM_GL = "GL";

        /// <summary>Sub Systems, Accounts Payable</summary>
        public const String SUB_SYSTEM_AP = "AP";

        /// <summary>Sub Systems, Accounts Receivable</summary>
        public const String SUB_SYSTEM_AR = "AR";

        /// <summary>Sub Systems, Gifts receivable</summary>
        public const String SUB_SYSTEM_GR = "GR";

        /// <summary>Unit-Type is key-min</summary>
        public const String GROUP_DETAIL_KEY_MIN = "KEYMIN";
        
        /// <summary>Used in Admin Grants Payable and Receivable</summary>
        public const int ADMIN_FEE_INCOME_ACCT = 3400;

        /// <summary>Used in Admin Grants Payable and Receivable</summary>
        public const int ADMIN_FEE_EXPENSE_ACCT = 4900;

    }
}