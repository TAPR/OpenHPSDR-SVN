#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Created on Tue Jan 28 18:02:31 2014

@author: Tom McDermott, N5EG    email: n5eg@tapr.org

Copyright 2014, Thomas C. McDermott.

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

"""

from gnuradio import gr
import numpy as np
#import sys
#import select

 
# output assumed at 100 hertz sample rate
 
class CWKeyboard(gr.sync_block):
    
    def __init__(self, sampRate, WPM):
        gr.sync_block.__init__(
            self,
            name="CWKeyboard",
            in_sig=None,
            out_sig=[np.float32])
     
    # Text to Morse dictionary
    
        ascii = ('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
           'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
           'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
           '\n', '\t', '/', ',', '.', '?', ' ')
    
        morse = ([1,0,1,1,1,0,0,0], [1,1,1,0,1,0,1,0,1,0,0,0],
           [1,1,1,0,1,0,1,1,1,0,1,0,0,0], [1,1,1,0,1,0,1,0,0,0],
           [1,0,0,0], [1,0,1,0,1,1,1,0,1,0,0,0],
           [1,1,1,0,1,1,1,0,1,0,0,0], [1,0,1,0,1,0,1,0,0,0],
           [1,0,1,0,0,0], [1,0,1,1,1,0,1,1,1,0,1,1,1,0,0,0],
           [1,1,1,0,1,0,1,1,1,0,0,0], [1,0,1,1,1,0,1,0,1,0,0,0],
           [1,1,1,0,1,1,1,0,0,0], [1,1,1,0,1,0,0,0],
           [1,1,1,0,1,1,1,0,1,1,1,0,0,0], [1,0,1,1,1,0,1,1,1,0,1,0,0,0],
           [1,1,1,0,1,1,1,0,1,0,1,1,1,0,0,0], [1,0,1,1,1,0,1,0,0,0],
           [1,0,1,0,1,0,0,0], [1,1,1,0,0,0],
           [1,0,1,0,1,1,1,0,0,0], [1,0,1,0,1,0,1,1,1,0,0,0],
           [1,0,1,1,1,0,1,1,1,0,0,0], [1,1,1,0,1,0,1,0,1,1,1,0,0,0],
           [1,1,1,0,1,0,1,1,1,0,1,1,1,0,0,0], [1,1,1,0,1,1,1,0,1,0,1,0,0,0],
           [1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0,0,0],
           [1,0,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0,0,0],
           [1,0,1,0,1,1,1,0,1,1,1,0,1,1,1,0,0,0],
           [1,0,1,0,1,0,1,1,1,0,1,1,1,0,0,0],
           [1,0,1,0,1,0,1,0,1,1,1,0,0,0],
           [1,0,1,0,1,0,1,0,1,0,0,0],
           [1,1,1,0,1,0,1,0,1,0,1,0,0,0],
           [1,1,1,0,1,1,1,0,1,0,1,0,1,0,0,0],
           [1,1,1,0,1,1,1,0,1,1,1,0,1,0,1,0,0,0],
           [1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0,1,0,0,0],
           [0,0,0], [0,0,0], [1,1,1,0,1,0,1,0,1,1,1,0,1,0,0,0],
           [1,1,1,0,1,1,1,0,1,0,1,0,1,1,1,0,1,1,1,0,0,0],
           [1,0,1,1,1,0,1,0,1,1,1,0,1,0,1,1,1,0,0,0],
           [1,0,1,0,1,1,1,0,1,1,1,0,1,0,1,0,0,0],
           [0,0,0,0] )
            
        self.d = dict(zip(ascii, morse))       
        self.bufReady = False
        self.nextdit = 0

#        print "Make: samp_rate = ", sampRate, "  WPM = ", WPM       
        self._WPM = WPM
        self._sampleRate = sampRate
        #number of samples per dit
        self.expfact = self.ExpFact(self._WPM, self._sampleRate)
        return

    def work(self, input_items, output_items):    
        out = output_items[0] 
        if self.bufReady == False:         #not sending
            if len(out) >= 256:
                out[:256] = np.zeros((256), dtype=np.float32)
                return 256
            else:
                return 0
        else:                               #sending
            if len(out) >= self.expfact:    #room for the next dit?
                if self.mseq[self.nextdit]:
                    out[:self.expfact] = np.ones(self.expfact, dtype=np.float32)
                else:
                    out[:self.expfact] = np.zeros(self.expfact, dtype=np.float32)
                if self.nextdit < self.mlen-1:
                    self.nextdit += 1
                else:
                    self.nextdit = 0
                    self.bufReady = False
                return self.expfact
            else:                           # not enough room for next dit
                return 0
                

# Possible improvement: append new text line an 'awaiting' text line,
# but not to the in-process text line.  Concern about thread safety...
    def set_TextInput(self, Text_In):
        if self.bufReady:
            print "Text input overrun, text discarded."
        else:                           # build Morse, one sample per dit
            self.mseq = np.array(0, dtype=np.float32)
            for char in Text_In.upper():
                if char in self.d:  #only append characters in our dictionary
                    self.mseq = np.append(self.mseq, np.array(self.d[char], dtype=np.float32))
            self.mlen = len(self.mseq)
            self.nextdit = 0
            self.bufReady = True  #treated like a semaphore
        return

    def ExpFact(self, WPM, sampRate):  #samples per dit (expansion factor)
        return int(sampRate * 1.2 / WPM)
        
    def set_WPM(self, WPM):
        if WPM < 8:
            print "::: CW Speed must be at at least 8 WPM. Setting to 8 WPM"
            self._WPM = 8
        else:
            self._WPM = WPM
        self.expfact = self.ExpFact(self._WPM, self._sampleRate)
        return
        
    def set_sampleRate(self, sampRate):
        self._sampleRate = sampRate
        self.expfact = self.ExpFact(self._WPM, self._sampleRate)
        return
    
       
        
#
# Computing the dit pulse expansion factor.
# 1 dit --> nnn samples. Depends on the sample_rate and the WordsPerMinute.
# One dit time in seconds  = 1.2 / WPM
# One dit expands to ss samples...
#        one dit time worth of samples = samp_rate * 1.2 / WPM
#
# Example:  at 20 WPM, and 48000 samples_sec, one isolated CW dit (without
# any pre- or post- spaces) lasts 1.2/20 = 60 milliseconds
# 60 msec is 48000 * .060 = 2880 samples
#
# The largest gnuradio buffer is usually 8192 samples, which
# corresponds to a little over 7 WPM at 48ksps.





                 
                        
            
      
    
        
        

    
