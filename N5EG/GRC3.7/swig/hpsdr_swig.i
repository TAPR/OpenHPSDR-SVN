/* -*- c++ -*- */

#define HPSDR_API

%include "gnuradio.i"			// the common stuff

//load generated python docstrings
%include "hpsdr_swig_doc.i"

%{
#include "hpsdr/hermesNB.h"
%}


%include "hpsdr/hermesNB.h"
GR_SWIG_BLOCK_MAGIC2(hpsdr, hermesNB);
