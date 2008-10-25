using System;
using System.Diagnostics;
using System.Windows.Forms;
using Logger;

namespace DataDecoder
{
    partial class Setup
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
            try
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                // Debug.WriteLine("   *** Base.Dispose started ***");
                base.Dispose(disposing);
                // Debug.WriteLine("   *** Dispose completed ***");

            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
       }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setup));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPorts = new System.Windows.Forms.TabPage();
            this.grpSlave = new System.Windows.Forms.GroupBox();
            this.chkFollow = new System.Windows.Forms.CheckBox();
            this.chkMode = new System.Windows.Forms.CheckBox();
            this.cboRadio = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cboRadData = new System.Windows.Forms.ComboBox();
            this.txtRadNum = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboSerAcc = new System.Windows.Forms.ComboBox();
            this.grpBox1 = new System.Windows.Forms.GroupBox();
            this.label46 = new System.Windows.Forms.Label();
            this.cboRCP1Rotor = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cboLogPort = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtInv = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboCAT = new System.Windows.Forms.ComboBox();
            this.grpBox2 = new System.Windows.Forms.GroupBox();
            this.lblPortBtn = new System.Windows.Forms.Label();
            this.btnPortNum = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblBCD = new System.Windows.Forms.Label();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkDevice = new System.Windows.Forms.CheckBox();
            this.grpLPT = new System.Windows.Forms.GroupBox();
            this.rbOther = new System.Windows.Forms.RadioButton();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.rb4 = new System.Windows.Forms.RadioButton();
            this.rb2 = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rb3 = new System.Windows.Forms.RadioButton();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.tabDevice = new System.Windows.Forms.TabPage();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave0 = new System.Windows.Forms.Button();
            this.btnReLoad0 = new System.Windows.Forms.Button();
            this.btnFile0 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFile0 = new System.Windows.Forms.TextBox();
            this.dg1 = new System.Windows.Forms.DataGridView();
            this.chkDev0 = new System.Windows.Forms.CheckBox();
            this.Dev0 = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.tabRCP = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.cboRCP4Rotor = new System.Windows.Forms.ComboBox();
            this.cboRCP3Rotor = new System.Windows.Forms.ComboBox();
            this.cboRCP2Rotor = new System.Windows.Forms.ComboBox();
            this.label40 = new System.Windows.Forms.Label();
            this.chkRCP2 = new System.Windows.Forms.CheckBox();
            this.txtRCP = new System.Windows.Forms.TextBox();
            this.cboRCP2 = new System.Windows.Forms.ComboBox();
            this.chkRCP4IF = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.chkRCP3IF = new System.Windows.Forms.CheckBox();
            this.chkRCP3 = new System.Windows.Forms.CheckBox();
            this.chkRCP2IF = new System.Windows.Forms.CheckBox();
            this.cboRCP3 = new System.Windows.Forms.ComboBox();
            this.chkRCP4DisPol = new System.Windows.Forms.CheckBox();
            this.chkRCP4 = new System.Windows.Forms.CheckBox();
            this.chkRCP3DisPol = new System.Windows.Forms.CheckBox();
            this.cboRCP4 = new System.Windows.Forms.ComboBox();
            this.chkRCP2DisPol = new System.Windows.Forms.CheckBox();
            this.tabOther = new System.Windows.Forms.TabPage();
            this.grpWN2 = new System.Windows.Forms.GroupBox();
            this.btnSensor = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.grpC4 = new System.Windows.Forms.GroupBox();
            this.rbC4K = new System.Windows.Forms.RadioButton();
            this.rbC4H = new System.Windows.Forms.RadioButton();
            this.rbC4Q = new System.Windows.Forms.RadioButton();
            this.grpC3 = new System.Windows.Forms.GroupBox();
            this.rbC3K = new System.Windows.Forms.RadioButton();
            this.rbC3H = new System.Windows.Forms.RadioButton();
            this.rbC3Q = new System.Windows.Forms.RadioButton();
            this.grpC2 = new System.Windows.Forms.GroupBox();
            this.rbC2K = new System.Windows.Forms.RadioButton();
            this.rbC2H = new System.Windows.Forms.RadioButton();
            this.rbC2Q = new System.Windows.Forms.RadioButton();
            this.grpC1 = new System.Windows.Forms.GroupBox();
            this.rbC1K = new System.Windows.Forms.RadioButton();
            this.rbC1H = new System.Windows.Forms.RadioButton();
            this.rbC1Q = new System.Windows.Forms.RadioButton();
            this.label63 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.chkWNEnab = new System.Windows.Forms.CheckBox();
            this.rbWN4 = new System.Windows.Forms.RadioButton();
            this.rbWN3 = new System.Windows.Forms.RadioButton();
            this.rbWN2 = new System.Windows.Forms.RadioButton();
            this.rbWN1 = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.cboStepCom = new System.Windows.Forms.ComboBox();
            this.cboStep = new System.Windows.Forms.ComboBox();
            this.txtStepInv = new System.Windows.Forms.TextBox();
            this.chkStep = new System.Windows.Forms.CheckBox();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkDisBcast = new System.Windows.Forms.CheckBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.txtPW1ra = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.cboPW1 = new System.Windows.Forms.ComboBox();
            this.txtPW1ta = new System.Windows.Forms.TextBox();
            this.chkPW1 = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.lblSleep = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnPFfile = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.txtProfLoc = new System.Windows.Forms.TextBox();
            this.grpLP = new System.Windows.Forms.GroupBox();
            this.rb100A = new System.Windows.Forms.RadioButton();
            this.rb100 = new System.Windows.Forms.RadioButton();
            this.lblPower = new System.Windows.Forms.Label();
            this.lblFast = new System.Windows.Forms.Label();
            this.lblAlarm = new System.Windows.Forms.Label();
            this.txtLPint = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnMode = new System.Windows.Forms.Button();
            this.btnFast = new System.Windows.Forms.Button();
            this.btnAlarm = new System.Windows.Forms.Button();
            this.cboLPport = new System.Windows.Forms.ComboBox();
            this.chkLPenab = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSleep = new System.Windows.Forms.TextBox();
            this.tabMisc = new System.Windows.Forms.TabPage();
            this.grpAlpha = new System.Windows.Forms.GroupBox();
            this.label74 = new System.Windows.Forms.Label();
            this.txtLoad = new System.Windows.Forms.TextBox();
            this.txtTune = new System.Windows.Forms.TextBox();
            this.label79 = new System.Windows.Forms.Label();
            this.label80 = new System.Windows.Forms.Label();
            this.txtFreq = new System.Windows.Forms.TextBox();
            this.label78 = new System.Windows.Forms.Label();
            this.cboAlphaBaud = new System.Windows.Forms.ComboBox();
            this.btnHF = new System.Windows.Forms.Button();
            this.btnSF = new System.Windows.Forms.Button();
            this.lblHF = new System.Windows.Forms.Label();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.lblSF = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.txtSeg = new System.Windows.Forms.TextBox();
            this.label70 = new System.Windows.Forms.Label();
            this.txtAlphaInt = new System.Windows.Forms.TextBox();
            this.btnHV = new System.Windows.Forms.Button();
            this.label77 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.cboAlpha = new System.Windows.Forms.ComboBox();
            this.label73 = new System.Windows.Forms.Label();
            this.label72 = new System.Windows.Forms.Label();
            this.btnOper = new System.Windows.Forms.Button();
            this.btnTune = new System.Windows.Forms.Button();
            this.label71 = new System.Windows.Forms.Label();
            this.txtBand = new System.Windows.Forms.TextBox();
            this.chkAlpha = new System.Windows.Forms.CheckBox();
            this.btnPwr = new System.Windows.Forms.Button();
            this.tabMacro = new System.Windows.Forms.TabPage();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.btnMacro12 = new System.Windows.Forms.Button();
            this.btnMacro11 = new System.Windows.Forms.Button();
            this.btnMacro10 = new System.Windows.Forms.Button();
            this.btnMacro9 = new System.Windows.Forms.Button();
            this.btnMacro8 = new System.Windows.Forms.Button();
            this.btnMacro7 = new System.Windows.Forms.Button();
            this.grpMacro = new System.Windows.Forms.GroupBox();
            this.btnMacSelect = new System.Windows.Forms.Button();
            this.btnMacSave = new System.Windows.Forms.Button();
            this.btnMacReLoad = new System.Windows.Forms.Button();
            this.label26 = new System.Windows.Forms.Label();
            this.txtMacFile = new System.Windows.Forms.TextBox();
            this.dgm = new System.Windows.Forms.DataGridView();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.btnMacro6 = new System.Windows.Forms.Button();
            this.btnMacro5 = new System.Windows.Forms.Button();
            this.btnMacro4 = new System.Windows.Forms.Button();
            this.btnMacro3 = new System.Windows.Forms.Button();
            this.btnMacro2 = new System.Windows.Forms.Button();
            this.btnMacro1 = new System.Windows.Forms.Button();
            this.tabRotor = new System.Windows.Forms.TabPage();
            this.grpPreset = new System.Windows.Forms.GroupBox();
            this.rbPre16 = new System.Windows.Forms.RadioButton();
            this.rbPre8 = new System.Windows.Forms.RadioButton();
            this.rbPre15 = new System.Windows.Forms.RadioButton();
            this.rbPre7 = new System.Windows.Forms.RadioButton();
            this.rbPre14 = new System.Windows.Forms.RadioButton();
            this.rbPre6 = new System.Windows.Forms.RadioButton();
            this.rbPre13 = new System.Windows.Forms.RadioButton();
            this.rbPre5 = new System.Windows.Forms.RadioButton();
            this.rbPre12 = new System.Windows.Forms.RadioButton();
            this.rbPre4 = new System.Windows.Forms.RadioButton();
            this.rbPre11 = new System.Windows.Forms.RadioButton();
            this.rbPre3 = new System.Windows.Forms.RadioButton();
            this.rbPre10 = new System.Windows.Forms.RadioButton();
            this.rbPre2 = new System.Windows.Forms.RadioButton();
            this.rbPre9 = new System.Windows.Forms.RadioButton();
            this.rbPre1 = new System.Windows.Forms.RadioButton();
            this.grpStepCtrl = new System.Windows.Forms.GroupBox();
            this.rb34 = new System.Windows.Forms.RadioButton();
            this.rbBiDir = new System.Windows.Forms.RadioButton();
            this.rb180 = new System.Windows.Forms.RadioButton();
            this.rbFwd = new System.Windows.Forms.RadioButton();
            this.btnCalib = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.lblDxTime = new System.Windows.Forms.Label();
            this.txtDxIOTA = new System.Windows.Forms.TextBox();
            this.label62 = new System.Windows.Forms.Label();
            this.txtDxTime = new System.Windows.Forms.TextBox();
            this.label61 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.txtDxITU = new System.Windows.Forms.TextBox();
            this.label57 = new System.Windows.Forms.Label();
            this.txtDxCQ = new System.Windows.Forms.TextBox();
            this.txtDxLat = new System.Windows.Forms.TextBox();
            this.label55 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.txtDxLong = new System.Windows.Forms.TextBox();
            this.label53 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.txtDxCont = new System.Windows.Forms.TextBox();
            this.txtDxDist = new System.Windows.Forms.TextBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label60 = new System.Windows.Forms.Label();
            this.txtRegion = new System.Windows.Forms.TextBox();
            this.cboEntity = new System.Windows.Forms.ComboBox();
            this.cboPrefix = new System.Windows.Forms.ComboBox();
            this.label45 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.lblLP = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.lblSP = new System.Windows.Forms.Label();
            this.btnSP = new System.Windows.Forms.Button();
            this.txtLP = new System.Windows.Forms.TextBox();
            this.btnLP = new System.Windows.Forms.Button();
            this.txtSP = new System.Windows.Forms.TextBox();
            this.tabSetup = new System.Windows.Forms.TabPage();
            this.chkOnTop = new System.Windows.Forms.CheckBox();
            this.chkModeChg = new System.Windows.Forms.CheckBox();
            this.chkAudio = new System.Windows.Forms.CheckBox();
            this.chkTips = new System.Windows.Forms.CheckBox();
            this.grpALC = new System.Windows.Forms.GroupBox();
            this.chk6 = new System.Windows.Forms.CheckBox();
            this.btnChkAll = new System.Windows.Forms.Button();
            this.btnClrAll = new System.Windows.Forms.Button();
            this.chk10 = new System.Windows.Forms.CheckBox();
            this.chk30 = new System.Windows.Forms.CheckBox();
            this.chk12 = new System.Windows.Forms.CheckBox();
            this.chk40 = new System.Windows.Forms.CheckBox();
            this.chk15 = new System.Windows.Forms.CheckBox();
            this.chk17 = new System.Windows.Forms.CheckBox();
            this.chk80 = new System.Windows.Forms.CheckBox();
            this.chk20 = new System.Windows.Forms.CheckBox();
            this.chk160 = new System.Windows.Forms.CheckBox();
            this.label68 = new System.Windows.Forms.Label();
            this.txtALC = new System.Windows.Forms.TextBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnCal = new System.Windows.Forms.Button();
            this.txtDrive = new System.Windows.Forms.TextBox();
            this.label64 = new System.Windows.Forms.Label();
            this.chkAlcEnab = new System.Windows.Forms.CheckBox();
            this.grpDog = new System.Windows.Forms.GroupBox();
            this.txtTimer = new System.Windows.Forms.TextBox();
            this.label67 = new System.Windows.Forms.Label();
            this.chkDog = new System.Windows.Forms.CheckBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label50 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.cboRotorCom = new System.Windows.Forms.ComboBox();
            this.cboRotorPort = new System.Windows.Forms.ComboBox();
            this.grpSpeed = new System.Windows.Forms.GroupBox();
            this.rbRtrSpd3 = new System.Windows.Forms.RadioButton();
            this.rbRtrSpd2 = new System.Windows.Forms.RadioButton();
            this.rbRtrSpd1 = new System.Windows.Forms.RadioButton();
            this.chkRotorEnab = new System.Windows.Forms.CheckBox();
            this.grpModel = new System.Windows.Forms.GroupBox();
            this.rbRtrMod7 = new System.Windows.Forms.RadioButton();
            this.rbRtrMod6 = new System.Windows.Forms.RadioButton();
            this.rbRtrMod5 = new System.Windows.Forms.RadioButton();
            this.rbRtrMod4 = new System.Windows.Forms.RadioButton();
            this.rbRtrMod3 = new System.Windows.Forms.RadioButton();
            this.rbRtrMod2 = new System.Windows.Forms.RadioButton();
            this.rbRtrMod1 = new System.Windows.Forms.RadioButton();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.txtCall = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txtGrid = new System.Windows.Forms.TextBox();
            this.txtLat = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.txtLong = new System.Windows.Forms.TextBox();
            this.tabExtCtrl = new System.Windows.Forms.TabPage();
            this.label132 = new System.Windows.Forms.Label();
            this.label131 = new System.Windows.Forms.Label();
            this.chkPortB = new System.Windows.Forms.CheckBox();
            this.btnClrPortB = new System.Windows.Forms.Button();
            this.btnClrPortA = new System.Windows.Forms.Button();
            this.grpPortB = new System.Windows.Forms.GroupBox();
            this.cb15t4 = new System.Windows.Forms.CheckBox();
            this.label105 = new System.Windows.Forms.Label();
            this.cb15t7 = new System.Windows.Forms.CheckBox();
            this.cb13t4 = new System.Windows.Forms.CheckBox();
            this.cb15t6 = new System.Windows.Forms.CheckBox();
            this.label107 = new System.Windows.Forms.Label();
            this.cb15t5 = new System.Windows.Forms.CheckBox();
            this.cb13t7 = new System.Windows.Forms.CheckBox();
            this.cb15t3 = new System.Windows.Forms.CheckBox();
            this.cb13t6 = new System.Windows.Forms.CheckBox();
            this.cb15t2 = new System.Windows.Forms.CheckBox();
            this.cb0t3 = new System.Windows.Forms.CheckBox();
            this.cb15t1 = new System.Windows.Forms.CheckBox();
            this.cb13t5 = new System.Windows.Forms.CheckBox();
            this.cb15t0 = new System.Windows.Forms.CheckBox();
            this.cb13t3 = new System.Windows.Forms.CheckBox();
            this.label135 = new System.Windows.Forms.Label();
            this.cb0t0 = new System.Windows.Forms.CheckBox();
            this.label136 = new System.Windows.Forms.Label();
            this.cb13t2 = new System.Windows.Forms.CheckBox();
            this.cb14t0 = new System.Windows.Forms.CheckBox();
            this.cb13t1 = new System.Windows.Forms.CheckBox();
            this.cb14t1 = new System.Windows.Forms.CheckBox();
            this.cb13t0 = new System.Windows.Forms.CheckBox();
            this.cb14t2 = new System.Windows.Forms.CheckBox();
            this.cb0t1 = new System.Windows.Forms.CheckBox();
            this.cb14t3 = new System.Windows.Forms.CheckBox();
            this.label108 = new System.Windows.Forms.Label();
            this.cb14t4 = new System.Windows.Forms.CheckBox();
            this.cb0t2 = new System.Windows.Forms.CheckBox();
            this.cb14t5 = new System.Windows.Forms.CheckBox();
            this.cb14t6 = new System.Windows.Forms.CheckBox();
            this.cb0t4 = new System.Windows.Forms.CheckBox();
            this.cb14t7 = new System.Windows.Forms.CheckBox();
            this.cb0t5 = new System.Windows.Forms.CheckBox();
            this.cb0t6 = new System.Windows.Forms.CheckBox();
            this.cb0t7 = new System.Windows.Forms.CheckBox();
            this.label109 = new System.Windows.Forms.Label();
            this.cb12t0 = new System.Windows.Forms.CheckBox();
            this.cb12t1 = new System.Windows.Forms.CheckBox();
            this.cb12t2 = new System.Windows.Forms.CheckBox();
            this.cb12t3 = new System.Windows.Forms.CheckBox();
            this.cb12t4 = new System.Windows.Forms.CheckBox();
            this.cb12t5 = new System.Windows.Forms.CheckBox();
            this.cb12t6 = new System.Windows.Forms.CheckBox();
            this.cb12t7 = new System.Windows.Forms.CheckBox();
            this.label110 = new System.Windows.Forms.Label();
            this.cb11t0 = new System.Windows.Forms.CheckBox();
            this.cb11t1 = new System.Windows.Forms.CheckBox();
            this.cb11t2 = new System.Windows.Forms.CheckBox();
            this.cb11t3 = new System.Windows.Forms.CheckBox();
            this.cb11t4 = new System.Windows.Forms.CheckBox();
            this.cb11t5 = new System.Windows.Forms.CheckBox();
            this.cb11t6 = new System.Windows.Forms.CheckBox();
            this.cb11t7 = new System.Windows.Forms.CheckBox();
            this.label111 = new System.Windows.Forms.Label();
            this.cb10t0 = new System.Windows.Forms.CheckBox();
            this.cb10t1 = new System.Windows.Forms.CheckBox();
            this.cb10t2 = new System.Windows.Forms.CheckBox();
            this.cb10t3 = new System.Windows.Forms.CheckBox();
            this.cb10t4 = new System.Windows.Forms.CheckBox();
            this.cb10t5 = new System.Windows.Forms.CheckBox();
            this.cb10t6 = new System.Windows.Forms.CheckBox();
            this.cb10t7 = new System.Windows.Forms.CheckBox();
            this.label112 = new System.Windows.Forms.Label();
            this.cb9t0 = new System.Windows.Forms.CheckBox();
            this.cb9t1 = new System.Windows.Forms.CheckBox();
            this.cb9t2 = new System.Windows.Forms.CheckBox();
            this.cb9t3 = new System.Windows.Forms.CheckBox();
            this.cb9t4 = new System.Windows.Forms.CheckBox();
            this.cb9t5 = new System.Windows.Forms.CheckBox();
            this.cb9t6 = new System.Windows.Forms.CheckBox();
            this.cb9t7 = new System.Windows.Forms.CheckBox();
            this.label113 = new System.Windows.Forms.Label();
            this.cb8t0 = new System.Windows.Forms.CheckBox();
            this.cb8t1 = new System.Windows.Forms.CheckBox();
            this.cb8t2 = new System.Windows.Forms.CheckBox();
            this.cb8t3 = new System.Windows.Forms.CheckBox();
            this.cb8t4 = new System.Windows.Forms.CheckBox();
            this.cb8t5 = new System.Windows.Forms.CheckBox();
            this.cb8t6 = new System.Windows.Forms.CheckBox();
            this.cb8t7 = new System.Windows.Forms.CheckBox();
            this.label114 = new System.Windows.Forms.Label();
            this.cb7t0 = new System.Windows.Forms.CheckBox();
            this.cb7t1 = new System.Windows.Forms.CheckBox();
            this.cb7t2 = new System.Windows.Forms.CheckBox();
            this.cb7t3 = new System.Windows.Forms.CheckBox();
            this.cb7t4 = new System.Windows.Forms.CheckBox();
            this.cb7t5 = new System.Windows.Forms.CheckBox();
            this.cb7t6 = new System.Windows.Forms.CheckBox();
            this.cb7t7 = new System.Windows.Forms.CheckBox();
            this.label115 = new System.Windows.Forms.Label();
            this.cb6t0 = new System.Windows.Forms.CheckBox();
            this.cb6t1 = new System.Windows.Forms.CheckBox();
            this.cb6t2 = new System.Windows.Forms.CheckBox();
            this.cb6t3 = new System.Windows.Forms.CheckBox();
            this.cb6t4 = new System.Windows.Forms.CheckBox();
            this.cb6t5 = new System.Windows.Forms.CheckBox();
            this.cb6t6 = new System.Windows.Forms.CheckBox();
            this.cb6t7 = new System.Windows.Forms.CheckBox();
            this.label116 = new System.Windows.Forms.Label();
            this.cb5t0 = new System.Windows.Forms.CheckBox();
            this.cb5t1 = new System.Windows.Forms.CheckBox();
            this.cb5t2 = new System.Windows.Forms.CheckBox();
            this.cb5t3 = new System.Windows.Forms.CheckBox();
            this.cb5t4 = new System.Windows.Forms.CheckBox();
            this.cb5t5 = new System.Windows.Forms.CheckBox();
            this.cb5t6 = new System.Windows.Forms.CheckBox();
            this.cb5t7 = new System.Windows.Forms.CheckBox();
            this.label117 = new System.Windows.Forms.Label();
            this.cb4t0 = new System.Windows.Forms.CheckBox();
            this.cb4t1 = new System.Windows.Forms.CheckBox();
            this.cb4t2 = new System.Windows.Forms.CheckBox();
            this.cb4t3 = new System.Windows.Forms.CheckBox();
            this.cb4t4 = new System.Windows.Forms.CheckBox();
            this.cb4t5 = new System.Windows.Forms.CheckBox();
            this.cb4t6 = new System.Windows.Forms.CheckBox();
            this.cb4t7 = new System.Windows.Forms.CheckBox();
            this.label118 = new System.Windows.Forms.Label();
            this.cb3t0 = new System.Windows.Forms.CheckBox();
            this.cb3t1 = new System.Windows.Forms.CheckBox();
            this.cb3t2 = new System.Windows.Forms.CheckBox();
            this.cb3t3 = new System.Windows.Forms.CheckBox();
            this.cb3t4 = new System.Windows.Forms.CheckBox();
            this.cb3t5 = new System.Windows.Forms.CheckBox();
            this.cb3t6 = new System.Windows.Forms.CheckBox();
            this.cb3t7 = new System.Windows.Forms.CheckBox();
            this.label119 = new System.Windows.Forms.Label();
            this.cb2t0 = new System.Windows.Forms.CheckBox();
            this.cb2t1 = new System.Windows.Forms.CheckBox();
            this.cb2t2 = new System.Windows.Forms.CheckBox();
            this.cb2t3 = new System.Windows.Forms.CheckBox();
            this.cb2t4 = new System.Windows.Forms.CheckBox();
            this.cb2t5 = new System.Windows.Forms.CheckBox();
            this.cb2t6 = new System.Windows.Forms.CheckBox();
            this.cb2t7 = new System.Windows.Forms.CheckBox();
            this.label120 = new System.Windows.Forms.Label();
            this.cb1t0 = new System.Windows.Forms.CheckBox();
            this.cb1t1 = new System.Windows.Forms.CheckBox();
            this.cb1t2 = new System.Windows.Forms.CheckBox();
            this.cb1t3 = new System.Windows.Forms.CheckBox();
            this.cb1t4 = new System.Windows.Forms.CheckBox();
            this.cb1t5 = new System.Windows.Forms.CheckBox();
            this.cb1t6 = new System.Windows.Forms.CheckBox();
            this.cb1t7 = new System.Windows.Forms.CheckBox();
            this.label121 = new System.Windows.Forms.Label();
            this.label122 = new System.Windows.Forms.Label();
            this.label123 = new System.Windows.Forms.Label();
            this.label124 = new System.Windows.Forms.Label();
            this.label125 = new System.Windows.Forms.Label();
            this.label126 = new System.Windows.Forms.Label();
            this.label127 = new System.Windows.Forms.Label();
            this.label128 = new System.Windows.Forms.Label();
            this.label130 = new System.Windows.Forms.Label();
            this.grpPortA = new System.Windows.Forms.GroupBox();
            this.cb15r4 = new System.Windows.Forms.CheckBox();
            this.cb15r7 = new System.Windows.Forms.CheckBox();
            this.cb15r6 = new System.Windows.Forms.CheckBox();
            this.cb15r5 = new System.Windows.Forms.CheckBox();
            this.cb15r3 = new System.Windows.Forms.CheckBox();
            this.cb15r2 = new System.Windows.Forms.CheckBox();
            this.cb15r1 = new System.Windows.Forms.CheckBox();
            this.cb15r0 = new System.Windows.Forms.CheckBox();
            this.label133 = new System.Windows.Forms.Label();
            this.label134 = new System.Windows.Forms.Label();
            this.cb14r0 = new System.Windows.Forms.CheckBox();
            this.cb14r1 = new System.Windows.Forms.CheckBox();
            this.cb14r2 = new System.Windows.Forms.CheckBox();
            this.cb14r3 = new System.Windows.Forms.CheckBox();
            this.cb14r4 = new System.Windows.Forms.CheckBox();
            this.cb14r5 = new System.Windows.Forms.CheckBox();
            this.cb14r6 = new System.Windows.Forms.CheckBox();
            this.cb14r7 = new System.Windows.Forms.CheckBox();
            this.label129 = new System.Windows.Forms.Label();
            this.cb13r4 = new System.Windows.Forms.CheckBox();
            this.label104 = new System.Windows.Forms.Label();
            this.cb13r7 = new System.Windows.Forms.CheckBox();
            this.cb13r6 = new System.Windows.Forms.CheckBox();
            this.cb0r3 = new System.Windows.Forms.CheckBox();
            this.cb13r5 = new System.Windows.Forms.CheckBox();
            this.cb13r3 = new System.Windows.Forms.CheckBox();
            this.cb0r0 = new System.Windows.Forms.CheckBox();
            this.cb13r2 = new System.Windows.Forms.CheckBox();
            this.cb13r1 = new System.Windows.Forms.CheckBox();
            this.cb13r0 = new System.Windows.Forms.CheckBox();
            this.cb0r1 = new System.Windows.Forms.CheckBox();
            this.label102 = new System.Windows.Forms.Label();
            this.cb0r2 = new System.Windows.Forms.CheckBox();
            this.cb0r4 = new System.Windows.Forms.CheckBox();
            this.cb0r5 = new System.Windows.Forms.CheckBox();
            this.cb0r6 = new System.Windows.Forms.CheckBox();
            this.cb0r7 = new System.Windows.Forms.CheckBox();
            this.label101 = new System.Windows.Forms.Label();
            this.cb12r0 = new System.Windows.Forms.CheckBox();
            this.cb12r1 = new System.Windows.Forms.CheckBox();
            this.cb12r2 = new System.Windows.Forms.CheckBox();
            this.cb12r3 = new System.Windows.Forms.CheckBox();
            this.cb12r4 = new System.Windows.Forms.CheckBox();
            this.cb12r5 = new System.Windows.Forms.CheckBox();
            this.cb12r6 = new System.Windows.Forms.CheckBox();
            this.cb12r7 = new System.Windows.Forms.CheckBox();
            this.label100 = new System.Windows.Forms.Label();
            this.cb11r0 = new System.Windows.Forms.CheckBox();
            this.cb11r1 = new System.Windows.Forms.CheckBox();
            this.cb11r2 = new System.Windows.Forms.CheckBox();
            this.cb11r3 = new System.Windows.Forms.CheckBox();
            this.cb11r4 = new System.Windows.Forms.CheckBox();
            this.cb11r5 = new System.Windows.Forms.CheckBox();
            this.cb11r6 = new System.Windows.Forms.CheckBox();
            this.cb11r7 = new System.Windows.Forms.CheckBox();
            this.label99 = new System.Windows.Forms.Label();
            this.cb10r0 = new System.Windows.Forms.CheckBox();
            this.cb10r1 = new System.Windows.Forms.CheckBox();
            this.cb10r2 = new System.Windows.Forms.CheckBox();
            this.cb10r3 = new System.Windows.Forms.CheckBox();
            this.cb10r4 = new System.Windows.Forms.CheckBox();
            this.cb10r5 = new System.Windows.Forms.CheckBox();
            this.cb10r6 = new System.Windows.Forms.CheckBox();
            this.cb10r7 = new System.Windows.Forms.CheckBox();
            this.label98 = new System.Windows.Forms.Label();
            this.cb9r0 = new System.Windows.Forms.CheckBox();
            this.cb9r1 = new System.Windows.Forms.CheckBox();
            this.cb9r2 = new System.Windows.Forms.CheckBox();
            this.cb9r3 = new System.Windows.Forms.CheckBox();
            this.cb9r4 = new System.Windows.Forms.CheckBox();
            this.cb9r5 = new System.Windows.Forms.CheckBox();
            this.cb9r6 = new System.Windows.Forms.CheckBox();
            this.cb9r7 = new System.Windows.Forms.CheckBox();
            this.label97 = new System.Windows.Forms.Label();
            this.cb8r0 = new System.Windows.Forms.CheckBox();
            this.cb8r1 = new System.Windows.Forms.CheckBox();
            this.cb8r2 = new System.Windows.Forms.CheckBox();
            this.cb8r3 = new System.Windows.Forms.CheckBox();
            this.cb8r4 = new System.Windows.Forms.CheckBox();
            this.cb8r5 = new System.Windows.Forms.CheckBox();
            this.cb8r6 = new System.Windows.Forms.CheckBox();
            this.cb8r7 = new System.Windows.Forms.CheckBox();
            this.label96 = new System.Windows.Forms.Label();
            this.cb7r0 = new System.Windows.Forms.CheckBox();
            this.cb7r1 = new System.Windows.Forms.CheckBox();
            this.cb7r2 = new System.Windows.Forms.CheckBox();
            this.cb7r3 = new System.Windows.Forms.CheckBox();
            this.cb7r4 = new System.Windows.Forms.CheckBox();
            this.cb7r5 = new System.Windows.Forms.CheckBox();
            this.cb7r6 = new System.Windows.Forms.CheckBox();
            this.cb7r7 = new System.Windows.Forms.CheckBox();
            this.label95 = new System.Windows.Forms.Label();
            this.cb6r0 = new System.Windows.Forms.CheckBox();
            this.cb6r1 = new System.Windows.Forms.CheckBox();
            this.cb6r2 = new System.Windows.Forms.CheckBox();
            this.cb6r3 = new System.Windows.Forms.CheckBox();
            this.cb6r4 = new System.Windows.Forms.CheckBox();
            this.cb6r5 = new System.Windows.Forms.CheckBox();
            this.cb6r6 = new System.Windows.Forms.CheckBox();
            this.cb6r7 = new System.Windows.Forms.CheckBox();
            this.label94 = new System.Windows.Forms.Label();
            this.cb5r0 = new System.Windows.Forms.CheckBox();
            this.cb5r1 = new System.Windows.Forms.CheckBox();
            this.cb5r2 = new System.Windows.Forms.CheckBox();
            this.cb5r3 = new System.Windows.Forms.CheckBox();
            this.cb5r4 = new System.Windows.Forms.CheckBox();
            this.cb5r5 = new System.Windows.Forms.CheckBox();
            this.cb5r6 = new System.Windows.Forms.CheckBox();
            this.cb5r7 = new System.Windows.Forms.CheckBox();
            this.label93 = new System.Windows.Forms.Label();
            this.cb4r0 = new System.Windows.Forms.CheckBox();
            this.cb4r1 = new System.Windows.Forms.CheckBox();
            this.cb4r2 = new System.Windows.Forms.CheckBox();
            this.cb4r3 = new System.Windows.Forms.CheckBox();
            this.cb4r4 = new System.Windows.Forms.CheckBox();
            this.cb4r5 = new System.Windows.Forms.CheckBox();
            this.cb4r6 = new System.Windows.Forms.CheckBox();
            this.cb4r7 = new System.Windows.Forms.CheckBox();
            this.label92 = new System.Windows.Forms.Label();
            this.cb3r0 = new System.Windows.Forms.CheckBox();
            this.cb3r1 = new System.Windows.Forms.CheckBox();
            this.cb3r2 = new System.Windows.Forms.CheckBox();
            this.cb3r3 = new System.Windows.Forms.CheckBox();
            this.cb3r4 = new System.Windows.Forms.CheckBox();
            this.cb3r5 = new System.Windows.Forms.CheckBox();
            this.cb3r6 = new System.Windows.Forms.CheckBox();
            this.cb3r7 = new System.Windows.Forms.CheckBox();
            this.label91 = new System.Windows.Forms.Label();
            this.cb2r0 = new System.Windows.Forms.CheckBox();
            this.cb2r1 = new System.Windows.Forms.CheckBox();
            this.cb2r2 = new System.Windows.Forms.CheckBox();
            this.cb2r3 = new System.Windows.Forms.CheckBox();
            this.cb2r4 = new System.Windows.Forms.CheckBox();
            this.cb2r5 = new System.Windows.Forms.CheckBox();
            this.cb2r6 = new System.Windows.Forms.CheckBox();
            this.cb2r7 = new System.Windows.Forms.CheckBox();
            this.label90 = new System.Windows.Forms.Label();
            this.cb1r0 = new System.Windows.Forms.CheckBox();
            this.cb1r1 = new System.Windows.Forms.CheckBox();
            this.cb1r2 = new System.Windows.Forms.CheckBox();
            this.cb1r3 = new System.Windows.Forms.CheckBox();
            this.cb1r4 = new System.Windows.Forms.CheckBox();
            this.cb1r5 = new System.Windows.Forms.CheckBox();
            this.cb1r6 = new System.Windows.Forms.CheckBox();
            this.cb1r7 = new System.Windows.Forms.CheckBox();
            this.label89 = new System.Windows.Forms.Label();
            this.label88 = new System.Windows.Forms.Label();
            this.label87 = new System.Windows.Forms.Label();
            this.label86 = new System.Windows.Forms.Label();
            this.label85 = new System.Windows.Forms.Label();
            this.label84 = new System.Windows.Forms.Label();
            this.label83 = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.label81 = new System.Windows.Forms.Label();
            this.chkPortA = new System.Windows.Forms.CheckBox();
            this.txtPortA = new System.Windows.Forms.TextBox();
            this.label103 = new System.Windows.Forms.Label();
            this.txtPortB = new System.Windows.Forms.TextBox();
            this.label106 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.AccPort = new System.IO.Ports.SerialPort(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtFwd = new System.Windows.Forms.TextBox();
            this.txtSWR = new System.Windows.Forms.TextBox();
            this.btnProfiler = new System.Windows.Forms.Button();
            this.btnReStart = new System.Windows.Forms.Button();
            this.txtTemp = new System.Windows.Forms.TextBox();
            this.lblAnt = new System.Windows.Forms.Label();
            this.txtAvg = new System.Windows.Forms.TextBox();
            this.LogPort = new System.IO.Ports.SerialPort(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.flexProfilerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iCOMICPW1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lP100ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.macroFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rCPPortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotorControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waveNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.LPport = new System.IO.Ports.SerialPort(this.components);
            this.lblFwd = new System.Windows.Forms.Label();
            this.lblSWR = new System.Windows.Forms.Label();
            this.RCP2port = new System.IO.Ports.SerialPort(this.components);
            this.RCP3port = new System.IO.Ports.SerialPort(this.components);
            this.RCP4port = new System.IO.Ports.SerialPort(this.components);
            this.PW1port = new System.IO.Ports.SerialPort(this.components);
            this.TestPort = new System.IO.Ports.SerialPort(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableErrorLoggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openErrorLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearErrorLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showMiniWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onLineHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupWizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.StepData = new System.IO.Ports.SerialPort(this.components);
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.radioButton11 = new System.Windows.Forms.RadioButton();
            this.radioButton12 = new System.Windows.Forms.RadioButton();
            this.radioButton13 = new System.Windows.Forms.RadioButton();
            this.radioButton14 = new System.Windows.Forms.RadioButton();
            this.radioButton15 = new System.Windows.Forms.RadioButton();
            this.RotorPort = new System.IO.Ports.SerialPort(this.components);
            this.RCP1Rotor = new System.IO.Ports.SerialPort(this.components);
            this.RCP2Rotor = new System.IO.Ports.SerialPort(this.components);
            this.RCP3Rotor = new System.IO.Ports.SerialPort(this.components);
            this.RCP4Rotor = new System.IO.Ports.SerialPort(this.components);
            this.lblAvg = new System.Windows.Forms.Label();
            this.WN2Timer = new System.Windows.Forms.Timer(this.components);
            this.AlphaPort = new System.IO.Ports.SerialPort(this.components);
            this.lblVHF = new System.Windows.Forms.Label();
            this.label137 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPorts.SuspendLayout();
            this.grpSlave.SuspendLayout();
            this.grpBox1.SuspendLayout();
            this.grpBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.grpLPT.SuspendLayout();
            this.tabDevice.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg1)).BeginInit();
            this.tabRCP.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabOther.SuspendLayout();
            this.grpWN2.SuspendLayout();
            this.grpC4.SuspendLayout();
            this.grpC3.SuspendLayout();
            this.grpC2.SuspendLayout();
            this.grpC1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpLP.SuspendLayout();
            this.tabMisc.SuspendLayout();
            this.grpAlpha.SuspendLayout();
            this.tabMacro.SuspendLayout();
            this.grpMacro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgm)).BeginInit();
            this.tabRotor.SuspendLayout();
            this.grpPreset.SuspendLayout();
            this.grpStepCtrl.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tabSetup.SuspendLayout();
            this.grpALC.SuspendLayout();
            this.grpDog.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.grpSpeed.SuspendLayout();
            this.grpModel.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.tabExtCtrl.SuspendLayout();
            this.grpPortB.SuspendLayout();
            this.grpPortA.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPorts);
            this.tabControl.Controls.Add(this.tabDevice);
            this.tabControl.Controls.Add(this.tabRCP);
            this.tabControl.Controls.Add(this.tabOther);
            this.tabControl.Controls.Add(this.tabMisc);
            this.tabControl.Controls.Add(this.tabMacro);
            this.tabControl.Controls.Add(this.tabRotor);
            this.tabControl.Controls.Add(this.tabSetup);
            this.tabControl.Controls.Add(this.tabExtCtrl);
            this.tabControl.ItemSize = new System.Drawing.Size(42, 18);
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(4, 3);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(425, 335);
            this.tabControl.TabIndex = 31;
            this.toolTip1.SetToolTip(this.tabControl, "Right-Click for Options");
            // 
            // tabPorts
            // 
            this.tabPorts.BackColor = System.Drawing.Color.Transparent;
            this.tabPorts.Controls.Add(this.grpSlave);
            this.tabPorts.Controls.Add(this.label5);
            this.tabPorts.Controls.Add(this.cboSerAcc);
            this.tabPorts.Controls.Add(this.grpBox1);
            this.tabPorts.Controls.Add(this.grpBox2);
            this.tabPorts.Location = new System.Drawing.Point(4, 22);
            this.tabPorts.Name = "tabPorts";
            this.tabPorts.Padding = new System.Windows.Forms.Padding(3);
            this.tabPorts.Size = new System.Drawing.Size(417, 309);
            this.tabPorts.TabIndex = 0;
            this.tabPorts.Text = "Ports";
            this.toolTip1.SetToolTip(this.tabPorts, "Right-Click for Options");
            this.tabPorts.UseVisualStyleBackColor = true;
            // 
            // grpSlave
            // 
            this.grpSlave.Controls.Add(this.chkFollow);
            this.grpSlave.Controls.Add(this.chkMode);
            this.grpSlave.Controls.Add(this.cboRadio);
            this.grpSlave.Controls.Add(this.label12);
            this.grpSlave.Controls.Add(this.label8);
            this.grpSlave.Controls.Add(this.cboRadData);
            this.grpSlave.Controls.Add(this.txtRadNum);
            this.grpSlave.Controls.Add(this.label9);
            this.grpSlave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpSlave.ForeColor = System.Drawing.Color.Navy;
            this.grpSlave.Location = new System.Drawing.Point(5, 235);
            this.grpSlave.Name = "grpSlave";
            this.grpSlave.Size = new System.Drawing.Size(405, 65);
            this.grpSlave.TabIndex = 31;
            this.grpSlave.TabStop = false;
            this.grpSlave.Text = "Slave Radio";
            // 
            // chkFollow
            // 
            this.chkFollow.AutoSize = true;
            this.chkFollow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkFollow.ForeColor = System.Drawing.Color.Navy;
            this.chkFollow.Location = new System.Drawing.Point(5, 20);
            this.chkFollow.Name = "chkFollow";
            this.chkFollow.Size = new System.Drawing.Size(82, 17);
            this.chkFollow.TabIndex = 26;
            this.chkFollow.Text = "Follow SDR";
            this.toolTip1.SetToolTip(this.chkFollow, "When selected, directs a Slave Radio connected to the Passive Listener port to fo" +
                    "llow the PowerSDR frequency .");
            this.chkFollow.UseVisualStyleBackColor = true;
            this.chkFollow.CheckedChanged += new System.EventHandler(this.chkFollow_CheckedChanged);
            // 
            // chkMode
            // 
            this.chkMode.AutoSize = true;
            this.chkMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkMode.ForeColor = System.Drawing.Color.Navy;
            this.chkMode.Location = new System.Drawing.Point(5, 40);
            this.chkMode.Name = "chkMode";
            this.chkMode.Size = new System.Drawing.Size(86, 17);
            this.chkMode.TabIndex = 30;
            this.chkMode.Text = "Follow Mode";
            this.toolTip1.SetToolTip(this.chkMode, "When selected, will broadcast PowerSDR mode to Slave radio.");
            this.chkMode.UseVisualStyleBackColor = true;
            this.chkMode.CheckedChanged += new System.EventHandler(this.chkMode_CheckedChanged);
            // 
            // cboRadio
            // 
            this.cboRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRadio.FormattingEnabled = true;
            this.cboRadio.Items.AddRange(new object[] {
            "None",
            "Kenwood",
            "Yaesu Type 1",
            "Yaesu Type 2",
            "Icom"});
            this.cboRadio.Location = new System.Drawing.Point(105, 19);
            this.cboRadio.Name = "cboRadio";
            this.cboRadio.Size = new System.Drawing.Size(89, 21);
            this.cboRadio.TabIndex = 26;
            this.toolTip1.SetToolTip(this.cboRadio, "Select Radio to Follow PowerSDR (right click on background for menu). ");
            this.cboRadio.SelectedIndexChanged += new System.EventHandler(this.cboRadio_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label12.ForeColor = System.Drawing.Color.Navy;
            this.label12.Location = new System.Drawing.Point(216, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 13);
            this.label12.TabIndex = 29;
            this.label12.Text = "PL/Radio Comm";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label8.ForeColor = System.Drawing.Color.Navy;
            this.label8.Location = new System.Drawing.Point(99, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Slave Radio Type";
            // 
            // cboRadData
            // 
            this.cboRadData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRadData.FormattingEnabled = true;
            this.cboRadData.Items.AddRange(new object[] {
            "9600 8-N-1",
            "9600 8-N-2",
            "4800 8-N-1",
            "4800 8-N-2",
            "2400 8-N-1",
            "2400 8-N-2",
            "1200 8-N-1",
            "1200 8-N-2"});
            this.cboRadData.Location = new System.Drawing.Point(217, 19);
            this.cboRadData.Name = "cboRadData";
            this.cboRadData.Size = new System.Drawing.Size(83, 21);
            this.cboRadData.TabIndex = 28;
            this.toolTip1.SetToolTip(this.cboRadData, "Sets Passive Listener / Slave Radio port comm parameters.");
            this.cboRadData.SelectedIndexChanged += new System.EventHandler(this.cboRadData_SelectedIndexChanged);
            // 
            // txtRadNum
            // 
            this.txtRadNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRadNum.Location = new System.Drawing.Point(334, 19);
            this.txtRadNum.Name = "txtRadNum";
            this.txtRadNum.Size = new System.Drawing.Size(32, 20);
            this.txtRadNum.TabIndex = 15;
            this.txtRadNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRadNum, "CI-V Address");
            this.txtRadNum.TextChanged += new System.EventHandler(this.txtRadNum_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label9.ForeColor = System.Drawing.Color.Navy;
            this.label9.Location = new System.Drawing.Point(326, 41);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "CI-V Addr";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.ForeColor = System.Drawing.Color.Firebrick;
            this.label5.Location = new System.Drawing.Point(164, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 15);
            this.label5.TabIndex = 3;
            this.label5.Text = "Pass Listen";
            this.toolTip1.SetToolTip(this.label5, "Select Passive Listener Port (A hardware serial port)");
            // 
            // cboSerAcc
            // 
            this.cboSerAcc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSerAcc.FormattingEnabled = true;
            this.cboSerAcc.Location = new System.Drawing.Point(165, 30);
            this.cboSerAcc.Name = "cboSerAcc";
            this.cboSerAcc.Size = new System.Drawing.Size(65, 21);
            this.cboSerAcc.TabIndex = 2;
            this.toolTip1.SetToolTip(this.cboSerAcc, "Select Passive Listener Port (must be hardware serial port)");
            this.cboSerAcc.SelectedIndexChanged += new System.EventHandler(this.cboSerAcc_SelectedIndexChanged);
            // 
            // grpBox1
            // 
            this.grpBox1.Controls.Add(this.label46);
            this.grpBox1.Controls.Add(this.cboRCP1Rotor);
            this.grpBox1.Controls.Add(this.label11);
            this.grpBox1.Controls.Add(this.cboLogPort);
            this.grpBox1.Controls.Add(this.label6);
            this.grpBox1.Controls.Add(this.txtInv);
            this.grpBox1.Controls.Add(this.label3);
            this.grpBox1.Controls.Add(this.cboCAT);
            this.grpBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBox1.ForeColor = System.Drawing.Color.Navy;
            this.grpBox1.Location = new System.Drawing.Point(5, 10);
            this.grpBox1.Name = "grpBox1";
            this.grpBox1.Size = new System.Drawing.Size(405, 65);
            this.grpBox1.TabIndex = 19;
            this.grpBox1.TabStop = false;
            this.grpBox1.Text = "Serial Ports";
            this.toolTip1.SetToolTip(this.grpBox1, "Right-Click for Options");
            // 
            // label46
            // 
            this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label46.ForeColor = System.Drawing.Color.Navy;
            this.label46.Location = new System.Drawing.Point(331, 43);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(67, 15);
            this.label46.TabIndex = 16;
            this.label46.Text = "RCP1 Rotor";
            this.toolTip1.SetToolTip(this.label46, "Select Master RCP1 Rotor Port");
            // 
            // cboRCP1Rotor
            // 
            this.cboRCP1Rotor.Enabled = false;
            this.cboRCP1Rotor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRCP1Rotor.FormattingEnabled = true;
            this.cboRCP1Rotor.Location = new System.Drawing.Point(330, 20);
            this.cboRCP1Rotor.Name = "cboRCP1Rotor";
            this.cboRCP1Rotor.Size = new System.Drawing.Size(65, 21);
            this.cboRCP1Rotor.TabIndex = 15;
            this.toolTip1.SetToolTip(this.cboRCP1Rotor, "Radio Control Program 1 Rotor Port");
            this.cboRCP1Rotor.SelectedIndexChanged += new System.EventHandler(this.cboRCP1Rotor_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label11.ForeColor = System.Drawing.Color.Navy;
            this.label11.Location = new System.Drawing.Point(245, 43);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 15);
            this.label11.TabIndex = 14;
            this.label11.Text = "RCP1 CAT";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.label11, "Select Master RCP1 CAT Port");
            // 
            // cboLogPort
            // 
            this.cboLogPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLogPort.FormattingEnabled = true;
            this.cboLogPort.Location = new System.Drawing.Point(245, 20);
            this.cboLogPort.Name = "cboLogPort";
            this.cboLogPort.Size = new System.Drawing.Size(65, 21);
            this.cboLogPort.TabIndex = 13;
            this.toolTip1.SetToolTip(this.cboLogPort, "Radio Control Program 1 CAT Port");
            this.cboLogPort.SelectedIndexChanged += new System.EventHandler(this.cboLogPort_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label6.ForeColor = System.Drawing.Color.Navy;
            this.label6.Location = new System.Drawing.Point(95, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "Poll Intvl";
            // 
            // txtInv
            // 
            this.txtInv.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInv.Location = new System.Drawing.Point(95, 20);
            this.txtInv.Name = "txtInv";
            this.txtInv.Size = new System.Drawing.Size(45, 20);
            this.txtInv.TabIndex = 11;
            this.txtInv.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtInv, "Radio CAT polling interval (with DDUtil only running)");
            this.txtInv.TextChanged += new System.EventHandler(this.txtInv_TextChanged);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(8, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Radio CAT";
            this.toolTip1.SetToolTip(this.label3, "Select Radio CAT Serial Port");
            // 
            // cboCAT
            // 
            this.cboCAT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCAT.FormattingEnabled = true;
            this.cboCAT.Location = new System.Drawing.Point(10, 19);
            this.cboCAT.Name = "cboCAT";
            this.cboCAT.Size = new System.Drawing.Size(65, 21);
            this.cboCAT.TabIndex = 0;
            this.toolTip1.SetToolTip(this.cboCAT, "Radio CAT Serial Port (virtual)");
            this.cboCAT.SelectedIndexChanged += new System.EventHandler(this.cboCAT_SelectedIndexChanged);
            // 
            // grpBox2
            // 
            this.grpBox2.Controls.Add(this.lblPortBtn);
            this.grpBox2.Controls.Add(this.btnPortNum);
            this.grpBox2.Controls.Add(this.label7);
            this.grpBox2.Controls.Add(this.groupBox5);
            this.grpBox2.Controls.Add(this.grpLPT);
            this.grpBox2.Controls.Add(this.txtPort);
            this.grpBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpBox2.ForeColor = System.Drawing.Color.Navy;
            this.grpBox2.Location = new System.Drawing.Point(5, 85);
            this.grpBox2.Name = "grpBox2";
            this.grpBox2.Size = new System.Drawing.Size(405, 139);
            this.grpBox2.TabIndex = 18;
            this.grpBox2.TabStop = false;
            this.grpBox2.Text = "Parallel Port";
            this.toolTip1.SetToolTip(this.grpBox2, "Right-Click for Options");
            // 
            // lblPortBtn
            // 
            this.lblPortBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortBtn.Location = new System.Drawing.Point(250, 92);
            this.lblPortBtn.Name = "lblPortBtn";
            this.lblPortBtn.Size = new System.Drawing.Size(100, 26);
            this.lblPortBtn.TabIndex = 30;
            this.lblPortBtn.Text = "Press to save new LPT port address";
            this.lblPortBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPortBtn.Visible = false;
            // 
            // btnPortNum
            // 
            this.btnPortNum.BackColor = System.Drawing.Color.Orchid;
            this.btnPortNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPortNum.Location = new System.Drawing.Point(222, 95);
            this.btnPortNum.Name = "btnPortNum";
            this.btnPortNum.Size = new System.Drawing.Size(19, 19);
            this.btnPortNum.TabIndex = 29;
            this.toolTip1.SetToolTip(this.btnPortNum, "Press to save non-standard port number");
            this.btnPortNum.UseVisualStyleBackColor = false;
            this.btnPortNum.Click += new System.EventHandler(this.btnPortNum_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(145, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "Port Number";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblBCD);
            this.groupBox5.Controls.Add(this.cboDevice);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.chkDevice);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox5.Location = new System.Drawing.Point(150, 25);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(210, 60);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Data Signals";
            this.toolTip1.SetToolTip(this.groupBox5, "Right-Click for Options");
            // 
            // lblBCD
            // 
            this.lblBCD.AutoSize = true;
            this.lblBCD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblBCD.ForeColor = System.Drawing.Color.Maroon;
            this.lblBCD.Location = new System.Drawing.Point(22, 39);
            this.lblBCD.Name = "lblBCD";
            this.lblBCD.Size = new System.Drawing.Size(13, 13);
            this.lblBCD.TabIndex = 26;
            this.lblBCD.Text = "0";
            // 
            // cboDevice
            // 
            this.cboDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(88, 16);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(116, 21);
            this.cboDevice.TabIndex = 22;
            this.toolTip1.SetToolTip(this.cboDevice, "Select Device to use for LPT Data Source.");
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            this.cboDevice.DropDown += new System.EventHandler(this.Dev0_DropDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(120, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Device";
            // 
            // chkDevice
            // 
            this.chkDevice.AutoSize = true;
            this.chkDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkDevice.Location = new System.Drawing.Point(23, 20);
            this.chkDevice.Name = "chkDevice";
            this.chkDevice.Size = new System.Drawing.Size(59, 17);
            this.chkDevice.TabIndex = 19;
            this.chkDevice.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkDevice, "Selects to output data on seleced LPT port from selected Device.");
            this.chkDevice.UseVisualStyleBackColor = true;
            this.chkDevice.CheckedChanged += new System.EventHandler(this.chkDevice_CheckedChanged);
            // 
            // grpLPT
            // 
            this.grpLPT.Controls.Add(this.rbOther);
            this.grpLPT.Controls.Add(this.rb1);
            this.grpLPT.Controls.Add(this.rb4);
            this.grpLPT.Controls.Add(this.rb2);
            this.grpLPT.Controls.Add(this.rbNone);
            this.grpLPT.Controls.Add(this.rb3);
            this.grpLPT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpLPT.Location = new System.Drawing.Point(10, 25);
            this.grpLPT.Name = "grpLPT";
            this.grpLPT.Size = new System.Drawing.Size(122, 100);
            this.grpLPT.TabIndex = 0;
            this.grpLPT.TabStop = false;
            this.grpLPT.Text = "Port Selector";
            this.toolTip1.SetToolTip(this.grpLPT, "Select Parallel Port");
            // 
            // rbOther
            // 
            this.rbOther.AutoSize = true;
            this.rbOther.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOther.Location = new System.Drawing.Point(63, 20);
            this.rbOther.Name = "rbOther";
            this.rbOther.Size = new System.Drawing.Size(51, 17);
            this.rbOther.TabIndex = 27;
            this.rbOther.Text = "Other";
            this.rbOther.UseVisualStyleBackColor = true;
            this.rbOther.CheckedChanged += new System.EventHandler(this.grpLPT_CheckedChanged);
            // 
            // rb1
            // 
            this.rb1.AutoSize = true;
            this.rb1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rb1.Location = new System.Drawing.Point(6, 43);
            this.rb1.Name = "rb1";
            this.rb1.Size = new System.Drawing.Size(51, 17);
            this.rb1.TabIndex = 17;
            this.rb1.Text = "LPT1";
            this.rb1.UseVisualStyleBackColor = true;
            this.rb1.CheckedChanged += new System.EventHandler(this.grpLPT_CheckedChanged);
            // 
            // rb4
            // 
            this.rb4.AutoSize = true;
            this.rb4.BackColor = System.Drawing.Color.Transparent;
            this.rb4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rb4.Location = new System.Drawing.Point(63, 66);
            this.rb4.Name = "rb4";
            this.rb4.Size = new System.Drawing.Size(51, 17);
            this.rb4.TabIndex = 23;
            this.rb4.Text = "LPT4";
            this.rb4.UseVisualStyleBackColor = false;
            this.rb4.CheckedChanged += new System.EventHandler(this.grpLPT_CheckedChanged);
            // 
            // rb2
            // 
            this.rb2.AutoSize = true;
            this.rb2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rb2.Location = new System.Drawing.Point(6, 66);
            this.rb2.Name = "rb2";
            this.rb2.Size = new System.Drawing.Size(51, 17);
            this.rb2.TabIndex = 21;
            this.rb2.Text = "LPT2";
            this.rb2.UseVisualStyleBackColor = true;
            this.rb2.CheckedChanged += new System.EventHandler(this.grpLPT_CheckedChanged);
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbNone.Location = new System.Drawing.Point(6, 20);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 26;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            this.rbNone.CheckedChanged += new System.EventHandler(this.grpLPT_CheckedChanged);
            // 
            // rb3
            // 
            this.rb3.AutoSize = true;
            this.rb3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rb3.Location = new System.Drawing.Point(63, 43);
            this.rb3.Name = "rb3";
            this.rb3.Size = new System.Drawing.Size(51, 17);
            this.rb3.TabIndex = 20;
            this.rb3.Text = "LPT3";
            this.rb3.UseVisualStyleBackColor = true;
            this.rb3.CheckedChanged += new System.EventHandler(this.grpLPT_CheckedChanged);
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.HideSelection = false;
            this.txtPort.Location = new System.Drawing.Point(149, 92);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(54, 20);
            this.txtPort.TabIndex = 10;
            this.toolTip1.SetToolTip(this.txtPort, "Decimal number of selected port. You can change this to any number (see help file" +
                    ").");
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // tabDevice
            // 
            this.tabDevice.Controls.Add(this.label20);
            this.tabDevice.Controls.Add(this.groupBox1);
            this.tabDevice.Controls.Add(this.dg1);
            this.tabDevice.Controls.Add(this.chkDev0);
            this.tabDevice.Controls.Add(this.Dev0);
            this.tabDevice.Controls.Add(this.label27);
            this.tabDevice.Location = new System.Drawing.Point(4, 22);
            this.tabDevice.Name = "tabDevice";
            this.tabDevice.Padding = new System.Windows.Forms.Padding(3);
            this.tabDevice.Size = new System.Drawing.Size(417, 309);
            this.tabDevice.TabIndex = 1;
            this.tabDevice.Text = "Device";
            this.toolTip1.SetToolTip(this.tabDevice, "Right-Click for Options");
            this.tabDevice.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.Blue;
            this.label20.Location = new System.Drawing.Point(27, 10);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(161, 24);
            this.label20.TabIndex = 61;
            this.label20.Text = "BCD Device Setup";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSave0);
            this.groupBox1.Controls.Add(this.btnReLoad0);
            this.groupBox1.Controls.Add(this.btnFile0);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtFile0);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.Navy;
            this.groupBox1.Location = new System.Drawing.Point(6, 223);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 79);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data File";
            this.toolTip1.SetToolTip(this.groupBox1, "Right-Click for Options");
            // 
            // btnSave0
            // 
            this.btnSave0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnSave0.Location = new System.Drawing.Point(253, 47);
            this.btnSave0.Name = "btnSave0";
            this.btnSave0.Size = new System.Drawing.Size(57, 23);
            this.btnSave0.TabIndex = 62;
            this.btnSave0.Text = "Save";
            this.toolTip1.SetToolTip(this.btnSave0, "Saves modified data file to file name listed in the data file box above.");
            this.btnSave0.UseVisualStyleBackColor = true;
            this.btnSave0.Click += new System.EventHandler(this.btnSave0_Click);
            // 
            // btnReLoad0
            // 
            this.btnReLoad0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnReLoad0.Location = new System.Drawing.Point(175, 47);
            this.btnReLoad0.Name = "btnReLoad0";
            this.btnReLoad0.Size = new System.Drawing.Size(57, 23);
            this.btnReLoad0.TabIndex = 61;
            this.btnReLoad0.Text = "Re-Load";
            this.toolTip1.SetToolTip(this.btnReLoad0, "Re-loads selected data file.");
            this.btnReLoad0.UseVisualStyleBackColor = true;
            this.btnReLoad0.Click += new System.EventHandler(this.btnReLoad0_Click);
            // 
            // btnFile0
            // 
            this.btnFile0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnFile0.Location = new System.Drawing.Point(95, 47);
            this.btnFile0.Name = "btnFile0";
            this.btnFile0.Size = new System.Drawing.Size(57, 23);
            this.btnFile0.TabIndex = 60;
            this.btnFile0.Text = "Select";
            this.toolTip1.SetToolTip(this.btnFile0, "Opens file dialog to select file to use for this device.");
            this.btnFile0.UseVisualStyleBackColor = true;
            this.btnFile0.Click += new System.EventHandler(this.btnFile0_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label4.Location = new System.Drawing.Point(10, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 59;
            this.label4.Text = "File Name";
            // 
            // txtFile0
            // 
            this.txtFile0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtFile0.Location = new System.Drawing.Point(6, 19);
            this.txtFile0.Name = "txtFile0";
            this.txtFile0.Size = new System.Drawing.Size(391, 20);
            this.txtFile0.TabIndex = 58;
            this.toolTip1.SetToolTip(this.txtFile0, "Data file to use for this device.");
            this.txtFile0.WordWrap = false;
            // 
            // dg1
            // 
            this.dg1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg1.Location = new System.Drawing.Point(217, 6);
            this.dg1.Name = "dg1";
            this.dg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dg1.Size = new System.Drawing.Size(186, 205);
            this.dg1.TabIndex = 42;
            this.dg1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg1_CellValueChanged);
            // 
            // chkDev0
            // 
            this.chkDev0.AutoSize = true;
            this.chkDev0.Location = new System.Drawing.Point(17, 53);
            this.chkDev0.Name = "chkDev0";
            this.chkDev0.Size = new System.Drawing.Size(65, 17);
            this.chkDev0.TabIndex = 44;
            this.chkDev0.Text = "Enabled";
            this.toolTip1.SetToolTip(this.chkDev0, "Check to enable this device");
            this.chkDev0.UseVisualStyleBackColor = true;
            this.chkDev0.CheckedChanged += new System.EventHandler(this.chkDev0_CheckedChanged);
            // 
            // Dev0
            // 
            this.Dev0.Location = new System.Drawing.Point(90, 49);
            this.Dev0.Name = "Dev0";
            this.Dev0.Size = new System.Drawing.Size(100, 20);
            this.Dev0.TabIndex = 51;
            this.toolTip1.SetToolTip(this.Dev0, "Device name (Amp, Antenna Switch)");
            this.Dev0.TextChanged += new System.EventHandler(this.Dev0_TextChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(92, 72);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(72, 13);
            this.label27.TabIndex = 52;
            this.label27.Text = "Device Name";
            // 
            // tabRCP
            // 
            this.tabRCP.Controls.Add(this.groupBox7);
            this.tabRCP.Location = new System.Drawing.Point(4, 22);
            this.tabRCP.Name = "tabRCP";
            this.tabRCP.Size = new System.Drawing.Size(417, 309);
            this.tabRCP.TabIndex = 3;
            this.tabRCP.Text = "RCP";
            this.tabRCP.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label18);
            this.groupBox7.Controls.Add(this.label16);
            this.groupBox7.Controls.Add(this.label66);
            this.groupBox7.Controls.Add(this.label65);
            this.groupBox7.Controls.Add(this.cboRCP4Rotor);
            this.groupBox7.Controls.Add(this.cboRCP3Rotor);
            this.groupBox7.Controls.Add(this.cboRCP2Rotor);
            this.groupBox7.Controls.Add(this.label40);
            this.groupBox7.Controls.Add(this.chkRCP2);
            this.groupBox7.Controls.Add(this.txtRCP);
            this.groupBox7.Controls.Add(this.cboRCP2);
            this.groupBox7.Controls.Add(this.chkRCP4IF);
            this.groupBox7.Controls.Add(this.label17);
            this.groupBox7.Controls.Add(this.chkRCP3IF);
            this.groupBox7.Controls.Add(this.chkRCP3);
            this.groupBox7.Controls.Add(this.chkRCP2IF);
            this.groupBox7.Controls.Add(this.cboRCP3);
            this.groupBox7.Controls.Add(this.chkRCP4DisPol);
            this.groupBox7.Controls.Add(this.chkRCP4);
            this.groupBox7.Controls.Add(this.chkRCP3DisPol);
            this.groupBox7.Controls.Add(this.cboRCP4);
            this.groupBox7.Controls.Add(this.chkRCP2DisPol);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox7.ForeColor = System.Drawing.Color.Navy;
            this.groupBox7.Location = new System.Drawing.Point(5, 15);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(403, 225);
            this.groupBox7.TabIndex = 66;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Radio Control Programs";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label18.Location = new System.Drawing.Point(5, 110);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 13);
            this.label18.TabIndex = 74;
            this.label18.Text = "RCP4";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label16.Location = new System.Drawing.Point(5, 75);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 13);
            this.label16.TabIndex = 73;
            this.label16.Text = "RCP3";
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label66.Location = new System.Drawing.Point(315, 21);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(80, 13);
            this.label66.TabIndex = 72;
            this.label66.Text = "RCP Rotor Port";
            this.label66.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label65.Location = new System.Drawing.Point(115, 21);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(75, 13);
            this.label65.TabIndex = 71;
            this.label65.Text = "RCP CAT Port";
            this.label65.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cboRCP4Rotor
            // 
            this.cboRCP4Rotor.Enabled = false;
            this.cboRCP4Rotor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboRCP4Rotor.FormattingEnabled = true;
            this.cboRCP4Rotor.Location = new System.Drawing.Point(325, 109);
            this.cboRCP4Rotor.Name = "cboRCP4Rotor";
            this.cboRCP4Rotor.Size = new System.Drawing.Size(63, 21);
            this.cboRCP4Rotor.TabIndex = 67;
            this.toolTip1.SetToolTip(this.cboRCP4Rotor, "Rotor Port for RCP 4");
            this.cboRCP4Rotor.SelectedIndexChanged += new System.EventHandler(this.cboRCP4Rotor_SelectedIndexChanged);
            // 
            // cboRCP3Rotor
            // 
            this.cboRCP3Rotor.Enabled = false;
            this.cboRCP3Rotor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboRCP3Rotor.FormattingEnabled = true;
            this.cboRCP3Rotor.Location = new System.Drawing.Point(325, 72);
            this.cboRCP3Rotor.Name = "cboRCP3Rotor";
            this.cboRCP3Rotor.Size = new System.Drawing.Size(63, 21);
            this.cboRCP3Rotor.TabIndex = 66;
            this.toolTip1.SetToolTip(this.cboRCP3Rotor, "Rotor Port for RCP 3");
            this.cboRCP3Rotor.SelectedIndexChanged += new System.EventHandler(this.cboRCP3Rotor_SelectedIndexChanged);
            // 
            // cboRCP2Rotor
            // 
            this.cboRCP2Rotor.Enabled = false;
            this.cboRCP2Rotor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboRCP2Rotor.FormattingEnabled = true;
            this.cboRCP2Rotor.Location = new System.Drawing.Point(325, 37);
            this.cboRCP2Rotor.Name = "cboRCP2Rotor";
            this.cboRCP2Rotor.Size = new System.Drawing.Size(63, 21);
            this.cboRCP2Rotor.TabIndex = 65;
            this.toolTip1.SetToolTip(this.cboRCP2Rotor, "Rotor Port for RCP 2");
            this.cboRCP2Rotor.SelectedIndexChanged += new System.EventHandler(this.cboRCP2Rotor_SelectedIndexChanged);
            // 
            // label40
            // 
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label40.Location = new System.Drawing.Point(150, 205);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(100, 13);
            this.label40.TabIndex = 64;
            this.label40.Text = "Program Memo";
            this.label40.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // chkRCP2
            // 
            this.chkRCP2.AutoSize = true;
            this.chkRCP2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP2.Location = new System.Drawing.Point(50, 40);
            this.chkRCP2.Name = "chkRCP2";
            this.chkRCP2.Size = new System.Drawing.Size(65, 17);
            this.chkRCP2.TabIndex = 48;
            this.chkRCP2.Text = "Enabled";
            this.toolTip1.SetToolTip(this.chkRCP2, "Check to enable RCP 2");
            this.chkRCP2.UseVisualStyleBackColor = true;
            this.chkRCP2.CheckedChanged += new System.EventHandler(this.chkRCP2_CheckedChanged);
            // 
            // txtRCP
            // 
            this.txtRCP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtRCP.Location = new System.Drawing.Point(119, 141);
            this.txtRCP.Multiline = true;
            this.txtRCP.Name = "txtRCP";
            this.txtRCP.Size = new System.Drawing.Size(164, 61);
            this.txtRCP.TabIndex = 63;
            this.txtRCP.Text = "RCP1 Commander\r\nRCP2 MixW\r\nRCP3 WriteLog\r\nRCP4 DXBase";
            this.toolTip1.SetToolTip(this.txtRCP, "Use this to keep track of which port your programs are on.");
            this.txtRCP.TextChanged += new System.EventHandler(this.txtRCP_TextChanged);
            // 
            // cboRCP2
            // 
            this.cboRCP2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboRCP2.FormattingEnabled = true;
            this.cboRCP2.Location = new System.Drawing.Point(121, 37);
            this.cboRCP2.Name = "cboRCP2";
            this.cboRCP2.Size = new System.Drawing.Size(63, 21);
            this.cboRCP2.TabIndex = 46;
            this.toolTip1.SetToolTip(this.cboRCP2, "RCP2 CAT Port");
            this.cboRCP2.SelectedIndexChanged += new System.EventHandler(this.cboRCP2_SelectedIndexChanged);
            // 
            // chkRCP4IF
            // 
            this.chkRCP4IF.AutoSize = true;
            this.chkRCP4IF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP4IF.Location = new System.Drawing.Point(191, 107);
            this.chkRCP4IF.Name = "chkRCP4IF";
            this.chkRCP4IF.Size = new System.Drawing.Size(57, 17);
            this.chkRCP4IF.TabIndex = 62;
            this.chkRCP4IF.Text = "IF only";
            this.toolTip1.SetToolTip(this.chkRCP4IF, "Check to disable all CAT commands but IF");
            this.chkRCP4IF.UseVisualStyleBackColor = true;
            this.chkRCP4IF.CheckedChanged += new System.EventHandler(this.chkRCP4IF_CheckedChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label17.Location = new System.Drawing.Point(6, 41);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(35, 13);
            this.label17.TabIndex = 49;
            this.label17.Text = "RCP2";
            // 
            // chkRCP3IF
            // 
            this.chkRCP3IF.AutoSize = true;
            this.chkRCP3IF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP3IF.Location = new System.Drawing.Point(191, 74);
            this.chkRCP3IF.Name = "chkRCP3IF";
            this.chkRCP3IF.Size = new System.Drawing.Size(57, 17);
            this.chkRCP3IF.TabIndex = 61;
            this.chkRCP3IF.Text = "IF only";
            this.toolTip1.SetToolTip(this.chkRCP3IF, "Check to disable all CAT commands but IF");
            this.chkRCP3IF.UseVisualStyleBackColor = true;
            this.chkRCP3IF.CheckedChanged += new System.EventHandler(this.chkRCP3IF_CheckedChanged);
            // 
            // chkRCP3
            // 
            this.chkRCP3.AutoSize = true;
            this.chkRCP3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP3.Location = new System.Drawing.Point(50, 74);
            this.chkRCP3.Name = "chkRCP3";
            this.chkRCP3.Size = new System.Drawing.Size(65, 17);
            this.chkRCP3.TabIndex = 51;
            this.chkRCP3.Text = "Enabled";
            this.toolTip1.SetToolTip(this.chkRCP3, "Check to enable RCP 3");
            this.chkRCP3.UseVisualStyleBackColor = true;
            this.chkRCP3.CheckedChanged += new System.EventHandler(this.chkRCP3_CheckedChanged);
            // 
            // chkRCP2IF
            // 
            this.chkRCP2IF.AutoSize = true;
            this.chkRCP2IF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP2IF.Location = new System.Drawing.Point(191, 40);
            this.chkRCP2IF.Name = "chkRCP2IF";
            this.chkRCP2IF.Size = new System.Drawing.Size(57, 17);
            this.chkRCP2IF.TabIndex = 60;
            this.chkRCP2IF.Text = "IF only";
            this.toolTip1.SetToolTip(this.chkRCP2IF, "Check to disable all CAT commands but IF");
            this.chkRCP2IF.UseVisualStyleBackColor = true;
            this.chkRCP2IF.CheckedChanged += new System.EventHandler(this.chkRCP2IF_CheckedChanged);
            // 
            // cboRCP3
            // 
            this.cboRCP3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboRCP3.FormattingEnabled = true;
            this.cboRCP3.Location = new System.Drawing.Point(121, 72);
            this.cboRCP3.Name = "cboRCP3";
            this.cboRCP3.Size = new System.Drawing.Size(63, 21);
            this.cboRCP3.TabIndex = 50;
            this.toolTip1.SetToolTip(this.cboRCP3, "RCP 3 CAT Port");
            this.cboRCP3.SelectedIndexChanged += new System.EventHandler(this.cboRCP3_SelectedIndexChanged);
            // 
            // chkRCP4DisPol
            // 
            this.chkRCP4DisPol.AutoSize = true;
            this.chkRCP4DisPol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP4DisPol.Location = new System.Drawing.Point(255, 108);
            this.chkRCP4DisPol.Name = "chkRCP4DisPol";
            this.chkRCP4DisPol.Size = new System.Drawing.Size(61, 17);
            this.chkRCP4DisPol.TabIndex = 59;
            this.chkRCP4DisPol.Text = "Dis Poll";
            this.toolTip1.SetToolTip(this.chkRCP4DisPol, "Check to disable polling for RCP 4");
            this.chkRCP4DisPol.UseVisualStyleBackColor = true;
            this.chkRCP4DisPol.CheckedChanged += new System.EventHandler(this.chkRCP4DisPol_CheckedChanged);
            // 
            // chkRCP4
            // 
            this.chkRCP4.AutoSize = true;
            this.chkRCP4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP4.Location = new System.Drawing.Point(50, 109);
            this.chkRCP4.Name = "chkRCP4";
            this.chkRCP4.Size = new System.Drawing.Size(65, 17);
            this.chkRCP4.TabIndex = 54;
            this.chkRCP4.Text = "Enabled";
            this.toolTip1.SetToolTip(this.chkRCP4, "Check to enable RCP 4");
            this.chkRCP4.UseVisualStyleBackColor = true;
            this.chkRCP4.CheckedChanged += new System.EventHandler(this.chkRCP4_CheckedChanged);
            // 
            // chkRCP3DisPol
            // 
            this.chkRCP3DisPol.AutoSize = true;
            this.chkRCP3DisPol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP3DisPol.Location = new System.Drawing.Point(255, 74);
            this.chkRCP3DisPol.Name = "chkRCP3DisPol";
            this.chkRCP3DisPol.Size = new System.Drawing.Size(61, 17);
            this.chkRCP3DisPol.TabIndex = 58;
            this.chkRCP3DisPol.Text = "Dis Poll";
            this.toolTip1.SetToolTip(this.chkRCP3DisPol, "Check to disable polling for RCP 3");
            this.chkRCP3DisPol.UseVisualStyleBackColor = true;
            this.chkRCP3DisPol.CheckedChanged += new System.EventHandler(this.chkRCP3DisPol_CheckedChanged);
            // 
            // cboRCP4
            // 
            this.cboRCP4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboRCP4.FormattingEnabled = true;
            this.cboRCP4.Location = new System.Drawing.Point(121, 106);
            this.cboRCP4.Name = "cboRCP4";
            this.cboRCP4.Size = new System.Drawing.Size(63, 21);
            this.cboRCP4.TabIndex = 53;
            this.toolTip1.SetToolTip(this.cboRCP4, "RCP 4 CAT Port");
            this.cboRCP4.SelectedIndexChanged += new System.EventHandler(this.cboRCP4_SelectedIndexChanged);
            // 
            // chkRCP2DisPol
            // 
            this.chkRCP2DisPol.AutoSize = true;
            this.chkRCP2DisPol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP2DisPol.Location = new System.Drawing.Point(255, 40);
            this.chkRCP2DisPol.Name = "chkRCP2DisPol";
            this.chkRCP2DisPol.Size = new System.Drawing.Size(61, 17);
            this.chkRCP2DisPol.TabIndex = 57;
            this.chkRCP2DisPol.Text = "Dis Poll";
            this.toolTip1.SetToolTip(this.chkRCP2DisPol, "Check to disable polling for RCP 2");
            this.chkRCP2DisPol.UseVisualStyleBackColor = true;
            this.chkRCP2DisPol.CheckedChanged += new System.EventHandler(this.chkRCP2DisPol_CheckedChanged);
            // 
            // tabOther
            // 
            this.tabOther.Controls.Add(this.grpWN2);
            this.tabOther.Controls.Add(this.groupBox6);
            this.tabOther.Controls.Add(this.groupBox4);
            this.tabOther.Controls.Add(this.lblSleep);
            this.tabOther.Controls.Add(this.groupBox3);
            this.tabOther.Controls.Add(this.grpLP);
            this.tabOther.Controls.Add(this.txtSleep);
            this.tabOther.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabOther.Location = new System.Drawing.Point(4, 22);
            this.tabOther.Name = "tabOther";
            this.tabOther.Size = new System.Drawing.Size(417, 309);
            this.tabOther.TabIndex = 2;
            this.tabOther.Text = "Other";
            this.tabOther.UseVisualStyleBackColor = true;
            this.tabOther.DoubleClick += new System.EventHandler(this.tabOther_DoubleClick);
            // 
            // grpWN2
            // 
            this.grpWN2.Controls.Add(this.btnSensor);
            this.grpWN2.Controls.Add(this.btnReset);
            this.grpWN2.Controls.Add(this.grpC4);
            this.grpWN2.Controls.Add(this.grpC3);
            this.grpWN2.Controls.Add(this.grpC2);
            this.grpWN2.Controls.Add(this.grpC1);
            this.grpWN2.Controls.Add(this.label63);
            this.grpWN2.Controls.Add(this.label14);
            this.grpWN2.Controls.Add(this.label10);
            this.grpWN2.Controls.Add(this.chkWNEnab);
            this.grpWN2.Controls.Add(this.rbWN4);
            this.grpWN2.Controls.Add(this.rbWN3);
            this.grpWN2.Controls.Add(this.rbWN2);
            this.grpWN2.Controls.Add(this.rbWN1);
            this.grpWN2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpWN2.ForeColor = System.Drawing.Color.Navy;
            this.grpWN2.Location = new System.Drawing.Point(200, 2);
            this.grpWN2.Name = "grpWN2";
            this.grpWN2.Size = new System.Drawing.Size(210, 138);
            this.grpWN2.TabIndex = 0;
            this.grpWN2.TabStop = false;
            this.grpWN2.Text = "WaveNode";
            // 
            // btnSensor
            // 
            this.btnSensor.BackColor = System.Drawing.Color.Lime;
            this.btnSensor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSensor.Location = new System.Drawing.Point(83, 10);
            this.btnSensor.Name = "btnSensor";
            this.btnSensor.Size = new System.Drawing.Size(10, 10);
            this.btnSensor.TabIndex = 73;
            this.toolTip1.SetToolTip(this.btnSensor, "Press to display sensor matrix.");
            this.btnSensor.UseVisualStyleBackColor = false;
            this.btnSensor.Click += new System.EventHandler(this.btnSensor_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Violet;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Location = new System.Drawing.Point(195, 10);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(10, 10);
            this.btnReset.TabIndex = 72;
            this.toolTip1.SetToolTip(this.btnReset, "Master Reset for the WN2 hardware. Use only after toggling Enabled check box fail" +
                    "s to correct .");
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // grpC4
            // 
            this.grpC4.Controls.Add(this.rbC4K);
            this.grpC4.Controls.Add(this.rbC4H);
            this.grpC4.Controls.Add(this.rbC4Q);
            this.grpC4.Location = new System.Drawing.Point(94, 105);
            this.grpC4.Name = "grpC4";
            this.grpC4.Size = new System.Drawing.Size(105, 25);
            this.grpC4.TabIndex = 70;
            this.grpC4.TabStop = false;
            // 
            // rbC4K
            // 
            this.rbC4K.AutoSize = true;
            this.rbC4K.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbC4K.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbC4K.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbC4K.ForeColor = System.Drawing.Color.Navy;
            this.rbC4K.Location = new System.Drawing.Point(80, 8);
            this.rbC4K.Name = "rbC4K";
            this.rbC4K.Size = new System.Drawing.Size(14, 13);
            this.rbC4K.TabIndex = 69;
            this.toolTip1.SetToolTip(this.rbC4K, "Selects coupler type");
            this.rbC4K.UseVisualStyleBackColor = true;
            this.rbC4K.CheckedChanged += new System.EventHandler(this.grpC4_CheckedChanged);
            // 
            // rbC4H
            // 
            this.rbC4H.AutoSize = true;
            this.rbC4H.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbC4H.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbC4H.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbC4H.ForeColor = System.Drawing.Color.Navy;
            this.rbC4H.Location = new System.Drawing.Point(45, 8);
            this.rbC4H.Name = "rbC4H";
            this.rbC4H.Size = new System.Drawing.Size(14, 13);
            this.rbC4H.TabIndex = 68;
            this.toolTip1.SetToolTip(this.rbC4H, "Selects coupler type");
            this.rbC4H.UseVisualStyleBackColor = true;
            this.rbC4H.CheckedChanged += new System.EventHandler(this.grpC4_CheckedChanged);
            // 
            // rbC4Q
            // 
            this.rbC4Q.AutoSize = true;
            this.rbC4Q.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbC4Q.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbC4Q.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbC4Q.ForeColor = System.Drawing.Color.Navy;
            this.rbC4Q.Location = new System.Drawing.Point(10, 8);
            this.rbC4Q.Name = "rbC4Q";
            this.rbC4Q.Size = new System.Drawing.Size(14, 13);
            this.rbC4Q.TabIndex = 67;
            this.toolTip1.SetToolTip(this.rbC4Q, "Selects coupler type");
            this.rbC4Q.UseVisualStyleBackColor = true;
            this.rbC4Q.CheckedChanged += new System.EventHandler(this.grpC4_CheckedChanged);
            // 
            // grpC3
            // 
            this.grpC3.Controls.Add(this.rbC3K);
            this.grpC3.Controls.Add(this.rbC3H);
            this.grpC3.Controls.Add(this.rbC3Q);
            this.grpC3.Location = new System.Drawing.Point(94, 80);
            this.grpC3.Name = "grpC3";
            this.grpC3.Size = new System.Drawing.Size(105, 25);
            this.grpC3.TabIndex = 71;
            this.grpC3.TabStop = false;
            // 
            // rbC3K
            // 
            this.rbC3K.AutoSize = true;
            this.rbC3K.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbC3K.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbC3K.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbC3K.ForeColor = System.Drawing.Color.Navy;
            this.rbC3K.Location = new System.Drawing.Point(80, 8);
            this.rbC3K.Name = "rbC3K";
            this.rbC3K.Size = new System.Drawing.Size(14, 13);
            this.rbC3K.TabIndex = 69;
            this.toolTip1.SetToolTip(this.rbC3K, "Selects coupler type");
            this.rbC3K.UseVisualStyleBackColor = true;
            this.rbC3K.CheckedChanged += new System.EventHandler(this.grpC3_CheckedChanged);
            // 
            // rbC3H
            // 
            this.rbC3H.AutoSize = true;
            this.rbC3H.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbC3H.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbC3H.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbC3H.ForeColor = System.Drawing.Color.Navy;
            this.rbC3H.Location = new System.Drawing.Point(45, 8);
            this.rbC3H.Name = "rbC3H";
            this.rbC3H.Size = new System.Drawing.Size(14, 13);
            this.rbC3H.TabIndex = 68;
            this.toolTip1.SetToolTip(this.rbC3H, "Selects coupler type");
            this.rbC3H.UseVisualStyleBackColor = true;
            this.rbC3H.CheckedChanged += new System.EventHandler(this.grpC3_CheckedChanged);
            // 
            // rbC3Q
            // 
            this.rbC3Q.AutoSize = true;
            this.rbC3Q.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbC3Q.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbC3Q.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbC3Q.ForeColor = System.Drawing.Color.Navy;
            this.rbC3Q.Location = new System.Drawing.Point(10, 8);
            this.rbC3Q.Name = "rbC3Q";
            this.rbC3Q.Size = new System.Drawing.Size(14, 13);
            this.rbC3Q.TabIndex = 67;
            this.toolTip1.SetToolTip(this.rbC3Q, "Selects coupler type");
            this.rbC3Q.UseVisualStyleBackColor = true;
            this.rbC3Q.CheckedChanged += new System.EventHandler(this.grpC3_CheckedChanged);
            // 
            // grpC2
            // 
            this.grpC2.Controls.Add(this.rbC2K);
            this.grpC2.Controls.Add(this.rbC2H);
            this.grpC2.Controls.Add(this.rbC2Q);
            this.grpC2.Location = new System.Drawing.Point(94, 55);
            this.grpC2.Name = "grpC2";
            this.grpC2.Size = new System.Drawing.Size(105, 25);
            this.grpC2.TabIndex = 70;
            this.grpC2.TabStop = false;
            // 
            // rbC2K
            // 
            this.rbC2K.AutoSize = true;
            this.rbC2K.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbC2K.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbC2K.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbC2K.ForeColor = System.Drawing.Color.Navy;
            this.rbC2K.Location = new System.Drawing.Point(80, 8);
            this.rbC2K.Name = "rbC2K";
            this.rbC2K.Size = new System.Drawing.Size(14, 13);
            this.rbC2K.TabIndex = 69;
            this.toolTip1.SetToolTip(this.rbC2K, "Selects coupler type");
            this.rbC2K.UseVisualStyleBackColor = true;
            this.rbC2K.CheckedChanged += new System.EventHandler(this.grpC2_CheckedChanged);
            // 
            // rbC2H
            // 
            this.rbC2H.AutoSize = true;
            this.rbC2H.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbC2H.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbC2H.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbC2H.ForeColor = System.Drawing.Color.Navy;
            this.rbC2H.Location = new System.Drawing.Point(45, 8);
            this.rbC2H.Name = "rbC2H";
            this.rbC2H.Size = new System.Drawing.Size(14, 13);
            this.rbC2H.TabIndex = 68;
            this.toolTip1.SetToolTip(this.rbC2H, "Selects coupler type");
            this.rbC2H.UseVisualStyleBackColor = true;
            this.rbC2H.CheckedChanged += new System.EventHandler(this.grpC2_CheckedChanged);
            // 
            // rbC2Q
            // 
            this.rbC2Q.AutoSize = true;
            this.rbC2Q.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbC2Q.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbC2Q.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbC2Q.ForeColor = System.Drawing.Color.Navy;
            this.rbC2Q.Location = new System.Drawing.Point(10, 8);
            this.rbC2Q.Name = "rbC2Q";
            this.rbC2Q.Size = new System.Drawing.Size(14, 13);
            this.rbC2Q.TabIndex = 67;
            this.toolTip1.SetToolTip(this.rbC2Q, "Selects coupler type");
            this.rbC2Q.UseVisualStyleBackColor = true;
            this.rbC2Q.CheckedChanged += new System.EventHandler(this.grpC2_CheckedChanged);
            // 
            // grpC1
            // 
            this.grpC1.Controls.Add(this.rbC1K);
            this.grpC1.Controls.Add(this.rbC1H);
            this.grpC1.Controls.Add(this.rbC1Q);
            this.grpC1.Location = new System.Drawing.Point(94, 29);
            this.grpC1.Name = "grpC1";
            this.grpC1.Size = new System.Drawing.Size(105, 25);
            this.grpC1.TabIndex = 65;
            this.grpC1.TabStop = false;
            // 
            // rbC1K
            // 
            this.rbC1K.AutoSize = true;
            this.rbC1K.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbC1K.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbC1K.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbC1K.ForeColor = System.Drawing.Color.Navy;
            this.rbC1K.Location = new System.Drawing.Point(80, 8);
            this.rbC1K.Name = "rbC1K";
            this.rbC1K.Size = new System.Drawing.Size(14, 13);
            this.rbC1K.TabIndex = 69;
            this.toolTip1.SetToolTip(this.rbC1K, "Selects coupler type");
            this.rbC1K.UseVisualStyleBackColor = true;
            this.rbC1K.CheckedChanged += new System.EventHandler(this.grpC1_CheckedChanged);
            // 
            // rbC1H
            // 
            this.rbC1H.AutoSize = true;
            this.rbC1H.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbC1H.ForeColor = System.Drawing.Color.Navy;
            this.rbC1H.Location = new System.Drawing.Point(45, 8);
            this.rbC1H.Name = "rbC1H";
            this.rbC1H.Size = new System.Drawing.Size(14, 13);
            this.rbC1H.TabIndex = 68;
            this.toolTip1.SetToolTip(this.rbC1H, "Selects coupler type");
            this.rbC1H.UseVisualStyleBackColor = true;
            this.rbC1H.CheckedChanged += new System.EventHandler(this.grpC1_CheckedChanged);
            // 
            // rbC1Q
            // 
            this.rbC1Q.AutoSize = true;
            this.rbC1Q.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbC1Q.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbC1Q.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbC1Q.ForeColor = System.Drawing.Color.Navy;
            this.rbC1Q.Location = new System.Drawing.Point(10, 8);
            this.rbC1Q.Name = "rbC1Q";
            this.rbC1Q.Size = new System.Drawing.Size(14, 13);
            this.rbC1Q.TabIndex = 67;
            this.toolTip1.SetToolTip(this.rbC1Q, "Selects coupler type");
            this.rbC1Q.UseVisualStyleBackColor = true;
            this.rbC1Q.CheckedChanged += new System.EventHandler(this.grpC1_CheckedChanged);
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label63.ForeColor = System.Drawing.Color.Firebrick;
            this.label63.Location = new System.Drawing.Point(172, 15);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(16, 12);
            this.label63.TabIndex = 64;
            this.label63.Text = "8K";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label63, "Coupler Type");
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Firebrick;
            this.label14.Location = new System.Drawing.Point(125, 15);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 12);
            this.label14.TabIndex = 63;
            this.label14.Text = "HF/UHF";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label14, "Coupler Type");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Firebrick;
            this.label10.Location = new System.Drawing.Point(96, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 12);
            this.label10.TabIndex = 62;
            this.label10.Text = "QRP";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label10, "Coupler Type");
            // 
            // chkWNEnab
            // 
            this.chkWNEnab.AutoSize = true;
            this.chkWNEnab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkWNEnab.ForeColor = System.Drawing.Color.Navy;
            this.chkWNEnab.Location = new System.Drawing.Point(10, 15);
            this.chkWNEnab.Name = "chkWNEnab";
            this.chkWNEnab.Size = new System.Drawing.Size(65, 17);
            this.chkWNEnab.TabIndex = 5;
            this.chkWNEnab.Text = "Enabled";
            this.toolTip1.SetToolTip(this.chkWNEnab, "When checked enables WaveNode Wattmeter");
            this.chkWNEnab.UseVisualStyleBackColor = true;
            this.chkWNEnab.CheckedChanged += new System.EventHandler(this.chkWNEnab_CheckedChanged);
            // 
            // rbWN4
            // 
            this.rbWN4.AutoSize = true;
            this.rbWN4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbWN4.ForeColor = System.Drawing.Color.Navy;
            this.rbWN4.Location = new System.Drawing.Point(10, 110);
            this.rbWN4.Name = "rbWN4";
            this.rbWN4.Size = new System.Drawing.Size(77, 17);
            this.rbWN4.TabIndex = 4;
            this.rbWN4.TabStop = true;
            this.rbWN4.Text = "Coupler #4";
            this.toolTip1.SetToolTip(this.rbWN4, "Selects watt meter coupler #4");
            this.rbWN4.UseVisualStyleBackColor = true;
            this.rbWN4.CheckedChanged += new System.EventHandler(this.grpWN2_CheckedChanged);
            // 
            // rbWN3
            // 
            this.rbWN3.AutoSize = true;
            this.rbWN3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbWN3.ForeColor = System.Drawing.Color.Navy;
            this.rbWN3.Location = new System.Drawing.Point(10, 85);
            this.rbWN3.Name = "rbWN3";
            this.rbWN3.Size = new System.Drawing.Size(77, 17);
            this.rbWN3.TabIndex = 3;
            this.rbWN3.TabStop = true;
            this.rbWN3.Text = "Coupler #3";
            this.toolTip1.SetToolTip(this.rbWN3, "Selects watt meter coupler #3");
            this.rbWN3.UseVisualStyleBackColor = true;
            this.rbWN3.CheckedChanged += new System.EventHandler(this.grpWN2_CheckedChanged);
            // 
            // rbWN2
            // 
            this.rbWN2.AutoSize = true;
            this.rbWN2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbWN2.ForeColor = System.Drawing.Color.Navy;
            this.rbWN2.Location = new System.Drawing.Point(10, 60);
            this.rbWN2.Name = "rbWN2";
            this.rbWN2.Size = new System.Drawing.Size(77, 17);
            this.rbWN2.TabIndex = 2;
            this.rbWN2.TabStop = true;
            this.rbWN2.Text = "Coupler #2";
            this.toolTip1.SetToolTip(this.rbWN2, "Selects watt meter coupler #2");
            this.rbWN2.UseVisualStyleBackColor = true;
            this.rbWN2.CheckedChanged += new System.EventHandler(this.grpWN2_CheckedChanged);
            // 
            // rbWN1
            // 
            this.rbWN1.AutoSize = true;
            this.rbWN1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbWN1.ForeColor = System.Drawing.Color.Navy;
            this.rbWN1.Location = new System.Drawing.Point(10, 35);
            this.rbWN1.Name = "rbWN1";
            this.rbWN1.Size = new System.Drawing.Size(77, 17);
            this.rbWN1.TabIndex = 1;
            this.rbWN1.TabStop = true;
            this.rbWN1.Text = "Coupler #1";
            this.toolTip1.SetToolTip(this.rbWN1, "Selects watt meter coupler #1");
            this.rbWN1.UseVisualStyleBackColor = true;
            this.rbWN1.CheckedChanged += new System.EventHandler(this.grpWN2_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label19);
            this.groupBox6.Controls.Add(this.cboStepCom);
            this.groupBox6.Controls.Add(this.cboStep);
            this.groupBox6.Controls.Add(this.txtStepInv);
            this.groupBox6.Controls.Add(this.chkStep);
            this.groupBox6.Controls.Add(this.label41);
            this.groupBox6.Controls.Add(this.label42);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox6.ForeColor = System.Drawing.Color.Blue;
            this.groupBox6.Location = new System.Drawing.Point(200, 146);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(165, 80);
            this.groupBox6.TabIndex = 48;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "SteppIR";
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(90, 60);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(39, 15);
            this.label19.TabIndex = 47;
            this.label19.Text = "Comm";
            // 
            // cboStepCom
            // 
            this.cboStepCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStepCom.FormattingEnabled = true;
            this.cboStepCom.Items.AddRange(new object[] {
            "9600 8-N-1",
            "9600 8-N-2",
            "4800 8-N-1",
            "4800 8-N-2",
            "2400 8-N-1",
            "2400 8-N-2",
            "1200 8-N-1",
            "1200 8-N-2"});
            this.cboStepCom.Location = new System.Drawing.Point(75, 40);
            this.cboStepCom.Name = "cboStepCom";
            this.cboStepCom.Size = new System.Drawing.Size(81, 21);
            this.cboStepCom.TabIndex = 46;
            this.toolTip1.SetToolTip(this.cboStepCom, "Select Comm Parameters for SteppIR Antenna Controller");
            this.cboStepCom.SelectedIndexChanged += new System.EventHandler(this.cboStepCom_SelectedIndexChanged);
            // 
            // cboStep
            // 
            this.cboStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStep.FormattingEnabled = true;
            this.cboStep.Location = new System.Drawing.Point(8, 40);
            this.cboStep.Name = "cboStep";
            this.cboStep.Size = new System.Drawing.Size(63, 21);
            this.cboStep.TabIndex = 17;
            this.toolTip1.SetToolTip(this.cboStep, "Select SteppIR Antenna Controller Port");
            this.cboStep.SelectedIndexChanged += new System.EventHandler(this.cboStep_SelectedIndexChanged);
            // 
            // txtStepInv
            // 
            this.txtStepInv.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStepInv.Location = new System.Drawing.Point(75, 15);
            this.txtStepInv.Name = "txtStepInv";
            this.txtStepInv.Size = new System.Drawing.Size(36, 20);
            this.txtStepInv.TabIndex = 15;
            this.toolTip1.SetToolTip(this.txtStepInv, "Select SteppIR port polling interval (200-500 recommended)");
            this.txtStepInv.TextChanged += new System.EventHandler(this.txtStepInv_TextChanged);
            // 
            // chkStep
            // 
            this.chkStep.AutoSize = true;
            this.chkStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStep.Location = new System.Drawing.Point(9, 17);
            this.chkStep.Name = "chkStep";
            this.chkStep.Size = new System.Drawing.Size(59, 17);
            this.chkStep.TabIndex = 45;
            this.chkStep.Text = "Enable";
            this.chkStep.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.toolTip1.SetToolTip(this.chkStep, "Check to enable SteppIR Antenna Control");
            this.chkStep.UseVisualStyleBackColor = true;
            this.chkStep.CheckedChanged += new System.EventHandler(this.chkStep_CheckedChanged);
            // 
            // label41
            // 
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(115, 19);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(45, 15);
            this.label41.TabIndex = 16;
            this.label41.Text = "Interval";
            this.label41.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label42
            // 
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.Location = new System.Drawing.Point(20, 61);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(29, 15);
            this.label42.TabIndex = 18;
            this.label42.Text = "Port";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkDisBcast);
            this.groupBox4.Controls.Add(this.btnTest);
            this.groupBox4.Controls.Add(this.label24);
            this.groupBox4.Controls.Add(this.txtPW1ra);
            this.groupBox4.Controls.Add(this.label23);
            this.groupBox4.Controls.Add(this.cboPW1);
            this.groupBox4.Controls.Add(this.txtPW1ta);
            this.groupBox4.Controls.Add(this.chkPW1);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox4.ForeColor = System.Drawing.Color.Firebrick;
            this.groupBox4.Location = new System.Drawing.Point(7, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(183, 100);
            this.groupBox4.TabIndex = 48;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "IC-PW1";
            // 
            // chkDisBcast
            // 
            this.chkDisBcast.AutoSize = true;
            this.chkDisBcast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDisBcast.Location = new System.Drawing.Point(5, 75);
            this.chkDisBcast.Name = "chkDisBcast";
            this.chkDisBcast.Size = new System.Drawing.Size(112, 17);
            this.chkDisBcast.TabIndex = 50;
            this.chkDisBcast.Text = "Disable Broadcast";
            this.toolTip1.SetToolTip(this.chkDisBcast, "Check to disable frequency broadcast to PW1");
            this.chkDisBcast.UseVisualStyleBackColor = true;
            this.chkDisBcast.CheckedChanged += new System.EventHandler(this.chkDisBcast_CheckedChanged);
            // 
            // btnTest
            // 
            this.btnTest.AutoSize = true;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.Location = new System.Drawing.Point(130, 70);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(42, 23);
            this.btnTest.TabIndex = 49;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(105, 47);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(18, 13);
            this.label24.TabIndex = 48;
            this.label24.Text = "ra";
            this.label24.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtPW1ra
            // 
            this.txtPW1ra.Enabled = false;
            this.txtPW1ra.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPW1ra.Location = new System.Drawing.Point(98, 24);
            this.txtPW1ra.Name = "txtPW1ra";
            this.txtPW1ra.Size = new System.Drawing.Size(32, 20);
            this.txtPW1ra.TabIndex = 47;
            this.txtPW1ra.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPW1ra, "CI-V Address for IC-PW1");
            this.txtPW1ra.TextChanged += new System.EventHandler(this.txtPW1ra_TextChanged);
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(146, 47);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(18, 13);
            this.label23.TabIndex = 46;
            this.label23.Text = "ta";
            this.label23.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cboPW1
            // 
            this.cboPW1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPW1.FormattingEnabled = true;
            this.cboPW1.Location = new System.Drawing.Point(5, 25);
            this.cboPW1.Name = "cboPW1";
            this.cboPW1.Size = new System.Drawing.Size(63, 21);
            this.cboPW1.TabIndex = 17;
            this.toolTip1.SetToolTip(this.cboPW1, "Select Port for IC-PW1");
            this.cboPW1.SelectedIndexChanged += new System.EventHandler(this.cboPW1_SelectedIndexChanged);
            // 
            // txtPW1ta
            // 
            this.txtPW1ta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPW1ta.Location = new System.Drawing.Point(138, 24);
            this.txtPW1ta.Name = "txtPW1ta";
            this.txtPW1ta.Size = new System.Drawing.Size(32, 20);
            this.txtPW1ta.TabIndex = 15;
            this.txtPW1ta.Text = "33";
            this.txtPW1ta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPW1ta, "CI-V Address for DDUtil");
            this.txtPW1ta.TextChanged += new System.EventHandler(this.txtPW1ta_TextChanged);
            // 
            // chkPW1
            // 
            this.chkPW1.AutoSize = true;
            this.chkPW1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPW1.Location = new System.Drawing.Point(5, 55);
            this.chkPW1.Name = "chkPW1";
            this.chkPW1.Size = new System.Drawing.Size(65, 17);
            this.chkPW1.TabIndex = 45;
            this.chkPW1.Text = "Enabled";
            this.toolTip1.SetToolTip(this.chkPW1, "Check to enable IC-PW1 Amplifier");
            this.chkPW1.UseVisualStyleBackColor = true;
            this.chkPW1.CheckedChanged += new System.EventHandler(this.chkPW1_CheckedChanged);
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(93, 8);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(84, 15);
            this.label21.TabIndex = 16;
            this.label21.Text = "--- CI-V Addr ---";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(70, 28);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(30, 15);
            this.label22.TabIndex = 18;
            this.label22.Text = "Port";
            // 
            // lblSleep
            // 
            this.lblSleep.Location = new System.Drawing.Point(377, 199);
            this.lblSleep.Name = "lblSleep";
            this.lblSleep.Size = new System.Drawing.Size(30, 16);
            this.lblSleep.TabIndex = 51;
            this.lblSleep.Text = "Sleep";
            this.lblSleep.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.btnPFfile);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.txtProfLoc);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox3.ForeColor = System.Drawing.Color.Navy;
            this.groupBox3.Location = new System.Drawing.Point(7, 230);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(403, 75);
            this.groupBox3.TabIndex = 61;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Flex Profiler";
            this.toolTip1.SetToolTip(this.groupBox3, "Right-Click for Options");
            // 
            // btnPFfile
            // 
            this.btnPFfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnPFfile.Location = new System.Drawing.Point(171, 45);
            this.btnPFfile.Name = "btnPFfile";
            this.btnPFfile.Size = new System.Drawing.Size(57, 23);
            this.btnPFfile.TabIndex = 60;
            this.btnPFfile.Text = "Select";
            this.toolTip1.SetToolTip(this.btnPFfile, "Opens file dialog to select Profiler path.");
            this.btnPFfile.UseVisualStyleBackColor = true;
            this.btnPFfile.Click += new System.EventHandler(this.btnPFfile_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label15.Location = new System.Drawing.Point(10, 45);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 13);
            this.label15.TabIndex = 59;
            this.label15.Text = "File Location";
            // 
            // txtProfLoc
            // 
            this.txtProfLoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtProfLoc.Location = new System.Drawing.Point(6, 20);
            this.txtProfLoc.Name = "txtProfLoc";
            this.txtProfLoc.Size = new System.Drawing.Size(391, 20);
            this.txtProfLoc.TabIndex = 58;
            this.toolTip1.SetToolTip(this.txtProfLoc, "Flex Profiler data file & location to use for this device.");
            this.txtProfLoc.WordWrap = false;
            // 
            // grpLP
            // 
            this.grpLP.Controls.Add(this.rb100A);
            this.grpLP.Controls.Add(this.rb100);
            this.grpLP.Controls.Add(this.lblPower);
            this.grpLP.Controls.Add(this.lblFast);
            this.grpLP.Controls.Add(this.lblAlarm);
            this.grpLP.Controls.Add(this.txtLPint);
            this.grpLP.Controls.Add(this.label13);
            this.grpLP.Controls.Add(this.btnMode);
            this.grpLP.Controls.Add(this.btnFast);
            this.grpLP.Controls.Add(this.btnAlarm);
            this.grpLP.Controls.Add(this.cboLPport);
            this.grpLP.Controls.Add(this.chkLPenab);
            this.grpLP.Controls.Add(this.label2);
            this.grpLP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpLP.ForeColor = System.Drawing.Color.Green;
            this.grpLP.Location = new System.Drawing.Point(7, 106);
            this.grpLP.Name = "grpLP";
            this.grpLP.Size = new System.Drawing.Size(183, 120);
            this.grpLP.TabIndex = 47;
            this.grpLP.TabStop = false;
            this.grpLP.Text = "LP-100";
            // 
            // rb100A
            // 
            this.rb100A.AutoSize = true;
            this.rb100A.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rb100A.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rb100A.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F);
            this.rb100A.ForeColor = System.Drawing.Color.Navy;
            this.rb100A.Location = new System.Drawing.Point(95, 99);
            this.rb100A.Name = "rb100A";
            this.rb100A.Size = new System.Drawing.Size(60, 16);
            this.rb100A.TabIndex = 84;
            this.rb100A.Text = "  LP100A";
            this.toolTip1.SetToolTip(this.rb100A, "Selects coupler type");
            this.rb100A.UseVisualStyleBackColor = true;
            this.rb100A.CheckedChanged += new System.EventHandler(this.grpLP_CheckChanged);
            // 
            // rb100
            // 
            this.rb100.AutoSize = true;
            this.rb100.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rb100.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rb100.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F);
            this.rb100.ForeColor = System.Drawing.Color.Navy;
            this.rb100.Location = new System.Drawing.Point(95, 82);
            this.rb100.Name = "rb100";
            this.rb100.Size = new System.Drawing.Size(53, 16);
            this.rb100.TabIndex = 83;
            this.rb100.Text = "  LP100";
            this.toolTip1.SetToolTip(this.rb100, "Selects coupler type");
            this.rb100.UseVisualStyleBackColor = true;
            this.rb100.CheckedChanged += new System.EventHandler(this.grpLP_CheckChanged);
            // 
            // lblPower
            // 
            this.lblPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F);
            this.lblPower.ForeColor = System.Drawing.Color.Navy;
            this.lblPower.Location = new System.Drawing.Point(115, 62);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(60, 14);
            this.lblPower.TabIndex = 81;
            this.lblPower.Text = "Power: None";
            this.lblPower.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.lblPower, "Current power setting reported from LP-100");
            // 
            // lblFast
            // 
            this.lblFast.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F);
            this.lblFast.ForeColor = System.Drawing.Color.Black;
            this.lblFast.Location = new System.Drawing.Point(115, 37);
            this.lblFast.Name = "lblFast";
            this.lblFast.Size = new System.Drawing.Size(27, 15);
            this.lblFast.TabIndex = 80;
            this.lblFast.Text = "Fast";
            this.lblFast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblFast, "Fast/Peak/Hold mode setting");
            // 
            // lblAlarm
            // 
            this.lblAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F);
            this.lblAlarm.ForeColor = System.Drawing.Color.Black;
            this.lblAlarm.Location = new System.Drawing.Point(115, 12);
            this.lblAlarm.Name = "lblAlarm";
            this.lblAlarm.Size = new System.Drawing.Size(28, 15);
            this.lblAlarm.TabIndex = 79;
            this.lblAlarm.Text = "2.5";
            this.lblAlarm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblAlarm, "Alarm setting");
            // 
            // txtLPint
            // 
            this.txtLPint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLPint.Location = new System.Drawing.Point(10, 79);
            this.txtLPint.Name = "txtLPint";
            this.txtLPint.Size = new System.Drawing.Size(44, 20);
            this.txtLPint.TabIndex = 15;
            this.toolTip1.SetToolTip(this.txtLPint, "Select CAT port polling interval (1000 = 1 secoond)");
            this.txtLPint.TextChanged += new System.EventHandler(this.txtLPint_TextChanged);
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(10, 101);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(45, 15);
            this.label13.TabIndex = 16;
            this.label13.Text = "Interval";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnMode
            // 
            this.btnMode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMode.BackColor = System.Drawing.SystemColors.Control;
            this.btnMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnMode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnMode.ForeColor = System.Drawing.Color.Firebrick;
            this.btnMode.Location = new System.Drawing.Point(90, 59);
            this.btnMode.Name = "btnMode";
            this.btnMode.Size = new System.Drawing.Size(20, 20);
            this.btnMode.TabIndex = 78;
            this.btnMode.Text = "M";
            this.toolTip1.SetToolTip(this.btnMode, "Press to toggle Controller Mode (see controller display)");
            this.btnMode.UseVisualStyleBackColor = false;
            this.btnMode.Click += new System.EventHandler(this.btnMode_Click);
            // 
            // btnFast
            // 
            this.btnFast.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnFast.BackColor = System.Drawing.SystemColors.Control;
            this.btnFast.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFast.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnFast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFast.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnFast.ForeColor = System.Drawing.Color.Firebrick;
            this.btnFast.Location = new System.Drawing.Point(90, 34);
            this.btnFast.Name = "btnFast";
            this.btnFast.Size = new System.Drawing.Size(20, 20);
            this.btnFast.TabIndex = 77;
            this.btnFast.Text = "F";
            this.toolTip1.SetToolTip(this.btnFast, "Press to toggle Fast/Peak/Tune mode.");
            this.btnFast.UseVisualStyleBackColor = false;
            this.btnFast.Click += new System.EventHandler(this.btnFast_Click);
            // 
            // btnAlarm
            // 
            this.btnAlarm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAlarm.BackColor = System.Drawing.SystemColors.Control;
            this.btnAlarm.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAlarm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnAlarm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnAlarm.ForeColor = System.Drawing.Color.Firebrick;
            this.btnAlarm.Location = new System.Drawing.Point(90, 9);
            this.btnAlarm.Name = "btnAlarm";
            this.btnAlarm.Size = new System.Drawing.Size(20, 20);
            this.btnAlarm.TabIndex = 76;
            this.btnAlarm.Text = "A";
            this.toolTip1.SetToolTip(this.btnAlarm, "Press to increment Alarm Set Point");
            this.btnAlarm.UseVisualStyleBackColor = false;
            this.btnAlarm.Click += new System.EventHandler(this.btnAlarm_Click);
            // 
            // cboLPport
            // 
            this.cboLPport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLPport.FormattingEnabled = true;
            this.cboLPport.Location = new System.Drawing.Point(10, 40);
            this.cboLPport.Name = "cboLPport";
            this.cboLPport.Size = new System.Drawing.Size(65, 21);
            this.cboLPport.TabIndex = 17;
            this.toolTip1.SetToolTip(this.cboLPport, "Select LP-100 serial port.");
            this.cboLPport.SelectedIndexChanged += new System.EventHandler(this.cboLPport_SelectedIndexChanged);
            // 
            // chkLPenab
            // 
            this.chkLPenab.AutoSize = true;
            this.chkLPenab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLPenab.Location = new System.Drawing.Point(10, 15);
            this.chkLPenab.Name = "chkLPenab";
            this.chkLPenab.Size = new System.Drawing.Size(65, 17);
            this.chkLPenab.TabIndex = 45;
            this.chkLPenab.Text = "Enabled";
            this.toolTip1.SetToolTip(this.chkLPenab, "Check to enable LP100 polling");
            this.chkLPenab.UseVisualStyleBackColor = true;
            this.chkLPenab.CheckedChanged += new System.EventHandler(this.chkLPenab_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 18;
            this.label2.Text = "Port";
            // 
            // txtSleep
            // 
            this.txtSleep.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSleep.Location = new System.Drawing.Point(375, 170);
            this.txtSleep.Name = "txtSleep";
            this.txtSleep.Size = new System.Drawing.Size(32, 22);
            this.txtSleep.TabIndex = 51;
            this.txtSleep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtSleep, "Danger Will Robinson! If you don\'t know what this is for don\'t change it.");
            this.txtSleep.Visible = false;
            this.txtSleep.TextChanged += new System.EventHandler(this.txtSleep_TextChanged);
            // 
            // tabMisc
            // 
            this.tabMisc.Controls.Add(this.grpAlpha);
            this.tabMisc.Location = new System.Drawing.Point(4, 22);
            this.tabMisc.Name = "tabMisc";
            this.tabMisc.Size = new System.Drawing.Size(417, 309);
            this.tabMisc.TabIndex = 7;
            this.tabMisc.Text = "Misc";
            this.tabMisc.UseVisualStyleBackColor = true;
            // 
            // grpAlpha
            // 
            this.grpAlpha.Controls.Add(this.label74);
            this.grpAlpha.Controls.Add(this.txtLoad);
            this.grpAlpha.Controls.Add(this.txtTune);
            this.grpAlpha.Controls.Add(this.label79);
            this.grpAlpha.Controls.Add(this.label80);
            this.grpAlpha.Controls.Add(this.txtFreq);
            this.grpAlpha.Controls.Add(this.label78);
            this.grpAlpha.Controls.Add(this.cboAlphaBaud);
            this.grpAlpha.Controls.Add(this.btnHF);
            this.grpAlpha.Controls.Add(this.btnSF);
            this.grpAlpha.Controls.Add(this.lblHF);
            this.grpAlpha.Controls.Add(this.txtMsg);
            this.grpAlpha.Controls.Add(this.lblSF);
            this.grpAlpha.Controls.Add(this.label69);
            this.grpAlpha.Controls.Add(this.txtSeg);
            this.grpAlpha.Controls.Add(this.label70);
            this.grpAlpha.Controls.Add(this.txtAlphaInt);
            this.grpAlpha.Controls.Add(this.btnHV);
            this.grpAlpha.Controls.Add(this.label77);
            this.grpAlpha.Controls.Add(this.label76);
            this.grpAlpha.Controls.Add(this.label75);
            this.grpAlpha.Controls.Add(this.cboAlpha);
            this.grpAlpha.Controls.Add(this.label73);
            this.grpAlpha.Controls.Add(this.label72);
            this.grpAlpha.Controls.Add(this.btnOper);
            this.grpAlpha.Controls.Add(this.btnTune);
            this.grpAlpha.Controls.Add(this.label71);
            this.grpAlpha.Controls.Add(this.txtBand);
            this.grpAlpha.Controls.Add(this.chkAlpha);
            this.grpAlpha.Controls.Add(this.btnPwr);
            this.grpAlpha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAlpha.ForeColor = System.Drawing.Color.Blue;
            this.grpAlpha.Location = new System.Drawing.Point(7, 164);
            this.grpAlpha.Name = "grpAlpha";
            this.grpAlpha.Size = new System.Drawing.Size(321, 129);
            this.grpAlpha.TabIndex = 0;
            this.grpAlpha.TabStop = false;
            this.grpAlpha.Text = "Alpha 87A";
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label74.ForeColor = System.Drawing.Color.Navy;
            this.label74.Location = new System.Drawing.Point(284, 87);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(31, 13);
            this.label74.TabIndex = 60;
            this.label74.Text = "Load";
            // 
            // txtLoad
            // 
            this.txtLoad.BackColor = System.Drawing.SystemColors.Info;
            this.txtLoad.Enabled = false;
            this.txtLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoad.Location = new System.Drawing.Point(285, 102);
            this.txtLoad.Name = "txtLoad";
            this.txtLoad.ReadOnly = true;
            this.txtLoad.Size = new System.Drawing.Size(26, 20);
            this.txtLoad.TabIndex = 58;
            this.txtLoad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLoad, "Amplifier Load setting");
            // 
            // txtTune
            // 
            this.txtTune.BackColor = System.Drawing.SystemColors.Info;
            this.txtTune.Enabled = false;
            this.txtTune.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTune.Location = new System.Drawing.Point(252, 102);
            this.txtTune.Name = "txtTune";
            this.txtTune.ReadOnly = true;
            this.txtTune.Size = new System.Drawing.Size(26, 20);
            this.txtTune.TabIndex = 57;
            this.txtTune.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTune, "Amplifier Tune setting");
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label79.ForeColor = System.Drawing.Color.Navy;
            this.label79.Location = new System.Drawing.Point(250, 87);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(30, 13);
            this.label79.TabIndex = 59;
            this.label79.Text = "Tune";
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label80.ForeColor = System.Drawing.Color.Navy;
            this.label80.Location = new System.Drawing.Point(277, 12);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(28, 13);
            this.label80.TabIndex = 56;
            this.label80.Text = "Freq";
            // 
            // txtFreq
            // 
            this.txtFreq.BackColor = System.Drawing.SystemColors.Info;
            this.txtFreq.Enabled = false;
            this.txtFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFreq.Location = new System.Drawing.Point(273, 26);
            this.txtFreq.Name = "txtFreq";
            this.txtFreq.ReadOnly = true;
            this.txtFreq.Size = new System.Drawing.Size(39, 20);
            this.txtFreq.TabIndex = 55;
            this.txtFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtFreq, "Amp Frequency Setting in mHz.");
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label78.ForeColor = System.Drawing.Color.Navy;
            this.label78.Location = new System.Drawing.Point(144, 11);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(57, 13);
            this.label78.TabIndex = 54;
            this.label78.Text = "Baud Rate";
            // 
            // cboAlphaBaud
            // 
            this.cboAlphaBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlphaBaud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAlphaBaud.ForeColor = System.Drawing.Color.Black;
            this.cboAlphaBaud.FormattingEnabled = true;
            this.cboAlphaBaud.Items.AddRange(new object[] {
            "4800",
            "9600"});
            this.cboAlphaBaud.Location = new System.Drawing.Point(146, 25);
            this.cboAlphaBaud.Name = "cboAlphaBaud";
            this.cboAlphaBaud.Size = new System.Drawing.Size(56, 21);
            this.cboAlphaBaud.TabIndex = 53;
            this.toolTip1.SetToolTip(this.cboAlphaBaud, "Serial port baud rate.");
            this.cboAlphaBaud.SelectedIndexChanged += new System.EventHandler(this.cboAlphaBaud_SelectedIndexChanged);
            // 
            // btnHF
            // 
            this.btnHF.BackColor = System.Drawing.SystemColors.Control;
            this.btnHF.Enabled = false;
            this.btnHF.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnHF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHF.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnHF.Location = new System.Drawing.Point(227, 67);
            this.btnHF.Name = "btnHF";
            this.btnHF.Size = new System.Drawing.Size(15, 15);
            this.btnHF.TabIndex = 52;
            this.toolTip1.SetToolTip(this.btnHF, "Press to display Hard Fault log (Double-Click button label to clear).");
            this.btnHF.UseVisualStyleBackColor = false;
            this.btnHF.Click += new System.EventHandler(this.btnHF_Click);
            // 
            // btnSF
            // 
            this.btnSF.BackColor = System.Drawing.SystemColors.Control;
            this.btnSF.Enabled = false;
            this.btnSF.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnSF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSF.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSF.Location = new System.Drawing.Point(206, 67);
            this.btnSF.Name = "btnSF";
            this.btnSF.Size = new System.Drawing.Size(15, 15);
            this.btnSF.TabIndex = 51;
            this.toolTip1.SetToolTip(this.btnSF, "Press to display Soft Fault log (Double-Click button label to clear).");
            this.btnSF.UseVisualStyleBackColor = false;
            this.btnSF.Click += new System.EventHandler(this.btnSF_Click);
            // 
            // lblHF
            // 
            this.lblHF.AutoSize = true;
            this.lblHF.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHF.ForeColor = System.Drawing.Color.Navy;
            this.lblHF.Location = new System.Drawing.Point(226, 49);
            this.lblHF.Name = "lblHF";
            this.lblHF.Size = new System.Drawing.Size(18, 12);
            this.lblHF.TabIndex = 50;
            this.lblHF.Text = "HF";
            this.lblHF.DoubleClick += new System.EventHandler(this.lblHF_DoubleClick);
            // 
            // txtMsg
            // 
            this.txtMsg.BackColor = System.Drawing.SystemColors.Info;
            this.txtMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMsg.Location = new System.Drawing.Point(6, 102);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ReadOnly = true;
            this.txtMsg.Size = new System.Drawing.Size(238, 20);
            this.txtMsg.TabIndex = 48;
            this.txtMsg.Text = "Status and Fault messages displayed here.";
            this.txtMsg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtMsg, "Displays Status and Fault messages.");
            // 
            // lblSF
            // 
            this.lblSF.AutoSize = true;
            this.lblSF.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSF.ForeColor = System.Drawing.Color.Navy;
            this.lblSF.Location = new System.Drawing.Point(205, 49);
            this.lblSF.Name = "lblSF";
            this.lblSF.Size = new System.Drawing.Size(17, 12);
            this.lblSF.TabIndex = 47;
            this.lblSF.Text = "SF";
            this.lblSF.DoubleClick += new System.EventHandler(this.lblSF_DoubleClick);
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label69.ForeColor = System.Drawing.Color.Navy;
            this.label69.Location = new System.Drawing.Point(285, 48);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(26, 13);
            this.label69.TabIndex = 46;
            this.label69.Text = "Seg";
            // 
            // txtSeg
            // 
            this.txtSeg.BackColor = System.Drawing.SystemColors.Info;
            this.txtSeg.Enabled = false;
            this.txtSeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSeg.Location = new System.Drawing.Point(285, 66);
            this.txtSeg.Name = "txtSeg";
            this.txtSeg.ReadOnly = true;
            this.txtSeg.Size = new System.Drawing.Size(26, 20);
            this.txtSeg.TabIndex = 45;
            this.txtSeg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtSeg, "Current Band segment");
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label70.ForeColor = System.Drawing.Color.Navy;
            this.label70.Location = new System.Drawing.Point(215, 11);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(42, 13);
            this.label70.TabIndex = 42;
            this.label70.Text = "Interval";
            // 
            // txtAlphaInt
            // 
            this.txtAlphaInt.BackColor = System.Drawing.SystemColors.Window;
            this.txtAlphaInt.Enabled = false;
            this.txtAlphaInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAlphaInt.Location = new System.Drawing.Point(213, 25);
            this.txtAlphaInt.Name = "txtAlphaInt";
            this.txtAlphaInt.Size = new System.Drawing.Size(47, 20);
            this.txtAlphaInt.TabIndex = 41;
            this.txtAlphaInt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtAlphaInt, "Interval to poll amplifier for status (1000 = 1 sec.)");
            this.txtAlphaInt.TextChanged += new System.EventHandler(this.txtAlphaInt_TextChanged);
            // 
            // btnHV
            // 
            this.btnHV.BackColor = System.Drawing.SystemColors.Control;
            this.btnHV.Enabled = false;
            this.btnHV.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnHV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHV.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnHV.Location = new System.Drawing.Point(157, 64);
            this.btnHV.Name = "btnHV";
            this.btnHV.Size = new System.Drawing.Size(41, 22);
            this.btnHV.TabIndex = 39;
            this.btnHV.Text = "Off";
            this.toolTip1.SetToolTip(this.btnHV, "Press to toggle plate voltage High/Low  .");
            this.btnHV.UseVisualStyleBackColor = false;
            this.btnHV.Click += new System.EventHandler(this.btnHV_Click);
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label77.ForeColor = System.Drawing.Color.Navy;
            this.label77.Location = new System.Drawing.Point(167, 49);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(21, 13);
            this.label77.TabIndex = 38;
            this.label77.Text = "HV";
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label76.ForeColor = System.Drawing.Color.Navy;
            this.label76.Location = new System.Drawing.Point(9, 49);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(37, 13);
            this.label76.TabIndex = 37;
            this.label76.Text = "Power";
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label75.ForeColor = System.Drawing.Color.Navy;
            this.label75.Location = new System.Drawing.Point(73, 11);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(55, 13);
            this.label75.TabIndex = 35;
            this.label75.Text = "Serial Port";
            // 
            // cboAlpha
            // 
            this.cboAlpha.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlpha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAlpha.ForeColor = System.Drawing.Color.Black;
            this.cboAlpha.FormattingEnabled = true;
            this.cboAlpha.Location = new System.Drawing.Point(68, 25);
            this.cboAlpha.Name = "cboAlpha";
            this.cboAlpha.Size = new System.Drawing.Size(68, 21);
            this.cboAlpha.TabIndex = 33;
            this.toolTip1.SetToolTip(this.cboAlpha, "Hardware serial port connected to amp.");
            this.cboAlpha.SelectedIndexChanged += new System.EventHandler(this.cboAlpha_SelectedIndexChanged);
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label73.ForeColor = System.Drawing.Color.Navy;
            this.label73.Location = new System.Drawing.Point(102, 49);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(52, 13);
            this.label73.TabIndex = 11;
            this.label73.Text = "AutoTune";
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label72.ForeColor = System.Drawing.Color.Navy;
            this.label72.Location = new System.Drawing.Point(249, 48);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(32, 13);
            this.label72.TabIndex = 10;
            this.label72.Text = "Band";
            // 
            // btnOper
            // 
            this.btnOper.BackColor = System.Drawing.SystemColors.Control;
            this.btnOper.Enabled = false;
            this.btnOper.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOper.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOper.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOper.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOper.Location = new System.Drawing.Point(57, 64);
            this.btnOper.Name = "btnOper";
            this.btnOper.Size = new System.Drawing.Size(41, 22);
            this.btnOper.TabIndex = 9;
            this.btnOper.Text = "Off";
            this.toolTip1.SetToolTip(this.btnOper, "Press to toggle Operate/StandBy modes.");
            this.btnOper.UseVisualStyleBackColor = false;
            this.btnOper.Click += new System.EventHandler(this.btnOper_Click);
            // 
            // btnTune
            // 
            this.btnTune.BackColor = System.Drawing.SystemColors.Control;
            this.btnTune.Enabled = false;
            this.btnTune.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnTune.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTune.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTune.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTune.Location = new System.Drawing.Point(107, 64);
            this.btnTune.Name = "btnTune";
            this.btnTune.Size = new System.Drawing.Size(41, 22);
            this.btnTune.TabIndex = 8;
            this.btnTune.Text = "Off";
            this.toolTip1.SetToolTip(this.btnTune, "Press to toggle AutoTune function On/Off.");
            this.btnTune.UseVisualStyleBackColor = false;
            this.btnTune.Click += new System.EventHandler(this.btnTune_Click);
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label71.ForeColor = System.Drawing.Color.Navy;
            this.label71.Location = new System.Drawing.Point(55, 49);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(45, 13);
            this.label71.TabIndex = 7;
            this.label71.Text = "Operate";
            this.toolTip1.SetToolTip(this.label71, "StdBy/Operate");
            // 
            // txtBand
            // 
            this.txtBand.BackColor = System.Drawing.SystemColors.Info;
            this.txtBand.Enabled = false;
            this.txtBand.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBand.Location = new System.Drawing.Point(252, 65);
            this.txtBand.Name = "txtBand";
            this.txtBand.ReadOnly = true;
            this.txtBand.Size = new System.Drawing.Size(26, 20);
            this.txtBand.TabIndex = 4;
            this.txtBand.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtBand, "Current Band setting in meters.");
            // 
            // chkAlpha
            // 
            this.chkAlpha.AutoSize = true;
            this.chkAlpha.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.chkAlpha.ForeColor = System.Drawing.Color.Navy;
            this.chkAlpha.Location = new System.Drawing.Point(9, 26);
            this.chkAlpha.Name = "chkAlpha";
            this.chkAlpha.Size = new System.Drawing.Size(59, 17);
            this.chkAlpha.TabIndex = 1;
            this.chkAlpha.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkAlpha, "Select to enable Alpha amplifier support");
            this.chkAlpha.UseVisualStyleBackColor = true;
            this.chkAlpha.CheckedChanged += new System.EventHandler(this.chkAlpha_CheckedChanged);
            // 
            // btnPwr
            // 
            this.btnPwr.BackColor = System.Drawing.SystemColors.Control;
            this.btnPwr.Enabled = false;
            this.btnPwr.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPwr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPwr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPwr.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPwr.Location = new System.Drawing.Point(7, 64);
            this.btnPwr.Name = "btnPwr";
            this.btnPwr.Size = new System.Drawing.Size(41, 22);
            this.btnPwr.TabIndex = 0;
            this.btnPwr.Text = "Off";
            this.toolTip1.SetToolTip(this.btnPwr, "Press to toggle Amplifier power On/Off amplifier.");
            this.btnPwr.UseVisualStyleBackColor = false;
            this.btnPwr.Click += new System.EventHandler(this.btnPwr_Click);
            // 
            // tabMacro
            // 
            this.tabMacro.Controls.Add(this.label34);
            this.tabMacro.Controls.Add(this.label35);
            this.tabMacro.Controls.Add(this.label36);
            this.tabMacro.Controls.Add(this.label37);
            this.tabMacro.Controls.Add(this.label38);
            this.tabMacro.Controls.Add(this.label39);
            this.tabMacro.Controls.Add(this.btnMacro12);
            this.tabMacro.Controls.Add(this.btnMacro11);
            this.tabMacro.Controls.Add(this.btnMacro10);
            this.tabMacro.Controls.Add(this.btnMacro9);
            this.tabMacro.Controls.Add(this.btnMacro8);
            this.tabMacro.Controls.Add(this.btnMacro7);
            this.tabMacro.Controls.Add(this.grpMacro);
            this.tabMacro.Controls.Add(this.dgm);
            this.tabMacro.Controls.Add(this.label33);
            this.tabMacro.Controls.Add(this.label32);
            this.tabMacro.Controls.Add(this.label31);
            this.tabMacro.Controls.Add(this.label30);
            this.tabMacro.Controls.Add(this.label29);
            this.tabMacro.Controls.Add(this.label28);
            this.tabMacro.Controls.Add(this.btnMacro6);
            this.tabMacro.Controls.Add(this.btnMacro5);
            this.tabMacro.Controls.Add(this.btnMacro4);
            this.tabMacro.Controls.Add(this.btnMacro3);
            this.tabMacro.Controls.Add(this.btnMacro2);
            this.tabMacro.Controls.Add(this.btnMacro1);
            this.tabMacro.Location = new System.Drawing.Point(4, 22);
            this.tabMacro.Name = "tabMacro";
            this.tabMacro.Size = new System.Drawing.Size(417, 309);
            this.tabMacro.TabIndex = 4;
            this.tabMacro.Text = "Macro";
            this.tabMacro.UseVisualStyleBackColor = true;
            this.tabMacro.Click += new System.EventHandler(this.tabMacro_Click);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.Color.Firebrick;
            this.label34.Location = new System.Drawing.Point(365, 225);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(24, 12);
            this.label34.TabIndex = 98;
            this.label34.Text = "F12";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.Firebrick;
            this.label35.Location = new System.Drawing.Point(295, 226);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(24, 12);
            this.label35.TabIndex = 97;
            this.label35.Text = "F11";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.ForeColor = System.Drawing.Color.Firebrick;
            this.label36.Location = new System.Drawing.Point(229, 226);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(24, 12);
            this.label36.TabIndex = 96;
            this.label36.Text = "F10";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.ForeColor = System.Drawing.Color.Firebrick;
            this.label37.Location = new System.Drawing.Point(165, 226);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(18, 12);
            this.label37.TabIndex = 95;
            this.label37.Text = "F9";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.ForeColor = System.Drawing.Color.Firebrick;
            this.label38.Location = new System.Drawing.Point(94, 226);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(18, 12);
            this.label38.TabIndex = 94;
            this.label38.Text = "F8";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.ForeColor = System.Drawing.Color.Firebrick;
            this.label39.Location = new System.Drawing.Point(26, 226);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(18, 12);
            this.label39.TabIndex = 93;
            this.label39.Text = "F7";
            // 
            // btnMacro12
            // 
            this.btnMacro12.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro12.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro12.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro12.Location = new System.Drawing.Point(346, 241);
            this.btnMacro12.Name = "btnMacro12";
            this.btnMacro12.Size = new System.Drawing.Size(65, 19);
            this.btnMacro12.TabIndex = 92;
            this.btnMacro12.Text = "Macro 12";
            this.toolTip1.SetToolTip(this.btnMacro12, "Executes the Macro Commands assigned to this button");
            this.btnMacro12.UseVisualStyleBackColor = false;
            this.btnMacro12.Click += new System.EventHandler(this.btnMacro12_Click);
            // 
            // btnMacro11
            // 
            this.btnMacro11.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro11.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro11.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro11.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro11.Location = new System.Drawing.Point(277, 241);
            this.btnMacro11.Name = "btnMacro11";
            this.btnMacro11.Size = new System.Drawing.Size(65, 19);
            this.btnMacro11.TabIndex = 91;
            this.btnMacro11.Text = "Macro 11";
            this.toolTip1.SetToolTip(this.btnMacro11, "Executes the Macro Commands assigned to this button");
            this.btnMacro11.UseVisualStyleBackColor = false;
            this.btnMacro11.Click += new System.EventHandler(this.btnMacro11_Click);
            // 
            // btnMacro10
            // 
            this.btnMacro10.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro10.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro10.Location = new System.Drawing.Point(209, 241);
            this.btnMacro10.Name = "btnMacro10";
            this.btnMacro10.Size = new System.Drawing.Size(65, 19);
            this.btnMacro10.TabIndex = 90;
            this.btnMacro10.Text = "Macro 10";
            this.toolTip1.SetToolTip(this.btnMacro10, "Executes the Macro Commands assigned to this button");
            this.btnMacro10.UseVisualStyleBackColor = false;
            this.btnMacro10.Click += new System.EventHandler(this.btnMacro10_Click);
            // 
            // btnMacro9
            // 
            this.btnMacro9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro9.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro9.Location = new System.Drawing.Point(141, 241);
            this.btnMacro9.Name = "btnMacro9";
            this.btnMacro9.Size = new System.Drawing.Size(65, 19);
            this.btnMacro9.TabIndex = 89;
            this.btnMacro9.Text = "Macro 9";
            this.toolTip1.SetToolTip(this.btnMacro9, "Executes the Macro Commands assigned to this button");
            this.btnMacro9.UseVisualStyleBackColor = false;
            this.btnMacro9.Click += new System.EventHandler(this.btnMacro9_Click);
            // 
            // btnMacro8
            // 
            this.btnMacro8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro8.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro8.Location = new System.Drawing.Point(74, 241);
            this.btnMacro8.Name = "btnMacro8";
            this.btnMacro8.Size = new System.Drawing.Size(65, 19);
            this.btnMacro8.TabIndex = 88;
            this.btnMacro8.Text = "Macro 8";
            this.toolTip1.SetToolTip(this.btnMacro8, "Executes the Macro Commands assigned to this button");
            this.btnMacro8.UseVisualStyleBackColor = false;
            this.btnMacro8.Click += new System.EventHandler(this.btnMacro8_Click);
            // 
            // btnMacro7
            // 
            this.btnMacro7.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro7.Location = new System.Drawing.Point(6, 241);
            this.btnMacro7.Name = "btnMacro7";
            this.btnMacro7.Size = new System.Drawing.Size(65, 19);
            this.btnMacro7.TabIndex = 87;
            this.btnMacro7.Text = "Macro 7";
            this.toolTip1.SetToolTip(this.btnMacro7, "Executes the Macro Commands assigned to this button");
            this.btnMacro7.UseVisualStyleBackColor = false;
            this.btnMacro7.Click += new System.EventHandler(this.btnMacro7_Click);
            // 
            // grpMacro
            // 
            this.grpMacro.BackColor = System.Drawing.Color.Transparent;
            this.grpMacro.Controls.Add(this.btnMacSelect);
            this.grpMacro.Controls.Add(this.btnMacSave);
            this.grpMacro.Controls.Add(this.btnMacReLoad);
            this.grpMacro.Controls.Add(this.label26);
            this.grpMacro.Controls.Add(this.txtMacFile);
            this.grpMacro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpMacro.ForeColor = System.Drawing.Color.Navy;
            this.grpMacro.Location = new System.Drawing.Point(8, 158);
            this.grpMacro.Name = "grpMacro";
            this.grpMacro.Size = new System.Drawing.Size(369, 64);
            this.grpMacro.TabIndex = 81;
            this.grpMacro.TabStop = false;
            this.grpMacro.Text = "Data File";
            // 
            // btnMacSelect
            // 
            this.btnMacSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacSelect.ForeColor = System.Drawing.Color.Blue;
            this.btnMacSelect.Location = new System.Drawing.Point(95, 39);
            this.btnMacSelect.Name = "btnMacSelect";
            this.btnMacSelect.Size = new System.Drawing.Size(55, 19);
            this.btnMacSelect.TabIndex = 60;
            this.btnMacSelect.Text = "Select";
            this.toolTip1.SetToolTip(this.btnMacSelect, "Select Macro Data File to load");
            this.btnMacSelect.UseVisualStyleBackColor = true;
            this.btnMacSelect.Click += new System.EventHandler(this.btnMacSelect_Click);
            // 
            // btnMacSave
            // 
            this.btnMacSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacSave.ForeColor = System.Drawing.Color.Blue;
            this.btnMacSave.Location = new System.Drawing.Point(252, 39);
            this.btnMacSave.Name = "btnMacSave";
            this.btnMacSave.Size = new System.Drawing.Size(55, 19);
            this.btnMacSave.TabIndex = 79;
            this.btnMacSave.Text = "Save";
            this.toolTip1.SetToolTip(this.btnMacSave, "Press to save Macro Data File shown in Data File window.");
            this.btnMacSave.UseVisualStyleBackColor = false;
            this.btnMacSave.Click += new System.EventHandler(this.btnMacSave_Click);
            // 
            // btnMacReLoad
            // 
            this.btnMacReLoad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacReLoad.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacReLoad.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacReLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacReLoad.ForeColor = System.Drawing.Color.Blue;
            this.btnMacReLoad.Location = new System.Drawing.Point(171, 39);
            this.btnMacReLoad.Name = "btnMacReLoad";
            this.btnMacReLoad.Size = new System.Drawing.Size(55, 19);
            this.btnMacReLoad.TabIndex = 80;
            this.btnMacReLoad.Text = "ReLoad";
            this.toolTip1.SetToolTip(this.btnMacReLoad, "Re-Load Macro Data File last loaded.");
            this.btnMacReLoad.UseVisualStyleBackColor = false;
            this.btnMacReLoad.Click += new System.EventHandler(this.btnMacReLoad_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label26.ForeColor = System.Drawing.Color.Navy;
            this.label26.Location = new System.Drawing.Point(10, 40);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(54, 13);
            this.label26.TabIndex = 59;
            this.label26.Text = "File Name";
            // 
            // txtMacFile
            // 
            this.txtMacFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtMacFile.Location = new System.Drawing.Point(6, 15);
            this.txtMacFile.Name = "txtMacFile";
            this.txtMacFile.Size = new System.Drawing.Size(357, 20);
            this.txtMacFile.TabIndex = 58;
            this.toolTip1.SetToolTip(this.txtMacFile, "Macro Dat File name and location");
            this.txtMacFile.WordWrap = false;
            // 
            // dgm
            // 
            this.dgm.AllowUserToAddRows = false;
            this.dgm.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            this.dgm.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgm.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgm.Location = new System.Drawing.Point(4, 3);
            this.dgm.Name = "dgm";
            this.dgm.RowHeadersWidth = 40;
            this.dgm.Size = new System.Drawing.Size(409, 151);
            this.dgm.TabIndex = 78;
            this.dgm.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgm_CellValueChanged);
            this.dgm.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgm_RowPostPaint);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.Firebrick;
            this.label33.Location = new System.Drawing.Point(373, 262);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(18, 12);
            this.label33.TabIndex = 86;
            this.label33.Text = "F6";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Firebrick;
            this.label32.Location = new System.Drawing.Point(296, 262);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(18, 12);
            this.label32.TabIndex = 85;
            this.label32.Text = "F5";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.Color.Firebrick;
            this.label31.Location = new System.Drawing.Point(230, 262);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(18, 12);
            this.label31.TabIndex = 84;
            this.label31.Text = "F4";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.Color.Firebrick;
            this.label30.Location = new System.Drawing.Point(160, 262);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(18, 12);
            this.label30.TabIndex = 83;
            this.label30.Text = "F3";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.Color.Firebrick;
            this.label29.Location = new System.Drawing.Point(95, 262);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(18, 12);
            this.label29.TabIndex = 82;
            this.label29.Text = "F2";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.Firebrick;
            this.label28.Location = new System.Drawing.Point(27, 262);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(18, 12);
            this.label28.TabIndex = 81;
            this.label28.Text = "F1";
            // 
            // btnMacro6
            // 
            this.btnMacro6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro6.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro6.Location = new System.Drawing.Point(347, 276);
            this.btnMacro6.Name = "btnMacro6";
            this.btnMacro6.Size = new System.Drawing.Size(65, 19);
            this.btnMacro6.TabIndex = 77;
            this.btnMacro6.Text = "Macro 6";
            this.toolTip1.SetToolTip(this.btnMacro6, "Executes the Macro Commands assigned to this button");
            this.btnMacro6.UseVisualStyleBackColor = false;
            this.btnMacro6.Click += new System.EventHandler(this.btnMacro6_Click);
            // 
            // btnMacro5
            // 
            this.btnMacro5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro5.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro5.Location = new System.Drawing.Point(278, 276);
            this.btnMacro5.Name = "btnMacro5";
            this.btnMacro5.Size = new System.Drawing.Size(65, 19);
            this.btnMacro5.TabIndex = 76;
            this.btnMacro5.Text = "Macro 5";
            this.toolTip1.SetToolTip(this.btnMacro5, "Executes the Macro Commands assigned to this button");
            this.btnMacro5.UseVisualStyleBackColor = false;
            this.btnMacro5.Click += new System.EventHandler(this.btnMacro5_Click);
            // 
            // btnMacro4
            // 
            this.btnMacro4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro4.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro4.Location = new System.Drawing.Point(210, 276);
            this.btnMacro4.Name = "btnMacro4";
            this.btnMacro4.Size = new System.Drawing.Size(65, 19);
            this.btnMacro4.TabIndex = 75;
            this.btnMacro4.Text = "Macro 4";
            this.toolTip1.SetToolTip(this.btnMacro4, "Executes the Macro Commands assigned to this button");
            this.btnMacro4.UseVisualStyleBackColor = false;
            this.btnMacro4.Click += new System.EventHandler(this.btnMacro4_Click);
            // 
            // btnMacro3
            // 
            this.btnMacro3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro3.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro3.Location = new System.Drawing.Point(142, 276);
            this.btnMacro3.Name = "btnMacro3";
            this.btnMacro3.Size = new System.Drawing.Size(65, 19);
            this.btnMacro3.TabIndex = 74;
            this.btnMacro3.Text = "Macro 3";
            this.toolTip1.SetToolTip(this.btnMacro3, "Executes the Macro Commands assigned to this button");
            this.btnMacro3.UseVisualStyleBackColor = false;
            this.btnMacro3.Click += new System.EventHandler(this.btnMacro3_Click);
            // 
            // btnMacro2
            // 
            this.btnMacro2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro2.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro2.Location = new System.Drawing.Point(75, 276);
            this.btnMacro2.Name = "btnMacro2";
            this.btnMacro2.Size = new System.Drawing.Size(65, 19);
            this.btnMacro2.TabIndex = 73;
            this.btnMacro2.Text = "Macro 2";
            this.toolTip1.SetToolTip(this.btnMacro2, "Executes the Macro Commands assigned to this button");
            this.btnMacro2.UseVisualStyleBackColor = false;
            this.btnMacro2.Click += new System.EventHandler(this.btnMacro2_Click);
            // 
            // btnMacro1
            // 
            this.btnMacro1.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro1.Location = new System.Drawing.Point(7, 276);
            this.btnMacro1.Name = "btnMacro1";
            this.btnMacro1.Size = new System.Drawing.Size(65, 19);
            this.btnMacro1.TabIndex = 72;
            this.btnMacro1.Text = "Macro 1";
            this.toolTip1.SetToolTip(this.btnMacro1, "Executes the Macro Commands assigned to this button");
            this.btnMacro1.UseVisualStyleBackColor = false;
            this.btnMacro1.Click += new System.EventHandler(this.btnMacro1_Click);
            // 
            // tabRotor
            // 
            this.tabRotor.Controls.Add(this.grpPreset);
            this.tabRotor.Controls.Add(this.grpStepCtrl);
            this.tabRotor.Controls.Add(this.groupBox11);
            this.tabRotor.Controls.Add(this.groupBox9);
            this.tabRotor.Controls.Add(this.groupBox8);
            this.tabRotor.Location = new System.Drawing.Point(4, 22);
            this.tabRotor.Name = "tabRotor";
            this.tabRotor.Size = new System.Drawing.Size(417, 309);
            this.tabRotor.TabIndex = 5;
            this.tabRotor.Text = "Rotor";
            this.tabRotor.UseVisualStyleBackColor = true;
            // 
            // grpPreset
            // 
            this.grpPreset.Controls.Add(this.rbPre16);
            this.grpPreset.Controls.Add(this.rbPre8);
            this.grpPreset.Controls.Add(this.rbPre15);
            this.grpPreset.Controls.Add(this.rbPre7);
            this.grpPreset.Controls.Add(this.rbPre14);
            this.grpPreset.Controls.Add(this.rbPre6);
            this.grpPreset.Controls.Add(this.rbPre13);
            this.grpPreset.Controls.Add(this.rbPre5);
            this.grpPreset.Controls.Add(this.rbPre12);
            this.grpPreset.Controls.Add(this.rbPre4);
            this.grpPreset.Controls.Add(this.rbPre11);
            this.grpPreset.Controls.Add(this.rbPre3);
            this.grpPreset.Controls.Add(this.rbPre10);
            this.grpPreset.Controls.Add(this.rbPre2);
            this.grpPreset.Controls.Add(this.rbPre9);
            this.grpPreset.Controls.Add(this.rbPre1);
            this.grpPreset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpPreset.ForeColor = System.Drawing.Color.Navy;
            this.grpPreset.Location = new System.Drawing.Point(5, 110);
            this.grpPreset.Name = "grpPreset";
            this.grpPreset.Size = new System.Drawing.Size(405, 50);
            this.grpPreset.TabIndex = 14;
            this.grpPreset.TabStop = false;
            this.grpPreset.Text = "Rotor Presets";
            // 
            // rbPre16
            // 
            this.rbPre16.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre16.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre16.ForeColor = System.Drawing.Color.Navy;
            this.rbPre16.Location = new System.Drawing.Point(355, 30);
            this.rbPre16.Name = "rbPre16";
            this.rbPre16.Size = new System.Drawing.Size(44, 17);
            this.rbPre16.TabIndex = 118;
            this.rbPre16.Text = "359";
            this.toolTip1.SetToolTip(this.rbPre16, "Preset antenna heading in degrees.");
            this.rbPre16.UseVisualStyleBackColor = true;
            this.rbPre16.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre8
            // 
            this.rbPre8.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre8.ForeColor = System.Drawing.Color.Navy;
            this.rbPre8.Location = new System.Drawing.Point(355, 15);
            this.rbPre8.Name = "rbPre8";
            this.rbPre8.Size = new System.Drawing.Size(44, 17);
            this.rbPre8.TabIndex = 117;
            this.rbPre8.Text = "150";
            this.toolTip1.SetToolTip(this.rbPre8, "Preset antenna heading in degrees.");
            this.rbPre8.UseVisualStyleBackColor = true;
            this.rbPre8.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre15
            // 
            this.rbPre15.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre15.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre15.ForeColor = System.Drawing.Color.Navy;
            this.rbPre15.Location = new System.Drawing.Point(305, 30);
            this.rbPre15.Name = "rbPre15";
            this.rbPre15.Size = new System.Drawing.Size(44, 17);
            this.rbPre15.TabIndex = 116;
            this.rbPre15.Text = "330";
            this.toolTip1.SetToolTip(this.rbPre15, "Preset antenna heading in degrees.");
            this.rbPre15.UseVisualStyleBackColor = true;
            this.rbPre15.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre7
            // 
            this.rbPre7.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre7.ForeColor = System.Drawing.Color.Navy;
            this.rbPre7.Location = new System.Drawing.Point(305, 15);
            this.rbPre7.Name = "rbPre7";
            this.rbPre7.Size = new System.Drawing.Size(44, 17);
            this.rbPre7.TabIndex = 115;
            this.rbPre7.Text = "120";
            this.toolTip1.SetToolTip(this.rbPre7, "Preset antenna heading in degrees.");
            this.rbPre7.UseVisualStyleBackColor = true;
            this.rbPre7.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre14
            // 
            this.rbPre14.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre14.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre14.ForeColor = System.Drawing.Color.Navy;
            this.rbPre14.Location = new System.Drawing.Point(255, 30);
            this.rbPre14.Name = "rbPre14";
            this.rbPre14.Size = new System.Drawing.Size(44, 17);
            this.rbPre14.TabIndex = 114;
            this.rbPre14.Text = "315";
            this.toolTip1.SetToolTip(this.rbPre14, "Preset antenna heading in degrees.");
            this.rbPre14.UseVisualStyleBackColor = true;
            this.rbPre14.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre6
            // 
            this.rbPre6.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre6.ForeColor = System.Drawing.Color.Navy;
            this.rbPre6.Location = new System.Drawing.Point(255, 15);
            this.rbPre6.Name = "rbPre6";
            this.rbPre6.Size = new System.Drawing.Size(44, 17);
            this.rbPre6.TabIndex = 113;
            this.rbPre6.Text = "90";
            this.toolTip1.SetToolTip(this.rbPre6, "Preset antenna heading in degrees.");
            this.rbPre6.UseVisualStyleBackColor = true;
            this.rbPre6.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre13
            // 
            this.rbPre13.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre13.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre13.ForeColor = System.Drawing.Color.Navy;
            this.rbPre13.Location = new System.Drawing.Point(205, 30);
            this.rbPre13.Name = "rbPre13";
            this.rbPre13.Size = new System.Drawing.Size(44, 17);
            this.rbPre13.TabIndex = 112;
            this.rbPre13.Text = "300";
            this.toolTip1.SetToolTip(this.rbPre13, "Preset antenna heading in degrees.");
            this.rbPre13.UseVisualStyleBackColor = true;
            this.rbPre13.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre5
            // 
            this.rbPre5.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre5.ForeColor = System.Drawing.Color.Navy;
            this.rbPre5.Location = new System.Drawing.Point(205, 15);
            this.rbPre5.Name = "rbPre5";
            this.rbPre5.Size = new System.Drawing.Size(44, 17);
            this.rbPre5.TabIndex = 111;
            this.rbPre5.Text = "75";
            this.toolTip1.SetToolTip(this.rbPre5, "Preset antenna heading in degrees.");
            this.rbPre5.UseVisualStyleBackColor = true;
            this.rbPre5.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre12
            // 
            this.rbPre12.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre12.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre12.ForeColor = System.Drawing.Color.Navy;
            this.rbPre12.Location = new System.Drawing.Point(155, 30);
            this.rbPre12.Name = "rbPre12";
            this.rbPre12.Size = new System.Drawing.Size(44, 17);
            this.rbPre12.TabIndex = 110;
            this.rbPre12.Text = "270";
            this.toolTip1.SetToolTip(this.rbPre12, "Preset antenna heading in degrees.");
            this.rbPre12.UseVisualStyleBackColor = true;
            this.rbPre12.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre4
            // 
            this.rbPre4.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre4.ForeColor = System.Drawing.Color.Navy;
            this.rbPre4.Location = new System.Drawing.Point(155, 15);
            this.rbPre4.Name = "rbPre4";
            this.rbPre4.Size = new System.Drawing.Size(44, 17);
            this.rbPre4.TabIndex = 109;
            this.rbPre4.Text = "60";
            this.toolTip1.SetToolTip(this.rbPre4, "Preset antenna heading in degrees.");
            this.rbPre4.UseVisualStyleBackColor = true;
            this.rbPre4.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre11
            // 
            this.rbPre11.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre11.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre11.ForeColor = System.Drawing.Color.Navy;
            this.rbPre11.Location = new System.Drawing.Point(105, 30);
            this.rbPre11.Name = "rbPre11";
            this.rbPre11.Size = new System.Drawing.Size(44, 17);
            this.rbPre11.TabIndex = 108;
            this.rbPre11.Text = "240";
            this.toolTip1.SetToolTip(this.rbPre11, "Preset antenna heading in degrees.");
            this.rbPre11.UseVisualStyleBackColor = true;
            this.rbPre11.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre3
            // 
            this.rbPre3.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre3.ForeColor = System.Drawing.Color.Navy;
            this.rbPre3.Location = new System.Drawing.Point(105, 15);
            this.rbPre3.Name = "rbPre3";
            this.rbPre3.Size = new System.Drawing.Size(44, 17);
            this.rbPre3.TabIndex = 107;
            this.rbPre3.Text = "45";
            this.toolTip1.SetToolTip(this.rbPre3, "Preset antenna heading in degrees.");
            this.rbPre3.UseVisualStyleBackColor = true;
            this.rbPre3.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre10
            // 
            this.rbPre10.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre10.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre10.ForeColor = System.Drawing.Color.Navy;
            this.rbPre10.Location = new System.Drawing.Point(55, 30);
            this.rbPre10.Name = "rbPre10";
            this.rbPre10.Size = new System.Drawing.Size(44, 17);
            this.rbPre10.TabIndex = 106;
            this.rbPre10.Text = "210";
            this.toolTip1.SetToolTip(this.rbPre10, "Preset antenna heading in degrees.");
            this.rbPre10.UseVisualStyleBackColor = true;
            this.rbPre10.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre2
            // 
            this.rbPre2.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre2.ForeColor = System.Drawing.Color.Navy;
            this.rbPre2.Location = new System.Drawing.Point(55, 15);
            this.rbPre2.Name = "rbPre2";
            this.rbPre2.Size = new System.Drawing.Size(44, 17);
            this.rbPre2.TabIndex = 105;
            this.rbPre2.Text = "30";
            this.toolTip1.SetToolTip(this.rbPre2, "Preset antenna heading in degrees.");
            this.rbPre2.UseVisualStyleBackColor = true;
            this.rbPre2.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre9
            // 
            this.rbPre9.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre9.ForeColor = System.Drawing.Color.Navy;
            this.rbPre9.Location = new System.Drawing.Point(5, 30);
            this.rbPre9.Name = "rbPre9";
            this.rbPre9.Size = new System.Drawing.Size(44, 17);
            this.rbPre9.TabIndex = 104;
            this.rbPre9.Text = "180";
            this.toolTip1.SetToolTip(this.rbPre9, "Preset antenna heading in degrees.");
            this.rbPre9.UseVisualStyleBackColor = true;
            this.rbPre9.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // rbPre1
            // 
            this.rbPre1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbPre1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbPre1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPre1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbPre1.ForeColor = System.Drawing.Color.Navy;
            this.rbPre1.Location = new System.Drawing.Point(5, 15);
            this.rbPre1.Name = "rbPre1";
            this.rbPre1.Size = new System.Drawing.Size(44, 17);
            this.rbPre1.TabIndex = 103;
            this.rbPre1.Text = "15";
            this.toolTip1.SetToolTip(this.rbPre1, "Preset antenna heading in degrees.");
            this.rbPre1.UseVisualStyleBackColor = true;
            this.rbPre1.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // grpStepCtrl
            // 
            this.grpStepCtrl.Controls.Add(this.rb34);
            this.grpStepCtrl.Controls.Add(this.rbBiDir);
            this.grpStepCtrl.Controls.Add(this.rb180);
            this.grpStepCtrl.Controls.Add(this.rbFwd);
            this.grpStepCtrl.Controls.Add(this.btnCalib);
            this.grpStepCtrl.Controls.Add(this.btnHome);
            this.grpStepCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpStepCtrl.ForeColor = System.Drawing.Color.Navy;
            this.grpStepCtrl.Location = new System.Drawing.Point(5, 265);
            this.grpStepCtrl.Name = "grpStepCtrl";
            this.grpStepCtrl.Size = new System.Drawing.Size(405, 40);
            this.grpStepCtrl.TabIndex = 13;
            this.grpStepCtrl.TabStop = false;
            this.grpStepCtrl.Text = "SteppIR Control";
            // 
            // rb34
            // 
            this.rb34.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rb34.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rb34.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rb34.ForeColor = System.Drawing.Color.Navy;
            this.rb34.Location = new System.Drawing.Point(190, 17);
            this.rb34.Name = "rb34";
            this.rb34.Size = new System.Drawing.Size(50, 17);
            this.rb34.TabIndex = 102;
            this.rb34.Text = "3/4";
            this.toolTip1.SetToolTip(this.rb34, "Select to put vertical in 3/4 wavelength mode (15 & 10  meters only)");
            this.rb34.UseVisualStyleBackColor = true;
            this.rb34.CheckedChanged += new System.EventHandler(this.rb34_CheckedChanged);
            // 
            // rbBiDir
            // 
            this.rbBiDir.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbBiDir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbBiDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbBiDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbBiDir.ForeColor = System.Drawing.Color.Navy;
            this.rbBiDir.Location = new System.Drawing.Point(130, 17);
            this.rbBiDir.Name = "rbBiDir";
            this.rbBiDir.Size = new System.Drawing.Size(54, 17);
            this.rbBiDir.TabIndex = 101;
            this.rbBiDir.Text = "<-->";
            this.toolTip1.SetToolTip(this.rbBiDir, "Select to put yagi in bi-directional mode");
            this.rbBiDir.UseVisualStyleBackColor = true;
            this.rbBiDir.CheckedChanged += new System.EventHandler(this.rbBiDir_CheckedChanged);
            // 
            // rb180
            // 
            this.rb180.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rb180.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rb180.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb180.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rb180.ForeColor = System.Drawing.Color.Navy;
            this.rb180.Location = new System.Drawing.Point(70, 17);
            this.rb180.Name = "rb180";
            this.rb180.Size = new System.Drawing.Size(51, 17);
            this.rb180.TabIndex = 100;
            this.rb180.Text = "180";
            this.toolTip1.SetToolTip(this.rb180, "Select to put yagi in 180 mode (reverse)");
            this.rb180.UseVisualStyleBackColor = true;
            this.rb180.CheckedChanged += new System.EventHandler(this.rb180_CheckedChanged);
            // 
            // rbFwd
            // 
            this.rbFwd.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbFwd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbFwd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbFwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbFwd.ForeColor = System.Drawing.Color.Navy;
            this.rbFwd.Location = new System.Drawing.Point(10, 17);
            this.rbFwd.Name = "rbFwd";
            this.rbFwd.Size = new System.Drawing.Size(53, 17);
            this.rbFwd.TabIndex = 99;
            this.rbFwd.Text = "Fwd";
            this.toolTip1.SetToolTip(this.rbFwd, "Select to put yagi in forward mode");
            this.rbFwd.UseVisualStyleBackColor = true;
            this.rbFwd.CheckedChanged += new System.EventHandler(this.rbFwd_CheckedChanged);
            // 
            // btnCalib
            // 
            this.btnCalib.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCalib.BackColor = System.Drawing.SystemColors.Control;
            this.btnCalib.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCalib.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnCalib.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnCalib.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCalib.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalib.ForeColor = System.Drawing.Color.Navy;
            this.btnCalib.Location = new System.Drawing.Point(330, 11);
            this.btnCalib.Name = "btnCalib";
            this.btnCalib.Size = new System.Drawing.Size(55, 21);
            this.btnCalib.TabIndex = 98;
            this.btnCalib.Text = "Calibrate";
            this.btnCalib.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.btnCalib, "Calibrate antenna elements (home & return).");
            this.btnCalib.UseVisualStyleBackColor = false;
            this.btnCalib.Click += new System.EventHandler(this.btnCalib_Click);
            // 
            // btnHome
            // 
            this.btnHome.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnHome.BackColor = System.Drawing.SystemColors.Control;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.Navy;
            this.btnHome.Location = new System.Drawing.Point(260, 11);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(55, 21);
            this.btnHome.TabIndex = 97;
            this.btnHome.Text = "Home";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.btnHome, "Retract the antenna elements (Home).");
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.lblDxTime);
            this.groupBox11.Controls.Add(this.txtDxIOTA);
            this.groupBox11.Controls.Add(this.label62);
            this.groupBox11.Controls.Add(this.txtDxTime);
            this.groupBox11.Controls.Add(this.label61);
            this.groupBox11.Controls.Add(this.label58);
            this.groupBox11.Controls.Add(this.txtDxITU);
            this.groupBox11.Controls.Add(this.label57);
            this.groupBox11.Controls.Add(this.txtDxCQ);
            this.groupBox11.Controls.Add(this.txtDxLat);
            this.groupBox11.Controls.Add(this.label55);
            this.groupBox11.Controls.Add(this.label56);
            this.groupBox11.Controls.Add(this.txtDxLong);
            this.groupBox11.Controls.Add(this.label53);
            this.groupBox11.Controls.Add(this.label54);
            this.groupBox11.Controls.Add(this.txtDxCont);
            this.groupBox11.Controls.Add(this.txtDxDist);
            this.groupBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox11.ForeColor = System.Drawing.Color.Navy;
            this.groupBox11.Location = new System.Drawing.Point(5, 165);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(405, 95);
            this.groupBox11.TabIndex = 12;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Location Data";
            // 
            // lblDxTime
            // 
            this.lblDxTime.AutoSize = true;
            this.lblDxTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDxTime.ForeColor = System.Drawing.Color.Blue;
            this.lblDxTime.Location = new System.Drawing.Point(95, 62);
            this.lblDxTime.Name = "lblDxTime";
            this.lblDxTime.Size = new System.Drawing.Size(113, 13);
            this.lblDxTime.TabIndex = 41;
            this.lblDxTime.Text = "Entity not selected";
            this.toolTip1.SetToolTip(this.lblDxTime, "Entity local time");
            // 
            // txtDxIOTA
            // 
            this.txtDxIOTA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDxIOTA.ForeColor = System.Drawing.Color.Firebrick;
            this.txtDxIOTA.Location = new System.Drawing.Point(335, 20);
            this.txtDxIOTA.Name = "txtDxIOTA";
            this.txtDxIOTA.Size = new System.Drawing.Size(55, 20);
            this.txtDxIOTA.TabIndex = 39;
            this.txtDxIOTA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtDxIOTA, "Entity IOTA number. To lookup enter number then hit Enter.");
            this.txtDxIOTA.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDxIOTA_KeyDown);
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label62.ForeColor = System.Drawing.Color.Navy;
            this.label62.Location = new System.Drawing.Point(345, 41);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(32, 13);
            this.label62.TabIndex = 40;
            this.label62.Text = "IOTA";
            this.label62.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtDxTime
            // 
            this.txtDxTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDxTime.ForeColor = System.Drawing.Color.Firebrick;
            this.txtDxTime.Location = new System.Drawing.Point(10, 56);
            this.txtDxTime.Name = "txtDxTime";
            this.txtDxTime.Size = new System.Drawing.Size(65, 20);
            this.txtDxTime.TabIndex = 37;
            this.txtDxTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtDxTime, "Entity Time Zone referenced to UTC");
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label61.ForeColor = System.Drawing.Color.Navy;
            this.label61.Location = new System.Drawing.Point(10, 77);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(58, 13);
            this.label61.TabIndex = 38;
            this.label61.Text = "Time Zone";
            this.label61.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label58.ForeColor = System.Drawing.Color.Navy;
            this.label58.Location = new System.Drawing.Point(305, 40);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(25, 13);
            this.label58.TabIndex = 36;
            this.label58.Text = "ITU";
            this.label58.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtDxITU
            // 
            this.txtDxITU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDxITU.ForeColor = System.Drawing.Color.Firebrick;
            this.txtDxITU.Location = new System.Drawing.Point(300, 19);
            this.txtDxITU.Name = "txtDxITU";
            this.txtDxITU.Size = new System.Drawing.Size(30, 20);
            this.txtDxITU.TabIndex = 35;
            this.txtDxITU.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtDxITU, "Entity ITU zone");
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label57.ForeColor = System.Drawing.Color.Navy;
            this.label57.Location = new System.Drawing.Point(270, 40);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(22, 13);
            this.label57.TabIndex = 34;
            this.label57.Text = "CQ";
            this.label57.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtDxCQ
            // 
            this.txtDxCQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDxCQ.ForeColor = System.Drawing.Color.Firebrick;
            this.txtDxCQ.Location = new System.Drawing.Point(265, 19);
            this.txtDxCQ.Name = "txtDxCQ";
            this.txtDxCQ.Size = new System.Drawing.Size(30, 20);
            this.txtDxCQ.TabIndex = 33;
            this.txtDxCQ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtDxCQ, "Entity CQ zone");
            // 
            // txtDxLat
            // 
            this.txtDxLat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDxLat.ForeColor = System.Drawing.Color.Firebrick;
            this.txtDxLat.Location = new System.Drawing.Point(10, 19);
            this.txtDxLat.Name = "txtDxLat";
            this.txtDxLat.Size = new System.Drawing.Size(70, 20);
            this.txtDxLat.TabIndex = 27;
            this.txtDxLat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtDxLat, "Entity latitude");
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label55.ForeColor = System.Drawing.Color.Navy;
            this.label55.Location = new System.Drawing.Point(20, 40);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(45, 13);
            this.label55.TabIndex = 29;
            this.label55.Text = "Latitude";
            this.label55.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label56.ForeColor = System.Drawing.Color.Navy;
            this.label56.Location = new System.Drawing.Point(100, 40);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(54, 13);
            this.label56.TabIndex = 30;
            this.label56.Text = "Longitude";
            this.label56.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtDxLong
            // 
            this.txtDxLong.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDxLong.ForeColor = System.Drawing.Color.Firebrick;
            this.txtDxLong.Location = new System.Drawing.Point(85, 19);
            this.txtDxLong.Name = "txtDxLong";
            this.txtDxLong.Size = new System.Drawing.Size(79, 20);
            this.txtDxLong.TabIndex = 28;
            this.txtDxLong.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtDxLong, "Entity longitude");
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label53.ForeColor = System.Drawing.Color.Navy;
            this.label53.Location = new System.Drawing.Point(230, 40);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(29, 13);
            this.label53.TabIndex = 6;
            this.label53.Text = "Cont";
            this.label53.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label54.ForeColor = System.Drawing.Color.Navy;
            this.label54.Location = new System.Drawing.Point(170, 40);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(49, 13);
            this.label54.TabIndex = 5;
            this.label54.Text = "Distance";
            this.label54.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtDxCont
            // 
            this.txtDxCont.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDxCont.ForeColor = System.Drawing.Color.Firebrick;
            this.txtDxCont.Location = new System.Drawing.Point(230, 19);
            this.txtDxCont.Name = "txtDxCont";
            this.txtDxCont.Size = new System.Drawing.Size(30, 20);
            this.txtDxCont.TabIndex = 4;
            this.txtDxCont.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtDxCont, "Entity Continent");
            // 
            // txtDxDist
            // 
            this.txtDxDist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDxDist.ForeColor = System.Drawing.Color.Firebrick;
            this.txtDxDist.Location = new System.Drawing.Point(170, 19);
            this.txtDxDist.Name = "txtDxDist";
            this.txtDxDist.Size = new System.Drawing.Size(55, 20);
            this.txtDxDist.TabIndex = 3;
            this.txtDxDist.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtDxDist, "Distance to Entity (red). Double-Click for long path (blue)");
            this.txtDxDist.DoubleClick += new System.EventHandler(this.txtDxDist_DoubleClick);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label60);
            this.groupBox9.Controls.Add(this.txtRegion);
            this.groupBox9.Controls.Add(this.cboEntity);
            this.groupBox9.Controls.Add(this.cboPrefix);
            this.groupBox9.Controls.Add(this.label45);
            this.groupBox9.Controls.Add(this.label47);
            this.groupBox9.Controls.Add(this.label48);
            this.groupBox9.Controls.Add(this.txtCode);
            this.groupBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox9.ForeColor = System.Drawing.Color.Navy;
            this.groupBox9.Location = new System.Drawing.Point(5, 5);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(310, 100);
            this.groupBox9.TabIndex = 11;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "DXCC Data";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label60.ForeColor = System.Drawing.Color.Navy;
            this.label60.Location = new System.Drawing.Point(5, 81);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(41, 13);
            this.label60.TabIndex = 14;
            this.label60.Text = "Region";
            // 
            // txtRegion
            // 
            this.txtRegion.ForeColor = System.Drawing.Color.Firebrick;
            this.txtRegion.Location = new System.Drawing.Point(10, 60);
            this.txtRegion.Name = "txtRegion";
            this.txtRegion.Size = new System.Drawing.Size(285, 20);
            this.txtRegion.TabIndex = 13;
            this.toolTip1.SetToolTip(this.txtRegion, "Country region");
            // 
            // cboEntity
            // 
            this.cboEntity.ForeColor = System.Drawing.Color.Firebrick;
            this.cboEntity.FormattingEnabled = true;
            this.cboEntity.Location = new System.Drawing.Point(85, 17);
            this.cboEntity.MaxDropDownItems = 15;
            this.cboEntity.Name = "cboEntity";
            this.cboEntity.Size = new System.Drawing.Size(175, 21);
            this.cboEntity.TabIndex = 12;
            this.toolTip1.SetToolTip(this.cboEntity, "DX station Entity");
            this.cboEntity.SelectedIndexChanged += new System.EventHandler(this.cboEntity_SelectedIndexChanged);
            this.cboEntity.Leave += new System.EventHandler(this.cboEntity_Leave);
            this.cboEntity.Enter += new System.EventHandler(this.cboEntity_Enter);
            this.cboEntity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboEntity_KeyPress);
            this.cboEntity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboEntity_KeyDown);
            this.cboEntity.Click += new System.EventHandler(this.cboEntity_Click);
            // 
            // cboPrefix
            // 
            this.cboPrefix.ForeColor = System.Drawing.Color.Firebrick;
            this.cboPrefix.FormattingEnabled = true;
            this.cboPrefix.Location = new System.Drawing.Point(10, 17);
            this.cboPrefix.MaxDropDownItems = 15;
            this.cboPrefix.Name = "cboPrefix";
            this.cboPrefix.Size = new System.Drawing.Size(70, 21);
            this.cboPrefix.TabIndex = 11;
            this.toolTip1.SetToolTip(this.cboPrefix, "DX station prefix");
            this.cboPrefix.SelectedIndexChanged += new System.EventHandler(this.cboPrefix_SelectedIndexChanged);
            this.cboPrefix.Leave += new System.EventHandler(this.cboPrefix_Leave);
            this.cboPrefix.Enter += new System.EventHandler(this.cboPrefix_Enter);
            this.cboPrefix.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboPrefix_KeyPress);
            this.cboPrefix.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboPrefix_KeyDown);
            this.cboPrefix.Click += new System.EventHandler(this.cboPrefix_Click);
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label45.ForeColor = System.Drawing.Color.Navy;
            this.label45.Location = new System.Drawing.Point(9, 40);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(33, 13);
            this.label45.TabIndex = 10;
            this.label45.Text = "Prefix";
            this.label45.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label47.ForeColor = System.Drawing.Color.Navy;
            this.label47.Location = new System.Drawing.Point(265, 39);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(32, 13);
            this.label47.TabIndex = 6;
            this.label47.Text = "Code";
            this.label47.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label48.ForeColor = System.Drawing.Color.Navy;
            this.label48.Location = new System.Drawing.Point(85, 40);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(33, 13);
            this.label48.TabIndex = 5;
            this.label48.Text = "Entity";
            this.label48.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.ForeColor = System.Drawing.Color.Firebrick;
            this.txtCode.Location = new System.Drawing.Point(265, 17);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(32, 20);
            this.txtCode.TabIndex = 4;
            this.txtCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtCode, "DXCC country code. To lookup enter number then hit Enter.");
            this.txtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.lblLP);
            this.groupBox8.Controls.Add(this.label59);
            this.groupBox8.Controls.Add(this.label43);
            this.groupBox8.Controls.Add(this.lblSP);
            this.groupBox8.Controls.Add(this.btnSP);
            this.groupBox8.Controls.Add(this.txtLP);
            this.groupBox8.Controls.Add(this.btnLP);
            this.groupBox8.Controls.Add(this.txtSP);
            this.groupBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox8.ForeColor = System.Drawing.Color.Navy;
            this.groupBox8.Location = new System.Drawing.Point(320, 5);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(90, 100);
            this.groupBox8.TabIndex = 5;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Heading";
            // 
            // lblLP
            // 
            this.lblLP.ForeColor = System.Drawing.Color.Green;
            this.lblLP.Location = new System.Drawing.Point(53, 80);
            this.lblLP.Name = "lblLP";
            this.lblLP.Size = new System.Drawing.Size(28, 13);
            this.lblLP.TabIndex = 39;
            this.lblLP.Tag = "";
            this.lblLP.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label59.ForeColor = System.Drawing.Color.Navy;
            this.label59.Location = new System.Drawing.Point(50, 16);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(31, 13);
            this.label59.TabIndex = 38;
            this.label59.Text = "Long";
            this.label59.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label43.ForeColor = System.Drawing.Color.Navy;
            this.label43.Location = new System.Drawing.Point(10, 16);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(32, 13);
            this.label43.TabIndex = 37;
            this.label43.Text = "Short";
            this.label43.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblSP
            // 
            this.lblSP.ForeColor = System.Drawing.Color.Green;
            this.lblSP.Location = new System.Drawing.Point(5, 80);
            this.lblSP.Name = "lblSP";
            this.lblSP.Size = new System.Drawing.Size(40, 13);
            this.lblSP.TabIndex = 5;
            this.lblSP.Tag = "";
            this.lblSP.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnSP
            // 
            this.btnSP.Enabled = false;
            this.btnSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSP.ForeColor = System.Drawing.Color.Blue;
            this.btnSP.Location = new System.Drawing.Point(8, 55);
            this.btnSP.Name = "btnSP";
            this.btnSP.Size = new System.Drawing.Size(36, 19);
            this.btnSP.TabIndex = 0;
            this.btnSP.Text = "SP";
            this.btnSP.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.btnSP, "Click to turn rotor to short path heading (Ctrl+Click to stop rotor)");
            this.btnSP.UseVisualStyleBackColor = true;
            this.btnSP.Click += new System.EventHandler(this.btnSP_Click);
            this.btnSP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSP_KeyDown);
            // 
            // txtLP
            // 
            this.txtLP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLP.ForeColor = System.Drawing.Color.Firebrick;
            this.txtLP.Location = new System.Drawing.Point(46, 30);
            this.txtLP.Name = "txtLP";
            this.txtLP.Size = new System.Drawing.Size(37, 20);
            this.txtLP.TabIndex = 4;
            this.txtLP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLP, "Long path bearing to prefix");
            // 
            // btnLP
            // 
            this.btnLP.Enabled = false;
            this.btnLP.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLP.ForeColor = System.Drawing.Color.Blue;
            this.btnLP.Location = new System.Drawing.Point(50, 55);
            this.btnLP.Name = "btnLP";
            this.btnLP.Size = new System.Drawing.Size(36, 19);
            this.btnLP.TabIndex = 1;
            this.btnLP.Text = "LP";
            this.btnLP.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.btnLP, "Click to turn rotor to long path heading (Ctrl+Click to stop rotor)");
            this.btnLP.UseVisualStyleBackColor = true;
            this.btnLP.Click += new System.EventHandler(this.btnLP_Click);
            this.btnLP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnLP_KeyDown);
            // 
            // txtSP
            // 
            this.txtSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSP.ForeColor = System.Drawing.Color.Firebrick;
            this.txtSP.Location = new System.Drawing.Point(7, 30);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(37, 20);
            this.txtSP.TabIndex = 3;
            this.txtSP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtSP, "Short path bearing to prefix");
            this.txtSP.TextChanged += new System.EventHandler(this.txtSP_TextChanged);
            // 
            // tabSetup
            // 
            this.tabSetup.Controls.Add(this.chkOnTop);
            this.tabSetup.Controls.Add(this.chkModeChg);
            this.tabSetup.Controls.Add(this.chkAudio);
            this.tabSetup.Controls.Add(this.chkTips);
            this.tabSetup.Controls.Add(this.grpALC);
            this.tabSetup.Controls.Add(this.grpDog);
            this.tabSetup.Controls.Add(this.groupBox10);
            this.tabSetup.Controls.Add(this.groupBox14);
            this.tabSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.tabSetup.Location = new System.Drawing.Point(4, 22);
            this.tabSetup.Name = "tabSetup";
            this.tabSetup.Size = new System.Drawing.Size(417, 309);
            this.tabSetup.TabIndex = 6;
            this.tabSetup.Text = "Setup";
            this.tabSetup.UseVisualStyleBackColor = true;
            // 
            // chkOnTop
            // 
            this.chkOnTop.AutoSize = true;
            this.chkOnTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkOnTop.ForeColor = System.Drawing.Color.Navy;
            this.chkOnTop.Location = new System.Drawing.Point(85, 285);
            this.chkOnTop.Name = "chkOnTop";
            this.chkOnTop.Size = new System.Drawing.Size(62, 17);
            this.chkOnTop.TabIndex = 88;
            this.chkOnTop.Text = "On Top";
            this.toolTip1.SetToolTip(this.chkOnTop, "When selected causes this window to be on top of all other windows.");
            this.chkOnTop.UseVisualStyleBackColor = true;
            this.chkOnTop.CheckedChanged += new System.EventHandler(this.chkOnTop_CheckedChanged);
            // 
            // chkModeChg
            // 
            this.chkModeChg.AutoSize = true;
            this.chkModeChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkModeChg.ForeColor = System.Drawing.Color.Navy;
            this.chkModeChg.Location = new System.Drawing.Point(10, 285);
            this.chkModeChg.Name = "chkModeChg";
            this.chkModeChg.Size = new System.Drawing.Size(75, 17);
            this.chkModeChg.TabIndex = 79;
            this.chkModeChg.Text = "Mode Chg";
            this.toolTip1.SetToolTip(this.chkModeChg, "Select to enable Macro execution when Mode changes.");
            this.chkModeChg.UseVisualStyleBackColor = true;
            this.chkModeChg.CheckedChanged += new System.EventHandler(this.chkModeChg_CheckedChanged);
            // 
            // chkAudio
            // 
            this.chkAudio.AutoSize = true;
            this.chkAudio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAudio.ForeColor = System.Drawing.Color.Navy;
            this.chkAudio.Location = new System.Drawing.Point(85, 265);
            this.chkAudio.Name = "chkAudio";
            this.chkAudio.Size = new System.Drawing.Size(70, 17);
            this.chkAudio.TabIndex = 34;
            this.chkAudio.Text = "Audio On";
            this.toolTip1.SetToolTip(this.chkAudio, "Select to enable sounds associated with warning messages.");
            this.chkAudio.UseVisualStyleBackColor = true;
            this.chkAudio.CheckedChanged += new System.EventHandler(this.chkAudio_CheckedChanged);
            // 
            // chkTips
            // 
            this.chkTips.AutoSize = true;
            this.chkTips.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTips.ForeColor = System.Drawing.Color.Navy;
            this.chkTips.Location = new System.Drawing.Point(10, 265);
            this.chkTips.Name = "chkTips";
            this.chkTips.Size = new System.Drawing.Size(70, 17);
            this.chkTips.TabIndex = 33;
            this.chkTips.Text = "Tool Tips";
            this.toolTip1.SetToolTip(this.chkTips, "Select to enable Tool Tips when cursor hovers over a control.");
            this.chkTips.UseVisualStyleBackColor = true;
            this.chkTips.CheckedChanged += new System.EventHandler(this.chkTips_CheckedChanged);
            // 
            // grpALC
            // 
            this.grpALC.Controls.Add(this.chk6);
            this.grpALC.Controls.Add(this.btnChkAll);
            this.grpALC.Controls.Add(this.btnClrAll);
            this.grpALC.Controls.Add(this.chk10);
            this.grpALC.Controls.Add(this.chk30);
            this.grpALC.Controls.Add(this.chk12);
            this.grpALC.Controls.Add(this.chk40);
            this.grpALC.Controls.Add(this.chk15);
            this.grpALC.Controls.Add(this.chk17);
            this.grpALC.Controls.Add(this.chk80);
            this.grpALC.Controls.Add(this.chk20);
            this.grpALC.Controls.Add(this.chk160);
            this.grpALC.Controls.Add(this.label68);
            this.grpALC.Controls.Add(this.txtALC);
            this.grpALC.Controls.Add(this.btnSet);
            this.grpALC.Controls.Add(this.btnCal);
            this.grpALC.Controls.Add(this.txtDrive);
            this.grpALC.Controls.Add(this.label64);
            this.grpALC.Controls.Add(this.chkAlcEnab);
            this.grpALC.Enabled = false;
            this.grpALC.ForeColor = System.Drawing.Color.Blue;
            this.grpALC.Location = new System.Drawing.Point(155, 205);
            this.grpALC.Name = "grpALC";
            this.grpALC.Size = new System.Drawing.Size(250, 100);
            this.grpALC.TabIndex = 78;
            this.grpALC.TabStop = false;
            this.grpALC.Text = "ALC";
            // 
            // chk6
            // 
            this.chk6.AutoSize = true;
            this.chk6.BackColor = System.Drawing.SystemColors.Control;
            this.chk6.Checked = true;
            this.chk6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk6.ForeColor = System.Drawing.Color.Navy;
            this.chk6.Location = new System.Drawing.Point(186, 60);
            this.chk6.Name = "chk6";
            this.chk6.Size = new System.Drawing.Size(32, 17);
            this.chk6.TabIndex = 93;
            this.chk6.Text = "6";
            this.toolTip1.SetToolTip(this.chk6, "Select to enable calibration on this band.");
            this.chk6.UseVisualStyleBackColor = false;
            // 
            // btnChkAll
            // 
            this.btnChkAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnChkAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnChkAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnChkAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChkAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnChkAll.Location = new System.Drawing.Point(55, 79);
            this.btnChkAll.Name = "btnChkAll";
            this.btnChkAll.Size = new System.Drawing.Size(50, 15);
            this.btnChkAll.TabIndex = 92;
            this.btnChkAll.Text = "Check All";
            this.toolTip1.SetToolTip(this.btnChkAll, "Checks all bands for calibration.");
            this.btnChkAll.UseVisualStyleBackColor = false;
            this.btnChkAll.Click += new System.EventHandler(this.btnChkAll_Click);
            // 
            // btnClrAll
            // 
            this.btnClrAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClrAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnClrAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClrAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClrAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClrAll.Location = new System.Drawing.Point(145, 79);
            this.btnClrAll.Name = "btnClrAll";
            this.btnClrAll.Size = new System.Drawing.Size(50, 15);
            this.btnClrAll.TabIndex = 91;
            this.btnClrAll.Text = "Clear All";
            this.toolTip1.SetToolTip(this.btnClrAll, "Un-Checks all bands for calibration.");
            this.btnClrAll.UseVisualStyleBackColor = false;
            this.btnClrAll.Click += new System.EventHandler(this.btnClrAll_Click);
            // 
            // chk10
            // 
            this.chk10.AutoSize = true;
            this.chk10.BackColor = System.Drawing.SystemColors.Control;
            this.chk10.Checked = true;
            this.chk10.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk10.ForeColor = System.Drawing.Color.Navy;
            this.chk10.Location = new System.Drawing.Point(146, 60);
            this.chk10.Name = "chk10";
            this.chk10.Size = new System.Drawing.Size(38, 17);
            this.chk10.TabIndex = 90;
            this.chk10.Text = "10";
            this.toolTip1.SetToolTip(this.chk10, "Select to enable calibration on this band.");
            this.chk10.UseVisualStyleBackColor = false;
            // 
            // chk30
            // 
            this.chk30.AutoSize = true;
            this.chk30.BackColor = System.Drawing.SystemColors.Control;
            this.chk30.Checked = true;
            this.chk30.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk30.ForeColor = System.Drawing.Color.Navy;
            this.chk30.Location = new System.Drawing.Point(146, 45);
            this.chk30.Name = "chk30";
            this.chk30.Size = new System.Drawing.Size(38, 17);
            this.chk30.TabIndex = 89;
            this.chk30.Text = "30";
            this.toolTip1.SetToolTip(this.chk30, "Select to enable calibration on this band.");
            this.chk30.UseVisualStyleBackColor = false;
            // 
            // chk12
            // 
            this.chk12.AutoSize = true;
            this.chk12.BackColor = System.Drawing.SystemColors.Control;
            this.chk12.Checked = true;
            this.chk12.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk12.ForeColor = System.Drawing.Color.Navy;
            this.chk12.Location = new System.Drawing.Point(101, 60);
            this.chk12.Name = "chk12";
            this.chk12.Size = new System.Drawing.Size(38, 17);
            this.chk12.TabIndex = 88;
            this.chk12.Text = "12";
            this.toolTip1.SetToolTip(this.chk12, "Select to enable calibration on this band.");
            this.chk12.UseVisualStyleBackColor = false;
            // 
            // chk40
            // 
            this.chk40.AutoSize = true;
            this.chk40.BackColor = System.Drawing.SystemColors.Control;
            this.chk40.Checked = true;
            this.chk40.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk40.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk40.ForeColor = System.Drawing.Color.Navy;
            this.chk40.Location = new System.Drawing.Point(101, 45);
            this.chk40.Name = "chk40";
            this.chk40.Size = new System.Drawing.Size(38, 17);
            this.chk40.TabIndex = 87;
            this.chk40.Text = "40";
            this.toolTip1.SetToolTip(this.chk40, "Select to enable calibration on this band.");
            this.chk40.UseVisualStyleBackColor = false;
            // 
            // chk15
            // 
            this.chk15.AutoSize = true;
            this.chk15.BackColor = System.Drawing.SystemColors.Control;
            this.chk15.Checked = true;
            this.chk15.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk15.ForeColor = System.Drawing.Color.Navy;
            this.chk15.Location = new System.Drawing.Point(56, 60);
            this.chk15.Name = "chk15";
            this.chk15.Size = new System.Drawing.Size(38, 17);
            this.chk15.TabIndex = 86;
            this.chk15.Text = "15";
            this.toolTip1.SetToolTip(this.chk15, "Select to enable calibration on this band.");
            this.chk15.UseVisualStyleBackColor = false;
            // 
            // chk17
            // 
            this.chk17.AutoSize = true;
            this.chk17.BackColor = System.Drawing.SystemColors.Control;
            this.chk17.Checked = true;
            this.chk17.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk17.ForeColor = System.Drawing.Color.Navy;
            this.chk17.Location = new System.Drawing.Point(11, 60);
            this.chk17.Name = "chk17";
            this.chk17.Size = new System.Drawing.Size(38, 17);
            this.chk17.TabIndex = 84;
            this.chk17.Text = "17";
            this.toolTip1.SetToolTip(this.chk17, "Select to enable calibration on this band.");
            this.chk17.UseVisualStyleBackColor = false;
            // 
            // chk80
            // 
            this.chk80.AutoSize = true;
            this.chk80.BackColor = System.Drawing.SystemColors.Control;
            this.chk80.Checked = true;
            this.chk80.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk80.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk80.ForeColor = System.Drawing.Color.Navy;
            this.chk80.Location = new System.Drawing.Point(56, 45);
            this.chk80.Name = "chk80";
            this.chk80.Size = new System.Drawing.Size(38, 17);
            this.chk80.TabIndex = 83;
            this.chk80.Text = "80";
            this.toolTip1.SetToolTip(this.chk80, "Select to enable calibration on this band.");
            this.chk80.UseVisualStyleBackColor = false;
            // 
            // chk20
            // 
            this.chk20.AutoSize = true;
            this.chk20.BackColor = System.Drawing.SystemColors.Control;
            this.chk20.Checked = true;
            this.chk20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk20.ForeColor = System.Drawing.Color.Navy;
            this.chk20.Location = new System.Drawing.Point(186, 45);
            this.chk20.Name = "chk20";
            this.chk20.Size = new System.Drawing.Size(38, 17);
            this.chk20.TabIndex = 82;
            this.chk20.Text = "20";
            this.toolTip1.SetToolTip(this.chk20, "Select to enable calibration on this band.");
            this.chk20.UseVisualStyleBackColor = false;
            // 
            // chk160
            // 
            this.chk160.AutoSize = true;
            this.chk160.BackColor = System.Drawing.SystemColors.Control;
            this.chk160.Checked = true;
            this.chk160.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk160.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk160.ForeColor = System.Drawing.Color.Navy;
            this.chk160.Location = new System.Drawing.Point(11, 45);
            this.chk160.Name = "chk160";
            this.chk160.Size = new System.Drawing.Size(44, 17);
            this.chk160.TabIndex = 81;
            this.chk160.Text = "160";
            this.toolTip1.SetToolTip(this.chk160, "Select to enable calibration on this band.");
            this.chk160.UseVisualStyleBackColor = false;
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label68.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label68.Location = new System.Drawing.Point(122, 30);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(27, 12);
            this.label68.TabIndex = 80;
            this.label68.Text = "ALC";
            this.toolTip1.SetToolTip(this.label68, "ALC voltage from amp.");
            // 
            // txtALC
            // 
            this.txtALC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtALC.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtALC.Location = new System.Drawing.Point(117, 10);
            this.txtALC.Name = "txtALC";
            this.txtALC.Size = new System.Drawing.Size(33, 20);
            this.txtALC.TabIndex = 79;
            this.txtALC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtALC, "Amplifier ALC volts: Used for calibration only.");
            // 
            // btnSet
            // 
            this.btnSet.AutoSize = true;
            this.btnSet.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet.ForeColor = System.Drawing.Color.Maroon;
            this.btnSet.Location = new System.Drawing.Point(205, 10);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(32, 22);
            this.btnSet.TabIndex = 78;
            this.btnSet.Text = "Set";
            this.toolTip1.SetToolTip(this.btnSet, "Press to save calibration values for this band.");
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnCal
            // 
            this.btnCal.AutoSize = true;
            this.btnCal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCal.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCal.ForeColor = System.Drawing.Color.Maroon;
            this.btnCal.Location = new System.Drawing.Point(160, 10);
            this.btnCal.Name = "btnCal";
            this.btnCal.Size = new System.Drawing.Size(32, 22);
            this.btnCal.TabIndex = 77;
            this.btnCal.Text = "Cal";
            this.toolTip1.SetToolTip(this.btnCal, "Press to start calibration routine: See documentation for procedure.");
            this.btnCal.UseVisualStyleBackColor = true;
            this.btnCal.Click += new System.EventHandler(this.btnCal_Click);
            // 
            // txtDrive
            // 
            this.txtDrive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDrive.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDrive.Location = new System.Drawing.Point(75, 10);
            this.txtDrive.Name = "txtDrive";
            this.txtDrive.Size = new System.Drawing.Size(33, 20);
            this.txtDrive.TabIndex = 76;
            this.txtDrive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtDrive, "PowerSDR drive level.");
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label64.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label64.Location = new System.Drawing.Point(78, 30);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(33, 12);
            this.label64.TabIndex = 31;
            this.label64.Text = "Drive";
            // 
            // chkAlcEnab
            // 
            this.chkAlcEnab.AutoSize = true;
            this.chkAlcEnab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAlcEnab.ForeColor = System.Drawing.Color.Navy;
            this.chkAlcEnab.Location = new System.Drawing.Point(10, 20);
            this.chkAlcEnab.Name = "chkAlcEnab";
            this.chkAlcEnab.Size = new System.Drawing.Size(59, 17);
            this.chkAlcEnab.TabIndex = 35;
            this.chkAlcEnab.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkAlcEnab, "Select to enable ALC monitoring");
            this.chkAlcEnab.UseVisualStyleBackColor = true;
            this.chkAlcEnab.CheckedChanged += new System.EventHandler(this.chkAlcEnab_CheckedChanged);
            // 
            // grpDog
            // 
            this.grpDog.Controls.Add(this.txtTimer);
            this.grpDog.Controls.Add(this.label67);
            this.grpDog.Controls.Add(this.chkDog);
            this.grpDog.ForeColor = System.Drawing.Color.Blue;
            this.grpDog.Location = new System.Drawing.Point(5, 205);
            this.grpDog.Name = "grpDog";
            this.grpDog.Size = new System.Drawing.Size(140, 55);
            this.grpDog.TabIndex = 77;
            this.grpDog.TabStop = false;
            this.grpDog.Text = "WatchDog Timer";
            // 
            // txtTimer
            // 
            this.txtTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimer.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtTimer.Location = new System.Drawing.Point(72, 23);
            this.txtTimer.Name = "txtTimer";
            this.txtTimer.Size = new System.Drawing.Size(20, 20);
            this.txtTimer.TabIndex = 76;
            this.txtTimer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTimer, "WatchDog timer minutes.");
            this.txtTimer.TextChanged += new System.EventHandler(this.txtTimer_TextChanged);
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label67.ForeColor = System.Drawing.Color.Navy;
            this.label67.Location = new System.Drawing.Point(95, 27);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(37, 13);
            this.label67.TabIndex = 31;
            this.label67.Text = "Period";
            // 
            // chkDog
            // 
            this.chkDog.AutoSize = true;
            this.chkDog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDog.ForeColor = System.Drawing.Color.Navy;
            this.chkDog.Location = new System.Drawing.Point(10, 25);
            this.chkDog.Name = "chkDog";
            this.chkDog.Size = new System.Drawing.Size(59, 17);
            this.chkDog.TabIndex = 35;
            this.chkDog.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkDog, "Select to enable a Watch Dog timer for transmit time out.");
            this.chkDog.UseVisualStyleBackColor = true;
            this.chkDog.CheckedChanged += new System.EventHandler(this.chkDog_CheckedChanged);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.label50);
            this.groupBox10.Controls.Add(this.label49);
            this.groupBox10.Controls.Add(this.cboRotorCom);
            this.groupBox10.Controls.Add(this.cboRotorPort);
            this.groupBox10.Controls.Add(this.grpSpeed);
            this.groupBox10.Controls.Add(this.chkRotorEnab);
            this.groupBox10.Controls.Add(this.grpModel);
            this.groupBox10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox10.ForeColor = System.Drawing.Color.Blue;
            this.groupBox10.Location = new System.Drawing.Point(5, 10);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(295, 190);
            this.groupBox10.TabIndex = 29;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Rotor Setup";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.ForeColor = System.Drawing.Color.Navy;
            this.label50.Location = new System.Drawing.Point(10, 115);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(62, 13);
            this.label50.TabIndex = 32;
            this.label50.Text = "Comm Data";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label49.ForeColor = System.Drawing.Color.Navy;
            this.label49.Location = new System.Drawing.Point(10, 72);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(55, 13);
            this.label49.TabIndex = 31;
            this.label49.Text = "Serial Port";
            // 
            // cboRotorCom
            // 
            this.cboRotorCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRotorCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRotorCom.ForeColor = System.Drawing.Color.Firebrick;
            this.cboRotorCom.FormattingEnabled = true;
            this.cboRotorCom.Items.AddRange(new object[] {
            "9600 8-N-1",
            "9600 8-N-2",
            "4800 8-N-1",
            "4800 8-N-2",
            "2400 8-N-1",
            "2400 8-N-2",
            "1200 8-N-1",
            "1200 8-N-2",
            "600 8-N-1"});
            this.cboRotorCom.Location = new System.Drawing.Point(10, 90);
            this.cboRotorCom.Name = "cboRotorCom";
            this.cboRotorCom.Size = new System.Drawing.Size(93, 21);
            this.cboRotorCom.TabIndex = 30;
            this.toolTip1.SetToolTip(this.cboRotorCom, "Rotor port serial com parameters");
            this.cboRotorCom.SelectedIndexChanged += new System.EventHandler(this.cboRotorCom_SelectedIndexChanged);
            // 
            // cboRotorPort
            // 
            this.cboRotorPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRotorPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRotorPort.ForeColor = System.Drawing.Color.Firebrick;
            this.cboRotorPort.FormattingEnabled = true;
            this.cboRotorPort.Location = new System.Drawing.Point(10, 50);
            this.cboRotorPort.Name = "cboRotorPort";
            this.cboRotorPort.Size = new System.Drawing.Size(71, 21);
            this.cboRotorPort.TabIndex = 29;
            this.toolTip1.SetToolTip(this.cboRotorPort, "Rotor serial port (hdwe)");
            this.cboRotorPort.SelectedIndexChanged += new System.EventHandler(this.cboRotorPort_SelectedIndexChanged);
            // 
            // grpSpeed
            // 
            this.grpSpeed.Controls.Add(this.rbRtrSpd3);
            this.grpSpeed.Controls.Add(this.rbRtrSpd2);
            this.grpSpeed.Controls.Add(this.rbRtrSpd1);
            this.grpSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpSpeed.ForeColor = System.Drawing.Color.Blue;
            this.grpSpeed.Location = new System.Drawing.Point(10, 135);
            this.grpSpeed.Name = "grpSpeed";
            this.grpSpeed.Size = new System.Drawing.Size(155, 45);
            this.grpSpeed.TabIndex = 14;
            this.grpSpeed.TabStop = false;
            this.grpSpeed.Text = "Speed";
            // 
            // rbRtrSpd3
            // 
            this.rbRtrSpd3.AutoSize = true;
            this.rbRtrSpd3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbRtrSpd3.ForeColor = System.Drawing.Color.Navy;
            this.rbRtrSpd3.Location = new System.Drawing.Point(105, 20);
            this.rbRtrSpd3.Name = "rbRtrSpd3";
            this.rbRtrSpd3.Size = new System.Drawing.Size(45, 17);
            this.rbRtrSpd3.TabIndex = 2;
            this.rbRtrSpd3.TabStop = true;
            this.rbRtrSpd3.Text = "Fast";
            this.toolTip1.SetToolTip(this.rbRtrSpd3, "Select rotor speed (if applicable)");
            this.rbRtrSpd3.UseVisualStyleBackColor = true;
            this.rbRtrSpd3.CheckedChanged += new System.EventHandler(this.grpSpeed_CheckedChanged);
            // 
            // rbRtrSpd2
            // 
            this.rbRtrSpd2.AutoSize = true;
            this.rbRtrSpd2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbRtrSpd2.ForeColor = System.Drawing.Color.Navy;
            this.rbRtrSpd2.Location = new System.Drawing.Point(55, 20);
            this.rbRtrSpd2.Name = "rbRtrSpd2";
            this.rbRtrSpd2.Size = new System.Drawing.Size(46, 17);
            this.rbRtrSpd2.TabIndex = 1;
            this.rbRtrSpd2.TabStop = true;
            this.rbRtrSpd2.Text = "Med";
            this.toolTip1.SetToolTip(this.rbRtrSpd2, "Select rotor speed (if applicable)");
            this.rbRtrSpd2.UseVisualStyleBackColor = true;
            this.rbRtrSpd2.CheckedChanged += new System.EventHandler(this.grpSpeed_CheckedChanged);
            // 
            // rbRtrSpd1
            // 
            this.rbRtrSpd1.AutoSize = true;
            this.rbRtrSpd1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbRtrSpd1.ForeColor = System.Drawing.Color.Navy;
            this.rbRtrSpd1.Location = new System.Drawing.Point(5, 20);
            this.rbRtrSpd1.Name = "rbRtrSpd1";
            this.rbRtrSpd1.Size = new System.Drawing.Size(48, 17);
            this.rbRtrSpd1.TabIndex = 0;
            this.rbRtrSpd1.TabStop = true;
            this.rbRtrSpd1.Text = "Slow";
            this.toolTip1.SetToolTip(this.rbRtrSpd1, "Select rotor speed (if applicable)");
            this.rbRtrSpd1.UseVisualStyleBackColor = true;
            this.rbRtrSpd1.CheckedChanged += new System.EventHandler(this.grpSpeed_CheckedChanged);
            // 
            // chkRotorEnab
            // 
            this.chkRotorEnab.AutoSize = true;
            this.chkRotorEnab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRotorEnab.ForeColor = System.Drawing.Color.Navy;
            this.chkRotorEnab.Location = new System.Drawing.Point(10, 24);
            this.chkRotorEnab.Name = "chkRotorEnab";
            this.chkRotorEnab.Size = new System.Drawing.Size(59, 17);
            this.chkRotorEnab.TabIndex = 28;
            this.chkRotorEnab.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkRotorEnab, "Select to enable rotor operation");
            this.chkRotorEnab.UseVisualStyleBackColor = true;
            this.chkRotorEnab.CheckedChanged += new System.EventHandler(this.chkRotorEnab_CheckedChanged);
            // 
            // grpModel
            // 
            this.grpModel.Controls.Add(this.rbRtrMod7);
            this.grpModel.Controls.Add(this.rbRtrMod6);
            this.grpModel.Controls.Add(this.rbRtrMod5);
            this.grpModel.Controls.Add(this.rbRtrMod4);
            this.grpModel.Controls.Add(this.rbRtrMod3);
            this.grpModel.Controls.Add(this.rbRtrMod2);
            this.grpModel.Controls.Add(this.rbRtrMod1);
            this.grpModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpModel.ForeColor = System.Drawing.Color.Blue;
            this.grpModel.Location = new System.Drawing.Point(175, 10);
            this.grpModel.Name = "grpModel";
            this.grpModel.Size = new System.Drawing.Size(110, 170);
            this.grpModel.TabIndex = 13;
            this.grpModel.TabStop = false;
            this.grpModel.Text = "Model";
            // 
            // rbRtrMod7
            // 
            this.rbRtrMod7.AutoSize = true;
            this.rbRtrMod7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbRtrMod7.ForeColor = System.Drawing.Color.Navy;
            this.rbRtrMod7.Location = new System.Drawing.Point(10, 140);
            this.rbRtrMod7.Name = "rbRtrMod7";
            this.rbRtrMod7.Size = new System.Drawing.Size(55, 17);
            this.rbRtrMod7.TabIndex = 6;
            this.rbRtrMod7.TabStop = true;
            this.rbRtrMod7.Text = "Yaesu";
            this.toolTip1.SetToolTip(this.rbRtrMod7, "Select to enable Yaesu rotors");
            this.rbRtrMod7.UseVisualStyleBackColor = true;
            this.rbRtrMod7.CheckedChanged += new System.EventHandler(this.grpModel_CheckedChanged);
            // 
            // rbRtrMod6
            // 
            this.rbRtrMod6.AutoSize = true;
            this.rbRtrMod6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbRtrMod6.ForeColor = System.Drawing.Color.Navy;
            this.rbRtrMod6.Location = new System.Drawing.Point(10, 120);
            this.rbRtrMod6.Name = "rbRtrMod6";
            this.rbRtrMod6.Size = new System.Drawing.Size(64, 17);
            this.rbRtrMod6.TabIndex = 5;
            this.rbRtrMod6.TabStop = true;
            this.rbRtrMod6.Text = "Prosistel";
            this.toolTip1.SetToolTip(this.rbRtrMod6, "Select to enable Porsistel rotors");
            this.rbRtrMod6.UseVisualStyleBackColor = true;
            this.rbRtrMod6.CheckedChanged += new System.EventHandler(this.grpModel_CheckedChanged);
            // 
            // rbRtrMod5
            // 
            this.rbRtrMod5.AutoSize = true;
            this.rbRtrMod5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbRtrMod5.ForeColor = System.Drawing.Color.Navy;
            this.rbRtrMod5.Location = new System.Drawing.Point(10, 100);
            this.rbRtrMod5.Name = "rbRtrMod5";
            this.rbRtrMod5.Size = new System.Drawing.Size(96, 17);
            this.rbRtrMod5.TabIndex = 4;
            this.rbRtrMod5.TabStop = true;
            this.rbRtrMod5.Text = "M2 RC2800PX";
            this.toolTip1.SetToolTip(this.rbRtrMod5, "Select to enable this Orion rotor");
            this.rbRtrMod5.UseVisualStyleBackColor = true;
            this.rbRtrMod5.CheckedChanged += new System.EventHandler(this.grpModel_CheckedChanged);
            // 
            // rbRtrMod4
            // 
            this.rbRtrMod4.AutoSize = true;
            this.rbRtrMod4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbRtrMod4.ForeColor = System.Drawing.Color.Navy;
            this.rbRtrMod4.Location = new System.Drawing.Point(10, 80);
            this.rbRtrMod4.Name = "rbRtrMod4";
            this.rbRtrMod4.Size = new System.Drawing.Size(96, 17);
            this.rbRtrMod4.TabIndex = 3;
            this.rbRtrMod4.TabStop = true;
            this.rbRtrMod4.Text = "M2 RC2800PA";
            this.toolTip1.SetToolTip(this.rbRtrMod4, "Select to enable this Orion rotor");
            this.rbRtrMod4.UseVisualStyleBackColor = true;
            this.rbRtrMod4.CheckedChanged += new System.EventHandler(this.grpModel_CheckedChanged);
            // 
            // rbRtrMod3
            // 
            this.rbRtrMod3.AutoSize = true;
            this.rbRtrMod3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbRtrMod3.ForeColor = System.Drawing.Color.Navy;
            this.rbRtrMod3.Location = new System.Drawing.Point(10, 60);
            this.rbRtrMod3.Name = "rbRtrMod3";
            this.rbRtrMod3.Size = new System.Drawing.Size(85, 17);
            this.rbRtrMod3.TabIndex = 2;
            this.rbRtrMod3.TabStop = true;
            this.rbRtrMod3.Text = "HyGain / EZ";
            this.toolTip1.SetToolTip(this.rbRtrMod3, "Select to enable HyGain DCU-1 or EZ rotor controllers.");
            this.rbRtrMod3.UseVisualStyleBackColor = true;
            this.rbRtrMod3.CheckedChanged += new System.EventHandler(this.grpModel_CheckedChanged);
            // 
            // rbRtrMod2
            // 
            this.rbRtrMod2.AutoSize = true;
            this.rbRtrMod2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbRtrMod2.ForeColor = System.Drawing.Color.Navy;
            this.rbRtrMod2.Location = new System.Drawing.Point(10, 40);
            this.rbRtrMod2.Name = "rbRtrMod2";
            this.rbRtrMod2.Size = new System.Drawing.Size(86, 17);
            this.rbRtrMod2.TabIndex = 1;
            this.rbRtrMod2.TabStop = true;
            this.rbRtrMod2.Text = "Green Heron";
            this.toolTip1.SetToolTip(this.rbRtrMod2, "Select to enable Green Heron rotor controller");
            this.rbRtrMod2.UseVisualStyleBackColor = true;
            this.rbRtrMod2.CheckedChanged += new System.EventHandler(this.grpModel_CheckedChanged);
            // 
            // rbRtrMod1
            // 
            this.rbRtrMod1.AutoSize = true;
            this.rbRtrMod1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbRtrMod1.ForeColor = System.Drawing.Color.Navy;
            this.rbRtrMod1.Location = new System.Drawing.Point(10, 20);
            this.rbRtrMod1.Name = "rbRtrMod1";
            this.rbRtrMod1.Size = new System.Drawing.Size(67, 17);
            this.rbRtrMod1.TabIndex = 0;
            this.rbRtrMod1.TabStop = true;
            this.rbRtrMod1.Text = "Alfa Spid";
            this.toolTip1.SetToolTip(this.rbRtrMod1, "Select to enable Alpha Spid rotors");
            this.rbRtrMod1.UseVisualStyleBackColor = true;
            this.rbRtrMod1.CheckedChanged += new System.EventHandler(this.grpModel_CheckedChanged);
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.txtCall);
            this.groupBox14.Controls.Add(this.label25);
            this.groupBox14.Controls.Add(this.txtGrid);
            this.groupBox14.Controls.Add(this.txtLat);
            this.groupBox14.Controls.Add(this.label44);
            this.groupBox14.Controls.Add(this.label51);
            this.groupBox14.Controls.Add(this.label52);
            this.groupBox14.Controls.Add(this.txtLong);
            this.groupBox14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox14.ForeColor = System.Drawing.Color.Blue;
            this.groupBox14.Location = new System.Drawing.Point(315, 10);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(91, 190);
            this.groupBox14.TabIndex = 27;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Station Info";
            // 
            // txtCall
            // 
            this.txtCall.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCall.ForeColor = System.Drawing.Color.Firebrick;
            this.txtCall.Location = new System.Drawing.Point(10, 25);
            this.txtCall.Name = "txtCall";
            this.txtCall.Size = new System.Drawing.Size(70, 20);
            this.txtCall.TabIndex = 29;
            this.txtCall.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtCall, "Home Station Call Sign");
            this.txtCall.TextChanged += new System.EventHandler(this.txtCall_TextChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label25.ForeColor = System.Drawing.Color.Navy;
            this.label25.Location = new System.Drawing.Point(22, 47);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(48, 13);
            this.label25.TabIndex = 30;
            this.label25.Text = "Call Sign";
            // 
            // txtGrid
            // 
            this.txtGrid.ForeColor = System.Drawing.Color.Firebrick;
            this.txtGrid.Location = new System.Drawing.Point(12, 150);
            this.txtGrid.Name = "txtGrid";
            this.txtGrid.Size = new System.Drawing.Size(63, 20);
            this.txtGrid.TabIndex = 27;
            this.txtGrid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtGrid, "Home station grid square");
            this.txtGrid.TextChanged += new System.EventHandler(this.txtGrid_TextChanged);
            // 
            // txtLat
            // 
            this.txtLat.ForeColor = System.Drawing.Color.Firebrick;
            this.txtLat.Location = new System.Drawing.Point(10, 65);
            this.txtLat.Name = "txtLat";
            this.txtLat.Size = new System.Drawing.Size(67, 20);
            this.txtLat.TabIndex = 23;
            this.txtLat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLat, "Home station latitude");
            this.txtLat.TextChanged += new System.EventHandler(this.txtLat_TextChanged);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label44.ForeColor = System.Drawing.Color.Navy;
            this.label44.Location = new System.Drawing.Point(30, 172);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(26, 13);
            this.label44.TabIndex = 28;
            this.label44.Text = "Grid";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label51.ForeColor = System.Drawing.Color.Navy;
            this.label51.Location = new System.Drawing.Point(22, 87);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(45, 13);
            this.label51.TabIndex = 25;
            this.label51.Text = "Latitude";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label52.ForeColor = System.Drawing.Color.Navy;
            this.label52.Location = new System.Drawing.Point(17, 132);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(54, 13);
            this.label52.TabIndex = 26;
            this.label52.Text = "Longitude";
            // 
            // txtLong
            // 
            this.txtLong.ForeColor = System.Drawing.Color.Firebrick;
            this.txtLong.Location = new System.Drawing.Point(10, 110);
            this.txtLong.Name = "txtLong";
            this.txtLong.Size = new System.Drawing.Size(67, 20);
            this.txtLong.TabIndex = 24;
            this.txtLong.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLong, "Home station longitude");
            this.txtLong.TextChanged += new System.EventHandler(this.txtLong_TextChanged);
            // 
            // tabExtCtrl
            // 
            this.tabExtCtrl.Controls.Add(this.label137);
            this.tabExtCtrl.Controls.Add(this.lblVHF);
            this.tabExtCtrl.Controls.Add(this.label132);
            this.tabExtCtrl.Controls.Add(this.label131);
            this.tabExtCtrl.Controls.Add(this.chkPortB);
            this.tabExtCtrl.Controls.Add(this.btnClrPortB);
            this.tabExtCtrl.Controls.Add(this.btnClrPortA);
            this.tabExtCtrl.Controls.Add(this.grpPortB);
            this.tabExtCtrl.Controls.Add(this.grpPortA);
            this.tabExtCtrl.Controls.Add(this.chkPortA);
            this.tabExtCtrl.Controls.Add(this.txtPortA);
            this.tabExtCtrl.Controls.Add(this.label103);
            this.tabExtCtrl.Controls.Add(this.txtPortB);
            this.tabExtCtrl.Controls.Add(this.label106);
            this.tabExtCtrl.Location = new System.Drawing.Point(4, 22);
            this.tabExtCtrl.Name = "tabExtCtrl";
            this.tabExtCtrl.Size = new System.Drawing.Size(417, 309);
            this.tabExtCtrl.TabIndex = 8;
            this.tabExtCtrl.Text = "ExtCtrl";
            this.tabExtCtrl.UseVisualStyleBackColor = true;
            // 
            // label132
            // 
            this.label132.AutoSize = true;
            this.label132.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label132.ForeColor = System.Drawing.Color.Blue;
            this.label132.Location = new System.Drawing.Point(192, 30);
            this.label132.Name = "label132";
            this.label132.Size = new System.Drawing.Size(34, 13);
            this.label132.TabIndex = 279;
            this.label132.Text = "<-----";
            // 
            // label131
            // 
            this.label131.AutoSize = true;
            this.label131.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label131.ForeColor = System.Drawing.Color.Blue;
            this.label131.Location = new System.Drawing.Point(192, 197);
            this.label131.Name = "label131";
            this.label131.Size = new System.Drawing.Size(34, 13);
            this.label131.TabIndex = 278;
            this.label131.Text = "----->";
            // 
            // chkPortB
            // 
            this.chkPortB.AutoSize = true;
            this.chkPortB.ForeColor = System.Drawing.Color.Navy;
            this.chkPortB.Location = new System.Drawing.Point(182, 215);
            this.chkPortB.Name = "chkPortB";
            this.chkPortB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkPortB.Size = new System.Drawing.Size(51, 17);
            this.chkPortB.TabIndex = 277;
            this.chkPortB.Text = "Enab";
            this.chkPortB.UseVisualStyleBackColor = true;
            this.chkPortB.CheckedChanged += new System.EventHandler(this.chkPortB_CheckedChanged);
            // 
            // btnClrPortB
            // 
            this.btnClrPortB.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClrPortB.BackColor = System.Drawing.SystemColors.Control;
            this.btnClrPortB.Enabled = false;
            this.btnClrPortB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClrPortB.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClrPortB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClrPortB.Location = new System.Drawing.Point(191, 277);
            this.btnClrPortB.Name = "btnClrPortB";
            this.btnClrPortB.Size = new System.Drawing.Size(35, 19);
            this.btnClrPortB.TabIndex = 276;
            this.btnClrPortB.Text = "Clear";
            this.toolTip1.SetToolTip(this.btnClrPortB, "Clears all Port B bits");
            this.btnClrPortB.UseVisualStyleBackColor = false;
            this.btnClrPortB.Click += new System.EventHandler(this.btnClrPortB_Click);
            // 
            // btnClrPortA
            // 
            this.btnClrPortA.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClrPortA.BackColor = System.Drawing.SystemColors.Control;
            this.btnClrPortA.Enabled = false;
            this.btnClrPortA.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClrPortA.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClrPortA.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClrPortA.Location = new System.Drawing.Point(192, 110);
            this.btnClrPortA.Name = "btnClrPortA";
            this.btnClrPortA.Size = new System.Drawing.Size(35, 19);
            this.btnClrPortA.TabIndex = 275;
            this.btnClrPortA.Text = "Clear";
            this.toolTip1.SetToolTip(this.btnClrPortA, "Clears all Port A bits");
            this.btnClrPortA.UseVisualStyleBackColor = false;
            this.btnClrPortA.Click += new System.EventHandler(this.btnClrPortA_Click);
            // 
            // grpPortB
            // 
            this.grpPortB.Controls.Add(this.cb15t4);
            this.grpPortB.Controls.Add(this.label105);
            this.grpPortB.Controls.Add(this.cb15t7);
            this.grpPortB.Controls.Add(this.cb13t4);
            this.grpPortB.Controls.Add(this.cb15t6);
            this.grpPortB.Controls.Add(this.label107);
            this.grpPortB.Controls.Add(this.cb15t5);
            this.grpPortB.Controls.Add(this.cb13t7);
            this.grpPortB.Controls.Add(this.cb15t3);
            this.grpPortB.Controls.Add(this.cb13t6);
            this.grpPortB.Controls.Add(this.cb15t2);
            this.grpPortB.Controls.Add(this.cb0t3);
            this.grpPortB.Controls.Add(this.cb15t1);
            this.grpPortB.Controls.Add(this.cb13t5);
            this.grpPortB.Controls.Add(this.cb15t0);
            this.grpPortB.Controls.Add(this.cb13t3);
            this.grpPortB.Controls.Add(this.label135);
            this.grpPortB.Controls.Add(this.cb0t0);
            this.grpPortB.Controls.Add(this.label136);
            this.grpPortB.Controls.Add(this.cb13t2);
            this.grpPortB.Controls.Add(this.cb14t0);
            this.grpPortB.Controls.Add(this.cb13t1);
            this.grpPortB.Controls.Add(this.cb14t1);
            this.grpPortB.Controls.Add(this.cb13t0);
            this.grpPortB.Controls.Add(this.cb14t2);
            this.grpPortB.Controls.Add(this.cb0t1);
            this.grpPortB.Controls.Add(this.cb14t3);
            this.grpPortB.Controls.Add(this.label108);
            this.grpPortB.Controls.Add(this.cb14t4);
            this.grpPortB.Controls.Add(this.cb0t2);
            this.grpPortB.Controls.Add(this.cb14t5);
            this.grpPortB.Controls.Add(this.cb14t6);
            this.grpPortB.Controls.Add(this.cb0t4);
            this.grpPortB.Controls.Add(this.cb14t7);
            this.grpPortB.Controls.Add(this.cb0t5);
            this.grpPortB.Controls.Add(this.cb0t6);
            this.grpPortB.Controls.Add(this.cb0t7);
            this.grpPortB.Controls.Add(this.label109);
            this.grpPortB.Controls.Add(this.cb12t0);
            this.grpPortB.Controls.Add(this.cb12t1);
            this.grpPortB.Controls.Add(this.cb12t2);
            this.grpPortB.Controls.Add(this.cb12t3);
            this.grpPortB.Controls.Add(this.cb12t4);
            this.grpPortB.Controls.Add(this.cb12t5);
            this.grpPortB.Controls.Add(this.cb12t6);
            this.grpPortB.Controls.Add(this.cb12t7);
            this.grpPortB.Controls.Add(this.label110);
            this.grpPortB.Controls.Add(this.cb11t0);
            this.grpPortB.Controls.Add(this.cb11t1);
            this.grpPortB.Controls.Add(this.cb11t2);
            this.grpPortB.Controls.Add(this.cb11t3);
            this.grpPortB.Controls.Add(this.cb11t4);
            this.grpPortB.Controls.Add(this.cb11t5);
            this.grpPortB.Controls.Add(this.cb11t6);
            this.grpPortB.Controls.Add(this.cb11t7);
            this.grpPortB.Controls.Add(this.label111);
            this.grpPortB.Controls.Add(this.cb10t0);
            this.grpPortB.Controls.Add(this.cb10t1);
            this.grpPortB.Controls.Add(this.cb10t2);
            this.grpPortB.Controls.Add(this.cb10t3);
            this.grpPortB.Controls.Add(this.cb10t4);
            this.grpPortB.Controls.Add(this.cb10t5);
            this.grpPortB.Controls.Add(this.cb10t6);
            this.grpPortB.Controls.Add(this.cb10t7);
            this.grpPortB.Controls.Add(this.label112);
            this.grpPortB.Controls.Add(this.cb9t0);
            this.grpPortB.Controls.Add(this.cb9t1);
            this.grpPortB.Controls.Add(this.cb9t2);
            this.grpPortB.Controls.Add(this.cb9t3);
            this.grpPortB.Controls.Add(this.cb9t4);
            this.grpPortB.Controls.Add(this.cb9t5);
            this.grpPortB.Controls.Add(this.cb9t6);
            this.grpPortB.Controls.Add(this.cb9t7);
            this.grpPortB.Controls.Add(this.label113);
            this.grpPortB.Controls.Add(this.cb8t0);
            this.grpPortB.Controls.Add(this.cb8t1);
            this.grpPortB.Controls.Add(this.cb8t2);
            this.grpPortB.Controls.Add(this.cb8t3);
            this.grpPortB.Controls.Add(this.cb8t4);
            this.grpPortB.Controls.Add(this.cb8t5);
            this.grpPortB.Controls.Add(this.cb8t6);
            this.grpPortB.Controls.Add(this.cb8t7);
            this.grpPortB.Controls.Add(this.label114);
            this.grpPortB.Controls.Add(this.cb7t0);
            this.grpPortB.Controls.Add(this.cb7t1);
            this.grpPortB.Controls.Add(this.cb7t2);
            this.grpPortB.Controls.Add(this.cb7t3);
            this.grpPortB.Controls.Add(this.cb7t4);
            this.grpPortB.Controls.Add(this.cb7t5);
            this.grpPortB.Controls.Add(this.cb7t6);
            this.grpPortB.Controls.Add(this.cb7t7);
            this.grpPortB.Controls.Add(this.label115);
            this.grpPortB.Controls.Add(this.cb6t0);
            this.grpPortB.Controls.Add(this.cb6t1);
            this.grpPortB.Controls.Add(this.cb6t2);
            this.grpPortB.Controls.Add(this.cb6t3);
            this.grpPortB.Controls.Add(this.cb6t4);
            this.grpPortB.Controls.Add(this.cb6t5);
            this.grpPortB.Controls.Add(this.cb6t6);
            this.grpPortB.Controls.Add(this.cb6t7);
            this.grpPortB.Controls.Add(this.label116);
            this.grpPortB.Controls.Add(this.cb5t0);
            this.grpPortB.Controls.Add(this.cb5t1);
            this.grpPortB.Controls.Add(this.cb5t2);
            this.grpPortB.Controls.Add(this.cb5t3);
            this.grpPortB.Controls.Add(this.cb5t4);
            this.grpPortB.Controls.Add(this.cb5t5);
            this.grpPortB.Controls.Add(this.cb5t6);
            this.grpPortB.Controls.Add(this.cb5t7);
            this.grpPortB.Controls.Add(this.label117);
            this.grpPortB.Controls.Add(this.cb4t0);
            this.grpPortB.Controls.Add(this.cb4t1);
            this.grpPortB.Controls.Add(this.cb4t2);
            this.grpPortB.Controls.Add(this.cb4t3);
            this.grpPortB.Controls.Add(this.cb4t4);
            this.grpPortB.Controls.Add(this.cb4t5);
            this.grpPortB.Controls.Add(this.cb4t6);
            this.grpPortB.Controls.Add(this.cb4t7);
            this.grpPortB.Controls.Add(this.label118);
            this.grpPortB.Controls.Add(this.cb3t0);
            this.grpPortB.Controls.Add(this.cb3t1);
            this.grpPortB.Controls.Add(this.cb3t2);
            this.grpPortB.Controls.Add(this.cb3t3);
            this.grpPortB.Controls.Add(this.cb3t4);
            this.grpPortB.Controls.Add(this.cb3t5);
            this.grpPortB.Controls.Add(this.cb3t6);
            this.grpPortB.Controls.Add(this.cb3t7);
            this.grpPortB.Controls.Add(this.label119);
            this.grpPortB.Controls.Add(this.cb2t0);
            this.grpPortB.Controls.Add(this.cb2t1);
            this.grpPortB.Controls.Add(this.cb2t2);
            this.grpPortB.Controls.Add(this.cb2t3);
            this.grpPortB.Controls.Add(this.cb2t4);
            this.grpPortB.Controls.Add(this.cb2t5);
            this.grpPortB.Controls.Add(this.cb2t6);
            this.grpPortB.Controls.Add(this.cb2t7);
            this.grpPortB.Controls.Add(this.label120);
            this.grpPortB.Controls.Add(this.cb1t0);
            this.grpPortB.Controls.Add(this.cb1t1);
            this.grpPortB.Controls.Add(this.cb1t2);
            this.grpPortB.Controls.Add(this.cb1t3);
            this.grpPortB.Controls.Add(this.cb1t4);
            this.grpPortB.Controls.Add(this.cb1t5);
            this.grpPortB.Controls.Add(this.cb1t6);
            this.grpPortB.Controls.Add(this.cb1t7);
            this.grpPortB.Controls.Add(this.label121);
            this.grpPortB.Controls.Add(this.label122);
            this.grpPortB.Controls.Add(this.label123);
            this.grpPortB.Controls.Add(this.label124);
            this.grpPortB.Controls.Add(this.label125);
            this.grpPortB.Controls.Add(this.label126);
            this.grpPortB.Controls.Add(this.label127);
            this.grpPortB.Controls.Add(this.label128);
            this.grpPortB.Controls.Add(this.label130);
            this.grpPortB.ForeColor = System.Drawing.Color.Blue;
            this.grpPortB.Location = new System.Drawing.Point(239, 6);
            this.grpPortB.Name = "grpPortB";
            this.grpPortB.Size = new System.Drawing.Size(174, 292);
            this.grpPortB.TabIndex = 1;
            this.grpPortB.TabStop = false;
            this.grpPortB.Text = "Port B";
            // 
            // cb15t4
            // 
            this.cb15t4.AutoSize = true;
            this.cb15t4.Enabled = false;
            this.cb15t4.Location = new System.Drawing.Point(89, 272);
            this.cb15t4.Name = "cb15t4";
            this.cb15t4.Size = new System.Drawing.Size(15, 14);
            this.cb15t4.TabIndex = 169;
            this.cb15t4.UseVisualStyleBackColor = true;
            this.cb15t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label105
            // 
            this.label105.AutoSize = true;
            this.label105.ForeColor = System.Drawing.Color.Firebrick;
            this.label105.Location = new System.Drawing.Point(2, 24);
            this.label105.Name = "label105";
            this.label105.Size = new System.Drawing.Size(34, 13);
            this.label105.TabIndex = 410;
            this.label105.Text = "VHF+";
            // 
            // cb15t7
            // 
            this.cb15t7.AutoSize = true;
            this.cb15t7.Enabled = false;
            this.cb15t7.Location = new System.Drawing.Point(40, 272);
            this.cb15t7.Name = "cb15t7";
            this.cb15t7.Size = new System.Drawing.Size(15, 14);
            this.cb15t7.TabIndex = 166;
            this.cb15t7.UseVisualStyleBackColor = true;
            this.cb15t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb13t4
            // 
            this.cb13t4.AutoSize = true;
            this.cb13t4.Enabled = false;
            this.cb13t4.Location = new System.Drawing.Point(89, 237);
            this.cb13t4.Name = "cb13t4";
            this.cb13t4.Size = new System.Drawing.Size(15, 14);
            this.cb13t4.TabIndex = 403;
            this.cb13t4.UseVisualStyleBackColor = true;
            this.cb13t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb15t6
            // 
            this.cb15t6.AutoSize = true;
            this.cb15t6.Enabled = false;
            this.cb15t6.Location = new System.Drawing.Point(57, 272);
            this.cb15t6.Name = "cb15t6";
            this.cb15t6.Size = new System.Drawing.Size(15, 14);
            this.cb15t6.TabIndex = 167;
            this.cb15t6.UseVisualStyleBackColor = true;
            this.cb15t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label107
            // 
            this.label107.AutoSize = true;
            this.label107.ForeColor = System.Drawing.Color.Navy;
            this.label107.Location = new System.Drawing.Point(39, 8);
            this.label107.Name = "label107";
            this.label107.Size = new System.Drawing.Size(130, 13);
            this.label107.TabIndex = 409;
            this.label107.Text = "- - - - - - - Port Bits - - - - - - -";
            // 
            // cb15t5
            // 
            this.cb15t5.AutoSize = true;
            this.cb15t5.Enabled = false;
            this.cb15t5.Location = new System.Drawing.Point(73, 272);
            this.cb15t5.Name = "cb15t5";
            this.cb15t5.Size = new System.Drawing.Size(15, 14);
            this.cb15t5.TabIndex = 168;
            this.cb15t5.UseVisualStyleBackColor = true;
            this.cb15t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb13t7
            // 
            this.cb13t7.AutoSize = true;
            this.cb13t7.Enabled = false;
            this.cb13t7.Location = new System.Drawing.Point(40, 237);
            this.cb13t7.Name = "cb13t7";
            this.cb13t7.Size = new System.Drawing.Size(15, 14);
            this.cb13t7.TabIndex = 400;
            this.cb13t7.UseVisualStyleBackColor = true;
            this.cb13t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb15t3
            // 
            this.cb15t3.AutoSize = true;
            this.cb15t3.Enabled = false;
            this.cb15t3.Location = new System.Drawing.Point(105, 272);
            this.cb15t3.Name = "cb15t3";
            this.cb15t3.Size = new System.Drawing.Size(15, 14);
            this.cb15t3.TabIndex = 170;
            this.cb15t3.UseVisualStyleBackColor = true;
            this.cb15t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb13t6
            // 
            this.cb13t6.AutoSize = true;
            this.cb13t6.Enabled = false;
            this.cb13t6.Location = new System.Drawing.Point(57, 237);
            this.cb13t6.Name = "cb13t6";
            this.cb13t6.Size = new System.Drawing.Size(15, 14);
            this.cb13t6.TabIndex = 401;
            this.cb13t6.UseVisualStyleBackColor = true;
            this.cb13t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb15t2
            // 
            this.cb15t2.AutoSize = true;
            this.cb15t2.Enabled = false;
            this.cb15t2.Location = new System.Drawing.Point(121, 272);
            this.cb15t2.Name = "cb15t2";
            this.cb15t2.Size = new System.Drawing.Size(15, 14);
            this.cb15t2.TabIndex = 171;
            this.cb15t2.UseVisualStyleBackColor = true;
            this.cb15t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb0t3
            // 
            this.cb0t3.AutoSize = true;
            this.cb0t3.Enabled = false;
            this.cb0t3.Location = new System.Drawing.Point(105, 40);
            this.cb0t3.Name = "cb0t3";
            this.cb0t3.Size = new System.Drawing.Size(15, 14);
            this.cb0t3.TabIndex = 278;
            this.cb0t3.UseVisualStyleBackColor = true;
            this.cb0t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb15t1
            // 
            this.cb15t1.AutoSize = true;
            this.cb15t1.Enabled = false;
            this.cb15t1.Location = new System.Drawing.Point(137, 272);
            this.cb15t1.Name = "cb15t1";
            this.cb15t1.Size = new System.Drawing.Size(15, 14);
            this.cb15t1.TabIndex = 172;
            this.cb15t1.UseVisualStyleBackColor = true;
            this.cb15t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb13t5
            // 
            this.cb13t5.AutoSize = true;
            this.cb13t5.Enabled = false;
            this.cb13t5.Location = new System.Drawing.Point(73, 237);
            this.cb13t5.Name = "cb13t5";
            this.cb13t5.Size = new System.Drawing.Size(15, 14);
            this.cb13t5.TabIndex = 402;
            this.cb13t5.UseVisualStyleBackColor = true;
            this.cb13t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb15t0
            // 
            this.cb15t0.AutoSize = true;
            this.cb15t0.Enabled = false;
            this.cb15t0.Location = new System.Drawing.Point(153, 272);
            this.cb15t0.Name = "cb15t0";
            this.cb15t0.Size = new System.Drawing.Size(15, 14);
            this.cb15t0.TabIndex = 173;
            this.cb15t0.UseVisualStyleBackColor = true;
            this.cb15t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb13t3
            // 
            this.cb13t3.AutoSize = true;
            this.cb13t3.Enabled = false;
            this.cb13t3.Location = new System.Drawing.Point(105, 237);
            this.cb13t3.Name = "cb13t3";
            this.cb13t3.Size = new System.Drawing.Size(15, 14);
            this.cb13t3.TabIndex = 404;
            this.cb13t3.UseVisualStyleBackColor = true;
            this.cb13t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label135
            // 
            this.label135.AutoSize = true;
            this.label135.ForeColor = System.Drawing.Color.Green;
            this.label135.Location = new System.Drawing.Point(7, 257);
            this.label135.Name = "label135";
            this.label135.Size = new System.Drawing.Size(27, 13);
            this.label135.TabIndex = 174;
            this.label135.Text = "HF6";
            // 
            // cb0t0
            // 
            this.cb0t0.AutoSize = true;
            this.cb0t0.Enabled = false;
            this.cb0t0.Location = new System.Drawing.Point(153, 40);
            this.cb0t0.Name = "cb0t0";
            this.cb0t0.Size = new System.Drawing.Size(15, 14);
            this.cb0t0.TabIndex = 275;
            this.cb0t0.UseVisualStyleBackColor = true;
            this.cb0t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label136
            // 
            this.label136.AutoSize = true;
            this.label136.ForeColor = System.Drawing.Color.Green;
            this.label136.Location = new System.Drawing.Point(7, 272);
            this.label136.Name = "label136";
            this.label136.Size = new System.Drawing.Size(27, 13);
            this.label136.TabIndex = 165;
            this.label136.Text = "HF2";
            // 
            // cb13t2
            // 
            this.cb13t2.AutoSize = true;
            this.cb13t2.Enabled = false;
            this.cb13t2.Location = new System.Drawing.Point(121, 237);
            this.cb13t2.Name = "cb13t2";
            this.cb13t2.Size = new System.Drawing.Size(15, 14);
            this.cb13t2.TabIndex = 405;
            this.cb13t2.UseVisualStyleBackColor = true;
            this.cb13t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb14t0
            // 
            this.cb14t0.AutoSize = true;
            this.cb14t0.Enabled = false;
            this.cb14t0.Location = new System.Drawing.Point(153, 257);
            this.cb14t0.Name = "cb14t0";
            this.cb14t0.Size = new System.Drawing.Size(15, 14);
            this.cb14t0.TabIndex = 164;
            this.cb14t0.UseVisualStyleBackColor = true;
            this.cb14t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb13t1
            // 
            this.cb13t1.AutoSize = true;
            this.cb13t1.Enabled = false;
            this.cb13t1.Location = new System.Drawing.Point(137, 237);
            this.cb13t1.Name = "cb13t1";
            this.cb13t1.Size = new System.Drawing.Size(15, 14);
            this.cb13t1.TabIndex = 406;
            this.cb13t1.UseVisualStyleBackColor = true;
            this.cb13t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb14t1
            // 
            this.cb14t1.AutoSize = true;
            this.cb14t1.Enabled = false;
            this.cb14t1.Location = new System.Drawing.Point(137, 257);
            this.cb14t1.Name = "cb14t1";
            this.cb14t1.Size = new System.Drawing.Size(15, 14);
            this.cb14t1.TabIndex = 163;
            this.cb14t1.UseVisualStyleBackColor = true;
            this.cb14t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb13t0
            // 
            this.cb13t0.AutoSize = true;
            this.cb13t0.Enabled = false;
            this.cb13t0.Location = new System.Drawing.Point(153, 237);
            this.cb13t0.Name = "cb13t0";
            this.cb13t0.Size = new System.Drawing.Size(15, 14);
            this.cb13t0.TabIndex = 407;
            this.cb13t0.UseVisualStyleBackColor = true;
            this.cb13t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb14t2
            // 
            this.cb14t2.AutoSize = true;
            this.cb14t2.Enabled = false;
            this.cb14t2.Location = new System.Drawing.Point(121, 257);
            this.cb14t2.Name = "cb14t2";
            this.cb14t2.Size = new System.Drawing.Size(15, 14);
            this.cb14t2.TabIndex = 162;
            this.cb14t2.UseVisualStyleBackColor = true;
            this.cb14t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb0t1
            // 
            this.cb0t1.AutoSize = true;
            this.cb0t1.Enabled = false;
            this.cb0t1.Location = new System.Drawing.Point(137, 40);
            this.cb0t1.Name = "cb0t1";
            this.cb0t1.Size = new System.Drawing.Size(15, 14);
            this.cb0t1.TabIndex = 276;
            this.cb0t1.UseVisualStyleBackColor = true;
            this.cb0t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb14t3
            // 
            this.cb14t3.AutoSize = true;
            this.cb14t3.Enabled = false;
            this.cb14t3.Location = new System.Drawing.Point(105, 257);
            this.cb14t3.Name = "cb14t3";
            this.cb14t3.Size = new System.Drawing.Size(15, 14);
            this.cb14t3.TabIndex = 161;
            this.cb14t3.UseVisualStyleBackColor = true;
            this.cb14t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label108
            // 
            this.label108.AutoSize = true;
            this.label108.ForeColor = System.Drawing.Color.Firebrick;
            this.label108.Location = new System.Drawing.Point(7, 222);
            this.label108.Name = "label108";
            this.label108.Size = new System.Drawing.Size(19, 13);
            this.label108.TabIndex = 408;
            this.label108.Text = "12";
            // 
            // cb14t4
            // 
            this.cb14t4.AutoSize = true;
            this.cb14t4.Enabled = false;
            this.cb14t4.Location = new System.Drawing.Point(89, 257);
            this.cb14t4.Name = "cb14t4";
            this.cb14t4.Size = new System.Drawing.Size(15, 14);
            this.cb14t4.TabIndex = 160;
            this.cb14t4.UseVisualStyleBackColor = true;
            this.cb14t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb0t2
            // 
            this.cb0t2.AutoSize = true;
            this.cb0t2.Enabled = false;
            this.cb0t2.Location = new System.Drawing.Point(121, 40);
            this.cb0t2.Name = "cb0t2";
            this.cb0t2.Size = new System.Drawing.Size(15, 14);
            this.cb0t2.TabIndex = 277;
            this.cb0t2.UseVisualStyleBackColor = true;
            this.cb0t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb14t5
            // 
            this.cb14t5.AutoSize = true;
            this.cb14t5.Enabled = false;
            this.cb14t5.Location = new System.Drawing.Point(73, 257);
            this.cb14t5.Name = "cb14t5";
            this.cb14t5.Size = new System.Drawing.Size(15, 14);
            this.cb14t5.TabIndex = 159;
            this.cb14t5.UseVisualStyleBackColor = true;
            this.cb14t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb14t6
            // 
            this.cb14t6.AutoSize = true;
            this.cb14t6.Enabled = false;
            this.cb14t6.Location = new System.Drawing.Point(57, 257);
            this.cb14t6.Name = "cb14t6";
            this.cb14t6.Size = new System.Drawing.Size(15, 14);
            this.cb14t6.TabIndex = 158;
            this.cb14t6.UseVisualStyleBackColor = true;
            this.cb14t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb0t4
            // 
            this.cb0t4.AutoSize = true;
            this.cb0t4.Enabled = false;
            this.cb0t4.Location = new System.Drawing.Point(90, 40);
            this.cb0t4.Name = "cb0t4";
            this.cb0t4.Size = new System.Drawing.Size(15, 14);
            this.cb0t4.TabIndex = 279;
            this.cb0t4.UseVisualStyleBackColor = true;
            this.cb0t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb14t7
            // 
            this.cb14t7.AutoSize = true;
            this.cb14t7.Enabled = false;
            this.cb14t7.Location = new System.Drawing.Point(40, 257);
            this.cb14t7.Name = "cb14t7";
            this.cb14t7.Size = new System.Drawing.Size(15, 14);
            this.cb14t7.TabIndex = 157;
            this.cb14t7.UseVisualStyleBackColor = true;
            this.cb14t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb0t5
            // 
            this.cb0t5.AutoSize = true;
            this.cb0t5.Enabled = false;
            this.cb0t5.Location = new System.Drawing.Point(73, 40);
            this.cb0t5.Name = "cb0t5";
            this.cb0t5.Size = new System.Drawing.Size(15, 14);
            this.cb0t5.TabIndex = 280;
            this.cb0t5.UseVisualStyleBackColor = true;
            this.cb0t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb0t6
            // 
            this.cb0t6.AutoSize = true;
            this.cb0t6.Enabled = false;
            this.cb0t6.Location = new System.Drawing.Point(57, 40);
            this.cb0t6.Name = "cb0t6";
            this.cb0t6.Size = new System.Drawing.Size(15, 14);
            this.cb0t6.TabIndex = 281;
            this.cb0t6.UseVisualStyleBackColor = true;
            this.cb0t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb0t7
            // 
            this.cb0t7.AutoSize = true;
            this.cb0t7.Enabled = false;
            this.cb0t7.Location = new System.Drawing.Point(40, 40);
            this.cb0t7.Name = "cb0t7";
            this.cb0t7.Size = new System.Drawing.Size(15, 14);
            this.cb0t7.TabIndex = 282;
            this.cb0t7.UseVisualStyleBackColor = true;
            this.cb0t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label109
            // 
            this.label109.AutoSize = true;
            this.label109.ForeColor = System.Drawing.Color.Firebrick;
            this.label109.Location = new System.Drawing.Point(7, 237);
            this.label109.Name = "label109";
            this.label109.Size = new System.Drawing.Size(19, 13);
            this.label109.TabIndex = 399;
            this.label109.Text = "13";
            // 
            // cb12t0
            // 
            this.cb12t0.AutoSize = true;
            this.cb12t0.Enabled = false;
            this.cb12t0.Location = new System.Drawing.Point(153, 222);
            this.cb12t0.Name = "cb12t0";
            this.cb12t0.Size = new System.Drawing.Size(15, 14);
            this.cb12t0.TabIndex = 398;
            this.cb12t0.UseVisualStyleBackColor = true;
            this.cb12t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb12t1
            // 
            this.cb12t1.AutoSize = true;
            this.cb12t1.Enabled = false;
            this.cb12t1.Location = new System.Drawing.Point(137, 222);
            this.cb12t1.Name = "cb12t1";
            this.cb12t1.Size = new System.Drawing.Size(15, 14);
            this.cb12t1.TabIndex = 397;
            this.cb12t1.UseVisualStyleBackColor = true;
            this.cb12t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb12t2
            // 
            this.cb12t2.AutoSize = true;
            this.cb12t2.Enabled = false;
            this.cb12t2.Location = new System.Drawing.Point(121, 222);
            this.cb12t2.Name = "cb12t2";
            this.cb12t2.Size = new System.Drawing.Size(15, 14);
            this.cb12t2.TabIndex = 396;
            this.cb12t2.UseVisualStyleBackColor = true;
            this.cb12t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb12t3
            // 
            this.cb12t3.AutoSize = true;
            this.cb12t3.Enabled = false;
            this.cb12t3.Location = new System.Drawing.Point(105, 222);
            this.cb12t3.Name = "cb12t3";
            this.cb12t3.Size = new System.Drawing.Size(15, 14);
            this.cb12t3.TabIndex = 395;
            this.cb12t3.UseVisualStyleBackColor = true;
            this.cb12t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb12t4
            // 
            this.cb12t4.AutoSize = true;
            this.cb12t4.Enabled = false;
            this.cb12t4.Location = new System.Drawing.Point(89, 222);
            this.cb12t4.Name = "cb12t4";
            this.cb12t4.Size = new System.Drawing.Size(15, 14);
            this.cb12t4.TabIndex = 394;
            this.cb12t4.UseVisualStyleBackColor = true;
            this.cb12t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb12t5
            // 
            this.cb12t5.AutoSize = true;
            this.cb12t5.Enabled = false;
            this.cb12t5.Location = new System.Drawing.Point(73, 222);
            this.cb12t5.Name = "cb12t5";
            this.cb12t5.Size = new System.Drawing.Size(15, 14);
            this.cb12t5.TabIndex = 393;
            this.cb12t5.UseVisualStyleBackColor = true;
            this.cb12t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb12t6
            // 
            this.cb12t6.AutoSize = true;
            this.cb12t6.Enabled = false;
            this.cb12t6.Location = new System.Drawing.Point(57, 222);
            this.cb12t6.Name = "cb12t6";
            this.cb12t6.Size = new System.Drawing.Size(15, 14);
            this.cb12t6.TabIndex = 392;
            this.cb12t6.UseVisualStyleBackColor = true;
            this.cb12t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb12t7
            // 
            this.cb12t7.AutoSize = true;
            this.cb12t7.Enabled = false;
            this.cb12t7.Location = new System.Drawing.Point(40, 222);
            this.cb12t7.Name = "cb12t7";
            this.cb12t7.Size = new System.Drawing.Size(15, 14);
            this.cb12t7.TabIndex = 391;
            this.cb12t7.UseVisualStyleBackColor = true;
            this.cb12t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label110
            // 
            this.label110.AutoSize = true;
            this.label110.ForeColor = System.Drawing.Color.Firebrick;
            this.label110.Location = new System.Drawing.Point(7, 207);
            this.label110.Name = "label110";
            this.label110.Size = new System.Drawing.Size(19, 13);
            this.label110.TabIndex = 390;
            this.label110.Text = "11";
            // 
            // cb11t0
            // 
            this.cb11t0.AutoSize = true;
            this.cb11t0.Enabled = false;
            this.cb11t0.Location = new System.Drawing.Point(153, 207);
            this.cb11t0.Name = "cb11t0";
            this.cb11t0.Size = new System.Drawing.Size(15, 14);
            this.cb11t0.TabIndex = 389;
            this.cb11t0.UseVisualStyleBackColor = true;
            this.cb11t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb11t1
            // 
            this.cb11t1.AutoSize = true;
            this.cb11t1.Enabled = false;
            this.cb11t1.Location = new System.Drawing.Point(137, 207);
            this.cb11t1.Name = "cb11t1";
            this.cb11t1.Size = new System.Drawing.Size(15, 14);
            this.cb11t1.TabIndex = 388;
            this.cb11t1.UseVisualStyleBackColor = true;
            this.cb11t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb11t2
            // 
            this.cb11t2.AutoSize = true;
            this.cb11t2.Enabled = false;
            this.cb11t2.Location = new System.Drawing.Point(121, 207);
            this.cb11t2.Name = "cb11t2";
            this.cb11t2.Size = new System.Drawing.Size(15, 14);
            this.cb11t2.TabIndex = 387;
            this.cb11t2.UseVisualStyleBackColor = true;
            this.cb11t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb11t3
            // 
            this.cb11t3.AutoSize = true;
            this.cb11t3.Enabled = false;
            this.cb11t3.Location = new System.Drawing.Point(105, 207);
            this.cb11t3.Name = "cb11t3";
            this.cb11t3.Size = new System.Drawing.Size(15, 14);
            this.cb11t3.TabIndex = 386;
            this.cb11t3.UseVisualStyleBackColor = true;
            this.cb11t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb11t4
            // 
            this.cb11t4.AutoSize = true;
            this.cb11t4.Enabled = false;
            this.cb11t4.Location = new System.Drawing.Point(89, 207);
            this.cb11t4.Name = "cb11t4";
            this.cb11t4.Size = new System.Drawing.Size(15, 14);
            this.cb11t4.TabIndex = 385;
            this.cb11t4.UseVisualStyleBackColor = true;
            this.cb11t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb11t5
            // 
            this.cb11t5.AutoSize = true;
            this.cb11t5.Enabled = false;
            this.cb11t5.Location = new System.Drawing.Point(73, 207);
            this.cb11t5.Name = "cb11t5";
            this.cb11t5.Size = new System.Drawing.Size(15, 14);
            this.cb11t5.TabIndex = 384;
            this.cb11t5.UseVisualStyleBackColor = true;
            this.cb11t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb11t6
            // 
            this.cb11t6.AutoSize = true;
            this.cb11t6.Enabled = false;
            this.cb11t6.Location = new System.Drawing.Point(57, 207);
            this.cb11t6.Name = "cb11t6";
            this.cb11t6.Size = new System.Drawing.Size(15, 14);
            this.cb11t6.TabIndex = 383;
            this.cb11t6.UseVisualStyleBackColor = true;
            this.cb11t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb11t7
            // 
            this.cb11t7.AutoSize = true;
            this.cb11t7.Enabled = false;
            this.cb11t7.Location = new System.Drawing.Point(40, 207);
            this.cb11t7.Name = "cb11t7";
            this.cb11t7.Size = new System.Drawing.Size(15, 14);
            this.cb11t7.TabIndex = 382;
            this.cb11t7.UseVisualStyleBackColor = true;
            this.cb11t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label111
            // 
            this.label111.AutoSize = true;
            this.label111.ForeColor = System.Drawing.Color.Firebrick;
            this.label111.Location = new System.Drawing.Point(7, 192);
            this.label111.Name = "label111";
            this.label111.Size = new System.Drawing.Size(19, 13);
            this.label111.TabIndex = 381;
            this.label111.Text = "10";
            // 
            // cb10t0
            // 
            this.cb10t0.AutoSize = true;
            this.cb10t0.Enabled = false;
            this.cb10t0.Location = new System.Drawing.Point(153, 192);
            this.cb10t0.Name = "cb10t0";
            this.cb10t0.Size = new System.Drawing.Size(15, 14);
            this.cb10t0.TabIndex = 380;
            this.cb10t0.UseVisualStyleBackColor = true;
            this.cb10t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb10t1
            // 
            this.cb10t1.AutoSize = true;
            this.cb10t1.Enabled = false;
            this.cb10t1.Location = new System.Drawing.Point(137, 192);
            this.cb10t1.Name = "cb10t1";
            this.cb10t1.Size = new System.Drawing.Size(15, 14);
            this.cb10t1.TabIndex = 379;
            this.cb10t1.UseVisualStyleBackColor = true;
            this.cb10t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb10t2
            // 
            this.cb10t2.AutoSize = true;
            this.cb10t2.Enabled = false;
            this.cb10t2.Location = new System.Drawing.Point(121, 192);
            this.cb10t2.Name = "cb10t2";
            this.cb10t2.Size = new System.Drawing.Size(15, 14);
            this.cb10t2.TabIndex = 378;
            this.cb10t2.UseVisualStyleBackColor = true;
            this.cb10t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb10t3
            // 
            this.cb10t3.AutoSize = true;
            this.cb10t3.Enabled = false;
            this.cb10t3.Location = new System.Drawing.Point(105, 192);
            this.cb10t3.Name = "cb10t3";
            this.cb10t3.Size = new System.Drawing.Size(15, 14);
            this.cb10t3.TabIndex = 377;
            this.cb10t3.UseVisualStyleBackColor = true;
            this.cb10t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb10t4
            // 
            this.cb10t4.AutoSize = true;
            this.cb10t4.Enabled = false;
            this.cb10t4.Location = new System.Drawing.Point(89, 192);
            this.cb10t4.Name = "cb10t4";
            this.cb10t4.Size = new System.Drawing.Size(15, 14);
            this.cb10t4.TabIndex = 376;
            this.cb10t4.UseVisualStyleBackColor = true;
            this.cb10t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb10t5
            // 
            this.cb10t5.AutoSize = true;
            this.cb10t5.Enabled = false;
            this.cb10t5.Location = new System.Drawing.Point(73, 192);
            this.cb10t5.Name = "cb10t5";
            this.cb10t5.Size = new System.Drawing.Size(15, 14);
            this.cb10t5.TabIndex = 375;
            this.cb10t5.UseVisualStyleBackColor = true;
            this.cb10t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb10t6
            // 
            this.cb10t6.AutoSize = true;
            this.cb10t6.Enabled = false;
            this.cb10t6.Location = new System.Drawing.Point(57, 192);
            this.cb10t6.Name = "cb10t6";
            this.cb10t6.Size = new System.Drawing.Size(15, 14);
            this.cb10t6.TabIndex = 374;
            this.cb10t6.UseVisualStyleBackColor = true;
            this.cb10t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb10t7
            // 
            this.cb10t7.AutoSize = true;
            this.cb10t7.Enabled = false;
            this.cb10t7.Location = new System.Drawing.Point(40, 192);
            this.cb10t7.Name = "cb10t7";
            this.cb10t7.Size = new System.Drawing.Size(15, 14);
            this.cb10t7.TabIndex = 373;
            this.cb10t7.UseVisualStyleBackColor = true;
            this.cb10t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label112
            // 
            this.label112.AutoSize = true;
            this.label112.ForeColor = System.Drawing.Color.Firebrick;
            this.label112.Location = new System.Drawing.Point(13, 177);
            this.label112.Name = "label112";
            this.label112.Size = new System.Drawing.Size(13, 13);
            this.label112.TabIndex = 372;
            this.label112.Text = "9";
            // 
            // cb9t0
            // 
            this.cb9t0.AutoSize = true;
            this.cb9t0.Enabled = false;
            this.cb9t0.Location = new System.Drawing.Point(153, 177);
            this.cb9t0.Name = "cb9t0";
            this.cb9t0.Size = new System.Drawing.Size(15, 14);
            this.cb9t0.TabIndex = 371;
            this.cb9t0.UseVisualStyleBackColor = true;
            this.cb9t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb9t1
            // 
            this.cb9t1.AutoSize = true;
            this.cb9t1.Enabled = false;
            this.cb9t1.Location = new System.Drawing.Point(137, 177);
            this.cb9t1.Name = "cb9t1";
            this.cb9t1.Size = new System.Drawing.Size(15, 14);
            this.cb9t1.TabIndex = 370;
            this.cb9t1.UseVisualStyleBackColor = true;
            this.cb9t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb9t2
            // 
            this.cb9t2.AutoSize = true;
            this.cb9t2.Enabled = false;
            this.cb9t2.Location = new System.Drawing.Point(121, 177);
            this.cb9t2.Name = "cb9t2";
            this.cb9t2.Size = new System.Drawing.Size(15, 14);
            this.cb9t2.TabIndex = 369;
            this.cb9t2.UseVisualStyleBackColor = true;
            this.cb9t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb9t3
            // 
            this.cb9t3.AutoSize = true;
            this.cb9t3.Enabled = false;
            this.cb9t3.Location = new System.Drawing.Point(105, 177);
            this.cb9t3.Name = "cb9t3";
            this.cb9t3.Size = new System.Drawing.Size(15, 14);
            this.cb9t3.TabIndex = 368;
            this.cb9t3.UseVisualStyleBackColor = true;
            this.cb9t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb9t4
            // 
            this.cb9t4.AutoSize = true;
            this.cb9t4.Enabled = false;
            this.cb9t4.Location = new System.Drawing.Point(89, 177);
            this.cb9t4.Name = "cb9t4";
            this.cb9t4.Size = new System.Drawing.Size(15, 14);
            this.cb9t4.TabIndex = 367;
            this.cb9t4.UseVisualStyleBackColor = true;
            this.cb9t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb9t5
            // 
            this.cb9t5.AutoSize = true;
            this.cb9t5.Enabled = false;
            this.cb9t5.Location = new System.Drawing.Point(73, 177);
            this.cb9t5.Name = "cb9t5";
            this.cb9t5.Size = new System.Drawing.Size(15, 14);
            this.cb9t5.TabIndex = 366;
            this.cb9t5.UseVisualStyleBackColor = true;
            this.cb9t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb9t6
            // 
            this.cb9t6.AutoSize = true;
            this.cb9t6.Enabled = false;
            this.cb9t6.Location = new System.Drawing.Point(57, 177);
            this.cb9t6.Name = "cb9t6";
            this.cb9t6.Size = new System.Drawing.Size(15, 14);
            this.cb9t6.TabIndex = 365;
            this.cb9t6.UseVisualStyleBackColor = true;
            this.cb9t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb9t7
            // 
            this.cb9t7.AutoSize = true;
            this.cb9t7.Enabled = false;
            this.cb9t7.Location = new System.Drawing.Point(40, 177);
            this.cb9t7.Name = "cb9t7";
            this.cb9t7.Size = new System.Drawing.Size(15, 14);
            this.cb9t7.TabIndex = 364;
            this.cb9t7.UseVisualStyleBackColor = true;
            this.cb9t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label113
            // 
            this.label113.AutoSize = true;
            this.label113.ForeColor = System.Drawing.Color.Firebrick;
            this.label113.Location = new System.Drawing.Point(13, 162);
            this.label113.Name = "label113";
            this.label113.Size = new System.Drawing.Size(13, 13);
            this.label113.TabIndex = 363;
            this.label113.Text = "8";
            // 
            // cb8t0
            // 
            this.cb8t0.AutoSize = true;
            this.cb8t0.Enabled = false;
            this.cb8t0.Location = new System.Drawing.Point(153, 162);
            this.cb8t0.Name = "cb8t0";
            this.cb8t0.Size = new System.Drawing.Size(15, 14);
            this.cb8t0.TabIndex = 362;
            this.cb8t0.UseVisualStyleBackColor = true;
            this.cb8t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb8t1
            // 
            this.cb8t1.AutoSize = true;
            this.cb8t1.Enabled = false;
            this.cb8t1.Location = new System.Drawing.Point(137, 162);
            this.cb8t1.Name = "cb8t1";
            this.cb8t1.Size = new System.Drawing.Size(15, 14);
            this.cb8t1.TabIndex = 361;
            this.cb8t1.UseVisualStyleBackColor = true;
            this.cb8t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb8t2
            // 
            this.cb8t2.AutoSize = true;
            this.cb8t2.Enabled = false;
            this.cb8t2.Location = new System.Drawing.Point(121, 162);
            this.cb8t2.Name = "cb8t2";
            this.cb8t2.Size = new System.Drawing.Size(15, 14);
            this.cb8t2.TabIndex = 360;
            this.cb8t2.UseVisualStyleBackColor = true;
            this.cb8t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb8t3
            // 
            this.cb8t3.AutoSize = true;
            this.cb8t3.Enabled = false;
            this.cb8t3.Location = new System.Drawing.Point(105, 162);
            this.cb8t3.Name = "cb8t3";
            this.cb8t3.Size = new System.Drawing.Size(15, 14);
            this.cb8t3.TabIndex = 359;
            this.cb8t3.UseVisualStyleBackColor = true;
            this.cb8t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb8t4
            // 
            this.cb8t4.AutoSize = true;
            this.cb8t4.Enabled = false;
            this.cb8t4.Location = new System.Drawing.Point(89, 162);
            this.cb8t4.Name = "cb8t4";
            this.cb8t4.Size = new System.Drawing.Size(15, 14);
            this.cb8t4.TabIndex = 358;
            this.cb8t4.UseVisualStyleBackColor = true;
            this.cb8t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb8t5
            // 
            this.cb8t5.AutoSize = true;
            this.cb8t5.Enabled = false;
            this.cb8t5.Location = new System.Drawing.Point(73, 162);
            this.cb8t5.Name = "cb8t5";
            this.cb8t5.Size = new System.Drawing.Size(15, 14);
            this.cb8t5.TabIndex = 357;
            this.cb8t5.UseVisualStyleBackColor = true;
            this.cb8t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb8t6
            // 
            this.cb8t6.AutoSize = true;
            this.cb8t6.Enabled = false;
            this.cb8t6.Location = new System.Drawing.Point(57, 162);
            this.cb8t6.Name = "cb8t6";
            this.cb8t6.Size = new System.Drawing.Size(15, 14);
            this.cb8t6.TabIndex = 356;
            this.cb8t6.UseVisualStyleBackColor = true;
            this.cb8t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb8t7
            // 
            this.cb8t7.AutoSize = true;
            this.cb8t7.Enabled = false;
            this.cb8t7.Location = new System.Drawing.Point(40, 162);
            this.cb8t7.Name = "cb8t7";
            this.cb8t7.Size = new System.Drawing.Size(15, 14);
            this.cb8t7.TabIndex = 355;
            this.cb8t7.UseVisualStyleBackColor = true;
            this.cb8t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label114
            // 
            this.label114.AutoSize = true;
            this.label114.ForeColor = System.Drawing.Color.Firebrick;
            this.label114.Location = new System.Drawing.Point(13, 147);
            this.label114.Name = "label114";
            this.label114.Size = new System.Drawing.Size(13, 13);
            this.label114.TabIndex = 354;
            this.label114.Text = "7";
            // 
            // cb7t0
            // 
            this.cb7t0.AutoSize = true;
            this.cb7t0.Enabled = false;
            this.cb7t0.Location = new System.Drawing.Point(153, 147);
            this.cb7t0.Name = "cb7t0";
            this.cb7t0.Size = new System.Drawing.Size(15, 14);
            this.cb7t0.TabIndex = 353;
            this.cb7t0.UseVisualStyleBackColor = true;
            this.cb7t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb7t1
            // 
            this.cb7t1.AutoSize = true;
            this.cb7t1.Enabled = false;
            this.cb7t1.Location = new System.Drawing.Point(137, 147);
            this.cb7t1.Name = "cb7t1";
            this.cb7t1.Size = new System.Drawing.Size(15, 14);
            this.cb7t1.TabIndex = 352;
            this.cb7t1.UseVisualStyleBackColor = true;
            this.cb7t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb7t2
            // 
            this.cb7t2.AutoSize = true;
            this.cb7t2.Enabled = false;
            this.cb7t2.Location = new System.Drawing.Point(121, 147);
            this.cb7t2.Name = "cb7t2";
            this.cb7t2.Size = new System.Drawing.Size(15, 14);
            this.cb7t2.TabIndex = 351;
            this.cb7t2.UseVisualStyleBackColor = true;
            this.cb7t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb7t3
            // 
            this.cb7t3.AutoSize = true;
            this.cb7t3.Enabled = false;
            this.cb7t3.Location = new System.Drawing.Point(105, 147);
            this.cb7t3.Name = "cb7t3";
            this.cb7t3.Size = new System.Drawing.Size(15, 14);
            this.cb7t3.TabIndex = 350;
            this.cb7t3.UseVisualStyleBackColor = true;
            this.cb7t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb7t4
            // 
            this.cb7t4.AutoSize = true;
            this.cb7t4.Enabled = false;
            this.cb7t4.Location = new System.Drawing.Point(89, 147);
            this.cb7t4.Name = "cb7t4";
            this.cb7t4.Size = new System.Drawing.Size(15, 14);
            this.cb7t4.TabIndex = 349;
            this.cb7t4.UseVisualStyleBackColor = true;
            this.cb7t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb7t5
            // 
            this.cb7t5.AutoSize = true;
            this.cb7t5.Enabled = false;
            this.cb7t5.Location = new System.Drawing.Point(73, 147);
            this.cb7t5.Name = "cb7t5";
            this.cb7t5.Size = new System.Drawing.Size(15, 14);
            this.cb7t5.TabIndex = 348;
            this.cb7t5.UseVisualStyleBackColor = true;
            this.cb7t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb7t6
            // 
            this.cb7t6.AutoSize = true;
            this.cb7t6.Enabled = false;
            this.cb7t6.Location = new System.Drawing.Point(57, 147);
            this.cb7t6.Name = "cb7t6";
            this.cb7t6.Size = new System.Drawing.Size(15, 14);
            this.cb7t6.TabIndex = 347;
            this.cb7t6.UseVisualStyleBackColor = true;
            this.cb7t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb7t7
            // 
            this.cb7t7.AutoSize = true;
            this.cb7t7.Enabled = false;
            this.cb7t7.Location = new System.Drawing.Point(40, 147);
            this.cb7t7.Name = "cb7t7";
            this.cb7t7.Size = new System.Drawing.Size(15, 14);
            this.cb7t7.TabIndex = 346;
            this.cb7t7.UseVisualStyleBackColor = true;
            this.cb7t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label115
            // 
            this.label115.AutoSize = true;
            this.label115.ForeColor = System.Drawing.Color.Firebrick;
            this.label115.Location = new System.Drawing.Point(13, 132);
            this.label115.Name = "label115";
            this.label115.Size = new System.Drawing.Size(13, 13);
            this.label115.TabIndex = 345;
            this.label115.Text = "6";
            // 
            // cb6t0
            // 
            this.cb6t0.AutoSize = true;
            this.cb6t0.Enabled = false;
            this.cb6t0.Location = new System.Drawing.Point(153, 132);
            this.cb6t0.Name = "cb6t0";
            this.cb6t0.Size = new System.Drawing.Size(15, 14);
            this.cb6t0.TabIndex = 344;
            this.cb6t0.UseVisualStyleBackColor = true;
            this.cb6t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb6t1
            // 
            this.cb6t1.AutoSize = true;
            this.cb6t1.Enabled = false;
            this.cb6t1.Location = new System.Drawing.Point(137, 132);
            this.cb6t1.Name = "cb6t1";
            this.cb6t1.Size = new System.Drawing.Size(15, 14);
            this.cb6t1.TabIndex = 343;
            this.cb6t1.UseVisualStyleBackColor = true;
            this.cb6t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb6t2
            // 
            this.cb6t2.AutoSize = true;
            this.cb6t2.Enabled = false;
            this.cb6t2.Location = new System.Drawing.Point(121, 132);
            this.cb6t2.Name = "cb6t2";
            this.cb6t2.Size = new System.Drawing.Size(15, 14);
            this.cb6t2.TabIndex = 342;
            this.cb6t2.UseVisualStyleBackColor = true;
            this.cb6t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb6t3
            // 
            this.cb6t3.AutoSize = true;
            this.cb6t3.Enabled = false;
            this.cb6t3.Location = new System.Drawing.Point(105, 132);
            this.cb6t3.Name = "cb6t3";
            this.cb6t3.Size = new System.Drawing.Size(15, 14);
            this.cb6t3.TabIndex = 341;
            this.cb6t3.UseVisualStyleBackColor = true;
            this.cb6t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb6t4
            // 
            this.cb6t4.AutoSize = true;
            this.cb6t4.Enabled = false;
            this.cb6t4.Location = new System.Drawing.Point(89, 132);
            this.cb6t4.Name = "cb6t4";
            this.cb6t4.Size = new System.Drawing.Size(15, 14);
            this.cb6t4.TabIndex = 340;
            this.cb6t4.UseVisualStyleBackColor = true;
            this.cb6t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb6t5
            // 
            this.cb6t5.AutoSize = true;
            this.cb6t5.Enabled = false;
            this.cb6t5.Location = new System.Drawing.Point(73, 132);
            this.cb6t5.Name = "cb6t5";
            this.cb6t5.Size = new System.Drawing.Size(15, 14);
            this.cb6t5.TabIndex = 339;
            this.cb6t5.UseVisualStyleBackColor = true;
            this.cb6t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb6t6
            // 
            this.cb6t6.AutoSize = true;
            this.cb6t6.Enabled = false;
            this.cb6t6.Location = new System.Drawing.Point(57, 132);
            this.cb6t6.Name = "cb6t6";
            this.cb6t6.Size = new System.Drawing.Size(15, 14);
            this.cb6t6.TabIndex = 338;
            this.cb6t6.UseVisualStyleBackColor = true;
            this.cb6t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb6t7
            // 
            this.cb6t7.AutoSize = true;
            this.cb6t7.Enabled = false;
            this.cb6t7.Location = new System.Drawing.Point(40, 132);
            this.cb6t7.Name = "cb6t7";
            this.cb6t7.Size = new System.Drawing.Size(15, 14);
            this.cb6t7.TabIndex = 337;
            this.cb6t7.UseVisualStyleBackColor = true;
            this.cb6t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label116
            // 
            this.label116.AutoSize = true;
            this.label116.ForeColor = System.Drawing.Color.Firebrick;
            this.label116.Location = new System.Drawing.Point(13, 118);
            this.label116.Name = "label116";
            this.label116.Size = new System.Drawing.Size(13, 13);
            this.label116.TabIndex = 336;
            this.label116.Text = "5";
            // 
            // cb5t0
            // 
            this.cb5t0.AutoSize = true;
            this.cb5t0.Enabled = false;
            this.cb5t0.Location = new System.Drawing.Point(153, 118);
            this.cb5t0.Name = "cb5t0";
            this.cb5t0.Size = new System.Drawing.Size(15, 14);
            this.cb5t0.TabIndex = 335;
            this.cb5t0.UseVisualStyleBackColor = true;
            this.cb5t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb5t1
            // 
            this.cb5t1.AutoSize = true;
            this.cb5t1.Enabled = false;
            this.cb5t1.Location = new System.Drawing.Point(137, 118);
            this.cb5t1.Name = "cb5t1";
            this.cb5t1.Size = new System.Drawing.Size(15, 14);
            this.cb5t1.TabIndex = 334;
            this.cb5t1.UseVisualStyleBackColor = true;
            this.cb5t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb5t2
            // 
            this.cb5t2.AutoSize = true;
            this.cb5t2.Enabled = false;
            this.cb5t2.Location = new System.Drawing.Point(121, 118);
            this.cb5t2.Name = "cb5t2";
            this.cb5t2.Size = new System.Drawing.Size(15, 14);
            this.cb5t2.TabIndex = 333;
            this.cb5t2.UseVisualStyleBackColor = true;
            this.cb5t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb5t3
            // 
            this.cb5t3.AutoSize = true;
            this.cb5t3.Enabled = false;
            this.cb5t3.Location = new System.Drawing.Point(105, 118);
            this.cb5t3.Name = "cb5t3";
            this.cb5t3.Size = new System.Drawing.Size(15, 14);
            this.cb5t3.TabIndex = 332;
            this.cb5t3.UseVisualStyleBackColor = true;
            this.cb5t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb5t4
            // 
            this.cb5t4.AutoSize = true;
            this.cb5t4.Enabled = false;
            this.cb5t4.Location = new System.Drawing.Point(89, 118);
            this.cb5t4.Name = "cb5t4";
            this.cb5t4.Size = new System.Drawing.Size(15, 14);
            this.cb5t4.TabIndex = 331;
            this.cb5t4.UseVisualStyleBackColor = true;
            this.cb5t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb5t5
            // 
            this.cb5t5.AutoSize = true;
            this.cb5t5.Enabled = false;
            this.cb5t5.Location = new System.Drawing.Point(73, 118);
            this.cb5t5.Name = "cb5t5";
            this.cb5t5.Size = new System.Drawing.Size(15, 14);
            this.cb5t5.TabIndex = 330;
            this.cb5t5.UseVisualStyleBackColor = true;
            this.cb5t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb5t6
            // 
            this.cb5t6.AutoSize = true;
            this.cb5t6.Enabled = false;
            this.cb5t6.Location = new System.Drawing.Point(57, 118);
            this.cb5t6.Name = "cb5t6";
            this.cb5t6.Size = new System.Drawing.Size(15, 14);
            this.cb5t6.TabIndex = 329;
            this.cb5t6.UseVisualStyleBackColor = true;
            this.cb5t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb5t7
            // 
            this.cb5t7.AutoSize = true;
            this.cb5t7.Enabled = false;
            this.cb5t7.Location = new System.Drawing.Point(40, 118);
            this.cb5t7.Name = "cb5t7";
            this.cb5t7.Size = new System.Drawing.Size(15, 14);
            this.cb5t7.TabIndex = 328;
            this.cb5t7.UseVisualStyleBackColor = true;
            this.cb5t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label117
            // 
            this.label117.AutoSize = true;
            this.label117.ForeColor = System.Drawing.Color.Firebrick;
            this.label117.Location = new System.Drawing.Point(13, 103);
            this.label117.Name = "label117";
            this.label117.Size = new System.Drawing.Size(13, 13);
            this.label117.TabIndex = 327;
            this.label117.Text = "4";
            // 
            // cb4t0
            // 
            this.cb4t0.AutoSize = true;
            this.cb4t0.Enabled = false;
            this.cb4t0.Location = new System.Drawing.Point(153, 103);
            this.cb4t0.Name = "cb4t0";
            this.cb4t0.Size = new System.Drawing.Size(15, 14);
            this.cb4t0.TabIndex = 326;
            this.cb4t0.UseVisualStyleBackColor = true;
            this.cb4t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb4t1
            // 
            this.cb4t1.AutoSize = true;
            this.cb4t1.Enabled = false;
            this.cb4t1.Location = new System.Drawing.Point(137, 103);
            this.cb4t1.Name = "cb4t1";
            this.cb4t1.Size = new System.Drawing.Size(15, 14);
            this.cb4t1.TabIndex = 325;
            this.cb4t1.UseVisualStyleBackColor = true;
            this.cb4t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb4t2
            // 
            this.cb4t2.AutoSize = true;
            this.cb4t2.Enabled = false;
            this.cb4t2.Location = new System.Drawing.Point(121, 103);
            this.cb4t2.Name = "cb4t2";
            this.cb4t2.Size = new System.Drawing.Size(15, 14);
            this.cb4t2.TabIndex = 324;
            this.cb4t2.UseVisualStyleBackColor = true;
            this.cb4t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb4t3
            // 
            this.cb4t3.AutoSize = true;
            this.cb4t3.Enabled = false;
            this.cb4t3.Location = new System.Drawing.Point(105, 103);
            this.cb4t3.Name = "cb4t3";
            this.cb4t3.Size = new System.Drawing.Size(15, 14);
            this.cb4t3.TabIndex = 323;
            this.cb4t3.UseVisualStyleBackColor = true;
            this.cb4t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb4t4
            // 
            this.cb4t4.AutoSize = true;
            this.cb4t4.Enabled = false;
            this.cb4t4.Location = new System.Drawing.Point(89, 103);
            this.cb4t4.Name = "cb4t4";
            this.cb4t4.Size = new System.Drawing.Size(15, 14);
            this.cb4t4.TabIndex = 322;
            this.cb4t4.UseVisualStyleBackColor = true;
            this.cb4t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb4t5
            // 
            this.cb4t5.AutoSize = true;
            this.cb4t5.Enabled = false;
            this.cb4t5.Location = new System.Drawing.Point(73, 103);
            this.cb4t5.Name = "cb4t5";
            this.cb4t5.Size = new System.Drawing.Size(15, 14);
            this.cb4t5.TabIndex = 321;
            this.cb4t5.UseVisualStyleBackColor = true;
            this.cb4t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb4t6
            // 
            this.cb4t6.AutoSize = true;
            this.cb4t6.Enabled = false;
            this.cb4t6.Location = new System.Drawing.Point(57, 103);
            this.cb4t6.Name = "cb4t6";
            this.cb4t6.Size = new System.Drawing.Size(15, 14);
            this.cb4t6.TabIndex = 320;
            this.cb4t6.UseVisualStyleBackColor = true;
            this.cb4t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb4t7
            // 
            this.cb4t7.AutoSize = true;
            this.cb4t7.Enabled = false;
            this.cb4t7.Location = new System.Drawing.Point(40, 103);
            this.cb4t7.Name = "cb4t7";
            this.cb4t7.Size = new System.Drawing.Size(15, 14);
            this.cb4t7.TabIndex = 319;
            this.cb4t7.UseVisualStyleBackColor = true;
            this.cb4t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label118
            // 
            this.label118.AutoSize = true;
            this.label118.ForeColor = System.Drawing.Color.Firebrick;
            this.label118.Location = new System.Drawing.Point(13, 87);
            this.label118.Name = "label118";
            this.label118.Size = new System.Drawing.Size(13, 13);
            this.label118.TabIndex = 318;
            this.label118.Text = "3";
            // 
            // cb3t0
            // 
            this.cb3t0.AutoSize = true;
            this.cb3t0.Enabled = false;
            this.cb3t0.Location = new System.Drawing.Point(153, 87);
            this.cb3t0.Name = "cb3t0";
            this.cb3t0.Size = new System.Drawing.Size(15, 14);
            this.cb3t0.TabIndex = 317;
            this.cb3t0.UseVisualStyleBackColor = true;
            this.cb3t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb3t1
            // 
            this.cb3t1.AutoSize = true;
            this.cb3t1.Enabled = false;
            this.cb3t1.Location = new System.Drawing.Point(137, 87);
            this.cb3t1.Name = "cb3t1";
            this.cb3t1.Size = new System.Drawing.Size(15, 14);
            this.cb3t1.TabIndex = 316;
            this.cb3t1.UseVisualStyleBackColor = true;
            this.cb3t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb3t2
            // 
            this.cb3t2.AutoSize = true;
            this.cb3t2.Enabled = false;
            this.cb3t2.Location = new System.Drawing.Point(121, 87);
            this.cb3t2.Name = "cb3t2";
            this.cb3t2.Size = new System.Drawing.Size(15, 14);
            this.cb3t2.TabIndex = 315;
            this.cb3t2.UseVisualStyleBackColor = true;
            this.cb3t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb3t3
            // 
            this.cb3t3.AutoSize = true;
            this.cb3t3.Enabled = false;
            this.cb3t3.Location = new System.Drawing.Point(105, 87);
            this.cb3t3.Name = "cb3t3";
            this.cb3t3.Size = new System.Drawing.Size(15, 14);
            this.cb3t3.TabIndex = 314;
            this.cb3t3.UseVisualStyleBackColor = true;
            this.cb3t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb3t4
            // 
            this.cb3t4.AutoSize = true;
            this.cb3t4.Enabled = false;
            this.cb3t4.Location = new System.Drawing.Point(89, 87);
            this.cb3t4.Name = "cb3t4";
            this.cb3t4.Size = new System.Drawing.Size(15, 14);
            this.cb3t4.TabIndex = 313;
            this.cb3t4.UseVisualStyleBackColor = true;
            this.cb3t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb3t5
            // 
            this.cb3t5.AutoSize = true;
            this.cb3t5.Enabled = false;
            this.cb3t5.Location = new System.Drawing.Point(73, 87);
            this.cb3t5.Name = "cb3t5";
            this.cb3t5.Size = new System.Drawing.Size(15, 14);
            this.cb3t5.TabIndex = 312;
            this.cb3t5.UseVisualStyleBackColor = true;
            this.cb3t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb3t6
            // 
            this.cb3t6.AutoSize = true;
            this.cb3t6.Enabled = false;
            this.cb3t6.Location = new System.Drawing.Point(57, 87);
            this.cb3t6.Name = "cb3t6";
            this.cb3t6.Size = new System.Drawing.Size(15, 14);
            this.cb3t6.TabIndex = 311;
            this.cb3t6.UseVisualStyleBackColor = true;
            this.cb3t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb3t7
            // 
            this.cb3t7.AutoSize = true;
            this.cb3t7.Enabled = false;
            this.cb3t7.Location = new System.Drawing.Point(40, 87);
            this.cb3t7.Name = "cb3t7";
            this.cb3t7.Size = new System.Drawing.Size(15, 14);
            this.cb3t7.TabIndex = 310;
            this.cb3t7.UseVisualStyleBackColor = true;
            this.cb3t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label119
            // 
            this.label119.AutoSize = true;
            this.label119.ForeColor = System.Drawing.Color.Firebrick;
            this.label119.Location = new System.Drawing.Point(13, 72);
            this.label119.Name = "label119";
            this.label119.Size = new System.Drawing.Size(13, 13);
            this.label119.TabIndex = 309;
            this.label119.Text = "2";
            // 
            // cb2t0
            // 
            this.cb2t0.AutoSize = true;
            this.cb2t0.Enabled = false;
            this.cb2t0.Location = new System.Drawing.Point(153, 72);
            this.cb2t0.Name = "cb2t0";
            this.cb2t0.Size = new System.Drawing.Size(15, 14);
            this.cb2t0.TabIndex = 308;
            this.cb2t0.UseVisualStyleBackColor = true;
            this.cb2t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb2t1
            // 
            this.cb2t1.AutoSize = true;
            this.cb2t1.Enabled = false;
            this.cb2t1.Location = new System.Drawing.Point(137, 72);
            this.cb2t1.Name = "cb2t1";
            this.cb2t1.Size = new System.Drawing.Size(15, 14);
            this.cb2t1.TabIndex = 307;
            this.cb2t1.UseVisualStyleBackColor = true;
            this.cb2t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb2t2
            // 
            this.cb2t2.AutoSize = true;
            this.cb2t2.Enabled = false;
            this.cb2t2.Location = new System.Drawing.Point(121, 72);
            this.cb2t2.Name = "cb2t2";
            this.cb2t2.Size = new System.Drawing.Size(15, 14);
            this.cb2t2.TabIndex = 306;
            this.cb2t2.UseVisualStyleBackColor = true;
            this.cb2t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb2t3
            // 
            this.cb2t3.AutoSize = true;
            this.cb2t3.Enabled = false;
            this.cb2t3.Location = new System.Drawing.Point(105, 72);
            this.cb2t3.Name = "cb2t3";
            this.cb2t3.Size = new System.Drawing.Size(15, 14);
            this.cb2t3.TabIndex = 305;
            this.cb2t3.UseVisualStyleBackColor = true;
            this.cb2t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb2t4
            // 
            this.cb2t4.AutoSize = true;
            this.cb2t4.Enabled = false;
            this.cb2t4.Location = new System.Drawing.Point(89, 72);
            this.cb2t4.Name = "cb2t4";
            this.cb2t4.Size = new System.Drawing.Size(15, 14);
            this.cb2t4.TabIndex = 304;
            this.cb2t4.UseVisualStyleBackColor = true;
            this.cb2t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb2t5
            // 
            this.cb2t5.AutoSize = true;
            this.cb2t5.Enabled = false;
            this.cb2t5.Location = new System.Drawing.Point(73, 72);
            this.cb2t5.Name = "cb2t5";
            this.cb2t5.Size = new System.Drawing.Size(15, 14);
            this.cb2t5.TabIndex = 303;
            this.cb2t5.UseVisualStyleBackColor = true;
            this.cb2t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb2t6
            // 
            this.cb2t6.AutoSize = true;
            this.cb2t6.Enabled = false;
            this.cb2t6.Location = new System.Drawing.Point(57, 72);
            this.cb2t6.Name = "cb2t6";
            this.cb2t6.Size = new System.Drawing.Size(15, 14);
            this.cb2t6.TabIndex = 302;
            this.cb2t6.UseVisualStyleBackColor = true;
            this.cb2t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb2t7
            // 
            this.cb2t7.AutoSize = true;
            this.cb2t7.Enabled = false;
            this.cb2t7.Location = new System.Drawing.Point(40, 72);
            this.cb2t7.Name = "cb2t7";
            this.cb2t7.Size = new System.Drawing.Size(15, 14);
            this.cb2t7.TabIndex = 301;
            this.cb2t7.UseVisualStyleBackColor = true;
            this.cb2t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label120
            // 
            this.label120.AutoSize = true;
            this.label120.ForeColor = System.Drawing.Color.Firebrick;
            this.label120.Location = new System.Drawing.Point(13, 56);
            this.label120.Name = "label120";
            this.label120.Size = new System.Drawing.Size(13, 13);
            this.label120.TabIndex = 300;
            this.label120.Text = "1";
            // 
            // cb1t0
            // 
            this.cb1t0.AutoSize = true;
            this.cb1t0.Enabled = false;
            this.cb1t0.Location = new System.Drawing.Point(153, 56);
            this.cb1t0.Name = "cb1t0";
            this.cb1t0.Size = new System.Drawing.Size(15, 14);
            this.cb1t0.TabIndex = 299;
            this.cb1t0.UseVisualStyleBackColor = true;
            this.cb1t0.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb1t1
            // 
            this.cb1t1.AutoSize = true;
            this.cb1t1.Enabled = false;
            this.cb1t1.Location = new System.Drawing.Point(137, 56);
            this.cb1t1.Name = "cb1t1";
            this.cb1t1.Size = new System.Drawing.Size(15, 14);
            this.cb1t1.TabIndex = 298;
            this.cb1t1.UseVisualStyleBackColor = true;
            this.cb1t1.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb1t2
            // 
            this.cb1t2.AutoSize = true;
            this.cb1t2.Enabled = false;
            this.cb1t2.Location = new System.Drawing.Point(121, 56);
            this.cb1t2.Name = "cb1t2";
            this.cb1t2.Size = new System.Drawing.Size(15, 14);
            this.cb1t2.TabIndex = 297;
            this.cb1t2.UseVisualStyleBackColor = true;
            this.cb1t2.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb1t3
            // 
            this.cb1t3.AutoSize = true;
            this.cb1t3.Enabled = false;
            this.cb1t3.Location = new System.Drawing.Point(105, 56);
            this.cb1t3.Name = "cb1t3";
            this.cb1t3.Size = new System.Drawing.Size(15, 14);
            this.cb1t3.TabIndex = 296;
            this.cb1t3.UseVisualStyleBackColor = true;
            this.cb1t3.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb1t4
            // 
            this.cb1t4.AutoSize = true;
            this.cb1t4.Enabled = false;
            this.cb1t4.Location = new System.Drawing.Point(89, 56);
            this.cb1t4.Name = "cb1t4";
            this.cb1t4.Size = new System.Drawing.Size(15, 14);
            this.cb1t4.TabIndex = 295;
            this.cb1t4.UseVisualStyleBackColor = true;
            this.cb1t4.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb1t5
            // 
            this.cb1t5.AutoSize = true;
            this.cb1t5.Enabled = false;
            this.cb1t5.Location = new System.Drawing.Point(73, 56);
            this.cb1t5.Name = "cb1t5";
            this.cb1t5.Size = new System.Drawing.Size(15, 14);
            this.cb1t5.TabIndex = 294;
            this.cb1t5.UseVisualStyleBackColor = true;
            this.cb1t5.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb1t6
            // 
            this.cb1t6.AutoSize = true;
            this.cb1t6.Enabled = false;
            this.cb1t6.Location = new System.Drawing.Point(57, 56);
            this.cb1t6.Name = "cb1t6";
            this.cb1t6.Size = new System.Drawing.Size(15, 14);
            this.cb1t6.TabIndex = 293;
            this.cb1t6.UseVisualStyleBackColor = true;
            this.cb1t6.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // cb1t7
            // 
            this.cb1t7.AutoSize = true;
            this.cb1t7.Enabled = false;
            this.cb1t7.Location = new System.Drawing.Point(40, 56);
            this.cb1t7.Name = "cb1t7";
            this.cb1t7.Size = new System.Drawing.Size(15, 14);
            this.cb1t7.TabIndex = 292;
            this.cb1t7.UseVisualStyleBackColor = true;
            this.cb1t7.CheckedChanged += new System.EventHandler(this.grpPortB_CheckedChanged);
            // 
            // label121
            // 
            this.label121.AutoSize = true;
            this.label121.ForeColor = System.Drawing.Color.Navy;
            this.label121.Location = new System.Drawing.Point(155, 24);
            this.label121.Name = "label121";
            this.label121.Size = new System.Drawing.Size(13, 13);
            this.label121.TabIndex = 291;
            this.label121.Text = "0";
            // 
            // label122
            // 
            this.label122.AutoSize = true;
            this.label122.ForeColor = System.Drawing.Color.Navy;
            this.label122.Location = new System.Drawing.Point(139, 24);
            this.label122.Name = "label122";
            this.label122.Size = new System.Drawing.Size(13, 13);
            this.label122.TabIndex = 290;
            this.label122.Text = "1";
            // 
            // label123
            // 
            this.label123.AutoSize = true;
            this.label123.ForeColor = System.Drawing.Color.Navy;
            this.label123.Location = new System.Drawing.Point(123, 24);
            this.label123.Name = "label123";
            this.label123.Size = new System.Drawing.Size(13, 13);
            this.label123.TabIndex = 289;
            this.label123.Text = "2";
            // 
            // label124
            // 
            this.label124.AutoSize = true;
            this.label124.ForeColor = System.Drawing.Color.Navy;
            this.label124.Location = new System.Drawing.Point(107, 24);
            this.label124.Name = "label124";
            this.label124.Size = new System.Drawing.Size(13, 13);
            this.label124.TabIndex = 288;
            this.label124.Text = "3";
            // 
            // label125
            // 
            this.label125.AutoSize = true;
            this.label125.ForeColor = System.Drawing.Color.Navy;
            this.label125.Location = new System.Drawing.Point(91, 24);
            this.label125.Name = "label125";
            this.label125.Size = new System.Drawing.Size(13, 13);
            this.label125.TabIndex = 287;
            this.label125.Text = "4";
            // 
            // label126
            // 
            this.label126.AutoSize = true;
            this.label126.ForeColor = System.Drawing.Color.Navy;
            this.label126.Location = new System.Drawing.Point(75, 24);
            this.label126.Name = "label126";
            this.label126.Size = new System.Drawing.Size(13, 13);
            this.label126.TabIndex = 286;
            this.label126.Text = "5";
            // 
            // label127
            // 
            this.label127.AutoSize = true;
            this.label127.ForeColor = System.Drawing.Color.Navy;
            this.label127.Location = new System.Drawing.Point(59, 24);
            this.label127.Name = "label127";
            this.label127.Size = new System.Drawing.Size(13, 13);
            this.label127.TabIndex = 285;
            this.label127.Text = "6";
            // 
            // label128
            // 
            this.label128.AutoSize = true;
            this.label128.ForeColor = System.Drawing.Color.Navy;
            this.label128.Location = new System.Drawing.Point(40, 24);
            this.label128.Name = "label128";
            this.label128.Size = new System.Drawing.Size(13, 13);
            this.label128.TabIndex = 284;
            this.label128.Text = "7";
            // 
            // label130
            // 
            this.label130.AutoSize = true;
            this.label130.ForeColor = System.Drawing.Color.Firebrick;
            this.label130.Location = new System.Drawing.Point(13, 40);
            this.label130.Name = "label130";
            this.label130.Size = new System.Drawing.Size(13, 13);
            this.label130.TabIndex = 283;
            this.label130.Text = "0";
            // 
            // grpPortA
            // 
            this.grpPortA.Controls.Add(this.cb15r4);
            this.grpPortA.Controls.Add(this.cb15r7);
            this.grpPortA.Controls.Add(this.cb15r6);
            this.grpPortA.Controls.Add(this.cb15r5);
            this.grpPortA.Controls.Add(this.cb15r3);
            this.grpPortA.Controls.Add(this.cb15r2);
            this.grpPortA.Controls.Add(this.cb15r1);
            this.grpPortA.Controls.Add(this.cb15r0);
            this.grpPortA.Controls.Add(this.label133);
            this.grpPortA.Controls.Add(this.label134);
            this.grpPortA.Controls.Add(this.cb14r0);
            this.grpPortA.Controls.Add(this.cb14r1);
            this.grpPortA.Controls.Add(this.cb14r2);
            this.grpPortA.Controls.Add(this.cb14r3);
            this.grpPortA.Controls.Add(this.cb14r4);
            this.grpPortA.Controls.Add(this.cb14r5);
            this.grpPortA.Controls.Add(this.cb14r6);
            this.grpPortA.Controls.Add(this.cb14r7);
            this.grpPortA.Controls.Add(this.label129);
            this.grpPortA.Controls.Add(this.cb13r4);
            this.grpPortA.Controls.Add(this.label104);
            this.grpPortA.Controls.Add(this.cb13r7);
            this.grpPortA.Controls.Add(this.cb13r6);
            this.grpPortA.Controls.Add(this.cb0r3);
            this.grpPortA.Controls.Add(this.cb13r5);
            this.grpPortA.Controls.Add(this.cb13r3);
            this.grpPortA.Controls.Add(this.cb0r0);
            this.grpPortA.Controls.Add(this.cb13r2);
            this.grpPortA.Controls.Add(this.cb13r1);
            this.grpPortA.Controls.Add(this.cb13r0);
            this.grpPortA.Controls.Add(this.cb0r1);
            this.grpPortA.Controls.Add(this.label102);
            this.grpPortA.Controls.Add(this.cb0r2);
            this.grpPortA.Controls.Add(this.cb0r4);
            this.grpPortA.Controls.Add(this.cb0r5);
            this.grpPortA.Controls.Add(this.cb0r6);
            this.grpPortA.Controls.Add(this.cb0r7);
            this.grpPortA.Controls.Add(this.label101);
            this.grpPortA.Controls.Add(this.cb12r0);
            this.grpPortA.Controls.Add(this.cb12r1);
            this.grpPortA.Controls.Add(this.cb12r2);
            this.grpPortA.Controls.Add(this.cb12r3);
            this.grpPortA.Controls.Add(this.cb12r4);
            this.grpPortA.Controls.Add(this.cb12r5);
            this.grpPortA.Controls.Add(this.cb12r6);
            this.grpPortA.Controls.Add(this.cb12r7);
            this.grpPortA.Controls.Add(this.label100);
            this.grpPortA.Controls.Add(this.cb11r0);
            this.grpPortA.Controls.Add(this.cb11r1);
            this.grpPortA.Controls.Add(this.cb11r2);
            this.grpPortA.Controls.Add(this.cb11r3);
            this.grpPortA.Controls.Add(this.cb11r4);
            this.grpPortA.Controls.Add(this.cb11r5);
            this.grpPortA.Controls.Add(this.cb11r6);
            this.grpPortA.Controls.Add(this.cb11r7);
            this.grpPortA.Controls.Add(this.label99);
            this.grpPortA.Controls.Add(this.cb10r0);
            this.grpPortA.Controls.Add(this.cb10r1);
            this.grpPortA.Controls.Add(this.cb10r2);
            this.grpPortA.Controls.Add(this.cb10r3);
            this.grpPortA.Controls.Add(this.cb10r4);
            this.grpPortA.Controls.Add(this.cb10r5);
            this.grpPortA.Controls.Add(this.cb10r6);
            this.grpPortA.Controls.Add(this.cb10r7);
            this.grpPortA.Controls.Add(this.label98);
            this.grpPortA.Controls.Add(this.cb9r0);
            this.grpPortA.Controls.Add(this.cb9r1);
            this.grpPortA.Controls.Add(this.cb9r2);
            this.grpPortA.Controls.Add(this.cb9r3);
            this.grpPortA.Controls.Add(this.cb9r4);
            this.grpPortA.Controls.Add(this.cb9r5);
            this.grpPortA.Controls.Add(this.cb9r6);
            this.grpPortA.Controls.Add(this.cb9r7);
            this.grpPortA.Controls.Add(this.label97);
            this.grpPortA.Controls.Add(this.cb8r0);
            this.grpPortA.Controls.Add(this.cb8r1);
            this.grpPortA.Controls.Add(this.cb8r2);
            this.grpPortA.Controls.Add(this.cb8r3);
            this.grpPortA.Controls.Add(this.cb8r4);
            this.grpPortA.Controls.Add(this.cb8r5);
            this.grpPortA.Controls.Add(this.cb8r6);
            this.grpPortA.Controls.Add(this.cb8r7);
            this.grpPortA.Controls.Add(this.label96);
            this.grpPortA.Controls.Add(this.cb7r0);
            this.grpPortA.Controls.Add(this.cb7r1);
            this.grpPortA.Controls.Add(this.cb7r2);
            this.grpPortA.Controls.Add(this.cb7r3);
            this.grpPortA.Controls.Add(this.cb7r4);
            this.grpPortA.Controls.Add(this.cb7r5);
            this.grpPortA.Controls.Add(this.cb7r6);
            this.grpPortA.Controls.Add(this.cb7r7);
            this.grpPortA.Controls.Add(this.label95);
            this.grpPortA.Controls.Add(this.cb6r0);
            this.grpPortA.Controls.Add(this.cb6r1);
            this.grpPortA.Controls.Add(this.cb6r2);
            this.grpPortA.Controls.Add(this.cb6r3);
            this.grpPortA.Controls.Add(this.cb6r4);
            this.grpPortA.Controls.Add(this.cb6r5);
            this.grpPortA.Controls.Add(this.cb6r6);
            this.grpPortA.Controls.Add(this.cb6r7);
            this.grpPortA.Controls.Add(this.label94);
            this.grpPortA.Controls.Add(this.cb5r0);
            this.grpPortA.Controls.Add(this.cb5r1);
            this.grpPortA.Controls.Add(this.cb5r2);
            this.grpPortA.Controls.Add(this.cb5r3);
            this.grpPortA.Controls.Add(this.cb5r4);
            this.grpPortA.Controls.Add(this.cb5r5);
            this.grpPortA.Controls.Add(this.cb5r6);
            this.grpPortA.Controls.Add(this.cb5r7);
            this.grpPortA.Controls.Add(this.label93);
            this.grpPortA.Controls.Add(this.cb4r0);
            this.grpPortA.Controls.Add(this.cb4r1);
            this.grpPortA.Controls.Add(this.cb4r2);
            this.grpPortA.Controls.Add(this.cb4r3);
            this.grpPortA.Controls.Add(this.cb4r4);
            this.grpPortA.Controls.Add(this.cb4r5);
            this.grpPortA.Controls.Add(this.cb4r6);
            this.grpPortA.Controls.Add(this.cb4r7);
            this.grpPortA.Controls.Add(this.label92);
            this.grpPortA.Controls.Add(this.cb3r0);
            this.grpPortA.Controls.Add(this.cb3r1);
            this.grpPortA.Controls.Add(this.cb3r2);
            this.grpPortA.Controls.Add(this.cb3r3);
            this.grpPortA.Controls.Add(this.cb3r4);
            this.grpPortA.Controls.Add(this.cb3r5);
            this.grpPortA.Controls.Add(this.cb3r6);
            this.grpPortA.Controls.Add(this.cb3r7);
            this.grpPortA.Controls.Add(this.label91);
            this.grpPortA.Controls.Add(this.cb2r0);
            this.grpPortA.Controls.Add(this.cb2r1);
            this.grpPortA.Controls.Add(this.cb2r2);
            this.grpPortA.Controls.Add(this.cb2r3);
            this.grpPortA.Controls.Add(this.cb2r4);
            this.grpPortA.Controls.Add(this.cb2r5);
            this.grpPortA.Controls.Add(this.cb2r6);
            this.grpPortA.Controls.Add(this.cb2r7);
            this.grpPortA.Controls.Add(this.label90);
            this.grpPortA.Controls.Add(this.cb1r0);
            this.grpPortA.Controls.Add(this.cb1r1);
            this.grpPortA.Controls.Add(this.cb1r2);
            this.grpPortA.Controls.Add(this.cb1r3);
            this.grpPortA.Controls.Add(this.cb1r4);
            this.grpPortA.Controls.Add(this.cb1r5);
            this.grpPortA.Controls.Add(this.cb1r6);
            this.grpPortA.Controls.Add(this.cb1r7);
            this.grpPortA.Controls.Add(this.label89);
            this.grpPortA.Controls.Add(this.label88);
            this.grpPortA.Controls.Add(this.label87);
            this.grpPortA.Controls.Add(this.label86);
            this.grpPortA.Controls.Add(this.label85);
            this.grpPortA.Controls.Add(this.label84);
            this.grpPortA.Controls.Add(this.label83);
            this.grpPortA.Controls.Add(this.label82);
            this.grpPortA.Controls.Add(this.label81);
            this.grpPortA.ForeColor = System.Drawing.Color.Blue;
            this.grpPortA.Location = new System.Drawing.Point(5, 6);
            this.grpPortA.Name = "grpPortA";
            this.grpPortA.Size = new System.Drawing.Size(175, 292);
            this.grpPortA.TabIndex = 0;
            this.grpPortA.TabStop = false;
            this.grpPortA.Text = "Port A";
            // 
            // cb15r4
            // 
            this.cb15r4.AutoSize = true;
            this.cb15r4.Enabled = false;
            this.cb15r4.Location = new System.Drawing.Point(90, 272);
            this.cb15r4.Name = "cb15r4";
            this.cb15r4.Size = new System.Drawing.Size(15, 14);
            this.cb15r4.TabIndex = 151;
            this.cb15r4.UseVisualStyleBackColor = true;
            this.cb15r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb15r7
            // 
            this.cb15r7.AutoSize = true;
            this.cb15r7.Enabled = false;
            this.cb15r7.Location = new System.Drawing.Point(41, 272);
            this.cb15r7.Name = "cb15r7";
            this.cb15r7.Size = new System.Drawing.Size(15, 14);
            this.cb15r7.TabIndex = 148;
            this.cb15r7.UseVisualStyleBackColor = true;
            this.cb15r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb15r6
            // 
            this.cb15r6.AutoSize = true;
            this.cb15r6.Enabled = false;
            this.cb15r6.Location = new System.Drawing.Point(58, 272);
            this.cb15r6.Name = "cb15r6";
            this.cb15r6.Size = new System.Drawing.Size(15, 14);
            this.cb15r6.TabIndex = 149;
            this.cb15r6.UseVisualStyleBackColor = true;
            this.cb15r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb15r5
            // 
            this.cb15r5.AutoSize = true;
            this.cb15r5.Enabled = false;
            this.cb15r5.Location = new System.Drawing.Point(74, 272);
            this.cb15r5.Name = "cb15r5";
            this.cb15r5.Size = new System.Drawing.Size(15, 14);
            this.cb15r5.TabIndex = 150;
            this.cb15r5.UseVisualStyleBackColor = true;
            this.cb15r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb15r3
            // 
            this.cb15r3.AutoSize = true;
            this.cb15r3.Enabled = false;
            this.cb15r3.Location = new System.Drawing.Point(106, 272);
            this.cb15r3.Name = "cb15r3";
            this.cb15r3.Size = new System.Drawing.Size(15, 14);
            this.cb15r3.TabIndex = 152;
            this.cb15r3.UseVisualStyleBackColor = true;
            this.cb15r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb15r2
            // 
            this.cb15r2.AutoSize = true;
            this.cb15r2.Enabled = false;
            this.cb15r2.Location = new System.Drawing.Point(122, 272);
            this.cb15r2.Name = "cb15r2";
            this.cb15r2.Size = new System.Drawing.Size(15, 14);
            this.cb15r2.TabIndex = 153;
            this.cb15r2.UseVisualStyleBackColor = true;
            this.cb15r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb15r1
            // 
            this.cb15r1.AutoSize = true;
            this.cb15r1.Enabled = false;
            this.cb15r1.Location = new System.Drawing.Point(138, 272);
            this.cb15r1.Name = "cb15r1";
            this.cb15r1.Size = new System.Drawing.Size(15, 14);
            this.cb15r1.TabIndex = 154;
            this.cb15r1.UseVisualStyleBackColor = true;
            this.cb15r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb15r0
            // 
            this.cb15r0.AutoSize = true;
            this.cb15r0.Enabled = false;
            this.cb15r0.Location = new System.Drawing.Point(154, 272);
            this.cb15r0.Name = "cb15r0";
            this.cb15r0.Size = new System.Drawing.Size(15, 14);
            this.cb15r0.TabIndex = 155;
            this.cb15r0.UseVisualStyleBackColor = true;
            this.cb15r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label133
            // 
            this.label133.AutoSize = true;
            this.label133.ForeColor = System.Drawing.Color.Green;
            this.label133.Location = new System.Drawing.Point(8, 257);
            this.label133.Name = "label133";
            this.label133.Size = new System.Drawing.Size(27, 13);
            this.label133.TabIndex = 156;
            this.label133.Text = "HF6";
            // 
            // label134
            // 
            this.label134.AutoSize = true;
            this.label134.ForeColor = System.Drawing.Color.Green;
            this.label134.Location = new System.Drawing.Point(8, 272);
            this.label134.Name = "label134";
            this.label134.Size = new System.Drawing.Size(27, 13);
            this.label134.TabIndex = 147;
            this.label134.Text = "HF2";
            // 
            // cb14r0
            // 
            this.cb14r0.AutoSize = true;
            this.cb14r0.Enabled = false;
            this.cb14r0.Location = new System.Drawing.Point(154, 257);
            this.cb14r0.Name = "cb14r0";
            this.cb14r0.Size = new System.Drawing.Size(15, 14);
            this.cb14r0.TabIndex = 146;
            this.cb14r0.UseVisualStyleBackColor = true;
            this.cb14r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb14r1
            // 
            this.cb14r1.AutoSize = true;
            this.cb14r1.Enabled = false;
            this.cb14r1.Location = new System.Drawing.Point(138, 257);
            this.cb14r1.Name = "cb14r1";
            this.cb14r1.Size = new System.Drawing.Size(15, 14);
            this.cb14r1.TabIndex = 145;
            this.cb14r1.UseVisualStyleBackColor = true;
            this.cb14r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb14r2
            // 
            this.cb14r2.AutoSize = true;
            this.cb14r2.Enabled = false;
            this.cb14r2.Location = new System.Drawing.Point(122, 257);
            this.cb14r2.Name = "cb14r2";
            this.cb14r2.Size = new System.Drawing.Size(15, 14);
            this.cb14r2.TabIndex = 144;
            this.cb14r2.UseVisualStyleBackColor = true;
            this.cb14r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb14r3
            // 
            this.cb14r3.AutoSize = true;
            this.cb14r3.Enabled = false;
            this.cb14r3.Location = new System.Drawing.Point(106, 257);
            this.cb14r3.Name = "cb14r3";
            this.cb14r3.Size = new System.Drawing.Size(15, 14);
            this.cb14r3.TabIndex = 143;
            this.cb14r3.UseVisualStyleBackColor = true;
            this.cb14r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb14r4
            // 
            this.cb14r4.AutoSize = true;
            this.cb14r4.Enabled = false;
            this.cb14r4.Location = new System.Drawing.Point(90, 257);
            this.cb14r4.Name = "cb14r4";
            this.cb14r4.Size = new System.Drawing.Size(15, 14);
            this.cb14r4.TabIndex = 142;
            this.cb14r4.UseVisualStyleBackColor = true;
            this.cb14r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb14r5
            // 
            this.cb14r5.AutoSize = true;
            this.cb14r5.Enabled = false;
            this.cb14r5.Location = new System.Drawing.Point(74, 257);
            this.cb14r5.Name = "cb14r5";
            this.cb14r5.Size = new System.Drawing.Size(15, 14);
            this.cb14r5.TabIndex = 141;
            this.cb14r5.UseVisualStyleBackColor = true;
            this.cb14r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb14r6
            // 
            this.cb14r6.AutoSize = true;
            this.cb14r6.Enabled = false;
            this.cb14r6.Location = new System.Drawing.Point(58, 257);
            this.cb14r6.Name = "cb14r6";
            this.cb14r6.Size = new System.Drawing.Size(15, 14);
            this.cb14r6.TabIndex = 140;
            this.cb14r6.UseVisualStyleBackColor = true;
            this.cb14r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb14r7
            // 
            this.cb14r7.AutoSize = true;
            this.cb14r7.Enabled = false;
            this.cb14r7.Location = new System.Drawing.Point(41, 257);
            this.cb14r7.Name = "cb14r7";
            this.cb14r7.Size = new System.Drawing.Size(15, 14);
            this.cb14r7.TabIndex = 139;
            this.cb14r7.UseVisualStyleBackColor = true;
            this.cb14r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label129
            // 
            this.label129.AutoSize = true;
            this.label129.ForeColor = System.Drawing.Color.Firebrick;
            this.label129.Location = new System.Drawing.Point(3, 24);
            this.label129.Name = "label129";
            this.label129.Size = new System.Drawing.Size(34, 13);
            this.label129.TabIndex = 138;
            this.label129.Text = "VHF+";
            // 
            // cb13r4
            // 
            this.cb13r4.AutoSize = true;
            this.cb13r4.Enabled = false;
            this.cb13r4.Location = new System.Drawing.Point(90, 237);
            this.cb13r4.Name = "cb13r4";
            this.cb13r4.Size = new System.Drawing.Size(15, 14);
            this.cb13r4.TabIndex = 128;
            this.cb13r4.UseVisualStyleBackColor = true;
            this.cb13r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label104
            // 
            this.label104.AutoSize = true;
            this.label104.ForeColor = System.Drawing.Color.Navy;
            this.label104.Location = new System.Drawing.Point(40, 8);
            this.label104.Name = "label104";
            this.label104.Size = new System.Drawing.Size(130, 13);
            this.label104.TabIndex = 137;
            this.label104.Text = "- - - - - - - Port Bits - - - - - - -";
            // 
            // cb13r7
            // 
            this.cb13r7.AutoSize = true;
            this.cb13r7.Enabled = false;
            this.cb13r7.Location = new System.Drawing.Point(41, 237);
            this.cb13r7.Name = "cb13r7";
            this.cb13r7.Size = new System.Drawing.Size(15, 14);
            this.cb13r7.TabIndex = 125;
            this.cb13r7.UseVisualStyleBackColor = true;
            this.cb13r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb13r6
            // 
            this.cb13r6.AutoSize = true;
            this.cb13r6.Enabled = false;
            this.cb13r6.Location = new System.Drawing.Point(58, 237);
            this.cb13r6.Name = "cb13r6";
            this.cb13r6.Size = new System.Drawing.Size(15, 14);
            this.cb13r6.TabIndex = 126;
            this.cb13r6.UseVisualStyleBackColor = true;
            this.cb13r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb0r3
            // 
            this.cb0r3.AutoSize = true;
            this.cb0r3.Enabled = false;
            this.cb0r3.Location = new System.Drawing.Point(106, 40);
            this.cb0r3.Name = "cb0r3";
            this.cb0r3.Size = new System.Drawing.Size(15, 14);
            this.cb0r3.TabIndex = 3;
            this.cb0r3.UseVisualStyleBackColor = true;
            this.cb0r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb13r5
            // 
            this.cb13r5.AutoSize = true;
            this.cb13r5.Enabled = false;
            this.cb13r5.Location = new System.Drawing.Point(74, 237);
            this.cb13r5.Name = "cb13r5";
            this.cb13r5.Size = new System.Drawing.Size(15, 14);
            this.cb13r5.TabIndex = 127;
            this.cb13r5.UseVisualStyleBackColor = true;
            this.cb13r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb13r3
            // 
            this.cb13r3.AutoSize = true;
            this.cb13r3.Enabled = false;
            this.cb13r3.Location = new System.Drawing.Point(106, 237);
            this.cb13r3.Name = "cb13r3";
            this.cb13r3.Size = new System.Drawing.Size(15, 14);
            this.cb13r3.TabIndex = 129;
            this.cb13r3.UseVisualStyleBackColor = true;
            this.cb13r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb0r0
            // 
            this.cb0r0.AutoSize = true;
            this.cb0r0.Enabled = false;
            this.cb0r0.Location = new System.Drawing.Point(154, 40);
            this.cb0r0.Name = "cb0r0";
            this.cb0r0.Size = new System.Drawing.Size(15, 14);
            this.cb0r0.TabIndex = 0;
            this.cb0r0.UseVisualStyleBackColor = true;
            this.cb0r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb13r2
            // 
            this.cb13r2.AutoSize = true;
            this.cb13r2.Enabled = false;
            this.cb13r2.Location = new System.Drawing.Point(122, 237);
            this.cb13r2.Name = "cb13r2";
            this.cb13r2.Size = new System.Drawing.Size(15, 14);
            this.cb13r2.TabIndex = 130;
            this.cb13r2.UseVisualStyleBackColor = true;
            this.cb13r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb13r1
            // 
            this.cb13r1.AutoSize = true;
            this.cb13r1.Enabled = false;
            this.cb13r1.Location = new System.Drawing.Point(138, 237);
            this.cb13r1.Name = "cb13r1";
            this.cb13r1.Size = new System.Drawing.Size(15, 14);
            this.cb13r1.TabIndex = 131;
            this.cb13r1.UseVisualStyleBackColor = true;
            this.cb13r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb13r0
            // 
            this.cb13r0.AutoSize = true;
            this.cb13r0.Enabled = false;
            this.cb13r0.Location = new System.Drawing.Point(154, 237);
            this.cb13r0.Name = "cb13r0";
            this.cb13r0.Size = new System.Drawing.Size(15, 14);
            this.cb13r0.TabIndex = 132;
            this.cb13r0.UseVisualStyleBackColor = true;
            this.cb13r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb0r1
            // 
            this.cb0r1.AutoSize = true;
            this.cb0r1.Enabled = false;
            this.cb0r1.Location = new System.Drawing.Point(138, 40);
            this.cb0r1.Name = "cb0r1";
            this.cb0r1.Size = new System.Drawing.Size(15, 14);
            this.cb0r1.TabIndex = 1;
            this.cb0r1.UseVisualStyleBackColor = true;
            this.cb0r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label102
            // 
            this.label102.AutoSize = true;
            this.label102.ForeColor = System.Drawing.Color.Firebrick;
            this.label102.Location = new System.Drawing.Point(8, 222);
            this.label102.Name = "label102";
            this.label102.Size = new System.Drawing.Size(19, 13);
            this.label102.TabIndex = 133;
            this.label102.Text = "12";
            // 
            // cb0r2
            // 
            this.cb0r2.AutoSize = true;
            this.cb0r2.Enabled = false;
            this.cb0r2.Location = new System.Drawing.Point(122, 40);
            this.cb0r2.Name = "cb0r2";
            this.cb0r2.Size = new System.Drawing.Size(15, 14);
            this.cb0r2.TabIndex = 2;
            this.cb0r2.UseVisualStyleBackColor = true;
            this.cb0r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb0r4
            // 
            this.cb0r4.AutoSize = true;
            this.cb0r4.Enabled = false;
            this.cb0r4.Location = new System.Drawing.Point(91, 40);
            this.cb0r4.Name = "cb0r4";
            this.cb0r4.Size = new System.Drawing.Size(15, 14);
            this.cb0r4.TabIndex = 4;
            this.cb0r4.UseVisualStyleBackColor = true;
            this.cb0r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb0r5
            // 
            this.cb0r5.AutoSize = true;
            this.cb0r5.Enabled = false;
            this.cb0r5.Location = new System.Drawing.Point(74, 40);
            this.cb0r5.Name = "cb0r5";
            this.cb0r5.Size = new System.Drawing.Size(15, 14);
            this.cb0r5.TabIndex = 5;
            this.cb0r5.UseVisualStyleBackColor = true;
            this.cb0r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb0r6
            // 
            this.cb0r6.AutoSize = true;
            this.cb0r6.Enabled = false;
            this.cb0r6.Location = new System.Drawing.Point(58, 40);
            this.cb0r6.Name = "cb0r6";
            this.cb0r6.Size = new System.Drawing.Size(15, 14);
            this.cb0r6.TabIndex = 6;
            this.cb0r6.UseVisualStyleBackColor = true;
            this.cb0r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb0r7
            // 
            this.cb0r7.AutoSize = true;
            this.cb0r7.Enabled = false;
            this.cb0r7.Location = new System.Drawing.Point(41, 40);
            this.cb0r7.Name = "cb0r7";
            this.cb0r7.Size = new System.Drawing.Size(15, 14);
            this.cb0r7.TabIndex = 7;
            this.cb0r7.UseVisualStyleBackColor = true;
            this.cb0r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label101
            // 
            this.label101.AutoSize = true;
            this.label101.ForeColor = System.Drawing.Color.Firebrick;
            this.label101.Location = new System.Drawing.Point(8, 237);
            this.label101.Name = "label101";
            this.label101.Size = new System.Drawing.Size(19, 13);
            this.label101.TabIndex = 124;
            this.label101.Text = "13";
            // 
            // cb12r0
            // 
            this.cb12r0.AutoSize = true;
            this.cb12r0.Enabled = false;
            this.cb12r0.Location = new System.Drawing.Point(154, 222);
            this.cb12r0.Name = "cb12r0";
            this.cb12r0.Size = new System.Drawing.Size(15, 14);
            this.cb12r0.TabIndex = 123;
            this.cb12r0.UseVisualStyleBackColor = true;
            this.cb12r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb12r1
            // 
            this.cb12r1.AutoSize = true;
            this.cb12r1.Enabled = false;
            this.cb12r1.Location = new System.Drawing.Point(138, 222);
            this.cb12r1.Name = "cb12r1";
            this.cb12r1.Size = new System.Drawing.Size(15, 14);
            this.cb12r1.TabIndex = 122;
            this.cb12r1.UseVisualStyleBackColor = true;
            this.cb12r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb12r2
            // 
            this.cb12r2.AutoSize = true;
            this.cb12r2.Enabled = false;
            this.cb12r2.Location = new System.Drawing.Point(122, 222);
            this.cb12r2.Name = "cb12r2";
            this.cb12r2.Size = new System.Drawing.Size(15, 14);
            this.cb12r2.TabIndex = 121;
            this.cb12r2.UseVisualStyleBackColor = true;
            this.cb12r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb12r3
            // 
            this.cb12r3.AutoSize = true;
            this.cb12r3.Enabled = false;
            this.cb12r3.Location = new System.Drawing.Point(106, 222);
            this.cb12r3.Name = "cb12r3";
            this.cb12r3.Size = new System.Drawing.Size(15, 14);
            this.cb12r3.TabIndex = 120;
            this.cb12r3.UseVisualStyleBackColor = true;
            this.cb12r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb12r4
            // 
            this.cb12r4.AutoSize = true;
            this.cb12r4.Enabled = false;
            this.cb12r4.Location = new System.Drawing.Point(90, 222);
            this.cb12r4.Name = "cb12r4";
            this.cb12r4.Size = new System.Drawing.Size(15, 14);
            this.cb12r4.TabIndex = 119;
            this.cb12r4.UseVisualStyleBackColor = true;
            this.cb12r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb12r5
            // 
            this.cb12r5.AutoSize = true;
            this.cb12r5.Enabled = false;
            this.cb12r5.Location = new System.Drawing.Point(74, 222);
            this.cb12r5.Name = "cb12r5";
            this.cb12r5.Size = new System.Drawing.Size(15, 14);
            this.cb12r5.TabIndex = 118;
            this.cb12r5.UseVisualStyleBackColor = true;
            this.cb12r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb12r6
            // 
            this.cb12r6.AutoSize = true;
            this.cb12r6.Enabled = false;
            this.cb12r6.Location = new System.Drawing.Point(58, 222);
            this.cb12r6.Name = "cb12r6";
            this.cb12r6.Size = new System.Drawing.Size(15, 14);
            this.cb12r6.TabIndex = 117;
            this.cb12r6.UseVisualStyleBackColor = true;
            this.cb12r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb12r7
            // 
            this.cb12r7.AutoSize = true;
            this.cb12r7.Enabled = false;
            this.cb12r7.Location = new System.Drawing.Point(41, 222);
            this.cb12r7.Name = "cb12r7";
            this.cb12r7.Size = new System.Drawing.Size(15, 14);
            this.cb12r7.TabIndex = 116;
            this.cb12r7.UseVisualStyleBackColor = true;
            this.cb12r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label100
            // 
            this.label100.AutoSize = true;
            this.label100.ForeColor = System.Drawing.Color.Firebrick;
            this.label100.Location = new System.Drawing.Point(8, 207);
            this.label100.Name = "label100";
            this.label100.Size = new System.Drawing.Size(19, 13);
            this.label100.TabIndex = 115;
            this.label100.Text = "11";
            // 
            // cb11r0
            // 
            this.cb11r0.AutoSize = true;
            this.cb11r0.Enabled = false;
            this.cb11r0.Location = new System.Drawing.Point(154, 207);
            this.cb11r0.Name = "cb11r0";
            this.cb11r0.Size = new System.Drawing.Size(15, 14);
            this.cb11r0.TabIndex = 114;
            this.cb11r0.UseVisualStyleBackColor = true;
            this.cb11r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb11r1
            // 
            this.cb11r1.AutoSize = true;
            this.cb11r1.Enabled = false;
            this.cb11r1.Location = new System.Drawing.Point(138, 207);
            this.cb11r1.Name = "cb11r1";
            this.cb11r1.Size = new System.Drawing.Size(15, 14);
            this.cb11r1.TabIndex = 113;
            this.cb11r1.UseVisualStyleBackColor = true;
            this.cb11r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb11r2
            // 
            this.cb11r2.AutoSize = true;
            this.cb11r2.Enabled = false;
            this.cb11r2.Location = new System.Drawing.Point(122, 207);
            this.cb11r2.Name = "cb11r2";
            this.cb11r2.Size = new System.Drawing.Size(15, 14);
            this.cb11r2.TabIndex = 112;
            this.cb11r2.UseVisualStyleBackColor = true;
            this.cb11r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb11r3
            // 
            this.cb11r3.AutoSize = true;
            this.cb11r3.Enabled = false;
            this.cb11r3.Location = new System.Drawing.Point(106, 207);
            this.cb11r3.Name = "cb11r3";
            this.cb11r3.Size = new System.Drawing.Size(15, 14);
            this.cb11r3.TabIndex = 111;
            this.cb11r3.UseVisualStyleBackColor = true;
            this.cb11r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb11r4
            // 
            this.cb11r4.AutoSize = true;
            this.cb11r4.Enabled = false;
            this.cb11r4.Location = new System.Drawing.Point(90, 207);
            this.cb11r4.Name = "cb11r4";
            this.cb11r4.Size = new System.Drawing.Size(15, 14);
            this.cb11r4.TabIndex = 110;
            this.cb11r4.UseVisualStyleBackColor = true;
            this.cb11r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb11r5
            // 
            this.cb11r5.AutoSize = true;
            this.cb11r5.Enabled = false;
            this.cb11r5.Location = new System.Drawing.Point(74, 207);
            this.cb11r5.Name = "cb11r5";
            this.cb11r5.Size = new System.Drawing.Size(15, 14);
            this.cb11r5.TabIndex = 109;
            this.cb11r5.UseVisualStyleBackColor = true;
            this.cb11r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb11r6
            // 
            this.cb11r6.AutoSize = true;
            this.cb11r6.Enabled = false;
            this.cb11r6.Location = new System.Drawing.Point(58, 207);
            this.cb11r6.Name = "cb11r6";
            this.cb11r6.Size = new System.Drawing.Size(15, 14);
            this.cb11r6.TabIndex = 108;
            this.cb11r6.UseVisualStyleBackColor = true;
            this.cb11r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb11r7
            // 
            this.cb11r7.AutoSize = true;
            this.cb11r7.Enabled = false;
            this.cb11r7.Location = new System.Drawing.Point(41, 207);
            this.cb11r7.Name = "cb11r7";
            this.cb11r7.Size = new System.Drawing.Size(15, 14);
            this.cb11r7.TabIndex = 107;
            this.cb11r7.UseVisualStyleBackColor = true;
            this.cb11r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label99
            // 
            this.label99.AutoSize = true;
            this.label99.ForeColor = System.Drawing.Color.Firebrick;
            this.label99.Location = new System.Drawing.Point(8, 192);
            this.label99.Name = "label99";
            this.label99.Size = new System.Drawing.Size(19, 13);
            this.label99.TabIndex = 106;
            this.label99.Text = "10";
            // 
            // cb10r0
            // 
            this.cb10r0.AutoSize = true;
            this.cb10r0.Enabled = false;
            this.cb10r0.Location = new System.Drawing.Point(154, 192);
            this.cb10r0.Name = "cb10r0";
            this.cb10r0.Size = new System.Drawing.Size(15, 14);
            this.cb10r0.TabIndex = 105;
            this.cb10r0.UseVisualStyleBackColor = true;
            this.cb10r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb10r1
            // 
            this.cb10r1.AutoSize = true;
            this.cb10r1.Enabled = false;
            this.cb10r1.Location = new System.Drawing.Point(138, 192);
            this.cb10r1.Name = "cb10r1";
            this.cb10r1.Size = new System.Drawing.Size(15, 14);
            this.cb10r1.TabIndex = 104;
            this.cb10r1.UseVisualStyleBackColor = true;
            this.cb10r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb10r2
            // 
            this.cb10r2.AutoSize = true;
            this.cb10r2.Enabled = false;
            this.cb10r2.Location = new System.Drawing.Point(122, 192);
            this.cb10r2.Name = "cb10r2";
            this.cb10r2.Size = new System.Drawing.Size(15, 14);
            this.cb10r2.TabIndex = 103;
            this.cb10r2.UseVisualStyleBackColor = true;
            this.cb10r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb10r3
            // 
            this.cb10r3.AutoSize = true;
            this.cb10r3.Enabled = false;
            this.cb10r3.Location = new System.Drawing.Point(106, 192);
            this.cb10r3.Name = "cb10r3";
            this.cb10r3.Size = new System.Drawing.Size(15, 14);
            this.cb10r3.TabIndex = 102;
            this.cb10r3.UseVisualStyleBackColor = true;
            this.cb10r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb10r4
            // 
            this.cb10r4.AutoSize = true;
            this.cb10r4.Enabled = false;
            this.cb10r4.Location = new System.Drawing.Point(90, 192);
            this.cb10r4.Name = "cb10r4";
            this.cb10r4.Size = new System.Drawing.Size(15, 14);
            this.cb10r4.TabIndex = 101;
            this.cb10r4.UseVisualStyleBackColor = true;
            this.cb10r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb10r5
            // 
            this.cb10r5.AutoSize = true;
            this.cb10r5.Enabled = false;
            this.cb10r5.Location = new System.Drawing.Point(74, 192);
            this.cb10r5.Name = "cb10r5";
            this.cb10r5.Size = new System.Drawing.Size(15, 14);
            this.cb10r5.TabIndex = 100;
            this.cb10r5.UseVisualStyleBackColor = true;
            this.cb10r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb10r6
            // 
            this.cb10r6.AutoSize = true;
            this.cb10r6.Enabled = false;
            this.cb10r6.Location = new System.Drawing.Point(58, 192);
            this.cb10r6.Name = "cb10r6";
            this.cb10r6.Size = new System.Drawing.Size(15, 14);
            this.cb10r6.TabIndex = 99;
            this.cb10r6.UseVisualStyleBackColor = true;
            this.cb10r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb10r7
            // 
            this.cb10r7.AutoSize = true;
            this.cb10r7.Enabled = false;
            this.cb10r7.Location = new System.Drawing.Point(41, 192);
            this.cb10r7.Name = "cb10r7";
            this.cb10r7.Size = new System.Drawing.Size(15, 14);
            this.cb10r7.TabIndex = 98;
            this.cb10r7.UseVisualStyleBackColor = true;
            this.cb10r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label98
            // 
            this.label98.AutoSize = true;
            this.label98.ForeColor = System.Drawing.Color.Firebrick;
            this.label98.Location = new System.Drawing.Point(14, 177);
            this.label98.Name = "label98";
            this.label98.Size = new System.Drawing.Size(13, 13);
            this.label98.TabIndex = 97;
            this.label98.Text = "9";
            // 
            // cb9r0
            // 
            this.cb9r0.AutoSize = true;
            this.cb9r0.Enabled = false;
            this.cb9r0.Location = new System.Drawing.Point(154, 177);
            this.cb9r0.Name = "cb9r0";
            this.cb9r0.Size = new System.Drawing.Size(15, 14);
            this.cb9r0.TabIndex = 96;
            this.cb9r0.UseVisualStyleBackColor = true;
            this.cb9r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb9r1
            // 
            this.cb9r1.AutoSize = true;
            this.cb9r1.Enabled = false;
            this.cb9r1.Location = new System.Drawing.Point(138, 177);
            this.cb9r1.Name = "cb9r1";
            this.cb9r1.Size = new System.Drawing.Size(15, 14);
            this.cb9r1.TabIndex = 95;
            this.cb9r1.UseVisualStyleBackColor = true;
            this.cb9r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb9r2
            // 
            this.cb9r2.AutoSize = true;
            this.cb9r2.Enabled = false;
            this.cb9r2.Location = new System.Drawing.Point(122, 177);
            this.cb9r2.Name = "cb9r2";
            this.cb9r2.Size = new System.Drawing.Size(15, 14);
            this.cb9r2.TabIndex = 94;
            this.cb9r2.UseVisualStyleBackColor = true;
            this.cb9r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb9r3
            // 
            this.cb9r3.AutoSize = true;
            this.cb9r3.Enabled = false;
            this.cb9r3.Location = new System.Drawing.Point(106, 177);
            this.cb9r3.Name = "cb9r3";
            this.cb9r3.Size = new System.Drawing.Size(15, 14);
            this.cb9r3.TabIndex = 93;
            this.cb9r3.UseVisualStyleBackColor = true;
            this.cb9r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb9r4
            // 
            this.cb9r4.AutoSize = true;
            this.cb9r4.Enabled = false;
            this.cb9r4.Location = new System.Drawing.Point(90, 177);
            this.cb9r4.Name = "cb9r4";
            this.cb9r4.Size = new System.Drawing.Size(15, 14);
            this.cb9r4.TabIndex = 92;
            this.cb9r4.UseVisualStyleBackColor = true;
            this.cb9r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb9r5
            // 
            this.cb9r5.AutoSize = true;
            this.cb9r5.Enabled = false;
            this.cb9r5.Location = new System.Drawing.Point(74, 177);
            this.cb9r5.Name = "cb9r5";
            this.cb9r5.Size = new System.Drawing.Size(15, 14);
            this.cb9r5.TabIndex = 91;
            this.cb9r5.UseVisualStyleBackColor = true;
            this.cb9r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb9r6
            // 
            this.cb9r6.AutoSize = true;
            this.cb9r6.Enabled = false;
            this.cb9r6.Location = new System.Drawing.Point(58, 177);
            this.cb9r6.Name = "cb9r6";
            this.cb9r6.Size = new System.Drawing.Size(15, 14);
            this.cb9r6.TabIndex = 90;
            this.cb9r6.UseVisualStyleBackColor = true;
            this.cb9r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb9r7
            // 
            this.cb9r7.AutoSize = true;
            this.cb9r7.Enabled = false;
            this.cb9r7.Location = new System.Drawing.Point(41, 177);
            this.cb9r7.Name = "cb9r7";
            this.cb9r7.Size = new System.Drawing.Size(15, 14);
            this.cb9r7.TabIndex = 89;
            this.cb9r7.UseVisualStyleBackColor = true;
            this.cb9r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label97
            // 
            this.label97.AutoSize = true;
            this.label97.ForeColor = System.Drawing.Color.Firebrick;
            this.label97.Location = new System.Drawing.Point(14, 162);
            this.label97.Name = "label97";
            this.label97.Size = new System.Drawing.Size(13, 13);
            this.label97.TabIndex = 88;
            this.label97.Text = "8";
            // 
            // cb8r0
            // 
            this.cb8r0.AutoSize = true;
            this.cb8r0.Enabled = false;
            this.cb8r0.Location = new System.Drawing.Point(154, 162);
            this.cb8r0.Name = "cb8r0";
            this.cb8r0.Size = new System.Drawing.Size(15, 14);
            this.cb8r0.TabIndex = 87;
            this.cb8r0.UseVisualStyleBackColor = true;
            this.cb8r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb8r1
            // 
            this.cb8r1.AutoSize = true;
            this.cb8r1.Enabled = false;
            this.cb8r1.Location = new System.Drawing.Point(138, 162);
            this.cb8r1.Name = "cb8r1";
            this.cb8r1.Size = new System.Drawing.Size(15, 14);
            this.cb8r1.TabIndex = 86;
            this.cb8r1.UseVisualStyleBackColor = true;
            this.cb8r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb8r2
            // 
            this.cb8r2.AutoSize = true;
            this.cb8r2.Enabled = false;
            this.cb8r2.Location = new System.Drawing.Point(122, 162);
            this.cb8r2.Name = "cb8r2";
            this.cb8r2.Size = new System.Drawing.Size(15, 14);
            this.cb8r2.TabIndex = 85;
            this.cb8r2.UseVisualStyleBackColor = true;
            this.cb8r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb8r3
            // 
            this.cb8r3.AutoSize = true;
            this.cb8r3.Enabled = false;
            this.cb8r3.Location = new System.Drawing.Point(106, 162);
            this.cb8r3.Name = "cb8r3";
            this.cb8r3.Size = new System.Drawing.Size(15, 14);
            this.cb8r3.TabIndex = 84;
            this.cb8r3.UseVisualStyleBackColor = true;
            this.cb8r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb8r4
            // 
            this.cb8r4.AutoSize = true;
            this.cb8r4.Enabled = false;
            this.cb8r4.Location = new System.Drawing.Point(90, 162);
            this.cb8r4.Name = "cb8r4";
            this.cb8r4.Size = new System.Drawing.Size(15, 14);
            this.cb8r4.TabIndex = 83;
            this.cb8r4.UseVisualStyleBackColor = true;
            this.cb8r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb8r5
            // 
            this.cb8r5.AutoSize = true;
            this.cb8r5.Enabled = false;
            this.cb8r5.Location = new System.Drawing.Point(74, 162);
            this.cb8r5.Name = "cb8r5";
            this.cb8r5.Size = new System.Drawing.Size(15, 14);
            this.cb8r5.TabIndex = 82;
            this.cb8r5.UseVisualStyleBackColor = true;
            this.cb8r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb8r6
            // 
            this.cb8r6.AutoSize = true;
            this.cb8r6.Enabled = false;
            this.cb8r6.Location = new System.Drawing.Point(58, 162);
            this.cb8r6.Name = "cb8r6";
            this.cb8r6.Size = new System.Drawing.Size(15, 14);
            this.cb8r6.TabIndex = 81;
            this.cb8r6.UseVisualStyleBackColor = true;
            this.cb8r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb8r7
            // 
            this.cb8r7.AutoSize = true;
            this.cb8r7.Enabled = false;
            this.cb8r7.Location = new System.Drawing.Point(41, 162);
            this.cb8r7.Name = "cb8r7";
            this.cb8r7.Size = new System.Drawing.Size(15, 14);
            this.cb8r7.TabIndex = 80;
            this.cb8r7.UseVisualStyleBackColor = true;
            this.cb8r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label96
            // 
            this.label96.AutoSize = true;
            this.label96.ForeColor = System.Drawing.Color.Firebrick;
            this.label96.Location = new System.Drawing.Point(14, 147);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(13, 13);
            this.label96.TabIndex = 79;
            this.label96.Text = "7";
            // 
            // cb7r0
            // 
            this.cb7r0.AutoSize = true;
            this.cb7r0.Enabled = false;
            this.cb7r0.Location = new System.Drawing.Point(154, 147);
            this.cb7r0.Name = "cb7r0";
            this.cb7r0.Size = new System.Drawing.Size(15, 14);
            this.cb7r0.TabIndex = 78;
            this.cb7r0.UseVisualStyleBackColor = true;
            this.cb7r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb7r1
            // 
            this.cb7r1.AutoSize = true;
            this.cb7r1.Enabled = false;
            this.cb7r1.Location = new System.Drawing.Point(138, 147);
            this.cb7r1.Name = "cb7r1";
            this.cb7r1.Size = new System.Drawing.Size(15, 14);
            this.cb7r1.TabIndex = 77;
            this.cb7r1.UseVisualStyleBackColor = true;
            this.cb7r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb7r2
            // 
            this.cb7r2.AutoSize = true;
            this.cb7r2.Enabled = false;
            this.cb7r2.Location = new System.Drawing.Point(122, 147);
            this.cb7r2.Name = "cb7r2";
            this.cb7r2.Size = new System.Drawing.Size(15, 14);
            this.cb7r2.TabIndex = 76;
            this.cb7r2.UseVisualStyleBackColor = true;
            this.cb7r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb7r3
            // 
            this.cb7r3.AutoSize = true;
            this.cb7r3.Enabled = false;
            this.cb7r3.Location = new System.Drawing.Point(106, 147);
            this.cb7r3.Name = "cb7r3";
            this.cb7r3.Size = new System.Drawing.Size(15, 14);
            this.cb7r3.TabIndex = 75;
            this.cb7r3.UseVisualStyleBackColor = true;
            this.cb7r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb7r4
            // 
            this.cb7r4.AutoSize = true;
            this.cb7r4.Enabled = false;
            this.cb7r4.Location = new System.Drawing.Point(90, 147);
            this.cb7r4.Name = "cb7r4";
            this.cb7r4.Size = new System.Drawing.Size(15, 14);
            this.cb7r4.TabIndex = 74;
            this.cb7r4.UseVisualStyleBackColor = true;
            this.cb7r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb7r5
            // 
            this.cb7r5.AutoSize = true;
            this.cb7r5.Enabled = false;
            this.cb7r5.Location = new System.Drawing.Point(74, 147);
            this.cb7r5.Name = "cb7r5";
            this.cb7r5.Size = new System.Drawing.Size(15, 14);
            this.cb7r5.TabIndex = 73;
            this.cb7r5.UseVisualStyleBackColor = true;
            this.cb7r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb7r6
            // 
            this.cb7r6.AutoSize = true;
            this.cb7r6.Enabled = false;
            this.cb7r6.Location = new System.Drawing.Point(58, 147);
            this.cb7r6.Name = "cb7r6";
            this.cb7r6.Size = new System.Drawing.Size(15, 14);
            this.cb7r6.TabIndex = 72;
            this.cb7r6.UseVisualStyleBackColor = true;
            this.cb7r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb7r7
            // 
            this.cb7r7.AutoSize = true;
            this.cb7r7.Enabled = false;
            this.cb7r7.Location = new System.Drawing.Point(41, 147);
            this.cb7r7.Name = "cb7r7";
            this.cb7r7.Size = new System.Drawing.Size(15, 14);
            this.cb7r7.TabIndex = 71;
            this.cb7r7.UseVisualStyleBackColor = true;
            this.cb7r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label95
            // 
            this.label95.AutoSize = true;
            this.label95.ForeColor = System.Drawing.Color.Firebrick;
            this.label95.Location = new System.Drawing.Point(14, 132);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(13, 13);
            this.label95.TabIndex = 70;
            this.label95.Text = "6";
            // 
            // cb6r0
            // 
            this.cb6r0.AutoSize = true;
            this.cb6r0.Enabled = false;
            this.cb6r0.Location = new System.Drawing.Point(154, 132);
            this.cb6r0.Name = "cb6r0";
            this.cb6r0.Size = new System.Drawing.Size(15, 14);
            this.cb6r0.TabIndex = 69;
            this.cb6r0.UseVisualStyleBackColor = true;
            this.cb6r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb6r1
            // 
            this.cb6r1.AutoSize = true;
            this.cb6r1.Enabled = false;
            this.cb6r1.Location = new System.Drawing.Point(138, 132);
            this.cb6r1.Name = "cb6r1";
            this.cb6r1.Size = new System.Drawing.Size(15, 14);
            this.cb6r1.TabIndex = 68;
            this.cb6r1.UseVisualStyleBackColor = true;
            this.cb6r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb6r2
            // 
            this.cb6r2.AutoSize = true;
            this.cb6r2.Enabled = false;
            this.cb6r2.Location = new System.Drawing.Point(122, 132);
            this.cb6r2.Name = "cb6r2";
            this.cb6r2.Size = new System.Drawing.Size(15, 14);
            this.cb6r2.TabIndex = 67;
            this.cb6r2.UseVisualStyleBackColor = true;
            this.cb6r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb6r3
            // 
            this.cb6r3.AutoSize = true;
            this.cb6r3.Enabled = false;
            this.cb6r3.Location = new System.Drawing.Point(106, 132);
            this.cb6r3.Name = "cb6r3";
            this.cb6r3.Size = new System.Drawing.Size(15, 14);
            this.cb6r3.TabIndex = 66;
            this.cb6r3.UseVisualStyleBackColor = true;
            this.cb6r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb6r4
            // 
            this.cb6r4.AutoSize = true;
            this.cb6r4.Enabled = false;
            this.cb6r4.Location = new System.Drawing.Point(90, 132);
            this.cb6r4.Name = "cb6r4";
            this.cb6r4.Size = new System.Drawing.Size(15, 14);
            this.cb6r4.TabIndex = 65;
            this.cb6r4.UseVisualStyleBackColor = true;
            this.cb6r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb6r5
            // 
            this.cb6r5.AutoSize = true;
            this.cb6r5.Enabled = false;
            this.cb6r5.Location = new System.Drawing.Point(74, 132);
            this.cb6r5.Name = "cb6r5";
            this.cb6r5.Size = new System.Drawing.Size(15, 14);
            this.cb6r5.TabIndex = 64;
            this.cb6r5.UseVisualStyleBackColor = true;
            this.cb6r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb6r6
            // 
            this.cb6r6.AutoSize = true;
            this.cb6r6.Enabled = false;
            this.cb6r6.Location = new System.Drawing.Point(58, 132);
            this.cb6r6.Name = "cb6r6";
            this.cb6r6.Size = new System.Drawing.Size(15, 14);
            this.cb6r6.TabIndex = 63;
            this.cb6r6.UseVisualStyleBackColor = true;
            this.cb6r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb6r7
            // 
            this.cb6r7.AutoSize = true;
            this.cb6r7.Enabled = false;
            this.cb6r7.Location = new System.Drawing.Point(41, 132);
            this.cb6r7.Name = "cb6r7";
            this.cb6r7.Size = new System.Drawing.Size(15, 14);
            this.cb6r7.TabIndex = 62;
            this.cb6r7.UseVisualStyleBackColor = true;
            this.cb6r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.ForeColor = System.Drawing.Color.Firebrick;
            this.label94.Location = new System.Drawing.Point(14, 118);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(13, 13);
            this.label94.TabIndex = 61;
            this.label94.Text = "5";
            // 
            // cb5r0
            // 
            this.cb5r0.AutoSize = true;
            this.cb5r0.Enabled = false;
            this.cb5r0.Location = new System.Drawing.Point(154, 118);
            this.cb5r0.Name = "cb5r0";
            this.cb5r0.Size = new System.Drawing.Size(15, 14);
            this.cb5r0.TabIndex = 60;
            this.cb5r0.UseVisualStyleBackColor = true;
            this.cb5r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb5r1
            // 
            this.cb5r1.AutoSize = true;
            this.cb5r1.Enabled = false;
            this.cb5r1.Location = new System.Drawing.Point(138, 118);
            this.cb5r1.Name = "cb5r1";
            this.cb5r1.Size = new System.Drawing.Size(15, 14);
            this.cb5r1.TabIndex = 59;
            this.cb5r1.UseVisualStyleBackColor = true;
            this.cb5r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb5r2
            // 
            this.cb5r2.AutoSize = true;
            this.cb5r2.Enabled = false;
            this.cb5r2.Location = new System.Drawing.Point(122, 118);
            this.cb5r2.Name = "cb5r2";
            this.cb5r2.Size = new System.Drawing.Size(15, 14);
            this.cb5r2.TabIndex = 58;
            this.cb5r2.UseVisualStyleBackColor = true;
            this.cb5r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb5r3
            // 
            this.cb5r3.AutoSize = true;
            this.cb5r3.Enabled = false;
            this.cb5r3.Location = new System.Drawing.Point(106, 118);
            this.cb5r3.Name = "cb5r3";
            this.cb5r3.Size = new System.Drawing.Size(15, 14);
            this.cb5r3.TabIndex = 57;
            this.cb5r3.UseVisualStyleBackColor = true;
            this.cb5r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb5r4
            // 
            this.cb5r4.AutoSize = true;
            this.cb5r4.Enabled = false;
            this.cb5r4.Location = new System.Drawing.Point(90, 118);
            this.cb5r4.Name = "cb5r4";
            this.cb5r4.Size = new System.Drawing.Size(15, 14);
            this.cb5r4.TabIndex = 56;
            this.cb5r4.UseVisualStyleBackColor = true;
            this.cb5r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb5r5
            // 
            this.cb5r5.AutoSize = true;
            this.cb5r5.Enabled = false;
            this.cb5r5.Location = new System.Drawing.Point(74, 118);
            this.cb5r5.Name = "cb5r5";
            this.cb5r5.Size = new System.Drawing.Size(15, 14);
            this.cb5r5.TabIndex = 55;
            this.cb5r5.UseVisualStyleBackColor = true;
            this.cb5r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb5r6
            // 
            this.cb5r6.AutoSize = true;
            this.cb5r6.Enabled = false;
            this.cb5r6.Location = new System.Drawing.Point(58, 118);
            this.cb5r6.Name = "cb5r6";
            this.cb5r6.Size = new System.Drawing.Size(15, 14);
            this.cb5r6.TabIndex = 54;
            this.cb5r6.UseVisualStyleBackColor = true;
            this.cb5r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb5r7
            // 
            this.cb5r7.AutoSize = true;
            this.cb5r7.Enabled = false;
            this.cb5r7.Location = new System.Drawing.Point(41, 118);
            this.cb5r7.Name = "cb5r7";
            this.cb5r7.Size = new System.Drawing.Size(15, 14);
            this.cb5r7.TabIndex = 53;
            this.cb5r7.UseVisualStyleBackColor = true;
            this.cb5r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.ForeColor = System.Drawing.Color.Firebrick;
            this.label93.Location = new System.Drawing.Point(14, 103);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(13, 13);
            this.label93.TabIndex = 52;
            this.label93.Text = "4";
            // 
            // cb4r0
            // 
            this.cb4r0.AutoSize = true;
            this.cb4r0.Enabled = false;
            this.cb4r0.Location = new System.Drawing.Point(154, 103);
            this.cb4r0.Name = "cb4r0";
            this.cb4r0.Size = new System.Drawing.Size(15, 14);
            this.cb4r0.TabIndex = 51;
            this.cb4r0.UseVisualStyleBackColor = true;
            this.cb4r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb4r1
            // 
            this.cb4r1.AutoSize = true;
            this.cb4r1.Enabled = false;
            this.cb4r1.Location = new System.Drawing.Point(138, 103);
            this.cb4r1.Name = "cb4r1";
            this.cb4r1.Size = new System.Drawing.Size(15, 14);
            this.cb4r1.TabIndex = 50;
            this.cb4r1.UseVisualStyleBackColor = true;
            this.cb4r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb4r2
            // 
            this.cb4r2.AutoSize = true;
            this.cb4r2.Enabled = false;
            this.cb4r2.Location = new System.Drawing.Point(122, 103);
            this.cb4r2.Name = "cb4r2";
            this.cb4r2.Size = new System.Drawing.Size(15, 14);
            this.cb4r2.TabIndex = 49;
            this.cb4r2.UseVisualStyleBackColor = true;
            this.cb4r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb4r3
            // 
            this.cb4r3.AutoSize = true;
            this.cb4r3.Enabled = false;
            this.cb4r3.Location = new System.Drawing.Point(106, 103);
            this.cb4r3.Name = "cb4r3";
            this.cb4r3.Size = new System.Drawing.Size(15, 14);
            this.cb4r3.TabIndex = 48;
            this.cb4r3.UseVisualStyleBackColor = true;
            this.cb4r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb4r4
            // 
            this.cb4r4.AutoSize = true;
            this.cb4r4.Enabled = false;
            this.cb4r4.Location = new System.Drawing.Point(90, 103);
            this.cb4r4.Name = "cb4r4";
            this.cb4r4.Size = new System.Drawing.Size(15, 14);
            this.cb4r4.TabIndex = 47;
            this.cb4r4.UseVisualStyleBackColor = true;
            this.cb4r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb4r5
            // 
            this.cb4r5.AutoSize = true;
            this.cb4r5.Enabled = false;
            this.cb4r5.Location = new System.Drawing.Point(74, 103);
            this.cb4r5.Name = "cb4r5";
            this.cb4r5.Size = new System.Drawing.Size(15, 14);
            this.cb4r5.TabIndex = 46;
            this.cb4r5.UseVisualStyleBackColor = true;
            this.cb4r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb4r6
            // 
            this.cb4r6.AutoSize = true;
            this.cb4r6.Enabled = false;
            this.cb4r6.Location = new System.Drawing.Point(58, 103);
            this.cb4r6.Name = "cb4r6";
            this.cb4r6.Size = new System.Drawing.Size(15, 14);
            this.cb4r6.TabIndex = 45;
            this.cb4r6.UseVisualStyleBackColor = true;
            this.cb4r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb4r7
            // 
            this.cb4r7.AutoSize = true;
            this.cb4r7.Enabled = false;
            this.cb4r7.Location = new System.Drawing.Point(41, 103);
            this.cb4r7.Name = "cb4r7";
            this.cb4r7.Size = new System.Drawing.Size(15, 14);
            this.cb4r7.TabIndex = 44;
            this.cb4r7.UseVisualStyleBackColor = true;
            this.cb4r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.ForeColor = System.Drawing.Color.Firebrick;
            this.label92.Location = new System.Drawing.Point(14, 87);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(13, 13);
            this.label92.TabIndex = 43;
            this.label92.Text = "3";
            // 
            // cb3r0
            // 
            this.cb3r0.AutoSize = true;
            this.cb3r0.Enabled = false;
            this.cb3r0.Location = new System.Drawing.Point(154, 87);
            this.cb3r0.Name = "cb3r0";
            this.cb3r0.Size = new System.Drawing.Size(15, 14);
            this.cb3r0.TabIndex = 42;
            this.cb3r0.UseVisualStyleBackColor = true;
            this.cb3r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb3r1
            // 
            this.cb3r1.AutoSize = true;
            this.cb3r1.Enabled = false;
            this.cb3r1.Location = new System.Drawing.Point(138, 87);
            this.cb3r1.Name = "cb3r1";
            this.cb3r1.Size = new System.Drawing.Size(15, 14);
            this.cb3r1.TabIndex = 41;
            this.cb3r1.UseVisualStyleBackColor = true;
            this.cb3r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb3r2
            // 
            this.cb3r2.AutoSize = true;
            this.cb3r2.Enabled = false;
            this.cb3r2.Location = new System.Drawing.Point(122, 87);
            this.cb3r2.Name = "cb3r2";
            this.cb3r2.Size = new System.Drawing.Size(15, 14);
            this.cb3r2.TabIndex = 40;
            this.cb3r2.UseVisualStyleBackColor = true;
            this.cb3r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb3r3
            // 
            this.cb3r3.AutoSize = true;
            this.cb3r3.Enabled = false;
            this.cb3r3.Location = new System.Drawing.Point(106, 87);
            this.cb3r3.Name = "cb3r3";
            this.cb3r3.Size = new System.Drawing.Size(15, 14);
            this.cb3r3.TabIndex = 39;
            this.cb3r3.UseVisualStyleBackColor = true;
            this.cb3r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb3r4
            // 
            this.cb3r4.AutoSize = true;
            this.cb3r4.Enabled = false;
            this.cb3r4.Location = new System.Drawing.Point(90, 87);
            this.cb3r4.Name = "cb3r4";
            this.cb3r4.Size = new System.Drawing.Size(15, 14);
            this.cb3r4.TabIndex = 38;
            this.cb3r4.UseVisualStyleBackColor = true;
            this.cb3r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb3r5
            // 
            this.cb3r5.AutoSize = true;
            this.cb3r5.Enabled = false;
            this.cb3r5.Location = new System.Drawing.Point(74, 87);
            this.cb3r5.Name = "cb3r5";
            this.cb3r5.Size = new System.Drawing.Size(15, 14);
            this.cb3r5.TabIndex = 37;
            this.cb3r5.UseVisualStyleBackColor = true;
            this.cb3r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb3r6
            // 
            this.cb3r6.AutoSize = true;
            this.cb3r6.Enabled = false;
            this.cb3r6.Location = new System.Drawing.Point(58, 87);
            this.cb3r6.Name = "cb3r6";
            this.cb3r6.Size = new System.Drawing.Size(15, 14);
            this.cb3r6.TabIndex = 36;
            this.cb3r6.UseVisualStyleBackColor = true;
            this.cb3r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb3r7
            // 
            this.cb3r7.AutoSize = true;
            this.cb3r7.Enabled = false;
            this.cb3r7.Location = new System.Drawing.Point(41, 87);
            this.cb3r7.Name = "cb3r7";
            this.cb3r7.Size = new System.Drawing.Size(15, 14);
            this.cb3r7.TabIndex = 35;
            this.cb3r7.UseVisualStyleBackColor = true;
            this.cb3r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label91
            // 
            this.label91.AutoSize = true;
            this.label91.ForeColor = System.Drawing.Color.Firebrick;
            this.label91.Location = new System.Drawing.Point(14, 72);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(13, 13);
            this.label91.TabIndex = 34;
            this.label91.Text = "2";
            // 
            // cb2r0
            // 
            this.cb2r0.AutoSize = true;
            this.cb2r0.Enabled = false;
            this.cb2r0.Location = new System.Drawing.Point(154, 72);
            this.cb2r0.Name = "cb2r0";
            this.cb2r0.Size = new System.Drawing.Size(15, 14);
            this.cb2r0.TabIndex = 33;
            this.cb2r0.UseVisualStyleBackColor = true;
            this.cb2r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb2r1
            // 
            this.cb2r1.AutoSize = true;
            this.cb2r1.Enabled = false;
            this.cb2r1.Location = new System.Drawing.Point(138, 72);
            this.cb2r1.Name = "cb2r1";
            this.cb2r1.Size = new System.Drawing.Size(15, 14);
            this.cb2r1.TabIndex = 32;
            this.cb2r1.UseVisualStyleBackColor = true;
            this.cb2r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb2r2
            // 
            this.cb2r2.AutoSize = true;
            this.cb2r2.Enabled = false;
            this.cb2r2.Location = new System.Drawing.Point(122, 72);
            this.cb2r2.Name = "cb2r2";
            this.cb2r2.Size = new System.Drawing.Size(15, 14);
            this.cb2r2.TabIndex = 31;
            this.cb2r2.UseVisualStyleBackColor = true;
            this.cb2r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb2r3
            // 
            this.cb2r3.AutoSize = true;
            this.cb2r3.Enabled = false;
            this.cb2r3.Location = new System.Drawing.Point(106, 72);
            this.cb2r3.Name = "cb2r3";
            this.cb2r3.Size = new System.Drawing.Size(15, 14);
            this.cb2r3.TabIndex = 30;
            this.cb2r3.UseVisualStyleBackColor = true;
            this.cb2r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb2r4
            // 
            this.cb2r4.AutoSize = true;
            this.cb2r4.Enabled = false;
            this.cb2r4.Location = new System.Drawing.Point(90, 72);
            this.cb2r4.Name = "cb2r4";
            this.cb2r4.Size = new System.Drawing.Size(15, 14);
            this.cb2r4.TabIndex = 29;
            this.cb2r4.UseVisualStyleBackColor = true;
            this.cb2r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb2r5
            // 
            this.cb2r5.AutoSize = true;
            this.cb2r5.Enabled = false;
            this.cb2r5.Location = new System.Drawing.Point(74, 72);
            this.cb2r5.Name = "cb2r5";
            this.cb2r5.Size = new System.Drawing.Size(15, 14);
            this.cb2r5.TabIndex = 28;
            this.cb2r5.UseVisualStyleBackColor = true;
            this.cb2r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb2r6
            // 
            this.cb2r6.AutoSize = true;
            this.cb2r6.Enabled = false;
            this.cb2r6.Location = new System.Drawing.Point(58, 72);
            this.cb2r6.Name = "cb2r6";
            this.cb2r6.Size = new System.Drawing.Size(15, 14);
            this.cb2r6.TabIndex = 27;
            this.cb2r6.UseVisualStyleBackColor = true;
            this.cb2r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb2r7
            // 
            this.cb2r7.AutoSize = true;
            this.cb2r7.Enabled = false;
            this.cb2r7.Location = new System.Drawing.Point(41, 72);
            this.cb2r7.Name = "cb2r7";
            this.cb2r7.Size = new System.Drawing.Size(15, 14);
            this.cb2r7.TabIndex = 26;
            this.cb2r7.UseVisualStyleBackColor = true;
            this.cb2r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label90
            // 
            this.label90.AutoSize = true;
            this.label90.ForeColor = System.Drawing.Color.Firebrick;
            this.label90.Location = new System.Drawing.Point(14, 56);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(13, 13);
            this.label90.TabIndex = 25;
            this.label90.Text = "1";
            // 
            // cb1r0
            // 
            this.cb1r0.AutoSize = true;
            this.cb1r0.Enabled = false;
            this.cb1r0.Location = new System.Drawing.Point(154, 56);
            this.cb1r0.Name = "cb1r0";
            this.cb1r0.Size = new System.Drawing.Size(15, 14);
            this.cb1r0.TabIndex = 24;
            this.cb1r0.UseVisualStyleBackColor = true;
            this.cb1r0.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb1r1
            // 
            this.cb1r1.AutoSize = true;
            this.cb1r1.Enabled = false;
            this.cb1r1.Location = new System.Drawing.Point(138, 56);
            this.cb1r1.Name = "cb1r1";
            this.cb1r1.Size = new System.Drawing.Size(15, 14);
            this.cb1r1.TabIndex = 23;
            this.cb1r1.UseVisualStyleBackColor = true;
            this.cb1r1.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb1r2
            // 
            this.cb1r2.AutoSize = true;
            this.cb1r2.Enabled = false;
            this.cb1r2.Location = new System.Drawing.Point(122, 56);
            this.cb1r2.Name = "cb1r2";
            this.cb1r2.Size = new System.Drawing.Size(15, 14);
            this.cb1r2.TabIndex = 22;
            this.cb1r2.UseVisualStyleBackColor = true;
            this.cb1r2.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb1r3
            // 
            this.cb1r3.AutoSize = true;
            this.cb1r3.Enabled = false;
            this.cb1r3.Location = new System.Drawing.Point(106, 56);
            this.cb1r3.Name = "cb1r3";
            this.cb1r3.Size = new System.Drawing.Size(15, 14);
            this.cb1r3.TabIndex = 21;
            this.cb1r3.UseVisualStyleBackColor = true;
            this.cb1r3.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb1r4
            // 
            this.cb1r4.AutoSize = true;
            this.cb1r4.Enabled = false;
            this.cb1r4.Location = new System.Drawing.Point(90, 56);
            this.cb1r4.Name = "cb1r4";
            this.cb1r4.Size = new System.Drawing.Size(15, 14);
            this.cb1r4.TabIndex = 20;
            this.cb1r4.UseVisualStyleBackColor = true;
            this.cb1r4.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb1r5
            // 
            this.cb1r5.AutoSize = true;
            this.cb1r5.Enabled = false;
            this.cb1r5.Location = new System.Drawing.Point(74, 56);
            this.cb1r5.Name = "cb1r5";
            this.cb1r5.Size = new System.Drawing.Size(15, 14);
            this.cb1r5.TabIndex = 19;
            this.cb1r5.UseVisualStyleBackColor = true;
            this.cb1r5.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb1r6
            // 
            this.cb1r6.AutoSize = true;
            this.cb1r6.Enabled = false;
            this.cb1r6.Location = new System.Drawing.Point(58, 56);
            this.cb1r6.Name = "cb1r6";
            this.cb1r6.Size = new System.Drawing.Size(15, 14);
            this.cb1r6.TabIndex = 18;
            this.cb1r6.UseVisualStyleBackColor = true;
            this.cb1r6.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // cb1r7
            // 
            this.cb1r7.AutoSize = true;
            this.cb1r7.Enabled = false;
            this.cb1r7.Location = new System.Drawing.Point(41, 56);
            this.cb1r7.Name = "cb1r7";
            this.cb1r7.Size = new System.Drawing.Size(15, 14);
            this.cb1r7.TabIndex = 17;
            this.cb1r7.UseVisualStyleBackColor = true;
            this.cb1r7.CheckedChanged += new System.EventHandler(this.grpPortA_CheckedChanged);
            // 
            // label89
            // 
            this.label89.AutoSize = true;
            this.label89.ForeColor = System.Drawing.Color.Navy;
            this.label89.Location = new System.Drawing.Point(156, 24);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(13, 13);
            this.label89.TabIndex = 16;
            this.label89.Text = "0";
            // 
            // label88
            // 
            this.label88.AutoSize = true;
            this.label88.ForeColor = System.Drawing.Color.Navy;
            this.label88.Location = new System.Drawing.Point(140, 24);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(13, 13);
            this.label88.TabIndex = 15;
            this.label88.Text = "1";
            // 
            // label87
            // 
            this.label87.AutoSize = true;
            this.label87.ForeColor = System.Drawing.Color.Navy;
            this.label87.Location = new System.Drawing.Point(124, 24);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(13, 13);
            this.label87.TabIndex = 14;
            this.label87.Text = "2";
            // 
            // label86
            // 
            this.label86.AutoSize = true;
            this.label86.ForeColor = System.Drawing.Color.Navy;
            this.label86.Location = new System.Drawing.Point(108, 24);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(13, 13);
            this.label86.TabIndex = 13;
            this.label86.Text = "3";
            // 
            // label85
            // 
            this.label85.AutoSize = true;
            this.label85.ForeColor = System.Drawing.Color.Navy;
            this.label85.Location = new System.Drawing.Point(92, 24);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(13, 13);
            this.label85.TabIndex = 12;
            this.label85.Text = "4";
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.ForeColor = System.Drawing.Color.Navy;
            this.label84.Location = new System.Drawing.Point(76, 24);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(13, 13);
            this.label84.TabIndex = 11;
            this.label84.Text = "5";
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.ForeColor = System.Drawing.Color.Navy;
            this.label83.Location = new System.Drawing.Point(60, 24);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(13, 13);
            this.label83.TabIndex = 10;
            this.label83.Text = "6";
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.label82.ForeColor = System.Drawing.Color.Navy;
            this.label82.Location = new System.Drawing.Point(41, 24);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(13, 13);
            this.label82.TabIndex = 9;
            this.label82.Text = "7";
            // 
            // label81
            // 
            this.label81.AutoSize = true;
            this.label81.ForeColor = System.Drawing.Color.Firebrick;
            this.label81.Location = new System.Drawing.Point(14, 40);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(13, 13);
            this.label81.TabIndex = 8;
            this.label81.Text = "0";
            // 
            // chkPortA
            // 
            this.chkPortA.AutoSize = true;
            this.chkPortA.ForeColor = System.Drawing.Color.Navy;
            this.chkPortA.Location = new System.Drawing.Point(184, 47);
            this.chkPortA.Name = "chkPortA";
            this.chkPortA.Size = new System.Drawing.Size(51, 17);
            this.chkPortA.TabIndex = 2;
            this.chkPortA.Text = "Enab";
            this.chkPortA.UseVisualStyleBackColor = true;
            this.chkPortA.CheckedChanged += new System.EventHandler(this.chkPortA_CheckedChanged);
            // 
            // txtPortA
            // 
            this.txtPortA.Enabled = false;
            this.txtPortA.Location = new System.Drawing.Point(189, 82);
            this.txtPortA.Name = "txtPortA";
            this.txtPortA.Size = new System.Drawing.Size(40, 20);
            this.txtPortA.TabIndex = 136;
            this.txtPortA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPortA, "Enter the port address (decimal).");
            this.txtPortA.TextChanged += new System.EventHandler(this.txtPortA_TextChanged);
            // 
            // label103
            // 
            this.label103.AutoSize = true;
            this.label103.ForeColor = System.Drawing.Color.Navy;
            this.label103.Location = new System.Drawing.Point(191, 66);
            this.label103.Name = "label103";
            this.label103.Size = new System.Drawing.Size(36, 13);
            this.label103.TabIndex = 135;
            this.label103.Text = "Port A";
            // 
            // txtPortB
            // 
            this.txtPortB.Enabled = false;
            this.txtPortB.Location = new System.Drawing.Point(188, 251);
            this.txtPortB.Name = "txtPortB";
            this.txtPortB.Size = new System.Drawing.Size(40, 20);
            this.txtPortB.TabIndex = 274;
            this.txtPortB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPortB, "Enter the port address (decimal).");
            this.txtPortB.TextChanged += new System.EventHandler(this.txtPortB_TextChanged);
            // 
            // label106
            // 
            this.label106.AutoSize = true;
            this.label106.ForeColor = System.Drawing.Color.Navy;
            this.label106.Location = new System.Drawing.Point(190, 235);
            this.label106.Name = "label106";
            this.label106.Size = new System.Drawing.Size(36, 13);
            this.label106.TabIndex = 273;
            this.label106.Text = "Port B";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // AccPort
            // 
            this.AccPort.ReadTimeout = 500;
            this.AccPort.WriteTimeout = 500;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 200;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.StripAmpersands = true;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;
            // 
            // txtFwd
            // 
            this.txtFwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFwd.Location = new System.Drawing.Point(5, 365);
            this.txtFwd.Name = "txtFwd";
            this.txtFwd.Size = new System.Drawing.Size(45, 20);
            this.txtFwd.TabIndex = 65;
            this.txtFwd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtFwd, "Transmitter Peak Forward Power");
            // 
            // txtSWR
            // 
            this.txtSWR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSWR.ForeColor = System.Drawing.Color.Red;
            this.txtSWR.Location = new System.Drawing.Point(150, 365);
            this.txtSWR.Name = "txtSWR";
            this.txtSWR.Size = new System.Drawing.Size(45, 20);
            this.txtSWR.TabIndex = 66;
            this.txtSWR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtSWR, "Antenna SWR");
            // 
            // btnProfiler
            // 
            this.btnProfiler.AutoSize = true;
            this.btnProfiler.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnProfiler.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnProfiler.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnProfiler.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this.btnProfiler.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnProfiler.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnProfiler.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold);
            this.btnProfiler.ForeColor = System.Drawing.Color.Navy;
            this.btnProfiler.Location = new System.Drawing.Point(230, 365);
            this.btnProfiler.Name = "btnProfiler";
            this.btnProfiler.Size = new System.Drawing.Size(62, 21);
            this.btnProfiler.TabIndex = 68;
            this.btnProfiler.Text = "PF Open";
            this.toolTip1.SetToolTip(this.btnProfiler, "Starts Flex Profiler program");
            this.btnProfiler.UseVisualStyleBackColor = false;
            this.btnProfiler.Click += new System.EventHandler(this.btnProfiler_Click);
            // 
            // btnReStart
            // 
            this.btnReStart.AutoSize = true;
            this.btnReStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnReStart.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnReStart.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnReStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnReStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnReStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnReStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold);
            this.btnReStart.ForeColor = System.Drawing.Color.Navy;
            this.btnReStart.Location = new System.Drawing.Point(299, 365);
            this.btnReStart.Name = "btnReStart";
            this.btnReStart.Size = new System.Drawing.Size(62, 21);
            this.btnReStart.TabIndex = 69;
            this.btnReStart.Text = "PFClose";
            this.toolTip1.SetToolTip(this.btnReStart, "Restarts DDUtil after closing Profiler");
            this.btnReStart.UseVisualStyleBackColor = false;
            this.btnReStart.Click += new System.EventHandler(this.btnReStart_Click);
            // 
            // txtTemp
            // 
            this.txtTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTemp.Location = new System.Drawing.Point(367, 364);
            this.txtTemp.Name = "txtTemp";
            this.txtTemp.Size = new System.Drawing.Size(50, 22);
            this.txtTemp.TabIndex = 62;
            this.txtTemp.Text = "Temp";
            this.txtTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTemp, "Flex-5000 PA Temp, click to change from C to F");
            this.txtTemp.Click += new System.EventHandler(this.txtTemp_Click);
            // 
            // lblAnt
            // 
            this.lblAnt.AutoSize = true;
            this.lblAnt.BackColor = System.Drawing.SystemColors.Control;
            this.lblAnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnt.ForeColor = System.Drawing.Color.Red;
            this.lblAnt.Location = new System.Drawing.Point(300, 397);
            this.lblAnt.Name = "lblAnt";
            this.lblAnt.Size = new System.Drawing.Size(100, 16);
            this.lblAnt.TabIndex = 73;
            this.lblAnt.Text = "Xmit Inhibited";
            this.toolTip1.SetToolTip(this.lblAnt, "Illuminated when antenna is adjusting");
            this.lblAnt.Visible = false;
            // 
            // txtAvg
            // 
            this.txtAvg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAvg.Location = new System.Drawing.Point(79, 365);
            this.txtAvg.Name = "txtAvg";
            this.txtAvg.Size = new System.Drawing.Size(45, 20);
            this.txtAvg.TabIndex = 75;
            this.txtAvg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtAvg, "Transmitter Average/Forward Power");
            // 
            // LogPort
            // 
            this.LogPort.ReadTimeout = 500;
            this.LogPort.WriteTimeout = 500;
            this.LogPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.OnReceive);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem4,
            this.flexProfilerToolStripMenuItem,
            this.iCOMICPW1ToolStripMenuItem,
            this.lP100ToolStripMenuItem,
            this.macroFormToolStripMenuItem,
            this.rCPPortsToolStripMenuItem,
            this.rotorControlToolStripMenuItem,
            this.waveNodeToolStripMenuItem,
            this.toolStripMenuItem3});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 246);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.toolStripMenuItem1.Text = "Restore Form Size";
            this.toolStripMenuItem1.ToolTipText = "Restore form to full size";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(172, 22);
            this.toolStripMenuItem2.Text = "Shrink Form Size";
            this.toolStripMenuItem2.ToolTipText = "Shrink form to title bar only";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(172, 22);
            this.toolStripMenuItem4.Text = "Slave Radio Info";
            this.toolStripMenuItem4.ToolTipText = "Info on choosing Slave Radio";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // flexProfilerToolStripMenuItem
            // 
            this.flexProfilerToolStripMenuItem.Name = "flexProfilerToolStripMenuItem";
            this.flexProfilerToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.flexProfilerToolStripMenuItem.Text = "Flex Profiler";
            this.flexProfilerToolStripMenuItem.Click += new System.EventHandler(this.flexProfilerToolStripMenuItem_Click);
            // 
            // iCOMICPW1ToolStripMenuItem
            // 
            this.iCOMICPW1ToolStripMenuItem.Name = "iCOMICPW1ToolStripMenuItem";
            this.iCOMICPW1ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.iCOMICPW1ToolStripMenuItem.Text = "ICOM IC-PW1";
            this.iCOMICPW1ToolStripMenuItem.Click += new System.EventHandler(this.iCOMICPW1ToolStripMenuItem_Click);
            // 
            // lP100ToolStripMenuItem
            // 
            this.lP100ToolStripMenuItem.Name = "lP100ToolStripMenuItem";
            this.lP100ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.lP100ToolStripMenuItem.Text = "LP-100";
            this.lP100ToolStripMenuItem.Click += new System.EventHandler(this.lP100ToolStripMenuItem_Click);
            // 
            // macroFormToolStripMenuItem
            // 
            this.macroFormToolStripMenuItem.Name = "macroFormToolStripMenuItem";
            this.macroFormToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.macroFormToolStripMenuItem.Text = "Macro Commands";
            this.macroFormToolStripMenuItem.Click += new System.EventHandler(this.macroFormToolStripMenuItem_Click);
            // 
            // rCPPortsToolStripMenuItem
            // 
            this.rCPPortsToolStripMenuItem.Name = "rCPPortsToolStripMenuItem";
            this.rCPPortsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.rCPPortsToolStripMenuItem.Text = "RCP Ports";
            this.rCPPortsToolStripMenuItem.Click += new System.EventHandler(this.rCPPortsToolStripMenuItem_Click);
            // 
            // rotorControlToolStripMenuItem
            // 
            this.rotorControlToolStripMenuItem.Name = "rotorControlToolStripMenuItem";
            this.rotorControlToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.rotorControlToolStripMenuItem.Text = "Rotor Control";
            this.rotorControlToolStripMenuItem.Click += new System.EventHandler(this.rotorControlToolStripMenuItem_Click);
            // 
            // waveNodeToolStripMenuItem
            // 
            this.waveNodeToolStripMenuItem.Name = "waveNodeToolStripMenuItem";
            this.waveNodeToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.waveNodeToolStripMenuItem.Text = "WaveNode";
            this.waveNodeToolStripMenuItem.Click += new System.EventHandler(this.waveNodeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(172, 22);
            this.toolStripMenuItem3.Text = "About DDUtil";
            this.toolStripMenuItem3.ToolTipText = "About DDUtil";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // LPport
            // 
            this.LPport.BaudRate = 38400;
            this.LPport.ReadTimeout = 500;
            this.LPport.WriteTimeout = 500;
            this.LPport.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.LPport_DataReceived);
            // 
            // lblFwd
            // 
            this.lblFwd.AutoSize = true;
            this.lblFwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFwd.ForeColor = System.Drawing.Color.Navy;
            this.lblFwd.Location = new System.Drawing.Point(50, 370);
            this.lblFwd.Name = "lblFwd";
            this.lblFwd.Size = new System.Drawing.Size(26, 12);
            this.lblFwd.TabIndex = 19;
            this.lblFwd.Text = "Peak";
            this.lblFwd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSWR
            // 
            this.lblSWR.AutoSize = true;
            this.lblSWR.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSWR.ForeColor = System.Drawing.Color.Navy;
            this.lblSWR.Location = new System.Drawing.Point(196, 370);
            this.lblSWR.Name = "lblSWR";
            this.lblSWR.Size = new System.Drawing.Size(27, 12);
            this.lblSWR.TabIndex = 67;
            this.lblSWR.Text = "SWR";
            this.lblSWR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RCP2port
            // 
            this.RCP2port.BaudRate = 38400;
            this.RCP2port.ReadTimeout = 500;
            this.RCP2port.WriteTimeout = 500;
            this.RCP2port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.RCP2port_DataReceived);
            // 
            // RCP3port
            // 
            this.RCP3port.BaudRate = 38400;
            this.RCP3port.ReadTimeout = 500;
            this.RCP3port.WriteTimeout = 500;
            this.RCP3port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.RCP3port_DataReceived);
            // 
            // RCP4port
            // 
            this.RCP4port.BaudRate = 38400;
            this.RCP4port.ReadTimeout = 500;
            this.RCP4port.WriteTimeout = 500;
            this.RCP4port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.RCP4port_DataReceived);
            // 
            // PW1port
            // 
            this.PW1port.ReadTimeout = 500;
            this.PW1port.WriteTimeout = 500;
            this.PW1port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.PW1port_DataReceived);
            // 
            // TestPort
            // 
            this.TestPort.PortName = "COM18";
            this.TestPort.ReadTimeout = 500;
            this.TestPort.WriteTimeout = 500;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(422, 24);
            this.menuStrip1.TabIndex = 70;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableErrorLoggingToolStripMenuItem,
            this.openErrorLogToolStripMenuItem,
            this.clearErrorLogToolStripMenuItem,
            this.showMiniWindowToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // enableErrorLoggingToolStripMenuItem
            // 
            this.enableErrorLoggingToolStripMenuItem.Name = "enableErrorLoggingToolStripMenuItem";
            this.enableErrorLoggingToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.enableErrorLoggingToolStripMenuItem.Text = "Enable Error Logging";
            this.enableErrorLoggingToolStripMenuItem.Click += new System.EventHandler(this.enableErrorLoggingToolStripMenuItem_Click);
            // 
            // openErrorLogToolStripMenuItem
            // 
            this.openErrorLogToolStripMenuItem.Name = "openErrorLogToolStripMenuItem";
            this.openErrorLogToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.openErrorLogToolStripMenuItem.Text = "Open Error Log";
            this.openErrorLogToolStripMenuItem.Click += new System.EventHandler(this.openErrorLogToolStripMenuItem_Click);
            // 
            // clearErrorLogToolStripMenuItem
            // 
            this.clearErrorLogToolStripMenuItem.Name = "clearErrorLogToolStripMenuItem";
            this.clearErrorLogToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.clearErrorLogToolStripMenuItem.Text = "Clear Error Log";
            this.clearErrorLogToolStripMenuItem.Click += new System.EventHandler(this.clearErrorLogToolStripMenuItem_Click);
            // 
            // showMiniWindowToolStripMenuItem
            // 
            this.showMiniWindowToolStripMenuItem.Name = "showMiniWindowToolStripMenuItem";
            this.showMiniWindowToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.M)));
            this.showMiniWindowToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.showMiniWindowToolStripMenuItem.Text = "Show Mini Window";
            this.showMiniWindowToolStripMenuItem.Click += new System.EventHandler(this.showMiniWindowToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onLineHelpToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem,
            this.setupWizardToolStripMenuItem,
            this.webSiteToolStripMenuItem,
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.aboutToolStripMenuItem.Text = "Help";
            // 
            // onLineHelpToolStripMenuItem
            // 
            this.onLineHelpToolStripMenuItem.Name = "onLineHelpToolStripMenuItem";
            this.onLineHelpToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.onLineHelpToolStripMenuItem.Text = "On-Line Help";
            this.onLineHelpToolStripMenuItem.Click += new System.EventHandler(this.onLineHelpToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // setupWizardToolStripMenuItem
            // 
            this.setupWizardToolStripMenuItem.Name = "setupWizardToolStripMenuItem";
            this.setupWizardToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.setupWizardToolStripMenuItem.Text = "Setup Wizard";
            this.setupWizardToolStripMenuItem.Click += new System.EventHandler(this.setupWizardToolStripMenuItem_Click);
            // 
            // webSiteToolStripMenuItem
            // 
            this.webSiteToolStripMenuItem.Name = "webSiteToolStripMenuItem";
            this.webSiteToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.webSiteToolStripMenuItem.Text = "Web Site";
            this.webSiteToolStripMenuItem.Click += new System.EventHandler(this.webSiteToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 390);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(422, 22);
            this.statusStrip1.TabIndex = 71;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusBar
            // 
            this.StatusBar.BackColor = System.Drawing.SystemColors.Control;
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(33, 17);
            this.StatusBar.Text = "DDUtil";
            // 
            // StepData
            // 
            this.StepData.ReadTimeout = 500;
            this.StepData.ReceivedBytesThreshold = 11;
            this.StepData.WriteTimeout = 500;
            this.StepData.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.StepData_DataReceived);
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Location = new System.Drawing.Point(11, 111);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(55, 17);
            this.radioButton10.TabIndex = 5;
            this.radioButton10.TabStop = true;
            this.radioButton10.Text = "Yaesu";
            this.radioButton10.UseVisualStyleBackColor = true;
            // 
            // radioButton11
            // 
            this.radioButton11.AutoSize = true;
            this.radioButton11.Location = new System.Drawing.Point(11, 93);
            this.radioButton11.Name = "radioButton11";
            this.radioButton11.Size = new System.Drawing.Size(64, 17);
            this.radioButton11.TabIndex = 4;
            this.radioButton11.TabStop = true;
            this.radioButton11.Text = "Prosistel";
            this.radioButton11.UseVisualStyleBackColor = true;
            // 
            // radioButton12
            // 
            this.radioButton12.AutoSize = true;
            this.radioButton12.Location = new System.Drawing.Point(11, 75);
            this.radioButton12.Name = "radioButton12";
            this.radioButton12.Size = new System.Drawing.Size(93, 17);
            this.radioButton12.TabIndex = 3;
            this.radioButton12.TabStop = true;
            this.radioButton12.Text = "M2RC2800PX";
            this.radioButton12.UseVisualStyleBackColor = true;
            // 
            // radioButton13
            // 
            this.radioButton13.AutoSize = true;
            this.radioButton13.Location = new System.Drawing.Point(11, 56);
            this.radioButton13.Name = "radioButton13";
            this.radioButton13.Size = new System.Drawing.Size(96, 17);
            this.radioButton13.TabIndex = 2;
            this.radioButton13.TabStop = true;
            this.radioButton13.Text = "M2RC2800P-A";
            this.radioButton13.UseVisualStyleBackColor = true;
            // 
            // radioButton14
            // 
            this.radioButton14.AutoSize = true;
            this.radioButton14.Location = new System.Drawing.Point(11, 38);
            this.radioButton14.Name = "radioButton14";
            this.radioButton14.Size = new System.Drawing.Size(58, 17);
            this.radioButton14.TabIndex = 1;
            this.radioButton14.TabStop = true;
            this.radioButton14.Text = "Hygain";
            this.radioButton14.UseVisualStyleBackColor = true;
            // 
            // radioButton15
            // 
            this.radioButton15.AutoSize = true;
            this.radioButton15.Location = new System.Drawing.Point(11, 20);
            this.radioButton15.Name = "radioButton15";
            this.radioButton15.Size = new System.Drawing.Size(64, 17);
            this.radioButton15.TabIndex = 0;
            this.radioButton15.TabStop = true;
            this.radioButton15.Text = "AlfaSpid";
            this.radioButton15.UseVisualStyleBackColor = true;
            // 
            // RotorPort
            // 
            this.RotorPort.DtrEnable = true;
            this.RotorPort.ReadTimeout = 500;
            this.RotorPort.RtsEnable = true;
            this.RotorPort.WriteTimeout = 500;
            this.RotorPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.RotorPort_DataReceived);
            // 
            // RCP1Rotor
            // 
            this.RCP1Rotor.ReadTimeout = 500;
            this.RCP1Rotor.WriteTimeout = 500;
            this.RCP1Rotor.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.RCP1Rotor_DataReceived);
            // 
            // RCP2Rotor
            // 
            this.RCP2Rotor.ReadTimeout = 500;
            this.RCP2Rotor.WriteTimeout = 500;
            this.RCP2Rotor.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.RCP2Rotor_DataReceived);
            // 
            // RCP3Rotor
            // 
            this.RCP3Rotor.ReadTimeout = 500;
            this.RCP3Rotor.WriteTimeout = 500;
            this.RCP3Rotor.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.RCP3Rotor_DataReceived);
            // 
            // RCP4Rotor
            // 
            this.RCP4Rotor.ReadTimeout = 500;
            this.RCP4Rotor.WriteTimeout = 500;
            this.RCP4Rotor.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.RCP4Rotor_DataReceived);
            // 
            // lblAvg
            // 
            this.lblAvg.AutoSize = true;
            this.lblAvg.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvg.ForeColor = System.Drawing.Color.Navy;
            this.lblAvg.Location = new System.Drawing.Point(125, 370);
            this.lblAvg.Name = "lblAvg";
            this.lblAvg.Size = new System.Drawing.Size(23, 12);
            this.lblAvg.TabIndex = 74;
            this.lblAvg.Text = "Avg";
            this.lblAvg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WN2Timer
            // 
            this.WN2Timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // AlphaPort
            // 
            this.AlphaPort.BaudRate = 4800;
            this.AlphaPort.DtrEnable = true;
            this.AlphaPort.RtsEnable = true;
            this.AlphaPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.AlphaPort_DataReceived);
            // 
            // lblVHF
            // 
            this.lblVHF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVHF.ForeColor = System.Drawing.Color.Firebrick;
            this.lblVHF.Location = new System.Drawing.Point(194, 163);
            this.lblVHF.Name = "lblVHF";
            this.lblVHF.Size = new System.Drawing.Size(30, 15);
            this.lblVHF.TabIndex = 280;
            this.lblVHF.Text = "- - -";
            this.lblVHF.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.lblVHF, "Band Button selected");
            // 
            // label137
            // 
            this.label137.AutoSize = true;
            this.label137.ForeColor = System.Drawing.Color.Navy;
            this.label137.Location = new System.Drawing.Point(192, 148);
            this.label137.Name = "label137";
            this.label137.Size = new System.Drawing.Size(32, 13);
            this.label137.TabIndex = 281;
            this.label137.Text = "Band";
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(422, 412);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.lblAnt);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblAvg);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.txtAvg);
            this.Controls.Add(this.txtTemp);
            this.Controls.Add(this.btnReStart);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.txtSWR);
            this.Controls.Add(this.lblSWR);
            this.Controls.Add(this.btnProfiler);
            this.Controls.Add(this.txtFwd);
            this.Controls.Add(this.lblFwd);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Setup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DDUtil - (Not Connected)";
            this.toolTip1.SetToolTip(this, "Right-Click for Options");
            this.Load += new System.EventHandler(this.Setup_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Setup_FormClosing);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Setup_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Setup_KeyDown);
            this.tabControl.ResumeLayout(false);
            this.tabPorts.ResumeLayout(false);
            this.grpSlave.ResumeLayout(false);
            this.grpSlave.PerformLayout();
            this.grpBox1.ResumeLayout(false);
            this.grpBox1.PerformLayout();
            this.grpBox2.ResumeLayout(false);
            this.grpBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.grpLPT.ResumeLayout(false);
            this.grpLPT.PerformLayout();
            this.tabDevice.ResumeLayout(false);
            this.tabDevice.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg1)).EndInit();
            this.tabRCP.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabOther.ResumeLayout(false);
            this.tabOther.PerformLayout();
            this.grpWN2.ResumeLayout(false);
            this.grpWN2.PerformLayout();
            this.grpC4.ResumeLayout(false);
            this.grpC4.PerformLayout();
            this.grpC3.ResumeLayout(false);
            this.grpC3.PerformLayout();
            this.grpC2.ResumeLayout(false);
            this.grpC2.PerformLayout();
            this.grpC1.ResumeLayout(false);
            this.grpC1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grpLP.ResumeLayout(false);
            this.grpLP.PerformLayout();
            this.tabMisc.ResumeLayout(false);
            this.grpAlpha.ResumeLayout(false);
            this.grpAlpha.PerformLayout();
            this.tabMacro.ResumeLayout(false);
            this.tabMacro.PerformLayout();
            this.grpMacro.ResumeLayout(false);
            this.grpMacro.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgm)).EndInit();
            this.tabRotor.ResumeLayout(false);
            this.grpPreset.ResumeLayout(false);
            this.grpStepCtrl.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tabSetup.ResumeLayout(false);
            this.tabSetup.PerformLayout();
            this.grpALC.ResumeLayout(false);
            this.grpALC.PerformLayout();
            this.grpDog.ResumeLayout(false);
            this.grpDog.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.grpSpeed.ResumeLayout(false);
            this.grpSpeed.PerformLayout();
            this.grpModel.ResumeLayout(false);
            this.grpModel.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.tabExtCtrl.ResumeLayout(false);
            this.tabExtCtrl.PerformLayout();
            this.grpPortB.ResumeLayout(false);
            this.grpPortB.PerformLayout();
            this.grpPortA.ResumeLayout(false);
            this.grpPortA.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPorts;
        private System.Windows.Forms.GroupBox grpBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkDevice;
        private System.Windows.Forms.GroupBox grpLPT;
        private System.Windows.Forms.RadioButton rb1;
        private System.Windows.Forms.RadioButton rb4;
        private System.Windows.Forms.RadioButton rb2;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rb3;
        private System.Windows.Forms.TabPage tabDevice;
        private System.Windows.Forms.CheckBox chkDev0;
        private System.Windows.Forms.DataGridView dg1;
        private System.Windows.Forms.TextBox Dev0;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.GroupBox grpBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboCAT;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFile0;
        private System.Windows.Forms.Button btnSave0;
        private System.Windows.Forms.Button btnReLoad0;
        private System.Windows.Forms.Button btnFile0;
        private System.Windows.Forms.ComboBox cboSerAcc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPort;
        private System.IO.Ports.SerialPort AccPort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtInv;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabOther;
        private System.IO.Ports.SerialPort LogPort;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboLogPort;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.RadioButton rbOther;
        private System.Windows.Forms.Button btnPortNum;
        private System.Windows.Forms.Label lblPortBtn;
        private System.Windows.Forms.CheckBox chkFollow;
        private System.Windows.Forms.ComboBox cboRadio;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.TextBox txtRadNum;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cboRadData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboLPport;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtLPint;
        private System.IO.Ports.SerialPort LPport;
        private System.Windows.Forms.TextBox txtFwd;
        private System.Windows.Forms.TextBox txtSWR;
        private System.Windows.Forms.Label lblSWR;
        private System.Windows.Forms.CheckBox chkLPenab;
        private System.Windows.Forms.ToolStripMenuItem lP100ToolStripMenuItem;
        private System.Windows.Forms.Button btnProfiler;
        private System.Windows.Forms.GroupBox grpLP;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnPFfile;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnReStart;
        private System.Windows.Forms.ToolStripMenuItem flexProfilerToolStripMenuItem;
        private System.Windows.Forms.TabPage tabRCP;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cboRCP2;
        private System.Windows.Forms.CheckBox chkRCP2;
        private System.Windows.Forms.ComboBox cboRCP4;
        private System.Windows.Forms.CheckBox chkRCP4;
        private System.Windows.Forms.ComboBox cboRCP3;
        private System.Windows.Forms.CheckBox chkRCP3;
        private System.IO.Ports.SerialPort RCP2port;
        private System.IO.Ports.SerialPort RCP3port;
        private System.IO.Ports.SerialPort RCP4port;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ToolStripMenuItem rCPPortsToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cboPW1;
        private System.Windows.Forms.TextBox txtPW1ta;
        private System.Windows.Forms.CheckBox chkPW1;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.CheckBox chkRCP4DisPol;
        private System.Windows.Forms.CheckBox chkRCP3DisPol;
        private System.Windows.Forms.CheckBox chkRCP2DisPol;
        private System.IO.Ports.SerialPort PW1port;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtPW1ra;
        private System.Windows.Forms.Button btnTest;
        private System.IO.Ports.SerialPort TestPort;
        private System.Windows.Forms.ToolStripMenuItem iCOMICPW1ToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkDisBcast;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem webSiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onLineHelpToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableErrorLoggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openErrorLogToolStripMenuItem;
        private System.Windows.Forms.Label lblBCD;
        private System.Windows.Forms.Label lblSleep;
        private System.Windows.Forms.TextBox txtSleep;
        private System.Windows.Forms.TextBox txtTemp;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusBar;
        private System.Windows.Forms.TabPage tabMacro;
        private System.Windows.Forms.Button btnMacro1;
        private System.Windows.Forms.Button btnMacro6;
        private System.Windows.Forms.Button btnMacro5;
        private System.Windows.Forms.Button btnMacro4;
        private System.Windows.Forms.Button btnMacro3;
        private System.Windows.Forms.Button btnMacro2;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.DataGridView dgm;
        private System.Windows.Forms.GroupBox grpMacro;
        private System.Windows.Forms.Button btnMacSelect;
        private System.Windows.Forms.Button btnMacSave;
        private System.Windows.Forms.Button btnMacReLoad;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtMacFile;
        private System.Windows.Forms.ToolStripMenuItem macroFormToolStripMenuItem;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Button btnMacro12;
        private System.Windows.Forms.Button btnMacro11;
        private System.Windows.Forms.Button btnMacro10;
        private System.Windows.Forms.Button btnMacro9;
        private System.Windows.Forms.Button btnMacro8;
        private System.Windows.Forms.Button btnMacro7;
        private System.Windows.Forms.CheckBox chkRCP4IF;
        private System.Windows.Forms.CheckBox chkRCP3IF;
        private System.Windows.Forms.CheckBox chkRCP2IF;
        private System.Windows.Forms.TextBox txtRCP;
        private System.Windows.Forms.Label label40;
        private System.IO.Ports.SerialPort StepData;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox cboStep;
        private System.Windows.Forms.TextBox txtStepInv;
        private System.Windows.Forms.CheckBox chkStep;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label lblAnt;
        private System.Windows.Forms.ComboBox cboStepCom;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TabPage tabRotor;
        private System.Windows.Forms.TextBox txtLP;
        private System.Windows.Forms.Button btnLP;
        private System.Windows.Forms.Button btnSP;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TabPage tabSetup;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TextBox txtLong;
        private System.Windows.Forms.TextBox txtLat;
        private System.Windows.Forms.GroupBox grpSpeed;
        private System.Windows.Forms.RadioButton rbRtrSpd3;
        private System.Windows.Forms.RadioButton rbRtrSpd2;
        private System.Windows.Forms.RadioButton rbRtrSpd1;
        private System.Windows.Forms.RadioButton rbRtrMod6;
        private System.Windows.Forms.RadioButton rbRtrMod5;
        private System.Windows.Forms.RadioButton rbRtrMod4;
        private System.Windows.Forms.RadioButton rbRtrMod3;
        private System.Windows.Forms.RadioButton rbRtrMod2;
        private System.Windows.Forms.RadioButton rbRtrMod1;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.RadioButton radioButton11;
        private System.Windows.Forms.RadioButton radioButton12;
        private System.Windows.Forms.RadioButton radioButton13;
        private System.Windows.Forms.RadioButton radioButton14;
        private System.Windows.Forms.RadioButton radioButton15;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.TextBox txtGrid;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.CheckBox chkRotorEnab;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.ComboBox cboRotorCom;
        private System.Windows.Forms.ComboBox cboRotorPort;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.TextBox txtDxCont;
        private System.Windows.Forms.TextBox txtDxDist;
        private System.Windows.Forms.TextBox txtDxLat;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.TextBox txtDxLong;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.TextBox txtDxCQ;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.TextBox txtDxITU;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox txtDxIOTA;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.TextBox txtDxTime;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label lblDxTime;
        private System.Windows.Forms.ComboBox cboPrefix;
        private System.Windows.Forms.ComboBox cboEntity;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.TextBox txtRegion;
        private System.Windows.Forms.Label lblLP;
        private System.Windows.Forms.RadioButton rbRtrMod7;
        private System.Windows.Forms.ComboBox cboRCP4Rotor;
        private System.Windows.Forms.ComboBox cboRCP3Rotor;
        private System.Windows.Forms.ComboBox cboRCP2Rotor;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.ToolStripMenuItem rotorControlToolStripMenuItem;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.ComboBox cboRCP1Rotor;
        private System.IO.Ports.SerialPort RCP1Rotor;
        private System.IO.Ports.SerialPort RCP2Rotor;
        private System.IO.Ports.SerialPort RCP3Rotor;
        private System.IO.Ports.SerialPort RCP4Rotor;
        private System.Windows.Forms.GroupBox grpStepCtrl;
        private System.Windows.Forms.RadioButton rb34;
        private System.Windows.Forms.RadioButton rbBiDir;
        private System.Windows.Forms.RadioButton rb180;
        private System.Windows.Forms.RadioButton rbFwd;
        private System.Windows.Forms.Button btnCalib;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.TextBox txtCall;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox chkTips;
        private System.Windows.Forms.CheckBox chkMode;
        private System.Windows.Forms.Label lblAvg;
        private System.Windows.Forms.TextBox txtAvg;
        private System.Windows.Forms.GroupBox grpWN2;
        private System.Windows.Forms.CheckBox chkWNEnab;
        private System.Windows.Forms.GroupBox grpPreset;
        private System.Windows.Forms.RadioButton rbPre16;
        private System.Windows.Forms.RadioButton rbPre8;
        private System.Windows.Forms.RadioButton rbPre15;
        private System.Windows.Forms.RadioButton rbPre7;
        private System.Windows.Forms.RadioButton rbPre14;
        private System.Windows.Forms.RadioButton rbPre6;
        private System.Windows.Forms.RadioButton rbPre13;
        private System.Windows.Forms.RadioButton rbPre5;
        private System.Windows.Forms.RadioButton rbPre12;
        private System.Windows.Forms.RadioButton rbPre4;
        private System.Windows.Forms.RadioButton rbPre11;
        private System.Windows.Forms.RadioButton rbPre3;
        private System.Windows.Forms.RadioButton rbPre10;
        private System.Windows.Forms.RadioButton rbPre2;
        private System.Windows.Forms.RadioButton rbPre9;
        private System.Windows.Forms.RadioButton rbPre1;
        private System.Windows.Forms.Button btnAlarm;
        private System.Windows.Forms.Button btnMode;
        private System.Windows.Forms.Button btnFast;
        private System.Windows.Forms.Label lblPower;
        private System.Windows.Forms.Label lblFast;
        private System.Windows.Forms.Label lblAlarm;
        private System.Windows.Forms.Timer WN2Timer;
        private System.Windows.Forms.GroupBox grpSlave;
        public System.Windows.Forms.CheckBox chkAudio;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox grpC1;
        private System.Windows.Forms.RadioButton rbC1Q;
        private System.Windows.Forms.RadioButton rbC1K;
        private System.Windows.Forms.RadioButton rbC1H;
        private System.Windows.Forms.GroupBox grpC4;
        private System.Windows.Forms.RadioButton rbC4K;
        private System.Windows.Forms.RadioButton rbC4H;
        private System.Windows.Forms.RadioButton rbC4Q;
        private System.Windows.Forms.GroupBox grpC3;
        private System.Windows.Forms.RadioButton rbC3K;
        private System.Windows.Forms.RadioButton rbC3H;
        private System.Windows.Forms.RadioButton rbC3Q;
        private System.Windows.Forms.GroupBox grpC2;
        private System.Windows.Forms.RadioButton rbC2K;
        private System.Windows.Forms.RadioButton rbC2H;
        private System.Windows.Forms.RadioButton rbC2Q;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ToolStripMenuItem waveNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMiniWindowToolStripMenuItem;
        public System.Windows.Forms.GroupBox grpModel;
        public System.IO.Ports.SerialPort RotorPort;
        public System.Windows.Forms.Label lblFwd;
        public System.Windows.Forms.TextBox txtProfLoc;
        public System.Windows.Forms.CheckBox chkDog;
        private System.Windows.Forms.TextBox txtTimer;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.GroupBox grpDog;
        private System.Windows.Forms.GroupBox grpALC;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnCal;
        private System.Windows.Forms.TextBox txtDrive;
        private System.Windows.Forms.Label label64;
        public System.Windows.Forms.CheckBox chkAlcEnab;
        private System.Windows.Forms.TextBox txtALC;
        private System.Windows.Forms.Label label68;
        public System.Windows.Forms.CheckBox chk20;
        public System.Windows.Forms.CheckBox chk160;
        private System.Windows.Forms.Button btnChkAll;
        private System.Windows.Forms.Button btnClrAll;
        public System.Windows.Forms.CheckBox chk10;
        public System.Windows.Forms.CheckBox chk30;
        public System.Windows.Forms.CheckBox chk12;
        public System.Windows.Forms.CheckBox chk40;
        public System.Windows.Forms.CheckBox chk15;
        public System.Windows.Forms.CheckBox chk17;
        public System.Windows.Forms.CheckBox chk80;
        public System.Windows.Forms.CheckBox chk6;
        public System.Windows.Forms.Label lblSP;
        private System.Windows.Forms.CheckBox chkModeChg;
        private System.Windows.Forms.CheckBox chkOnTop;
        private System.Windows.Forms.RadioButton rb100A;
        private System.Windows.Forms.RadioButton rb100;
        private System.Windows.Forms.ToolStripMenuItem clearErrorLogToolStripMenuItem;
        public System.Windows.Forms.TextBox txtSP;
        private System.Windows.Forms.ToolStripMenuItem setupWizardToolStripMenuItem;
        private TabPage tabMisc;
        private GroupBox grpAlpha;
        private CheckBox chkAlpha;
        private TextBox txtBand;
        private Label label71;
        private Label label72;
        private Label label73;
        private Label label75;
        private ComboBox cboAlpha;
        private Label label76;
        private Label label77;
        private Label label70;
        private TextBox txtAlphaInt;
        private Label lblSF;
        private Label label69;
        private TextBox txtSeg;
        private Label lblHF;
        public System.IO.Ports.SerialPort AlphaPort;
        public Button btnPwr;
        public Button btnOper;
        public Button btnTune;
        public Button btnHV;
        public TextBox txtMsg;
        public Button btnSF;
        public Button btnHF;
        private Label label78;
        private ComboBox cboAlphaBaud;
        private Label label80;
        private TextBox txtFreq;
        private TextBox txtLoad;
        private TextBox txtTune;
        private Label label74;
        private Label label79;
        private Button btnSensor;
        public RadioButton rbWN4;
        public RadioButton rbWN3;
        public RadioButton rbWN2;
        public RadioButton rbWN1;
        private TabPage tabExtCtrl;
        private GroupBox grpPortB;
        private GroupBox grpPortA;
        private CheckBox cb0r7;
        private CheckBox cb0r6;
        private CheckBox cb0r5;
        private CheckBox cb0r4;
        private CheckBox cb0r3;
        private CheckBox cb0r2;
        private CheckBox cb0r1;
        private CheckBox cb0r0;
        private Label label89;
        private Label label88;
        private Label label87;
        private Label label86;
        private Label label85;
        private Label label84;
        private Label label83;
        private Label label82;
        private Label label81;
        private Label label100;
        private CheckBox cb11r0;
        private CheckBox cb11r1;
        private CheckBox cb11r2;
        private CheckBox cb11r3;
        private CheckBox cb11r4;
        private CheckBox cb11r5;
        private CheckBox cb11r6;
        private CheckBox cb11r7;
        private Label label99;
        private CheckBox cb10r0;
        private CheckBox cb10r1;
        private CheckBox cb10r2;
        private CheckBox cb10r3;
        private CheckBox cb10r4;
        private CheckBox cb10r5;
        private CheckBox cb10r6;
        private CheckBox cb10r7;
        private Label label98;
        private CheckBox cb9r0;
        private CheckBox cb9r1;
        private CheckBox cb9r2;
        private CheckBox cb9r3;
        private CheckBox cb9r4;
        private CheckBox cb9r5;
        private CheckBox cb9r6;
        private CheckBox cb9r7;
        private Label label97;
        private CheckBox cb8r0;
        private CheckBox cb8r1;
        private CheckBox cb8r2;
        private CheckBox cb8r3;
        private CheckBox cb8r4;
        private CheckBox cb8r5;
        private CheckBox cb8r6;
        private CheckBox cb8r7;
        private Label label96;
        private CheckBox cb7r0;
        private CheckBox cb7r1;
        private CheckBox cb7r2;
        private CheckBox cb7r3;
        private CheckBox cb7r4;
        private CheckBox cb7r5;
        private CheckBox cb7r6;
        private CheckBox cb7r7;
        private Label label95;
        private CheckBox cb6r0;
        private CheckBox cb6r1;
        private CheckBox cb6r2;
        private CheckBox cb6r3;
        private CheckBox cb6r4;
        private CheckBox cb6r5;
        private CheckBox cb6r6;
        private CheckBox cb6r7;
        private Label label94;
        private CheckBox cb5r0;
        private CheckBox cb5r1;
        private CheckBox cb5r2;
        private CheckBox cb5r3;
        private CheckBox cb5r4;
        private CheckBox cb5r5;
        private CheckBox cb5r6;
        private CheckBox cb5r7;
        private Label label93;
        private CheckBox cb4r0;
        private CheckBox cb4r1;
        private CheckBox cb4r2;
        private CheckBox cb4r3;
        private CheckBox cb4r4;
        private CheckBox cb4r5;
        private CheckBox cb4r6;
        private CheckBox cb4r7;
        private Label label92;
        private CheckBox cb3r0;
        private CheckBox cb3r1;
        private CheckBox cb3r2;
        private CheckBox cb3r3;
        private CheckBox cb3r4;
        private CheckBox cb3r5;
        private CheckBox cb3r6;
        private CheckBox cb3r7;
        private Label label91;
        private CheckBox cb2r0;
        private CheckBox cb2r1;
        private CheckBox cb2r2;
        private CheckBox cb2r3;
        private CheckBox cb2r4;
        private CheckBox cb2r5;
        private CheckBox cb2r6;
        private CheckBox cb2r7;
        private Label label90;
        private CheckBox cb1r0;
        private CheckBox cb1r1;
        private CheckBox cb1r2;
        private CheckBox cb1r3;
        private CheckBox cb1r4;
        private CheckBox cb1r5;
        private CheckBox cb1r6;
        private CheckBox cb1r7;
        private Label label101;
        private CheckBox cb12r0;
        private CheckBox cb12r1;
        private CheckBox cb12r2;
        private CheckBox cb12r3;
        private CheckBox cb12r4;
        private CheckBox cb12r5;
        private CheckBox cb12r6;
        private CheckBox cb12r7;
        private Label label103;
        private Label label102;
        private CheckBox cb13r0;
        private CheckBox cb13r1;
        private CheckBox cb13r2;
        private CheckBox cb13r3;
        private CheckBox cb13r4;
        private CheckBox cb13r5;
        private CheckBox cb13r6;
        private CheckBox cb13r7;
        private TextBox txtPortA;
        private CheckBox chkPortA;
        private Label label104;
        private TextBox txtPortB;
        private Label label106;
        private Label label129;
        private Label label105;
        private CheckBox cb13t4;
        private Label label107;
        private CheckBox cb13t7;
        private CheckBox cb13t6;
        private CheckBox cb0t3;
        private CheckBox cb13t5;
        private CheckBox cb13t3;
        private CheckBox cb0t0;
        private CheckBox cb13t2;
        private CheckBox cb13t1;
        private CheckBox cb13t0;
        private CheckBox cb0t1;
        private Label label108;
        private CheckBox cb0t2;
        private CheckBox cb0t4;
        private CheckBox cb0t5;
        private CheckBox cb0t6;
        private CheckBox cb0t7;
        private Label label109;
        private CheckBox cb12t0;
        private CheckBox cb12t1;
        private CheckBox cb12t2;
        private CheckBox cb12t3;
        private CheckBox cb12t4;
        private CheckBox cb12t5;
        private CheckBox cb12t6;
        private CheckBox cb12t7;
        private Label label110;
        private CheckBox cb11t0;
        private CheckBox cb11t1;
        private CheckBox cb11t2;
        private CheckBox cb11t3;
        private CheckBox cb11t4;
        private CheckBox cb11t5;
        private CheckBox cb11t6;
        private CheckBox cb11t7;
        private Label label111;
        private CheckBox cb10t0;
        private CheckBox cb10t1;
        private CheckBox cb10t2;
        private CheckBox cb10t3;
        private CheckBox cb10t4;
        private CheckBox cb10t5;
        private CheckBox cb10t6;
        private CheckBox cb10t7;
        private Label label112;
        private CheckBox cb9t0;
        private CheckBox cb9t1;
        private CheckBox cb9t2;
        private CheckBox cb9t3;
        private CheckBox cb9t4;
        private CheckBox cb9t5;
        private CheckBox cb9t6;
        private CheckBox cb9t7;
        private Label label113;
        private CheckBox cb8t0;
        private CheckBox cb8t1;
        private CheckBox cb8t2;
        private CheckBox cb8t3;
        private CheckBox cb8t4;
        private CheckBox cb8t5;
        private CheckBox cb8t6;
        private CheckBox cb8t7;
        private Label label114;
        private CheckBox cb7t0;
        private CheckBox cb7t1;
        private CheckBox cb7t2;
        private CheckBox cb7t3;
        private CheckBox cb7t4;
        private CheckBox cb7t5;
        private CheckBox cb7t6;
        private CheckBox cb7t7;
        private Label label115;
        private CheckBox cb6t0;
        private CheckBox cb6t1;
        private CheckBox cb6t2;
        private CheckBox cb6t3;
        private CheckBox cb6t4;
        private CheckBox cb6t5;
        private CheckBox cb6t6;
        private CheckBox cb6t7;
        private Label label116;
        private CheckBox cb5t0;
        private CheckBox cb5t1;
        private CheckBox cb5t2;
        private CheckBox cb5t3;
        private CheckBox cb5t4;
        private CheckBox cb5t5;
        private CheckBox cb5t6;
        private CheckBox cb5t7;
        private Label label117;
        private CheckBox cb4t0;
        private CheckBox cb4t1;
        private CheckBox cb4t2;
        private CheckBox cb4t3;
        private CheckBox cb4t4;
        private CheckBox cb4t5;
        private CheckBox cb4t6;
        private CheckBox cb4t7;
        private Label label118;
        private CheckBox cb3t0;
        private CheckBox cb3t1;
        private CheckBox cb3t2;
        private CheckBox cb3t3;
        private CheckBox cb3t4;
        private CheckBox cb3t5;
        private CheckBox cb3t6;
        private CheckBox cb3t7;
        private Label label119;
        private CheckBox cb2t0;
        private CheckBox cb2t1;
        private CheckBox cb2t2;
        private CheckBox cb2t3;
        private CheckBox cb2t4;
        private CheckBox cb2t5;
        private CheckBox cb2t6;
        private CheckBox cb2t7;
        private Label label120;
        private CheckBox cb1t0;
        private CheckBox cb1t1;
        private CheckBox cb1t2;
        private CheckBox cb1t3;
        private CheckBox cb1t4;
        private CheckBox cb1t5;
        private CheckBox cb1t6;
        private CheckBox cb1t7;
        private Label label121;
        private Label label122;
        private Label label123;
        private Label label124;
        private Label label125;
        private Label label126;
        private Label label127;
        private Label label128;
        private Label label130;
        private Button btnClrPortB;
        private Button btnClrPortA;
        private CheckBox chkPortB;
        private Label label132;
        private Label label131;
        private CheckBox cb15t4;
        private CheckBox cb15t7;
        private CheckBox cb15t6;
        private CheckBox cb15t5;
        private CheckBox cb15t3;
        private CheckBox cb15t2;
        private CheckBox cb15t1;
        private CheckBox cb15t0;
        private Label label135;
        private Label label136;
        private CheckBox cb14t0;
        private CheckBox cb14t1;
        private CheckBox cb14t2;
        private CheckBox cb14t3;
        private CheckBox cb14t4;
        private CheckBox cb14t5;
        private CheckBox cb14t6;
        private CheckBox cb14t7;
        private CheckBox cb15r4;
        private CheckBox cb15r7;
        private CheckBox cb15r6;
        private CheckBox cb15r5;
        private CheckBox cb15r3;
        private CheckBox cb15r2;
        private CheckBox cb15r1;
        private CheckBox cb15r0;
        private Label label133;
        private Label label134;
        private CheckBox cb14r0;
        private CheckBox cb14r1;
        private CheckBox cb14r2;
        private CheckBox cb14r3;
        private CheckBox cb14r4;
        private CheckBox cb14r5;
        private CheckBox cb14r6;
        private CheckBox cb14r7;
        private Label lblVHF;
        private Label label137;

    }
}

