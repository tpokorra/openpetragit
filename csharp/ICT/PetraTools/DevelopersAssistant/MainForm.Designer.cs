﻿namespace Ict.Tools.DevelopersAssistant
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.TaskPage = new System.Windows.Forms.TabPage();
            this.linkLabelBranchLocation = new System.Windows.Forms.LinkLabel();
            this.txtBranchLocation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpMultiple = new System.Windows.Forms.GroupBox();
            this.btnRunAltSequence = new System.Windows.Forms.Button();
            this.linkModifyAltSequence = new System.Windows.Forms.LinkLabel();
            this.txtAltSequence = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnRunSequence = new System.Windows.Forms.Button();
            this.linkModifySequence = new System.Windows.Forms.LinkLabel();
            this.txtSequence = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.grpSingle = new System.Windows.Forms.GroupBox();
            this.chkStartClientAfterGenerateWinform = new System.Windows.Forms.CheckBox();
            this.chkCompileWinform = new System.Windows.Forms.CheckBox();
            this.btnCompilation = new System.Windows.Forms.Button();
            this.btnCodeGeneration = new System.Windows.Forms.Button();
            this.cboCompilation = new System.Windows.Forms.ComboBox();
            this.cboCodeGeneration = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.linkLabelRestartServer = new System.Windows.Forms.LinkLabel();
            this.linkLabelStopServer = new System.Windows.Forms.LinkLabel();
            this.linkLabelStartServer = new System.Windows.Forms.LinkLabel();
            this.linkLabelYamlFile = new System.Windows.Forms.LinkLabel();
            this.btnMiscellaneous = new System.Windows.Forms.Button();
            this.cboMiscellaneous = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnStartClient = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGenerateWinform = new System.Windows.Forms.Button();
            this.txtYAMLPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DatabasePage = new System.Windows.Forms.TabPage();
            this.lblBranchLocation = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnEditDbBuildConfig = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.btnRemoveDbBuildConfig = new System.Windows.Forms.Button();
            this.btnAddDbBuildConfig = new System.Windows.Forms.Button();
            this.lblDbBuildConfig = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.listDbBuildConfig = new System.Windows.Forms.ListBox();
            this.btnSaveDbBuildConfig = new System.Windows.Forms.Button();
            this.btnDatabase = new System.Windows.Forms.Button();
            this.cboDatabase = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.OutputPage = new System.Windows.Forms.TabPage();
            this.btnNextWarning = new System.Windows.Forms.Button();
            this.btnPrevWarning = new System.Windows.Forms.Button();
            this.lblWarnings = new System.Windows.Forms.Label();
            this.chkVerbose = new System.Windows.Forms.CheckBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.OptionsPage = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtFlashAfterSeconds = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnResetClientConfig = new System.Windows.Forms.Button();
            this.chkUseAutoLogon = new System.Windows.Forms.CheckBox();
            this.btnUpdateMyClientConfig = new System.Windows.Forms.Button();
            this.txtAutoLogonAction = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtAutoLogonPW = new System.Windows.Forms.TextBox();
            this.lblAutoLogonPW = new System.Windows.Forms.Label();
            this.txtAutoLogonUser = new System.Windows.Forms.TextBox();
            this.lblAutoLogonUser = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkMinimizeServer = new System.Windows.Forms.CheckBox();
            this.chkAutoStopServer = new System.Windows.Forms.CheckBox();
            this.chkAutoStartServer = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.TaskPage.SuspendLayout();
            this.grpMultiple.SuspendLayout();
            this.grpSingle.SuspendLayout();
            this.DatabasePage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.OutputPage.SuspendLayout();
            this.OptionsPage.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.TaskPage);
            this.tabControl.Controls.Add(this.DatabasePage);
            this.tabControl.Controls.Add(this.OutputPage);
            this.tabControl.Controls.Add(this.OptionsPage);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(741, 482);
            this.tabControl.TabIndex = 0;
            // 
            // TaskPage
            // 
            this.TaskPage.Controls.Add(this.linkLabelBranchLocation);
            this.TaskPage.Controls.Add(this.txtBranchLocation);
            this.TaskPage.Controls.Add(this.label1);
            this.TaskPage.Controls.Add(this.grpMultiple);
            this.TaskPage.Controls.Add(this.grpSingle);
            this.TaskPage.Location = new System.Drawing.Point(4, 22);
            this.TaskPage.Name = "TaskPage";
            this.TaskPage.Padding = new System.Windows.Forms.Padding(3);
            this.TaskPage.Size = new System.Drawing.Size(733, 456);
            this.TaskPage.TabIndex = 0;
            this.TaskPage.Text = "Tasks";
            this.TaskPage.UseVisualStyleBackColor = true;
            // 
            // linkLabelBranchLocation
            // 
            this.linkLabelBranchLocation.AutoSize = true;
            this.linkLabelBranchLocation.Location = new System.Drawing.Point(90, 34);
            this.linkLabelBranchLocation.Name = "linkLabelBranchLocation";
            this.linkLabelBranchLocation.Size = new System.Drawing.Size(138, 13);
            this.linkLabelBranchLocation.TabIndex = 2;
            this.linkLabelBranchLocation.TabStop = true;
            this.linkLabelBranchLocation.Text = "Change the branch location";
            this.linkLabelBranchLocation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelBranchLocation_LinkClicked);
            // 
            // txtBranchLocation
            // 
            this.txtBranchLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBranchLocation.Location = new System.Drawing.Point(93, 11);
            this.txtBranchLocation.Name = "txtBranchLocation";
            this.txtBranchLocation.ReadOnly = true;
            this.txtBranchLocation.Size = new System.Drawing.Size(615, 20);
            this.txtBranchLocation.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Branch location";
            // 
            // grpMultiple
            // 
            this.grpMultiple.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMultiple.AutoSize = true;
            this.grpMultiple.Controls.Add(this.btnRunAltSequence);
            this.grpMultiple.Controls.Add(this.linkModifyAltSequence);
            this.grpMultiple.Controls.Add(this.txtAltSequence);
            this.grpMultiple.Controls.Add(this.label10);
            this.grpMultiple.Controls.Add(this.btnRunSequence);
            this.grpMultiple.Controls.Add(this.linkModifySequence);
            this.grpMultiple.Controls.Add(this.txtSequence);
            this.grpMultiple.Controls.Add(this.label9);
            this.grpMultiple.Location = new System.Drawing.Point(405, 52);
            this.grpMultiple.Name = "grpMultiple";
            this.grpMultiple.Size = new System.Drawing.Size(322, 391);
            this.grpMultiple.TabIndex = 4;
            this.grpMultiple.TabStop = false;
            this.grpMultiple.Text = "Multiple Tasks";
            // 
            // btnRunAltSequence
            // 
            this.btnRunAltSequence.Location = new System.Drawing.Point(203, 349);
            this.btnRunAltSequence.Name = "btnRunAltSequence";
            this.btnRunAltSequence.Size = new System.Drawing.Size(100, 23);
            this.btnRunAltSequence.TabIndex = 7;
            this.btnRunAltSequence.Text = "Run Sequence";
            this.btnRunAltSequence.UseVisualStyleBackColor = true;
            this.btnRunAltSequence.Click += new System.EventHandler(this.btnRunAltSequence_Click);
            // 
            // linkModifyAltSequence
            // 
            this.linkModifyAltSequence.AutoSize = true;
            this.linkModifyAltSequence.Location = new System.Drawing.Point(32, 354);
            this.linkModifyAltSequence.Name = "linkModifyAltSequence";
            this.linkModifyAltSequence.Size = new System.Drawing.Size(88, 13);
            this.linkModifyAltSequence.TabIndex = 6;
            this.linkModifyAltSequence.TabStop = true;
            this.linkModifyAltSequence.Text = "Modify sequence";
            this.linkModifyAltSequence.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkModifyAltSequence_LinkClicked);
            // 
            // txtAltSequence
            // 
            this.txtAltSequence.Location = new System.Drawing.Point(17, 228);
            this.txtAltSequence.Multiline = true;
            this.txtAltSequence.Name = "txtAltSequence";
            this.txtAltSequence.ReadOnly = true;
            this.txtAltSequence.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAltSequence.Size = new System.Drawing.Size(286, 115);
            this.txtAltSequence.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 212);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(140, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Alternate Sequence (Alt+F5)";
            // 
            // btnRunSequence
            // 
            this.btnRunSequence.Location = new System.Drawing.Point(203, 160);
            this.btnRunSequence.Name = "btnRunSequence";
            this.btnRunSequence.Size = new System.Drawing.Size(100, 23);
            this.btnRunSequence.TabIndex = 3;
            this.btnRunSequence.Text = "Run Sequence";
            this.btnRunSequence.UseVisualStyleBackColor = true;
            this.btnRunSequence.Click += new System.EventHandler(this.btnRunSequence_Click);
            // 
            // linkModifySequence
            // 
            this.linkModifySequence.AutoSize = true;
            this.linkModifySequence.Location = new System.Drawing.Point(32, 165);
            this.linkModifySequence.Name = "linkModifySequence";
            this.linkModifySequence.Size = new System.Drawing.Size(88, 13);
            this.linkModifySequence.TabIndex = 2;
            this.linkModifySequence.TabStop = true;
            this.linkModifySequence.Text = "Modify sequence";
            this.linkModifySequence.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkModifySequence_LinkClicked);
            // 
            // txtSequence
            // 
            this.txtSequence.Location = new System.Drawing.Point(17, 39);
            this.txtSequence.Multiline = true;
            this.txtSequence.Name = "txtSequence";
            this.txtSequence.ReadOnly = true;
            this.txtSequence.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSequence.Size = new System.Drawing.Size(286, 115);
            this.txtSequence.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Standard sequence (F5)";
            // 
            // grpSingle
            // 
            this.grpSingle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.grpSingle.AutoSize = true;
            this.grpSingle.Controls.Add(this.chkStartClientAfterGenerateWinform);
            this.grpSingle.Controls.Add(this.chkCompileWinform);
            this.grpSingle.Controls.Add(this.btnCompilation);
            this.grpSingle.Controls.Add(this.btnCodeGeneration);
            this.grpSingle.Controls.Add(this.cboCompilation);
            this.grpSingle.Controls.Add(this.cboCodeGeneration);
            this.grpSingle.Controls.Add(this.label8);
            this.grpSingle.Controls.Add(this.label5);
            this.grpSingle.Controls.Add(this.linkLabelRestartServer);
            this.grpSingle.Controls.Add(this.linkLabelStopServer);
            this.grpSingle.Controls.Add(this.linkLabelStartServer);
            this.grpSingle.Controls.Add(this.linkLabelYamlFile);
            this.grpSingle.Controls.Add(this.btnMiscellaneous);
            this.grpSingle.Controls.Add(this.cboMiscellaneous);
            this.grpSingle.Controls.Add(this.label6);
            this.grpSingle.Controls.Add(this.btnStartClient);
            this.grpSingle.Controls.Add(this.label4);
            this.grpSingle.Controls.Add(this.label3);
            this.grpSingle.Controls.Add(this.btnGenerateWinform);
            this.grpSingle.Controls.Add(this.txtYAMLPath);
            this.grpSingle.Controls.Add(this.label2);
            this.grpSingle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSingle.Location = new System.Drawing.Point(6, 52);
            this.grpSingle.Name = "grpSingle";
            this.grpSingle.Size = new System.Drawing.Size(390, 391);
            this.grpSingle.TabIndex = 3;
            this.grpSingle.TabStop = false;
            this.grpSingle.Text = "Individual Tasks";
            // 
            // chkStartClientAfterGenerateWinform
            // 
            this.chkStartClientAfterGenerateWinform.AutoSize = true;
            this.chkStartClientAfterGenerateWinform.Checked = true;
            this.chkStartClientAfterGenerateWinform.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStartClientAfterGenerateWinform.Location = new System.Drawing.Point(150, 148);
            this.chkStartClientAfterGenerateWinform.Name = "chkStartClientAfterGenerateWinform";
            this.chkStartClientAfterGenerateWinform.Size = new System.Drawing.Size(156, 17);
            this.chkStartClientAfterGenerateWinform.TabIndex = 8;
            this.chkStartClientAfterGenerateWinform.Text = "Start client after compilation";
            this.chkStartClientAfterGenerateWinform.UseVisualStyleBackColor = true;
            // 
            // chkCompileWinform
            // 
            this.chkCompileWinform.AutoSize = true;
            this.chkCompileWinform.Checked = true;
            this.chkCompileWinform.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCompileWinform.Location = new System.Drawing.Point(150, 125);
            this.chkCompileWinform.Name = "chkCompileWinform";
            this.chkCompileWinform.Size = new System.Drawing.Size(181, 17);
            this.chkCompileWinform.TabIndex = 7;
            this.chkCompileWinform.Text = "Compile after generating the form";
            this.chkCompileWinform.UseVisualStyleBackColor = true;
            this.chkCompileWinform.CheckedChanged += new System.EventHandler(this.chkCompileWinform_CheckedChanged);
            // 
            // btnCompilation
            // 
            this.btnCompilation.Location = new System.Drawing.Point(352, 249);
            this.btnCompilation.Name = "btnCompilation";
            this.btnCompilation.Size = new System.Drawing.Size(32, 23);
            this.btnCompilation.TabIndex = 15;
            this.btnCompilation.Text = "Go";
            this.btnCompilation.UseVisualStyleBackColor = true;
            this.btnCompilation.Click += new System.EventHandler(this.btnCompilation_Click);
            // 
            // btnCodeGeneration
            // 
            this.btnCodeGeneration.Location = new System.Drawing.Point(352, 199);
            this.btnCodeGeneration.Name = "btnCodeGeneration";
            this.btnCodeGeneration.Size = new System.Drawing.Size(32, 23);
            this.btnCodeGeneration.TabIndex = 12;
            this.btnCodeGeneration.Text = "Go";
            this.btnCodeGeneration.UseVisualStyleBackColor = true;
            this.btnCodeGeneration.Click += new System.EventHandler(this.btnCodeGeneration_Click);
            // 
            // cboCompilation
            // 
            this.cboCompilation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCompilation.FormattingEnabled = true;
            this.cboCompilation.Location = new System.Drawing.Point(97, 250);
            this.cboCompilation.Name = "cboCompilation";
            this.cboCompilation.Size = new System.Drawing.Size(249, 21);
            this.cboCompilation.TabIndex = 14;
            // 
            // cboCodeGeneration
            // 
            this.cboCodeGeneration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCodeGeneration.FormattingEnabled = true;
            this.cboCodeGeneration.Location = new System.Drawing.Point(97, 200);
            this.cboCodeGeneration.Name = "cboCodeGeneration";
            this.cboCodeGeneration.Size = new System.Drawing.Size(249, 21);
            this.cboCodeGeneration.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 253);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Compilation";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Code generation";
            // 
            // linkLabelRestartServer
            // 
            this.linkLabelRestartServer.AutoSize = true;
            this.linkLabelRestartServer.Location = new System.Drawing.Point(246, 48);
            this.linkLabelRestartServer.Name = "linkLabelRestartServer";
            this.linkLabelRestartServer.Size = new System.Drawing.Size(76, 13);
            this.linkLabelRestartServer.TabIndex = 3;
            this.linkLabelRestartServer.TabStop = true;
            this.linkLabelRestartServer.Text = "Re-start server";
            this.linkLabelRestartServer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelRestartServer_LinkClicked);
            // 
            // linkLabelStopServer
            // 
            this.linkLabelStopServer.AutoSize = true;
            this.linkLabelStopServer.Location = new System.Drawing.Point(139, 48);
            this.linkLabelStopServer.Name = "linkLabelStopServer";
            this.linkLabelStopServer.Size = new System.Drawing.Size(61, 13);
            this.linkLabelStopServer.TabIndex = 2;
            this.linkLabelStopServer.TabStop = true;
            this.linkLabelStopServer.Text = "Stop server";
            this.linkLabelStopServer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelStopServer_LinkClicked);
            // 
            // linkLabelStartServer
            // 
            this.linkLabelStartServer.AutoSize = true;
            this.linkLabelStartServer.Location = new System.Drawing.Point(38, 48);
            this.linkLabelStartServer.Name = "linkLabelStartServer";
            this.linkLabelStartServer.Size = new System.Drawing.Size(61, 13);
            this.linkLabelStartServer.TabIndex = 1;
            this.linkLabelStartServer.TabStop = true;
            this.linkLabelStartServer.Text = "Start server";
            this.linkLabelStartServer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelStartServer_LinkClicked);
            // 
            // linkLabelYamlFile
            // 
            this.linkLabelYamlFile.AutoSize = true;
            this.linkLabelYamlFile.Location = new System.Drawing.Point(6, 122);
            this.linkLabelYamlFile.Name = "linkLabelYamlFile";
            this.linkLabelYamlFile.Size = new System.Drawing.Size(104, 13);
            this.linkLabelYamlFile.TabIndex = 6;
            this.linkLabelYamlFile.TabStop = true;
            this.linkLabelYamlFile.Text = "Change the filename";
            this.linkLabelYamlFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelYamlFile_LinkClicked);
            // 
            // btnMiscellaneous
            // 
            this.btnMiscellaneous.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMiscellaneous.Location = new System.Drawing.Point(352, 299);
            this.btnMiscellaneous.Name = "btnMiscellaneous";
            this.btnMiscellaneous.Size = new System.Drawing.Size(32, 23);
            this.btnMiscellaneous.TabIndex = 18;
            this.btnMiscellaneous.Text = "Go";
            this.btnMiscellaneous.UseVisualStyleBackColor = true;
            this.btnMiscellaneous.Click += new System.EventHandler(this.btnMiscellaneous_Click);
            // 
            // cboMiscellaneous
            // 
            this.cboMiscellaneous.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboMiscellaneous.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMiscellaneous.FormattingEnabled = true;
            this.cboMiscellaneous.Location = new System.Drawing.Point(97, 300);
            this.cboMiscellaneous.Name = "cboMiscellaneous";
            this.cboMiscellaneous.Size = new System.Drawing.Size(249, 21);
            this.cboMiscellaneous.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 303);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Miscellaneous";
            // 
            // btnStartClient
            // 
            this.btnStartClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartClient.Location = new System.Drawing.Point(352, 349);
            this.btnStartClient.Name = "btnStartClient";
            this.btnStartClient.Size = new System.Drawing.Size(32, 23);
            this.btnStartClient.TabIndex = 20;
            this.btnStartClient.Text = "Go";
            this.btnStartClient.UseVisualStyleBackColor = true;
            this.btnStartClient.Click += new System.EventHandler(this.btnStartClient_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(94, 354);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Start Petra Client";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(6, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Petra Server";
            // 
            // btnGenerateWinform
            // 
            this.btnGenerateWinform.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateWinform.Location = new System.Drawing.Point(352, 142);
            this.btnGenerateWinform.Name = "btnGenerateWinform";
            this.btnGenerateWinform.Size = new System.Drawing.Size(32, 23);
            this.btnGenerateWinform.TabIndex = 9;
            this.btnGenerateWinform.Text = "Go";
            this.btnGenerateWinform.UseVisualStyleBackColor = true;
            this.btnGenerateWinform.Click += new System.EventHandler(this.btnGenerateWinform_Click);
            // 
            // txtYAMLPath
            // 
            this.txtYAMLPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtYAMLPath.Location = new System.Drawing.Point(9, 99);
            this.txtYAMLPath.Name = "txtYAMLPath";
            this.txtYAMLPath.ReadOnly = true;
            this.txtYAMLPath.Size = new System.Drawing.Size(375, 20);
            this.txtYAMLPath.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Generate a Windows Form from YAML";
            // 
            // DatabasePage
            // 
            this.DatabasePage.Controls.Add(this.lblBranchLocation);
            this.DatabasePage.Controls.Add(this.label16);
            this.DatabasePage.Controls.Add(this.groupBox2);
            this.DatabasePage.Controls.Add(this.btnDatabase);
            this.DatabasePage.Controls.Add(this.cboDatabase);
            this.DatabasePage.Controls.Add(this.label7);
            this.DatabasePage.Location = new System.Drawing.Point(4, 22);
            this.DatabasePage.Name = "DatabasePage";
            this.DatabasePage.Size = new System.Drawing.Size(733, 456);
            this.DatabasePage.TabIndex = 3;
            this.DatabasePage.Text = "Database";
            this.DatabasePage.UseVisualStyleBackColor = true;
            // 
            // lblBranchLocation
            // 
            this.lblBranchLocation.AutoSize = true;
            this.lblBranchLocation.Location = new System.Drawing.Point(129, 20);
            this.lblBranchLocation.Name = "lblBranchLocation";
            this.lblBranchLocation.Size = new System.Drawing.Size(41, 13);
            this.lblBranchLocation.TabIndex = 1;
            this.lblBranchLocation.Text = "label13";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(29, 20);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(94, 13);
            this.label16.TabIndex = 0;
            this.label16.Text = "Branch Location:  ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnEditDbBuildConfig);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.btnRemoveDbBuildConfig);
            this.groupBox2.Controls.Add(this.btnAddDbBuildConfig);
            this.groupBox2.Controls.Add(this.lblDbBuildConfig);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.listDbBuildConfig);
            this.groupBox2.Controls.Add(this.btnSaveDbBuildConfig);
            this.groupBox2.Location = new System.Drawing.Point(3, 49);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(727, 240);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Build Configuration";
            // 
            // btnEditDbBuildConfig
            // 
            this.btnEditDbBuildConfig.Location = new System.Drawing.Point(87, 200);
            this.btnEditDbBuildConfig.Name = "btnEditDbBuildConfig";
            this.btnEditDbBuildConfig.Size = new System.Drawing.Size(75, 23);
            this.btnEditDbBuildConfig.TabIndex = 5;
            this.btnEditDbBuildConfig.Text = "Edit";
            this.btnEditDbBuildConfig.UseVisualStyleBackColor = true;
            this.btnEditDbBuildConfig.Click += new System.EventHandler(this.btnEditDbBuildConfig_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 83);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(159, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "My favourite build configurations";
            // 
            // btnRemoveDbBuildConfig
            // 
            this.btnRemoveDbBuildConfig.Location = new System.Drawing.Point(168, 200);
            this.btnRemoveDbBuildConfig.Name = "btnRemoveDbBuildConfig";
            this.btnRemoveDbBuildConfig.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveDbBuildConfig.TabIndex = 6;
            this.btnRemoveDbBuildConfig.Text = "Remove";
            this.btnRemoveDbBuildConfig.UseVisualStyleBackColor = true;
            this.btnRemoveDbBuildConfig.Click += new System.EventHandler(this.btnRemoveDbBuildConfig_Click);
            // 
            // btnAddDbBuildConfig
            // 
            this.btnAddDbBuildConfig.Location = new System.Drawing.Point(6, 200);
            this.btnAddDbBuildConfig.Name = "btnAddDbBuildConfig";
            this.btnAddDbBuildConfig.Size = new System.Drawing.Size(75, 23);
            this.btnAddDbBuildConfig.TabIndex = 4;
            this.btnAddDbBuildConfig.Text = "Add";
            this.btnAddDbBuildConfig.UseVisualStyleBackColor = true;
            this.btnAddDbBuildConfig.Click += new System.EventHandler(this.btnAddDbBuildConfig_Click);
            // 
            // lblDbBuildConfig
            // 
            this.lblDbBuildConfig.Location = new System.Drawing.Point(140, 27);
            this.lblDbBuildConfig.Name = "lblDbBuildConfig";
            this.lblDbBuildConfig.Size = new System.Drawing.Size(581, 36);
            this.lblDbBuildConfig.TabIndex = 1;
            this.lblDbBuildConfig.Text = "label12";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(26, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Current configuration:";
            // 
            // listDbBuildConfig
            // 
            this.listDbBuildConfig.FormattingEnabled = true;
            this.listDbBuildConfig.Location = new System.Drawing.Point(6, 99);
            this.listDbBuildConfig.Name = "listDbBuildConfig";
            this.listDbBuildConfig.Size = new System.Drawing.Size(715, 95);
            this.listDbBuildConfig.TabIndex = 3;
            this.listDbBuildConfig.DoubleClick += new System.EventHandler(this.listDbBuildConfig_DoubleClick);
            // 
            // btnSaveDbBuildConfig
            // 
            this.btnSaveDbBuildConfig.Location = new System.Drawing.Point(506, 200);
            this.btnSaveDbBuildConfig.Name = "btnSaveDbBuildConfig";
            this.btnSaveDbBuildConfig.Size = new System.Drawing.Size(215, 23);
            this.btnSaveDbBuildConfig.TabIndex = 7;
            this.btnSaveDbBuildConfig.Text = "Save As Current Build Configuration";
            this.btnSaveDbBuildConfig.UseVisualStyleBackColor = true;
            this.btnSaveDbBuildConfig.Click += new System.EventHandler(this.btnSaveDbBuildConfig_Click);
            // 
            // btnDatabase
            // 
            this.btnDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDatabase.Location = new System.Drawing.Point(523, 311);
            this.btnDatabase.Name = "btnDatabase";
            this.btnDatabase.Size = new System.Drawing.Size(32, 23);
            this.btnDatabase.TabIndex = 5;
            this.btnDatabase.Text = "Go";
            this.btnDatabase.UseVisualStyleBackColor = true;
            this.btnDatabase.Click += new System.EventHandler(this.btnDatabase_Click);
            // 
            // cboDatabase
            // 
            this.cboDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDatabase.FormattingEnabled = true;
            this.cboDatabase.Location = new System.Drawing.Point(268, 313);
            this.cboDatabase.Name = "cboDatabase";
            this.cboDatabase.Size = new System.Drawing.Size(249, 21);
            this.cboDatabase.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(168, 316);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Database Tasks";
            // 
            // OutputPage
            // 
            this.OutputPage.Controls.Add(this.btnNextWarning);
            this.OutputPage.Controls.Add(this.btnPrevWarning);
            this.OutputPage.Controls.Add(this.lblWarnings);
            this.OutputPage.Controls.Add(this.chkVerbose);
            this.OutputPage.Controls.Add(this.txtOutput);
            this.OutputPage.Location = new System.Drawing.Point(4, 22);
            this.OutputPage.Name = "OutputPage";
            this.OutputPage.Padding = new System.Windows.Forms.Padding(3);
            this.OutputPage.Size = new System.Drawing.Size(733, 456);
            this.OutputPage.TabIndex = 1;
            this.OutputPage.Text = "Output";
            this.OutputPage.UseVisualStyleBackColor = true;
            // 
            // btnNextWarning
            // 
            this.btnNextWarning.Enabled = false;
            this.btnNextWarning.Location = new System.Drawing.Point(677, 9);
            this.btnNextWarning.Name = "btnNextWarning";
            this.btnNextWarning.Size = new System.Drawing.Size(50, 23);
            this.btnNextWarning.TabIndex = 3;
            this.btnNextWarning.Text = "Next";
            this.btnNextWarning.UseVisualStyleBackColor = true;
            this.btnNextWarning.Click += new System.EventHandler(this.btnNextWarning_Click);
            // 
            // btnPrevWarning
            // 
            this.btnPrevWarning.Enabled = false;
            this.btnPrevWarning.Location = new System.Drawing.Point(621, 9);
            this.btnPrevWarning.Name = "btnPrevWarning";
            this.btnPrevWarning.Size = new System.Drawing.Size(50, 23);
            this.btnPrevWarning.TabIndex = 2;
            this.btnPrevWarning.Text = "Prev";
            this.btnPrevWarning.UseVisualStyleBackColor = true;
            this.btnPrevWarning.Click += new System.EventHandler(this.btnPrevWarning_Click);
            // 
            // lblWarnings
            // 
            this.lblWarnings.Location = new System.Drawing.Point(418, 14);
            this.lblWarnings.Name = "lblWarnings";
            this.lblWarnings.Size = new System.Drawing.Size(197, 16);
            this.lblWarnings.TabIndex = 1;
            this.lblWarnings.Text = "Warnings/Errors";
            this.lblWarnings.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkVerbose
            // 
            this.chkVerbose.AutoSize = true;
            this.chkVerbose.Location = new System.Drawing.Point(3, 13);
            this.chkVerbose.Name = "chkVerbose";
            this.chkVerbose.Size = new System.Drawing.Size(127, 17);
            this.chkVerbose.TabIndex = 0;
            this.chkVerbose.Text = "Show verbose output";
            this.chkVerbose.UseVisualStyleBackColor = true;
            this.chkVerbose.CheckedChanged += new System.EventHandler(this.chkVerbose_CheckedChanged);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(6, 36);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(721, 414);
            this.txtOutput.TabIndex = 4;
            // 
            // OptionsPage
            // 
            this.OptionsPage.Controls.Add(this.groupBox4);
            this.OptionsPage.Controls.Add(this.groupBox3);
            this.OptionsPage.Controls.Add(this.lblVersion);
            this.OptionsPage.Controls.Add(this.groupBox1);
            this.OptionsPage.Location = new System.Drawing.Point(4, 22);
            this.OptionsPage.Name = "OptionsPage";
            this.OptionsPage.Size = new System.Drawing.Size(733, 456);
            this.OptionsPage.TabIndex = 2;
            this.OptionsPage.Text = "Options";
            this.OptionsPage.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.txtFlashAfterSeconds);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Location = new System.Drawing.Point(21, 341);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(691, 66);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Options that apply to all tasks";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(299, 29);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(297, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "seconds and I am working on a different Windows application";
            // 
            // txtFlashAfterSeconds
            // 
            this.txtFlashAfterSeconds.Location = new System.Drawing.Point(264, 26);
            this.txtFlashAfterSeconds.Name = "txtFlashAfterSeconds";
            this.txtFlashAfterSeconds.Size = new System.Drawing.Size(29, 20);
            this.txtFlashAfterSeconds.TabIndex = 1;
            this.txtFlashAfterSeconds.Text = "15";
            this.txtFlashAfterSeconds.Validating += new System.ComponentModel.CancelEventHandler(this.txtFlashAfterSeconds_Validating);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(26, 29);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(232, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Alert me if a task or sequence takes longer than";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnResetClientConfig);
            this.groupBox3.Controls.Add(this.chkUseAutoLogon);
            this.groupBox3.Controls.Add(this.btnUpdateMyClientConfig);
            this.groupBox3.Controls.Add(this.txtAutoLogonAction);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.txtAutoLogonPW);
            this.groupBox3.Controls.Add(this.lblAutoLogonPW);
            this.groupBox3.Controls.Add(this.txtAutoLogonUser);
            this.groupBox3.Controls.Add(this.lblAutoLogonUser);
            this.groupBox3.Location = new System.Drawing.Point(21, 161);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(691, 171);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Options that apply to the startup of the client";
            // 
            // btnResetClientConfig
            // 
            this.btnResetClientConfig.Location = new System.Drawing.Point(610, 17);
            this.btnResetClientConfig.Name = "btnResetClientConfig";
            this.btnResetClientConfig.Size = new System.Drawing.Size(75, 23);
            this.btnResetClientConfig.TabIndex = 7;
            this.btnResetClientConfig.Text = "Reset";
            this.btnResetClientConfig.UseVisualStyleBackColor = true;
            this.btnResetClientConfig.Click += new System.EventHandler(this.btnResetClientConfig_Click);
            // 
            // chkUseAutoLogon
            // 
            this.chkUseAutoLogon.AutoSize = true;
            this.chkUseAutoLogon.Location = new System.Drawing.Point(41, 20);
            this.chkUseAutoLogon.Name = "chkUseAutoLogon";
            this.chkUseAutoLogon.Size = new System.Drawing.Size(315, 17);
            this.chkUseAutoLogon.TabIndex = 0;
            this.chkUseAutoLogon.Text = "Use the auto-logon capability  (Over-ride at run-time with ALT)";
            this.chkUseAutoLogon.UseVisualStyleBackColor = true;
            this.chkUseAutoLogon.CheckedChanged += new System.EventHandler(this.chkUseAutoLogon_CheckedChanged);
            // 
            // btnUpdateMyClientConfig
            // 
            this.btnUpdateMyClientConfig.Location = new System.Drawing.Point(610, 136);
            this.btnUpdateMyClientConfig.Name = "btnUpdateMyClientConfig";
            this.btnUpdateMyClientConfig.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateMyClientConfig.TabIndex = 8;
            this.btnUpdateMyClientConfig.Text = "Update";
            this.btnUpdateMyClientConfig.UseVisualStyleBackColor = true;
            this.btnUpdateMyClientConfig.Click += new System.EventHandler(this.btnUpdateMyClientConfig_Click);
            // 
            // txtAutoLogonAction
            // 
            this.txtAutoLogonAction.Location = new System.Drawing.Point(9, 99);
            this.txtAutoLogonAction.Multiline = true;
            this.txtAutoLogonAction.Name = "txtAutoLogonAction";
            this.txtAutoLogonAction.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAutoLogonAction.Size = new System.Drawing.Size(583, 60);
            this.txtAutoLogonAction.TabIndex = 6;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 83);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(530, 13);
            this.label15.TabIndex = 5;
            this.label15.Text = "Test action (Put one property=value on each line and do not include commas.  Over" +
                "-ride at run-time with CTRL)";
            // 
            // txtAutoLogonPW
            // 
            this.txtAutoLogonPW.Enabled = false;
            this.txtAutoLogonPW.Location = new System.Drawing.Point(475, 43);
            this.txtAutoLogonPW.Name = "txtAutoLogonPW";
            this.txtAutoLogonPW.Size = new System.Drawing.Size(117, 20);
            this.txtAutoLogonPW.TabIndex = 4;
            // 
            // lblAutoLogonPW
            // 
            this.lblAutoLogonPW.AutoSize = true;
            this.lblAutoLogonPW.Enabled = false;
            this.lblAutoLogonPW.Location = new System.Drawing.Point(363, 46);
            this.lblAutoLogonPW.Name = "lblAutoLogonPW";
            this.lblAutoLogonPW.Size = new System.Drawing.Size(106, 13);
            this.lblAutoLogonPW.TabIndex = 3;
            this.lblAutoLogonPW.Text = "Auto-logon password";
            // 
            // txtAutoLogonUser
            // 
            this.txtAutoLogonUser.Enabled = false;
            this.txtAutoLogonUser.Location = new System.Drawing.Point(164, 43);
            this.txtAutoLogonUser.Name = "txtAutoLogonUser";
            this.txtAutoLogonUser.Size = new System.Drawing.Size(149, 20);
            this.txtAutoLogonUser.TabIndex = 2;
            // 
            // lblAutoLogonUser
            // 
            this.lblAutoLogonUser.AutoSize = true;
            this.lblAutoLogonUser.Enabled = false;
            this.lblAutoLogonUser.Location = new System.Drawing.Point(77, 46);
            this.lblAutoLogonUser.Name = "lblAutoLogonUser";
            this.lblAutoLogonUser.Size = new System.Drawing.Size(81, 13);
            this.lblAutoLogonUser.TabIndex = 1;
            this.lblAutoLogonUser.Text = "Auto-logon user";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(18, 432);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(42, 13);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "Version";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkMinimizeServer);
            this.groupBox1.Controls.Add(this.chkAutoStopServer);
            this.groupBox1.Controls.Add(this.chkAutoStartServer);
            this.groupBox1.Location = new System.Drawing.Point(21, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(691, 136);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options that apply to individual tasks";
            // 
            // chkMinimizeServer
            // 
            this.chkMinimizeServer.AutoSize = true;
            this.chkMinimizeServer.Location = new System.Drawing.Point(41, 101);
            this.chkMinimizeServer.Name = "chkMinimizeServer";
            this.chkMinimizeServer.Size = new System.Drawing.Size(302, 17);
            this.chkMinimizeServer.TabIndex = 2;
            this.chkMinimizeServer.Text = "Minimize the server window as soon as the server starts up";
            this.chkMinimizeServer.UseVisualStyleBackColor = true;
            // 
            // chkAutoStopServer
            // 
            this.chkAutoStopServer.AutoSize = true;
            this.chkAutoStopServer.Location = new System.Drawing.Point(41, 68);
            this.chkAutoStopServer.Name = "chkAutoStopServer";
            this.chkAutoStopServer.Size = new System.Drawing.Size(419, 17);
            this.chkAutoStopServer.TabIndex = 1;
            this.chkAutoStopServer.Text = "Automatically stop the server before any task that compiles it, if the server is " +
                "running";
            this.chkAutoStopServer.UseVisualStyleBackColor = true;
            // 
            // chkAutoStartServer
            // 
            this.chkAutoStartServer.AutoSize = true;
            this.chkAutoStartServer.Location = new System.Drawing.Point(41, 35);
            this.chkAutoStartServer.Name = "chkAutoStartServer";
            this.chkAutoStartServer.Size = new System.Drawing.Size(428, 17);
            this.chkAutoStartServer.TabIndex = 0;
            this.chkAutoStartServer.Text = "Automatically start the server when the client starts, if the server is not alrea" +
                "dy running";
            this.chkAutoStartServer.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 506);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Open Petra Developer\'s Assistant";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.TaskPage.ResumeLayout(false);
            this.TaskPage.PerformLayout();
            this.grpMultiple.ResumeLayout(false);
            this.grpMultiple.PerformLayout();
            this.grpSingle.ResumeLayout(false);
            this.grpSingle.PerformLayout();
            this.DatabasePage.ResumeLayout(false);
            this.DatabasePage.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.OutputPage.ResumeLayout(false);
            this.OutputPage.PerformLayout();
            this.OptionsPage.ResumeLayout(false);
            this.OptionsPage.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage TaskPage;
        private System.Windows.Forms.TabPage OutputPage;
        private System.Windows.Forms.GroupBox grpMultiple;
        private System.Windows.Forms.GroupBox grpSingle;
        private System.Windows.Forms.CheckBox chkVerbose;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.LinkLabel linkLabelBranchLocation;
        private System.Windows.Forms.TextBox txtBranchLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboMiscellaneous;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnStartClient;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGenerateWinform;
        private System.Windows.Forms.TextBox txtYAMLPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMiscellaneous;
        private System.Windows.Forms.LinkLabel linkLabelRestartServer;
        private System.Windows.Forms.LinkLabel linkLabelStopServer;
        private System.Windows.Forms.LinkLabel linkLabelStartServer;
        private System.Windows.Forms.LinkLabel linkLabelYamlFile;
        private System.Windows.Forms.Button btnCompilation;
        private System.Windows.Forms.Button btnCodeGeneration;
        private System.Windows.Forms.ComboBox cboCompilation;
        private System.Windows.Forms.ComboBox cboCodeGeneration;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRunAltSequence;
        private System.Windows.Forms.LinkLabel linkModifyAltSequence;
        private System.Windows.Forms.TextBox txtAltSequence;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnRunSequence;
        private System.Windows.Forms.LinkLabel linkModifySequence;
        private System.Windows.Forms.TextBox txtSequence;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnNextWarning;
        private System.Windows.Forms.Button btnPrevWarning;
        private System.Windows.Forms.Label lblWarnings;
        private System.Windows.Forms.TabPage OptionsPage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAutoStopServer;
        private System.Windows.Forms.CheckBox chkAutoStartServer;
        private System.Windows.Forms.CheckBox chkMinimizeServer;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.TabPage DatabasePage;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSaveDbBuildConfig;
        private System.Windows.Forms.Button btnDatabase;
        private System.Windows.Forms.ComboBox cboDatabase;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblBranchLocation;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnRemoveDbBuildConfig;
        private System.Windows.Forms.Button btnAddDbBuildConfig;
        private System.Windows.Forms.Label lblDbBuildConfig;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListBox listDbBuildConfig;
        private System.Windows.Forms.Button btnEditDbBuildConfig;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnUpdateMyClientConfig;
        private System.Windows.Forms.TextBox txtAutoLogonAction;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtAutoLogonPW;
        private System.Windows.Forms.Label lblAutoLogonPW;
        private System.Windows.Forms.TextBox txtAutoLogonUser;
        private System.Windows.Forms.Label lblAutoLogonUser;
        private System.Windows.Forms.CheckBox chkUseAutoLogon;
        private System.Windows.Forms.Button btnResetClientConfig;
        private System.Windows.Forms.CheckBox chkStartClientAfterGenerateWinform;
        private System.Windows.Forms.CheckBox chkCompileWinform;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtFlashAfterSeconds;
        private System.Windows.Forms.Label label13;
    }
}

