﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Ict.Tools.DevelopersAssistant
{
    /****************************************************************************************************************************************
     *
     * The main window class for openPetra Developer's Assistant
     *
     * *************************************************************************************************************************************/

    /// <summary>
    /// The main window class foir the application
    /// </summary>
    public partial class MainForm : Form
    {
        private SettingsDictionary _localSettings = null;                                   // Our settings persisted locally between sessions
        private bool _serverIsRunning = false;                                              // Local variable holds server state
        private List <NantTask.TaskItem>_sequence = new List <NantTask.TaskItem>();         // List of tasks in the standard sequence
        private List <NantTask.TaskItem>_altSequence = new List <NantTask.TaskItem>();      // List of tasks in the alternate sequence
        private List <OutputText.ErrorItem>_warnings = new List <OutputText.ErrorItem>();   // List of positions/severities in verbose text where warnings/errors appear
        private int _currentWarning = -1;                                                   // 'Current' warning ID in _warnings list

        /**************************************************************************************************************************************
         *
         * Initialisation and GUI state routines
         *
         * ***********************************************************************************************************************************/

        /// <summary>
        /// Constructor for the class
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, @"OM_International\DevelopersAssistant.ini");
            string appVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
            _localSettings = new SettingsDictionary(path, appVersion);
            _localSettings.Load();

            PopulateCombos();

            this.Text = Program.APP_TITLE;
            cboCodeGeneration.SelectedIndex = _localSettings.CodeGenerationComboID;
            cboCompilation.SelectedIndex = _localSettings.CompilationComboID;
            cboMiscellaneous.SelectedIndex = _localSettings.MiscellaneousComboID;
            cboDatabase.SelectedIndex = _localSettings.DatabaseComboID;
            chkAutoStartServer.Checked = _localSettings.AutoStartServer;
            chkAutoStopServer.Checked = _localSettings.AutoStopServer;
            chkMinimizeServer.Checked = _localSettings.MinimiseServerAtStartup;
            chkTreatWarningsAsErrors.Checked = _localSettings.TreatWarningsAsErrors;
            txtBranchLocation.Text = _localSettings.BranchLocation;
            txtYAMLPath.Text = _localSettings.YAMLLocation;
            txtFlashAfterSeconds.Text = _localSettings.FlashAfterSeconds.ToString();
            txtBazaarPath.Text = _localSettings.BazaarPath;
            ValidateBazaarPath();

            _sequence = ConvertStringToSequenceList(_localSettings.Sequence);
            _altSequence = ConvertStringToSequenceList(_localSettings.AltSequence);
            ShowSequence(txtSequence, _sequence);
            ShowSequence(txtAltSequence, _altSequence);
            lblVersion.Text = "Version " + appVersion;

            SetBranchDependencies();

            GetServerState();

            SetEnabledStates();

            SetToolTips();
        }

        private void SetBranchDependencies()
        {
            lblBranchLocation.Text = (txtBranchLocation.Text == String.Empty) ? "Not defined" : txtBranchLocation.Text;

            BuildConfiguration dbCfg = new BuildConfiguration(txtBranchLocation.Text, _localSettings);

            if (txtBranchLocation.Text == String.Empty)
            {
                lblDbBuildConfig.Text = "";
            }
            else
            {
                lblDbBuildConfig.Text = dbCfg.CurrentConfig;
            }

            dbCfg.ListAllConfigs(listDbBuildConfig);
            btnRemoveDbBuildConfig.Enabled = listDbBuildConfig.Items.Count > 0;
            btnEditDbBuildConfig.Enabled = listDbBuildConfig.Items.Count > 0;
            btnSaveDbBuildConfig.Enabled = listDbBuildConfig.Items.Count > 0 && txtBranchLocation.Text != String.Empty;

            if (txtBranchLocation.Text == String.Empty)
            {
                txtAutoLogonUser.Text = String.Empty;
                txtAutoLogonPW.Text = String.Empty;
                txtAutoLogonAction.Text = String.Empty;
                chkUseAutoLogon.Checked = false;
                chkUseAutoLogon.Enabled = false;
            }
            else
            {
                ClientAutoLogOn calo = new ClientAutoLogOn(txtBranchLocation.Text);
                txtAutoLogonUser.Text = calo.UserName;
                txtAutoLogonPW.Text = calo.Password;
                txtAutoLogonAction.Text = calo.TestAction.Replace(",", "\r\n");
                chkUseAutoLogon.Checked = (txtAutoLogonUser.Text != String.Empty);
                chkUseAutoLogon.Enabled = true;
            }
        }

        private void SetToolTips()
        {
            toolTip.SetToolTip(btnDatabase, toolTip.GetToolTip(btnDatabase) + Environment.NewLine + "Shortcut: Ctrl + D");
            toolTip.SetToolTip(btnCodeGeneration, toolTip.GetToolTip(btnCodeGeneration) + Environment.NewLine + "Shortcut: Ctrl + G");
            toolTip.SetToolTip(btnCompilation, toolTip.GetToolTip(btnCompilation) + Environment.NewLine + "Shortcut: Ctrl + I");
            toolTip.SetToolTip(btnMiscellaneous, toolTip.GetToolTip(btnMiscellaneous) + Environment.NewLine + "Shortcut: Ctrl + M");
            toolTip.SetToolTip(btnStartClient, toolTip.GetToolTip(btnStartClient) + Environment.NewLine + "Shortcut: Ctrl + O");
            toolTip.SetToolTip(linkLabelRestartServer, toolTip.GetToolTip(linkLabelRestartServer) + Environment.NewLine + "Shortcut: Ctrl + R");
            toolTip.SetToolTip(linkLabelStartServer, toolTip.GetToolTip(linkLabelStartServer) + Environment.NewLine + "Shortcut: Ctrl + S");
            toolTip.SetToolTip(btnGenerateWinform, toolTip.GetToolTip(btnGenerateWinform) + Environment.NewLine + "Shortcut: Ctrl + Y");
            toolTip.SetToolTip(linkLabelBazaar, toolTip.GetToolTip(linkLabelBazaar) + Environment.NewLine + "Shortcut: Ctrl + Z");
        }

        private void PopulateCombos()
        {
            // Code generation
            for (NantTask.TaskItem i = NantTask.FirstCodeGenItem; i <= NantTask.LastCodeGenItem; i++)
            {
                NantTask t = new NantTask(i);
                cboCodeGeneration.Items.Add(t.ShortDescription);
            }

            // Compile
            for (NantTask.TaskItem i = NantTask.FirstCompileItem; i <= NantTask.LastCompileItem; i++)
            {
                NantTask t = new NantTask(i);
                cboCompilation.Items.Add(t.ShortDescription);
            }

            // Misc
            for (NantTask.TaskItem i = NantTask.FirstMiscItem; i <= NantTask.LastMiscItem; i++)
            {
                NantTask t = new NantTask(i);
                cboMiscellaneous.Items.Add(t.ShortDescription);
            }

            // Database
            for (NantTask.TaskItem i = NantTask.FirstDatabaseItem; i <= NantTask.LastDatabaseItem; i++)
            {
                NantTask t = new NantTask(i);
                cboDatabase.Items.Add(t.ShortDescription);
            }
        }

        private void SetEnabledStates()
        {
            bool bGotBranch = txtBranchLocation.Text != String.Empty;

            linkLabelStartServer.Enabled = bGotBranch && !_serverIsRunning;
            linkLabelStopServer.Enabled = bGotBranch && _serverIsRunning;
            linkLabelRestartServer.Enabled = bGotBranch && _serverIsRunning;
            linkLabelYamlFile.Enabled = bGotBranch;

            btnGenerateWinform.Enabled = bGotBranch && txtYAMLPath.Text != String.Empty;
            btnCodeGeneration.Enabled = bGotBranch;
            btnCompilation.Enabled = bGotBranch;
            btnMiscellaneous.Enabled = bGotBranch;
            btnDatabase.Enabled = bGotBranch;
            btnStartClient.Enabled = bGotBranch;
            btnRunSequence.Enabled = bGotBranch && txtSequence.Text != String.Empty;
            btnRunAltSequence.Enabled = bGotBranch && txtAltSequence.Text != String.Empty;
            btnResetClientConfig.Enabled = bGotBranch;
            btnUpdateMyClientConfig.Enabled = bGotBranch;
        }

        private void SetWarningButtons()
        {
            btnPrevWarning.Enabled = (_warnings.Count > 0 && chkVerbose.Checked);
            btnNextWarning.Enabled = (_warnings.Count > 0 && chkVerbose.Checked);
        }

        private void GetServerState()
        {
            _serverIsRunning = NantExecutor.IsServerRunning();
        }

        private List <NantTask.TaskItem>ConvertStringToSequenceList(string s)
        {
            List <NantTask.TaskItem>list = new List <NantTask.TaskItem>();

            if (s != String.Empty)
            {
                string[] items = s.Split(';');

                for (int i = 0; i < items.Length; i++)
                {
                    list.Add((NantTask.TaskItem)Convert.ToInt32(items[i]));
                }
            }

            return list;
        }

        private string ConvertSequenceListToString(List <NantTask.TaskItem>list)
        {
            string s = "";

            for (int i = 0; i < list.Count; i++)
            {
                if (s != String.Empty)
                {
                    s += "; ";
                }

                s += ((int)list[i]).ToString();
            }

            return s;
        }

        private void ShowSequence(TextBox tb, List <NantTask.TaskItem>sequence)
        {
            string s = "";

            for (int i = 0; i < sequence.Count; i++)
            {
                if (s != String.Empty)
                {
                    s += "\r\n";
                }

                NantTask task = new NantTask(sequence[i]);
                s += task.Description;
            }

            tb.Text = s;
        }

        /************************************************************************************************************************
         *
         * GUI event handlers
         *
         * *********************************************************************************************************************/

        private void linkLabelBranchLocation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            dlg.ShowNewFolderButton = false;
            dlg.Description = "Choose a new location of your working branch";
            dlg.RootFolder = Environment.SpecialFolder.MyComputer;

            if (txtBranchLocation.Text != String.Empty)
            {
                dlg.SelectedPath = txtBranchLocation.Text;
            }

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            if (String.Compare(dlg.SelectedPath, txtBranchLocation.Text, true) == 0)
            {
                return;                                                                         // no change
            }

            if (!File.Exists(dlg.SelectedPath + @"\OpenPetra.Build"))
            {
                MessageBox.Show(
                    "The location that you have chosen is not a recognised OpenPetra source folder.  The file 'OpenPetra.Build' could not be found.",
                    Program.APP_TITLE,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            txtBranchLocation.Text = dlg.SelectedPath;

            SetBranchDependencies();
            SetEnabledStates();
        }

        private void linkLabelYamlFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = "yaml";
            dlg.AddExtension = true;
            dlg.Filter = "YAML files|*.yaml|All files|*.*";
            dlg.InitialDirectory = txtBranchLocation.Text + "\\csharp\\ICT\\Petra\\Client";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            string s = dlg.FileName;
            int p = s.IndexOf("\\Petra\\Client\\", 0, StringComparison.InvariantCultureIgnoreCase);
            txtYAMLPath.Text = s.Substring(p + 14);

            SetEnabledStates();
        }

        private void chkCompileWinform_CheckedChanged(object sender, EventArgs e)
        {
            chkStartClientAfterGenerateWinform.Enabled = chkCompileWinform.Checked;
            chkStartClientAfterGenerateWinform.Checked = chkCompileWinform.Checked;
        }

        private void linkModifySequence_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DlgSequence dlg = new DlgSequence();

            dlg.InitializeList(_sequence);

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            _sequence = dlg.ExitSequence;
            ShowSequence(txtSequence, _sequence);
            SetEnabledStates();
        }

        private void linkModifyAltSequence_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DlgSequence dlg = new DlgSequence();

            dlg.InitializeList(_altSequence);

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            _altSequence = dlg.ExitSequence;
            ShowSequence(txtAltSequence, _altSequence);
            SetEnabledStates();
        }

        private void chkVerbose_CheckedChanged(object sender, EventArgs e)
        {
            txtOutput.Text = chkVerbose.Checked ? OutputText.VerboseOutput : OutputText.ConciseOutput;
            SetWarningButtons();
            lblWarnings.Text = GetWarningDisplayText();
        }

        private void btnNextWarning_Click(object sender, EventArgs e)
        {
            _currentWarning++;

            if (_currentWarning >= _warnings.Count)
            {
                _currentWarning = 0;
            }

            txtOutput.Focus();
            txtOutput.SelectionStart = _warnings[_currentWarning].Position;
            txtOutput.SelectionLength = _warnings[_currentWarning].SelLength;
            txtOutput.ScrollToCaret();
            lblWarnings.Text = GetWarningDisplayText();
            SetWarningButtons();
        }

        private void btnPrevWarning_Click(object sender, EventArgs e)
        {
            _currentWarning--;

            if (_currentWarning < 0)
            {
                _currentWarning = _warnings.Count - 1;
            }

            txtOutput.Focus();
            txtOutput.SelectionStart = _warnings[_currentWarning].Position;
            txtOutput.SelectionLength = _warnings[_currentWarning].SelLength;
            txtOutput.ScrollToCaret();
            lblWarnings.Text = GetWarningDisplayText();
            SetWarningButtons();
        }

        private void btnAddDbBuildConfig_Click(object sender, EventArgs e)
        {
            DlgDbBuildConfig dlg = new DlgDbBuildConfig();

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            BuildConfiguration dbCfg = new BuildConfiguration(txtBranchLocation.Text, _localSettings);
            dbCfg.AddConfig(dlg.ExitData);
            SetBranchDependencies();
            listDbBuildConfig.SelectedIndex = listDbBuildConfig.Items.Count - 1;
        }

        private void btnRemoveDbBuildConfig_Click(object sender, EventArgs e)
        {
            int index = listDbBuildConfig.SelectedIndex;

            if (index < 0)
            {
                return;
            }

            string msg = "This will remove the selected item from the saved configurations list.  Are you sure?";

            if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            BuildConfiguration dbCfg = new BuildConfiguration(txtBranchLocation.Text, _localSettings);
            dbCfg.RemoveConfig(index);
            SetBranchDependencies();

            if ((--index < 0) && (listDbBuildConfig.Items.Count > 0))
            {
                index = 0;
            }

            listDbBuildConfig.SelectedIndex = index;
        }

        private void btnEditDbBuildConfig_Click(object sender, EventArgs e)
        {
            int index = listDbBuildConfig.SelectedIndex;

            if (index < 0)
            {
                return;
            }

            DlgDbBuildConfig dlg = new DlgDbBuildConfig();
            dlg.InitializeDialog(txtBranchLocation.Text, listDbBuildConfig.SelectedIndex, _localSettings);

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            BuildConfiguration dbCfg = new BuildConfiguration(txtBranchLocation.Text, _localSettings);
            dbCfg.EditConfig(listDbBuildConfig.SelectedIndex, dlg.ExitData);
            SetBranchDependencies();
            listDbBuildConfig.SelectedIndex = index;
        }

        private void listDbBuildConfig_DoubleClick(object sender, EventArgs e)
        {
            btnEditDbBuildConfig_Click(sender, e);
        }

        private void btnSaveDbBuildConfig_Click(object sender, EventArgs e)
        {
            if (listDbBuildConfig.SelectedIndex < 0)
            {
                return;
            }

            string msg =
                "This will save the selected details to your 'Database Build Configuration File'.  Your existing configuration will be overwritten.  Are you sure you want to do this?";

            if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            //  Ok - we write the specified settings to the config file and remove the unspecified ones
            BuildConfiguration DbCfg = new BuildConfiguration(txtBranchLocation.Text, _localSettings);

            if (!DbCfg.SetConfigAsDefault(listDbBuildConfig.SelectedIndex))
            {
                return;
            }

            SetBranchDependencies();

            // Optionally run initConfigFiles to get everything matched up
            msg = "Do you want to run the 'InitConfigFiles' task to initialize the other configuration files?";

            if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                RunInitConfigFiles();
            }
        }

        private bool RunInitConfigFiles()
        {
            bool ret = true;
            int NumFailures = 0;
            int NumWarnings = 0;

            RunSimpleNantTarget(new NantTask(NantTask.TaskItem.initConfigFiles), ref NumFailures, ref NumWarnings);

            txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

            if ((NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
            {
                tabControl.SelectedTab = OutputPage;
                chkVerbose.Checked = true;
                ret = false;
            }

            PrepareWarnings();
            return ret;
        }

        private void btnSaveCurrentDbBuildConfig_Click(object sender, EventArgs e)
        {
            new BuildConfiguration(txtBranchLocation.Text, _localSettings);
        }

        private void chkUseAutoLogon_CheckedChanged(object sender, EventArgs e)
        {
            txtAutoLogonUser.Enabled = chkUseAutoLogon.Checked;
            txtAutoLogonPW.Enabled = chkUseAutoLogon.Checked;
            lblAutoLogonUser.Enabled = chkUseAutoLogon.Checked;
            lblAutoLogonPW.Enabled = chkUseAutoLogon.Checked;
        }

        private void btnUpdateMyClientConfig_Click(object sender, EventArgs e)
        {
            if (chkUseAutoLogon.Checked && (txtAutoLogonUser.Text == String.Empty))
            {
                string msg = "If you want to use the Auto-Logon capability, you must supply a Username.";
                MessageBox.Show(msg, Program.APP_TITLE);
                txtAutoLogonUser.Focus();
                return;
            }

            ClientAutoLogOn calo = new ClientAutoLogOn(txtBranchLocation.Text);

            // Validate the format of the action text
            string[] sep =
            {
                "\r\n"
            };
            string[] items = txtAutoLogonAction.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < items.Length; i++)
            {
                items[i].Trim();
            }

            string s = String.Empty;

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != String.Empty)
                {
                    if (s != String.Empty)
                    {
                        s += ",";
                    }

                    s += items[i];
                }
            }

            if (calo.UpdateConfig(chkUseAutoLogon.Checked, txtAutoLogonUser.Text, txtAutoLogonPW.Text, s))
            {
                // Now we will run the InitConfigFiles task, which copies the new file ready for running the client
                // By doing it here users will see the effect of their changes if they run direct from the IDE as opposed to using this app to start the client
                if (RunInitConfigFiles())
                {
                    MessageBox.Show("The update was applied successfully.", Program.APP_TITLE);
                }
                else
                {
                    MessageBox.Show(
                        @"The update was applied successfully to \inc\Template\etc\Client.config.my, but an error occurred in running the InitConfigFiles task.",
                        Program.APP_TITLE);
                }
            }
        }

        private void btnResetClientConfig_Click(object sender, EventArgs e)
        {
            string msg =
                "This action will reset all your client options (and not simply the startup options) because it will overwrite your personal copy of your entire ";

            msg += "client configuration with the latest version from the source code repository.  ";
            msg += "Usually this is a good thing because the repository copy may contain enhancements that your current personal copy is lacking.  ";
            msg += "You may notice changes in the behaviour of your client application as a result of this action.\r\n\r\n";
            msg += "Do you want to proceed?";

            if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ClientAutoLogOn calo = new ClientAutoLogOn(txtBranchLocation.Text);

            if (!calo.ResetConfig())
            {
                return;
            }

            SetBranchDependencies();

            if (!RunInitConfigFiles())
            {
                msg =
                    @"Your configuration was reset successfully in \inc\Template\etc\Client.config.my, but an error occurred in running the InitConfigFiles task.  ";
                msg += "This will mean that the Open Petra Client may not rspond correctly to your changes.";
                MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private bool bHaveAlertedFlashSetting = false;
        private void txtFlashAfterSeconds_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                Convert.ToUInt32(txtFlashAfterSeconds.Text);
                bHaveAlertedFlashSetting = false;
            }
            catch (Exception)
            {
                e.Cancel = true;
                tabControl.SelectedTab = OptionsPage;

                if (!bHaveAlertedFlashSetting)
                {
                    MessageBox.Show("Please enter a numeric value for the flash delay.", Program.APP_TITLE);
                }

                bHaveAlertedFlashSetting = true;
                txtFlashAfterSeconds.Focus();
                txtFlashAfterSeconds.SelectAll();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _localSettings.AltSequence = ConvertSequenceListToString(_altSequence);
            _localSettings.BazaarPath = txtBazaarPath.Text;
            _localSettings.BranchLocation = txtBranchLocation.Text;
            _localSettings.AutoStartServer = chkAutoStartServer.Checked;
            _localSettings.AutoStopServer = chkAutoStopServer.Checked;
            _localSettings.CodeGenerationComboID = cboCodeGeneration.SelectedIndex;
            _localSettings.CompilationComboID = cboCompilation.SelectedIndex;
            _localSettings.DatabaseComboID = cboDatabase.SelectedIndex;
            _localSettings.FlashAfterSeconds = Convert.ToUInt32(txtFlashAfterSeconds.Text);
            _localSettings.MinimiseServerAtStartup = chkMinimizeServer.Checked;
            _localSettings.MiscellaneousComboID = cboMiscellaneous.SelectedIndex;
            _localSettings.Sequence = ConvertSequenceListToString(_sequence);
            _localSettings.TreatWarningsAsErrors = chkTreatWarningsAsErrors.Checked;
            _localSettings.YAMLLocation = txtYAMLPath.Text;

            _localSettings.ContentHeader = String.Format("; Settings file for Open Petra Developer's Assistant\r\n; Application {0}\r\n",
                lblVersion.Text);
            _localSettings.Save();
        }

        /// <summary>
        /// Handler to capture keypress for keyboard accelerators
        /// </summary>
        /// <param name="message">Windows message</param>
        /// <param name="keys">Keys invoked</param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            if (tabControl.SelectedIndex == 0)
            {
                switch (keys)
                {
                    case Keys.F5:
                        btnRunSequence_Click(null, null);
                        return true;

                    case Keys.F5 | Keys.Alt:
                        btnRunAltSequence_Click(null, null);
                        return true;

                    case Keys.G | Keys.Control:
                        btnCodeGeneration_Click(null, null);
                        return true;

                    case Keys.I | Keys.Control:
                        btnCompilation_Click(null, null);
                        return true;

                    case Keys.M | Keys.Control:
                        btnMiscellaneous_Click(null, null);
                        return true;

                    case Keys.O | Keys.Control:
                        btnStartClient_Click(null, null);
                        return true;

                    case Keys.R | Keys.Control:
                        linkLabelRestartServer_LinkClicked(null, null);
                        return true;

                    case Keys.S | Keys.Control:
                        linkLabelStartServer_LinkClicked(null, null);
                        return true;

                    case Keys.Y | Keys.Control:
                        btnGenerateWinform_Click(null, null);
                        return true;

                    case Keys.Z | Keys.Control:
                        linkLabelBazaar_LinkClicked(null, null);
                        return true;
                }
            }

            if (tabControl.SelectedIndex == 1)
            {
                switch (keys)
                {
                    case Keys.D | Keys.Control:
                        btnDatabase_Click(null, null);
                        return true;
                }
            }

            return base.ProcessCmdKey(ref message, keys);
        }

        /*****************************************************************************************************************************
         *
         * These are the main OpenPetra Actions initiated from the GUI
         *
         * **************************************************************************************************************************/
        private void linkLabelStartServer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OutputText.ResetOutput();
            int NumFailures = 0;
            int NumWarnings = 0;

            // Check if we are up to date with the server state - someone may have started it manually at a cmd window
            GetServerState();

            if (_serverIsRunning)
            {
                SetEnabledStates();
            }
            else
            {
                // Ok.  It needs starting
                RunSimpleNantTarget(new NantTask(NantTask.TaskItem.startPetraServer), ref NumFailures, ref NumWarnings);

                txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

                if ((NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
                {
                    tabControl.SelectedTab = OutputPage;
                    chkVerbose.Checked = true;
                }

                PrepareWarnings();
            }
        }

        private void linkLabelStopServer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OutputText.ResetOutput();
            int NumFailures = 0;
            int NumWarnings = 0;

            // Check if we are up to date with the server state - someone may have stopped it manually at a cmd window
            GetServerState();

            if (_serverIsRunning)
            {
                // Ok.  It needs stopping
                RunSimpleNantTarget(new NantTask(NantTask.TaskItem.stopPetraServer), ref NumFailures, ref NumWarnings);

                txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

                if ((NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
                {
                    tabControl.SelectedTab = OutputPage;
                    chkVerbose.Checked = true;
                }

                PrepareWarnings();
            }
            else
            {
                SetEnabledStates();
            }
        }

        private void linkLabelRestartServer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OutputText.ResetOutput();
            int NumFailures = 0;
            int NumWarnings = 0;

            // Check if we are up to date with the server state - someone may have stopped it manually at a cmd window
            GetServerState();

            if (_serverIsRunning)
            {
                RunSimpleNantTarget(new NantTask(NantTask.TaskItem.stopPetraServer), ref NumFailures, ref NumWarnings);
            }

            if (NumFailures == 0)
            {
                RunSimpleNantTarget(new NantTask(NantTask.TaskItem.startPetraServer), ref NumFailures, ref NumWarnings);
            }

            txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

            if ((NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
            {
                tabControl.SelectedTab = OutputPage;
                chkVerbose.Checked = true;
            }

            PrepareWarnings();
        }

        private void btnCodeGeneration_Click(object sender, EventArgs e)
        {
            DateTime dtStart = DateTime.UtcNow;

            OutputText.ResetOutput();
            NantTask task = new NantTask(cboCodeGeneration.Items[cboCodeGeneration.SelectedIndex].ToString());
            int NumFailures = 0;
            int NumWarnings = 0;

            if (chkAutoStopServer.Checked && (task.Item == NantTask.TaskItem.generateSolution))
            {
                // This is a case where we need to auto-stop the server first
                GetServerState();

                if (_serverIsRunning)
                {
                    RunSimpleNantTarget(new NantTask(NantTask.TaskItem.stopPetraServer), ref NumFailures, ref NumWarnings);
                }
            }

            // Now we are ready to perform the original task
            if (NumFailures == 0)
            {
                RunSimpleNantTarget(task, ref NumFailures, ref NumWarnings);
            }

            txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

            if ((NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
            {
                tabControl.SelectedTab = OutputPage;
                chkVerbose.Checked = true;
            }

            PrepareWarnings();

            if ((DateTime.UtcNow - dtStart > TimeSpan.FromSeconds(Convert.ToUInt32(txtFlashAfterSeconds.Text))) && !Focused)
            {
                FlashWindow.Flash(this, 5);
            }
        }

        private void btnCompilation_Click(object sender, EventArgs e)
        {
            DateTime dtStart = DateTime.UtcNow;

            OutputText.ResetOutput();
            NantTask task = new NantTask(cboCompilation.Items[cboCompilation.SelectedIndex].ToString());
            int NumFailures = 0;
            int NumWarnings = 0;

            if (chkAutoStopServer.Checked && ((task.Item == NantTask.TaskItem.compile) || (task.Item == NantTask.TaskItem.quickCompileServer)))
            {
                // This is a case where we need to auto-stop the server first
                GetServerState();

                if (_serverIsRunning)
                {
                    RunSimpleNantTarget(new NantTask(NantTask.TaskItem.stopPetraServer), ref NumFailures, ref NumWarnings);
                }
            }

            // Now we are ready to perform the original task
            if (NumFailures == 0)
            {
                RunSimpleNantTarget(task, ref NumFailures, ref NumWarnings);
            }

            txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

            if ((NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
            {
                tabControl.SelectedTab = OutputPage;
                chkVerbose.Checked = true;
            }

            PrepareWarnings();

            if ((DateTime.UtcNow - dtStart > TimeSpan.FromSeconds(Convert.ToUInt32(txtFlashAfterSeconds.Text))) && !Focused)
            {
                FlashWindow.Flash(this, 5);
            }
        }

        private void btnMiscellaneous_Click(object sender, EventArgs e)
        {
            DateTime dtStart = DateTime.UtcNow;

            OutputText.ResetOutput();
            NantTask task = new NantTask(cboMiscellaneous.Items[cboMiscellaneous.SelectedIndex].ToString());
            int NumFailures = 0;
            int NumWarnings = 0;

            if (task.Item == NantTask.TaskItem.uncrustify)
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "Select a folder to Uncrustify";
                dlg.SelectedPath = txtBranchLocation.Text;
                dlg.RootFolder = Environment.SpecialFolder.MyComputer;
                dlg.ShowNewFolderButton = false;

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }

                // check the selection is based on teh current branch
                if (!dlg.SelectedPath.StartsWith(txtBranchLocation.Text, StringComparison.InvariantCultureIgnoreCase))
                {
                    MessageBox.Show("You must choose a folder within the current branch.", Program.APP_TITLE);
                    return;
                }

                // check that the folder contains a .build file
                string[] files = Directory.GetFiles(dlg.SelectedPath, "*.build", SearchOption.TopDirectoryOnly);

                if (files.Length == 0)
                {
                    MessageBox.Show("The selected folder cannot be Uncrustified.  You must choose a folder that contains a BUILD file.",
                        Program.APP_TITLE);
                    return;
                }

                // Ready to run - overriding the usual root location with the specified folder
                RunSimpleNantTarget(task, dlg.SelectedPath, ref NumFailures, ref NumWarnings);
            }
            else
            {
                RunSimpleNantTarget(task, ref NumFailures, ref NumWarnings);
            }

            txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

            if ((NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
            {
                tabControl.SelectedTab = OutputPage;
                chkVerbose.Checked = true;
            }

            PrepareWarnings();

            if ((DateTime.UtcNow - dtStart > TimeSpan.FromSeconds(Convert.ToUInt32(txtFlashAfterSeconds.Text))) && !Focused)
            {
                FlashWindow.Flash(this, 5);
            }
        }

        private void btnDatabase_Click(object sender, EventArgs e)
        {
            DateTime dtStart = DateTime.UtcNow;

            OutputText.ResetOutput();
            NantTask task = new NantTask(cboDatabase.Items[cboDatabase.SelectedIndex].ToString());
            int NumFailures = 0;
            int NumWarnings = 0;

            if (chkAutoStopServer.Checked && (task.Item == NantTask.TaskItem.recreateDatabase))
            {
                // This is a case where we need to auto-stop the server first
                GetServerState();

                if (_serverIsRunning)
                {
                    RunSimpleNantTarget(new NantTask(NantTask.TaskItem.stopPetraServer), ref NumFailures, ref NumWarnings);
                }
            }

            // Now we are ready to perform the original task
            if (NumFailures == 0)
            {
                RunSimpleNantTarget(task, ref NumFailures, ref NumWarnings);
            }

            txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

            if ((NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
            {
                tabControl.SelectedTab = OutputPage;
                chkVerbose.Checked = true;
            }

            PrepareWarnings();

            if ((DateTime.UtcNow - dtStart > TimeSpan.FromSeconds(Convert.ToUInt32(txtFlashAfterSeconds.Text))) && !Focused)
            {
                FlashWindow.Flash(this, 5);
            }
        }

        private void btnStartClient_Click(object sender, EventArgs e)
        {
            OutputText.ResetOutput();
            DateTime dtStart = DateTime.UtcNow;
            NantTask task = new NantTask(NantTask.TaskItem.startPetraClient);
            int NumFailures = 0;
            int NumWarnings = 0;

            if (chkAutoStartServer.Checked)
            {
                // This is a case where we need to auto-start the server first
                GetServerState();

                if (!_serverIsRunning)
                {
                    RunSimpleNantTarget(new NantTask(NantTask.TaskItem.startPetraServer), ref NumFailures, ref NumWarnings);
                }
            }

            // Now we are ready to perform the original task
            if (NumFailures == 0)
            {
                RunSimpleNantTarget(task, ref NumFailures, ref NumWarnings);
            }

            txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

            if ((NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
            {
                tabControl.SelectedTab = OutputPage;
                chkVerbose.Checked = true;
            }

            PrepareWarnings();

            if ((DateTime.UtcNow - dtStart > TimeSpan.FromSeconds(Convert.ToUInt32(txtFlashAfterSeconds.Text))) && !Focused)
            {
                FlashWindow.Flash(this, 5);
            }
        }

        private void btnGenerateWinform_Click(object sender, EventArgs e)
        {
            DateTime dtStart = DateTime.UtcNow;

            OutputText.ResetOutput();
            int NumFailures = 0;
            int NumWarnings = 0;

            if (chkAutoStartServer.Checked && chkStartClientAfterGenerateWinform.Checked)
            {
                // This is a case where we need to auto-start the server first
                GetServerState();

                if (!_serverIsRunning)
                {
                    RunSimpleNantTarget(new NantTask(NantTask.TaskItem.startPetraServer), ref NumFailures, ref NumWarnings);
                }
            }

            // Now we are ready to perform the original task
            if (NumFailures == 0)
            {
                // This is the one that is different from the rest
                RunGenerateWinform(ref NumFailures, ref NumWarnings);
            }

            txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

            if ((NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
            {
                tabControl.SelectedTab = OutputPage;
                chkVerbose.Checked = true;
            }

            PrepareWarnings();

            if ((DateTime.UtcNow - dtStart > TimeSpan.FromSeconds(Convert.ToUInt32(txtFlashAfterSeconds.Text))) && !Focused)
            {
                FlashWindow.Flash(this, 5);
            }
        }

        private void btnRunSequence_Click(object sender, EventArgs e)
        {
            RunSequence(_sequence);
        }

        private void btnRunAltSequence_Click(object sender, EventArgs e)
        {
            RunSequence(_altSequence);
        }

        /******************************************************************************************************************************************
        *
        * Helper functions
        *
        * ****************************************************************************************************************************************/

        // Generic method that runs most tasks.
        // It reads and parses the log file for errors/warnings and returns the number of errors and warnings found.
        // It handles the display of the splash dialog and the text that will end up in the output window
        private void RunSimpleNantTarget(NantTask Task, ref int NumFailures, ref int NumWarnings)
        {
            RunSimpleNantTarget(Task, txtBranchLocation.Text, ref NumFailures, ref NumWarnings);
        }

        private void RunSimpleNantTarget(NantTask Task, string WorkingFolder, ref int NumFailures, ref int NumWarnings)
        {
            // Basic routine that runs a simple target with no parameters
            ProgressDialog dlg = new ProgressDialog();

            dlg.lblStatus.Text = Task.StatusText;
            dlg.Show();

            NumFailures = 0;
            NumWarnings = 0;
            OutputText.AppendText(OutputText.OutputStream.Both, String.Format("~~~~~~~~~~~~~~~~ {0} ...\r\n", Task.LogText));
            dlg.Refresh();

            bool bOk;

            switch (Task.Item)
            {
                case NantTask.TaskItem.startPetraServer:
                    bOk = NantExecutor.StartServer(WorkingFolder, _localSettings.MinimiseServerAtStartup);
                    break;

                case NantTask.TaskItem.stopPetraServer:
                    bOk = NantExecutor.StopServer(WorkingFolder);
                    break;

                default:
                    bOk = NantExecutor.RunGenericNantTarget(WorkingFolder, Task.TargetName);
                    break;
            }

            if (bOk)
            {
                // It ran successfully - let us check the output ...
                OutputText.AddLogFileOutput(WorkingFolder + @"\opda.txt", ref NumFailures, ref NumWarnings);

                if ((Task.Item == NantTask.TaskItem.startPetraServer) || (Task.Item == NantTask.TaskItem.stopPetraServer))
                {
                    GetServerState();
                    SetEnabledStates();
                }
            }
            else
            {
                NumFailures++;
            }

            dlg.Close();
        }

        // Generic method to run generateWinform because it is in a different disk location to all the rest and takes additional parameters.
        private void RunGenerateWinform(ref int NumFailures, ref int NumWarnings)
        {
            NantTask task = new NantTask(NantTask.TaskItem.generateWinform);

            ProgressDialog dlg = new ProgressDialog();

            dlg.lblStatus.Text = task.StatusText;
            dlg.Show();

            NumFailures = 0;
            NumWarnings = 0;
            OutputText.AppendText(OutputText.OutputStream.Both, String.Format("~~~~~~~~~~~~~~~~ {0} ...\r\n", task.LogText));
            dlg.Refresh();

            if (NantExecutor.RunGenerateWinform(txtBranchLocation.Text, txtYAMLPath.Text, chkCompileWinform.Checked,
                    chkStartClientAfterGenerateWinform.Checked))
            {
                // It ran successfully - let us check the output ...
                OutputText.AddLogFileOutput(txtBranchLocation.Text + @"\csharp\ICT\Petra\Client\opda.txt", ref NumFailures, ref NumWarnings);
            }
            else
            {
                NumFailures++;
            }

            dlg.Close();
        }

        // Generic method to run a sequence of tasks
        private void RunSequence(List <NantTask.TaskItem>Sequence)
        {
            DateTime dtStart = DateTime.UtcNow;

            OutputText.ResetOutput();
            bool bShowOutputTab = false;

            for (int i = 0; i < Sequence.Count; i++)
            {
                NantTask task = new NantTask(Sequence[i]);
                int NumFailures = 0;
                int NumWarnings = 0;

                switch (task.Item)
                {
                    case NantTask.TaskItem.generateWinform:

                        if (btnGenerateWinform.Enabled)
                        {
                            RunGenerateWinform(ref NumFailures, ref NumWarnings);
                        }
                        else
                        {
                            OutputText.AppendText(OutputText.OutputStream.Both,
                                "\r\n\r\n~~~~~~~~~ Cannot generate Winform: No YAML file specified!\r\n\r\n");
                            NumFailures++;
                        }

                        break;

                    case NantTask.TaskItem.startPetraServer:
                        GetServerState();

                        if (!_serverIsRunning)
                        {
                            RunSimpleNantTarget(task, ref NumFailures, ref NumWarnings);
                        }
                        else
                        {
                            OutputText.AppendText(OutputText.OutputStream.Both,
                                "\r\n\r\n~~~~~~~~~ Skipping 'start server'.  The server is already running.\r\n\r\n");
                        }

                        break;

                    case NantTask.TaskItem.stopPetraServer:
                        GetServerState();

                        if (_serverIsRunning)
                        {
                            RunSimpleNantTarget(task, ref NumFailures, ref NumWarnings);
                        }
                        else
                        {
                            OutputText.AppendText(OutputText.OutputStream.Both,
                                "\r\n\r\n~~~~~~~~~ Skipping 'stop server'.  The server is already not running.\r\n\r\n");
                        }

                        break;

                    default:
                        RunSimpleNantTarget(task, ref NumFailures, ref NumWarnings);
                        break;
                }

                if ((NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
                {
                    bShowOutputTab = true;
                }

                if (NumFailures > 0)
                {
                    break;
                }
            }

            txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

            if (bShowOutputTab)
            {
                tabControl.SelectedTab = OutputPage;
                chkVerbose.Checked = true;
            }

            PrepareWarnings();

            if ((DateTime.UtcNow - dtStart > TimeSpan.FromSeconds(Convert.ToUInt32(txtFlashAfterSeconds.Text))) && !Focused)
            {
                FlashWindow.Flash(this, 5);
            }
        }

        // Call this method at the end of a task or sequence of tasks to initialise the verbose output warnings handler.
        // It sets up the list of positions where warnings occur, initialises the button states and initialises the text label.
        private void PrepareWarnings()
        {
            _warnings = OutputText.FindWarnings();
            _currentWarning = -1;
            lblWarnings.Text = String.Format("{0} failed, {1} errors/warnings", OutputText.ErrorCount, OutputText.WarningCount);
            SetWarningButtons();

            if (btnNextWarning.Enabled)
            {
                btnNextWarning_Click(null, null);
            }
        }

        private void btnBrowseBazaar_Click(object sender, EventArgs e)
        {
            string x86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)", EnvironmentVariableTarget.Process);

            if (x86 == null)
            {
                x86 = Environment.GetEnvironmentVariable("ProgramFiles", EnvironmentVariableTarget.Process);
            }

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Browse for the Bazaar Explorer Application";
            dlg.FileName = "bzrw.exe";
            dlg.Filter = "Applications|*.exe";
            dlg.CheckFileExists = true;

            if (x86 != null)
            {
                dlg.InitialDirectory = x86;
            }

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            txtBazaarPath.Text = dlg.FileName;
            linkLabelBazaar.Enabled = true;
        }

        private void linkLabelBazaar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Launch Bazaar using <Path-to-Bazaar> explorer <branch-location>
            System.Diagnostics.ProcessStartInfo si = new System.Diagnostics.ProcessStartInfo(txtBazaarPath.Text);
            si.Arguments = String.Format("explorer \"{0}\"", txtBranchLocation.Text);
            si.WorkingDirectory = txtBranchLocation.Text;

            try
            {
                System.Diagnostics.Process.Start(si);
            }
            catch (Exception ex)
            {
                MessageBox.Show("The Assistant failed to launch the Bazaar Explorer.  The system error message was: " + ex.Message, Program.APP_TITLE);
            }
        }

        private void ValidateBazaarPath()
        {
            if (txtBazaarPath.Text != String.Empty)
            {
                if (File.Exists(txtBazaarPath.Text))
                {
                    // All is good
                    linkLabelBazaar.Enabled = true;
                    return;
                }
                else
                {
                    // Did it get moved??
                    txtBazaarPath.Text = String.Empty;
                }
            }

            if (txtBazaarPath.Text == String.Empty)
            {
                // We will try and find it
                string x86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)", EnvironmentVariableTarget.Process);

                if (x86 == null)
                {
                    x86 = Environment.GetEnvironmentVariable("ProgramFiles", EnvironmentVariableTarget.Process);
                }

                if (x86 == null)
                {
                    return;
                }

                try
                {
                    string[] tryPath = Directory.GetFiles(x86, "bzrw.exe", SearchOption.AllDirectories);

                    if ((tryPath == null) || (tryPath.Length < 1))
                    {
                        return;
                    }

                    txtBazaarPath.Text = tryPath[0];
                    linkLabelBazaar.Enabled = true;
                }
                catch (Exception ex)
                {
                    string msg = "The Assistant could not verify the location of bzrw.exe - the Windows executable for the Bazaar Explorer.  ";
                    msg += String.Format("The Assistant searched the folders beneath '{0}' but the following error was generated: {1}",
                        x86,
                        ex.Message);
                    msg += (Environment.NewLine + Environment.NewLine);
                    msg +=
                        "You should select the Options Tab on the Assistant main window and click the small browse button to manually locate the bzrw.exe file.  ";
                    msg += "This will prevent this message from being displayed again.";
                    MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBazaarPath.Text = String.Empty;
                }
            }
        }

        private string GetWarningDisplayText()
        {
            if (chkVerbose.Checked)
            {
                return String.Format("{0} failed, {1} errors/warnings : Showing {2} of {3}",
                    OutputText.ErrorCount,
                    OutputText.WarningCount,
                    _currentWarning + 1,
                    _warnings.Count);
            }
            else
            {
                return String.Format("{0} failed, {1} errors/warnings", OutputText.ErrorCount, OutputText.WarningCount);
            }
        }
    }
}