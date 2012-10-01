﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Xml;
using System.Drawing;
using System.Windows.Forms;

using Ict.Common.Controls;

namespace ControlTestBench
{
/// <summary>
/// Description of CollapsiblePanelHosterTest.
/// </summary>
public partial class CollapsiblePanelHosterTest : Form
{
    XmlNode FTestYAMLNode = null;
    TVisualStylesEnum FEnumStyle = TVisualStylesEnum.vsTaskPanel;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public CollapsiblePanelHosterTest()
    {
        //
        // The InitializeComponent() call is required for Windows Forms designer support.
        //
        InitializeComponent();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ATestYAMLNode"></param>
    /// <param name="AEnumStyle"></param>
    public CollapsiblePanelHosterTest(XmlNode ATestYAMLNode, TVisualStylesEnum AEnumStyle) : this()
    {
        FTestYAMLNode = ATestYAMLNode;
        FEnumStyle = AEnumStyle;
        TPnlCollapsibleHoster CollPanelHoster;
        
        CollPanelHoster = new TPnlCollapsibleHoster(FTestYAMLNode, FEnumStyle);
        CollPanelHoster.DistanceBetweenCollapsiblePanels = 25;
        CollPanelHoster.Dock = DockStyle.Fill;
        pnlCollapsiblePanelHostTest.Controls.Add(CollPanelHoster);
        
        CollPanelHoster.RealiseCollapsiblePanelsNow();
    }       
}
}