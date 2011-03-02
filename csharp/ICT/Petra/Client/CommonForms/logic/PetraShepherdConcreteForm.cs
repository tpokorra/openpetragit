//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK, tomn, pauln
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
//this is a test
using System;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.Collections.Generic; //another small change

using Ict.Common;
/// <summary>
/// this is a temporary change..
/// </summary>
namespace Ict.Petra.Client.CommonForms.Logic
{
    ///<summary>Logic class for the internal behaviour of a Shepherd</summary>
    public class TPetraShepherdFormLogic : object
    {
        private const string STARTPAGE_NAME = "|||START PAGE|||";
        private const string FINISHPAGE_NAME = "|||FINISH PAGE|||";

        /// <summary>Holds a typed list of 0..n TPetraShepherdPage's</summary>
        private TPetraShepherdPagesList FShepherdPages;
        /// <summary>Holds the instance of the Shepherd Form management class</summary>
        private IPetraShepherdConcreteFormInterface FForm;
        /// <summary>Holds the instance of the current shepherd page</summary>
        private TPetraShepherdPage FCurrentPage;
        /// <summary>'Blackboard' for exchanging data between shepherd pages which isn't stored in the DB</summary>
        private SortedList FPagesDataHeap;

        ///<summary>List of Shepherd Pages</summary>
        public TPetraShepherdPagesList ShepherdPages
        {
            /// <summary>
            /// Read in XML nodes from YAML file and call TShepherdPage constructor
            /// with the particular XML node needed for that page.
            /// </summary>
            get
            {
                TLogging.Log("TPetraShepherdPagesList called get method in the attribute.");
                return FShepherdPages;
            }
        }

        ///<summary>Currently displayed Shepherd Page</summary>
        public TPetraShepherdPage CurrentPage
        {
            get
            {
                return FCurrentPage;
            }
            set
            {
                FCurrentPage = value;
            }
        }

        ///<summary>Constructor</summary>
        public TPetraShepherdFormLogic(string AYamlFile, IPetraShepherdConcreteFormInterface APetraShepherdForm)
        {
            TLogging.Log(
                "Entering TPetraShepherdFormLogic Constructor. AYamlFile = " + AYamlFile + "; APetraShepherdForm = " +
                APetraShepherdForm.ToString() +
                "...");

            FForm = APetraShepherdForm;

            // Take AYamlFile and parse it into an XmlNode structure


            FShepherdPages = new TPetraShepherdPagesList(AYamlFile);

            SwitchToStartPage();
            TLogging.Log("The TPetraShepherdFormLogic constructor has switched to the first page.");

            // Iterate over all FPetraShepherdPages and add the VisibleOrEnabledChangedEventHandler

            // FShepherdPages needs to get added an auto-generated TPetraShepherdFinishPage
            // for the Finish Page (that is not specified in the YAML file!)
            // Note: That Finish Page (and only this) will have IsLastPage = true!!!


            TLogging.Log(
                "TPetraShepherdFormLogic Constructor ran and returned to the TPetraShepherdFormLogic constructor in PetraShepherdConcreteForm.");
        }

        ///<summary>Returns an instance of a Page UserControl</summary>
        protected UserControl InstantiatePageUC(string AID)
        {
            TLogging.Log("SwitchToPage (in TPetraShepherdFormLogic) was called for Page UserControl '" + AID + "'");

            return null;
        }

        ///<summary>Switches the current page</summary>
        protected void SwitchToPage(string APage)
        {
            TLogging.Log("SwitchToPage (in TPetraShepherdFormLogic) was called for Page '" + APage + "'");
            // ....
            CurrentPage = FShepherdPages.Pages[APage];
            TLogging.Log("PetraShepherdConcreteForm: SwitchToPage -- Page number = " + CurrentPage.ID);

            try
            {
                FForm.ShowCurrentPage();
            }
            catch (Exception e)
            {
                //This line always throws an exception during the first run of the code;
                //I don't know if that's okay, but we should probably resolve it.
                TLogging.Log("The exception for ShowCurrentPage() in PetraShepherdConcreteForm was caught.");
                TLogging.Log(e.Message);
            }
        }

        /// <summary>
        /// Switches to the first page
        /// Iterates through FShepeherdPages.Pages to find the first page that is both visible and enabled.
        /// </summary>
        protected void SwitchToStartPage()
        {
            //Should switch to start page set the isFirstPage attribute in the ShepherdPages dictionary to true?
            TLogging.Log("SwitchToStartPage (in TPetrashepherdFormLogic)");

            string startPage = "";             //temporary string to hold the key of the StartPage

            foreach (KeyValuePair <string, TPetraShepherdPage>pair in FShepherdPages.Pages)
            {
                if (pair.Value.Visible && pair.Value.Enabled)
                {
                    TLogging.Log("SwitchToStartPage foreach loop returned the following value that was both visible and enabled: " + pair.Key);
                    startPage = pair.Key;
                    pair.Value.IsFirstPage = true; 
                    CurrentPage = pair.Value;
                    break;
                }
            }

            TLogging.Log("temporary page was assigned to " + startPage + " in SwitchToStartPage.");
            try
            {
                SwitchToPage(startPage);
            }
            catch (KeyNotFoundException ex)
            {
                TLogging.Log("KeyNotFoundException Thrown in SwitchToStartPage when SwitchToPage(startPage) was called.");
            }
        }

        ///<summary>Switches the Finish page</summary>
        protected void SwitchToFinishPage()
        {
            TLogging.Log("SwitchToFinishPage (in TPetraShepherdFormLogic)");

            SwitchToPage(FINISHPAGE_NAME);
        }

        ///<summary>Switches to the 'next' page (whatever page this is)</summary>
        public virtual void HandleActionNext()
        {
            TLogging.Log("HandleActionNext (in TPetraShepherdFormLogic)");

            string nextPage = "";     //temporary string to hold the key of the StartPage
            bool hasPassedCurrentPage = false;     // used to tell if the iteration has already checked to see if you have passed the current page.
            bool hasPassedItAgain = false;

            foreach (KeyValuePair <string, TPetraShepherdPage>pair in FShepherdPages.Pages)
            {
                if (hasPassedCurrentPage)         //TODO: there has to be a better way to handle iterating through the loop one more time; it works now, but is ugly.
                {
                    hasPassedItAgain = true;
                }

                if (pair.Key == CurrentPage.ID)
                {
                    TLogging.Log("Found the equivilance.");
                    hasPassedCurrentPage = true;
                    TLogging.Log("Set the hasPassedCurrentPage bool to true.");
                }

                if (pair.Value.Visible && pair.Value.Enabled && hasPassedItAgain)
                {
                    TLogging.Log("Switchtonextpage foreach loop returned the following value that was both visible and enabled: " + pair.Key);
                    nextPage = pair.Key;
                    CurrentPage = pair.Value;
                    break;
                }
            }

            TLogging.Log("temporary next page was assigned to " + nextPage + " in HandleActionNext().");

            try // rather than a try/catch statement, the next button should instead be greyed out
            {
                SwitchToPage(nextPage);
            }
            catch (KeyNotFoundException e)
            {
                TLogging.Log("KeyNotFoundException Thrown at HandleActionNext when SwitchToPage(nextPage) was called.");
            }
        }

        ///<summary>Switches to the 'previous' page (whatever page this is)</summary>
        public virtual void HandleActionBack()
        {
            TLogging.Log("HandleActionBack (in TPetraShepherdFormLogic)");

            string backPage = ""; //temporary string to hold the key of the StartPage
            bool hasPassedCurrentPage = false;     // used to tell if the iteration has already checked to see if you have passed the current page.
            TPetraShepherdPage temporaryPage = CurrentPage;
            int counter = 0;

            foreach (KeyValuePair <string, TPetraShepherdPage>pair in FShepherdPages.Pages)
            {
            	if(pair.Value == CurrentPage)
            	{
            		backPage = temporaryPage.ID; 
            		break; 
            		TLogging.Log("Set the backpage to the following: " + temporaryPage.ID); 
            	}
            	temporaryPage = pair.Value; 
            	counter++; 
            }
			backPage = temporaryPage.ID; 
           	SwitchToPage(backPage);
        }

        ///<summary>Causes to close the Shepherd without saving if the user chooses to do that</summary>
        public void HandleActionCancel()
        {
            TLogging.Log("HandleActionCancel");
            // Show Message Box with Yes, No, Cancel options

            // If YES ->
            FForm.CancelShepherd();
        }

        ///<summary>Shows the context-sensitive help file content for the current page, if available, otherwise for the Shepherd as a whole</summary>
        public void HandleActionHelp()
        {
            TLogging.Log("HandleActionHelp");
        }

        ///<summary>Switches to the auto-generated 'Finish' page.</summary>
        public void HandleActionFinish()
        {
            TLogging.Log("HandleActionFinish");
            // .....

            SwitchToFinishPage();
        }

        ///<summary>Determines the 'First' page (whatever page this is).</summary>
        private void VisibleOrEnabledChangedEventHandler()
        {
            TLogging.Log("VisibleOrEnabledChangedEventHandler");

            // re-enumerate FShepherdPages!

            FForm.UpdateNavigation();
        }

        /// <summary>Looks up a key in the PagesDataHeap and returns its value, or null if it doesn't exist</summary>
        public string PagesDataHeapPeek(string AKey)
        {
            TLogging.Log("VisibleOrEnabledChangedEventHandler");

            return String.Empty;
        }

        /// <summary>Sets a key-value pair in the PagesDataHeap</summary>
        public void PagesDataHeapPoke(string AKey, string AValue)
        {
            TLogging.Log("PagesDataHeapPoke");
        }
    }
}