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
            this.chkPSDR = new System.Windows.Forms.CheckBox();
            this.btnFlexOn = new System.Windows.Forms.Button();
            this.grpSlave = new System.Windows.Forms.GroupBox();
            this.chkSlaveRTS = new System.Windows.Forms.CheckBox();
            this.chkSlaveDTR = new System.Windows.Forms.CheckBox();
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
            this.Dev0 = new System.Windows.Forms.TextBox();
            this.chkDSInvert = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkDevice = new System.Windows.Forms.CheckBox();
            this.grpLPT = new System.Windows.Forms.GroupBox();
            this.rbFW = new System.Windows.Forms.RadioButton();
            this.rbOther = new System.Windows.Forms.RadioButton();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.rb4 = new System.Windows.Forms.RadioButton();
            this.rb2 = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rb3 = new System.Windows.Forms.RadioButton();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.tabBCD = new System.Windows.Forms.TabPage();
            this.pixBox1 = new System.Windows.Forms.PictureBox();
            this.grpAuxBcdControl = new System.Windows.Forms.GroupBox();
            this.chkAuxMacro = new System.Windows.Forms.CheckBox();
            this.chkFollowFreq = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkAuxEnab = new System.Windows.Forms.CheckBox();
            this.chkAuxBit7 = new System.Windows.Forms.CheckBox();
            this.chkAuxBit8 = new System.Windows.Forms.CheckBox();
            this.chkAuxBit5 = new System.Windows.Forms.CheckBox();
            this.chkAuxBit3 = new System.Windows.Forms.CheckBox();
            this.chkAuxBit6 = new System.Windows.Forms.CheckBox();
            this.chkAuxBit1 = new System.Windows.Forms.CheckBox();
            this.chkAuxBit4 = new System.Windows.Forms.CheckBox();
            this.chkAuxBit2 = new System.Windows.Forms.CheckBox();
            this.grpBCDover = new System.Windows.Forms.GroupBox();
            this.rbOvr12 = new System.Windows.Forms.RadioButton();
            this.rbOvr11 = new System.Windows.Forms.RadioButton();
            this.rbOvr10 = new System.Windows.Forms.RadioButton();
            this.rbOvr9 = new System.Windows.Forms.RadioButton();
            this.rbOvr8 = new System.Windows.Forms.RadioButton();
            this.rbOvr4 = new System.Windows.Forms.RadioButton();
            this.rbOvr7 = new System.Windows.Forms.RadioButton();
            this.rbOvr3 = new System.Windows.Forms.RadioButton();
            this.rbOvr6 = new System.Windows.Forms.RadioButton();
            this.rbOvr2 = new System.Windows.Forms.RadioButton();
            this.rbOvr5 = new System.Windows.Forms.RadioButton();
            this.rbOvr1 = new System.Windows.Forms.RadioButton();
            this.chkOvride = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave0 = new System.Windows.Forms.Button();
            this.btnReLoad0 = new System.Windows.Forms.Button();
            this.btnFile0 = new System.Windows.Forms.Button();
            this.txtFile0 = new System.Windows.Forms.TextBox();
            this.dg1 = new System.Windows.Forms.DataGridView();
            this.chkDev0 = new System.Windows.Forms.CheckBox();
            this.tabRCP = new System.Windows.Forms.TabPage();
            this.grpTKnob = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.lblZE = new System.Windows.Forms.Label();
            this.txtZE = new System.Windows.Forms.TextBox();
            this.label232 = new System.Windows.Forms.Label();
            this.label231 = new System.Windows.Forms.Label();
            this.label230 = new System.Windows.Forms.Label();
            this.label229 = new System.Windows.Forms.Label();
            this.cboTkSw3 = new System.Windows.Forms.ComboBox();
            this.cboTkSw1 = new System.Windows.Forms.ComboBox();
            this.cboTkSw2 = new System.Windows.Forms.ComboBox();
            this.label228 = new System.Windows.Forms.Label();
            this.lblZR = new System.Windows.Forms.Label();
            this.cboTstep = new System.Windows.Forms.ComboBox();
            this.txtZR = new System.Windows.Forms.TextBox();
            this.lblZL = new System.Windows.Forms.Label();
            this.txtZL = new System.Windows.Forms.TextBox();
            this.lblZC = new System.Windows.Forms.Label();
            this.txtZC = new System.Windows.Forms.TextBox();
            this.chkKnobExpert = new System.Windows.Forms.CheckBox();
            this.cboKnobBDC = new System.Windows.Forms.ComboBox();
            this.cboKnobBOn = new System.Windows.Forms.ComboBox();
            this.cboKnobBOff = new System.Windows.Forms.ComboBox();
            this.label221 = new System.Windows.Forms.Label();
            this.label220 = new System.Windows.Forms.Label();
            this.label219 = new System.Windows.Forms.Label();
            this.label218 = new System.Windows.Forms.Label();
            this.label217 = new System.Windows.Forms.Label();
            this.cboKnobADC = new System.Windows.Forms.ComboBox();
            this.cboKnobAOn = new System.Windows.Forms.ComboBox();
            this.cboKnobAOff = new System.Windows.Forms.ComboBox();
            this.label216 = new System.Windows.Forms.Label();
            this.cboKnobPort = new System.Windows.Forms.ComboBox();
            this.chkKnobEnab = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label227 = new System.Windows.Forms.Label();
            this.label154 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.cboRCP4Rotor = new System.Windows.Forms.ComboBox();
            this.cboRCP3Rotor = new System.Windows.Forms.ComboBox();
            this.cboRCP2Rotor = new System.Windows.Forms.ComboBox();
            this.chkRCP2 = new System.Windows.Forms.CheckBox();
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
            this.txtRCP = new System.Windows.Forms.TextBox();
            this.tabWatt = new System.Windows.Forms.TabPage();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.chkSWR = new System.Windows.Forms.CheckBox();
            this.numSWR = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grpNet = new System.Windows.Forms.GroupBox();
            this.rbNet = new System.Windows.Forms.RadioButton();
            this.rbStd = new System.Windows.Forms.RadioButton();
            this.numRev = new System.Windows.Forms.NumericUpDown();
            this.numFwd = new System.Windows.Forms.NumericUpDown();
            this.cboPMcom = new System.Windows.Forms.ComboBox();
            this.label142 = new System.Windows.Forms.Label();
            this.label138 = new System.Windows.Forms.Label();
            this.label139 = new System.Windows.Forms.Label();
            this.cboPMport = new System.Windows.Forms.ComboBox();
            this.chkPM = new System.Windows.Forms.CheckBox();
            this.label140 = new System.Windows.Forms.Label();
            this.label141 = new System.Windows.Forms.Label();
            this.grpWN2 = new System.Windows.Forms.GroupBox();
            this.btnWnSensor = new System.Windows.Forms.Button();
            this.btnWnReset = new System.Windows.Forms.Button();
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
            this.tabAmp = new System.Windows.Forms.TabPage();
            this.grpSPE = new System.Windows.Forms.GroupBox();
            this.btnSPEright = new System.Windows.Forms.Button();
            this.btnSPEset = new System.Windows.Forms.Button();
            this.btnSPEleft = new System.Windows.Forms.Button();
            this.rbLed = new System.Windows.Forms.RadioButton();
            this.label161 = new System.Windows.Forms.Label();
            this.txtSPEant = new System.Windows.Forms.TextBox();
            this.btnSPEant = new System.Windows.Forms.Button();
            this.btnSPEdisp = new System.Windows.Forms.Button();
            this.btnSPEoff = new System.Windows.Forms.Button();
            this.label157 = new System.Windows.Forms.Label();
            this.btnSPEpwr = new System.Windows.Forms.Button();
            this.txtSPEmsg = new System.Windows.Forms.TextBox();
            this.btnSPEtune = new System.Windows.Forms.Button();
            this.label158 = new System.Windows.Forms.Label();
            this.cboSPEport = new System.Windows.Forms.ComboBox();
            this.chkSPEenab = new System.Windows.Forms.CheckBox();
            this.txtSPEtemp = new System.Windows.Forms.TextBox();
            this.btnSPEoper = new System.Windows.Forms.Button();
            this.btnSPEon = new System.Windows.Forms.Button();
            this.grpAmpBand = new System.Windows.Forms.GroupBox();
            this.label148 = new System.Windows.Forms.Label();
            this.numDrive = new System.Windows.Forms.NumericUpDown();
            this.btnInfo = new System.Windows.Forms.Button();
            this.chkB80 = new System.Windows.Forms.CheckBox();
            this.btnBandClear = new System.Windows.Forms.Button();
            this.btnBandAll = new System.Windows.Forms.Button();
            this.chkB160 = new System.Windows.Forms.CheckBox();
            this.chkB30 = new System.Windows.Forms.CheckBox();
            this.chkB17 = new System.Windows.Forms.CheckBox();
            this.chkB40 = new System.Windows.Forms.CheckBox();
            this.chkB10 = new System.Windows.Forms.CheckBox();
            this.chkB12 = new System.Windows.Forms.CheckBox();
            this.chkB20 = new System.Windows.Forms.CheckBox();
            this.chkB15 = new System.Windows.Forms.CheckBox();
            this.grpPTT = new System.Windows.Forms.GroupBox();
            this.btnByp = new System.Windows.Forms.Button();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label224 = new System.Windows.Forms.Label();
            this.label223 = new System.Windows.Forms.Label();
            this.label222 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.txtDigi = new System.Windows.Forms.TextBox();
            this.txtSSB = new System.Windows.Forms.TextBox();
            this.txtAM = new System.Windows.Forms.TextBox();
            this.chkAutoExpert = new System.Windows.Forms.CheckBox();
            this.label151 = new System.Windows.Forms.Label();
            this.chkAutoDrv = new System.Windows.Forms.CheckBox();
            this.btnDrive = new System.Windows.Forms.Button();
            this.grpAmp = new System.Windows.Forms.GroupBox();
            this.lblLoad = new System.Windows.Forms.Label();
            this.txtLoad = new System.Windows.Forms.TextBox();
            this.txtTune = new System.Windows.Forms.TextBox();
            this.lblTune = new System.Windows.Forms.Label();
            this.label80 = new System.Windows.Forms.Label();
            this.txtFreq = new System.Windows.Forms.TextBox();
            this.label78 = new System.Windows.Forms.Label();
            this.cboAlphaBaud = new System.Windows.Forms.ComboBox();
            this.btnHF = new System.Windows.Forms.Button();
            this.btnSF = new System.Windows.Forms.Button();
            this.lblHF = new System.Windows.Forms.Label();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.lblSF = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.txtSeg = new System.Windows.Forms.TextBox();
            this.lblAmpInt = new System.Windows.Forms.Label();
            this.txtAlphaInt = new System.Windows.Forms.TextBox();
            this.btnHV = new System.Windows.Forms.Button();
            this.label77 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.cboAlpha = new System.Windows.Forms.ComboBox();
            this.label73 = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.label72 = new System.Windows.Forms.Label();
            this.btnOper = new System.Windows.Forms.Button();
            this.btnTune = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label71 = new System.Windows.Forms.Label();
            this.txtBand = new System.Windows.Forms.TextBox();
            this.chkAlpha = new System.Windows.Forms.CheckBox();
            this.btnPwr = new System.Windows.Forms.Button();
            this.grpAmpBands = new System.Windows.Forms.GroupBox();
            this.btnClrAll = new System.Windows.Forms.Button();
            this.chkAmp6 = new System.Windows.Forms.CheckBox();
            this.chkAmp80 = new System.Windows.Forms.CheckBox();
            this.chkAmp160 = new System.Windows.Forms.CheckBox();
            this.chkAmp30 = new System.Windows.Forms.CheckBox();
            this.btnCkAll = new System.Windows.Forms.Button();
            this.chkAmp17 = new System.Windows.Forms.CheckBox();
            this.chkAmp40 = new System.Windows.Forms.CheckBox();
            this.chkAmp10 = new System.Windows.Forms.CheckBox();
            this.chkAmp12 = new System.Windows.Forms.CheckBox();
            this.chkAmp20 = new System.Windows.Forms.CheckBox();
            this.chkAmp15 = new System.Windows.Forms.CheckBox();
            this.tabOther = new System.Windows.Forms.TabPage();
            this.grpMacSw = new System.Windows.Forms.GroupBox();
            this.txtMacSwMsg = new System.Windows.Forms.TextBox();
            this.txtMacSwOff = new System.Windows.Forms.TextBox();
            this.label226 = new System.Windows.Forms.Label();
            this.txtMacSwOn = new System.Windows.Forms.TextBox();
            this.label225 = new System.Windows.Forms.Label();
            this.label155 = new System.Windows.Forms.Label();
            this.chkMacSwEnab = new System.Windows.Forms.CheckBox();
            this.cboMacSwPort = new System.Windows.Forms.ComboBox();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.txtVspMgr = new System.Windows.Forms.TextBox();
            this.grpRepeat = new System.Windows.Forms.GroupBox();
            this.label79 = new System.Windows.Forms.Label();
            this.label147 = new System.Windows.Forms.Label();
            this.cboRepeatCom = new System.Windows.Forms.ComboBox();
            this.grpRmode = new System.Windows.Forms.GroupBox();
            this.rbRptPal = new System.Windows.Forms.RadioButton();
            this.rbRptNone = new System.Windows.Forms.RadioButton();
            this.rbIFonly = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.cboRepeatPort = new System.Windows.Forms.ComboBox();
            this.chkRepeat = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.cboStepCom = new System.Windows.Forms.ComboBox();
            this.cboStep = new System.Windows.Forms.ComboBox();
            this.txtStepInv = new System.Windows.Forms.TextBox();
            this.chkStep = new System.Windows.Forms.CheckBox();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.grpPro = new System.Windows.Forms.GroupBox();
            this.button6 = new System.Windows.Forms.Button();
            this.label153 = new System.Windows.Forms.Label();
            this.txtPmsg = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label152 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label149 = new System.Windows.Forms.Label();
            this.label150 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.grpInP = new System.Windows.Forms.GroupBox();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.grpAntP = new System.Windows.Forms.GroupBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.txtSleep = new System.Windows.Forms.TextBox();
            this.lblSleep = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkPwRTS = new System.Windows.Forms.CheckBox();
            this.chkPwDTR = new System.Windows.Forms.CheckBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.chkDisBcast = new System.Windows.Forms.CheckBox();
            this.label24 = new System.Windows.Forms.Label();
            this.txtPW1ra = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.cboPW1 = new System.Windows.Forms.ComboBox();
            this.txtPW1ta = new System.Windows.Forms.TextBox();
            this.chkPW1 = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.tabMacro = new System.Windows.Forms.TabPage();
            this.label146 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label143 = new System.Windows.Forms.Label();
            this.label144 = new System.Windows.Forms.Label();
            this.label145 = new System.Windows.Forms.Label();
            this.btnMacro16 = new System.Windows.Forms.Button();
            this.btnMacro15 = new System.Windows.Forms.Button();
            this.btnMacro13 = new System.Windows.Forms.Button();
            this.btnMacro14 = new System.Windows.Forms.Button();
            this.label34 = new System.Windows.Forms.Label();
            this.grpMacro = new System.Windows.Forms.GroupBox();
            this.btnMacSelect = new System.Windows.Forms.Button();
            this.btnMacSave = new System.Windows.Forms.Button();
            this.btnMacReLoad = new System.Windows.Forms.Button();
            this.label26 = new System.Windows.Forms.Label();
            this.txtMacFile = new System.Windows.Forms.TextBox();
            this.dgm = new System.Windows.Forms.DataGridView();
            this.label33 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.btnMacro6 = new System.Windows.Forms.Button();
            this.label38 = new System.Windows.Forms.Label();
            this.btnMacro5 = new System.Windows.Forms.Button();
            this.btnMacro4 = new System.Windows.Forms.Button();
            this.label39 = new System.Windows.Forms.Label();
            this.btnMacro3 = new System.Windows.Forms.Button();
            this.btnMacro2 = new System.Windows.Forms.Button();
            this.btnMacro12 = new System.Windows.Forms.Button();
            this.btnMacro1 = new System.Windows.Forms.Button();
            this.btnMacro7 = new System.Windows.Forms.Button();
            this.btnMacro11 = new System.Windows.Forms.Button();
            this.btnMacro8 = new System.Windows.Forms.Button();
            this.btnMacro9 = new System.Windows.Forms.Button();
            this.btnMacro10 = new System.Windows.Forms.Button();
            this.tabRotor = new System.Windows.Forms.TabPage();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.txtKm = new System.Windows.Forms.TextBox();
            this.txtSm = new System.Windows.Forms.TextBox();
            this.txtLoc = new System.Windows.Forms.TextBox();
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
            this.lblStepFreq = new System.Windows.Forms.Label();
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
            this.label40 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.txtCallSign = new System.Windows.Forms.TextBox();
            this.txtRegion = new System.Windows.Forms.TextBox();
            this.cboEntity = new System.Windows.Forms.ComboBox();
            this.label47 = new System.Windows.Forms.Label();
            this.cboPrefix = new System.Windows.Forms.ComboBox();
            this.label45 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblLP = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.lblSP = new System.Windows.Forms.Label();
            this.btnSP = new System.Windows.Forms.Button();
            this.txtLP = new System.Windows.Forms.TextBox();
            this.btnLP = new System.Windows.Forms.Button();
            this.txtSP = new System.Windows.Forms.TextBox();
            this.tabExtCtrl = new System.Windows.Forms.TabPage();
            this.chkFWb = new System.Windows.Forms.CheckBox();
            this.chkFWa = new System.Windows.Forms.CheckBox();
            this.chkInvertB = new System.Windows.Forms.CheckBox();
            this.chkInvertA = new System.Windows.Forms.CheckBox();
            this.label137 = new System.Windows.Forms.Label();
            this.lblVHF = new System.Windows.Forms.Label();
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
            this.tabSetup = new System.Windows.Forms.TabPage();
            this.chkShortCut = new System.Windows.Forms.CheckBox();
            this.chkCwTx = new System.Windows.Forms.CheckBox();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.txtPSDR = new System.Windows.Forms.TextBox();
            this.label68 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.cboPwrPort = new System.Windows.Forms.ComboBox();
            this.chkOnTop = new System.Windows.Forms.CheckBox();
            this.chkNoBdChg = new System.Windows.Forms.CheckBox();
            this.chkModeChg = new System.Windows.Forms.CheckBox();
            this.chkAudio = new System.Windows.Forms.CheckBox();
            this.chkTips = new System.Windows.Forms.CheckBox();
            this.grpDog = new System.Windows.Forms.GroupBox();
            this.txtTimer = new System.Windows.Forms.TextBox();
            this.label67 = new System.Windows.Forms.Label();
            this.chkDog = new System.Windows.Forms.CheckBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.chkStepDipole = new System.Windows.Forms.CheckBox();
            this.txtAzOffset = new System.Windows.Forms.TextBox();
            this.label156 = new System.Windows.Forms.Label();
            this.chkTenths = new System.Windows.Forms.CheckBox();
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
            this.tabProp = new System.Windows.Forms.TabPage();
            this.grpSO2R = new System.Windows.Forms.GroupBox();
            this.btnSO2Rclear = new System.Windows.Forms.Button();
            this.btnLoadSO2R = new System.Windows.Forms.Button();
            this.label215 = new System.Windows.Forms.Label();
            this.txtTxB = new System.Windows.Forms.TextBox();
            this.label214 = new System.Windows.Forms.Label();
            this.txtTxA = new System.Windows.Forms.TextBox();
            this.label202 = new System.Windows.Forms.Label();
            this.label204 = new System.Windows.Forms.Label();
            this.label206 = new System.Windows.Forms.Label();
            this.label207 = new System.Windows.Forms.Label();
            this.label208 = new System.Windows.Forms.Label();
            this.label209 = new System.Windows.Forms.Label();
            this.label210 = new System.Windows.Forms.Label();
            this.label211 = new System.Windows.Forms.Label();
            this.label212 = new System.Windows.Forms.Label();
            this.label213 = new System.Windows.Forms.Label();
            this.txtHi2_5a = new System.Windows.Forms.TextBox();
            this.chkSO10b = new System.Windows.Forms.CheckBox();
            this.chkSO9b = new System.Windows.Forms.CheckBox();
            this.chkSO8b = new System.Windows.Forms.CheckBox();
            this.chkSO7b = new System.Windows.Forms.CheckBox();
            this.chkSO6b = new System.Windows.Forms.CheckBox();
            this.chkSO5b = new System.Windows.Forms.CheckBox();
            this.chkSO4b = new System.Windows.Forms.CheckBox();
            this.chkSO3b = new System.Windows.Forms.CheckBox();
            this.chkSO2b = new System.Windows.Forms.CheckBox();
            this.chkSO1b = new System.Windows.Forms.CheckBox();
            this.label200 = new System.Windows.Forms.Label();
            this.chk1Amp = new System.Windows.Forms.CheckBox();
            this.label205 = new System.Windows.Forms.Label();
            this.label203 = new System.Windows.Forms.Label();
            this.txtLPT2 = new System.Windows.Forms.TextBox();
            this.btnSaveSO2R = new System.Windows.Forms.Button();
            this.label184 = new System.Windows.Forms.Label();
            this.txtLo1_10b = new System.Windows.Forms.TextBox();
            this.txtLo1_9b = new System.Windows.Forms.TextBox();
            this.txtLo1_8b = new System.Windows.Forms.TextBox();
            this.txtLo1_7b = new System.Windows.Forms.TextBox();
            this.txtLo1_6b = new System.Windows.Forms.TextBox();
            this.txtLo1_5b = new System.Windows.Forms.TextBox();
            this.txtLo1_4b = new System.Windows.Forms.TextBox();
            this.cboSwPort = new System.Windows.Forms.ComboBox();
            this.txtLo1_3b = new System.Windows.Forms.TextBox();
            this.txtLo1_2b = new System.Windows.Forms.TextBox();
            this.chkSoEnab = new System.Windows.Forms.CheckBox();
            this.txtLo1_1b = new System.Windows.Forms.TextBox();
            this.label201 = new System.Windows.Forms.Label();
            this.label199 = new System.Windows.Forms.Label();
            this.txtLo2_10b = new System.Windows.Forms.TextBox();
            this.txtTx_10b = new System.Windows.Forms.TextBox();
            this.txtPwr_10b = new System.Windows.Forms.TextBox();
            this.txtRx_10b = new System.Windows.Forms.TextBox();
            this.label198 = new System.Windows.Forms.Label();
            this.txtLo2_9b = new System.Windows.Forms.TextBox();
            this.txtTx_9b = new System.Windows.Forms.TextBox();
            this.txtPwr_9b = new System.Windows.Forms.TextBox();
            this.txtRx_9b = new System.Windows.Forms.TextBox();
            this.label197 = new System.Windows.Forms.Label();
            this.txtLo2_8b = new System.Windows.Forms.TextBox();
            this.txtTx_8b = new System.Windows.Forms.TextBox();
            this.txtPwr_8b = new System.Windows.Forms.TextBox();
            this.txtRx_8b = new System.Windows.Forms.TextBox();
            this.label196 = new System.Windows.Forms.Label();
            this.txtLo2_7b = new System.Windows.Forms.TextBox();
            this.txtTx_7b = new System.Windows.Forms.TextBox();
            this.txtPwr_7b = new System.Windows.Forms.TextBox();
            this.txtRx_7b = new System.Windows.Forms.TextBox();
            this.label195 = new System.Windows.Forms.Label();
            this.txtLo2_6b = new System.Windows.Forms.TextBox();
            this.txtTx_6b = new System.Windows.Forms.TextBox();
            this.txtPwr_6b = new System.Windows.Forms.TextBox();
            this.txtRx_6b = new System.Windows.Forms.TextBox();
            this.label194 = new System.Windows.Forms.Label();
            this.txtLo2_5b = new System.Windows.Forms.TextBox();
            this.txtTx_5b = new System.Windows.Forms.TextBox();
            this.txtPwr_5b = new System.Windows.Forms.TextBox();
            this.txtRx_5b = new System.Windows.Forms.TextBox();
            this.label193 = new System.Windows.Forms.Label();
            this.txtLo2_4b = new System.Windows.Forms.TextBox();
            this.txtTx_4b = new System.Windows.Forms.TextBox();
            this.txtPwr_4b = new System.Windows.Forms.TextBox();
            this.txtRx_4b = new System.Windows.Forms.TextBox();
            this.label192 = new System.Windows.Forms.Label();
            this.txtLo2_3b = new System.Windows.Forms.TextBox();
            this.txtTx_3b = new System.Windows.Forms.TextBox();
            this.txtPwr_3b = new System.Windows.Forms.TextBox();
            this.txtRx_3b = new System.Windows.Forms.TextBox();
            this.label191 = new System.Windows.Forms.Label();
            this.txtLo2_2b = new System.Windows.Forms.TextBox();
            this.txtTx_2b = new System.Windows.Forms.TextBox();
            this.txtPwr_2b = new System.Windows.Forms.TextBox();
            this.txtRx_2b = new System.Windows.Forms.TextBox();
            this.label190 = new System.Windows.Forms.Label();
            this.label189 = new System.Windows.Forms.Label();
            this.label185 = new System.Windows.Forms.Label();
            this.txtLo2_1b = new System.Windows.Forms.TextBox();
            this.label186 = new System.Windows.Forms.Label();
            this.txtTx_1b = new System.Windows.Forms.TextBox();
            this.label187 = new System.Windows.Forms.Label();
            this.label188 = new System.Windows.Forms.Label();
            this.txtPwr_1b = new System.Windows.Forms.TextBox();
            this.txtRx_1b = new System.Windows.Forms.TextBox();
            this.label183 = new System.Windows.Forms.Label();
            this.txtHi2_10a = new System.Windows.Forms.TextBox();
            this.txtTx_10a = new System.Windows.Forms.TextBox();
            this.txtPwr_10a = new System.Windows.Forms.TextBox();
            this.txtRx_10a = new System.Windows.Forms.TextBox();
            this.txtHi1_10a = new System.Windows.Forms.TextBox();
            this.txtLPT1_10 = new System.Windows.Forms.TextBox();
            this.label179 = new System.Windows.Forms.Label();
            this.chkSO10a = new System.Windows.Forms.CheckBox();
            this.txtHi2_9a = new System.Windows.Forms.TextBox();
            this.txtTx_9a = new System.Windows.Forms.TextBox();
            this.txtPwr_9a = new System.Windows.Forms.TextBox();
            this.txtRx_9a = new System.Windows.Forms.TextBox();
            this.txtHi1_9a = new System.Windows.Forms.TextBox();
            this.txtLPT1_9 = new System.Windows.Forms.TextBox();
            this.label180 = new System.Windows.Forms.Label();
            this.chkSO9a = new System.Windows.Forms.CheckBox();
            this.txtHi2_8a = new System.Windows.Forms.TextBox();
            this.txtTx_8a = new System.Windows.Forms.TextBox();
            this.txtPwr_8a = new System.Windows.Forms.TextBox();
            this.txtRx_8a = new System.Windows.Forms.TextBox();
            this.txtHi1_8a = new System.Windows.Forms.TextBox();
            this.txtLPT1_8 = new System.Windows.Forms.TextBox();
            this.label181 = new System.Windows.Forms.Label();
            this.chkSO8a = new System.Windows.Forms.CheckBox();
            this.txtHi2_7a = new System.Windows.Forms.TextBox();
            this.txtTx_7a = new System.Windows.Forms.TextBox();
            this.txtPwr_7a = new System.Windows.Forms.TextBox();
            this.txtRx_7a = new System.Windows.Forms.TextBox();
            this.txtHi1_7a = new System.Windows.Forms.TextBox();
            this.txtLPT1_7 = new System.Windows.Forms.TextBox();
            this.label182 = new System.Windows.Forms.Label();
            this.chkSO7a = new System.Windows.Forms.CheckBox();
            this.txtHi2_6a = new System.Windows.Forms.TextBox();
            this.txtTx_6a = new System.Windows.Forms.TextBox();
            this.txtPwr_6a = new System.Windows.Forms.TextBox();
            this.txtRx_6a = new System.Windows.Forms.TextBox();
            this.txtHi1_6a = new System.Windows.Forms.TextBox();
            this.txtLPT1_6 = new System.Windows.Forms.TextBox();
            this.label178 = new System.Windows.Forms.Label();
            this.chkSO6a = new System.Windows.Forms.CheckBox();
            this.txtTx_5a = new System.Windows.Forms.TextBox();
            this.txtPwr_5a = new System.Windows.Forms.TextBox();
            this.txtRx_5a = new System.Windows.Forms.TextBox();
            this.txtHi1_5a = new System.Windows.Forms.TextBox();
            this.txtLPT1_5 = new System.Windows.Forms.TextBox();
            this.label177 = new System.Windows.Forms.Label();
            this.chkSO5a = new System.Windows.Forms.CheckBox();
            this.txtHi2_4a = new System.Windows.Forms.TextBox();
            this.txtTx_4a = new System.Windows.Forms.TextBox();
            this.txtPwr_4a = new System.Windows.Forms.TextBox();
            this.txtRx_4a = new System.Windows.Forms.TextBox();
            this.txtHi1_4a = new System.Windows.Forms.TextBox();
            this.txtLPT1_4 = new System.Windows.Forms.TextBox();
            this.label176 = new System.Windows.Forms.Label();
            this.chkSO4a = new System.Windows.Forms.CheckBox();
            this.txtHi2_3a = new System.Windows.Forms.TextBox();
            this.txtTx_3a = new System.Windows.Forms.TextBox();
            this.txtPwr_3a = new System.Windows.Forms.TextBox();
            this.txtRx_3a = new System.Windows.Forms.TextBox();
            this.txtHi1_3a = new System.Windows.Forms.TextBox();
            this.txtLPT1_3 = new System.Windows.Forms.TextBox();
            this.label175 = new System.Windows.Forms.Label();
            this.chkSO3a = new System.Windows.Forms.CheckBox();
            this.txtHi2_2a = new System.Windows.Forms.TextBox();
            this.txtTx_2a = new System.Windows.Forms.TextBox();
            this.txtPwr_2a = new System.Windows.Forms.TextBox();
            this.txtRx_2a = new System.Windows.Forms.TextBox();
            this.txtHi1_2a = new System.Windows.Forms.TextBox();
            this.txtLPT1_2 = new System.Windows.Forms.TextBox();
            this.label168 = new System.Windows.Forms.Label();
            this.chkSO2a = new System.Windows.Forms.CheckBox();
            this.label174 = new System.Windows.Forms.Label();
            this.txtHi2_1a = new System.Windows.Forms.TextBox();
            this.label173 = new System.Windows.Forms.Label();
            this.txtTx_1a = new System.Windows.Forms.TextBox();
            this.label172 = new System.Windows.Forms.Label();
            this.label170 = new System.Windows.Forms.Label();
            this.txtPwr_1a = new System.Windows.Forms.TextBox();
            this.txtRx_1a = new System.Windows.Forms.TextBox();
            this.txtHi1_1a = new System.Windows.Forms.TextBox();
            this.label167 = new System.Windows.Forms.Label();
            this.txtLPT1_1 = new System.Windows.Forms.TextBox();
            this.label166 = new System.Windows.Forms.Label();
            this.label169 = new System.Windows.Forms.Label();
            this.chkSO1a = new System.Windows.Forms.CheckBox();
            this.label171 = new System.Windows.Forms.Label();
            this.lblBCD = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.AccPort = new System.IO.Ports.SerialPort(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtFwd = new System.Windows.Forms.TextBox();
            this.txtSWR = new System.Windows.Forms.TextBox();
            this.txtTemp = new System.Windows.Forms.TextBox();
            this.lblAnt = new System.Windows.Forms.Label();
            this.txtAvg = new System.Windows.Forms.TextBox();
            this.btnSplit = new System.Windows.Forms.Button();
            this.numSplit = new System.Windows.Forms.NumericUpDown();
            this.txtAlcInd = new System.Windows.Forms.TextBox();
            this.btnFZ = new System.Windows.Forms.Button();
            this.lblHighSWR = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.txtVolts = new System.Windows.Forms.TextBox();
            this.btnReCall = new System.Windows.Forms.Button();
            this.btnMemSave = new System.Windows.Forms.Button();
            this.btnMV = new System.Windows.Forms.Button();
            this.btnMemClear = new System.Windows.Forms.Button();
            this.txtMemFreq = new System.Windows.Forms.TextBox();
            this.txtMemMode = new System.Windows.Forms.TextBox();
            this.btnMemLoad = new System.Windows.Forms.Button();
            this.txtMemIdx = new System.Windows.Forms.TextBox();
            this.txtMemVfo = new System.Windows.Forms.TextBox();
            this.TkRed = new System.Windows.Forms.Button();
            this.LogPort = new System.IO.Ports.SerialPort(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.autoDriveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barefootSettingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.enableExpertModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.errorLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableLoggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openErrorLogToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.clearErrorLogToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slaveRadioInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iCOMICPW1ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lP100ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.macroCommandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rCPPortsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.rotorControlToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.waveNodeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.shortCutKeysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.getLatestVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userForumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
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
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoDriveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ampSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barefootSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.tubeAmpsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aCOM2000AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alpha9500ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.acomAntDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.antToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableErrorLoggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openErrorLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearErrorLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.resetDontAskMeAgainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.showMiniWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.vSPManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onLineHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupWizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTipsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.updateToLatestVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userForumToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
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
            this.PMport = new System.IO.Ports.SerialPort(this.components);
            this.RepeatPort = new System.IO.Ports.SerialPort(this.components);
            this.PwrPort = new System.IO.Ports.SerialPort(this.components);
            this.pdTimer = new System.Windows.Forms.Timer(this.components);
            this.SPEport = new System.IO.Ports.SerialPort(this.components);
            this.label159 = new System.Windows.Forms.Label();
            this.label160 = new System.Windows.Forms.Label();
            this.label162 = new System.Windows.Forms.Label();
            this.label163 = new System.Windows.Forms.Label();
            this.label164 = new System.Windows.Forms.Label();
            this.label165 = new System.Windows.Forms.Label();
            this.SwitchPort = new System.IO.Ports.SerialPort(this.components);
            this.KnobPort = new System.IO.Ports.SerialPort(this.components);
            this.MacroPort = new System.IO.Ports.SerialPort(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem22 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem24 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem26 = new System.Windows.Forms.ToolStripMenuItem();
            this.TkGrn = new System.Windows.Forms.Button();
            this.TkYel = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPorts.SuspendLayout();
            this.grpSlave.SuspendLayout();
            this.grpBox1.SuspendLayout();
            this.grpBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.grpLPT.SuspendLayout();
            this.tabBCD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pixBox1)).BeginInit();
            this.grpAuxBcdControl.SuspendLayout();
            this.grpBCDover.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg1)).BeginInit();
            this.tabRCP.SuspendLayout();
            this.grpTKnob.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabWatt.SuspendLayout();
            this.groupBox15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSWR)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.grpNet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFwd)).BeginInit();
            this.grpWN2.SuspendLayout();
            this.grpC4.SuspendLayout();
            this.grpC3.SuspendLayout();
            this.grpC2.SuspendLayout();
            this.grpC1.SuspendLayout();
            this.grpLP.SuspendLayout();
            this.tabAmp.SuspendLayout();
            this.grpSPE.SuspendLayout();
            this.grpAmpBand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDrive)).BeginInit();
            this.grpPTT.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.grpAmp.SuspendLayout();
            this.grpAmpBands.SuspendLayout();
            this.tabOther.SuspendLayout();
            this.grpMacSw.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.grpRepeat.SuspendLayout();
            this.grpRmode.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.grpPro.SuspendLayout();
            this.grpInP.SuspendLayout();
            this.grpAntP.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabMacro.SuspendLayout();
            this.grpMacro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgm)).BeginInit();
            this.tabRotor.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.grpPreset.SuspendLayout();
            this.grpStepCtrl.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tabExtCtrl.SuspendLayout();
            this.grpPortB.SuspendLayout();
            this.grpPortA.SuspendLayout();
            this.tabSetup.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.grpDog.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.grpSpeed.SuspendLayout();
            this.grpModel.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.tabProp.SuspendLayout();
            this.grpSO2R.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSplit)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPorts);
            this.tabControl.Controls.Add(this.tabBCD);
            this.tabControl.Controls.Add(this.tabRCP);
            this.tabControl.Controls.Add(this.tabWatt);
            this.tabControl.Controls.Add(this.tabAmp);
            this.tabControl.Controls.Add(this.tabOther);
            this.tabControl.Controls.Add(this.tabMacro);
            this.tabControl.Controls.Add(this.tabRotor);
            this.tabControl.Controls.Add(this.tabExtCtrl);
            this.tabControl.Controls.Add(this.tabSetup);
            this.tabControl.Controls.Add(this.tabProp);
            this.tabControl.ItemSize = new System.Drawing.Size(42, 18);
            this.tabControl.Location = new System.Drawing.Point(-1, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(4, 3);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(439, 337);
            this.tabControl.TabIndex = 31;
            this.toolTip1.SetToolTip(this.tabControl, "Right-Click for Context Menu.");
            // 
            // tabPorts
            // 
            this.tabPorts.BackColor = System.Drawing.Color.Transparent;
            this.tabPorts.Controls.Add(this.chkPSDR);
            this.tabPorts.Controls.Add(this.btnFlexOn);
            this.tabPorts.Controls.Add(this.grpSlave);
            this.tabPorts.Controls.Add(this.label5);
            this.tabPorts.Controls.Add(this.cboSerAcc);
            this.tabPorts.Controls.Add(this.grpBox1);
            this.tabPorts.Controls.Add(this.grpBox2);
            this.tabPorts.Location = new System.Drawing.Point(4, 22);
            this.tabPorts.Name = "tabPorts";
            this.tabPorts.Padding = new System.Windows.Forms.Padding(3);
            this.tabPorts.Size = new System.Drawing.Size(431, 311);
            this.tabPorts.TabIndex = 0;
            this.tabPorts.Text = "Ports";
            this.toolTip1.SetToolTip(this.tabPorts, "Right-Click for Options");
            this.tabPorts.UseVisualStyleBackColor = true;
            // 
            // chkPSDR
            // 
            this.chkPSDR.AutoSize = true;
            this.chkPSDR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkPSDR.ForeColor = System.Drawing.Color.Navy;
            this.chkPSDR.Location = new System.Drawing.Point(375, 53);
            this.chkPSDR.Name = "chkPSDR";
            this.chkPSDR.Size = new System.Drawing.Size(56, 17);
            this.chkPSDR.TabIndex = 70;
            this.chkPSDR.Text = "PSDR";
            this.toolTip1.SetToolTip(this.chkPSDR, "Select to start/stop PSDR with radio.");
            this.chkPSDR.UseVisualStyleBackColor = true;
            this.chkPSDR.CheckedChanged += new System.EventHandler(this.chkPSDR_CheckedChanged);
            // 
            // btnFlexOn
            // 
            this.btnFlexOn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnFlexOn.BackColor = System.Drawing.Color.Transparent;
            this.btnFlexOn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFlexOn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnFlexOn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnFlexOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFlexOn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnFlexOn.Location = new System.Drawing.Point(373, 25);
            this.btnFlexOn.Name = "btnFlexOn";
            this.btnFlexOn.Size = new System.Drawing.Size(46, 23);
            this.btnFlexOn.TabIndex = 69;
            this.btnFlexOn.Text = "Power";
            this.toolTip1.SetToolTip(this.btnFlexOn, "Press to toggle Flex Power On/Off (Ctrl+Shift+S) Note: requires hdwe.");
            this.btnFlexOn.UseVisualStyleBackColor = false;
            this.btnFlexOn.Click += new System.EventHandler(this.btnFlexOn_Click);
            // 
            // grpSlave
            // 
            this.grpSlave.Controls.Add(this.chkSlaveRTS);
            this.grpSlave.Controls.Add(this.chkSlaveDTR);
            this.grpSlave.Controls.Add(this.chkFollow);
            this.grpSlave.Controls.Add(this.chkMode);
            this.grpSlave.Controls.Add(this.cboRadio);
            this.grpSlave.Controls.Add(this.label12);
            this.grpSlave.Controls.Add(this.label8);
            this.grpSlave.Controls.Add(this.cboRadData);
            this.grpSlave.Controls.Add(this.txtRadNum);
            this.grpSlave.Controls.Add(this.label9);
            this.grpSlave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpSlave.ForeColor = System.Drawing.Color.Blue;
            this.grpSlave.Location = new System.Drawing.Point(4, 235);
            this.grpSlave.Name = "grpSlave";
            this.grpSlave.Size = new System.Drawing.Size(405, 65);
            this.grpSlave.TabIndex = 31;
            this.grpSlave.TabStop = false;
            this.grpSlave.Text = "Slave Radio";
            // 
            // chkSlaveRTS
            // 
            this.chkSlaveRTS.AutoSize = true;
            this.chkSlaveRTS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSlaveRTS.ForeColor = System.Drawing.Color.Navy;
            this.chkSlaveRTS.Location = new System.Drawing.Point(292, 37);
            this.chkSlaveRTS.Name = "chkSlaveRTS";
            this.chkSlaveRTS.Size = new System.Drawing.Size(48, 17);
            this.chkSlaveRTS.TabIndex = 31;
            this.chkSlaveRTS.Text = "RTS";
            this.toolTip1.SetToolTip(this.chkSlaveRTS, "Select if your CI-V adapter requires power from the RTS line.");
            this.chkSlaveRTS.UseVisualStyleBackColor = true;
            this.chkSlaveRTS.CheckedChanged += new System.EventHandler(this.chkSlaveRTS_CheckedChanged);
            // 
            // chkSlaveDTR
            // 
            this.chkSlaveDTR.AutoSize = true;
            this.chkSlaveDTR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSlaveDTR.ForeColor = System.Drawing.Color.Navy;
            this.chkSlaveDTR.Location = new System.Drawing.Point(292, 20);
            this.chkSlaveDTR.Name = "chkSlaveDTR";
            this.chkSlaveDTR.Size = new System.Drawing.Size(49, 17);
            this.chkSlaveDTR.TabIndex = 27;
            this.chkSlaveDTR.Text = "DTR";
            this.toolTip1.SetToolTip(this.chkSlaveDTR, "Select if your CI-V adapter requires power from the DTR line.");
            this.chkSlaveDTR.UseVisualStyleBackColor = true;
            this.chkSlaveDTR.CheckedChanged += new System.EventHandler(this.chkSlaveDTR_CheckedChanged);
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
            this.cboRadio.Location = new System.Drawing.Point(96, 19);
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
            this.label12.Location = new System.Drawing.Point(196, 42);
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
            this.label8.Location = new System.Drawing.Point(93, 42);
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
            this.cboRadData.Location = new System.Drawing.Point(197, 19);
            this.cboRadData.Name = "cboRadData";
            this.cboRadData.Size = new System.Drawing.Size(83, 21);
            this.cboRadData.TabIndex = 28;
            this.toolTip1.SetToolTip(this.cboRadData, "Sets Passive Listener / Slave Radio port comm parameters.");
            this.cboRadData.SelectedIndexChanged += new System.EventHandler(this.cboRadData_SelectedIndexChanged);
            // 
            // txtRadNum
            // 
            this.txtRadNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRadNum.Location = new System.Drawing.Point(352, 19);
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
            this.label9.Location = new System.Drawing.Point(344, 41);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "CI-V Addr";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.ForeColor = System.Drawing.Color.Firebrick;
            this.label5.Location = new System.Drawing.Point(133, 53);
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
            this.cboSerAcc.Location = new System.Drawing.Point(134, 30);
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
            this.grpBox1.ForeColor = System.Drawing.Color.Blue;
            this.grpBox1.Location = new System.Drawing.Point(3, 10);
            this.grpBox1.Name = "grpBox1";
            this.grpBox1.Size = new System.Drawing.Size(358, 65);
            this.grpBox1.TabIndex = 19;
            this.grpBox1.TabStop = false;
            this.grpBox1.Text = "Serial Ports";
            this.toolTip1.SetToolTip(this.grpBox1, "Right-Click for Options");
            // 
            // label46
            // 
            this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label46.ForeColor = System.Drawing.Color.Navy;
            this.label46.Location = new System.Drawing.Point(282, 43);
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
            this.cboRCP1Rotor.Location = new System.Drawing.Point(280, 20);
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
            this.label11.Location = new System.Drawing.Point(205, 43);
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
            this.cboLogPort.Location = new System.Drawing.Point(205, 20);
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
            this.label6.Location = new System.Drawing.Point(80, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "Poll Intvl";
            // 
            // txtInv
            // 
            this.txtInv.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInv.Location = new System.Drawing.Point(87, 20);
            this.txtInv.Name = "txtInv";
            this.txtInv.Size = new System.Drawing.Size(34, 20);
            this.txtInv.TabIndex = 11;
            this.txtInv.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtInv, "Radio CAT polling interval in ms (with DDUtil only running)");
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
            this.grpBox2.ForeColor = System.Drawing.Color.Blue;
            this.grpBox2.Location = new System.Drawing.Point(3, 85);
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
            this.lblPortBtn.ForeColor = System.Drawing.Color.Navy;
            this.lblPortBtn.Location = new System.Drawing.Point(244, 92);
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
            this.btnPortNum.Location = new System.Drawing.Point(216, 95);
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
            this.label7.ForeColor = System.Drawing.Color.Navy;
            this.label7.Location = new System.Drawing.Point(145, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "Port Number";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.Dev0);
            this.groupBox5.Controls.Add(this.chkDSInvert);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.chkDevice);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox5.ForeColor = System.Drawing.Color.MediumBlue;
            this.groupBox5.Location = new System.Drawing.Point(149, 17);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(210, 60);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Data Signals";
            this.toolTip1.SetToolTip(this.groupBox5, "Right-Click for Options");
            // 
            // Dev0
            // 
            this.Dev0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dev0.Location = new System.Drawing.Point(94, 14);
            this.Dev0.Name = "Dev0";
            this.Dev0.Size = new System.Drawing.Size(95, 20);
            this.Dev0.TabIndex = 52;
            this.toolTip1.SetToolTip(this.Dev0, "Device name (Amp, Antenna Switch)");
            this.Dev0.TextChanged += new System.EventHandler(this.Dev0_TextChanged);
            // 
            // chkDSInvert
            // 
            this.chkDSInvert.AutoSize = true;
            this.chkDSInvert.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkDSInvert.ForeColor = System.Drawing.Color.Navy;
            this.chkDSInvert.Location = new System.Drawing.Point(12, 38);
            this.chkDSInvert.Name = "chkDSInvert";
            this.chkDSInvert.Size = new System.Drawing.Size(53, 17);
            this.chkDSInvert.TabIndex = 27;
            this.chkDSInvert.Text = "Invert";
            this.toolTip1.SetToolTip(this.chkDSInvert, "Selects to output BCD data Inverted on seleced port.");
            this.chkDSInvert.UseVisualStyleBackColor = true;
            this.chkDSInvert.CheckedChanged += new System.EventHandler(this.chkDSInvert_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(106, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Device Name";
            // 
            // chkDevice
            // 
            this.chkDevice.AutoSize = true;
            this.chkDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkDevice.ForeColor = System.Drawing.Color.Navy;
            this.chkDevice.Location = new System.Drawing.Point(12, 17);
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
            this.grpLPT.Controls.Add(this.rbFW);
            this.grpLPT.Controls.Add(this.rbOther);
            this.grpLPT.Controls.Add(this.rb1);
            this.grpLPT.Controls.Add(this.rb4);
            this.grpLPT.Controls.Add(this.rb2);
            this.grpLPT.Controls.Add(this.rbNone);
            this.grpLPT.Controls.Add(this.rb3);
            this.grpLPT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpLPT.ForeColor = System.Drawing.Color.MediumBlue;
            this.grpLPT.Location = new System.Drawing.Point(10, 17);
            this.grpLPT.Name = "grpLPT";
            this.grpLPT.Size = new System.Drawing.Size(128, 110);
            this.grpLPT.TabIndex = 0;
            this.grpLPT.TabStop = false;
            this.grpLPT.Text = "Port Selector";
            this.toolTip1.SetToolTip(this.grpLPT, "Select Parallel Port");
            // 
            // rbFW
            // 
            this.rbFW.AutoSize = true;
            this.rbFW.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbFW.ForeColor = System.Drawing.Color.Navy;
            this.rbFW.Location = new System.Drawing.Point(61, 66);
            this.rbFW.Name = "rbFW";
            this.rbFW.Size = new System.Drawing.Size(66, 17);
            this.rbFW.TabIndex = 28;
            this.rbFW.Text = "FlexWire";
            this.rbFW.UseVisualStyleBackColor = true;
            this.rbFW.CheckedChanged += new System.EventHandler(this.grpLPT_CheckedChanged);
            // 
            // rbOther
            // 
            this.rbOther.AutoSize = true;
            this.rbOther.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOther.ForeColor = System.Drawing.Color.Navy;
            this.rbOther.Location = new System.Drawing.Point(61, 43);
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
            this.rb1.ForeColor = System.Drawing.Color.Navy;
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
            this.rb4.ForeColor = System.Drawing.Color.Navy;
            this.rb4.Location = new System.Drawing.Point(61, 20);
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
            this.rb2.ForeColor = System.Drawing.Color.Navy;
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
            this.rbNone.ForeColor = System.Drawing.Color.Navy;
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
            this.rb3.ForeColor = System.Drawing.Color.Navy;
            this.rb3.Location = new System.Drawing.Point(6, 89);
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
            // tabBCD
            // 
            this.tabBCD.Controls.Add(this.pixBox1);
            this.tabBCD.Controls.Add(this.grpAuxBcdControl);
            this.tabBCD.Controls.Add(this.grpBCDover);
            this.tabBCD.Controls.Add(this.label20);
            this.tabBCD.Controls.Add(this.groupBox1);
            this.tabBCD.Controls.Add(this.dg1);
            this.tabBCD.Controls.Add(this.chkDev0);
            this.tabBCD.Location = new System.Drawing.Point(4, 22);
            this.tabBCD.Name = "tabBCD";
            this.tabBCD.Padding = new System.Windows.Forms.Padding(3);
            this.tabBCD.Size = new System.Drawing.Size(431, 311);
            this.tabBCD.TabIndex = 1;
            this.tabBCD.Text = "BCD";
            this.toolTip1.SetToolTip(this.tabBCD, "Right-Click for Options");
            this.tabBCD.UseVisualStyleBackColor = true;
            // 
            // pixBox1
            // 
            this.pixBox1.ImageLocation = "http://qsonet.com/propadex.png";
            this.pixBox1.InitialImage = null;
            this.pixBox1.Location = new System.Drawing.Point(278, 181);
            this.pixBox1.Name = "pixBox1";
            this.pixBox1.Size = new System.Drawing.Size(146, 121);
            this.pixBox1.TabIndex = 80;
            this.pixBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pixBox1, "Current propagation condition (click image for more info).");
            this.pixBox1.Click += new System.EventHandler(this.pixBox1_Click);
            // 
            // grpAuxBcdControl
            // 
            this.grpAuxBcdControl.Controls.Add(this.chkAuxMacro);
            this.grpAuxBcdControl.Controls.Add(this.chkFollowFreq);
            this.grpAuxBcdControl.Controls.Add(this.label4);
            this.grpAuxBcdControl.Controls.Add(this.chkAuxEnab);
            this.grpAuxBcdControl.Controls.Add(this.chkAuxBit7);
            this.grpAuxBcdControl.Controls.Add(this.chkAuxBit8);
            this.grpAuxBcdControl.Controls.Add(this.chkAuxBit5);
            this.grpAuxBcdControl.Controls.Add(this.chkAuxBit3);
            this.grpAuxBcdControl.Controls.Add(this.chkAuxBit6);
            this.grpAuxBcdControl.Controls.Add(this.chkAuxBit1);
            this.grpAuxBcdControl.Controls.Add(this.chkAuxBit4);
            this.grpAuxBcdControl.Controls.Add(this.chkAuxBit2);
            this.grpAuxBcdControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAuxBcdControl.ForeColor = System.Drawing.Color.Firebrick;
            this.grpAuxBcdControl.Location = new System.Drawing.Point(3, 235);
            this.grpAuxBcdControl.Name = "grpAuxBcdControl";
            this.grpAuxBcdControl.Size = new System.Drawing.Size(264, 70);
            this.grpAuxBcdControl.TabIndex = 110;
            this.grpAuxBcdControl.TabStop = false;
            this.grpAuxBcdControl.Text = "Aux BCD Control";
            this.toolTip1.SetToolTip(this.grpAuxBcdControl, "Allows auxilary control of BCD port");
            // 
            // chkAuxMacro
            // 
            this.chkAuxMacro.AutoSize = true;
            this.chkAuxMacro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkAuxMacro.ForeColor = System.Drawing.Color.Navy;
            this.chkAuxMacro.Location = new System.Drawing.Point(180, 47);
            this.chkAuxMacro.Name = "chkAuxMacro";
            this.chkAuxMacro.Size = new System.Drawing.Size(84, 17);
            this.chkAuxMacro.TabIndex = 114;
            this.chkAuxMacro.Text = "Allow Macro";
            this.toolTip1.SetToolTip(this.chkAuxMacro, "Select to allow macro modification of bits.");
            this.chkAuxMacro.UseVisualStyleBackColor = true;
            this.chkAuxMacro.CheckedChanged += new System.EventHandler(this.chkAuxMacro_CheckedChanged);
            // 
            // chkFollowFreq
            // 
            this.chkFollowFreq.AutoSize = true;
            this.chkFollowFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkFollowFreq.ForeColor = System.Drawing.Color.Navy;
            this.chkFollowFreq.Location = new System.Drawing.Point(180, 29);
            this.chkFollowFreq.Name = "chkFollowFreq";
            this.chkFollowFreq.Size = new System.Drawing.Size(80, 17);
            this.chkFollowFreq.TabIndex = 113;
            this.chkFollowFreq.Text = "Follow Freq";
            this.toolTip1.SetToolTip(this.chkFollowFreq, "Select to enable following BCD file frequency settings.");
            this.chkFollowFreq.UseVisualStyleBackColor = true;
            this.chkFollowFreq.CheckedChanged += new System.EventHandler(this.chkFollowFreq_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F);
            this.label4.ForeColor = System.Drawing.Color.Navy;
            this.label4.Location = new System.Drawing.Point(5, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 13);
            this.label4.TabIndex = 112;
            this.label4.Text = "8     7     6     5     4     3     2     1";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkAuxEnab
            // 
            this.chkAuxEnab.AutoSize = true;
            this.chkAuxEnab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkAuxEnab.ForeColor = System.Drawing.Color.Navy;
            this.chkAuxEnab.Location = new System.Drawing.Point(180, 11);
            this.chkAuxEnab.Name = "chkAuxEnab";
            this.chkAuxEnab.Size = new System.Drawing.Size(59, 17);
            this.chkAuxEnab.TabIndex = 111;
            this.chkAuxEnab.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkAuxEnab, "Select to override standard BCD file behavior.");
            this.chkAuxEnab.UseVisualStyleBackColor = true;
            this.chkAuxEnab.CheckedChanged += new System.EventHandler(this.chkAuxEnab_CheckedChanged);
            // 
            // chkAuxBit7
            // 
            this.chkAuxBit7.AutoSize = true;
            this.chkAuxBit7.BackColor = System.Drawing.Color.Transparent;
            this.chkAuxBit7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAuxBit7.ForeColor = System.Drawing.Color.Navy;
            this.chkAuxBit7.Location = new System.Drawing.Point(26, 29);
            this.chkAuxBit7.Name = "chkAuxBit7";
            this.chkAuxBit7.Size = new System.Drawing.Size(15, 14);
            this.chkAuxBit7.TabIndex = 98;
            this.toolTip1.SetToolTip(this.chkAuxBit7, "Select to enable this bit.");
            this.chkAuxBit7.UseVisualStyleBackColor = true;
            this.chkAuxBit7.CheckedChanged += new System.EventHandler(this.chkAuxBit7_CheckedChanged);
            // 
            // chkAuxBit8
            // 
            this.chkAuxBit8.AutoSize = true;
            this.chkAuxBit8.BackColor = System.Drawing.Color.Transparent;
            this.chkAuxBit8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAuxBit8.ForeColor = System.Drawing.Color.Navy;
            this.chkAuxBit8.Location = new System.Drawing.Point(5, 29);
            this.chkAuxBit8.Name = "chkAuxBit8";
            this.chkAuxBit8.Size = new System.Drawing.Size(15, 14);
            this.chkAuxBit8.TabIndex = 96;
            this.toolTip1.SetToolTip(this.chkAuxBit8, "Select to enable this bit.");
            this.chkAuxBit8.UseVisualStyleBackColor = true;
            this.chkAuxBit8.CheckedChanged += new System.EventHandler(this.chkAuxBit8_CheckedChanged);
            // 
            // chkAuxBit5
            // 
            this.chkAuxBit5.AutoSize = true;
            this.chkAuxBit5.BackColor = System.Drawing.Color.Transparent;
            this.chkAuxBit5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAuxBit5.ForeColor = System.Drawing.Color.Navy;
            this.chkAuxBit5.Location = new System.Drawing.Point(68, 29);
            this.chkAuxBit5.Name = "chkAuxBit5";
            this.chkAuxBit5.Size = new System.Drawing.Size(15, 14);
            this.chkAuxBit5.TabIndex = 103;
            this.toolTip1.SetToolTip(this.chkAuxBit5, "Select to enable this bit.");
            this.chkAuxBit5.UseVisualStyleBackColor = true;
            this.chkAuxBit5.CheckedChanged += new System.EventHandler(this.chkAuxBit5_CheckedChanged);
            // 
            // chkAuxBit3
            // 
            this.chkAuxBit3.AutoSize = true;
            this.chkAuxBit3.BackColor = System.Drawing.Color.Transparent;
            this.chkAuxBit3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAuxBit3.ForeColor = System.Drawing.Color.Navy;
            this.chkAuxBit3.Location = new System.Drawing.Point(110, 29);
            this.chkAuxBit3.Name = "chkAuxBit3";
            this.chkAuxBit3.Size = new System.Drawing.Size(15, 14);
            this.chkAuxBit3.TabIndex = 99;
            this.toolTip1.SetToolTip(this.chkAuxBit3, "Select to enable this bit.");
            this.chkAuxBit3.UseVisualStyleBackColor = true;
            this.chkAuxBit3.CheckedChanged += new System.EventHandler(this.chkAuxBit3_CheckedChanged);
            // 
            // chkAuxBit6
            // 
            this.chkAuxBit6.AutoSize = true;
            this.chkAuxBit6.BackColor = System.Drawing.Color.Transparent;
            this.chkAuxBit6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAuxBit6.ForeColor = System.Drawing.Color.Navy;
            this.chkAuxBit6.Location = new System.Drawing.Point(47, 29);
            this.chkAuxBit6.Name = "chkAuxBit6";
            this.chkAuxBit6.Size = new System.Drawing.Size(15, 14);
            this.chkAuxBit6.TabIndex = 101;
            this.toolTip1.SetToolTip(this.chkAuxBit6, "Select to enable this bit.");
            this.chkAuxBit6.UseVisualStyleBackColor = true;
            this.chkAuxBit6.CheckedChanged += new System.EventHandler(this.chkAuxBit6_CheckedChanged);
            // 
            // chkAuxBit1
            // 
            this.chkAuxBit1.AutoSize = true;
            this.chkAuxBit1.BackColor = System.Drawing.Color.Transparent;
            this.chkAuxBit1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAuxBit1.ForeColor = System.Drawing.Color.Navy;
            this.chkAuxBit1.Location = new System.Drawing.Point(152, 29);
            this.chkAuxBit1.Name = "chkAuxBit1";
            this.chkAuxBit1.Size = new System.Drawing.Size(15, 14);
            this.chkAuxBit1.TabIndex = 102;
            this.toolTip1.SetToolTip(this.chkAuxBit1, "Select to enable this bit.");
            this.chkAuxBit1.UseVisualStyleBackColor = true;
            this.chkAuxBit1.CheckedChanged += new System.EventHandler(this.chkAuxBit1_CheckedChanged);
            // 
            // chkAuxBit4
            // 
            this.chkAuxBit4.AutoSize = true;
            this.chkAuxBit4.BackColor = System.Drawing.Color.Transparent;
            this.chkAuxBit4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAuxBit4.ForeColor = System.Drawing.Color.Navy;
            this.chkAuxBit4.Location = new System.Drawing.Point(89, 29);
            this.chkAuxBit4.Name = "chkAuxBit4";
            this.chkAuxBit4.Size = new System.Drawing.Size(15, 14);
            this.chkAuxBit4.TabIndex = 97;
            this.toolTip1.SetToolTip(this.chkAuxBit4, "Select to enable this bit.");
            this.chkAuxBit4.UseVisualStyleBackColor = true;
            this.chkAuxBit4.CheckedChanged += new System.EventHandler(this.chkAuxBit4_CheckedChanged);
            // 
            // chkAuxBit2
            // 
            this.chkAuxBit2.AutoSize = true;
            this.chkAuxBit2.BackColor = System.Drawing.Color.Transparent;
            this.chkAuxBit2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAuxBit2.ForeColor = System.Drawing.Color.Navy;
            this.chkAuxBit2.Location = new System.Drawing.Point(131, 29);
            this.chkAuxBit2.Name = "chkAuxBit2";
            this.chkAuxBit2.Size = new System.Drawing.Size(15, 14);
            this.chkAuxBit2.TabIndex = 100;
            this.toolTip1.SetToolTip(this.chkAuxBit2, "Select to enable this bit.");
            this.chkAuxBit2.UseVisualStyleBackColor = true;
            this.chkAuxBit2.CheckedChanged += new System.EventHandler(this.chkAuxBit2_CheckedChanged);
            // 
            // grpBCDover
            // 
            this.grpBCDover.Controls.Add(this.rbOvr12);
            this.grpBCDover.Controls.Add(this.rbOvr11);
            this.grpBCDover.Controls.Add(this.rbOvr10);
            this.grpBCDover.Controls.Add(this.rbOvr9);
            this.grpBCDover.Controls.Add(this.rbOvr8);
            this.grpBCDover.Controls.Add(this.rbOvr4);
            this.grpBCDover.Controls.Add(this.rbOvr7);
            this.grpBCDover.Controls.Add(this.rbOvr3);
            this.grpBCDover.Controls.Add(this.rbOvr6);
            this.grpBCDover.Controls.Add(this.rbOvr2);
            this.grpBCDover.Controls.Add(this.rbOvr5);
            this.grpBCDover.Controls.Add(this.rbOvr1);
            this.grpBCDover.Controls.Add(this.chkOvride);
            this.grpBCDover.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBCDover.ForeColor = System.Drawing.Color.Firebrick;
            this.grpBCDover.Location = new System.Drawing.Point(3, 52);
            this.grpBCDover.Name = "grpBCDover";
            this.grpBCDover.Size = new System.Drawing.Size(167, 100);
            this.grpBCDover.TabIndex = 62;
            this.grpBCDover.TabStop = false;
            this.grpBCDover.Text = "Manual Control";
            this.toolTip1.SetToolTip(this.grpBCDover, "Select Override then press one of the RB for that digit to be sent to the LPT por" +
                    "t.");
            // 
            // rbOvr12
            // 
            this.rbOvr12.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbOvr12.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbOvr12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbOvr12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOvr12.ForeColor = System.Drawing.Color.Navy;
            this.rbOvr12.Location = new System.Drawing.Point(117, 73);
            this.rbOvr12.Name = "rbOvr12";
            this.rbOvr12.Size = new System.Drawing.Size(44, 17);
            this.rbOvr12.TabIndex = 126;
            this.rbOvr12.Text = "12";
            this.rbOvr12.UseVisualStyleBackColor = true;
            this.rbOvr12.CheckedChanged += new System.EventHandler(this.grpBCDover_CheckChanged);
            // 
            // rbOvr11
            // 
            this.rbOvr11.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbOvr11.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbOvr11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbOvr11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOvr11.ForeColor = System.Drawing.Color.Navy;
            this.rbOvr11.Location = new System.Drawing.Point(80, 73);
            this.rbOvr11.Name = "rbOvr11";
            this.rbOvr11.Size = new System.Drawing.Size(44, 17);
            this.rbOvr11.TabIndex = 125;
            this.rbOvr11.Text = "11";
            this.rbOvr11.UseVisualStyleBackColor = true;
            this.rbOvr11.CheckedChanged += new System.EventHandler(this.grpBCDover_CheckChanged);
            // 
            // rbOvr10
            // 
            this.rbOvr10.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbOvr10.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbOvr10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbOvr10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOvr10.ForeColor = System.Drawing.Color.Navy;
            this.rbOvr10.Location = new System.Drawing.Point(41, 73);
            this.rbOvr10.Name = "rbOvr10";
            this.rbOvr10.Size = new System.Drawing.Size(44, 17);
            this.rbOvr10.TabIndex = 124;
            this.rbOvr10.Text = "10";
            this.rbOvr10.UseVisualStyleBackColor = true;
            this.rbOvr10.CheckedChanged += new System.EventHandler(this.grpBCDover_CheckChanged);
            // 
            // rbOvr9
            // 
            this.rbOvr9.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbOvr9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbOvr9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbOvr9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOvr9.ForeColor = System.Drawing.Color.Navy;
            this.rbOvr9.Location = new System.Drawing.Point(7, 73);
            this.rbOvr9.Name = "rbOvr9";
            this.rbOvr9.Size = new System.Drawing.Size(44, 17);
            this.rbOvr9.TabIndex = 123;
            this.rbOvr9.Text = "9";
            this.rbOvr9.UseVisualStyleBackColor = true;
            this.rbOvr9.CheckedChanged += new System.EventHandler(this.grpBCDover_CheckChanged);
            // 
            // rbOvr8
            // 
            this.rbOvr8.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbOvr8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbOvr8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbOvr8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOvr8.ForeColor = System.Drawing.Color.Navy;
            this.rbOvr8.Location = new System.Drawing.Point(117, 55);
            this.rbOvr8.Name = "rbOvr8";
            this.rbOvr8.Size = new System.Drawing.Size(44, 17);
            this.rbOvr8.TabIndex = 120;
            this.rbOvr8.Text = "8";
            this.rbOvr8.UseVisualStyleBackColor = true;
            this.rbOvr8.CheckedChanged += new System.EventHandler(this.grpBCDover_CheckChanged);
            // 
            // rbOvr4
            // 
            this.rbOvr4.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbOvr4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbOvr4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbOvr4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOvr4.ForeColor = System.Drawing.Color.Navy;
            this.rbOvr4.Location = new System.Drawing.Point(117, 38);
            this.rbOvr4.Name = "rbOvr4";
            this.rbOvr4.Size = new System.Drawing.Size(44, 17);
            this.rbOvr4.TabIndex = 119;
            this.rbOvr4.Text = "4";
            this.rbOvr4.UseVisualStyleBackColor = true;
            this.rbOvr4.CheckedChanged += new System.EventHandler(this.grpBCDover_CheckChanged);
            // 
            // rbOvr7
            // 
            this.rbOvr7.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbOvr7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbOvr7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbOvr7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOvr7.ForeColor = System.Drawing.Color.Navy;
            this.rbOvr7.Location = new System.Drawing.Point(80, 55);
            this.rbOvr7.Name = "rbOvr7";
            this.rbOvr7.Size = new System.Drawing.Size(44, 17);
            this.rbOvr7.TabIndex = 118;
            this.rbOvr7.Text = "7";
            this.rbOvr7.UseVisualStyleBackColor = true;
            this.rbOvr7.CheckedChanged += new System.EventHandler(this.grpBCDover_CheckChanged);
            // 
            // rbOvr3
            // 
            this.rbOvr3.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbOvr3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbOvr3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbOvr3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOvr3.ForeColor = System.Drawing.Color.Navy;
            this.rbOvr3.Location = new System.Drawing.Point(80, 38);
            this.rbOvr3.Name = "rbOvr3";
            this.rbOvr3.Size = new System.Drawing.Size(44, 17);
            this.rbOvr3.TabIndex = 117;
            this.rbOvr3.Text = "3";
            this.rbOvr3.UseVisualStyleBackColor = true;
            this.rbOvr3.CheckedChanged += new System.EventHandler(this.grpBCDover_CheckChanged);
            // 
            // rbOvr6
            // 
            this.rbOvr6.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbOvr6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbOvr6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbOvr6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOvr6.ForeColor = System.Drawing.Color.Navy;
            this.rbOvr6.Location = new System.Drawing.Point(41, 55);
            this.rbOvr6.Name = "rbOvr6";
            this.rbOvr6.Size = new System.Drawing.Size(44, 17);
            this.rbOvr6.TabIndex = 116;
            this.rbOvr6.Text = "6";
            this.rbOvr6.UseVisualStyleBackColor = true;
            this.rbOvr6.CheckedChanged += new System.EventHandler(this.grpBCDover_CheckChanged);
            // 
            // rbOvr2
            // 
            this.rbOvr2.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbOvr2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbOvr2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbOvr2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOvr2.ForeColor = System.Drawing.Color.Navy;
            this.rbOvr2.Location = new System.Drawing.Point(41, 38);
            this.rbOvr2.Name = "rbOvr2";
            this.rbOvr2.Size = new System.Drawing.Size(44, 17);
            this.rbOvr2.TabIndex = 115;
            this.rbOvr2.Text = "2";
            this.rbOvr2.UseVisualStyleBackColor = true;
            this.rbOvr2.CheckedChanged += new System.EventHandler(this.grpBCDover_CheckChanged);
            // 
            // rbOvr5
            // 
            this.rbOvr5.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbOvr5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbOvr5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbOvr5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOvr5.ForeColor = System.Drawing.Color.Navy;
            this.rbOvr5.Location = new System.Drawing.Point(7, 55);
            this.rbOvr5.Name = "rbOvr5";
            this.rbOvr5.Size = new System.Drawing.Size(44, 17);
            this.rbOvr5.TabIndex = 114;
            this.rbOvr5.Text = "5";
            this.rbOvr5.UseVisualStyleBackColor = true;
            this.rbOvr5.CheckedChanged += new System.EventHandler(this.grpBCDover_CheckChanged);
            // 
            // rbOvr1
            // 
            this.rbOvr1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbOvr1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbOvr1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbOvr1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbOvr1.ForeColor = System.Drawing.Color.Navy;
            this.rbOvr1.Location = new System.Drawing.Point(7, 38);
            this.rbOvr1.Name = "rbOvr1";
            this.rbOvr1.Size = new System.Drawing.Size(44, 17);
            this.rbOvr1.TabIndex = 113;
            this.rbOvr1.Text = "1";
            this.rbOvr1.UseVisualStyleBackColor = true;
            this.rbOvr1.CheckedChanged += new System.EventHandler(this.grpBCDover_CheckChanged);
            // 
            // chkOvride
            // 
            this.chkOvride.AutoSize = true;
            this.chkOvride.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOvride.ForeColor = System.Drawing.Color.Navy;
            this.chkOvride.Location = new System.Drawing.Point(6, 19);
            this.chkOvride.Name = "chkOvride";
            this.chkOvride.Size = new System.Drawing.Size(74, 17);
            this.chkOvride.TabIndex = 63;
            this.chkOvride.Text = "Override";
            this.toolTip1.SetToolTip(this.chkOvride, "Check to override auto frequency setting BCD device.");
            this.chkOvride.UseVisualStyleBackColor = true;
            this.chkOvride.CheckedChanged += new System.EventHandler(this.chkOvride_CheckedChanged);
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.Blue;
            this.label20.Location = new System.Drawing.Point(4, 5);
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
            this.groupBox1.Controls.Add(this.txtFile0);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.Navy;
            this.groupBox1.Location = new System.Drawing.Point(5, 162);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 69);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data File";
            this.toolTip1.SetToolTip(this.groupBox1, "Right-Click for Options");
            // 
            // btnSave0
            // 
            this.btnSave0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnSave0.Location = new System.Drawing.Point(182, 40);
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
            this.btnReLoad0.Location = new System.Drawing.Point(103, 40);
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
            this.btnFile0.Location = new System.Drawing.Point(21, 40);
            this.btnFile0.Name = "btnFile0";
            this.btnFile0.Size = new System.Drawing.Size(57, 23);
            this.btnFile0.TabIndex = 60;
            this.btnFile0.Text = "Select";
            this.toolTip1.SetToolTip(this.btnFile0, "Opens file dialog to select file to use for this device.");
            this.btnFile0.UseVisualStyleBackColor = true;
            this.btnFile0.Click += new System.EventHandler(this.btnFile0_Click);
            // 
            // txtFile0
            // 
            this.txtFile0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtFile0.Location = new System.Drawing.Point(6, 19);
            this.txtFile0.Name = "txtFile0";
            this.txtFile0.Size = new System.Drawing.Size(250, 20);
            this.txtFile0.TabIndex = 58;
            this.toolTip1.SetToolTip(this.txtFile0, "Data file to use for this device.");
            this.txtFile0.WordWrap = false;
            // 
            // dg1
            // 
            this.dg1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg1.Location = new System.Drawing.Point(217, 11);
            this.dg1.Name = "dg1";
            this.dg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dg1.Size = new System.Drawing.Size(186, 150);
            this.dg1.TabIndex = 42;
            this.dg1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg1_CellValueChanged);
            // 
            // chkDev0
            // 
            this.chkDev0.AutoSize = true;
            this.chkDev0.Location = new System.Drawing.Point(10, 30);
            this.chkDev0.Name = "chkDev0";
            this.chkDev0.Size = new System.Drawing.Size(59, 17);
            this.chkDev0.TabIndex = 44;
            this.chkDev0.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkDev0, "Check to enable this device");
            this.chkDev0.UseVisualStyleBackColor = true;
            this.chkDev0.CheckedChanged += new System.EventHandler(this.chkDev0_CheckedChanged);
            // 
            // tabRCP
            // 
            this.tabRCP.Controls.Add(this.grpTKnob);
            this.tabRCP.Controls.Add(this.groupBox7);
            this.tabRCP.Controls.Add(this.txtRCP);
            this.tabRCP.Location = new System.Drawing.Point(4, 22);
            this.tabRCP.Name = "tabRCP";
            this.tabRCP.Size = new System.Drawing.Size(431, 311);
            this.tabRCP.TabIndex = 3;
            this.tabRCP.Text = "RCP";
            this.tabRCP.UseVisualStyleBackColor = true;
            this.tabRCP.DoubleClick += new System.EventHandler(this.tabRCP_DoubleClick);
            // 
            // grpTKnob
            // 
            this.grpTKnob.Controls.Add(this.button7);
            this.grpTKnob.Controls.Add(this.lblZE);
            this.grpTKnob.Controls.Add(this.txtZE);
            this.grpTKnob.Controls.Add(this.label232);
            this.grpTKnob.Controls.Add(this.label231);
            this.grpTKnob.Controls.Add(this.label230);
            this.grpTKnob.Controls.Add(this.label229);
            this.grpTKnob.Controls.Add(this.cboTkSw3);
            this.grpTKnob.Controls.Add(this.cboTkSw1);
            this.grpTKnob.Controls.Add(this.cboTkSw2);
            this.grpTKnob.Controls.Add(this.label228);
            this.grpTKnob.Controls.Add(this.lblZR);
            this.grpTKnob.Controls.Add(this.cboTstep);
            this.grpTKnob.Controls.Add(this.txtZR);
            this.grpTKnob.Controls.Add(this.lblZL);
            this.grpTKnob.Controls.Add(this.txtZL);
            this.grpTKnob.Controls.Add(this.lblZC);
            this.grpTKnob.Controls.Add(this.txtZC);
            this.grpTKnob.Controls.Add(this.chkKnobExpert);
            this.grpTKnob.Controls.Add(this.cboKnobBDC);
            this.grpTKnob.Controls.Add(this.cboKnobBOn);
            this.grpTKnob.Controls.Add(this.cboKnobBOff);
            this.grpTKnob.Controls.Add(this.label221);
            this.grpTKnob.Controls.Add(this.label220);
            this.grpTKnob.Controls.Add(this.label219);
            this.grpTKnob.Controls.Add(this.label218);
            this.grpTKnob.Controls.Add(this.label217);
            this.grpTKnob.Controls.Add(this.cboKnobADC);
            this.grpTKnob.Controls.Add(this.cboKnobAOn);
            this.grpTKnob.Controls.Add(this.cboKnobAOff);
            this.grpTKnob.Controls.Add(this.label216);
            this.grpTKnob.Controls.Add(this.cboKnobPort);
            this.grpTKnob.Controls.Add(this.chkKnobEnab);
            this.grpTKnob.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpTKnob.ForeColor = System.Drawing.Color.Blue;
            this.grpTKnob.Location = new System.Drawing.Point(5, 149);
            this.grpTKnob.Name = "grpTKnob";
            this.grpTKnob.Size = new System.Drawing.Size(417, 156);
            this.grpTKnob.TabIndex = 67;
            this.grpTKnob.TabStop = false;
            this.grpTKnob.Text = "Tuning Knob";
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.SystemColors.Control;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Location = new System.Drawing.Point(284, 19);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(10, 10);
            this.button7.TabIndex = 110;
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // lblZE
            // 
            this.lblZE.AutoSize = true;
            this.lblZE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblZE.ForeColor = System.Drawing.Color.Navy;
            this.lblZE.Location = new System.Drawing.Point(157, 130);
            this.lblZE.Name = "lblZE";
            this.lblZE.Size = new System.Drawing.Size(21, 13);
            this.lblZE.TabIndex = 109;
            this.lblZE.Text = "ZE";
            this.lblZE.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblZE.Visible = false;
            this.lblZE.DoubleClick += new System.EventHandler(this.lblZE_DoubleClick);
            // 
            // txtZE
            // 
            this.txtZE.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtZE.Enabled = false;
            this.txtZE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZE.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtZE.Location = new System.Drawing.Point(127, 126);
            this.txtZE.MaxLength = 2;
            this.txtZE.Name = "txtZE";
            this.txtZE.Size = new System.Drawing.Size(28, 20);
            this.txtZE.TabIndex = 108;
            this.txtZE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtZE, "Set value for the Encoder count parameter (8-bit hex).");
            this.txtZE.Visible = false;
            this.txtZE.DoubleClick += new System.EventHandler(this.txtZE_DoubleClick);
            this.txtZE.TextChanged += new System.EventHandler(this.txtZE_TextChanged);
            // 
            // label232
            // 
            this.label232.AutoSize = true;
            this.label232.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label232.ForeColor = System.Drawing.Color.Navy;
            this.label232.Location = new System.Drawing.Point(312, 57);
            this.label232.Name = "label232";
            this.label232.Size = new System.Drawing.Size(71, 13);
            this.label232.TabIndex = 107;
            this.label232.Text = "Aux Switches";
            this.label232.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label231
            // 
            this.label231.AutoSize = true;
            this.label231.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label231.ForeColor = System.Drawing.Color.Navy;
            this.label231.Location = new System.Drawing.Point(401, 129);
            this.label231.Name = "label231";
            this.label231.Size = new System.Drawing.Size(13, 13);
            this.label231.TabIndex = 106;
            this.label231.Text = "3";
            this.label231.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label230
            // 
            this.label230.AutoSize = true;
            this.label230.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label230.ForeColor = System.Drawing.Color.Navy;
            this.label230.Location = new System.Drawing.Point(401, 103);
            this.label230.Name = "label230";
            this.label230.Size = new System.Drawing.Size(13, 13);
            this.label230.TabIndex = 105;
            this.label230.Text = "2";
            this.label230.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label229
            // 
            this.label229.AutoSize = true;
            this.label229.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label229.ForeColor = System.Drawing.Color.Navy;
            this.label229.Location = new System.Drawing.Point(401, 75);
            this.label229.Name = "label229";
            this.label229.Size = new System.Drawing.Size(13, 13);
            this.label229.TabIndex = 104;
            this.label229.Text = "1";
            this.label229.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboTkSw3
            // 
            this.cboTkSw3.BackColor = System.Drawing.SystemColors.Window;
            this.cboTkSw3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboTkSw3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboTkSw3.FormattingEnabled = true;
            this.cboTkSw3.Items.AddRange(new object[] {
            "Tune Step Up",
            "Tune Step Dn",
            "Macro 1",
            "Macro 2",
            "Macro 3"});
            this.cboTkSw3.Location = new System.Drawing.Point(309, 126);
            this.cboTkSw3.Name = "cboTkSw3";
            this.cboTkSw3.Size = new System.Drawing.Size(90, 21);
            this.cboTkSw3.TabIndex = 103;
            this.toolTip1.SetToolTip(this.cboTkSw3, "Select auxiliary switch 3 function.");
            this.cboTkSw3.SelectedIndexChanged += new System.EventHandler(this.cboTkSw3_SelectedIndexChanged);
            // 
            // cboTkSw1
            // 
            this.cboTkSw1.BackColor = System.Drawing.SystemColors.Window;
            this.cboTkSw1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboTkSw1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboTkSw1.FormattingEnabled = true;
            this.cboTkSw1.Items.AddRange(new object[] {
            "Tune Step Up",
            "Tune Step Dn",
            "Macro 1",
            "Macro 2",
            "Macro 3"});
            this.cboTkSw1.Location = new System.Drawing.Point(309, 72);
            this.cboTkSw1.Name = "cboTkSw1";
            this.cboTkSw1.Size = new System.Drawing.Size(90, 21);
            this.cboTkSw1.TabIndex = 102;
            this.toolTip1.SetToolTip(this.cboTkSw1, "Select auxiliary switch 1 function.");
            this.cboTkSw1.SelectedIndexChanged += new System.EventHandler(this.cboTkSw1_SelectedIndexChanged);
            // 
            // cboTkSw2
            // 
            this.cboTkSw2.BackColor = System.Drawing.SystemColors.Window;
            this.cboTkSw2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboTkSw2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboTkSw2.FormattingEnabled = true;
            this.cboTkSw2.Items.AddRange(new object[] {
            "Tune Step Up",
            "Tune Step Dn",
            "Macro 1",
            "Macro 2",
            "Macro 3"});
            this.cboTkSw2.Location = new System.Drawing.Point(309, 99);
            this.cboTkSw2.Name = "cboTkSw2";
            this.cboTkSw2.Size = new System.Drawing.Size(90, 21);
            this.cboTkSw2.TabIndex = 101;
            this.toolTip1.SetToolTip(this.cboTkSw2, "Select auxiliary switch 2 function.");
            this.cboTkSw2.SelectedIndexChanged += new System.EventHandler(this.cboTkSw2_SelectedIndexChanged);
            // 
            // label228
            // 
            this.label228.AutoSize = true;
            this.label228.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label228.ForeColor = System.Drawing.Color.Navy;
            this.label228.Location = new System.Drawing.Point(157, 14);
            this.label228.Name = "label228";
            this.label228.Size = new System.Drawing.Size(57, 13);
            this.label228.TabIndex = 100;
            this.label228.Text = "Tune Step";
            // 
            // lblZR
            // 
            this.lblZR.AutoSize = true;
            this.lblZR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblZR.ForeColor = System.Drawing.Color.Navy;
            this.lblZR.Location = new System.Drawing.Point(276, 130);
            this.lblZR.Name = "lblZR";
            this.lblZR.Size = new System.Drawing.Size(22, 13);
            this.lblZR.TabIndex = 98;
            this.lblZR.Text = "ZR";
            this.lblZR.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblZR.Visible = false;
            // 
            // cboTstep
            // 
            this.cboTstep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboTstep.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboTstep.FormattingEnabled = true;
            this.cboTstep.Items.AddRange(new object[] {
            "1 Hz",
            "10 Hz",
            "50 Hz",
            "100 Hz",
            "250 Hz",
            "500Hz",
            "1 KhZ",
            "5 KhZ",
            "9 KhZ",
            "10 KhZ",
            "100 KhZ",
            "250 KhZ",
            "500 KhZ",
            "1 MhZ",
            "10 MhZ"});
            this.cboTstep.Location = new System.Drawing.Point(158, 29);
            this.cboTstep.Name = "cboTstep";
            this.cboTstep.Size = new System.Drawing.Size(69, 21);
            this.cboTstep.TabIndex = 99;
            this.toolTip1.SetToolTip(this.cboTstep, "Select base tuning step (ctrl++ up, ctrl+- down).");
            this.cboTstep.SelectedIndexChanged += new System.EventHandler(this.cboTstep_SelectedIndexChanged);
            // 
            // txtZR
            // 
            this.txtZR.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtZR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZR.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtZR.Location = new System.Drawing.Point(246, 126);
            this.txtZR.MaxLength = 2;
            this.txtZR.Name = "txtZR";
            this.txtZR.Size = new System.Drawing.Size(28, 20);
            this.txtZR.TabIndex = 97;
            this.txtZR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtZR, "Set value for rate period (8-bit hex).");
            this.txtZR.Visible = false;
            this.txtZR.DoubleClick += new System.EventHandler(this.txtZR_DoubleClick);
            this.txtZR.TextChanged += new System.EventHandler(this.txtZR_TextChanged);
            // 
            // lblZL
            // 
            this.lblZL.AutoSize = true;
            this.lblZL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblZL.ForeColor = System.Drawing.Color.Navy;
            this.lblZL.Location = new System.Drawing.Point(217, 130);
            this.lblZL.Name = "lblZL";
            this.lblZL.Size = new System.Drawing.Size(20, 13);
            this.lblZL.TabIndex = 96;
            this.lblZL.Text = "ZL";
            this.lblZL.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblZL.Visible = false;
            // 
            // txtZL
            // 
            this.txtZL.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtZL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZL.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtZL.Location = new System.Drawing.Point(187, 126);
            this.txtZL.MaxLength = 2;
            this.txtZL.Name = "txtZL";
            this.txtZL.Size = new System.Drawing.Size(28, 20);
            this.txtZL.TabIndex = 95;
            this.txtZL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtZL, "Set value for long-click timer (8-bit hex).");
            this.txtZL.Visible = false;
            this.txtZL.DoubleClick += new System.EventHandler(this.txtZL_DoubleClick);
            this.txtZL.TextChanged += new System.EventHandler(this.txtZL_TextChanged);
            // 
            // lblZC
            // 
            this.lblZC.AutoSize = true;
            this.lblZC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblZC.ForeColor = System.Drawing.Color.Navy;
            this.lblZC.Location = new System.Drawing.Point(97, 130);
            this.lblZC.Name = "lblZC";
            this.lblZC.Size = new System.Drawing.Size(21, 13);
            this.lblZC.TabIndex = 94;
            this.lblZC.Text = "ZC";
            this.lblZC.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblZC.Visible = false;
            // 
            // txtZC
            // 
            this.txtZC.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtZC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZC.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtZC.Location = new System.Drawing.Point(67, 126);
            this.txtZC.MaxLength = 2;
            this.txtZC.Name = "txtZC";
            this.txtZC.Size = new System.Drawing.Size(28, 20);
            this.txtZC.TabIndex = 93;
            this.txtZC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtZC, "Set value for double-click timer (8-bit hex).");
            this.txtZC.Visible = false;
            this.txtZC.DoubleClick += new System.EventHandler(this.txtZC_DoubleClick);
            this.txtZC.TextChanged += new System.EventHandler(this.txtZC_TextChanged);
            // 
            // chkKnobExpert
            // 
            this.chkKnobExpert.AutoSize = true;
            this.chkKnobExpert.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkKnobExpert.ForeColor = System.Drawing.Color.Navy;
            this.chkKnobExpert.Location = new System.Drawing.Point(4, 128);
            this.chkKnobExpert.Name = "chkKnobExpert";
            this.chkKnobExpert.Size = new System.Drawing.Size(56, 17);
            this.chkKnobExpert.TabIndex = 92;
            this.chkKnobExpert.Text = "Expert";
            this.toolTip1.SetToolTip(this.chkKnobExpert, "Check to enable Expert controls");
            this.chkKnobExpert.UseVisualStyleBackColor = true;
            this.chkKnobExpert.CheckedChanged += new System.EventHandler(this.chkKnobAdv_CheckedChanged);
            // 
            // cboKnobBDC
            // 
            this.cboKnobBDC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboKnobBDC.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboKnobBDC.FormattingEnabled = true;
            this.cboKnobBDC.Items.AddRange(new object[] {
            "Swap Tx Vfo",
            "Swap A<>B",
            "Split On/Off",
            "Clear RIT",
            "Clear XIT"});
            this.cboKnobBDC.Location = new System.Drawing.Point(209, 99);
            this.cboKnobBDC.Name = "cboKnobBDC";
            this.cboKnobBDC.Size = new System.Drawing.Size(90, 21);
            this.cboKnobBDC.TabIndex = 90;
            this.toolTip1.SetToolTip(this.cboKnobBDC, "Select Mode B Double-Click function to perform.");
            this.cboKnobBDC.SelectedIndexChanged += new System.EventHandler(this.cboKnobBDC_SelectedIndexChanged);
            // 
            // cboKnobBOn
            // 
            this.cboKnobBOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboKnobBOn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboKnobBOn.FormattingEnabled = true;
            this.cboKnobBOn.Items.AddRange(new object[] {
            "Tune VFO A",
            "Tune VFO B",
            "Tune RIT",
            "Tune XIT",
            "Audio Gain"});
            this.cboKnobBOn.Location = new System.Drawing.Point(17, 99);
            this.cboKnobBOn.Name = "cboKnobBOn";
            this.cboKnobBOn.Size = new System.Drawing.Size(90, 21);
            this.cboKnobBOn.TabIndex = 89;
            this.cboKnobBOn.Text = " ";
            this.toolTip1.SetToolTip(this.cboKnobBOn, "Select Mode B On function to perform.");
            this.cboKnobBOn.SelectedIndexChanged += new System.EventHandler(this.cboKnobBOn_SelectedIndexChanged);
            // 
            // cboKnobBOff
            // 
            this.cboKnobBOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboKnobBOff.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboKnobBOff.FormattingEnabled = true;
            this.cboKnobBOff.Items.AddRange(new object[] {
            "Tune VFO A",
            "Tune VFO B",
            "Tune RIT",
            "Tune XIT",
            "Audio Gain"});
            this.cboKnobBOff.Location = new System.Drawing.Point(113, 99);
            this.cboKnobBOff.Name = "cboKnobBOff";
            this.cboKnobBOff.Size = new System.Drawing.Size(90, 21);
            this.cboKnobBOff.TabIndex = 88;
            this.toolTip1.SetToolTip(this.cboKnobBOff, "Select Mode B Off function to perform.");
            this.cboKnobBOff.SelectedIndexChanged += new System.EventHandler(this.cboKnobBOff_SelectedIndexChanged);
            // 
            // label221
            // 
            this.label221.AutoSize = true;
            this.label221.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label221.ForeColor = System.Drawing.Color.Navy;
            this.label221.Location = new System.Drawing.Point(2, 102);
            this.label221.Name = "label221";
            this.label221.Size = new System.Drawing.Size(14, 13);
            this.label221.TabIndex = 86;
            this.label221.Text = "B";
            this.label221.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label220
            // 
            this.label220.AutoSize = true;
            this.label220.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label220.ForeColor = System.Drawing.Color.Navy;
            this.label220.Location = new System.Drawing.Point(2, 76);
            this.label220.Name = "label220";
            this.label220.Size = new System.Drawing.Size(14, 13);
            this.label220.TabIndex = 85;
            this.label220.Text = "A";
            this.label220.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label219
            // 
            this.label219.AutoSize = true;
            this.label219.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label219.ForeColor = System.Drawing.Color.Navy;
            this.label219.Location = new System.Drawing.Point(227, 57);
            this.label219.Name = "label219";
            this.label219.Size = new System.Drawing.Size(41, 13);
            this.label219.TabIndex = 84;
            this.label219.Text = "Dbl-Clk";
            this.label219.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label218
            // 
            this.label218.AutoSize = true;
            this.label218.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label218.ForeColor = System.Drawing.Color.Navy;
            this.label218.Location = new System.Drawing.Point(140, 57);
            this.label218.Name = "label218";
            this.label218.Size = new System.Drawing.Size(21, 13);
            this.label218.TabIndex = 83;
            this.label218.Text = "Off";
            this.label218.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label217
            // 
            this.label217.AutoSize = true;
            this.label217.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label217.ForeColor = System.Drawing.Color.Navy;
            this.label217.Location = new System.Drawing.Point(45, 57);
            this.label217.Name = "label217";
            this.label217.Size = new System.Drawing.Size(21, 13);
            this.label217.TabIndex = 82;
            this.label217.Text = "On";
            this.label217.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboKnobADC
            // 
            this.cboKnobADC.BackColor = System.Drawing.SystemColors.Window;
            this.cboKnobADC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboKnobADC.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboKnobADC.FormattingEnabled = true;
            this.cboKnobADC.Items.AddRange(new object[] {
            "Swap Tx Vfo",
            "Swap A<>B",
            "Split On/Off",
            "Clear RIT",
            "Clear XIT"});
            this.cboKnobADC.Location = new System.Drawing.Point(209, 72);
            this.cboKnobADC.Name = "cboKnobADC";
            this.cboKnobADC.Size = new System.Drawing.Size(90, 21);
            this.cboKnobADC.TabIndex = 81;
            this.toolTip1.SetToolTip(this.cboKnobADC, "Select Mode A Double-Click function to perform.");
            this.cboKnobADC.SelectedIndexChanged += new System.EventHandler(this.cboKnobADC_SelectedIndexChanged);
            // 
            // cboKnobAOn
            // 
            this.cboKnobAOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboKnobAOn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboKnobAOn.FormattingEnabled = true;
            this.cboKnobAOn.Items.AddRange(new object[] {
            "Tune VFO A",
            "Tune VFO B",
            "Tune RIT",
            "Tune XIT",
            "Audio Gain"});
            this.cboKnobAOn.Location = new System.Drawing.Point(17, 72);
            this.cboKnobAOn.Name = "cboKnobAOn";
            this.cboKnobAOn.Size = new System.Drawing.Size(90, 21);
            this.cboKnobAOn.TabIndex = 80;
            this.cboKnobAOn.Text = " ";
            this.toolTip1.SetToolTip(this.cboKnobAOn, "Select Mode A On function to perform.");
            this.cboKnobAOn.SelectedIndexChanged += new System.EventHandler(this.cboKnobAOn_SelectedIndexChanged);
            // 
            // cboKnobAOff
            // 
            this.cboKnobAOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboKnobAOff.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboKnobAOff.FormattingEnabled = true;
            this.cboKnobAOff.Items.AddRange(new object[] {
            "Tune VFO A",
            "Tune VFO B",
            "Tune RIT",
            "Tune XIT",
            "Audio Gain"});
            this.cboKnobAOff.Location = new System.Drawing.Point(113, 72);
            this.cboKnobAOff.Name = "cboKnobAOff";
            this.cboKnobAOff.Size = new System.Drawing.Size(90, 21);
            this.cboKnobAOff.TabIndex = 79;
            this.toolTip1.SetToolTip(this.cboKnobAOff, "Select Mode A Off function to perform.");
            this.cboKnobAOff.SelectedIndexChanged += new System.EventHandler(this.cboKnobAOff_SelectedIndexChanged);
            // 
            // label216
            // 
            this.label216.AutoSize = true;
            this.label216.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label216.ForeColor = System.Drawing.Color.Navy;
            this.label216.Location = new System.Drawing.Point(89, 14);
            this.label216.Name = "label216";
            this.label216.Size = new System.Drawing.Size(26, 13);
            this.label216.TabIndex = 78;
            this.label216.Text = "Port";
            this.label216.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cboKnobPort
            // 
            this.cboKnobPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboKnobPort.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboKnobPort.FormattingEnabled = true;
            this.cboKnobPort.Location = new System.Drawing.Point(77, 29);
            this.cboKnobPort.Name = "cboKnobPort";
            this.cboKnobPort.Size = new System.Drawing.Size(69, 21);
            this.cboKnobPort.TabIndex = 75;
            this.toolTip1.SetToolTip(this.cboKnobPort, "Serial Port for knob (see docs).");
            this.cboKnobPort.SelectedIndexChanged += new System.EventHandler(this.cboKnobPort_SelectedIndexChanged);
            // 
            // chkKnobEnab
            // 
            this.chkKnobEnab.AutoSize = true;
            this.chkKnobEnab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkKnobEnab.ForeColor = System.Drawing.Color.Navy;
            this.chkKnobEnab.Location = new System.Drawing.Point(17, 31);
            this.chkKnobEnab.Name = "chkKnobEnab";
            this.chkKnobEnab.Size = new System.Drawing.Size(59, 17);
            this.chkKnobEnab.TabIndex = 76;
            this.chkKnobEnab.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkKnobEnab, "Check to enable the Tuning Knob");
            this.chkKnobEnab.UseVisualStyleBackColor = true;
            this.chkKnobEnab.CheckedChanged += new System.EventHandler(this.chkKnobEnab_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label227);
            this.groupBox7.Controls.Add(this.label154);
            this.groupBox7.Controls.Add(this.label15);
            this.groupBox7.Controls.Add(this.label18);
            this.groupBox7.Controls.Add(this.label16);
            this.groupBox7.Controls.Add(this.label66);
            this.groupBox7.Controls.Add(this.label65);
            this.groupBox7.Controls.Add(this.cboRCP4Rotor);
            this.groupBox7.Controls.Add(this.cboRCP3Rotor);
            this.groupBox7.Controls.Add(this.cboRCP2Rotor);
            this.groupBox7.Controls.Add(this.chkRCP2);
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
            this.groupBox7.ForeColor = System.Drawing.Color.Blue;
            this.groupBox7.Location = new System.Drawing.Point(5, 2);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(283, 110);
            this.groupBox7.TabIndex = 66;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "RCPs";
            this.toolTip1.SetToolTip(this.groupBox7, "Radio Control Programs");
            // 
            // label227
            // 
            this.label227.AutoSize = true;
            this.label227.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label227.ForeColor = System.Drawing.Color.Navy;
            this.label227.Location = new System.Drawing.Point(181, 9);
            this.label227.Name = "label227";
            this.label227.Size = new System.Drawing.Size(22, 13);
            this.label227.TabIndex = 77;
            this.label227.Text = "Dis";
            // 
            // label154
            // 
            this.label154.AutoSize = true;
            this.label154.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label154.ForeColor = System.Drawing.Color.Navy;
            this.label154.Location = new System.Drawing.Point(155, 9);
            this.label154.Name = "label154";
            this.label154.Size = new System.Drawing.Size(16, 13);
            this.label154.TabIndex = 76;
            this.label154.Text = "IF";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label15.ForeColor = System.Drawing.Color.Navy;
            this.label15.Location = new System.Drawing.Point(42, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 13);
            this.label15.TabIndex = 75;
            this.label15.Text = "Enab";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label18.ForeColor = System.Drawing.Color.Navy;
            this.label18.Location = new System.Drawing.Point(5, 84);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 13);
            this.label18.TabIndex = 74;
            this.label18.Text = "RCP4";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label16.ForeColor = System.Drawing.Color.Navy;
            this.label16.Location = new System.Drawing.Point(5, 55);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 13);
            this.label16.TabIndex = 73;
            this.label16.Text = "RCP3";
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label66.ForeColor = System.Drawing.Color.Navy;
            this.label66.Location = new System.Drawing.Point(211, 9);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(55, 13);
            this.label66.TabIndex = 72;
            this.label66.Text = "Rotor Port";
            this.label66.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label65.ForeColor = System.Drawing.Color.Navy;
            this.label65.Location = new System.Drawing.Point(81, 9);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(50, 13);
            this.label65.TabIndex = 71;
            this.label65.Text = "CAT Port";
            this.label65.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cboRCP4Rotor
            // 
            this.cboRCP4Rotor.Enabled = false;
            this.cboRCP4Rotor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboRCP4Rotor.FormattingEnabled = true;
            this.cboRCP4Rotor.Location = new System.Drawing.Point(211, 80);
            this.cboRCP4Rotor.Name = "cboRCP4Rotor";
            this.cboRCP4Rotor.Size = new System.Drawing.Size(66, 21);
            this.cboRCP4Rotor.TabIndex = 67;
            this.toolTip1.SetToolTip(this.cboRCP4Rotor, "Rotor Port for RCP 4");
            this.cboRCP4Rotor.SelectedIndexChanged += new System.EventHandler(this.cboRCP4Rotor_SelectedIndexChanged);
            // 
            // cboRCP3Rotor
            // 
            this.cboRCP3Rotor.Enabled = false;
            this.cboRCP3Rotor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboRCP3Rotor.FormattingEnabled = true;
            this.cboRCP3Rotor.Location = new System.Drawing.Point(211, 52);
            this.cboRCP3Rotor.Name = "cboRCP3Rotor";
            this.cboRCP3Rotor.Size = new System.Drawing.Size(66, 21);
            this.cboRCP3Rotor.TabIndex = 66;
            this.toolTip1.SetToolTip(this.cboRCP3Rotor, "Rotor Port for RCP 3");
            this.cboRCP3Rotor.SelectedIndexChanged += new System.EventHandler(this.cboRCP3Rotor_SelectedIndexChanged);
            // 
            // cboRCP2Rotor
            // 
            this.cboRCP2Rotor.Enabled = false;
            this.cboRCP2Rotor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboRCP2Rotor.FormattingEnabled = true;
            this.cboRCP2Rotor.Location = new System.Drawing.Point(211, 24);
            this.cboRCP2Rotor.Name = "cboRCP2Rotor";
            this.cboRCP2Rotor.Size = new System.Drawing.Size(66, 21);
            this.cboRCP2Rotor.TabIndex = 65;
            this.toolTip1.SetToolTip(this.cboRCP2Rotor, "Rotor Port for RCP 2");
            this.cboRCP2Rotor.SelectedIndexChanged += new System.EventHandler(this.cboRCP2Rotor_SelectedIndexChanged);
            // 
            // chkRCP2
            // 
            this.chkRCP2.AutoSize = true;
            this.chkRCP2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP2.Location = new System.Drawing.Point(50, 28);
            this.chkRCP2.Name = "chkRCP2";
            this.chkRCP2.Size = new System.Drawing.Size(15, 14);
            this.chkRCP2.TabIndex = 48;
            this.toolTip1.SetToolTip(this.chkRCP2, "Check to enable RCP 2");
            this.chkRCP2.UseVisualStyleBackColor = true;
            this.chkRCP2.CheckedChanged += new System.EventHandler(this.chkRCP2_CheckedChanged);
            // 
            // cboRCP2
            // 
            this.cboRCP2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboRCP2.FormattingEnabled = true;
            this.cboRCP2.Location = new System.Drawing.Point(78, 24);
            this.cboRCP2.Name = "cboRCP2";
            this.cboRCP2.Size = new System.Drawing.Size(66, 21);
            this.cboRCP2.TabIndex = 46;
            this.toolTip1.SetToolTip(this.cboRCP2, "RCP2 CAT Port");
            this.cboRCP2.SelectedIndexChanged += new System.EventHandler(this.cboRCP2_SelectedIndexChanged);
            // 
            // chkRCP4IF
            // 
            this.chkRCP4IF.AutoSize = true;
            this.chkRCP4IF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP4IF.Location = new System.Drawing.Point(156, 84);
            this.chkRCP4IF.Name = "chkRCP4IF";
            this.chkRCP4IF.Size = new System.Drawing.Size(15, 14);
            this.chkRCP4IF.TabIndex = 62;
            this.toolTip1.SetToolTip(this.chkRCP4IF, "Check to disable all CAT commands but IF");
            this.chkRCP4IF.UseVisualStyleBackColor = true;
            this.chkRCP4IF.CheckedChanged += new System.EventHandler(this.chkRCP4IF_CheckedChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label17.ForeColor = System.Drawing.Color.Navy;
            this.label17.Location = new System.Drawing.Point(6, 28);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(35, 13);
            this.label17.TabIndex = 49;
            this.label17.Text = "RCP2";
            // 
            // chkRCP3IF
            // 
            this.chkRCP3IF.AutoSize = true;
            this.chkRCP3IF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP3IF.Location = new System.Drawing.Point(156, 57);
            this.chkRCP3IF.Name = "chkRCP3IF";
            this.chkRCP3IF.Size = new System.Drawing.Size(15, 14);
            this.chkRCP3IF.TabIndex = 61;
            this.toolTip1.SetToolTip(this.chkRCP3IF, "Check to disable all CAT commands but IF");
            this.chkRCP3IF.UseVisualStyleBackColor = true;
            this.chkRCP3IF.CheckedChanged += new System.EventHandler(this.chkRCP3IF_CheckedChanged);
            // 
            // chkRCP3
            // 
            this.chkRCP3.AutoSize = true;
            this.chkRCP3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP3.Location = new System.Drawing.Point(50, 56);
            this.chkRCP3.Name = "chkRCP3";
            this.chkRCP3.Size = new System.Drawing.Size(15, 14);
            this.chkRCP3.TabIndex = 51;
            this.toolTip1.SetToolTip(this.chkRCP3, "Check to enable RCP 3");
            this.chkRCP3.UseVisualStyleBackColor = true;
            this.chkRCP3.CheckedChanged += new System.EventHandler(this.chkRCP3_CheckedChanged);
            // 
            // chkRCP2IF
            // 
            this.chkRCP2IF.AutoSize = true;
            this.chkRCP2IF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP2IF.Location = new System.Drawing.Point(156, 28);
            this.chkRCP2IF.Name = "chkRCP2IF";
            this.chkRCP2IF.Size = new System.Drawing.Size(15, 14);
            this.chkRCP2IF.TabIndex = 60;
            this.toolTip1.SetToolTip(this.chkRCP2IF, "Check to disable all CAT commands but IF");
            this.chkRCP2IF.UseVisualStyleBackColor = true;
            this.chkRCP2IF.CheckedChanged += new System.EventHandler(this.chkRCP2IF_CheckedChanged);
            // 
            // cboRCP3
            // 
            this.cboRCP3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboRCP3.FormattingEnabled = true;
            this.cboRCP3.Location = new System.Drawing.Point(78, 52);
            this.cboRCP3.Name = "cboRCP3";
            this.cboRCP3.Size = new System.Drawing.Size(66, 21);
            this.cboRCP3.TabIndex = 50;
            this.toolTip1.SetToolTip(this.cboRCP3, "RCP 3 CAT Port");
            this.cboRCP3.SelectedIndexChanged += new System.EventHandler(this.cboRCP3_SelectedIndexChanged);
            // 
            // chkRCP4DisPol
            // 
            this.chkRCP4DisPol.AutoSize = true;
            this.chkRCP4DisPol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP4DisPol.Location = new System.Drawing.Point(184, 84);
            this.chkRCP4DisPol.Name = "chkRCP4DisPol";
            this.chkRCP4DisPol.Size = new System.Drawing.Size(15, 14);
            this.chkRCP4DisPol.TabIndex = 59;
            this.toolTip1.SetToolTip(this.chkRCP4DisPol, "Check to disable polling for RCP 4");
            this.chkRCP4DisPol.UseVisualStyleBackColor = true;
            this.chkRCP4DisPol.CheckedChanged += new System.EventHandler(this.chkRCP4DisPol_CheckedChanged);
            // 
            // chkRCP4
            // 
            this.chkRCP4.AutoSize = true;
            this.chkRCP4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP4.Location = new System.Drawing.Point(50, 84);
            this.chkRCP4.Name = "chkRCP4";
            this.chkRCP4.Size = new System.Drawing.Size(15, 14);
            this.chkRCP4.TabIndex = 54;
            this.toolTip1.SetToolTip(this.chkRCP4, "Check to enable RCP 4");
            this.chkRCP4.UseVisualStyleBackColor = true;
            this.chkRCP4.CheckedChanged += new System.EventHandler(this.chkRCP4_CheckedChanged);
            // 
            // chkRCP3DisPol
            // 
            this.chkRCP3DisPol.AutoSize = true;
            this.chkRCP3DisPol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP3DisPol.Location = new System.Drawing.Point(184, 57);
            this.chkRCP3DisPol.Name = "chkRCP3DisPol";
            this.chkRCP3DisPol.Size = new System.Drawing.Size(15, 14);
            this.chkRCP3DisPol.TabIndex = 58;
            this.toolTip1.SetToolTip(this.chkRCP3DisPol, "Check to disable polling for RCP 3");
            this.chkRCP3DisPol.UseVisualStyleBackColor = true;
            this.chkRCP3DisPol.CheckedChanged += new System.EventHandler(this.chkRCP3DisPol_CheckedChanged);
            // 
            // cboRCP4
            // 
            this.cboRCP4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboRCP4.FormattingEnabled = true;
            this.cboRCP4.Location = new System.Drawing.Point(78, 80);
            this.cboRCP4.Name = "cboRCP4";
            this.cboRCP4.Size = new System.Drawing.Size(66, 21);
            this.cboRCP4.TabIndex = 53;
            this.toolTip1.SetToolTip(this.cboRCP4, "RCP 4 CAT Port");
            this.cboRCP4.SelectedIndexChanged += new System.EventHandler(this.cboRCP4_SelectedIndexChanged);
            // 
            // chkRCP2DisPol
            // 
            this.chkRCP2DisPol.AutoSize = true;
            this.chkRCP2DisPol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP2DisPol.Location = new System.Drawing.Point(184, 28);
            this.chkRCP2DisPol.Name = "chkRCP2DisPol";
            this.chkRCP2DisPol.Size = new System.Drawing.Size(15, 14);
            this.chkRCP2DisPol.TabIndex = 57;
            this.toolTip1.SetToolTip(this.chkRCP2DisPol, "Check to disable polling for RCP 2");
            this.chkRCP2DisPol.UseVisualStyleBackColor = true;
            this.chkRCP2DisPol.CheckedChanged += new System.EventHandler(this.chkRCP2DisPol_CheckedChanged);
            // 
            // txtRCP
            // 
            this.txtRCP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtRCP.Location = new System.Drawing.Point(293, 8);
            this.txtRCP.Multiline = true;
            this.txtRCP.Name = "txtRCP";
            this.txtRCP.Size = new System.Drawing.Size(132, 104);
            this.txtRCP.TabIndex = 63;
            this.txtRCP.Text = "RCP1 Commander\r\nRCP2 MixW\r\nRCP3 WriteLog\r\nRCP4 DXBase";
            this.toolTip1.SetToolTip(this.txtRCP, "Program Notes: Use this to keep track of which port your programs are on.");
            this.txtRCP.WordWrap = false;
            this.txtRCP.TextChanged += new System.EventHandler(this.txtRCP_TextChanged);
            // 
            // tabWatt
            // 
            this.tabWatt.Controls.Add(this.groupBox15);
            this.tabWatt.Controls.Add(this.groupBox2);
            this.tabWatt.Controls.Add(this.grpWN2);
            this.tabWatt.Controls.Add(this.grpLP);
            this.tabWatt.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabWatt.Location = new System.Drawing.Point(4, 22);
            this.tabWatt.Name = "tabWatt";
            this.tabWatt.Size = new System.Drawing.Size(431, 311);
            this.tabWatt.TabIndex = 2;
            this.tabWatt.Text = "Watt";
            this.tabWatt.UseVisualStyleBackColor = true;
            this.tabWatt.DoubleClick += new System.EventHandler(this.tabOther_DoubleClick);
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.chkSWR);
            this.groupBox15.Controls.Add(this.numSWR);
            this.groupBox15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox15.ForeColor = System.Drawing.Color.Blue;
            this.groupBox15.Location = new System.Drawing.Point(200, 181);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(121, 49);
            this.groupBox15.TabIndex = 81;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "SWR Alarm";
            this.toolTip1.SetToolTip(this.groupBox15, "If using one of these watt meters, enableing this control will disable PSDR trans" +
                    "mit if SWR setting is exceeded.");
            // 
            // chkSWR
            // 
            this.chkSWR.AutoSize = true;
            this.chkSWR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSWR.ForeColor = System.Drawing.Color.Firebrick;
            this.chkSWR.Location = new System.Drawing.Point(10, 21);
            this.chkSWR.Name = "chkSWR";
            this.chkSWR.Size = new System.Drawing.Size(59, 17);
            this.chkSWR.TabIndex = 80;
            this.chkSWR.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkSWR, "Check to enable SWR alarm");
            this.chkSWR.UseVisualStyleBackColor = true;
            this.chkSWR.CheckedChanged += new System.EventHandler(this.chkSWR_CheckedChanged);
            // 
            // numSWR
            // 
            this.numSWR.DecimalPlaces = 1;
            this.numSWR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSWR.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numSWR.Location = new System.Drawing.Point(72, 19);
            this.numSWR.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSWR.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSWR.Name = "numSWR";
            this.numSWR.Size = new System.Drawing.Size(39, 20);
            this.numSWR.TabIndex = 79;
            this.numSWR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numSWR, "Set the maximum SWR before Transmit Disable");
            this.numSWR.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numSWR.ValueChanged += new System.EventHandler(this.numSWR_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grpNet);
            this.groupBox2.Controls.Add(this.numRev);
            this.groupBox2.Controls.Add(this.numFwd);
            this.groupBox2.Controls.Add(this.cboPMcom);
            this.groupBox2.Controls.Add(this.label142);
            this.groupBox2.Controls.Add(this.label138);
            this.groupBox2.Controls.Add(this.label139);
            this.groupBox2.Controls.Add(this.cboPMport);
            this.groupBox2.Controls.Add(this.chkPM);
            this.groupBox2.Controls.Add(this.label140);
            this.groupBox2.Controls.Add(this.label141);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Location = new System.Drawing.Point(7, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 104);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Pwr Master";
            // 
            // grpNet
            // 
            this.grpNet.Controls.Add(this.rbNet);
            this.grpNet.Controls.Add(this.rbStd);
            this.grpNet.Location = new System.Drawing.Point(87, 8);
            this.grpNet.Name = "grpNet";
            this.grpNet.Size = new System.Drawing.Size(91, 26);
            this.grpNet.TabIndex = 77;
            this.grpNet.TabStop = false;
            // 
            // rbNet
            // 
            this.rbNet.AutoSize = true;
            this.rbNet.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbNet.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbNet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbNet.ForeColor = System.Drawing.Color.Navy;
            this.rbNet.Location = new System.Drawing.Point(44, 6);
            this.rbNet.Name = "rbNet";
            this.rbNet.Size = new System.Drawing.Size(42, 17);
            this.rbNet.TabIndex = 71;
            this.rbNet.Text = "Net";
            this.toolTip1.SetToolTip(this.rbNet, "Selects Forward Power - Reflected Power (Net) to display.");
            this.rbNet.UseVisualStyleBackColor = true;
            this.rbNet.CheckedChanged += new System.EventHandler(this.grpNet_CheckedChanged);
            // 
            // rbStd
            // 
            this.rbStd.AutoSize = true;
            this.rbStd.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbStd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbStd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbStd.ForeColor = System.Drawing.Color.Navy;
            this.rbStd.Location = new System.Drawing.Point(4, 6);
            this.rbStd.Name = "rbStd";
            this.rbStd.Size = new System.Drawing.Size(41, 17);
            this.rbStd.TabIndex = 70;
            this.rbStd.Text = "Std";
            this.toolTip1.SetToolTip(this.rbStd, "Selects Forward Power (Std) to display.");
            this.rbStd.UseVisualStyleBackColor = true;
            this.rbStd.CheckedChanged += new System.EventHandler(this.grpNet_CheckedChanged);
            // 
            // numRev
            // 
            this.numRev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numRev.Location = new System.Drawing.Point(130, 77);
            this.numRev.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numRev.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            -2147483648});
            this.numRev.Name = "numRev";
            this.numRev.Size = new System.Drawing.Size(42, 20);
            this.numRev.TabIndex = 76;
            this.numRev.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numRev, "Displays/Sets the Rev Trim setting for the coupler being used.");
            this.numRev.ValueChanged += new System.EventHandler(this.numRev_ValueChanged);
            // 
            // numFwd
            // 
            this.numFwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numFwd.Location = new System.Drawing.Point(130, 50);
            this.numFwd.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numFwd.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            -2147483648});
            this.numFwd.Name = "numFwd";
            this.numFwd.Size = new System.Drawing.Size(42, 20);
            this.numFwd.TabIndex = 75;
            this.numFwd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numFwd, "Displays/Sets the Fwd Trim setting for the coupler being used.");
            this.numFwd.ValueChanged += new System.EventHandler(this.numFwd_ValueChanged);
            // 
            // cboPMcom
            // 
            this.cboPMcom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPMcom.FormattingEnabled = true;
            this.cboPMcom.Items.AddRange(new object[] {
            "38400",
            "19200",
            "9600"});
            this.cboPMcom.Location = new System.Drawing.Point(6, 77);
            this.cboPMcom.Name = "cboPMcom";
            this.cboPMcom.Size = new System.Drawing.Size(63, 21);
            this.cboPMcom.TabIndex = 50;
            this.toolTip1.SetToolTip(this.cboPMcom, "Select Port Baud Rate");
            this.cboPMcom.SelectedIndexChanged += new System.EventHandler(this.cboPMcom_SelectedIndexChanged);
            // 
            // label142
            // 
            this.label142.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label142.ForeColor = System.Drawing.Color.Navy;
            this.label142.Location = new System.Drawing.Point(13, 65);
            this.label142.Name = "label142";
            this.label142.Size = new System.Drawing.Size(38, 15);
            this.label142.TabIndex = 51;
            this.label142.Text = "Comm";
            // 
            // label138
            // 
            this.label138.AutoSize = true;
            this.label138.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label138.ForeColor = System.Drawing.Color.Navy;
            this.label138.Location = new System.Drawing.Point(101, 53);
            this.label138.Name = "label138";
            this.label138.Size = new System.Drawing.Size(27, 13);
            this.label138.TabIndex = 48;
            this.label138.Text = "Fwd";
            this.label138.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label139
            // 
            this.label139.AutoSize = true;
            this.label139.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label139.ForeColor = System.Drawing.Color.Navy;
            this.label139.Location = new System.Drawing.Point(101, 79);
            this.label139.Name = "label139";
            this.label139.Size = new System.Drawing.Size(27, 13);
            this.label139.TabIndex = 46;
            this.label139.Text = "Rev";
            this.label139.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cboPMport
            // 
            this.cboPMport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPMport.FormattingEnabled = true;
            this.cboPMport.Location = new System.Drawing.Point(6, 43);
            this.cboPMport.Name = "cboPMport";
            this.cboPMport.Size = new System.Drawing.Size(63, 21);
            this.cboPMport.TabIndex = 17;
            this.toolTip1.SetToolTip(this.cboPMport, "Select Port for Power Master");
            this.cboPMport.SelectedIndexChanged += new System.EventHandler(this.cboPMport_SelectedIndexChanged);
            // 
            // chkPM
            // 
            this.chkPM.AutoSize = true;
            this.chkPM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPM.ForeColor = System.Drawing.Color.Firebrick;
            this.chkPM.Location = new System.Drawing.Point(6, 15);
            this.chkPM.Name = "chkPM";
            this.chkPM.Size = new System.Drawing.Size(59, 17);
            this.chkPM.TabIndex = 45;
            this.chkPM.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkPM, "Check to enable");
            this.chkPM.UseVisualStyleBackColor = true;
            this.chkPM.CheckedChanged += new System.EventHandler(this.chkPM_CheckedChanged);
            // 
            // label140
            // 
            this.label140.AutoSize = true;
            this.label140.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label140.ForeColor = System.Drawing.Color.Navy;
            this.label140.Location = new System.Drawing.Point(138, 36);
            this.label140.Name = "label140";
            this.label140.Size = new System.Drawing.Size(27, 13);
            this.label140.TabIndex = 16;
            this.label140.Text = "Trim";
            this.label140.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label141
            // 
            this.label141.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label141.ForeColor = System.Drawing.Color.Navy;
            this.label141.Location = new System.Drawing.Point(15, 30);
            this.label141.Name = "label141";
            this.label141.Size = new System.Drawing.Size(30, 15);
            this.label141.TabIndex = 18;
            this.label141.Text = "Port";
            this.label141.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // grpWN2
            // 
            this.grpWN2.Controls.Add(this.btnWnSensor);
            this.grpWN2.Controls.Add(this.btnWnReset);
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
            this.grpWN2.ForeColor = System.Drawing.Color.Blue;
            this.grpWN2.Location = new System.Drawing.Point(200, 2);
            this.grpWN2.Name = "grpWN2";
            this.grpWN2.Size = new System.Drawing.Size(210, 138);
            this.grpWN2.TabIndex = 0;
            this.grpWN2.TabStop = false;
            this.grpWN2.Text = "WaveNode";
            // 
            // btnWnSensor
            // 
            this.btnWnSensor.BackColor = System.Drawing.Color.Lime;
            this.btnWnSensor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWnSensor.Location = new System.Drawing.Point(83, 10);
            this.btnWnSensor.Name = "btnWnSensor";
            this.btnWnSensor.Size = new System.Drawing.Size(10, 10);
            this.btnWnSensor.TabIndex = 73;
            this.toolTip1.SetToolTip(this.btnWnSensor, "Press to display sensor matrix.");
            this.btnWnSensor.UseVisualStyleBackColor = false;
            this.btnWnSensor.Click += new System.EventHandler(this.btnWnSensor_Click);
            // 
            // btnWnReset
            // 
            this.btnWnReset.BackColor = System.Drawing.Color.Violet;
            this.btnWnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWnReset.Location = new System.Drawing.Point(193, 10);
            this.btnWnReset.Name = "btnWnReset";
            this.btnWnReset.Size = new System.Drawing.Size(10, 10);
            this.btnWnReset.TabIndex = 72;
            this.toolTip1.SetToolTip(this.btnWnReset, "Master Reset for the WN2 hardware. Use only after toggling Enabled check box fail" +
                    "s to correct .");
            this.btnWnReset.UseVisualStyleBackColor = false;
            this.btnWnReset.Click += new System.EventHandler(this.btnWnReset_Click);
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
            this.chkWNEnab.ForeColor = System.Drawing.Color.Firebrick;
            this.chkWNEnab.Location = new System.Drawing.Point(10, 15);
            this.chkWNEnab.Name = "chkWNEnab";
            this.chkWNEnab.Size = new System.Drawing.Size(59, 17);
            this.chkWNEnab.TabIndex = 5;
            this.chkWNEnab.Text = "Enable";
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
            this.grpLP.ForeColor = System.Drawing.Color.Blue;
            this.grpLP.Location = new System.Drawing.Point(7, 110);
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
            this.label13.ForeColor = System.Drawing.Color.Navy;
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
            this.btnMode.BackColor = System.Drawing.Color.Transparent;
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
            this.btnFast.BackColor = System.Drawing.Color.Transparent;
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
            this.btnAlarm.BackColor = System.Drawing.Color.Transparent;
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
            this.chkLPenab.ForeColor = System.Drawing.Color.Firebrick;
            this.chkLPenab.Location = new System.Drawing.Point(10, 15);
            this.chkLPenab.Name = "chkLPenab";
            this.chkLPenab.Size = new System.Drawing.Size(59, 17);
            this.chkLPenab.TabIndex = 45;
            this.chkLPenab.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkLPenab, "Check to enable LP100 polling");
            this.chkLPenab.UseVisualStyleBackColor = true;
            this.chkLPenab.CheckedChanged += new System.EventHandler(this.chkLPenab_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(20, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 18;
            this.label2.Text = "Port";
            // 
            // tabAmp
            // 
            this.tabAmp.Controls.Add(this.grpSPE);
            this.tabAmp.Controls.Add(this.grpAmpBand);
            this.tabAmp.Controls.Add(this.grpPTT);
            this.tabAmp.Controls.Add(this.groupBox13);
            this.tabAmp.Controls.Add(this.grpAmp);
            this.tabAmp.Controls.Add(this.grpAmpBands);
            this.tabAmp.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F);
            this.tabAmp.Location = new System.Drawing.Point(4, 22);
            this.tabAmp.Name = "tabAmp";
            this.tabAmp.Size = new System.Drawing.Size(431, 311);
            this.tabAmp.TabIndex = 7;
            this.tabAmp.Text = "Amps";
            this.tabAmp.UseVisualStyleBackColor = true;
            // 
            // grpSPE
            // 
            this.grpSPE.Controls.Add(this.btnSPEright);
            this.grpSPE.Controls.Add(this.btnSPEset);
            this.grpSPE.Controls.Add(this.btnSPEleft);
            this.grpSPE.Controls.Add(this.rbLed);
            this.grpSPE.Controls.Add(this.label161);
            this.grpSPE.Controls.Add(this.txtSPEant);
            this.grpSPE.Controls.Add(this.btnSPEant);
            this.grpSPE.Controls.Add(this.btnSPEdisp);
            this.grpSPE.Controls.Add(this.btnSPEoff);
            this.grpSPE.Controls.Add(this.label157);
            this.grpSPE.Controls.Add(this.btnSPEpwr);
            this.grpSPE.Controls.Add(this.txtSPEmsg);
            this.grpSPE.Controls.Add(this.btnSPEtune);
            this.grpSPE.Controls.Add(this.label158);
            this.grpSPE.Controls.Add(this.cboSPEport);
            this.grpSPE.Controls.Add(this.chkSPEenab);
            this.grpSPE.Controls.Add(this.txtSPEtemp);
            this.grpSPE.Controls.Add(this.btnSPEoper);
            this.grpSPE.Controls.Add(this.btnSPEon);
            this.grpSPE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSPE.ForeColor = System.Drawing.Color.Blue;
            this.grpSPE.Location = new System.Drawing.Point(3, 2);
            this.grpSPE.Name = "grpSPE";
            this.grpSPE.Size = new System.Drawing.Size(225, 162);
            this.grpSPE.TabIndex = 112;
            this.grpSPE.TabStop = false;
            this.grpSPE.Text = "SPE 1K-FA";
            this.toolTip1.SetToolTip(this.grpSPE, "SPE 1K-FA Amplifier controls");
            // 
            // btnSPEright
            // 
            this.btnSPEright.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSPEright.BackColor = System.Drawing.Color.Transparent;
            this.btnSPEright.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSPEright.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this.btnSPEright.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSPEright.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnSPEright.ForeColor = System.Drawing.Color.Navy;
            this.btnSPEright.Location = new System.Drawing.Point(86, 49);
            this.btnSPEright.Name = "btnSPEright";
            this.btnSPEright.Size = new System.Drawing.Size(31, 20);
            this.btnSPEright.TabIndex = 116;
            this.btnSPEright.Text = ">>>";
            this.toolTip1.SetToolTip(this.btnSPEright, "Press to initiate --> function on amp.");
            this.btnSPEright.UseVisualStyleBackColor = false;
            this.btnSPEright.Click += new System.EventHandler(this.btnSPEright_Click);
            // 
            // btnSPEset
            // 
            this.btnSPEset.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSPEset.BackColor = System.Drawing.Color.Transparent;
            this.btnSPEset.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSPEset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this.btnSPEset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSPEset.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnSPEset.ForeColor = System.Drawing.Color.Navy;
            this.btnSPEset.Location = new System.Drawing.Point(121, 49);
            this.btnSPEset.Name = "btnSPEset";
            this.btnSPEset.Size = new System.Drawing.Size(31, 20);
            this.btnSPEset.TabIndex = 115;
            this.btnSPEset.Text = "SET";
            this.toolTip1.SetToolTip(this.btnSPEset, "Press to initiate SET function on amp.");
            this.btnSPEset.UseVisualStyleBackColor = false;
            this.btnSPEset.Click += new System.EventHandler(this.btnSPEset_Click);
            // 
            // btnSPEleft
            // 
            this.btnSPEleft.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSPEleft.BackColor = System.Drawing.Color.Transparent;
            this.btnSPEleft.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSPEleft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this.btnSPEleft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSPEleft.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnSPEleft.ForeColor = System.Drawing.Color.Navy;
            this.btnSPEleft.Location = new System.Drawing.Point(51, 49);
            this.btnSPEleft.Name = "btnSPEleft";
            this.btnSPEleft.Size = new System.Drawing.Size(31, 20);
            this.btnSPEleft.TabIndex = 114;
            this.btnSPEleft.Text = "<<<";
            this.toolTip1.SetToolTip(this.btnSPEleft, "Press to initiate <-- function on amp.");
            this.btnSPEleft.UseVisualStyleBackColor = false;
            this.btnSPEleft.Click += new System.EventHandler(this.btnSPEleft_Click);
            // 
            // rbLed
            // 
            this.rbLed.AutoSize = true;
            this.rbLed.Location = new System.Drawing.Point(8, 42);
            this.rbLed.Name = "rbLed";
            this.rbLed.Size = new System.Drawing.Size(14, 13);
            this.rbLed.TabIndex = 113;
            this.rbLed.TabStop = true;
            this.toolTip1.SetToolTip(this.rbLed, "Comm activity indicator.");
            this.rbLed.UseVisualStyleBackColor = true;
            // 
            // label161
            // 
            this.label161.AutoSize = true;
            this.label161.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label161.ForeColor = System.Drawing.Color.Navy;
            this.label161.Location = new System.Drawing.Point(180, 10);
            this.label161.Name = "label161";
            this.label161.Size = new System.Drawing.Size(39, 13);
            this.label161.TabIndex = 86;
            this.label161.Text = "Output";
            // 
            // txtSPEant
            // 
            this.txtSPEant.BackColor = System.Drawing.SystemColors.Window;
            this.txtSPEant.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSPEant.Location = new System.Drawing.Point(158, 50);
            this.txtSPEant.Name = "txtSPEant";
            this.txtSPEant.Size = new System.Drawing.Size(15, 18);
            this.txtSPEant.TabIndex = 85;
            this.txtSPEant.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtSPEant, "Selected antenna (yellow background indicates Contest mode).");
            // 
            // btnSPEant
            // 
            this.btnSPEant.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSPEant.BackColor = System.Drawing.Color.Transparent;
            this.btnSPEant.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSPEant.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnSPEant.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSPEant.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnSPEant.ForeColor = System.Drawing.Color.Navy;
            this.btnSPEant.Location = new System.Drawing.Point(179, 49);
            this.btnSPEant.Name = "btnSPEant";
            this.btnSPEant.Size = new System.Drawing.Size(39, 20);
            this.btnSPEant.TabIndex = 82;
            this.btnSPEant.Text = "ANT";
            this.toolTip1.SetToolTip(this.btnSPEant, "Press to select next antenna.");
            this.btnSPEant.UseVisualStyleBackColor = false;
            this.btnSPEant.Click += new System.EventHandler(this.btnSPEant_Click);
            // 
            // btnSPEdisp
            // 
            this.btnSPEdisp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSPEdisp.BackColor = System.Drawing.Color.Transparent;
            this.btnSPEdisp.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSPEdisp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnSPEdisp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSPEdisp.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnSPEdisp.ForeColor = System.Drawing.Color.Navy;
            this.btnSPEdisp.Location = new System.Drawing.Point(179, 73);
            this.btnSPEdisp.Name = "btnSPEdisp";
            this.btnSPEdisp.Size = new System.Drawing.Size(39, 20);
            this.btnSPEdisp.TabIndex = 80;
            this.btnSPEdisp.Text = "Disply";
            this.toolTip1.SetToolTip(this.btnSPEdisp, "Press to display the accumulated error messages.");
            this.btnSPEdisp.UseVisualStyleBackColor = false;
            this.btnSPEdisp.Click += new System.EventHandler(this.btnSPEdisp_Click);
            // 
            // btnSPEoff
            // 
            this.btnSPEoff.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSPEoff.BackColor = System.Drawing.Color.Transparent;
            this.btnSPEoff.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSPEoff.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this.btnSPEoff.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSPEoff.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnSPEoff.ForeColor = System.Drawing.Color.Navy;
            this.btnSPEoff.Location = new System.Drawing.Point(49, 73);
            this.btnSPEoff.Name = "btnSPEoff";
            this.btnSPEoff.Size = new System.Drawing.Size(39, 20);
            this.btnSPEoff.TabIndex = 80;
            this.btnSPEoff.Text = "OFF";
            this.toolTip1.SetToolTip(this.btnSPEoff, "Press to turn Mains Power Off.");
            this.btnSPEoff.UseVisualStyleBackColor = false;
            this.btnSPEoff.Click += new System.EventHandler(this.btnSPEoff_Click);
            // 
            // label157
            // 
            this.label157.AutoSize = true;
            this.label157.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label157.ForeColor = System.Drawing.Color.Navy;
            this.label157.Location = new System.Drawing.Point(73, 9);
            this.label157.Name = "label157";
            this.label157.Size = new System.Drawing.Size(55, 13);
            this.label157.TabIndex = 62;
            this.label157.Text = "Serial Port";
            // 
            // btnSPEpwr
            // 
            this.btnSPEpwr.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSPEpwr.BackColor = System.Drawing.Color.Transparent;
            this.btnSPEpwr.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSPEpwr.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this.btnSPEpwr.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSPEpwr.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnSPEpwr.ForeColor = System.Drawing.Color.Navy;
            this.btnSPEpwr.Location = new System.Drawing.Point(179, 25);
            this.btnSPEpwr.Name = "btnSPEpwr";
            this.btnSPEpwr.Size = new System.Drawing.Size(39, 20);
            this.btnSPEpwr.TabIndex = 81;
            this.btnSPEpwr.Text = "Off";
            this.toolTip1.SetToolTip(this.btnSPEpwr, "Press to toggle Full/Half power.");
            this.btnSPEpwr.UseVisualStyleBackColor = false;
            this.btnSPEpwr.Click += new System.EventHandler(this.btnSPEpwr_Click);
            // 
            // txtSPEmsg
            // 
            this.txtSPEmsg.BackColor = System.Drawing.Color.LightYellow;
            this.txtSPEmsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F);
            this.txtSPEmsg.Location = new System.Drawing.Point(5, 98);
            this.txtSPEmsg.MaxLength = 1500;
            this.txtSPEmsg.Multiline = true;
            this.txtSPEmsg.Name = "txtSPEmsg";
            this.txtSPEmsg.ReadOnly = true;
            this.txtSPEmsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSPEmsg.Size = new System.Drawing.Size(214, 59);
            this.txtSPEmsg.TabIndex = 61;
            this.txtSPEmsg.Text = "- Amp Condition Status\r\n- Error Messages\r\n- Double-Click to Clear\r\nNo Antenna Sel" +
                "ected!";
            this.toolTip1.SetToolTip(this.txtSPEmsg, "Displays Status and Fault messages (double-click to clear).");
            this.txtSPEmsg.DoubleClick += new System.EventHandler(this.txtSPEmsg_DoubleClick);
            // 
            // btnSPEtune
            // 
            this.btnSPEtune.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSPEtune.BackColor = System.Drawing.Color.Transparent;
            this.btnSPEtune.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSPEtune.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnSPEtune.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSPEtune.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnSPEtune.ForeColor = System.Drawing.Color.Navy;
            this.btnSPEtune.Location = new System.Drawing.Point(135, 73);
            this.btnSPEtune.Name = "btnSPEtune";
            this.btnSPEtune.Size = new System.Drawing.Size(39, 20);
            this.btnSPEtune.TabIndex = 79;
            this.btnSPEtune.Text = "Tuner";
            this.toolTip1.SetToolTip(this.btnSPEtune, "Press to activate internal SPE tuner.");
            this.btnSPEtune.UseVisualStyleBackColor = false;
            this.btnSPEtune.Click += new System.EventHandler(this.btnSPEtune_Click);
            // 
            // label158
            // 
            this.label158.AutoSize = true;
            this.label158.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label158.ForeColor = System.Drawing.Color.Navy;
            this.label158.Location = new System.Drawing.Point(140, 10);
            this.label158.Name = "label158";
            this.label158.Size = new System.Drawing.Size(32, 13);
            this.label158.TabIndex = 62;
            this.label158.Text = "Temp";
            // 
            // cboSPEport
            // 
            this.cboSPEport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSPEport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSPEport.ForeColor = System.Drawing.Color.Black;
            this.cboSPEport.FormattingEnabled = true;
            this.cboSPEport.Location = new System.Drawing.Point(65, 23);
            this.cboSPEport.Name = "cboSPEport";
            this.cboSPEport.Size = new System.Drawing.Size(68, 21);
            this.cboSPEport.TabIndex = 61;
            this.toolTip1.SetToolTip(this.cboSPEport, "Select serial port for amp (9600 8N1).");
            this.cboSPEport.SelectedIndexChanged += new System.EventHandler(this.cboSPEport_SelectedIndexChanged);
            // 
            // chkSPEenab
            // 
            this.chkSPEenab.AutoSize = true;
            this.chkSPEenab.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.chkSPEenab.ForeColor = System.Drawing.Color.Navy;
            this.chkSPEenab.Location = new System.Drawing.Point(8, 24);
            this.chkSPEenab.Name = "chkSPEenab";
            this.chkSPEenab.Size = new System.Drawing.Size(59, 17);
            this.chkSPEenab.TabIndex = 61;
            this.chkSPEenab.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkSPEenab, "Select to enable amplifier.");
            this.chkSPEenab.UseVisualStyleBackColor = true;
            this.chkSPEenab.CheckedChanged += new System.EventHandler(this.chkSPEenab_CheckedChanged);
            // 
            // txtSPEtemp
            // 
            this.txtSPEtemp.BackColor = System.Drawing.SystemColors.Window;
            this.txtSPEtemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtSPEtemp.Location = new System.Drawing.Point(139, 25);
            this.txtSPEtemp.Name = "txtSPEtemp";
            this.txtSPEtemp.Size = new System.Drawing.Size(34, 18);
            this.txtSPEtemp.TabIndex = 61;
            this.txtSPEtemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtSPEtemp, "PA temperature");
            // 
            // btnSPEoper
            // 
            this.btnSPEoper.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSPEoper.BackColor = System.Drawing.Color.Transparent;
            this.btnSPEoper.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSPEoper.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnSPEoper.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSPEoper.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnSPEoper.ForeColor = System.Drawing.Color.Navy;
            this.btnSPEoper.Location = new System.Drawing.Point(92, 73);
            this.btnSPEoper.Name = "btnSPEoper";
            this.btnSPEoper.Size = new System.Drawing.Size(39, 20);
            this.btnSPEoper.TabIndex = 73;
            this.btnSPEoper.Text = "Mode";
            this.toolTip1.SetToolTip(this.btnSPEoper, "Press to toggle Operate/StandBy mode.");
            this.btnSPEoper.UseVisualStyleBackColor = false;
            this.btnSPEoper.Click += new System.EventHandler(this.btnSPEoper_Click);
            // 
            // btnSPEon
            // 
            this.btnSPEon.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSPEon.BackColor = System.Drawing.Color.Transparent;
            this.btnSPEon.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSPEon.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this.btnSPEon.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSPEon.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnSPEon.ForeColor = System.Drawing.Color.Navy;
            this.btnSPEon.Location = new System.Drawing.Point(6, 73);
            this.btnSPEon.Name = "btnSPEon";
            this.btnSPEon.Size = new System.Drawing.Size(39, 20);
            this.btnSPEon.TabIndex = 72;
            this.btnSPEon.Text = "PWR";
            this.toolTip1.SetToolTip(this.btnSPEon, "Press to turn Mains Power On.");
            this.btnSPEon.UseVisualStyleBackColor = false;
            this.btnSPEon.Click += new System.EventHandler(this.btnSPEon_Click);
            // 
            // grpAmpBand
            // 
            this.grpAmpBand.Controls.Add(this.label148);
            this.grpAmpBand.Controls.Add(this.numDrive);
            this.grpAmpBand.Controls.Add(this.btnInfo);
            this.grpAmpBand.Controls.Add(this.chkB80);
            this.grpAmpBand.Controls.Add(this.btnBandClear);
            this.grpAmpBand.Controls.Add(this.btnBandAll);
            this.grpAmpBand.Controls.Add(this.chkB160);
            this.grpAmpBand.Controls.Add(this.chkB30);
            this.grpAmpBand.Controls.Add(this.chkB17);
            this.grpAmpBand.Controls.Add(this.chkB40);
            this.grpAmpBand.Controls.Add(this.chkB10);
            this.grpAmpBand.Controls.Add(this.chkB12);
            this.grpAmpBand.Controls.Add(this.chkB20);
            this.grpAmpBand.Controls.Add(this.chkB15);
            this.grpAmpBand.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAmpBand.ForeColor = System.Drawing.Color.Blue;
            this.grpAmpBand.Location = new System.Drawing.Point(331, 168);
            this.grpAmpBand.Name = "grpAmpBand";
            this.grpAmpBand.Size = new System.Drawing.Size(93, 136);
            this.grpAmpBand.TabIndex = 108;
            this.grpAmpBand.TabStop = false;
            this.grpAmpBand.Text = "Bands";
            this.toolTip1.SetToolTip(this.grpAmpBand, "Select band(s) for ACOM 2000A Auto Tune procedure.");
            this.grpAmpBand.Visible = false;
            // 
            // label148
            // 
            this.label148.AutoSize = true;
            this.label148.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label148.ForeColor = System.Drawing.Color.Blue;
            this.label148.Location = new System.Drawing.Point(53, 80);
            this.label148.Name = "label148";
            this.label148.Size = new System.Drawing.Size(30, 13);
            this.label148.TabIndex = 64;
            this.label148.Text = "Tune";
            // 
            // numDrive
            // 
            this.numDrive.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F);
            this.numDrive.Location = new System.Drawing.Point(50, 93);
            this.numDrive.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numDrive.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numDrive.Name = "numDrive";
            this.numDrive.Size = new System.Drawing.Size(39, 19);
            this.numDrive.TabIndex = 109;
            this.numDrive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numDrive, "Select the PowerSDR Tune level.");
            this.numDrive.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numDrive.ValueChanged += new System.EventHandler(this.numDrive_ValueChanged);
            // 
            // btnInfo
            // 
            this.btnInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnInfo.BackColor = System.Drawing.SystemColors.Control;
            this.btnInfo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnInfo.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Info;
            this.btnInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnInfo.Location = new System.Drawing.Point(10, 98);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(30, 15);
            this.btnInfo.TabIndex = 107;
            this.btnInfo.Text = "Info";
            this.toolTip1.SetToolTip(this.btnInfo, "Press for instruction/cautions about using this feature.");
            this.btnInfo.UseVisualStyleBackColor = false;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // chkB80
            // 
            this.chkB80.AutoSize = true;
            this.chkB80.BackColor = System.Drawing.Color.Transparent;
            this.chkB80.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkB80.ForeColor = System.Drawing.Color.Navy;
            this.chkB80.Location = new System.Drawing.Point(53, 16);
            this.chkB80.Name = "chkB80";
            this.chkB80.Size = new System.Drawing.Size(38, 17);
            this.chkB80.TabIndex = 98;
            this.chkB80.Text = "80";
            this.toolTip1.SetToolTip(this.chkB80, "Select to enable Auto Tune on this band.");
            this.chkB80.UseVisualStyleBackColor = false;
            // 
            // btnBandClear
            // 
            this.btnBandClear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBandClear.BackColor = System.Drawing.SystemColors.Control;
            this.btnBandClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBandClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBandClear.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBandClear.Location = new System.Drawing.Point(49, 117);
            this.btnBandClear.Name = "btnBandClear";
            this.btnBandClear.Size = new System.Drawing.Size(40, 15);
            this.btnBandClear.TabIndex = 105;
            this.btnBandClear.Text = "Clr All";
            this.toolTip1.SetToolTip(this.btnBandClear, "Clears all bands.");
            this.btnBandClear.UseVisualStyleBackColor = false;
            this.btnBandClear.Click += new System.EventHandler(this.btnBandClear_Click);
            // 
            // btnBandAll
            // 
            this.btnBandAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBandAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnBandAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBandAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBandAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBandAll.Location = new System.Drawing.Point(5, 117);
            this.btnBandAll.Name = "btnBandAll";
            this.btnBandAll.Size = new System.Drawing.Size(40, 15);
            this.btnBandAll.TabIndex = 106;
            this.btnBandAll.Text = "Chk All";
            this.toolTip1.SetToolTip(this.btnBandAll, "Checks all bands.");
            this.btnBandAll.UseVisualStyleBackColor = false;
            this.btnBandAll.Click += new System.EventHandler(this.btnBandAll_Click);
            // 
            // chkB160
            // 
            this.chkB160.AutoSize = true;
            this.chkB160.BackColor = System.Drawing.Color.Transparent;
            this.chkB160.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkB160.ForeColor = System.Drawing.Color.Navy;
            this.chkB160.Location = new System.Drawing.Point(2, 16);
            this.chkB160.Name = "chkB160";
            this.chkB160.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkB160.Size = new System.Drawing.Size(44, 17);
            this.chkB160.TabIndex = 96;
            this.chkB160.Text = "160";
            this.toolTip1.SetToolTip(this.chkB160, "Select to enable Auto Tune on this band.");
            this.chkB160.UseVisualStyleBackColor = false;
            // 
            // chkB30
            // 
            this.chkB30.AutoSize = true;
            this.chkB30.BackColor = System.Drawing.Color.Transparent;
            this.chkB30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkB30.ForeColor = System.Drawing.Color.Navy;
            this.chkB30.Location = new System.Drawing.Point(53, 32);
            this.chkB30.Name = "chkB30";
            this.chkB30.Size = new System.Drawing.Size(38, 17);
            this.chkB30.TabIndex = 103;
            this.chkB30.Text = "30";
            this.toolTip1.SetToolTip(this.chkB30, "Select to enable Auto Tune on this band.");
            this.chkB30.UseVisualStyleBackColor = false;
            // 
            // chkB17
            // 
            this.chkB17.AutoSize = true;
            this.chkB17.BackColor = System.Drawing.Color.Transparent;
            this.chkB17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkB17.ForeColor = System.Drawing.Color.Navy;
            this.chkB17.Location = new System.Drawing.Point(53, 48);
            this.chkB17.Name = "chkB17";
            this.chkB17.Size = new System.Drawing.Size(38, 17);
            this.chkB17.TabIndex = 99;
            this.chkB17.Text = "17";
            this.toolTip1.SetToolTip(this.chkB17, "Select to enable Auto Tune on this band.");
            this.chkB17.UseVisualStyleBackColor = false;
            // 
            // chkB40
            // 
            this.chkB40.AutoSize = true;
            this.chkB40.BackColor = System.Drawing.Color.Transparent;
            this.chkB40.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkB40.ForeColor = System.Drawing.Color.Navy;
            this.chkB40.Location = new System.Drawing.Point(8, 32);
            this.chkB40.Name = "chkB40";
            this.chkB40.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkB40.Size = new System.Drawing.Size(38, 17);
            this.chkB40.TabIndex = 101;
            this.chkB40.Text = "40";
            this.toolTip1.SetToolTip(this.chkB40, "Select to enable Auto Tune on this band.");
            this.chkB40.UseVisualStyleBackColor = false;
            // 
            // chkB10
            // 
            this.chkB10.AutoSize = true;
            this.chkB10.BackColor = System.Drawing.Color.Transparent;
            this.chkB10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkB10.ForeColor = System.Drawing.Color.Navy;
            this.chkB10.Location = new System.Drawing.Point(8, 80);
            this.chkB10.Name = "chkB10";
            this.chkB10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkB10.Size = new System.Drawing.Size(38, 17);
            this.chkB10.TabIndex = 104;
            this.chkB10.Text = "10";
            this.toolTip1.SetToolTip(this.chkB10, "Select to enable Auto Tune on this band.");
            this.chkB10.UseVisualStyleBackColor = false;
            // 
            // chkB12
            // 
            this.chkB12.AutoSize = true;
            this.chkB12.BackColor = System.Drawing.Color.Transparent;
            this.chkB12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkB12.ForeColor = System.Drawing.Color.Navy;
            this.chkB12.Location = new System.Drawing.Point(53, 64);
            this.chkB12.Name = "chkB12";
            this.chkB12.Size = new System.Drawing.Size(38, 17);
            this.chkB12.TabIndex = 102;
            this.chkB12.Text = "12";
            this.toolTip1.SetToolTip(this.chkB12, "Select to enable Auto Tune on this band.");
            this.chkB12.UseVisualStyleBackColor = false;
            // 
            // chkB20
            // 
            this.chkB20.AutoSize = true;
            this.chkB20.BackColor = System.Drawing.Color.Transparent;
            this.chkB20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkB20.ForeColor = System.Drawing.Color.Navy;
            this.chkB20.Location = new System.Drawing.Point(8, 48);
            this.chkB20.Name = "chkB20";
            this.chkB20.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkB20.Size = new System.Drawing.Size(38, 17);
            this.chkB20.TabIndex = 97;
            this.chkB20.Text = "20";
            this.toolTip1.SetToolTip(this.chkB20, "Select to enable Auto Tune on this band.");
            this.chkB20.UseVisualStyleBackColor = false;
            // 
            // chkB15
            // 
            this.chkB15.AutoSize = true;
            this.chkB15.BackColor = System.Drawing.Color.Transparent;
            this.chkB15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkB15.ForeColor = System.Drawing.Color.Navy;
            this.chkB15.Location = new System.Drawing.Point(8, 64);
            this.chkB15.Name = "chkB15";
            this.chkB15.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkB15.Size = new System.Drawing.Size(38, 17);
            this.chkB15.TabIndex = 100;
            this.chkB15.Text = "15";
            this.toolTip1.SetToolTip(this.chkB15, "Select to enable Auto Tune on this band.");
            this.chkB15.UseVisualStyleBackColor = false;
            // 
            // grpPTT
            // 
            this.grpPTT.Controls.Add(this.btnByp);
            this.grpPTT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPTT.ForeColor = System.Drawing.Color.Firebrick;
            this.grpPTT.Location = new System.Drawing.Point(233, 2);
            this.grpPTT.Name = "grpPTT";
            this.grpPTT.Size = new System.Drawing.Size(56, 57);
            this.grpPTT.TabIndex = 111;
            this.grpPTT.TabStop = false;
            this.grpPTT.Text = "PTT";
            this.toolTip1.SetToolTip(this.grpPTT, "Push-To-Talk control");
            // 
            // btnByp
            // 
            this.btnByp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnByp.BackColor = System.Drawing.Color.Transparent;
            this.btnByp.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnByp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnByp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnByp.Location = new System.Drawing.Point(4, 23);
            this.btnByp.Name = "btnByp";
            this.btnByp.Size = new System.Drawing.Size(47, 23);
            this.btnByp.TabIndex = 69;
            this.btnByp.Text = "STBY";
            this.toolTip1.SetToolTip(this.btnByp, "Press to toggle PTT on/off  (ctrl+O) works with Amp Bands (setup tab).");
            this.btnByp.UseVisualStyleBackColor = false;
            this.btnByp.Click += new System.EventHandler(this.btnByp_Click);
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.label224);
            this.groupBox13.Controls.Add(this.label223);
            this.groupBox13.Controls.Add(this.label222);
            this.groupBox13.Controls.Add(this.label27);
            this.groupBox13.Controls.Add(this.txtDigi);
            this.groupBox13.Controls.Add(this.txtSSB);
            this.groupBox13.Controls.Add(this.txtAM);
            this.groupBox13.Controls.Add(this.chkAutoExpert);
            this.groupBox13.Controls.Add(this.label151);
            this.groupBox13.Controls.Add(this.chkAutoDrv);
            this.groupBox13.Controls.Add(this.btnDrive);
            this.groupBox13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox13.ForeColor = System.Drawing.Color.Firebrick;
            this.groupBox13.Location = new System.Drawing.Point(294, 1);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(131, 69);
            this.groupBox13.TabIndex = 110;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Auto Drive";
            this.toolTip1.SetToolTip(this.groupBox13, "Auto Drive controls");
            // 
            // label224
            // 
            this.label224.AutoSize = true;
            this.label224.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label224.ForeColor = System.Drawing.Color.Navy;
            this.label224.Location = new System.Drawing.Point(2, 53);
            this.label224.Name = "label224";
            this.label224.Size = new System.Drawing.Size(32, 13);
            this.label224.TabIndex = 71;
            this.label224.Text = "Xpert";
            // 
            // label223
            // 
            this.label223.AutoSize = true;
            this.label223.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label223.ForeColor = System.Drawing.Color.Navy;
            this.label223.Location = new System.Drawing.Point(100, 53);
            this.label223.Name = "label223";
            this.label223.Size = new System.Drawing.Size(25, 13);
            this.label223.TabIndex = 70;
            this.label223.Text = "Digi";
            // 
            // label222
            // 
            this.label222.AutoSize = true;
            this.label222.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label222.ForeColor = System.Drawing.Color.Navy;
            this.label222.Location = new System.Drawing.Point(69, 53);
            this.label222.Name = "label222";
            this.label222.Size = new System.Drawing.Size(28, 13);
            this.label222.TabIndex = 69;
            this.label222.Text = "SSB";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label27.ForeColor = System.Drawing.Color.Navy;
            this.label27.Location = new System.Drawing.Point(39, 53);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(23, 13);
            this.label27.TabIndex = 68;
            this.label27.Text = "AM";
            // 
            // txtDigi
            // 
            this.txtDigi.BackColor = System.Drawing.SystemColors.Window;
            this.txtDigi.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtDigi.Location = new System.Drawing.Point(99, 33);
            this.txtDigi.Name = "txtDigi";
            this.txtDigi.Size = new System.Drawing.Size(26, 18);
            this.txtDigi.TabIndex = 67;
            this.txtDigi.Text = "-25";
            this.txtDigi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtDigi, "Enter percentage increase/decrease for this mode (enter as nn for pos or -nn for " +
                    "neg).");
            this.txtDigi.TextChanged += new System.EventHandler(this.txtDigi_TextChanged);
            // 
            // txtSSB
            // 
            this.txtSSB.BackColor = System.Drawing.SystemColors.Window;
            this.txtSSB.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtSSB.Location = new System.Drawing.Point(68, 33);
            this.txtSSB.Name = "txtSSB";
            this.txtSSB.Size = new System.Drawing.Size(26, 18);
            this.txtSSB.TabIndex = 66;
            this.txtSSB.Text = "-50";
            this.txtSSB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtSSB, "Enter percentage increase/decrease for this mode (enter as nn for pos or -nn for " +
                    "neg).");
            this.txtSSB.TextChanged += new System.EventHandler(this.txtSSB_TextChanged);
            // 
            // txtAM
            // 
            this.txtAM.BackColor = System.Drawing.SystemColors.Window;
            this.txtAM.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtAM.Location = new System.Drawing.Point(37, 33);
            this.txtAM.Name = "txtAM";
            this.txtAM.Size = new System.Drawing.Size(26, 18);
            this.txtAM.TabIndex = 65;
            this.txtAM.Text = "-50";
            this.txtAM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtAM, "Enter percentage increase/decrease for this mode (enter as nn for pos or -nn for " +
                    "neg).");
            this.txtAM.TextChanged += new System.EventHandler(this.txtAM_TextChanged);
            // 
            // chkAutoExpert
            // 
            this.chkAutoExpert.AutoSize = true;
            this.chkAutoExpert.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.chkAutoExpert.ForeColor = System.Drawing.Color.Navy;
            this.chkAutoExpert.Location = new System.Drawing.Point(10, 36);
            this.chkAutoExpert.Name = "chkAutoExpert";
            this.chkAutoExpert.Size = new System.Drawing.Size(15, 14);
            this.chkAutoExpert.TabIndex = 64;
            this.toolTip1.SetToolTip(this.chkAutoExpert, "Select to enable Auto Drive Expert mode.");
            this.chkAutoExpert.UseVisualStyleBackColor = true;
            this.chkAutoExpert.CheckedChanged += new System.EventHandler(this.chkAutoExpert_CheckedChanged);
            // 
            // label151
            // 
            this.label151.AutoSize = true;
            this.label151.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label151.ForeColor = System.Drawing.Color.Navy;
            this.label151.Location = new System.Drawing.Point(100, 16);
            this.label151.Name = "label151";
            this.label151.Size = new System.Drawing.Size(23, 13);
            this.label151.TabIndex = 61;
            this.label151.Text = "Set";
            // 
            // chkAutoDrv
            // 
            this.chkAutoDrv.AutoSize = true;
            this.chkAutoDrv.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.chkAutoDrv.ForeColor = System.Drawing.Color.Navy;
            this.chkAutoDrv.Location = new System.Drawing.Point(10, 15);
            this.chkAutoDrv.Name = "chkAutoDrv";
            this.chkAutoDrv.Size = new System.Drawing.Size(59, 17);
            this.chkAutoDrv.TabIndex = 63;
            this.chkAutoDrv.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkAutoDrv, "Select to enable Auto Drive Level setting.");
            this.chkAutoDrv.UseVisualStyleBackColor = true;
            this.chkAutoDrv.CheckedChanged += new System.EventHandler(this.chkAutoDrv_CheckedChanged);
            // 
            // btnDrive
            // 
            this.btnDrive.BackColor = System.Drawing.Color.Moccasin;
            this.btnDrive.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnDrive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDrive.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDrive.Location = new System.Drawing.Point(71, 15);
            this.btnDrive.Name = "btnDrive";
            this.btnDrive.Size = new System.Drawing.Size(25, 15);
            this.btnDrive.TabIndex = 62;
            this.toolTip1.SetToolTip(this.btnDrive, "Press to save drive level for this band (Ctrl+A).");
            this.btnDrive.UseVisualStyleBackColor = false;
            this.btnDrive.Click += new System.EventHandler(this.btnDrive_Click);
            // 
            // grpAmp
            // 
            this.grpAmp.Controls.Add(this.lblLoad);
            this.grpAmp.Controls.Add(this.txtLoad);
            this.grpAmp.Controls.Add(this.txtTune);
            this.grpAmp.Controls.Add(this.lblTune);
            this.grpAmp.Controls.Add(this.label80);
            this.grpAmp.Controls.Add(this.txtFreq);
            this.grpAmp.Controls.Add(this.label78);
            this.grpAmp.Controls.Add(this.cboAlphaBaud);
            this.grpAmp.Controls.Add(this.btnHF);
            this.grpAmp.Controls.Add(this.btnSF);
            this.grpAmp.Controls.Add(this.lblHF);
            this.grpAmp.Controls.Add(this.txtMsg);
            this.grpAmp.Controls.Add(this.lblSF);
            this.grpAmp.Controls.Add(this.label74);
            this.grpAmp.Controls.Add(this.label69);
            this.grpAmp.Controls.Add(this.txtSeg);
            this.grpAmp.Controls.Add(this.lblAmpInt);
            this.grpAmp.Controls.Add(this.txtAlphaInt);
            this.grpAmp.Controls.Add(this.btnHV);
            this.grpAmp.Controls.Add(this.label77);
            this.grpAmp.Controls.Add(this.label76);
            this.grpAmp.Controls.Add(this.label75);
            this.grpAmp.Controls.Add(this.cboAlpha);
            this.grpAmp.Controls.Add(this.label73);
            this.grpAmp.Controls.Add(this.label70);
            this.grpAmp.Controls.Add(this.label72);
            this.grpAmp.Controls.Add(this.btnOper);
            this.grpAmp.Controls.Add(this.btnTune);
            this.grpAmp.Controls.Add(this.textBox1);
            this.grpAmp.Controls.Add(this.label71);
            this.grpAmp.Controls.Add(this.txtBand);
            this.grpAmp.Controls.Add(this.chkAlpha);
            this.grpAmp.Controls.Add(this.btnPwr);
            this.grpAmp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAmp.ForeColor = System.Drawing.Color.Blue;
            this.grpAmp.Location = new System.Drawing.Point(3, 168);
            this.grpAmp.Name = "grpAmp";
            this.grpAmp.Size = new System.Drawing.Size(320, 136);
            this.grpAmp.TabIndex = 0;
            this.grpAmp.TabStop = false;
            this.grpAmp.Text = "Tube Amps";
            this.toolTip1.SetToolTip(this.grpAmp, "Select Amp from Options menu");
            // 
            // lblLoad
            // 
            this.lblLoad.AutoSize = true;
            this.lblLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.lblLoad.ForeColor = System.Drawing.Color.Navy;
            this.lblLoad.Location = new System.Drawing.Point(284, 91);
            this.lblLoad.Name = "lblLoad";
            this.lblLoad.Size = new System.Drawing.Size(31, 13);
            this.lblLoad.TabIndex = 60;
            this.lblLoad.Text = "Load";
            // 
            // txtLoad
            // 
            this.txtLoad.BackColor = System.Drawing.SystemColors.Info;
            this.txtLoad.Enabled = false;
            this.txtLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoad.Location = new System.Drawing.Point(285, 106);
            this.txtLoad.Name = "txtLoad";
            this.txtLoad.ReadOnly = true;
            this.txtLoad.Size = new System.Drawing.Size(26, 20);
            this.txtLoad.TabIndex = 58;
            this.txtLoad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLoad.DoubleClick += new System.EventHandler(this.txtLoad_DoubleClick);
            // 
            // txtTune
            // 
            this.txtTune.BackColor = System.Drawing.SystemColors.Info;
            this.txtTune.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTune.Location = new System.Drawing.Point(252, 106);
            this.txtTune.Name = "txtTune";
            this.txtTune.Size = new System.Drawing.Size(26, 20);
            this.txtTune.TabIndex = 57;
            this.txtTune.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTune.DoubleClick += new System.EventHandler(this.txtTune_DoubleClick);
            this.txtTune.TextChanged += new System.EventHandler(this.txtTune_TextChanged);
            // 
            // lblTune
            // 
            this.lblTune.AutoSize = true;
            this.lblTune.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.lblTune.ForeColor = System.Drawing.Color.Navy;
            this.lblTune.Location = new System.Drawing.Point(250, 91);
            this.lblTune.Name = "lblTune";
            this.lblTune.Size = new System.Drawing.Size(30, 13);
            this.lblTune.TabIndex = 59;
            this.lblTune.Text = "Tune";
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label80.ForeColor = System.Drawing.Color.Navy;
            this.label80.Location = new System.Drawing.Point(278, 12);
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
            this.toolTip1.SetToolTip(this.txtFreq, "Amp Frequency Setting in MHz.");
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
            "1200",
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
            this.btnHF.BackColor = System.Drawing.Color.Transparent;
            this.btnHF.Enabled = false;
            this.btnHF.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnHF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHF.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnHF.Location = new System.Drawing.Point(236, 67);
            this.btnHF.Name = "btnHF";
            this.btnHF.Size = new System.Drawing.Size(12, 12);
            this.btnHF.TabIndex = 52;
            this.toolTip1.SetToolTip(this.btnHF, "Press to display Hard Fault Log.");
            this.btnHF.UseVisualStyleBackColor = false;
            this.btnHF.Click += new System.EventHandler(this.btnHF_Click);
            // 
            // btnSF
            // 
            this.btnSF.BackColor = System.Drawing.Color.Transparent;
            this.btnSF.Enabled = false;
            this.btnSF.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnSF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSF.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSF.Location = new System.Drawing.Point(219, 67);
            this.btnSF.Name = "btnSF";
            this.btnSF.Size = new System.Drawing.Size(12, 12);
            this.btnSF.TabIndex = 51;
            this.toolTip1.SetToolTip(this.btnSF, "Press to display Soft Fault Log.");
            this.btnSF.UseVisualStyleBackColor = false;
            this.btnSF.Click += new System.EventHandler(this.btnSF_Click);
            // 
            // lblHF
            // 
            this.lblHF.AutoSize = true;
            this.lblHF.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHF.ForeColor = System.Drawing.Color.Navy;
            this.lblHF.Location = new System.Drawing.Point(234, 51);
            this.lblHF.Name = "lblHF";
            this.lblHF.Size = new System.Drawing.Size(18, 12);
            this.lblHF.TabIndex = 50;
            this.lblHF.Text = "HF";
            this.lblHF.DoubleClick += new System.EventHandler(this.lblHF_DoubleClick);
            // 
            // txtMsg
            // 
            this.txtMsg.BackColor = System.Drawing.Color.LightYellow;
            this.txtMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F);
            this.txtMsg.Location = new System.Drawing.Point(6, 85);
            this.txtMsg.MaxLength = 1500;
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ReadOnly = true;
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMsg.Size = new System.Drawing.Size(238, 46);
            this.txtMsg.TabIndex = 48;
            this.toolTip1.SetToolTip(this.txtMsg, "Displays Status and Fault messages.");
            this.txtMsg.DoubleClick += new System.EventHandler(this.txtMsg_DoubleClick);
            // 
            // lblSF
            // 
            this.lblSF.AutoSize = true;
            this.lblSF.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSF.ForeColor = System.Drawing.Color.Navy;
            this.lblSF.Location = new System.Drawing.Point(218, 51);
            this.lblSF.Name = "lblSF";
            this.lblSF.Size = new System.Drawing.Size(17, 12);
            this.lblSF.TabIndex = 47;
            this.lblSF.Text = "SF";
            this.lblSF.DoubleClick += new System.EventHandler(this.lblSF_DoubleClick);
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label74.ForeColor = System.Drawing.Color.Navy;
            this.label74.Location = new System.Drawing.Point(320, 45);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(26, 13);
            this.label74.TabIndex = 46;
            this.label74.Text = "Seg";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label69.ForeColor = System.Drawing.Color.Navy;
            this.label69.Location = new System.Drawing.Point(285, 45);
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
            this.txtSeg.Location = new System.Drawing.Point(285, 63);
            this.txtSeg.Name = "txtSeg";
            this.txtSeg.ReadOnly = true;
            this.txtSeg.Size = new System.Drawing.Size(26, 20);
            this.txtSeg.TabIndex = 45;
            this.txtSeg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtSeg, "Current Band segment used in amp");
            // 
            // lblAmpInt
            // 
            this.lblAmpInt.AutoSize = true;
            this.lblAmpInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.lblAmpInt.ForeColor = System.Drawing.Color.Navy;
            this.lblAmpInt.Location = new System.Drawing.Point(215, 11);
            this.lblAmpInt.Name = "lblAmpInt";
            this.lblAmpInt.Size = new System.Drawing.Size(42, 13);
            this.lblAmpInt.TabIndex = 42;
            this.lblAmpInt.Text = "Interval";
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
            this.txtAlphaInt.TextChanged += new System.EventHandler(this.txtAlphaInt_TextChanged);
            // 
            // btnHV
            // 
            this.btnHV.BackColor = System.Drawing.Color.Transparent;
            this.btnHV.Enabled = false;
            this.btnHV.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnHV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHV.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnHV.Location = new System.Drawing.Point(156, 61);
            this.btnHV.Name = "btnHV";
            this.btnHV.Size = new System.Drawing.Size(41, 22);
            this.btnHV.TabIndex = 39;
            this.btnHV.Text = "Off";
            this.btnHV.UseVisualStyleBackColor = false;
            this.btnHV.Click += new System.EventHandler(this.btnHV_Click);
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label77.ForeColor = System.Drawing.Color.Navy;
            this.label77.Location = new System.Drawing.Point(166, 46);
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
            this.label76.Location = new System.Drawing.Point(9, 46);
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
            this.label73.Location = new System.Drawing.Point(101, 46);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(52, 13);
            this.label73.TabIndex = 11;
            this.label73.Text = "AutoTune";
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label70.ForeColor = System.Drawing.Color.Navy;
            this.label70.Location = new System.Drawing.Point(284, 45);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(32, 13);
            this.label70.TabIndex = 10;
            this.label70.Text = "Band";
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label72.ForeColor = System.Drawing.Color.Navy;
            this.label72.Location = new System.Drawing.Point(249, 45);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(32, 13);
            this.label72.TabIndex = 10;
            this.label72.Text = "Band";
            // 
            // btnOper
            // 
            this.btnOper.BackColor = System.Drawing.Color.Transparent;
            this.btnOper.Enabled = false;
            this.btnOper.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOper.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOper.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOper.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOper.Location = new System.Drawing.Point(56, 61);
            this.btnOper.Name = "btnOper";
            this.btnOper.Size = new System.Drawing.Size(41, 22);
            this.btnOper.TabIndex = 9;
            this.btnOper.Text = "Off";
            this.toolTip1.SetToolTip(this.btnOper, "Press to toggle Operate/StandBy modes.");
            this.btnOper.UseCompatibleTextRendering = true;
            this.btnOper.UseVisualStyleBackColor = false;
            this.btnOper.Click += new System.EventHandler(this.btnOper_Click);
            // 
            // btnTune
            // 
            this.btnTune.BackColor = System.Drawing.Color.Transparent;
            this.btnTune.Enabled = false;
            this.btnTune.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnTune.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTune.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTune.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTune.Location = new System.Drawing.Point(106, 61);
            this.btnTune.Name = "btnTune";
            this.btnTune.Size = new System.Drawing.Size(41, 22);
            this.btnTune.TabIndex = 8;
            this.btnTune.Text = "Off";
            this.btnTune.UseVisualStyleBackColor = false;
            this.btnTune.Click += new System.EventHandler(this.btnTune_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Info;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(287, 62);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(26, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label71.ForeColor = System.Drawing.Color.Navy;
            this.label71.Location = new System.Drawing.Point(54, 46);
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
            this.txtBand.Location = new System.Drawing.Point(252, 62);
            this.txtBand.Name = "txtBand";
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
            this.toolTip1.SetToolTip(this.chkAlpha, "Select to enable amplifier.");
            this.chkAlpha.UseVisualStyleBackColor = true;
            this.chkAlpha.CheckedChanged += new System.EventHandler(this.chkAlpha_CheckedChanged);
            // 
            // btnPwr
            // 
            this.btnPwr.BackColor = System.Drawing.Color.Transparent;
            this.btnPwr.Enabled = false;
            this.btnPwr.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPwr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPwr.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.btnPwr.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPwr.Location = new System.Drawing.Point(7, 61);
            this.btnPwr.Name = "btnPwr";
            this.btnPwr.Size = new System.Drawing.Size(41, 22);
            this.btnPwr.TabIndex = 0;
            this.btnPwr.Text = "Off";
            this.toolTip1.SetToolTip(this.btnPwr, "Press to toggle Amplifier power On/Off amplifier.");
            this.btnPwr.UseVisualStyleBackColor = false;
            this.btnPwr.Click += new System.EventHandler(this.btnPwr_Click);
            // 
            // grpAmpBands
            // 
            this.grpAmpBands.Controls.Add(this.btnClrAll);
            this.grpAmpBands.Controls.Add(this.chkAmp6);
            this.grpAmpBands.Controls.Add(this.chkAmp80);
            this.grpAmpBands.Controls.Add(this.chkAmp160);
            this.grpAmpBands.Controls.Add(this.chkAmp30);
            this.grpAmpBands.Controls.Add(this.btnCkAll);
            this.grpAmpBands.Controls.Add(this.chkAmp17);
            this.grpAmpBands.Controls.Add(this.chkAmp40);
            this.grpAmpBands.Controls.Add(this.chkAmp10);
            this.grpAmpBands.Controls.Add(this.chkAmp12);
            this.grpAmpBands.Controls.Add(this.chkAmp20);
            this.grpAmpBands.Controls.Add(this.chkAmp15);
            this.grpAmpBands.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAmpBands.ForeColor = System.Drawing.Color.Blue;
            this.grpAmpBands.Location = new System.Drawing.Point(248, 81);
            this.grpAmpBands.Name = "grpAmpBands";
            this.grpAmpBands.Size = new System.Drawing.Size(163, 77);
            this.grpAmpBands.TabIndex = 109;
            this.grpAmpBands.TabStop = false;
            this.grpAmpBands.Text = "Amp Bands";
            this.toolTip1.SetToolTip(this.grpAmpBands, "Select bands to enable amplifier usage (rqd for Oper/Stby button).");
            // 
            // btnClrAll
            // 
            this.btnClrAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClrAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnClrAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClrAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClrAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClrAll.Location = new System.Drawing.Point(123, 55);
            this.btnClrAll.Name = "btnClrAll";
            this.btnClrAll.Size = new System.Drawing.Size(30, 15);
            this.btnClrAll.TabIndex = 107;
            this.btnClrAll.Text = "None";
            this.toolTip1.SetToolTip(this.btnClrAll, "Clears all bands.");
            this.btnClrAll.UseVisualStyleBackColor = false;
            this.btnClrAll.Click += new System.EventHandler(this.btnClrAll_Click);
            // 
            // chkAmp6
            // 
            this.chkAmp6.AutoSize = true;
            this.chkAmp6.BackColor = System.Drawing.Color.Transparent;
            this.chkAmp6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAmp6.ForeColor = System.Drawing.Color.Navy;
            this.chkAmp6.Location = new System.Drawing.Point(49, 52);
            this.chkAmp6.Name = "chkAmp6";
            this.chkAmp6.Size = new System.Drawing.Size(32, 17);
            this.chkAmp6.TabIndex = 105;
            this.chkAmp6.Text = "6";
            this.toolTip1.SetToolTip(this.chkAmp6, "Select to enable Amp on this band.");
            this.chkAmp6.UseVisualStyleBackColor = true;
            this.chkAmp6.CheckedChanged += new System.EventHandler(this.chkAmp6_CheckedChanged);
            // 
            // chkAmp80
            // 
            this.chkAmp80.AutoSize = true;
            this.chkAmp80.BackColor = System.Drawing.Color.Transparent;
            this.chkAmp80.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAmp80.ForeColor = System.Drawing.Color.Navy;
            this.chkAmp80.Location = new System.Drawing.Point(49, 16);
            this.chkAmp80.Name = "chkAmp80";
            this.chkAmp80.Size = new System.Drawing.Size(38, 17);
            this.chkAmp80.TabIndex = 98;
            this.chkAmp80.Text = "80";
            this.toolTip1.SetToolTip(this.chkAmp80, "Select to enable Amp on this band.");
            this.chkAmp80.UseVisualStyleBackColor = true;
            this.chkAmp80.CheckedChanged += new System.EventHandler(this.chkAmp80_CheckedChanged);
            // 
            // chkAmp160
            // 
            this.chkAmp160.AutoSize = true;
            this.chkAmp160.BackColor = System.Drawing.Color.Transparent;
            this.chkAmp160.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAmp160.ForeColor = System.Drawing.Color.Navy;
            this.chkAmp160.Location = new System.Drawing.Point(5, 16);
            this.chkAmp160.Name = "chkAmp160";
            this.chkAmp160.Size = new System.Drawing.Size(44, 17);
            this.chkAmp160.TabIndex = 96;
            this.chkAmp160.Text = "160";
            this.toolTip1.SetToolTip(this.chkAmp160, "Select to enable Amp on this band.");
            this.chkAmp160.UseVisualStyleBackColor = true;
            this.chkAmp160.CheckedChanged += new System.EventHandler(this.chkAmp160_CheckedChanged);
            // 
            // chkAmp30
            // 
            this.chkAmp30.AutoSize = true;
            this.chkAmp30.BackColor = System.Drawing.Color.Transparent;
            this.chkAmp30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAmp30.ForeColor = System.Drawing.Color.Navy;
            this.chkAmp30.Location = new System.Drawing.Point(125, 16);
            this.chkAmp30.Name = "chkAmp30";
            this.chkAmp30.Size = new System.Drawing.Size(38, 17);
            this.chkAmp30.TabIndex = 103;
            this.chkAmp30.Text = "30";
            this.toolTip1.SetToolTip(this.chkAmp30, "Select to enable Amp on this band.");
            this.chkAmp30.UseVisualStyleBackColor = true;
            this.chkAmp30.CheckedChanged += new System.EventHandler(this.chkAmp30_CheckedChanged);
            // 
            // btnCkAll
            // 
            this.btnCkAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCkAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnCkAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCkAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCkAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCkAll.Location = new System.Drawing.Point(87, 55);
            this.btnCkAll.Name = "btnCkAll";
            this.btnCkAll.Size = new System.Drawing.Size(30, 15);
            this.btnCkAll.TabIndex = 108;
            this.btnCkAll.Text = "All";
            this.toolTip1.SetToolTip(this.btnCkAll, "Checks all bands.");
            this.btnCkAll.UseVisualStyleBackColor = false;
            this.btnCkAll.Click += new System.EventHandler(this.btnCkAll_Click);
            // 
            // chkAmp17
            // 
            this.chkAmp17.AutoSize = true;
            this.chkAmp17.BackColor = System.Drawing.Color.Transparent;
            this.chkAmp17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAmp17.ForeColor = System.Drawing.Color.Navy;
            this.chkAmp17.Location = new System.Drawing.Point(49, 34);
            this.chkAmp17.Name = "chkAmp17";
            this.chkAmp17.Size = new System.Drawing.Size(38, 17);
            this.chkAmp17.TabIndex = 99;
            this.chkAmp17.Text = "17";
            this.toolTip1.SetToolTip(this.chkAmp17, "Select to enable Amp on this band.");
            this.chkAmp17.UseVisualStyleBackColor = true;
            this.chkAmp17.CheckedChanged += new System.EventHandler(this.chkAmp17_CheckedChanged);
            // 
            // chkAmp40
            // 
            this.chkAmp40.AutoSize = true;
            this.chkAmp40.BackColor = System.Drawing.Color.Transparent;
            this.chkAmp40.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAmp40.ForeColor = System.Drawing.Color.Navy;
            this.chkAmp40.Location = new System.Drawing.Point(87, 16);
            this.chkAmp40.Name = "chkAmp40";
            this.chkAmp40.Size = new System.Drawing.Size(38, 17);
            this.chkAmp40.TabIndex = 101;
            this.chkAmp40.Text = "40";
            this.toolTip1.SetToolTip(this.chkAmp40, "Select to enable Amp on this band.");
            this.chkAmp40.UseVisualStyleBackColor = true;
            this.chkAmp40.CheckedChanged += new System.EventHandler(this.chkAmp40_CheckedChanged);
            // 
            // chkAmp10
            // 
            this.chkAmp10.AutoSize = true;
            this.chkAmp10.BackColor = System.Drawing.Color.Transparent;
            this.chkAmp10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAmp10.ForeColor = System.Drawing.Color.Navy;
            this.chkAmp10.Location = new System.Drawing.Point(5, 52);
            this.chkAmp10.Name = "chkAmp10";
            this.chkAmp10.Size = new System.Drawing.Size(38, 17);
            this.chkAmp10.TabIndex = 104;
            this.chkAmp10.Text = "10";
            this.toolTip1.SetToolTip(this.chkAmp10, "Select to enable Amp on this band.");
            this.chkAmp10.UseVisualStyleBackColor = true;
            this.chkAmp10.CheckedChanged += new System.EventHandler(this.chkAmp10_CheckedChanged);
            // 
            // chkAmp12
            // 
            this.chkAmp12.AutoSize = true;
            this.chkAmp12.BackColor = System.Drawing.Color.Transparent;
            this.chkAmp12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAmp12.ForeColor = System.Drawing.Color.Navy;
            this.chkAmp12.Location = new System.Drawing.Point(125, 34);
            this.chkAmp12.Name = "chkAmp12";
            this.chkAmp12.Size = new System.Drawing.Size(38, 17);
            this.chkAmp12.TabIndex = 102;
            this.chkAmp12.Text = "12";
            this.toolTip1.SetToolTip(this.chkAmp12, "Select to enable Amp on this band.");
            this.chkAmp12.UseVisualStyleBackColor = true;
            this.chkAmp12.CheckedChanged += new System.EventHandler(this.chkAmp12_CheckedChanged);
            // 
            // chkAmp20
            // 
            this.chkAmp20.AutoSize = true;
            this.chkAmp20.BackColor = System.Drawing.Color.Transparent;
            this.chkAmp20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAmp20.ForeColor = System.Drawing.Color.Navy;
            this.chkAmp20.Location = new System.Drawing.Point(5, 34);
            this.chkAmp20.Name = "chkAmp20";
            this.chkAmp20.Size = new System.Drawing.Size(38, 17);
            this.chkAmp20.TabIndex = 97;
            this.chkAmp20.Text = "20";
            this.toolTip1.SetToolTip(this.chkAmp20, "Select to enable Amp on this band.");
            this.chkAmp20.UseVisualStyleBackColor = true;
            this.chkAmp20.CheckedChanged += new System.EventHandler(this.chkAmp20_CheckedChanged);
            // 
            // chkAmp15
            // 
            this.chkAmp15.AutoSize = true;
            this.chkAmp15.BackColor = System.Drawing.Color.Transparent;
            this.chkAmp15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAmp15.ForeColor = System.Drawing.Color.Navy;
            this.chkAmp15.Location = new System.Drawing.Point(87, 34);
            this.chkAmp15.Name = "chkAmp15";
            this.chkAmp15.Size = new System.Drawing.Size(38, 17);
            this.chkAmp15.TabIndex = 100;
            this.chkAmp15.Text = "15";
            this.toolTip1.SetToolTip(this.chkAmp15, "Select to enable Amp on this band.");
            this.chkAmp15.UseVisualStyleBackColor = true;
            this.chkAmp15.CheckedChanged += new System.EventHandler(this.chkAmp15_CheckedChanged);
            // 
            // tabOther
            // 
            this.tabOther.Controls.Add(this.grpMacSw);
            this.tabOther.Controls.Add(this.groupBox17);
            this.tabOther.Controls.Add(this.grpRepeat);
            this.tabOther.Controls.Add(this.groupBox6);
            this.tabOther.Controls.Add(this.grpPro);
            this.tabOther.Controls.Add(this.txtSleep);
            this.tabOther.Controls.Add(this.lblSleep);
            this.tabOther.Controls.Add(this.groupBox4);
            this.tabOther.Location = new System.Drawing.Point(4, 22);
            this.tabOther.Name = "tabOther";
            this.tabOther.Size = new System.Drawing.Size(431, 311);
            this.tabOther.TabIndex = 9;
            this.tabOther.Text = "Other";
            this.tabOther.UseVisualStyleBackColor = true;
            this.tabOther.DoubleClick += new System.EventHandler(this.tabOther_DoubleClick);
            // 
            // grpMacSw
            // 
            this.grpMacSw.Controls.Add(this.txtMacSwMsg);
            this.grpMacSw.Controls.Add(this.txtMacSwOff);
            this.grpMacSw.Controls.Add(this.label226);
            this.grpMacSw.Controls.Add(this.txtMacSwOn);
            this.grpMacSw.Controls.Add(this.label225);
            this.grpMacSw.Controls.Add(this.label155);
            this.grpMacSw.Controls.Add(this.chkMacSwEnab);
            this.grpMacSw.Controls.Add(this.cboMacSwPort);
            this.grpMacSw.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpMacSw.ForeColor = System.Drawing.Color.Blue;
            this.grpMacSw.Location = new System.Drawing.Point(6, 185);
            this.grpMacSw.Name = "grpMacSw";
            this.grpMacSw.Size = new System.Drawing.Size(188, 116);
            this.grpMacSw.TabIndex = 232;
            this.grpMacSw.TabStop = false;
            this.grpMacSw.Text = "Macro Switch";
            // 
            // txtMacSwMsg
            // 
            this.txtMacSwMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.txtMacSwMsg.ForeColor = System.Drawing.Color.Firebrick;
            this.txtMacSwMsg.Location = new System.Drawing.Point(2, 65);
            this.txtMacSwMsg.Multiline = true;
            this.txtMacSwMsg.Name = "txtMacSwMsg";
            this.txtMacSwMsg.Size = new System.Drawing.Size(185, 47);
            this.txtMacSwMsg.TabIndex = 30;
            this.txtMacSwMsg.Text = "Message to display when event fires.\r\nMay be multiple lines in length.\r\nDouble cl" +
                "ick this window for help.";
            this.toolTip1.SetToolTip(this.txtMacSwMsg, "Macro commands or a message to display when the Macro Switch ON event fires (Doub" +
                    "le click this window for help).");
            this.txtMacSwMsg.DoubleClick += new System.EventHandler(this.txtMacSwMsg_DoubleClick);
            this.txtMacSwMsg.TextChanged += new System.EventHandler(this.txtMacSwMsg_TextChanged);
            // 
            // txtMacSwOff
            // 
            this.txtMacSwOff.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMacSwOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtMacSwOff.ForeColor = System.Drawing.Color.Firebrick;
            this.txtMacSwOff.Location = new System.Drawing.Point(115, 41);
            this.txtMacSwOff.Multiline = true;
            this.txtMacSwOff.Name = "txtMacSwOff";
            this.txtMacSwOff.Size = new System.Drawing.Size(56, 20);
            this.txtMacSwOff.TabIndex = 234;
            this.txtMacSwOff.Text = "ZZSW0;";
            this.txtMacSwOff.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtMacSwOff, "CAT command to send for switch Off.");
            this.txtMacSwOff.TextChanged += new System.EventHandler(this.txtMacSwOff_TextChanged);
            // 
            // label226
            // 
            this.label226.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label226.ForeColor = System.Drawing.Color.Navy;
            this.label226.Location = new System.Drawing.Point(89, 44);
            this.label226.Name = "label226";
            this.label226.Size = new System.Drawing.Size(25, 15);
            this.label226.TabIndex = 235;
            this.label226.Text = "Off";
            this.label226.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtMacSwOn
            // 
            this.txtMacSwOn.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMacSwOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtMacSwOn.ForeColor = System.Drawing.Color.Firebrick;
            this.txtMacSwOn.Location = new System.Drawing.Point(28, 41);
            this.txtMacSwOn.Multiline = true;
            this.txtMacSwOn.Name = "txtMacSwOn";
            this.txtMacSwOn.Size = new System.Drawing.Size(56, 20);
            this.txtMacSwOn.TabIndex = 232;
            this.txtMacSwOn.Text = "ZZSW1;";
            this.txtMacSwOn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtMacSwOn, "CAT command to send for switch on.");
            this.txtMacSwOn.TextChanged += new System.EventHandler(this.txtMacSwOn_TextChanged);
            // 
            // label225
            // 
            this.label225.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label225.ForeColor = System.Drawing.Color.Navy;
            this.label225.Location = new System.Drawing.Point(2, 44);
            this.label225.Name = "label225";
            this.label225.Size = new System.Drawing.Size(25, 15);
            this.label225.TabIndex = 233;
            this.label225.Text = "On";
            this.label225.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label155
            // 
            this.label155.AutoSize = true;
            this.label155.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label155.ForeColor = System.Drawing.Color.Navy;
            this.label155.Location = new System.Drawing.Point(87, 20);
            this.label155.Name = "label155";
            this.label155.Size = new System.Drawing.Size(26, 13);
            this.label155.TabIndex = 231;
            this.label155.Text = "Port";
            // 
            // chkMacSwEnab
            // 
            this.chkMacSwEnab.AutoSize = true;
            this.chkMacSwEnab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkMacSwEnab.ForeColor = System.Drawing.Color.Navy;
            this.chkMacSwEnab.Location = new System.Drawing.Point(7, 19);
            this.chkMacSwEnab.Name = "chkMacSwEnab";
            this.chkMacSwEnab.Size = new System.Drawing.Size(59, 17);
            this.chkMacSwEnab.TabIndex = 229;
            this.chkMacSwEnab.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkMacSwEnab, "Select to enable Macro Switch operation");
            this.chkMacSwEnab.UseVisualStyleBackColor = true;
            this.chkMacSwEnab.CheckedChanged += new System.EventHandler(this.chkMacSwEnab_CheckedChanged);
            // 
            // cboMacSwPort
            // 
            this.cboMacSwPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMacSwPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold);
            this.cboMacSwPort.ForeColor = System.Drawing.Color.Firebrick;
            this.cboMacSwPort.FormattingEnabled = true;
            this.cboMacSwPort.Location = new System.Drawing.Point(115, 16);
            this.cboMacSwPort.Name = "cboMacSwPort";
            this.cboMacSwPort.Size = new System.Drawing.Size(68, 21);
            this.cboMacSwPort.TabIndex = 230;
            this.toolTip1.SetToolTip(this.cboMacSwPort, "Select Macro switch serial port.");
            this.cboMacSwPort.SelectedIndexChanged += new System.EventHandler(this.cboMacSwPort_SelectedIndexChanged);
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.txtVspMgr);
            this.groupBox17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox17.ForeColor = System.Drawing.Color.Blue;
            this.groupBox17.Location = new System.Drawing.Point(202, 254);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(188, 47);
            this.groupBox17.TabIndex = 80;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "VSP Manager";
            // 
            // txtVspMgr
            // 
            this.txtVspMgr.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold);
            this.txtVspMgr.ForeColor = System.Drawing.Color.Firebrick;
            this.txtVspMgr.Location = new System.Drawing.Point(10, 17);
            this.txtVspMgr.Name = "txtVspMgr";
            this.txtVspMgr.Size = new System.Drawing.Size(170, 18);
            this.txtVspMgr.TabIndex = 29;
            this.toolTip1.SetToolTip(this.txtVspMgr, "VSP Manager\'s file location (double-click for file dialog).");
            this.txtVspMgr.DoubleClick += new System.EventHandler(this.txtVspMgr_DoubleClick);
            this.txtVspMgr.TextChanged += new System.EventHandler(this.txtVspMgr_TextChanged);
            // 
            // grpRepeat
            // 
            this.grpRepeat.Controls.Add(this.label79);
            this.grpRepeat.Controls.Add(this.label147);
            this.grpRepeat.Controls.Add(this.cboRepeatCom);
            this.grpRepeat.Controls.Add(this.grpRmode);
            this.grpRepeat.Controls.Add(this.cboRepeatPort);
            this.grpRepeat.Controls.Add(this.chkRepeat);
            this.grpRepeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRepeat.ForeColor = System.Drawing.Color.Blue;
            this.grpRepeat.Location = new System.Drawing.Point(210, 3);
            this.grpRepeat.Name = "grpRepeat";
            this.grpRepeat.Size = new System.Drawing.Size(216, 80);
            this.grpRepeat.TabIndex = 78;
            this.grpRepeat.TabStop = false;
            this.grpRepeat.Text = "Repeater";
            // 
            // label79
            // 
            this.label79.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label79.ForeColor = System.Drawing.Color.Navy;
            this.label79.Location = new System.Drawing.Point(153, 6);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(38, 15);
            this.label79.TabIndex = 69;
            this.label79.Text = "Comm";
            // 
            // label147
            // 
            this.label147.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label147.ForeColor = System.Drawing.Color.Navy;
            this.label147.Location = new System.Drawing.Point(91, 6);
            this.label147.Name = "label147";
            this.label147.Size = new System.Drawing.Size(30, 15);
            this.label147.TabIndex = 68;
            this.label147.Text = "Port";
            this.label147.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cboRepeatCom
            // 
            this.cboRepeatCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRepeatCom.ForeColor = System.Drawing.Color.Navy;
            this.cboRepeatCom.FormattingEnabled = true;
            this.cboRepeatCom.Items.AddRange(new object[] {
            "57600",
            "38400",
            "19200",
            "9600",
            "4800"});
            this.cboRepeatCom.Location = new System.Drawing.Point(145, 21);
            this.cboRepeatCom.Name = "cboRepeatCom";
            this.cboRepeatCom.Size = new System.Drawing.Size(63, 21);
            this.cboRepeatCom.TabIndex = 67;
            this.toolTip1.SetToolTip(this.cboRepeatCom, "Select Port Baud Rate");
            this.cboRepeatCom.SelectedIndexChanged += new System.EventHandler(this.cboRepeatCom_SelectedIndexChanged);
            // 
            // grpRmode
            // 
            this.grpRmode.Controls.Add(this.rbRptPal);
            this.grpRmode.Controls.Add(this.rbRptNone);
            this.grpRmode.Controls.Add(this.rbIFonly);
            this.grpRmode.Controls.Add(this.rbAll);
            this.grpRmode.ForeColor = System.Drawing.Color.Blue;
            this.grpRmode.Location = new System.Drawing.Point(9, 42);
            this.grpRmode.Name = "grpRmode";
            this.grpRmode.Size = new System.Drawing.Size(199, 34);
            this.grpRmode.TabIndex = 66;
            this.grpRmode.TabStop = false;
            this.grpRmode.Text = "Data Type";
            // 
            // rbRptPal
            // 
            this.rbRptPal.AutoSize = true;
            this.rbRptPal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbRptPal.ForeColor = System.Drawing.Color.Navy;
            this.rbRptPal.Location = new System.Drawing.Point(99, 12);
            this.rbRptPal.Name = "rbRptPal";
            this.rbRptPal.Size = new System.Drawing.Size(40, 17);
            this.rbRptPal.TabIndex = 70;
            this.rbRptPal.Text = "Pal";
            this.toolTip1.SetToolTip(this.rbRptPal, "Port is configured for Palstar command mode.");
            this.rbRptPal.UseVisualStyleBackColor = true;
            this.rbRptPal.CheckedChanged += new System.EventHandler(this.grpRmode_CheckedChanged);
            // 
            // rbRptNone
            // 
            this.rbRptNone.AutoSize = true;
            this.rbRptNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbRptNone.ForeColor = System.Drawing.Color.Navy;
            this.rbRptNone.Location = new System.Drawing.Point(143, 12);
            this.rbRptNone.Name = "rbRptNone";
            this.rbRptNone.Size = new System.Drawing.Size(51, 17);
            this.rbRptNone.TabIndex = 69;
            this.rbRptNone.Text = "None";
            this.toolTip1.SetToolTip(this.rbRptNone, "No Cat cmds are broadcast to this port.");
            this.rbRptNone.UseVisualStyleBackColor = true;
            this.rbRptNone.CheckedChanged += new System.EventHandler(this.grpRmode_CheckedChanged);
            // 
            // rbIFonly
            // 
            this.rbIFonly.AutoSize = true;
            this.rbIFonly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbIFonly.ForeColor = System.Drawing.Color.Navy;
            this.rbIFonly.Location = new System.Drawing.Point(42, 12);
            this.rbIFonly.Name = "rbIFonly";
            this.rbIFonly.Size = new System.Drawing.Size(56, 17);
            this.rbIFonly.TabIndex = 68;
            this.rbIFonly.Text = "IF only";
            this.toolTip1.SetToolTip(this.rbIFonly, "Only \'IF\' CAT cmds are passed.");
            this.rbIFonly.UseVisualStyleBackColor = true;
            this.rbIFonly.CheckedChanged += new System.EventHandler(this.grpRmode_CheckedChanged);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbAll.ForeColor = System.Drawing.Color.Navy;
            this.rbAll.Location = new System.Drawing.Point(4, 12);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(36, 17);
            this.rbAll.TabIndex = 67;
            this.rbAll.Text = "All";
            this.toolTip1.SetToolTip(this.rbAll, "All commands are reported.");
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.CheckedChanged += new System.EventHandler(this.grpRmode_CheckedChanged);
            // 
            // cboRepeatPort
            // 
            this.cboRepeatPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboRepeatPort.ForeColor = System.Drawing.Color.Navy;
            this.cboRepeatPort.FormattingEnabled = true;
            this.cboRepeatPort.Location = new System.Drawing.Point(77, 21);
            this.cboRepeatPort.Name = "cboRepeatPort";
            this.cboRepeatPort.Size = new System.Drawing.Size(63, 21);
            this.cboRepeatPort.TabIndex = 49;
            this.toolTip1.SetToolTip(this.cboRepeatPort, "Repeater CAT Port");
            this.cboRepeatPort.SelectedIndexChanged += new System.EventHandler(this.cboRepeatPort_SelectedIndexChanged);
            // 
            // chkRepeat
            // 
            this.chkRepeat.AutoSize = true;
            this.chkRepeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRepeat.ForeColor = System.Drawing.Color.Navy;
            this.chkRepeat.Location = new System.Drawing.Point(13, 17);
            this.chkRepeat.Name = "chkRepeat";
            this.chkRepeat.Size = new System.Drawing.Size(59, 17);
            this.chkRepeat.TabIndex = 50;
            this.chkRepeat.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkRepeat, "Check to enable the Repeater port.");
            this.chkRepeat.UseVisualStyleBackColor = true;
            this.chkRepeat.CheckedChanged += new System.EventHandler(this.chkRepeat_CheckedChanged);
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
            this.groupBox6.Location = new System.Drawing.Point(6, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(165, 80);
            this.groupBox6.TabIndex = 48;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "SteppIR";
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(90, 61);
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
            "19200 8N1",
            "9600 8N1",
            "9600 8N2",
            "4800 8N1",
            "4800 8N2",
            "2400 8N1",
            "2400 8N2",
            "1200 8N1",
            "1200 8N2"});
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
            this.label42.Location = new System.Drawing.Point(20, 62);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(29, 15);
            this.label42.TabIndex = 18;
            this.label42.Text = "Port";
            // 
            // grpPro
            // 
            this.grpPro.Controls.Add(this.button6);
            this.grpPro.Controls.Add(this.label153);
            this.grpPro.Controls.Add(this.txtPmsg);
            this.grpPro.Controls.Add(this.button1);
            this.grpPro.Controls.Add(this.label152);
            this.grpPro.Controls.Add(this.comboBox1);
            this.grpPro.Controls.Add(this.checkBox2);
            this.grpPro.Controls.Add(this.textBox2);
            this.grpPro.Controls.Add(this.button2);
            this.grpPro.Controls.Add(this.button5);
            this.grpPro.Controls.Add(this.label149);
            this.grpPro.Controls.Add(this.label150);
            this.grpPro.Controls.Add(this.button3);
            this.grpPro.Controls.Add(this.button4);
            this.grpPro.Controls.Add(this.grpInP);
            this.grpPro.Controls.Add(this.grpAntP);
            this.grpPro.Enabled = false;
            this.grpPro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPro.ForeColor = System.Drawing.Color.Blue;
            this.grpPro.Location = new System.Drawing.Point(202, 87);
            this.grpPro.Name = "grpPro";
            this.grpPro.Size = new System.Drawing.Size(225, 166);
            this.grpPro.TabIndex = 109;
            this.grpPro.TabStop = false;
            this.grpPro.Text = "Prometheus";
            this.toolTip1.SetToolTip(this.grpPro, "Prometheus (DX-2400L1) Amplifier controls");
            // 
            // button6
            // 
            this.button6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button6.BackColor = System.Drawing.Color.Transparent;
            this.button6.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.button6.ForeColor = System.Drawing.Color.Navy;
            this.button6.Location = new System.Drawing.Point(122, 83);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(35, 20);
            this.button6.TabIndex = 80;
            this.button6.Text = "?";
            this.toolTip1.SetToolTip(this.button6, "Turns Attenuator On/Off");
            this.button6.UseVisualStyleBackColor = false;
            // 
            // label153
            // 
            this.label153.AutoSize = true;
            this.label153.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label153.ForeColor = System.Drawing.Color.Navy;
            this.label153.Location = new System.Drawing.Point(9, 40);
            this.label153.Name = "label153";
            this.label153.Size = new System.Drawing.Size(55, 13);
            this.label153.TabIndex = 62;
            this.label153.Text = "Serial Port";
            // 
            // txtPmsg
            // 
            this.txtPmsg.BackColor = System.Drawing.Color.LightYellow;
            this.txtPmsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F);
            this.txtPmsg.Location = new System.Drawing.Point(4, 107);
            this.txtPmsg.MaxLength = 1500;
            this.txtPmsg.Multiline = true;
            this.txtPmsg.Name = "txtPmsg";
            this.txtPmsg.ReadOnly = true;
            this.txtPmsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPmsg.Size = new System.Drawing.Size(217, 46);
            this.txtPmsg.TabIndex = 61;
            this.txtPmsg.Text = "Scrollable Message Display:\r\nDisplays up to 50 lines of text\r\nHard & Soft faults " +
                "& other Errors";
            this.toolTip1.SetToolTip(this.txtPmsg, "Displays Status and Fault messages.");
            // 
            // button1
            // 
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.button1.ForeColor = System.Drawing.Color.Navy;
            this.button1.Location = new System.Drawing.Point(83, 83);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 20);
            this.button1.TabIndex = 79;
            this.button1.Text = "FAN";
            this.toolTip1.SetToolTip(this.button1, "Turns Attenuator On/Off");
            this.button1.UseVisualStyleBackColor = false;
            // 
            // label152
            // 
            this.label152.AutoSize = true;
            this.label152.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label152.ForeColor = System.Drawing.Color.Navy;
            this.label152.Location = new System.Drawing.Point(82, 7);
            this.label152.Name = "label152";
            this.label152.Size = new System.Drawing.Size(43, 13);
            this.label152.TabIndex = 62;
            this.label152.Text = "PA Tmp";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.ForeColor = System.Drawing.Color.Black;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(4, 55);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(68, 21);
            this.comboBox1.TabIndex = 61;
            this.toolTip1.SetToolTip(this.comboBox1, "Hardware serial port connected to amp.");
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.checkBox2.ForeColor = System.Drawing.Color.Navy;
            this.checkBox2.Location = new System.Drawing.Point(8, 19);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(59, 17);
            this.checkBox2.TabIndex = 61;
            this.checkBox2.Text = "Enable";
            this.toolTip1.SetToolTip(this.checkBox2, "Select to enable amplifier.");
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Window;
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.textBox2.Location = new System.Drawing.Point(80, 21);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(47, 18);
            this.textBox2.TabIndex = 61;
            this.textBox2.Text = "37 C";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.Location = new System.Drawing.Point(105, 61);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(12, 12);
            this.button2.TabIndex = 78;
            this.toolTip1.SetToolTip(this.button2, "Press to display Hard Fault Log.");
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Transparent;
            this.button5.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button5.Location = new System.Drawing.Point(88, 61);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(12, 12);
            this.button5.TabIndex = 77;
            this.toolTip1.SetToolTip(this.button5, "Press to display Soft Fault Log.");
            this.button5.UseVisualStyleBackColor = false;
            // 
            // label149
            // 
            this.label149.AutoSize = true;
            this.label149.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label149.ForeColor = System.Drawing.Color.Navy;
            this.label149.Location = new System.Drawing.Point(103, 46);
            this.label149.Name = "label149";
            this.label149.Size = new System.Drawing.Size(18, 12);
            this.label149.TabIndex = 76;
            this.label149.Text = "HF";
            // 
            // label150
            // 
            this.label150.AutoSize = true;
            this.label150.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label150.ForeColor = System.Drawing.Color.Navy;
            this.label150.Location = new System.Drawing.Point(87, 46);
            this.label150.Name = "label150";
            this.label150.Size = new System.Drawing.Size(17, 12);
            this.label150.TabIndex = 75;
            this.label150.Text = "SF";
            // 
            // button3
            // 
            this.button3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.button3.ForeColor = System.Drawing.Color.Navy;
            this.button3.Location = new System.Drawing.Point(44, 83);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(35, 20);
            this.button3.TabIndex = 73;
            this.button3.Text = "ATT";
            this.toolTip1.SetToolTip(this.button3, "Turns Attenuator On/Off");
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button4.BackColor = System.Drawing.Color.Transparent;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.button4.ForeColor = System.Drawing.Color.Navy;
            this.button4.Location = new System.Drawing.Point(5, 83);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(35, 20);
            this.button4.TabIndex = 72;
            this.button4.Text = "STBY";
            this.toolTip1.SetToolTip(this.button4, "Press to toggle Operate/StandBy modes.");
            this.button4.UseVisualStyleBackColor = false;
            // 
            // grpInP
            // 
            this.grpInP.Controls.Add(this.radioButton7);
            this.grpInP.Controls.Add(this.radioButton8);
            this.grpInP.ForeColor = System.Drawing.Color.Navy;
            this.grpInP.Location = new System.Drawing.Point(133, 9);
            this.grpInP.Name = "grpInP";
            this.grpInP.Size = new System.Drawing.Size(39, 48);
            this.grpInP.TabIndex = 71;
            this.grpInP.TabStop = false;
            this.grpInP.Text = "In";
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.radioButton7.ForeColor = System.Drawing.Color.Navy;
            this.radioButton7.Location = new System.Drawing.Point(5, 28);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(31, 17);
            this.radioButton7.TabIndex = 68;
            this.radioButton7.Text = "2";
            this.toolTip1.SetToolTip(this.radioButton7, "Selects coupler type");
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Checked = true;
            this.radioButton8.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.radioButton8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.radioButton8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.radioButton8.ForeColor = System.Drawing.Color.Navy;
            this.radioButton8.Location = new System.Drawing.Point(5, 12);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(31, 17);
            this.radioButton8.TabIndex = 67;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "1";
            this.toolTip1.SetToolTip(this.radioButton8, "Selects coupler type");
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // grpAntP
            // 
            this.grpAntP.Controls.Add(this.radioButton4);
            this.grpAntP.Controls.Add(this.radioButton1);
            this.grpAntP.Controls.Add(this.radioButton2);
            this.grpAntP.Controls.Add(this.radioButton3);
            this.grpAntP.ForeColor = System.Drawing.Color.Navy;
            this.grpAntP.Location = new System.Drawing.Point(176, 9);
            this.grpAntP.Name = "grpAntP";
            this.grpAntP.Size = new System.Drawing.Size(44, 77);
            this.grpAntP.TabIndex = 66;
            this.grpAntP.TabStop = false;
            this.grpAntP.Text = "Ant";
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.radioButton4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.radioButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.radioButton4.ForeColor = System.Drawing.Color.Navy;
            this.radioButton4.Location = new System.Drawing.Point(10, 58);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(31, 17);
            this.radioButton4.TabIndex = 70;
            this.radioButton4.Text = "4";
            this.toolTip1.SetToolTip(this.radioButton4, "Selects coupler type");
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.radioButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.radioButton1.ForeColor = System.Drawing.Color.Navy;
            this.radioButton1.Location = new System.Drawing.Point(10, 43);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(31, 17);
            this.radioButton1.TabIndex = 69;
            this.radioButton1.Text = "3";
            this.toolTip1.SetToolTip(this.radioButton1, "Selects coupler type");
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.radioButton2.ForeColor = System.Drawing.Color.Navy;
            this.radioButton2.Location = new System.Drawing.Point(10, 28);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(31, 17);
            this.radioButton2.TabIndex = 68;
            this.radioButton2.Text = "2";
            this.toolTip1.SetToolTip(this.radioButton2, "Selects coupler type");
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.radioButton3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.radioButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.radioButton3.ForeColor = System.Drawing.Color.Navy;
            this.radioButton3.Location = new System.Drawing.Point(10, 12);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(31, 17);
            this.radioButton3.TabIndex = 67;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "1";
            this.toolTip1.SetToolTip(this.radioButton3, "Selects coupler type");
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // txtSleep
            // 
            this.txtSleep.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F);
            this.txtSleep.Location = new System.Drawing.Point(175, 24);
            this.txtSleep.Name = "txtSleep";
            this.txtSleep.Size = new System.Drawing.Size(28, 19);
            this.txtSleep.TabIndex = 51;
            this.txtSleep.Text = "0";
            this.txtSleep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtSleep, "Danger Will Robinson! If you don\'t know what this is for don\'t change it.");
            this.txtSleep.Visible = false;
            this.txtSleep.TextChanged += new System.EventHandler(this.txtSleep_TextChanged);
            // 
            // lblSleep
            // 
            this.lblSleep.Location = new System.Drawing.Point(172, 44);
            this.lblSleep.Name = "lblSleep";
            this.lblSleep.Size = new System.Drawing.Size(41, 19);
            this.lblSleep.TabIndex = 51;
            this.lblSleep.Text = "Sleep";
            this.lblSleep.Visible = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkPwRTS);
            this.groupBox4.Controls.Add(this.chkPwDTR);
            this.groupBox4.Controls.Add(this.btnTest);
            this.groupBox4.Controls.Add(this.chkDisBcast);
            this.groupBox4.Controls.Add(this.label24);
            this.groupBox4.Controls.Add(this.txtPW1ra);
            this.groupBox4.Controls.Add(this.label23);
            this.groupBox4.Controls.Add(this.cboPW1);
            this.groupBox4.Controls.Add(this.txtPW1ta);
            this.groupBox4.Controls.Add(this.chkPW1);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox4.ForeColor = System.Drawing.Color.Blue;
            this.groupBox4.Location = new System.Drawing.Point(6, 84);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(188, 100);
            this.groupBox4.TabIndex = 48;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "IC-PW1";
            // 
            // chkPwRTS
            // 
            this.chkPwRTS.AutoSize = true;
            this.chkPwRTS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkPwRTS.ForeColor = System.Drawing.Color.Navy;
            this.chkPwRTS.Location = new System.Drawing.Point(124, 77);
            this.chkPwRTS.Name = "chkPwRTS";
            this.chkPwRTS.Size = new System.Drawing.Size(48, 17);
            this.chkPwRTS.TabIndex = 52;
            this.chkPwRTS.Text = "RTS";
            this.toolTip1.SetToolTip(this.chkPwRTS, "Select if your CI-V adapter requires power from the RTS line.");
            this.chkPwRTS.UseVisualStyleBackColor = true;
            this.chkPwRTS.CheckedChanged += new System.EventHandler(this.chkPwRTS_CheckedChanged);
            // 
            // chkPwDTR
            // 
            this.chkPwDTR.AutoSize = true;
            this.chkPwDTR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkPwDTR.ForeColor = System.Drawing.Color.Navy;
            this.chkPwDTR.Location = new System.Drawing.Point(124, 60);
            this.chkPwDTR.Name = "chkPwDTR";
            this.chkPwDTR.Size = new System.Drawing.Size(49, 17);
            this.chkPwDTR.TabIndex = 51;
            this.chkPwDTR.Text = "DTR";
            this.toolTip1.SetToolTip(this.chkPwDTR, "Select if your CI-V adapter requires power from the DTR line.");
            this.chkPwDTR.UseVisualStyleBackColor = true;
            this.chkPwDTR.CheckedChanged += new System.EventHandler(this.chkPwDTR_CheckedChanged);
            // 
            // btnTest
            // 
            this.btnTest.AutoSize = true;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.Navy;
            this.btnTest.Location = new System.Drawing.Point(69, 50);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(38, 23);
            this.btnTest.TabIndex = 49;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // chkDisBcast
            // 
            this.chkDisBcast.AutoSize = true;
            this.chkDisBcast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDisBcast.ForeColor = System.Drawing.Color.Navy;
            this.chkDisBcast.Location = new System.Drawing.Point(7, 75);
            this.chkDisBcast.Name = "chkDisBcast";
            this.chkDisBcast.Size = new System.Drawing.Size(112, 17);
            this.chkDisBcast.TabIndex = 50;
            this.chkDisBcast.Text = "Disable Broadcast";
            this.toolTip1.SetToolTip(this.chkDisBcast, "Check to disable frequency broadcast to PW1");
            this.chkDisBcast.UseVisualStyleBackColor = true;
            this.chkDisBcast.CheckedChanged += new System.EventHandler(this.chkDisBcast_CheckedChanged);
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.Navy;
            this.label24.Location = new System.Drawing.Point(107, 45);
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
            this.txtPW1ra.Location = new System.Drawing.Point(100, 24);
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
            this.label23.ForeColor = System.Drawing.Color.Navy;
            this.label23.Location = new System.Drawing.Point(148, 45);
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
            this.cboPW1.Location = new System.Drawing.Point(7, 25);
            this.cboPW1.Name = "cboPW1";
            this.cboPW1.Size = new System.Drawing.Size(63, 21);
            this.cboPW1.TabIndex = 17;
            this.toolTip1.SetToolTip(this.cboPW1, "Select Port for IC-PW1");
            this.cboPW1.SelectedIndexChanged += new System.EventHandler(this.cboPW1_SelectedIndexChanged);
            // 
            // txtPW1ta
            // 
            this.txtPW1ta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPW1ta.Location = new System.Drawing.Point(140, 24);
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
            this.chkPW1.ForeColor = System.Drawing.Color.Navy;
            this.chkPW1.Location = new System.Drawing.Point(7, 55);
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
            this.label21.ForeColor = System.Drawing.Color.Navy;
            this.label21.Location = new System.Drawing.Point(95, 8);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(84, 15);
            this.label21.TabIndex = 16;
            this.label21.Text = "--- CI-V Addr ---";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Navy;
            this.label22.Location = new System.Drawing.Point(72, 28);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(30, 15);
            this.label22.TabIndex = 18;
            this.label22.Text = "Port";
            // 
            // tabMacro
            // 
            this.tabMacro.Controls.Add(this.label146);
            this.tabMacro.Controls.Add(this.label29);
            this.tabMacro.Controls.Add(this.label143);
            this.tabMacro.Controls.Add(this.label144);
            this.tabMacro.Controls.Add(this.label145);
            this.tabMacro.Controls.Add(this.btnMacro16);
            this.tabMacro.Controls.Add(this.btnMacro15);
            this.tabMacro.Controls.Add(this.btnMacro13);
            this.tabMacro.Controls.Add(this.btnMacro14);
            this.tabMacro.Controls.Add(this.label34);
            this.tabMacro.Controls.Add(this.grpMacro);
            this.tabMacro.Controls.Add(this.dgm);
            this.tabMacro.Controls.Add(this.label33);
            this.tabMacro.Controls.Add(this.label35);
            this.tabMacro.Controls.Add(this.label32);
            this.tabMacro.Controls.Add(this.label31);
            this.tabMacro.Controls.Add(this.label36);
            this.tabMacro.Controls.Add(this.label30);
            this.tabMacro.Controls.Add(this.label37);
            this.tabMacro.Controls.Add(this.label28);
            this.tabMacro.Controls.Add(this.btnMacro6);
            this.tabMacro.Controls.Add(this.label38);
            this.tabMacro.Controls.Add(this.btnMacro5);
            this.tabMacro.Controls.Add(this.btnMacro4);
            this.tabMacro.Controls.Add(this.label39);
            this.tabMacro.Controls.Add(this.btnMacro3);
            this.tabMacro.Controls.Add(this.btnMacro2);
            this.tabMacro.Controls.Add(this.btnMacro12);
            this.tabMacro.Controls.Add(this.btnMacro1);
            this.tabMacro.Controls.Add(this.btnMacro7);
            this.tabMacro.Controls.Add(this.btnMacro11);
            this.tabMacro.Controls.Add(this.btnMacro8);
            this.tabMacro.Controls.Add(this.btnMacro9);
            this.tabMacro.Controls.Add(this.btnMacro10);
            this.tabMacro.Location = new System.Drawing.Point(4, 22);
            this.tabMacro.Name = "tabMacro";
            this.tabMacro.Size = new System.Drawing.Size(431, 311);
            this.tabMacro.TabIndex = 4;
            this.tabMacro.Text = "Macro";
            this.tabMacro.UseVisualStyleBackColor = true;
            this.tabMacro.Click += new System.EventHandler(this.tabMacro_Click);
            // 
            // label146
            // 
            this.label146.AutoSize = true;
            this.label146.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label146.ForeColor = System.Drawing.Color.Firebrick;
            this.label146.Location = new System.Drawing.Point(73, 224);
            this.label146.Name = "label146";
            this.label146.Size = new System.Drawing.Size(18, 12);
            this.label146.TabIndex = 107;
            this.label146.Text = "F2";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.Color.Firebrick;
            this.label29.Location = new System.Drawing.Point(387, 267);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(24, 12);
            this.label29.TabIndex = 106;
            this.label29.Text = "^F4";
            // 
            // label143
            // 
            this.label143.AutoSize = true;
            this.label143.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label143.ForeColor = System.Drawing.Color.Firebrick;
            this.label143.Location = new System.Drawing.Point(335, 267);
            this.label143.Name = "label143";
            this.label143.Size = new System.Drawing.Size(24, 12);
            this.label143.TabIndex = 105;
            this.label143.Text = "^F3";
            // 
            // label144
            // 
            this.label144.AutoSize = true;
            this.label144.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label144.ForeColor = System.Drawing.Color.Firebrick;
            this.label144.Location = new System.Drawing.Point(281, 267);
            this.label144.Name = "label144";
            this.label144.Size = new System.Drawing.Size(24, 12);
            this.label144.TabIndex = 104;
            this.label144.Text = "^F2";
            // 
            // label145
            // 
            this.label145.AutoSize = true;
            this.label145.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label145.ForeColor = System.Drawing.Color.Firebrick;
            this.label145.Location = new System.Drawing.Point(227, 267);
            this.label145.Name = "label145";
            this.label145.Size = new System.Drawing.Size(24, 12);
            this.label145.TabIndex = 103;
            this.label145.Text = "^F1";
            // 
            // btnMacro16
            // 
            this.btnMacro16.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro16.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro16.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro16.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro16.Location = new System.Drawing.Point(376, 281);
            this.btnMacro16.Name = "btnMacro16";
            this.btnMacro16.Size = new System.Drawing.Size(49, 25);
            this.btnMacro16.TabIndex = 102;
            this.btnMacro16.Text = "M16";
            this.toolTip1.SetToolTip(this.btnMacro16, "Executes the Macro Commands assigned to this button");
            this.btnMacro16.UseVisualStyleBackColor = false;
            this.btnMacro16.Click += new System.EventHandler(this.btnMacro16_Click);
            // 
            // btnMacro15
            // 
            this.btnMacro15.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro15.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro15.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro15.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro15.Location = new System.Drawing.Point(323, 281);
            this.btnMacro15.Name = "btnMacro15";
            this.btnMacro15.Size = new System.Drawing.Size(49, 25);
            this.btnMacro15.TabIndex = 101;
            this.btnMacro15.Text = "M15";
            this.toolTip1.SetToolTip(this.btnMacro15, "Executes the Macro Commands assigned to this button");
            this.btnMacro15.UseVisualStyleBackColor = false;
            this.btnMacro15.Click += new System.EventHandler(this.btnMacro15_Click);
            // 
            // btnMacro13
            // 
            this.btnMacro13.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro13.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro13.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro13.Location = new System.Drawing.Point(215, 281);
            this.btnMacro13.Name = "btnMacro13";
            this.btnMacro13.Size = new System.Drawing.Size(49, 25);
            this.btnMacro13.TabIndex = 99;
            this.btnMacro13.Text = "M13";
            this.toolTip1.SetToolTip(this.btnMacro13, "Executes the Macro Commands assigned to this button");
            this.btnMacro13.UseVisualStyleBackColor = false;
            this.btnMacro13.Click += new System.EventHandler(this.btnMacro13_Click);
            // 
            // btnMacro14
            // 
            this.btnMacro14.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro14.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro14.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro14.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro14.Location = new System.Drawing.Point(270, 281);
            this.btnMacro14.Name = "btnMacro14";
            this.btnMacro14.Size = new System.Drawing.Size(49, 25);
            this.btnMacro14.TabIndex = 100;
            this.btnMacro14.Text = "M14";
            this.toolTip1.SetToolTip(this.btnMacro14, "Executes the Macro Commands assigned to this button");
            this.btnMacro14.UseVisualStyleBackColor = false;
            this.btnMacro14.Click += new System.EventHandler(this.btnMacro14_Click);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.Color.Firebrick;
            this.label34.Location = new System.Drawing.Point(176, 267);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(24, 12);
            this.label34.TabIndex = 98;
            this.label34.Text = "F12";
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
            this.dgm.Size = new System.Drawing.Size(421, 151);
            this.dgm.TabIndex = 78;
            this.dgm.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgm_CellValueChanged);
            this.dgm.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgm_MouseDoubleClick);
            this.dgm.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgm_RowPostPaint);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.Firebrick;
            this.label33.Location = new System.Drawing.Point(286, 224);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(18, 12);
            this.label33.TabIndex = 86;
            this.label33.Text = "F6";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.Firebrick;
            this.label35.Location = new System.Drawing.Point(124, 267);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(24, 12);
            this.label35.TabIndex = 97;
            this.label35.Text = "F11";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Firebrick;
            this.label32.Location = new System.Drawing.Point(232, 224);
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
            this.label31.Location = new System.Drawing.Point(180, 224);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(18, 12);
            this.label31.TabIndex = 84;
            this.label31.Text = "F4";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.ForeColor = System.Drawing.Color.Firebrick;
            this.label36.Location = new System.Drawing.Point(71, 267);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(24, 12);
            this.label36.TabIndex = 96;
            this.label36.Text = "F10";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.Color.Firebrick;
            this.label30.Location = new System.Drawing.Point(126, 224);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(18, 12);
            this.label30.TabIndex = 83;
            this.label30.Text = "F3";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.ForeColor = System.Drawing.Color.Firebrick;
            this.label37.Location = new System.Drawing.Point(18, 267);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(18, 12);
            this.label37.TabIndex = 95;
            this.label37.Text = "F9";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.Firebrick;
            this.label28.Location = new System.Drawing.Point(19, 224);
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
            this.btnMacro6.Location = new System.Drawing.Point(270, 238);
            this.btnMacro6.Name = "btnMacro6";
            this.btnMacro6.Size = new System.Drawing.Size(49, 25);
            this.btnMacro6.TabIndex = 77;
            this.btnMacro6.Text = "M6";
            this.toolTip1.SetToolTip(this.btnMacro6, "Executes the Macro Commands assigned to this button");
            this.btnMacro6.UseVisualStyleBackColor = false;
            this.btnMacro6.Click += new System.EventHandler(this.btnMacro6_Click);
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.ForeColor = System.Drawing.Color.Firebrick;
            this.label38.Location = new System.Drawing.Point(391, 224);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(18, 12);
            this.label38.TabIndex = 94;
            this.label38.Text = "F8";
            // 
            // btnMacro5
            // 
            this.btnMacro5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro5.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro5.Location = new System.Drawing.Point(217, 238);
            this.btnMacro5.Name = "btnMacro5";
            this.btnMacro5.Size = new System.Drawing.Size(49, 25);
            this.btnMacro5.TabIndex = 76;
            this.btnMacro5.Text = "M5";
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
            this.btnMacro4.Location = new System.Drawing.Point(164, 238);
            this.btnMacro4.Name = "btnMacro4";
            this.btnMacro4.Size = new System.Drawing.Size(49, 25);
            this.btnMacro4.TabIndex = 75;
            this.btnMacro4.Text = "M4";
            this.toolTip1.SetToolTip(this.btnMacro4, "Executes the Macro Commands assigned to this button");
            this.btnMacro4.UseVisualStyleBackColor = false;
            this.btnMacro4.Click += new System.EventHandler(this.btnMacro4_Click);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.ForeColor = System.Drawing.Color.Firebrick;
            this.label39.Location = new System.Drawing.Point(338, 224);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(18, 12);
            this.label39.TabIndex = 93;
            this.label39.Text = "F7";
            // 
            // btnMacro3
            // 
            this.btnMacro3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro3.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro3.Location = new System.Drawing.Point(111, 238);
            this.btnMacro3.Name = "btnMacro3";
            this.btnMacro3.Size = new System.Drawing.Size(49, 25);
            this.btnMacro3.TabIndex = 74;
            this.btnMacro3.Text = "M3";
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
            this.btnMacro2.Location = new System.Drawing.Point(58, 238);
            this.btnMacro2.Name = "btnMacro2";
            this.btnMacro2.Size = new System.Drawing.Size(49, 25);
            this.btnMacro2.TabIndex = 73;
            this.btnMacro2.Text = "M2";
            this.toolTip1.SetToolTip(this.btnMacro2, "Executes the Macro Commands assigned to this button");
            this.btnMacro2.UseVisualStyleBackColor = false;
            this.btnMacro2.Click += new System.EventHandler(this.btnMacro2_Click);
            // 
            // btnMacro12
            // 
            this.btnMacro12.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro12.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro12.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro12.Location = new System.Drawing.Point(164, 281);
            this.btnMacro12.Name = "btnMacro12";
            this.btnMacro12.Size = new System.Drawing.Size(49, 25);
            this.btnMacro12.TabIndex = 92;
            this.btnMacro12.Text = "M12";
            this.toolTip1.SetToolTip(this.btnMacro12, "Executes the Macro Commands assigned to this button");
            this.btnMacro12.UseVisualStyleBackColor = false;
            this.btnMacro12.Click += new System.EventHandler(this.btnMacro12_Click);
            // 
            // btnMacro1
            // 
            this.btnMacro1.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro1.Location = new System.Drawing.Point(4, 238);
            this.btnMacro1.Name = "btnMacro1";
            this.btnMacro1.Size = new System.Drawing.Size(49, 25);
            this.btnMacro1.TabIndex = 72;
            this.btnMacro1.Text = "M1";
            this.toolTip1.SetToolTip(this.btnMacro1, "Executes the Macro Commands assigned to this button");
            this.btnMacro1.UseVisualStyleBackColor = false;
            this.btnMacro1.Click += new System.EventHandler(this.btnMacro1_Click);
            // 
            // btnMacro7
            // 
            this.btnMacro7.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro7.Location = new System.Drawing.Point(323, 238);
            this.btnMacro7.Name = "btnMacro7";
            this.btnMacro7.Size = new System.Drawing.Size(49, 25);
            this.btnMacro7.TabIndex = 87;
            this.btnMacro7.Text = "M7";
            this.toolTip1.SetToolTip(this.btnMacro7, "Executes the Macro Commands assigned to this button");
            this.btnMacro7.UseVisualStyleBackColor = false;
            this.btnMacro7.Click += new System.EventHandler(this.btnMacro7_Click);
            // 
            // btnMacro11
            // 
            this.btnMacro11.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro11.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro11.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro11.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro11.Location = new System.Drawing.Point(111, 281);
            this.btnMacro11.Name = "btnMacro11";
            this.btnMacro11.Size = new System.Drawing.Size(49, 25);
            this.btnMacro11.TabIndex = 91;
            this.btnMacro11.Text = "M11";
            this.toolTip1.SetToolTip(this.btnMacro11, "Executes the Macro Commands assigned to this button");
            this.btnMacro11.UseVisualStyleBackColor = false;
            this.btnMacro11.Click += new System.EventHandler(this.btnMacro11_Click);
            // 
            // btnMacro8
            // 
            this.btnMacro8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro8.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro8.Location = new System.Drawing.Point(376, 238);
            this.btnMacro8.Name = "btnMacro8";
            this.btnMacro8.Size = new System.Drawing.Size(49, 25);
            this.btnMacro8.TabIndex = 88;
            this.btnMacro8.Text = "M8";
            this.toolTip1.SetToolTip(this.btnMacro8, "Executes the Macro Commands assigned to this button");
            this.btnMacro8.UseVisualStyleBackColor = false;
            this.btnMacro8.Click += new System.EventHandler(this.btnMacro8_Click);
            // 
            // btnMacro9
            // 
            this.btnMacro9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro9.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro9.Location = new System.Drawing.Point(3, 281);
            this.btnMacro9.Name = "btnMacro9";
            this.btnMacro9.Size = new System.Drawing.Size(49, 25);
            this.btnMacro9.TabIndex = 89;
            this.btnMacro9.Text = "M9";
            this.toolTip1.SetToolTip(this.btnMacro9, "Executes the Macro Commands assigned to this button");
            this.btnMacro9.UseVisualStyleBackColor = false;
            this.btnMacro9.Click += new System.EventHandler(this.btnMacro9_Click);
            // 
            // btnMacro10
            // 
            this.btnMacro10.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMacro10.BackColor = System.Drawing.SystemColors.Control;
            this.btnMacro10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMacro10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMacro10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnMacro10.Location = new System.Drawing.Point(58, 281);
            this.btnMacro10.Name = "btnMacro10";
            this.btnMacro10.Size = new System.Drawing.Size(49, 25);
            this.btnMacro10.TabIndex = 90;
            this.btnMacro10.Text = "M10";
            this.toolTip1.SetToolTip(this.btnMacro10, "Executes the Macro Commands assigned to this button");
            this.btnMacro10.UseVisualStyleBackColor = false;
            this.btnMacro10.Click += new System.EventHandler(this.btnMacro10_Click);
            // 
            // tabRotor
            // 
            this.tabRotor.Controls.Add(this.groupBox12);
            this.tabRotor.Controls.Add(this.grpPreset);
            this.tabRotor.Controls.Add(this.grpStepCtrl);
            this.tabRotor.Controls.Add(this.groupBox11);
            this.tabRotor.Controls.Add(this.groupBox9);
            this.tabRotor.Controls.Add(this.groupBox8);
            this.tabRotor.Location = new System.Drawing.Point(4, 22);
            this.tabRotor.Name = "tabRotor";
            this.tabRotor.Size = new System.Drawing.Size(431, 311);
            this.tabRotor.TabIndex = 5;
            this.tabRotor.Text = "Rotor";
            this.tabRotor.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.txtKm);
            this.groupBox12.Controls.Add(this.txtSm);
            this.groupBox12.Controls.Add(this.txtLoc);
            this.groupBox12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox12.ForeColor = System.Drawing.Color.Navy;
            this.groupBox12.Location = new System.Drawing.Point(345, 165);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(73, 95);
            this.groupBox12.TabIndex = 15;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Grid";
            this.toolTip1.SetToolTip(this.groupBox12, "Calculate heading and distance to a grid square.");
            // 
            // txtKm
            // 
            this.txtKm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKm.ForeColor = System.Drawing.Color.Firebrick;
            this.txtKm.Location = new System.Drawing.Point(5, 69);
            this.txtKm.Name = "txtKm";
            this.txtKm.Size = new System.Drawing.Size(61, 20);
            this.txtKm.TabIndex = 44;
            this.txtKm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtKm, "Kilometers between grids.");
            // 
            // txtSm
            // 
            this.txtSm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSm.ForeColor = System.Drawing.Color.Firebrick;
            this.txtSm.Location = new System.Drawing.Point(5, 44);
            this.txtSm.Name = "txtSm";
            this.txtSm.Size = new System.Drawing.Size(61, 20);
            this.txtSm.TabIndex = 43;
            this.txtSm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtSm, "Statute miles between grids.");
            // 
            // txtLoc
            // 
            this.txtLoc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoc.ForeColor = System.Drawing.Color.Blue;
            this.txtLoc.Location = new System.Drawing.Point(5, 19);
            this.txtLoc.MaxLength = 6;
            this.txtLoc.Name = "txtLoc";
            this.txtLoc.Size = new System.Drawing.Size(61, 20);
            this.txtLoc.TabIndex = 42;
            this.txtLoc.Text = "NF86SQ";
            this.txtLoc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLoc, "Grid Sq. of entity Or enter a grid sq. and hit [Enter] to get position & heading " +
                    "of a grid from your QTH.");
            this.txtLoc.DoubleClick += new System.EventHandler(this.txtLoc_DoubleClick);
            this.txtLoc.Click += new System.EventHandler(this.grpC4_CheckedChanged);
            this.txtLoc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLoc_KeyDown);
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
            this.grpPreset.ForeColor = System.Drawing.Color.Blue;
            this.grpPreset.Location = new System.Drawing.Point(7, 110);
            this.grpPreset.Name = "grpPreset";
            this.grpPreset.Size = new System.Drawing.Size(399, 50);
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
            this.rbPre16.Text = "330";
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
            this.rbPre15.Text = "300";
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
            this.rbPre14.Text = "270";
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
            this.rbPre13.Text = "240";
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
            this.rbPre5.Text = "60";
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
            this.rbPre12.Text = "225";
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
            this.rbPre4.Text = "45";
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
            this.rbPre11.Text = "210";
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
            this.rbPre3.Text = "30";
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
            this.rbPre10.Text = "195";
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
            this.rbPre2.Text = "15";
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
            this.rbPre1.Text = "0";
            this.toolTip1.SetToolTip(this.rbPre1, "Preset antenna heading in degrees.");
            this.rbPre1.UseVisualStyleBackColor = true;
            this.rbPre1.CheckedChanged += new System.EventHandler(this.grpPreset_CheckChanged);
            // 
            // grpStepCtrl
            // 
            this.grpStepCtrl.Controls.Add(this.lblStepFreq);
            this.grpStepCtrl.Controls.Add(this.rb34);
            this.grpStepCtrl.Controls.Add(this.rbBiDir);
            this.grpStepCtrl.Controls.Add(this.rb180);
            this.grpStepCtrl.Controls.Add(this.rbFwd);
            this.grpStepCtrl.Controls.Add(this.btnCalib);
            this.grpStepCtrl.Controls.Add(this.btnHome);
            this.grpStepCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpStepCtrl.ForeColor = System.Drawing.Color.Blue;
            this.grpStepCtrl.Location = new System.Drawing.Point(5, 265);
            this.grpStepCtrl.Name = "grpStepCtrl";
            this.grpStepCtrl.Size = new System.Drawing.Size(405, 40);
            this.grpStepCtrl.TabIndex = 13;
            this.grpStepCtrl.TabStop = false;
            this.grpStepCtrl.Text = "SteppIR Control";
            // 
            // lblStepFreq
            // 
            this.lblStepFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepFreq.ForeColor = System.Drawing.Color.Red;
            this.lblStepFreq.Location = new System.Drawing.Point(202, 19);
            this.lblStepFreq.Name = "lblStepFreq";
            this.lblStepFreq.Size = new System.Drawing.Size(68, 15);
            this.lblStepFreq.TabIndex = 103;
            this.lblStepFreq.Text = ". . . . .";
            this.lblStepFreq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.lblStepFreq, "Frequency reported from SteppIR controller.");
            // 
            // rb34
            // 
            this.rb34.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rb34.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rb34.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rb34.ForeColor = System.Drawing.Color.Navy;
            this.rb34.Location = new System.Drawing.Point(159, 17);
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
            this.rbBiDir.Location = new System.Drawing.Point(110, 17);
            this.rbBiDir.Name = "rbBiDir";
            this.rbBiDir.Size = new System.Drawing.Size(54, 17);
            this.rbBiDir.TabIndex = 101;
            this.rbBiDir.Text = "<-->";
            this.toolTip1.SetToolTip(this.rbBiDir, "Select to put yagi in bi-directional mode (ctrl+B).");
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
            this.rb180.Location = new System.Drawing.Point(62, 17);
            this.rb180.Name = "rb180";
            this.rb180.Size = new System.Drawing.Size(51, 17);
            this.rb180.TabIndex = 100;
            this.rb180.Text = "180";
            this.toolTip1.SetToolTip(this.rb180, "Select to put yagi in 180 (reverse) mode (ctrl+R).");
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
            this.toolTip1.SetToolTip(this.rbFwd, "Select to put yagi in forward mode (ctrl+F).");
            this.rbFwd.UseVisualStyleBackColor = true;
            this.rbFwd.CheckedChanged += new System.EventHandler(this.rbFwd_CheckedChanged);
            // 
            // btnCalib
            // 
            this.btnCalib.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCalib.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnCalib.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCalib.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnCalib.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnCalib.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalib.ForeColor = System.Drawing.Color.Black;
            this.btnCalib.Location = new System.Drawing.Point(338, 13);
            this.btnCalib.Name = "btnCalib";
            this.btnCalib.Size = new System.Drawing.Size(60, 21);
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
            this.btnHome.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.Black;
            this.btnHome.Location = new System.Drawing.Point(275, 13);
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
            this.groupBox11.ForeColor = System.Drawing.Color.Blue;
            this.groupBox11.Location = new System.Drawing.Point(5, 165);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(336, 95);
            this.groupBox11.TabIndex = 12;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Location Data";
            // 
            // lblDxTime
            // 
            this.lblDxTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDxTime.ForeColor = System.Drawing.Color.Blue;
            this.lblDxTime.Location = new System.Drawing.Point(82, 61);
            this.lblDxTime.Name = "lblDxTime";
            this.lblDxTime.Size = new System.Drawing.Size(178, 13);
            this.lblDxTime.TabIndex = 41;
            this.lblDxTime.Text = "Entity not selected";
            this.toolTip1.SetToolTip(this.lblDxTime, "Entity local time");
            // 
            // txtDxIOTA
            // 
            this.txtDxIOTA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDxIOTA.ForeColor = System.Drawing.Color.Firebrick;
            this.txtDxIOTA.Location = new System.Drawing.Point(274, 56);
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
            this.label62.Location = new System.Drawing.Point(284, 78);
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
            this.label61.Location = new System.Drawing.Point(14, 78);
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
            this.label58.Location = new System.Drawing.Point(303, 41);
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
            this.label57.Location = new System.Drawing.Point(269, 41);
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
            this.label55.Location = new System.Drawing.Point(23, 41);
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
            this.label56.Location = new System.Drawing.Point(98, 41);
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
            this.label53.Location = new System.Drawing.Point(230, 41);
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
            this.label54.Location = new System.Drawing.Point(173, 41);
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
            this.groupBox9.Controls.Add(this.label40);
            this.groupBox9.Controls.Add(this.label60);
            this.groupBox9.Controls.Add(this.txtCallSign);
            this.groupBox9.Controls.Add(this.txtRegion);
            this.groupBox9.Controls.Add(this.cboEntity);
            this.groupBox9.Controls.Add(this.label47);
            this.groupBox9.Controls.Add(this.cboPrefix);
            this.groupBox9.Controls.Add(this.label45);
            this.groupBox9.Controls.Add(this.label48);
            this.groupBox9.Controls.Add(this.txtCode);
            this.groupBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox9.ForeColor = System.Drawing.Color.Blue;
            this.groupBox9.Location = new System.Drawing.Point(5, 5);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(319, 100);
            this.groupBox9.TabIndex = 11;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "DX Data";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label40.ForeColor = System.Drawing.Color.Navy;
            this.label40.Location = new System.Drawing.Point(6, 39);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(53, 13);
            this.label40.TabIndex = 43;
            this.label40.Text = "Call Prefix";
            this.label40.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label60.ForeColor = System.Drawing.Color.Navy;
            this.label60.Location = new System.Drawing.Point(3, 81);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(41, 13);
            this.label60.TabIndex = 14;
            this.label60.Text = "Region";
            // 
            // txtCallSign
            // 
            this.txtCallSign.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCallSign.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCallSign.ForeColor = System.Drawing.Color.Firebrick;
            this.txtCallSign.Location = new System.Drawing.Point(4, 17);
            this.txtCallSign.Name = "txtCallSign";
            this.txtCallSign.Size = new System.Drawing.Size(55, 20);
            this.txtCallSign.TabIndex = 42;
            this.txtCallSign.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtCallSign, "Enter a Call Sign Prefix then hit [Enter] (double-click to clear).");
            this.txtCallSign.DoubleClick += new System.EventHandler(this.txtCallSign_DoubleClick);
            this.txtCallSign.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCallSign_KeyDown);
            // 
            // txtRegion
            // 
            this.txtRegion.ForeColor = System.Drawing.Color.Firebrick;
            this.txtRegion.Location = new System.Drawing.Point(4, 60);
            this.txtRegion.Name = "txtRegion";
            this.txtRegion.Size = new System.Drawing.Size(271, 20);
            this.txtRegion.TabIndex = 13;
            this.toolTip1.SetToolTip(this.txtRegion, "Country region");
            // 
            // cboEntity
            // 
            this.cboEntity.ForeColor = System.Drawing.Color.Firebrick;
            this.cboEntity.FormattingEnabled = true;
            this.cboEntity.Location = new System.Drawing.Point(139, 17);
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
            this.cboEntity.DropDown += new System.EventHandler(this.cboEntity_DropDown);
            this.cboEntity.Click += new System.EventHandler(this.cboEntity_Click);
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label47.ForeColor = System.Drawing.Color.Navy;
            this.label47.Location = new System.Drawing.Point(281, 82);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(32, 13);
            this.label47.TabIndex = 6;
            this.label47.Text = "Code";
            this.label47.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cboPrefix
            // 
            this.cboPrefix.ForeColor = System.Drawing.Color.Firebrick;
            this.cboPrefix.FormattingEnabled = true;
            this.cboPrefix.Location = new System.Drawing.Point(64, 17);
            this.cboPrefix.MaxDropDownItems = 15;
            this.cboPrefix.Name = "cboPrefix";
            this.cboPrefix.Size = new System.Drawing.Size(70, 21);
            this.cboPrefix.TabIndex = 11;
            this.toolTip1.SetToolTip(this.cboPrefix, "DXCC Prefix");
            this.cboPrefix.SelectedIndexChanged += new System.EventHandler(this.cboPrefix_SelectedIndexChanged);
            this.cboPrefix.Leave += new System.EventHandler(this.cboPrefix_Leave);
            this.cboPrefix.Enter += new System.EventHandler(this.cboPrefix_Enter);
            this.cboPrefix.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboPrefix_KeyPress);
            this.cboPrefix.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboPrefix_KeyDown);
            this.cboPrefix.DropDown += new System.EventHandler(this.cboPrefix_DropDown);
            this.cboPrefix.Click += new System.EventHandler(this.cboPrefix_Click);
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label45.ForeColor = System.Drawing.Color.Navy;
            this.label45.Location = new System.Drawing.Point(67, 40);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(33, 13);
            this.label45.TabIndex = 10;
            this.label45.Text = "Prefix";
            this.label45.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label48.ForeColor = System.Drawing.Color.Navy;
            this.label48.Location = new System.Drawing.Point(143, 40);
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
            this.txtCode.Location = new System.Drawing.Point(281, 60);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(32, 20);
            this.txtCode.TabIndex = 4;
            this.txtCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtCode, "DXCC country code. To lookup enter number then hit Enter.");
            this.txtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.btnStop);
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
            this.groupBox8.Location = new System.Drawing.Point(329, 5);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(97, 100);
            this.groupBox8.TabIndex = 5;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Heading";
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Red;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Location = new System.Drawing.Point(43, 60);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(10, 10);
            this.btnStop.TabIndex = 73;
            this.toolTip1.SetToolTip(this.btnStop, "Press to stop rotation.");
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblLP
            // 
            this.lblLP.ForeColor = System.Drawing.Color.Green;
            this.lblLP.Location = new System.Drawing.Point(52, 80);
            this.lblLP.Name = "lblLP";
            this.lblLP.Size = new System.Drawing.Size(40, 13);
            this.lblLP.TabIndex = 39;
            this.lblLP.Tag = "";
            this.lblLP.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label59.ForeColor = System.Drawing.Color.Navy;
            this.label59.Location = new System.Drawing.Point(56, 16);
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
            this.lblSP.Location = new System.Drawing.Point(4, 80);
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
            this.btnSP.Size = new System.Drawing.Size(30, 19);
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
            this.txtLP.Location = new System.Drawing.Point(52, 30);
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
            this.btnLP.Location = new System.Drawing.Point(58, 55);
            this.btnLP.Name = "btnLP";
            this.btnLP.Size = new System.Drawing.Size(30, 19);
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
            // tabExtCtrl
            // 
            this.tabExtCtrl.Controls.Add(this.chkFWb);
            this.tabExtCtrl.Controls.Add(this.chkFWa);
            this.tabExtCtrl.Controls.Add(this.chkInvertB);
            this.tabExtCtrl.Controls.Add(this.chkInvertA);
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
            this.tabExtCtrl.Size = new System.Drawing.Size(431, 311);
            this.tabExtCtrl.TabIndex = 8;
            this.tabExtCtrl.Text = "ExtCtrl";
            this.tabExtCtrl.UseVisualStyleBackColor = true;
            // 
            // chkFWb
            // 
            this.chkFWb.AutoSize = true;
            this.chkFWb.ForeColor = System.Drawing.Color.Green;
            this.chkFWb.Location = new System.Drawing.Point(193, 221);
            this.chkFWb.Name = "chkFWb";
            this.chkFWb.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkFWb.Size = new System.Drawing.Size(43, 17);
            this.chkFWb.TabIndex = 285;
            this.chkFWb.Text = "FW";
            this.toolTip1.SetToolTip(this.chkFWb, "Select to enable Port B output to FlexWire Adapter.");
            this.chkFWb.UseVisualStyleBackColor = true;
            this.chkFWb.CheckedChanged += new System.EventHandler(this.chkFWb_CheckedChanged);
            // 
            // chkFWa
            // 
            this.chkFWa.AutoSize = true;
            this.chkFWa.ForeColor = System.Drawing.Color.Green;
            this.chkFWa.Location = new System.Drawing.Point(186, 45);
            this.chkFWa.Name = "chkFWa";
            this.chkFWa.Size = new System.Drawing.Size(43, 17);
            this.chkFWa.TabIndex = 284;
            this.chkFWa.Text = "FW";
            this.toolTip1.SetToolTip(this.chkFWa, "Select to enable Port A output to FlexWire Adapter.");
            this.chkFWa.UseVisualStyleBackColor = true;
            this.chkFWa.CheckedChanged += new System.EventHandler(this.chkFWa_CheckedChanged);
            // 
            // chkInvertB
            // 
            this.chkInvertB.AutoSize = true;
            this.chkInvertB.ForeColor = System.Drawing.Color.Maroon;
            this.chkInvertB.Location = new System.Drawing.Point(183, 205);
            this.chkInvertB.Name = "chkInvertB";
            this.chkInvertB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkInvertB.Size = new System.Drawing.Size(53, 17);
            this.chkInvertB.TabIndex = 283;
            this.chkInvertB.Text = "Invert";
            this.toolTip1.SetToolTip(this.chkInvertB, "Select if data is going to a device with inverted logic (K3TUF UCB)");
            this.chkInvertB.UseVisualStyleBackColor = true;
            this.chkInvertB.CheckedChanged += new System.EventHandler(this.chkInvertB_CheckedChanged);
            // 
            // chkInvertA
            // 
            this.chkInvertA.AutoSize = true;
            this.chkInvertA.ForeColor = System.Drawing.Color.Maroon;
            this.chkInvertA.Location = new System.Drawing.Point(186, 30);
            this.chkInvertA.Name = "chkInvertA";
            this.chkInvertA.Size = new System.Drawing.Size(53, 17);
            this.chkInvertA.TabIndex = 282;
            this.chkInvertA.Text = "Invert";
            this.toolTip1.SetToolTip(this.chkInvertA, "Select if data is going to a device with inverted logic (K3TUF UCB)");
            this.chkInvertA.UseVisualStyleBackColor = true;
            this.chkInvertA.CheckedChanged += new System.EventHandler(this.chkInvertA_CheckedChanged);
            // 
            // label137
            // 
            this.label137.AutoSize = true;
            this.label137.ForeColor = System.Drawing.Color.Navy;
            this.label137.Location = new System.Drawing.Point(194, 137);
            this.label137.Name = "label137";
            this.label137.Size = new System.Drawing.Size(32, 13);
            this.label137.TabIndex = 281;
            this.label137.Text = "Band";
            // 
            // lblVHF
            // 
            this.lblVHF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVHF.ForeColor = System.Drawing.Color.Firebrick;
            this.lblVHF.Location = new System.Drawing.Point(196, 152);
            this.lblVHF.Name = "lblVHF";
            this.lblVHF.Size = new System.Drawing.Size(30, 15);
            this.lblVHF.TabIndex = 280;
            this.lblVHF.Text = "- - -";
            this.lblVHF.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.lblVHF, "Band Button selected");
            // 
            // label132
            // 
            this.label132.AutoSize = true;
            this.label132.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label132.ForeColor = System.Drawing.Color.Blue;
            this.label132.Location = new System.Drawing.Point(202, 5);
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
            this.label131.Location = new System.Drawing.Point(186, 179);
            this.label131.Name = "label131";
            this.label131.Size = new System.Drawing.Size(34, 13);
            this.label131.TabIndex = 278;
            this.label131.Text = "----->";
            // 
            // chkPortB
            // 
            this.chkPortB.AutoSize = true;
            this.chkPortB.ForeColor = System.Drawing.Color.Navy;
            this.chkPortB.Location = new System.Drawing.Point(185, 189);
            this.chkPortB.Name = "chkPortB";
            this.chkPortB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkPortB.Size = new System.Drawing.Size(51, 17);
            this.chkPortB.TabIndex = 277;
            this.chkPortB.Text = "Enab";
            this.toolTip1.SetToolTip(this.chkPortB, "Select to enable Port B output.");
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
            this.btnClrPortB.Location = new System.Drawing.Point(193, 283);
            this.btnClrPortB.Name = "btnClrPortB";
            this.btnClrPortB.Size = new System.Drawing.Size(35, 15);
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
            this.btnClrPortA.Location = new System.Drawing.Point(194, 106);
            this.btnClrPortA.Name = "btnClrPortA";
            this.btnClrPortA.Size = new System.Drawing.Size(35, 15);
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
            this.grpPortB.Location = new System.Drawing.Point(247, 6);
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
            this.cb13t4.Location = new System.Drawing.Point(89, 238);
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
            this.cb13t7.Location = new System.Drawing.Point(40, 238);
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
            this.cb13t6.Location = new System.Drawing.Point(57, 238);
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
            this.cb13t5.Location = new System.Drawing.Point(73, 238);
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
            this.cb13t3.Location = new System.Drawing.Point(105, 238);
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
            this.cb13t2.Location = new System.Drawing.Point(121, 238);
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
            this.cb13t1.Location = new System.Drawing.Point(137, 238);
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
            this.cb13t0.Location = new System.Drawing.Point(153, 238);
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
            this.label108.Location = new System.Drawing.Point(7, 223);
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
            this.label109.Location = new System.Drawing.Point(7, 238);
            this.label109.Name = "label109";
            this.label109.Size = new System.Drawing.Size(19, 13);
            this.label109.TabIndex = 399;
            this.label109.Text = "13";
            // 
            // cb12t0
            // 
            this.cb12t0.AutoSize = true;
            this.cb12t0.Enabled = false;
            this.cb12t0.Location = new System.Drawing.Point(153, 223);
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
            this.cb12t1.Location = new System.Drawing.Point(137, 223);
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
            this.cb12t2.Location = new System.Drawing.Point(121, 223);
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
            this.cb12t3.Location = new System.Drawing.Point(105, 223);
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
            this.cb12t4.Location = new System.Drawing.Point(89, 223);
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
            this.cb12t5.Location = new System.Drawing.Point(73, 223);
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
            this.cb12t6.Location = new System.Drawing.Point(57, 223);
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
            this.cb12t7.Location = new System.Drawing.Point(40, 223);
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
            this.label110.Location = new System.Drawing.Point(7, 208);
            this.label110.Name = "label110";
            this.label110.Size = new System.Drawing.Size(19, 13);
            this.label110.TabIndex = 390;
            this.label110.Text = "11";
            // 
            // cb11t0
            // 
            this.cb11t0.AutoSize = true;
            this.cb11t0.Enabled = false;
            this.cb11t0.Location = new System.Drawing.Point(153, 208);
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
            this.cb11t1.Location = new System.Drawing.Point(137, 208);
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
            this.cb11t2.Location = new System.Drawing.Point(121, 208);
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
            this.cb11t3.Location = new System.Drawing.Point(105, 208);
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
            this.cb11t4.Location = new System.Drawing.Point(89, 208);
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
            this.cb11t5.Location = new System.Drawing.Point(73, 208);
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
            this.cb11t6.Location = new System.Drawing.Point(57, 208);
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
            this.cb11t7.Location = new System.Drawing.Point(40, 208);
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
            this.label111.Location = new System.Drawing.Point(7, 193);
            this.label111.Name = "label111";
            this.label111.Size = new System.Drawing.Size(19, 13);
            this.label111.TabIndex = 381;
            this.label111.Text = "10";
            // 
            // cb10t0
            // 
            this.cb10t0.AutoSize = true;
            this.cb10t0.Enabled = false;
            this.cb10t0.Location = new System.Drawing.Point(153, 193);
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
            this.cb10t1.Location = new System.Drawing.Point(137, 193);
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
            this.cb10t2.Location = new System.Drawing.Point(121, 193);
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
            this.cb10t3.Location = new System.Drawing.Point(105, 193);
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
            this.cb10t4.Location = new System.Drawing.Point(89, 193);
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
            this.cb10t5.Location = new System.Drawing.Point(73, 193);
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
            this.cb10t6.Location = new System.Drawing.Point(57, 193);
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
            this.cb10t7.Location = new System.Drawing.Point(40, 193);
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
            this.label112.Location = new System.Drawing.Point(13, 178);
            this.label112.Name = "label112";
            this.label112.Size = new System.Drawing.Size(13, 13);
            this.label112.TabIndex = 372;
            this.label112.Text = "9";
            // 
            // cb9t0
            // 
            this.cb9t0.AutoSize = true;
            this.cb9t0.Enabled = false;
            this.cb9t0.Location = new System.Drawing.Point(153, 178);
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
            this.cb9t1.Location = new System.Drawing.Point(137, 178);
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
            this.cb9t2.Location = new System.Drawing.Point(121, 178);
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
            this.cb9t3.Location = new System.Drawing.Point(105, 178);
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
            this.cb9t4.Location = new System.Drawing.Point(89, 178);
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
            this.cb9t5.Location = new System.Drawing.Point(73, 178);
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
            this.cb9t6.Location = new System.Drawing.Point(57, 178);
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
            this.cb9t7.Location = new System.Drawing.Point(40, 178);
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
            this.label113.Location = new System.Drawing.Point(13, 163);
            this.label113.Name = "label113";
            this.label113.Size = new System.Drawing.Size(13, 13);
            this.label113.TabIndex = 363;
            this.label113.Text = "8";
            // 
            // cb8t0
            // 
            this.cb8t0.AutoSize = true;
            this.cb8t0.Enabled = false;
            this.cb8t0.Location = new System.Drawing.Point(153, 163);
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
            this.cb8t1.Location = new System.Drawing.Point(137, 163);
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
            this.cb8t2.Location = new System.Drawing.Point(121, 163);
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
            this.cb8t3.Location = new System.Drawing.Point(105, 163);
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
            this.cb8t4.Location = new System.Drawing.Point(89, 163);
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
            this.cb8t5.Location = new System.Drawing.Point(73, 163);
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
            this.cb8t6.Location = new System.Drawing.Point(57, 163);
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
            this.cb8t7.Location = new System.Drawing.Point(40, 163);
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
            this.label114.Location = new System.Drawing.Point(13, 148);
            this.label114.Name = "label114";
            this.label114.Size = new System.Drawing.Size(13, 13);
            this.label114.TabIndex = 354;
            this.label114.Text = "7";
            // 
            // cb7t0
            // 
            this.cb7t0.AutoSize = true;
            this.cb7t0.Enabled = false;
            this.cb7t0.Location = new System.Drawing.Point(153, 148);
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
            this.cb7t1.Location = new System.Drawing.Point(137, 148);
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
            this.cb7t2.Location = new System.Drawing.Point(121, 148);
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
            this.cb7t3.Location = new System.Drawing.Point(105, 148);
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
            this.cb7t4.Location = new System.Drawing.Point(89, 148);
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
            this.cb7t5.Location = new System.Drawing.Point(73, 148);
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
            this.cb7t6.Location = new System.Drawing.Point(57, 148);
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
            this.cb7t7.Location = new System.Drawing.Point(40, 148);
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
            this.label115.Location = new System.Drawing.Point(13, 133);
            this.label115.Name = "label115";
            this.label115.Size = new System.Drawing.Size(13, 13);
            this.label115.TabIndex = 345;
            this.label115.Text = "6";
            // 
            // cb6t0
            // 
            this.cb6t0.AutoSize = true;
            this.cb6t0.Enabled = false;
            this.cb6t0.Location = new System.Drawing.Point(153, 133);
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
            this.cb6t1.Location = new System.Drawing.Point(137, 133);
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
            this.cb6t2.Location = new System.Drawing.Point(121, 133);
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
            this.cb6t3.Location = new System.Drawing.Point(105, 133);
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
            this.cb6t4.Location = new System.Drawing.Point(89, 133);
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
            this.cb6t5.Location = new System.Drawing.Point(73, 133);
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
            this.cb6t6.Location = new System.Drawing.Point(57, 133);
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
            this.cb6t7.Location = new System.Drawing.Point(40, 133);
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
            this.grpPortA.Location = new System.Drawing.Point(2, 6);
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
            this.cb13r4.Location = new System.Drawing.Point(90, 238);
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
            this.cb13r7.Location = new System.Drawing.Point(41, 238);
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
            this.cb13r6.Location = new System.Drawing.Point(58, 238);
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
            this.cb13r5.Location = new System.Drawing.Point(74, 238);
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
            this.cb13r3.Location = new System.Drawing.Point(106, 238);
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
            this.cb13r2.Location = new System.Drawing.Point(122, 238);
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
            this.cb13r1.Location = new System.Drawing.Point(138, 238);
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
            this.cb13r0.Location = new System.Drawing.Point(154, 238);
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
            this.label102.Location = new System.Drawing.Point(8, 223);
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
            this.label101.Location = new System.Drawing.Point(8, 238);
            this.label101.Name = "label101";
            this.label101.Size = new System.Drawing.Size(19, 13);
            this.label101.TabIndex = 124;
            this.label101.Text = "13";
            // 
            // cb12r0
            // 
            this.cb12r0.AutoSize = true;
            this.cb12r0.Enabled = false;
            this.cb12r0.Location = new System.Drawing.Point(154, 223);
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
            this.cb12r1.Location = new System.Drawing.Point(138, 223);
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
            this.cb12r2.Location = new System.Drawing.Point(122, 223);
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
            this.cb12r3.Location = new System.Drawing.Point(106, 223);
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
            this.cb12r4.Location = new System.Drawing.Point(90, 223);
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
            this.cb12r5.Location = new System.Drawing.Point(74, 223);
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
            this.cb12r6.Location = new System.Drawing.Point(58, 223);
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
            this.cb12r7.Location = new System.Drawing.Point(41, 223);
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
            this.label100.Location = new System.Drawing.Point(8, 208);
            this.label100.Name = "label100";
            this.label100.Size = new System.Drawing.Size(19, 13);
            this.label100.TabIndex = 115;
            this.label100.Text = "11";
            // 
            // cb11r0
            // 
            this.cb11r0.AutoSize = true;
            this.cb11r0.Enabled = false;
            this.cb11r0.Location = new System.Drawing.Point(154, 208);
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
            this.cb11r1.Location = new System.Drawing.Point(138, 208);
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
            this.cb11r2.Location = new System.Drawing.Point(122, 208);
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
            this.cb11r3.Location = new System.Drawing.Point(106, 208);
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
            this.cb11r4.Location = new System.Drawing.Point(90, 208);
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
            this.cb11r5.Location = new System.Drawing.Point(74, 208);
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
            this.cb11r6.Location = new System.Drawing.Point(58, 208);
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
            this.cb11r7.Location = new System.Drawing.Point(41, 208);
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
            this.label99.Location = new System.Drawing.Point(8, 193);
            this.label99.Name = "label99";
            this.label99.Size = new System.Drawing.Size(19, 13);
            this.label99.TabIndex = 106;
            this.label99.Text = "10";
            // 
            // cb10r0
            // 
            this.cb10r0.AutoSize = true;
            this.cb10r0.Enabled = false;
            this.cb10r0.Location = new System.Drawing.Point(154, 193);
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
            this.cb10r1.Location = new System.Drawing.Point(138, 193);
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
            this.cb10r2.Location = new System.Drawing.Point(122, 193);
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
            this.cb10r3.Location = new System.Drawing.Point(106, 193);
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
            this.cb10r4.Location = new System.Drawing.Point(90, 193);
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
            this.cb10r5.Location = new System.Drawing.Point(74, 193);
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
            this.cb10r6.Location = new System.Drawing.Point(58, 193);
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
            this.cb10r7.Location = new System.Drawing.Point(41, 193);
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
            this.label98.Location = new System.Drawing.Point(14, 178);
            this.label98.Name = "label98";
            this.label98.Size = new System.Drawing.Size(13, 13);
            this.label98.TabIndex = 97;
            this.label98.Text = "9";
            // 
            // cb9r0
            // 
            this.cb9r0.AutoSize = true;
            this.cb9r0.Enabled = false;
            this.cb9r0.Location = new System.Drawing.Point(154, 178);
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
            this.cb9r1.Location = new System.Drawing.Point(138, 178);
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
            this.cb9r2.Location = new System.Drawing.Point(122, 178);
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
            this.cb9r3.Location = new System.Drawing.Point(106, 178);
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
            this.cb9r4.Location = new System.Drawing.Point(90, 178);
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
            this.cb9r5.Location = new System.Drawing.Point(74, 178);
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
            this.cb9r6.Location = new System.Drawing.Point(58, 178);
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
            this.cb9r7.Location = new System.Drawing.Point(41, 178);
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
            this.label97.Location = new System.Drawing.Point(14, 163);
            this.label97.Name = "label97";
            this.label97.Size = new System.Drawing.Size(13, 13);
            this.label97.TabIndex = 88;
            this.label97.Text = "8";
            // 
            // cb8r0
            // 
            this.cb8r0.AutoSize = true;
            this.cb8r0.Enabled = false;
            this.cb8r0.Location = new System.Drawing.Point(154, 163);
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
            this.cb8r1.Location = new System.Drawing.Point(138, 163);
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
            this.cb8r2.Location = new System.Drawing.Point(122, 163);
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
            this.cb8r3.Location = new System.Drawing.Point(106, 163);
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
            this.cb8r4.Location = new System.Drawing.Point(90, 163);
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
            this.cb8r5.Location = new System.Drawing.Point(74, 163);
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
            this.cb8r6.Location = new System.Drawing.Point(58, 163);
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
            this.cb8r7.Location = new System.Drawing.Point(41, 163);
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
            this.label96.Location = new System.Drawing.Point(14, 148);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(13, 13);
            this.label96.TabIndex = 79;
            this.label96.Text = "7";
            // 
            // cb7r0
            // 
            this.cb7r0.AutoSize = true;
            this.cb7r0.Enabled = false;
            this.cb7r0.Location = new System.Drawing.Point(154, 148);
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
            this.cb7r1.Location = new System.Drawing.Point(138, 148);
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
            this.cb7r2.Location = new System.Drawing.Point(122, 148);
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
            this.cb7r3.Location = new System.Drawing.Point(106, 148);
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
            this.cb7r4.Location = new System.Drawing.Point(90, 148);
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
            this.cb7r5.Location = new System.Drawing.Point(74, 148);
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
            this.cb7r6.Location = new System.Drawing.Point(58, 148);
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
            this.cb7r7.Location = new System.Drawing.Point(41, 148);
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
            this.label95.Location = new System.Drawing.Point(14, 133);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(13, 13);
            this.label95.TabIndex = 70;
            this.label95.Text = "6";
            // 
            // cb6r0
            // 
            this.cb6r0.AutoSize = true;
            this.cb6r0.Enabled = false;
            this.cb6r0.Location = new System.Drawing.Point(154, 133);
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
            this.cb6r1.Location = new System.Drawing.Point(138, 133);
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
            this.cb6r2.Location = new System.Drawing.Point(122, 133);
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
            this.cb6r3.Location = new System.Drawing.Point(106, 133);
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
            this.cb6r4.Location = new System.Drawing.Point(90, 133);
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
            this.cb6r5.Location = new System.Drawing.Point(74, 133);
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
            this.cb6r6.Location = new System.Drawing.Point(58, 133);
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
            this.cb6r7.Location = new System.Drawing.Point(41, 133);
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
            this.chkPortA.Location = new System.Drawing.Point(186, 15);
            this.chkPortA.Name = "chkPortA";
            this.chkPortA.Size = new System.Drawing.Size(51, 17);
            this.chkPortA.TabIndex = 2;
            this.chkPortA.Text = "Enab";
            this.toolTip1.SetToolTip(this.chkPortA, "Select to enable Port A output.");
            this.chkPortA.UseVisualStyleBackColor = true;
            this.chkPortA.CheckedChanged += new System.EventHandler(this.chkPortA_CheckedChanged);
            // 
            // txtPortA
            // 
            this.txtPortA.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPortA.Enabled = false;
            this.txtPortA.Location = new System.Drawing.Point(191, 81);
            this.txtPortA.Name = "txtPortA";
            this.txtPortA.Size = new System.Drawing.Size(40, 20);
            this.txtPortA.TabIndex = 136;
            this.txtPortA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPortA, "Enter the port address (decimal) or FW for FlexWire.");
            this.txtPortA.TextChanged += new System.EventHandler(this.txtPortA_TextChanged);
            // 
            // label103
            // 
            this.label103.AutoSize = true;
            this.label103.ForeColor = System.Drawing.Color.Navy;
            this.label103.Location = new System.Drawing.Point(193, 67);
            this.label103.Name = "label103";
            this.label103.Size = new System.Drawing.Size(36, 13);
            this.label103.TabIndex = 135;
            this.label103.Text = "Port A";
            // 
            // txtPortB
            // 
            this.txtPortB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPortB.Enabled = false;
            this.txtPortB.Location = new System.Drawing.Point(190, 257);
            this.txtPortB.Name = "txtPortB";
            this.txtPortB.Size = new System.Drawing.Size(40, 20);
            this.txtPortB.TabIndex = 274;
            this.txtPortB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPortB, "Enter the port address (decimal) or FW for FlexWire.");
            this.txtPortB.TextChanged += new System.EventHandler(this.txtPortB_TextChanged);
            // 
            // label106
            // 
            this.label106.AutoSize = true;
            this.label106.ForeColor = System.Drawing.Color.Navy;
            this.label106.Location = new System.Drawing.Point(192, 243);
            this.label106.Name = "label106";
            this.label106.Size = new System.Drawing.Size(36, 13);
            this.label106.TabIndex = 273;
            this.label106.Text = "Port B";
            // 
            // tabSetup
            // 
            this.tabSetup.Controls.Add(this.chkShortCut);
            this.tabSetup.Controls.Add(this.chkCwTx);
            this.tabSetup.Controls.Add(this.groupBox16);
            this.tabSetup.Controls.Add(this.chkOnTop);
            this.tabSetup.Controls.Add(this.chkNoBdChg);
            this.tabSetup.Controls.Add(this.chkModeChg);
            this.tabSetup.Controls.Add(this.chkAudio);
            this.tabSetup.Controls.Add(this.chkTips);
            this.tabSetup.Controls.Add(this.grpDog);
            this.tabSetup.Controls.Add(this.groupBox10);
            this.tabSetup.Controls.Add(this.groupBox14);
            this.tabSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.tabSetup.Location = new System.Drawing.Point(4, 22);
            this.tabSetup.Name = "tabSetup";
            this.tabSetup.Size = new System.Drawing.Size(431, 311);
            this.tabSetup.TabIndex = 6;
            this.tabSetup.Text = "Setup";
            this.tabSetup.UseVisualStyleBackColor = true;
            // 
            // chkShortCut
            // 
            this.chkShortCut.AutoSize = true;
            this.chkShortCut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkShortCut.ForeColor = System.Drawing.Color.Navy;
            this.chkShortCut.Location = new System.Drawing.Point(156, 250);
            this.chkShortCut.Name = "chkShortCut";
            this.chkShortCut.Size = new System.Drawing.Size(113, 17);
            this.chkShortCut.TabIndex = 111;
            this.chkShortCut.Text = "Disable Short-Cuts";
            this.toolTip1.SetToolTip(this.chkShortCut, "Select to Disable Short-Cut keys.");
            this.chkShortCut.UseVisualStyleBackColor = true;
            this.chkShortCut.CheckedChanged += new System.EventHandler(this.chkShortCut_CheckedChanged);
            // 
            // chkCwTx
            // 
            this.chkCwTx.AutoSize = true;
            this.chkCwTx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkCwTx.ForeColor = System.Drawing.Color.Navy;
            this.chkCwTx.Location = new System.Drawing.Point(156, 270);
            this.chkCwTx.Name = "chkCwTx";
            this.chkCwTx.Size = new System.Drawing.Size(91, 17);
            this.chkCwTx.TabIndex = 110;
            this.chkCwTx.Text = "Show CW TX";
            this.toolTip1.SetToolTip(this.chkCwTx, "Select to show CW Tx filter when in Split.");
            this.chkCwTx.UseVisualStyleBackColor = true;
            this.chkCwTx.CheckedChanged += new System.EventHandler(this.chkCwTx_CheckedChanged);
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.txtPSDR);
            this.groupBox16.Controls.Add(this.label68);
            this.groupBox16.Controls.Add(this.label64);
            this.groupBox16.Controls.Add(this.cboPwrPort);
            this.groupBox16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox16.ForeColor = System.Drawing.Color.Blue;
            this.groupBox16.Location = new System.Drawing.Point(143, 191);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(283, 56);
            this.groupBox16.TabIndex = 31;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Flex Remote";
            // 
            // txtPSDR
            // 
            this.txtPSDR.ForeColor = System.Drawing.Color.Firebrick;
            this.txtPSDR.Location = new System.Drawing.Point(10, 17);
            this.txtPSDR.Name = "txtPSDR";
            this.txtPSDR.Size = new System.Drawing.Size(186, 20);
            this.txtPSDR.TabIndex = 29;
            this.txtPSDR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPSDR, "PowerSDR\'s file location here (double-click for file dialog).");
            this.txtPSDR.DoubleClick += new System.EventHandler(this.txtPSDR_DoubleClick);
            this.txtPSDR.TextChanged += new System.EventHandler(this.txtPSDR_TextChanged);
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label68.ForeColor = System.Drawing.Color.Navy;
            this.label68.Location = new System.Drawing.Point(9, 39);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(123, 13);
            this.label68.TabIndex = 30;
            this.label68.Text = "PowerSDR File Location";
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label64.ForeColor = System.Drawing.Color.Navy;
            this.label64.Location = new System.Drawing.Point(209, 40);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(26, 13);
            this.label64.TabIndex = 35;
            this.label64.Text = "Port";
            // 
            // cboPwrPort
            // 
            this.cboPwrPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPwrPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPwrPort.ForeColor = System.Drawing.Color.Firebrick;
            this.cboPwrPort.FormattingEnabled = true;
            this.cboPwrPort.Location = new System.Drawing.Point(206, 16);
            this.cboPwrPort.Name = "cboPwrPort";
            this.cboPwrPort.Size = new System.Drawing.Size(71, 21);
            this.cboPwrPort.TabIndex = 34;
            this.toolTip1.SetToolTip(this.cboPwrPort, "Flex On/Off control port (uses RTS line, fixed 9600 8N1).");
            this.cboPwrPort.SelectedIndexChanged += new System.EventHandler(this.cboPwrPort_SelectedIndexChanged);
            // 
            // chkOnTop
            // 
            this.chkOnTop.AutoSize = true;
            this.chkOnTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkOnTop.ForeColor = System.Drawing.Color.Navy;
            this.chkOnTop.Location = new System.Drawing.Point(86, 270);
            this.chkOnTop.Name = "chkOnTop";
            this.chkOnTop.Size = new System.Drawing.Size(62, 17);
            this.chkOnTop.TabIndex = 88;
            this.chkOnTop.Text = "On Top";
            this.toolTip1.SetToolTip(this.chkOnTop, "When selected causes this window to be on top of all other windows.");
            this.chkOnTop.UseVisualStyleBackColor = true;
            this.chkOnTop.CheckedChanged += new System.EventHandler(this.chkOnTop_CheckedChanged);
            // 
            // chkNoBdChg
            // 
            this.chkNoBdChg.AutoSize = true;
            this.chkNoBdChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkNoBdChg.ForeColor = System.Drawing.Color.Navy;
            this.chkNoBdChg.Location = new System.Drawing.Point(10, 289);
            this.chkNoBdChg.Name = "chkNoBdChg";
            this.chkNoBdChg.Size = new System.Drawing.Size(55, 17);
            this.chkNoBdChg.TabIndex = 90;
            this.chkNoBdChg.Text = "BOBC";
            this.toolTip1.SetToolTip(this.chkNoBdChg, "Select to disable PTT on Band Change.");
            this.chkNoBdChg.UseVisualStyleBackColor = true;
            this.chkNoBdChg.CheckedChanged += new System.EventHandler(this.chkNoBdChg_CheckedChanged);
            // 
            // chkModeChg
            // 
            this.chkModeChg.AutoSize = true;
            this.chkModeChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkModeChg.ForeColor = System.Drawing.Color.Navy;
            this.chkModeChg.Location = new System.Drawing.Point(10, 270);
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
            this.chkAudio.Location = new System.Drawing.Point(85, 250);
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
            this.chkTips.Location = new System.Drawing.Point(10, 250);
            this.chkTips.Name = "chkTips";
            this.chkTips.Size = new System.Drawing.Size(70, 17);
            this.chkTips.TabIndex = 33;
            this.chkTips.Text = "Tool Tips";
            this.toolTip1.SetToolTip(this.chkTips, "Select to enable Tool Tips when cursor hovers over a control.");
            this.chkTips.UseVisualStyleBackColor = true;
            this.chkTips.CheckedChanged += new System.EventHandler(this.chkTips_CheckedChanged);
            // 
            // grpDog
            // 
            this.grpDog.Controls.Add(this.txtTimer);
            this.grpDog.Controls.Add(this.label67);
            this.grpDog.Controls.Add(this.chkDog);
            this.grpDog.ForeColor = System.Drawing.Color.Blue;
            this.grpDog.Location = new System.Drawing.Point(5, 191);
            this.grpDog.Name = "grpDog";
            this.grpDog.Size = new System.Drawing.Size(127, 56);
            this.grpDog.TabIndex = 77;
            this.grpDog.TabStop = false;
            this.grpDog.Text = "WatchDog Timer";
            // 
            // txtTimer
            // 
            this.txtTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimer.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtTimer.Location = new System.Drawing.Point(64, 22);
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
            this.label67.Location = new System.Drawing.Point(87, 26);
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
            this.chkDog.Location = new System.Drawing.Point(5, 23);
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
            this.groupBox10.Controls.Add(this.chkStepDipole);
            this.groupBox10.Controls.Add(this.txtAzOffset);
            this.groupBox10.Controls.Add(this.label156);
            this.groupBox10.Controls.Add(this.chkTenths);
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
            this.groupBox10.Size = new System.Drawing.Size(295, 177);
            this.groupBox10.TabIndex = 29;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Rotor Setup";
            // 
            // chkStepDipole
            // 
            this.chkStepDipole.AutoSize = true;
            this.chkStepDipole.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStepDipole.ForeColor = System.Drawing.Color.Navy;
            this.chkStepDipole.Location = new System.Drawing.Point(10, 131);
            this.chkStepDipole.Name = "chkStepDipole";
            this.chkStepDipole.Size = new System.Drawing.Size(98, 17);
            this.chkStepDipole.TabIndex = 36;
            this.chkStepDipole.Text = "SteppIR Dipole";
            this.toolTip1.SetToolTip(this.chkStepDipole, "Select to adjust rotor heading for SteppIR Dipole on 80 meters.");
            this.chkStepDipole.UseVisualStyleBackColor = true;
            this.chkStepDipole.CheckedChanged += new System.EventHandler(this.chkStepDipole_CheckedChanged);
            // 
            // txtAzOffset
            // 
            this.txtAzOffset.ForeColor = System.Drawing.Color.Firebrick;
            this.txtAzOffset.Location = new System.Drawing.Point(122, 144);
            this.txtAzOffset.Name = "txtAzOffset";
            this.txtAzOffset.Size = new System.Drawing.Size(40, 20);
            this.txtAzOffset.TabIndex = 35;
            this.txtAzOffset.Text = "0";
            this.txtAzOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtAzOffset, "Azimuth offset degrees (<=180).");
            this.txtAzOffset.TextChanged += new System.EventHandler(this.txtAzOffset_TextChanged);
            // 
            // label156
            // 
            this.label156.AutoSize = true;
            this.label156.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label156.ForeColor = System.Drawing.Color.Navy;
            this.label156.Location = new System.Drawing.Point(118, 129);
            this.label156.Name = "label156";
            this.label156.Size = new System.Drawing.Size(50, 13);
            this.label156.TabIndex = 34;
            this.label156.Text = "Az Offset";
            // 
            // chkTenths
            // 
            this.chkTenths.AutoSize = true;
            this.chkTenths.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTenths.ForeColor = System.Drawing.Color.Navy;
            this.chkTenths.Location = new System.Drawing.Point(10, 154);
            this.chkTenths.Name = "chkTenths";
            this.chkTenths.Size = new System.Drawing.Size(96, 17);
            this.chkTenths.TabIndex = 33;
            this.chkTenths.Text = "Display Tenths";
            this.toolTip1.SetToolTip(this.chkTenths, "Select to enable rotor display in tenths of degrees.");
            this.chkTenths.UseVisualStyleBackColor = true;
            this.chkTenths.CheckedChanged += new System.EventHandler(this.chkTenths_CheckedChanged);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.ForeColor = System.Drawing.Color.Navy;
            this.label50.Location = new System.Drawing.Point(10, 109);
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
            this.label49.Location = new System.Drawing.Point(10, 66);
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
            this.cboRotorCom.Location = new System.Drawing.Point(10, 86);
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
            this.cboRotorPort.Location = new System.Drawing.Point(10, 43);
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
            this.grpSpeed.Location = new System.Drawing.Point(112, 8);
            this.grpSpeed.Name = "grpSpeed";
            this.grpSpeed.Size = new System.Drawing.Size(57, 71);
            this.grpSpeed.TabIndex = 14;
            this.grpSpeed.TabStop = false;
            this.grpSpeed.Text = "Speed";
            // 
            // rbRtrSpd3
            // 
            this.rbRtrSpd3.AutoSize = true;
            this.rbRtrSpd3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbRtrSpd3.ForeColor = System.Drawing.Color.Navy;
            this.rbRtrSpd3.Location = new System.Drawing.Point(5, 50);
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
            this.rbRtrSpd2.Location = new System.Drawing.Point(5, 33);
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
            this.rbRtrSpd1.Location = new System.Drawing.Point(5, 16);
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
            this.grpModel.Size = new System.Drawing.Size(110, 161);
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
            this.groupBox14.Location = new System.Drawing.Point(335, 10);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(91, 178);
            this.groupBox14.TabIndex = 27;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Station Info";
            // 
            // txtCall
            // 
            this.txtCall.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCall.ForeColor = System.Drawing.Color.Firebrick;
            this.txtCall.Location = new System.Drawing.Point(10, 17);
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
            this.label25.Location = new System.Drawing.Point(22, 39);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(48, 13);
            this.label25.TabIndex = 30;
            this.label25.Text = "Call Sign";
            // 
            // txtGrid
            // 
            this.txtGrid.ForeColor = System.Drawing.Color.Firebrick;
            this.txtGrid.Location = new System.Drawing.Point(12, 142);
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
            this.txtLat.Location = new System.Drawing.Point(10, 57);
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
            this.label44.Location = new System.Drawing.Point(30, 164);
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
            this.label51.Location = new System.Drawing.Point(22, 79);
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
            this.label52.Location = new System.Drawing.Point(17, 124);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(54, 13);
            this.label52.TabIndex = 26;
            this.label52.Text = "Longitude";
            // 
            // txtLong
            // 
            this.txtLong.ForeColor = System.Drawing.Color.Firebrick;
            this.txtLong.Location = new System.Drawing.Point(10, 102);
            this.txtLong.Name = "txtLong";
            this.txtLong.Size = new System.Drawing.Size(67, 20);
            this.txtLong.TabIndex = 24;
            this.txtLong.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLong, "Home station longitude");
            this.txtLong.TextChanged += new System.EventHandler(this.txtLong_TextChanged);
            // 
            // tabProp
            // 
            this.tabProp.Controls.Add(this.grpSO2R);
            this.tabProp.Location = new System.Drawing.Point(4, 22);
            this.tabProp.Name = "tabProp";
            this.tabProp.Size = new System.Drawing.Size(431, 311);
            this.tabProp.TabIndex = 10;
            this.tabProp.Text = "SO2R";
            this.tabProp.UseVisualStyleBackColor = true;
            // 
            // grpSO2R
            // 
            this.grpSO2R.Controls.Add(this.btnSO2Rclear);
            this.grpSO2R.Controls.Add(this.btnLoadSO2R);
            this.grpSO2R.Controls.Add(this.label215);
            this.grpSO2R.Controls.Add(this.txtTxB);
            this.grpSO2R.Controls.Add(this.label214);
            this.grpSO2R.Controls.Add(this.txtTxA);
            this.grpSO2R.Controls.Add(this.label202);
            this.grpSO2R.Controls.Add(this.label204);
            this.grpSO2R.Controls.Add(this.label206);
            this.grpSO2R.Controls.Add(this.label207);
            this.grpSO2R.Controls.Add(this.label208);
            this.grpSO2R.Controls.Add(this.label209);
            this.grpSO2R.Controls.Add(this.label210);
            this.grpSO2R.Controls.Add(this.label211);
            this.grpSO2R.Controls.Add(this.label212);
            this.grpSO2R.Controls.Add(this.label213);
            this.grpSO2R.Controls.Add(this.txtHi2_5a);
            this.grpSO2R.Controls.Add(this.chkSO10b);
            this.grpSO2R.Controls.Add(this.chkSO9b);
            this.grpSO2R.Controls.Add(this.chkSO8b);
            this.grpSO2R.Controls.Add(this.chkSO7b);
            this.grpSO2R.Controls.Add(this.chkSO6b);
            this.grpSO2R.Controls.Add(this.chkSO5b);
            this.grpSO2R.Controls.Add(this.chkSO4b);
            this.grpSO2R.Controls.Add(this.chkSO3b);
            this.grpSO2R.Controls.Add(this.chkSO2b);
            this.grpSO2R.Controls.Add(this.chkSO1b);
            this.grpSO2R.Controls.Add(this.label200);
            this.grpSO2R.Controls.Add(this.chk1Amp);
            this.grpSO2R.Controls.Add(this.label205);
            this.grpSO2R.Controls.Add(this.label203);
            this.grpSO2R.Controls.Add(this.txtLPT2);
            this.grpSO2R.Controls.Add(this.btnSaveSO2R);
            this.grpSO2R.Controls.Add(this.label184);
            this.grpSO2R.Controls.Add(this.txtLo1_10b);
            this.grpSO2R.Controls.Add(this.txtLo1_9b);
            this.grpSO2R.Controls.Add(this.txtLo1_8b);
            this.grpSO2R.Controls.Add(this.txtLo1_7b);
            this.grpSO2R.Controls.Add(this.txtLo1_6b);
            this.grpSO2R.Controls.Add(this.txtLo1_5b);
            this.grpSO2R.Controls.Add(this.txtLo1_4b);
            this.grpSO2R.Controls.Add(this.cboSwPort);
            this.grpSO2R.Controls.Add(this.txtLo1_3b);
            this.grpSO2R.Controls.Add(this.txtLo1_2b);
            this.grpSO2R.Controls.Add(this.chkSoEnab);
            this.grpSO2R.Controls.Add(this.txtLo1_1b);
            this.grpSO2R.Controls.Add(this.label201);
            this.grpSO2R.Controls.Add(this.label199);
            this.grpSO2R.Controls.Add(this.txtLo2_10b);
            this.grpSO2R.Controls.Add(this.txtTx_10b);
            this.grpSO2R.Controls.Add(this.txtPwr_10b);
            this.grpSO2R.Controls.Add(this.txtRx_10b);
            this.grpSO2R.Controls.Add(this.label198);
            this.grpSO2R.Controls.Add(this.txtLo2_9b);
            this.grpSO2R.Controls.Add(this.txtTx_9b);
            this.grpSO2R.Controls.Add(this.txtPwr_9b);
            this.grpSO2R.Controls.Add(this.txtRx_9b);
            this.grpSO2R.Controls.Add(this.label197);
            this.grpSO2R.Controls.Add(this.txtLo2_8b);
            this.grpSO2R.Controls.Add(this.txtTx_8b);
            this.grpSO2R.Controls.Add(this.txtPwr_8b);
            this.grpSO2R.Controls.Add(this.txtRx_8b);
            this.grpSO2R.Controls.Add(this.label196);
            this.grpSO2R.Controls.Add(this.txtLo2_7b);
            this.grpSO2R.Controls.Add(this.txtTx_7b);
            this.grpSO2R.Controls.Add(this.txtPwr_7b);
            this.grpSO2R.Controls.Add(this.txtRx_7b);
            this.grpSO2R.Controls.Add(this.label195);
            this.grpSO2R.Controls.Add(this.txtLo2_6b);
            this.grpSO2R.Controls.Add(this.txtTx_6b);
            this.grpSO2R.Controls.Add(this.txtPwr_6b);
            this.grpSO2R.Controls.Add(this.txtRx_6b);
            this.grpSO2R.Controls.Add(this.label194);
            this.grpSO2R.Controls.Add(this.txtLo2_5b);
            this.grpSO2R.Controls.Add(this.txtTx_5b);
            this.grpSO2R.Controls.Add(this.txtPwr_5b);
            this.grpSO2R.Controls.Add(this.txtRx_5b);
            this.grpSO2R.Controls.Add(this.label193);
            this.grpSO2R.Controls.Add(this.txtLo2_4b);
            this.grpSO2R.Controls.Add(this.txtTx_4b);
            this.grpSO2R.Controls.Add(this.txtPwr_4b);
            this.grpSO2R.Controls.Add(this.txtRx_4b);
            this.grpSO2R.Controls.Add(this.label192);
            this.grpSO2R.Controls.Add(this.txtLo2_3b);
            this.grpSO2R.Controls.Add(this.txtTx_3b);
            this.grpSO2R.Controls.Add(this.txtPwr_3b);
            this.grpSO2R.Controls.Add(this.txtRx_3b);
            this.grpSO2R.Controls.Add(this.label191);
            this.grpSO2R.Controls.Add(this.txtLo2_2b);
            this.grpSO2R.Controls.Add(this.txtTx_2b);
            this.grpSO2R.Controls.Add(this.txtPwr_2b);
            this.grpSO2R.Controls.Add(this.txtRx_2b);
            this.grpSO2R.Controls.Add(this.label190);
            this.grpSO2R.Controls.Add(this.label189);
            this.grpSO2R.Controls.Add(this.label185);
            this.grpSO2R.Controls.Add(this.txtLo2_1b);
            this.grpSO2R.Controls.Add(this.label186);
            this.grpSO2R.Controls.Add(this.txtTx_1b);
            this.grpSO2R.Controls.Add(this.label187);
            this.grpSO2R.Controls.Add(this.label188);
            this.grpSO2R.Controls.Add(this.txtPwr_1b);
            this.grpSO2R.Controls.Add(this.txtRx_1b);
            this.grpSO2R.Controls.Add(this.label183);
            this.grpSO2R.Controls.Add(this.txtHi2_10a);
            this.grpSO2R.Controls.Add(this.txtTx_10a);
            this.grpSO2R.Controls.Add(this.txtPwr_10a);
            this.grpSO2R.Controls.Add(this.txtRx_10a);
            this.grpSO2R.Controls.Add(this.txtHi1_10a);
            this.grpSO2R.Controls.Add(this.txtLPT1_10);
            this.grpSO2R.Controls.Add(this.label179);
            this.grpSO2R.Controls.Add(this.chkSO10a);
            this.grpSO2R.Controls.Add(this.txtHi2_9a);
            this.grpSO2R.Controls.Add(this.txtTx_9a);
            this.grpSO2R.Controls.Add(this.txtPwr_9a);
            this.grpSO2R.Controls.Add(this.txtRx_9a);
            this.grpSO2R.Controls.Add(this.txtHi1_9a);
            this.grpSO2R.Controls.Add(this.txtLPT1_9);
            this.grpSO2R.Controls.Add(this.label180);
            this.grpSO2R.Controls.Add(this.chkSO9a);
            this.grpSO2R.Controls.Add(this.txtHi2_8a);
            this.grpSO2R.Controls.Add(this.txtTx_8a);
            this.grpSO2R.Controls.Add(this.txtPwr_8a);
            this.grpSO2R.Controls.Add(this.txtRx_8a);
            this.grpSO2R.Controls.Add(this.txtHi1_8a);
            this.grpSO2R.Controls.Add(this.txtLPT1_8);
            this.grpSO2R.Controls.Add(this.label181);
            this.grpSO2R.Controls.Add(this.chkSO8a);
            this.grpSO2R.Controls.Add(this.txtHi2_7a);
            this.grpSO2R.Controls.Add(this.txtTx_7a);
            this.grpSO2R.Controls.Add(this.txtPwr_7a);
            this.grpSO2R.Controls.Add(this.txtRx_7a);
            this.grpSO2R.Controls.Add(this.txtHi1_7a);
            this.grpSO2R.Controls.Add(this.txtLPT1_7);
            this.grpSO2R.Controls.Add(this.label182);
            this.grpSO2R.Controls.Add(this.chkSO7a);
            this.grpSO2R.Controls.Add(this.txtHi2_6a);
            this.grpSO2R.Controls.Add(this.txtTx_6a);
            this.grpSO2R.Controls.Add(this.txtPwr_6a);
            this.grpSO2R.Controls.Add(this.txtRx_6a);
            this.grpSO2R.Controls.Add(this.txtHi1_6a);
            this.grpSO2R.Controls.Add(this.txtLPT1_6);
            this.grpSO2R.Controls.Add(this.label178);
            this.grpSO2R.Controls.Add(this.chkSO6a);
            this.grpSO2R.Controls.Add(this.txtTx_5a);
            this.grpSO2R.Controls.Add(this.txtPwr_5a);
            this.grpSO2R.Controls.Add(this.txtRx_5a);
            this.grpSO2R.Controls.Add(this.txtHi1_5a);
            this.grpSO2R.Controls.Add(this.txtLPT1_5);
            this.grpSO2R.Controls.Add(this.label177);
            this.grpSO2R.Controls.Add(this.chkSO5a);
            this.grpSO2R.Controls.Add(this.txtHi2_4a);
            this.grpSO2R.Controls.Add(this.txtTx_4a);
            this.grpSO2R.Controls.Add(this.txtPwr_4a);
            this.grpSO2R.Controls.Add(this.txtRx_4a);
            this.grpSO2R.Controls.Add(this.txtHi1_4a);
            this.grpSO2R.Controls.Add(this.txtLPT1_4);
            this.grpSO2R.Controls.Add(this.label176);
            this.grpSO2R.Controls.Add(this.chkSO4a);
            this.grpSO2R.Controls.Add(this.txtHi2_3a);
            this.grpSO2R.Controls.Add(this.txtTx_3a);
            this.grpSO2R.Controls.Add(this.txtPwr_3a);
            this.grpSO2R.Controls.Add(this.txtRx_3a);
            this.grpSO2R.Controls.Add(this.txtHi1_3a);
            this.grpSO2R.Controls.Add(this.txtLPT1_3);
            this.grpSO2R.Controls.Add(this.label175);
            this.grpSO2R.Controls.Add(this.chkSO3a);
            this.grpSO2R.Controls.Add(this.txtHi2_2a);
            this.grpSO2R.Controls.Add(this.txtTx_2a);
            this.grpSO2R.Controls.Add(this.txtPwr_2a);
            this.grpSO2R.Controls.Add(this.txtRx_2a);
            this.grpSO2R.Controls.Add(this.txtHi1_2a);
            this.grpSO2R.Controls.Add(this.txtLPT1_2);
            this.grpSO2R.Controls.Add(this.label168);
            this.grpSO2R.Controls.Add(this.chkSO2a);
            this.grpSO2R.Controls.Add(this.label174);
            this.grpSO2R.Controls.Add(this.txtHi2_1a);
            this.grpSO2R.Controls.Add(this.label173);
            this.grpSO2R.Controls.Add(this.txtTx_1a);
            this.grpSO2R.Controls.Add(this.label172);
            this.grpSO2R.Controls.Add(this.label170);
            this.grpSO2R.Controls.Add(this.txtPwr_1a);
            this.grpSO2R.Controls.Add(this.txtRx_1a);
            this.grpSO2R.Controls.Add(this.txtHi1_1a);
            this.grpSO2R.Controls.Add(this.label167);
            this.grpSO2R.Controls.Add(this.txtLPT1_1);
            this.grpSO2R.Controls.Add(this.label166);
            this.grpSO2R.Controls.Add(this.label169);
            this.grpSO2R.Controls.Add(this.chkSO1a);
            this.grpSO2R.Controls.Add(this.label171);
            this.grpSO2R.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSO2R.ForeColor = System.Drawing.Color.Blue;
            this.grpSO2R.Location = new System.Drawing.Point(3, 4);
            this.grpSO2R.Name = "grpSO2R";
            this.grpSO2R.Size = new System.Drawing.Size(425, 303);
            this.grpSO2R.TabIndex = 81;
            this.grpSO2R.TabStop = false;
            this.grpSO2R.Text = "SO2R   (SO2R.xml)";
            // 
            // btnSO2Rclear
            // 
            this.btnSO2Rclear.BackColor = System.Drawing.Color.Crimson;
            this.btnSO2Rclear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSO2Rclear.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSO2Rclear.Location = new System.Drawing.Point(401, 18);
            this.btnSO2Rclear.Name = "btnSO2Rclear";
            this.btnSO2Rclear.Size = new System.Drawing.Size(10, 10);
            this.btnSO2Rclear.TabIndex = 267;
            this.toolTip1.SetToolTip(this.btnSO2Rclear, "Press to clear matrix.");
            this.btnSO2Rclear.UseVisualStyleBackColor = false;
            this.btnSO2Rclear.Click += new System.EventHandler(this.btnSO2Rclear_Click);
            // 
            // btnLoadSO2R
            // 
            this.btnLoadSO2R.AutoSize = true;
            this.btnLoadSO2R.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLoadSO2R.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnLoadSO2R.ForeColor = System.Drawing.Color.Navy;
            this.btnLoadSO2R.Location = new System.Drawing.Point(348, 11);
            this.btnLoadSO2R.Name = "btnLoadSO2R";
            this.btnLoadSO2R.Size = new System.Drawing.Size(45, 23);
            this.btnLoadSO2R.TabIndex = 266;
            this.btnLoadSO2R.Text = "Load";
            this.toolTip1.SetToolTip(this.btnLoadSO2R, "Press to load matrix data.");
            this.btnLoadSO2R.UseVisualStyleBackColor = true;
            this.btnLoadSO2R.Click += new System.EventHandler(this.btnLoadSO2R_Click);
            // 
            // label215
            // 
            this.label215.AutoSize = true;
            this.label215.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label215.ForeColor = System.Drawing.Color.Blue;
            this.label215.Location = new System.Drawing.Point(202, 49);
            this.label215.Name = "label215";
            this.label215.Size = new System.Drawing.Size(64, 13);
            this.label215.TabIndex = 265;
            this.label215.Text = "AmpKey B";
            // 
            // txtTxB
            // 
            this.txtTxB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTxB.Location = new System.Drawing.Point(185, 45);
            this.txtTxB.Name = "txtTxB";
            this.txtTxB.Size = new System.Drawing.Size(15, 20);
            this.txtTxB.TabIndex = 264;
            this.txtTxB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTxB, "Enter VFO B TX control line to use for PTT (1, 2, 3).");
            this.txtTxB.TextChanged += new System.EventHandler(this.txtTxB_TextChanged);
            // 
            // label214
            // 
            this.label214.AutoSize = true;
            this.label214.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label214.ForeColor = System.Drawing.Color.Firebrick;
            this.label214.Location = new System.Drawing.Point(202, 25);
            this.label214.Name = "label214";
            this.label214.Size = new System.Drawing.Size(64, 13);
            this.label214.TabIndex = 263;
            this.label214.Text = "AmpKey A";
            // 
            // txtTxA
            // 
            this.txtTxA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTxA.Location = new System.Drawing.Point(185, 21);
            this.txtTxA.Name = "txtTxA";
            this.txtTxA.Size = new System.Drawing.Size(15, 20);
            this.txtTxA.TabIndex = 262;
            this.txtTxA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTxA, "Enter VFO A TX control line to use for PTT (1, 2, 3).");
            this.txtTxA.TextChanged += new System.EventHandler(this.txtTxA_TextChanged);
            // 
            // label202
            // 
            this.label202.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label202.ForeColor = System.Drawing.Color.Navy;
            this.label202.Location = new System.Drawing.Point(397, 280);
            this.label202.Name = "label202";
            this.label202.Size = new System.Drawing.Size(25, 13);
            this.label202.TabIndex = 261;
            this.label202.Text = "6";
            this.label202.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label204
            // 
            this.label204.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label204.ForeColor = System.Drawing.Color.Navy;
            this.label204.Location = new System.Drawing.Point(397, 260);
            this.label204.Name = "label204";
            this.label204.Size = new System.Drawing.Size(25, 13);
            this.label204.TabIndex = 260;
            this.label204.Text = "10";
            // 
            // label206
            // 
            this.label206.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label206.ForeColor = System.Drawing.Color.Navy;
            this.label206.Location = new System.Drawing.Point(397, 240);
            this.label206.Name = "label206";
            this.label206.Size = new System.Drawing.Size(25, 13);
            this.label206.TabIndex = 259;
            this.label206.Text = "12";
            // 
            // label207
            // 
            this.label207.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label207.ForeColor = System.Drawing.Color.Navy;
            this.label207.Location = new System.Drawing.Point(397, 220);
            this.label207.Name = "label207";
            this.label207.Size = new System.Drawing.Size(25, 13);
            this.label207.TabIndex = 258;
            this.label207.Text = "15";
            // 
            // label208
            // 
            this.label208.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label208.ForeColor = System.Drawing.Color.Navy;
            this.label208.Location = new System.Drawing.Point(397, 200);
            this.label208.Name = "label208";
            this.label208.Size = new System.Drawing.Size(25, 13);
            this.label208.TabIndex = 257;
            this.label208.Text = "17";
            // 
            // label209
            // 
            this.label209.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label209.ForeColor = System.Drawing.Color.Navy;
            this.label209.Location = new System.Drawing.Point(397, 180);
            this.label209.Name = "label209";
            this.label209.Size = new System.Drawing.Size(25, 13);
            this.label209.TabIndex = 256;
            this.label209.Text = "20";
            // 
            // label210
            // 
            this.label210.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label210.ForeColor = System.Drawing.Color.Navy;
            this.label210.Location = new System.Drawing.Point(397, 160);
            this.label210.Name = "label210";
            this.label210.Size = new System.Drawing.Size(25, 13);
            this.label210.TabIndex = 255;
            this.label210.Text = "30";
            // 
            // label211
            // 
            this.label211.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label211.ForeColor = System.Drawing.Color.Navy;
            this.label211.Location = new System.Drawing.Point(397, 140);
            this.label211.Name = "label211";
            this.label211.Size = new System.Drawing.Size(25, 13);
            this.label211.TabIndex = 254;
            this.label211.Text = "40";
            // 
            // label212
            // 
            this.label212.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label212.ForeColor = System.Drawing.Color.Navy;
            this.label212.Location = new System.Drawing.Point(397, 120);
            this.label212.Name = "label212";
            this.label212.Size = new System.Drawing.Size(25, 13);
            this.label212.TabIndex = 253;
            this.label212.Text = "80";
            // 
            // label213
            // 
            this.label213.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label213.ForeColor = System.Drawing.Color.Navy;
            this.label213.Location = new System.Drawing.Point(395, 100);
            this.label213.Name = "label213";
            this.label213.Size = new System.Drawing.Size(28, 13);
            this.label213.TabIndex = 252;
            this.label213.Text = "160";
            // 
            // txtHi2_5a
            // 
            this.txtHi2_5a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi2_5a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi2_5a.Location = new System.Drawing.Point(211, 176);
            this.txtHi2_5a.Name = "txtHi2_5a";
            this.txtHi2_5a.Size = new System.Drawing.Size(18, 18);
            this.txtHi2_5a.TabIndex = 251;
            this.txtHi2_5a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi2_5a, "Amplifer A control code (hex notation)");
            this.txtHi2_5a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // chkSO10b
            // 
            this.chkSO10b.AutoSize = true;
            this.chkSO10b.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO10b.Location = new System.Drawing.Point(378, 280);
            this.chkSO10b.Name = "chkSO10b";
            this.chkSO10b.Size = new System.Drawing.Size(15, 14);
            this.chkSO10b.TabIndex = 250;
            this.toolTip1.SetToolTip(this.chkSO10b, "Check to enable the use of this band for VFO B.");
            this.chkSO10b.UseVisualStyleBackColor = true;
            this.chkSO10b.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // chkSO9b
            // 
            this.chkSO9b.AutoSize = true;
            this.chkSO9b.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO9b.Location = new System.Drawing.Point(378, 260);
            this.chkSO9b.Name = "chkSO9b";
            this.chkSO9b.Size = new System.Drawing.Size(15, 14);
            this.chkSO9b.TabIndex = 249;
            this.toolTip1.SetToolTip(this.chkSO9b, "Check to enable the use of this band for VFO B.");
            this.chkSO9b.UseVisualStyleBackColor = true;
            this.chkSO9b.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // chkSO8b
            // 
            this.chkSO8b.AutoSize = true;
            this.chkSO8b.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO8b.Location = new System.Drawing.Point(378, 240);
            this.chkSO8b.Name = "chkSO8b";
            this.chkSO8b.Size = new System.Drawing.Size(15, 14);
            this.chkSO8b.TabIndex = 248;
            this.toolTip1.SetToolTip(this.chkSO8b, "Check to enable the use of this band for VFO B.");
            this.chkSO8b.UseVisualStyleBackColor = true;
            this.chkSO8b.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // chkSO7b
            // 
            this.chkSO7b.AutoSize = true;
            this.chkSO7b.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO7b.Location = new System.Drawing.Point(378, 220);
            this.chkSO7b.Name = "chkSO7b";
            this.chkSO7b.Size = new System.Drawing.Size(15, 14);
            this.chkSO7b.TabIndex = 247;
            this.toolTip1.SetToolTip(this.chkSO7b, "Check to enable the use of this band for VFO B.");
            this.chkSO7b.UseVisualStyleBackColor = true;
            this.chkSO7b.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // chkSO6b
            // 
            this.chkSO6b.AutoSize = true;
            this.chkSO6b.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO6b.Location = new System.Drawing.Point(378, 200);
            this.chkSO6b.Name = "chkSO6b";
            this.chkSO6b.Size = new System.Drawing.Size(15, 14);
            this.chkSO6b.TabIndex = 246;
            this.toolTip1.SetToolTip(this.chkSO6b, "Check to enable the use of this band for VFO B.");
            this.chkSO6b.UseVisualStyleBackColor = true;
            this.chkSO6b.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // chkSO5b
            // 
            this.chkSO5b.AutoSize = true;
            this.chkSO5b.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO5b.Location = new System.Drawing.Point(378, 180);
            this.chkSO5b.Name = "chkSO5b";
            this.chkSO5b.Size = new System.Drawing.Size(15, 14);
            this.chkSO5b.TabIndex = 245;
            this.toolTip1.SetToolTip(this.chkSO5b, "Check to enable the use of this band for VFO B.");
            this.chkSO5b.UseVisualStyleBackColor = true;
            this.chkSO5b.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // chkSO4b
            // 
            this.chkSO4b.AutoSize = true;
            this.chkSO4b.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO4b.Location = new System.Drawing.Point(378, 160);
            this.chkSO4b.Name = "chkSO4b";
            this.chkSO4b.Size = new System.Drawing.Size(15, 14);
            this.chkSO4b.TabIndex = 244;
            this.toolTip1.SetToolTip(this.chkSO4b, "Check to enable the use of this band for VFO B.");
            this.chkSO4b.UseVisualStyleBackColor = true;
            this.chkSO4b.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // chkSO3b
            // 
            this.chkSO3b.AutoSize = true;
            this.chkSO3b.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO3b.Location = new System.Drawing.Point(378, 140);
            this.chkSO3b.Name = "chkSO3b";
            this.chkSO3b.Size = new System.Drawing.Size(15, 14);
            this.chkSO3b.TabIndex = 243;
            this.toolTip1.SetToolTip(this.chkSO3b, "Check to enable the use of this band for VFO B.");
            this.chkSO3b.UseVisualStyleBackColor = true;
            this.chkSO3b.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // chkSO2b
            // 
            this.chkSO2b.AutoSize = true;
            this.chkSO2b.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO2b.Location = new System.Drawing.Point(378, 120);
            this.chkSO2b.Name = "chkSO2b";
            this.chkSO2b.Size = new System.Drawing.Size(15, 14);
            this.chkSO2b.TabIndex = 242;
            this.toolTip1.SetToolTip(this.chkSO2b, "Check to enable the use of this band for VFO B.");
            this.chkSO2b.UseVisualStyleBackColor = true;
            this.chkSO2b.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // chkSO1b
            // 
            this.chkSO1b.AutoSize = true;
            this.chkSO1b.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO1b.Location = new System.Drawing.Point(378, 100);
            this.chkSO1b.Name = "chkSO1b";
            this.chkSO1b.Size = new System.Drawing.Size(15, 14);
            this.chkSO1b.TabIndex = 240;
            this.toolTip1.SetToolTip(this.chkSO1b, "Check to enable the use of this band for VFO B.");
            this.chkSO1b.UseVisualStyleBackColor = true;
            this.chkSO1b.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // label200
            // 
            this.label200.AutoSize = true;
            this.label200.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label200.ForeColor = System.Drawing.Color.Blue;
            this.label200.Location = new System.Drawing.Point(372, 81);
            this.label200.Name = "label200";
            this.label200.Size = new System.Drawing.Size(29, 13);
            this.label200.TabIndex = 241;
            this.label200.Text = "Use";
            // 
            // chk1Amp
            // 
            this.chk1Amp.AutoSize = true;
            this.chk1Amp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.chk1Amp.ForeColor = System.Drawing.Color.Navy;
            this.chk1Amp.Location = new System.Drawing.Point(9, 42);
            this.chk1Amp.Name = "chk1Amp";
            this.chk1Amp.Size = new System.Drawing.Size(89, 17);
            this.chk1Amp.TabIndex = 239;
            this.chk1Amp.Text = "Single Amp";
            this.toolTip1.SetToolTip(this.chk1Amp, "Select to enable only a single amplifier.");
            this.chk1Amp.UseVisualStyleBackColor = true;
            this.chk1Amp.CheckedChanged += new System.EventHandler(this.chk1Amp_CheckedChanged);
            // 
            // label205
            // 
            this.label205.AutoSize = true;
            this.label205.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold);
            this.label205.ForeColor = System.Drawing.Color.Navy;
            this.label205.Location = new System.Drawing.Point(480, 158);
            this.label205.Name = "label205";
            this.label205.Size = new System.Drawing.Size(46, 13);
            this.label205.TabIndex = 238;
            this.label205.Text = "LPT #1";
            // 
            // label203
            // 
            this.label203.AutoSize = true;
            this.label203.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label203.ForeColor = System.Drawing.Color.Blue;
            this.label203.Location = new System.Drawing.Point(350, 44);
            this.label203.Name = "label203";
            this.label203.Size = new System.Drawing.Size(41, 13);
            this.label203.TabIndex = 234;
            this.label203.Text = "LPT 2";
            // 
            // txtLPT2
            // 
            this.txtLPT2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLPT2.Location = new System.Drawing.Point(298, 40);
            this.txtLPT2.Name = "txtLPT2";
            this.txtLPT2.Size = new System.Drawing.Size(42, 20);
            this.txtLPT2.TabIndex = 233;
            this.txtLPT2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLPT2, "LPT port number (decimal) for VFO B band and data control.");
            this.txtLPT2.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // btnSaveSO2R
            // 
            this.btnSaveSO2R.AutoSize = true;
            this.btnSaveSO2R.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSaveSO2R.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSaveSO2R.ForeColor = System.Drawing.Color.Navy;
            this.btnSaveSO2R.Location = new System.Drawing.Point(296, 11);
            this.btnSaveSO2R.Name = "btnSaveSO2R";
            this.btnSaveSO2R.Size = new System.Drawing.Size(46, 23);
            this.btnSaveSO2R.TabIndex = 230;
            this.btnSaveSO2R.Text = "Save";
            this.toolTip1.SetToolTip(this.btnSaveSO2R, "Press to save matrix data.");
            this.btnSaveSO2R.UseVisualStyleBackColor = true;
            this.btnSaveSO2R.Click += new System.EventHandler(this.btnSaveSO2R_Click);
            // 
            // label184
            // 
            this.label184.AutoSize = true;
            this.label184.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label184.ForeColor = System.Drawing.Color.Navy;
            this.label184.Location = new System.Drawing.Point(103, 43);
            this.label184.Name = "label184";
            this.label184.Size = new System.Drawing.Size(65, 13);
            this.label184.TabIndex = 228;
            this.label184.Text = "TX Switch";
            // 
            // txtLo1_10b
            // 
            this.txtLo1_10b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo1_10b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo1_10b.Location = new System.Drawing.Point(244, 276);
            this.txtLo1_10b.Name = "txtLo1_10b";
            this.txtLo1_10b.Size = new System.Drawing.Size(18, 18);
            this.txtLo1_10b.TabIndex = 225;
            this.txtLo1_10b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo1_10b, "VFO B band data (Hex notation).");
            this.txtLo1_10b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo1_9b
            // 
            this.txtLo1_9b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo1_9b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo1_9b.Location = new System.Drawing.Point(244, 256);
            this.txtLo1_9b.Name = "txtLo1_9b";
            this.txtLo1_9b.Size = new System.Drawing.Size(18, 18);
            this.txtLo1_9b.TabIndex = 224;
            this.txtLo1_9b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo1_9b, "VFO B band data (Hex notation).");
            this.txtLo1_9b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo1_8b
            // 
            this.txtLo1_8b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo1_8b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo1_8b.Location = new System.Drawing.Point(244, 236);
            this.txtLo1_8b.Name = "txtLo1_8b";
            this.txtLo1_8b.Size = new System.Drawing.Size(18, 18);
            this.txtLo1_8b.TabIndex = 223;
            this.txtLo1_8b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo1_8b, "VFO B band data (Hex notation).");
            this.txtLo1_8b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo1_7b
            // 
            this.txtLo1_7b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo1_7b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo1_7b.Location = new System.Drawing.Point(244, 216);
            this.txtLo1_7b.Name = "txtLo1_7b";
            this.txtLo1_7b.Size = new System.Drawing.Size(18, 18);
            this.txtLo1_7b.TabIndex = 222;
            this.txtLo1_7b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo1_7b, "VFO B band data (Hex notation).");
            this.txtLo1_7b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo1_6b
            // 
            this.txtLo1_6b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo1_6b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo1_6b.Location = new System.Drawing.Point(244, 196);
            this.txtLo1_6b.Name = "txtLo1_6b";
            this.txtLo1_6b.Size = new System.Drawing.Size(18, 18);
            this.txtLo1_6b.TabIndex = 221;
            this.txtLo1_6b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo1_6b, "VFO B band data (Hex notation).");
            this.txtLo1_6b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo1_5b
            // 
            this.txtLo1_5b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo1_5b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo1_5b.Location = new System.Drawing.Point(244, 176);
            this.txtLo1_5b.Name = "txtLo1_5b";
            this.txtLo1_5b.Size = new System.Drawing.Size(18, 18);
            this.txtLo1_5b.TabIndex = 220;
            this.txtLo1_5b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo1_5b, "VFO B band data (Hex notation).");
            this.txtLo1_5b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo1_4b
            // 
            this.txtLo1_4b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo1_4b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo1_4b.Location = new System.Drawing.Point(244, 156);
            this.txtLo1_4b.Name = "txtLo1_4b";
            this.txtLo1_4b.Size = new System.Drawing.Size(18, 18);
            this.txtLo1_4b.TabIndex = 219;
            this.txtLo1_4b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo1_4b, "VFO B band data (Hex notation).");
            this.txtLo1_4b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // cboSwPort
            // 
            this.cboSwPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSwPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold);
            this.cboSwPort.ForeColor = System.Drawing.Color.Firebrick;
            this.cboSwPort.FormattingEnabled = true;
            this.cboSwPort.Location = new System.Drawing.Point(103, 20);
            this.cboSwPort.Name = "cboSwPort";
            this.cboSwPort.Size = new System.Drawing.Size(68, 21);
            this.cboSwPort.TabIndex = 227;
            this.toolTip1.SetToolTip(this.cboSwPort, "TX select switch port");
            this.cboSwPort.SelectedIndexChanged += new System.EventHandler(this.cboSwPort_SelectedIndexChanged);
            // 
            // txtLo1_3b
            // 
            this.txtLo1_3b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo1_3b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo1_3b.Location = new System.Drawing.Point(244, 136);
            this.txtLo1_3b.Name = "txtLo1_3b";
            this.txtLo1_3b.Size = new System.Drawing.Size(18, 18);
            this.txtLo1_3b.TabIndex = 218;
            this.txtLo1_3b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo1_3b, "VFO B band data (Hex notation).");
            this.txtLo1_3b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo1_2b
            // 
            this.txtLo1_2b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo1_2b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo1_2b.Location = new System.Drawing.Point(244, 116);
            this.txtLo1_2b.Name = "txtLo1_2b";
            this.txtLo1_2b.Size = new System.Drawing.Size(18, 18);
            this.txtLo1_2b.TabIndex = 217;
            this.txtLo1_2b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo1_2b, "VFO B band data (Hex notation).");
            this.txtLo1_2b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // chkSoEnab
            // 
            this.chkSoEnab.AutoSize = true;
            this.chkSoEnab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.chkSoEnab.ForeColor = System.Drawing.Color.Navy;
            this.chkSoEnab.Location = new System.Drawing.Point(9, 22);
            this.chkSoEnab.Name = "chkSoEnab";
            this.chkSoEnab.Size = new System.Drawing.Size(65, 17);
            this.chkSoEnab.TabIndex = 226;
            this.chkSoEnab.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkSoEnab, "Select to enable SO2R operation");
            this.chkSoEnab.UseVisualStyleBackColor = true;
            this.chkSoEnab.CheckedChanged += new System.EventHandler(this.chkSoEnab_CheckedChanged);
            // 
            // txtLo1_1b
            // 
            this.txtLo1_1b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo1_1b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo1_1b.Location = new System.Drawing.Point(244, 96);
            this.txtLo1_1b.Name = "txtLo1_1b";
            this.txtLo1_1b.Size = new System.Drawing.Size(18, 18);
            this.txtLo1_1b.TabIndex = 216;
            this.txtLo1_1b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo1_1b, "VFO B band data (Hex notation).");
            this.txtLo1_1b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label201
            // 
            this.label201.AutoSize = true;
            this.label201.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label201.ForeColor = System.Drawing.Color.Blue;
            this.label201.Location = new System.Drawing.Point(243, 80);
            this.label201.Name = "label201";
            this.label201.Size = new System.Drawing.Size(23, 13);
            this.label201.TabIndex = 215;
            this.label201.Text = "Db";
            this.label201.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label199
            // 
            this.label199.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label199.ForeColor = System.Drawing.Color.Navy;
            this.label199.Location = new System.Drawing.Point(233, 279);
            this.label199.Name = "label199";
            this.label199.Size = new System.Drawing.Size(9, 13);
            this.label199.TabIndex = 214;
            this.label199.Text = "-";
            this.label199.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label199.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo2_10b
            // 
            this.txtLo2_10b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo2_10b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo2_10b.Location = new System.Drawing.Point(351, 276);
            this.txtLo2_10b.Name = "txtLo2_10b";
            this.txtLo2_10b.Size = new System.Drawing.Size(18, 18);
            this.txtLo2_10b.TabIndex = 213;
            this.txtLo2_10b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo2_10b, "Amplifer B control code (hex notation).");
            this.txtLo2_10b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_10b
            // 
            this.txtTx_10b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_10b.Location = new System.Drawing.Point(327, 276);
            this.txtTx_10b.Name = "txtTx_10b";
            this.txtTx_10b.Size = new System.Drawing.Size(18, 18);
            this.txtTx_10b.TabIndex = 212;
            this.txtTx_10b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_10b, "TX antenna to use for this band (1-3).");
            this.txtTx_10b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_10b
            // 
            this.txtPwr_10b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_10b.Location = new System.Drawing.Point(267, 276);
            this.txtPwr_10b.Name = "txtPwr_10b";
            this.txtPwr_10b.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_10b.TabIndex = 211;
            this.txtPwr_10b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_10b, "Drive Power Amp B (decimal).");
            this.txtPwr_10b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_10b
            // 
            this.txtRx_10b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_10b.Location = new System.Drawing.Point(303, 276);
            this.txtRx_10b.Name = "txtRx_10b";
            this.txtRx_10b.Size = new System.Drawing.Size(18, 18);
            this.txtRx_10b.TabIndex = 210;
            this.txtRx_10b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_10b, "RX2 antenna to use for this band (1-3).");
            this.txtRx_10b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label198
            // 
            this.label198.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label198.ForeColor = System.Drawing.Color.Navy;
            this.label198.Location = new System.Drawing.Point(233, 259);
            this.label198.Name = "label198";
            this.label198.Size = new System.Drawing.Size(9, 13);
            this.label198.TabIndex = 209;
            this.label198.Text = "-";
            this.label198.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label198.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo2_9b
            // 
            this.txtLo2_9b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo2_9b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo2_9b.Location = new System.Drawing.Point(351, 256);
            this.txtLo2_9b.Name = "txtLo2_9b";
            this.txtLo2_9b.Size = new System.Drawing.Size(18, 18);
            this.txtLo2_9b.TabIndex = 208;
            this.txtLo2_9b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo2_9b, "Amplifer B control code (hex notation).");
            this.txtLo2_9b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_9b
            // 
            this.txtTx_9b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_9b.Location = new System.Drawing.Point(327, 256);
            this.txtTx_9b.Name = "txtTx_9b";
            this.txtTx_9b.Size = new System.Drawing.Size(18, 18);
            this.txtTx_9b.TabIndex = 207;
            this.txtTx_9b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_9b, "TX antenna to use for this band (1-3).");
            this.txtTx_9b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_9b
            // 
            this.txtPwr_9b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_9b.Location = new System.Drawing.Point(267, 256);
            this.txtPwr_9b.Name = "txtPwr_9b";
            this.txtPwr_9b.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_9b.TabIndex = 206;
            this.txtPwr_9b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_9b, "Drive Power Amp B (decimal).");
            this.txtPwr_9b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_9b
            // 
            this.txtRx_9b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_9b.Location = new System.Drawing.Point(303, 256);
            this.txtRx_9b.Name = "txtRx_9b";
            this.txtRx_9b.Size = new System.Drawing.Size(18, 18);
            this.txtRx_9b.TabIndex = 205;
            this.txtRx_9b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_9b, "RX2 antenna to use for this band (1-3).");
            this.txtRx_9b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label197
            // 
            this.label197.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label197.ForeColor = System.Drawing.Color.Navy;
            this.label197.Location = new System.Drawing.Point(233, 239);
            this.label197.Name = "label197";
            this.label197.Size = new System.Drawing.Size(9, 13);
            this.label197.TabIndex = 204;
            this.label197.Text = "-";
            this.label197.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label197.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo2_8b
            // 
            this.txtLo2_8b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo2_8b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo2_8b.Location = new System.Drawing.Point(351, 236);
            this.txtLo2_8b.Name = "txtLo2_8b";
            this.txtLo2_8b.Size = new System.Drawing.Size(18, 18);
            this.txtLo2_8b.TabIndex = 203;
            this.txtLo2_8b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo2_8b, "Amplifer B control code (hex notation).");
            this.txtLo2_8b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_8b
            // 
            this.txtTx_8b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_8b.Location = new System.Drawing.Point(327, 236);
            this.txtTx_8b.Name = "txtTx_8b";
            this.txtTx_8b.Size = new System.Drawing.Size(18, 18);
            this.txtTx_8b.TabIndex = 202;
            this.txtTx_8b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_8b, "TX antenna to use for this band (1-3).");
            this.txtTx_8b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_8b
            // 
            this.txtPwr_8b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_8b.Location = new System.Drawing.Point(267, 236);
            this.txtPwr_8b.Name = "txtPwr_8b";
            this.txtPwr_8b.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_8b.TabIndex = 201;
            this.txtPwr_8b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_8b, "Drive Power Amp B (decimal).");
            this.txtPwr_8b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_8b
            // 
            this.txtRx_8b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_8b.Location = new System.Drawing.Point(303, 236);
            this.txtRx_8b.Name = "txtRx_8b";
            this.txtRx_8b.Size = new System.Drawing.Size(18, 18);
            this.txtRx_8b.TabIndex = 200;
            this.txtRx_8b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_8b, "RX2 antenna to use for this band (1-3).");
            this.txtRx_8b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label196
            // 
            this.label196.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label196.ForeColor = System.Drawing.Color.Navy;
            this.label196.Location = new System.Drawing.Point(233, 219);
            this.label196.Name = "label196";
            this.label196.Size = new System.Drawing.Size(9, 13);
            this.label196.TabIndex = 199;
            this.label196.Text = "-";
            this.label196.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label196.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo2_7b
            // 
            this.txtLo2_7b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo2_7b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo2_7b.Location = new System.Drawing.Point(351, 216);
            this.txtLo2_7b.Name = "txtLo2_7b";
            this.txtLo2_7b.Size = new System.Drawing.Size(18, 18);
            this.txtLo2_7b.TabIndex = 198;
            this.txtLo2_7b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo2_7b, "Amplifer B control code (hex notation).");
            this.txtLo2_7b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_7b
            // 
            this.txtTx_7b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_7b.Location = new System.Drawing.Point(327, 216);
            this.txtTx_7b.Name = "txtTx_7b";
            this.txtTx_7b.Size = new System.Drawing.Size(18, 18);
            this.txtTx_7b.TabIndex = 197;
            this.txtTx_7b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_7b, "TX antenna to use for this band (1-3).");
            this.txtTx_7b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_7b
            // 
            this.txtPwr_7b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_7b.Location = new System.Drawing.Point(267, 216);
            this.txtPwr_7b.Name = "txtPwr_7b";
            this.txtPwr_7b.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_7b.TabIndex = 196;
            this.txtPwr_7b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_7b, "Drive Power Amp B (decimal).");
            this.txtPwr_7b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_7b
            // 
            this.txtRx_7b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_7b.Location = new System.Drawing.Point(303, 216);
            this.txtRx_7b.Name = "txtRx_7b";
            this.txtRx_7b.Size = new System.Drawing.Size(18, 18);
            this.txtRx_7b.TabIndex = 195;
            this.txtRx_7b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_7b, "RX2 antenna to use for this band (1-3).");
            this.txtRx_7b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label195
            // 
            this.label195.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label195.ForeColor = System.Drawing.Color.Navy;
            this.label195.Location = new System.Drawing.Point(233, 199);
            this.label195.Name = "label195";
            this.label195.Size = new System.Drawing.Size(9, 13);
            this.label195.TabIndex = 194;
            this.label195.Text = "-";
            this.label195.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label195.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo2_6b
            // 
            this.txtLo2_6b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo2_6b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo2_6b.Location = new System.Drawing.Point(351, 196);
            this.txtLo2_6b.Name = "txtLo2_6b";
            this.txtLo2_6b.Size = new System.Drawing.Size(18, 18);
            this.txtLo2_6b.TabIndex = 193;
            this.txtLo2_6b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo2_6b, "Amplifer B control code (hex notation).");
            this.txtLo2_6b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_6b
            // 
            this.txtTx_6b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_6b.Location = new System.Drawing.Point(327, 196);
            this.txtTx_6b.Name = "txtTx_6b";
            this.txtTx_6b.Size = new System.Drawing.Size(18, 18);
            this.txtTx_6b.TabIndex = 192;
            this.txtTx_6b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_6b, "TX antenna to use for this band (1-3).");
            this.txtTx_6b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_6b
            // 
            this.txtPwr_6b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_6b.Location = new System.Drawing.Point(267, 196);
            this.txtPwr_6b.Name = "txtPwr_6b";
            this.txtPwr_6b.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_6b.TabIndex = 191;
            this.txtPwr_6b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_6b, "Drive Power Amp B (decimal).");
            this.txtPwr_6b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_6b
            // 
            this.txtRx_6b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_6b.Location = new System.Drawing.Point(303, 196);
            this.txtRx_6b.Name = "txtRx_6b";
            this.txtRx_6b.Size = new System.Drawing.Size(18, 18);
            this.txtRx_6b.TabIndex = 190;
            this.txtRx_6b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_6b, "RX2 antenna to use for this band (1-3).");
            this.txtRx_6b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label194
            // 
            this.label194.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label194.ForeColor = System.Drawing.Color.Navy;
            this.label194.Location = new System.Drawing.Point(233, 179);
            this.label194.Name = "label194";
            this.label194.Size = new System.Drawing.Size(9, 13);
            this.label194.TabIndex = 189;
            this.label194.Text = "-";
            this.label194.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label194.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo2_5b
            // 
            this.txtLo2_5b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo2_5b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo2_5b.Location = new System.Drawing.Point(351, 176);
            this.txtLo2_5b.Name = "txtLo2_5b";
            this.txtLo2_5b.Size = new System.Drawing.Size(18, 18);
            this.txtLo2_5b.TabIndex = 188;
            this.txtLo2_5b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo2_5b, "Amplifer B control code (hex notation).");
            this.txtLo2_5b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_5b
            // 
            this.txtTx_5b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_5b.Location = new System.Drawing.Point(327, 176);
            this.txtTx_5b.Name = "txtTx_5b";
            this.txtTx_5b.Size = new System.Drawing.Size(18, 18);
            this.txtTx_5b.TabIndex = 187;
            this.txtTx_5b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_5b, "TX antenna to use for this band (1-3).");
            this.txtTx_5b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_5b
            // 
            this.txtPwr_5b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_5b.Location = new System.Drawing.Point(267, 176);
            this.txtPwr_5b.Name = "txtPwr_5b";
            this.txtPwr_5b.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_5b.TabIndex = 186;
            this.txtPwr_5b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_5b, "Drive Power Amp B (decimal).");
            this.txtPwr_5b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_5b
            // 
            this.txtRx_5b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_5b.Location = new System.Drawing.Point(303, 176);
            this.txtRx_5b.Name = "txtRx_5b";
            this.txtRx_5b.Size = new System.Drawing.Size(18, 18);
            this.txtRx_5b.TabIndex = 185;
            this.txtRx_5b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_5b, "RX2 antenna to use for this band (1-3).");
            this.txtRx_5b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label193
            // 
            this.label193.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label193.ForeColor = System.Drawing.Color.Navy;
            this.label193.Location = new System.Drawing.Point(233, 159);
            this.label193.Name = "label193";
            this.label193.Size = new System.Drawing.Size(9, 13);
            this.label193.TabIndex = 184;
            this.label193.Text = "-";
            this.label193.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label193.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo2_4b
            // 
            this.txtLo2_4b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo2_4b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo2_4b.Location = new System.Drawing.Point(351, 156);
            this.txtLo2_4b.Name = "txtLo2_4b";
            this.txtLo2_4b.Size = new System.Drawing.Size(18, 18);
            this.txtLo2_4b.TabIndex = 183;
            this.txtLo2_4b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo2_4b, "Amplifer B control code (hex notation).");
            this.txtLo2_4b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_4b
            // 
            this.txtTx_4b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_4b.Location = new System.Drawing.Point(327, 156);
            this.txtTx_4b.Name = "txtTx_4b";
            this.txtTx_4b.Size = new System.Drawing.Size(18, 18);
            this.txtTx_4b.TabIndex = 182;
            this.txtTx_4b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_4b, "TX antenna to use for this band (1-3).");
            this.txtTx_4b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_4b
            // 
            this.txtPwr_4b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_4b.Location = new System.Drawing.Point(267, 156);
            this.txtPwr_4b.Name = "txtPwr_4b";
            this.txtPwr_4b.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_4b.TabIndex = 181;
            this.txtPwr_4b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_4b, "Drive Power Amp B (decimal).");
            this.txtPwr_4b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_4b
            // 
            this.txtRx_4b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_4b.Location = new System.Drawing.Point(303, 156);
            this.txtRx_4b.Name = "txtRx_4b";
            this.txtRx_4b.Size = new System.Drawing.Size(18, 18);
            this.txtRx_4b.TabIndex = 180;
            this.txtRx_4b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_4b, "RX2 antenna to use for this band (1-3).");
            this.txtRx_4b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label192
            // 
            this.label192.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label192.ForeColor = System.Drawing.Color.Navy;
            this.label192.Location = new System.Drawing.Point(233, 139);
            this.label192.Name = "label192";
            this.label192.Size = new System.Drawing.Size(9, 13);
            this.label192.TabIndex = 179;
            this.label192.Text = "-";
            this.label192.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label192.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo2_3b
            // 
            this.txtLo2_3b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo2_3b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo2_3b.Location = new System.Drawing.Point(351, 136);
            this.txtLo2_3b.Name = "txtLo2_3b";
            this.txtLo2_3b.Size = new System.Drawing.Size(18, 18);
            this.txtLo2_3b.TabIndex = 178;
            this.txtLo2_3b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo2_3b, "Amplifer B control code (hex notation).");
            this.txtLo2_3b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_3b
            // 
            this.txtTx_3b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_3b.Location = new System.Drawing.Point(327, 136);
            this.txtTx_3b.Name = "txtTx_3b";
            this.txtTx_3b.Size = new System.Drawing.Size(18, 18);
            this.txtTx_3b.TabIndex = 177;
            this.txtTx_3b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_3b, "TX antenna to use for this band (1-3).");
            this.txtTx_3b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_3b
            // 
            this.txtPwr_3b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_3b.Location = new System.Drawing.Point(267, 136);
            this.txtPwr_3b.Name = "txtPwr_3b";
            this.txtPwr_3b.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_3b.TabIndex = 176;
            this.txtPwr_3b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_3b, "Drive Power Amp B (decimal).");
            this.txtPwr_3b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_3b
            // 
            this.txtRx_3b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_3b.Location = new System.Drawing.Point(303, 136);
            this.txtRx_3b.Name = "txtRx_3b";
            this.txtRx_3b.Size = new System.Drawing.Size(18, 18);
            this.txtRx_3b.TabIndex = 175;
            this.txtRx_3b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_3b, "RX2 antenna to use for this band (1-3).");
            this.txtRx_3b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label191
            // 
            this.label191.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label191.ForeColor = System.Drawing.Color.Navy;
            this.label191.Location = new System.Drawing.Point(233, 119);
            this.label191.Name = "label191";
            this.label191.Size = new System.Drawing.Size(9, 13);
            this.label191.TabIndex = 174;
            this.label191.Text = "-";
            this.label191.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label191.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLo2_2b
            // 
            this.txtLo2_2b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo2_2b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo2_2b.Location = new System.Drawing.Point(351, 116);
            this.txtLo2_2b.Name = "txtLo2_2b";
            this.txtLo2_2b.Size = new System.Drawing.Size(18, 18);
            this.txtLo2_2b.TabIndex = 173;
            this.txtLo2_2b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo2_2b, "Amplifer B control code (hex notation).");
            this.txtLo2_2b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_2b
            // 
            this.txtTx_2b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_2b.Location = new System.Drawing.Point(327, 116);
            this.txtTx_2b.Name = "txtTx_2b";
            this.txtTx_2b.Size = new System.Drawing.Size(18, 18);
            this.txtTx_2b.TabIndex = 172;
            this.txtTx_2b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_2b, "TX antenna to use for this band (1-3).");
            this.txtTx_2b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_2b
            // 
            this.txtPwr_2b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_2b.Location = new System.Drawing.Point(267, 116);
            this.txtPwr_2b.Name = "txtPwr_2b";
            this.txtPwr_2b.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_2b.TabIndex = 171;
            this.txtPwr_2b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_2b, "Drive Power Amp B (decimal).");
            this.txtPwr_2b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_2b
            // 
            this.txtRx_2b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_2b.Location = new System.Drawing.Point(303, 116);
            this.txtRx_2b.Name = "txtRx_2b";
            this.txtRx_2b.Size = new System.Drawing.Size(18, 18);
            this.txtRx_2b.TabIndex = 170;
            this.txtRx_2b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_2b, "RX2 antenna to use for this band (1-3).");
            this.txtRx_2b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label190
            // 
            this.label190.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label190.ForeColor = System.Drawing.Color.Navy;
            this.label190.Location = new System.Drawing.Point(233, 99);
            this.label190.Name = "label190";
            this.label190.Size = new System.Drawing.Size(9, 13);
            this.label190.TabIndex = 169;
            this.label190.Text = "-";
            this.label190.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label190.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label189
            // 
            this.label189.AutoSize = true;
            this.label189.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label189.ForeColor = System.Drawing.Color.Blue;
            this.label189.Location = new System.Drawing.Point(242, 65);
            this.label189.Name = "label189";
            this.label189.Size = new System.Drawing.Size(159, 13);
            this.label189.TabIndex = 168;
            this.label189.Text = "              VFO B               ";
            this.label189.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label185
            // 
            this.label185.AutoSize = true;
            this.label185.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label185.ForeColor = System.Drawing.Color.Blue;
            this.label185.Location = new System.Drawing.Point(350, 81);
            this.label185.Name = "label185";
            this.label185.Size = new System.Drawing.Size(22, 13);
            this.label185.TabIndex = 167;
            this.label185.Text = "Ab";
            // 
            // txtLo2_1b
            // 
            this.txtLo2_1b.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLo2_1b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLo2_1b.Location = new System.Drawing.Point(351, 96);
            this.txtLo2_1b.Name = "txtLo2_1b";
            this.txtLo2_1b.Size = new System.Drawing.Size(18, 18);
            this.txtLo2_1b.TabIndex = 166;
            this.txtLo2_1b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLo2_1b, "Amplifer B control code (hex notation).");
            this.txtLo2_1b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label186
            // 
            this.label186.AutoSize = true;
            this.label186.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label186.ForeColor = System.Drawing.Color.Blue;
            this.label186.Location = new System.Drawing.Point(323, 81);
            this.label186.Name = "label186";
            this.label186.Size = new System.Drawing.Size(23, 13);
            this.label186.TabIndex = 165;
            this.label186.Text = "TX";
            // 
            // txtTx_1b
            // 
            this.txtTx_1b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_1b.Location = new System.Drawing.Point(327, 96);
            this.txtTx_1b.Name = "txtTx_1b";
            this.txtTx_1b.Size = new System.Drawing.Size(18, 18);
            this.txtTx_1b.TabIndex = 164;
            this.txtTx_1b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_1b, "TX antenna to use for this band (1-3).");
            this.txtTx_1b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label187
            // 
            this.label187.AutoSize = true;
            this.label187.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label187.ForeColor = System.Drawing.Color.Blue;
            this.label187.Location = new System.Drawing.Point(300, 81);
            this.label187.Name = "label187";
            this.label187.Size = new System.Drawing.Size(23, 13);
            this.label187.TabIndex = 163;
            this.label187.Text = "R2";
            // 
            // label188
            // 
            this.label188.AutoSize = true;
            this.label188.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label188.ForeColor = System.Drawing.Color.Blue;
            this.label188.Location = new System.Drawing.Point(268, 80);
            this.label188.Name = "label188";
            this.label188.Size = new System.Drawing.Size(27, 13);
            this.label188.TabIndex = 162;
            this.label188.Text = "Drv";
            // 
            // txtPwr_1b
            // 
            this.txtPwr_1b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_1b.Location = new System.Drawing.Point(267, 96);
            this.txtPwr_1b.Name = "txtPwr_1b";
            this.txtPwr_1b.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_1b.TabIndex = 161;
            this.txtPwr_1b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_1b, "Drive Power Amp B (decimal).");
            this.txtPwr_1b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_1b
            // 
            this.txtRx_1b.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_1b.Location = new System.Drawing.Point(303, 96);
            this.txtRx_1b.Name = "txtRx_1b";
            this.txtRx_1b.Size = new System.Drawing.Size(18, 18);
            this.txtRx_1b.TabIndex = 160;
            this.txtRx_1b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_1b, "RX2 antenna to use for this band (1-3).");
            this.txtRx_1b.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label183
            // 
            this.label183.AutoSize = true;
            this.label183.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label183.ForeColor = System.Drawing.Color.Firebrick;
            this.label183.Location = new System.Drawing.Point(24, 65);
            this.label183.Name = "label183";
            this.label183.Size = new System.Drawing.Size(207, 13);
            this.label183.TabIndex = 158;
            this.label183.Text = "                    VFO A                     ";
            this.label183.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtHi2_10a
            // 
            this.txtHi2_10a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi2_10a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi2_10a.Location = new System.Drawing.Point(211, 276);
            this.txtHi2_10a.Name = "txtHi2_10a";
            this.txtHi2_10a.Size = new System.Drawing.Size(18, 18);
            this.txtHi2_10a.TabIndex = 157;
            this.txtHi2_10a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi2_10a, "Amplifer A control code (hex notation)");
            this.txtHi2_10a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_10a
            // 
            this.txtTx_10a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_10a.Location = new System.Drawing.Point(187, 276);
            this.txtTx_10a.Name = "txtTx_10a";
            this.txtTx_10a.Size = new System.Drawing.Size(18, 18);
            this.txtTx_10a.TabIndex = 156;
            this.txtTx_10a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_10a, "TX antenna to use for this band (1-3).");
            this.txtTx_10a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_10a
            // 
            this.txtPwr_10a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_10a.Location = new System.Drawing.Point(127, 276);
            this.txtPwr_10a.Name = "txtPwr_10a";
            this.txtPwr_10a.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_10a.TabIndex = 155;
            this.txtPwr_10a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_10a, "Drive power (decimal).");
            this.txtPwr_10a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_10a
            // 
            this.txtRx_10a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_10a.Location = new System.Drawing.Point(163, 276);
            this.txtRx_10a.Name = "txtRx_10a";
            this.txtRx_10a.Size = new System.Drawing.Size(18, 18);
            this.txtRx_10a.TabIndex = 154;
            this.txtRx_10a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_10a, "RX1 antenna to use for this band (1-3).");
            this.txtRx_10a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtHi1_10a
            // 
            this.txtHi1_10a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi1_10a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi1_10a.Location = new System.Drawing.Point(103, 276);
            this.txtHi1_10a.Name = "txtHi1_10a";
            this.txtHi1_10a.Size = new System.Drawing.Size(18, 18);
            this.txtHi1_10a.TabIndex = 153;
            this.txtHi1_10a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi1_10a, "Band data (Hex notation).");
            this.txtHi1_10a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLPT1_10
            // 
            this.txtLPT1_10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLPT1_10.Location = new System.Drawing.Point(55, 276);
            this.txtLPT1_10.Name = "txtLPT1_10";
            this.txtLPT1_10.Size = new System.Drawing.Size(42, 18);
            this.txtLPT1_10.TabIndex = 152;
            this.txtLPT1_10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLPT1_10, "LPT port number (decimal) to use for VFO A band and data control.");
            this.txtLPT1_10.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label179
            // 
            this.label179.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label179.ForeColor = System.Drawing.Color.Navy;
            this.label179.Location = new System.Drawing.Point(2, 278);
            this.label179.Name = "label179";
            this.label179.Size = new System.Drawing.Size(25, 13);
            this.label179.TabIndex = 151;
            this.label179.Text = "6";
            this.label179.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkSO10a
            // 
            this.chkSO10a.AutoSize = true;
            this.chkSO10a.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO10a.Location = new System.Drawing.Point(34, 279);
            this.chkSO10a.Name = "chkSO10a";
            this.chkSO10a.Size = new System.Drawing.Size(15, 14);
            this.chkSO10a.TabIndex = 150;
            this.toolTip1.SetToolTip(this.chkSO10a, "Check to enable the use of this band for VFO A");
            this.chkSO10a.UseVisualStyleBackColor = true;
            this.chkSO10a.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // txtHi2_9a
            // 
            this.txtHi2_9a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi2_9a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi2_9a.Location = new System.Drawing.Point(211, 256);
            this.txtHi2_9a.Name = "txtHi2_9a";
            this.txtHi2_9a.Size = new System.Drawing.Size(18, 18);
            this.txtHi2_9a.TabIndex = 149;
            this.txtHi2_9a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi2_9a, "Amplifer A control code (hex notation)");
            this.txtHi2_9a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_9a
            // 
            this.txtTx_9a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_9a.Location = new System.Drawing.Point(187, 256);
            this.txtTx_9a.Name = "txtTx_9a";
            this.txtTx_9a.Size = new System.Drawing.Size(18, 18);
            this.txtTx_9a.TabIndex = 148;
            this.txtTx_9a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_9a, "TX antenna to use for this band (1-3).");
            this.txtTx_9a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_9a
            // 
            this.txtPwr_9a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_9a.Location = new System.Drawing.Point(127, 256);
            this.txtPwr_9a.Name = "txtPwr_9a";
            this.txtPwr_9a.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_9a.TabIndex = 147;
            this.txtPwr_9a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_9a, "Drive power (decimal).");
            this.txtPwr_9a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_9a
            // 
            this.txtRx_9a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_9a.Location = new System.Drawing.Point(163, 256);
            this.txtRx_9a.Name = "txtRx_9a";
            this.txtRx_9a.Size = new System.Drawing.Size(18, 18);
            this.txtRx_9a.TabIndex = 146;
            this.txtRx_9a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_9a, "RX1 antenna to use for this band (1-3).");
            this.txtRx_9a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtHi1_9a
            // 
            this.txtHi1_9a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi1_9a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi1_9a.Location = new System.Drawing.Point(103, 256);
            this.txtHi1_9a.Name = "txtHi1_9a";
            this.txtHi1_9a.Size = new System.Drawing.Size(18, 18);
            this.txtHi1_9a.TabIndex = 145;
            this.txtHi1_9a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi1_9a, "Band data (Hex notation).");
            this.txtHi1_9a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLPT1_9
            // 
            this.txtLPT1_9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLPT1_9.Location = new System.Drawing.Point(55, 256);
            this.txtLPT1_9.Name = "txtLPT1_9";
            this.txtLPT1_9.Size = new System.Drawing.Size(42, 18);
            this.txtLPT1_9.TabIndex = 144;
            this.txtLPT1_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLPT1_9, "LPT port number (decimal) to use for VFO A band and data control.");
            this.txtLPT1_9.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label180
            // 
            this.label180.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label180.ForeColor = System.Drawing.Color.Navy;
            this.label180.Location = new System.Drawing.Point(2, 258);
            this.label180.Name = "label180";
            this.label180.Size = new System.Drawing.Size(25, 13);
            this.label180.TabIndex = 143;
            this.label180.Text = "10";
            this.label180.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkSO9a
            // 
            this.chkSO9a.AutoSize = true;
            this.chkSO9a.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO9a.Location = new System.Drawing.Point(34, 259);
            this.chkSO9a.Name = "chkSO9a";
            this.chkSO9a.Size = new System.Drawing.Size(15, 14);
            this.chkSO9a.TabIndex = 142;
            this.toolTip1.SetToolTip(this.chkSO9a, "Check to enable the use of this band for VFO A");
            this.chkSO9a.UseVisualStyleBackColor = true;
            this.chkSO9a.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // txtHi2_8a
            // 
            this.txtHi2_8a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi2_8a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi2_8a.Location = new System.Drawing.Point(211, 236);
            this.txtHi2_8a.Name = "txtHi2_8a";
            this.txtHi2_8a.Size = new System.Drawing.Size(18, 18);
            this.txtHi2_8a.TabIndex = 141;
            this.txtHi2_8a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi2_8a, "Amplifer A control code (hex notation)");
            this.txtHi2_8a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_8a
            // 
            this.txtTx_8a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_8a.Location = new System.Drawing.Point(187, 236);
            this.txtTx_8a.Name = "txtTx_8a";
            this.txtTx_8a.Size = new System.Drawing.Size(18, 18);
            this.txtTx_8a.TabIndex = 140;
            this.txtTx_8a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_8a, "TX antenna to use for this band (1-3).");
            this.txtTx_8a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_8a
            // 
            this.txtPwr_8a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_8a.Location = new System.Drawing.Point(127, 236);
            this.txtPwr_8a.Name = "txtPwr_8a";
            this.txtPwr_8a.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_8a.TabIndex = 139;
            this.txtPwr_8a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_8a, "Drive power (decimal).");
            this.txtPwr_8a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_8a
            // 
            this.txtRx_8a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_8a.Location = new System.Drawing.Point(163, 236);
            this.txtRx_8a.Name = "txtRx_8a";
            this.txtRx_8a.Size = new System.Drawing.Size(18, 18);
            this.txtRx_8a.TabIndex = 138;
            this.txtRx_8a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_8a, "RX1 antenna to use for this band (1-3).");
            this.txtRx_8a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtHi1_8a
            // 
            this.txtHi1_8a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi1_8a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi1_8a.Location = new System.Drawing.Point(103, 236);
            this.txtHi1_8a.Name = "txtHi1_8a";
            this.txtHi1_8a.Size = new System.Drawing.Size(18, 18);
            this.txtHi1_8a.TabIndex = 137;
            this.txtHi1_8a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi1_8a, "Band data (Hex notation).");
            this.txtHi1_8a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLPT1_8
            // 
            this.txtLPT1_8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLPT1_8.Location = new System.Drawing.Point(55, 236);
            this.txtLPT1_8.Name = "txtLPT1_8";
            this.txtLPT1_8.Size = new System.Drawing.Size(42, 18);
            this.txtLPT1_8.TabIndex = 136;
            this.txtLPT1_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLPT1_8, "LPT port number (decimal) to use for VFO A band and data control.");
            this.txtLPT1_8.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label181
            // 
            this.label181.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label181.ForeColor = System.Drawing.Color.Navy;
            this.label181.Location = new System.Drawing.Point(2, 238);
            this.label181.Name = "label181";
            this.label181.Size = new System.Drawing.Size(25, 13);
            this.label181.TabIndex = 135;
            this.label181.Text = "12";
            this.label181.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkSO8a
            // 
            this.chkSO8a.AutoSize = true;
            this.chkSO8a.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO8a.Location = new System.Drawing.Point(34, 239);
            this.chkSO8a.Name = "chkSO8a";
            this.chkSO8a.Size = new System.Drawing.Size(15, 14);
            this.chkSO8a.TabIndex = 134;
            this.toolTip1.SetToolTip(this.chkSO8a, "Check to enable the use of this band for VFO A");
            this.chkSO8a.UseVisualStyleBackColor = true;
            this.chkSO8a.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // txtHi2_7a
            // 
            this.txtHi2_7a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi2_7a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi2_7a.Location = new System.Drawing.Point(211, 216);
            this.txtHi2_7a.Name = "txtHi2_7a";
            this.txtHi2_7a.Size = new System.Drawing.Size(18, 18);
            this.txtHi2_7a.TabIndex = 133;
            this.txtHi2_7a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi2_7a, "Amplifer A control code (hex notation)");
            this.txtHi2_7a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_7a
            // 
            this.txtTx_7a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_7a.Location = new System.Drawing.Point(187, 216);
            this.txtTx_7a.Name = "txtTx_7a";
            this.txtTx_7a.Size = new System.Drawing.Size(18, 18);
            this.txtTx_7a.TabIndex = 132;
            this.txtTx_7a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_7a, "TX antenna to use for this band (1-3).");
            this.txtTx_7a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_7a
            // 
            this.txtPwr_7a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_7a.Location = new System.Drawing.Point(127, 216);
            this.txtPwr_7a.Name = "txtPwr_7a";
            this.txtPwr_7a.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_7a.TabIndex = 131;
            this.txtPwr_7a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_7a, "Drive power (decimal).");
            this.txtPwr_7a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_7a
            // 
            this.txtRx_7a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_7a.Location = new System.Drawing.Point(163, 216);
            this.txtRx_7a.Name = "txtRx_7a";
            this.txtRx_7a.Size = new System.Drawing.Size(18, 18);
            this.txtRx_7a.TabIndex = 130;
            this.txtRx_7a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_7a, "RX1 antenna to use for this band (1-3).");
            this.txtRx_7a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtHi1_7a
            // 
            this.txtHi1_7a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi1_7a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi1_7a.Location = new System.Drawing.Point(103, 216);
            this.txtHi1_7a.Name = "txtHi1_7a";
            this.txtHi1_7a.Size = new System.Drawing.Size(18, 18);
            this.txtHi1_7a.TabIndex = 129;
            this.txtHi1_7a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi1_7a, "Band data (Hex notation).");
            this.txtHi1_7a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLPT1_7
            // 
            this.txtLPT1_7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLPT1_7.Location = new System.Drawing.Point(55, 216);
            this.txtLPT1_7.Name = "txtLPT1_7";
            this.txtLPT1_7.Size = new System.Drawing.Size(42, 18);
            this.txtLPT1_7.TabIndex = 128;
            this.txtLPT1_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLPT1_7, "LPT port number (decimal) to use for VFO A band and data control.");
            this.txtLPT1_7.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label182
            // 
            this.label182.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label182.ForeColor = System.Drawing.Color.Navy;
            this.label182.Location = new System.Drawing.Point(2, 218);
            this.label182.Name = "label182";
            this.label182.Size = new System.Drawing.Size(25, 13);
            this.label182.TabIndex = 127;
            this.label182.Text = "15";
            this.label182.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkSO7a
            // 
            this.chkSO7a.AutoSize = true;
            this.chkSO7a.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO7a.Location = new System.Drawing.Point(34, 219);
            this.chkSO7a.Name = "chkSO7a";
            this.chkSO7a.Size = new System.Drawing.Size(15, 14);
            this.chkSO7a.TabIndex = 126;
            this.toolTip1.SetToolTip(this.chkSO7a, "Check to enable the use of this band for VFO A");
            this.chkSO7a.UseVisualStyleBackColor = true;
            this.chkSO7a.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // txtHi2_6a
            // 
            this.txtHi2_6a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi2_6a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi2_6a.Location = new System.Drawing.Point(211, 196);
            this.txtHi2_6a.Name = "txtHi2_6a";
            this.txtHi2_6a.Size = new System.Drawing.Size(18, 18);
            this.txtHi2_6a.TabIndex = 125;
            this.txtHi2_6a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi2_6a, "Amplifer A control code (hex notation)");
            this.txtHi2_6a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_6a
            // 
            this.txtTx_6a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_6a.Location = new System.Drawing.Point(187, 196);
            this.txtTx_6a.Name = "txtTx_6a";
            this.txtTx_6a.Size = new System.Drawing.Size(18, 18);
            this.txtTx_6a.TabIndex = 124;
            this.txtTx_6a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_6a, "TX antenna to use for this band (1-3).");
            this.txtTx_6a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_6a
            // 
            this.txtPwr_6a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_6a.Location = new System.Drawing.Point(127, 196);
            this.txtPwr_6a.Name = "txtPwr_6a";
            this.txtPwr_6a.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_6a.TabIndex = 123;
            this.txtPwr_6a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_6a, "Drive power (decimal).");
            this.txtPwr_6a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_6a
            // 
            this.txtRx_6a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_6a.Location = new System.Drawing.Point(163, 196);
            this.txtRx_6a.Name = "txtRx_6a";
            this.txtRx_6a.Size = new System.Drawing.Size(18, 18);
            this.txtRx_6a.TabIndex = 122;
            this.txtRx_6a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_6a, "RX1 antenna to use for this band (1-3).");
            this.txtRx_6a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtHi1_6a
            // 
            this.txtHi1_6a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi1_6a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi1_6a.Location = new System.Drawing.Point(103, 196);
            this.txtHi1_6a.Name = "txtHi1_6a";
            this.txtHi1_6a.Size = new System.Drawing.Size(18, 18);
            this.txtHi1_6a.TabIndex = 121;
            this.txtHi1_6a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi1_6a, "Band data (Hex notation).");
            this.txtHi1_6a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLPT1_6
            // 
            this.txtLPT1_6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLPT1_6.Location = new System.Drawing.Point(55, 196);
            this.txtLPT1_6.Name = "txtLPT1_6";
            this.txtLPT1_6.Size = new System.Drawing.Size(42, 18);
            this.txtLPT1_6.TabIndex = 120;
            this.txtLPT1_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLPT1_6, "LPT port number (decimal) to use for VFO A band and data control.");
            this.txtLPT1_6.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label178
            // 
            this.label178.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label178.ForeColor = System.Drawing.Color.Navy;
            this.label178.Location = new System.Drawing.Point(2, 198);
            this.label178.Name = "label178";
            this.label178.Size = new System.Drawing.Size(25, 13);
            this.label178.TabIndex = 119;
            this.label178.Text = "17";
            this.label178.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkSO6a
            // 
            this.chkSO6a.AutoSize = true;
            this.chkSO6a.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO6a.Location = new System.Drawing.Point(34, 199);
            this.chkSO6a.Name = "chkSO6a";
            this.chkSO6a.Size = new System.Drawing.Size(15, 14);
            this.chkSO6a.TabIndex = 118;
            this.toolTip1.SetToolTip(this.chkSO6a, "Check to enable the use of this band for VFO A");
            this.chkSO6a.UseVisualStyleBackColor = true;
            this.chkSO6a.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // txtTx_5a
            // 
            this.txtTx_5a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_5a.Location = new System.Drawing.Point(187, 176);
            this.txtTx_5a.Name = "txtTx_5a";
            this.txtTx_5a.Size = new System.Drawing.Size(18, 18);
            this.txtTx_5a.TabIndex = 116;
            this.txtTx_5a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_5a, "TX antenna to use for this band (1-3).");
            this.txtTx_5a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_5a
            // 
            this.txtPwr_5a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_5a.Location = new System.Drawing.Point(127, 176);
            this.txtPwr_5a.Name = "txtPwr_5a";
            this.txtPwr_5a.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_5a.TabIndex = 115;
            this.txtPwr_5a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_5a, "Drive power (decimal).");
            this.txtPwr_5a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_5a
            // 
            this.txtRx_5a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_5a.Location = new System.Drawing.Point(163, 176);
            this.txtRx_5a.Name = "txtRx_5a";
            this.txtRx_5a.Size = new System.Drawing.Size(18, 18);
            this.txtRx_5a.TabIndex = 114;
            this.txtRx_5a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_5a, "RX1 antenna to use for this band (1-3).");
            this.txtRx_5a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtHi1_5a
            // 
            this.txtHi1_5a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi1_5a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi1_5a.Location = new System.Drawing.Point(103, 176);
            this.txtHi1_5a.Name = "txtHi1_5a";
            this.txtHi1_5a.Size = new System.Drawing.Size(18, 18);
            this.txtHi1_5a.TabIndex = 113;
            this.txtHi1_5a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi1_5a, "Band data (Hex notation).");
            this.txtHi1_5a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLPT1_5
            // 
            this.txtLPT1_5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLPT1_5.Location = new System.Drawing.Point(55, 176);
            this.txtLPT1_5.Name = "txtLPT1_5";
            this.txtLPT1_5.Size = new System.Drawing.Size(42, 18);
            this.txtLPT1_5.TabIndex = 112;
            this.txtLPT1_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLPT1_5, "LPT port number (decimal) to use for VFO A band and data control.");
            this.txtLPT1_5.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label177
            // 
            this.label177.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label177.ForeColor = System.Drawing.Color.Navy;
            this.label177.Location = new System.Drawing.Point(2, 178);
            this.label177.Name = "label177";
            this.label177.Size = new System.Drawing.Size(25, 13);
            this.label177.TabIndex = 111;
            this.label177.Text = "20";
            this.label177.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkSO5a
            // 
            this.chkSO5a.AutoSize = true;
            this.chkSO5a.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO5a.Location = new System.Drawing.Point(34, 179);
            this.chkSO5a.Name = "chkSO5a";
            this.chkSO5a.Size = new System.Drawing.Size(15, 14);
            this.chkSO5a.TabIndex = 110;
            this.toolTip1.SetToolTip(this.chkSO5a, "Check to enable the use of this band for VFO A");
            this.chkSO5a.UseVisualStyleBackColor = true;
            this.chkSO5a.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // txtHi2_4a
            // 
            this.txtHi2_4a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi2_4a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi2_4a.Location = new System.Drawing.Point(211, 156);
            this.txtHi2_4a.Name = "txtHi2_4a";
            this.txtHi2_4a.Size = new System.Drawing.Size(18, 18);
            this.txtHi2_4a.TabIndex = 109;
            this.txtHi2_4a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi2_4a, "Amplifer A control code (hex notation)");
            this.txtHi2_4a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_4a
            // 
            this.txtTx_4a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_4a.Location = new System.Drawing.Point(187, 156);
            this.txtTx_4a.Name = "txtTx_4a";
            this.txtTx_4a.Size = new System.Drawing.Size(18, 18);
            this.txtTx_4a.TabIndex = 108;
            this.txtTx_4a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_4a, "TX antenna to use for this band (1-3).");
            this.txtTx_4a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_4a
            // 
            this.txtPwr_4a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_4a.Location = new System.Drawing.Point(127, 156);
            this.txtPwr_4a.Name = "txtPwr_4a";
            this.txtPwr_4a.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_4a.TabIndex = 107;
            this.txtPwr_4a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_4a, "Drive power (decimal).");
            this.txtPwr_4a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_4a
            // 
            this.txtRx_4a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_4a.Location = new System.Drawing.Point(163, 156);
            this.txtRx_4a.Name = "txtRx_4a";
            this.txtRx_4a.Size = new System.Drawing.Size(18, 18);
            this.txtRx_4a.TabIndex = 106;
            this.txtRx_4a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_4a, "RX1 antenna to use for this band (1-3).");
            this.txtRx_4a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtHi1_4a
            // 
            this.txtHi1_4a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi1_4a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi1_4a.Location = new System.Drawing.Point(103, 156);
            this.txtHi1_4a.Name = "txtHi1_4a";
            this.txtHi1_4a.Size = new System.Drawing.Size(18, 18);
            this.txtHi1_4a.TabIndex = 105;
            this.txtHi1_4a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi1_4a, "Band data (Hex notation).");
            this.txtHi1_4a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLPT1_4
            // 
            this.txtLPT1_4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLPT1_4.Location = new System.Drawing.Point(55, 156);
            this.txtLPT1_4.Name = "txtLPT1_4";
            this.txtLPT1_4.Size = new System.Drawing.Size(42, 18);
            this.txtLPT1_4.TabIndex = 104;
            this.txtLPT1_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLPT1_4, "LPT port number (decimal) to use for VFO A band and data control.");
            this.txtLPT1_4.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label176
            // 
            this.label176.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label176.ForeColor = System.Drawing.Color.Navy;
            this.label176.Location = new System.Drawing.Point(2, 158);
            this.label176.Name = "label176";
            this.label176.Size = new System.Drawing.Size(25, 13);
            this.label176.TabIndex = 103;
            this.label176.Text = "30";
            this.label176.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkSO4a
            // 
            this.chkSO4a.AutoSize = true;
            this.chkSO4a.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO4a.Location = new System.Drawing.Point(34, 159);
            this.chkSO4a.Name = "chkSO4a";
            this.chkSO4a.Size = new System.Drawing.Size(15, 14);
            this.chkSO4a.TabIndex = 102;
            this.toolTip1.SetToolTip(this.chkSO4a, "Check to enable the use of this band for VFO A");
            this.chkSO4a.UseVisualStyleBackColor = true;
            this.chkSO4a.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // txtHi2_3a
            // 
            this.txtHi2_3a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi2_3a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi2_3a.Location = new System.Drawing.Point(211, 136);
            this.txtHi2_3a.Name = "txtHi2_3a";
            this.txtHi2_3a.Size = new System.Drawing.Size(18, 18);
            this.txtHi2_3a.TabIndex = 101;
            this.txtHi2_3a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi2_3a, "Amplifer A control code (hex notation)");
            this.txtHi2_3a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_3a
            // 
            this.txtTx_3a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_3a.Location = new System.Drawing.Point(187, 136);
            this.txtTx_3a.Name = "txtTx_3a";
            this.txtTx_3a.Size = new System.Drawing.Size(18, 18);
            this.txtTx_3a.TabIndex = 100;
            this.txtTx_3a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_3a, "TX antenna to use for this band (1-3).");
            this.txtTx_3a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_3a
            // 
            this.txtPwr_3a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_3a.Location = new System.Drawing.Point(127, 136);
            this.txtPwr_3a.Name = "txtPwr_3a";
            this.txtPwr_3a.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_3a.TabIndex = 99;
            this.txtPwr_3a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_3a, "Drive power (decimal).");
            this.txtPwr_3a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_3a
            // 
            this.txtRx_3a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_3a.Location = new System.Drawing.Point(163, 136);
            this.txtRx_3a.Name = "txtRx_3a";
            this.txtRx_3a.Size = new System.Drawing.Size(18, 18);
            this.txtRx_3a.TabIndex = 98;
            this.txtRx_3a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_3a, "RX1 antenna to use for this band (1-3).");
            this.txtRx_3a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtHi1_3a
            // 
            this.txtHi1_3a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi1_3a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi1_3a.Location = new System.Drawing.Point(103, 136);
            this.txtHi1_3a.Name = "txtHi1_3a";
            this.txtHi1_3a.Size = new System.Drawing.Size(18, 18);
            this.txtHi1_3a.TabIndex = 97;
            this.txtHi1_3a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi1_3a, "Band data (Hex notation).");
            this.txtHi1_3a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLPT1_3
            // 
            this.txtLPT1_3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLPT1_3.Location = new System.Drawing.Point(55, 136);
            this.txtLPT1_3.Name = "txtLPT1_3";
            this.txtLPT1_3.Size = new System.Drawing.Size(42, 18);
            this.txtLPT1_3.TabIndex = 96;
            this.txtLPT1_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLPT1_3, "LPT port number (decimal) to use for VFO A band and data control.");
            this.txtLPT1_3.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label175
            // 
            this.label175.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label175.ForeColor = System.Drawing.Color.Navy;
            this.label175.Location = new System.Drawing.Point(2, 138);
            this.label175.Name = "label175";
            this.label175.Size = new System.Drawing.Size(25, 13);
            this.label175.TabIndex = 95;
            this.label175.Text = "40";
            this.label175.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkSO3a
            // 
            this.chkSO3a.AutoSize = true;
            this.chkSO3a.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO3a.Location = new System.Drawing.Point(34, 139);
            this.chkSO3a.Name = "chkSO3a";
            this.chkSO3a.Size = new System.Drawing.Size(15, 14);
            this.chkSO3a.TabIndex = 94;
            this.toolTip1.SetToolTip(this.chkSO3a, "Check to enable the use of this band for VFO A");
            this.chkSO3a.UseVisualStyleBackColor = true;
            this.chkSO3a.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // txtHi2_2a
            // 
            this.txtHi2_2a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi2_2a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi2_2a.Location = new System.Drawing.Point(211, 116);
            this.txtHi2_2a.Name = "txtHi2_2a";
            this.txtHi2_2a.Size = new System.Drawing.Size(18, 18);
            this.txtHi2_2a.TabIndex = 93;
            this.txtHi2_2a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi2_2a, "Amplifer A control code (hex notation)");
            this.txtHi2_2a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtTx_2a
            // 
            this.txtTx_2a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_2a.Location = new System.Drawing.Point(187, 116);
            this.txtTx_2a.Name = "txtTx_2a";
            this.txtTx_2a.Size = new System.Drawing.Size(18, 18);
            this.txtTx_2a.TabIndex = 92;
            this.txtTx_2a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_2a, "TX antenna to use for this band (1-3).");
            this.txtTx_2a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtPwr_2a
            // 
            this.txtPwr_2a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_2a.Location = new System.Drawing.Point(127, 116);
            this.txtPwr_2a.Name = "txtPwr_2a";
            this.txtPwr_2a.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_2a.TabIndex = 91;
            this.txtPwr_2a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_2a, "Drive power (decimal).");
            this.txtPwr_2a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_2a
            // 
            this.txtRx_2a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_2a.Location = new System.Drawing.Point(163, 116);
            this.txtRx_2a.Name = "txtRx_2a";
            this.txtRx_2a.Size = new System.Drawing.Size(18, 18);
            this.txtRx_2a.TabIndex = 90;
            this.txtRx_2a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_2a, "RX1 antenna to use for this band (1-3).");
            this.txtRx_2a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtHi1_2a
            // 
            this.txtHi1_2a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi1_2a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi1_2a.Location = new System.Drawing.Point(103, 116);
            this.txtHi1_2a.Name = "txtHi1_2a";
            this.txtHi1_2a.Size = new System.Drawing.Size(18, 18);
            this.txtHi1_2a.TabIndex = 89;
            this.txtHi1_2a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi1_2a, "Band data (Hex notation).");
            this.txtHi1_2a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtLPT1_2
            // 
            this.txtLPT1_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLPT1_2.Location = new System.Drawing.Point(55, 116);
            this.txtLPT1_2.Name = "txtLPT1_2";
            this.txtLPT1_2.Size = new System.Drawing.Size(42, 18);
            this.txtLPT1_2.TabIndex = 88;
            this.txtLPT1_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLPT1_2, "LPT port number (decimal) to use for VFO A band and data control.");
            this.txtLPT1_2.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label168
            // 
            this.label168.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label168.ForeColor = System.Drawing.Color.Navy;
            this.label168.Location = new System.Drawing.Point(2, 118);
            this.label168.Name = "label168";
            this.label168.Size = new System.Drawing.Size(25, 13);
            this.label168.TabIndex = 87;
            this.label168.Text = "80";
            this.label168.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkSO2a
            // 
            this.chkSO2a.AutoSize = true;
            this.chkSO2a.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO2a.Location = new System.Drawing.Point(34, 119);
            this.chkSO2a.Name = "chkSO2a";
            this.chkSO2a.Size = new System.Drawing.Size(15, 14);
            this.chkSO2a.TabIndex = 86;
            this.toolTip1.SetToolTip(this.chkSO2a, "Check to enable the use of this band for VFO A");
            this.chkSO2a.UseVisualStyleBackColor = true;
            this.chkSO2a.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // label174
            // 
            this.label174.AutoSize = true;
            this.label174.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label174.ForeColor = System.Drawing.Color.Firebrick;
            this.label174.Location = new System.Drawing.Point(209, 80);
            this.label174.Name = "label174";
            this.label174.Size = new System.Drawing.Size(22, 13);
            this.label174.TabIndex = 85;
            this.label174.Text = "Aa";
            // 
            // txtHi2_1a
            // 
            this.txtHi2_1a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi2_1a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi2_1a.Location = new System.Drawing.Point(211, 96);
            this.txtHi2_1a.Name = "txtHi2_1a";
            this.txtHi2_1a.Size = new System.Drawing.Size(18, 18);
            this.txtHi2_1a.TabIndex = 84;
            this.txtHi2_1a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi2_1a, "Amplifer A control code (hex notation)");
            this.txtHi2_1a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label173
            // 
            this.label173.AutoSize = true;
            this.label173.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label173.ForeColor = System.Drawing.Color.Firebrick;
            this.label173.Location = new System.Drawing.Point(183, 80);
            this.label173.Name = "label173";
            this.label173.Size = new System.Drawing.Size(23, 13);
            this.label173.TabIndex = 83;
            this.label173.Text = "TX";
            // 
            // txtTx_1a
            // 
            this.txtTx_1a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtTx_1a.Location = new System.Drawing.Point(187, 96);
            this.txtTx_1a.Name = "txtTx_1a";
            this.txtTx_1a.Size = new System.Drawing.Size(18, 18);
            this.txtTx_1a.TabIndex = 82;
            this.txtTx_1a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTx_1a, "TX antenna to use for this band (1-3).");
            this.txtTx_1a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label172
            // 
            this.label172.AutoSize = true;
            this.label172.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label172.ForeColor = System.Drawing.Color.Firebrick;
            this.label172.Location = new System.Drawing.Point(161, 80);
            this.label172.Name = "label172";
            this.label172.Size = new System.Drawing.Size(23, 13);
            this.label172.TabIndex = 81;
            this.label172.Text = "R1";
            // 
            // label170
            // 
            this.label170.AutoSize = true;
            this.label170.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label170.ForeColor = System.Drawing.Color.Firebrick;
            this.label170.Location = new System.Drawing.Point(129, 79);
            this.label170.Name = "label170";
            this.label170.Size = new System.Drawing.Size(27, 13);
            this.label170.TabIndex = 80;
            this.label170.Text = "Drv";
            // 
            // txtPwr_1a
            // 
            this.txtPwr_1a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtPwr_1a.Location = new System.Drawing.Point(127, 96);
            this.txtPwr_1a.Name = "txtPwr_1a";
            this.txtPwr_1a.Size = new System.Drawing.Size(30, 18);
            this.txtPwr_1a.TabIndex = 79;
            this.txtPwr_1a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPwr_1a, "Drive Power Amp A (decimal).");
            this.txtPwr_1a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtRx_1a
            // 
            this.txtRx_1a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtRx_1a.Location = new System.Drawing.Point(163, 96);
            this.txtRx_1a.Name = "txtRx_1a";
            this.txtRx_1a.Size = new System.Drawing.Size(18, 18);
            this.txtRx_1a.TabIndex = 78;
            this.txtRx_1a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRx_1a, "RX1 antenna to use for this band (1-3).");
            this.txtRx_1a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // txtHi1_1a
            // 
            this.txtHi1_1a.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHi1_1a.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtHi1_1a.Location = new System.Drawing.Point(103, 96);
            this.txtHi1_1a.Name = "txtHi1_1a";
            this.txtHi1_1a.Size = new System.Drawing.Size(18, 18);
            this.txtHi1_1a.TabIndex = 77;
            this.txtHi1_1a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtHi1_1a, "Band data high 4 bits (Hex notation).");
            this.txtHi1_1a.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label167
            // 
            this.label167.AutoSize = true;
            this.label167.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label167.ForeColor = System.Drawing.Color.Firebrick;
            this.label167.Location = new System.Drawing.Point(56, 79);
            this.label167.Name = "label167";
            this.label167.Size = new System.Drawing.Size(41, 13);
            this.label167.TabIndex = 75;
            this.label167.Text = "LPT 1";
            // 
            // txtLPT1_1
            // 
            this.txtLPT1_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLPT1_1.Location = new System.Drawing.Point(55, 96);
            this.txtLPT1_1.Name = "txtLPT1_1";
            this.txtLPT1_1.Size = new System.Drawing.Size(42, 18);
            this.txtLPT1_1.TabIndex = 74;
            this.txtLPT1_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtLPT1_1, "LPT port number (decimal) to use for VFO A band and data control.");
            this.txtLPT1_1.TextChanged += new System.EventHandler(this.grpSo2R_TextChanged);
            // 
            // label166
            // 
            this.label166.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label166.ForeColor = System.Drawing.Color.Navy;
            this.label166.Location = new System.Drawing.Point(0, 98);
            this.label166.Name = "label166";
            this.label166.Size = new System.Drawing.Size(28, 13);
            this.label166.TabIndex = 73;
            this.label166.Text = "160";
            // 
            // label169
            // 
            this.label169.AutoSize = true;
            this.label169.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label169.ForeColor = System.Drawing.Color.Firebrick;
            this.label169.Location = new System.Drawing.Point(101, 79);
            this.label169.Name = "label169";
            this.label169.Size = new System.Drawing.Size(23, 13);
            this.label169.TabIndex = 71;
            this.label169.Text = "Da";
            this.label169.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // chkSO1a
            // 
            this.chkSO1a.AutoSize = true;
            this.chkSO1a.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkSO1a.Location = new System.Drawing.Point(34, 99);
            this.chkSO1a.Name = "chkSO1a";
            this.chkSO1a.Size = new System.Drawing.Size(15, 14);
            this.chkSO1a.TabIndex = 48;
            this.toolTip1.SetToolTip(this.chkSO1a, "Check to enable the use of this band for VFO A.");
            this.chkSO1a.UseVisualStyleBackColor = true;
            this.chkSO1a.CheckedChanged += new System.EventHandler(this.SO2Rband_CheckedChanged);
            // 
            // label171
            // 
            this.label171.AutoSize = true;
            this.label171.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label171.ForeColor = System.Drawing.Color.Firebrick;
            this.label171.Location = new System.Drawing.Point(25, 79);
            this.label171.Name = "label171";
            this.label171.Size = new System.Drawing.Size(29, 13);
            this.label171.TabIndex = 49;
            this.label171.Text = "Use";
            // 
            // lblBCD
            // 
            this.lblBCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBCD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBCD.ForeColor = System.Drawing.Color.Red;
            this.lblBCD.Location = new System.Drawing.Point(245, 367);
            this.lblBCD.Name = "lblBCD";
            this.lblBCD.Size = new System.Drawing.Size(30, 17);
            this.lblBCD.TabIndex = 26;
            this.lblBCD.Text = "255";
            this.lblBCD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblBCD, "This number indicates the BCD number (decimal) being output. Red = BCD file, Blue" +
                    " = Override, Green = Aux BCD.");
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.CheckFileExists = false;
            // 
            // AccPort
            // 
            this.AccPort.ReadTimeout = 500;
            this.AccPort.WriteTimeout = 500;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 15000;
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
            this.txtFwd.Location = new System.Drawing.Point(4, 365);
            this.txtFwd.Name = "txtFwd";
            this.txtFwd.Size = new System.Drawing.Size(45, 20);
            this.txtFwd.TabIndex = 65;
            this.txtFwd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtFwd, "Transmitter Peak Forward Power");
            // 
            // txtSWR
            // 
            this.txtSWR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSWR.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSWR.Location = new System.Drawing.Point(141, 365);
            this.txtSWR.Name = "txtSWR";
            this.txtSWR.Size = new System.Drawing.Size(45, 20);
            this.txtSWR.TabIndex = 66;
            this.txtSWR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtSWR, "Antenna SWR");
            // 
            // txtTemp
            // 
            this.txtTemp.BackColor = System.Drawing.Color.LightGreen;
            this.txtTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtTemp.Location = new System.Drawing.Point(382, 364);
            this.txtTemp.Name = "txtTemp";
            this.txtTemp.Size = new System.Drawing.Size(50, 21);
            this.txtTemp.TabIndex = 62;
            this.txtTemp.Text = "PA Tmp";
            this.txtTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTemp, "Flex-5000 PA Temp, click to change from C to F");
            this.txtTemp.Click += new System.EventHandler(this.txtTemp_Click);
            // 
            // lblAnt
            // 
            this.lblAnt.AutoSize = true;
            this.lblAnt.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblAnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnt.ForeColor = System.Drawing.Color.Red;
            this.lblAnt.Location = new System.Drawing.Point(269, 3);
            this.lblAnt.Name = "lblAnt";
            this.lblAnt.Size = new System.Drawing.Size(141, 18);
            this.lblAnt.TabIndex = 73;
            this.lblAnt.Text = "Transmit Inhibited";
            this.toolTip1.SetToolTip(this.lblAnt, "Illuminated when antenna is adjusting");
            this.lblAnt.Visible = false;
            // 
            // txtAvg
            // 
            this.txtAvg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAvg.Location = new System.Drawing.Point(69, 365);
            this.txtAvg.Name = "txtAvg";
            this.txtAvg.Size = new System.Drawing.Size(45, 20);
            this.txtAvg.TabIndex = 75;
            this.txtAvg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtAvg, "Transmitter Average/Forward Power");
            // 
            // btnSplit
            // 
            this.btnSplit.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnSplit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSplit.Location = new System.Drawing.Point(323, 366);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(14, 17);
            this.btnSplit.TabIndex = 77;
            this.toolTip1.SetToolTip(this.btnSplit, "Press to toggle Split mode w/vfo A>B +n kHz (ctrl+~)");
            this.btnSplit.UseVisualStyleBackColor = false;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // numSplit
            // 
            this.numSplit.Location = new System.Drawing.Point(279, 365);
            this.numSplit.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numSplit.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.numSplit.Name = "numSplit";
            this.numSplit.Size = new System.Drawing.Size(40, 20);
            this.numSplit.TabIndex = 76;
            this.numSplit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numSplit, "Press the up/down arrows to select offset (kHz) for vfo B");
            this.numSplit.ValueChanged += new System.EventHandler(this.numSplit_ValueChanged);
            // 
            // txtAlcInd
            // 
            this.txtAlcInd.BackColor = System.Drawing.SystemColors.Control;
            this.txtAlcInd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAlcInd.Font = new System.Drawing.Font("Microsoft Sans Serif", 4.25F);
            this.txtAlcInd.Location = new System.Drawing.Point(415, 5);
            this.txtAlcInd.Name = "txtAlcInd";
            this.txtAlcInd.Size = new System.Drawing.Size(14, 14);
            this.txtAlcInd.TabIndex = 80;
            this.toolTip1.SetToolTip(this.txtAlcInd, "Status Ind for OPer/Stby");
            // 
            // btnFZ
            // 
            this.btnFZ.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnFZ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnFZ.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFZ.ForeColor = System.Drawing.Color.Navy;
            this.btnFZ.Location = new System.Drawing.Point(211, 364);
            this.btnFZ.Name = "btnFZ";
            this.btnFZ.Size = new System.Drawing.Size(31, 20);
            this.btnFZ.TabIndex = 83;
            this.btnFZ.Text = "FZ";
            this.toolTip1.SetToolTip(this.btnFZ, "Opens the Mini Window and Minimizes the Main Window.");
            this.btnFZ.UseVisualStyleBackColor = false;
            this.btnFZ.Click += new System.EventHandler(this.btnFZ_Click);
            // 
            // lblHighSWR
            // 
            this.lblHighSWR.AutoSize = true;
            this.lblHighSWR.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblHighSWR.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHighSWR.ForeColor = System.Drawing.Color.Red;
            this.lblHighSWR.Location = new System.Drawing.Point(188, 3);
            this.lblHighSWR.Name = "lblHighSWR";
            this.lblHighSWR.Size = new System.Drawing.Size(86, 18);
            this.lblHighSWR.TabIndex = 84;
            this.lblHighSWR.Text = "High SWR";
            this.toolTip1.SetToolTip(this.lblHighSWR, "Illuminated when antenna is adjusting");
            this.lblHighSWR.Visible = false;
            // 
            // comboBox2
            // 
            this.comboBox2.Enabled = false;
            this.comboBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(325, 109);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(63, 21);
            this.comboBox2.TabIndex = 67;
            this.toolTip1.SetToolTip(this.comboBox2, "Rotor Port for RCP 4");
            // 
            // comboBox3
            // 
            this.comboBox3.Enabled = false;
            this.comboBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(325, 72);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(63, 21);
            this.comboBox3.TabIndex = 66;
            this.toolTip1.SetToolTip(this.comboBox3, "Rotor Port for RCP 3");
            // 
            // comboBox4
            // 
            this.comboBox4.Enabled = false;
            this.comboBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(325, 37);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(63, 21);
            this.comboBox4.TabIndex = 65;
            this.toolTip1.SetToolTip(this.comboBox4, "Rotor Port for RCP 2");
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox1.Location = new System.Drawing.Point(50, 40);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(65, 17);
            this.checkBox1.TabIndex = 48;
            this.checkBox1.Text = "Enabled";
            this.toolTip1.SetToolTip(this.checkBox1, "Check to enable RCP 2");
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.textBox3.Location = new System.Drawing.Point(119, 141);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(164, 61);
            this.textBox3.TabIndex = 63;
            this.textBox3.Text = "RCP1 Commander\r\nRCP2 MixW\r\nRCP3 WriteLog\r\nRCP4 DXBase";
            this.toolTip1.SetToolTip(this.textBox3, "Use this to keep track of which port your programs are on.");
            // 
            // comboBox5
            // 
            this.comboBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point(121, 37);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(63, 21);
            this.comboBox5.TabIndex = 46;
            this.toolTip1.SetToolTip(this.comboBox5, "RCP2 CAT Port");
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox3.Location = new System.Drawing.Point(191, 107);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(57, 17);
            this.checkBox3.TabIndex = 62;
            this.checkBox3.Text = "IF only";
            this.toolTip1.SetToolTip(this.checkBox3, "Check to disable all CAT commands but IF");
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox4.Location = new System.Drawing.Point(191, 74);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(57, 17);
            this.checkBox4.TabIndex = 61;
            this.checkBox4.Text = "IF only";
            this.toolTip1.SetToolTip(this.checkBox4, "Check to disable all CAT commands but IF");
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox5.Location = new System.Drawing.Point(50, 74);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(65, 17);
            this.checkBox5.TabIndex = 51;
            this.checkBox5.Text = "Enabled";
            this.toolTip1.SetToolTip(this.checkBox5, "Check to enable RCP 3");
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox6.Location = new System.Drawing.Point(191, 40);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(57, 17);
            this.checkBox6.TabIndex = 60;
            this.checkBox6.Text = "IF only";
            this.toolTip1.SetToolTip(this.checkBox6, "Check to disable all CAT commands but IF");
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // comboBox6
            // 
            this.comboBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Location = new System.Drawing.Point(121, 72);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(63, 21);
            this.comboBox6.TabIndex = 50;
            this.toolTip1.SetToolTip(this.comboBox6, "RCP 3 CAT Port");
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox7.Location = new System.Drawing.Point(255, 108);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(61, 17);
            this.checkBox7.TabIndex = 59;
            this.checkBox7.Text = "Dis Poll";
            this.toolTip1.SetToolTip(this.checkBox7, "Check to disable polling for RCP 4");
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox8.Location = new System.Drawing.Point(50, 109);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(65, 17);
            this.checkBox8.TabIndex = 54;
            this.checkBox8.Text = "Enabled";
            this.toolTip1.SetToolTip(this.checkBox8, "Check to enable RCP 4");
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox9.Location = new System.Drawing.Point(255, 74);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(61, 17);
            this.checkBox9.TabIndex = 58;
            this.checkBox9.Text = "Dis Poll";
            this.toolTip1.SetToolTip(this.checkBox9, "Check to disable polling for RCP 3");
            this.checkBox9.UseVisualStyleBackColor = true;
            // 
            // comboBox7
            // 
            this.comboBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Location = new System.Drawing.Point(121, 106);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(63, 21);
            this.comboBox7.TabIndex = 53;
            this.toolTip1.SetToolTip(this.comboBox7, "RCP 4 CAT Port");
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox10.Location = new System.Drawing.Point(255, 40);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(61, 17);
            this.checkBox10.TabIndex = 57;
            this.checkBox10.Text = "Dis Poll";
            this.toolTip1.SetToolTip(this.checkBox10, "Check to disable polling for RCP 2");
            this.checkBox10.UseVisualStyleBackColor = true;
            // 
            // txtVolts
            // 
            this.txtVolts.BackColor = System.Drawing.Color.LightGreen;
            this.txtVolts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtVolts.Location = new System.Drawing.Point(342, 364);
            this.txtVolts.Name = "txtVolts";
            this.txtVolts.Size = new System.Drawing.Size(36, 21);
            this.txtVolts.TabIndex = 85;
            this.txtVolts.Text = "Volts";
            this.txtVolts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtVolts, "Power Supply voltage.");
            // 
            // btnReCall
            // 
            this.btnReCall.AutoSize = true;
            this.btnReCall.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnReCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnReCall.ForeColor = System.Drawing.Color.Blue;
            this.btnReCall.Location = new System.Drawing.Point(110, 390);
            this.btnReCall.Name = "btnReCall";
            this.btnReCall.Size = new System.Drawing.Size(54, 23);
            this.btnReCall.TabIndex = 92;
            this.btnReCall.Text = "ReCall";
            this.toolTip1.SetToolTip(this.btnReCall, "Press to step-thru memories (Ctrl+X).");
            this.btnReCall.UseVisualStyleBackColor = true;
            this.btnReCall.Click += new System.EventHandler(this.btnReCall_Click);
            // 
            // btnMemSave
            // 
            this.btnMemSave.AutoSize = true;
            this.btnMemSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMemSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnMemSave.ForeColor = System.Drawing.Color.Blue;
            this.btnMemSave.Location = new System.Drawing.Point(292, 391);
            this.btnMemSave.Name = "btnMemSave";
            this.btnMemSave.Size = new System.Drawing.Size(46, 23);
            this.btnMemSave.TabIndex = 93;
            this.btnMemSave.Text = "Save";
            this.toolTip1.SetToolTip(this.btnMemSave, "Press to save current memory data (Ctrl+V).");
            this.btnMemSave.UseVisualStyleBackColor = true;
            this.btnMemSave.Click += new System.EventHandler(this.btnMemSave_Click);
            // 
            // btnMV
            // 
            this.btnMV.AutoSize = true;
            this.btnMV.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnMV.ForeColor = System.Drawing.Color.ForestGreen;
            this.btnMV.Location = new System.Drawing.Point(58, 390);
            this.btnMV.Name = "btnMV";
            this.btnMV.Size = new System.Drawing.Size(49, 23);
            this.btnMV.TabIndex = 94;
            this.btnMV.Text = "V<>M";
            this.toolTip1.SetToolTip(this.btnMV, "Press to toggle between VFO A and selected memory (Ctrl+Z) .");
            this.btnMV.UseVisualStyleBackColor = true;
            this.btnMV.Click += new System.EventHandler(this.btnMV_Click);
            // 
            // btnMemClear
            // 
            this.btnMemClear.AutoSize = true;
            this.btnMemClear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMemClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnMemClear.ForeColor = System.Drawing.Color.Red;
            this.btnMemClear.Location = new System.Drawing.Point(386, 390);
            this.btnMemClear.Name = "btnMemClear";
            this.btnMemClear.Size = new System.Drawing.Size(46, 23);
            this.btnMemClear.TabIndex = 95;
            this.btnMemClear.Text = "Clear";
            this.toolTip1.SetToolTip(this.btnMemClear, "Press to clear selected memory. Press Ctrl+Shift+Clear to delete ALL memories.");
            this.btnMemClear.UseVisualStyleBackColor = true;
            this.btnMemClear.Click += new System.EventHandler(this.btnMemClear_Click);
            this.btnMemClear.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnMemClear_KeyDown);
            // 
            // txtMemFreq
            // 
            this.txtMemFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMemFreq.ForeColor = System.Drawing.Color.Navy;
            this.txtMemFreq.Location = new System.Drawing.Point(238, 392);
            this.txtMemFreq.Name = "txtMemFreq";
            this.txtMemFreq.Size = new System.Drawing.Size(50, 20);
            this.txtMemFreq.TabIndex = 96;
            this.txtMemFreq.Text = "MHZ";
            this.txtMemFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtMemFreq, "Memory frequency (double-click [Ctrl+N] to add note).");
            this.txtMemFreq.WordWrap = false;
            this.txtMemFreq.DoubleClick += new System.EventHandler(this.txtMemFreq_DoubleClick);
            // 
            // txtMemMode
            // 
            this.txtMemMode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMemMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMemMode.ForeColor = System.Drawing.Color.Navy;
            this.txtMemMode.Location = new System.Drawing.Point(342, 392);
            this.txtMemMode.Name = "txtMemMode";
            this.txtMemMode.Size = new System.Drawing.Size(40, 20);
            this.txtMemMode.TabIndex = 98;
            this.txtMemMode.Text = "MODE";
            this.txtMemMode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtMemMode, "Memory Mode");
            this.txtMemMode.WordWrap = false;
            // 
            // btnMemLoad
            // 
            this.btnMemLoad.AutoSize = true;
            this.btnMemLoad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMemLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnMemLoad.ForeColor = System.Drawing.Color.Firebrick;
            this.btnMemLoad.Location = new System.Drawing.Point(167, 390);
            this.btnMemLoad.Name = "btnMemLoad";
            this.btnMemLoad.Size = new System.Drawing.Size(45, 23);
            this.btnMemLoad.TabIndex = 99;
            this.btnMemLoad.Text = "Load";
            this.toolTip1.SetToolTip(this.btnMemLoad, "Press to load current memory data to PSDR (Ctrl+C).");
            this.btnMemLoad.UseVisualStyleBackColor = true;
            this.btnMemLoad.Click += new System.EventHandler(this.btnMemLoad_Click);
            // 
            // txtMemIdx
            // 
            this.txtMemIdx.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMemIdx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMemIdx.ForeColor = System.Drawing.Color.Navy;
            this.txtMemIdx.Location = new System.Drawing.Point(216, 392);
            this.txtMemIdx.Name = "txtMemIdx";
            this.txtMemIdx.Size = new System.Drawing.Size(17, 20);
            this.txtMemIdx.TabIndex = 100;
            this.txtMemIdx.Text = "#";
            this.txtMemIdx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtMemIdx, "Memory number");
            this.txtMemIdx.WordWrap = false;
            // 
            // txtMemVfo
            // 
            this.txtMemVfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMemVfo.ForeColor = System.Drawing.Color.Navy;
            this.txtMemVfo.Location = new System.Drawing.Point(4, 392);
            this.txtMemVfo.Name = "txtMemVfo";
            this.txtMemVfo.Size = new System.Drawing.Size(50, 20);
            this.txtMemVfo.TabIndex = 101;
            this.txtMemVfo.Text = "VFO";
            this.txtMemVfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtMemVfo, "VFO frequency saved.");
            this.txtMemVfo.WordWrap = false;
            // 
            // TkRed
            // 
            this.TkRed.BackColor = System.Drawing.SystemColors.Control;
            this.TkRed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TkRed.Location = new System.Drawing.Point(371, 422);
            this.TkRed.Name = "TkRed";
            this.TkRed.Size = new System.Drawing.Size(10, 10);
            this.TkRed.TabIndex = 103;
            this.toolTip1.SetToolTip(this.TkRed, "Tuning Knob Long-Click (On = Mode A, Off = Mode B)");
            this.TkRed.UseVisualStyleBackColor = false;
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
            this.toolStripSeparator2,
            this.autoDriveToolStripMenuItem1,
            this.toolStripSeparator3,
            this.errorLogToolStripMenuItem,
            this.toolStripSeparator1,
            this.helpToolStripMenuItem,
            this.toolStripSeparator4,
            this.getLatestVersionToolStripMenuItem,
            this.userForumToolStripMenuItem,
            this.toolStripSeparator9,
            this.toolStripMenuItem5});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowCheckMargin = true;
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(175, 166);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(171, 6);
            // 
            // autoDriveToolStripMenuItem1
            // 
            this.autoDriveToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.barefootSettingsToolStripMenuItem1,
            this.enableExpertModeToolStripMenuItem});
            this.autoDriveToolStripMenuItem1.Name = "autoDriveToolStripMenuItem1";
            this.autoDriveToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
            this.autoDriveToolStripMenuItem1.Text = "Auto Drive";
            this.autoDriveToolStripMenuItem1.ToolTipText = "Auto Drive Functions (check mark indicates enabled).";
            // 
            // enableToolStripMenuItem
            // 
            this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            this.enableToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.enableToolStripMenuItem.Text = "Enable Auto Drive";
            this.enableToolStripMenuItem.Click += new System.EventHandler(this.enableToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.settingsToolStripMenuItem.Text = "Amp Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // barefootSettingsToolStripMenuItem1
            // 
            this.barefootSettingsToolStripMenuItem1.Name = "barefootSettingsToolStripMenuItem1";
            this.barefootSettingsToolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
            this.barefootSettingsToolStripMenuItem1.Text = "Barefoot Settings";
            this.barefootSettingsToolStripMenuItem1.Click += new System.EventHandler(this.barefootSettingsToolStripMenuItem1_Click);
            // 
            // enableExpertModeToolStripMenuItem
            // 
            this.enableExpertModeToolStripMenuItem.Name = "enableExpertModeToolStripMenuItem";
            this.enableExpertModeToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.enableExpertModeToolStripMenuItem.Text = "Enable Expert Mode";
            this.enableExpertModeToolStripMenuItem.Click += new System.EventHandler(this.enableExpertModeToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(171, 6);
            // 
            // errorLogToolStripMenuItem
            // 
            this.errorLogToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableLoggingToolStripMenuItem,
            this.openErrorLogToolStripMenuItem1,
            this.clearErrorLogToolStripMenuItem1});
            this.errorLogToolStripMenuItem.Name = "errorLogToolStripMenuItem";
            this.errorLogToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.errorLogToolStripMenuItem.Text = "Error Log";
            this.errorLogToolStripMenuItem.ToolTipText = "Error Log Functions (check mark indicates enabled).";
            // 
            // enableLoggingToolStripMenuItem
            // 
            this.enableLoggingToolStripMenuItem.Name = "enableLoggingToolStripMenuItem";
            this.enableLoggingToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.enableLoggingToolStripMenuItem.Text = "Enable Logging";
            this.enableLoggingToolStripMenuItem.Click += new System.EventHandler(this.enableLoggingToolStripMenuItem_Click);
            // 
            // openErrorLogToolStripMenuItem1
            // 
            this.openErrorLogToolStripMenuItem1.Name = "openErrorLogToolStripMenuItem1";
            this.openErrorLogToolStripMenuItem1.Size = new System.Drawing.Size(158, 22);
            this.openErrorLogToolStripMenuItem1.Text = "Open Error Log";
            this.openErrorLogToolStripMenuItem1.Click += new System.EventHandler(this.openErrorLogToolStripMenuItem1_Click);
            // 
            // clearErrorLogToolStripMenuItem1
            // 
            this.clearErrorLogToolStripMenuItem1.Name = "clearErrorLogToolStripMenuItem1";
            this.clearErrorLogToolStripMenuItem1.Size = new System.Drawing.Size(158, 22);
            this.clearErrorLogToolStripMenuItem1.Text = "Clear Error Log";
            this.clearErrorLogToolStripMenuItem1.Click += new System.EventHandler(this.clearErrorLogToolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(171, 6);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slaveRadioInfoToolStripMenuItem,
            this.iCOMICPW1ToolStripMenuItem1,
            this.lP100ToolStripMenuItem1,
            this.macroCommandsToolStripMenuItem,
            this.rCPPortsToolStripMenuItem1,
            this.rotorControlToolStripMenuItem1,
            this.waveNodeToolStripMenuItem1,
            this.shortCutKeysToolStripMenuItem,
            this.aboutToolStripMenuItem2});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.helpToolStripMenuItem.Text = "Help Topics";
            this.helpToolStripMenuItem.ToolTipText = "General Help Topics.";
            // 
            // slaveRadioInfoToolStripMenuItem
            // 
            this.slaveRadioInfoToolStripMenuItem.Name = "slaveRadioInfoToolStripMenuItem";
            this.slaveRadioInfoToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.slaveRadioInfoToolStripMenuItem.Text = "Slave Radio Info";
            this.slaveRadioInfoToolStripMenuItem.Click += new System.EventHandler(this.slaveRadioInfoToolStripMenuItem_Click);
            // 
            // iCOMICPW1ToolStripMenuItem1
            // 
            this.iCOMICPW1ToolStripMenuItem1.Name = "iCOMICPW1ToolStripMenuItem1";
            this.iCOMICPW1ToolStripMenuItem1.Size = new System.Drawing.Size(169, 22);
            this.iCOMICPW1ToolStripMenuItem1.Text = "ICOM IC-PW1";
            this.iCOMICPW1ToolStripMenuItem1.Click += new System.EventHandler(this.iCOMICPW1ToolStripMenuItem1_Click);
            // 
            // lP100ToolStripMenuItem1
            // 
            this.lP100ToolStripMenuItem1.Name = "lP100ToolStripMenuItem1";
            this.lP100ToolStripMenuItem1.Size = new System.Drawing.Size(169, 22);
            this.lP100ToolStripMenuItem1.Text = "LP-100";
            this.lP100ToolStripMenuItem1.Click += new System.EventHandler(this.lP100ToolStripMenuItem1_Click);
            // 
            // macroCommandsToolStripMenuItem
            // 
            this.macroCommandsToolStripMenuItem.Name = "macroCommandsToolStripMenuItem";
            this.macroCommandsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.macroCommandsToolStripMenuItem.Text = "Macro Commands";
            this.macroCommandsToolStripMenuItem.Click += new System.EventHandler(this.macroCommandsToolStripMenuItem_Click);
            // 
            // rCPPortsToolStripMenuItem1
            // 
            this.rCPPortsToolStripMenuItem1.Name = "rCPPortsToolStripMenuItem1";
            this.rCPPortsToolStripMenuItem1.Size = new System.Drawing.Size(169, 22);
            this.rCPPortsToolStripMenuItem1.Text = "RCP Ports";
            this.rCPPortsToolStripMenuItem1.Click += new System.EventHandler(this.rCPPortsToolStripMenuItem1_Click);
            // 
            // rotorControlToolStripMenuItem1
            // 
            this.rotorControlToolStripMenuItem1.Name = "rotorControlToolStripMenuItem1";
            this.rotorControlToolStripMenuItem1.Size = new System.Drawing.Size(169, 22);
            this.rotorControlToolStripMenuItem1.Text = "Rotor Control";
            this.rotorControlToolStripMenuItem1.Click += new System.EventHandler(this.rotorControlToolStripMenuItem1_Click);
            // 
            // waveNodeToolStripMenuItem1
            // 
            this.waveNodeToolStripMenuItem1.Name = "waveNodeToolStripMenuItem1";
            this.waveNodeToolStripMenuItem1.Size = new System.Drawing.Size(169, 22);
            this.waveNodeToolStripMenuItem1.Text = "Wave Node";
            this.waveNodeToolStripMenuItem1.Click += new System.EventHandler(this.waveNodeToolStripMenuItem1_Click);
            // 
            // shortCutKeysToolStripMenuItem
            // 
            this.shortCutKeysToolStripMenuItem.Name = "shortCutKeysToolStripMenuItem";
            this.shortCutKeysToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.shortCutKeysToolStripMenuItem.Text = "Short-Cut Keys";
            this.shortCutKeysToolStripMenuItem.Click += new System.EventHandler(this.shortCutKeysToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem2
            // 
            this.aboutToolStripMenuItem2.Name = "aboutToolStripMenuItem2";
            this.aboutToolStripMenuItem2.Size = new System.Drawing.Size(169, 22);
            this.aboutToolStripMenuItem2.Text = "About";
            this.aboutToolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(171, 6);
            // 
            // getLatestVersionToolStripMenuItem
            // 
            this.getLatestVersionToolStripMenuItem.Name = "getLatestVersionToolStripMenuItem";
            this.getLatestVersionToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.getLatestVersionToolStripMenuItem.Text = "Check for Updates";
            this.getLatestVersionToolStripMenuItem.ToolTipText = "Get the latest version of DDUtil from the web site.";
            this.getLatestVersionToolStripMenuItem.Click += new System.EventHandler(this.getLatestVersionToolStripMenuItem_Click);
            // 
            // userForumToolStripMenuItem
            // 
            this.userForumToolStripMenuItem.Name = "userForumToolStripMenuItem";
            this.userForumToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.userForumToolStripMenuItem.Text = "User Community";
            this.userForumToolStripMenuItem.ToolTipText = "User community help forums.";
            this.userForumToolStripMenuItem.Click += new System.EventHandler(this.userForumToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(171, 6);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItem5.Size = new System.Drawing.Size(174, 22);
            this.toolStripMenuItem5.Text = "Exit";
            this.toolStripMenuItem5.ToolTipText = "Exit DDUtil";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // LPport
            // 
            this.LPport.ReadTimeout = 500;
            this.LPport.WriteTimeout = 500;
            this.LPport.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.LPport_DataReceived);
            // 
            // lblFwd
            // 
            this.lblFwd.AutoSize = true;
            this.lblFwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFwd.ForeColor = System.Drawing.Color.Navy;
            this.lblFwd.Location = new System.Drawing.Point(51, 369);
            this.lblFwd.Name = "lblFwd";
            this.lblFwd.Size = new System.Drawing.Size(16, 12);
            this.lblFwd.TabIndex = 19;
            this.lblFwd.Text = "Pk";
            this.lblFwd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSWR
            // 
            this.lblSWR.AutoSize = true;
            this.lblSWR.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSWR.ForeColor = System.Drawing.Color.Navy;
            this.lblSWR.Location = new System.Drawing.Point(188, 369);
            this.lblSWR.Name = "lblSWR";
            this.lblSWR.Size = new System.Drawing.Size(22, 12);
            this.lblSWR.TabIndex = 67;
            this.lblSWR.Text = "Swr";
            this.lblSWR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RCP2port
            // 
            this.RCP2port.ReadTimeout = 500;
            this.RCP2port.WriteTimeout = 500;
            this.RCP2port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.RCP2port_DataReceived);
            // 
            // RCP3port
            // 
            this.RCP3port.ReadTimeout = 500;
            this.RCP3port.WriteTimeout = 500;
            this.RCP3port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.RCP3port_DataReceived);
            // 
            // RCP4port
            // 
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
            this.TestPort.ReadTimeout = 500;
            this.TestPort.ReceivedBytesThreshold = 6;
            this.TestPort.WriteBufferSize = 4096;
            this.TestPort.WriteTimeout = 1000;
            this.TestPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.TestPort_DataReceived);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(435, 24);
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
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoDriveToolStripMenuItem,
            this.toolStripSeparator15,
            this.tubeAmpsToolStripMenuItem,
            this.toolStripSeparator16,
            this.acomAntDefaultToolStripMenuItem,
            this.toolStripSeparator17,
            this.antToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // autoDriveToolStripMenuItem
            // 
            this.autoDriveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ampSettingsToolStripMenuItem,
            this.barefootSettingsToolStripMenuItem});
            this.autoDriveToolStripMenuItem.Name = "autoDriveToolStripMenuItem";
            this.autoDriveToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.autoDriveToolStripMenuItem.Text = "Auto Drive";
            this.autoDriveToolStripMenuItem.Click += new System.EventHandler(this.autoDriveToolStripMenuItem_Click);
            // 
            // ampSettingsToolStripMenuItem
            // 
            this.ampSettingsToolStripMenuItem.Name = "ampSettingsToolStripMenuItem";
            this.ampSettingsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.ampSettingsToolStripMenuItem.Text = "Amp Settings";
            this.ampSettingsToolStripMenuItem.Click += new System.EventHandler(this.ampSettingsToolStripMenuItem_Click);
            // 
            // barefootSettingsToolStripMenuItem
            // 
            this.barefootSettingsToolStripMenuItem.Name = "barefootSettingsToolStripMenuItem";
            this.barefootSettingsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.barefootSettingsToolStripMenuItem.Text = "Barefoot Settings";
            this.barefootSettingsToolStripMenuItem.Click += new System.EventHandler(this.barefootSettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(166, 6);
            // 
            // tubeAmpsToolStripMenuItem
            // 
            this.tubeAmpsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aCOM2000AToolStripMenuItem,
            this.alpToolStripMenuItem,
            this.alpha9500ToolStripMenuItem});
            this.tubeAmpsToolStripMenuItem.Name = "tubeAmpsToolStripMenuItem";
            this.tubeAmpsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.tubeAmpsToolStripMenuItem.Text = "Tube Amps";
            // 
            // aCOM2000AToolStripMenuItem
            // 
            this.aCOM2000AToolStripMenuItem.Name = "aCOM2000AToolStripMenuItem";
            this.aCOM2000AToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.aCOM2000AToolStripMenuItem.Text = "Acom2000A";
            this.aCOM2000AToolStripMenuItem.ToolTipText = "Click to select this amp.";
            this.aCOM2000AToolStripMenuItem.Click += new System.EventHandler(this.ACOM2K_Click);
            // 
            // alpToolStripMenuItem
            // 
            this.alpToolStripMenuItem.Name = "alpToolStripMenuItem";
            this.alpToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.alpToolStripMenuItem.Text = "Alpha 87A";
            this.alpToolStripMenuItem.ToolTipText = "Click to select this amp.";
            this.alpToolStripMenuItem.Click += new System.EventHandler(this.Alpha87_Click);
            // 
            // alpha9500ToolStripMenuItem
            // 
            this.alpha9500ToolStripMenuItem.Name = "alpha9500ToolStripMenuItem";
            this.alpha9500ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.alpha9500ToolStripMenuItem.Text = "Alpha 9500";
            this.alpha9500ToolStripMenuItem.ToolTipText = "Click to select this amp.";
            this.alpha9500ToolStripMenuItem.Click += new System.EventHandler(this.Alpha95_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(166, 6);
            // 
            // acomAntDefaultToolStripMenuItem
            // 
            this.acomAntDefaultToolStripMenuItem.Name = "acomAntDefaultToolStripMenuItem";
            this.acomAntDefaultToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.acomAntDefaultToolStripMenuItem.Text = "Acom Ant Default";
            this.acomAntDefaultToolStripMenuItem.Click += new System.EventHandler(this.acomAntDefaultToolStripMenuItem_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(166, 6);
            // 
            // antToolStripMenuItem
            // 
            this.antToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recallToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.antToolStripMenuItem.Name = "antToolStripMenuItem";
            this.antToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.antToolStripMenuItem.Text = "Ant Profiles";
            // 
            // recallToolStripMenuItem
            // 
            this.recallToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6,
            this.toolStripMenuItem7});
            this.recallToolStripMenuItem.Name = "recallToolStripMenuItem";
            this.recallToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.recallToolStripMenuItem.Text = "Recall";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem6.Text = "Profile 1";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem7.Text = "Profile 2";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8,
            this.toolStripMenuItem9});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem8.Text = "Profile 1";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem9.Text = "Profile 2";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripMenuItem9_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableErrorLoggingToolStripMenuItem,
            this.openErrorLogToolStripMenuItem,
            this.clearErrorLogToolStripMenuItem,
            this.toolStripSeparator12,
            this.resetDontAskMeAgainToolStripMenuItem,
            this.toolStripSeparator13,
            this.showMiniWindowToolStripMenuItem,
            this.toolStripSeparator14,
            this.vSPManagerToolStripMenuItem});
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
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(206, 6);
            // 
            // resetDontAskMeAgainToolStripMenuItem
            // 
            this.resetDontAskMeAgainToolStripMenuItem.Name = "resetDontAskMeAgainToolStripMenuItem";
            this.resetDontAskMeAgainToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.resetDontAskMeAgainToolStripMenuItem.Text = "Reset Don\'t Ask Me Again";
            this.resetDontAskMeAgainToolStripMenuItem.Click += new System.EventHandler(this.resetDontAskMeAgainToolStripMenuItem_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(206, 6);
            // 
            // showMiniWindowToolStripMenuItem
            // 
            this.showMiniWindowToolStripMenuItem.Name = "showMiniWindowToolStripMenuItem";
            this.showMiniWindowToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.M)));
            this.showMiniWindowToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.showMiniWindowToolStripMenuItem.Text = "Show Mini Window";
            this.showMiniWindowToolStripMenuItem.Click += new System.EventHandler(this.showMiniWindowToolStripMenuItem_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(206, 6);
            // 
            // vSPManagerToolStripMenuItem
            // 
            this.vSPManagerToolStripMenuItem.Name = "vSPManagerToolStripMenuItem";
            this.vSPManagerToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.vSPManagerToolStripMenuItem.Text = "Start VSP Manager";
            this.vSPManagerToolStripMenuItem.ToolTipText = "Start VSP Manager (note: must be located in C:\\Program Files\\vspMgr directory).";
            this.vSPManagerToolStripMenuItem.Click += new System.EventHandler(this.vSPManagerToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onLineHelpToolStripMenuItem,
            this.setupWizardToolStripMenuItem,
            this.showTipsToolStripMenuItem,
            this.toolStripSeparator10,
            this.updateToLatestVersionToolStripMenuItem,
            this.userForumToolStripMenuItem1,
            this.toolStripSeparator11,
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
            // setupWizardToolStripMenuItem
            // 
            this.setupWizardToolStripMenuItem.Name = "setupWizardToolStripMenuItem";
            this.setupWizardToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.setupWizardToolStripMenuItem.Text = "Setup Wizard";
            this.setupWizardToolStripMenuItem.Click += new System.EventHandler(this.setupWizardToolStripMenuItem_Click);
            // 
            // showTipsToolStripMenuItem
            // 
            this.showTipsToolStripMenuItem.Name = "showTipsToolStripMenuItem";
            this.showTipsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.showTipsToolStripMenuItem.Text = "Show Tips";
            this.showTipsToolStripMenuItem.Click += new System.EventHandler(this.showTipsToolStripMenuItem_Click_1);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(171, 6);
            // 
            // updateToLatestVersionToolStripMenuItem
            // 
            this.updateToLatestVersionToolStripMenuItem.Name = "updateToLatestVersionToolStripMenuItem";
            this.updateToLatestVersionToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.updateToLatestVersionToolStripMenuItem.Text = "Check for Updates";
            this.updateToLatestVersionToolStripMenuItem.Click += new System.EventHandler(this.updateToLatestVersionToolStripMenuItem_Click);
            // 
            // userForumToolStripMenuItem1
            // 
            this.userForumToolStripMenuItem1.Name = "userForumToolStripMenuItem1";
            this.userForumToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
            this.userForumToolStripMenuItem1.Text = "User Community";
            this.userForumToolStripMenuItem1.ToolTipText = "User community help forums.";
            this.userForumToolStripMenuItem1.Click += new System.EventHandler(this.userForumToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(171, 6);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 416);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(435, 22);
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
            this.lblAvg.Location = new System.Drawing.Point(116, 369);
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
            // PMport
            // 
            this.PMport.BaudRate = 38400;
            this.PMport.ReadBufferSize = 32760;
            this.PMport.RtsEnable = true;
            this.PMport.WriteBufferSize = 512;
            this.PMport.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.PMport_DataReceived);
            // 
            // RepeatPort
            // 
            this.RepeatPort.ReadTimeout = 500;
            this.RepeatPort.WriteTimeout = 500;
            this.RepeatPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.RepeatPort_DataReceived);
            // 
            // PwrPort
            // 
            this.PwrPort.ReadTimeout = 500;
            this.PwrPort.WriteTimeout = 500;
            // 
            // pdTimer
            // 
            this.pdTimer.Interval = 50;
            this.pdTimer.Tick += new System.EventHandler(this.pdTimer_Tick);
            // 
            // SPEport
            // 
            this.SPEport.ReadTimeout = 500;
            this.SPEport.WriteTimeout = 500;
            this.SPEport.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SPEport_DataReceived);
            // 
            // label159
            // 
            this.label159.AutoSize = true;
            this.label159.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label159.Location = new System.Drawing.Point(5, 110);
            this.label159.Name = "label159";
            this.label159.Size = new System.Drawing.Size(35, 13);
            this.label159.TabIndex = 74;
            this.label159.Text = "RCP4";
            // 
            // label160
            // 
            this.label160.AutoSize = true;
            this.label160.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label160.Location = new System.Drawing.Point(5, 75);
            this.label160.Name = "label160";
            this.label160.Size = new System.Drawing.Size(35, 13);
            this.label160.TabIndex = 73;
            this.label160.Text = "RCP3";
            // 
            // label162
            // 
            this.label162.AutoSize = true;
            this.label162.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label162.Location = new System.Drawing.Point(315, 21);
            this.label162.Name = "label162";
            this.label162.Size = new System.Drawing.Size(80, 13);
            this.label162.TabIndex = 72;
            this.label162.Text = "RCP Rotor Port";
            this.label162.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label163
            // 
            this.label163.AutoSize = true;
            this.label163.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label163.Location = new System.Drawing.Point(115, 21);
            this.label163.Name = "label163";
            this.label163.Size = new System.Drawing.Size(75, 13);
            this.label163.TabIndex = 71;
            this.label163.Text = "RCP CAT Port";
            this.label163.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label164
            // 
            this.label164.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label164.Location = new System.Drawing.Point(150, 205);
            this.label164.Name = "label164";
            this.label164.Size = new System.Drawing.Size(100, 13);
            this.label164.TabIndex = 64;
            this.label164.Text = "Program Memo";
            this.label164.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label165
            // 
            this.label165.AutoSize = true;
            this.label165.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label165.Location = new System.Drawing.Point(6, 41);
            this.label165.Name = "label165";
            this.label165.Size = new System.Drawing.Size(35, 13);
            this.label165.TabIndex = 49;
            this.label165.Text = "RCP2";
            // 
            // SwitchPort
            // 
            this.SwitchPort.DtrEnable = true;
            this.SwitchPort.RtsEnable = true;
            this.SwitchPort.PinChanged += new System.IO.Ports.SerialPinChangedEventHandler(this.PinChanged);
            // 
            // KnobPort
            // 
            this.KnobPort.RtsEnable = true;
            this.KnobPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.KnobPort_DataReceived);
            // 
            // MacroPort
            // 
            this.MacroPort.DtrEnable = true;
            this.MacroPort.RtsEnable = true;
            this.MacroPort.PinChanged += new System.IO.Ports.SerialPinChangedEventHandler(this.MacroPort_PinChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "DDUtil";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator5,
            this.toolStripMenuItem1,
            this.toolStripSeparator6,
            this.toolStripMenuItem11,
            this.toolStripSeparator7,
            this.toolStripMenuItem15,
            this.toolStripSeparator8,
            this.toolStripMenuItem25,
            this.toolStripMenuItem26});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.ShowCheckMargin = true;
            this.contextMenuStrip2.ShowImageMargin = false;
            this.contextMenuStrip2.Size = new System.Drawing.Size(174, 138);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(170, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem10});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(173, 22);
            this.toolStripMenuItem1.Text = "Auto Drive";
            this.toolStripMenuItem1.ToolTipText = "Auto Drive Functions (check mark indicates enabled).";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(181, 22);
            this.toolStripMenuItem2.Text = "Enable Auto Drive";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(181, 22);
            this.toolStripMenuItem3.Text = "Amp Settings";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(181, 22);
            this.toolStripMenuItem4.Text = "Barefoot Settings";
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(181, 22);
            this.toolStripMenuItem10.Text = "Enable Expert Mode";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(170, 6);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem12,
            this.toolStripMenuItem13,
            this.toolStripMenuItem14});
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(173, 22);
            this.toolStripMenuItem11.Text = "Error Log";
            this.toolStripMenuItem11.ToolTipText = "Error Log Functions (check mark indicates enabled).";
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem12.Text = "Enable Logging";
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem13.Text = "Open Error Log";
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem14.Text = "Clear Error Log";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(170, 6);
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem16,
            this.toolStripMenuItem17,
            this.toolStripMenuItem18,
            this.toolStripMenuItem19,
            this.toolStripMenuItem20,
            this.toolStripMenuItem21,
            this.toolStripMenuItem22,
            this.toolStripMenuItem23,
            this.toolStripMenuItem24});
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(173, 22);
            this.toolStripMenuItem15.Text = "Help Topics";
            this.toolStripMenuItem15.ToolTipText = "General Help Topics.";
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem16.Text = "Slave Radio Info";
            // 
            // toolStripMenuItem17
            // 
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            this.toolStripMenuItem17.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem17.Text = "ICOM IC-PW1";
            // 
            // toolStripMenuItem18
            // 
            this.toolStripMenuItem18.Name = "toolStripMenuItem18";
            this.toolStripMenuItem18.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem18.Text = "LP-100";
            // 
            // toolStripMenuItem19
            // 
            this.toolStripMenuItem19.Name = "toolStripMenuItem19";
            this.toolStripMenuItem19.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem19.Text = "Macro Commands";
            // 
            // toolStripMenuItem20
            // 
            this.toolStripMenuItem20.Name = "toolStripMenuItem20";
            this.toolStripMenuItem20.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem20.Text = "RCP Ports";
            // 
            // toolStripMenuItem21
            // 
            this.toolStripMenuItem21.Name = "toolStripMenuItem21";
            this.toolStripMenuItem21.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem21.Text = "Rotor Control";
            // 
            // toolStripMenuItem22
            // 
            this.toolStripMenuItem22.Name = "toolStripMenuItem22";
            this.toolStripMenuItem22.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem22.Text = "Wave Node";
            // 
            // toolStripMenuItem23
            // 
            this.toolStripMenuItem23.Name = "toolStripMenuItem23";
            this.toolStripMenuItem23.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem23.Text = "Short-Cut Keys";
            // 
            // toolStripMenuItem24
            // 
            this.toolStripMenuItem24.Name = "toolStripMenuItem24";
            this.toolStripMenuItem24.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem24.Text = "About";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(170, 6);
            // 
            // toolStripMenuItem25
            // 
            this.toolStripMenuItem25.Name = "toolStripMenuItem25";
            this.toolStripMenuItem25.Size = new System.Drawing.Size(173, 22);
            this.toolStripMenuItem25.Text = "Get Latest Version";
            this.toolStripMenuItem25.ToolTipText = "Get the latest DDUtil version from the web site.";
            // 
            // toolStripMenuItem26
            // 
            this.toolStripMenuItem26.Name = "toolStripMenuItem26";
            this.toolStripMenuItem26.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItem26.Size = new System.Drawing.Size(173, 22);
            this.toolStripMenuItem26.Text = "Exit";
            this.toolStripMenuItem26.ToolTipText = "Exit DDUtil";
            // 
            // TkGrn
            // 
            this.TkGrn.BackColor = System.Drawing.SystemColors.Control;
            this.TkGrn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TkGrn.Location = new System.Drawing.Point(407, 422);
            this.TkGrn.Name = "TkGrn";
            this.TkGrn.Size = new System.Drawing.Size(10, 10);
            this.TkGrn.TabIndex = 102;
            this.toolTip1.SetToolTip(this.TkGrn, "Tuning Knob Double-Click (On/Off)");
            this.TkGrn.UseVisualStyleBackColor = false;
            // 
            // TkYel
            // 
            this.TkYel.BackColor = System.Drawing.SystemColors.Control;
            this.TkYel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TkYel.Location = new System.Drawing.Point(390, 422);
            this.TkYel.Name = "TkYel";
            this.TkYel.Size = new System.Drawing.Size(10, 10);
            this.TkYel.TabIndex = 74;
            this.toolTip1.SetToolTip(this.TkYel, "Tuning Knob Short-Click (On/Off)");
            this.TkYel.UseVisualStyleBackColor = false;
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(435, 438);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.TkRed);
            this.Controls.Add(this.TkYel);
            this.Controls.Add(this.TkGrn);
            this.Controls.Add(this.txtMemVfo);
            this.Controls.Add(this.btnMV);
            this.Controls.Add(this.btnMemClear);
            this.Controls.Add(this.lblBCD);
            this.Controls.Add(this.txtMemMode);
            this.Controls.Add(this.btnReCall);
            this.Controls.Add(this.btnMemLoad);
            this.Controls.Add(this.txtMemIdx);
            this.Controls.Add(this.txtVolts);
            this.Controls.Add(this.txtMemFreq);
            this.Controls.Add(this.lblHighSWR);
            this.Controls.Add(this.txtAlcInd);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblAvg);
            this.Controls.Add(this.txtAvg);
            this.Controls.Add(this.txtTemp);
            this.Controls.Add(this.lblAnt);
            this.Controls.Add(this.btnMemSave);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.txtSWR);
            this.Controls.Add(this.lblSWR);
            this.Controls.Add(this.lblFwd);
            this.Controls.Add(this.txtFwd);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnFZ);
            this.Controls.Add(this.numSplit);
            this.Controls.Add(this.btnSplit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Setup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DDUtil - (Not Connected)";
            this.toolTip1.SetToolTip(this, "Right-Click for Context Menu.");
            this.Load += new System.EventHandler(this.Setup_Load);
            this.Shown += new System.EventHandler(this.Setup_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Setup_FormClosing);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Setup_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Setup_KeyDown);
            this.tabControl.ResumeLayout(false);
            this.tabPorts.ResumeLayout(false);
            this.tabPorts.PerformLayout();
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
            this.tabBCD.ResumeLayout(false);
            this.tabBCD.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pixBox1)).EndInit();
            this.grpAuxBcdControl.ResumeLayout(false);
            this.grpAuxBcdControl.PerformLayout();
            this.grpBCDover.ResumeLayout(false);
            this.grpBCDover.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg1)).EndInit();
            this.tabRCP.ResumeLayout(false);
            this.tabRCP.PerformLayout();
            this.grpTKnob.ResumeLayout(false);
            this.grpTKnob.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabWatt.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSWR)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpNet.ResumeLayout(false);
            this.grpNet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFwd)).EndInit();
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
            this.grpLP.ResumeLayout(false);
            this.grpLP.PerformLayout();
            this.tabAmp.ResumeLayout(false);
            this.grpSPE.ResumeLayout(false);
            this.grpSPE.PerformLayout();
            this.grpAmpBand.ResumeLayout(false);
            this.grpAmpBand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDrive)).EndInit();
            this.grpPTT.ResumeLayout(false);
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.grpAmp.ResumeLayout(false);
            this.grpAmp.PerformLayout();
            this.grpAmpBands.ResumeLayout(false);
            this.grpAmpBands.PerformLayout();
            this.tabOther.ResumeLayout(false);
            this.tabOther.PerformLayout();
            this.grpMacSw.ResumeLayout(false);
            this.grpMacSw.PerformLayout();
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.grpRepeat.ResumeLayout(false);
            this.grpRepeat.PerformLayout();
            this.grpRmode.ResumeLayout(false);
            this.grpRmode.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.grpPro.ResumeLayout(false);
            this.grpPro.PerformLayout();
            this.grpInP.ResumeLayout(false);
            this.grpInP.PerformLayout();
            this.grpAntP.ResumeLayout(false);
            this.grpAntP.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabMacro.ResumeLayout(false);
            this.tabMacro.PerformLayout();
            this.grpMacro.ResumeLayout(false);
            this.grpMacro.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgm)).EndInit();
            this.tabRotor.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.grpPreset.ResumeLayout(false);
            this.grpStepCtrl.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tabExtCtrl.ResumeLayout(false);
            this.tabExtCtrl.PerformLayout();
            this.grpPortB.ResumeLayout(false);
            this.grpPortB.PerformLayout();
            this.grpPortA.ResumeLayout(false);
            this.grpPortA.PerformLayout();
            this.tabSetup.ResumeLayout(false);
            this.tabSetup.PerformLayout();
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
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
            this.tabProp.ResumeLayout(false);
            this.grpSO2R.ResumeLayout(false);
            this.grpSO2R.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSplit)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPorts;
        private System.Windows.Forms.GroupBox grpBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkDevice;
        private System.Windows.Forms.GroupBox grpLPT;
        private System.Windows.Forms.RadioButton rb1;
        private System.Windows.Forms.RadioButton rb4;
        private System.Windows.Forms.RadioButton rb2;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rb3;
        private System.Windows.Forms.TabPage tabBCD;
        private System.Windows.Forms.CheckBox chkDev0;
        private System.Windows.Forms.DataGridView dg1;
        private System.Windows.Forms.GroupBox grpBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboCAT;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
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
        private System.Windows.Forms.TabPage tabWatt;
        private System.IO.Ports.SerialPort LogPort;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboLogPort;
        private System.Windows.Forms.RadioButton rbOther;
        private System.Windows.Forms.Button btnPortNum;
        private System.Windows.Forms.Label lblPortBtn;
        private System.Windows.Forms.CheckBox chkFollow;
        private System.Windows.Forms.ComboBox cboRadio;
        private System.Windows.Forms.Label label8;
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
        private System.Windows.Forms.GroupBox grpLP;
        private System.Windows.Forms.TabPage tabRCP;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cboRCP2;
        private System.Windows.Forms.ComboBox cboRCP4;
        private System.Windows.Forms.ComboBox cboRCP3;
        private System.IO.Ports.SerialPort RCP2port;
        private System.IO.Ports.SerialPort RCP3port;
        private System.IO.Ports.SerialPort RCP4port;
        private System.Windows.Forms.Label label20;
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
        private System.Windows.Forms.CheckBox chkDisBcast;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
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
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label30;
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
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.CheckBox chkRCP4IF;
        private System.Windows.Forms.CheckBox chkRCP3IF;
        private System.Windows.Forms.CheckBox chkRCP2IF;
        private System.Windows.Forms.TextBox txtRCP;
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
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.ComboBox cboRCP1Rotor;
        private System.IO.Ports.SerialPort RCP1Rotor;
        private System.IO.Ports.SerialPort RCP2Rotor;
        private System.IO.Ports.SerialPort RCP3Rotor;
        private System.IO.Ports.SerialPort RCP4Rotor;
        private System.Windows.Forms.GroupBox grpStepCtrl;
        private System.Windows.Forms.RadioButton rb34;
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
        private System.Windows.Forms.Button btnWnReset;
        private System.Windows.Forms.ToolStripMenuItem showMiniWindowToolStripMenuItem;
        public System.Windows.Forms.GroupBox grpModel;
        public System.IO.Ports.SerialPort RotorPort;
        public System.Windows.Forms.Label lblFwd;
        public System.Windows.Forms.CheckBox chkDog;
        private System.Windows.Forms.TextBox txtTimer;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.GroupBox grpDog;
        public System.Windows.Forms.Label lblSP;
        private System.Windows.Forms.CheckBox chkModeChg;
        private System.Windows.Forms.CheckBox chkOnTop;
        private System.Windows.Forms.RadioButton rb100A;
        private System.Windows.Forms.RadioButton rb100;
        private System.Windows.Forms.ToolStripMenuItem clearErrorLogToolStripMenuItem;
        public System.Windows.Forms.TextBox txtSP;
        private System.Windows.Forms.ToolStripMenuItem setupWizardToolStripMenuItem;
        private TabPage tabAmp;
        private GroupBox grpAmp;
        private CheckBox chkAlpha;
        private TextBox txtBand;
        private Label label71;
        private Label label72;
        private Label label73;
        private Label label75;
        private ComboBox cboAlpha;
        private Label label76;
        private Label label77;
        private Label lblAmpInt;
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
        private Label lblLoad;
        private Label lblTune;
        private Button btnWnSensor;
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
        private CheckBox chkInvertA;
        private CheckBox chkInvertB;
        private GroupBox groupBox2;
        private Label label138;
        private Label label139;
        private ComboBox cboPMport;
        private CheckBox chkPM;
        private Label label140;
        private Label label141;
        private ComboBox cboPMcom;
        private Label label142;
        private NumericUpDown numFwd;
        private NumericUpDown numRev;
        private System.IO.Ports.SerialPort PMport;
        private GroupBox grpNet;
        private RadioButton rbNet;
        private RadioButton rbStd;
        private System.IO.Ports.SerialPort RepeatPort;
        private NumericUpDown numSplit;
        private ToolStripMenuItem vSPManagerToolStripMenuItem;
        private Label label29;
        private Label label143;
        private Label label144;
        private Label label145;
        private Label label146;
        private CheckBox chkSlaveDTR;
        private CheckBox chkSlaveRTS;
        private CheckBox chkPwRTS;
        private CheckBox chkPwDTR;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem tubeAmpsToolStripMenuItem;
        private ToolStripMenuItem alpToolStripMenuItem;
        private ToolStripMenuItem aCOM2000AToolStripMenuItem;
        private ToolStripMenuItem alpha9500ToolStripMenuItem;
        private GroupBox grpAmpBand;
        public CheckBox chkB80;
        public CheckBox chkB160;
        public CheckBox chkB30;
        public CheckBox chkB40;
        public CheckBox chkB17;
        public CheckBox chkB20;
        private Button btnBandAll;
        private Button btnBandClear;
        public CheckBox chkB10;
        public CheckBox chkB12;
        public CheckBox chkB15;
        private Button btnInfo;
        private Label label74;
        private Label label70;
        private TextBox textBox1;
        public Button btnDrive;
        private CheckBox chkAutoDrv;
        private NumericUpDown numDrive;
        private Label label148;
        private GroupBox grpBCDover;
        private CheckBox chkOvride;
        private RadioButton rbOvr12;
        private RadioButton rbOvr11;
        private RadioButton rbOvr10;
        private RadioButton rbOvr9;
        private RadioButton rbOvr8;
        private RadioButton rbOvr4;
        private RadioButton rbOvr7;
        private RadioButton rbOvr3;
        private RadioButton rbOvr6;
        private RadioButton rbOvr2;
        private RadioButton rbOvr5;
        private RadioButton rbOvr1;
        private TextBox txtLoc;
        private GroupBox groupBox12;
        private TextBox txtKm;
        private TextBox txtSm;
        private GroupBox grpPro;
        private GroupBox grpAntP;
        private RadioButton radioButton4;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private GroupBox grpInP;
        private RadioButton radioButton7;
        private RadioButton radioButton8;
        private Button button3;
        private Button button4;
        public Button button2;
        public Button button5;
        private Label label149;
        private Label label150;
        private GroupBox groupBox13;
        private Label label151;
        private CheckBox checkBox2;
        private Button button1;
        private Label label152;
        private TextBox textBox2;
        private TabPage tabOther;
        private GroupBox grpRepeat;
        private Label label79;
        private Label label147;
        private ComboBox cboRepeatCom;
        private GroupBox grpRmode;
        private RadioButton rbRptNone;
        private RadioButton rbIFonly;
        private RadioButton rbAll;
        private ComboBox cboRepeatPort;
        private CheckBox chkRepeat;
        public TextBox txtPmsg;
        private Label label153;
        private ComboBox comboBox1;
        private Button button6;
        private RadioButton rbRptPal;
        private RadioButton rbFW;
        private CheckBox chkDSInvert;
        private CheckBox chkFWa;
        private CheckBox chkFWb;
        private GroupBox grpPTT;
        private ToolStripMenuItem autoDriveToolStripMenuItem;
        private TextBox txtAlcInd;
        private CheckBox chkNoBdChg;
        public Button btnByp;
        public CheckBox chkRCP2;
        public CheckBox chkRCP4;
        public CheckBox chkRCP3;
        public Button btnMacro1;
        public Button btnMacro6;
        public Button btnMacro5;
        public Button btnMacro4;
        public Button btnMacro3;
        public Button btnMacro2;
        public Button btnMacro12;
        public Button btnMacro11;
        public Button btnMacro10;
        public Button btnMacro9;
        public Button btnMacro8;
        public Button btnMacro7;
        public Button btnMacro16;
        public Button btnMacro15;
        public Button btnMacro13;
        public Button btnMacro14;
        private Button btnFZ;
        private GroupBox grpAmpBands;
        private ToolStripMenuItem acomAntDefaultToolStripMenuItem;
        private CheckBox chkAmp6;
        private CheckBox chkAmp80;
        private CheckBox chkAmp160;
        private CheckBox chkAmp30;
        private CheckBox chkAmp17;
        private CheckBox chkAmp40;
        private CheckBox chkAmp10;
        private CheckBox chkAmp12;
        private CheckBox chkAmp20;
        private CheckBox chkAmp15;
        private GroupBox groupBox15;
        private CheckBox chkSWR;
        private NumericUpDown numSWR;
        private Label lblHighSWR;
        private CheckBox chkTenths;
        private Label lblStepFreq;
        public Button btnFlexOn;
        private Label label64;
        private ComboBox cboPwrPort;
        private System.IO.Ports.SerialPort PwrPort;
        private GroupBox groupBox16;
        private TextBox txtPSDR;
        private Label label68;
        private Button btnClrAll;
        private Button btnCkAll;
        private ToolStripMenuItem resetDontAskMeAgainToolStripMenuItem;
        private Button btnStop;
        private Timer pdTimer;
        private CheckBox chkPSDR;
        private GroupBox groupBox17;
        private TextBox txtVspMgr;
        private TextBox txtAzOffset;
        private Label label156;
        private GroupBox grpSPE;
        private Button btnSPEdisp;
        private Label label157;
        public TextBox txtSPEmsg;
        private Button btnSPEtune;
        private Label label158;
        private ComboBox cboSPEport;
        private CheckBox chkSPEenab;
        private TextBox txtSPEtemp;
        private Button btnSPEoper;
        private Button btnSPEon;
        private Button btnSPEoff;
        private Button btnSPEpwr;
        private System.IO.Ports.SerialPort SPEport;
        private Button btnSPEant;
        private TextBox txtSPEant;
        private Label label161;
        private RadioButton rbLed;
        private ToolStripMenuItem ampSettingsToolStripMenuItem;
        private ToolStripMenuItem barefootSettingsToolStripMenuItem;
        private Button btnSPEset;
        private Button btnSPEleft;
        private Button btnSPEright;
        private ToolStripMenuItem showTipsToolStripMenuItem;
        private TabPage tabProp;
        private GroupBox grpSO2R;
        private Label label166;
        private Label label169;
        public CheckBox chkSO1a;
        private Label label171;
        private Label label159;
        private Label label160;
        private Label label162;
        private Label label163;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private ComboBox comboBox4;
        private Label label164;
        public CheckBox checkBox1;
        private TextBox textBox3;
        private ComboBox comboBox5;
        private CheckBox checkBox3;
        private Label label165;
        private CheckBox checkBox4;
        public CheckBox checkBox5;
        private CheckBox checkBox6;
        private ComboBox comboBox6;
        private CheckBox checkBox7;
        public CheckBox checkBox8;
        private CheckBox checkBox9;
        private ComboBox comboBox7;
        private CheckBox checkBox10;
        private TextBox txtHi1_1a;
        private Label label167;
        private TextBox txtLPT1_1;
        private TextBox txtRx_1a;
        private Label label170;
        private TextBox txtPwr_1a;
        private Label label174;
        private TextBox txtHi2_1a;
        private Label label173;
        private TextBox txtTx_1a;
        private Label label172;
        private TextBox txtHi2_10a;
        private TextBox txtTx_10a;
        private TextBox txtPwr_10a;
        private TextBox txtRx_10a;
        private TextBox txtHi1_10a;
        private TextBox txtLPT1_10;
        private Label label179;
        public CheckBox chkSO10a;
        private TextBox txtHi2_9a;
        private TextBox txtTx_9a;
        private TextBox txtPwr_9a;
        private TextBox txtRx_9a;
        private TextBox txtHi1_9a;
        private TextBox txtLPT1_9;
        private Label label180;
        public CheckBox chkSO9a;
        private TextBox txtHi2_8a;
        private TextBox txtTx_8a;
        private TextBox txtPwr_8a;
        private TextBox txtRx_8a;
        private TextBox txtHi1_8a;
        private TextBox txtLPT1_8;
        private Label label181;
        public CheckBox chkSO8a;
        private TextBox txtHi2_7a;
        private TextBox txtTx_7a;
        private TextBox txtPwr_7a;
        private TextBox txtRx_7a;
        private TextBox txtHi1_7a;
        private TextBox txtLPT1_7;
        private Label label182;
        public CheckBox chkSO7a;
        private TextBox txtHi2_6a;
        private TextBox txtTx_6a;
        private TextBox txtPwr_6a;
        private TextBox txtRx_6a;
        private TextBox txtHi1_6a;
        private TextBox txtLPT1_6;
        private Label label178;
        public CheckBox chkSO6a;
        private TextBox txtTx_5a;
        private TextBox txtPwr_5a;
        private TextBox txtRx_5a;
        private TextBox txtHi1_5a;
        private TextBox txtLPT1_5;
        private Label label177;
        public CheckBox chkSO5a;
        private TextBox txtHi2_4a;
        private TextBox txtTx_4a;
        private TextBox txtPwr_4a;
        private TextBox txtRx_4a;
        private TextBox txtHi1_4a;
        private TextBox txtLPT1_4;
        private Label label176;
        public CheckBox chkSO4a;
        private TextBox txtHi2_3a;
        private TextBox txtTx_3a;
        private TextBox txtPwr_3a;
        private TextBox txtRx_3a;
        private TextBox txtHi1_3a;
        private TextBox txtLPT1_3;
        private Label label175;
        public CheckBox chkSO3a;
        private TextBox txtHi2_2a;
        private TextBox txtTx_2a;
        private TextBox txtPwr_2a;
        private TextBox txtRx_2a;
        private TextBox txtHi1_2a;
        private TextBox txtLPT1_2;
        private Label label168;
        public CheckBox chkSO2a;
        private Label label183;
        private Label label189;
        private Label label185;
        private TextBox txtLo2_1b;
        private Label label186;
        private TextBox txtTx_1b;
        private Label label187;
        private Label label188;
        private TextBox txtPwr_1b;
        private TextBox txtRx_1b;
        private Label label194;
        private TextBox txtLo2_5b;
        private TextBox txtTx_5b;
        private TextBox txtPwr_5b;
        private TextBox txtRx_5b;
        private Label label193;
        private TextBox txtLo2_4b;
        private TextBox txtTx_4b;
        private TextBox txtPwr_4b;
        private TextBox txtRx_4b;
        private Label label192;
        private TextBox txtLo2_3b;
        private TextBox txtTx_3b;
        private TextBox txtPwr_3b;
        private TextBox txtRx_3b;
        private Label label191;
        private TextBox txtLo2_2b;
        private TextBox txtTx_2b;
        private TextBox txtPwr_2b;
        private TextBox txtRx_2b;
        private Label label190;
        private Label label195;
        private TextBox txtLo2_6b;
        private TextBox txtTx_6b;
        private TextBox txtPwr_6b;
        private TextBox txtRx_6b;
        private Label label199;
        private TextBox txtLo2_10b;
        private TextBox txtTx_10b;
        private TextBox txtPwr_10b;
        private TextBox txtRx_10b;
        private Label label198;
        private TextBox txtLo2_9b;
        private TextBox txtTx_9b;
        private TextBox txtPwr_9b;
        private TextBox txtRx_9b;
        private Label label197;
        private TextBox txtLo2_8b;
        private TextBox txtTx_8b;
        private TextBox txtPwr_8b;
        private TextBox txtRx_8b;
        private Label label196;
        private TextBox txtLo2_7b;
        private TextBox txtTx_7b;
        private TextBox txtPwr_7b;
        private TextBox txtRx_7b;
        private TextBox txtLo1_10b;
        private TextBox txtLo1_9b;
        private TextBox txtLo1_8b;
        private TextBox txtLo1_7b;
        private TextBox txtLo1_6b;
        private TextBox txtLo1_5b;
        private TextBox txtLo1_4b;
        private TextBox txtLo1_3b;
        private TextBox txtLo1_2b;
        private TextBox txtLo1_1b;
        private Label label201;
        private Label label184;
        private ComboBox cboSwPort;
        private CheckBox chkSoEnab;
        private Button btnSaveSO2R;
        private Label label203;
        private TextBox txtLPT2;
        private Label label205;
        private System.IO.Ports.SerialPort SwitchPort;
        private CheckBox chk1Amp;
        public CheckBox chkSO10b;
        public CheckBox chkSO9b;
        public CheckBox chkSO8b;
        public CheckBox chkSO7b;
        public CheckBox chkSO6b;
        public CheckBox chkSO5b;
        public CheckBox chkSO4b;
        public CheckBox chkSO3b;
        public CheckBox chkSO2b;
        public CheckBox chkSO1b;
        private Label label200;
        private TextBox txtHi2_5a;
        private Label label202;
        private Label label204;
        private Label label206;
        private Label label207;
        private Label label208;
        private Label label209;
        private Label label210;
        private Label label211;
        private Label label212;
        private Label label213;
        private Label label215;
        private TextBox txtTxB;
        private Label label214;
        private TextBox txtTxA;
        private GroupBox grpTKnob;
        private Label label216;
        private ComboBox cboKnobPort;
        public CheckBox chkKnobEnab;
        private Label label219;
        private Label label218;
        private Label label217;
        private ComboBox cboKnobADC;
        private ComboBox cboKnobAOn;
        private ComboBox cboKnobAOff;
        private System.IO.Ports.SerialPort KnobPort;
        private Label label221;
        private Label label220;
        private ComboBox cboKnobBDC;
        private ComboBox cboKnobBOn;
        private ComboBox cboKnobBOff;
        private TextBox Dev0;
        public CheckBox chkKnobExpert;
        private Label lblZC;
        private TextBox txtZC;
        private Label lblZR;
        private TextBox txtZR;
        private Label lblZL;
        private TextBox txtZL;
        private Button btnLoadSO2R;
        private Button btnSO2Rclear;
        private TextBox txtAM;
        private CheckBox chkAutoExpert;
        private TextBox txtDigi;
        private TextBox txtSSB;
        private Label label224;
        private Label label223;
        private Label label222;
        private Label label27;
        private ComboBox cboMacSwPort;
        private CheckBox chkMacSwEnab;
        private GroupBox grpMacSw;
        private Label label155;
        private System.IO.Ports.SerialPort MacroPort;
        private TextBox txtMacSwOff;
        private Label label226;
        private TextBox txtMacSwOn;
        private Label label225;
        private TextBox txtMacSwMsg;
        private ToolStripMenuItem antToolStripMenuItem;
        private ToolStripMenuItem recallToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem9;
        private TextBox txtVolts;
        public Button btnReCall;
        public Button btnMemSave;
        public Button btnMV;
        public Button btnMemClear;
        public TextBox txtMemFreq;
        public TextBox txtMemMode;
        public Button btnMemLoad;
        public TextBox txtMemIdx;
        public TextBox txtMemVfo;
        private CheckBox chkStepDipole;
        private GroupBox grpAuxBcdControl;
        private CheckBox chkAuxBit7;
        private CheckBox chkAuxBit8;
        private CheckBox chkAuxBit5;
        private CheckBox chkAuxBit3;
        private CheckBox chkAuxBit6;
        private CheckBox chkAuxBit1;
        private CheckBox chkAuxBit4;
        private CheckBox chkAuxBit2;
        private CheckBox chkAuxEnab;
        private Label label4;
        private CheckBox chkAuxMacro;
        private CheckBox chkFollowFreq;
        private PictureBox pixBox1;
        private Label label154;
        private Label label15;
        private Label label227;
        private CheckBox chkCwTx;
        private NotifyIcon notifyIcon1;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem slaveRadioInfoToolStripMenuItem;
        private ToolStripMenuItem iCOMICPW1ToolStripMenuItem1;
        private ToolStripMenuItem lP100ToolStripMenuItem1;
        private ToolStripMenuItem macroCommandsToolStripMenuItem;
        private ToolStripMenuItem rCPPortsToolStripMenuItem1;
        private ToolStripMenuItem rotorControlToolStripMenuItem1;
        private ToolStripMenuItem waveNodeToolStripMenuItem1;
        private ToolStripMenuItem aboutToolStripMenuItem2;
        private ToolStripMenuItem errorLogToolStripMenuItem;
        private ToolStripMenuItem enableLoggingToolStripMenuItem;
        private ToolStripMenuItem openErrorLogToolStripMenuItem1;
        private ToolStripMenuItem clearErrorLogToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator3;
        public ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem autoDriveToolStripMenuItem1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem barefootSettingsToolStripMenuItem1;
        private ToolStripMenuItem enableToolStripMenuItem;
        private ToolStripMenuItem enableExpertModeToolStripMenuItem;
        public RadioButton rbBiDir;
        public RadioButton rb180;
        public RadioButton rbFwd;
        public Button btnSplit;
        private ToolStripMenuItem shortCutKeysToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem updateToLatestVersionToolStripMenuItem;
        private ToolStripMenuItem getLatestVersionToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator9;
        public ContextMenuStrip contextMenuStrip2;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem10;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem toolStripMenuItem11;
        private ToolStripMenuItem toolStripMenuItem12;
        private ToolStripMenuItem toolStripMenuItem13;
        private ToolStripMenuItem toolStripMenuItem14;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem toolStripMenuItem15;
        private ToolStripMenuItem toolStripMenuItem16;
        private ToolStripMenuItem toolStripMenuItem17;
        private ToolStripMenuItem toolStripMenuItem18;
        private ToolStripMenuItem toolStripMenuItem19;
        private ToolStripMenuItem toolStripMenuItem20;
        private ToolStripMenuItem toolStripMenuItem21;
        private ToolStripMenuItem toolStripMenuItem22;
        private ToolStripMenuItem toolStripMenuItem23;
        private ToolStripMenuItem toolStripMenuItem24;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem toolStripMenuItem25;
        private ToolStripMenuItem toolStripMenuItem26;
        private ToolStripMenuItem userForumToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripMenuItem userForumToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripSeparator toolStripSeparator13;
        private ToolStripSeparator toolStripSeparator14;
        private ToolStripSeparator toolStripSeparator15;
        private ToolStripSeparator toolStripSeparator16;
        private ToolStripSeparator toolStripSeparator17;
        private Label label40;
        private TextBox txtCallSign;
        private Label label228;
        private ComboBox cboTstep;
        private CheckBox chkShortCut;
        private Label label232;
        private Label label231;
        private Label label230;
        private Label label229;
        private ComboBox cboTkSw3;
        private ComboBox cboTkSw1;
        private ComboBox cboTkSw2;
        private Label lblZE;
        private TextBox txtZE;
        private Button TkGrn;
        private Button TkYel;
        private Button TkRed;
        private Button button7;

    }
}

