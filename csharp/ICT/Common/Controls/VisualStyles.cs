﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 chadds
//		 ashleyc
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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Ict.Common.Controls
{
    /// <summary>
    /// All possible values that can be used to create a TVisualStyles Object
    /// </summary>
    public enum TVisualStylesEnum
    {
        /// <summary>Accordion Panel</summary>
        vsAccordionPanel,
        /// <summary>Task Panel</summary>
        vsTaskPanel,
        /// <summary>Dashboard</summary>
        vsDashboard,
        /// <summary>Shepherd/Assistant</summary>
        vsShepherd,
        /// <summary>Horizontal Collapse</summary>
        vsHorizontalCollapse
    }
    /// <summary>
    /// Object that defines values for different visual styles in OpenPetra
    /// </summary>
    public class TVisualStyles
    {
        //Font Variables
        private Font InternalTitleFont;
        /// <summary>The TitleFont property gets the value of InternalTitleFont</summary>
        /// <value>The TitleFont Property represents the Font type of the Title Font of the user control.</value>
        public Font TitleFont {
            get
            {
                return InternalTitleFont;
            }
        }

        private Font InternalContentFont;
        /// <summary>The ContentFont property gets the value of InternalContentFont</summary>
        /// <value>The ContentFont property represents the Font specifications of the ContentFont of the user control</value>
        public Font ContentFont {
            get
            {
                return InternalContentFont;
            }
        }

        private Color InternalTitleFontColour;
        /// <summary>The TitleFontColour gets the value of the InternalTitleFontColour </summary>
        /// <value> The TItleFontColour property represents the Colour specification of the TitleFont of the user control</value>
        public Color TitleFontColour {
            get
            {
                return InternalTitleFontColour;
            }
        }

        private Color InternalContentFontColour;
        /// <summary>The ContentFontColour property gets the value of the InternalContentFontColour</summary>
        /// <value> The ContentFontColour property represents the Colour specification of the Content text when the mouse is not hovering over the Content text</value>
        public Color ContentFontColour {
            get
            {
                return InternalContentFontColour;
            }
        }

        private Color InternalTitleFontColourHover;
        /// <summary>The TitleFontColourHover property gets the value of the InternalTitleFontColourHover</summary>
        /// <value> The TitleFontColourHover property represents the Colour specification of the title text when the mouse hovers over the title</value>
        public Color TitleFontColourHover {
            get
            {
                return InternalTitleFontColourHover;
            }
        }

        private Color InternalContentActivatedFontColour;
        /// <summary>The ContentActivatedFontColour property gets the value of the InternalContentActivatedFontColour</summary>
        /// <value> The ContentActivatedFontColour property represents the colour of activated content text</value>
        public Color ContentActivatedFontColour {
            get
            {
                return InternalContentActivatedFontColour;
            }
        }

        private Color InternalContentActivationEventFontColour;
        /// <summary>The ContentActivationEventFontColour property gets the value of the InternalContentActivationEventFontColour</summary>
        /// <value> The ContentActivationEventFontColour property represents the colour of activated content text while the activion is happening (i.e. in the moment when the 'click' for an activation happens)</value>
        public Color ContentActivationEventFontColour {
            get
            {
                return InternalContentActivationEventFontColour;
            }
        }

        private bool InternalContentActivatedFontUnderline = true;
        /// <summary>The ContentActivatedFontUnderline property gets the value of the InternalContentActivatedFontUnderline</summary>
        /// <value> The ContentActivatedFontUnderline property specifies whether an activated content text should be shown underlined, or not.</value>
        public bool ContentActivatedFontUnderline {
            get
            {
                return InternalContentActivatedFontUnderline;
            }
        }
        
        private int InternalTitlePaddingTop;
        /// <summary>The TitlePaddingTop property gets the value of the InternalTitlePaddingTop</summary>
        /// <value> The TitlePaddingTop property represents the padding in pixels that is added before the Title (from the top)</value>       
        public int TitlePaddingTop {
            get
            {
                return InternalTitlePaddingTop;
            }
        }
        
        private int InternalTitlePaddingLeft;
        /// <summary>The TitlePaddingLeft property gets the value of the InternalTitlePaddingLeft</summary>
        /// <value> The TitlePaddingLeft property represents the padding in pixels that is added before the Title (from the left)</value>       
        public int TitlePaddingLeft {
            get
            {
                return InternalTitlePaddingLeft;
            }
        }
        
        private int InternalTitlePaddingBottom;
        /// <summary>The TitlePaddingBottom property gets the value of the InternalTitlePaddingBottom</summary>
        /// <value> The TitlePaddingBottom property represents the padding in pixels that is added after the Title (at the bottom)</value>       
        public int TitlePaddingBottom {
            get
            {
                return InternalTitlePaddingBottom;
            }
        }
        
        private int InternalTitlePaddingRight;
        /// <summary>The TitlePaddingRight property gets the value of the InternalTitlePaddingRight</summary>
        /// <value> The TitlePaddingRight property represents the padding in pixels that is added after the Title (to the right)</value>       
        public int TitlePaddingRight {
            get
            {
                return InternalTitlePaddingRight;
            }
        }

        private int InternalContentPaddingTop;
        /// <summary>The ContentPaddingTop property gets the value of the InternalContentPaddingTop</summary>
        /// <value> The ContentPaddingTop property represents the padding in pixels that is added before the content (from the top)</value>       
        public int ContentPaddingTop {
            get
            {
                return InternalContentPaddingTop;
            }
        }
        
        private int InternalContentPaddingLeft;
        /// <summary>The ContentPaddingLeft property gets the value of the InternalContentPaddingLeft</summary>
        /// <value> The ContentPaddingLeft property represents the padding in pixels that is added before the content (from the left)</value>       
        public int ContentPaddingLeft {
            get
            {
                return InternalContentPaddingLeft;
            }
        }
        
        private int InternalContentPaddingBottom;
        /// <summary>The ContentPaddingBottom property gets the value of the InternalContentPaddingBottom</summary>
        /// <value> The ContentPaddingBottom property represents the padding in pixels that is added after the content (at the bottom)</value>       
        public int ContentPaddingBottom {
            get
            {
                return InternalContentPaddingBottom;
            }
        }
        
        private int InternalContentPaddingRight;
        /// <summary>The ContentPaddingRight property gets the value of the InternalContentPaddingRight</summary>
        /// <value> The ContentPaddingRight property represents the padding in pixels that is added after the content (to the right)</value>       
        public int ContentPaddingRight {
            get
            {
                return InternalContentPaddingRight;
            }
        }
        
        private Color InternalContentHoverFontColour;
        /// <summary>The ContentHoverFontColour property gets the value of the InternalContentHoverFontColour</summary>
        /// <value> The ContentHOverFontColour property represents the colour of the content text when the mouse hovers over it</value>
        public Color ContentHoverFontColour {
            get
            {
                return InternalContentHoverFontColour;
            }
        }

        //Background Variables
        private Color InternalContentBackgroundColour;
        /// <summary>The ContentBackgroundColour property gets the value of the InternalContentBackgroundColour </summary>
        /// <value> The ContentBackgroundColour property represents the content's background color.</value>
        public Color ContentBackgroundColour {
            get
            {
                return InternalContentBackgroundColour;
            }
        }

        private Color InternalTitleBackgroundColour;
        /// <summary>The TitleBackgroundColour property gets the value of the IntenralTitleBackgroundColour</summary>
        /// <value> The TItleBackgroundColour property represents the title's background colour</value>
        public Color TitleBackgroundColour {
            get
            {
                return InternalTitleBackgroundColour;
            }
        }

        private Color InternalContentHoverBackgroundColour;
        /// <summary>The ContentHoverBackgroundColour property gets the value of the InternalContentHoverBackgroundColour</summary>
        /// <value> The ContentHOoverBackgroundColour property represent the content's background colour when the mouse hovers over it</value>
        public Color ContentHoverBackgroundColour {
            get
            {
                return InternalContentHoverBackgroundColour;
            }
        }

        private Color InternalContentActivatedBackgroundColour;
        /// <summary>The ContentActivatedBackgroundColour property gets the value of the InternalContentAcivatedBackgroundColour </summary>
        /// <value> The ContentActivatedBackgroundCOlour property representf the content background colour when the content is activated </value>
        public Color ContentActivatedBackgroundColour {
            get
            {
                return InternalContentActivatedBackgroundColour;
            }
        }

        //Gradient Variables
        private Color InternalTitleGradientStart;
        /// <summary>The PanelGradientStart property gets the value of the InternalPanelGradientStart</summary>
        /// <value> The PanelGradientStart property represents the beginning colour of the panel gradient</value>
        public Color TitleGradientStart {
            get
            {
                return InternalTitleGradientStart;
            }
        }

        private Color InternalTitleGradientEnd;
        /// <summary>The PanelGradientEnd property gets the value of the InternalPanelGradientEnd</summary>
        /// <value> The PanelGradientEnd property represents the end colour of the title panel gradient.</value>
        public Color TitleGradientEnd {
            get
            {
                return InternalTitleGradientEnd;
            }
        }

        private LinearGradientMode InternalTitleGradientMode;
        /// <summary>The TitleGradientMode property gets the value of the InternalTitleGradientMode</summary>
        /// <value> The TitleGradientMode property represents the direction of the gradient in the title panel gradient</value>
        public LinearGradientMode TitleGradientMode {
            get
            {
                return InternalTitleGradientMode;
            }
        }

        private Color InternalContentGradientStart;
        /// <summary>The ContentGradientStart property gets the value of the InternalContentGradientstart</summary>
        /// <value> The ContentGradeientStart property represents the beginning colour of the content panel gradient</value>
        public Color ContentGradientStart {
            get
            {
                return InternalContentGradientStart;
            }
        }

        private Color InternalContentGradientEnd;
        /// <summary>The ContentGradientEnd property gets the value of the InternalContentGradientEnd</summary>
        /// <value> THe ContentGradientEnd property represents the end colour of the content panel gradient</value>
        public Color ContentGradientEnd {
            get
            {
                return InternalContentGradientEnd;
            }
        }

        private LinearGradientMode InternalContentGradientMode;
        /// <summary>The ContentGradientMode property gets the value of the InternalContentGradientMode</summary>
        /// <value> The ContentGradientMode property represents the direction of the gradient in the content panel gradient</value>
        public LinearGradientMode ContentGradientMode {
            get
            {
                return InternalContentGradientMode;
            }
        }

        //Layout Variables
        private int InternalTaskIndentation;
        /// <summary> The TaskIndentation property gets the value of the InternalTaskIndentation</summary>
        /// <value> The TaskIndentation property represents the number of spaces the tasks are indented in the content panel</value>
        public int TaskIndentation {
            get
            {
                return InternalTaskIndentation;
            }
        }

        private int InternalTaskHeight;
        /// <summary> The TaskHeight property gets the value of the InternalTaskHeight</summary>
        /// <value> The TaskHeight property represents the distance that tasks are vertically separated by in the content panel</value>
        public int TaskHeight {
            get
            {
                return InternalTaskHeight;
            }
        }
        
        private int InternalTitleHeightAdjustment;
        /// <summary> The TitleHeightAdjustment property gets the value of the InternalTitleHeightAdjustment</summary>
        /// <value> The TitleHeightAdjustment property allows adjustment of the height of the Title</value>
        public int TitleHeightAdjustment {
            get
            {
                return InternalTitleHeightAdjustment;
            }
        }        
        
        private bool InternalAutomaticNumbering;
        /// <summary>The AutomaticNumbering property gets the value of the InternalAutomaticNumbering</summary>
        /// <value> The AutomaticNumbering property determines if automatic numbering is on</value>
        public bool AutomaticNumbering {
            get
            {
                return InternalAutomaticNumbering;
            }
        }

        #region Variables for determining style in TaskList
        
        /// <summary>
        /// Boolean flag to specify whether the style uses a gradient background in the panel (title element)
        /// </summary>
        public bool UseTitleGradient;
        
        /// <summary>
        /// Boolean flag to specify whether certain controls will use the background colours
        /// </summary>
        public bool UseContentBackgroundColours;
        
        /// <summary>
        /// Boolean flag to specify whether the style uses a gradient background in the content section
        /// </summary>
        public bool UseContentGradient;

        #endregion
        
        #region Constructor

        /// <summary>
        /// Constructor to create a Visual Styles Object
        /// </summary>
        /// <param name="style">A TVisualStylesEnum Object</param>
        public TVisualStyles(TVisualStylesEnum style)
        {
            switch (style)
            {
                case TVisualStylesEnum.vsAccordionPanel:
                    //TitleFont Variables
                    InternalTitleFont = new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Bold);
                    InternalTitleFontColour = Color.FromArgb(21, 66, 139);
                    InternalTitleFontColourHover = Color.FromArgb(32, 101, 215);

                    //ContentFont Variables
                    InternalContentFont = new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Bold);
                    InternalContentFontColour = Color.FromArgb(21, 66, 139);
                    InternalContentHoverFontColour = Color.FromArgb(32, 101, 215);
                    InternalContentActivatedFontColour = InternalContentHoverFontColour;
                    InternalContentActivationEventFontColour = Color.FromArgb(255, 0, 0);

                    // Padding Variables
                    InternalTitlePaddingTop = 4;
                    InternalTitlePaddingLeft = 4;
                    InternalTitlePaddingRight = 8;
                    InternalTitlePaddingBottom = 0;
                    InternalContentPaddingTop = 8;
                    InternalContentPaddingLeft = 0;
                    InternalContentPaddingRight = 10;
                    InternalContentPaddingBottom = 5;
                    
                    //Gradient Variables
                    InternalTitleGradientStart = Color.FromArgb(205, 221, 242);
                    InternalTitleGradientEnd = Color.FromArgb(183, 202, 226);
                    InternalTitleGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                    InternalContentGradientStart = Color.FromArgb(212, 228, 254);
                    InternalContentGradientEnd = Color.FromArgb(205, 218, 254);
                    InternalContentGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;

                    //Layout Variables
                    InternalTaskIndentation = 20;
                    InternalTaskHeight = 18;
                    InternalTitleHeightAdjustment = 0;
                    InternalAutomaticNumbering = false;

                    //bool variables
                    UseTitleGradient = true;
                    UseContentGradient = true;
                    UseContentBackgroundColours = false;
                    break;
                    
                case TVisualStylesEnum.vsTaskPanel:
                    //TitleFont Variables
                    InternalTitleFont = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Bold);
                    InternalTitleFontColour = Color.FromArgb(30, 92, 196);
                    InternalTitleFontColourHover = Color.FromArgb(102, 134, 181);

                    //ContentFont Variables
                    InternalContentFont = new System.Drawing.Font("Verdana", 8);
                    InternalContentFontColour = Color.FromArgb(62, 117, 255);
                    InternalContentHoverFontColour = Color.FromArgb(90, 155, 252);
                    InternalContentActivatedFontColour = InternalContentFontColour;

                    // Padding Variables
                    InternalTitlePaddingTop = 6;
                    InternalTitlePaddingLeft = 10;
                    InternalTitlePaddingRight = 8;
                    InternalTitlePaddingBottom = 0;
                    InternalContentPaddingTop = 12;
                    InternalContentPaddingLeft = 0;
                    InternalContentPaddingRight = 5;
                    InternalContentPaddingBottom = 15;

                    //BackgroundVariables
                    InternalContentBackgroundColour = Color.FromArgb(212, 221, 249);
                    InternalTitleBackgroundColour = Color.FromArgb(212, 221, 249);

                    //Gradient Variables
                    InternalTitleGradientStart = Color.FromArgb(255, 255, 255);
                    InternalTitleGradientEnd = Color.FromArgb(190, 212, 254);
                    InternalTitleGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;

                    //Layout Variables
                    InternalTaskIndentation = 18;
                    InternalTaskHeight = 18;
                    InternalTitleHeightAdjustment = 3;
                    InternalAutomaticNumbering = false;

                    //bool variables
                    UseTitleGradient = true;
                    UseContentGradient = false;
                    UseContentBackgroundColours = false;
                    break;

                case TVisualStylesEnum.vsDashboard:
                    //TitleFont Variables
                    InternalTitleFont = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Bold);
                    InternalTitleFontColour = Color.FromArgb(0, 0, 160);
                    InternalTitleFontColourHover = Color.FromArgb(50, 94, 222);

                    //ContentFont Variables


                    // Padding Variables
                    InternalTitlePaddingTop = 6;
                    InternalTitlePaddingLeft = 2;
                    InternalTitlePaddingRight = 8;
                    InternalTitlePaddingBottom = 0;
                    InternalContentPaddingTop = 0;
                    InternalContentPaddingLeft = 0;
                    InternalContentPaddingRight = 0;
                    InternalContentPaddingBottom = 0;

                    //BackgroundVariables
                    InternalContentBackgroundColour = Color.FromArgb(176, 196, 222);
                    InternalTitleBackgroundColour = Color.FromArgb(176, 196, 222);

                    //Gradient Variables
                    InternalTitleGradientStart = Color.FromArgb(255, 255, 255);
                    InternalTitleGradientEnd = Color.FromArgb(190, 212, 254);
                    InternalTitleGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;

                    //Layout Variables
                    InternalTaskIndentation = 25;
                    InternalTaskHeight = 20;
                    InternalTitleHeightAdjustment = 3;
                    InternalAutomaticNumbering = false;

                    //bool variables
                    UseTitleGradient = false;
                    UseContentGradient = false;
                    UseContentBackgroundColours = false;

                    break;

                case TVisualStylesEnum.vsShepherd:
                    //TitleFont Variables
                    InternalTitleFont = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Bold);
                    InternalTitleFontColour = Color.FromArgb(0, 0, 0);
                    InternalTitleFontColourHover = Color.FromArgb(60, 60, 60);

                    //ContentFont Variables
                    InternalContentFont = new System.Drawing.Font("Verdana", 8);
                    InternalContentFontColour = Color.FromArgb(0, 0, 0);
                    InternalContentHoverFontColour = Color.FromArgb(60, 60, 60);
                    InternalContentActivatedFontColour = Color.FromArgb(255, 255, 255);
                    InternalContentActivationEventFontColour = Color.FromArgb(255, 0, 0);
                    InternalContentActivatedFontUnderline = false;
                    
                    // Padding Variables
                    InternalTitlePaddingTop = 5;
                    InternalTitlePaddingLeft = 5;
                    InternalTitlePaddingRight = 8;
                    InternalTitlePaddingBottom = 0;
                    InternalContentPaddingTop = 15;
                    InternalContentPaddingLeft = 0;
                    InternalContentPaddingRight = 5;
                    InternalContentPaddingBottom = 14;

                    //Gradient Variables
                    InternalTitleGradientStart = Color.FromArgb(255, 255, 255);
                    InternalTitleGradientEnd = Color.FromArgb(255, 255, 255);
                    InternalTitleGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;

                    //Background variables
                    InternalTitleBackgroundColour = Color.FromArgb(255, 255, 255);
                    InternalContentBackgroundColour = Color.FromArgb(255, 255, 255);
                    InternalContentHoverBackgroundColour = Color.FromArgb(255, 255, 255);  
                    InternalContentActivatedBackgroundColour = Color.FromArgb(0, 128, 192);


                    //Layout Variables
                    InternalTaskIndentation = 8;
                    InternalTaskHeight = 20;
                    InternalTitleHeightAdjustment = 2;
                    InternalAutomaticNumbering = true;

                    //bool variables
                    UseTitleGradient = false;
                    UseContentGradient = false;
                    UseContentBackgroundColours = true;
                    break;

                case TVisualStylesEnum.vsHorizontalCollapse:
                     //TitleFont Variables
                    InternalTitleFont = new System.Drawing.Font("Verdana", 13, System.Drawing.FontStyle.Bold);
                    InternalTitleFontColour = Color.FromArgb(21, 66, 139);
                    InternalTitleFontColourHover = Color.FromArgb(32, 101, 215);

                    //ContentFont Variables
                    InternalContentFont = new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Bold);
                    InternalContentFontColour = Color.FromArgb(21, 66, 139);
                    InternalContentHoverFontColour = Color.FromArgb(32, 101, 215);
                    InternalContentActivatedFontColour = InternalContentHoverFontColour;
                    InternalContentActivationEventFontColour = Color.FromArgb(255, 0, 0);

                    // Padding Variables
                    InternalTitlePaddingTop = 1;
                    InternalTitlePaddingLeft = 3;
                    InternalTitlePaddingRight = 8;
                    InternalTitlePaddingBottom = 0;
                    InternalContentPaddingTop = 8;
                    InternalContentPaddingLeft = 0;
                    InternalContentPaddingRight = 10;
                    InternalContentPaddingBottom = 5;
                    
                    //Gradient Variables
                    InternalTitleGradientStart = Color.FromArgb(255, 255, 255);
                    InternalTitleGradientEnd = Color.FromArgb(183, 202, 226);
                    InternalTitleGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                    InternalContentGradientStart = Color.FromArgb(212, 228, 254);
                    InternalContentGradientEnd = Color.FromArgb(205, 218, 254);
                    InternalContentGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;

                    //Layout Variables
                    InternalTaskIndentation = 30;
                    InternalTaskHeight = 18;
                    InternalTitleHeightAdjustment = 5;
                    InternalAutomaticNumbering = false;

                    //bool variables
                    UseTitleGradient = true;
                    UseContentGradient = true;
                    UseContentBackgroundColours = false;
                    break;
            
                default:
                    System.Windows.Forms.MessageBox.Show("TVisualStyles Constructor: no support for style '" + style.ToString("G") + "'");
                    break;
           }
        }
        
        #endregion
    }
}