// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template inc\template\src\Winforms\windowEditWebConnectorMasterDetail
//
// DO NOT edit manually, DO NOT edit with the designer
//
{#GPLFILEHEADER}
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;	  
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
{#IFDEF SHAREDVALIDATIONNAMESPACEMODULE}
using {#SHAREDVALIDATIONNAMESPACEMODULE};
{#ENDIF SHAREDVALIDATIONNAMESPACEMODULE}
{#USINGNAMESPACES}

namespace {#NAMESPACE}
{

  /// auto generated: {#FORMTITLE}
  public partial class {#CLASSNAME}: System.Windows.Forms.Form, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;
    private {#DATASETTYPE} FMainDS;
{#IFDEF SHOWDETAILS}
    private int FCurrentRow;
{#ENDIF SHOWDETAILS}

    /// constructor
    public {#CLASSNAME}(Form AParentForm) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      {#CATALOGI18N}
      #endregion

      {#ASSIGNFONTATTRIBUTES}
      
      FPetraUtilsObject = new {#UTILOBJECTCLASS}(AParentForm, this, stbMain);
      {#INITUSERCONTROLS}
      FMainDS = new {#DATASETTYPE}();
      {#INITMANUALCODE}
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      {#INITACTIONSTATE}

{#IFDEF MASTERTABLE OR DETAILTABLE}
      BuildValidationControlsDict();
{#ENDIF MASTERTABLE OR DETAILTABLE}
    }

    {#EVENTHANDLERSIMPLEMENTATION}

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position

    }

{#IFDEF CANFINDWEBCONNECTOR_CREATEMASTER}
    /// automatically generated function from webconnector
    public bool Create{#MASTERTABLE}({#CREATEMASTER_FORMALPARAMETERS})
    {
{#IFDEF CREATEMASTER_WITHVERIFICATION}
        TVerificationResultCollection VerificationResult;

        FMainDS = {#WEBCONNECTORMASTER}.Create{#MASTERTABLE}({#CREATEMASTER_ACTUALPARAMETERS}, out VerificationResult);

        if (VerificationResult != null && VerificationResult.Count > 0)
        {
            return CreateMasterManual({#CREATEMASTER_ACTUALPARAMETERS}, VerificationResult);
        }
        else
        {
            FPetraUtilsObject.SetChangedFlag();

            ShowData(FMainDS.{#MASTERTABLE}[0]);
            
            return true;
        }
{#ENDIF CREATEMASTER_WITHVERIFICATION}
{#IFDEF CREATEMASTER_WITHOUTVERIFICATION}
        FMainDS = {#WEBCONNECTORMASTER}.Create{#MASTERTABLE}({#CREATEMASTER_ACTUALPARAMETERS});

        FPetraUtilsObject.SetChangedFlag();

        ShowData(FMainDS.{#MASTERTABLE}[0]);
        
        return true;
{#ENDIF CREATEMASTER_WITHOUTVERIFICATION}
    }
{#ENDIF CANFINDWEBCONNECTOR_CREATEMASTER}

{#IFDEF CANFINDWEBCONNECTOR_CREATEDETAIL}

    private bool newRecordUnsavedInFocus = false;

    /// automatically generated, create a new record of {#DETAILTABLE} and display on the edit screen
    public bool Create{#DETAILTABLE}({#CREATEDETAIL_FORMALPARAMETERS})
    {
        if(ValidateAllData(true, true))
        {    
            FMainDS.Merge({#WEBCONNECTORDETAIL}.Create{#DETAILTABLE}({#CREATEDETAIL_ACTUALPARAMETERS}));
            FMainDS.InitVars();
            FMainDS.{#DETAILTABLE}.InitVars();

            FPetraUtilsObject.SetChangedFlag();

            DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
            myDataView.AllowNew = false;
			grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

            SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1);
            InvokeFocusedRowChanged(grdDetails.SelectedRowIndex());

            //Must be set after the FocusRowChanged event is called as it sets this flag to false
            newRecordUnsavedInFocus = true;

            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
            ShowDetails(FPreviouslySelectedDetailRow);
			
            Control[] pnl = this.Controls.Find("pnlDetails", true);
            if (pnl.Length > 0)
            {
	            //Look for Key & Description fields
	            bool keyFieldFound = false;
	            foreach (Control detailsCtrl in pnl[0].Controls)
	            {
	                if (!keyFieldFound && (detailsCtrl is TextBox || detailsCtrl is ComboBox))
	                {
	                    keyFieldFound = true;
	                    detailsCtrl.Focus();
	                }
	
	                if (detailsCtrl is TextBox && detailsCtrl.Name.Contains("Descr") && detailsCtrl.Text == string.Empty)
	                {
	                    detailsCtrl.Text = "PLEASE ENTER DESCRIPTION";
	                    break;
	                }
	            }
            }
        
            return true;
        }
        else
        {
            return false;
        }
    }
{#ENDIF CANFINDWEBCONNECTOR_CREATEDETAIL}
{#IFDEF DETAILTABLE}

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.{#DETAILTABLE}.PrimaryKey)
            {
                string value1 = FMainDS.{#DETAILTABLE}.Rows[ARowNumberInTable][myColumn].ToString();
                string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[Counter][myColumn.Ordinal].ToString();
                if (value1 != value2)
                {
                    found = false;
                }
            }
            if (found)
            {
                RowNumberGrid = Counter + 1;
                break;
            }
        }

        grdDetails.SelectRowInGrid(RowNumberGrid, TSgrdDataGrid.TInvokeGridFocusEventEnum.NoFocusEvent);
    }

{#IFDEF SHOWDETAILS OR GENERATEGETSELECTEDDETAILROW}

    /// return the selected row
    public {#DETAILTABLETYPE}Row GetSelectedDetailRow()
    {
        {#GETSELECTEDDETAILROW}
    }
{#ENDIF SHOWDETAILS OR GENERATEGETSELECTEDDETAILROW}

{#ENDIF DETAILTABLE}

{#IFDEF CANFINDWEBCONNECTOR_LOADMASTER}

    /// automatically generated function from webconnector
    public bool Load{#MASTERTABLE}({#LOADMASTER_FORMALPARAMETERS})
    {
        FMainDS.Merge({#WEBCONNECTORMASTER}.Load{#MASTERTABLE}({#LOADMASTER_ACTUALPARAMETERS}));

        ShowData(FMainDS.{#MASTERTABLE}[0]);
        
        return true;
    }
{#ENDIF CANFINDWEBCONNECTOR_LOADMASTER}

    private void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
        {#PRIMARYKEYCONTROLSREADONLY}
    }

{#IFDEF SHOWDATA}
    private void ShowData({#MASTERTABLETYPE}Row ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
{#IFDEF SAVEDETAILS}
        grdDetails.Selection.FocusRowLeaving -= new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
{#ENDIF SAVEDETAILS}
        
        {#SHOWDATA}
{#IFDEF DETAILTABLE}
        pnlDetails.Enabled = false;
        if (FMainDS.{#DETAILTABLE} != null)
        {
            DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
{#IFDEF DETAILTABLESORT}
            myDataView.Sort = "{#DETAILTABLESORT}";
{#ENDIF DETAILTABLESORT}
{#IFDEF DETAILTABLEFILTER}
            myDataView.RowFilter = {#DETAILTABLEFILTER};
{#ENDIF DETAILTABLEFILTER}
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            if (FMainDS.{#DETAILTABLE}.Rows.Count > 0)
            {
                grdDetails.SelectRowInGrid(1);
                pnlDetails.Enabled = true;
            }
        }
{#ENDIF DETAILTABLE}
        FPetraUtilsObject.EnableDataChangedEvent();
{#IFDEF SAVEDETAILS}
        grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
{#ENDIF SAVEDETAILS}
    }
{#ENDIF SHOWDATA}
{#IFDEF UNDODATA}

    private void UndoData(DataRow ARow, Control AControl)
    {
        {#UNDODATA}
    }
{#ENDIF UNDODATA}

{#IFDEF SHOWDETAILS}
    private void ShowDetails({#DETAILTABLETYPE}Row ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        {#SHOWDETAILS}
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private {#DETAILTABLETYPE}Row FPreviouslySelectedDetailRow = null;
	
    private bool firstFocusEventHasRun = false;
    private bool isRepeatLeaveEvent = false;
    private int gridRowsCount = 0;
    private int numGridRows	 = 0;
    private int gridRowsCountHasChanged = 0;
    private bool newFocusEventStarted = false;

    private void FocusPreparation(bool AIsLeaveEvent)
    {
    	if (isRepeatLeaveEvent)
    	{
    		return;
    	}
    	
    	numGridRows = grdDetails.Rows.Count;

		//first run only
    	if (!firstFocusEventHasRun)
    	{
    		firstFocusEventHasRun = true;
    		gridRowsCount = numGridRows;
    	}
    	
    	//Specify if it is a row change, add or delete
    	if (gridRowsCount == numGridRows)
    	{
    		gridRowsCountHasChanged = 0;
    	}
    	else if (gridRowsCount > numGridRows)
        {
        	gridRowsCount = numGridRows;
        	gridRowsCountHasChanged = -1;
        }
    	else if (gridRowsCount < numGridRows)
    	{
        	gridRowsCount = numGridRows;
        	gridRowsCountHasChanged = 1;
    	}
    	
    }
    
    private void InvokeFocusedRowChanged(int AGridRowNumber)
    {
		SourceGrid.RowEventArgs rowArgs  = new SourceGrid.RowEventArgs(AGridRowNumber);
		FocusedRowChanged(grdDetails, rowArgs);
    }
{#IFDEF SAVEDETAILS}

    private void FocusRowLeaving(object sender, SourceGrid.RowCancelEventArgs e)
    {        
		//Ignore this event if currently sorting
    	if (grdDetails.Sorting)
    	{
    		newFocusEventStarted = false;
    		return;
    	}    	
    	
    	if (newFocusEventStarted == false)
    	{
    		newFocusEventStarted = true;
    	}

    	FocusPreparation(true);

    	if (!isRepeatLeaveEvent)
        {
	    	isRepeatLeaveEvent = true;
	    	
            if (gridRowsCountHasChanged == -1 || numGridRows == 2)  //do not run validation if cancelling current row
																	// OR only 1 row present so no rowleaving event possible
            {
            	e.Cancel = true;
            }
            
            Console.WriteLine("FocusRowLeaving");
            
            if (!ValidateAllData(true, true))
            {
                e.Cancel = true;
            }
        }
        else
        {
            // Reset flag
	    	isRepeatLeaveEvent = false;
            e.Cancel = true;
        }
    }
{#ENDIF SAVEDETAILS}

    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
        newRecordUnsavedInFocus = false;
    	
        isRepeatLeaveEvent = false;

        if (!grdDetails.Sorting)
        {
	    	//Sometimes, FocusedRowChanged get called without FocusRowLeaving
	    	//  so need to handle that
	    	if (!newFocusEventStarted)
	    	{
	    		//This implies start of a new event chain without a previous FocusRowLeaving
	    		FocusPreparation(false);
	    	}
	    	
	        //Only allow, row change, add or delete, not repeat events from grid changing focus
	    	if(e.Row != FCurrentRow && gridRowsCountHasChanged == 0)
	        {
	    		// Transfer data from Controls into the DataTable
	            if (FPreviouslySelectedDetailRow != null)
	            {
	                GetDetailsFromControls(FPreviouslySelectedDetailRow);
	            }
	
	            // Display the details of the currently selected Row
	            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
	            ShowDetails(FPreviouslySelectedDetailRow);
	            pnlDetails.Enabled = true;
	    	}
	    	else if (gridRowsCountHasChanged == 1) //Addition
	    	{
	    		
	    	}
	    	else if (gridRowsCountHasChanged == -1) //Deletion
	    	{
	    		if (numGridRows > 1) //Implies at least one record still left
	    		{
	    			// Select and display the details of the currently selected Row without causing an event
	    			grdDetails.SelectRowInGrid(e.Row, TSgrdDataGrid.TInvokeGridFocusEventEnum.NoFocusEvent);
		            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
		            ShowDetails(FPreviouslySelectedDetailRow);
		            pnlDetails.Enabled = true;
	    		}
	    		else	
	    		{
	                FPreviouslySelectedDetailRow = null;
		            pnlDetails.Enabled = false;
	    		}
	    	}
        }
        
    	FCurrentRow = e.Row;
	    
	    //Event chain tidy-up
		gridRowsCountHasChanged = 0;
	    newFocusEventStarted = false;
	}
{#ENDIF SHOWDETAILS}
   
{#IFDEF MASTERTABLE}

    private void GetDataFromControls({#MASTERTABLETYPE}Row ARow)
    {
{#IFDEF SAVEDATA}
        {#SAVEDATA}
{#ENDIF SAVEDATA}
{#IFDEF SAVEDETAILS}
        GetDetailsFromControls(FPreviouslySelectedDetailRow);
{#ENDIF SAVEDETAILS}
    }
{#ENDIF MASTERTABLE}
{#IFNDEF MASTERTABLE}

    private void GetDataFromControls()
    {
{#IFDEF SAVEDATA}
        {#SAVEDATA}
{#ENDIF SAVEDATA}
    }
{#ENDIFN MASTERTABLE}

{#IFDEF SAVEDETAILS}
    private void GetDetailsFromControls({#DETAILTABLETYPE}Row ARow)
    {
        if (ARow != null)
        {
            ARow.BeginEdit();
            {#SAVEDETAILS}
            ARow.BeginEdit();
        }
    }

{#ENDIF SAVEDETAILS}
    /// <summary>
    /// Performs data validation.
    /// </summary>
    /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing 
    /// to another record, otherwise set it to false.</param>
    /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
    /// user, otherwise set it to false.</param>
    /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>    
    private bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors)
    {
        bool ReturnValue = false;

        // Record a new Data Validation Run. (All TVerificationResults/TScreenVerificationResults that are created during this 'run' are associated with this 'run' through that.)
        FPetraUtilsObject.VerificationResultCollection.RecordNewDataValidationRun();

{#IFDEF MASTERTABLE}
        // Validate MasterTable
        GetDataFromControls(FMainDS.{#MASTERTABLE}[0]);
        ValidateData(FMainDS.{#MASTERTABLE}[0]);
{#IFDEF VALIDATEDATAMANUAL}
        ValidateDataManual(FMainDS.{#MASTERTABLE}[0]);
{#ENDIF VALIDATEDATAMANUAL}
{#ENDIF MASTERTABLE}
{#IFNDEF MASTERTABLE}
        GetDataFromControls();
{#ENDIFN MASTERTABLE}

{#IFDEF SHOWDETAILS}
        {#DETAILTABLETYPE}Row CurrentRow = GetSelectedDetailRow();

        if (CurrentRow != null)
        {
            // Validate DetailTable
{#IFNDEF MASTERTABLE}
            GetDetailsFromControls(CurrentRow);
{#ENDIFN MASTERTABLE}
            ValidateDataDetails(CurrentRow);
{#IFDEF VALIDATEDATADETAILSMANUAL}
            ValidateDataDetailsManual(CurrentRow);
{#ENDIF VALIDATEDATADETAILSMANUAL}            
{#ENDIF SHOWDETAILS}
{#IFDEF PERFORMUSERCONTROLVALIDATION}

            // Perform validation in UserControls, too
            {#USERCONTROLVALIDATION}
{#ENDIF PERFORMUSERCONTROLVALIDATION}

{#IFDEF SHOWDETAILS}
            if (AProcessAnyDataValidationErrors)
            {
                if (!FPetraUtilsObject.VerificationResultCollection.Contains(FMainDS.{#DETAILTABLE})) 
                {
                    // There isn't a Data Validation Error/Warning recorded for the Detail Table, therefore don't present the
                    // Data Validation Errors/Warnins as something that is record-related.
                    ARecordChangeVerification = false;
                }

                // Process Data Validation result(s)
                ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
                    this.GetType(), null);
            }
{#IFDEF MASTERTABLE}
        }
        else if (AProcessAnyDataValidationErrors)
        {
            if (!FPetraUtilsObject.VerificationResultCollection.Contains(FMainDS.{#DETAILTABLE})) 
            {
                // There isn't a Data Validation Error/Warning recorded for the Detail Table, therefore don't present the
                // Data Validation Errors/Warnins as something that is record-related.
                ARecordChangeVerification = false;
            }

            // Process Data Validation result(s)
            ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
                this.GetType(), null);
        }
{#ENDIF MASTERTABLE}
{#IFNDEF MASTERTABLE}            
        }
        else
        {
            ReturnValue = true;
        }
{#ENDIFN MASTERTABLE}
{#ENDIF SHOWDETAILS}
{#IFNDEF SHOWDETAILS}
        if (AProcessAnyDataValidationErrors)
        {
            // Process Data Validation result(s)
            ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
                this.GetType(), null);
        }
{#ENDIFN SHOWDETAILS}

        if(ReturnValue)
        {
            // Remove a possibly shown Validation ToolTip as the data validation succeeded
            FPetraUtilsObject.ValidationToolTip.RemoveAll();
        }

        return ReturnValue;
    }

#region Implement interface functions

    /// auto generated
    public void RunOnceOnActivation()
    {
        {#RUNONCEONACTIVATIONMANUAL}
        {#RUNONCEINTERFACEIMPLEMENTATION}
    }

    /// <summary>
    /// Adds event handlers for the appropiate onChange event to call a central procedure
    /// </summary>
    public void HookupAllControls()
    {
        {#HOOKUPINTERFACEIMPLEMENTATION}
    }

    /// auto generated
    public void HookupAllInContainer(Control container)
    {
        FPetraUtilsObject.HookupAllInContainer(container);
    }

    /// auto generated
    public bool CanClose()
    {
        return FPetraUtilsObject.CanClose();
    }

    /// auto generated
    public TFrmPetraUtils GetPetraUtilsObject()
    {
        return (TFrmPetraUtils)FPetraUtilsObject;
    }

    /// auto generated
    public void FileSave(object sender, EventArgs e)
    {
        SaveChanges();
    }

    /// <summary>
    /// save the changes on the screen
    /// </summary>
    /// <returns></returns>
    public bool SaveChanges()
    {
        bool ReturnValue = false;
        
        FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());

//TODO?  still needed?      FMainDS.AApDocument.Rows[0].BeginEdit();
        GetDataFromControls(FMainDS.{#MASTERTABLE}[0]);

        // Clear any validation errors so that the following call to ValidateAllData starts with a 'clean slate'.
        FPetraUtilsObject.VerificationResultCollection.Clear();

        if (ValidateAllData(false, true))
        {
            foreach (DataTable InspectDT in FMainDS.Tables)
            {
                foreach (DataRow InspectDR in InspectDT.Rows)
                {
                    InspectDR.EndEdit();
                }
            }

            if (FPetraUtilsObject.HasChanges)
            {
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataInProgress);
                this.Cursor = Cursors.WaitCursor;

                TSubmitChangesResult SubmissionResult;
                TVerificationResultCollection VerificationResult;

                {#DATASETTYPE} SubmitDS = FMainDS.GetChangesTyped(true);
                
                if (SubmitDS == null)
                {
                    // There is nothing to be saved.
                    // Update UI
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);
                    this.Cursor = Cursors.Default;

                    // We don't have unsaved changes anymore
                    FPetraUtilsObject.DisableSaveButton();

                    return true;
                }

                // Submit changes to the PETRAServer
                try
                {
                    SubmissionResult = {#WEBCONNECTORMASTER}.Save{#MASTERTABLE}(ref SubmitDS, out VerificationResult);
                }
                catch (ESecurityDBTableAccessDeniedException Exp)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                    this.Cursor = Cursors.Default;

                    TMessages.MsgSecurityException(Exp, this.GetType());
                    
                    ReturnValue = false;
                    FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
                catch (EDBConcurrencyException Exp)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);

                    this.Cursor = Cursors.Default;

                    TMessages.MsgDBConcurrencyException(Exp, this.GetType());

                    ReturnValue = false;
                    FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
                catch (Exception)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                    this.Cursor = Cursors.Default;

                    FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));                    
                    throw;
                }

                switch (SubmissionResult)
                {
                    case TSubmitChangesResult.scrOK:

                        // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                        FMainDS.AcceptChanges();

                        // Merge back with data from the Server (eg. for getting Sequence values)
                        FMainDS.Merge(SubmitDS, false);

                        // need to accept the new modification ID
                        FMainDS.AcceptChanges();

                        // Update UI
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataSuccessful);
                        this.Cursor = Cursors.Default;

                        // We don't have unsaved changes anymore
                        FPetraUtilsObject.DisableSaveButton();

                        SetPrimaryKeyReadOnly(true);

                        ReturnValue = true;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));

                        if((VerificationResult != null)
                            && (VerificationResult.HasCriticalOrNonCriticalErrors))
                        {
                            TDataValidation.ProcessAnyDataValidationErrors(false, VerificationResult,
                                this.GetType(), null);
                        }

                        break;

                    case TSubmitChangesResult.scrError:
                        this.Cursor = Cursors.Default;
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataErrorOccured);

                        TDataValidation.ProcessAnyDataValidationErrors(false, VerificationResult,
                            this.GetType(), null);

                        FPetraUtilsObject.SubmitChangesContinue = false;

                        ReturnValue = false;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        break;

                    case TSubmitChangesResult.scrNothingToBeSaved:

                        this.Cursor = Cursors.Default;
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);

                        // We don't have unsaved changes anymore
                        FPetraUtilsObject.DisableSaveButton();
                        
                        ReturnValue = true;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        break;

                    case TSubmitChangesResult.scrInfoNeeded:

                        // TODO scrInfoNeeded
                        this.Cursor = Cursors.Default;
                        break;
                }
				
            	//The sorting will be affected when a new row is saved, so need to reselect row
                if (newRecordUnsavedInFocus)
	            {
	            	SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1);
					InvokeFocusedRowChanged(grdDetails.SelectedRowIndex());
				}
            }
            else
            {
                // Update UI
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);
                this.Cursor = Cursors.Default;
                FPetraUtilsObject.DisableSaveButton();

                // We don't have unsaved changes anymore
                FPetraUtilsObject.HasChanges = false;

                ReturnValue = true;
                FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
            }                
        }

        return ReturnValue;
    }
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        {#ACTIONENABLING}
        {#ACTIONENABLINGDISABLEMISSINGFUNCS}
    }

    {#ACTIONHANDLERS}

#endregion

#region Data Validation
    
    private void ControlValidatedHandler(object sender, EventArgs e)
    {
        TScreenVerificationResult SingleVerificationResult;
        
        ValidateAllData(true, false);
        
        FPetraUtilsObject.ValidationToolTip.RemoveAll();
        
        if (FPetraUtilsObject.VerificationResultCollection.Count > 0) 
        {
            for (int Counter = 0; Counter < FPetraUtilsObject.VerificationResultCollection.Count; Counter++) 
            {
                SingleVerificationResult = (TScreenVerificationResult)FPetraUtilsObject.VerificationResultCollection[Counter];
                
                if (SingleVerificationResult.ResultControl == sender) 
                {
                    if (FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested)
                    {
                        SingleVerificationResult.ResultControl.Focus();
                        FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = false;
                    }

                    FPetraUtilsObject.ValidationToolTipSeverity = SingleVerificationResult.ResultSeverity;

                    if (SingleVerificationResult.ResultTextCaption != String.Empty) 
                    {
                        FPetraUtilsObject.ValidationToolTip.ToolTipTitle += ":  " + SingleVerificationResult.ResultTextCaption;    
                    }
{#IFDEF UNDODATA}

                    if(SingleVerificationResult.ControlValueUndoRequested)
                    {
                        UndoData(SingleVerificationResult.ResultColumn.Table.Rows[0], SingleVerificationResult.ResultControl);
                        SingleVerificationResult.OverrideResultText(SingleVerificationResult.ResultText + Environment.NewLine + Environment.NewLine + 
                            Catalog.GetString("--> The value you entered has been changed back to what it was before! <--"));
                    }
{#ENDIF UNDODATA}

                    FPetraUtilsObject.ValidationToolTip.Show(SingleVerificationResult.ResultText, (Control)sender, 
                        ((Control)sender).Width / 2, ((Control)sender).Height);
                }
            }
        }
    }
{#IFDEF MASTERTABLE}
    private void ValidateData({#MASTERTABLE}Row ARow)
    {
        TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

        {#MASTERTABLE}Validation.Validate(this, ARow, ref VerificationResultCollection,
            FPetraUtilsObject.ValidationControlsDict);
    }
{#ENDIF MASTERTABLE}
{#IFDEF DETAILTABLE}
    private void ValidateDataDetails({#DETAILTABLE}Row ARow)
    {
        TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

        {#DETAILTABLE}Validation.Validate(this, ARow, ref VerificationResultCollection,
            FPetraUtilsObject.ValidationControlsDict);
    }
{#ENDIF DETAILTABLE}
{#IFDEF MASTERTABLE OR DETAILTABLE}

    private void BuildValidationControlsDict()
    {
        if (FMainDS != null)
        {
{#IFDEF ADDCONTROLTOVALIDATIONCONTROLSDICT}
            {#ADDCONTROLTOVALIDATIONCONTROLSDICT}
{#ENDIF ADDCONTROLTOVALIDATIONCONTROLSDICT}
        }
    }
{#ENDIF MASTERTABLE OR DETAILTABLE}    

#endregion
  }
}

{#INCLUDE copyvalues.cs}
{#INCLUDE validationcontrolsdict.cs}