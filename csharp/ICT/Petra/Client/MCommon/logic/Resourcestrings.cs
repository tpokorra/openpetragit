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

using Ict.Common;


namespace Ict.Petra.Client.MCommon
{
    /// <summary>
    /// Contains resourcetexts that are used across several Petra Modules.
    /// </summary>
    public class MCommonResourcestrings
    {
        /// <summary>todoComment</summary>
        public static readonly string StrGenericInfo = Catalog.GetString("Information");

        /// <summary>todoComment</summary>
        public static readonly string StrGenericWarning = Catalog.GetString("Warning");

        /// <summary>todoComment</summary>
        public static readonly string StrGenericError = Catalog.GetString("Error");

        /// <summary>todoComment</summary>
        public static readonly string StrGenericReady = Catalog.GetString("Ready");

        /// <summary>todoComment</summary>
        public static readonly string StrDetails = Catalog.GetString("Details: ");

        /// <summary>todoComment</summary>
        public static readonly string StrGenericInactiveCode = Catalog.GetString(" (inactive)");

        /// <summary>todoComment</summary>
        public static readonly string StrGenericFunctionalityNotAvailable = Catalog.GetString("Functionality not available");

        /// <summary>todoComment</summary>
        public static readonly string StrFormHasUnsavedChanges = Catalog.GetString("This window has changes that have not been saved.");

        /// <summary>todoComment</summary>
        public static readonly string StrFormHasUnsavedChangesQuestion = Catalog.GetString("Save changes before closing?");

        /// <summary>Shown while data is being saved.</summary>
        public static readonly string StrSavingDataInProgress = Catalog.GetString("Saving data...");

        /// <summary>Shown when data was saved successfully.</summary>
        public static readonly string StrSavingDataSuccessful = Catalog.GetString("Data successfully saved.");

        /// <summary>Shown when saving of data failed.</summary>
        public static readonly string StrSavingDataException = Catalog.GetString("Data could not be saved because an unexpected error occured!");

        /// <summary>todoComment</summary>
        public static readonly string StrSavingDataErrorOccured = Catalog.GetString("Data could not be saved because an error occured!");

        /// <summary>todoComment</summary>
        public static readonly string StrSavingDataCancelled = Catalog.GetString("Saving of data cancelled by user!");

        /// <summary>Shown when no data needs saving.</summary>
        public static readonly string StrSavingDataNothingToSave = Catalog.GetString("There was nothing to be saved.");

        /// <summary>todoComment</summary>
        public static readonly string StrPetraServerTooBusy = Catalog.GetString("The OpenPetra Server is currently too busy to {0}.\r\n\r\n" +
            "Please wait a few seconds and press 'Retry' then to retry, or 'Cancel' to abort.");

        /// <summary>todoComment</summary>
        public static readonly string StrPetraServerTooBusyTitle = Catalog.GetString("OpenPetra Server Too Busy");

        /// <summary>todoComment</summary>
        public static readonly string StrOpeningCancelledByUser = Catalog.GetString("Opening of {0} screen got cancelled by user.");

        /// <summary>todoComment</summary>
        public static readonly string StrOpeningCancelledByUserTitle = Catalog.GetString("Screen opening cancelled");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorOnlyForPerson = Catalog.GetString("This only works for Partners of Partner Class PERSON");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorOnlyForFamilyOrPerson = Catalog.GetString(
            "This only works for Partners of Partner Classes FAMILY or PERSON");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorOnlyForPersonOrUnit = Catalog.GetString(
            "This only works for Partners of Partner Classes PERSON or UNIT");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorNoInstalledSites = Catalog.GetString("No Installed Sites!");

        /// <summary>todoComment</summary>
        public static readonly string StrBtnTextNew = Catalog.GetString("&New");

        /// <summary>todoComment</summary>
        public static readonly string StrBtnTextEdit = Catalog.GetString("Edi&t");

        /// <summary>todoComment</summary>
        public static readonly string StrBtnTextDelete = Catalog.GetString("&Delete");
 
        /// <summary>todoComment</summary>
        public static readonly string StrBtnTextDone = Catalog.GetString("D&one");

        /// <summary>todoComment</summary>
        public static readonly string StrPartnerStatusChange = Catalog.GetString("The Partner's Status is currently" + " '{0}'.\r\n" +
            "OpenPetra will change it automatically to '{1}'");

        /// <summary>todoComment</summary>
        public static readonly string StrPartnerReActivationTitle = Catalog.GetString("Partner Gets Re-activated!");

        /// <summary>todoComment</summary>
        public static readonly string StrValueUnassignable = Catalog.GetString("Unassignable Value");

        /// <summary>todoComment</summary>
        public const String StrCtrlSuppressChangeDetection = "SuppressChangeDetection";
    }
}