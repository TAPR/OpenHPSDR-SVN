% CIC design file for Mercury filter given parameters
% !!!!!! CHANGE FILTER FILE NAME AND VERILOG NAME WHEN SAVING !!!!!!

% cic_8_1_5.m  24 bits in and 24 out 


Fs = 12.5e6;  % Sampling rate 125 MHz
R = 8; % decimation value 
M = 1; % differential delay
N = 5; % number of sections
IWL = 24; % number of bits in input data
OWL = 24; % number of bits in output data


Hm = mfilt.cicdecim(R,M,N, IWL,OWL);
Hm.inputfraclength = 0;
hfvt = fvtool(Hm,'Fs',Fs); % using Fs adds frequency to the graph
hfvt.showreference = 'off';
legend(hfvt, 'CIC Decimator: R=8, M=1, N=5');
set(hfvt,'Color','white');


% ***** CHANGE FILTER NAME HERE ALSO WHEN REDEFINING ******

% Generate Verilog 

workingdir = tempname;
generatehdl(Hm,'Name','cic_8_1_5_nofraction','TargetLanguage','Verilog',...
    'TargetDirectory','c:\HPSDR\trunk\VK6APH\Mercury\Matlab-files', 'AddPipelineRegisters', 'on', ...
'CoeffMultipliers', 'csd', 'FIRAdderStyle', 'tree');



