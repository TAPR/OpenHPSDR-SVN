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
IFL = 0;  % Input Fraction Length
OWL = 18; % Output Word Length

hcic1 = mfilt.cicdecim(R,D,N,IWL,OWL);
hcic1.InputFracLength = IFL;
%
% SECOND STAGE
% CIC filter design R=4, M=1, N=8
%
R = 4; % decimation factor
D = 1;   % Differential Delay
N = 10;   % number of sections
IWL = 18; % Input Word Length
OWL = 20; % Output Word Length

hcic2 = mfilt.cicdecim(R,D,N,IWL,OWL);

% %
% % THIRD STAGE
% % compensation FIR decim = 4
% %
% Decim = 4;
% Fscomp = 100.000e6/130;
% Apass = 0.01;
% Astop = 100;
% Aslope = 80;
% Fpass = 96e3;
% Fstop = 250e3;
% 
% d = fdesign.decimator(Decim, 'ciccomp', D, N, Fpass, Fstop, ...
%     Apass, Astop, Fscomp);
% 
% hcfir = design(d, 'equiripple', 'StopbandShape', 'linear', ...
%     'Stopbanddecay', Aslope);
% 
% set(hcfir, ...
%     'Arithmetic', 'fixed', ...
%     'CoeffWordLength', 18, ...
%     'InputWordLength', 24, ...
%     'FilterInternals', 'SpecifyPrecision', ...
%     'OutputWordLength', 32);

hcas = cascade(hcic1, hcic2);

% Fs_in = 100.000e6;
% h = fvtool(hcas, 'Fs', Fs_in);

generatehdl(hcas,'Name', 'CASCADE_D260', 'TargetLanguage', 'Verilog',...
    'TargetDirectory', 'c:\data', 'AddPipelineRegisters', 'on', ...
    'CoeffMultipliers', 'csd', 'FIRAdderStyle', 'tree');