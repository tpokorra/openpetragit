//
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
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core;


namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupCorporateExchangeRate
    {
        /// <summary>
        /// The base currency is used to initialize the "from" combobox
        /// </summary>
        private String baseCurrencyOfLedger;

        private bool blnSelectedRowChangeable = false;

        /// <summary>
        /// The definition of the ledger number is used to define some
        /// default values and it initializes the dialog to run in the non modal
        /// form ...
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                ALedgerRow ledger =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                         TCacheableFinanceTablesEnum.LedgerDetails, value))[0];
                baseCurrencyOfLedger = ledger.BaseCurrency;

                mniImport.Enabled = true;
                tbbImport.Enabled = true;
            }
        }

        private void RunOnceOnActivationManual()
        {
            this.txtDetailRateOfExchange.Validated +=
                new System.EventHandler(this.ValidatedExchangeRate);

            this.cmbDetailFromCurrencyCode.SelectedValueChanged +=
                new System.EventHandler(this.ValueChangedCurrencyCode);
            this.cmbDetailToCurrencyCode.SelectedValueChanged +=
                new System.EventHandler(this.ValueChangedCurrencyCode);

            this.tbbSave.Click +=
                new System.EventHandler(this.SetTheFocusToTheGrid);

            this.btnInvertExchangeRate.Click +=
                new System.EventHandler(this.InvertExchangeRate);

            FMainDS.ACorporateExchangeRate.DefaultView.Sort = ACorporateExchangeRateTable.GetDateEffectiveFromDBName() + " DESC, " +
                                                              ACorporateExchangeRateTable.GetTimeEffectiveFromDBName() + " DESC";
            FMainDS.ACorporateExchangeRate.DefaultView.RowFilter = "";
        }

        /// <summary>
        /// The focus is send to the grid to "unfocus" the input controls and to
        /// enforce that the dataset verification routines are invoked
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void SetTheFocusToTheGrid(object sender, EventArgs e)
        {
            grdDetails.Focus();
        }

        /// <summary>
        /// Create a new CorporateExchangeRateRow ...
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            DateTime NewDateEffectiveFrom;

            // Calculate the Date from which the Exchange Rate will be effective. It needs to be preset to the first day of the current month.
            NewDateEffectiveFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            string NewToCurrencyCode;

            ACorporateExchangeRateRow ACorporateExRateRow = FMainDS.ACorporateExchangeRate.NewRowTyped();
            ACorporateExRateRow.FromCurrencyCode = "USD";

            if (FPreviouslySelectedDetailRow == null)
            {
                if (baseCurrencyOfLedger != null)
                {
                    NewToCurrencyCode = baseCurrencyOfLedger; // Corporate Exchange rates are not part of any ledger, so this may not be set...
                }
                else
                {
                    NewToCurrencyCode = "USD";
                }

                ACorporateExRateRow.RateOfExchange = 1.0m;
            }
            else
            {
                NewToCurrencyCode = cmbDetailToCurrencyCode.GetSelectedString();
                ACorporateExRateRow.RateOfExchange = txtDetailRateOfExchange.NumberValueDecimal.Value;
            }

            ACorporateExRateRow.ToCurrencyCode = NewToCurrencyCode;

            // Ensure we don't create a duplicate record
            while (FMainDS.ACorporateExchangeRate.Rows.Find(new object[] {
                           ACorporateExRateRow.FromCurrencyCode, NewToCurrencyCode, NewDateEffectiveFrom.ToString()
                       }) != null)
            {
                NewDateEffectiveFrom = NewDateEffectiveFrom.AddMonths(1);
            }

            if (FPreviouslySelectedDetailRow == null)
            {
                cmbDetailFromCurrencyCode.SetSelectedString(ACorporateExRateRow.FromCurrencyCode);
                cmbDetailToCurrencyCode.SetSelectedString(ACorporateExRateRow.ToCurrencyCode);
            }

            ACorporateExRateRow.DateEffectiveFrom = NewDateEffectiveFrom;
/*
 // It seems weird to set the date as the first of this month, but the time as now?
            ACorporateExRateRow.TimeEffectiveFrom =
                (DateTime.Now.Hour * 60 + DateTime.Now.Minute) * 60 + DateTime.Now.Second;
 */
            ACorporateExRateRow.TimeEffectiveFrom = 0;

            FMainDS.ACorporateExchangeRate.Rows.Add(ACorporateExRateRow);
            grdDetails.Refresh();

            FPetraUtilsObject.SetChangedFlag();
            SelectDetailRowByDataTableIndex(FMainDS.ACorporateExchangeRate.Rows.Count - 1);

            UpdateExchangeRateLabels();
        }

        /// <summary>
        /// Validated Event for txtDetailRateOfExchange
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void ValidatedExchangeRate(System.Object sender, EventArgs e)
        {
            UpdateExchangeRateLabels();
        }

        /// <summary>
        /// Updates the lblValueOneDirection and lblValueOtherDirection labels
        /// </summary>
        private void UpdateExchangeRateLabels()
        {
            TSetupExchangeRates.SetExchangeRateLabels(cmbDetailFromCurrencyCode.GetSelectedString(),
                cmbDetailToCurrencyCode.GetSelectedString(), GetSelectedDetailRow(),
                txtDetailRateOfExchange.NumberValueDecimal.Value, lblValueOneDirection, lblValueOtherDirection);
        }

        /// <summary>
        /// ValueChanged Event for the currency boxes
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void ValueChangedCurrencyCode(System.Object sender, EventArgs e)
        {
            ValueChangedCurrencyCode();
        }

        /// <summary>
        /// Main routine for the ValueChanged Event of the currency boxes
        /// </summary>
        private void ValueChangedCurrencyCode()
        {
            if (cmbDetailFromCurrencyCode.GetSelectedString() ==
                cmbDetailToCurrencyCode.GetSelectedString())
            {
                txtDetailRateOfExchange.NumberValueDecimal = 1.0m;
                UpdateExchangeRateLabels();
                txtDetailRateOfExchange.Enabled = false;
                btnInvertExchangeRate.Enabled = false;
            }
            else
            {
                if (blnSelectedRowChangeable)
                {
                    txtDetailRateOfExchange.Enabled = true;
                    btnInvertExchangeRate.Enabled = true;
                }
            }

            if (blnSelectedRowChangeable)
            {
                cmbDetailToCurrencyCode.Enabled = true;
                cmbDetailFromCurrencyCode.Enabled = true;
            }

            if (txtDetailRateOfExchange.NumberValueDecimal.HasValue)
            {
                UpdateExchangeRateLabels();
            }
        }

        /// <summary>
        /// This routines supports a small gui-calculator. The user can easily calculate the
        /// reciproke value of the exchange rate.
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void InvertExchangeRate(System.Object sender, EventArgs e)
        {
            decimal? exchangeRate;

            try
            {
                exchangeRate = txtDetailRateOfExchange.NumberValueDecimal;
                exchangeRate = 1 / exchangeRate;
                exchangeRate = Math.Round(exchangeRate.Value, 10);
                txtDetailRateOfExchange.NumberValueDecimal = exchangeRate;
            }
            catch (Exception)
            {
            }

            UpdateExchangeRateLabels();
        }

        /// <summary>
        /// Standardroutine
        /// </summary>
        /// <param name="ARow"></param>
        private void ShowDetailsManual(ACorporateExchangeRateRow ARow)
        {
            if (ARow != null)
            {
                blnSelectedRowChangeable = !(ARow.RowState == DataRowState.Unchanged);
                UpdateExchangeRateLabels();
                txtDetailRateOfExchange.Enabled = true;
                btnInvertExchangeRate.Enabled = (ARow.RowState == DataRowState.Added);
                blnSelectedRowChangeable = (ARow.RowState == DataRowState.Added);
                ValueChangedCurrencyCode();
            }
            else
            {
                blnSelectedRowChangeable = false;
                txtDetailRateOfExchange.Enabled = false;
                txtDetailRateOfExchange.NumberValueDecimal = null;
            }
        }

        private void Import(System.Object sender, EventArgs e)
        {
            TImportExchangeRates.ImportCurrencyExRates(FMainDS.ACorporateExchangeRate, "Corporate");
            FPetraUtilsObject.SetChangedFlag();
        }
    }
}