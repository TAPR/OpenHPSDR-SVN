sources = any-true.c /
api.c /
block.c /
choose-radix.c /
conf.c /
dft-problem.c /
dft-rank1-bigvec.c /
dft-rank1.c /
dft-rank-geq2.c /
dft-rank-geq2-transposed.c /
dft-serial.c /
dft-solve.c /
dtensor.c /
f03-wrap.c /
mpi-bench.c /
rdft2-problem.c /
rdft2-rank-geq2.c /
rdft2-rank-geq2-transposed.c /
rdft2-serial.c /
rdft2-solve.c /
rdft-problem.c /
rdft-rank1-bigvec.c /
rdft-rank-geq2.c /
rdft-rank-geq2-transposed.c /
rdft-serial.c /
rdft-solve.c /
rearrange.c /
testsched.c /
transpose-alltoall.c /
transpose-pairwise.c /
transpose-problem.c /
transpose-recurse.c /
transpose-solve.c /
wisdom-api.c

LOCAL_SRC_FILES += $(sources:%=fftw3/mpi/%)
