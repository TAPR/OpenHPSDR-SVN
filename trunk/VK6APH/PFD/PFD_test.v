// test code for Phase Frequency Detector
// Divides both 10MHz reference signal and 125MHz VCXO 
// to give 2.5MHz signals.  These are fed to a Phase Frequency Detector (PFD)
// the output of which is filtered and fed to the VCXO. 
// The PFD has a lock detector that is active low. 
// The circuit checks that the 10MHz reference is present, if not 
// it connects the 2.5MHz from the VCXO to the PFD output so that 
// the VCXO LPF output is constant at 3.3v/2




module PFD_test(ref_in, osc_in, tune, LED1, LED2, C5, ref_2_5M, osc_2_5M);

input ref_in;		// 10MHz reference 
input osc_in;		// 125MHz from VCXO
output tune;		// signal to loop LPF
output LED1;
output LED2;
output C5;			// 12MHz signal over Atlas bus
wire tune;
wire pfd_out;
wire lock;			// is low when loop is locked
output ref_2_5M;
output osc_2_5M;


reg ref_2_5M;
reg osc_2_5M;
reg ref_OK;			// is low when reference signal is present
wire cout1;
wire cout2;
wire C5;

assign C5 = osc_in; 


// divide 10MHz reference clock by 4 to give 2.5MHz

reg [2:0]ref_count;

always @ (posedge ref_in)
begin
	if (ref_count == 1)
	begin
		ref_2_5M <= ~ref_2_5M;
		ref_count <= 0;
	end
	else ref_count <= ref_count + 1;
end

// divide 125MHz  clock by 50 to give 2.5MHz

reg [5:0]osc_count;

always @ (posedge osc_in)
begin
	if (osc_count == 24)
	begin
		osc_2_5M <= ~osc_2_5M;
		osc_count <= 0;
	end 
	else osc_count <= osc_count + 1;
end

// apply to PFD

//pfd Janus_pfd(.ref_in(ref_2_5M),.osc_in(osc_2_5M),.pfd_out(pfd_out),.lock(lock));

//
// check if the 10MHz reference signal is present.
// if so use PFD output else use 2.5MHz so as to set the 
// VCXO control voltage to 3.3v/2 
//

//always @ (posedge ref_in or posedge osc_5M)
//begin 
//if (ref_in)ref_OK <= 1'b0;   // ref_OK goes low if ref present so we can drive an LED
//else ref_OK <= 1'b1;		 // goes high if no reference 
//end 

// select the signal to send to the loop LPF depending if the 10MHz reference is present
//assign  tune = ref_OK ?  osc_5M : pfd_out; 

//assign  tune = pfd_out; 

wire pre_tune = ref_2_5M ^ osc_2_5M;
//wire pre_tune = pfd_out;

reg pre_tune_d;
reg pre_tune_dd;

always @(posedge osc_in)
  begin
    pre_tune_d <=  pre_tune;
    pre_tune_dd <= pre_tune_d;
  end

assign tune = pre_tune_dd;


// LEDs for testing


assign LED1 = ref_OK;
//assign LED2 = osc_8k;

assign LED2 = (lock | ref_OK);  // low when loop is locked and we have a reference signal 


endmodule
