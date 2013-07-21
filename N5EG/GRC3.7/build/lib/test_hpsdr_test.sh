#!/bin/sh
export GR_DONT_LOAD_PREFS=1
export srcdir=/home/tom/gr-hpsdr/lib
export PATH=/home/tom/gr-hpsdr/build/lib:$PATH
export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:$DYLD_LIBRARY_PATH
export DYLD_LIBRARY_PATH=$LD_LIBRARY_PATH:$DYLD_LIBRARY_PATH
export PYTHONPATH=$PYTHONPATH
test-hpsdr 
