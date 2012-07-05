﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//       thomasw
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
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Person;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_IndividualData_Abilities
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private PtAbilityAreaTable FAbilityAreaNameDT;
        private PtAbilityLevelTable FAbilityLevelDT;

        #region Properties

        /// <summary>used for passing through the Clientside Proxy for the UIConnector</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }

        #endregion

        #region Events

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        #endregion

        /// <summary>
        /// todoComment
        /// </summary>
        public void SpecialInitUserControl(IndividualDataTDS AMainDS)
        {
            FMainDS = AMainDS;

            LoadDataOnDemand();

            FAbilityAreaNameDT = (PtAbilityAreaTable)TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.AbilityAreaList);

            // enable grid to react to insert and delete keyboard keys
            grdDetails.InsertKeyPressed += new TKeyPressedEventHandler(grdDetails_InsertKeyPressed);
            grdDetails.DeleteKeyPressed += new TKeyPressedEventHandler(grdDetails_DeleteKeyPressed);

            if (grdDetails.Rows.Count <= 1)
            {
                btnDelete.Enabled = false;
                pnlDetails.Visible = false;
            }
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            this.CreateNewPmPersonAbility();
        }

        private void NewRowManual(ref PmPersonAbilityRow ARow)
        {
            string newName;
            Int32 countNewDetail = 0;

            ARow.PartnerKey = FMainDS.PPerson[0].PartnerKey;
            newName = FAbilityAreaNameDT[0].AbilityAreaName;

            if (FMainDS.PmPersonAbility.Rows.Find(new object[] { ARow.PartnerKey, newName }) != null)
            {
                while (FMainDS.PmPersonAbility.Rows.Find(new object[] { ARow.PartnerKey, newName }) != null)
                {
                    countNewDetail++;
                    newName = FAbilityAreaNameDT[countNewDetail].AbilityAreaName;
                }
            }

            ARow.AbilityAreaName = newName;
        }

        private void DeleteRow(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            if (MessageBox.Show(String.Format(Catalog.GetString(
                            "You have choosen to delete this record ({0}).\n\nDo you really want to delete it?"),
                        FPreviouslySelectedDetailRow.AbilityAreaName), Catalog.GetString("Confirm Delete"),
                    MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                int rowIndex = grdDetails.SelectedRowIndex();
                FPreviouslySelectedDetailRow.Delete();
                FPetraUtilsObject.SetChangedFlag();

                // temporarily reset selected row to avoid interference with validation
                FPreviouslySelectedDetailRow = null;
                grdDetails.SelectRowInGrid(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);

                DoRecalculateScreenParts();

                if (grdDetails.Rows.Count <= 1)
                {
                    // hide details part and disable buttons if no record in grid (first row for headings)
                    btnDelete.Enabled = false;
                    pnlDetails.Visible = false;
                }
            }
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        private void ShowDetailsManual(PmPersonAbilityRow ARow)
        {
            if (ARow != null)
            {
                btnDelete.Enabled = true;
                pnlDetails.Visible = true;
            }

            // In theory, the next Method call could be done in Methods NewRowManual; however, NewRowManual runs before
            // the Row is actually added and this would result in the Count to be one too less, so we do the Method call here, short
            // of a non-existing 'AfterNewRowManual' Method....
            DoRecalculateScreenParts();
        }

        private void ShowAbilityLevelExplanation(System.Object sender, EventArgs e)
        {
            PtLanguageLevelRow LangLevelDR;

            if (FAbilityLevelDT == null)
            {
                FAbilityLevelDT = (PtAbilityLevelTable)TDataCache.TMPersonnel.GetCacheablePersonnelTable(
                    TCacheablePersonTablesEnum.LanguageLevelList);
            }

            LangLevelDR = (PtLanguageLevelRow)FAbilityLevelDT.Rows.Find(new object[] { Convert.ToInt32(cmbAbilityLevel.cmbCombobox.SelectedValue) });

            if (LangLevelDR != null)
            {
                MessageBox.Show(LangLevelDR.LanguageLevelDescr.Trim() + ":" + Environment.NewLine + LangLevelDR.LanguageComment,
                    Catalog.GetString("Ability Level Explanation"));
            }
            else
            {
                MessageBox.Show(String.Format(Catalog.GetString("There is no explanation available for Ability Level {0}."), cmbAbilityLevel.Text));
            }
        }

        /// <summary>
        /// Gets the data from all controls on this UserControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        public void GetDataFromControls2()
        {
            // Get data out of the Controls only if there is at least one row of data (Note: Column Headers count as one row)
            if (grdDetails.Rows.Count > 1)
            {
                GetDataFromControls();
            }
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        /// <summary>
        /// Loads Person Ability Data from Petra Server into FMainDS, if not already loaded.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.PmPersonAbility == null)
                {
                    FMainDS.Tables.Add(new PmPersonAbilityTable());
                    FMainDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading
                    && (FMainDS.PmPersonAbility.Rows.Count == 0))
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataPersonnelIndividualData(TIndividualDataItemEnum.idiPersonalAbilities));

                    // Make DataRows unchanged
                    if (FMainDS.PmPersonAbility.Rows.Count > 0)
                    {
                        if (FMainDS.PmPersonAbility.Rows[0].RowState != DataRowState.Added)
                        {
                            FMainDS.PmPersonAbility.AcceptChanges();
                        }
                    }
                }

                if (FMainDS.PmPersonAbility.Rows.Count != 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }

            return ReturnValue;
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_InsertKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            NewRow(this, null);
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_DeleteKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            if (e.Row != -1)
            {
                this.DeleteRow(this, null);
            }
        }
    }
}