% CASCADE FILTER DESIGN
% P.COVINGTON
% 09/19/2006
%
% FIRST STAGE
% CIC filter design R=65, M=1, N=6
%
R = 65; % decimation factor
D = 1;   % Differential Delay
N = 6;   % number of sections
IWL = 16; % Input Word Length
IFL = 15;  % Input Fraction Length
OWL = 20; % Output Word Length

hcic1 = mfilt.cicdecim(R,D,N,IWL,OWL);
hcic1.InputFracLength = IFL;

%
% SECOND STAGE
% CIC filter design R=2, M=1, N=8
%
R = 2; % decimation factor
D = 1;   % Differential Delay
N = 10;   % number of sections
IWL = 20; % Input Word Length
IFL = 15;  % Input Fraction Length
OWL = 24; % Output Word Length

hcic2 = mfilt.cicdecim(R,D,N,IWL,OWL);
hcic2.InputFracLength = IFL;

%
% THIRD STAGE
% compensation FIR decim = 4
%
hcas = cascade(hcic1, hcic2);

Fs_in = 100.000e6;
h = fvtool(hcas, 'Fs', Fs_in);

% generatehdl(hcic,'Name', 'CIC_R64_M1_N4', 'TargetLanguage', 'Verilog',...
%     'TargetDirectory', 'c:\data', 'AddPipelineRegisters', 'on', ...
%     'CoeffMultipliers', 'csd', 'FIRAdderStyle', 'tree');