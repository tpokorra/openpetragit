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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using GNU.Gettext;
using Ict.Petra.Client.CommonControls;
using Ict.Common.Controls;
using SourceGrid;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MPartner;
using Ict.Common;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// Partner New Dialog. Called from Partner Edit screen.
    /// </summary>
    public partial class TPartnerNewDialogWinForm : System.Windows.Forms.Form, IFrmPetra
    {
        #region Resourcestrings

        private static readonly string StrCantCreateNewPartner = Catalog.GetString(
            "New Partner can't be created because there are no Installed Sites available!\r\n" +
            "Please set up at least one Installed Site in the System Manager Module!");

        private static readonly string StrFamilyNeedsToBeSelected = Catalog.GetString(
            "A Family needs to be selected when a new Partner of Partner Class 'PERSON' should be created!");

        private static readonly string StrFamilyNeedsToBeSelectedTitle = Catalog.GetString("Family Needed!");

        private static readonly string StrCorrectFamilyKeyNeedsToBeEntered = Catalog.GetString(
            "The correct key of an existing family needs to be entered when a new Partner of Partner Class 'PERSON' should be created!");

        private static readonly string StrCorrectFamilyKeyNeedsToBeEnteredTitle = Catalog.GetString("A correct family key needs to be entered!");

        private static readonly string StrAPartnerKeyExists = Catalog.GetString(
            "A Partner with Partner Key {0} already exists.\r\nPlease choose a different Partner Key!");

        private static readonly string StrAPartnerKeyExistsTitle = Catalog.GetString("Partner Key already in use");

        #endregion

        private TPartnerNewDialogScreenLogic FLogic;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private Int64 FPartnerKey;
        private Int64 FSiteKey;
        private String FPartnerClass;
        private String FAcquisitionCode;
        private Int64 FFamilyPartnerKey;
        private Int32 FFamilyLocationKey;
        private Int64 FFamilySiteKey;
        private Boolean FPrivatePartner;
        private DataTable FInstalledSitesDT;
        private Boolean FDataGridRowEnteredRepeatedly;
        private Boolean FFamilyPartnerSelectionSetup;
        private Boolean FFormSetupFinished = false;

        private void DataGrid_FocusRowEntered(System.Object ASender, RowEventArgs AEventArgs)
        {
            Int64 NewSiteKey;

            NewSiteKey = FLogic.DetermineCurrentSitePartnerKey(grdInstalledSites);

            // different SiteKey? If yes, retrieve the next available Partner Key for the Site
            if ((NewSiteKey != FSiteKey) || (!FDataGridRowEnteredRepeatedly))
            {
                FPartnerKey = FPartnerEditUIConnector.GetPartnerKeyForNewPartner(NewSiteKey);
                txtPartnerKey.PartnerKey = FPartnerKey;
                FSiteKey = NewSiteKey;
                FDataGridRowEnteredRepeatedly = true;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        /// <param name="AFamilyPartnerKey"></param>
        /// <param name="AFamilyLocationKey"></param>
        /// <param name="AFamilySiteKey"></param>
        /// <returns></returns>
        public Boolean GetReturnedParameters(out String APartnerClass,
            out System.Int64 ASiteKey,
            out System.Int64 APartnerKey,
            out String AAcquisitionCode,
            out Boolean APrivatePartner,
            out Int64 AFamilyPartnerKey,
            out Int32 AFamilyLocationKey,
            out Int64 AFamilySiteKey)
        {
            Boolean ReturnValue;

            APartnerClass = "";
            AAcquisitionCode = "";
            ASiteKey = -1;
            APartnerKey = -1;
            APrivatePartner = false;
            AFamilyPartnerKey = -1;
            AFamilyLocationKey = -1;
            AFamilySiteKey = -1;

            if (FFormSetupFinished)
            {
                APartnerClass = cmbPartnerClass.GetSelectedString();
                AAcquisitionCode = cmbAcquisitionCode.GetSelectedString();
                ASiteKey = FSiteKey;
                APartnerKey = FPartnerKey;
                APrivatePartner = chkPrivatePartner.Checked;
                AFamilyPartnerKey = FFamilyPartnerKey;
                AFamilyLocationKey = FFamilyLocationKey;
                AFamilySiteKey = FFamilySiteKey;
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TPartnerNewDialogWinForm(Form AParentForm) : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.btnOK.Text = Catalog.GetString("&OK");
            this.btnCancel.Text = Catalog.GetString("&Cancel");
            this.btnHelp.Text = Catalog.GetString("&Help");
            this.lblSitesAvailable.Text = Catalog.GetString("S&ites Available:");
            this.lblPartnerKey.Text = Catalog.GetString("Partner &Key:");
            this.lblPartnerClass.Text = Catalog.GetString("Partner C&lass:");
            this.Label1.Text = Catalog.GetString("&Acquisition Code:");
            this.chkPrivatePartner.Text = Catalog.GetString("&Private Partner:");
            this.txtPartnerKey.LabelText = Catalog.GetString("Partner Key");
            this.txtFamilyPartnerBox.ButtonText = Catalog.GetString("&Family...");
            this.Text = Catalog.GetString("New Partner");
            #endregion

            FPetraUtilsObject = new TFrmPetraUtils(AParentForm, this, stbMain);
            this.FPetraUtilsObject.SetStatusBarText(this.btnOK, Catalog.GetString("Accept data and continue"));
            this.FPetraUtilsObject.SetStatusBarText(this.btnCancel, Catalog.GetString("Cancel data entry and close"));
            this.FPetraUtilsObject.SetStatusBarText(this.btnHelp, Catalog.GetString("Help"));
            this.FPetraUtilsObject.SetStatusBarText(this.cmbAcquisitionCode, Catalog.GetString("Please select an Acquisition Code"));
            this.FPetraUtilsObject.SetStatusBarText(this.grdInstalledSites, Catalog.GetString("Please select a Site"));
            this.FPetraUtilsObject.SetStatusBarText(this.cmbPartnerClass, Catalog.GetString("Please select a Partner Class"));
            this.FPetraUtilsObject.SetStatusBarText(this.txtPartnerKey,
                Catalog.GetString("Please enter a Partner Key or Accept the default Partner Key"));
            this.FPetraUtilsObject.SetStatusBarText(this.txtFamilyPartnerBox,
                Catalog.GetString("Please select a Family that the Person should belong to"));

            FFormSetupFinished = false;
        }

        private TFrmPetraUtils FPetraUtilsObject;

        private void TxtFamilyPartnerBox_PartnerFound(Int64 APartnerKey, int ALocationKey)
        {
            FFamilyPartnerKey = APartnerKey;
            FFamilyLocationKey = ALocationKey;

            // todo: return the sitekey of the family; at the moment only the FIXED_SITE_KEY is used for families
            FFamilySiteKey = SharedConstants.FIXED_SITE_KEY;
        }

        private void CmbPartnerClass_SelectedValueChanged(System.Object Sender, System.EventArgs e)
        {
            if (FFormSetupFinished)
            {
                if (cmbPartnerClass.GetSelectedString() == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                {
                    ShowFamilyPartnerSelection(true);
                }
                else
                {
                    ShowFamilyPartnerSelection(false);
                }
            }
        }

        private void ShowFamilyPartnerSelection(Boolean AShow)
        {
            if (AShow)
            {
                if (!FFamilyPartnerSelectionSetup)
                {
                    if (FFamilyPartnerKey != -1)
                    {
                        txtFamilyPartnerBox.Text = FFamilyPartnerKey.ToString();
                    }

                    FFamilyPartnerSelectionSetup = true;
                }
            }

            txtFamilyPartnerBox.Visible = AShow;
        }

        private void BtnHelp_Click(System.Object sender, System.EventArgs e)
        {
            // TODO TCommonMenuCommands.PetraHelp.AboutAndHelp(this);
        }

        private void BtnOK_Click(System.Object sender, System.EventArgs e)
        {
            Int64 NewPartnerKey;
            String NewPartnerClass;

            NewPartnerClass = cmbPartnerClass.GetSelectedString();

            if (NewPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
            {
                if (TAppSettingsManager.GetValue("AllowCreationPersonRecords", "true", false).ToLower() != "true")
                {
                    MessageBox.Show("We are planning to change the Person and Family system to something more easy to understand." +
                        Environment.NewLine +
                        "To avoid problems upgrading your database, please create a FAMILY partner rather than a PERSON partner!" +
                        Environment.NewLine +
                        "Otherwise, please add a parameter AllowCreationPersonRecords with value true to your config files.",
                        "NO CREATION OF PERSONS AT THE MOMENT",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                FFamilyPartnerKey = Convert.ToInt32(txtFamilyPartnerBox.Text);

                if (FFamilyPartnerKey == 0)
                {
                    MessageBox.Show(StrFamilyNeedsToBeSelected, StrFamilyNeedsToBeSelectedTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtFamilyPartnerBox.Focus();
                    return;
                }
                else //check if a family with the given familyPartnerKey exists; if this is not the case then diplay a message box
                {
                    TPartnerClass[] AValidPartnerClasses = new TPartnerClass[1];
                    AValidPartnerClasses[0] = TPartnerClass.FAMILY;
                    bool APartnerExists;
                    String APartnerShortName;
                    TPartnerClass APartnerClass;
                    Boolean AIsMergedPartner;

                    if (!TServerLookup.TMPartner.VerifyPartner(FFamilyPartnerKey, AValidPartnerClasses,
                            out APartnerExists,
                            out APartnerShortName,
                            out APartnerClass,
                            out AIsMergedPartner))
                    {
                        MessageBox.Show(StrCorrectFamilyKeyNeedsToBeEntered,
                            StrCorrectFamilyKeyNeedsToBeEnteredTitle,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        txtFamilyPartnerBox.Focus();
                        return;
                    }
                }
            }

            NewPartnerKey = txtPartnerKey.PartnerKey;

            if (FPartnerEditUIConnector.SubmitPartnerKeyForNewPartner(FSiteKey, FPartnerKey, ref NewPartnerKey))
            {
                FPartnerKey = NewPartnerKey;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(String.Format(StrAPartnerKeyExists, txtPartnerKey.PartnerKey, StrAPartnerKeyExistsTitle));
                txtPartnerKey.Focus();
            }
        }

        private void InitialiseUI()
        {
            if (FPartnerClass != "")
            {
                cmbPartnerClass.SetSelectedString(FPartnerClass);
            }
            else
            {
                // Default value: FAMILY
                cmbPartnerClass.SetSelectedString("FAMILY");
            }

            if (FAcquisitionCode != "")
            {
                cmbAcquisitionCode.SetSelectedString(FAcquisitionCode);
            }
            else
            {
                // Default value: UserDefault PARTNER_ACQUISITIONCODE
                cmbAcquisitionCode.SetSelectedString(TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_ACQUISITIONCODE, "MAILROOM"));
            }

            // Default value: false (from SetParameters default)
            chkPrivatePartner.Checked = FPrivatePartner;

            if (FPartnerKey != -1)
            {
                txtPartnerKey.PartnerKey = FPartnerKey;

                // suppress setting of PartnerKey through DataGrid_FocusRowEntered...
                FDataGridRowEnteredRepeatedly = true;
            }
        }

        private void TPartnerNewDialogWinForm_Load(System.Object sender, System.EventArgs e)
        {
            FLogic = new TPartnerNewDialogScreenLogic();
            cmbPartnerClass.InitialiseUserControl();
            cmbAcquisitionCode.InitialiseUserControl();

            // Hide invalid Acquisition Codes
            this.cmbAcquisitionCode.Filter = PAcquisitionTable.GetValidAcquisitionDBName() + " <> 0";

            // Setup Data
            FInstalledSitesDT = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.InstalledSitesList);

            if (FInstalledSitesDT.Rows.Count != 0)
            {
                FLogic.CreateColumns(grdInstalledSites, FInstalledSitesDT);

                // DataBindingrelated stuff
                SetupDataGridDataBinding();

                // Setup screen with default values
                InitialiseUI();

                if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                {
                    ShowFamilyPartnerSelection(true);
                }

                // Make the Grid respond on updown keys
                cmbPartnerClass.Focus();
                grdInstalledSites.Focus();

                // make sure that the grid row gets selected; Mono would not do it automatically
                DataGrid_FocusRowEntered(null, null);

                FFormSetupFinished = true;
            }
            else
            {
                MessageBox.Show(StrCantCreateNewPartner, MCommonResourcestrings.StrErrorNoInstalledSites);
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
                return;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        /// <param name="AFamilyPartnerKey"></param>
        /// <param name="AFamilyLocationKey"></param>
        /// <param name="AFamilySiteKey"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String AAcquisitionCode,
            Boolean APrivatePartner,
            Int64 AFamilyPartnerKey,
            Int32 AFamilyLocationKey,
            Int64 AFamilySiteKey)
        {
            FPartnerEditUIConnector = APartnerEditUIConnector;
            FPartnerKey = APartnerKey;
            FSiteKey = ASiteKey;
            FPartnerClass = APartnerClass;
            FAcquisitionCode = AAcquisitionCode;
            FPrivatePartner = APrivatePartner;
            FFamilyPartnerKey = AFamilyPartnerKey;
            FFamilyLocationKey = AFamilyLocationKey;
            FFamilySiteKey = AFamilySiteKey;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        /// <param name="AFamilyPartnerKey"></param>
        /// <param name="AFamilyLocationKey"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String AAcquisitionCode,
            Boolean APrivatePartner,
            Int64 AFamilyPartnerKey,
            Int32 AFamilyLocationKey)
        {
            SetParameters(APartnerEditUIConnector,
                APartnerClass,
                ASiteKey,
                APartnerKey,
                AAcquisitionCode,
                APrivatePartner,
                AFamilyPartnerKey,
                AFamilyLocationKey,
                -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        /// <param name="AFamilyPartnerKey"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String AAcquisitionCode,
            Boolean APrivatePartner,
            Int64 AFamilyPartnerKey)
        {
            SetParameters(APartnerEditUIConnector, APartnerClass, ASiteKey, APartnerKey, AAcquisitionCode, APrivatePartner, AFamilyPartnerKey, -1, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String AAcquisitionCode,
            Boolean APrivatePartner)
        {
            SetParameters(APartnerEditUIConnector, APartnerClass, ASiteKey, APartnerKey, AAcquisitionCode, APrivatePartner, -1, -1, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAcquisitionCode"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String AAcquisitionCode)
        {
            SetParameters(APartnerEditUIConnector, APartnerClass, ASiteKey, APartnerKey, AAcquisitionCode, false, -1, -1, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey)
        {
            SetParameters(APartnerEditUIConnector, APartnerClass, ASiteKey, APartnerKey, "", false, -1, -1, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector, String APartnerClass, System.Int64 ASiteKey)
        {
            SetParameters(APartnerEditUIConnector, APartnerClass, ASiteKey, -1, "", false, -1, -1, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector, String APartnerClass)
        {
            SetParameters(APartnerEditUIConnector, APartnerClass, -1, -1, "", false, -1, -1, -1);
        }

        #region Setup SourceDataGrid
        private void SetupDataGridDataBinding()
        {
            DataView InstalledSitesDV;
            Int32 BestSiteRowNumber;

            InstalledSitesDV = FInstalledSitesDT.DefaultView;
            InstalledSitesDV.AllowNew = false;
            InstalledSitesDV.AllowDelete = false;
            InstalledSitesDV.AllowEdit = false;

            // InstalledSitesDV.Sort := 'p_partner_short_name_c ASC';   for testing purposes only
            // DataBind the DataGrid
            grdInstalledSites.DataSource = new DevAge.ComponentModel.BoundDataView(InstalledSitesDV);

            // Hook up event that fires when a different Row is selected
            grdInstalledSites.Selection.FocusRowEntered += new RowEventHandler(this.DataGrid_FocusRowEntered);

            // Determine the Row that contains 'Best Address' > this Row should be initially selected
            FLogic.DetermineInitiallySelectedSite((grdInstalledSites.DataSource as DevAge.ComponentModel.BoundDataView).DataView,
                FSiteKey,
                out BestSiteRowNumber,
                out FSiteKey);

            // MessageBox.Show('BestSiteRowNumber: ' + BestSiteRowNumber.ToString +
            // #13#10'BestSiteKey: ' + FSiteKey.ToString);

            // Select Row that contains 'Best Site'
            grdInstalledSites.Selection.SelectRow(BestSiteRowNumber, true);
        }

        #endregion

        /// <summary>
        /// implemented for IFrmPetraEdit
        /// </summary>
        /// <returns></returns>
        public bool CanClose()
        {
            return FPetraUtilsObject.CanClose();
        }

        /// <summary>
        /// implemented for IFrmPetra
        /// </summary>
        public void RunOnceOnActivation()
        {
        }

        /// <summary>
        /// implemented for IFrmPetra
        /// </summary>
        public TFrmPetraUtils GetPetraUtilsObject()
        {
            return (TFrmPetraUtils)FPetraUtilsObject;
        }
    }
}