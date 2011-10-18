//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash
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
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Printing;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TFrmPersonnelStaffData
    {
        private void InitializeManualCode()
        {
            pnlDetails.Enabled = false;
            cmbDetailStatusCode.ComboBoxWidth = 150;
        }

        /// <summary>The new button was pressed;create a new row</summary>
        public void NewRow(System.Object sender, EventArgs e)
        {
            CreateNewPmStaffData();
            UpdatePartnerKey();
        }

        private void UpdatePartnerKey()
        {
            txtDetailPartnerKey.Text = Convert.ToString(FPartnerKey);
        }

        private Int64 GetSiteKey()
        {
            return Convert.ToInt64(TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_SITEKEY, ""));
        }

        private void NewRowManual(ref PmStaffDataRow ANewRow)
        {
            ANewRow.SiteKey = GetSiteKey();
            //search the latest used (max) id to build the new key, ignore sitekey
            long max = 0;

            foreach (PmStaffDataRow am in FMainDS.PmStaffData.Rows)
            {
                if ((am.RowState != DataRowState.Deleted) && (am.Key > max))
                {
                    max = am.Key;
                }
            }

            ANewRow.Key = max + 1;
            ANewRow.PartnerKey = FPartnerKey;
            ANewRow.ReceivingField = 0;
            ANewRow.SetReceivingFieldOfficeNull();
            ANewRow.OfficeRecruitedBy = 0;
            ANewRow.HomeOffice = 0;
        }

        private Int64 FPartnerKey;

        /// <summary>Partnerkey is the part of the "real" primary key for this table</summary>
        public long PartnerKey {
            get
            {
                return FPartnerKey;
            }
            set
            {
                FPartnerKey = value;
                UpdatePartnerKey();
                LoadPersonellStaffData();
            }
        }
        private void LoadPersonellStaffData()
        {
            FMainDS = TRemote.MPersonnel.WebConnectors.LoadPersonellStaffData(FPartnerKey);
            cmbDetailStatusCode.InitialiseUserControl(FMainDS.PmCommitmentStatus,
                PmCommitmentStatusTable.GetCodeDBName(), PmCommitmentStatusTable.GetDescDBName(), null);

            if (FMainDS != null)
            {
                DataView myDataView = FMainDS.PmStaffData.DefaultView;
                myDataView.AllowNew = false;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
                PPartnerRow partnerRow = FMainDS.PPartner[0];
                txtName.Text = partnerRow.PartnerShortName;
                txtPartnerStatusCode.Text = partnerRow.StatusCode;
                txtLanguageCode.Text = partnerRow.LanguageCode;
            }
        }

        private void DeleteRow(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                || (MessageBox.Show(String.Format(Catalog.GetString(
                                "You have choosen to delete this entry with start of commitment date ({0:d}).\n\nDo you really want to delete it?"),
                            FPreviouslySelectedDetailRow.StartOfCommitment), Catalog.GetString("Confirm Delete"),
                        MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                int rowIndex = CurrentRowIndex();
                FPreviouslySelectedDetailRow.Delete();
                FPetraUtilsObject.SetChangedFlag();
                SelectByIndex(rowIndex);
            }
        }

        private TSubmitChangesResult StoreManualCode(ref PersonnelTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            return TRemote.MPersonnel.WebConnectors.SavePersonnelTDS(ref ASubmitChanges, out AVerificationResult);
        }

        private void ShowDetailsManual(PmStaffDataRow ARow)
        {
            if (ARow == null)
            {
                pnlDetails.Enabled = false;
            }
            else
            {
                pnlDetails.Enabled = true;
            }
        }

        private int CurrentRowIndex()
        {
            int rowIndex = -1;

            SourceGrid.RangeRegion selectedRegion = grdDetails.Selection.GetSelectionRegion();

            if ((selectedRegion != null) && (selectedRegion.GetRowsIndex().Length > 0))
            {
                rowIndex = selectedRegion.GetRowsIndex()[0];
            }

            return rowIndex;
        }

        /// <summary>
        /// validate the data entered, so that the caller can cancel the current operation if data is missing
        /// </summary>
        /// <param name="ARow"></param>
        /// <param name="AVerifications"></param>
        /// <returns>true if everything is fine</returns>
        private bool ValidateDetailsManual(PmStaffDataRow ARow, out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();

            try
            {
                Int64 ReceivingField = Convert.ToInt64(txtDetailReceivingField.Text);

                if (ReceivingField == 0)
                {
                    throw new Exception("invalid office 0");
                }
            }
            catch (Exception)
            {
                AVerifications.Add(new TVerificationResult(Catalog.GetString("Receiving Field"),
                        Catalog.GetString("You need to select the office that the person will join"),
                        TResultSeverity.Resv_Critical));
            }

            try
            {
                if (!dtpDetailStartOfCommitment.Date.HasValue)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                AVerifications.Add(new TVerificationResult(Catalog.GetString("Start of Commitment"),
                        Catalog.GetString("Please enter a valid start date"),
                        TResultSeverity.Resv_Critical));
            }

            try
            {
                if (!dtpDetailEndOfCommitment.Date.HasValue)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                AVerifications.Add(new TVerificationResult(Catalog.GetString("End of Commitment"),
                        Catalog.GetString("Please enter a valid end date"),
                        TResultSeverity.Resv_Critical));
            }

            try
            {
                Int64 HomeOffice = Convert.ToInt64(txtDetailHomeOffice.Text);

                if (HomeOffice == 0)
                {
                    throw new Exception("invalid office 0");
                }
            }
            catch (Exception)
            {
                AVerifications.Add(new TVerificationResult(Catalog.GetString("Home office"),
                        Catalog.GetString("You need to select the office that is sending the person"),
                        TResultSeverity.Resv_Critical));
            }

            try
            {
                Int64 OfficeRecruitedBy = Convert.ToInt64(txtDetailOfficeRecruitedBy.Text);

                if (OfficeRecruitedBy == 0)
                {
                    throw new Exception("invalid office 0");
                }
            }
            catch (Exception)
            {
                AVerifications.Add(new TVerificationResult(Catalog.GetString("Recruiting Office"),
                        Catalog.GetString("You need to select the office that has recruited the person"),
                        TResultSeverity.Resv_Critical));
            }


            return !AVerifications.HasCriticalError();
        }

        private void GetDetailDataFromControlsManual(PmStaffDataRow ARow)
        {
            //TODO THis is a workaround, where is the input of ReceivingFieldOffice?
            ARow.ReceivingFieldOffice = Convert.ToInt64(txtDetailReceivingField.Text);
        }

        private void SelectByIndex(int rowIndex)
        {
            if (rowIndex >= grdDetails.Rows.Count)
            {
                rowIndex = grdDetails.Rows.Count - 1;
            }

            if ((rowIndex < 1) && (grdDetails.Rows.Count > 1))
            {
                rowIndex = 1;
            }

            if ((rowIndex >= 1) && (grdDetails.Rows.Count > 1))
            {
                grdDetails.Selection.SelectRow(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);
            }
            else
            {
                FPreviouslySelectedDetailRow = null;
            }
        }
    }
}