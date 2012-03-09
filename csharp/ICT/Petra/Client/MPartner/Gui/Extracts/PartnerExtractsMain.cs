//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Windows.Forms;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Gui.MPartner;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    /// <summary>
    /// the manually written part of TFrmPartnerMain
    /// </summary>
    public class TPartnerExtractsMain
    {
        /// <summary>
        /// open screen to create Publication Extract
        /// </summary>
        public static void PartnerBySubscriptionExtract(Form AParentForm)
        {
            TFrmPartnerBySubscription frm = new TFrmPartnerBySubscription(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// open screen to create "Partner by City" Extract
        /// </summary>
        public static void PartnerByCityExtract(Form AParentForm)
        {
            TFrmPartnerByCity frm = new TFrmPartnerByCity(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// open screen to create "Partner by Special Type" Extract
        /// </summary>
        public static void PartnerBySpecialTypeExtract(Form AParentForm)
        {
            TFrmPartnerBySpecialType frm = new TFrmPartnerBySpecialType(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }
    }
}