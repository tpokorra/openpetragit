﻿//
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
using Ict.Common.Controls;
using System.Windows.Forms;
using NUnit.Extensions.Forms;

namespace Ict.Testing.NUnitForms
{
    /// <summary>
    /// test for our own combobox
    /// </summary>
    public class TTxtNumericTextBoxTester : ControlTester <TTxtNumericTextBox, TTxtNumericTextBoxTester>
    {
        /// constructor
        public TTxtNumericTextBoxTester()
        {
        }

        /// constructor
        public TTxtNumericTextBoxTester(string name, Form form) : base(name, form)
        {
        }

        /// constructor
        public TTxtNumericTextBoxTester(string name, string formName) : base(name, formName)
        {
        }

        /// constructor
        public TTxtNumericTextBoxTester(string name) : base(name)
        {
        }

        /// constructor
        public TTxtNumericTextBoxTester(TTxtNumericTextBoxTester tester, int index) : base(tester, index)
        {
        }

        /// <summary>
        /// access the properties of the auto populated combobox
        /// </summary>
        public new TTxtNumericTextBox Properties
        {
            get
            {
                return (TTxtNumericTextBox) base.TheObject;
            }
        }
    }
}