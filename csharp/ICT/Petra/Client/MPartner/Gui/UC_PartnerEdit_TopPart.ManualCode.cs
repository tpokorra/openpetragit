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
using System.Windows.Forms;

using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MPartner.Verification;
using GNU.Gettext;
using Ict.Common;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>Delegate declaration</summary>
    public delegate void TDelegateMaintainWorkerField();

    /// <summary>
    /// Event Arguments declaration
    /// </summary>
    public class TPartnerClassMainDataChangedEventArgs : System.EventArgs
    {
        /// <summary>todoComment</summary>
        public String PartnerClass;
    }

    /// <summary>Event handler declaration</summary>
    public delegate void TPartnerClassMainDataChangedHandler(System.Object Sender, TPartnerClassMainDataChangedEventArgs e);


    public partial class TUC_PartnerEdit_TopPart
    {
        #region Fields

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private String FPartnerClass;

        private DataView FPartnerDefaultView;

        // <summary>
        // Delegate for telling the Partner Edit screen that the 'Worker Field...' button has been clicked.
        // </summary>
        // <remarks>The Partner Edit screen acts on that Delegate and opens the corresponding screen.</remarks>
        private TDelegateMaintainWorkerField FDelegateMaintainWorkerField;

        #endregion

        #region Events

        /// <summary>
        /// This Event is thrown when the 'main data' of a DataTable for a certain
        /// PartnerClass has changed.
        /// </summary>
        public event TPartnerClassMainDataChangedHandler PartnerClassMainDataChanged;

        #endregion

        #region Properties

        /// <summary>UIConnector that the screen uses</summary>
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

        /// <summary>Holds verification results.</summary>
        public TVerificationResultCollection VerificationResultCollection
        {
            get
            {
                return FPetraUtilsObject.VerificationResultCollection;
            }

            set
            {
                FPetraUtilsObject.VerificationResultCollection = value;
            }
        }

        #endregion


        #region Public Methods

        /// arrange the panels and controls according to the partner class
        public void InitialiseUserControl()
        {
            FPartnerDefaultView = FMainDS.PPartner.DefaultView;

            FMainDS.PPartner.ColumnChanging += new DataColumnChangeEventHandler(OnPartnerDataColumnChanging);

            #region Show fields according to Partner Class

            switch (SharedTypes.PartnerClassStringToEnum(FPartnerClass))
            {
                case TPartnerClass.PERSON:
                    pnlPerson.Visible = true;
                    pnlWorkerField.Visible = true;
                    pnlSpacer.Visible = false;
                    txtPartnerClass.BackColor = System.Drawing.Color.Yellow;

                    // Set ToolTips in addition to StatusBar texts for fields to make it clearer what to fill in there...
#if TODO
                    tipMain.SetToolTip(this.txtPersonTitle, PPersonTable.GetTitleHelp());
                    tipMain.SetToolTip(this.txtPersonFirstName, PPersonTable.GetFirstNameHelp());
                    tipMain.SetToolTip(this.txtPersonMiddleName, PPersonTable.GetMiddleName1Help());
                    tipMain.SetToolTip(this.txtPersonFamilyName, PPersonTable.GetFamilyNameHelp());
#endif
                    txtPersonTitle.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtPersonFirstName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtPersonMiddleName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtPersonFamilyName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    this.cmbPersonGender.SelectedValueChanged += new System.EventHandler(this.CmbPersonGender_SelectedValueChanged);

                    break;

                case TPartnerClass.FAMILY:
                    pnlFamily.Visible = true;
                    pnlWorkerField.Visible = true;
                    pnlSpacer.Visible = false;

                    // Set ToolTips in addition to StatusBar texts for fields to make it clearer what to fill in there...
#if TODO
                    tipMain.SetToolTip(this.txtFamilyTitle, PFamilyTable.GetTitleHelp());
                    tipMain.SetToolTip(this.txtFamilyFirstName, PFamilyTable.GetFirstNameHelp());
                    tipMain.SetToolTip(this.txtFamilyFamilyName, PFamilyTable.GetFamilyNameHelp());
#endif
                    txtFamilyTitle.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtFamilyFirstName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtFamilyFamilyName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    break;

                case TPartnerClass.CHURCH:
                    pnlChurch.Visible = true;
                    pnlOther.Visible = true;

                    txtChurchName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    break;

                case TPartnerClass.ORGANISATION:
                    pnlOrganisation.Visible = true;
                    pnlOther.Visible = true;

                    txtOrganisationName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    break;

                case TPartnerClass.UNIT:
                    pnlUnit.Visible = true;
                    pnlOther.Visible = true;

                    txtUnitName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    FMainDS.PUnit.ColumnChanging += new DataColumnChangeEventHandler(OnUnitDataColumnChanging);

                    break;

                case TPartnerClass.BANK:
                    pnlBank.Visible = true;
                    pnlOther.Visible = true;

                    txtBranchName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    break;

                case TPartnerClass.VENUE:
                    pnlVenue.Visible = true;
                    pnlOther.Visible = true;

                    txtVenueName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    break;

                default:
                    MessageBox.Show(String.Format(Catalog.GetString("Unrecognised Partner Class '{0}'!"), FPartnerClass));
                    break;
            }

            #endregion
        }

        /// <summary>
        /// Shows the data that is in FMainDS
        /// </summary>
        public void ShowData()
        {
            FPartnerClass = FMainDS.PPartner[0].PartnerClass.ToString();

            ShowData(FMainDS.PPartner[0]);

// TODO            SetupBtnCreated();
            SetupChkNoSolicitations();
            ApplySecurity();
        }

        /// <summary>
        /// Retrieves data that is in the Controls and puts it into the Tables in FMainDS
        /// </summary>
        public void GetDataFromControls()
        {
            GetDataFromControls(FMainDS.PPartner[0]);

            GetDataFromControlsExtra(FMainDS.PPartner[0]);
        }

        /// <summary>
        /// Initialises Delegate Function to handle click on the "Worker Field..." button.
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseDelegateMaintainWorkerField(TDelegateMaintainWorkerField ADelegateFunction)
        {
            FDelegateMaintainWorkerField = ADelegateFunction;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AIncludePartnerClass"></param>
        /// <returns></returns>
        public String PartnerQuickInfo(Boolean AIncludePartnerClass)
        {
            String TmpString;

            TmpString = txtPartnerKey.Text + "   ";

            if (!FMainDS.PPartner[0].IsPartnerShortNameNull())
            {
                TmpString = TmpString + FMainDS.PPartner[0].PartnerShortName;
            }

            if (AIncludePartnerClass)
            {
                TmpString = TmpString + "   [" + FPartnerClass.ToString() + ']';
            }

            return TmpString;
        }

        /// <summary>
        /// Sets the Text of the Worker Field.
        /// </summary>
        /// <param name="AWorkerField">Worker Field.</param>
        public void SetWorkerFieldText(String AWorkerField)
        {
            txtWorkerField.Text = AWorkerField;
        }

        #endregion

        #region Private Methods

        private void ShowDataManual(PPartnerRow ARow)
        {
            // Ensure that the PartnerKey, which is the PrimaryKey of the p_partner Table, is always read-only
            // (Read-Only gets set to false in ShowData() for a NEW Partner because it is the PrimaryKey).
            txtPartnerKey.ReadOnly = true;
        }

        private void GetDataFromControlsExtra(PPartnerRow ARow)
        {
            /*
             * Extra logic is needed for FAMILY and PERSON Partners because ARow.NoSolicitations is overwritten in
             * the auto-generated GetDataFromControls Method by the value of chkOtherNoSolicitations.Checked!
             */
            if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
            {
                ARow.NoSolicitations = chkFamilyNoSolicitations.Checked;
            }
            else if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
            {
                ARow.NoSolicitations = chkPersonNoSolicitations.Checked;
            }
        }

        /// <summary>
        /// Sets the background colour of the CheckBox depending on whether it is
        /// Checked or not.
        /// </summary>
        /// <returns>void</returns>
        private void SetupChkNoSolicitations()
        {
            CheckBox ChkNoSolicitations;

            #region Choose CheckBox according to Partner Class

            switch (SharedTypes.PartnerClassStringToEnum(FPartnerClass))
            {
                case TPartnerClass.PERSON:
                    ChkNoSolicitations = chkPersonNoSolicitations;
                    break;

                case TPartnerClass.FAMILY:
                    ChkNoSolicitations = chkFamilyNoSolicitations;
                    break;

                case TPartnerClass.CHURCH:
                case TPartnerClass.ORGANISATION:
                case TPartnerClass.UNIT:
                case TPartnerClass.BANK:
                    ChkNoSolicitations = chkOtherNoSolicitations;
                    break;

                default:
                    ChkNoSolicitations = chkOtherNoSolicitations;
                    break;
            }

            #endregion

            if (ChkNoSolicitations.Checked)
            {
                ChkNoSolicitations.BackColor = System.Drawing.Color.PeachPuff;
            }
            else
            {
                ChkNoSolicitations.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        private void ApplySecurity()
        {
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPartnerTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_partner. This continues in the switch statments!
                CustomEnablingDisabling.DisableControlGroup(pnlRight);
            }

            switch (SharedTypes.PartnerClassStringToEnum(FPartnerClass))
            {
                case TPartnerClass.PERSON:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPersonTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlPerson, cmbPersonAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlPerson, chkPersonNoSolicitations);

                        // need to disable all Fields that are DataBound to p_person
                        CustomEnablingDisabling.DisableControlGroup(pnlPerson);

                        cmbPersonAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.FAMILY:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PFamilyTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlFamily, cmbFamilyAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlFamily, chkFamilyNoSolicitations);

                        // need to disable all Fields that are DataBound to p_family
                        CustomEnablingDisabling.DisableControlGroup(pnlFamily);

                        cmbFamilyAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.CHURCH:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PChurchTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_church
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.ORGANISATION:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, POrganisationTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_organisation
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.UNIT:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PUnitTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_unit
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.BANK:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PBankTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_bank
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.VENUE:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PVenueTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_venue
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                default:
                    MessageBox.Show(String.Format(Catalog.GetString("Unrecognised Partner Class '{0}'!"), FPartnerClass));
                    break;
            }
        }

        private Boolean PartnerStatusCodeChangePromotion(DataColumnChangeEventArgs e)
        {
            Boolean ReturnValue;
            String FamilyMembersText;
            PartnerEditTDSFamilyMembersTable FamilyMembersDT;
            Int32 Counter;
            Int32 Counter2;
            PartnerEditTDSFamilyMembersRow FamilyMembersDR;
            PartnerEditTDSFamilyMembersInfoForStatusChangeRow FamilyMembersInfoDR;
            DialogResult FamilyMembersResult;
            DataView FamilyMembersDV;

            ReturnValue = true;
            FamilyMembersText = "";

            /* Retrieve Family Members from the PetraServer */
            FamilyMembersDT = FPartnerEditUIConnector.GetDataFamilyMembers(FMainDS.PPartner[0].PartnerKey, "");
            FamilyMembersDV = new DataView(FamilyMembersDT, "", PPersonTable.GetFamilyIdDBName() + " ASC", DataViewRowState.CurrentRows);

            /* Build a formatted String of Family Members' PartnerKeys and ShortNames */
            for (Counter = 0; Counter <= FamilyMembersDV.Count - 1; Counter += 1)
            {
                FamilyMembersDR = (PartnerEditTDSFamilyMembersRow)FamilyMembersDV[Counter].Row;
                FamilyMembersText = FamilyMembersText + "   " + StringHelper.FormatStrToPartnerKeyString(FamilyMembersDR.PartnerKey.ToString()) +
                                    "   " + FamilyMembersDR.PartnerShortName + Environment.NewLine;
            }

            /* If there are Family Members, ... */
            if (FamilyMembersText != "")
            {
                /* show MessageBox with Family Members to the user, asking whether to promote. */
                FamilyMembersResult =
                    MessageBox.Show(
                        String.Format(
                            Catalog.GetString("Partner Status change from '{0}' to '{1}': \r\n" +
                                "Should OpenPetra apply this change to all Family Members of this Family?"),
                            ((PPartnerRow)e.Row).StatusCode,
                            e.ProposedValue) + Environment.NewLine + Environment.NewLine +
                        Catalog.GetString("The Family has the following Family Members:") + Environment.NewLine +
                        FamilyMembersText + Environment.NewLine +
                        Catalog.GetString("(Choose 'Cancel' to cancel the change of the Partner Status\r\n" +
                            "for this Partner)."),
                        Catalog.GetString("Promote Partner Status Change to All Family Members?"),
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                /* Check User's response */
                switch (FamilyMembersResult)
                {
                    case System.Windows.Forms.DialogResult.Yes:

                        /*
                         * User wants to promote the Partner StatusCode change to Family
                         * Members: add new DataTable for that purpose if it doesn't exist yet.
                         */
                        if (FMainDS.FamilyMembersInfoForStatusChange == null)
                        {
                            FMainDS.Tables.Add(new PartnerEditTDSFamilyMembersInfoForStatusChangeTable());
                            FMainDS.InitVars();
                        }

                        /*
                         * Remove any existing DataRows so we start from a 'clean slate'
                         * (the user could change the Partner StatusCode more than once...)
                         */
                        FMainDS.FamilyMembersInfoForStatusChange.Rows.Clear();

                        /*
                         * Add the PartnerKeys of the Family Members that we have just displayed
                         * to the user to the DataTable.
                         *
                         * Note: This DataTable will be sent to the PetraServer when the user
                         * saves the Partner. The UIConnector will pick it up and process it.
                         */
                        for (Counter2 = 0; Counter2 <= FamilyMembersDV.Count - 1; Counter2 += 1)
                        {
                            FamilyMembersDR = (PartnerEditTDSFamilyMembersRow)FamilyMembersDV[Counter2].Row;
                            FamilyMembersInfoDR = FMainDS.FamilyMembersInfoForStatusChange.NewRowTyped(false);
                            FamilyMembersInfoDR.PartnerKey = FamilyMembersDR.PartnerKey;
                            FMainDS.FamilyMembersInfoForStatusChange.Rows.Add(FamilyMembersInfoDR);
                        }

                        break;

                    case System.Windows.Forms.DialogResult.No:

                        /* no promotion wanted > nothing to do */
                        /* (StatusCode will be changed only for the Family) */
                        break;

                    case System.Windows.Forms.DialogResult.Cancel:
                        ReturnValue = false;
                        break;
                }
            }
            else
            {
            }

            /* no promotion needed since there are no Family Members */
            /* (StatusCode will be changed only for the Family) */
            return ReturnValue;
        }

        #endregion


        #region Event handlers


        private void OnAnyDataColumnChanging(System.Object sender, EventArgs e)
        {
            TPartnerClassMainDataChangedEventArgs EventFireArgs;

            /* messagebox.show('Column_Changing Event: Column=' + e.Column.ColumnName + */
            /* '; Column content=' + e.Row[e.Column.ColumnName].ToString + */
            /* '; ' + e.ProposedValue.ToString); */
            /* MessageBox.Show('PartnerClass: ' + FPartnerClass.ToString); */
            EventFireArgs = new TPartnerClassMainDataChangedEventArgs();
            EventFireArgs.PartnerClass = FPartnerClass;

            if (FPartnerClass == "PERSON")
            {
                if ((sender == txtPersonTitle)
                    || (sender == txtPersonFirstName)
                    || (sender == txtPersonMiddleName) || (sender == txtPersonFamilyName))
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtPersonFamilyName.Text,
                        txtPersonTitle.Text,
                        txtPersonFirstName.Text,
                        txtPersonMiddleName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "FAMILY")
            {
                if ((sender == txtFamilyTitle) || (sender == txtFamilyFirstName)
                    || (sender == txtFamilyFamilyName))
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtFamilyFamilyName.Text,
                        txtFamilyTitle.Text,
                        txtFamilyFirstName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "CHURCH")
            {
                if (sender == txtChurchName)
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtChurchName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "ORGANISATION")
            {
                if (sender == txtOrganisationName)
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtOrganisationName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "UNIT")
            {
                if (sender == txtUnitName)
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtUnitName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "BANK")
            {
                if (sender == txtBranchName)
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtBranchName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "VENUE")
            {
                if (sender == txtVenueName)
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtVenueName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
        }

        private void OnPartnerDataColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            TVerificationResult VerificationResultReturned;
            TScreenVerificationResult VerificationResultEntry;
            Control BoundControl = null;

            // MessageBox.Show('Column ''' + e.Column.ToString + ''' is changing...');
            try
            {
                if (TPartnerVerification.VerifyPartnerData(e, out VerificationResultReturned) == false)
                {
                    if (VerificationResultReturned.ResultCode != PetraErrorCodes.ERR_PARTNERSTATUSMERGEDCHANGEUNDONE)
                    {
                        TMessages.MsgVerificationError(VerificationResultReturned, this.GetType());

// TODO                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FMainDS.PPartner], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
// TODO                        BoundControl.Focus();
                        VerificationResultEntry = new TScreenVerificationResult(this,
                            e.Column,
                            VerificationResultReturned.ResultText,
                            VerificationResultReturned.ResultTextCaption,
                            VerificationResultReturned.ResultCode,
                            BoundControl,
                            VerificationResultReturned.ResultSeverity);
                        FPetraUtilsObject.VerificationResultCollection.Add(VerificationResultEntry);

                        // MessageBox.Show('After setting the error: ' + e.ProposedValue.ToString);
                    }
                    else
                    {
                        // undo the change in the DataColumn
                        e.ProposedValue = e.Row[e.Column.ColumnName];

                        // need to assign this to make the change actually visible...
                        cmbPartnerStatus.SetSelectedString(e.ProposedValue.ToString());

                        TMessages.MsgVerificationError(VerificationResultReturned, this.GetType());

// TODO                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FPartnerDefaultView], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
// TODO                        BoundControl.Focus();
                    }
                }
                else
                {
                    if (FPetraUtilsObject.VerificationResultCollection.Contains(e.Column))
                    {
                        FPetraUtilsObject.VerificationResultCollection.Remove(e.Column);
                    }

                    // Business Rule: if the Partner's StatusCode changes, give the user the
                    // option to promote the change to all Family Members (if the Partner is
                    // a FAMILY and has Family Members).
                    if (e.Column.ColumnName == PPartnerTable.GetStatusCodeDBName())
                    {
                        if (PartnerStatusCodeChangePromotion(e))
                        {
                            // Set the StatusChange date (this would be done on the server side
                            // automatically, but we want to display it now for immediate user feedback)
                            FMainDS.PPartner[0].StatusChange = DateTime.Today;
                        }
                        else
                        {
                            // User wants to cancel the change of the Partner StatusCode
                            // Undo the change in the DataColumn
                            e.ProposedValue = e.Row[e.Column.ColumnName];

                            // Need to assign this to make the change actually visible...
                            cmbPartnerStatus.SetSelectedString(e.ProposedValue.ToString());
                        }
                    }
                }
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.ToString());
            }
        }

        private void OnUnitDataColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            TVerificationResult VerificationResultReturned;
            TScreenVerificationResult VerificationResultEntry;
            Control BoundControl;

            // MessageBox.Show('Column ''' + e.Column.ToString + ''' is changing...');
            try
            {
                if (TPartnerVerification.VerifyUnitData(e, FMainDS, out VerificationResultReturned) == false)
                {
                    if (VerificationResultReturned.ResultCode != PetraErrorCodes.ERR_UNITNAMECHANGEUNDONE)
                    {
                        TMessages.MsgVerificationError(VerificationResultReturned, this.GetType());

                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FMainDS.PUnit], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
// TODO                        BoundControl.Focus();
                        VerificationResultEntry = new TScreenVerificationResult(this,
                            e.Column,
                            VerificationResultReturned.ResultText,
                            VerificationResultReturned.ResultTextCaption,
                            VerificationResultReturned.ResultCode,
                            BoundControl,
                            VerificationResultReturned.ResultSeverity);
                        FPetraUtilsObject.VerificationResultCollection.Add(VerificationResultEntry);

                        // MessageBox.Show('After setting the error: ' + e.ProposedValue.ToString);
                    }
                    else
                    {
                        // undo the change in the DataColumn
                        e.ProposedValue = e.Row[e.Column.ColumnName, DataRowVersion.Original];

                        // need to assign this to make the change actually visible...
                        txtUnitName.Text = e.ProposedValue.ToString();
// TODO                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FMainDS.PUnit], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
// TODO                        BoundControl.Focus();
                    }
                }
                else
                {
                    if (FPetraUtilsObject.VerificationResultCollection.Contains(e.Column))
                    {
                        FPetraUtilsObject.VerificationResultCollection.Remove(e.Column);
                    }
                }
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.ToString());
            }
        }

        private void CmbPersonGender_SelectedValueChanged(System.Object sender, System.EventArgs e)
        {
            if (cmbPersonGender.GetSelectedString() == "Female")
            {
                cmbPersonAddresseeTypeCode.SetSelectedString(SharedTypes.StdAddresseeTypeCodeEnumToString(TStdAddresseeTypeCode.satcFEMALE));
            }
            else if (cmbPersonGender.GetSelectedString() == "Male")
            {
                cmbPersonAddresseeTypeCode.SetSelectedString(SharedTypes.StdAddresseeTypeCodeEnumToString(TStdAddresseeTypeCode.satcMALE));
            }

//            /*
//             * Also assign the value directly to the databound data field!
//             * Strangely enough, this is necessary for the case if the user doesn't TAB out
//             * of cmbPersonGender, but uses the mouse to select anything else on the screen
//             * *except* cmbAddresseeType!
//             */
//            FMainDS.PPartner[0].AddresseeTypeCode = cmbAddresseeType.SelectedItem.ToString();
        }

        private void UpdateNoSolicitationsColouring(System.Object sender, System.EventArgs e)
        {
            SetupChkNoSolicitations();
        }

        #endregion


        #region Custom Events

        private void OnPartnerClassMainDataChanged(TPartnerClassMainDataChangedEventArgs e)
        {
            /* MessageBox.Show('OnPartnerClassMainDataChanged. e.PartnerClass: ' + e.PartnerClass.ToString); */
            if (PartnerClassMainDataChanged != null)
            {
                PartnerClassMainDataChanged(this, e);
            }
        }

        #endregion


        #region Actions

        private void MaintainWorkerField(System.Object sender, System.EventArgs e)
        {
            if (this.FDelegateMaintainWorkerField != null)
            {
                this.FDelegateMaintainWorkerField();
            }
            else
            {
                throw new EVerificationMissing(Catalog.GetString("FDelegateMaintainWorkerField"));
            }
        }

        #endregion
    }
}