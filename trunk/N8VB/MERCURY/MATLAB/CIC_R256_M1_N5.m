%
% CIC filter design R=256, M=1, N=5
%
R = 256; % decimation factor
D = 1;   % Differential Delay
N = 5;   % number of sections
IWL = 16; % Input Word Length
IFL = 0;  % Input Fraction Length
OWL = 18; % Output Word Length

hcic = mfilt.cicdecim(R,D,N,IWL,OWL);
hcic.InputFracLength = IFL;

Fs_in = 100.000e6;
h = fvtool(hcic, 'Fs', Fs_in);

generatehdl(hcic,'Name', 'CIC_R256_M1_N5', 'TargetLanguage', 'Verilog',...
    'TargetDirectory', 'c:\data', 'AddPipelineRegisters', 'on', ...
    'CoeffMultipliers', 'csd', 'FIRAdderStyle', 'tree');