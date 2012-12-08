//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Data;
using GNU.Gettext;

namespace Ict.Common.IO
{
    /// <summary>
    /// Description of SelectCSVSeparator.
    /// </summary>
    public partial class TDlgSelectCSVSeparator : Form
    {
        /// <summary>american number format: decimal point, comma for thousands separator</summary>
        public static string NUMBERFORMAT_AMERICAN = "American";
        /// <summary>european number format: decimal comma, point for thousands separator</summary>
        public static string NUMBERFORMAT_EUROPEAN = "European";

        private string FSeparator;
        private bool FFileHasCaption;
        private List <String>FCSVRows = null;

        /// <summary>
        /// read the separator that the user has selected
        /// </summary>
        public string SelectedSeparator
        {
            get
            {
                return FSeparator;
            }
            set
            {
                FSeparator = value;
                UpdateRadioButtons();
            }
        }

        /// <summary>
        /// read/set the date format that the user has selected
        /// </summary>
        public string DateFormat
        {
            get
            {
                return cmbDateFormat.SelectedItem.ToString();
            }
            set
            {
                //Conversion of some old Petra Values
                if (value.Equals("MDY"))
                {
                    value = "MM/dd/yyyy";
                }
                else
                {
                    if (value.Equals("DMY"))
                    {
                        value = "dd/MM/yyyy";
                    }
                }

                if (!cmbDateFormat.Items.Contains(value))
                {
                    cmbDateFormat.Items.Add(value);
                }

                cmbDateFormat.SelectedItem = value;
            }
        }

        /// <summary>
        /// returns a string constant for the selected number format
        /// </summary>
        public String NumberFormat
        {
            set
            {
                cmbNumberFormat.SelectedIndex = (value == NUMBERFORMAT_AMERICAN ? 0 : 1);
            }
            get
            {
                return cmbNumberFormat.SelectedIndex == 0 ? NUMBERFORMAT_AMERICAN : NUMBERFORMAT_EUROPEAN;
            }
        }

        /// <summary>
        /// constructor
        /// TODO: select if first row contains captions? or use a parameter to avoid or request captions?
        /// </summary>
        public TDlgSelectCSVSeparator(bool AFileHasCaption)
        {
            FFileHasCaption = AFileHasCaption;

            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.rbtComma.Text = Catalog.GetString("Comma");
            this.rbtTabulator.Text = Catalog.GetString("Tabulator");
            this.rbtOther.Text = Catalog.GetString("Other Separator") + ":";
            this.rbtSemicolon.Text = Catalog.GetString("Semicolon");
            this.btnCancel.Text = Catalog.GetString("Cancel");
            this.btnOK.Text = Catalog.GetString("OK");
            this.lblDateFormat.Text = Catalog.GetString("Date format") + ":";
            this.lblNumberFormat.Text = Catalog.GetString("Number format") + ":";
            this.Text = Catalog.GetString("Select CSV Separator");
            #endregion

            FSeparator = TAppSettingsManager.GetValue("CSVSeparator",
                System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
            System.Globalization.CultureInfo myCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            String regionalDateString = myCulture.DateTimeFormat.ShortDatePattern;

            if (!cmbDateFormat.Items.Contains(regionalDateString))
            {
                cmbDateFormat.Items.Insert(0, regionalDateString);
            }

            cmbDateFormat.SelectedIndex = cmbDateFormat.Items.IndexOf(regionalDateString);

            cmbNumberFormat.SelectedIndex = cmbNumberFormat.Items.IndexOf(myCulture.NumberFormat.NumberDecimalSeparator);

            UpdateRadioButtons();
        }

        private void UpdateRadioButtons()
        {
            if (FSeparator == ";")
            {
                rbtSemicolon.Checked = true;
            }
            else if (FSeparator == ",")
            {
                rbtComma.Checked = true;
            }
            else if (FSeparator == "\t")
            {
                rbtTabulator.Checked = true;
            }
            else
            {
                rbtOther.Checked = true;
                txtOtherSeparator.Text = FSeparator;
            }
        }

        /// <summary>
        /// set the filename for CSV
        /// </summary>
        public string CSVFileName
        {
            set
            {
                StreamReader reader = new StreamReader(value, TTextFile.GetFileEncoding(value), false);

                FCSVRows = new List <string>();

                while (!reader.EndOfStream && FCSVRows.Count < 6)
                {
                    FCSVRows.Add(reader.ReadLine());
                }

                reader.Close();
                RbtCheckedChanged(null, null);
            }
        }

        void RbtCheckedChanged(object sender, EventArgs e)
        {
            txtOtherSeparator.Enabled = rbtOther.Checked;

            if (rbtComma.Checked)
            {
                FSeparator = ",";
            }
            else if (rbtSemicolon.Checked)
            {
                FSeparator = ";";
            }
            else if (rbtTabulator.Checked)
            {
                FSeparator = "\t";
            }
            else if (rbtOther.Checked)
            {
                FSeparator = txtOtherSeparator.Text;
            }

            if ((FSeparator.Length > 0) && (FCSVRows != null))
            {
                DataTable table = new DataTable();
                string line = FCSVRows[0];
                int counter = 0;

                if (FFileHasCaption)
                {
                    while (line.Length > 0)
                    {
                        string header = StringHelper.GetNextCSV(ref line, FSeparator);

                        if (header.StartsWith("#"))
                        {
                            header = header.Substring(1);
                        }

                        table.Columns.Add(header);
                    }

                    counter++;
                }

                char columnCounter = (char)('A' + table.Columns.Count);

                for (; counter < FCSVRows.Count; counter++)
                {
                    line = FCSVRows[counter];
                    DataRow row = table.NewRow();
                    int countColumns = 0;

                    while (line.Length > 0)
                    {
                        if (countColumns + 1 > table.Columns.Count)
                        {
                            table.Columns.Add(columnCounter.ToString());
                            columnCounter++;
                        }

                        row[countColumns] = StringHelper.GetNextCSV(ref line, FSeparator);
                        countColumns++;
                    }

                    table.Rows.Add(row);
                }

                table.DefaultView.AllowNew = false;
                table.DefaultView.AllowDelete = false;
                table.DefaultView.AllowEdit = false;
                grdPreview.DataSource = table;
            }
        }
    }
}