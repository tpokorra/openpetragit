//
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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MPartner;
using System.Globalization;
using Ict.Petra.Client.App.Gui;
using Ict.Common.Controls;
using Ict.Common;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// UserControl that automatically arranges panels with Find Criteria for
    /// Partner Find.
    /// Allows Find Criteria to be added, removed and moved up, down, left, right
    /// in conjunction with the PartnerFind_Options screen.
    /// </summary>
    public partial class TUC_PartnerFindCriteria : System.Windows.Forms.UserControl, IPetraUserControl
    {
        private const String StrSpacer = "Spacer";
        private const String StrBeginGroup = "BeginGroup";
        private const String StrPartnerClassFindHelpText = "Restricts search to Partners of specified Partner Class (* = all Classes)";
        private const String StrWorkerFamilyOnlyFindHelpText = "Filters the search to Worker Families";
        private const String StrPreviousNameFindHelpText = "Enter a Previous Name (eg. Maiden Name, old name of an Organisation or Church)";
        private const String StrPartnerKeyNonExactInfoTest = "Trailing zero(es) in the Partner Key are used as wilcard characters.\n\r" +
                                                             "Example: Entering Partner Key '0029000000' will return all Partners whose Partner Key was generated\n\r"
                                                             +
                                                             "for Unit 0029000000 (Netherlands).\n\r" +
                                                             "This is because the 'Exact Partner Key Match' Option is turned off in the Partner Find Options.";

        /// <summary>Private Declarations</summary>
        private ArrayList FCriteriaFieldsLeft;
        private ArrayList FCriteriaFieldsRight;
        private Boolean FCriteriaSetupMode;
        private TSelectedCriteriaPanel FSelectedPanel;
        private DataTable FPartnerClassDataTable;
        private Boolean FWorkerFamOnly;
        private string[] FRestrictedParterClasses;
        private String FDefaultPartnerClass;
        private PartnerFindTDSSearchCriteriaRow FDefaultValues;

// TODO        private int FPreviousSelectedPartnerClass;
        private Boolean FShowAllPartnerClasses;

        private string PartnerStatus
        {
            get
            {
                string ReturnValue = "";

                if (rbtStatusActive.Checked)
                {
                    ReturnValue = "ACTIVE";
                }

                if (rbtStatusAll.Checked)
                {
                    ReturnValue = "ALL";
                }

                if (rbtPrivate.Checked)
                {
                    ReturnValue = "PRIVATE";
                }

                return ReturnValue;
            }

            set
            {
                // start off with default colours
                rbtStatusAll.BackColor = System.Drawing.SystemColors.Control;
                rbtPrivate.BackColor = System.Drawing.SystemColors.Control;

                if (value == "ACTIVE")
                {
                    rbtStatusActive.Checked = true;
                }

                if (value == "ALL")
                {
                    rbtStatusAll.Checked = true;
                    rbtStatusAll.BackColor = System.Drawing.Color.PeachPuff;
                }

                if (value == "PRIVATE")
                {
                    rbtPrivate.Checked = true;
                    rbtPrivate.BackColor = System.Drawing.Color.PeachPuff;
                }

                try
                {
                    FFindCriteriaDataTable.Rows[0]["PartnerStatus"] = value;
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>todoComment</summary>
        public ArrayList CriteriaFieldsLeft
        {
            get
            {
                return FCriteriaFieldsLeft;
            }

            set
            {
                FCriteriaFieldsLeft = value;
            }
        }

        /// <summary>todoComment</summary>
        public ArrayList CriteriaFieldsRight
        {
            get
            {
                return FCriteriaFieldsRight;
            }

            set
            {
                FCriteriaFieldsRight = value;
            }
        }

        /// <summary>todoComment</summary>
        public Boolean CriteriaSetupMode
        {
            get
            {
                return FCriteriaSetupMode;
            }

            set
            {
                FCriteriaSetupMode = value;

                // MessageBox.Show('CriteriaSetupMode: ' + FCriteriaSetupMode.ToString);
                if (FCriteriaSetupMode == true)
                {
                    // CustomEnablingDisabling.DisableControlGroup(pnlLeftColumn);
                    // CustomEnablingDisabling.DisableControlGroup(pnlRightColumn);
                    CustomDisablePanels(pnlLeftColumn);
                    CustomDisablePanels(pnlRightColumn);
                }
                else
                {
                }

                // CustomEnablingDisabling.EnableControlGroup(pnlLeftColumn);
                // CustomEnablingDisabling.EnableControlGroup(pnlRightColumn);
            }
        }

        private PartnerFindTDSSearchCriteriaTable FFindCriteriaDataTable;

        /// <summary>todoComment</summary>
        public DataTable CriteriaData
        {
            get
            {
                // MessageBox.Show('PartnerName: ' + FFindCriteriaDataTable.Rows[0]['PartnerName'].ToString);
                return FFindCriteriaDataTable;
            }
        }

        /// Event that fires when one of the SearchCriteria's contents is changed.following line appeers to be brokne by the disigner regularlycorrect line is: property OnCriteriaContentChanged: System.EventHandler add OnCriteriaContentChanged remove
        /// <summary>OnCriteriaContentChanged;</summary>
        public event System.EventHandler OnCriteriaContentChanged;

        /// <summary>todoComment</summary>
        public String[] RestrictedPartnerClass
        {
            get
            {
                return FRestrictedParterClasses;
            }

            set
            {
                DataRow PartnerClassDataRow;
                String tmpString;
                Boolean miWorkerFamOnly;

                FRestrictedParterClasses = value;

                // this flag is set to true IF the FIRST item in list will be WORKER-FAM
                miWorkerFamOnly = false;

                // are there any restrictions specified?
                if ((value != null) && (value.Length > 0))
                {
                    // clear table combo is bound to , and start again
                    FPartnerClassDataTable.Rows.Clear();
                    tmpString = value[0];

                    if (tmpString.IndexOf("WORKER-FAM") >= 0)
                    {
                        // first item selected will be WORKER-FAM
                        miWorkerFamOnly = true;
                    }

                    tmpString = tmpString.Replace("WORKER-FAM", "FAMILY");
                    FDefaultPartnerClass = tmpString;

                    foreach (String eachPart in value)
                    {
                        // .Split(new (array [] of Char, (','))) do

                        // MessageBox.Show('eachPart: ' + eachPart);
                        if (eachPart == "WORKER-FAM")
                        {
                            // add FAMILY Row has own special case
                            PartnerClassDataRow = FPartnerClassDataTable.NewRow();

                            // Add FAMILY not WORKER-FAM
                            PartnerClassDataRow["PartnerClass"] = "FAMILY";
                            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);

                            // set the flag, so combo box handler knows this is the case
                            FWorkerFamOnly = true;
                        }
                        else
                        {
                            // just add item
                            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                            PartnerClassDataRow["PartnerClass"] = eachPart;
                            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                        }
                    }

                    // end for each
                    // ensure top choice is databound
                    FFindCriteriaDataTable.Rows[0]["PartnerClass"] = FDefaultPartnerClass;
                    FFindCriteriaDataTable.Rows[0]["WORKERFAMONLY"] = miWorkerFamOnly;
                    cmbPartnerClass.ResetBindings();
                }

                if (cmbPartnerClass.Items.Count > 0)
                {
                    cmbPartnerClass.SelectedIndex = 0;

                    // this ensures that the checkbox is visible if needed
                    // and disabled if needed
                    // and checked if needed
                    HandlePartnerClassGui();
                }
            }
        }

        /// <summary>todoComment</summary>
        public event FindCriteriaSelectionChangedHandler FindCriteriaSelectionChanged;

        /// <summary>
        /// constructor
        /// </summary>
        public TUC_PartnerFindCriteria() : base()
        {
            FFindCriteriaDataTable = new PartnerFindTDSSearchCriteriaTable();
            FDefaultValues = FFindCriteriaDataTable.NewRowTyped();

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.btnLocationKey.Text = Catalog.GetString("Location Key");
            this.chkWorkerFamOnly.Text = Catalog.GetString("Worker Families O&nly");
            this.lblPartnerClass.Text = Catalog.GetString("Partner C&lass:");
            this.txtPartnerKey.Text = Catalog.GetString("0000000000");
            this.lblPartnerKey.Text = Catalog.GetString("Partner &Key:");
            this.lblPartnerKeyNonExactMatch.Text = Catalog.GetString("(trailing 0 = --*)");
            this.rbtPrivate.Text = Catalog.GetString("Private");
            this.rbtStatusActive.Text = Catalog.GetString("Acti&ve");
            this.rbtStatusAll.Text = Catalog.GetString("All");
            this.lblPartnerStatus.Text = Catalog.GetString("Status:");
            this.lblPhoneNumber.Text = Catalog.GetString("Phone Number:");
            this.lblAddress3.Text = Catalog.GetString("Address &3:");
            this.lblAddress2.Text = Catalog.GetString("Address &2:");
            this.lblEmail.Text = Catalog.GetString("&Email:");
            this.lblPartnerName.Text = Catalog.GetString("Partner &Name:");
            this.lblPersonalName.Text = Catalog.GetString("Personal &(First) Name:");
            this.lblPreviousName.Text = Catalog.GetString("Previous Name:");
            this.lblAddress1.Text = Catalog.GetString("Address &1:");
            this.lblPostCode.Text = Catalog.GetString("P&ost Code:");
            this.lblCity.Text = Catalog.GetString("Cit&y/Town:");
            this.lblCounty.Text = Catalog.GetString("Co&unty:");
            this.lblCountry.Text = Catalog.GetString("Co&untry:");
            this.lblMailingAddressOnly.Text = Catalog.GetString("Mailin&g Addresses Only:");
            #endregion
        }

        private TFrmPetraUtils FPetraUtilsObject;

        /// <summary>
        /// this provides general functionality for screens
        /// </summary>
        public TFrmPetraUtils PetraUtilsObject
        {
            get
            {
                return FPetraUtilsObject;
            }
            set
            {
                FPetraUtilsObject = value;
            }
        }

        private void RbtStatusActive_Click(System.Object sender, System.EventArgs e)
        {
            this.PartnerStatus = "ACTIVE";
        }

        private void RbtStatusAll_Click(System.Object sender, System.EventArgs e)
        {
            this.PartnerStatus = "ALL";
        }

        private void RbtPrivate_Click(System.Object sender, System.EventArgs e)
        {
            this.PartnerStatus = "PRIVATE";
        }

        private void CmbPartnerClass_SelectedValueChanged(System.Object sender, System.EventArgs e)
        {
            // This is not very nice but it seems to be the only way to handle changes in
            // Partner Class combo box
            if (FFindCriteriaDataTable.Rows.Count > 0)
            {
                DataRow SingleDataRow = FFindCriteriaDataTable.Rows[0];
                SingleDataRow.BeginEdit();
                SingleDataRow["PersonalName"] = txtPersonalName.Text;

                // it probably happens too early on Mono, and there are no values yet;
                // therefore cmbPartnerClass.Text and cmbPartnerClass.SelectedValue both return System.Data.DataRowView
                if (cmbPartnerClass.Text != "System.Data.DataRowView")
                {
                    SingleDataRow["PartnerClass"] = cmbPartnerClass.Text;
                }
                else
                {
                    SingleDataRow["PartnerClass"] = "*";
                }

                SingleDataRow.EndEdit();

                if (txtPersonalName.Text.Length == 1)
                {
                    // Set manually the cursor at the end of the textbox when user there is only
                    // one character left.
                    // Reason: When the user types the first character the cursor is somehow set
                    // back to position 0. So here we set it back to position 1.
                    txtPersonalName.SelectionStart = 1;
                }
            }

            HandlePartnerClassGui();
        }

        private void HandlePartnerClassGui()
        {
            chkWorkerFamOnly.Visible = false;
            chkWorkerFamOnly.Checked = false;
            pnlPartnerClass.Height = cmbPartnerClass.Height + 3;

            // the partner class combo might not be on the form!
            if (cmbPartnerClass.SelectedValue == null)
            {
                return;

                // get out of here!
            }

            if (cmbPartnerClass.SelectedValue.ToString() == "FAMILY")
            {
                chkWorkerFamOnly.Visible = true;
                chkWorkerFamOnly.Checked = false;

                if (FWorkerFamOnly == true)
                {
                    chkWorkerFamOnly.Checked = true;
                    chkWorkerFamOnly.Enabled = false;
                }

                pnlPartnerClass.Height = cmbPartnerClass.Height + chkWorkerFamOnly.Height + 3;

                // enable personal Name field
                if (txtPersonalName.Enabled == false)
                {
                    txtPersonalName.Enabled = true;
                }
            }
            // enable txtPersonalName only if we have *, Family or Person as PartnerClass
            // and when it is not enabled.
            else if (cmbPartnerClass.SelectedValue.ToString() == "PERSON")
            {
                if (txtPersonalName.Enabled == false)
                {
                    txtPersonalName.Enabled = true;
                }
            }
            else if (cmbPartnerClass.SelectedValue.ToString() == "*")
            {
                if (txtPersonalName.Enabled == false)
                {
                    txtPersonalName.Enabled = true;
                }
            }
            else if (txtPersonalName.Enabled == true)
            {
                // disable pesonal name field when we don't have *, Family or Person as Partner Class
                // and when it is enabled
                txtPersonalName.Enabled = false;
            }
        }

        private void BtnLocationKey_Click(System.Object sender, System.EventArgs e)
        {
// TODO BtnLocationKey_Click
#if TODO
            TPartnerLocationFind frmPartnerLS;

            // hourglass cursor
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            Application.DoEvents();

            // give windows half a chance to show cursor
            frmPartnerLS = new TPartnerLocationFind();

            if (frmPartnerLS.ShowDialog() == DialogResult.OK)
            {
                // fill in the location key
                txtLocationKey.Text = frmPartnerLS.SelectedLocation.LocationKey.ToString();

                // disable all other controls
                this.DisableAllPanel(pnlLocationKey);

                // KICK the databinding which hasn't notices the textbox has changed
                txtLocationKey.Focus();
            }

            // normal mouse cursor
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            Application.DoEvents();

            // give windows half a chance to show cursor
#endif
        }

        private void LblAddress2_Click(System.Object sender, System.EventArgs e)
        {
        }

        #region User Defaults for Match Criteria Buttons

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="parentctrl"></param>
        /// <returns></returns>
        public String GetMatchButtonsString(Control parentctrl)
        {
            String strOutput;
            ArrayList outList;
            Boolean firstOne;

            outList = new ArrayList();
            strOutput = "";
            firstOne = true;

            foreach (Control ctrl in parentctrl.Controls)
            {
                if (ctrl.GetType() == typeof(Panel))
                {
                    foreach (Control innerCtrl in ctrl.Controls)
                    {
                        if (innerCtrl.GetType() == typeof(SplitButton))
                        {
                            outList.Add(innerCtrl.Name.ToString() + ',' + ((SplitButton)innerCtrl).SelectedValue);
                        }
                    }

                    // for innerCtrl
                }

                // if Panel;
            }

            // for ctrl
            foreach (String strItem in outList)
            {
                if (firstOne == false)
                {
                    strOutput = strOutput + '|';
                }

                strOutput = strOutput + strItem;
                firstOne = false;
            }

            return strOutput;
        }

        /// <summary>
        /// function
        /// </summary>
        public void SaveMatchButtonSettings()
        {
            String strLeft;
            String strRight;

            strLeft = GetMatchButtonsString(pnlLeftColumn);
            strRight = GetMatchButtonsString(pnlRightColumn);
            TUserDefaults.SetDefault(TUserDefaults.PARTNER_EDIT_MATCHSETTINGS_LEFT, strLeft);
            TUserDefaults.SetDefault(TUserDefaults.PARTNER_EDIT_MATCHSETTINGS_RIGHT, strRight);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void RestoreSplitterSetting()
        {
            spcCriteria.SplitterDistance = TUserDefaults.GetInt32Default(TUserDefaults.PARTNER_FIND_SPLITPOS_CRITERIA, 326);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void SaveSplitterSetting()
        {
            TUserDefaults.SetDefault(TUserDefaults.PARTNER_FIND_SPLITPOS_CRITERIA, spcCriteria.SplitterDistance);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void LoadMatchButtonSettings()
        {
            String strLeft;
            String strRight;

            // retrieve defaults
            strLeft = TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_EDIT_MATCHSETTINGS_LEFT, "");
            strRight = TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_EDIT_MATCHSETTINGS_RIGHT, "");

            if (FFindCriteriaDataTable.Rows.Count != 0)
            {
                FFindCriteriaDataTable.Rows[0].BeginEdit();
            }

            // FIRST DO LEFT LIST
            if (strLeft != "")
            {
                SetMatchButtonValues(strLeft);
            }

            // if strLeft
            // NOW RIGHT LIST
            if (strRight != "")
            {
                SetMatchButtonValues(strRight);
            }

            // if strRight
            if (FFindCriteriaDataTable.Rows.Count != 0)
            {
                FFindCriteriaDataTable.Rows[0].EndEdit();
            }

            AssociateCriteriaButtons();
        }

        /// <summary>
        /// Takes a delimited string and sets the controls properties
        /// </summary>
        /// <param name="AValues"></param>
        public void SetMatchButtonValues(String AValues)
        {
            String[] miLineArray;
            String ctrlName;
            String ctrlValue;
            String tempString;
            Control ctrl;
            Int32 x;

            if (AValues != "")
            {
                // setup delimiters
                char[] lineDelimiter = new char[] {
                    '|'
                };
                char[] fieldDelimiter = new char[] {
                    ','
                };
                miLineArray = AValues.Split(lineDelimiter);

                for (x = 0; x <= miLineArray.Length - 1; x += 1)
                {
                    tempString = Convert.ToString(miLineArray[x]);
                    ctrlName = tempString.Split(fieldDelimiter)[0];
                    ctrlValue = tempString.Split(fieldDelimiter)[1];
                    ctrl = FindControl(this, ctrlName);

                    if (ctrl != null)
                    {
                        FFindCriteriaDataTable.Rows[0].BeginEdit();
                        TLogging.Log("Running SetMatchButtonValues for " + ctrl.Name + ".");
                        ((SplitButton)ctrl).SelectedValue = ctrlValue;
                        FFindCriteriaDataTable.Rows[0].EndEdit();

                        //TODO: It seems databinding is broken on this control
                        // this needs to happen in the SplitButton control really
                        string fieldname = ((SplitButton)ctrl).DataBindings[0].BindingMemberInfo.BindingMember;
                        FFindCriteriaDataTable.Rows[0][fieldname] = ctrlValue;

                        // MessageBox.Show('control found:' + ctrl.Name);
                    }
                }

                // for
            }

            // if strRight
        }

        /// <summary>
        /// Recursively searches for a control by name in a given container
        /// </summary>
        /// <param name="AContainer"></param>
        /// <param name="AName"></param>
        /// <returns></returns>
        public Control FindControl(Control AContainer, String AName)
        {
            Control miResult;

            miResult = null;

            foreach (Control ctrl in AContainer.Controls)
            {
                if ((miResult == null) && (ctrl.Name == AName))
                {
                    miResult = ctrl;
                }

                if ((miResult == null) && (ctrl.HasChildren == true))
                {
                    miResult = FindControl(ctrl, AName);
                }
            }

            // for ctrl
            return miResult;
        }

        #endregion

        #region Key Press Events for showing context menus

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATextBox"></param>
        /// <param name="ACriteriaControl"></param>
        /// <param name="e"></param>
        public void GeneralKeyHandler(TextBox ATextBox, SplitButton ACriteriaControl, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                // This is needed
                // without it, databinding fails
                ACriteriaControl.ShowContextMenu();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATextBox"></param>
        /// <param name="ACriteriaControl"></param>
        public void GeneralLeaveHandler(TextBox ATextBox, SplitButton ACriteriaControl)
        {
            TMatches NewMatchValue;
            string TextBoxText = ATextBox.Text;
            string CriteriaValue;

            //            TLogging.Log("GeneralLeaveHandler for " + ATextBox.Name + ". SplitButton: " + ACriteriaControl.Name);

            if (ATextBox.Text.Contains("*"))
            {
                if (ATextBox.Text.StartsWith("*")
                    && !(ATextBox.Text.EndsWith("*")))
                {
                    //                    TLogging.Log(ATextBox.Name + " starts with *");
                    NewMatchValue = TMatches.ENDS;
                }
                else if (ATextBox.Text.EndsWith("*")
                         && !(ATextBox.Text.StartsWith("*")))
                {
                    //                    TLogging.Log(ATextBox.Name + " ends with *");
                    NewMatchValue = TMatches.BEGINS;
                }
                else
                {
                    //                    TLogging.Log(ATextBox.Name + " contains *");
                    NewMatchValue = TMatches.CONTAINS;
                }

                // See what the Criteria Value would be without any 'joker' characters ( * )
                CriteriaValue = TextBoxText.Replace("*", String.Empty);

                if (CriteriaValue != String.Empty)
                {
                    // There is still a valid CriteriaValue

                    FFindCriteriaDataTable.Rows[0].BeginEdit();
                    ACriteriaControl.SelectedValue = Enum.GetName(typeof(TMatches), NewMatchValue);
                    FFindCriteriaDataTable.Rows[0].EndEdit();

                    //TODO: It seems databinding is broken on this control
                    // this needs to happen in the SplitButton control really
                    string fieldname = ((SplitButton)ACriteriaControl).DataBindings[0].BindingMemberInfo.BindingMember;
                    FFindCriteriaDataTable.Rows[0][fieldname] = Enum.GetName(typeof(TMatches), NewMatchValue);

                    //TODO: DataBinding is really doing strange things here; we have to
                    //assign the just entered Text again, otherwise it is lost!!!
                    ATextBox.Text = TextBoxText;
                }
                else
                {
                    // No valid Criteria Value, therefore empty the TextBox's Text.
                    ATextBox.Text = String.Empty;
                }
            }
        }

        private void RemoveJokersFromTextBox(SplitButton ASplitButton,
            TextBox AAssociatedTextBox,
            TMatches ALastSelection)
        {
            string NewText;

            try
            {
                if (AAssociatedTextBox != null)
                {
                    NewText = AAssociatedTextBox.Text.Replace("*", String.Empty);

//                    TLogging.Log(
//                        "RemoveJokersFromTextBox:  Associated TextBox's (" + AAssociatedTextBox.Name + ") Text (1): " + AAssociatedTextBox.Text);

//                    FFindCriteriaDataTable.Rows[0].BeginEdit();
//                    AAssociatedTextBox.Text = AAssociatedTextBox.Text.Replace("*", String.Empty);
//                    FFindCriteriaDataTable.Rows[0].EndEdit();

                    string fieldname = ((TextBox)AAssociatedTextBox).DataBindings[0].BindingMemberInfo.BindingMember;
                    FFindCriteriaDataTable.Rows[0][fieldname] = NewText;
                    fieldname = ((SplitButton)ASplitButton).DataBindings[0].BindingMemberInfo.BindingMember;
                    FFindCriteriaDataTable.Rows[0][fieldname] = Enum.GetName(typeof(TMatches), ALastSelection);

//
//                    AAssociatedTextBox.Text = NewText;
//
//                    TLogging.Log(
//                        "RemoveJokersFromTextBox:  Associated TextBox's (" + AAssociatedTextBox.Name + ") Text (2): " + AAssociatedTextBox.Text);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Exception in RemoveJokersFromTextBox: " + exp.ToString());
            }
        }

        private void TxtCounty_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtCounty, critCounty, e);
        }

        private void TxtCounty_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtCounty, critCounty);
        }

        private void TxtEmail_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtEmail, critEmail, e);
        }

        private void TxtEmail_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtEmail, critEmail);
        }

        private void TxtPhoneNumber_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtPhoneNumber, critPhoneNumber, e);
        }

        private void TxtPhoneNumber_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtPhoneNumber, critPhoneNumber);
        }

        private void TxtPostCode_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtPostCode, critPostCode, e);
        }

        private void TxtPostCode_Leave(System.Object sender, EventArgs e)
        {
            // capitalise when leaving control
            txtPostCode.Text = txtPostCode.Text.ToUpper();

            GeneralLeaveHandler(txtPostCode, critPostCode);
        }

        private void TxtCity_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtCity, critCity, e);
        }

        private void TxtCity_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtCity, critCity);
        }

        private void TxtAddress1_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtAddress1, critAddress1, e);
        }

        private void TxtAddress1_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtAddress1, critAddress1);
        }

        private void TxtAddress2_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtAddress2, critAddress2, e);
        }

        private void TxtAddress2_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtAddress2, critAddress2);
        }

        private void TxtAddress3_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtAddress3, critAddress3, e);
        }

        private void TxtAddress3_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtAddress3, critAddress3);
        }

        private void TxtPreviousName_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtPreviousName, critPreviousName, e);
        }

        private void TxtPreviousName_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtPreviousName, critPreviousName);
        }

        private void TxtPersonalName_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtPersonalName, critPersonalName);
        }

        private void TxtPersonalName_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtPersonalName, critPersonalName, e);
            DataRow PartnerClassDataRow;
            int selectedIndex = 0;

            if ((txtPersonalName.Text.Length > 0)
                && FShowAllPartnerClasses)
            {
                // Here we have a personal name
                // So make sure that only family and persons in the partner class
                // combo box

                selectedIndex = cmbPartnerClass.SelectedIndex;
                FPartnerClassDataTable.Rows.Clear();

                if (FRestrictedParterClasses.Length > 0)
                {
                    InsertRestrictedPartnerClassComboBox(false);
                }
                else
                {
                    PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                    PartnerClassDataRow["PartnerClass"] = "PERSON";
                    FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                    PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                    PartnerClassDataRow["PartnerClass"] = "FAMILY";
                    FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                }

                FShowAllPartnerClasses = false;

                // the partner class combo might not be on the form!
                if ((cmbPartnerClass.SelectedValue != null)
                    && (cmbPartnerClass.Items.Count > 0))
                {
                    // We need to perform these things to make the gui work properly
                    cmbPartnerClass.ResetBindings();

                    switch (cmbPartnerClass.Items.Count)
                    {
                        case 0:
                            break;

                        case 1:
                            cmbPartnerClass.SelectedIndex = 0;

                            if (txtPersonalName.Text.Length == 1)
                            {
                                txtPersonalName.SelectionStart = 1;
                            }

                            break;

                        default:
                        {
                            // Set Partner Class to Family or Person, depending on what the previous value was
                            // Need to change it twice to get a selected value changed event
                            switch (selectedIndex)
                            {
                                case 1:
                                    cmbPartnerClass.SelectedIndex = 1;
                                    cmbPartnerClass.SelectedIndex = 0;
                                    break;

                                default:
                                    cmbPartnerClass.SelectedIndex = 0;
                                    cmbPartnerClass.SelectedIndex = 1;
                                    break;
                            }

                            break;
                        }
                    }
                }
            }
            else if ((cmbPartnerClass.SelectedValue != null)
                     && (txtPersonalName.Text.Length == 0)
                     && !FShowAllPartnerClasses)
            {
                // We don't have a personal name
                // We show all available types in the partner class combo box.
                FPartnerClassDataTable.Rows.Clear();

                if (FRestrictedParterClasses.Length > 0)
                {
                    InsertRestrictedPartnerClassComboBox(true);
                }
                else
                {
                    InsertDefaultPartnerClassComboBox();
                }

                FShowAllPartnerClasses = true;
                cmbPartnerClass.ResetBindings();

                // Need to change the selection value twice to get a selected value changed event
                switch (cmbPartnerClass.Items.Count)
                {
                    case 0:
                        break;

                    case 1:
                        cmbPartnerClass.SelectedIndex = 0;
                        break;

                    default:
                        cmbPartnerClass.SelectedIndex = 1;
                        cmbPartnerClass.SelectedIndex = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Insert the restricted values into the partner class combo box
        /// </summary>
        /// <param name="AllPartnerClasses">true: all items are inserted. false: only family and person items are inserted</param>
        private void InsertRestrictedPartnerClassComboBox(Boolean AllPartnerClasses)
        {
            DataRow PartnerClassDataRow;

            if (!AllPartnerClasses)
            {
                foreach (String ClassItem in FRestrictedParterClasses)
                {
                    if ((ClassItem == "PERSON")
                        || (ClassItem == "FAMILY"))
                    {
                        PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                        PartnerClassDataRow["PartnerClass"] = ClassItem;
                        FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                    }

                    if (ClassItem == "WORKER-FAM")
                    {
                        PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                        PartnerClassDataRow["PartnerClass"] = "FAMILY";
                        FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                    }
                }
            }
            else
            {
                foreach (String ClassItem in FRestrictedParterClasses)
                {
                    if (ClassItem == "WORKER-FAM")
                    {
                        PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                        PartnerClassDataRow["PartnerClass"] = "FAMILY";
                        FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                    }
                    else
                    {
                        // just add item
                        PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                        PartnerClassDataRow["PartnerClass"] = ClassItem;
                        FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                    }
                }
            }
        }

        private void TxtPartnerName_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtPartnerName, critPartnerName, e);
        }

        private void TxtPartnerName_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtPartnerName, critPartnerName);
        }

        #endregion

        #region KeyPress Events which enable/disable other controls
        private void TxtPartnerKey_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (TUserDefaults.GetBooleanDefault(
                    TUserDefaults.PARTNER_FINDOPTIONS_EXACTPARTNERKEYMATCHSEARCH,
                    true))
            {
                if (System.Convert.ToInt64(txtPartnerKey.Text) == 0)
                {
                    this.EnableAllPanel();
                }

                if (System.Convert.ToInt64(txtPartnerKey.Text) != 0)
                {
                    this.DisableAllPanel(pnlPartnerKey);
                }
            }
        }

        private void TxtLocationKey_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (txtLocationKey.Text.Length == 0)
            {
                this.EnableAllPanel();
            }

            if (txtLocationKey.Text.Length > 0)
            {
                this.DisableAllPanel(pnlLocationKey);
            }
        }

        #endregion

        private void SetupPartnerClassComboBox()
        {
            // MessageBox.Show('SetupPartnerClassComboBox called');
            FPartnerClassDataTable = new DataTable("PartnerClass");
            FPartnerClassDataTable.Columns.Add("PartnerClass");
            FPartnerClassDataTable.PrimaryKey = new DataColumn[] {
                (FPartnerClassDataTable.Columns[0])
            };

            InsertDefaultPartnerClassComboBox();

            cmbPartnerClass.DataSource = FPartnerClassDataTable;
            cmbPartnerClass.DisplayMember = "PartnerClass";
            cmbPartnerClass.ValueMember = "PartnerClass";
            cmbPartnerClass.SelectedIndex = 0;

            FShowAllPartnerClasses = true;
        }

        /// <summary>
        /// Insert default values into the partner class combo box.
        /// </summary>
        private void InsertDefaultPartnerClassComboBox()
        {
            DataRow PartnerClassDataRow;

            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = '*';
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "PERSON";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "FAMILY";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "CHURCH";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "ORGANISATION";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "BANK";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "UNIT";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "VENUE";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
        }

        private void SetSearchCriteriaDefaultValues(ref DataRow SingleDataRow)
        {
            // MessageBox.Show('SetSearchCriteriaDefaultValues called');
            SingleDataRow["PartnerName"] = "";
            SingleDataRow["PersonalName"] = "";
            SingleDataRow["PreviousName"] = "";
            SingleDataRow["Address1"] = "";
            SingleDataRow["Address2"] = "";
            SingleDataRow["Address3"] = "";
            SingleDataRow["City"] = "";
            SingleDataRow["PostCode"] = "";
            SingleDataRow["County"] = "";
            SingleDataRow["Country"] = "";
            SingleDataRow["MailingAddressOnly"] = false;
            SingleDataRow["PartnerClass"] = FDefaultPartnerClass;
            SingleDataRow["PartnerKey"] = 0;
            SingleDataRow["PartnerStatus"] = "ACTIVE";
            SingleDataRow["Email"] = "";
            SingleDataRow["LocationKey"] = "";
            SingleDataRow["WORKERFAMONLY"] = false;
            SingleDataRow["PhoneNumber"] = "";
            SingleDataRow["ExactPartnerKeyMatch"] = true;
            SingleDataRow["PartnerNameMatch"] = "BEGINS";
            SingleDataRow["PersonalNameMatch"] = "BEGINS";
            SingleDataRow["PreviousNameMatch"] = "BEGINS";
            SingleDataRow["Address1Match"] = "BEGINS";
            SingleDataRow["Address2Match"] = "BEGINS";
            SingleDataRow["Address3Match"] = "BEGINS";
            SingleDataRow["CityMatch"] = "BEGINS";
            SingleDataRow["PostCodeMatch"] = "BEGINS";
            SingleDataRow["CountyMatch"] = "BEGINS";
            SingleDataRow["EmailMatch"] = "BEGINS";
            SingleDataRow["PhoneNumberMatch"] = "BEGINS";

            if (cmbPartnerClass.Items.Count > 0)
            {
                FPartnerClassDataTable.Rows.Clear();

                if ((FRestrictedParterClasses == null)
                    || ((FRestrictedParterClasses.Length) == 0))
                {
                    // use these default values only if we don't have the restricted partner classen
                    // which means if we are not in modal mode.
                    InsertDefaultPartnerClassComboBox();
                }
                else
                {
                    InsertRestrictedPartnerClassComboBox(true);
                }

                cmbPartnerClass.ResetBindings();
                cmbPartnerClass.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void ResetSearchCriteriaValuesToDefault()
        {
            DataRow SingleDataRow;

            if (FFindCriteriaDataTable.Rows.Count != 0)
            {
                SingleDataRow = FFindCriteriaDataTable.Rows[0];
                SingleDataRow.BeginEdit();
                SetSearchCriteriaDefaultValues(ref SingleDataRow);
                SingleDataRow.EndEdit();

                if (cmbPartnerClass.SelectedValue != null)
                {
                    // only do this if it is there on screen!
                    cmbPartnerClass.SelectedIndex = 0;
                }

                this.PartnerStatus = "ACTIVE";
            }
            else
            {
                SingleDataRow = FFindCriteriaDataTable.NewRow();
                SetSearchCriteriaDefaultValues(ref SingleDataRow);
                FFindCriteriaDataTable.Rows.Add(SingleDataRow);
            }

            FFindCriteriaDataTable.Rows[0].BeginEdit();
            FDefaultValues.ItemArray = FFindCriteriaDataTable.Rows[0].ItemArray;
            FShowAllPartnerClasses = true;
            this.EnableAllPanel();
            HandlePartnerClassGui();
        }

        /// <summary>
        /// Checks if the internal CriteriaDataTable has any criteria to search for.
        /// </summary>
        /// <returns>true if there are any search criterias </returns>
        public bool HasSearchCriteria()
        {
            if (FFindCriteriaDataTable.Rows.Count != 1)
            {
                return true;
            }

            DataRow SearchDataRow = FFindCriteriaDataTable.NewRow();
            SearchDataRow.ItemArray = FFindCriteriaDataTable.Rows[0].ItemArray;

            for (int Counter = 0; Counter < SearchDataRow.ItemArray.Length; ++Counter)
            {
                if (FFindCriteriaDataTable.Columns[Counter].ColumnName.EndsWith("Match"))
                {
                    // ignore changes of the Values like "ExactPartnerKeyMatch" or
                    // "EmailMatch" or "Address3Match"...
                    // because just a change in these values doesn't mean that there is a search criteria
                    continue;
                }

                if (FFindCriteriaDataTable.Columns[Counter].ColumnName.CompareTo("PartnerStatus") == 0)
                {
                    if ((SearchDataRow[Counter].ToString() == "ALL")
                        || (SearchDataRow[Counter].ToString() == "ACTIVE"))
                    {
                        // if there is partner status "All" or "Active" marked
                        // treat it as if there is no search criteria selected
                        continue;
                    }
                    else
                    {
                        return true;
                    }
                }

                if (SearchDataRow[Counter].ToString() != FDefaultValues[Counter].ToString())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitialiseCriteriaFields()
        {
            String LocalisedCountyLabel;
            String Dummy;

            ucoCountryComboBox.InitialiseUserControl();
            ucoCountryComboBox.AddNotSetRow("", "");
            ucoCountryComboBox.PerformDataBinding(FFindCriteriaDataTable, "Country");

            SetupPartnerClassComboBox();

            if (cmbPartnerClass.Items.Count > 0)
            {
                cmbPartnerClass.SelectedIndex = 0;
            }

            // Use Localised String for County Label
            LocalisedStrings.GetLocStrCounty(out LocalisedCountyLabel, out Dummy);

            if (LocalisedCountyLabel == Ict.Petra.Client.App.Gui.LocalisedStrings.COUNTY_DEFAULT_LABEL)
            {
                /*
                 * / The default label text of the County Label has a shortcut character
                 * / that conflicts with other shortcut character on this screen, so we
                 * / remove the shortcut character.
                 */
                LocalisedCountyLabel = LocalisedCountyLabel.Replace("&", "");
            }

            lblCounty.Text = LocalisedCountyLabel;

            /*
             * Restore 'Mailing Addresses Only' and 'Partner Status' Criteria
             * settings from UserDefaults
             */
            FindCriteriaUserDefaultRestore();

            ShowOrHidePartnerKeyMatchInfoText();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void AssociateCriteriaButtons()
        {
            critPartnerName.AssociatedTextBox = txtPartnerName;
            critPersonalName.AssociatedTextBox = txtPersonalName;
            critPreviousName.AssociatedTextBox = txtPreviousName;
            critAddress1.AssociatedTextBox = txtAddress1;
            critAddress2.AssociatedTextBox = txtAddress2;
            critAddress3.AssociatedTextBox = txtAddress3;
            critPostCode.AssociatedTextBox = txtPostCode;
            critCity.AssociatedTextBox = txtCity;
            critCounty.AssociatedTextBox = txtCounty;
            critEmail.AssociatedTextBox = txtEmail;
            critPhoneNumber.AssociatedTextBox = txtPhoneNumber;

            critPartnerName.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critPersonalName.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critPreviousName.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critAddress1.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critAddress2.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critAddress3.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critPostCode.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critCity.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critCounty.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critEmail.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critPhoneNumber.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
        }

        /// <summary>
        /// Restores 'Mailing Addresses Only' and 'Partner Status' Criteria
        /// settings from UserDefaults.
        /// </summary>
        void FindCriteriaUserDefaultRestore()
        {
            String FindCurrentAddrOnlyUserDefault;
            String PartnerStatusUserDefault;

            FindCurrentAddrOnlyUserDefault = TUserDefaults.GetStringDefault(
                TUserDefaults.PARTNER_FIND_CRIT_FINDCURRADDRONLY, "NO").ToUpper();

//            MessageBox.Show("FindCriteriaUserDefaultRestore:  FindCurrentAddrOnlyUserDefault: " + FindCurrentAddrOnlyUserDefault);

            if (FindCurrentAddrOnlyUserDefault == "YES")
            {
                FFindCriteriaDataTable.Rows[0]["MailingAddressOnly"] = true;
            }

            PartnerStatusUserDefault = TUserDefaults.GetStringDefault(
                TUserDefaults.PARTNER_FIND_CRIT_PARTNERSTATUS, "ACTIVE").ToUpper();

//            MessageBox.Show("FindCriteriaUserDefaultRestore:  PartnerStatusUserDefault: " + PartnerStatusUserDefault);

            if (PartnerStatusUserDefault == "ACTIVE")
            {
                rbtStatusActive.Checked = true;
                FFindCriteriaDataTable.Rows[0]["PartnerStatus"] = "ACTIVE";
            }
            else if (PartnerStatusUserDefault == "ALL")
            {
                rbtStatusAll.Checked = true;
                FFindCriteriaDataTable.Rows[0]["PartnerStatus"] = "ALL";
            }
            else
            {
                rbtPrivate.Checked = true;
                FFindCriteriaDataTable.Rows[0]["PartnerStatus"] = "PRIVATE";
            }
        }

        /// <summary>
        /// Saves 'Mailing Addresses Only' and 'Partner Status' Criteria
        /// settings to UserDefaults.
        /// </summary>
        public void FindCriteriaUserDefaultSave()
        {
            if (chkMailingAddressOnly.Checked)
            {
                TUserDefaults.SetDefault(
                    TUserDefaults.PARTNER_FIND_CRIT_FINDCURRADDRONLY, "YES");
            }
            else
            {
                TUserDefaults.SetDefault(
                    TUserDefaults.PARTNER_FIND_CRIT_FINDCURRADDRONLY, "NO");
            }

            if (rbtStatusActive.Checked)
            {
                TUserDefaults.SetDefault(
                    TUserDefaults.PARTNER_FIND_CRIT_PARTNERSTATUS, "ACTIVE");
            }
            else if (rbtStatusAll.Checked)
            {
                TUserDefaults.SetDefault(
                    TUserDefaults.PARTNER_FIND_CRIT_PARTNERSTATUS, "ALL");
            }
            else
            {
                TUserDefaults.SetDefault(
                    TUserDefaults.PARTNER_FIND_CRIT_PARTNERSTATUS, "PRIVATE");
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitUserControl()
        {
            SingleLineFlow LayoutManagerLeftColumn = new SingleLineFlow(pnlLeftColumn, 1, 1);

            LayoutManagerLeftColumn.SpacerDistance = 7;
            SingleLineFlow LayoutManagerRightColumn = new SingleLineFlow(pnlRightColumn, 1, 1);
            LayoutManagerRightColumn.SpacerDistance = 7;
            FWorkerFamOnly = false;
            FDefaultPartnerClass = "*";

            if (!DesignMode)
            {
                ResetSearchCriteriaValuesToDefault();
            }

            ucoPartnerFind_PersonnelCriteria_CollapsiblePart.InitialiseUserControl();
            FCriteriaFieldsLeft = new ArrayList();
            FCriteriaFieldsRight = new ArrayList();

            // FFindCriteriaDataTable := new DataTable('FindCriteria');
            FFindCriteriaDataTable.ColumnChanging += new DataColumnChangeEventHandler(this.OnCriteriaChanging);

            // Set status bar texts
            FPetraUtilsObject.SetStatusBarText(txtPartnerName, Resourcestrings.StrPartnerNameFindHelptext);
            FPetraUtilsObject.SetStatusBarText(txtPersonalName, Resourcestrings.StrPersonalNameFindHelpText);
            FPetraUtilsObject.SetStatusBarText(txtPreviousName, StrPreviousNameFindHelpText);
            FPetraUtilsObject.SetStatusBarText(txtEmail, Resourcestrings.StrEmailAddressHelpText);
            FPetraUtilsObject.SetStatusBarText(txtAddress1, Resourcestrings.StrAddress1Helptext);
            FPetraUtilsObject.SetStatusBarText(txtAddress2, Resourcestrings.StrAddress2Helptext);
            FPetraUtilsObject.SetStatusBarText(txtAddress3, Resourcestrings.StrAddress3Helptext);
            FPetraUtilsObject.SetStatusBarText(txtCity, Resourcestrings.StrCityHelptext);
            FPetraUtilsObject.SetStatusBarText(txtPostCode, Resourcestrings.StrPostCodeHelpText);
            FPetraUtilsObject.SetStatusBarText(txtCounty, Resourcestrings.StrCountyHelpText);
            FPetraUtilsObject.SetStatusBarText(cmbPartnerClass, StrPartnerClassFindHelpText);
            FPetraUtilsObject.SetStatusBarText(txtLocationKey, Resourcestrings.StrLocationKeyHelpText + Resourcestrings.StrLocationKeyExtraHelpText);
            FPetraUtilsObject.SetStatusBarText(btnLocationKey, Resourcestrings.StrLocationKeyButtonFindHelpText);
            FPetraUtilsObject.SetStatusBarText(chkWorkerFamOnly, StrWorkerFamilyOnlyFindHelpText);
            FPetraUtilsObject.SetStatusBarText(rbtStatusActive, Resourcestrings.StrActivePartnersFindHelpText);
            FPetraUtilsObject.SetStatusBarText(rbtStatusAll, Resourcestrings.StrAllPartnersFindHelpText);
            FPetraUtilsObject.SetStatusBarText(chkMailingAddressOnly, Resourcestrings.StrMailingOnlyFindHelpText);
            FPetraUtilsObject.SetStatusBarText(txtPhoneNumber, Resourcestrings.StrPhoneNumberFindHelpText);
            FPetraUtilsObject.SetStatusBarText(txtPartnerKey, Resourcestrings.StrPartnerKeyFindHelpText);
            FPetraUtilsObject.SetStatusBarText(ucoCountryComboBox, Resourcestrings.StrCountryHelpText);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns>void</returns>
        private void ReturnCriteriaFieldControls(out TSelfExpandingArrayList CriteriaFieldsLeftControls,
            out TSelfExpandingArrayList CriteriaFieldsRightControls)
        {
            Control TheControl;

            FieldInfo[] FieldsInForm;
            TSelfExpandingArrayList InternalCriteriaFieldLeftControls;
            TSelfExpandingArrayList InternalCriteriaFieldRightControls;
            Int32 Counter1;
            Int32 Counter3;
            Int32 PositionInArray;
            String SimplifiedControlName;
            Boolean FoundInLeftColumnArray;
            Boolean FoundInRightColumnArray;

            // init some variables:
            InternalCriteriaFieldLeftControls = null;
            InternalCriteriaFieldRightControls = null;

            // MessageBox.Show('FCriteriaFieldsLeft.Count: ' + FCriteriaFieldsLeft.Count.ToString + '; FCriteriaFieldsRight.Count: ' + FCriteriaFieldsRight.Count.ToString);
            if ((FCriteriaFieldsLeft.Count > 0) || (FCriteriaFieldsRight.Count > 0))
            {
                // FFindCriteriaDataTable.Columns.Clear();
                InternalCriteriaFieldLeftControls = new TSelfExpandingArrayList();
                InternalCriteriaFieldRightControls = new TSelfExpandingArrayList();
                FieldsInForm = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                for (Counter1 = 0; Counter1 <= FieldsInForm.Length - 1; Counter1 += 1)
                {
                    FoundInLeftColumnArray = false;
                    FoundInRightColumnArray = false;

                    // MessageBox.Show('Inspecting Field ''' + FieldsInForm[Counter1].Name + ''': FieldType: ' + FieldsInForm[Counter1].FieldType.ToString);
                    if (FieldsInForm[Counter1].FieldType.ToString() == "System.Windows.Forms.Panel")
                    {
                        if (FieldsInForm[Counter1].GetValue(this) is Control)
                        {
                            TheControl = (Control)FieldsInForm[Counter1].GetValue(this);

                            // MessageBox.Show('Inspecting Panel ''' + TheControl.Name + '''...' );
                            SimplifiedControlName = (TheControl.Name).Substring(3);
                            PositionInArray = FCriteriaFieldsLeft.IndexOf(SimplifiedControlName);

                            if (PositionInArray != -1)
                            {
                                FoundInLeftColumnArray = true;
                                FoundInRightColumnArray = false;
                            }
                            else
                            {
                                // MessageBox.Show('Processing SimplifiedControlName: ' + SimplifiedControlName);
                                PositionInArray = FCriteriaFieldsRight.IndexOf(SimplifiedControlName);

                                if (PositionInArray != -1)
                                {
                                    FoundInRightColumnArray = true;
                                    FoundInLeftColumnArray = false;
                                }
                            }

                            // MessageBox.Show('FoundInLeftColumnArray: ' + FoundInLeftColumnArray.ToString + '; FoundInRightColumnArray: ' +FoundInRightColumnArray.ToString);
                            TheControl.Tag = null;
                            TheControl.Visible = true;

                            if ((FoundInLeftColumnArray) || (FoundInRightColumnArray))
                            {
                                if (FoundInLeftColumnArray)
                                {
                                    // MessageBox.Show('Found Panel ''' + TheControl.Name + ''' in FCriteriaFieldsLeft @ position ' + PositionInArray.ToString );
                                    if (PositionInArray != 0)
                                    {
                                        if (FCriteriaFieldsLeft[PositionInArray - 1].ToString() == StrSpacer)
                                        {
                                            TheControl.Tag = StrBeginGroup;
                                        }
                                    }

                                    // MessageBox.Show('Adding TheControl ''' + TheControl.Name + ''' to InternalCriteriaFieldLeftControls @ Position ' + PositionInArray.ToString);
                                    InternalCriteriaFieldLeftControls[PositionInArray] = TheControl;

                                    // FFindCriteriaDataTable.Columns.Add(TheControl.Name.Substring(3));
                                }
                                else
                                {
                                    // MessageBox.Show('Found Panel ''' + TheControl.Name + ''' in FCriteriaFieldsRight @ position ' + PositionInArray.ToString );
                                    if (PositionInArray != 0)
                                    {
                                        if (FCriteriaFieldsRight[PositionInArray - 1].ToString() == StrSpacer)
                                        {
                                            TheControl.Tag = StrBeginGroup;
                                        }
                                    }

                                    // if InternalCriteriaFieldRightControls.Count  1 <= PositionInArray then
                                    // begin
                                    // InternalCriteriaFieldRightControls = new SomeType[PositionInArray + 1];
                                    // end;
                                    // MessageBox.Show('Adding TheControl ''' + TheControl.Name + ''' to InternalCriteriaFieldRightControls @ Position ' + PositionInArray.ToString);
                                    InternalCriteriaFieldRightControls[PositionInArray] = TheControl;

                                    // FFindCriteriaDataTable.Columns.Add(TheControl.Name.Substring(3));
                                }

                                // FoundInLeftColumnArray
                            }

                            // (FoundInLeftColumnArray) or (FoundInRightColumnArray
                        }

                        // FieldsInForm[Counter1].GetValue(this) is Control
                    }

                    // FieldsInForm[Counter1].FieldType.ToString = 'System.Windows.Forms.Panel'
                }

                // Counter1 := 0 To Length(FieldsInForm)  1
                // Check for and remove all empty entries (which could come from Spacers or invalid panel names)
                // MessageBox.Show('Compacting...');
                InternalCriteriaFieldLeftControls.Compact();
            }

            // (FCriteriaFieldsLeft.Count > 0) or (FCriteriaFieldsRight.Count > 0)
            // MessageBox.Show('Building dynamic output array...');
            // Build output dynamic array
            if (InternalCriteriaFieldLeftControls != null)
            {
                InternalCriteriaFieldLeftControls.TrimToSize();
                CriteriaFieldsLeftControls = InternalCriteriaFieldLeftControls;
            }
            else
            {
                CriteriaFieldsLeftControls = new TSelfExpandingArrayList();
            }

            if (InternalCriteriaFieldRightControls != null)
            {
                InternalCriteriaFieldRightControls.TrimToSize();
                CriteriaFieldsRightControls = InternalCriteriaFieldRightControls;
            }
            else
            {
                CriteriaFieldsRightControls = new TSelfExpandingArrayList();
            }

            if (FCriteriaSetupMode)
            {
                // If the number of items in FCriteriaFieldsLeft is not equal to
                // the number of items in CriteriaFieldsLeftControls then there
                // were invalid entries in FCriteriaFieldsLeft (that don't represent valid panels).
                // In this case rebuild FCriteriaFieldsLeft so that it contains only valid
                // entries.
                // MessageBox.Show('FCriteriaFieldsLeft.Count: ' + FCriteriaFieldsLeft.Count.ToString +
                // '; CriteriaFieldsLeftControls.Count  1: ' + Int16(CriteriaFieldsLeftControls.Count  1).ToString );
                if (FCriteriaFieldsLeft.Count != CriteriaFieldsLeftControls.Count - 1)
                {
                    FCriteriaFieldsLeft.Clear();

                    for (Counter3 = 0; Counter3 <= CriteriaFieldsLeftControls.Count - 1; Counter3 += 1)
                    {
                        if (((Control)CriteriaFieldsLeftControls[Counter3]).Tag != null)
                        {
                            // MessageBox.Show('CriteriaFieldsLeftControls[' + Counter3.ToString + ']:' + CriteriaFieldsLeftControls[Counter3].ToString + ' has Tag.');
                            if (((Control)CriteriaFieldsLeftControls[Counter3]).Tag.ToString() == StrBeginGroup)
                            {
                                FCriteriaFieldsLeft.Add(StrSpacer);

                                // MessageBox.Show('Adding Spacer before control ' + (CriteriaFieldsLeftControls[Counter3] as Control).Name);
                            }
                        }

                        FCriteriaFieldsLeft.Add((((Control)CriteriaFieldsLeftControls[Counter3]).Name).Substring(3));
                    }
                }

                // If the number of items in FCriteriaFieldsRight is not equal to
                // the number of items in CriteriaFieldsRightControls then there
                // were invalid entries in FCriteriaFieldsRight (that don't represent valid panels).
                // In this case rebuild FCriteriaFieldsRight so that it contains only valid
                // entries.
                // MessageBox.Show('FCriteriaFieldsRight.Count: ' + FCriteriaFieldsRight.Count.ToString +
                // '; CriteriaFieldsRightControls.Count  1: ' + Int16(CriteriaFieldsRightControls.Count  1).ToString );
                if (FCriteriaFieldsRight.Count != CriteriaFieldsRightControls.Count - 1)
                {
                    FCriteriaFieldsRight.Clear();

                    for (Counter3 = 0; Counter3 <= CriteriaFieldsRightControls.Count - 1; Counter3 += 1)
                    {
                        if (((Control)CriteriaFieldsRightControls[Counter3]).Tag != null)
                        {
                            // MessageBox.Show('CriteriaFieldsRightControls[' + Counter3.ToString + ']:' + CriteriaFieldsRightControls[Counter3].ToString + ' has Tag.');
                            if (((Control)CriteriaFieldsRightControls[Counter3]).Tag.ToString() == StrBeginGroup)
                            {
                                FCriteriaFieldsRight.Add(StrSpacer);

                                // MessageBox.Show('Adding Spacer before control' + (CriteriaFieldsRightControls[Counter3] as Control).Name);
                            }
                        }

                        FCriteriaFieldsRight.Add((((Control)CriteriaFieldsRightControls[Counter3]).Name).Substring(3));
                    }
                }
            }
        }

        private void CustomDisablePanels(Control ControlGroup)
        {
            Int16 Counter1;

            // Counter3: Int16;
            // InvisiblePanel: System.Windows.Forms.Panel;
            // TransparentRegion: System.Drawing.Region;
            for (Counter1 = 0; Counter1 <= ControlGroup.Controls.Count - 1; Counter1 += 1)
            {
                CustomDisableCriteria(ControlGroup.Controls[Counter1]);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="PanelName"></param>
        public void DisableCriteria(String PanelName)
        {
            Control TheControl;

            FieldInfo[] FieldsInForm;
            Int16 Counter1;
            FieldsInForm = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            for (Counter1 = 0; Counter1 <= FieldsInForm.Length - 1; Counter1 += 1)
            {
                // MessageBox.Show('Inspecting Field ''' + FieldsInForm[Counter1].Name + ''': FieldType: ' + FieldsInForm[Counter1].FieldType.ToString);
                if (FieldsInForm[Counter1].FieldType.ToString() == "System.Windows.Forms.Panel")
                {
                    if (FieldsInForm[Counter1].GetValue(this) is Control)
                    {
                        TheControl = (Control)FieldsInForm[Counter1].GetValue(this);

                        if (TheControl.Name == "pnl" + PanelName)
                        {
                            CustomDisableCriteria(TheControl);
                            return;
                        }
                    }
                }
            }
        }

        private void CustomDisableCriteria(Control ControlGroup)
        {
            Int16 Counter2;

            if (ControlGroup is Panel)
            {
                // MessageBox.Show('Disabling Controls in Panel: ' + ControlGroup.Name);
                for (Counter2 = 0; Counter2 <= ControlGroup.Controls.Count - 1; Counter2 += 1)
                {
                    // MessageBox.Show('Disabling Control: ' + ControlGroup.Controls[Counter2].Name);
                    if (ControlGroup.Controls[Counter2] is System.Windows.Forms.Label)
                    {
                        ControlGroup.Controls[Counter2].Click += new EventHandler(this.CorrespondingLabel_Click);
                    }
                    else if (ControlGroup.Controls[Counter2] is System.Windows.Forms.UserControl)
                    {
                        ControlGroup.Controls[Counter2].Click += new EventHandler(this.CorrespondingLabel_Click);
                        (ControlGroup.Controls[Counter2]).BackColor = this.BackColor;

                        // MessageBox.Show('Calling CustomDisablePanels for UC: ' + ControlGroup.Controls[Counter2].Name);
                        CustomDisablePanels(ControlGroup.Controls[Counter2]);
                    }
                    else
                    {
                        ControlGroup.Controls[Counter2].Enabled = false;
                    }
                }
            }
            else
            {
                CustomEnablingDisabling.DisableControl(ControlGroup.Parent, ControlGroup);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void CompleteBindings()
        {
            this.BindingContext[CriteriaData].EndCurrentEdit();
        }

        private void CorrespondingLabel_Click(System.Object sender, System.EventArgs e)
        {
            if (!((((System.Windows.Forms.Control)sender).Parent).Parent is UserControl))
            {
                FSelectedPanel = SelectCriteriaPanel(((System.Windows.Forms.Control)sender).Name.Substring(3));
            }
            else
            {
                // MessageBox.Show('Klicked on Panel in UC ''' + ((sender as System.Windows.Forms.Control).Parent).Parent.Name + ''', selecting Panel ''' + (((sender as System.Windows.Forms.Control).Parent).Parent).Parent.Name + '''.');
                FSelectedPanel = SelectCriteriaPanel(((((System.Windows.Forms.Control)sender).Parent).Parent).Parent.Name.Substring(3));
                (((UserControl)((System.Windows.Forms.Control)sender).Parent).Parent).BackColor = System.Drawing.SystemColors.InactiveCaption;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ACriteriaName"></param>
        /// <returns></returns>
        public TSelectedCriteriaPanel SelectCriteriaPanel(String ACriteriaName)
        {
            System.Windows.Forms.Panel SearchedPanel;
            System.Windows.Forms.Panel OtherPanel;
            Int32 Counter1;
            Int32 Counter2;
            Int32 NotFoundInPanelCounter;
            TPartnerFindCriteriaSelectionChangedEventArgs FindCriteriaSelectionChangedArgs;
            SearchedPanel = pnlLeftColumn;
            OtherPanel = pnlRightColumn;
            NotFoundInPanelCounter = 0;

            while (1 == 1)
            {
                for (Counter1 = 0; Counter1 <= SearchedPanel.Controls.Count - 1; Counter1 += 1)
                {
                    if (SearchedPanel.Controls[Counter1] is Panel)
                    {
                        if (SearchedPanel.Controls[Counter1].Name == "pnl" + ACriteriaName)
                        {
                            // Highlight the correct panel
                            // MessageBox.Show('Highlighting Panel: ' + SearchedPanel.Controls[Counter1].Name );
                            SearchedPanel.Controls[Counter1].BackColor = System.Drawing.SystemColors.InactiveCaption;

                            // Dehighlight all other panels in the SearchedPanel
                            for (Counter2 = 0; Counter2 <= SearchedPanel.Controls.Count - 1; Counter2 += 1)
                            {
                                if ((SearchedPanel.Controls[Counter2] is Panel)
                                    && (SearchedPanel.Controls[Counter2].Name != SearchedPanel.Controls[Counter1].Name))
                                {
                                    SearchedPanel.Controls[Counter2].BackColor = this.BackColor;

                                    // pnlPersonnelCriteria contains a UserControls which BackColor needs also to be reset
                                    if (SearchedPanel.Controls[Counter2].Name == "pnlPersonnelCriteria")
                                    {
                                        SearchedPanel.Controls[Counter2].Controls[0].BackColor = this.BackColor;
                                    }
                                }
                            }

                            // Dehighlight all panels in the OtherPanel
                            for (Counter2 = 0; Counter2 <= OtherPanel.Controls.Count - 1; Counter2 += 1)
                            {
                                if (OtherPanel.Controls[Counter2] is Panel)
                                {
                                    OtherPanel.Controls[Counter2].BackColor = this.BackColor;

                                    // pnlPersonnelCriteria contains a UserControls which BackColor needs also to be reset
                                    if (OtherPanel.Controls[Counter2].Name == "pnlPersonnelCriteria")
                                    {
                                        OtherPanel.Controls[Counter2].Controls[0].BackColor = this.BackColor;
                                    }
                                }
                            }

                            // Raise FindCriteriaSelectionChangedArgs event
                            FindCriteriaSelectionChangedArgs = new TPartnerFindCriteriaSelectionChangedEventArgs();
                            FindCriteriaSelectionChangedArgs.SelectedCriteria = SearchedPanel.Controls[Counter1].Name.Substring(3);

                            if (SearchedPanel == pnlLeftColumn)
                            {
                                FindCriteriaSelectionChangedArgs.CriteriaColumn = TFindCriteriaColumn.fccLeft;
                            }
                            else
                            {
                                FindCriteriaSelectionChangedArgs.CriteriaColumn = TFindCriteriaColumn.fccRight;
                            }

                            FindCriteriaSelectionChangedArgs.IsFirstInColumn = false;
                            FindCriteriaSelectionChangedArgs.IsLastInColumn = false;

                            if (Counter1 == 0)
                            {
                                FindCriteriaSelectionChangedArgs.IsFirstInColumn = true;
                            }

                            if (Counter1 == SearchedPanel.Controls.Count - 1)
                            {
                                FindCriteriaSelectionChangedArgs.IsLastInColumn = true;
                            }

                            // MessageBox.Show('Raising FindCriteriaSelectionChanged event.');
                            OnFindCriteriaSelectionChanged(FindCriteriaSelectionChangedArgs);
                            return new TSelectedCriteriaPanel(((Panel)SearchedPanel.Controls[Counter1]),
                                FindCriteriaSelectionChangedArgs.CriteriaColumn,
                                FindCriteriaSelectionChangedArgs.IsFirstInColumn,
                                FindCriteriaSelectionChangedArgs.IsLastInColumn,
                                Counter1);
                        }
                    }
                }

                NotFoundInPanelCounter = NotFoundInPanelCounter + 1;

                if (NotFoundInPanelCounter == 1)
                {
                    // Panel with matching name not found in pnlLeftColumn, try in pnlRightColumn
                    SearchedPanel = pnlRightColumn;
                    OtherPanel = pnlLeftColumn;
                }
                else
                {
                    // No panel with matching name found!
                    return null;
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ACriteriaName"></param>
        /// <returns></returns>
        public Boolean UnSelectCriteriaPanel(String ACriteriaName)
        {
            System.Windows.Forms.Panel SearchedPanel;
            System.Windows.Forms.Panel OtherPanel;
            TPartnerFindCriteriaSelectionChangedEventArgs FindCriteriaSelectionChangedArgs;
            Int32 Counter1;
            Int32 NotFoundInPanelCounter;
            SearchedPanel = pnlLeftColumn;
            OtherPanel = pnlRightColumn;
            NotFoundInPanelCounter = 0;

            while (1 == 1)
            {
                for (Counter1 = 0; Counter1 <= SearchedPanel.Controls.Count - 1; Counter1 += 1)
                {
                    if (SearchedPanel.Controls[Counter1] is Panel)
                    {
                        if (SearchedPanel.Controls[Counter1].Name == "pnl" + ACriteriaName)
                        {
                            // Dehighlight the correct panel
                            // MessageBox.Show('Dehighlighting Panel: ' + SearchedPanel.Controls[Counter1].Name );
                            SearchedPanel.Controls[Counter1].BackColor = SearchedPanel.Controls[Counter1].Parent.BackColor;

                            if (!(FSelectedPanel == null))
                            {
                                if ("pnl" + ACriteriaName == FSelectedPanel.SelectedCriteriaPanel.Name)
                                {
                                    // Raise FindCriteriaSelectionChangedArgs event to signal that no criteria is selected
                                    FindCriteriaSelectionChangedArgs = new TPartnerFindCriteriaSelectionChangedEventArgs();
                                    FindCriteriaSelectionChangedArgs.SelectedCriteria = null;
                                    FindCriteriaSelectionChangedArgs.CriteriaColumn = TFindCriteriaColumn.fccLeft;
                                    FindCriteriaSelectionChangedArgs.IsFirstInColumn = false;
                                    FindCriteriaSelectionChangedArgs.IsLastInColumn = false;

                                    // MessageBox.Show('Raising FindCriteriaSelectionChanged event.');
                                    OnFindCriteriaSelectionChanged(FindCriteriaSelectionChangedArgs);
                                    FSelectedPanel = null;
                                }
                            }

                            return true;
                        }
                    }
                }

                NotFoundInPanelCounter = NotFoundInPanelCounter + 1;

                if (NotFoundInPanelCounter == 1)
                {
                    // Panel with matching name not found in pnlLeftColumn, try in pnlRightColumn
                    SearchedPanel = pnlRightColumn;
                    OtherPanel = pnlLeftColumn;
                }
                else
                {
                    // No panel with matching name found!
                    return false;
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void DisplayCriteriaFieldControls()
        {
            TSelfExpandingArrayList CriteriaFieldsLeftControls;
            TSelfExpandingArrayList CriteriaFieldsRightControls;

            Control[] TempCriteriaFieldsLeftControls;
            Control[] TempCriteriaFieldsRightControls;

            Boolean MatchButtonsSetting;

            ReturnCriteriaFieldControls(out CriteriaFieldsLeftControls, out CriteriaFieldsRightControls);

            if (CriteriaFieldsLeftControls.Count != 0)
            {
                TempCriteriaFieldsLeftControls = new Control[CriteriaFieldsLeftControls.Count + 1];
                CriteriaFieldsLeftControls.CopyTo(TempCriteriaFieldsLeftControls);

                // remove all controls first
                pnlLeftColumn.Controls.Clear();

                // add specified controls
                pnlLeftColumn.Controls.AddRange(TempCriteriaFieldsLeftControls);
            }
            else
            {
                // remove all controls
                pnlLeftColumn.Controls.Clear();
            }

            MatchButtonsSetting = TUserDefaults.GetBooleanDefault(TUserDefaults.PARTNER_FINDOPTIONS_SHOWMATCHBUTTONS, true);

            if (CriteriaFieldsRightControls.Count != 0)
            {
                TempCriteriaFieldsRightControls = new Control[CriteriaFieldsRightControls.Count + 1];
                CriteriaFieldsRightControls.CopyTo(TempCriteriaFieldsRightControls);

                // remove all controls first
                pnlRightColumn.Controls.Clear();

                // add specified controls
                pnlRightColumn.Controls.AddRange(TempCriteriaFieldsRightControls);
            }
            else
            {
                // remove all controls
                pnlRightColumn.Controls.Clear();
            }

            // This section shows / hides the MatchButtons
            // and adjusts the textbox widths accordingly
            TidyPanel(ref pnlLeftColumn, 38, 7, MatchButtonsSetting);
            TidyPanel(ref pnlRightColumn, 38, 7, MatchButtonsSetting);
        }

        private void TidyPanel(ref Panel p, Int32 MatchButtonWidth, Int32 GapWidth, Boolean WithMatchButtons)
        {
            foreach (Control TempControl in p.Controls)
            {
                // tempcontroll
                foreach (Control TempInnerControl in TempControl.Controls)
                {
                    // tempInnerControl
                    if (TempInnerControl.GetType() == typeof(SplitButton))
                    {
                        // tempInnerControl.Type =      SplitButton
                        TempInnerControl.Visible = WithMatchButtons;
                    }

                    // tempInnerControl.Type =     SplitButton
                }

                // tempInnerControl
            }

            // tempcontroll
        }

        /// <summary>
        /// Shows or hides Match info text for Partner Key Crtieria
        /// </summary>
        public void ShowOrHidePartnerKeyMatchInfoText()
        {
            // Show Match info for Partner Key if Non-Exact Matching is enabled
            if (!TUserDefaults.GetBooleanDefault(
                    TUserDefaults.PARTNER_FINDOPTIONS_EXACTPARTNERKEYMATCHSEARCH,
                    true))
            {
                lblPartnerKeyNonExactMatch.Visible = true;
            }
            else
            {
                lblPartnerKeyNonExactMatch.Visible = false;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="args"></param>
        protected void OnFindCriteriaSelectionChanged(TPartnerFindCriteriaSelectionChangedEventArgs args)
        {
            if (FindCriteriaSelectionChanged != null)
            {
                FindCriteriaSelectionChanged(this, args);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void MoveUpSelectedCriteriaPanel()
        {
            ArrayList CriteriaFieldsArrayList;
            TPartnerFindCriteriaSelectionChangedEventArgs FindCriteriaSelectionChangedArgs;
            Int32 CurrentIndexInFieldsArrayList;

            if (!FSelectedPanel.IsFirstInColumn)
            {
                // MessageBox.Show('Moving item up');
                if (FSelectedPanel.CriteriaColumn == TFindCriteriaColumn.fccLeft)
                {
                    CriteriaFieldsArrayList = FCriteriaFieldsLeft;
                }
                else
                {
                    CriteriaFieldsArrayList = FCriteriaFieldsRight;
                }

                CurrentIndexInFieldsArrayList = CriteriaFieldsArrayList.IndexOf(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                CriteriaFieldsArrayList.Remove(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                CriteriaFieldsArrayList.Insert(CurrentIndexInFieldsArrayList - 1, FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                FSelectedPanel.PositionInCriteriaColumn = FSelectedPanel.PositionInCriteriaColumn - 1;
                DisplayCriteriaFieldControls();

                // Raise FindCriteriaSelectionChangedArgs event and update IsFirstInColumn + IsLastInColumn
                FindCriteriaSelectionChangedArgs = new TPartnerFindCriteriaSelectionChangedEventArgs();
                FindCriteriaSelectionChangedArgs.SelectedCriteria = FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3);
                FindCriteriaSelectionChangedArgs.CriteriaColumn = FSelectedPanel.CriteriaColumn;
                FindCriteriaSelectionChangedArgs.IsFirstInColumn = false;
                FindCriteriaSelectionChangedArgs.IsLastInColumn = false;
                FSelectedPanel.IsFirstInColumn = false;
                FSelectedPanel.IsLastInColumn = false;

                if (CurrentIndexInFieldsArrayList - 1 == 0)
                {
                    FindCriteriaSelectionChangedArgs.IsFirstInColumn = true;
                    FSelectedPanel.IsFirstInColumn = true;
                }

                // MessageBox.Show('Raising FindCriteriaSelectionChanged event.');
                OnFindCriteriaSelectionChanged(FindCriteriaSelectionChangedArgs);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void MoveDownSelectedCriteriaPanel()
        {
            ArrayList CriteriaFieldsArrayList;
            TPartnerFindCriteriaSelectionChangedEventArgs FindCriteriaSelectionChangedArgs;
            Int32 CurrentIndexInFieldsArrayList;

            if (!FSelectedPanel.IsLastInColumn)
            {
                // MessageBox.Show('Moving item down');
                if (FSelectedPanel.CriteriaColumn == TFindCriteriaColumn.fccLeft)
                {
                    CriteriaFieldsArrayList = FCriteriaFieldsLeft;
                }
                else
                {
                    CriteriaFieldsArrayList = FCriteriaFieldsRight;
                }

                CurrentIndexInFieldsArrayList = CriteriaFieldsArrayList.IndexOf(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                CriteriaFieldsArrayList.Remove(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                CriteriaFieldsArrayList.Insert(CurrentIndexInFieldsArrayList + 1, FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                FSelectedPanel.PositionInCriteriaColumn = FSelectedPanel.PositionInCriteriaColumn + 1;
                DisplayCriteriaFieldControls();

                // Raise FindCriteriaSelectionChangedArgs event and update IsFirstInColumn + IsLastInColumn
                FindCriteriaSelectionChangedArgs = new TPartnerFindCriteriaSelectionChangedEventArgs();
                FindCriteriaSelectionChangedArgs.SelectedCriteria = FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3);
                FindCriteriaSelectionChangedArgs.CriteriaColumn = FSelectedPanel.CriteriaColumn;
                FindCriteriaSelectionChangedArgs.IsFirstInColumn = false;
                FindCriteriaSelectionChangedArgs.IsLastInColumn = false;
                FSelectedPanel.IsFirstInColumn = false;
                FSelectedPanel.IsLastInColumn = false;

                if (CurrentIndexInFieldsArrayList + 1 == CriteriaFieldsArrayList.Count - 1)
                {
                    FindCriteriaSelectionChangedArgs.IsLastInColumn = true;
                    FSelectedPanel.IsLastInColumn = true;
                }

                // MessageBox.Show('Raising FindCriteriaSelectionChanged event.');
                OnFindCriteriaSelectionChanged(FindCriteriaSelectionChangedArgs);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void MoveLeftSelectedCriteriaPanel()
        {
            TPartnerFindCriteriaSelectionChangedEventArgs FindCriteriaSelectionChangedArgs;

            if (FSelectedPanel.CriteriaColumn == TFindCriteriaColumn.fccRight)
            {
                FCriteriaFieldsRight.Remove(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));

                // MessageBox.Show('PositionInCriteriaColumn: ' + FSelectedPanel.PositionInCriteriaColumn.ToString +
                // 'FCriteriaFieldsLeft.Count  1: ' + Int16(FCriteriaFieldsLeft.Count  1).ToString);
                if (FSelectedPanel.PositionInCriteriaColumn <= FCriteriaFieldsLeft.Count - 1)
                {
                    FCriteriaFieldsLeft.Insert(FSelectedPanel.PositionInCriteriaColumn, FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                }
                else
                {
                    FSelectedPanel.PositionInCriteriaColumn = FCriteriaFieldsLeft.Add(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                }

                FSelectedPanel.CriteriaColumn = TFindCriteriaColumn.fccLeft;
                DisplayCriteriaFieldControls();

                // Raise FindCriteriaSelectionChangedArgs event and update IsFirstInColumn + IsLastInColumn
                FindCriteriaSelectionChangedArgs = new TPartnerFindCriteriaSelectionChangedEventArgs();
                FindCriteriaSelectionChangedArgs.SelectedCriteria = FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3);
                FindCriteriaSelectionChangedArgs.CriteriaColumn = FSelectedPanel.CriteriaColumn;
                FindCriteriaSelectionChangedArgs.IsFirstInColumn = false;
                FindCriteriaSelectionChangedArgs.IsLastInColumn = false;
                FSelectedPanel.IsFirstInColumn = false;
                FSelectedPanel.IsLastInColumn = false;

                if (FSelectedPanel.PositionInCriteriaColumn == 0)
                {
                    FindCriteriaSelectionChangedArgs.IsFirstInColumn = true;
                    FSelectedPanel.IsFirstInColumn = true;
                }

                if (FSelectedPanel.PositionInCriteriaColumn == FCriteriaFieldsLeft.Count - 1)
                {
                    FindCriteriaSelectionChangedArgs.IsLastInColumn = true;
                    FSelectedPanel.IsLastInColumn = true;
                }

                // MessageBox.Show('Raising FindCriteriaSelectionChanged event.');
                OnFindCriteriaSelectionChanged(FindCriteriaSelectionChangedArgs);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void MoveRightSelectedCriteriaPanel()
        {
            TPartnerFindCriteriaSelectionChangedEventArgs FindCriteriaSelectionChangedArgs;

            if (FSelectedPanel.CriteriaColumn == TFindCriteriaColumn.fccLeft)
            {
                FCriteriaFieldsLeft.Remove(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));

                // MessageBox.Show('PositionInCriteriaColumn: ' + FSelectedPanel.PositionInCriteriaColumn.ToString +
                // 'FCriteriaFieldsRight.Count  1: ' + Int16(FCriteriaFieldsRight.Count  1).ToString);
                if (FSelectedPanel.PositionInCriteriaColumn <= FCriteriaFieldsRight.Count - 1)
                {
                    FCriteriaFieldsRight.Insert(FSelectedPanel.PositionInCriteriaColumn, FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                }
                else
                {
                    FSelectedPanel.PositionInCriteriaColumn = FCriteriaFieldsRight.Add(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                }

                FSelectedPanel.CriteriaColumn = TFindCriteriaColumn.fccRight;
                DisplayCriteriaFieldControls();

                // Raise FindCriteriaSelectionChangedArgs event and update IsFirstInColumn + IsLastInColumn
                FindCriteriaSelectionChangedArgs = new TPartnerFindCriteriaSelectionChangedEventArgs();
                FindCriteriaSelectionChangedArgs.SelectedCriteria = FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3);
                FindCriteriaSelectionChangedArgs.CriteriaColumn = FSelectedPanel.CriteriaColumn;
                FindCriteriaSelectionChangedArgs.IsFirstInColumn = false;
                FindCriteriaSelectionChangedArgs.IsLastInColumn = false;
                FSelectedPanel.IsFirstInColumn = false;
                FSelectedPanel.IsLastInColumn = false;

                if (FSelectedPanel.PositionInCriteriaColumn == 0)
                {
                    FindCriteriaSelectionChangedArgs.IsFirstInColumn = true;
                    FSelectedPanel.IsFirstInColumn = true;
                }

                if (FSelectedPanel.PositionInCriteriaColumn == FCriteriaFieldsRight.Count - 1)
                {
                    FindCriteriaSelectionChangedArgs.IsLastInColumn = true;
                    FSelectedPanel.IsLastInColumn = true;
                }

                // MessageBox.Show('Raising FindCriteriaSelectionChanged event.');
                OnFindCriteriaSelectionChanged(FindCriteriaSelectionChangedArgs);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void OnCriteriaChanging(System.Object sender, DataColumnChangeEventArgs args)
        {
            // MessageBox.Show(System.String.Format("Column_Changing Event: name=0; Column=1; proposed name=2",
            //                                      args.Row["name"], args.Column.ColumnName, args.ProposedValue) );

            if (args.ProposedValue.ToString() != FFindCriteriaDataTable.Rows[0][args.Column.ColumnName].ToString())
            {
                FFindCriteriaDataTable.Rows[0].EndEdit();

                //         MessageBox.Show("Value changed in " + FFindCriteriaDataTable.Columns[args.Column.ColumnName].ColumnName + ": " +Environment.NewLine +
                //                         "FFindCriteriaDataTable.Rows[0][args.Column.ColumnName]: " + FFindCriteriaDataTable.Rows[0][args.Column.ColumnName].ToString() + "; " +
                //                         "args.ProposedValue: " + args.ProposedValue.ToString());

                // Fire event
                if (OnCriteriaContentChanged != null)
                {
                    OnCriteriaContentChanged(this, EventArgs.Empty);
                }
            }
        }

        private void DisableAllPanel(Panel butNotThisOne)
        {
            foreach (Control TempPanel in pnlLeftColumn.Controls)
            {
                if (TempPanel.Equals(butNotThisOne) == false)
                {
                    TempPanel.Enabled = false;
                }
            }

            foreach (Control TempPanel in pnlRightColumn.Controls)
            {
                if (TempPanel.Equals(butNotThisOne) == false)
                {
                    TempPanel.Enabled = false;
                }
            }
        }

        private void EnableAllPanel()
        {
            foreach (Control TempPanel in pnlLeftColumn.Controls)
            {
                TempPanel.Enabled = true;
            }

            foreach (Control TempPanel in pnlRightColumn.Controls)
            {
                TempPanel.Enabled = true;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APassedLocationKey"></param>
        public void FocusLocationKey(Int32 APassedLocationKey)
        {
            // First make sure that the LocationKey Panel is there...
            if (!pnlRightColumn.Controls.Contains(pnlLocationKey))
            {
                pnlRightColumn.Controls.Add(pnlLocationKey);
            }

            // Set Focus on txtLocationKey
            txtLocationKey.Focus();

            // Set Text if APassedLocationKey is passed in
            if (APassedLocationKey != -1)
            {
                txtLocationKey.Text = APassedLocationKey.ToString();
                txtLocationKey.SelectAll();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AEnabled"></param>
        public void SetMatchButtonFunctionality(Boolean AEnabled)
        {
            foreach (Control TempPanel in pnlLeftColumn.Controls)
            {
                foreach (Control TempCtrl in TempPanel.Controls)
                {
                    if (TempCtrl.GetType() == typeof(SplitButton))
                    {
                        TempCtrl.Enabled = AEnabled;
                    }
                }
            }

            foreach (Control TempPanel in pnlRightColumn.Controls)
            {
                foreach (Control TempCtrl in TempPanel.Controls)
                {
                    if (TempCtrl.GetType() == typeof(SplitButton))
                    {
                        TempCtrl.Enabled = AEnabled;
                    }
                }
            }
        }
    }

    /// <summary>todoComment</summary>
    public delegate void FindCriteriaSelectionChangedHandler(System.Object Sender, TPartnerFindCriteriaSelectionChangedEventArgs e);

    /// <summary>
    /// todoComment
    /// </summary>
    public class TSelectedCriteriaPanel : System.Object
    {
        private System.Windows.Forms.Panel FSelectedCriteriaPanel;
        private TFindCriteriaColumn FCriteriaColumn;
        private Boolean FIsFirstInColumn;
        private Boolean FIsLastInColumn;
        private Int32 FPositionInCriteriaColumn;

        /// <summary>todoComment</summary>
        public System.Windows.Forms.Panel SelectedCriteriaPanel
        {
            get
            {
                return FSelectedCriteriaPanel;
            }

            set
            {
                FSelectedCriteriaPanel = value;
            }
        }

        /// <summary>todoComment</summary>
        public TFindCriteriaColumn CriteriaColumn
        {
            get
            {
                return FCriteriaColumn;
            }

            set
            {
                FCriteriaColumn = value;
            }
        }

        /// <summary>todoComment</summary>
        public Boolean IsFirstInColumn
        {
            get
            {
                return FIsFirstInColumn;
            }

            set
            {
                FIsFirstInColumn = value;
            }
        }

        /// <summary>todoComment</summary>
        public Boolean IsLastInColumn
        {
            get
            {
                return FIsLastInColumn;
            }

            set
            {
                FIsLastInColumn = value;
            }
        }

        /// <summary>todoComment</summary>
        public Int32 PositionInCriteriaColumn
        {
            get
            {
                return FPositionInCriteriaColumn;
            }

            set
            {
                FPositionInCriteriaColumn = value;
            }
        }


        #region TSelectedCriteriaPanel

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="SelectedCriteriaPanel"></param>
        /// <param name="CriteriaColumn"></param>
        /// <param name="IsFirstInColumn"></param>
        /// <param name="IsLastInColumn"></param>
        /// <param name="PositionInCriteriaColumn"></param>
        public TSelectedCriteriaPanel(System.Windows.Forms.Panel SelectedCriteriaPanel,
            TFindCriteriaColumn CriteriaColumn,
            Boolean IsFirstInColumn,
            Boolean IsLastInColumn,
            Int32 PositionInCriteriaColumn) :
            base()
        {
            FSelectedCriteriaPanel = SelectedCriteriaPanel;
            FCriteriaColumn = CriteriaColumn;
            FIsFirstInColumn = IsFirstInColumn;
            FIsLastInColumn = IsLastInColumn;
            FPositionInCriteriaColumn = PositionInCriteriaColumn;
        }

        #endregion
    }
}