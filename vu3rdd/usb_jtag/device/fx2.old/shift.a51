;;-----------------------------------------------------------------------------
;; Fast JTAG serial bit output for usbjtag
;;-----------------------------------------------------------------------------
;; Copyright (C) 2005,2006 Kolja Waschk, ixo.de
;;-----------------------------------------------------------------------------
;; This code is part of usbjtag. usbjtag is free software; you can redistribute
;; it and/or modify it under the terms of the GNU General Public License as
;; published by the Free Software Foundation; either version 2 of the License,
;; or (at your option) any later version. usbjtag is distributed in the hope
;; that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
;; warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
;; GNU General Public License for more details.  You should have received a
;; copy of the GNU General Public License along with this program in the file
;; COPYING; if not, write to the Free Software Foundation, Inc., 51 Franklin
;; St, Fifth Floor, Boston, MA  02110-1301  USA
;;-----------------------------------------------------------------------------

public _ShiftOut, _ShiftInOut

DSCR SEGMENT CODE PAGE

        rseg DSCR

TDI BIT 0A0H+0H
TDO BIT 0A0H+1H
TCK BIT 0A0H+2H
TMS BIT 0A0H+3H

;; 4 Instructions per bit: 48/4/4 = 3 MHz JTAG clock.
;; Code is written so that TCK has about 50% duty cycle.

_ShiftOut:
        MOV  A,R7
        ;; Bit0
        RRC  A
        MOV  TDI,C
        SETB TCK
        ;; Bit1
        RRC  A
        CLR  TCK
        MOV  TDI,C
        SETB TCK
        ;; Bit2
        RRC  A
        CLR  TCK
        MOV  TDI,C
        SETB TCK
        ;; Bit3
        RRC  A
        CLR  TCK
        MOV  TDI,C
        SETB TCK
        ;; Bit4
        RRC  A
        CLR  TCK
        MOV  TDI,C
        SETB TCK
        ;; Bit5
        RRC  A
        CLR  TCK
        MOV  TDI,C
        SETB TCK
        ;; Bit6
        RRC  A
        CLR  TCK
        MOV  TDI,C
        SETB TCK
        ;; Bit7
        RRC  A
        CLR  TCK
        MOV  TDI,C
        SETB TCK
        NOP 
        CLR  TCK

        ret

;; For ShiftInOut, the timing is a little more
;; critical because we have to read TDO/shift/set TDI
;; when TCK is low. But 20% duty cycle at 48/4/5 MHz
;; is just like 50% at 6 Mhz, and that's still acceptable

_ShiftInOut:
        MOV  A,R7

        ;; Bit0
        MOV  C,TDO
        RRC  A
        MOV  TDI,C
        SETB TCK
        CLR  TCK
        ;; Bit1
        MOV  C,TDO
        RRC  A
        MOV  TDI,C
        SETB TCK
        CLR  TCK
        ;; Bit2
        MOV  C,TDO
        RRC  A
        MOV  TDI,C
        SETB TCK
        CLR  TCK
        ;; Bit3
        MOV  C,TDO
        RRC  A
        MOV  TDI,C
        SETB TCK
        CLR  TCK
        ;; Bit4
        MOV  C,TDO
        RRC  A
        MOV  TDI,C
        SETB TCK
        CLR  TCK
        ;; Bit5
        MOV  C,TDO
        RRC  A
        MOV  TDI,C
        SETB TCK
        CLR  TCK
        ;; Bit6
        MOV  C,TDO
        RRC  A
        MOV  TDI,C
        SETB TCK
        CLR  TCK
        ;; Bit7
        MOV  C,TDO
        RRC  A
        MOV  TDI,C
        SETB TCK
        CLR  TCK

        MOV  R7,A
        ret
END
