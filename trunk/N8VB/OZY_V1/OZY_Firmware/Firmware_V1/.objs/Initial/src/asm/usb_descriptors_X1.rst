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
                             27 ;;; USB Descriptor table for the HPSDR
                             28 ;;;
                             29 ;;; We're a high-speed only device (480 Mb/sec) with 1 configuration
                             30 ;;; and 3 interfaces.
                             31 ;;;
                             32 ;;;	interface 0:	command and status (ep0 COMMAND)
                             33 ;;;	interface 1:	Transmit path (ep2 OUT BULK)
                             34 ;;;	interface 2:	Receive path (ep6 IN BULK)
                             35 ;;;
                             36 ;;;
                             37 ;;; Use asx8051 -plosgff x.asm
                             38 ;;;
                             39 	.module usb_descriptors
                             40 
                    FFFE     41 	VID_FREE	 = 0xfffe	; Free Software Folks
                    0007     42 	PID_HPSDR	 = 0x0007	; HPSDR
                             43 
                             44 	;; We distinguish configured from unconfigured HPSDRs using the Device ID.
                             45 	;; If the MSB of the DID is 0, the device is unconfigured.
                             46 	;; The LSB of the DID is reserved for hardware revs.
                             47 
                    0100     48 	DID_HPSDR	 = 0x0100	; Device ID (bcd)
                             49 
                             50 
                    0001     51 	DSCR_DEVICE	 =   1	; Descriptor type: Device
                    0002     52 	DSCR_CONFIG	 =   2	; Descriptor type: Configuration
                    0003     53 	DSCR_STRING	 =   3	; Descriptor type: String
                    0004     54 	DSCR_INTRFC	 =   4	; Descriptor type: Interface
                    0005     55 	DSCR_ENDPNT	 =   5	; Descriptor type: Endpoint
                    0006     56 	DSCR_DEVQUAL	 =   6	; Descriptor type: Device Qualifier
                             57 
                    0012     58 	DSCR_DEVICE_LEN	 =  18
                    0009     59 	DSCR_CONFIG_LEN  =   9
                    0009     60 	DSCR_INTRFC_LEN  =   9
                    0007     61 	DSCR_ENDPNT_LEN  =   7
                    000A     62 	DSCR_DEVQUAL_LEN =  10
                             63 
                    0000     64 	ET_CONTROL	 =   0	; Endpoint type: Control
                    0001     65 	ET_ISO		 =   1	; Endpoint type: Isochronous
                    0002     66 	ET_BULK		 =   2	; Endpoint type: Bulk
                    0003     67 	ET_INT		 =   3	; Endpoint type: Interrupt
                             68 
                             69 
                             70 	;; configuration attributes
                    0040     71 	bmSELF_POWERED	=	1 << 6
                    0080     72 	bmBUS_POWERED = 1 << 7
                    0032     73 	bmMAX_POWER_100 = 50
                             74 
                             75 ;;; --------------------------------------------------------
                             76 ;;;	external ram data
                             77 ;;;--------------------------------------------------------
                             78 
                             79 	.area USBDESCSEG    (XDATA)
                             80 
                             81 ;;; ----------------------------------------------------------------
                             82 ;;; descriptors used when operating at high speed (480Mb/sec)
                             83 ;;; ----------------------------------------------------------------
                             84 
   E000                      85 	.even	; descriptors must be 2-byte aligned for SUDPTR{H,L} to work
                             86 
                             87 	;; The .even directive isn't really honored by the linker.  Bummer!
                             88 	;; (There's no way to specify an alignment requirement for a given area,
                             89 	;; hence when they're concatenated together, even doesn't work.)
                             90 	;;
                             91 	;; We work around this by telling the linker to put USBDESCSEG
                             92 	;; at 0xE000 absolute.  This means that the maximimum length of this
                             93 	;; segment is 480 bytes, leaving room for the two hash slots
                             94 	;; at 0xE1EO to 0xE1FF.
                             95 	;;
                             96 
   E000                      97 _high_speed_device_descr::
   E000 12                   98 	.db	DSCR_DEVICE_LEN
   E001 01                   99 	.db	DSCR_DEVICE
   E002 00                  100 	.db	<0x0200		; Specification version (LSB)
   E003 02                  101 	.db	>0x0200		; Specification version (MSB)
   E004 FF                  102 	.db	0xff		; device class (vendor specific)
   E005 FF                  103 	.db	0xff		; device subclass (vendor specific)
   E006 FF                  104 	.db	0xff		; device protocol (vendor specific)
   E007 40                  105 	.db	64		; bMaxPacketSize0 for endpoint 0
   E008 FE                  106 	.db	<VID_FREE	; idVendor
   E009 FF                  107 	.db	>VID_FREE	; idVendor
   E00A 07                  108 	.db	<PID_HPSDR	; idProduct
   E00B 00                  109 	.db	>PID_HPSDR	; idProduct
   E00C                     110 _usb_desc_hw_rev_binary_patch_location_0::
   E00C 00                  111 	.db	<DID_HPSDR	; bcdDevice
   E00D 01                  112 	.db	>DID_HPSDR	; bcdDevice
   E00E 01                  113 	.db	SI_VENDOR	; iManufacturer (string index)
   E00F 02                  114 	.db	SI_PRODUCT	; iProduct (string index)
   E010 04                  115 	.db	SI_SERIAL	; iSerial number (string index)
   E011 01                  116 	.db	1		; bNumConfigurations
                            117 
                            118 ;;; describes the other speed (12Mb/sec)
   E012                     119 	.even
   E012                     120 _high_speed_devqual_descr::
   E012 0A                  121 	.db	DSCR_DEVQUAL_LEN
   E013 06                  122 	.db	DSCR_DEVQUAL
   E014 00                  123 	.db	<0x0200		; bcdUSB (LSB)
   E015 02                  124 	.db	>0x0200		; bcdUSB (MSB)
   E016 FF                  125 	.db	0xff		; bDeviceClass
   E017 FF                  126 	.db	0xff		; bDeviceSubClass
   E018 FF                  127 	.db	0xff		; bDeviceProtocol
   E019 40                  128 	.db	64		; bMaxPacketSize0
   E01A 01                  129 	.db	1		; bNumConfigurations (one config at 12Mb/sec)
   E01B 00                  130 	.db	0		; bReserved
                            131 
   E01C                     132 	.even
   E01C                     133 _high_speed_config_descr::
   E01C 09                  134 	.db	DSCR_CONFIG_LEN
   E01D 02                  135 	.db	DSCR_CONFIG
   E01E 20                  136 	.db	<(_high_speed_config_descr_end - _high_speed_config_descr) ; LSB
   E01F 00                  137 	.db	>(_high_speed_config_descr_end - _high_speed_config_descr) ; MSB
   E020 01                  138 	.db	1		; bNumInterfaces
   E021 01                  139 	.db	1		; bConfigurationValue
   E022 00                  140 	.db	0		; iConfiguration
   E023 80                  141 	.db	0x80 | bmBUS_POWERED ; bmAttributes
   E024 32                  142 	.db	50	; bMaxPower 100mA
                            143 
                            144 	;; interface descriptor 0
                            145 
   E025 09                  146 	.db	DSCR_INTRFC_LEN
   E026 04                  147 	.db	DSCR_INTRFC
   E027 00                  148 	.db	0		; bInterfaceNumber (zero based)
   E028 00                  149 	.db	0		; bAlternateSetting
   E029 02                  150 	.db	4		; bNumEndpoints
   E02A FF                  151 	.db	0xff		; bInterfaceClass (vendor specific)
   E02B FF                  152 	.db	0xff		; bInterfaceSubClass (vendor specific)
   E02C FF                  153 	.db	0xff		; bInterfaceProtocol (vendor specific)
   E02D 03                  154 	.db	SI_COMMAND_AND_FIFO	; iInterface (description)
                            155 
                            156 	;; end point 2
                            157 
   E02E 07                  158 	.db	DSCR_ENDPNT_LEN
   E02F 05                  159 	.db	DSCR_ENDPNT
   E030 02                  160 	.db	0x02		; bEndpointAddress (ep 2 OUT)
   E031 02                  161 	.db	ET_BULK		; bmAttributes
   E032 00                  162 	.db	<512		; wMaxPacketSize (LSB)
   E033 02                  163 	.db	>512		; wMaxPacketSize (MSB)
   E034 00                  164 	.db	0		; bInterval (iso only)
                            165 
                            166 	;; end point 4
                            167 
   E035 07                  168 	.db	DSCR_ENDPNT_LEN
   E036 05                  169 	.db	DSCR_ENDPNT
   E037 86                  170 	.db	0x04		; bEndpointAddress (ep 4 OUT)
   E038 02                  171 	.db	ET_BULK		; bmAttributes
   E039 00                  172 	.db	<512		; wMaxPacketSize (LSB)
   E03A 02                  173 	.db	>512		; wMaxPacketSize (MSB)
   E03B 00                  174 	.db	0		; bInterval (iso only)
                            175 
                            176 	;; end point 6
                            177 
   E03C 07                  178 	.db	DSCR_ENDPNT_LEN
   E03C 05                  179 	.db	DSCR_ENDPNT
   E03C 86                  180 	.db	0x86		; bEndpointAddress (ep 2 IN)
   E03C 12                  181 	.db	ET_BULK		; bmAttributes
   E03D 01                  182 	.db	<512		; wMaxPacketSize (LSB)
   E03E 00                  183 	.db	>512		; wMaxPacketSize (MSB)
   E03F 02                  184 	.db	0		; bInterval (iso only)
                            185 
                            186 	;; end point 8
                            187 
   E040 FF                  188 	.db	DSCR_ENDPNT_LEN
   E041 FF                  189 	.db	DSCR_ENDPNT
   E042 FF                  190 	.db	0x88		; bEndpointAddress (ep 4 IN)
   E043 40                  191 	.db	ET_BULK		; bmAttributes
   E044 FE                  192 	.db	<512		; wMaxPacketSize (LSB)
   E045 FF                  193 	.db	>512		; wMaxPacketSize (MSB)
   E046 07                  194 	.db	0		; bInterval (iso only)
                            195 
   004A                     196 _high_speed_config_descr_end:
                            197 
                            198 ;;; ----------------------------------------------------------------
                            199 ;;; descriptors used when operating at full speed (12Mb/sec)
                            200 ;;; ----------------------------------------------------------------
                            201 
   004A                     202 	.even
   004A                     203 _full_speed_device_descr::
   E047 00                  204 	.db	DSCR_DEVICE_LEN
   E048 01                  205 	.db	DSCR_DEVICE
   E048 00                  206 	.db	<0x0200		; Specification version (LSB)
   E049 01                  207 	.db	>0x0200		; Specification version (MSB)
   E04A 01                  208 	.db	0xff		; device class (vendor specific)
   E04B 02                  209 	.db	0xff		; device subclass (vendor specific)
   E04C 00                  210 	.db	0xff		; device protocol (vendor specific)
   E04D 01                  211 	.db	64		; bMaxPacketSize0 for endpoint 0
   E04E FE                  212 	.db	<VID_FREE	; idVendor
   E04E FF                  213 	.db	>VID_FREE	; idVendor
   E04E 0A                  214 	.db	<PID_HPSDR	; idProduct
   E04F 06                  215 	.db	>PID_HPSDR	; idProduct
   0056                     216 _usb_desc_hw_rev_binary_patch_location_1::
   E050 00                  217 	.db	<DID_HPSDR	; bcdDevice
   E051 02                  218 	.db	>DID_HPSDR	; bcdDevice
   E052 FF                  219 	.db	SI_VENDOR	; iManufacturer (string index)
   E053 FF                  220 	.db	SI_PRODUCT	; iProduct (string index)
   E054 FF                  221 	.db	SI_NONE		; iSerial number (None)
   E055 40                  222 	.db	1		; bNumConfigurations
                            223 
                            224 
                            225 ;;; describes the other speed (480Mb/sec)
   005C                     226 	.even
   005C                     227 _full_speed_devqual_descr::
   E056 01                  228 	.db	DSCR_DEVQUAL_LEN
   E057 00                  229 	.db	DSCR_DEVQUAL
   E058 00                  230 	.db	<0x0200		; bcdUSB
   E058 02                  231 	.db	>0x0200		; bcdUSB
   E058 09                  232 	.db	0xff		; bDeviceClass
   E059 02                  233 	.db	0xff		; bDeviceSubClass
   E05A 12                  234 	.db	0xff		; bDeviceProtocol
   E05B 00                  235 	.db	64		; bMaxPacketSize0
   E05C 01                  236 	.db	1		; bNumConfigurations (one config at 480Mb/sec)
   E05D 01                  237 	.db	0		; bReserved
                            238 
   0066                     239 	.even
   0066                     240 _full_speed_config_descr::
   E05E 00                  241 	.db	DSCR_CONFIG_LEN
   E05F 80                  242 	.db	DSCR_CONFIG
   E060 32                  243 	.db	<(_full_speed_config_descr_end - _full_speed_config_descr) ; LSB
   E061 09                  244 	.db	>(_full_speed_config_descr_end - _full_speed_config_descr) ; MSB
   E062 04                  245 	.db	1		; bNumInterfaces
   E063 00                  246 	.db	1		; bConfigurationValue
   E064 00                  247 	.db	0		; iConfiguration
   E065 00                  248 	.db	0x80 | bmBUS_POWERED ; bmAttributes
   E066 FF                  249 	.db	50	; bMaxPower
                            250 
                            251 	;; interface descriptor 0 (command & status, ep0 COMMAND)
                            252 
   E067 FF                  253 	.db	DSCR_INTRFC_LEN
   E068 FF                  254 	.db	DSCR_INTRFC
   E069 03                  255 	.db	0		; bInterfaceNumber (zero based)
   E06A 00                  256 	.db	0		; bAlternateSetting
   E06A 00                  257 	.db	0		; bNumEndpoints
   E06A 05                  258 	.db	0xff		; bInterfaceClass (vendor specific)
   E06B FF                  259 	.db	0xff		; bInterfaceSubClass (vendor specific)
   E06B 76                  260 	.db	0xff		; bInterfaceProtocol (vendor specific)
   E06C E0                  261 	.db	SI_COMMAND_AND_FIFO	; iInterface (description)
                            262 
   0078                     263 _full_speed_config_descr_end:
                            264 
                            265 ;;; ----------------------------------------------------------------
                            266 ;;;			string descriptors
                            267 ;;; ----------------------------------------------------------------
                            268 
   0078                     269 _nstring_descriptors::
   E06D 7C                  270 	.db	(_string_descriptors_end - _string_descriptors) / 2
                            271 
   0079                     272 _string_descriptors::
   E06E E0 A4               273 	.db	<str0, >str0
   E070 E0 C4               274 	.db	<str1, >str1
   E072 E0 E6               275 	.db	<str2, >str2
   E074 E0s00               276 	.db	<str3, >str3
   E075rF4s00               277 	.db	<str4, >str4
   E076                     278 _string_descriptors_end:
                            279 
                    0000    280 	SI_NONE = 0
                            281 	;; str0 contains the language ID's.
   0084                     282 	.even
   E076 06                  283 str0:	.db	str0_end - str0
   E077 03                  284 	.db	DSCR_STRING
   E078 00                  285 	.db	0
   E079 00                  286 	.db	0
   E07A 09                  287 	.db	<0x0409		; magic code for US English (LSB)
   E07B 04                  288 	.db	>0x0409		; magic code for US English (MSB)
   E07C                     289 str0_end:
                            290 
                    0001    291 	SI_VENDOR = 1
   E07C                     292 	.even
   E07C 28                  293 str1:	.db	str1_end - str1
   E07D 03                  294 	.db	DSCR_STRING
   E07E 46 00               295 	.db	'F, 0		; 16-bit unicode
   E080 72 00               296 	.db	'r, 0
   E082 65 00               297 	.db	'e, 0
   E084 65 00               298 	.db	'e, 0
   E086 20 00               299 	.db	' , 0
   E088 53 00               300 	.db	'S, 0
   E08A 6F 00               301 	.db	'o, 0
   E08C 66 00               302 	.db	'f, 0
   E08E 74 00               303 	.db	't, 0
   E090 77 00               304 	.db	'w, 0
   E092 61 00               305 	.db	'a, 0
   E094 72 00               306 	.db	'r, 0
   E096 65 00               307 	.db	'e, 0
   E098 20 00               308 	.db	' , 0
   E09A 46 00               309 	.db	'F, 0
   E09C 6F 00               310 	.db	'o, 0
   E09E 6C 00               311 	.db	'l, 0
   E0A0 6B 00               312 	.db	'k, 0
   E0A2 73 00               313 	.db	's, 0
   E0A4                     314 str1_end:
                            315 
                    0002    316 	SI_PRODUCT = 2
   E0A4                     317 	.even
   E0A4 20                  318 str2:	.db	str2_end - str2
   E0A5 03                  319 	.db	DSCR_STRING
   E0A6 48 00               320 	.db	'H, 0
   E0A8 50 00               321 	.db	'P, 0
   E0AA 53 00               322 	.db	'S, 0
   E0AC 44 00               323 	.db	'D, 0
   E0AE 52 00               324 	.db	'R, 0
   E0B0 20 00               325 	.db    ' , 0
   E0B2 4F 00               326 	.db    'O, 0
   E0B4 5A 00               327 	.db    'Z, 0
   E0B6 59 00               328 	.db    'Y, 0
   E0B8 20 00               329 	.db	' , 0
   E0BA 52 00               330 	.db	'R, 0
   E0BC 65 00               331 	.db	'e, 0
   E0BE 76 00               332 	.db	'v, 0
   E0C0 20 00               333 	.db	' , 0
   E0C2                     334 _usb_desc_hw_rev_ascii_patch_location_0::
   E0C2 3F 00               335 	.db	'?, 0
   E0C4                     336 str2_end:
                            337 
                    0003    338 	SI_COMMAND_AND_FIFO = 3
   E0C4                     339 	.even
   E0C4 22                  340 str3:	.db	str3_end - str3
   E0C5 03                  341 	.db	DSCR_STRING
   E0C6 43 00               342 	.db	'C, 0
   E0C8 6F 00               343 	.db	'o, 0
   E0CA 6D 00               344 	.db	'm, 0
   E0CC 6D 00               345 	.db	'm, 0
   E0CE 61 00               346 	.db	'a, 0
   E0D0 6E 00               347 	.db	'n, 0
   E0D2 64 00               348 	.db	'd, 0
   E0D4 20 00               349 	.db	' , 0
   E0D6 26 00               350 	.db	'&, 0
   E0D8 20 00               351 	.db	' , 0
   E0DA 53 00               352 	.db	'S, 0
   E0DC 74 00               353 	.db	't, 0
   E0DE 61 00               354 	.db	'a, 0
   E0E0 74 00               355 	.db	't, 0
   E0E2 75 00               356 	.db	'u, 0
   E0E4 73 00               357 	.db	's, 0
   E0E6                     358 str3_end:
                            359 
                    0004    360 	SI_SERIAL = 4
   E0E6                     361 	.even
   E0E6 12                  362 str4:	.db	str4_end - str4
   E0E7 03                  363 	.db	DSCR_STRING
   E0E8                     364 _usb_desc_serial_number_ascii::
   E0E8 31 00               365 	.db	'1, 0
   E0EA 2E 00               366 	.db	'., 0
   E0EC 34 00               367 	.db	'4, 0
   E0EE 31 00               368 	.db	'1, 0
   E0F0 34 00               369 	.db	'4, 0
   E0F2 32 00               370 	.db	'2, 0
   E0F4 31 00               371 	.db	'1, 0
   E0F6 33 00               372 	.db	'3, 0
   E0F8                     373 str4_end:
                            374 
