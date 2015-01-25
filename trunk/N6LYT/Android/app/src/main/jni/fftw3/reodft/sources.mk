sources = conf.c  \
redft00e-r2hc.c  \
redft00e-r2hc-pad.c  \
reodft00e-splitradix.c  \
reodft010e-r2hc.c  \
reodft11e-r2hc.c  \
reodft11e-r2hc-odd.c  \
reodft11e-radix2.c  \
rodft00e-r2hc.c  \
rodft00e-r2hc-pad.c 

LOCAL_SRC_FILES += $(sources:%=fftw3/reodft/%)
