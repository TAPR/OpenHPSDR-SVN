if {[file exist [project env]] > 0} {project close}
if {[file exist "C:/Data/HPSDR/trunk/N8VB/OZY_V1/OZY_Verilog_Test/MERC_NCO/a_nco.mpf"] == 0} {
  project new C:/Data/HPSDR/trunk/N8VB/OZY_V1/OZY_Verilog_Test/MERC_NCO/ a_nco
} else	{
project open a_nco
}
# Create default work directory if not present
if {[file exist work] ==0} 	{
  exec vlib work
  exec vmap work work}      
# Map lpm library
if {[file exist lpm] ==0} 	{
  exec vlib lpm
  exec vmap lpm lpm}      
vcom -93 -work lpm c:/altera/quartus60//eda/sim_lib/220pack.vhd 
vcom -93 -work lpm c:/altera/quartus60//eda/sim_lib/220model.vhd 
# Map altera_mf library
if {[file exist altera_mf] ==0} 	{
  exec vlib altera_mf
  exec vmap altera_mf altera_mf}      
vcom -93 -work altera_mf c:/altera/quartus60//eda/sim_lib/altera_mf_components.vhd 
vcom -93 -work altera_mf c:/altera/quartus60//eda/sim_lib/altera_mf.vhd 
# Map sgate library
if {[file exist sgate] ==0} 	{
  exec vlib sgate
  exec vmap sgate sgate}      
vcom -93 -work sgate c:/altera/quartus60//eda/sim_lib/sgate_pack.vhd 
vcom -93 -work sgate c:/altera/quartus60//eda/sim_lib/sgate.vhd 
vcom -93 a_nco.vho
vcom -93 a_nco_tb.vhd
vsim a_nco_tb
do a_nco_wave.do
run 22000 ns;
