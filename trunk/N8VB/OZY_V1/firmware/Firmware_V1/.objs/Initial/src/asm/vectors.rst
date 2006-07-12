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
   0000                      40 __standard_interrupt_vector::
   0000                      41 __reset_vector::
   0000 02 0F 01             42 	ljmp	s_GSINIT
                             43 	
                             44 	;; 13 8-byte entries.  We point them all at __isr_nop
   0003 02 00 6B             45 	ljmp	__isr_nop	; 3 bytes
   0006                      46 	.ds	5		; + 5 = 8 bytes for vector slot
   000B 02 00 6B             47 	ljmp	__isr_nop
   000E                      48 	.ds	5
   0013 02 00 6B             49 	ljmp	__isr_nop
   0016                      50 	.ds	5
   001B 02 00 6B             51 	ljmp	__isr_nop
   001E                      52 	.ds	5
   0023 02 00 6B             53 	ljmp	__isr_nop
   0026                      54 	.ds	5
   002B 02 00 6B             55 	ljmp	__isr_nop
   002E                      56 	.ds	5
   0033 02 00 6B             57 	ljmp	__isr_nop
   0036                      58 	.ds	5
   003B 02 00 6B             59 	ljmp	__isr_nop
   003E                      60 	.ds	5
   0043 02 00 6B             61 	ljmp	__isr_nop
   0046                      62 	.ds	5
   004B 02 00 6B             63 	ljmp	__isr_nop
   004E                      64 	.ds	5
   0053 02 00 6B             65 	ljmp	__isr_nop
   0056                      66 	.ds	5
   005B 02 00 6B             67 	ljmp	__isr_nop
   005E                      68 	.ds	5
   0063 02 00 6B             69 	ljmp	__isr_nop
   0066                      70 	.ds	5
                             71 
   006B                      72 __isr_nop::
   006B 32                   73 	reti
                             74 
                             75 ;;; ----------------------------------------------------------------
                             76 ;;; the FIFO/GPIF autovector.  14 4-byte entries.
                             77 ;;; must start on a 128 byte boundary.
                             78 ;;; ----------------------------------------------------------------
                             79 	
                    0080     80 	. = __reset_vector + 0x0080
                             81 		
   0080                      82 __fifo_gpif_autovector::
   0080 02 00 6B             83 	ljmp	__isr_nop
   0083 00                   84 	nop	
   0084 02 00 6B             85 	ljmp	__isr_nop
   0087 00                   86 	nop	
   0088 02 00 6B             87 	ljmp	__isr_nop
   008B 00                   88 	nop	
   008C 02 00 6B             89 	ljmp	__isr_nop
   008F 00                   90 	nop	
   0090 02 00 6B             91 	ljmp	__isr_nop
   0093 00                   92 	nop	
   0094 02 00 6B             93 	ljmp	__isr_nop
   0097 00                   94 	nop	
   0098 02 00 6B             95 	ljmp	__isr_nop
   009B 00                   96 	nop	
   009C 02 00 6B             97 	ljmp	__isr_nop
   009F 00                   98 	nop	
   00A0 02 00 6B             99 	ljmp	__isr_nop
   00A3 00                  100 	nop	
   00A4 02 00 6B            101 	ljmp	__isr_nop
   00A7 00                  102 	nop	
   00A8 02 00 6B            103 	ljmp	__isr_nop
   00AB 00                  104 	nop	
   00AC 02 00 6B            105 	ljmp	__isr_nop
   00AF 00                  106 	nop	
   00B0 02 00 6B            107 	ljmp	__isr_nop
   00B3 00                  108 	nop	
   00B4 02 00 6B            109 	ljmp	__isr_nop
   00B7 00                  110 	nop	
                            111 
                            112 	
                            113 ;;; ----------------------------------------------------------------
                            114 ;;; the USB autovector.  32 4-byte entries.
                            115 ;;; must start on a 256 byte boundary.
                            116 ;;; ----------------------------------------------------------------
                            117 
                    0100    118 	. = __reset_vector + 0x0100
                            119 	
   0100                     120 __usb_autovector::
   0100 02 00 6B            121 	ljmp	__isr_nop
   0103 00                  122 	nop
   0104 02 00 6B            123 	ljmp	__isr_nop
   0107 00                  124 	nop
   0108 02 00 6B            125 	ljmp	__isr_nop
   010B 00                  126 	nop
   010C 02 00 6B            127 	ljmp	__isr_nop
   010F 00                  128 	nop
   0110 02 00 6B            129 	ljmp	__isr_nop
   0113 00                  130 	nop
   0114 02 00 6B            131 	ljmp	__isr_nop
   0117 00                  132 	nop
   0118 02 00 6B            133 	ljmp	__isr_nop
   011B 00                  134 	nop
   011C 02 00 6B            135 	ljmp	__isr_nop
   011F 00                  136 	nop
   0120 02 00 6B            137 	ljmp	__isr_nop
   0123 00                  138 	nop
   0124 02 00 6B            139 	ljmp	__isr_nop
   0127 00                  140 	nop
   0128 02 00 6B            141 	ljmp	__isr_nop
   012B 00                  142 	nop
   012C 02 00 6B            143 	ljmp	__isr_nop
   012F 00                  144 	nop
   0130 02 00 6B            145 	ljmp	__isr_nop
   0133 00                  146 	nop
   0134 02 00 6B            147 	ljmp	__isr_nop
   0137 00                  148 	nop
   0138 02 00 6B            149 	ljmp	__isr_nop
   013B 00                  150 	nop
   013C 02 00 6B            151 	ljmp	__isr_nop
   013F 00                  152 	nop
   0140 02 00 6B            153 	ljmp	__isr_nop
   0143 00                  154 	nop
   0144 02 00 6B            155 	ljmp	__isr_nop
   0147 00                  156 	nop
   0148 02 00 6B            157 	ljmp	__isr_nop
   014B 00                  158 	nop
   014C 02 00 6B            159 	ljmp	__isr_nop
   014F 00                  160 	nop
   0150 02 00 6B            161 	ljmp	__isr_nop
   0153 00                  162 	nop
   0154 02 00 6B            163 	ljmp	__isr_nop
   0157 00                  164 	nop
   0158 02 00 6B            165 	ljmp	__isr_nop
   015B 00                  166 	nop
   015C 02 00 6B            167 	ljmp	__isr_nop
   015F 00                  168 	nop
   0160 02 00 6B            169 	ljmp	__isr_nop
   0163 00                  170 	nop
   0164 02 00 6B            171 	ljmp	__isr_nop
   0167 00                  172 	nop
   0168 02 00 6B            173 	ljmp	__isr_nop
   016B 00                  174 	nop
   016C 02 00 6B            175 	ljmp	__isr_nop
   016F 00                  176 	nop
   0170 02 00 6B            177 	ljmp	__isr_nop
   0173 00                  178 	nop
   0174 02 00 6B            179 	ljmp	__isr_nop
   0177 00                  180 	nop
   0178 02 00 6B            181 	ljmp	__isr_nop
   017B 00                  182 	nop
   017C 02 00 6B            183 	ljmp	__isr_nop
   017F 00                  184 	nop
