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
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;

namespace Ict.Tools.NAntTasks
{
    /// <summary>
    /// create project files and solution files, in one go
    /// </summary>
    [TaskName("GenerateProjectFiles")]
    public class GenerateProjectFiles : NAnt.Core.Task
    {
        string DATE_TIME_STRING = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        private string FDependencyMapFilename = null;
        /// <summary>
        /// path of the file, where the dependancy map will be saved and read from
        /// </summary>
        [TaskAttribute("DependencyMapFilename", Required = true)]
        public string DependencyMapFilename {
            get
            {
                return FDependencyMapFilename;
            }
            set
            {
                FDependencyMapFilename = value;
            }
        }

        private string FGUIDMapFilename = null;
        /// <summary>
        /// path of the file, where the guids of the projects are stored
        /// </summary>
        [TaskAttribute("GUIDMapFilename", Required = true)]
        public string GUIDMapFilename {
            get
            {
                return FGUIDMapFilename;
            }
            set
            {
                FGUIDMapFilename = value;
            }
        }

        private string FCodeRootDir = null;
        /// <summary>
        /// should point to csharp directory
        /// </summary>
        [TaskAttribute("CodeRootDir", Required = true)]
        public string CodeRootDir {
            get
            {
                return FCodeRootDir;
            }
            set
            {
                FCodeRootDir = value;
            }
        }

        private string FTemplateDir = null;
        /// <summary>
        /// the directory that contains the template project files
        /// </summary>
        [TaskAttribute("TemplateDir", Required = true)]
        public string TemplateDir {
            get
            {
                return FTemplateDir;
            }
            set
            {
                FTemplateDir = value;
            }
        }

        private string FDevEnvironments = null;
        /// <summary>
        /// the list of IDEs that we should build the project files for
        /// </summary>
        [TaskAttribute("DevEnvironments", Required = true)]
        public string DevEnvironments {
            get
            {
                return FDevEnvironments;
            }
            set
            {
                FDevEnvironments = value;
            }
        }

        private string FDirBin = null;
        /// <summary>
        /// the directory that will contain the binary files
        /// </summary>
        [TaskAttribute("DirBin", Required = true)]
        public string DirBin {
            get
            {
                return FDirBin;
            }
            set
            {
                FDirBin = value;
            }
        }

        private string FNetFrameworkVersion = null;
        /// <summary>
        /// the .net framework version
        /// </summary>
        [TaskAttribute("NetFrameworkVersion", Required = true)]
        public string NetFrameworkVersion {
            get
            {
                return FNetFrameworkVersion;
            }
            set
            {
                FNetFrameworkVersion = value;
            }
        }

        private string FDirProjectFiles = null;
        /// <summary>
        /// where to put the generated project files
        /// </summary>
        [TaskAttribute("DirProjectFiles", Required = true)]
        public string DirProjectFiles {
            get
            {
                return FDirProjectFiles;
            }
            set
            {
                FDirProjectFiles = value;
            }
        }

        private Dictionary <string, string>FProjectGUIDs;
        private Dictionary <string, TDetailsOfDll>FProjectDependencies;

        /// <summary>
        /// create project files
        /// </summary>
        protected override void ExecuteTask()
        {
            ReadMap(FDependencyMapFilename, out FProjectDependencies);
            FProjectGUIDs = ReadProjectGUIDs(FGUIDMapFilename);

            string[] IDEs = FDevEnvironments.Split(new char[] { ',' });

            foreach (string ide in IDEs)
            {
                foreach (string projectName in FProjectDependencies.Keys)
                {
                    if (!Directory.Exists(FDirProjectFiles + Path.DirectorySeparatorChar + ide))
                    {
                        Directory.CreateDirectory(FDirProjectFiles + Path.DirectorySeparatorChar + ide);
                    }

                    string srcPath = FCodeRootDir + Path.DirectorySeparatorChar +
                                     projectName.
                                     Replace("Ict.Tools.", "ICT.PetraTools.").
                                     Replace("Ict.", "ICT.").
                                     Replace('.', Path.DirectorySeparatorChar);

                    string ProjectType = FProjectDependencies[projectName].OutputType;

                    string ProjectGUID = GetProjectGUID(projectName);

                    WriteProjectFile(
                        FTemplateDir,
                        ide.Trim(),
                        srcPath,
                        projectName,
                        ProjectType,
                        FProjectDependencies[projectName].ReferencedDlls,
                        ProjectGUID);
                }

                WriteSolutionFile(FTemplateDir, ide.Trim(),
                    "OpenPetra.sln",
                    "Ict.Common,Ict.Petra,Ict.Tools,Ict.Testing");
                WriteSolutionFile(FTemplateDir, ide.Trim(),
                    "OpenPetra.Server.sln",
                    "Ict.Common,Ict.Petra.Shared,Ict.Petra.Server,Ict.Petra.ServerPlugins,Ict.Petra.PetraServerConsole");
                WriteSolutionFile(FTemplateDir, ide.Trim(),
                    "OpenPetra.Client.sln",
                    "Ict.Common,Ict.Petra.Shared,Ict.Petra.Client,Ict.Petra.ClientPlugins,Ict.Petra.PetraClient");
                WriteSolutionFile(FTemplateDir, ide.Trim(),
                    "OpenPetra.Tools.sln",
                    "Ict.Common,Ict.Tools");
                WriteSolutionFile(FTemplateDir, ide.Trim(),
                    "OpenPetra.Testing.sln",
                    "Ict.Common,Ict.Petra,Ict.Testing");
            }

            WriteProjectGUIDs(FGUIDMapFilename, FProjectGUIDs);
        }

        Dictionary <string, string>FTemplateFiles = new Dictionary <string, string>();
        private StringBuilder GetTemplateFile(string filename)
        {
            if (!FTemplateFiles.ContainsKey(filename))
            {
                StreamReader sr = new StreamReader(filename);
                FTemplateFiles.Add(filename, sr.ReadToEnd());
                sr.Close();
            }

            return new StringBuilder(FTemplateFiles[filename]);
        }

        private void WriteSolutionFile(
            string ATemplateDir,
            string ADevName,
            string ASolutionFilename,
            string AIncludeNamespaces)
        {
            ATemplateDir += Path.DirectorySeparatorChar + ADevName + Path.DirectorySeparatorChar;
            StringBuilder template = GetTemplateFile(ATemplateDir + "template.sln");
            List <string>IncludeNamespaces = new List <string>(AIncludeNamespaces.Split(new char[] { ',' }));

            string Projects = string.Empty;
            string ProjectConfiguration = string.Empty;
            string SolutionFilename = FDirProjectFiles + Path.DirectorySeparatorChar + ADevName + Path.DirectorySeparatorChar + ASolutionFilename;

            foreach (string projectName in FProjectDependencies.Keys)
            {
                bool includeProject = false;

                foreach (string incNamespace in IncludeNamespaces)
                {
                    if (projectName.StartsWith(incNamespace))
                    {
                        includeProject = true;
                        break;
                    }
                }

                if (includeProject)
                {
                    StringBuilder temp = GetTemplateFile(ATemplateDir + "template.sln.project");
                    temp.Replace("${SolutionGuid}", GetProjectGUID(ASolutionFilename));
                    temp.Replace("${ProjectName}", projectName);
                    temp.Replace("${ProjectFile}",
                        FDirProjectFiles + Path.DirectorySeparatorChar + ADevName + Path.DirectorySeparatorChar + projectName + ".csproj");
                    temp.Replace("${ProjectGuid}", GetProjectGUID(projectName));
                    Projects += temp.ToString();

                    temp = GetTemplateFile(ATemplateDir + "template.sln.configuration");
                    temp.Replace("${ProjectGuid}", GetProjectGUID(projectName));
                    ProjectConfiguration += temp.ToString();
                }
            }

            template.Replace("${TemplateProject}", Projects);
            template.Replace("${TemplateConfiguration}", ProjectConfiguration);

            StreamWriter sw = new StreamWriter(SolutionFilename);
            sw.WriteLine(template.ToString());
            sw.Close();
        }

        /// get the relative path, that leads from the workingDirectory to the absolutePath
        public static string GetRelativePath(string absolutePath, string workingDirectory)
        {
            absolutePath = absolutePath.Replace("\\", "/");
            workingDirectory = workingDirectory.Replace("\\", "/");

            int countSame = 0;

            while (countSame < absolutePath.Length
                   && countSame < workingDirectory.Length
                   && absolutePath[countSame] == workingDirectory[countSame])
            {
                countSame++;
            }

            // go back to the last directory seperator
            countSame = absolutePath.Substring(0, countSame).LastIndexOf("/") + 1;
            string Result = absolutePath.Substring(countSame);

            if (countSame > 0)
            {
                // how many directories do we need to go up from the working Directory
                while (countSame < workingDirectory.Length)
                {
                    if (workingDirectory[countSame] == '/')
                    {
                        Result = "../" + Result;
                    }

                    countSame++;
                }
            }

            return Result;
        }

        private void WriteProjectFile(
            string ATemplateDir,
            string ADevName,
            string ASrcPath,
            string AProjectName,
            string AProjectType,
            List <string>AProjectDependencies,
            string AProjectGUID)
        {
            ATemplateDir += Path.DirectorySeparatorChar + ADevName + Path.DirectorySeparatorChar;
            StringBuilder template = GetTemplateFile(ATemplateDir + "template.csproj");

            // replace simple variables
            template.Replace("${ProjectGuid}", AProjectGUID);
            template.Replace("${OutputType}", AProjectType);
            template.Replace("${Namespace}", AProjectName);
            template.Replace("${NETframework-version}", FNetFrameworkVersion);
            template.Replace("${dir.bin}", FDirBin);
            template.Replace("${DebugStartArguments}", ""); // TODO? read from directory

            StringBuilder templateReference = GetTemplateFile(ATemplateDir + "template.csproj.reference");
            StringBuilder temp;

            // replace references
            StringBuilder ProjectReferences = new StringBuilder();
            StringBuilder OtherReferences = new StringBuilder();

            foreach (string referencedProject in AProjectDependencies)
            {
                // TODO referencenohint?
                if (!FProjectDependencies.ContainsKey(referencedProject))
                {
                    if (referencedProject.Contains("${csharpStdLibs}"))
                    {
                        temp = GetTemplateFile(ATemplateDir + "template.csproj.referencenohint");
                    }
                    else
                    {
                        temp = GetTemplateFile(ATemplateDir + "template.csproj.reference");
                    }

                    temp.Replace("${reference-name}", Path.GetFileNameWithoutExtension(referencedProject));
                    temp.Replace("${reference-path}", referencedProject.Replace('/', '\\'));
                    temp.Replace("${relative-reference-path}", referencedProject);
                    OtherReferences.Append(temp.ToString());
                }
                else
                {
                    temp = GetTemplateFile(ATemplateDir + "template.csproj.projectreference");
                    temp.Replace("${reference-project-file-name}", referencedProject + ".csproj");
                    temp.Replace("${relative-reference-project-file}", referencedProject + ".csproj");
                    temp.Replace("${relative-reference-path}", referencedProject + ".csproj");
                    temp.Replace(
                        "${reference-project-file}",
                        FDirProjectFiles + Path.DirectorySeparatorChar + ADevName + Path.DirectorySeparatorChar + referencedProject + ".csproj");
                    temp.Replace("${reference-project-guid}", GetProjectGUID(referencedProject));
                    temp.Replace("${reference-name}", referencedProject);
                    ProjectReferences.Append(temp.ToString());
                }
            }

            template.Replace("${TemplateProjectReferences}", ProjectReferences.ToString());

            template.Replace("${TemplateReferences}", OtherReferences.ToString());

            // TODO: create assembly info file

            StringBuilder CompileFile = new StringBuilder();

            List <string>ContainsFiles = new List <string>(Directory.GetFiles(ASrcPath, "*.cs", SearchOption.TopDirectoryOnly));

            foreach (string ContainedFile in ContainsFiles)
            {
                string relativeFilename = GetRelativePath(ContainedFile, ATemplateDir).Replace('\\', '/');
                string relativeFilenameBackslash = relativeFilename.Replace('/', '\\');

                if ((ContainedFile.EndsWith(".ManualCode.cs") && File.Exists(ContainedFile.Replace(".ManualCode.cs", "-generated.cs")))
                    || (ContainedFile.EndsWith(".Designer.cs") && File.Exists(ContainedFile.Replace(".Designer.cs", ".cs"))))
                {
                    // ignore and insert with the main file
                }
                else
                {
                    temp = GetTemplateFile(ATemplateDir + "template.csproj.compile");
                    temp.Replace("${filename}", ContainedFile);
                    temp.Replace("${relative-filename-backslash}", relativeFilenameBackslash);
                    temp.Replace("${relative-filename}", relativeFilename);
                    temp.Replace("${justfilename}", Path.GetFileName(ContainedFile));
                    CompileFile.Append(temp.ToString());

                    if (ContainsFiles.Contains(ContainedFile.Replace(".cs", ".Designer.cs")))
                    {
                        string OtherFile = ContainedFile.Replace(".cs", ".Designer.cs");

                        temp = GetTemplateFile(ATemplateDir + "template.csproj.compile.DependentUpon");
                        temp.Replace("${filename}", OtherFile);
                        temp.Replace("${relative-filename-backslash}", relativeFilenameBackslash.Replace(".cs", ".Designer.cs"));
                        temp.Replace("${relative-filename}", relativeFilename.Replace(".cs", ".Designer.cs"));
                        temp.Replace("${DependentUpon}", ContainedFile);
                        temp.Replace("${relative-DependentUpon}", Path.GetFileName(relativeFilename));
                        CompileFile.Append(temp.ToString());
                    }

                    if (ContainedFile.Contains("-generated.cs") && ContainsFiles.Contains(ContainedFile.Replace("-generated.cs", ".ManualCode.cs")))
                    {
                        string OtherFile = ContainedFile.Replace("-generated.cs", ".ManualCode.cs");

                        temp = GetTemplateFile(ATemplateDir + "template.csproj.compile.DependentUpon");
                        temp.Replace("${filename}", OtherFile);
                        temp.Replace("${relative-filename-backslash}", relativeFilenameBackslash.Replace("-generated.cs", ".ManualCode.cs"));
                        temp.Replace("${relative-filename}", relativeFilename.Replace("-generated.cs", ".ManualCode.cs"));
                        temp.Replace("${DependentUpon}", ContainedFile);
                        temp.Replace("${relative-DependentUpon}", Path.GetFileName(relativeFilename));
                        CompileFile.Append(temp.ToString());
                    }
                }
            }

            template.Replace("${TemplateCompile}", CompileFile.ToString());

            StringBuilder Resources = new StringBuilder();

            string[] ContainsResources = Directory.GetFiles(ASrcPath, "*.resx", SearchOption.TopDirectoryOnly);

            foreach (string ContainedFile in ContainsResources)
            {
                string relativeFilename = GetRelativePath(ContainedFile, ATemplateDir);
                string relativeFilenameBackslash = relativeFilename.Replace('/', '\\');

                if (ContainsFiles.Contains(ContainedFile.Replace(".resx", ".cs")))
                {
                    temp = GetTemplateFile(ATemplateDir + "template.csproj.resource.DependentUpon");
                    temp.Replace("${filename}", ContainedFile);
                    temp.Replace("${relative-filename-backslash}", relativeFilenameBackslash);
                    temp.Replace("${relative-filename}", relativeFilename);
                    temp.Replace("${DependentUpon}", ContainedFile.Replace(".resx", ".cs"));
                    temp.Replace("${relative-DependentUpon}", Path.GetFileName(relativeFilename.Replace(".resx", ".cs")));
                    Resources.Append(temp.ToString());
                }
                else
                {
                    temp = GetTemplateFile(ATemplateDir + "template.csproj.resource");
                    temp.Replace("${filename}", ContainedFile);
                    temp.Replace("${relative-filename-backslash}", relativeFilenameBackslash);
                    temp.Replace("${relative-filename}", relativeFilename);
                    Resources.Append(temp.ToString());
                }
            }

            template.Replace("${TemplateResource}", Resources.ToString());

            template.Replace("${dir.3rdParty}", FCodeRootDir + "\\ThirdParty");
            template.Replace("${csharpStdLibs}", ""); // TODO?

            string completedFile = template.ToString();

            string filename = FDirProjectFiles + Path.DirectorySeparatorChar + ADevName + Path.DirectorySeparatorChar + AProjectName + ".csproj";
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine(completedFile);
            sw.Close();

            if (completedFile.Contains("${"))
            {
                if (File.Exists(filename + ".error"))
                {
                    File.Delete(filename + ".error");
                }

                File.Move(filename, filename + ".error");
                throw new Exception("Template has not been filled in completely yet. See " + filename + ".error");
            }
        }

        string GetProjectGUID(string projectName)
        {
            if (!FProjectGUIDs.ContainsKey(projectName))
            {
                FProjectGUIDs.Add(projectName, "{" + Guid.NewGuid().ToString("D").ToUpper() + "}");
            }

            return FProjectGUIDs[projectName];
        }

        private Dictionary <string, string>ReadProjectGUIDs(string filename)
        {
            Dictionary <string, string>Result = new Dictionary <string, string>();

            if (!File.Exists(filename))
            {
                return Result;
            }

            StreamReader sr = new StreamReader(filename);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                if (line[0] != '#')
                {
                    string[] values = line.Split(new char[] { '=' });
                    Result.Add(values[0], values[1]);
                }
            }

            sr.Close();

            return Result;
        }

        private void WriteProjectGUIDs(string filename, Dictionary <string, string>guids)
        {
            StreamWriter sw = new StreamWriter(filename);

            sw.WriteLine("# Generated with GenerateProjectFiles at " + DATE_TIME_STRING);

            foreach (string key in guids.Keys)
            {
                sw.WriteLine(key + "=" + guids[key]);
            }

            sw.Close();
        }

        private void ReadMap(string filename, out Dictionary <string, TDetailsOfDll>map)
        {
            if (!File.Exists(filename))
            {
                throw new Exception("Cannot find file " + filename + ". Please first run nant generateNamespaceMap!");
            }

            StreamReader sr = new StreamReader(filename);

            map = new Dictionary <string, TDetailsOfDll>();
            TDetailsOfDll currentDll = null;

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                char firstChar = line[0];

                if (firstChar == ' ')
                {
                    currentDll.ReferencedDlls.Add(line.Substring(2));
                }
                else if (firstChar != '#')
                {
                    currentDll = new TDetailsOfDll();
                    string[] LineDetails = line.Split(new char[] { ',' });
                    currentDll.OutputType = LineDetails[1];
                    map.Add(LineDetails[0], currentDll);
                }
            }

            sr.Close();
        }
    }
}