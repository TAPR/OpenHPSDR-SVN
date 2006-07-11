                              1 ;;; -*- asm -*-
                              2 ;;;
                              3 ;;; HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
                              4 ;;;
                              5 ;;; Adapted from USRP firmware 07/10/2006 by Phil Covington N8VB
                              6 ;;;
                              7 ;;; Copyright 2003 Free Software Foundation, Inc.
                              8 ;;; 
                              9 ;;; This file is part of GNU Radio
                             10 ;;; 
                             11 ;;; GNU Radio is free software; you can redistribute it and/or modify
                             12 ;;; it under the terms of the GNU General Public License as published by
                             13 ;;; the Free Software Foundation; either version 2, or (at your option)
                             14 ;;; any later version.
                             15 ;;; 
                             16 ;;; GNU Radio is distributed in the hope that it will be useful,
                             17 ;;; but WITHOUT ANY WARRANTY; without even the implied warranty of
                             18 ;;; MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
                             19 ;;; GNU General Public License for more details.
                             20 ;;; 
                             21 ;;; You should have received a copy of the GNU General Public License
                             22 ;;; along with GNU Radio; see the file COPYING.  If not, write to
                             23 ;;; the Free Software Foundation, Inc., 59 Temple Place - Suite 330,
                             24 ;;; Boston, MA 02111-1307, USA.
                             25 ;;; 
                             26 
                             27 ;;; Interrupt vectors.
                             28 
                             29 ;;; N.B. This object module must come first in the list of modules
                             30 
                             31 	.module vectors
                             32 
                             33 ;;; ----------------------------------------------------------------
                             34 ;;;		  standard FX2 interrupt vectors
                             35 ;;; ----------------------------------------------------------------
                             36 
                             37 	.area CSEG (CODE)
                             38 	.area GSINIT (CODE)
                             39 	.area CSEG (CODE)
   01F5                      40 __standard_interrupt_vector::
   01F5                      41 __reset_vector::
   01F5 02 0F 5A             42 	ljmp	s_GSINIT
                             43 	
                             44 	;; 13 8-byte entries.  We point them all at __isr_nop
   01F8 02 02 60             45 	ljmp	__isr_nop	; 3 bytes
   01FB                      46 	.ds	5		; + 5 = 8 bytes for vector slot
   0200 02 02 60             47 	ljmp	__isr_nop
   0203                      48 	.ds	5
   0208 02 02 60             49 	ljmp	__isr_nop
   020B                      50 	.ds	5
   0210 02 02 60             51 	ljmp	__isr_nop
   0213                      52 	.ds	5
   0218 02 02 60             53 	ljmp	__isr_nop
   021B                      54 	.ds	5
   0220 02 02 60             55 	ljmp	__isr_nop
   0223                      56 	.ds	5
   0228 02 02 60             57 	ljmp	__isr_nop
   022B                      58 	.ds	5
   0230 02 02 60             59 	ljmp	__isr_nop
   0233                      60 	.ds	5
   0238 02 02 60             61 	ljmp	__isr_nop
   023B                      62 	.ds	5
   0240 02 02 60             63 	ljmp	__isr_nop
   0243                      64 	.ds	5
   0248 02 02 60             65 	ljmp	__isr_nop
   024B                      66 	.ds	5
   0250 02 02 60             67 	ljmp	__isr_nop
   0253                      68 	.ds	5
   0258 02 02 60             69 	ljmp	__isr_nop
   025B                      70 	.ds	5
                             71 
   0260                      72 __isr_nop::
   0260 32                   73 	reti
                             74 
                             75 ;;; ----------------------------------------------------------------
                             76 ;;; the FIFO/GPIF autovector.  14 4-byte entries.
                             77 ;;; must start on a 128 byte boundary.
                             78 ;;; ----------------------------------------------------------------
                             79 	
                    0080     80 	. = __reset_vector + 0x0080
                             81 		
   0275                      82 __fifo_gpif_autovector::
   0275 02 02 60             83 	ljmp	__isr_nop
   0278 00                   84 	nop	
   0279 02 02 60             85 	ljmp	__isr_nop
   027C 00                   86 	nop	
   027D 02 02 60             87 	ljmp	__isr_nop
   0280 00                   88 	nop	
   0281 02 02 60             89 	ljmp	__isr_nop
   0284 00                   90 	nop	
   0285 02 02 60             91 	ljmp	__isr_nop
   0288 00                   92 	nop	
   0289 02 02 60             93 	ljmp	__isr_nop
   028C 00                   94 	nop	
   028D 02 02 60             95 	ljmp	__isr_nop
   0290 00                   96 	nop	
   0291 02 02 60             97 	ljmp	__isr_nop
   0294 00                   98 	nop	
   0295 02 02 60             99 	ljmp	__isr_nop
   0298 00                  100 	nop	
   0299 02 02 60            101 	ljmp	__isr_nop
   029C 00                  102 	nop	
   029D 02 02 60            103 	ljmp	__isr_nop
   02A0 00                  104 	nop	
   02A1 02 02 60            105 	ljmp	__isr_nop
   02A4 00                  106 	nop	
   02A5 02 02 60            107 	ljmp	__isr_nop
   02A8 00                  108 	nop	
   02A9 02 02 60            109 	ljmp	__isr_nop
   02AC 00                  110 	nop	
                            111 
                            112 	
                            113 ;;; ----------------------------------------------------------------
                            114 ;;; the USB autovector.  32 4-byte entries.
                            115 ;;; must start on a 256 byte boundary.
                            116 ;;; ----------------------------------------------------------------
                            117 
                    0100    118 	. = __reset_vector + 0x0100
                            119 	
   02F5                     120 __usb_autovector::
   02F5 02 02 60            121 	ljmp	__isr_nop
   02F8 00                  122 	nop
   02F9 02 02 60            123 	ljmp	__isr_nop
   02FC 00                  124 	nop
   02FD 02 02 60            125 	ljmp	__isr_nop
   0300 00                  126 	nop
   0301 02 02 60            127 	ljmp	__isr_nop
   0304 00                  128 	nop
   0305 02 02 60            129 	ljmp	__isr_nop
   0308 00                  130 	nop
   0309 02 02 60            131 	ljmp	__isr_nop
   030C 00                  132 	nop
   030D 02 02 60            133 	ljmp	__isr_nop
   0310 00                  134 	nop
   0311 02 02 60            135 	ljmp	__isr_nop
   0314 00                  136 	nop
   0315 02 02 60            137 	ljmp	__isr_nop
   0318 00                  138 	nop
   0319 02 02 60            139 	ljmp	__isr_nop
   031C 00                  140 	nop
   031D 02 02 60            141 	ljmp	__isr_nop
   0320 00                  142 	nop
   0321 02 02 60            143 	ljmp	__isr_nop
   0324 00                  144 	nop
   0325 02 02 60            145 	ljmp	__isr_nop
   0328 00                  146 	nop
   0329 02 02 60            147 	ljmp	__isr_nop
   032C 00                  148 	nop
   032D 02 02 60            149 	ljmp	__isr_nop
   0330 00                  150 	nop
   0331 02 02 60            151 	ljmp	__isr_nop
   0334 00                  152 	nop
   0335 02 02 60            153 	ljmp	__isr_nop
   0338 00                  154 	nop
   0339 02 02 60            155 	ljmp	__isr_nop
   033C 00                  156 	nop
   033D 02 02 60            157 	ljmp	__isr_nop
   0340 00                  158 	nop
   0341 02 02 60            159 	ljmp	__isr_nop
   0344 00                  160 	nop
   0345 02 02 60            161 	ljmp	__isr_nop
   0348 00                  162 	nop
   0349 02 02 60            163 	ljmp	__isr_nop
   034C 00                  164 	nop
   034D 02 02 60            165 	ljmp	__isr_nop
   0350 00                  166 	nop
   0351 02 02 60            167 	ljmp	__isr_nop
   0354 00                  168 	nop
   0355 02 02 60            169 	ljmp	__isr_nop
   0358 00                  170 	nop
   0359 02 02 60            171 	ljmp	__isr_nop
   035C 00                  172 	nop
   035D 02 02 60            173 	ljmp	__isr_nop
   0360 00                  174 	nop
   0361 02 02 60            175 	ljmp	__isr_nop
   0364 00                  176 	nop
   0365 02 02 60            177 	ljmp	__isr_nop
   0368 00                  178 	nop
   0369 02 02 60            179 	ljmp	__isr_nop
   036C 00                  180 	nop
   036D 02 02 60            181 	ljmp	__isr_nop
   0370 00                  182 	nop
   0371 02 02 60            183 	ljmp	__isr_nop
   0374 00                  184 	nop
