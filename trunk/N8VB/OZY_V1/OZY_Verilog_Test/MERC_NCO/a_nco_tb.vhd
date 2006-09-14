--	Copyright (C) 1988-2004 Altera Corporation

--	Any megafunction design, and related net list (encrypted or decrypted),
--	support information, device programming or simulation file, and any other
--	associated documentation or information provided by Altera or a partner
--	under Altera's Megafunction Partnership Program may be used only to
--	program PLD devices (but not masked PLD devices) from Altera.  Any other
--	use of such megafunction design, net list, support information, device
--	programming or simulation file, or any other related documentation or
--	information is prohibited for any other purpose, including, but not
--	limited to modification, reverse engineering, de-compiling, or use with
--	any other silicon devices, unless such use is explicitly licensed under
--	a separate agreement with Altera or a megafunction partner.  Title to
--	the intellectual property, including patents, copyrights, trademarks,
--	trade secrets, or maskworks, embodied in any such megafunction design,
--	net list, support information, device programming or simulation file, or
--	any other related documentation or information provided by Altera or a
--	megafunction partner, remains with Altera, the megafunction partner, or
--	their respective licensors.  No other licenses, including any licenses
--	needed under any third party's intellectual property, are provided herein.

--NCO COMPILER ver 2.2.0 VHDL TESTBENCH

library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_arith.all;
use ieee.std_logic_signed.all;

use std.textio.all;

entity a_nco_tb is   
  generic(
		APR	:	INTEGER:=32;
		MPR	:	INTEGER:=16
        );
 
end a_nco_tb;


architecture tb of a_nco_tb is

function int2ustd(value : integer; width : integer) return std_logic_vector is 
-- convert integer to unsigned std_logicvector 
variable temp :   std_logic_vector(width-1 downto 0);
begin
	if (width>0) then
		temp:=conv_std_logic_vector(conv_unsigned(value, width ), width);
	end if ;
	return temp;
end int2ustd;


component a_nco

port(
		  phi_inc_i		: IN STD_LOGIC_VECTOR (APR-1 DOWNTO 0);
		clk		: IN STD_LOGIC ;
		clken		: IN STD_LOGIC ;
		reset		: IN STD_LOGIC ;
		fsin_o		: OUT STD_LOGIC_VECTOR (MPR-1 DOWNTO 0);
		fcos_o		: OUT STD_LOGIC_VECTOR (MPR-1 DOWNTO 0);
		data_ready		: OUT STD_LOGIC
		);
end component;

signal clk                 : std_logic;
signal reset               : std_logic;
signal clken               : std_logic;
signal sin_val	      	: std_logic_vector (MPR-1 downto 0);
signal cos_val      	  : std_logic_vector (MPR-1 downto 0);
signal phi      	        : std_logic_vector (APR-1 downto 0);
signal sel_phi            : std_logic_vector(2 downto 0);
signal sel_output         : std_logic_vector(2 downto 0);
signal data_ready            : std_logic;
constant clk_period_2 	: time := 5 ns;


begin

-- NCO component instantiation

u1: a_nco

port map(  clk=>clk,
           reset=>reset,
           clken=>clken,
           phi_inc_i=>phi,
           fsin_o=>sin_val,
           fcos_o=>cos_val,
           data_ready=>data_ready
	);
 
reset <= '1',
         '0' after 14*clk_period_2 ;
clken <= '1';

phi<="00000010100011110101110000101001";

-----------------------------------------------------------------------------------------------
-- Testbench Clock Generation
-----------------------------------------------------------------------------------------------
clk_gen : process
begin
	loop
		clk<='0' ,
     		     '1'  after clk_period_2;
     		wait for clk_period_2*2;
        end loop;
end process;

-----------------------------------------------------------------------------------------------
-- Output Sinusoidal Signals to Text Files 
-----------------------------------------------------------------------------------------------
testbench_o : process(clk) 



file sin_file 		: text open write_mode is "fsin_o_vhdl_a_nco.txt";
file cos_file 		: text open write_mode is "fcos_o_vhdl_a_nco.txt";
variable ls		: line;
variable lc		: line;
variable sin_int	: integer ;
variable cos_int	: integer ;

  begin
    if rising_edge(clk) then
      if(reset='0' and data_ready='1') then
        sin_int := conv_integer(sin_val);
        cos_int := conv_integer(cos_val);
        write(ls,sin_int);
        writeline(sin_file,ls);
        write(lc,cos_int);
        writeline(cos_file,lc);
     end if;		
	end if;		
end process testbench_o;

end tb;
