if {[file exist [project env]] > 0} {project close}
if {[file exist "C:/Data/HPSDR/trunk/N8VB/OZY_V1/OZY_Verilog_Test/MERC_NCO/a_nco.mpf"] == 0} {
  project new C:/Data/HPSDR/trunk/N8VB/OZY_V1/OZY_Verilog_Test/MERC_NCO/ a_nco
} else	{
project open a_nco
}
if {[file exist work] ==0} 	{
  exec vlib work
  exec vmap work work}      
vlog c:/altera/quartus60//eda/sim_lib/220model.v
vlog c:/altera/quartus60//eda/sim_lib/altera_mf.v
vlog c:/altera/quartus60//eda/sim_lib/sgate.v
vlog a_nco.vo
vlog a_nco_tb.v
vsim a_nco_tb -pli c:/altera/quartus60//eda/mentor/modelsim/convert_hex2ver.dll
do a_nco_wave.do
run 22000 ns;
