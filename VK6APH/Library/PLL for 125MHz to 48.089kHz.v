/*      
	  The following code provides clocks as required for the TLV320, 
        PWM D/A converters. The first two  always operate at 48kHz.
        Note the phase relationship required betweeen LRCLK and BCLK.
        LRCLK must change state on the negative edge of BCLK.
*/

// Use PLL megafunction to generate 24.621212MHz from 125MHz clock (divide by 13/66)
/*
wire _24MHZ;

PLL PLL_24MHZ(.inclk0(clock),.c0(_24MHZ),.locked());

// divide _24MHZ from PLL by 2 to give 12.31MHz. This is then used as the PCLK_12MHZ 
// and will divide down to 48.08Khz. 

reg PCLK_12MHZ;

always @ (posedge _24MHZ)
begin
	PCLK_12MHZ <= ~PCLK_12MHZ;
end

*/