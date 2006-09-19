%
% CIC filter design R=64, M=1, N=7
%
R = 64; % decimation factor
D = 1;   % Differential Delay
N = 8;   % number of sections
IWL = 18; % Input Word Length
IFL = 0;  % Input Fraction Length
OWL = 32; % Output Word Length

hcic = mfilt.cicdecim(R,D,N,IWL,OWL);
hcic.InputFracLength = IFL;

Fs_in = 100.000e6;
%h = fvtool(hcic, 'Fs', Fs_in);

generatehdl(hcic,'Name', 'CIC_R64_M1_N8', 'TargetLanguage', 'Verilog',...
    'TargetDirectory', 'c:\data', 'AddPipelineRegisters', 'on', ...
    'CoeffMultipliers', 'csd', 'FIRAdderStyle', 'tree');