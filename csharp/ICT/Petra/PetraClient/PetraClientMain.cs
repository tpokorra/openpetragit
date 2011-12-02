//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Windows.Forms;
using System.Globalization;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner; // Implicit reference
using Ict.Petra.Shared.Security;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.MConference.Gui;
using Ict.Petra.Client.MPartner.Gui;
using SplashScreen;
using PetraClientShutdown;

namespace Ict.Petra.Client.App.PetraClient
{
    /// <summary>
    /// Main Unit for the Petra Client application.
    ///
    /// Startup of the application begins here.
    /// </summary>
    public class TPetraClientMain
    {
//        /// <summary>Email Address is invalid.</summary>
//        [ErrCodeAttribute("Email Address is invalid.",
//                          FullDescription = "The email address entered is not a valid date.")]
//        public const String ERR_EMAILADDRESSINVALID = "GENC.00007V";

        private static TSplashScreenManager FSplashScreen;

        /// <summary>tells whether the Login was successful, or not</summary>
        private static Boolean FLoginSuccessful;

        private static Boolean FUseNewNavigation = true;

        /// <summary>ProcessID (unique) assigned by the PetraServer</summary>
        private static Int32 FProcessID;

        /// <summary>Welcome message (passed on to the MainWindow)</summary>
        private static String FWelcomeMessage;

        /// <summary>Tells whether the Petra System is enabled, or not (passed on to the MainWindow)</summary>
        private static Boolean FSystemEnabled;
        private static TLogging FLogging;

        [DllImport("user32.dll")] private static extern int FindWindow(string classname, string windowname);
        [DllImport("user32.dll")] private static extern int SendMessage(
            int hWnd,                    // handle to destination window
            uint Msg,                     // message
            long wParam,                // first message parameter
            long lParam                 // second message parameter
            );

        /// <summary>
        /// Loads read-only Client settings (from .NET Configuration File and Command
        /// Line)
        ///
        /// </summary>
        /// <returns>true if settings could be loaded, otherwise false.
        /// </returns>
        public static bool LoadClientSettings()
        {
            try
            {
                new TClientSettings();
            }
            catch (Exception e)
            {
                FSplashScreen.ShowMessageBox(e.Message, "Failure");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Display the Login Dialog, and get the permissions of the user that just has logged in
        /// </summary>
        /// <returns>void</returns>
        public static void PerformLogin()
        {
            try
            {
                TLoginForm AConnectDialog;

                // TODO: close current connection if it is open
                FLoginSuccessful = false;

                // Need to show and hide Connect Dialog before closing the Splash Screen so that it can receive input focus!
                AConnectDialog = new TLoginForm();
                AConnectDialog.Show();
                AConnectDialog.Visible = false;

                // Close Splash Screen
                FSplashScreen.Close();
                FSplashScreen = null;

                // Show the Connect Dialog
                if (AConnectDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    FLoginSuccessful = false;
                }
                else
                {
                    // TODO reset any caches
                    AConnectDialog.GetReturnedParameters(out FProcessID, out FWelcomeMessage, out FSystemEnabled);

                    // get Connection Dialog out of memory
                    AConnectDialog.Dispose();

                    FLoginSuccessful = true;

                    Ict.Petra.Client.MSysMan.Gui.TFrmMaintainLanguageCulture.InitLanguageAndCulture();
                }
            }
            catch (Exception exp)
            {
                if (FSplashScreen == null)
                {
                    MessageBox.Show("Exception caught in Method PerformLogin: " + exp.ToString());
                }
                else
                {
                    FSplashScreen.ShowMessageBox("Exception caught in Method PerformLogin: " + exp.ToString());
                }
            }
        }

        /// <summary>
        /// start the Petra server
        /// </summary>
        /// <returns></returns>
        public static Boolean StartServer()
        {
            System.Diagnostics.Process PetraServerProcess;

            // start the Petra server (e.g. c:\petra2\bin22\PetraServerConsole.exe C:C:\petra2\ServerStandalone.config RunWithoutMenu:true
            try
            {
                FSplashScreen.ProgressText = Catalog.GetString("Starting OpenPetra Server...");
                PetraServerProcess = new System.Diagnostics.Process();
                PetraServerProcess.EnableRaisingEvents = false;
                PetraServerProcess.StartInfo.FileName = TClientSettings.Petra_Path_Bin + "/PetraServerConsole.exe";
                PetraServerProcess.StartInfo.WorkingDirectory = TClientSettings.Petra_Path_Bin;
                PetraServerProcess.StartInfo.Arguments = "-C:\"" + TClientSettings.PetraServer_Configfile + "\" -RunWithoutMenu:true";
                PetraServerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                PetraServerProcess.EnableRaisingEvents = false;

                if (!PetraServerProcess.Start())
                {
#if TESTMODE
                    TLogging.Log("failed to start " + PetraServerProcess.StartInfo.FileName);
#endif
#if  TESTMODE
#else
                    FSplashScreen.ShowMessageBox("failed to start " + PetraServerProcess.StartInfo.FileName);
#endif
                    return false;
                }
            }
            catch (Exception exp)
            {
#if TESTMODE
                TLogging.Log("Exception while starting PetraServer process: " + exp.ToString());
#endif
#if  TESTMODE
#else
                FSplashScreen.ShowMessageBox("Exception while starting OpenPetra Server process: " + exp.ToString());
#endif
                return false;
            }
#if TODO
            // We can't minimize the command window for the PostgreSql server directly after starting.
            // So we do it here.
            MinimizePostgreSqlWindow();
#endif
            return true;
        }

        /// <summary>
        /// Gets called from the Splash Screen to provide information about the Petra installation.
        /// </summary>
        /// <returns>void</returns>
        private static void SplashScreenInfoCallback(out string APetraVersion, out string AInstallationKind, out string ACustomText)
        {
            APetraVersion = TClientInfo.ClientAssemblyVersion;
            AInstallationKind = TClientInfo.InstallationKind;
            ACustomText = TClientSettings.CustomStartupMessage;
        }

        /// <summary>
        /// this is usually only used for remote clients; standalone clients are patched with a windows installer program
        /// </summary>
        private static void CheckForPatches()
        {
            FSplashScreen.ProgressText = "Running checks that are specific to Remote Installation...";

            // todo: check whether the user has SYSADMIN rights; should not be required
            // todo: check whether the user has write access to the bin directory
            // check whether the user has access to the server and the Petra patches directory
            if ((TClientSettings.Petra_Path_RemotePatches.Length > 0)
                && !(TClientSettings.Petra_Path_RemotePatches.ToLower().StartsWith("http://")
                     || TClientSettings.Petra_Path_RemotePatches.ToLower().StartsWith("https://"))
                && !System.IO.Directory.Exists(TClientSettings.Petra_Path_RemotePatches))
            {
                FSplashScreen.ShowMessageBox(
                    String.Format(
                        Catalog.GetString(
                            "Please make sure that you have logged in to your network drive\nand can access the directory\n{0}\nIf this is the case and you still get this message,\nyou might use an IP address rather than a hostname for the server.\nPlease ask your local System Administrator for help."),
                        TClientSettings.Petra_Path_RemotePatches),
                    Catalog.GetString("Cannot check for patches"));
            }

            // check whether there is a patch available; if this is a remote version, try to download a patch from the server
            TPatchTools patchTools = new TPatchTools(Path.GetFullPath(TClientSettings.Petra_Path_Bin + Path.DirectorySeparatorChar + ".."),
                "30",
                TClientSettings.PathTemp,
                "",
                "",
                TClientSettings.Petra_Path_Patches,
                TClientSettings.Petra_Path_RemotePatches);

            string PatchStatusMessage;

            // TODO: run this only if necessary. seem adding cost centre does not update the cache?
            TDataCache.ClearAllCaches();

            if (patchTools.CheckForRecentPatch(false, out PatchStatusMessage))
            {
                // todo: display a list of all patches that will be installed? or confusing with different builds?
                if (FSplashScreen.ShowMessageBox(String.Format(Catalog.GetString("There is a new patch available: {0}" +
                                ".\r\nThe currently installed version is {1}" +
                                ".\r\nThe patch will be installed to directory '{2}'.\r\nDo you want to install now?"),
                            patchTools.GetLatestPatchVersion(), patchTools.GetCurrentPatchVersion(), TClientSettings.Petra_Path_Bin),
                        String.Format(Catalog.GetString("Install new OpenPetra patch")), MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    // reset the caches in IsolatedStorage. This can help if things have changed drastically in the database
                    // TODO: run this also after the software has been reinstalled with the InnoSetup installer? Remember the current patch number in the IsolatedStorage?
                    TDataCache.ClearAllCaches();

                    // create the temp directory; using the Petra tmp directory, so that we don't need to change the drive in the batch file
                    string TempPath = TClientSettings.PathTemp + Path.DirectorySeparatorChar + "petrapatch";
                    Directory.CreateDirectory(TempPath);

                    // check for newer patchtool
                    patchTools.CopyLatestPatchProgram(TempPath);

                    // need to stop petra client, start the patch in temppath, restart Petra client
                    Process PatchProcess = new System.Diagnostics.Process();
                    PatchProcess.EnableRaisingEvents = false;
                    PatchProcess.StartInfo.FileName = TempPath + Path.DirectorySeparatorChar + "PatchTool.exe";
                    PatchProcess.StartInfo.Arguments = "-action:patchRemote" + " -C:\"" + Path.GetFullPath(TClientSettings.ConfigurationFile) +
                                                       "\" -OpenPetra.Path:\"" + Path.GetFullPath(
                        TClientSettings.Petra_Path_Bin + Path.DirectorySeparatorChar + "..") +
                                                       "\" -OpenPetra.Path.Bin:\"" + Path.GetFullPath(
                        TClientSettings.Petra_Path_Bin) + "\"";
                    PatchProcess.Start();

                    // Application stops here !!!
                    Environment.Exit(0);
                }
            }
            else
            {
                if (PatchStatusMessage != String.Empty)
                {
                    FSplashScreen.ShowMessageBox(PatchStatusMessage, "");
                }
            }
        }

        /// <summary>
        /// start the client
        /// </summary>
        public static void StartUp()
        {
            ExceptionHandling.GApplicationShutdownCallback = Shutdown.SaveUserDefaultsAndDisconnectAndStop;

            FLogging = new TLogging(TClientSettings.GetPathTemp() + Path.DirectorySeparatorChar + "PetraClient.log");

            Catalog.Init();

//            ErrorCodeInventory.BuildErrorCodeInventory(new Ict.Common.CommonErrorCodes().GetType());
//            ErrorCodeInventory.BuildErrorCodeInventory(new Ict.Petra.Shared.PetraErrorCodes().GetType());
//            ErrorCodeInventory.BuildErrorCodeInventory(new Ict.Common.Verification.TStringChecks().GetType());

            ErrorCodeInventory.RegisteredTypes.Add(new Ict.Petra.Shared.PetraErrorCodes().GetType());

//            System.Windows.Forms.MessageBox.Show(ErrorCodes.GetErrorInfo("GENC.00001V").ShortDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00001V").FullDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00001V").Category.ToString("G") + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00001V").HelpID);
//            System.Windows.Forms.MessageBox.Show(ErrorCodes.GetErrorInfo("GENC.00002V").ShortDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00002V").FullDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00002V").ErrorMessageText + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00002V").ErrorMessageTitle + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00002V").Category.ToString("G") + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00002V").HelpID);
//            System.Windows.Forms.MessageBox.Show(ErrorCodes.GetErrorInfo("GEN.00004E").ShortDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GEN.00004E").FullDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GEN.00004E").Category.ToString("G") + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GEN.00004E").HelpID);
//            System.Windows.Forms.MessageBox.Show(ErrorCodes.GetErrorInfo("PARTN.00005V").ShortDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("PARTN.00005V").FullDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("PARTN.00005V").Category.ToString("G") + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("PARTN.00005V").HelpID);


//MessageBox.Show(ErrorCodes.GetErrorInfo(ERR_EMAILADDRESSINVALID).ShortDescription);

            // TODO another Catalog.Init("org", "./locale") for organisation specific words?

            /* Show Splash Screen.
             * This is non-blocking since it is done in a separate Thread, that means
             * that the startup procedure continues while the Splash Screen is initialised and shown!!! */
            FSplashScreen = new TSplashScreenManager(new TSplashScreenCallback(SplashScreenInfoCallback));
            Shutdown.SplashScreen = FSplashScreen;
            FSplashScreen.Show();

            /*
             * IMPORTANT: Always use FSplashScreen.ShowMessageBox instead of MessageBox.Show
             * as long as the Splash Screen is displayed to show the MessageBox on the correct
             * Thread and in front of the Splash Screen!!!
             */

            try
            {
                InitialiseClasses();
            }
            catch (Exception e)
            {
                FSplashScreen.Close();
                MessageBox.Show(e.ToString());
                Shutdown.StopPetraClient();
            }

            if (!LoadClientSettings())
            {
                Environment.Exit(0);
            }

            /*
             * Specific information about this Petra installation can only be shown in the
             * Splash Screen after Client settings are loaded (done in LoadClientSettings).
             */
            FSplashScreen.UpdateTexts();

            // only do automatic patch installation on remote situation
            // needs to be done before login, because the login connects to the updated server, and could go wrong because of changed interfaces
            if (TClientSettings.RunAsRemote == true)
            {
                try
                {
                    CheckForPatches();
                }
                catch (Exception e)
                {
                    TLogging.Log("Problem during checking for patches: " + e.Message);
                    TLogging.Log(e.StackTrace);
                }
            }

            if (TClientSettings.RunAsStandalone == true)
            {
                FSplashScreen.ProgressText = "Starting OpenPetra Server Environment...";

                if (!StartServer())
                {
                    Environment.Exit(0);
                }
            }

            FSplashScreen.ProgressText = "Connecting to your OpenPetra.org Server...";

            /*
             * Show Petra Login screen.
             * Connections to PetraServer are established in here as well.
             */
            try
            {
                PerformLogin();
            }
            catch (Exception)
            {
#if TESTMODE
                // in Testmode, if no connection to applink, just stop here
                Environment.Exit(0);
#endif
#if  TESTMODE
#else
                throw;
#endif
            }

            if (FLoginSuccessful)
            {
                try
                {
                    if (TClientSettings.RunAsStandalone == true)
                    {
                        ProcessReminders.StartStandaloneRemindersProcessing();
                    }

                    // This loads the Main Window of Petra
                    Form MainWindow;

                    if (FUseNewNavigation)
                    {
                        MainWindow = new TFrmMainWindowNew(IntPtr.Zero);
                    }
                    else
                    {
                        MainWindow = new TFrmMainWindow(IntPtr.Zero);
                    }

                    // TODO: user defined constructor with more details
                    //                    FProcessID, FWelcomeMessage, FSystemEnabled);

                    Application.Run(MainWindow);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    TLogging.Log(e.Message);
                    TLogging.Log(e.StackTrace);
                }
                finally
                {
                    /*
                     * This code gets executed only after the Main Window of Petra has
                     * closed.
                     * At the moment, we will never get here, since we call Environment.Exit in the MainWindow (both old and new navigation)
                     */
                    Shutdown.SaveUserDefaultsAndDisconnect();
                }
            }
            else
            {
                // No successful login

                // APPLICATION STOPS IN THIS PROCEDURE !!!
                Shutdown.StopPetraClient();
            }

            // APPLICATION STOPS IN THIS PROCEDURE !!!
            Shutdown.StopPetraClient();
        }

#if TODO
// TODO: should this go to the server side? will we have Postgresql at all on the client?
        /// <summary>
        /// Start the PostgreSql database.
        /// If user has admin rights then start as a service.
        /// If user has no admin rights then start as an executible.
        /// </summary>
        /// <returns>true if startup was successful</returns>
        private static bool StartPostgreSqlServer()
        {
            System.Diagnostics.Process PostgreSqlServerProcess;

            if (TClientSettings.RunAsStandalone)
            {
                // start the PostgreSql server as exe(e.g. c:\Program Files\Postgres\8.3\bin\pg_ctl.exe -D C:\petra2\db23_pg start
                try
                {
                    FSplashScreen.ProgressText = "Starting PostgreSql Server...";
                    PostgreSqlServerProcess = new System.Diagnostics.Process();
                    PostgreSqlServerProcess.StartInfo.FileName = "\"" + TClientSettings.PostgreSql_BaseDir + "\\bin\\pg_ctl.exe\"";
                    PostgreSqlServerProcess.StartInfo.Arguments = "-D " + TClientSettings.PostgreSql_DataDir + " start";
                    PostgreSqlServerProcess.StartInfo.WorkingDirectory = TClientSettings.PostgreSql_DataDir;
                    PostgreSqlServerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    PostgreSqlServerProcess.EnableRaisingEvents = false;
                    PostgreSqlServerProcess.StartInfo.UseShellExecute = false;

                    System.Security.SecureString MyPassword = new System.Security.SecureString();
                    String Pwd = "petra";

                    foreach (char c in Pwd)
                    {
                        MyPassword.AppendChar(c);
                    }

                    PostgreSqlServerProcess.StartInfo.Password = MyPassword;
                    PostgreSqlServerProcess.StartInfo.UserName = "petrapostgresqluser";

                    if (PostgreSqlServerProcess.Start())
                    {
                    }
                    else
                    {
#if TESTMODE
                        TLogging.Log("failed to start " + PostgreSqlServerProcess.StartInfo.FileName);
#endif
#if  TESTMODE
#else
                        FSplashScreen.ShowMessageBox("failed to start " + PostgreSqlServerProcess.StartInfo.FileName);
#endif
                        return false;
                    }
                }
                catch (Exception exp)
                {
#if TESTMODE
                    TLogging.Log("Exception while starting PostgreSql process: " + exp.ToString());
#endif
#if  TESTMODE
#else
                    FSplashScreen.ShowMessageBox("Exception while starting PostgreSql process: " + exp.ToString());
#endif
                    return false;
                }
                PostgreSqlServerProcess.WaitForExit(20000);

                return true;
            }

            return true;
        }
#endif



        /// <summary>
        /// Checks if the current user has Administrator rights
        /// </summary>
        /// <returns>true if the user has Admin rights</returns>
        public static bool HasAdministratorPrivileges()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Perform necessary initializations of Classes
        /// </summary>
        /// <returns>void</returns>
        private static void InitialiseClasses()
        {
            TClientInfo.InitializeUnit();
            TCacheableTablesManager.InitializeUnit();

            // Set up Delegates for forwarding of calls to Screens in various Assemblies
            TCommonScreensForwarding.OpenPartnerFindScreen = @TPartnerFindScreenManager.OpenModalForm;
            TCommonScreensForwarding.OpenConferenceFindScreen = @TConferenceFindScreenManager.OpenModalForm;

            // I18N: assign proper font which helps to read asian characters
            // this is the first place where it is called, and we need to initialize the TAppSettingsManager
            TAppSettingsManager.InitFontI18N();

            // to avoid dependency cycle, we need to add module windows this way
            TFrmPetraModuleUtils.AddModuleWindow(typeof(Ict.Petra.Client.MPartner.Gui.TFrmPartnerMain));
            TFrmPetraModuleUtils.AddModuleWindow(typeof(Ict.Petra.Client.MFinance.Gui.TFrmFinanceMain));
            TFrmPetraModuleUtils.AddModuleWindow(typeof(Ict.Petra.Client.MPersonnel.Gui.TFrmPersonnelMain));
        }

        /// <summary>
        /// Minimize the command window that starts the PostgreSql database.
        /// </summary>
        private static void MinimizePostgreSqlWindow()
        {
            int wHandle = 0;

            String WindowName = TClientSettings.PostgreSql_BaseDir + "\\bin\\pg_ctl.exe";

            wHandle = FindWindow(null, WindowName);

            if (wHandle > 0)
            {
                // 0xF020 is the WM_MINIMIZE message
                SendMessage(wHandle, 0x0112, 0xF020, 0);
            }
        }
    }
}