//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Windows.Forms;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Gui.MFinance;

namespace Ict.Petra.Client.MFinance.Gui
{
    /// <summary>
    /// Elements of the old TfrmFinanceMain that don't have their own home
    /// </summary>
    public class TFinanceMain
    {
        /// Open screen for creating "Recipient by Field" Extract
        public static void RecipientByFieldExtract(Form AParentForm)
        {
            TFrmRecipientByField frm = new TFrmRecipientByField(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// Open screen for creating "Donor by Field" Extract
        public static void DonorByFieldExtract(Form AParentForm)
        {
            TFrmDonorByField frm = new TFrmDonorByField(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// Open screen for creating "Donor by Motivation" Extract
        public static void DonorByMotivationExtract(Form AParentForm)
        {
            TFrmDonorByMotivation frm = new TFrmDonorByMotivation(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// Open screen for creating "Donor by Amount" Extract
        public static void DonorByAmountExtract(Form AParentForm)
        {
            TFrmDonorByAmount frm = new TFrmDonorByAmount(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// Open screen for creating "Donor by Miscellaneous" Extract
        public static void DonorByMiscellaneousExtract(Form AParentForm)
        {
            TFrmDonorByMiscellaneous frm = new TFrmDonorByMiscellaneous(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }
    }
}