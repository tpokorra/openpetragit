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
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;
using NAnt.DotNet.Tasks;

namespace Ict.Tools.NAntTasks
{
    /// <summary>
    /// compile a solution file (in SharpDevelop4 format). the projects have already been put into the right order
    /// </summary>
    [TaskName("CompileSolution")]
    public class CompileSolution : NAnt.Core.Task
    {
        private string FSolutionFile = null;
        /// <summary>
        /// the SharpDevelop solution file that should be compiled
        /// </summary>
        [TaskAttribute("SolutionFile", Required = true)]
        public string SolutionFile {
            set
            {
                FSolutionFile = value;
            }
            get
            {
                return FSolutionFile;
            }
        }

        /// <summary>
        /// compile the solution
        /// </summary>
        protected override void ExecuteTask()
        {
            Console.WriteLine("compiling " + Path.GetFileNameWithoutExtension(FSolutionFile));

            StreamReader sr = new StreamReader(FSolutionFile);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                if (line.StartsWith("Project("))
                {
                    CompileProject compileProject = new CompileProject();
                    this.CopyTo(compileProject);

                    string[] projDef = line.Substring(line.IndexOf("=") + 1).Split(new char[] { ',' });
                    compileProject.CSProjFile = projDef[1].Trim().Trim(new char[] { '"' });

                    compileProject.Execute();
                }
            }
        }
    }
}