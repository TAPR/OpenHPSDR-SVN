sources = altivec.c \
avx.c \
neon.c \
sse2.c \
sse2-nonportable.c \
taint.c

LOCAL_SRC_FILES += $(sources:%=fftw3/simd-support/%)

