#ifndef G721_H
#define G721_H

class G721
{
public:
    G721();
    unsigned char encode(int sl);
    short decode(int i);
private:

    void update(
            int             code_size,      /* distinguish 723_40 with others */
            int             y,              /* quantizer step size */
            int             wi,             /* scale factor multiplier */
            int             fi,             /* for long/short term energies */
            int             dq,             /* quantized prediction difference */
            int             sr,             /* reconstructed signal */
            int             dqsez);          /* difference from 2-pole predictor */

    int predictor_zero();
    int predictor_pole();
    int step_size();
    int quantize(
        int             d,      /* Raw difference signal sample */
        int             y,      /* Step size multiplier */
        short           *table, /* quantization table */
        int             size);  /* table size of short integers */

    int reconstruct(
        int             sign,   /* 0 for non-negative value */
        int             dqln,   /* G.72x codeword */
        int             y);     /* Step size multiplier */
    int quan(
            int             val,
            short           *table,
            int             size);

    int fmult(
            int             an,
            int             srn);


        long yl;        /* Locked or steady state step size multiplier. */
        short yu;       /* Unlocked or non-steady state step size multiplier. */
        short dms;      /* Short term energy estimate. */
        short dml;      /* Long term energy estimate. */
        short ap;       /* Linear weighting coefficient of 'yl' and 'yu'. */

        short a[2];     /* Coefficients of pole portion of prediction filter. */
        short b[6];     /* Coefficients of zero portion of prediction filter. */
        short pk[2];    /*
                         * Signs of previous two samples of a partially
                         * reconstructed signal.
                         */
        short _dq[6];    /*
                         * Previous 6 samples of the quantized difference
                         * signal represented in an internal floating point
                         * format.
                         */
        short _sr[2];    /*
                         * Previous 2 samples of the quantized difference
                         * signal represented in an internal floating point
                         * format.
                         */
        char td;        /* delayed tone detect, new in 1988 version */


};

#endif // G721_H
