#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Created on Mon Jun 23 16:00:00 2014

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

 
# Clip the signal at +ClipLevel volts and at -Cliplevel volts
 
class Clipper(gr.sync_block):
    
    def __init__(self, ClipLevel):
        gr.sync_block.__init__(
            self,
            name="Clipper",
            in_sig=[np.complex64],
            out_sig=[np.complex64])     

        self.ClipLevel = ClipLevel
        return



# If Mag > 1 then   I' = I/Mag  and Q' = Q/Mag
# len(inp) is the number of items to operate on

    def work(self, input_items, output_items):    
        inp = input_items[0]
        out = output_items[0]
        for i in range(len(inp)):
            re = inp[i].real
            im = inp[i].imag
            mag = math.sqrt(re**2+im**2)
            if mag>self.ClipLevel:
                out[i] = complex(re*self.ClipLevel/mag, im*self.ClipLevel/mag)
            else:
                out[i] = complex(re, im)
        return len(inp)
                
            

    def set_ClipLevel(self, ClipLevel):
        self.ClipLevel = ClipLevel
        return

 

            
      
    
        
        

    
