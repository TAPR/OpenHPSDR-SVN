hm0 = mfilt.cicdecim(13,1,3);
hm1 = mfilt.cicdecim(5,1,6);
hm2 = mfilt.cicdecim(2,1,7);

cfir = fdesign.ciccomp(1,7,0.25,0.35,0.05,100);
hmx = equiripple(cfir);
coef = coefficients(hmx);

hm4 = mfilt.firdecim(4, coef{1});

hcas = cascade(hm0,hm1,hm2,hm4);
fvtool(hcas);
