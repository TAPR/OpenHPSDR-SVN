sources = buffered2.c \
buffered.c \
conf.c \
ct-hc2c.c \
ct-hc2c-direct.c \
dft-r2hc.c \
dht-r2hc.c \
dht-rader.c \
direct2.c \
direct-r2c.c \
direct-r2r.c \
generic.c \
hc2hc.c \
hc2hc-direct.c \
hc2hc-generic.c \
indirect.c \
khc2c.c \
khc2hc.c \
kr2c.c \
kr2r.c \
nop2.c \
nop.c \
plan2.c \
plan.c \
problem2.c \
problem.c \
rank0.c \
rank0-rdft2.c \
rank-geq2.c \
rank-geq2-rdft2.c \
rdft2-inplace-strides.c \
rdft2-rdft.c \
rdft2-strides.c \
rdft2-tensor-max-index.c \
rdft-dht.c \
solve2.c \
solve.c \
vrank3-transpose.c \
vrank-geq1.c \
vrank-geq1-rdft2.c

LOCAL_SRC_FILES += $(sources:%=fftw3/rdft/%)
