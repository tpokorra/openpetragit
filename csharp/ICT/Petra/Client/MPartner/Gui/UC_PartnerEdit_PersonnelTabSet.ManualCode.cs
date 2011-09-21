﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_PartnerEdit_PersonnelTabSet
    {
        #region Fields

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private TPartnerEditTabPageEnum FInitiallySelectedTabPage = TPartnerEditTabPageEnum.petpDefault;

        private TPartnerEditTabPageEnum FCurrentlySelectedTabPage;

        private Boolean FUserControlInitialised;

        #endregion
                
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

        /// <summary>todoComment</summary>
        public TPartnerEditTabPageEnum InitiallySelectedTabPage
        {
            get
            {
                return FInitiallySelectedTabPage;
            }

            set
            {
                FInitiallySelectedTabPage = value;
            }
        }

        /// <summary>todoComment</summary>
        public TPartnerEditTabPageEnum CurrentlySelectedTabPage
        {
            get
            {
                return FCurrentlySelectedTabPage;
            }

            set
            {
                FCurrentlySelectedTabPage = value;
            }
        }

        #endregion

        #region Public Events

//        /// <summary>todoComment</summary>
//        public event THookupDataChangeEventHandler HookupDataChange;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupPartnerEditDataChange;

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Initialization of Manual Code logic.
        /// </summary>
        public void InitializeManualCode()
        {
            if (FTabPageEvent == null)
            {
                FTabPageEvent += this.TabPageEventHandler;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void SpecialInitUserControl()
        {
            OnDataLoadingStarted();

            FUserControlInitialised = true;

//            this.tabPartners.SelectedIndexChanged += new EventHandler(this.tabPartners_SelectedIndexChanged);

            SelectTabPage(FInitiallySelectedTabPage);

            CalculateTabHeaderCounters(this);

            OnDataLoadingFinished();
        }

        /// <summary>
        /// Gets the data from all controls on this TabControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        public void GetDataFromControls()
        {
//            switch (GetPartnerDetailsVariableUC())
//            {
//                case TDynamicLoadableUserControls.dlucPartnerDetailsPerson:
//
//                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsPerson))
//                    {
//                        TUC_PartnerDetails_Person UCPartnerDetailsPerson =
//                            (TUC_PartnerDetails_Person)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsPerson];
//                        UCPartnerDetailsPerson.GetDataFromControls2();
//                    }
//
//                    break;
//
//                case TDynamicLoadableUserControls.dlucPartnerDetailsFamily:
//
//                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsFamily))
//                    {
//                        TUC_PartnerDetails_Family UCPartnerDetailsFamily =
//                            (TUC_PartnerDetails_Family)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsFamily];
//                        UCPartnerDetailsFamily.GetDataFromControls2();
//                    }
//
//                    break;
//
//                case TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation:
//
//                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation))
//                    {
//                        TUC_PartnerDetails_Organisation UCPartnerDetailsOrganisation =
//                            (TUC_PartnerDetails_Organisation)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation];
//                        UCPartnerDetailsOrganisation.GetDataFromControls2();
//                    }
//
//                    break;
//
//                case TDynamicLoadableUserControls.dlucPartnerDetailsChurch:
//
//                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsChurch))
//                    {
//                        TUC_PartnerDetails_Church UCPartnerDetailsChurch =
//                            (TUC_PartnerDetails_Church)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsChurch];
//                        UCPartnerDetailsChurch.GetDataFromControls2();
//                    }
//
//                    break;
//
//                case TDynamicLoadableUserControls.dlucPartnerDetailsUnit:
//
//                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsUnit))
//                    {
//                        TUC_PartnerDetails_Unit UCPartnerDetailsUnit =
//                            (TUC_PartnerDetails_Unit)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsUnit];
//                        UCPartnerDetailsUnit.GetDataFromControls2();
//                    }
//
//                    break;
//
//                case TDynamicLoadableUserControls.dlucPartnerDetailsVenue:
//
//                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsVenue))
//                    {
//                        TUC_PartnerDetails_Venue UCPartnerDetailsVenue =
//                            (TUC_PartnerDetails_Venue)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsVenue];
//                        UCPartnerDetailsVenue.GetDataFromControls2();
//                    }
//
//                    break;
//
//                case TDynamicLoadableUserControls.dlucPartnerDetailsBank:
//
//                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsBank))
//                    {
//                        TUC_PartnerDetails_Bank UCPartnerDetailsBank =
//                            (TUC_PartnerDetails_Bank)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsBank];
//                        UCPartnerDetailsBank.GetDataFromControls2();
//                    }
//
//                    break;
//            }
        }

        #endregion
        
        #region Private Methods
        
        private void TabPageEventHandler(object sender, TTabPageEventArgs ATabPageEventArgs)
        {
            if (ATabPageEventArgs.Event == "InitialActivation")
            {
                if (ATabPageEventArgs.Tab == tpgIndividualData)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpAddresses;

//                    FUcoIndividualData.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoIndividualData.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);

                    FUcoIndividualData.InitialiseUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgApplications)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpPartnerTypes;

                    // Hook up RecalculateScreenParts Event
                    FUcoApplications.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

//                    FUcoApplications.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoApplications.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);

//                    FUcoApplications.SpecialInitUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
            }
        }

        private void RecalculateTabHeaderCounters(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            // MessageBox.Show('TUC_PartnerEdit_PartnerTabSet2.RecalculateTabHeaderCounters');
            if (e.ScreenPart == TScreenPartEnum.spCounters)
            {
                CalculateTabHeaderCounters(sender);
            }
        }

        private void CalculateTabHeaderCounters(System.Object ASender)
        {
            DataView TmpDV;
            string DynamicTabTitle;
            string DynamicToolTipPart1;
            Int32 CountAll;
            Int32 CountActive;

            if ((ASender is TUC_PartnerEdit_PersonnelTabSet) || (ASender is TUC_Applications))
            {
                // TODO
//                tpgApplications.Text = String.Format(StrApplicationsTabHeader + " ({0})", CountActive);

            }
        }

        /// <summary>
        /// Changed data (eg. caused by the data saving process) will make a databound SourceGrid redraw,
        /// and through that it can get it's size wrong and appear too wide if the user has
        /// a non-standard display setting, (eg. "Large Fonts (120DPI).
        /// This Method fixes that by calling the 'AdjustAfterResizing' Method in Tabs that
        /// host a SourceGrid.
        /// </summary>
        private void CorrectDataGridWidthsAfterDataChange()
        {
            if (TClientSettings.GUIRunningOnNonStandardDPI)
            {
                if (FUcoIndividualData != null)
                {
                    FUcoIndividualData.AdjustAfterResizing();
                }

                if (FUcoApplications != null)
                {
                    FUcoApplications.AdjustAfterResizing();
                }
            }
        }

//        private void Uco_HookupDataChange(System.Object sender, System.EventArgs e)
//        {
//            if (HookupDataChange != null)
//            {
//                HookupDataChange(this, e);
//            }
//        }
        
        private void Uco_HookupPartnerEditDataChange(System.Object sender, THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupPartnerEditDataChange != null)
            {
                HookupPartnerEditDataChange(this, e);
            }
        }        
        
        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATabPage"></param>
        public void SelectTabPage(TPartnerEditTabPageEnum ATabPage)
        {
            TabPage SelectedTabPageBeforeReSelecting;

            if (!FUserControlInitialised)
            {
                throw new ApplicationException("SelectTabPage must not be called if the UserControl is not yet initialised");
            }

            OnDataLoadingStarted();

            // supress detection changing
            SelectedTabPageBeforeReSelecting = tabPersonnel.SelectedTab;

            switch (ATabPage)
            {
                case TPartnerEditTabPageEnum.petpPersonnelIndividualData:
                    tabPersonnel.SelectedTab = tpgIndividualData;
                    break;

                case TPartnerEditTabPageEnum.petpPersonnelApplications:
                    tabPersonnel.SelectedTab = tpgApplications;
                    break;

            }

            // Check if the selected TabPage actually changed...
            if (SelectedTabPageBeforeReSelecting == tabPersonnel.SelectedTab)
            {
                // Tab was already selected; therefore raise the SelectedIndexChanged Event 'manually', which did not get raised by selecting the Tab again!
                TabSelectionChanged(this, new System.EventArgs());
            }

            OnDataLoadingFinished();
        }
        
        #endregion
    }
}
