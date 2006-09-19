% CASCADE FILTER DESIGN
% P.COVINGTON
% 09/19/2006
%
% SECOND STAGE
% CIC filter design R=4, M=1, N=10
%
R = 4; % decimation factor
D = 1;   % Differential Delay
N = 10;   % number of sections
IWL = 16; % Input Word Length
OWL = 20; % Output Word Length

hcic2 = mfilt.cicdecim(R,D,N,IWL,OWL);

set(hcic2, 'InputFracLength', 0);

% Fs_in = 100.000e6;
% h = fvtool(hcic1, 'Fs', Fs_in);

generatehdl(hcic2,'Name', 'CIC_D4', 'TargetLanguage', 'Verilog',...
    'TargetDirectory', 'c:\data', 'AddPipelineRegisters', 'on', ...
    'CoeffMultipliers', 'csd', 'FIRAdderStyle', 'tree');