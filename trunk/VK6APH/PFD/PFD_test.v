// test code for Phase Frequency Detector
// Divides both 10MHz reference signal and 12.288MHz VCXO 
// to give 1kHz signals.  These are fed to a Phase Frequency Detector (PFD)
// the output of which is filtered and fed to the VCXO. 
// The PFD has a lock detector that is active low. 
// The circuit checks that the 10MHz reference is present, if not 
// it connects the 1kHz from the VCXO to the PFD output so that 
// the VCXO LPF output is constant at 3.3v/2

// NOTE: Check odd clitch in output when runing off 1kHz derived from 
// VCXO - may have to divide FX2 clock to fix. 


module PFD_test(MHz, ref_in, ref_out, osc_in, VCXO_out, LED);

input MHz;			// 48MHz from FX2 for testing
input ref_in;		// will be 10MHz from the Altas bus
input osc_in;		// 12.288MHz from VCXO
output VCXO_out;	// signal to loop LPF
output [7:0]LED;
output ref_out;		// used for testing using 48MHz from FX2

wire VCXO_out;
wire pfd_out;
wire lock;				// is low when loop is locked
reg ref_1k;
reg osc_1k;
reg ref_OK;				// is low when reference signal is present
reg [14:0]ref_count;
reg [12:0]osc_count;

assign ref_out = MHz;	// for testing - remove when 10MHz is available



// divide 48MHz reference clock down to 1kHz

always @ (posedge ref_in)
begin
	if (ref_count == 15'd24008) 
		begin 
			ref_1k <= ~ref_1k;
			ref_count <= 15'd0;
		end
	else ref_count <= ref_count + 15'b1;
end 

// divide 12.288MHz ADC  clock down to 1kHz

always @ (posedge osc_in)
begin
	if (osc_count == 13'd6144) 
		begin 
			osc_1k <= ~osc_1k;
			osc_count <= 13'd0;
		end
	else osc_count <= osc_count + 13'b1;
end 

// apply to PFD

 pfd Janus_pfd(.ref_in(ref_1k),.osc_in(osc_1k),.pfd_out(pfd_out),.lock(lock));

//
// check if the 10MHz reference signal is present.
// if so use PFD output else use 1kHz so as to set the 
// VCXO control voltage to 3.3v/2 
//

always @ (posedge ref_in or posedge osc_1k)
begin 
if (ref_in)ref_OK <= 1'b0;   // ref_OK goes low if ref present so we can drive an LED
else ref_OK <= 1'b1;		 // goes high if no reference 
end 

// select the signal to send to the loop LPF depending if the 10MHz reference is present
assign  VCXO_out = ref_OK ?  osc_1k : pfd_out; 

// LEDs for testing

assign LED[0] = ref_in;
assign LED[1] = osc_in;
assign LED[2] = ref_1k;
assign LED[3] = osc_1k;
assign LED[4] = pfd_out;
assign LED[5] = 1'b1;
assign LED[6] = ref_OK;
assign LED[7] = (lock | ref_OK);  // low when loop is locked and we have a reference signal 


endmodule