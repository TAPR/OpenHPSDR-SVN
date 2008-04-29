% CIC design file for Mercury filter given parameters
% !!!!!! CHANGE FILTER FILE NAME AND VERILOG NAME WHEN SAVING !!!!!!

% cic_4_1_8.m  24 bits in and 24 out 


Fs = 1.5625e6;  % Sampling rate 125 MHz
R = 4; % decimation value 
M = 1; % differential delay
N = 8; % number of sections
IWL = 24; % number of bits in input data
OWL = 24; % number of bits in output data


Hm = mfilt.cicdecim(R,M,N, IWL,OWL);
Hm.inputfraclength = 0;
hfvt = fvtool(Hm,'Fs',Fs); % using Fs adds frequency to the graph
hfvt.showreference = 'off';
legend(hfvt, 'CIC Decimator: R=4, M=1, N=8');
set(hfvt,'Color','white');


% ***** CHANGE FILTER NAME HERE ALSO WHEN REDEFINING ******

% Generate Verilog 

workingdir = tempname;
generatehdl(Hm,'Name','cic_4_1_8_nofraction','TargetLanguage','Verilog',...
    'TargetDirectory','c:\HPSDR\trunk\VK6APH\Mercury\Matlab-files', 'AddPipelineRegisters', 'on', ...
'CoeffMultipliers', 'csd', 'FIRAdderStyle', 'tree');



