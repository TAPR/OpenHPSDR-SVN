% CIC design file for Mercury filter given parameters
% !!!!!! CHANGE FILTER FILE NAME AND VERILOG NAME WHEN SAVING !!!!!!

% cic_10_1_3.m  16 bits in and 24 out 


Fs = 125e6;  % Sampling rate 125 MHz
R = 10; % decimation value 
M = 1; % differential delay
N = 3; % number of sections
IWL = 18; % number of bits in input data
OWL = 24; % number of bits in output data


Hm = mfilt.cicdecim(R,M,N, IWL,OWL);
Hm.inputfraclength = 0;
hfvt = fvtool(Hm,'Fs',Fs); % using Fs adds frequency to the graph
hfvt.showreference = 'off';
legend(hfvt, 'CIC Decimator: R=10, M=1, N=3');
set(hfvt,'Color','white');


% ***** CHANGE FILTER NAME HERE ALSO WHEN REDEFINING ******

% Generate Verilog 

workingdir = tempname;
generatehdl(Hm,'Name','cic_10_1_3_nofraction','TargetLanguage','Verilog',...
    'TargetDirectory','c:\HPSDR\trunk\VK6APH\Mercury\Matlab-files', 'AddPipelineRegisters', 'on', ...
'CoeffMultipliers', 'csd', 'FIRAdderStyle', 'tree');



