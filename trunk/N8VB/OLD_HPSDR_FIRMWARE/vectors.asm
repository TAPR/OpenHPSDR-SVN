;/*
;Copyright (c) 2005 Philip A Covington, N8VB
;
;Permission is hereby granted, free of charge, to any person obtaining a copy of
;this software and associated documentation files (the "Software"), to deal in
;the Software without restriction, including without limitation the rights to
;use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
;the Software, and to permit persons to whom the Software is furnished to do so,
;subject to the following conditions:
;
;The above copyright notice and this permission notice shall be included in all
;copies or substantial portions of the Software.
;
;THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
;IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
;FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
;COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
;IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
;CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
;*/

; Interrupt vectors for FX2 when using SDCC
;
; assemble with: asx8051 -plosgff vectors.asm
; put the resulting object module first in the list of object modules
; compile like this: sdcc -Wl -bVECT=0x0000 --code-loc 0x0200 <your_file_name>.rel vectors.rel
; to start code at 0x0200 and place VECT at 0x0000 in code

	.module vectors

; ----------------------------------------------------------------
;		  FX2 interrupt vectors
; ----------------------------------------------------------------

	.area CSEG (CODE)
	.area GSINIT (CODE)
	.area VECT (CODE)

__standard_interrupt_vector::
__reset_vector::

	ljmp	s_GSINIT		
					
	ljmp	_ISR_Ext0 		;INT0 (EXT0)
	.ds	5			
	ljmp	_ISR_Tmr0		;INT1 (TMR0)
	.ds	5
	ljmp	_ISR_Ext1		;INT2 (EXT1)
	.ds	5
	ljmp	_ISR_Tmr1		;INT3	(TMR1)
	.ds	5
	ljmp	_ISR_Ser0		;INT4 (SER0)
	.ds	5
	ljmp	_ISR_Tmr2		;INT5 (TMR2)
	.ds	5
	ljmp	_ISR_Resume		;INT6 (RESUME)
	.ds	5
	ljmp	_ISR_Ser1		;INT7	(SER1)
	.ds	5
	ljmp	__usb_autovector	;INT8 (USB)
	.ds	5
	ljmp	_ISR_I2c		;INT9 (I2C)
	.ds	5
	ljmp	__fifo_gpif_autovector	;INT10 (GPIF/FIFO)
	.ds	5
	ljmp	_ISR_Int5pin		;INT11 (INT5PIN)
	.ds	5
	ljmp	_ISR_Int6pin		;INT12 (INT6PIN)
	.ds	5

; ----------------------------------------------------------------
; the FIFO/GPIF autovector.  14 4-byte entries.
; must start on a 128 byte boundary.
; ----------------------------------------------------------------
	
	. = __reset_vector + 0x0080
		
__fifo_gpif_autovector::		

	ljmp	_ISR_Ep2pf
	nop	
	ljmp	_ISR_Ep4pf
	nop	
	ljmp	_ISR_Ep6pf
	nop	
	ljmp	_ISR_Ep8pf
	nop	
	ljmp	_ISR_Ep2ef
	nop	
	ljmp	_ISR_Ep4ef
	nop	
	ljmp	_ISR_Ep6ef
	nop	
	ljmp	_ISR_Ep8ef
	nop	
	ljmp	_ISR_Ep2ff
	nop	
	ljmp	_ISR_Ep4ff
	nop	
	ljmp	_ISR_Ep6ff
	nop	
	ljmp	_ISR_Ep8ff
	nop	
	ljmp	_ISR_Gpifdone
	nop	
	ljmp	_ISR_Gpifwf
	nop	

	
; ----------------------------------------------------------------
; the USB autovector.  32 4-byte entries.
; must start on a 256 byte boundary.
; ----------------------------------------------------------------

	. = __reset_vector + 0x0100
	
__usb_autovector::  
					
	ljmp	_ISR_Sudav		;SUDAV
	nop
	ljmp	_ISR_Sof		;SOF
	nop
	ljmp	_ISR_Sutok		;SUTOK
	nop
	ljmp	_ISR_Suspend		;SUSPEND
	nop
	ljmp	_ISR_Usb_Reset		;RESET
	nop
	ljmp	_ISR_Highspeed		;HISPEED
	nop
	ljmp	_ISR_Ep0ack		;EP0ACK
	nop
	ljmp	__isr_nop		;RESERVED
	nop
	ljmp	_ISR_Ep0in		;EP0-IN
	nop
	ljmp	_ISR_Ep0out		;EP0-OUT
	nop
	ljmp	_ISR_Ep1in		;EP1-IN
	nop
	ljmp	_ISR_Ep1out		;EP1-OUT	
	nop
	ljmp	_ISR_Ep2inout		;EP2 IN-OUT
	nop
	ljmp	_ISR_Ep4inout		;EP4 IN-OUT
	nop
	ljmp	_ISR_Ep6inout		;EP6 IN-OUT
	nop
	ljmp	_ISR_Ep8inout		;EP8 IN-OUT
	nop
	ljmp	_ISR_Ibn		;IBN IN-Bulk-NAK
	nop
	ljmp	__isr_nop		;RESERVED
	nop
	ljmp	_ISR_Ep0ping		;EP0PING
	nop
	ljmp	_ISR_Ep1ping		;EP1PING
	nop
	ljmp	_ISR_Ep2ping		;EP2PING
	nop
	ljmp	_ISR_Ep4ping		;EP4PING
	nop
	ljmp	_ISR_Ep6ping		;EP6PING
	nop
	ljmp	_ISR_Ep8ping		;EP8PING
	nop
	ljmp	_ISR_Errlimit		;ERRLIMIT
	nop
	ljmp	__isr_nop		;RESERVED
	nop
	ljmp	__isr_nop		;RESERVED
	nop
	ljmp	__isr_nop		;RESERVED
	nop
	ljmp	_ISR_Ep2isoerr		;EP2ISOERR
	nop
	ljmp	_ISR_Ep4isoerr		;EP4ISOERR
	nop
	ljmp	_ISR_Ep6isoerr		;EP6ISOERR
	nop
	ljmp	_ISR_Ep8isoerr		;EP8ISOERR
	nop

; Comment out used ISRs that
; are used so they don't point to reti
; below.

_ISR_Ext0::
_ISR_Tmr0::
_ISR_Ext1::
_ISR_Tmr1::
_ISR_Ser0::
_ISR_Tmr2::
_ISR_Resume::
_ISR_Ser1::
_ISR_I2c::
_ISR_Int5pin::
_ISR_Int6pin::

_ISR_Ep2pf::
_ISR_Ep4pf::
_ISR_Ep6pf::
_ISR_Ep8pf::
_ISR_Ep2ef::
_ISR_Ep4ef::
_ISR_Ep6ef::
_ISR_Ep8ef::
_ISR_Ep2ff::
_ISR_Ep4ff::
_ISR_Ep6ff::
_ISR_Ep8ff::
_ISR_Gpifdone::
_ISR_Gpifwf::

;_ISR_Sudav::
_ISR_Sof::
;_ISR_Sutok::
_ISR_Suspend::
;_ISR_Usb_Reset::
;_ISR_Highspeed::
_ISR_Ep0ack::
_ISR_Ep0in::
_ISR_Ep0out::
_ISR_Ep1in::
;_ISR_Ep1out::
_ISR_Ep2inout::
_ISR_Ep4inout::
_ISR_Ep6inout::
_ISR_Ep8inout::
_ISR_Ibn::
_ISR_Ep0ping::
_ISR_Ep1ping::
_ISR_Ep2ping::
_ISR_Ep4ping::
_ISR_Ep6ping::
_ISR_Ep8ping::
_ISR_Errlimit::
_ISR_Ep2isoerr::
_ISR_Ep4isoerr::
_ISR_Ep6isoerr::
_ISR_Ep8isoerr::
__isr_nop::
	reti
__end_of_vector_table::
	nop
