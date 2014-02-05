#!/usr/bin/env python
##################################################
# Gnuradio Python Flow Graph
# Title: Fast Autocor
# Generated: Fri Dec 13 11:54:34 2013
##################################################

from gnuradio import blocks
from gnuradio import fft
from gnuradio import gr
from gnuradio.fft import window
from gnuradio.filter import firdes

class fast_autocor(gr.hier_block2):

    def __init__(self, fft_size=4096):
        gr.hier_block2.__init__(
            self, "Fast Autocor",
            gr.io_signature(1, 1, gr.sizeof_gr_complex*1),
            gr.io_signature(1, 1, gr.sizeof_float*1),
        )

        ##################################################
        # Parameters
        ##################################################
        self.fft_size = fft_size

        ##################################################
        # Blocks
        ##################################################
        self.fft_vxx_0_0 = fft.fft_vcc(fft_size, False, (window.hamming(fft_size)), False, 1)
        self.fft_vxx_0 = fft.fft_vcc(fft_size, True, (window.hamming(fft_size)), False, 1)
        self.blocks_vector_to_stream_0_0 = blocks.vector_to_stream(gr.sizeof_gr_complex*1, fft_size)
        self.blocks_vector_to_stream_0 = blocks.vector_to_stream(gr.sizeof_gr_complex*1, fft_size)
        self.blocks_stream_to_vector_0_0 = blocks.stream_to_vector(gr.sizeof_gr_complex*1, fft_size)
        self.blocks_stream_to_vector_0 = blocks.stream_to_vector(gr.sizeof_gr_complex*1, fft_size)
        self.blocks_multiply_const_vxx_1 = blocks.multiply_const_vff((1.0/fft_size, ))
        self.blocks_multiply_const_vxx_0 = blocks.multiply_const_vff((2048.0/fft_size, ))
        self.blocks_float_to_complex_0 = blocks.float_to_complex(1)
        self.blocks_complex_to_real_0 = blocks.complex_to_real(1)
        self.blocks_complex_to_mag_squared_1 = blocks.complex_to_mag_squared(1)

        ##################################################
        # Connections
        ##################################################
        self.connect((self.blocks_vector_to_stream_0, 0), (self.blocks_complex_to_mag_squared_1, 0))
        self.connect((self.fft_vxx_0, 0), (self.blocks_vector_to_stream_0, 0))
        self.connect((self.blocks_stream_to_vector_0, 0), (self.fft_vxx_0, 0))
        self.connect((self.blocks_stream_to_vector_0_0, 0), (self.fft_vxx_0_0, 0))
        self.connect((self.fft_vxx_0_0, 0), (self.blocks_vector_to_stream_0_0, 0))
        self.connect((self.blocks_vector_to_stream_0_0, 0), (self.blocks_complex_to_real_0, 0))
        self.connect((self.blocks_float_to_complex_0, 0), (self.blocks_stream_to_vector_0_0, 0))
        self.connect((self.blocks_complex_to_real_0, 0), (self.blocks_multiply_const_vxx_0, 0))
        self.connect((self, 0), (self.blocks_stream_to_vector_0, 0))
        self.connect((self.blocks_multiply_const_vxx_0, 0), (self, 0))
        self.connect((self.blocks_complex_to_mag_squared_1, 0), (self.blocks_multiply_const_vxx_1, 0))
        self.connect((self.blocks_multiply_const_vxx_1, 0), (self.blocks_float_to_complex_0, 0))


# QT sink close method reimplementation

    def get_fft_size(self):
        return self.fft_size

    def set_fft_size(self, fft_size):
        self.fft_size = fft_size
        self.blocks_multiply_const_vxx_1.set_k((1.0/self.fft_size, ))
        self.blocks_multiply_const_vxx_0.set_k((2048.0/self.fft_size, ))


