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
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevAge.Drawing;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Controllers;
using SourceGrid.Cells.Models;
using SourceGrid.Cells.Views;

namespace Ict.Common.Controls
{
    /// <summary>Delegate forward declaration</summary>
    public delegate System.Drawing.Image DelegateGetImageForRow(int ARow);

    #region TextColumn

    /// <summary>
    /// This is a custom DataGridColumn for Text, for use with TSgrdDataGrid.
    /// </summary>
    public class TSgrdTextColumn : SourceGrid.DataGridColumn
    {
        /// <summary>
        /// the grid that this column belongs to
        /// </summary>
        protected SourceGrid.DataGrid FGrid;

        /// <summary>
        /// the currently selected cell
        /// </summary>
        protected SourceGrid.Cells.ICellVirtual FDataCellSelected;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ADataGrid"></param>
        /// <param name="ADataColumn"></param>
        /// <param name="ACaption"></param>
        /// <param name="ADataCell"></param>
        public TSgrdTextColumn(SourceGrid.DataGrid ADataGrid,
            System.Data.DataColumn ADataColumn,
            string ACaption,
            SourceGrid.Cells.ICellVirtual ADataCell) :
            this(ADataGrid, ADataColumn, ACaption, ADataCell, -1, true)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ADataGrid"></param>
        /// <param name="ADataColumn"></param>
        /// <param name="ACaption"></param>
        /// <param name="ADataCell"></param>
        /// <param name="ASortableHeader"></param>
        public TSgrdTextColumn(SourceGrid.DataGrid ADataGrid,
            System.Data.DataColumn ADataColumn,
            string ACaption,
            SourceGrid.Cells.ICellVirtual ADataCell,
            Boolean ASortableHeader) :
            this(ADataGrid, ADataColumn, ACaption, ADataCell, -1, ASortableHeader)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ADataGrid"></param>
        /// <param name="ADataColumn"></param>
        /// <param name="ACaption"></param>
        /// <param name="ADataCell"></param>
        /// <param name="AColumnWidth"></param>
        /// <param name="ASortableHeader"></param>
        public TSgrdTextColumn(SourceGrid.DataGrid ADataGrid,
            System.Data.DataColumn ADataColumn,
            string ACaption,
            SourceGrid.Cells.ICellVirtual ADataCell,
            Int16 AColumnWidth,
            Boolean ASortableHeader) :
            base(ADataGrid)
        {
            HeaderCell = new SourceGrid.Cells.ColumnHeader(ACaption);

            if (!ASortableHeader)
            {
                ((SourceGrid.Cells.ColumnHeader)HeaderCell).AutomaticSortEnabled = false;
            }

            HeaderCell.View = ((TSgrdDataGrid)ADataGrid).ColumnHeaderView;

            if (ADataColumn != null)
            {
                PropertyName = ADataColumn.ColumnName;
            }

            DataCell = ADataCell;

            if (AColumnWidth != -1)
            {
                Width = AColumnWidth;
                AutoSizeMode = SourceGrid.AutoSizeMode.MinimumSize;
            }

            FGrid = ADataGrid;
        }

        /// <summary>
        /// get the data cell of a row
        /// </summary>
        /// <param name="AGridRow"></param>
        /// <returns></returns>
        public override SourceGrid.Cells.ICellVirtual GetDataCell(int AGridRow)
        {
            SourceGrid.Cells.ICellVirtual ReturnValue;
            SourceGrid.Cells.ICellVirtual BaseDataCell = base.GetDataCell(AGridRow);
            SourceGrid.Cells.ICellVirtual AlternatingDataCellSelected;
            int Reminder;

            HeaderCell.View = ((TSgrdDataGrid)FGrid).ColumnHeaderView;
            FDataCellSelected = BaseDataCell.Copy();

            // Create a ToolTip
            FDataCellSelected.AddController(SourceGrid.Cells.Controllers.ToolTipText.Default);
            FDataCellSelected.Model.AddModel(TToolTipModel.myDefault);

            // Alternating BackColor (banding effect)
            Math.DivRem(AGridRow, 2, out Reminder);

            if (Reminder == 0)
            {
                ReturnValue = FDataCellSelected;
            }
            else
            {
                if (((TSgrdDataGrid)FGrid).AlternatingBackgroundColour != Color.Empty)
                {
                    AlternatingDataCellSelected = FDataCellSelected.Copy();
                    AlternatingDataCellSelected.View = (SourceGrid.Cells.Views.IView)FDataCellSelected.View.Clone();
                    AlternatingDataCellSelected.View.BackColor = ((TSgrdDataGrid)FGrid).AlternatingBackgroundColour;
                    ReturnValue = AlternatingDataCellSelected;
                }
                else
                {
                    ReturnValue = FDataCellSelected;
                }
            }

            return ReturnValue;
        }
    }
    #endregion

    #region ImageColumn

    /// <summary>
    /// This is a custom DataGridColumn for Images, for use with TSgrdDataGrid.
    /// </summary>
    public class TSgrdImageColumn : TSgrdTextColumn
    {
        private const Int32 IMAGECOLUMN_HORIZONTAL_PADDING = 4;
        private DelegateGetImageForRow FGetImage;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ADataGrid"></param>
        /// <param name="ACaption"></param>
        /// <param name="AGetImage"></param>
        public TSgrdImageColumn(SourceGrid.DataGrid ADataGrid, string ACaption, DelegateGetImageForRow AGetImage)
            : base(ADataGrid, null, ACaption, null, -1, false)
        {
            HeaderCell = new SourceGrid.Cells.ColumnHeader(ACaption);
            ((SourceGrid.Cells.ColumnHeader)HeaderCell).AutomaticSortEnabled = false;
            HeaderCell.View = ((TSgrdDataGrid)FGrid).ColumnHeaderView;
            PropertyName = null;
            DataCell = null;
            FGrid = ADataGrid;
            this.AutoSizeMode = SourceGrid.AutoSizeMode.MinimumSize;
            FGetImage = AGetImage;
        }

        /// <summary>
        /// get the data cell of a row
        /// </summary>
        /// <param name="AGridRow"></param>
        /// <returns></returns>
        public override SourceGrid.Cells.ICellVirtual GetDataCell(int AGridRow)
        {
            int Reminder;

            System.Drawing.Image TheImage;
            FDataCellSelected = new SourceGrid.Cells.Virtual.CellVirtual();

            // Create Icon
            TheImage = FGetImage(AGridRow - 1);
            FDataCellSelected.Model.AddModel(new SourceGrid.Cells.Models.Image(TheImage));
            ((ViewBase)FDataCellSelected.View).ImageAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            // Automatic calculation of column width
            Width = TheImage.Width + IMAGECOLUMN_HORIZONTAL_PADDING;

            // Create a ToolTip
            FDataCellSelected.AddController(SourceGrid.Cells.Controllers.ToolTipText.Default);
            FDataCellSelected.Model.AddModel(TToolTipModel.myDefault);

            // Alternating BackColor (banding effect)
            Math.DivRem(AGridRow, 2, out Reminder);

            if (Reminder != 0)
            {
                if (((TSgrdDataGrid)FGrid).AlternatingBackgroundColour != Color.Empty)
                {
                    FDataCellSelected.View = (SourceGrid.Cells.Views.IView)FDataCellSelected.View.Clone();
                    FDataCellSelected.View.BackColor = ((TSgrdDataGrid)FGrid).AlternatingBackgroundColour;
                }
            }

            return FDataCellSelected;
        }
    }
    #endregion

    #region TToolTipModel

    /// <summary>
    /// tooltips for grid
    /// </summary>
    public class TToolTipModel : System.Object, SourceGrid.Cells.Models.IToolTipText
    {
        /// default tooltip
        public static TToolTipModel myDefault;

        /// <summary>
        /// this needs to be called once for the whole application
        /// </summary>
        public static void InitializeUnit()
        {
            // Initialisation of Unit needs to be done once.
            if (TToolTipModel.myDefault == null)
            {
                TToolTipModel.myDefault = new TToolTipModel();
            }
        }

        /// <summary>
        /// get the correct tooltip for the current cell
        /// </summary>
        /// <param name="ACellContext"></param>
        /// <returns>tooltip text</returns>
        public string GetToolTipText(SourceGrid.CellContext ACellContext)
        {
            string ReturnValue = "";
            DataRowView TheRow;
            TSgrdDataGrid TheGrid;

            TheGrid = (TSgrdDataGrid)(SourceGrid.DataGrid)ACellContext.Grid;
            TheRow = (DataRowView)TheGrid.Rows.IndexToDataSourceRow(ACellContext.Position.Row);

            // MessageBox.Show('TToolTipModel.GetToolTipText.  Row: ' + Convert.ToString(ACellContext.Position.Row  1) );
            if (TheRow != null)
            {
                try
                {
                    // Create a ToolTip
                    if (TheGrid.ToolTipTextDelegate != null)
                    {
                        // MessageBox.Show('TToolTipModel.GetToolTipText.  Inquiring ToolTip Text...');
                        ReturnValue = TheGrid.ToolTipTextDelegate((short)ACellContext.Position.Column, (short)(ACellContext.Position.Row - 1));

//                      MessageBox.Show("TToolTipModel.GetToolTipText.  ToolTip Text: " + ReturnValue);
                        return ReturnValue;
                    }
                }
                catch (Exception E)
                {
                    MessageBox.Show("Exception in TToolTipModel.GetToolTipText: " + E.ToString());
                }
            }
            else
            {
                ReturnValue = "";
            }

            return ReturnValue;
        }
    }
    #endregion
}