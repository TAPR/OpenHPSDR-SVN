%
% CIC filter design R=100, M=1, N=5
%
R = 100; % decimation factor
D = 1;   % Differential Delay
N = 5;   % number of sections
IWL = 16; % Input Word Length
IFL = 0;  % Input Fraction Length
OWL = 18; % Output Word Length

hcic = mfilt.cicdecim(R,D,N,IWL,OWL);
hcic.InputFracLength = IFL;

Fscomp = 100.000e6/R;
Apass = 0.01;
Astop = 80;
Aslope = 80;
Fpass = 50e3;
Fstop = 200e3;

d = fdesign.decimator(2, 'ciccomp', D, N, Fpass, Fstop, ...
    Apass, Astop, Fscomp);

hcfir = design(d, 'equiripple', 'StopbandShape', 'linear', ...
    'Stopbanddecay', Aslope);

set(hcfir, ...
    'Arithmetic', 'fixed', ...
    'CoeffWordLength', 16, ...
    'InputWordLength', 16, ...
    'InputFracLength', 0);

hcas = cascade(hcic, hcfir);

generatehdl(hcas,'Name', 'CIC_R100_M1_N5_CFIR', 'TargetLanguage', 'Verilog',...
    'TargetDirectory', 'c:\data', 'AddPipelineRegisters', 'on', ...
    'CoeffMultipliers', 'csd', 'FIRAdderStyle', 'tree');