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
using Ict.Petra.Client.MReporting.Gui.MPersonnel;

namespace Ict.Petra.Client.MPersonnel.Gui
{
    /// <summary>
    /// Methods of the former TFrmPersonnelMain that don't have a home elsewhere
    /// </summary>
    public class TPersonnelMain
    {
        /// <summary>
        /// open screen to create "Conference" Extract
        /// </summary>
        public static void PartnerByConferenceExtract(Form AParentForm)
        {
            TFrmPartnerByEvent frm = new TFrmPartnerByEvent(AParentForm);

            frm.CalledFromExtracts = true;
            frm.CalledForConferences = true;
            frm.Show();
        }

        /// <summary>
        /// open screen to create "Outreach" Extract
        /// </summary>
        public static void PartnerByOutreachExtract(Form AParentForm)
        {
            TFrmPartnerByEvent frm = new TFrmPartnerByEvent(AParentForm);

            frm.CalledFromExtracts = true;
            frm.CalledForConferences = false;
            frm.Show();
        }

        /// Open screen for creating "Partner by Commitment" Extract
        public static void PartnerByCommitmentExtract(Form AParentForm)
        {
            TFrmPartnerByCommitmentExtract frm = new TFrmPartnerByCommitmentExtract(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// Open screen for creating "Partner by Field" Extract
        public static void PartnerByFieldExtract(Form AParentForm)
        {
            TFrmPartnerByField frm = new TFrmPartnerByField(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }
    }
}