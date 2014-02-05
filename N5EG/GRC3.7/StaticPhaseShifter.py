#!/usr/bin/env python
##################################################
# Gnuradio Python Flow Graph
# Title: Staticphaseshifter
# Generated: Sat Sep 28 10:31:06 2013
##################################################

from gnuradio import blocks
from gnuradio import gr
from gnuradio.filter import firdes

class StaticPhaseShifter(gr.hier_block2):

    def __init__(self):
        gr.hier_block2.__init__(
            self, "Staticphaseshifter",
            gr.io_signaturev(2, 2, [gr.sizeof_gr_complex*1, gr.sizeof_float*1]),
            gr.io_signature(1, 1, gr.sizeof_gr_complex*1),
        )

        ##################################################
        # Blocks
        ##################################################
        self.blocks_transcendental_0_0 = blocks.transcendental("sin", "float")
        self.blocks_transcendental_0 = blocks.transcendental("cos", "float")
        self.blocks_multiply_xx_0_0 = blocks.multiply_vcc(1)
        self.blocks_float_to_complex_0_0 = blocks.float_to_complex(1)

        ##################################################
        # Connections
        ##################################################
        self.connect((self, 1), (self.blocks_transcendental_0_0, 0))
        self.connect((self, 0), (self.blocks_multiply_xx_0_0, 0))
        self.connect((self.blocks_multiply_xx_0_0, 0), (self, 0))
        self.connect((self.blocks_float_to_complex_0_0, 0), (self.blocks_multiply_xx_0_0, 1))
        self.connect((self.blocks_transcendental_0, 0), (self.blocks_float_to_complex_0_0, 0))
        self.connect((self.blocks_transcendental_0_0, 0), (self.blocks_float_to_complex_0_0, 1))
        self.connect((self, 1), (self.blocks_transcendental_0, 0))


# QT sink close method reimplementation


