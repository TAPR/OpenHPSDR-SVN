% CASCADE FILTER DESIGN
% P.COVINGTON
% 09/19/2006
%
% THIRD STAGE
% Half Band Filter Design
%
N = 40;   % filter order
w = hamming(N+1);
fp = 0.45;
d = 0.0001; % 80 db atten
IWL = 16; % Input Word Length
OWL = 20; % Output Word Length

%b = firhalfband(N,d,'dev');
%b = firhalfband(N,w);
b = firhalfband('minorder',fp,d,'kaiser');

hm = mfilt.firdecim(2,b);

set(hm, 'Arithmetic', 'fixed');
set(hm, 'FilterInternals', 'SpecifyPrecision');
set(hm, 'InputWordLength', IWL);
set(hm, 'OutputWordLength', OWL);

% Fs_in = 100.000e6/260;
% h = fvtool(hm, 'Fs', Fs_in);

generatehdl(hm,'Name', 'HB', 'TargetLanguage', 'Verilog',...
    'TargetDirectory', 'c:\data', 'AddPipelineRegisters', 'on', ...
    'CoeffMultipliers', 'csd', 'FIRAdderStyle', 'tree');