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

; Descriptor table for HPSDR_FPGA_USB_VER1

.module dscr
   
DSCR_DEVICE   		=   1   ; Descriptor type: Device
DSCR_CONFIG   		=   2   ; Descriptor type: Configuration
DSCR_STRING   		=   3   ; Descriptor type: String
DSCR_INTRFC   		=   4   ; Descriptor type: Interface
DSCR_ENDPNT   		=   5   ; Descriptor type: Endpoint
DSCR_DEVQUAL  		=   6   ; Descriptor type: Device Qualifier

DSCR_DEVICE_LEN		=   18
DSCR_CONFIG_LEN 	=    9
DSCR_INTRFC_LEN 	=    9
DSCR_ENDPNT_LEN 	=    7
DSCR_DEVQUAL_LEN	=   10

ET_CONTROL   		=   0   ; Endpoint type: Control
ET_ISO       		=   1   ; Endpoint type: Isochronous
ET_BULK      		=   2   ; Endpoint type: Bulk
ET_INT       		=   3   ; Endpoint type: Interrupt

VEND_ID	     		=   0x04b4
PROD_ID      		=   0x1005

; Note we need to tell the linker where to start the descriptor
; table.  Use asx8051 -plosgff dscr.asm
; Then compile like this: 
; sdcc --code-loc 0x0200 --code-size 0x2200 --xram-loc 0x2200 --xram-size 0x1800 -Wl -bDESCR_SEG=0xE000
; -Wl -bVECT=0x0000 HPSDR_firmware_1_0_0.rel dscr.rel vectors.rel
; to start code at 0x0200, VECT at 0x0000 in code, and DESCR_SEG at 0xE000 in code

.area DESCR_SEG	(XDATA)

; Tell linker 2-byte align the table, otherwise SUDPTRL:H won't work.
.even
_DeviceDscr::   
	.db   DSCR_DEVICE_LEN	; Descriptor length
	.db   DSCR_DEVICE   	; Decriptor type
	.db   0x02		; Specification Version LSB
	.db   0x00		; Specification Version MSB
        .db   0x00        	; Device class
	.db   0x00         	; Device sub-class
	.db   0x00         	; Device sub-sub-class
	.db   0x40		; Maximum packet size
	.db   <VEND_ID  	; Vendor ID LSB
	.db   >VEND_ID  	; Vendor ID MSB
	.db   <PROD_ID		; Product ID LSB
	.db   >PROD_ID		; Product ID MSB
	.db   0x01		; Product version LSB
	.db   0x00		; Product version MSB
	.db   0x01      	; Manufacturer string index
	.db   0x02         	; Product string index
	.db   0x00         	; Serial number string index
	.db   0x01         	; Number of configurations

.even
_DeviceQualDscr::
	.db   DSCR_DEVQUAL_LEN  ; Descriptor length
	.db   DSCR_DEVQUAL   	; Decriptor type
	.db   0x02		; Specification Version LSB
	.db   0x00		; Specification Version MSB
	.db   0x00        	; Device class
	.db   0x00         	; Device sub-class
	.db   0x00         	; Device sub-sub-class
	.db   0x40   		; Maximum packet size
	.db   0x01      	; Number of configurations
	.db   0x00      	; Reserved

.even
_HighSpeedConfigDscr::
_FullSpeedConfigDscr::  
	.db   DSCR_CONFIG_LEN   ; Descriptor length
	.db   DSCR_CONFIG       ; Descriptor type
	.db   <(_HighSpeedConfigDscrEnd-_HighSpeedConfigDscr) ;; Total Length (LSB)
	.db   >(_HighSpeedConfigDscrEnd-_HighSpeedConfigDscr) ;; Total Length (MSB)
	.db   0x01      	; Number of interfaces
	.db   0x01      	; Configuration number
	.db   0x00      	; Configuration string
	.db   0xA0   		; Attributes (b7 - buspwr, b6 - selfpwr, b5 - rwu)
	.db   50      		; Power requirement (div 2 ma)

; Interface Descriptor
	.db   DSCR_INTRFC_LEN   ; Descriptor length
	.db   DSCR_INTRFC       ; Descriptor type
	.db   0x00              ; Zero-based index of this interface
	.db   0x00              ; Alternate setting
	.db   0x02              ; Number of end points 
	.db   0xff            	; Interface class
	.db   0x00              ; Interface sub class
	.db   0x00              ; Interface sub sub class
	.db   0x00              ; Interface descriptor string index
      
; Endpoint Descriptor
	.db   DSCR_ENDPNT_LEN   ; Descriptor length
	.db   DSCR_ENDPNT       ; Descriptor type
	.db   0x01              ; Endpoint number, and direction
	.db   ET_BULK           ; Endpoint type
	.db   0x40              ; Maximun packet size LSB
	.db   0x00              ; Max packect size MSB
	.db   0x00              ; Polling interval

; Endpoint Descriptor
	.db   DSCR_ENDPNT_LEN   ; Descriptor length
	.db   DSCR_ENDPNT       ; Descriptor type
	.db   0x81              ; Endpoint number, and direction
	.db   ET_BULK           ; Endpoint type
	.db   0x40              ; Maximun packet size LSB
	.db   0x00              ; Max packect size MSB
	.db   0x00              ; Polling interval

_HighSpeedConfigDscrEnd:   

.even
_StringDscr::
_StringDscr0::   
	.db   _StringDscr0End-_StringDscr0      ; String descriptor length
	.db   DSCR_STRING
	.db   0x02
	.db   0x00
_StringDscr0End:

.even
_StringDscr1::   
	.db   _StringDscr1End-_StringDscr1      ; String descriptor length
	.db   DSCR_STRING
	.db   'H
	.db   0x00
	.db   'P
	.db   0x00
	.db   'S
	.db   0x00
	.db   'D
	.db   0x00
	.db   'R
	.db   0x00
_StringDscr1End:

.even
_StringDscr2::   
	.db   _StringDscr2End-_StringDscr2      ; Descriptor length
	.db   DSCR_STRING
	.db   'F
	.db   0x00
	.db   'P
	.db   0x00
	.db   'G
	.db   0x00
	.db   'A
	.db   0x00
	.db   '_
	.db   0x00
	.db   'U
	.db   0x00
	.db   'S
	.db   0x00
	.db   'B
	.db   0x00
	.db   '_
	.db   0x00
	.db   'V
	.db   0x00
	.db   'E
	.db   0x00
	.db   'R
	.db   0x00
	.db   '1
	.db   0x00
_StringDscr2End:

.even
_UserDscr::      
	.db   0x00
	.db   0x00
