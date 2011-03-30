﻿/*
 * Created by SharpDevelop.
 * User: Paul
 * Date: 2/21/2011
 * Time: 9:40 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Ict.Petra.Client.CommonForms.Logic; 
using NUnit.Framework;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using Ict.Common;
using Ict.Common.Data;
using System.Xml; 
using System.Collections.Generic; 
using Ict.Petra.Client.MPartner.Gui; 
	
namespace Tests.Shepherd
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	/// TODO: implement this class in the dve
	public class TestInterface : Ict.Petra.Client.CommonForms.Logic.IPetraShepherdConcreteFormInterface 
	{
		public TestInterface()
		{
		}
		///<summary>Update navigation buttons and navigation panel</summary>

			public void UpdateNavigation()
	        {
				System.Console.WriteLine("Updating the navigation buttons in the GUI.. ");
	        }
	        ///<summary>Displays the 'current' Shepherd Page and updates the navigation buttons and Navigation Panel</summary>
	        public void ShowCurrentPage()
	        {
	        	System.Console.WriteLine("Showing the current page in the GUI.. ");
	        }
	        ///<summary>Closes the Shepherd without any further ado and without saving</summary>
	        public void CancelShepherd()
	        {
	        	System.Console.WriteLine("Canceling the shepherd's GUI and logic.. ");
	        }
	        public void UpdateShepherdFormProperties(string AString, int width, int height)
	        {
	        	System.Console.WriteLine("The ShepherdFormProperties would have been updated if this wasn't a dummy interface."); 
	        }
 
		}
	}

