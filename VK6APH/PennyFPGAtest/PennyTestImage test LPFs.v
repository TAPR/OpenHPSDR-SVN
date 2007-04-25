//
// Copyright (c) Bill Tracey 2006 (bill@ewjt.com) Phil Harman 2007 VK6APH 
//
// Simple test load for the Penny FPGA -- takes in a clock, divides it down via a register and feeds it out to all the FPGA pins so one can
// validate board wiring.

// This version uses the 10MHz clock source.

// This code was used to check the operation of the various LPFs, Tx relay etc.


module PennyTestImage(
   	//input  _125MHZ,
	//input  _10MHZ,
	output TXRX,
	output nTXRX,
	output _60MLPF,
	output n60MLPF,
	output _20MLPF,
	output n20MLPF,
	output _40MLPF,
	output n40MLPF,
	output _10MLPF,
	output n10MLPF,
	output _160MLPF,
	output n160MLPF,
	output _6MLPF,
	output n6MLPF,
	output _80MLPF,
	output n80MLPF,
	output _30MLPF,
	output n30MLPF 

	); 

       
	assign TXRX  = 1'b1;		// select Rx
	assign nTXRX = !TXRX;
	assign _60MLPF = 0;		
	assign n60MLPF = !_60MLPF;
	assign _20MLPF = 0;
	assign n20MLPF = !_20MLPF;
	assign _40MLPF = 0;
	assign n40MLPF = !_40MLPF;
	assign _10MLPF = 1'b1;		// select 10m LPF 
	assign n10MLPF = !_10MLPF;
	assign _160MLPF = 0;
	assign n160MLPF = !_160MLPF;
	assign _6MLPF = 0;
	assign n6MLPF = !_6MLPF;
	assign _80MLPF = 0;
	assign n80MLPF = !_80MLPF;
	assign _30MLPF = 0;
	assign n30MLPF = !_30MLPF;


endmodule
