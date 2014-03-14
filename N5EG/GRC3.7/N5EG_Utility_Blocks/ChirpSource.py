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

 
# create a linear chirped baseband output, from -freq to +freq
# The parameter is chirp rate - in Hertz per millisecond.

class ChirpSource(gr.sync_block):
    
    def __init__(self, chirpRate, sampRate, direction, amplitude):
        gr.sync_block.__init__(
            self,
            name="ChirpSource",
            in_sig=None,
            out_sig=[np.complex64])     

        self.chirpRate = chirpRate
        self.sampleRate = sampRate
        self.direction = direction
        self.amplitude = amplitude
        self.indexer = 0
        self.BuildArray(chirpRate, sampRate, direction, amplitude)    
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
            


#
# f(t) = f0 + kt
# k = (f1 - f0)/t1 = chirpRate in Hertz per second
# for full band sweep, goes from -sampRate/2 to +sampRate/2
# phi(t) = 2PI *(f0t + (kt^2)/2 )
# X(t) = cos(phi(t)) + jsin(phi(t))
# samples in sweep =  sampRate / chirpRate
# k = chirp rate in Hertz per second
# n = number of samples in one chirp sweep
# t = time in seconds for one chirp sweep


    def BuildArray(self, chirpRate, sampRate, direction, amplitude):  
        self.k = chirpRate * 1000  # convert to Hertz per second
        self.t = sampRate / float(self.k) # time for one chirp
        self.n = int(self.t * sampRate)    # number of time samples in one chirp
        self.f0 = -sampRate/2    #lowest frequency (start of) chirp
        self.buf = []           # empty list
        self.chirpbuf = []      #empty list

        
        print "BuildArray: k=",self.k," t=",self.t," n=",self.n," f0=",self.f0,
         
        # build complete sample array as one python list 
        for i in range(self.n):
            time = i / float(sampRate)
            phi = 2.0 * math.pi * (self.f0 * time + self.k * time * time /2.0)
            re = amplitude * math.cos(phi)
            im = amplitude * math.sin(phi)
            if direction:
                self.buf.append(complex(re, -im))
            else:
                self.buf.append(complex(re, im))

                          
        # split sample list into multiple np.arrays  
        self.count = self.n / 4096        # number of full buffers
        self.remainder = self.n % 4096    #last buffer size
        
        print " count =",self.count," remainder =",self.remainder
                   
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
        

    def set_chirpRate(self, chirpRate):
        self.chirpRate = chirpRate
        self.BuildArray(chirpRate, self.sampleRate, self.direction, self.amplitude)
        return

        
    def set_sampleRate(self, sampRate):
        self.sampleRate = sampRate
        self.BuildArray(self.chirpRate, sampRate, self.direction, self.amplitude)
        return


    def set_direction(self, direction):
        self.direction = direction
        self.BuildArray(self.chirpRate, self.sampRate, direction, self.amplitude)
        return


    def set_amplitude(self, amplitude):
        self.amplitude = amplitude
        self.BuildArray(self.chirpRate, self.sampRate, self.direction, amplitude)
        return


            
      
    
        
        

    
