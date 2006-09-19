% CASCADE FILTER DESIGN
% P.COVINGTON
% 09/19/2006
%
% FIRST STAGE
% CIC filter design R=65, M=1, N=5
%
R = 65; % decimation factor
D = 1;   % Differential Delay
N = 5;   % number of sections
IWL = 16; % Input Word Length
OWL = 20; % Output Word Length

hcic1 = mfilt.cicdecim(R,D,N,IWL,OWL);

set(hcic1, 'InputFracLength', 0);

% Fs_in = 100.000e6;
% h = fvtool(hcic1, 'Fs', Fs_in);

generatehdl(hcic1,'Name', 'CIC_D65', 'TargetLanguage', 'Verilog',...
    'TargetDirectory', 'c:\data', 'AddPipelineRegisters', 'on', ...
    'CoeffMultipliers', 'csd', 'FIRAdderStyle', 'tree');