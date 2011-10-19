//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
//
using System;
using System.Collections;
using System.Collections.Specialized;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.DataStore
{
    /// <summary>
    /// tools for the code generation
    /// </summary>
    public class CodeGenerationPetra
    {
        /// <summary>
        /// convert an sql type from our xml file into a dotnet type.
        /// should also work for Convert.To&lt;DotNetType&gt;
        /// </summary>
        /// <param name="tableField"></param>
        /// <returns></returns>
        public static string ToDotNetType(TTableField tableField)
        {
            return tableField.GetDotNetType();
        }

        /// convert the type from the xml file to an ODBC type
        public static string ToOdbcTypeString(TTableField tableField)
        {
            if ((tableField.strType == "number") && (tableField.iLength == 24))
            {
                // currency valuse
                return "OdbcType.Decimal";
            }
            else if ((tableField.strType == "number") || ((tableField.strTypeDotNet != null) && tableField.strTypeDotNet.ToLower().Contains("int64")))
            {
                return "OdbcType.Decimal";
            }
            else if ((tableField.strType == "varchar") || ((tableField.strTypeDotNet != null) && tableField.strTypeDotNet.ToLower().Contains("string")))
            {
                return "OdbcType.VarChar";
            }
            else if ((tableField.strType == "bit") || ((tableField.strTypeDotNet != null) && tableField.strTypeDotNet.ToLower().Contains("bool")))
            {
                return "OdbcType.Bit";
            }
            else if ((tableField.strType == "date") || ((tableField.strTypeDotNet != null) && tableField.strTypeDotNet.ToLower().Contains("datetime")))
            {
                return "OdbcType.Date";
            }
            else if (tableField.strType == "integer")
            {
                return "OdbcType.Int";
            }
            else
            {
                return "OdbcType.Int";
            }
        }
    }
}