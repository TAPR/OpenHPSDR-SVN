namespace DataDecoder
{
    partial class SetupWiz
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Select end 1 of virtual serial port pair connected to RCP CAT.");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("*Select end 1 of virtual serial port pair connected to RCP Rotor.");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Ports Tab (RCP1 CAT)", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Select end 2 of virtual serial pair connected to DDUtil RCP1 CAT.");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("*Select end 2 of virtual serial pair connected to DDUtil RCP1 Rotor.");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Radio Control Program", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("See DDUtil wiki (How-To: Setup a Radio Control Program) for RCP2~4 and additional" +
                    " info.");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("* Indicate an optional feature.");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Setup BCD file in Device tab");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Check the Enable checkbox");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Device Tab", new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Select the parallel port you wish to use");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Select device to use");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Check the Enable checkbox");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Ports Tab (Parallel Port group)", new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode13,
            treeNode14});
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Attach cable to port and device");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("See DDUtil wiki (How-To: BCD Devices) for additionsl info.");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("BCD Device", new System.Windows.Forms.TreeNode[] {
            treeNode11,
            treeNode15,
            treeNode16,
            treeNode17});
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Select serial port for Pass Listen");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Ports Tab (Serial Ports group)", new System.Windows.Forms.TreeNode[] {
            treeNode19});
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Select Comm parameters in the PL/Radio Comm window.");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Ports Tab (Slave Radio group)", new System.Windows.Forms.TreeNode[] {
            treeNode21});
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Setup Comm parameters to match DDUtil settings.");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Passive Listener Device", new System.Windows.Forms.TreeNode[] {
            treeNode23});
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("See DDUtil wiki (How-To: RS-232 PL Devices) for additionsl info.");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("RS-232 Device", new System.Windows.Forms.TreeNode[] {
            treeNode20,
            treeNode22,
            treeNode24,
            treeNode25});
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("A slave radio can be made to follow PowerSDR in lieu of a Passive Listener device" +
                    ".");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("See DDUtil wiki (Setup: Other) for additionsl info.");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Slave Radio (Slave Radio group)", new System.Windows.Forms.TreeNode[] {
            treeNode27,
            treeNode28});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupWiz));
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Select a port for end 1 of a virtual serial port pair connected to PowerSDR\'s CAT" +
                    " port.");
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Select the interval (ms) for DDUtil to poll PowerSDR for data (200~500 is typical" +
                    ").");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Ports Tab (Serial Ports group)", new System.Windows.Forms.TreeNode[] {
            treeNode30,
            treeNode31});
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("Disable the Enable checkbox if enabled.");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("Select a port for the other end of a virtual pair connected to DDUtil\'s Radio CAT" +
                    " port.");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("Select the Enable checkbox. ");
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("PowerSDR (Setup | CAT Control tab)", new System.Windows.Forms.TreeNode[] {
            treeNode33,
            treeNode34,
            treeNode35});
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("Radio CAT Port", new System.Windows.Forms.TreeNode[] {
            treeNode32,
            treeNode36});
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("Setup BCD file in Device tab");
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("Check the Enable checkbox");
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("Device Tab", new System.Windows.Forms.TreeNode[] {
            treeNode38,
            treeNode39});
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("Select the parallel port you wish to use");
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("Select device to use");
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("Check the Enable checkbox");
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("Ports Tab (Parallel Port group)", new System.Windows.Forms.TreeNode[] {
            treeNode41,
            treeNode42,
            treeNode43});
            System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("Attach cable to port and device");
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("See DDUtil wiki (How-To: BCD Devices) for additionsl info.");
            System.Windows.Forms.TreeNode treeNode47 = new System.Windows.Forms.TreeNode("BCD Device", new System.Windows.Forms.TreeNode[] {
            treeNode40,
            treeNode44,
            treeNode45,
            treeNode46});
            System.Windows.Forms.TreeNode treeNode48 = new System.Windows.Forms.TreeNode("Select serial port for Pass Listen");
            System.Windows.Forms.TreeNode treeNode49 = new System.Windows.Forms.TreeNode("Ports Tab (Serial Ports group)", new System.Windows.Forms.TreeNode[] {
            treeNode48});
            System.Windows.Forms.TreeNode treeNode50 = new System.Windows.Forms.TreeNode("Select Comm parameters in the PL/Radio Comm window.");
            System.Windows.Forms.TreeNode treeNode51 = new System.Windows.Forms.TreeNode("Ports Tab (Slave Radio group)", new System.Windows.Forms.TreeNode[] {
            treeNode50});
            System.Windows.Forms.TreeNode treeNode52 = new System.Windows.Forms.TreeNode("Setup Comm parameters to match DDUtil settings.");
            System.Windows.Forms.TreeNode treeNode53 = new System.Windows.Forms.TreeNode("Passive Listener Device", new System.Windows.Forms.TreeNode[] {
            treeNode52});
            System.Windows.Forms.TreeNode treeNode54 = new System.Windows.Forms.TreeNode("See DDUtil wiki (How-To: RS-232 PL Devices) for additionsl info.");
            System.Windows.Forms.TreeNode treeNode55 = new System.Windows.Forms.TreeNode("RS-232 Device", new System.Windows.Forms.TreeNode[] {
            treeNode49,
            treeNode51,
            treeNode53,
            treeNode54});
            System.Windows.Forms.TreeNode treeNode56 = new System.Windows.Forms.TreeNode("A slave radio can be made to follow PowerSDR in lieu of a Passive Listener device" +
                    ".");
            System.Windows.Forms.TreeNode treeNode57 = new System.Windows.Forms.TreeNode("See DDUtil wiki (Setup: Other) for additionsl info.");
            System.Windows.Forms.TreeNode treeNode58 = new System.Windows.Forms.TreeNode("Slave Radio (Slave Radio group)", new System.Windows.Forms.TreeNode[] {
            treeNode56,
            treeNode57});
            System.Windows.Forms.TreeNode treeNode59 = new System.Windows.Forms.TreeNode("Select end 1 of virtual serial port pair connected to RCP CAT.");
            System.Windows.Forms.TreeNode treeNode60 = new System.Windows.Forms.TreeNode("*Select end 1 of virtual serial port pair connected to RCP Rotor.");
            System.Windows.Forms.TreeNode treeNode61 = new System.Windows.Forms.TreeNode("Ports Tab (RCP1 CAT)", new System.Windows.Forms.TreeNode[] {
            treeNode59,
            treeNode60});
            System.Windows.Forms.TreeNode treeNode62 = new System.Windows.Forms.TreeNode("Select end 2 of virtual serial pair connected to DDUtil RCP1 CAT.");
            System.Windows.Forms.TreeNode treeNode63 = new System.Windows.Forms.TreeNode("*Select end 2 of virtual serial pair connected to DDUtil RCP1 Rotor.");
            System.Windows.Forms.TreeNode treeNode64 = new System.Windows.Forms.TreeNode("Radio Control Program", new System.Windows.Forms.TreeNode[] {
            treeNode62,
            treeNode63});
            System.Windows.Forms.TreeNode treeNode65 = new System.Windows.Forms.TreeNode("See DDUtil wiki (How-To: Setup a Radio Control Program) for RCP2~4 and additional" +
                    " info.");
            System.Windows.Forms.TreeNode treeNode66 = new System.Windows.Forms.TreeNode("* Indicate an optional feature.");
            System.Windows.Forms.TreeNode treeNode67 = new System.Windows.Forms.TreeNode("Select the serial port connected to the rotator.");
            System.Windows.Forms.TreeNode treeNode68 = new System.Windows.Forms.TreeNode("Select the correct communication speed and data format for the rotator.");
            System.Windows.Forms.TreeNode treeNode69 = new System.Windows.Forms.TreeNode("Select the rotator model (and speed if shown).");
            System.Windows.Forms.TreeNode treeNode70 = new System.Windows.Forms.TreeNode("For additional info see the DDUtil wiki (How-To: Setup Rotor Functions).");
            System.Windows.Forms.TreeNode treeNode71 = new System.Windows.Forms.TreeNode("Setup Tab (Rotor Setup)", new System.Windows.Forms.TreeNode[] {
            treeNode67,
            treeNode68,
            treeNode69,
            treeNode70});
            this.wizardControl1 = new WizardBase.WizardControl();
            this.treeView5 = new System.Windows.Forms.TreeView();
            this.treeView3 = new System.Windows.Forms.TreeView();
            this.startStep1 = new WizardBase.StartStep();
            this.label1 = new System.Windows.Forms.Label();
            this.intermediateStep1 = new WizardBase.IntermediateStep();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.intermediateStep2 = new WizardBase.IntermediateStep();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.intermediateStep3 = new WizardBase.IntermediateStep();
            this.treeView4 = new System.Windows.Forms.TreeView();
            this.intermediateStep4 = new WizardBase.IntermediateStep();
            this.treeView6 = new System.Windows.Forms.TreeView();
            this.finishStep1 = new WizardBase.FinishStep();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.wizardControl1.SuspendLayout();
            this.startStep1.SuspendLayout();
            this.intermediateStep1.SuspendLayout();
            this.intermediateStep2.SuspendLayout();
            this.intermediateStep3.SuspendLayout();
            this.intermediateStep4.SuspendLayout();
            this.finishStep1.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizardControl1
            // 
            this.wizardControl1.BackButtonEnabled = true;
            this.wizardControl1.BackButtonVisible = true;
            this.wizardControl1.CancelButtonEnabled = true;
            this.wizardControl1.CancelButtonVisible = true;
            this.wizardControl1.Controls.Add(this.treeView5);
            this.wizardControl1.Controls.Add(this.treeView3);
            this.wizardControl1.EulaButtonEnabled = true;
            this.wizardControl1.EulaButtonText = "eula";
            this.wizardControl1.EulaButtonVisible = true;
            this.wizardControl1.HelpButtonEnabled = true;
            this.wizardControl1.HelpButtonVisible = true;
            this.wizardControl1.Location = new System.Drawing.Point(0, 0);
            this.wizardControl1.Name = "wizardControl1";
            this.wizardControl1.NextButtonEnabled = true;
            this.wizardControl1.NextButtonVisible = true;
            this.wizardControl1.Size = new System.Drawing.Size(534, 403);
            this.wizardControl1.WizardSteps.AddRange(new WizardBase.WizardStep[] {
            this.startStep1,
            this.intermediateStep1,
            this.intermediateStep2,
            this.intermediateStep3,
            this.intermediateStep4,
            this.finishStep1});
            this.wizardControl1.FinishButtonClick += new System.EventHandler(this.wizardControl1_FinishButtonClick);
            // 
            // treeView5
            // 
            this.treeView5.BackColor = System.Drawing.SystemColors.Control;
            this.treeView5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView5.Location = new System.Drawing.Point(10, 69);
            this.treeView5.Name = "treeView5";
            treeNode1.Name = "Node1";
            treeNode1.Text = "Select end 1 of virtual serial port pair connected to RCP CAT.";
            treeNode2.Name = "Node8";
            treeNode2.Text = "*Select end 1 of virtual serial port pair connected to RCP Rotor.";
            treeNode3.Name = "Node4";
            treeNode3.Text = "Ports Tab (RCP1 CAT)";
            treeNode4.Name = "Node6";
            treeNode4.Text = "Select end 2 of virtual serial pair connected to DDUtil RCP1 CAT.";
            treeNode5.Name = "Node3";
            treeNode5.Text = "*Select end 2 of virtual serial pair connected to DDUtil RCP1 Rotor.";
            treeNode6.Name = "Node5";
            treeNode6.Text = "Radio Control Program";
            treeNode7.Name = "Node11";
            treeNode7.Text = "See DDUtil wiki (How-To: Setup a Radio Control Program) for RCP2~4 and additional" +
                " info.";
            treeNode8.Name = "Node9";
            treeNode8.Text = "* Indicate an optional feature.";
            this.treeView5.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode6,
            treeNode7,
            treeNode8});
            this.treeView5.Size = new System.Drawing.Size(510, 275);
            this.treeView5.TabIndex = 10;
            // 
            // treeView3
            // 
            this.treeView3.BackColor = System.Drawing.SystemColors.Control;
            this.treeView3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView3.Location = new System.Drawing.Point(12, 64);
            this.treeView3.Name = "treeView3";
            treeNode9.Name = "Node1";
            treeNode9.Text = "Setup BCD file in Device tab";
            treeNode10.Name = "Node8";
            treeNode10.Text = "Check the Enable checkbox";
            treeNode11.Name = "Node4";
            treeNode11.Text = "Device Tab";
            treeNode12.Name = "Node3";
            treeNode12.Text = "Select the parallel port you wish to use";
            treeNode13.Name = "Node6";
            treeNode13.Text = "Select device to use";
            treeNode14.Name = "Node7";
            treeNode14.Text = "Check the Enable checkbox";
            treeNode15.Name = "Node5";
            treeNode15.Text = "Ports Tab (Parallel Port group)";
            treeNode16.Name = "Node9";
            treeNode16.Text = "Attach cable to port and device";
            treeNode17.Name = "Node11";
            treeNode17.Text = "See DDUtil wiki (How-To: BCD Devices) for additionsl info.";
            treeNode18.Name = "Node0";
            treeNode18.Text = "BCD Device";
            treeNode19.Name = "Node13";
            treeNode19.Text = "Select serial port for Pass Listen";
            treeNode20.Name = "Node12";
            treeNode20.Text = "Ports Tab (Serial Ports group)";
            treeNode21.Name = "Node16";
            treeNode21.Text = "Select Comm parameters in the PL/Radio Comm window.";
            treeNode22.Name = "Node15";
            treeNode22.Text = "Ports Tab (Slave Radio group)";
            treeNode23.Name = "Node18";
            treeNode23.Text = "Setup Comm parameters to match DDUtil settings.";
            treeNode24.Name = "Node17";
            treeNode24.Text = "Passive Listener Device";
            treeNode25.Name = "Node14";
            treeNode25.Text = "See DDUtil wiki (How-To: RS-232 PL Devices) for additionsl info.";
            treeNode26.Name = "Node2";
            treeNode26.Text = "RS-232 Device";
            treeNode27.Name = "Node20";
            treeNode27.Text = "A slave radio can be made to follow PowerSDR in lieu of a Passive Listener device" +
                ".";
            treeNode28.Name = "Node21";
            treeNode28.Text = "See DDUtil wiki (Setup: Other) for additionsl info.";
            treeNode29.Name = "Node19";
            treeNode29.Text = "Slave Radio (Slave Radio group)";
            this.treeView3.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode18,
            treeNode26,
            treeNode29});
            this.treeView3.Size = new System.Drawing.Size(510, 275);
            this.treeView3.TabIndex = 9;
            // 
            // startStep1
            // 
            this.startStep1.BindingImage = ((System.Drawing.Image)(resources.GetObject("startStep1.BindingImage")));
            this.startStep1.Controls.Add(this.label1);
            this.startStep1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.startStep1.Icon = null;
            this.startStep1.Name = "startStep1";
            this.startStep1.Subtitle = resources.GetString("startStep1.Subtitle");
            this.startStep1.Title = "Welcome to the DDUtil Setup Wizard";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(220, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 0;
            // 
            // intermediateStep1
            // 
            this.intermediateStep1.BackColor = System.Drawing.SystemColors.Control;
            this.intermediateStep1.BindingImage = null;
            this.intermediateStep1.Controls.Add(this.treeView2);
            this.intermediateStep1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.intermediateStep1.HeaderPair = ((WizardBase.ColorPair)(resources.GetObject("intermediateStep1.HeaderPair")));
            this.intermediateStep1.Name = "intermediateStep1";
            this.intermediateStep1.Subtitle = "\r\nFollow the directions below to setup the CAT ports for DDUtil and PowerSDR.";
            this.intermediateStep1.Title = "Setup the Radio CAT Port";
            // 
            // treeView2
            // 
            this.treeView2.BackColor = System.Drawing.SystemColors.Control;
            this.treeView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView2.Location = new System.Drawing.Point(10, 80);
            this.treeView2.Name = "treeView2";
            treeNode30.Name = "Node3";
            treeNode30.Text = "Select a port for end 1 of a virtual serial port pair connected to PowerSDR\'s CAT" +
                " port.";
            treeNode31.Name = "Node11";
            treeNode31.Text = "Select the interval (ms) for DDUtil to poll PowerSDR for data (200~500 is typical" +
                ").";
            treeNode32.Name = "Node5";
            treeNode32.Text = "Ports Tab (Serial Ports group)";
            treeNode33.Name = "Node1";
            treeNode33.Text = "Disable the Enable checkbox if enabled.";
            treeNode34.Name = "Node2";
            treeNode34.Text = "Select a port for the other end of a virtual pair connected to DDUtil\'s Radio CAT" +
                " port.";
            treeNode35.Name = "Node3";
            treeNode35.Text = "Select the Enable checkbox. ";
            treeNode36.Name = "Node0";
            treeNode36.Text = "PowerSDR (Setup | CAT Control tab)";
            treeNode37.Name = "Node0";
            treeNode37.Text = "Radio CAT Port";
            this.treeView2.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode37});
            this.treeView2.Size = new System.Drawing.Size(510, 275);
            this.treeView2.TabIndex = 9;
            // 
            // intermediateStep2
            // 
            this.intermediateStep2.BackColor = System.Drawing.SystemColors.Control;
            this.intermediateStep2.BindingImage = null;
            this.intermediateStep2.Controls.Add(this.treeView1);
            this.intermediateStep2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.intermediateStep2.HeaderPair = ((WizardBase.ColorPair)(resources.GetObject("intermediateStep2.HeaderPair")));
            this.intermediateStep2.Name = "intermediateStep2";
            this.intermediateStep2.Subtitle = "\r\nFollow the directions below to setup a Passive Listener device.";
            this.intermediateStep2.Title = "Setup Passive Listener Devices";
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.SystemColors.Control;
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Location = new System.Drawing.Point(10, 70);
            this.treeView1.Name = "treeView1";
            treeNode38.Name = "Node1";
            treeNode38.Text = "Setup BCD file in Device tab";
            treeNode39.Name = "Node8";
            treeNode39.Text = "Check the Enable checkbox";
            treeNode40.Name = "Node4";
            treeNode40.Text = "Device Tab";
            treeNode41.Name = "Node3";
            treeNode41.Text = "Select the parallel port you wish to use";
            treeNode42.Name = "Node6";
            treeNode42.Text = "Select device to use";
            treeNode43.Name = "Node7";
            treeNode43.Text = "Check the Enable checkbox";
            treeNode44.Name = "Node5";
            treeNode44.Text = "Ports Tab (Parallel Port group)";
            treeNode45.Name = "Node9";
            treeNode45.Text = "Attach cable to port and device";
            treeNode46.Name = "Node11";
            treeNode46.Text = "See DDUtil wiki (How-To: BCD Devices) for additionsl info.";
            treeNode47.Name = "Node0";
            treeNode47.Text = "BCD Device";
            treeNode48.Name = "Node13";
            treeNode48.Text = "Select serial port for Pass Listen";
            treeNode49.Name = "Node12";
            treeNode49.Text = "Ports Tab (Serial Ports group)";
            treeNode50.Name = "Node16";
            treeNode50.Text = "Select Comm parameters in the PL/Radio Comm window.";
            treeNode51.Name = "Node15";
            treeNode51.Text = "Ports Tab (Slave Radio group)";
            treeNode52.Name = "Node18";
            treeNode52.Text = "Setup Comm parameters to match DDUtil settings.";
            treeNode53.Name = "Node17";
            treeNode53.Text = "Passive Listener Device";
            treeNode54.Name = "Node14";
            treeNode54.Text = "See DDUtil wiki (How-To: RS-232 PL Devices) for additionsl info.";
            treeNode55.Name = "Node2";
            treeNode55.Text = "RS-232 Device";
            treeNode56.Name = "Node20";
            treeNode56.Text = "A slave radio can be made to follow PowerSDR in lieu of a Passive Listener device" +
                ".";
            treeNode57.Name = "Node21";
            treeNode57.Text = "See DDUtil wiki (Setup: Other) for additionsl info.";
            treeNode58.Name = "Node19";
            treeNode58.Text = "Slave Radio (Slave Radio group)";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode47,
            treeNode55,
            treeNode58});
            this.treeView1.Size = new System.Drawing.Size(510, 275);
            this.treeView1.TabIndex = 8;
            // 
            // intermediateStep3
            // 
            this.intermediateStep3.BackColor = System.Drawing.SystemColors.Control;
            this.intermediateStep3.BindingImage = null;
            this.intermediateStep3.Controls.Add(this.treeView4);
            this.intermediateStep3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.intermediateStep3.HeaderPair = ((WizardBase.ColorPair)(resources.GetObject("intermediateStep3.HeaderPair")));
            this.intermediateStep3.Name = "intermediateStep3";
            this.intermediateStep3.Subtitle = "\r\nFollow the directions below to setup a Radio Control Program.";
            this.intermediateStep3.Title = "Setup a Radio Control Program (RCP)";
            // 
            // treeView4
            // 
            this.treeView4.BackColor = System.Drawing.SystemColors.Control;
            this.treeView4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView4.Location = new System.Drawing.Point(10, 75);
            this.treeView4.Name = "treeView4";
            treeNode59.Name = "Node1";
            treeNode59.Text = "Select end 1 of virtual serial port pair connected to RCP CAT.";
            treeNode60.Name = "Node8";
            treeNode60.Text = "*Select end 1 of virtual serial port pair connected to RCP Rotor.";
            treeNode61.Name = "Node4";
            treeNode61.Text = "Ports Tab (RCP1 CAT)";
            treeNode62.Name = "Node6";
            treeNode62.Text = "Select end 2 of virtual serial pair connected to DDUtil RCP1 CAT.";
            treeNode63.Name = "Node3";
            treeNode63.Text = "*Select end 2 of virtual serial pair connected to DDUtil RCP1 Rotor.";
            treeNode64.Name = "Node5";
            treeNode64.Text = "Radio Control Program";
            treeNode65.Name = "Node11";
            treeNode65.Text = "See DDUtil wiki (How-To: Setup a Radio Control Program) for RCP2~4 and additional" +
                " info.";
            treeNode66.Name = "Node9";
            treeNode66.Text = "* Indicate an optional feature.";
            this.treeView4.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode61,
            treeNode64,
            treeNode65,
            treeNode66});
            this.treeView4.Size = new System.Drawing.Size(510, 275);
            this.treeView4.TabIndex = 9;
            // 
            // intermediateStep4
            // 
            this.intermediateStep4.BindingImage = null;
            this.intermediateStep4.Controls.Add(this.treeView6);
            this.intermediateStep4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.intermediateStep4.HeaderPair = ((WizardBase.ColorPair)(resources.GetObject("intermediateStep4.HeaderPair")));
            this.intermediateStep4.Name = "intermediateStep4";
            this.intermediateStep4.Subtitle = "\r\nFollow the directions below to setup a Rotator.";
            this.intermediateStep4.Title = "Setup a Rotator";
            // 
            // treeView6
            // 
            this.treeView6.BackColor = System.Drawing.SystemColors.Control;
            this.treeView6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView6.Location = new System.Drawing.Point(10, 75);
            this.treeView6.Name = "treeView6";
            treeNode67.Name = "Node1";
            treeNode67.Text = "Select the serial port connected to the rotator.";
            treeNode68.Name = "Node8";
            treeNode68.Text = "Select the correct communication speed and data format for the rotator.";
            treeNode69.Name = "Node5";
            treeNode69.Text = "Select the rotator model (and speed if shown).";
            treeNode70.Name = "Node11";
            treeNode70.Text = "For additional info see the DDUtil wiki (How-To: Setup Rotor Functions).";
            treeNode71.Name = "Node4";
            treeNode71.Text = "Setup Tab (Rotor Setup)";
            this.treeView6.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode71});
            this.treeView6.Size = new System.Drawing.Size(510, 275);
            this.treeView6.TabIndex = 10;
            // 
            // finishStep1
            // 
            this.finishStep1.BindingImage = null;
            this.finishStep1.Controls.Add(this.textBox1);
            this.finishStep1.Name = "finishStep1";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Comic Sans MS", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.textBox1.ForeColor = System.Drawing.Color.Blue;
            this.textBox1.Location = new System.Drawing.Point(30, 110);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(455, 110);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Thanks for using the DDUtil Setup Wizard.";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SetupWiz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 400);
            this.Controls.Add(this.wizardControl1);
            this.Name = "SetupWiz";
            this.Text = "DDUtil Setup Wizard";
            this.wizardControl1.ResumeLayout(false);
            this.startStep1.ResumeLayout(false);
            this.startStep1.PerformLayout();
            this.intermediateStep1.ResumeLayout(false);
            this.intermediateStep2.ResumeLayout(false);
            this.intermediateStep3.ResumeLayout(false);
            this.intermediateStep4.ResumeLayout(false);
            this.finishStep1.ResumeLayout(false);
            this.finishStep1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private WizardBase.WizardControl wizardControl1;
        private WizardBase.StartStep startStep1;
        private WizardBase.IntermediateStep intermediateStep1;
        private WizardBase.FinishStep finishStep1;
        private System.Windows.Forms.Label label1;
        private WizardBase.IntermediateStep intermediateStep2;
        private WizardBase.IntermediateStep intermediateStep3;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.TreeView treeView3;
        private System.Windows.Forms.TreeView treeView4;
        private WizardBase.IntermediateStep intermediateStep4;
        private System.Windows.Forms.TreeView treeView5;
        private System.Windows.Forms.TreeView treeView6;
        private System.Windows.Forms.TextBox textBox1;
    }
}