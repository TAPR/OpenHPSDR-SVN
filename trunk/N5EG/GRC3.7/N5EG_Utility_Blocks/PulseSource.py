#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Created on Mon Mar 10 19:48:00 2014

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
import math

 
# Create a sinsoidal signal at +6 KHz offset from zero
# Pulse width and pulse separation are in units of 166.67 microsecond
 
class PulseSource(gr.sync_block):
    
    def __init__(self, pulseWidth, sampRate, pulseSep, amplitude):
        gr.sync_block.__init__(
            self,
            name="ChirpSource",
            in_sig=None,
            out_sig=[np.complex64])     

        self.pulseWidth = pulseWidth
        self.sampleRate = sampRate
        self.pulseSep = pulseSep
        self.amplitude = amplitude
        self.indexer = 0
        self.BuildArray(pulseWidth, sampRate, pulseSep, amplitude)    
        return




    def work(self, input_items, output_items):    
        out = output_items[0]
        if len(out) >= 4096:    #room for a buffer
            if self.indexer < self.count:  #full buffer available
                out[:4096] = self.chirpbuf[self.indexer]
                self.indexer += 1
                return 4096
            else:
                if self.remainder > 0:
                    out[:self.remainder] = self.chirpbuf[self.indexer]
                self.indexer = 0
                return self.remainder
        else:
            return 0
            


# build the pulse into one (or more) buffers.

    def BuildArray(self, pulseWidth, sampRate, pulseSep, amplitude):  
        # n = number of time samples in    pulse + sep
        # At 48,000 samp/sec, it's 8 samples per unit (166.67 microseconds per unit)
        self.n = (pulseWidth + pulseSep) * (sampRate/6000) 
        self.buf = []           # empty list
        self.chirpbuf = []      #empty list

        
        print "BuildArray: n=",self.n,
         
        # build complete sample array as one python list 

        for i in range(self.pulseWidth * (sampRate/6000)): #on time
            phi = 2.0 * math.pi * float(( i % (sampRate/6000) / 8.0))
            re = amplitude * math.cos(phi)
            im = amplitude * math.sin(phi)
            self.buf.append(complex(re, im))         
             
        for i in range(self.pulseSep * (sampRate/6000)):  #off time
            self.buf.append(complex(0.0, 0.0))

                          
        # split sample list into multiple np.arrays  
        self.count = self.n / 4096        # number of full buffers
        self.remainder = self.n % 4096    #last buffer size
        
#        print " count =",self.count," remainder =",self.remainder
                   
        x = 0
        while x < self.count:
            front = x * 4096
            back = front + 4096
            self.chirpbuf.append(np.array(self.buf[front:back], dtype=np.complex64))
            x += 1

        if self.remainder > 0:
            front = x * 4096
            back = front + self.remainder
#            print "front =",front," back =",back, "x =",x
            self.chirpbuf.append(np.array(self.buf[front:back], dtype=np.complex64))
                           
        return
        

    def set_pulseWidth(self, pulseWidth):
        self.pulseWidth = pulseWidth
        self.BuildArray(pulseWidth, self.sampleRate, self.pulseSep, self.amplitude)
        return

        
    def set_sampleRate(self, sampRate):
        self.sampleRate = sampRate
        self.BuildArray(self.pulseWidth, sampRate, self.pulseSep, self.amplitude)
        return


    def set_pulseSep(self, pulseSep):
        self.pulseSep = pulseSep
        self.BuildArray(self.pulseWidth, self.sampRate, pulseSep, self.amplitude)
        return


    def set_amplitude(self, amplitude):
        self.amplitude = amplitude
        self.BuildArray(self.pulseWidth, self.sampRate, self.pulseSep, amplitude)
        return


            
      
    
        
        

    
