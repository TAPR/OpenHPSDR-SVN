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

 
# create a linear chirped baseband output, from -deviation to +deviation
# The parameter is chirp rate - in Hertz per millisecond.
# Optionally zero-pad to 2^N samples. 
# zeropad pads out the chirp to 2^n sampltes with zeros front and back.
# window is the time to ramp voltage up and down using a raised cosine time window

class ChirpSource(gr.sync_block):
    
    def __init__(self, chirpRate, sampRate, direction, amplitude, deviation,
                 zeropad, window):
        gr.sync_block.__init__(
            self,
            name="ChirpSource",
            in_sig=None,
            out_sig=[np.complex64])     

        self.chirpRate = chirpRate
        self.sampleRate = sampRate
        self.direction = direction
        self.amplitude = amplitude
        self.deviation = deviation
        self.zeropad = zeropad
        self.window = window
        self.indexer = 0
        self.BuildArray()    
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


    def BuildArray(self):  
        self.k = self.chirpRate * 1000  # convert to Hertz per second
        self.t = (self.deviation * 2) / float(self.k) # time for one chirp
        self.f0 = -self.deviation    #lowest frequency (start of) chirp
        self.n = int(self.t * self.sampleRate)    # number of time samples in one chirp
        self.w = (self.window * self.sampleRate) #ramp duration in samples

        # compute size of 2^n block that fits the set of samples
        if self.zeropad == 1:
            log2size = math.log(self.n, 2)
            m = int(log2size)
            if log2size > float(m):
                m += 1
            block = 2 ** m
            excess = block - self.n
            self.pre = int(excess / 2)       # #samples to pre-zeropad
            self.post = excess - self.pre    # #samples to post-zeropad
        else:
            self.pre = 0                     # no zeropad
            self.post = 0

        self.buf = []           # empty list
        self.chirpbuf = []      #empty list

        
        print "BuildArray: k=",self.k," t=",self.t," n=",self.n," f0=",self.f0,
        print "padding =", self.pre+self.post, "total_block =",self.n+self.pre+self.post
        print "window =", self.window, " w=",self.w     
         
         
        # build complete sample array as one python list 

        for i in range(self.pre):
            self.buf.append(complex(0.0, 0.0))
            
        for i in range(self.n):
            ampl = self.amplitude
            
            if i < self.w:  #raised cosine ramp up
                rampangle = float(i) / float(self.w) * math.pi
                ampl = ampl * (1.0 - (1.0 + math.cos(rampangle))/2.0)
            if i > (self.n - self.w): #raised cosine ramp down
                rampangle = float(self.n - i) * math.pi / float(self.w)
                ampl = ampl * (1.0 - (1.0 + math.cos(rampangle))/2.0) 
                
            time = i / float(self.sampleRate)
            phi = 2.0 * math.pi * (self.f0 * time + self.k * time * time /2.0)
            re = ampl * math.cos(phi)
            im = ampl * math.sin(phi)        
            if self.direction:
                self.buf.append(complex(re, -im))
            else:
                self.buf.append(complex(re, im))

        for i in range(self.post):
            self.buf.append(complex(0.0, 0.0))
     
        self.n = self.pre + self.n + self.post # total sample count
     
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
        

    def set_chirpRate(self, chirpRate):
        self.chirpRate = chirpRate
        self.BuildArray()
        return

        
    def set_sampleRate(self, sampRate):
        self.sampleRate = sampRate
        self.BuildArray()
        return


    def set_direction(self, direction):
        self.direction = direction
        self.BuildArray()
        return


    def set_amplitude(self, amplitude):
        self.amplitude = amplitude
        self.BuildArray()
        return


    def set_deviation(self, deviation):
        self.deviation = deviation
        self.BuildArray()
        return

    def set_zeropad(self, zeropad):
        self.zeropad = zeropad
        self.BuildArray()
        return
            
    def set_window(self, window):
        self.window = window
        self.BuildArray()
        return
    
        
        

    
