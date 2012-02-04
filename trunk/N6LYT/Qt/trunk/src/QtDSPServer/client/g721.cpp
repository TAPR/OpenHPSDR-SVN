#include "g721.h"

#include <stdlib.h>

static short qtab_721[7] = {-124, 80, 178, 246, 300, 349, 400};
/*
 * Maps G.721 code word to reconstructed scale factor normalized log
 * magnitude values.
 */
static short    _dqlntab[16] = {-2048, 4, 135, 213, 273, 323, 373, 425,
                                425, 373, 323, 273, 213, 135, 4, -2048};

/* Maps G.721 code word to log of scale factor multiplier. */
short    _witab[16] = {-12, 18, 41, 64, 112, 198, 355, 1122,
                       1122, 355, 198, 112, 64, 41, 18, -12};
/*
 * Maps G.721 code words to a set of values whose long and short
 * term averages are computed and then compared to give an indication
 * how stationary (steady state) the signal is.
 */
static short    _fitab[16] = {0, 0, 0, 0x200, 0x200, 0x200, 0x600, 0xE00,
                              0xE00, 0x600, 0x200, 0x200, 0x200, 0, 0, 0};

static short power2[15] = {1, 2, 4, 8, 0x10, 0x20, 0x40, 0x80,
                        0x100, 0x200, 0x400, 0x800, 0x1000, 0x2000, 0x4000};

G721::G721()
{
    int cnta;

    yl = 34816;
    yu = 544;
    dms = 0;
    dml = 0;
    ap = 0;
    for (cnta = 0; cnta < 2; cnta++) {
        a[cnta] = 0;
        pk[cnta] = 0;
        _sr[cnta] = 32;
    }
    for (cnta = 0; cnta < 6; cnta++) {
        b[cnta] = 0;
        _dq[cnta] = 32;
    }
    td = 0;
}

unsigned char G721::encode(int sl) {
    short           sezi, se, sez;          /* ACCUM */
    short           d;                      /* SUBTA */
    short           sr;                     /* ADDB */
    short           y;                      /* MIX */
    short           dqsez;                  /* ADDC */
    short           dq, i;

    sl >>= 2; // only 14 bits

    sezi = predictor_zero();
    sez = sezi >> 1;
    se = (sezi + predictor_pole()) >> 1;   /* estimated signal */

    d = sl - se;                            /* estimation difference */

    /* quantize the prediction difference */
    y = step_size();               /* quantizer step size */
    i = quantize(d, y, qtab_721, 7);        /* i = ADPCM code */

    dq = reconstruct(i & 8, _dqlntab[i], y);        /* quantized est diff */

    sr = (dq < 0) ? se - (dq & 0x3FFF) : se + dq;   /* reconst. signal */

    dqsez = sr + sez - se;                  /* pole prediction diff. */

    update(4, y, _witab[i] << 5, _fitab[i], dq, sr, dqsez);

    return (i);

}

short G721::decode(int i) {
    short           sezi, sei, sez, se;     /* ACCUM */
    short           y;                      /* MIX */
    short           sr;                     /* ADDB */
    short           dq;
    short           dqsez;

    i &= 0x0f;                      /* mask to get proper bits */
    sezi = predictor_zero();
    sez = sezi >> 1;
    sei = sezi + predictor_pole();
    se = sei >> 1;                  /* se = estimated signal */

    y = step_size();       /* dynamic quantizer step size */

    dq = reconstruct(i & 0x08, _dqlntab[i], y); /* quantized diff. */

    sr = (dq < 0) ? (se - (dq & 0x3FFF)) : se + dq; /* reconst. signal */

    dqsez = sr - se + sez;                  /* pole prediction diff. */

    update(4, y, _witab[i] << 5, _fitab[i], dq, sr, dqsez);

    return (sr << 2);       /* sr was 14-bit dynamic range */

}

/*
 * update()
 *
 * updates the state variables for each output code
 */
void G721::update(
        int             code_size,      /* distinguish 723_40 with others */
        int             y,              /* quantizer step size */
        int             wi,             /* scale factor multiplier */
        int             fi,             /* for long/short term energies */
        int             dq,             /* quantized prediction difference */
        int             sr,             /* reconstructed signal */
        int             dqsez)          /* difference from 2-pole predictor */
{
    int             cnt;
    short           mag, exp, mant; /* Adaptive predictor, FLOAT A */
    short           a2p;            /* LIMC */
    short           a1ul;           /* UPA1 */
    short           ua2, pks1;      /* UPA2 */
    short           uga2a, fa1;
    short           uga2b;
    char            tr;             /* tone/transition detector */
    short           ylint, thr2, dqthr;
    short           ylfrac, thr1;
    short           pk0;

    pk0 = (dqsez < 0) ? 1 : 0;      /* needed in updating predictor poles */

    mag = dq & 0x7FFF;              /* prediction difference magnitude */
    /* TRANS */
    ylint = yl >> 15;    /* exponent part of yl */
    ylfrac = (yl >> 10) & 0x1F;  /* fractional part of yl */
    thr1 = (32 + ylfrac) << ylint;          /* threshold */
    thr2 = (ylint > 9) ? 31 << 10 : thr1;   /* limit thr2 to 31 << 10 */
    dqthr = (thr2 + (thr2 >> 1)) >> 1;      /* dqthr = 0.75 * thr2 */
    if (td == 0)         /* signal supposed voice */
        tr = 0;
    else if (mag <= dqthr)          /* supposed data, but small mag */
        tr = 0;                 /* treated as voice */
    else                            /* signal is data (modem) */
        tr = 1;

    /*
     * Quantizer scale factor adaptation.
     */

    /* FUNCTW & FILTD & DELAY */
    /* update non-steady state step size multiplier */
    yu = y + ((wi - y) >> 5);

    /* LIMB */
    if (yu < 544)        /* 544 <= yu <= 5120 */
        yu = 544;
    else if (yu > 5120)
        yu = 5120;

    /* FILTE & DELAY */
    /* update steady state step size multiplier */
    yl += yu + ((-yl) >> 6);

    /*
     * Adaptive predictor coefficients.
     */
    if (tr == 1) {                  /* reset a's and b's for modem signal */
        a[0] = 0;
        a[1] = 0;
        b[0] = 0;
        b[1] = 0;
        b[2] = 0;
        b[3] = 0;
        b[4] = 0;
        b[5] = 0;
    } else {                        /* update a's and b's */
        pks1 = pk0 ^ pk[0];          /* UPA2 */

        /* update predictor pole a[1] */
        a2p = a[1] - (a[1] >> 7);
        if (dqsez != 0) {
            fa1 = (pks1) ? a[0] : -a[0];
            if (fa1 < -8191)        /* a2p = function of fa1 */
                a2p -= 0x100;
            else if (fa1 > 8191)
                a2p += 0xFF;
            else
                a2p += fa1 >> 5;

            if (pk0 ^ pk[1])
                /* LIMC */
                if (a2p <= -12160)
                    a2p = -12288;
                else if (a2p >= 12416)
                    a2p = 12288;
                else
                    a2p -= 0x80;
            else if (a2p <= -12416)
                a2p = -12288;
            else if (a2p >= 12160)
                a2p = 12288;
            else
                a2p += 0x80;
        }

        /* TRIGB & DELAY */
        a[1] = a2p;

        /* UPA1 */
        /* update predictor pole a[0] */
        a[0] -= a[0] >> 8;
        if (dqsez != 0)
            if (pks1 == 0)
                a[0] += 192;
            else
                a[0] -= 192;
        /* LIMD */
        a1ul = 15360 - a2p;
        if (a[0] < -a1ul)
            a[0] = -a1ul;
        else if (a[0] > a1ul)
            a[0] = a1ul;

        /* UPB : update predictor zeros b[6] */
        for (cnt = 0; cnt < 6; cnt++) {
            if (code_size == 5)             /* for 40Kbps G.723 */
                b[cnt] -= b[cnt] >> 9;
            else                    /* for G.721 and 24Kbps G.723 */
                b[cnt] -= b[cnt] >> 8;
            if (dq & 0x7FFF) {                      /* XOR */
                if ((dq ^ _dq[cnt]) >= 0)
                    b[cnt] += 128;
                else
                    b[cnt] -= 128;
            }
        }
    }

    for (cnt = 5; cnt > 0; cnt--)
        _dq[cnt] = _dq[cnt-1];
    /* FLOAT A : convert _dq[0] to 4-bit exp, 6-bit mantissa f.p. */
    if (mag == 0) {
        _dq[0] = (dq >= 0) ? 0x20 : 0xFC20;
    } else {
        exp = quan(mag, power2, 15);
        _dq[0] = (dq >= 0) ?
                    (exp << 6) + ((mag << 6) >> exp) :
                    (exp << 6) + ((mag << 6) >> exp) - 0x400;
    }

    _sr[1] = _sr[0];
    /* FLOAT B : convert sr to 4-bit exp., 6-bit mantissa f.p. */
    if (sr == 0) {        } else if (sr > -32768) {
        mag = -sr;
        exp = quan(mag, power2, 15);
        _sr[0] =  (exp << 6) + ((mag << 6) >> exp) - 0x400;
    } else
        _sr[0] = 0xFC20;

    /* DELAY A */
    pk[1] = pk[0];
    pk[0] = pk0;

    /* TONE */
    if (tr == 1)            /* this sample has been treated as data */
        td = 0;      /* next one will be treated as voice */
    else if (a2p < -11776)  /* small sample-to-sample correlation */
        td = 1;      /* signal may be data */
    else                            /* signal is voice */
        td = 0;

    /*
* Adaptation speed control.
*/
    dms += (fi - dms) >> 5;           /* FILTA */
    dml += (((fi << 2) - dml) >> 7);  /* FILTB */

    if (tr == 1)
        ap = 256;
    else if (y < 1536)                                      /* SUBTC */
        ap += (0x200 - ap) >> 4;
    else if (td == 1)
        ap += (0x200 - ap) >> 4;
    else if (abs((dms << 2) - dml) >=
             (dml >> 3))
        ap += (0x200 - ap) >> 4;
    else
        ap += (-ap) >> 4;
}


/*
 * predictor_zero()
 *
 * computes the estimated signal from 6-zero predictor.
 *
 */
int G721::predictor_zero() {
    int             i;
    int             sezi;

    sezi = fmult(b[0] >> 2, _dq[0]);
    for (i = 1; i < 6; i++)                 /* ACCUM */
        sezi += fmult(b[i] >> 2, _dq[i]);
    return (sezi);
}

/*
 * predictor_pole()
 *
 * computes the estimated signal from 2-pole predictor.
 *
 */
int G721::predictor_pole() {
    return (fmult(a[1] >> 2, _sr[1]) +
            fmult(a[0] >> 2, _sr[0]));
}

/*
 * step_size()
 *
 * computes the quantization step size of the adaptive quantizer.
 *
 */
int G721::step_size() {
    int             y;
    int             dif;
    int             al;

    if (ap >= 256)
        return (yu);
    else {
        y = yl >> 6;
        dif = yu - y;
        al = ap >> 2;
        if (dif > 0)
            y += (dif * al) >> 6;
        else if (dif < 0)
            y += (dif * al + 0x3F) >> 6;
        return (y);
    }
}

/*
 * quantize()
 *
 * Given a raw sample, 'd', of the difference signal and a
 * quantization step size scale factor, 'y', this routine returns the
 * ADPCM codeword to which that sample gets quantized.  The step
 * size scale factor division operation is done in the log base 2 domain
 * as a subtraction.
 */
int G721::quantize(
        int             d,      /* Raw difference signal sample */
        int             y,      /* Step size multiplier */
        short           *table, /* quantization table */
        int             size)   /* table size of short integers */
{
    short           dqm;    /* Magnitude of 'd' */
    short           exp;    /* Integer part of base 2 log of 'd' */
    short           mant;   /* Fractional part of base 2 log */
    short           dl;     /* Log of magnitude of 'd' */
    short           dln;    /* Step size scale factor normalized log */
    int             i;

    /*
         * LOG
         *
         * Compute base 2 log of 'd', and store in 'dl'.
         */
    dqm = abs(d);
    exp = quan(dqm >> 1, power2, 15);
    mant = ((dqm << 7) >> exp) & 0x7F;      /* Fractional portion. */
    dl = (exp << 7) + mant;

    /*
         * SUBTB
         *
         * "Divide" by step size multiplier.
         */
    dln = dl - (y >> 2);

    /*
         * QUAN
         *
         * Obtain codword i for 'd'.
         */
    i = quan(dln, table, size);
    if (d < 0)                      /* take 1's complement of i */
        return ((size << 1) + 1 - i);
    else if (i == 0)                /* take 1's complement of 0 */
        return ((size << 1) + 1); /* new in 1988 */
    else
        return (i);
}

/*
 * reconstruct()
 *
 * Returns reconstructed difference signal 'dq' obtained from
 * codeword 'i' and quantization step size scale factor 'y'.
 * Multiplication is performed in log base 2 domain as addition.
 */
int G721::reconstruct(
        int             sign,   /* 0 for non-negative value */
        int             dqln,   /* G.72x codeword */
        int             y)      /* Step size multiplier */
{
    short           dql;    /* Log of 'dq' magnitude */
    short           dex;    /* Integer part of log */
    short           dqt;
    short           dq;     /* Reconstructed difference signal sample */

    dql = dqln + (y >> 2);  /* ADDA */

    if (dql < 0) {
        return ((sign) ? -0x8000 : 0);
    } else {                /* ANTILOG */
        dex = (dql >> 7) & 15;
        dqt = 128 + (dql & 127);
        dq = (dqt << 7) >> (14 - dex);
        return ((sign) ? (dq - 0x8000) : dq);
    }
}

/*
 * quan()
 *
 * quantizes the input val against the table of size short integers.
 * It returns i if table[i - 1] <= val < table[i].
 *
 * Using linear search for simple coding.
 */
int G721::quan(
        int             val,
        short           *table,
        int             size)
{
    int             i;

    for (i = 0; i < size; i++)
        if (val < *table++)
            break;
    return (i);
}

/*
 * fmult()
 *
 * returns the integer product of the 14-bit integer "an" and
 * "floating point" representation (4-bit exponent, 6-bit mantessa) "srn".
 */
int G721::fmult(
        int             an,
        int             srn)
{
    short           anmag, anexp, anmant;
    short           wanexp, wanmag, wanmant;
    short           retval;

    anmag = (an > 0) ? an : ((-an) & 0x1FFF);
    anexp = quan(anmag, power2, 15) - 6;
    anmant = (anmag == 0) ? 32 :
                            (anexp >= 0) ? anmag >> anexp : anmag << -anexp;
    wanexp = anexp + ((srn >> 6) & 0xF) - 13;

    wanmant = (anmant * (srn & 077) + 0x30) >> 4;
    retval = (wanexp >= 0) ? ((wanmant << wanexp) & 0x7FFF) :
                             (wanmant >> -wanexp);

    return (((an ^ srn) < 0) ? -retval : retval);
}
