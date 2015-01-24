sources = codlist.c \
e01_8.c \
e10_8.c

LOCAL_SRC_FILES += $(sources:%=fftw3/rdft/scalar/r2r/%)
