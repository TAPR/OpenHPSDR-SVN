sources = bluestein.c\
    buffered.c\
    conf.c\
    ct.c\
    dftw-direct.c\
    dftw-directsq.c\
    dftw-genericbuf.c\
    dftw-generic.c\
    direct.c\
    generic.c\
    indirect.c\
    indirect-transpose.c\
    kdft.c\
    kdft-dif.c\
    kdft-difsq.c\
    kdft-dit.c\
    nop.c\
    plan.c\
    problem.c\
    rader.c\
    rank-geq2.c\
    solve.c\
    vrank-geq1.c\
    zero.c

LOCAL_SRC_FILES += $(sources:%=fftw3/dft/%)
