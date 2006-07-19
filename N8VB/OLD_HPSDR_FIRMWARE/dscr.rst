                              1 ;/*
                              2 ; * Copyright (c) 2005 Philip A Covington, N8VB
                              3 ; *
                              4 ; * This program is free software; you can redistribute it and/or modify
                              5 ; * it under the terms of the GNU General Public License as published by
                              6 ; * the Free Software Foundation; either version 2 of the License, or
                              7 ; * (at your option) any later version.
                              8 ; *
                              9 ; * This program is distributed in the hope that it will be useful,
                             10 ; * but WITHOUT ANY WARRANTY; without even the implied warranty of
                             11 ; * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
                             12 ; * GNU General Public License for more details.
                             13 ; *
                             14 ; * You should have received a copy of the GNU General Public License
                             15 ; * along with this program; if not, write to the Free Software
                             16 ; * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
                             17 ; *
                             18 ; */
                             19 
                             20 ; Descriptor table for HPSDR_FPGA_USB_VER1
                             21 
                             22 .module dscr
                             23    
                    0001     24 DSCR_DEVICE   		=   1   ; Descriptor type: Device
                    0002     25 DSCR_CONFIG   		=   2   ; Descriptor type: Configuration
                    0003     26 DSCR_STRING   		=   3   ; Descriptor type: String
                    0004     27 DSCR_INTRFC   		=   4   ; Descriptor type: Interface
                    0005     28 DSCR_ENDPNT   		=   5   ; Descriptor type: Endpoint
                    0006     29 DSCR_DEVQUAL  		=   6   ; Descriptor type: Device Qualifier
                             30 
                    0012     31 DSCR_DEVICE_LEN		=   18
                    0009     32 DSCR_CONFIG_LEN 	=    9
                    0009     33 DSCR_INTRFC_LEN 	=    9
                    0007     34 DSCR_ENDPNT_LEN 	=    7
                    000A     35 DSCR_DEVQUAL_LEN	=   10
                             36 
                    0000     37 ET_CONTROL   		=   0   ; Endpoint type: Control
                    0001     38 ET_ISO       		=   1   ; Endpoint type: Isochronous
                    0002     39 ET_BULK      		=   2   ; Endpoint type: Bulk
                    0003     40 ET_INT       		=   3   ; Endpoint type: Interrupt
                             41 
                    04B4     42 VEND_ID	     		=   0x04b4
                    1005     43 PROD_ID      		=   0x1005
                             44 
                             45 ; Note we need to tell the linker where to start the descriptor
                             46 ; table.  Use asx8051 -plosgff dscr.asm
                             47 ; Then compile like this: 
                             48 ; sdcc --code-loc 0x0200 --code-size 0x2200 --xram-loc 0x2200 --xram-size 0x1800 -Wl -bDESCR_SEG=0xE000
                             49 ; -Wl -bVECT=0x0000 HPSDR_firmware_1_0_0.rel dscr.rel vectors.rel
                             50 ; to start code at 0x0200, VECT at 0x0000 in code, and DESCR_SEG at 0xE000 in code
                             51 
                             52 .area DESCR_SEG	(XDATA)
                             53 
                             54 ; Tell linker 2-byte align the table, otherwise SUDPTRL:H won't work.
   E000                      55 .even
   E000                      56 _DeviceDscr::   
   E000 12                   57 	.db   DSCR_DEVICE_LEN	; Descriptor length
   E001 01                   58 	.db   DSCR_DEVICE   	; Decriptor type
   E002 02                   59 	.db   0x02		; Specification Version LSB
   E003 00                   60 	.db   0x00		; Specification Version MSB
   E004 00                   61         .db   0x00        	; Device class
   E005 00                   62 	.db   0x00         	; Device sub-class
   E006 00                   63 	.db   0x00         	; Device sub-sub-class
   E007 40                   64 	.db   0x40		; Maximum packet size
   E008 B4                   65 	.db   <VEND_ID  	; Vendor ID LSB
   E009 04                   66 	.db   >VEND_ID  	; Vendor ID MSB
   E00A 05                   67 	.db   <PROD_ID		; Product ID LSB
   E00B 10                   68 	.db   >PROD_ID		; Product ID MSB
   E00C 01                   69 	.db   0x01		; Product version LSB
   E00D 00                   70 	.db   0x00		; Product version MSB
   E00E 01                   71 	.db   0x01      	; Manufacturer string index
   E00F 02                   72 	.db   0x02         	; Product string index
   E010 00                   73 	.db   0x00         	; Serial number string index
   E011 01                   74 	.db   0x01         	; Number of configurations
                             75 
   E012                      76 .even
   E012                      77 _DeviceQualDscr::
   E012 0A                   78 	.db   DSCR_DEVQUAL_LEN  ; Descriptor length
   E013 06                   79 	.db   DSCR_DEVQUAL   	; Decriptor type
   E014 02                   80 	.db   0x02		; Specification Version LSB
   E015 00                   81 	.db   0x00		; Specification Version MSB
   E016 00                   82 	.db   0x00        	; Device class
   E017 00                   83 	.db   0x00         	; Device sub-class
   E018 00                   84 	.db   0x00         	; Device sub-sub-class
   E019 40                   85 	.db   0x40   		; Maximum packet size
   E01A 01                   86 	.db   0x01      	; Number of configurations
   E01B 00                   87 	.db   0x00      	; Reserved
                             88 
   E01C                      89 .even
   E01C                      90 _HighSpeedConfigDscr::
   E01C                      91 _FullSpeedConfigDscr::  
   E01C 09                   92 	.db   DSCR_CONFIG_LEN   ; Descriptor length
   E01D 02                   93 	.db   DSCR_CONFIG       ; Descriptor type
   E01E 20                   94 	.db   <(_HighSpeedConfigDscrEnd-_HighSpeedConfigDscr) ;; Total Length (LSB)
   E01F 00                   95 	.db   >(_HighSpeedConfigDscrEnd-_HighSpeedConfigDscr) ;; Total Length (MSB)
   E020 01                   96 	.db   0x01      	; Number of interfaces
   E021 01                   97 	.db   0x01      	; Configuration number
   E022 00                   98 	.db   0x00      	; Configuration string
   E023 A0                   99 	.db   0xA0   		; Attributes (b7 - buspwr, b6 - selfpwr, b5 - rwu)
   E024 32                  100 	.db   50      		; Power requirement (div 2 ma)
                            101 
                            102 ; Interface Descriptor
   E025 09                  103 	.db   DSCR_INTRFC_LEN   ; Descriptor length
   E026 04                  104 	.db   DSCR_INTRFC       ; Descriptor type
   E027 00                  105 	.db   0x00              ; Zero-based index of this interface
   E028 00                  106 	.db   0x00              ; Alternate setting
   E029 02                  107 	.db   0x02              ; Number of end points 
   E02A FF                  108 	.db   0xff            	; Interface class
   E02B 00                  109 	.db   0x00              ; Interface sub class
   E02C 00                  110 	.db   0x00              ; Interface sub sub class
   E02D 00                  111 	.db   0x00              ; Interface descriptor string index
                            112       
                            113 ; Endpoint Descriptor
   E02E 07                  114 	.db   DSCR_ENDPNT_LEN   ; Descriptor length
   E02F 05                  115 	.db   DSCR_ENDPNT       ; Descriptor type
   E030 01                  116 	.db   0x01              ; Endpoint number, and direction
   E031 02                  117 	.db   ET_BULK           ; Endpoint type
   E032 40                  118 	.db   0x40              ; Maximun packet size LSB
   E033 00                  119 	.db   0x00              ; Max packect size MSB
   E034 00                  120 	.db   0x00              ; Polling interval
                            121 
                            122 ; Endpoint Descriptor
   E035 07                  123 	.db   DSCR_ENDPNT_LEN   ; Descriptor length
   E036 05                  124 	.db   DSCR_ENDPNT       ; Descriptor type
   E037 81                  125 	.db   0x81              ; Endpoint number, and direction
   E038 02                  126 	.db   ET_BULK           ; Endpoint type
   E039 40                  127 	.db   0x40              ; Maximun packet size LSB
   E03A 00                  128 	.db   0x00              ; Max packect size MSB
   E03B 00                  129 	.db   0x00              ; Polling interval
                            130 
   E03C                     131 _HighSpeedConfigDscrEnd:   
                            132 
   E03C                     133 .even
   E03C                     134 _StringDscr::
   E03C                     135 _StringDscr0::   
   E03C 04                  136 	.db   _StringDscr0End-_StringDscr0      ; String descriptor length
   E03D 03                  137 	.db   DSCR_STRING
   E03E 02                  138 	.db   0x02
   E03F 00                  139 	.db   0x00
   E040                     140 _StringDscr0End:
                            141 
   E040                     142 .even
   E040                     143 _StringDscr1::   
   E040 0C                  144 	.db   _StringDscr1End-_StringDscr1      ; String descriptor length
   E041 03                  145 	.db   DSCR_STRING
   E042 48                  146 	.db   'H
   E043 00                  147 	.db   0x00
   E044 50                  148 	.db   'P
   E045 00                  149 	.db   0x00
   E046 53                  150 	.db   'S
   E047 00                  151 	.db   0x00
   E048 44                  152 	.db   'D
   E049 00                  153 	.db   0x00
   E04A 52                  154 	.db   'R
   E04B 00                  155 	.db   0x00
   E04C                     156 _StringDscr1End:
                            157 
   E04C                     158 .even
   E04C                     159 _StringDscr2::   
   E04C 1C                  160 	.db   _StringDscr2End-_StringDscr2      ; Descriptor length
   E04D 03                  161 	.db   DSCR_STRING
   E04E 46                  162 	.db   'F
   E04F 00                  163 	.db   0x00
   E050 50                  164 	.db   'P
   E051 00                  165 	.db   0x00
   E052 47                  166 	.db   'G
   E053 00                  167 	.db   0x00
   E054 41                  168 	.db   'A
   E055 00                  169 	.db   0x00
   E056 5F                  170 	.db   '_
   E057 00                  171 	.db   0x00
   E058 55                  172 	.db   'U
   E059 00                  173 	.db   0x00
   E05A 53                  174 	.db   'S
   E05B 00                  175 	.db   0x00
   E05C 42                  176 	.db   'B
   E05D 00                  177 	.db   0x00
   E05E 5F                  178 	.db   '_
   E05F 00                  179 	.db   0x00
   E060 56                  180 	.db   'V
   E061 00                  181 	.db   0x00
   E062 45                  182 	.db   'E
   E063 00                  183 	.db   0x00
   E064 52                  184 	.db   'R
   E065 00                  185 	.db   0x00
   E066 31                  186 	.db   '1
   E067 00                  187 	.db   0x00
   E068                     188 _StringDscr2End:
                            189 
   E068                     190 .even
   E068                     191 _UserDscr::      
   E068 00                  192 	.db   0x00
   E069 00                  193 	.db   0x00
