%Create CIC Interpolating Filter
%Create a 5-stage CIC Interpolator with interpolation factor of 2048.
%The input and output wordlengths are set to 14. 

Fs = 100e6;  % Sampling rate 100 MHz
R = 2048; % interpolation value
M = 1; % differential delay
N = 5; % number of sections
IWL = 14; % number of bits in input data
OWL = 14; % number of bits in output data

Frequency = 100e6;

hcic = mfilt.cicinterp(R, M, N, IWL, OWL);
hcic.inputfraclength = 0;

%hduc = cascade(hpfir, hcfir, hcic);
fvtool(hcic, 'Fs',Fs);


% ***** CHANGE FILTER NAME HERE ALSO WHEN REDEFINING ******

% Generate Verilog 

workingdir = tempname;
generatehdl(hcic,'Name','cicint','TargetLanguage','Verilog',...
    'TargetDirectory',fullfile(workingdir,'hdlsrc'));