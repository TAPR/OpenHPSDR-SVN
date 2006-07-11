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
                             35 
                             36 	.module usb_descriptors
                             37 	
                    FFFE     38 	VID_FREE	 = 0xfffe	; Free Software Folks
                    0007     39 	PID_USRP	 = 0x0007	; HPSDR
                             40 
                             41 	;; We distinguish configured from unconfigured USRPs using the Device ID.
                             42 	;; If the MSB of the DID is 0, the device is unconfigured.
                             43 	;; The LSB of the DID is reserved for hardware revs.
                             44 	
                    0100     45 	DID_USRP	 = 0x0100	; Device ID (bcd)
                             46 
                             47 	
                    0001     48 	DSCR_DEVICE	 =   1	; Descriptor type: Device
                    0002     49 	DSCR_CONFIG	 =   2	; Descriptor type: Configuration
                    0003     50 	DSCR_STRING	 =   3	; Descriptor type: String
                    0004     51 	DSCR_INTRFC	 =   4	; Descriptor type: Interface
                    0005     52 	DSCR_ENDPNT	 =   5	; Descriptor type: Endpoint
                    0006     53 	DSCR_DEVQUAL	 =   6	; Descriptor type: Device Qualifier
                             54 	
                    0012     55 	DSCR_DEVICE_LEN	 =  18
                    0009     56 	DSCR_CONFIG_LEN  =   9
                    0009     57 	DSCR_INTRFC_LEN  =   9
                    0007     58 	DSCR_ENDPNT_LEN  =   7
                    000A     59 	DSCR_DEVQUAL_LEN =  10
                             60 	
                    0000     61 	ET_CONTROL	 =   0	; Endpoint type: Control
                    0001     62 	ET_ISO		 =   1	; Endpoint type: Isochronous
                    0002     63 	ET_BULK		 =   2	; Endpoint type: Bulk
                    0003     64 	ET_INT		 =   3	; Endpoint type: Interrupt
                             65 	
                             66 	
                             67 	;; configuration attributes
                    0040     68 	bmSELF_POWERED	=	1 << 6
                             69 
                             70 ;;; --------------------------------------------------------
                             71 ;;;	external ram data
                             72 ;;;--------------------------------------------------------
                             73 	
                             74 	.area USBDESCSEG    (XDATA)
                             75 	
                             76 ;;; ----------------------------------------------------------------
                             77 ;;; descriptors used when operating at high speed (480Mb/sec)
                             78 ;;; ----------------------------------------------------------------
                             79 	
   200A                      80 	.even	; descriptors must be 2-byte aligned for SUDPTR{H,L} to work
                             81 
                             82 	;; The .even directive isn't really honored by the linker.  Bummer!
                             83 	;; (There's no way to specify an alignment requirement for a given area,
                             84 	;; hence when they're concatenated together, even doesn't work.)
                             85 	;; 
                             86 	;; We work around this by telling the linker to put USBDESCSEG
                             87 	;; at 0xE000 absolute.  This means that the maximimum length of this
                             88 	;; segment is 480 bytes, leaving room for the two hash slots 
                             89 	;; at 0xE1EO to 0xE1FF.  
                             90 	;; 
                             91 	;; As of July 7, 2004, this segment is 326 bytes long
                             92 	
   200A                      93 _high_speed_device_descr::
   200A 12                   94 	.db	DSCR_DEVICE_LEN
   200B 01                   95 	.db	DSCR_DEVICE
   200C 00                   96 	.db	<0x0200		; Specification version (LSB)
   200D 02                   97 	.db	>0x0200		; Specification version (MSB)
   200E FF                   98 	.db	0xff		; device class (vendor specific)
   200F FF                   99 	.db	0xff		; device subclass (vendor specific)
   2010 FF                  100 	.db	0xff		; device protocol (vendor specific)
   2011 40                  101 	.db	64		; bMaxPacketSize0 for endpoint 0
   2012 FE                  102 	.db	<VID_FREE	; idVendor
   2013 FF                  103 	.db	>VID_FREE	; idVendor
   2014 07                  104 	.db	<PID_USRP	; idProduct
   2015 00                  105 	.db	>PID_USRP	; idProduct
   2016                     106 _usb_desc_hw_rev_binary_patch_location_0::
   2016 00                  107 	.db	<DID_USRP	; bcdDevice
   2017 01                  108 	.db	>DID_USRP	; bcdDevice
   2018 01                  109 	.db	SI_VENDOR	; iManufacturer (string index)
   2019 02                  110 	.db	SI_PRODUCT	; iProduct (string index)
   201A 06                  111 	.db	SI_SERIAL	; iSerial number (string index)
   201B 01                  112 	.db	1		; bNumConfigurations
                            113 	
                            114 ;;; describes the other speed (12Mb/sec)
   201C                     115 	.even
   201C                     116 _high_speed_devqual_descr::
   201C 0A                  117 	.db	DSCR_DEVQUAL_LEN
   201D 06                  118 	.db	DSCR_DEVQUAL
   201E 00                  119 	.db	<0x0200		; bcdUSB (LSB)
   201F 02                  120 	.db	>0x0200		; bcdUSB (MSB)
   2020 FF                  121 	.db	0xff		; bDeviceClass
   2021 FF                  122 	.db	0xff		; bDeviceSubClass
   2022 FF                  123 	.db	0xff		; bDeviceProtocol
   2023 40                  124 	.db	64		; bMaxPacketSize0
   2024 01                  125 	.db	1		; bNumConfigurations (one config at 12Mb/sec)
   2025 00                  126 	.db	0		; bReserved
                            127 	
   2026                     128 	.even
   2026                     129 _high_speed_config_descr::	
   2026 09                  130 	.db	DSCR_CONFIG_LEN
   2027 02                  131 	.db	DSCR_CONFIG
   2028 32                  132 	.db	<(_high_speed_config_descr_end - _high_speed_config_descr) ; LSB
   2029 00                  133 	.db	>(_high_speed_config_descr_end - _high_speed_config_descr) ; MSB
   202A 03                  134 	.db	3		; bNumInterfaces
   202B 01                  135 	.db	1		; bConfigurationValue
   202C 00                  136 	.db	0		; iConfiguration
   202D C0                  137 	.db	0x80 | bmSELF_POWERED ; bmAttributes
   202E 00                  138 	.db	0		; bMaxPower
                            139 
                            140 	;; interface descriptor 0 (command & status, ep0 COMMAND)
                            141 	
   202F 09                  142 	.db	DSCR_INTRFC_LEN
   2030 04                  143 	.db	DSCR_INTRFC
   2031 00                  144 	.db	0		; bInterfaceNumber (zero based)
   2032 00                  145 	.db	0		; bAlternateSetting
   2033 00                  146 	.db	0		; bNumEndpoints
   2034 FF                  147 	.db	0xff		; bInterfaceClass (vendor specific)
   2035 FF                  148 	.db	0xff		; bInterfaceSubClass (vendor specific)
   2036 FF                  149 	.db	0xff		; bInterfaceProtocol (vendor specific)
   2037 03                  150 	.db	SI_COMMAND_AND_STATUS	; iInterface (description)
                            151 
                            152 	;; interface descriptor 1 (transmit path, ep2 OUT BULK)
                            153 	
   2038 09                  154 	.db	DSCR_INTRFC_LEN
   2039 04                  155 	.db	DSCR_INTRFC
   203A 01                  156 	.db	1		; bInterfaceNumber (zero based)
   203B 00                  157 	.db	0		; bAlternateSetting
   203C 01                  158 	.db	1		; bNumEndpoints
   203D FF                  159 	.db	0xff		; bInterfaceClass (vendor specific)
   203E FF                  160 	.db	0xff		; bInterfaceSubClass (vendor specific)
   203F FF                  161 	.db	0xff		; bInterfaceProtocol (vendor specific)
   2040 04                  162 	.db	SI_TX_PATH	; iInterface (description)
                            163 
                            164 	;; interface 1's end point
                            165 
   2041 07                  166 	.db	DSCR_ENDPNT_LEN
   2042 05                  167 	.db	DSCR_ENDPNT
   2043 02                  168 	.db	0x02		; bEndpointAddress (ep 2 OUT)
   2044 02                  169 	.db	ET_BULK		; bmAttributes
   2045 00                  170 	.db	<512		; wMaxPacketSize (LSB)
   2046 02                  171 	.db	>512		; wMaxPacketSize (MSB)
   2047 00                  172 	.db	0		; bInterval (iso only)
                            173 
                            174 	;; interface descriptor 2 (receive path, ep6 IN BULK)
                            175 	
   2048 09                  176 	.db	DSCR_INTRFC_LEN
   2049 04                  177 	.db	DSCR_INTRFC
   204A 02                  178 	.db	2		; bInterfaceNumber (zero based)
   204B 00                  179 	.db	0		; bAlternateSetting
   204C 01                  180 	.db	1		; bNumEndpoints
   204D FF                  181 	.db	0xff		; bInterfaceClass (vendor specific)
   204E FF                  182 	.db	0xff		; bInterfaceSubClass (vendor specific)
   204F FF                  183 	.db	0xff		; bInterfaceProtocol (vendor specific)
   2050 05                  184 	.db	SI_RX_PATH	; iInterface (description)
                            185 
                            186 	;; interface 2's end point
                            187 
   2051 07                  188 	.db	DSCR_ENDPNT_LEN
   2052 05                  189 	.db	DSCR_ENDPNT
   2053 86                  190 	.db	0x86		; bEndpointAddress (ep 6 IN)
   2054 02                  191 	.db	ET_BULK		; bmAttributes
   2055 00                  192 	.db	<512		; wMaxPacketSize (LSB)
   2056 02                  193 	.db	>512		; wMaxPacketSize (MSB)
   2057 00                  194 	.db	0		; bInterval (iso only)
                            195 
   2058                     196 _high_speed_config_descr_end:		
                            197 
                            198 ;;; ----------------------------------------------------------------
                            199 ;;; descriptors used when operating at full speed (12Mb/sec)
                            200 ;;; ----------------------------------------------------------------
                            201 
   2058                     202 	.even
   2058                     203 _full_speed_device_descr::	
   2058 12                  204 	.db	DSCR_DEVICE_LEN
   2059 01                  205 	.db	DSCR_DEVICE
   205A 00                  206 	.db	<0x0200		; Specification version (LSB)
   205B 02                  207 	.db	>0x0200		; Specification version (MSB)
   205C FF                  208 	.db	0xff		; device class (vendor specific)
   205D FF                  209 	.db	0xff		; device subclass (vendor specific)
   205E FF                  210 	.db	0xff		; device protocol (vendor specific)
   205F 40                  211 	.db	64		; bMaxPacketSize0 for endpoint 0
   2060 FE                  212 	.db	<VID_FREE	; idVendor
   2061 FF                  213 	.db	>VID_FREE	; idVendor
   2062 07                  214 	.db	<PID_USRP	; idProduct
   2063 00                  215 	.db	>PID_USRP	; idProduct
   2064                     216 _usb_desc_hw_rev_binary_patch_location_1::
   2064 00                  217 	.db	<DID_USRP	; bcdDevice
   2065 01                  218 	.db	>DID_USRP	; bcdDevice
   2066 01                  219 	.db	SI_VENDOR	; iManufacturer (string index)
   2067 02                  220 	.db	SI_PRODUCT	; iProduct (string index)
   2068 00                  221 	.db	SI_NONE		; iSerial number (None)
   2069 01                  222 	.db	1		; bNumConfigurations
                            223 	
                            224 	
                            225 ;;; describes the other speed (480Mb/sec)
   206A                     226 	.even
   206A                     227 _full_speed_devqual_descr::
   206A 0A                  228 	.db	DSCR_DEVQUAL_LEN
   206B 06                  229 	.db	DSCR_DEVQUAL
   206C 00                  230 	.db	<0x0200		; bcdUSB
   206D 02                  231 	.db	>0x0200		; bcdUSB
   206E FF                  232 	.db	0xff		; bDeviceClass
   206F FF                  233 	.db	0xff		; bDeviceSubClass
   2070 FF                  234 	.db	0xff		; bDeviceProtocol
   2071 40                  235 	.db	64		; bMaxPacketSize0
   2072 01                  236 	.db	1		; bNumConfigurations (one config at 480Mb/sec)
   2073 00                  237 	.db	0		; bReserved
                            238 	
   2074                     239 	.even
   2074                     240 _full_speed_config_descr::	
   2074 09                  241 	.db	DSCR_CONFIG_LEN
   2075 02                  242 	.db	DSCR_CONFIG
   2076 12                  243 	.db	<(_full_speed_config_descr_end - _full_speed_config_descr) ; LSB
   2077 00                  244 	.db	>(_full_speed_config_descr_end - _full_speed_config_descr) ; MSB
   2078 01                  245 	.db	1		; bNumInterfaces
   2079 01                  246 	.db	1		; bConfigurationValue
   207A 00                  247 	.db	0		; iConfiguration
   207B C0                  248 	.db	0x80 | bmSELF_POWERED ; bmAttributes
   207C 00                  249 	.db	0		; bMaxPower
                            250 
                            251 	;; interface descriptor 0 (command & status, ep0 COMMAND)
                            252 	
   207D 09                  253 	.db	DSCR_INTRFC_LEN
   207E 04                  254 	.db	DSCR_INTRFC
   207F 00                  255 	.db	0		; bInterfaceNumber (zero based)
   2080 00                  256 	.db	0		; bAlternateSetting
   2081 00                  257 	.db	0		; bNumEndpoints
   2082 FF                  258 	.db	0xff		; bInterfaceClass (vendor specific)
   2083 FF                  259 	.db	0xff		; bInterfaceSubClass (vendor specific)
   2084 FF                  260 	.db	0xff		; bInterfaceProtocol (vendor specific)
   2085 03                  261 	.db	SI_COMMAND_AND_STATUS	; iInterface (description)
                            262 	
   2086                     263 _full_speed_config_descr_end:	
                            264 	
                            265 ;;; ----------------------------------------------------------------
                            266 ;;;			string descriptors
                            267 ;;; ----------------------------------------------------------------
                            268 
   2086                     269 _nstring_descriptors::
   2086 07                  270 	.db	(_string_descriptors_end - _string_descriptors) / 2
                            271 
   2087                     272 _string_descriptors::
   2087 96 20               273 	.db	<str0, >str0
   2089 9C 20               274 	.db	<str1, >str1
   208B C4 20               275 	.db	<str2, >str2
   208D DC 20               276 	.db	<str3, >str3
   208F FE 20               277 	.db	<str4, >str4
   2091 1A 21               278 	.db	<str5, >str5
   2093 34 21               279 	.db	<str6, >str6
   2095                     280 _string_descriptors_end:
                            281 
                    0000    282 	SI_NONE = 0
                            283 	;; str0 contains the language ID's.
   2096                     284 	.even
   2096 06                  285 str0:	.db	str0_end - str0
   2097 03                  286 	.db	DSCR_STRING
   2098 00                  287 	.db	0
   2099 00                  288 	.db	0
   209A 09                  289 	.db	<0x0409		; magic code for US English (LSB)
   209B 04                  290 	.db	>0x0409		; magic code for US English (MSB)
   209C                     291 str0_end:
                            292 
                    0001    293 	SI_VENDOR = 1
   209C                     294 	.even
   209C 28                  295 str1:	.db	str1_end - str1
   209D 03                  296 	.db	DSCR_STRING
   209E 46 00               297 	.db	'F, 0		; 16-bit unicode
   20A0 72 00               298 	.db	'r, 0
   20A2 65 00               299 	.db	'e, 0
   20A4 65 00               300 	.db	'e, 0
   20A6 20 00               301 	.db	' , 0
   20A8 53 00               302 	.db	'S, 0
   20AA 6F 00               303 	.db	'o, 0
   20AC 66 00               304 	.db	'f, 0
   20AE 74 00               305 	.db	't, 0
   20B0 77 00               306 	.db	'w, 0
   20B2 61 00               307 	.db	'a, 0
   20B4 72 00               308 	.db	'r, 0
   20B6 65 00               309 	.db	'e, 0
   20B8 20 00               310 	.db	' , 0
   20BA 46 00               311 	.db	'F, 0
   20BC 6F 00               312 	.db	'o, 0
   20BE 6C 00               313 	.db	'l, 0
   20C0 6B 00               314 	.db	'k, 0
   20C2 73 00               315 	.db	's, 0
   20C4                     316 str1_end:
                            317 
                    0002    318 	SI_PRODUCT = 2
   20C4                     319 	.even
   20C4 18                  320 str2:	.db	str2_end - str2
   20C5 03                  321 	.db	DSCR_STRING
   20C6 48 00               322 	.db	'H, 0
   20C8 50 00               323 	.db	'P, 0
   20CA 53 00               324 	.db	'S, 0
   20CC 44 00               325 	.db	'D, 0
   20CE 52 00               326 	.db	'R, 0
   20D0 20 00               327 	.db	' , 0
   20D2 52 00               328 	.db	'R, 0
   20D4 65 00               329 	.db	'e, 0
   20D6 76 00               330 	.db	'v, 0
   20D8 20 00               331 	.db	' , 0
   20DA                     332 _usb_desc_hw_rev_ascii_patch_location_0::
   20DA 3F 00               333 	.db	'?, 0
   20DC                     334 str2_end:
                            335 
                    0003    336 	SI_COMMAND_AND_STATUS = 3
   20DC                     337 	.even
   20DC 22                  338 str3:	.db	str3_end - str3
   20DD 03                  339 	.db	DSCR_STRING
   20DE 43 00               340 	.db	'C, 0
   20E0 6F 00               341 	.db	'o, 0
   20E2 6D 00               342 	.db	'm, 0
   20E4 6D 00               343 	.db	'm, 0
   20E6 61 00               344 	.db	'a, 0
   20E8 6E 00               345 	.db	'n, 0
   20EA 64 00               346 	.db	'd, 0
   20EC 20 00               347 	.db	' , 0
   20EE 26 00               348 	.db	'&, 0
   20F0 20 00               349 	.db	' , 0
   20F2 53 00               350 	.db	'S, 0
   20F4 74 00               351 	.db	't, 0
   20F6 61 00               352 	.db	'a, 0
   20F8 74 00               353 	.db	't, 0
   20FA 75 00               354 	.db	'u, 0
   20FC 73 00               355 	.db	's, 0
   20FE                     356 str3_end:
                            357 
                    0004    358 	SI_TX_PATH = 4
   20FE                     359 	.even
   20FE 1C                  360 str4:	.db	str4_end - str4
   20FF 03                  361 	.db	DSCR_STRING
   2100 54 00               362 	.db	'T, 0
   2102 72 00               363 	.db	'r, 0
   2104 61 00               364 	.db	'a, 0
   2106 6E 00               365 	.db	'n, 0
   2108 73 00               366 	.db	's, 0
   210A 6D 00               367 	.db	'm, 0
   210C 69 00               368 	.db	'i, 0
   210E 74 00               369 	.db	't, 0
   2110 20 00               370 	.db	' , 0
   2112 50 00               371 	.db	'P, 0
   2114 61 00               372 	.db	'a, 0
   2116 74 00               373 	.db	't, 0
   2118 68 00               374 	.db	'h, 0
   211A                     375 str4_end:
                            376 
                    0005    377 	SI_RX_PATH = 5
   211A                     378 	.even
   211A 1A                  379 str5:	.db	str5_end - str5
   211B 03                  380 	.db	DSCR_STRING
   211C 52 00               381 	.db	'R, 0
   211E 65 00               382 	.db	'e, 0
   2120 63 00               383 	.db	'c, 0
   2122 65 00               384 	.db	'e, 0
   2124 69 00               385 	.db	'i, 0
   2126 76 00               386 	.db	'v, 0
   2128 65 00               387 	.db	'e, 0
   212A 20 00               388 	.db	' , 0
   212C 50 00               389 	.db	'P, 0
   212E 61 00               390 	.db	'a, 0
   2130 74 00               391 	.db	't, 0
   2132 68 00               392 	.db	'h, 0
   2134                     393 str5_end:
                            394 
                    0006    395 	SI_SERIAL = 6
   2134                     396 	.even
   2134 12                  397 str6:	.db	str6_end - str6
   2135 03                  398 	.db	DSCR_STRING
   2136                     399 _usb_desc_serial_number_ascii::
   2136 33 00               400 	.db	'3, 0
   2138 2E 00               401 	.db	'., 0
   213A 31 00               402 	.db	'1, 0
   213C 34 00               403 	.db	'4, 0
   213E 31 00               404 	.db	'1, 0
   2140 35 00               405 	.db	'5, 0
   2142 39 00               406 	.db	'9, 0
   2144 33 00               407 	.db	'3, 0
   2146                     408 str6_end:
                            409 
