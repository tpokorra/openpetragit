﻿//
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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MCommon.Validation;

namespace Ict.Petra.Client.MCommon.Gui.Setup
{
    public partial class TFrmInternationalPostalTypeSetup
    {
        private void NewRowManual(ref PInternationalPostalTypeRow ARow)
        {
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.PInternationalPostalType.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PInternationalPostalType.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.InternatPostalTypeCode = newName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPInternationalPostalType();
        }
        
        private void DeleteRecord(Object sender, EventArgs e)
        {
        	DeletePInternationalPostalType();
        }
        
        
        private bool PreDeleteManual(ref PInternationalPostalTypeRow ARowToDelete, ref string ADeletionQuestion)
        {
        	return true;
        }

        private bool DeleteRowManual(ref PInternationalPostalTypeRow ARowToDelete, ref string ACompletionMessage)
        {
        	FPreviouslySelectedDetailRow.Delete();
        	ACompletionMessage = "Deletion has taken place";
        	return true;
        }

        private void PostDeleteManual(bool AAllowDeletion, bool ADeletionPerformed, ref PInternationalPostalTypeRow ARowToDelete)
        {
        	//Code to execute post delete
        	MessageBox.Show("Post Delete");
        }

    }
}