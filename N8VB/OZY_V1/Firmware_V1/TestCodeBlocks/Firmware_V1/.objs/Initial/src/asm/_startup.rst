                              1 ;;; -*- asm -*-
                              2 ;;;
                              3 ;;; HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
                              4 ;;;
                              5 ;;; Adapted from USRP firmware 07/10/2006 by Phil Covington N8VB
                              6 ;;;
                              7 ;;; Copyright 2003,2004 Free Software Foundation, Inc.
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
                             25 
                             26     
                             27 ;;; The default external memory initialization provided by sdcc is not
                             28 ;;; appropriate to the FX2.  This is derived from the sdcc code, but uses 
                             29 ;;; the FX2 specific _MPAGE sfr.
                             30 
                             31 
                             32 	;; .area XISEG   (XDATA)  ; the initialized external data area
                             33 	;; .area XINIT   (CODE)	  ; the code space consts to init XISEG
                             34 	.area XSEG    (XDATA)	  ; zero initialized xdata
                             35 	.area USBDESCSEG (XDATA)  ; usb descriptors
                             36 
                             37 	
                             38 	.area CSEG    (CODE)
                             39 
                             40 	;; sfr that sets upper address byte of MOVX using @r0 or @r1
                    0092     41 	_MPAGE	=	0x0092
                             42 
   04FE                      43 __sdcc_external_startup::
                             44 	;; This system is now compiled with the --no-xinit-opt 
                             45 	;; which means that any initialized XDATA is handled
                             46 	;; inline by code in the GSINIT segs emitted for each file.
                             47 	;; 
                             48 	;; We zero XSEG and all of the internal ram to ensure 
                             49 	;; a known good state for uninitialized variables.
                             50 
                             51 ;	_mcs51_genRAMCLEAR() start
   04FE 78 0A                52 	mov	r0,#l_XSEG
   0500 E8                   53 	mov	a,r0
   0501 44 00                54 	orl	a,#(l_XSEG >> 8)
   0503 60 0C                55 	jz	00002$
   0505 79 01                56 	mov	r1,#((l_XSEG + 255) >> 8)
   0507 90 20 00             57 	mov	dptr,#s_XSEG
   050A E4                   58 	clr     a
                             59 	
   050B F0                   60 00001$:	movx	@dptr,a
   050C A3                   61 	inc	dptr
   050D D8 FC                62 	djnz	r0,00001$
   050F D9 FA                63 	djnz	r1,00001$
                             64 	
                             65 	;; We're about to clear internal memory.  This will overwrite
                             66 	;; the stack which contains our return address.
                             67 	;; Pop our return address into DPH, DPL
   0511 D0 83                68 00002$:	pop	dph
   0513 D0 82                69 	pop	dpl
                             70 	
                             71 	;; R0 and A contain 0.  This loop will execute 256 times.
                             72 	;; 
                             73 	;; FWIW the first iteration writes direct address 0x00,
                             74 	;; which is the location of r0.  We get lucky, we're 
                             75 	;; writing the correct value (0)
                             76 	
   0515 F6                   77 00003$:	mov	@r0,a
   0516 D8 FD                78 	djnz	r0,00003$
                             79 
   0518 C0 82                80 	push	dpl		; restore our return address
   051A C0 83                81 	push	dph
                             82 
   051C 75 82 00             83 	mov	dpl,#0		; indicate that data init is still required
   051F 22                   84 	ret
