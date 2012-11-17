/* -*- c++ -*- */

#define HPSDR_API

%include "gnuradio.i"			// the common stuff

//load generated python docstrings
%include "hpsdr_swig_doc.i"


%{
#include "hpsdr_hermesNB.h"
%}


GR_SWIG_BLOCK_MAGIC(hpsdr,hermesNB);
%include "hpsdr_hermesNB.h"
