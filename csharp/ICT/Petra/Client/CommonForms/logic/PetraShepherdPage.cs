//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       pauln, AustinS
//
// Copyright 2004-2013 by OM International
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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using System;

using Ict.Common;
using Ict.Common.IO;

namespace Ict.Petra.Client.CommonForms.Logic
{
    /// <summary>
    /// Instance of a Shepherd Page (based on data from a Shepherd's YAML Definition file).
    /// </summary>
    public class TPetraShepherdPage
    {
        #region Fields

        /// <summary>Shepherd Page ID (=a unique ID for each individual Page).</summary>
        string FID;

        /// <summary>Shepherd Page Title.</summary>
        protected string FTitle;

        /// <summary>Shepherd Page Note.</summary>
        string FNote;

        /// <summary>Is the Shepherd Page visible?</summary>
        bool FVisible = true;

        /// <summary>Is the Shepherd Page enabled?</summary>
        bool FEnabled = true;

        /// <summary>Instance of the UserControl on the Shepherd Page.</summary>
        UserControl FUserControl = null;

        /// <summary>Namespace of the UserControl on the Shepherd Page.</summary>
        protected string FUserControlNamespace;

        /// <summary>Class Name of the UserControl on the Shepherd Page.</summary>
        protected string FUserControlClassName;

        /// <summary>Shepherd Help Context.</summary>
        protected string FHelpContext;

        /// <summary>Type of the UserControl on the Shepherd Page.</summary>
        string FUserControlType;

        /// <summary>Is the Shepherd Page the Finish page in the Shepherd?</summary>
        protected bool FIsLastPage = false;

        /// <summary>Is the Shepherd Page the First page in the Shepherd?</summary>
        bool FIsFirstPage = false;

        #endregion

        #region Properties

        /// <summary>Shepherd Page ID (=a unique ID for each individual Page).</summary>
        public string ID
        {
            get
            {
                return FID;
            }

            set
            {
                FID = value;
            }
        }

        /// <summary>Shepherd Page Title.</summary>
        public string Title
        {
            get
            {
                return FTitle;
            }
        }

        /// <summary>Shepherd Page Note.</summary>
        public string Note
        {
            get
            {
                return FNote;
            }

            set
            {
                FNote = value;
            }
        }

        /// <summary>Is the Shepherd Page visible?</summary>
        public bool Visible
        {
            get
            {
                return FVisible;
            }

            set
            {
                if (FVisible != value)
                {
                    FVisible = value;

                    OnVisibleOrEnabledChangedEvent();
                }
            }
        }

        ///<summary>Is the Shepherd Page enabled?</summary>
        public bool Enabled
        {
            get
            {
                return FEnabled;
            }
            set
            {
                if (FEnabled != value)
                {
                    FEnabled = value;

                    OnVisibleOrEnabledChangedEvent();
                }
            }
        }

        /// <summary>Namespace of the UserControl on the Shepherd Page.</summary>
        public string UserControlNamespace
        {
            get
            {
                return FUserControlNamespace;
            }
        }

        /// <summary>Class Name of the UserControl on the Shepherd Page.</summary>
        public string UserControlClassName
        {
            get
            {
                return FUserControlClassName;
            }
        }

        /// <summary>Shepherd Help Context.</summary>
        public string HelpContext
        {
            get
            {
                return FHelpContext;
            }

            set
            {
                FHelpContext = value;
            }
        }

        /// <summary>Type of the UserControl on the Shepherd Page.</summary>
        public string UserControlClassType
        {
            get
            {
                return FUserControlType;
            }
            set
            {
                FUserControlType = value;
            }
        }

        /// <summary>Is the Shepherd Page the first reachable page in the Shepherd?</summary>
        public bool IsFirstPage
        {
            get
            {
                return FIsFirstPage;
            }
            set
            {
                FIsFirstPage = value;
            }
        }

        /// <summary>Is the Shepherd Page the Finish page in the Shepherd?</summary>
        public bool IsLastPage
        {
            get
            {
                return FIsLastPage;
            }
            set
            {
                FIsLastPage = value;
            }
        }

        ///<summary>Instance of the UserControl on the Shepherd Page.</summary>
        public UserControl UserControlInstance
        {
            get
            {
                if (FUserControl == null)
                {
                    return RealiseUserControl();
                }
                else
                {
                    return FUserControl;
                }
            }
        }

        #endregion

        #region Events

        ///<summary>Raised if value of Visible or Enabled Property changes.</summary>
        public event System.EventHandler VisibleOrEnabledChangedEvent;

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor for TPetraShepherdPage.
        /// </summary>
        /// <remarks>Needed for inheritance only (<see cref="TPetraShepherdFinishPage" />).</remarks>
        protected TPetraShepherdPage()
        {
        }

        /// <summary>
        /// Constructor for individual Shepherd pages. Each time a page is constructed, it
        /// recieves information from the ShepherdPageNode and stores it in each of the
        /// variables.
        /// </summary>
        /// <remarks>Note that this Constructor is NOT called through inheritance by <see cref="TPetraShepherdFinishPage" />!</remarks>
        /// <param name="ShepherdPageNode">XmlNode that represents this Shepherd Page.</param>
        public TPetraShepherdPage(XmlNode ShepherdPageNode)
        {
            TLogging.Log("Constructor REACHED");

            FID = ShepherdPageNode.Attributes["ID"].Value;
            TLogging.Log("~~ID Assigned~~ " + FID);

            FTitle = ShepherdPageNode.Attributes["Title"].Value;
            TLogging.Log("~~Title Assigned~~ " + FTitle);

            FNote = ShepherdPageNode.Attributes["Note"].Value;
            TLogging.Log("~~Note Assigned~~ " + FNote);

            FVisible = System.Convert.ToBoolean(ShepherdPageNode.Attributes["Visible"].Value);
            TLogging.Log("~~Visible Assigned~~ " + System.Convert.ToString(FVisible));

            FEnabled = System.Convert.ToBoolean(ShepherdPageNode.Attributes["Enabled"].Value);
            TLogging.Log("~~Enabled Assigned~~ " + System.Convert.ToString(FEnabled));

            FUserControlNamespace = ShepherdPageNode.Attributes["UserControlNamespace"].Value;
            TLogging.Log("~~UserControlNamespace Assigned~~ " + FUserControlNamespace);

            FUserControlClassName = ShepherdPageNode.Attributes["UserControlClassName"].Value;
            TLogging.Log("~~UserControlClassName Assigned~~ " + FUserControlClassName);

            FHelpContext = ShepherdPageNode.Attributes["HelpContext"].Value;
            TLogging.Log("~~HelpContext Assigned~~ " + FHelpContext);

            //FUserControlType = ShepherdPageNode.Attributes["UserControlType"].Value;
            //TLogging.Log("~~UserControlType Assigned~~ ");

/*
 *          FIsLastPage=System.Convert.ToBoolean(ShepherdPageNode.Attributes["IsLastPage"].Value);
 *          TLogging.Log("~~IsLastPage Assigned~~ " + System.Convert.ToString(FIsLastPage));
 *
 *          FIsFirstPage=System.Convert.ToBoolean(ShepherdPageNode.Attributes["IsFirstPage"].Value);
 *          TLogging.Log("~~IsFirstPage Assigned~~ " + System.Convert.ToString(FIsFirstPage));
 */
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Function that instantiates and returns the UserControl needed for each Shepherd Page.
        /// That user control will then be displayed in the content of each page.
        /// </summary>
        /// <returns>Instantiated UserControl.</returns>
        private UserControl RealiseUserControl()
        {
            Assembly asm = Assembly.LoadFrom(FUserControlNamespace + ".dll");

            System.Type classType = asm.GetType(FUserControlNamespace + "." + FUserControlClassName);

            if (classType == null)
            {
                MessageBox.Show("TPnlCollapsible.RealiseUserControl: Cannot find class " + FUserControlNamespace + "." + FUserControlClassName);
            }

            FUserControl = (UserControl)Activator.CreateInstance(classType);

            //FUserControl.Controls.Add();

            TLogging.Log("PetraShepherdPage: The user control has been realised.");

            return FUserControl;
        }

        /// <summary>
        /// Fires the VisibleOrEnabledChangedEvent if something subscribed to it.
        /// </summary>
        private void OnVisibleOrEnabledChangedEvent()
        {
            if (VisibleOrEnabledChangedEvent != null)
            {
                VisibleOrEnabledChangedEvent(this, new System.EventArgs());
            }
        }

        #endregion
    }


    /// <summary>
    /// List of 0..n Shepherd Pages (based on data from a Shepherd's YAML Definition file).
    /// </summary>
    public class TPetraShepherdPagesList
    {
        #region Fields

        /// <summary>Dictionary containing a list of TPetraShepherdPage using the page's Unique ID as an identifier.</summary>
        Dictionary <string, TPetraShepherdPage>FPagesList = new Dictionary <string, TPetraShepherdPage>();

        #endregion

        #region Properties

        /// <summary>
        /// Allows for read-only access to a Dictionary of Pages.
        /// </summary>
        public Dictionary <string, TPetraShepherdPage>Pages
        {
            get
            {
                return FPagesList;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for TPetraShepherdPagesList. This function reads in a yaml file from the appropriate
        /// namespace, parses it into xmlNodes, and adds them to the list of pages so that they can be read
        /// by the individual ShepherdPage constructor, which it calls.
        /// </summary>
        /// <param name="AYamlFile">Full path to the Shepherd's YAML Definition file.</param>
        public TPetraShepherdPagesList(string AYamlFile)
        {
            TLogging.Log("Entering TPetraShepherdPagesList Constructor. AYamlFile = " + AYamlFile + "...");

            TYml2Xml parser = new TYml2Xml(AYamlFile);
            XmlDocument XmlPages = parser.ParseYML2XML();

            TLogging.Log("TPetraShepherdPagesList currently has this many nodes: " + XmlPages.LastChild.LastChild.LastChild.ChildNodes.Count);

            XmlNode temporaryXmlNode = XmlPages.LastChild.LastChild.LastChild.FirstChild;   //...LastChild.LastChild.LastChild.FirstChild is required because of the structure of the XML File after parsing.

            int counter = 0;

            XmlNodeList nodeList;
            XmlNode root = XmlPages.DocumentElement;
            nodeList = XmlPages.LastChild.LastChild.LastChild.ChildNodes;
            TLogging.Log("The amount of nodes in the nodeList in the TPetraShepherdPagesList constructor is as follows: " + nodeList.Count);

            foreach (XmlNode node in nodeList)
            {
                if (node.Name.Contains("SubShepherd."))
                {
                    TLogging.Log("TPetraSHepherdPagesList Constructor loop: Found a sub shepherd.. Skipping.. ");
                }
                else
                {
                    TPetraShepherdPage temporaryPetraShepherdPage = new TPetraShepherdPage(node);   // Constructor call for each page built off an XML node.

                    TLogging.Log("TPetraShepherdPagesList Constructor loop: THE TITLE OF THE CURRENT PAGE IS: " + temporaryPetraShepherdPage.Title);

                    FPagesList.Add(temporaryPetraShepherdPage.ID, temporaryPetraShepherdPage);
                }

                counter++;
            }

            TPetraShepherdFinishPage shepherdFinishPage = new TPetraShepherdFinishPage(XmlPages);
            TLogging.Log("Adding a shepherd finish page: " + shepherdFinishPage.ID);
            FPagesList.Add(shepherdFinishPage.ID, shepherdFinishPage);

            //Temporary Statement to add a subshepherd finish page in addition to the Finish page above
            TPetraShepherdFinishPage shepherdSubFinishPage = new TPetraShepherdFinishPage(XmlPages, "SubShepherd");
            TLogging.Log("Adding a shepherd sub-finish page: " + shepherdSubFinishPage.ID);
            FPagesList.Add(shepherdSubFinishPage.ID, shepherdSubFinishPage);

            TLogging.Log("TPetraShepherdPagesList Constructor ran successfully.");
        }

        #endregion
    }


    /// <summary>
    /// Specialisation of a Shepherd Page: Finish Page
    /// </summary>
    public class TPetraShepherdFinishPage : TPetraShepherdPage
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="XmlPages">TODO</param>
        public TPetraShepherdFinishPage(XmlDocument XmlPages)
        {
            base.ID = "FINISHPAGE_MASTER";
            Init(XmlPages);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="XmlPages">TODO</param>
        /// <param name="SubShepherdName">Name of the Sub-Shepherd.</param>
        public TPetraShepherdFinishPage(XmlDocument XmlPages, string SubShepherdName)
        {
            base.ID = "FINISHPAGE_CHILD_" + SubShepherdName.ToUpper();
            Init(XmlPages);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="XmlPages">TODO</param>
        /// <returns></returns>
        protected string GetFinishPageNote(XmlDocument XmlPages)
        {
            XmlNode FileElementData = XmlPages.DocumentElement;

            FileElementData = XmlPages.LastChild.LastChild;

            if (FileElementData.Attributes["FinishPageNote"] != null)
            {
                TLogging.Log("VALUE OF FINISH PAGE NOTE: " + FileElementData.Attributes["FinishPageNote"].Value);
                return FileElementData.Attributes["FinishPageNote"].Value;
            }
            else
            {
                return "Choose \'Finish\' to commit the data.";
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="XmlPages">TODO</param>
        private void Init(XmlDocument XmlPages)
        {
            base.Enabled = true;
            base.Visible = true;
            base.FIsLastPage = false;
            base.FUserControlClassName = "TUC_PetraShepherdFinishPage";
            base.FUserControlNamespace = "Ict.Petra.Client.CommonForms";
            base.FTitle = "Here is a summary of the information you have provided:" + base.ID;
            base.Note = GetFinishPageNote(XmlPages);
        }

        #endregion
    }
}