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
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevAge.ComponentModel;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Controllers;
using SourceGrid.Cells.DataGrid;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Models;
using SourceGrid.Cells.Views;
using SourceGrid.Cells.Virtual;
using SourceGrid.Selection;
using DevAge.ComponentModel.Converter;
using DevAge.ComponentModel.Validator;
using DevAge.Windows.Forms;
using DevAge.Drawing;
using System.Globalization;

namespace Ict.Common.Controls
{
    #region TSgrdDataGrid

    /// <summary>
    /// TSgrdDataGrid is an extension of SourceGrid.DataGrid that contains several
    /// customisations and helper functions, especially for viewing DataTable data in
    /// a List-like manner.
    ///
    /// </summary>
    public class TSgrdDataGrid : SourceGrid.DataGrid
    {
        private const Int32 WM_KEYDOWN = 0x100;

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// View for the ColumnHeaders of this Grid
        ///
        /// </summary>
        private SourceGrid.Cells.Views.ColumnHeader FColumnHeaderView;

        /// <summary>
        /// Determines whether the column headers should appear 'greyed out' if the Grid
        /// is disabled.
        ///
        /// </summary>
        private Boolean FShowColumnHeadersDisabled;

        /// <summary>
        /// Used by to colour the background of every odd numbered row differently to
        /// generate a 'banding' effect (works only with columns defined in
        /// sgrdDataGrid.Columns!).
        ///
        /// </summary>
        private Color FAlternateBackColor;

        /// <summary>
        /// If set to an appropriate delegate function, this provides ToolTips on each
        /// Cell of the Grid (works only with columns defined in sgrdDataGrid.Columns!).
        ///
        /// </summary>
        private TDelegateGetToolTipText FToolTipTextDelegate;

        /// <summary>
        /// Determines whether the column headers should support sorting by clicking on
        /// them (works only with columns defined in sgrdDataGrid.Columns!).
        ///
        /// </summary>
        private Boolean FSortableHeaders;

        /// <summary>
        /// Keeps track of the the selected rows before sorting in order to be able
        /// to select them again after sorting the Grid.
        ///
        /// </summary>
        private DataRowView[] FRowsSelectedBeforeSort;

        /// <summary>
        /// Maintains the state of whether the currently selected row should stay
        /// selected after sorting the Grid.
        ///
        /// </summary>
        private Boolean FKeepRowSelectedAfterSort;

        /// <summary>
        /// Used by the PerformAutoFindFirstCharacter procedure.
        ///
        /// </summary>
        private Keys FLastKeyCode;

        /// <summary>
        /// Used by the PerformAutoFindFirstCharacter procedure.
        ///
        /// </summary>
        private TAutoFindModeEnum FAutoFindMode;

        /// <summary>
        /// Used by the PerformAutoFindFirstCharacter procedure.
        ///
        /// </summary>
        private Int16 FAutoFindColumn;

        /// <summary>
        /// Used by the PerformAutoFindFirstCharacter procedure.
        ///
        /// </summary>
        private DataView FAutoFindMatchingDataView;

        /// <summary>
        /// Maintains a state for the PerformAutoFindFirstCharacter procedure.
        ///
        /// </summary>
        private Boolean FAutoFindListRebuildNeeded;

        /// <summary>
        /// Read access to the View for the ColumnHeaders of this Grid (used by
        /// sgrdDataGrid.Columns).
        ///
        /// </summary>
        public SourceGrid.Cells.Views.ColumnHeader ColumnHeaderView
        {
            get
            {
                return FColumnHeaderView;
            }
        }

        /// <summary>
        /// Set this to an appropriate delegate function to provide ToolTips for each
        /// Cell of the Grid.
        ///
        /// </summary>
        public TDelegateGetToolTipText ToolTipTextDelegate
        {
            get
            {
                return FToolTipTextDelegate;
            }

            set
            {
                FToolTipTextDelegate = value;
            }
        }

        /**
         * This property determines whether AutoStretchColumnsToFitWidth should be used.
         *
         */
        [Category("Misc"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(true),
         Browsable(true)]
        public new Boolean AutoStretchColumnsToFitWidth
        {
            get
            {
                return base.AutoStretchColumnsToFitWidth;
            }

            set
            {
                base.AutoStretchColumnsToFitWidth = value;
            }
        }

        /**
         * This property determines which MinimumHeight should be used.
         *
         */
        [Category("Misc"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(19),
         Browsable(true)]
        public new Int32 MinimumHeight
        {
            get
            {
                return base.MinimumHeight;
            }

            set
            {
                base.MinimumHeight = value;
            }
        }

        /// <summary>
        /// This property determines which SpecialKeys should be used.
        /// </summary>
        public new GridSpecialKeys SpecialKeys
        {
            get
            {
                return base.SpecialKeys;
            }

            set
            {
                base.SpecialKeys = value;
            }
        }

        /// <summary>
        /// Helper function for SourceGrid 4 that enables us to hook up our ListChangedEventHandler
        /// </summary>
        public new DevAge.ComponentModel.IBoundList DataSource
        {
            get
            {
                return base.DataSource;
            }

            set
            {
                bool HookupListChangedEvent = false;

                //              if (((DevAge.ComponentModel.BoundDataView) base.DataSource).mDataView.ListChanged != null)  // the compiler doesn't get over this code, so I used the next line to get approximately what I want...
                if (base.DataSource == null)
                {
                    HookupListChangedEvent = true;
                }

                base.DataSource = value;

                if (value != null)
                {
                    if (HookupListChangedEvent)
                    {
                        ((DevAge.ComponentModel.BoundDataView) base.DataSource).ListChanged += new ListChangedEventHandler(this.OnDataViewChanged);
                    }
                }
            }
        }

        /// <summary>
        /// Helper function for SourceGrid 4 that returns the SelectedDataRows as a DataRowView
        /// (as SourceGrid 3 did).
        /// </summary>
        public DataRowView[] SelectedDataRowsAsDataRowView
        {
            get
            {
                if (this.SelectedDataRows != null)
                {
                    DataRowView[] ReturnValue = new DataRowView[this.SelectedDataRows.Length];

                    for (int Counter = 0; Counter < this.SelectedDataRows.Length; Counter++)
                    {
                        ReturnValue[Counter] = (DataRowView) this.SelectedDataRows[Counter];
                    }

                    return ReturnValue;
                }
                else
                {
                    return null;
                }
            }
        }

        /** / Custom properties follow
         * This property determines which AlternatingBackgroundColour should be used.
         *
         */
        [Category("Appearance"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Browsable(true),
         Description("The colour that is used to set the backgroud colour of every second line.")]
        public Color AlternatingBackgroundColour
        {
            get
            {
                return FAlternateBackColor;
            }

            set
            {
                FAlternateBackColor = value;
                this.Invalidate();
            }
        }

        /**
         * This property determines whether the column headers should support sorting
         * by clicking on them.
         *
         */
        [Category("Sorting"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(true),
         Browsable(true),
         Description("Determines whether the column headers should support sorting by clicking on them.")]
        public Boolean SortableHeaders
        {
            get
            {
                return FSortableHeaders;
            }

            set
            {
                FSortableHeaders = value;
            }
        }

        /**
         * This property determines whether the currently selected row should stay
         * selected after sorting the Grid.
         *
         */
        [Category("Sorting"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(true),
         Browsable(true),
         Description("Determines whether the currently selected row should stay selected after sorting the Grid.")]
        public Boolean KeepRowSelectedAfterSort
        {
            get
            {
                return FKeepRowSelectedAfterSort;
            }

            set
            {
                FKeepRowSelectedAfterSort = value;
            }
        }

        /**
         * This property determines which AutoFindMode should be used.
         *
         */
        [Category("AutoFind"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(TAutoFindModeEnum.NoAutoFind),
         Browsable(true),
         Description("Determines which AutoFindMode should be used.")]
        public TAutoFindModeEnum AutoFindMode
        {
            get
            {
                return FAutoFindMode;
            }

            set
            {
                if (value == TAutoFindModeEnum.FullString)
                {
                    if (DesignMode)
                    {
                        throw new TDataGridAutoFindModeNotImplementedYetException(
                            "Sorry, AutoFindMode 'FullString' is not implemented yet! You could implement it, though, if you really need it!");
                    }
                }

                FAutoFindMode = value;
            }
        }

        /**
         * This property determines which Column of the DataGrid should be
         * enabled for AutoFind (Note: This is not the DataColumn of the DataView!).
         *
         */
        [Category("AutoFind"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(-1),
         Browsable(true),
         Description("Determines which Column of the DataGrid should be enabled for AutoFind (Note: This is not the DataColumn of the DataView!).")]
        public Int16 AutoFindColumn
        {
            get
            {
                return FAutoFindColumn;
            }

            set
            {
                FAutoFindColumn = value;
                FAutoFindListRebuildNeeded = true;
            }
        }

        // Custom Events follow
        //      /**
        //      This Event is thrown when a Cell of the Grid is DoubleClicked with the mouse.
        //      */
        //      [Category("Action"),
        //       Browsable(true),
        //       RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
        //       Description ("Occurs when when a Cell of the Grid is Clicked with the mouse.")]
        //       property ClickCell: TClickCellEventHandler add FClickCell remove FClickCell;
//
        //       [Category("Action"),
        //       Browsable(true),
        //       RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
        //       Description ("Occurs when when a HeaderCell of the Grid is Clicked with the mouse.")]
        //       property ClickHeaderCell: TClickHeaderCellEventHandler add FClickHeaderCell remove FClickHeaderCell;

        /**
         * This Event is thrown when a Cell of the Grid is DoubleClicked with the mouse.
         *
         */
        [Category("Action"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when when a Cell of the Grid is DoubleClicked with the mouse.")]
        public event TDoubleClickCellEventHandler DoubleClickCell;

        /// <summary>
        /// Occurs when when a HeaderCell of the Grid is DoubleClicked with the mouse.
        /// </summary>
        [Category("Action"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when when a HeaderCell of the Grid is DoubleClicked with the mouse.")]
        public event TDoubleClickHeaderCellEventHandler DoubleClickHeaderCell;

        /**
         * This Event is thrown when the Insert key is pressed on the Grid.
         *
         */
        [Category("Custom Key Handling"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when the Insert key is pressed on the Grid.")]
        public event TKeyPressedEventHandler InsertKeyPressed;

        /**
         * This Event is thrown when the Delete key is pressed on the Grid.
         *
         */
        [Category("Custom Key Handling"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when the Delete key is pressed on the Grid.")]
        public event TKeyPressedEventHandler DeleteKeyPressed;

        /**
         * This Event is thrown when the Enter key is pressed on the Grid.
         *
         */
        [Category("Custom Key Handling"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when the Enter key is pressed on the Grid.")]
        public event TKeyPressedEventHandler EnterKeyPressed;

        /**
         * This Event is thrown when the Space key is pressed on the Grid.
         *
         */
        [Category("Custom Key Handling"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when the Space key is pressed on the Grid.")]
        public event TKeyPressedEventHandler SpaceKeyPressed;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            //
            // TSgrdDataGrid
            //
            this.Name = "TSgrdDataGrid";
        }

        #endregion

        /// <summary>
        /// TSgrdDataGrid is an extension of SourceGrid.DataGrid that contains several
        /// customisations and helper functions, especially for viewing DataTable data in
        /// a List-like manner.
        /// </summary>
        public TSgrdDataGrid() : base()
        {
            this.Set_DefaultProperties();
            InitializeComponent();

            FColumnHeaderView = new SourceGrid.Cells.Views.ColumnHeader();

            // Hook up our custom DoubleClick Handler
            this.Controller.AddController(new DoubleClickController());
            SpecialKeys = SourceGrid.GridSpecialKeys.Default ^ SourceGrid.GridSpecialKeys.Tab;

            TToolTipModel.InitializeUnit();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        /// <param name="Disposing"></param>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
        }

        /// <summary>
        /// This procedure sets the default properties.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void Set_DefaultProperties()
        {
            // Default size
            this.Height = 100;
            this.Width = 400;

            // Default look
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.AlternatingBackgroundColour = System.Drawing.Color.FromArgb(230, 230, 230);
            this.AutoStretchColumnsToFitWidth = true;
            this.MinimumHeight = 19;
            ((SelectionBase) this.Selection).Border = new DevAge.Drawing.RectangleBorder();
            ((SelectionBase) this.Selection).BackColor = Color.FromArgb(150, Color.FromKnownColor(KnownColor.Highlight));
            ((SelectionBase) this.Selection).FocusBackColor = ((SelectionBase) this.Selection).BackColor;

            // Default behaviour
            this.TabStop = true;
            this.SpecialKeys = SourceGrid.GridSpecialKeys.Default ^ SourceGrid.GridSpecialKeys.Tab;
            this.DeleteQuestionMessage = "You have chosen to delete this record." + Environment.NewLine + Environment.NewLine +
                                         "Do you really want to delete it?";
            this.FixedRows = 1;
            this.SortableHeaders = true;
            this.KeepRowSelectedAfterSort = true;
            this.AutoFindColumn = -1;
            this.Selection.FocusStyle = SourceGrid.FocusStyle.FocusFirstCellOnEnter | SourceGrid.FocusStyle.RemoveFocusCellOnLeave;
            this.Invalidate();
        }

        #region Methods for adding typed columns

        /// <summary>
        /// Easy method to add a new Text column.
        ///
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound
        /// </param>
        /// <returns>void</returns>
        public void AddTextColumn(String AColumnTitle, DataColumn ADataColumn)
        {
            AddTextColumn(AColumnTitle, ADataColumn, -1);
        }

        /// <summary>
        /// Easy method to add a new Text column.
        ///
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)
        /// </param>
        /// <returns>void</returns>
        public void AddTextColumn(String AColumnTitle, DataColumn ADataColumn, Int16 AColumnWidth)
        {
            AddTextColumn(AColumnTitle, ADataColumn, AColumnWidth, null, null, null, null);
        }

        /// <summary>
        /// Easy method to add a new Text column.
        /// </summary>
        /// <param name="AColumnTitle"></param>
        /// <param name="ADataColumn"></param>
        /// <param name="AColumnWidth"></param>
        /// <param name="AEditor"></param>
        public void AddTextColumn(String AColumnTitle, DataColumn ADataColumn, Int16 AColumnWidth, EditorBase AEditor)
        {
            AddTextColumn(AColumnTitle, ADataColumn, AColumnWidth, null, AEditor, null, null);
        }

        /// <summary>
        /// Easy method to add a new Text column.
        ///
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        /// <param name="AController"></param>
        /// <param name="AEditor">An instance of an Editor (based on ICellVirtual.Editor)</param>
        /// <param name="AModel"></param>
        /// <param name="AView"></param>
        /// <returns>void</returns>
        public void AddTextColumn(String AColumnTitle,
            DataColumn ADataColumn,
            Int16 AColumnWidth,
            ControllerBase AController,
            EditorBase AEditor,
            ModelContainer AModel,
            IView AView)
        {
            AddTextColumn(AColumnTitle, ADataColumn, AColumnWidth,
                AController, AEditor, AModel, AView, null);
        }

        /// <summary>
        /// Easy method to add a new Text column.
        ///
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        /// <param name="AController"></param>
        /// <param name="AEditor">An instance of an Editor (based on ICellVirtual.Editor)</param>
        /// <param name="AModel"></param>
        /// <param name="AView"></param>
        /// <param name="AConditionView"></param>
        ///
        /// <returns>void</returns>
        public void AddTextColumn(String AColumnTitle,
            DataColumn ADataColumn,
            Int16 AColumnWidth,
            ControllerBase AController,
            EditorBase AEditor,
            ModelContainer AModel,
            IView AView,
            SourceGrid.Conditions.ConditionView AConditionView)
        {
            SourceGrid.Cells.ICellVirtual ADataCell;
            SourceGrid.DataGridColumn AGridColumn;

            if (ADataColumn == null)
            {
                throw new ArgumentNullException("ADataColumn", "ADataColumn must not be nil!");
            }

            ADataCell = new SourceGrid.Cells.DataGrid.Cell();

            if (AController != null)
            {
                MessageBox.Show("AController <> nil!");
                try
                {
                    ADataCell.AddController(AController);
                }
                catch (Exception Exp)
                {
                    MessageBox.Show("TSgrdDataGrid.AddTextColumn: Exeption: " + Exp.ToString());
                }
            }

            if (AEditor != null)
            {
                ADataCell.Editor = AEditor;
            }

            if (AModel != null)
            {
                ADataCell.Model = AModel;
            }

            if (AView != null)
            {
                ADataCell.View = AView;
            }

            AGridColumn = new TSgrdTextColumn(this, ADataColumn, AColumnTitle, ADataCell, AColumnWidth, FSortableHeaders);

            if (AConditionView != null)
            {
                AGridColumn.Conditions.Add(AConditionView);
            }

            this.Columns.Insert(this.Columns.Count, AGridColumn);
        }

        /// <summary>
        /// Easy method to add a new CheckBox column.
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound
        /// </param>
        /// <returns>void</returns>
        public void AddCheckBoxColumn(String AColumnTitle, DataColumn ADataColumn)
        {
            AddCheckBoxColumn(AColumnTitle, ADataColumn, -1);
        }

        /// <summary>
        /// Easy method to add a new CheckBox column.
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AReadOnly">Set to true if the column should be read-only</param>
        /// <returns>void</returns>
        public void AddCheckBoxColumn(String AColumnTitle, DataColumn ADataColumn, bool AReadOnly)
        {
            AddCheckBoxColumn(AColumnTitle, ADataColumn, -1, AReadOnly);
        }

        /// <summary>
        /// Easy method to add a new CheckBox column.
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)
        /// </param>
        /// <returns>void</returns>
        public void AddCheckBoxColumn(String AColumnTitle, DataColumn ADataColumn, Int16 AColumnWidth)
        {
            AddCheckBoxColumn(AColumnTitle, ADataColumn, -1, null, true);
        }

        /// <summary>
        /// Easy method to add a new CheckBox column.
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        /// <param name="AReadOnly">Set to true if the column should be read-only</param>
        /// <returns>void</returns>
        public void AddCheckBoxColumn(String AColumnTitle, DataColumn ADataColumn, Int16 AColumnWidth, bool AReadOnly)
        {
            AddCheckBoxColumn(AColumnTitle, ADataColumn, -1, null, AReadOnly);
        }

        /// <summary>
        /// Easy method to add a new CheckBox column.
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        /// <param name="AEditor"></param>
        /// <param name="AReadOnly">Set to true if the column should be read-only</param>
        /// <returns>void</returns>
        public void AddCheckBoxColumn(String AColumnTitle, DataColumn ADataColumn, Int16 AColumnWidth, EditorBase AEditor, bool AReadOnly)
        {
            SourceGrid.Cells.ICellVirtual ADataCell;
            SourceGrid.DataGridColumn AGridColumn;

            if (ADataColumn == null)
            {
                throw new ArgumentNullException("ADataColumn", "ADataColumn must not be nil!");
            }

            ADataCell = new SourceGrid.Cells.DataGrid.CheckBox();

            if (AEditor != null)
            {
                ADataCell.Editor = AEditor;
            }

            AGridColumn = new TSgrdTextColumn(this, ADataColumn, AColumnTitle, ADataCell, AColumnWidth, FSortableHeaders);

            if (AReadOnly)
            {
                ADataCell.Editor.EnableEdit = false;
            }

            this.Columns.Insert(this.Columns.Count, AGridColumn);
        }

        /// <summary>
        /// Easy method to add a new Image column without a header text.
        ///
        /// </summary>
        /// <param name="AGetImageDelegate">Delegate method that will be called to retrieve
        /// the Image which should be displayed in the cell.
        /// </param>
        /// <returns>void</returns>
        public void AddImageColumn(DelegateGetImageForRow AGetImageDelegate)
        {
            AddImageColumn("", AGetImageDelegate);
        }

        /// <summary>
        /// Easy method to add a new Image column with header text.
        ///
        /// </summary>
        /// <param name="AGetImageDelegate">Delegate method that will be called to retrieve
        /// the Image which should be displayed in the cell.</param>
        /// <param name="AColumnTitle">Title of the HeaderColumn
        /// </param>
        /// <returns>void</returns>
        public void AddImageColumn(String AColumnTitle, DelegateGetImageForRow AGetImageDelegate)
        {
            SourceGrid.DataGridColumn AGridColumn;

            if (!(AGetImageDelegate != null))
            {
                throw new ArgumentNullException("AGetImageDelegate", "AGetImageDelegate must contain an assigned Delegate!");
            }

            AGridColumn = new TSgrdImageColumn(this, AColumnTitle, AGetImageDelegate);
            this.Columns.Insert(this.Columns.Count, AGridColumn);
        }

        /// <summary>
        /// Add a date column that is read-only. The date is displayed in an a common international data format, independent of a computer's date formatting settings.
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        public void AddDateColumn(String AColumnTitle, DataColumn ADataColumn)
        {
            SourceGrid.Cells.Editors.TextBoxUITypeEditor DateEditor = new SourceGrid.Cells.Editors.TextBoxUITypeEditor(typeof(DateTime));
            Ict.Common.TypeConverter.TDateConverter DateTypeConverter = new Ict.Common.TypeConverter.TDateConverter();

            DateEditor.EditableMode = EditableMode.None;
            DateEditor.TypeConverter = DateTypeConverter;

            AddTextColumn(AColumnTitle, ADataColumn, -1, null, DateEditor, null, null);
        }

        /// <summary>
        /// add a column that shows a currency value.
        /// aligns the value to the right.
        /// prints number in red if it is negative
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        public void AddCurrencyColumn(String AColumnTitle, DataColumn ADataColumn)
        {
            AddCurrencyColumn(AColumnTitle, ADataColumn, 2);
        }

        /// <summary>
        /// add a column that shows a currency value.
        /// aligns the value to the right.
        /// prints number in red if it is negative
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AFractionDigits">Number of digits after the decimal point</param>
        public void AddCurrencyColumn(String AColumnTitle, DataColumn ADataColumn, int AFractionDigits)
        {
            SourceGrid.Cells.Editors.TextBox CurrencyEditor = new SourceGrid.Cells.Editors.TextBox(typeof(decimal));
            CurrencyEditor.TypeConverter = new DevAge.ComponentModel.Converter.NumberTypeConverter(typeof(decimal), "N" + AFractionDigits.ToString());

            CurrencyEditor.EditableMode = EditableMode.None;


            // Non-negative value View
            SourceGrid.Cells.Views.Cell view = new SourceGrid.Cells.Views.Cell();
            view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            // Negative value View
            SourceGrid.Cells.Views.Cell NegativeNumberView = new SourceGrid.Cells.Views.Cell();
            NegativeNumberView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            NegativeNumberView.ForeColor = Color.Red;

            // Condition for negative value View
            SourceGrid.Conditions.ConditionView selectedConditionNegative =
                new SourceGrid.Conditions.ConditionView(NegativeNumberView);
            selectedConditionNegative.EvaluateFunction = (delegate(SourceGrid.DataGridColumn column,
                                                                   int gridRow, object itemRow)
                                                          {
                                                              DataRowView row = (DataRowView)itemRow;
                                                              return row[ADataColumn.ColumnName] is decimal
                                                              && (decimal)row[ADataColumn.ColumnName] < 0;
                                                          });

            AddTextColumn(AColumnTitle, ADataColumn, -1, null, CurrencyEditor, null, view, selectedConditionNegative);
        }

        #endregion

        #region Overridden Events

        /// <summary>
        /// when the grid is enabled or disabled
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnabledChanged(System.EventArgs e)
        {
            // MessageBox.Show('TSgrdDataGrid.OnEnabledChanged');
            if (!FShowColumnHeadersDisabled)
            {
                FShowColumnHeadersDisabled = true;
                FColumnHeaderView.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            }
            else
            {
                FShowColumnHeadersDisabled = false;
                FColumnHeaderView.ForeColor = System.Drawing.SystemColors.ControlText;
            }

            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// after sorting
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSortedRangeRows(SourceGrid.SortRangeRowsEventArgs e)
        {
            base.OnSortedRangeRows(e);

            // MessageBox.Show('Length(FRowsSelectedBeforeSort): ' + Convert.ToString(Length(FRowsSelectedBeforeSort)));
            if (FRowsSelectedBeforeSort.Length > 0)
            {
                if (FKeepRowSelectedAfterSort)
                {
                    this.Selection.ResetSelection(false);
                    this.Selection.Focus(Position.Empty, true);
                    this.Selection.SelectRow(this.Rows.DataSourceRowToIndex(FRowsSelectedBeforeSort[0]) + 1, true);
                }

                this.Selection.Focus(new Position(this.Rows.DataSourceRowToIndex(this.SelectedDataRows[0]) + 1, 0), true);
            }

            // MessageBox.Show('TSgrdDataGrid.OnSortedRangeRows');
        }

        /// <summary>
        /// before sorting
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSortingRangeRows(SourceGrid.SortRangeRowsEventArgs e)
        {
            FRowsSelectedBeforeSort = this.SelectedDataRowsAsDataRowView;
            base.OnSortingRangeRows(e);
        }

        #endregion

        #region Custom Events

        /// <summary>
        /// something changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataViewChanged(Object sender, ListChangedEventArgs e)
        {
            FAutoFindListRebuildNeeded = true;
        }

        /// <summary>
        /// double click on cell
        /// </summary>
        /// <param name="e"></param>
        private void OnDoubleClickCell(CellContextEventArgs e)
        {
            // MessageBox.Show('OnCellDoubleClick.  Column: ' + e.CellContext.Position.Column.ToString  + '; Row: ' + e.CellContext.Position.Row.ToString);
            if (DoubleClickCell != null)
            {
                DoubleClickCell(e.CellContext.Grid, e);
            }
        }

        /// <summary>
        /// double click on the header cell
        /// </summary>
        /// <param name="e"></param>
        private void OnDoubleClickHeaderCell(ColumnEventArgs e)
        {
            // MessageBox.Show('OnDoubleClickHeaderCell.  Column: ' + e.Column.ToString);
            if (DoubleClickHeaderCell != null)
            {
                DoubleClickHeaderCell(this, e);
            }
        }

        /// <summary>
        /// key has been pressed
        /// </summary>
        /// <param name="e"></param>
        private void OnInsertKeyPressed(RowEventArgs e)
        {
            // MessageBox.Show('OnInsertKeyPressed.  Row: ' + e.Row.ToString);
            if (InsertKeyPressed != null)
            {
                InsertKeyPressed(this, e);
            }
        }

        /// <summary>
        /// delete key has been pressed
        /// </summary>
        /// <param name="e"></param>
        private void OnDeleteKeyPressed(RowEventArgs e)
        {
            // MessageBox.Show('OnDeleteKeyPressed.  Row: ' + e.Row.ToString);
            if (DeleteKeyPressed != null)
            {
                // prevent the Grid from deleting the row as well!
                this.DeleteRowsWithDeleteKey = false;
                DeleteKeyPressed(this, e);
            }
        }

        /// <summary>
        /// enter key has been pressed
        /// </summary>
        /// <param name="e"></param>
        private void OnEnterKeyPressed(RowEventArgs e)
        {
            // MessageBox.Show('OnEnterKeyPressed.  Row: ' + e.Row.ToString);
            if (EnterKeyPressed != null)
            {
                EnterKeyPressed(this, e);
            }
        }

        /// <summary>
        /// space key has been pressed
        /// </summary>
        /// <param name="e"></param>
        private void OnSpaceKeyPressed(RowEventArgs e)
        {
            // MessageBox.Show('OnEnterKeyPressed.  Row: ' + e.Row.ToString);
            if (SpaceKeyPressed != null)
            {
                SpaceKeyPressed(this, e);
            }
        }

        #endregion

        #region Functionality customisation

        /**
         * This custom SourceGrid Controller handles the DoubleClick event of the Grid and
         * fires either the OnDoubleClickCell or OnDoubleClickHeaderCell Event.
         */
        class DoubleClickController : SourceGrid.Cells.Controllers.ControllerBase
        {
            public override void OnDoubleClick(SourceGrid.CellContext sender, EventArgs e)
            {
                base.OnDoubleClick(sender, e);

                SourceGrid.Position ClickPosition;
                SourceGrid.CellContextEventArgs CellArgs;
                SourceGrid.ColumnEventArgs HeaderCellArgs;

                // MessageBox.Show('TSgrdDataGrid.OnDoubleClick');

                ClickPosition = sender.Grid.PositionAtPoint(sender.Grid.PointToClient(MousePosition));

                if (ClickPosition != SourceGrid.Position.Empty)
                {
                    if ((sender.Grid.FixedRows == 1) && (ClickPosition.Row == 0))
                    {
                        // DoubleClick occured in a HeaderCell > fire OnDoubleClickHeaderCell Event
                        HeaderCellArgs = new SourceGrid.ColumnEventArgs(ClickPosition.Column);
                        ((TSgrdDataGrid)(sender.Grid)).OnDoubleClickHeaderCell(HeaderCellArgs);
                    }
                    else
                    {
                        Position ClickPosWithoutHeader = new Position(ClickPosition.Row, ClickPosition.Column);

                        if (sender.Grid.FixedRows == 1)
                        {
                            ClickPosWithoutHeader = new Position(ClickPosition.Row - 1, ClickPosition.Column);
                        }

                        // DoubleClick occured in a Cell > fire OnDoubleClickCell Event
                        CellArgs =
                            new SourceGrid.CellContextEventArgs(new CellContext(sender.Grid, ClickPosWithoutHeader));
                        ((TSgrdDataGrid)(sender.Grid)).OnDoubleClickCell(CellArgs);
                    }
                }
            }
        }

        /// <summary>
        /// Used only internally to process the Enter and Space keys. Must be public to
        /// work.
        ///
        /// </summary>
        /// <param name="AKeyData">Passed by the Operating System.
        /// @result See .NET 1.1 API documentation on how this needs to be set.
        /// </param>
        /// <returns>void</returns>
        protected override Boolean ProcessDialogKey(Keys AKeyData)
        {
            Int32 SelectedDataRow;

            // MessageBox.Show('TSgrdDataGrid.ProcessDialogKey.  KeyCode: ' + Enum(AKeyData).ToString("G"));
            if (AKeyData == Keys.Enter)
            {
                // Enter key should raise OnEnterKeyPressed Event (if assigned)
                if (EnterKeyPressed != null)
                {
                    if (this.SelectedDataRows.Length > 0)
                    {
                        SelectedDataRow = this.Rows.DataSourceRowToIndex(this.SelectedDataRows[0]);
                    }
                    else
                    {
                        SelectedDataRow = -1;
                    }

                    this.OnEnterKeyPressed(new RowEventArgs(SelectedDataRow));
                    return true;
                }
            }
            else if (AKeyData == Keys.Space)
            {
                // Space key should raise OnSpaceKeyPressed Event (if assigned)
                if (SpaceKeyPressed != null)
                {
                    if (this.SelectedDataRows.Length > 0)
                    {
                        SelectedDataRow = this.Rows.DataSourceRowToIndex(this.SelectedDataRows[0]);
                    }
                    else
                    {
                        SelectedDataRow = -1;
                    }

                    this.OnSpaceKeyPressed(new RowEventArgs(SelectedDataRow));
                    return true;
                }
            }

            return base.ProcessDialogKey(AKeyData);
        }

        /// <summary>
        /// This is a replacement for the DataGrid.Rows.DataSourceRowToIndex function.
        /// The DataSourceRowToIndex function internally uses IList.IndexOf, which doesn't find DataRowView
        /// objects that are returned from a DataView that is created from the Grid's DataView
        /// [((DevAge.ComponentModel.BoundDataView) base.DataSource).mDataView] when searchin in the Grid's DataView
        /// --- for IList.IndexOf(), the same DataRowView object seem to be two differnt objects...!
        ///
        /// DataSourceRowToIndex2 manually iterates through the Grid's DataView and compares Rows objects. This works!
        /// </summary>
        /// <returns>void</returns>
        public int DataSourceRowToIndex2(DataRowView ADataRowView)
        {
            int RowIndex = -1;

            for (int Counter2 = 0; Counter2 < (this.DataSource as BoundDataView).DataView.Count; Counter2++)
            {
                if ((this.DataSource as BoundDataView).DataView[Counter2].Row == ADataRowView.Row)
                {
                    RowIndex = Counter2;
                }
            }

            return RowIndex;
        }

        /// select a row in the grid, and invoke the even for FocusedRowChanged
        public void SelectRowInGrid(Int32 ARowNumberInGrid)
        {
            this.Selection.ResetSelection(false);
            this.Selection.SelectRow(ARowNumberInGrid, true);

            // scroll to the row
            this.ShowCell(new SourceGrid.Position(ARowNumberInGrid, 0), true);

            // invoke the event for FocusedRowChanged
            this.Selection.FocusRow(ARowNumberInGrid);

            //FocusRowEntered.Invoke(this, new SourceGrid.RowEventArgs(ARowNumberInGrid));
        }

        /// <summary>
        /// Performs selection of rows with a matching first character as the user
        /// presses certain keys that produce characters that this procedure can search
        /// for.
        ///
        /// </summary>
        /// <param name="AKey">Keyboard code, passed in from ProcessSpecialGridKey
        /// </param>
        /// <returns>void</returns>
        private void PerformAutoFindFirstCharacter(Keys AKey)
        {
            DataTable GridDataTable;
            Int16 Counter;
            Int32 CurrentlySelectedGridRow;
            Int32 FoundGridRow;
            Int32 NewSelectedItemRow;
            DataRowView TmpDataRowView;
            String DataColumnName;
            String SearchCharacter = "";
            String SelectClause;

            if ((AKey != FLastKeyCode) || (FAutoFindListRebuildNeeded))
            {
                // Build a DataView that start with the typed character
                GridDataTable = ((DevAge.ComponentModel.BoundDataView) base.DataSource).DataView.Table;

                if ((FAutoFindColumn < 0) || (FAutoFindColumn >= this.Columns.Count))
                {
                    // GridDataTable.Columns.Count
                    throw new TDataGridInvalidAutoFindColumnException(
                        "The specified AutoFindColumn is out of the range of DataGridColumns that the Grid has");
                }

                if (this.Columns[FAutoFindColumn].PropertyName == null)
                {
                    throw new TDataGridInvalidAutoFindColumnException(
                        "The specified AutoFindColumn is not a DataBound DataGridColumn! AutoFind can only be used with DataBound DataGridColumns.");
                }
                else
                {
                    DataColumnName = this.Columns[FAutoFindColumn].PropertyName;

                    //                  MessageBox.Show("DataColumnName: " + DataColumnName);
                }

                // Determine SearchCharacter
                if ((AKey >= Keys.A) && (AKey <= Keys.Z))
                {
                    SearchCharacter = AKey.ToString("G");
                }
                else if ((AKey == Keys.Add) || (AKey == Keys.Oemplus))
                {
                    SearchCharacter = "+";
                }
                else if ((AKey == Keys.Subtract) || (AKey == Keys.OemMinus))
                {
                    SearchCharacter = "-";
                }
                else if (AKey == Keys.Space)
                {
                    // Note: Space only gets through to there when SpaceKeyPressed is not assigned!
                    SearchCharacter = " ";
                }
                else if ((AKey >= Keys.D0) && (AKey <= Keys.D9))
                {
                    // Remove leading 'D'
                    SearchCharacter = AKey.ToString("G").Substring(1);
                }
                else if ((AKey >= Keys.NumPad0) && (AKey <= Keys.NumPad9))
                {
                    // Remove leading 'NumPad'
                    SearchCharacter = AKey.ToString("G").Substring(6);
                }

                //              MessageBox.Show("SearchCharacter: " + SearchCharacter);

                SelectClause = DataColumnName + " LIKE '" + SearchCharacter + "%'";

                //              MessageBox.Show("SelectClause: " + SelectClause);
                FAutoFindMatchingDataView = new DataView(GridDataTable,
                    SelectClause,
                    ((DevAge.ComponentModel.BoundDataView) base.DataSource).DataView.Sort,
                    DataViewRowState.CurrentRows);

                //              MessageBox.Show("FAutoFindMatchingDataView.Count: " + FAutoFindMatchingDataView.Count.ToString());
                FAutoFindListRebuildNeeded = false;
            }

            if (FAutoFindMatchingDataView.Count > 0)
            {
                if (this.Selection.IsEmpty())
                {
                    CurrentlySelectedGridRow = 0;
                }
                else
                {
                    CurrentlySelectedGridRow = this.Selection.ActivePosition.Row;
                }

                //              MessageBox.Show("CurrentlySelectedGridRow: " + CurrentlySelectedGridRow.ToString());

                NewSelectedItemRow = -1;

                // Remove focus from previous DataCell (to unhighlight it)
                this.Selection.Focus(Position.Empty, true);

                for (Counter = 0; Counter <= FAutoFindMatchingDataView.Count - 1; Counter += 1)
                {
                    TmpDataRowView = FAutoFindMatchingDataView[Counter];

                    /*
                     * Can't use "FoundGridRow := Self.Rows.DataSourceRowToIndex(TmpDataRowView);" - this internally
                     * uses IList.IndexOf, which doesn't find DataRowView Objects returned from FAutoFindMatchingDataView
                     * in ((DevAge.ComponentModel.BoundDataView) base.DataSource).mDataView - they seem to be two differnt
                     * objects...!
                     * Therefore I need to use our own function, DataSourceRowToIndex2, which manually iterates through the
                     * mDataView and compares Row objects. This works!
                     */
                    FoundGridRow = this.DataSourceRowToIndex2(TmpDataRowView);

                    //                  MessageBox.Show("FoundGridRow: " + FoundGridRow.ToString());
                    if (FoundGridRow >= CurrentlySelectedGridRow)
                    {
                        NewSelectedItemRow = FoundGridRow;

                        //                      MessageBox.Show("Found Row below CurrentGridRow. NewSelectedItemRow: " + NewSelectedItemRow.ToString());
                        break;
                    }
                }

                if (NewSelectedItemRow != -1)
                {
                    // A matching Row was found after the currently selected row, so select it
                    // Scroll grid to line where the new record is now displayed
                    this.ShowCell(new Position(NewSelectedItemRow + 1, 0), false);
                    this.Selection.Focus(new Position(NewSelectedItemRow + 1, 0), true);
                }
                else
                {
                    // No matching Row was found after the currently selected row, so select the first matching Row
                    TmpDataRowView = FAutoFindMatchingDataView[0];

                    /*
                     * Can't use "FoundGridRow := Self.Rows.DataSourceRowToIndex(TmpDataRowView);" - this internally
                     * uses IList.IndexOf, which doesn't find DataRowView Objects returned from FAutoFindMatchingDataView
                     * in ((DevAge.ComponentModel.BoundDataView) base.DataSource).mDataView - they seem to be two differnt
                     * objects...!
                     * Therefore I need to use our own function, DataSourceRowToIndex2, which manually iterates through the
                     * mDataView and compare Rows objects. This works!
                     */
                    NewSelectedItemRow = this.DataSourceRowToIndex2(TmpDataRowView);

                    //                  MessageBox.Show("Only found Row above CurrentGridRow! NewSelectedItemRow: " + NewSelectedItemRow.ToString());

                    // Scroll grid to line where the new record is now displayed
                    this.ShowCell(new Position(NewSelectedItemRow + 1, 0), false);
                    this.Selection.Focus(new Position(NewSelectedItemRow + 1, 0), true);
                }
            }
        }

        /// <summary>
        /// Used only internally to to be able to implement other custom key handling.
        /// Must be public to work.
        /// </summary>
        /// <param name="AKeyEventArgs">Passed by the Operating System.</param>
        /// <returns>void</returns>
        public override void ProcessSpecialGridKey(KeyEventArgs AKeyEventArgs)
        {
            base.ProcessSpecialGridKey(AKeyEventArgs);
            Int32 SelectedDataRow;

            // MessageBox.Show('TSgrdDataGrid.ProcessSpecialGridKey:  KeyCode: ' + Enum(AKeyEventArgs.KeyCode).ToString("G") +
            // 'Modifiers: ' + Enum(AKeyEventArgs.Modifiers).ToString("G")  );
            if (AKeyEventArgs.KeyCode == Keys.Home)
            {
                // Key for scrolling to and selecting the first row in the Grid
                // MessageBox.Show('Home pressed!');
                this.Selection.ResetSelection(false);
                this.Selection.Focus(Position.Empty, false);
                this.Selection.SelectRow(1, true);

                // Scroll grid to line where the selection is now displayed
                this.ShowCell(new Position(1, 0), false);

                // Give focus to the rows so that Cursor keys, PageUp/PageDown, etc. work
                this.Selection.Focus(new Position(this.Rows.DataSourceRowToIndex(this.SelectedDataRows[0]) + 1, 1), false);
            }
            // Key for scrolling to and selecting the last row in the Grid
            else if (AKeyEventArgs.KeyCode == Keys.End)
            {
                // MessageBox.Show('End pressed!  Rows: ' + this.Rows.Count.ToString);
                this.Selection.ResetSelection(false);
                this.Selection.Focus(Position.Empty, false);
                this.Selection.SelectRow(this.Rows.Count - 1, true);

                // Scroll grid to line where the selection is now displayed
                this.ShowCell(new Position(this.Rows.Count - 1, 0), false);

                // Give focus to the rows so that Cursor keys, PageUp/PageDown, etc. work
                this.Selection.Focus(new Position(this.Rows.DataSourceRowToIndex(this.SelectedDataRows[0]) + 1, 1), false);
            }
            // Key for firing OnInsertKeyPressed event
            else if (AKeyEventArgs.KeyCode == Keys.Insert)
            {
                // MessageBox.Show('Insert pressed!');
                // Fire OnInsertKeyPressed event.
                if (this.SelectedDataRows.Length > 0)
                {
                    SelectedDataRow = this.Rows.DataSourceRowToIndex(this.SelectedDataRows[0]) + 1;
                }
                else
                {
                    SelectedDataRow = -1;
                }

                this.OnInsertKeyPressed(new RowEventArgs(SelectedDataRow));
            }
            // Key for firing OnDeleteKeyPressed event
            else if (AKeyEventArgs.KeyCode == Keys.Delete)
            {
                // MessageBox.Show('Delete pressed!');
                // Fire OnDeleteKeyPressed event.
                if (this.SelectedDataRows.Length > 0)
                {
                    SelectedDataRow = this.Rows.DataSourceRowToIndex(this.SelectedDataRows[0]) + 1;
                }
                else
                {
                    SelectedDataRow = -1;
                }

                this.OnDeleteKeyPressed(new RowEventArgs(SelectedDataRow));
            }
            // Keys that can trigger AutoFind
            else if (((AKeyEventArgs.KeyCode >= Keys.A)
                      && (AKeyEventArgs.KeyCode <= Keys.Z)) || (AKeyEventArgs.KeyCode == Keys.Add) || (AKeyEventArgs.KeyCode == Keys.Subtract)
                     || (AKeyEventArgs.KeyCode == Keys.Oemplus) || (AKeyEventArgs.KeyCode == Keys.OemMinus) || (AKeyEventArgs.KeyCode == Keys.Space)
                     || ((AKeyEventArgs.KeyCode >= Keys.D0)
                         && (AKeyEventArgs.KeyCode <= Keys.D9)) || ((AKeyEventArgs.KeyCode >= Keys.NumPad0) && (AKeyEventArgs.KeyCode <= Keys.NumPad9)))
            {
                // Note: Space only gets through to there when SpaceKeyPressed is not assigned!
                if (AutoFindMode == TAutoFindModeEnum.FirstCharacter)
                {
                    PerformAutoFindFirstCharacter(AKeyEventArgs.KeyCode);
                }
            }

            FLastKeyCode = AKeyEventArgs.KeyCode;
        }

        #endregion
    }
    #endregion

    /// <summary>
    /// behaviour for typing a value and going to an appropriate row
    /// </summary>
    public enum TAutoFindModeEnum
    {
        /// <summary>
        /// no auto find at all
        /// </summary>
        NoAutoFind,

        /// <summary>
        /// look for lines starting with a given character
        /// </summary>
        FirstCharacter,

        /// <summary>
        /// look for the full string (not implemented yet)
        /// </summary>
        FullString
    };

    /// <summary>
    /// delegate for tooltip
    /// </summary>
    public delegate String TDelegateGetToolTipText(Int16 AColumn, Int16 ARow);

    /// <summary>
    /// what happens when key has been pressed
    /// </summary>
    public delegate void TKeyPressedEventHandler(System.Object Sender, RowEventArgs e);

    /// <summary>
    /// cell has been clicked
    /// </summary>
    public delegate void TClickCellEventHandler(System.Object Sender, CellContextEventArgs e);

    /// <summary>
    /// header row has been clicked
    /// </summary>
    public delegate void TClickHeaderCellEventHandler(System.Object Sender, ColumnEventArgs e);

    /// <summary>
    /// cell has been double clicked
    /// </summary>
    public delegate void TDoubleClickCellEventHandler(System.Object Sender, CellContextEventArgs e);

    /// <summary>
    /// header cell has been double clicked
    /// </summary>
    public delegate void TDoubleClickHeaderCellEventHandler(System.Object Sender, ColumnEventArgs e);


    #region TDataGridInvalidAutoFindColumnException

    /// <summary>
    /// cannot find
    /// </summary>
    public class TDataGridInvalidAutoFindColumnException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TDataGridInvalidAutoFindColumnException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public TDataGridInvalidAutoFindColumnException(String msg) : base(msg)
        {
        }
    }
    #endregion

    #region TDataGridAutoFindModeNotImplementedYetException

    /// <summary>
    /// Auto Find not implemented yet
    /// </summary>
    public class TDataGridAutoFindModeNotImplementedYetException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TDataGridAutoFindModeNotImplementedYetException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public TDataGridAutoFindModeNotImplementedYetException(String msg) : base(msg)
        {
        }
    }
    #endregion
}