// test code for Phase Frequency Detector
// Divides both 10MHz reference signal and 12.288MHz VCXO 
// to give 8kHz signals.  These are fed to a Phase Frequency Detector (PFD)
// the output of which is filtered and fed to the VCXO. 
// The PFD has a lock detector that is active low. 
// The circuit checks that the 10MHz reference is present, if not 
// it connects the 1kHz from the VCXO to the PFD output so that 
// the VCXO LPF output is constant at 3.3v/2

// V1.1 11th Nov 2006 changed ref to 8kHz since this is the HCF of 10MHz and 12.288MHz that
// allows an integer divider for both signals
// 27th November - changed to use Megafunctions for the dividers and moved to Janus CPLD


module PFD_test(ref_in, osc_in, tune, LED1, LED2, C5);

input ref_in;		// 10MHz reference 
input osc_in;		// 12.288MHz from VCXO
output tune;		// signal to loop LPF
output LED1;
output LED2;
output C5;			// 12MHz signal over Atlas bus
wire tune;
wire pfd_out;
wire lock;			// is low when loop is locked
reg ref_8k;
reg osc_8k;
reg ref_OK;			// is low when reference signal is present
wire cout1;
wire cout2;
wire C5;

assign C5 = osc_in; 


// divide 10MHz reference clock by 625 to give  16kHz
counter1 counter1(.clock(ref_in), .cout(cout1));

// divide by 2 to give a square wave
always @ (posedge cout1)
	ref_8k <= ~ref_8k;


// divide 12.288MHz ADC  clock by 768 to give 16kHz
counter2 counter2(.clock(osc_in), .cout(cout2));

// divide by 2 to give a square wave
always @ (posedge cout2)
	osc_8k <= ~osc_8k;


// apply to PFD

pfd Janus_pfd(.ref_in(ref_8k),.osc_in(osc_8k),.pfd_out(pfd_out),.lock(lock));

//
// check if the 10MHz reference signal is present.
// if so use PFD output else use 4kHz so as to set the 
// VCXO control voltage to 3.3v/2 
//

always @ (posedge ref_in or posedge osc_8k)
begin 
if (ref_in)ref_OK <= 1'b0;   // ref_OK goes low if ref present so we can drive an LED
else ref_OK <= 1'b1;		 // goes high if no reference 
end 

// select the signal to send to the loop LPF depending if the 10MHz reference is present
assign  tune = ref_OK ?  osc_8k : pfd_out; 

//assign  tune = pfd_out; 

//assign tune = ~ref_8k ^ osc_8k;

// LEDs for testing


assign LED1 = ref_OK;
//assign LED2 = osc_8k;

assign LED2 = (lock | ref_OK);  // low when loop is locked and we have a reference signal 


endmodule