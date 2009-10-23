/*
 *
 * Copyright (C) 2009 Bill Tracey, KD5TFD  (bill@ewjt.com) 
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */


#define _CRT_SECURE_NO_WARNINGS  (1) /* I'm an old C programmer, I can handle the danger! */ 

#include <stdio.h>
#include <string.h> 
#define HAVE_REMOTE (1) 

#include <pcap.h>

#include <winsock.h>
// #include <ws2def.h> 
#include <winsock2.h>
#include <iphlpapi.h> 
#pragma comment(lib, "IPHLPAPI.lib")
#include <windows.h>
#include <nanotimer.h> 

#if 0 
#include <mstcpip.h>
#endif 


unsigned char pktData[] = { 
	0x7f, 0x7f, 0x7f, 0,    0,    0,    0,    0, 
    0x00, 0x00, 0x00, 0x7f, 0xff, 0xff, 0x00, 0x00, /*        0 (0x0000)  8388607 (0x7fffff)         0 (0x00)  */
    0x25, 0xba, 0x89, 0x7a, 0x50, 0x35, 0xda, 0x46, /*  2472585 (0x25ba89)  8015925 (0x7a5035)     -9658 (0xffffda46)  */
    0x48, 0x1a, 0xdf, 0x69, 0xc2, 0x30, 0xb7, 0xe6, /*  4725471 (0x481adf)  6930992 (0x69c230)    -18458 (0xffffb7e6)  */
    0x64, 0x13, 0x0d, 0x4f, 0xce, 0x83, 0x9b, 0xee, /*  6558477 (0x64130d)  5230211 (0x4fce83)    -25618 (0xffff9bee)  */
    0x77, 0x26, 0xde, 0x2e, 0xc3, 0x7e, 0x88, 0xda, /*  7808734 (0x7726de)  3064702 (0x2ec37e)    -30502 (0xffff88da)  */
    0x7f, 0xa4, 0x5f, 0x09, 0x90, 0xc1, 0x80, 0x5d, /*  8365151 (0x7fa45f)   626881 (0x990c1)    -32675 (0xffff805d)  */
    0x7c, 0xca, 0x6f, 0xe3, 0x84, 0x6f, 0x83, 0x37, /*  8178287 (0x7cca6f) -1866641 (0xffe3846f)    -31945 (0xffff8337)  */
    0x6e, 0xd9, 0xeb, 0xc0, 0x00, 0x00, 0x91, 0x27, /*  7264747 (0x6ed9eb) -4194304 (0xffc00000)    -28377 (0xffff9127)  */
    0x57, 0x0f, 0xe6, 0xa2, 0x2b, 0x5c, 0xa8, 0xf1, /*  5705702 (0x570fe6) -6149284 (0xffa22b5c)    -22287 (0xffffa8f1)  */
    0x37, 0x89, 0x80, 0x8c, 0xad, 0x0e, 0xc8, 0x77, /*  3639680 (0x378980) -7557874 (0xff8cad0e)    -14217 (0xffffc877)  */
    0x13, 0x13, 0xd1, 0x81, 0x6d, 0xff, 0xec, 0xec, /*  1250257 (0x1313d1) -8294913 (0xff816dff)     -4884 (0xffffecec)  */
    0xec, 0xec, 0x2f, 0x81, 0x6d, 0xff, 0x13, 0x14, /* -1250257 (0xffecec2f) -8294913 (0xff816dff)      4884 (0x1314)  */
    0xc8, 0x76, 0x80, 0x8c, 0xad, 0x0e, 0x37, 0x89, /* -3639680 (0xffc87680) -7557874 (0xff8cad0e)     14217 (0x3789)  */
    0xa8, 0xf0, 0x1a, 0xa2, 0x2b, 0x5c, 0x57, 0x0f, /* -5705702 (0xffa8f01a) -6149284 (0xffa22b5c)     22287 (0x570f)  */
    0x91, 0x26, 0x15, 0xc0, 0x00, 0x00, 0x6e, 0xd9, /* -7264747 (0xff912615) -4194304 (0xffc00000)     28377 (0x6ed9)  */
    0x83, 0x35, 0x91, 0xe3, 0x84, 0x6f, 0x7c, 0xc9, /* -8178287 (0xff833591) -1866641 (0xffe3846f)     31945 (0x7cc9)  */
    0x80, 0x5b, 0xa1, 0x09, 0x90, 0xc1, 0x7f, 0xa3, /* -8365151 (0xff805ba1)   626881 (0x990c1)     32675 (0x7fa3)  */
    0x88, 0xd9, 0x22, 0x2e, 0xc3, 0x7e, 0x77, 0x26, /* -7808734 (0xff88d922)  3064702 (0x2ec37e)     30502 (0x7726)  */
    0x9b, 0xec, 0xf3, 0x4f, 0xce, 0x83, 0x64, 0x12, /* -6558477 (0xff9becf3)  5230211 (0x4fce83)     25618 (0x6412)  */
    0xb7, 0xe5, 0x21, 0x69, 0xc2, 0x30, 0x48, 0x1a, /* -4725471 (0xffb7e521)  6930992 (0x69c230)     18458 (0x481a)  */
    0xda, 0x45, 0x77, 0x7a, 0x50, 0x35, 0x25, 0xba, /* -2472585 (0xffda4577)  8015925 (0x7a5035)      9658 (0x25ba)  */
    0x00, 0x00, 0x00, 0x7f, 0xff, 0xff, 0x00, 0x00, /*        0 (0x0000)  8388607 (0x7fffff)         0 (0x00)  */
    0x25, 0xba, 0x89, 0x7a, 0x50, 0x35, 0xda, 0x46, /*  2472585 (0x25ba89)  8015925 (0x7a5035)     -9658 (0xffffda46)  */
    0x48, 0x1a, 0xdf, 0x69, 0xc2, 0x30, 0xb7, 0xe6, /*  4725471 (0x481adf)  6930992 (0x69c230)    -18458 (0xffffb7e6)  */
    0x64, 0x13, 0x0d, 0x4f, 0xce, 0x83, 0x9b, 0xee, /*  6558477 (0x64130d)  5230211 (0x4fce83)    -25618 (0xffff9bee)  */
    0x77, 0x26, 0xde, 0x2e, 0xc3, 0x7e, 0x88, 0xda, /*  7808734 (0x7726de)  3064702 (0x2ec37e)    -30502 (0xffff88da)  */
    0x7f, 0xa4, 0x5f, 0x09, 0x90, 0xc1, 0x80, 0x5d, /*  8365151 (0x7fa45f)   626881 (0x990c1)    -32675 (0xffff805d)  */
    0x7c, 0xca, 0x6f, 0xe3, 0x84, 0x6f, 0x83, 0x37, /*  8178287 (0x7cca6f) -1866641 (0xffe3846f)    -31945 (0xffff8337)  */
    0x6e, 0xd9, 0xeb, 0xc0, 0x00, 0x01, 0x91, 0x27, /*  7264747 (0x6ed9eb) -4194303 (0xffc00001)    -28377 (0xffff9127)  */
    0x57, 0x0f, 0xe6, 0xa2, 0x2b, 0x5c, 0xa8, 0xf1, /*  5705702 (0x570fe6) -6149284 (0xffa22b5c)    -22287 (0xffffa8f1)  */
    0x37, 0x89, 0x80, 0x8c, 0xad, 0x0e, 0xc8, 0x77, /*  3639680 (0x378980) -7557874 (0xff8cad0e)    -14217 (0xffffc877)  */
    0x13, 0x13, 0xd1, 0x81, 0x6d, 0xff, 0xec, 0xec, /*  1250257 (0x1313d1) -8294913 (0xff816dff)     -4884 (0xffffecec)  */
    0xec, 0xec, 0x2f, 0x81, 0x6d, 0xff, 0x13, 0x14, /* -1250257 (0xffecec2f) -8294913 (0xff816dff)      4884 (0x1314)  */
    0xc8, 0x76, 0x80, 0x8c, 0xad, 0x0e, 0x37, 0x89, /* -3639680 (0xffc87680) -7557874 (0xff8cad0e)     14217 (0x3789)  */
    0xa8, 0xf0, 0x1a, 0xa2, 0x2b, 0x5c, 0x57, 0x0f, /* -5705702 (0xffa8f01a) -6149284 (0xffa22b5c)     22287 (0x570f)  */
    0x91, 0x26, 0x15, 0xc0, 0x00, 0x00, 0x6e, 0xd9, /* -7264747 (0xff912615) -4194304 (0xffc00000)     28377 (0x6ed9)  */
    0x83, 0x35, 0x91, 0xe3, 0x84, 0x6f, 0x7c, 0xc9, /* -8178287 (0xff833591) -1866641 (0xffe3846f)     31945 (0x7cc9)  */
    0x80, 0x5b, 0xa1, 0x09, 0x90, 0xc1, 0x7f, 0xa3, /* -8365151 (0xff805ba1)   626881 (0x990c1)     32675 (0x7fa3)  */
    0x88, 0xd9, 0x22, 0x2e, 0xc3, 0x7e, 0x77, 0x26, /* -7808734 (0xff88d922)  3064702 (0x2ec37e)     30502 (0x7726)  */
    0x9b, 0xec, 0xf3, 0x4f, 0xce, 0x83, 0x64, 0x12, /* -6558477 (0xff9becf3)  5230211 (0x4fce83)     25618 (0x6412)  */
    0xb7, 0xe5, 0x21, 0x69, 0xc2, 0x30, 0x48, 0x1a, /* -4725471 (0xffb7e521)  6930992 (0x69c230)     18458 (0x481a)  */
    0xda, 0x45, 0x77, 0x7a, 0x50, 0x35, 0x25, 0xba, /* -2472585 (0xffda4577)  8015925 (0x7a5035)      9658 (0x25ba)  */
    0x00, 0x00, 0x00, 0x7f, 0xff, 0xff, 0x00, 0x00, /*        0 (0x0000)  8388607 (0x7fffff)         0 (0x00)  */
    0x25, 0xba, 0x89, 0x7a, 0x50, 0x35, 0xda, 0x46, /*  2472585 (0x25ba89)  8015925 (0x7a5035)     -9658 (0xffffda46)  */
    0x48, 0x1a, 0xdf, 0x69, 0xc2, 0x30, 0xb7, 0xe6, /*  4725471 (0x481adf)  6930992 (0x69c230)    -18458 (0xffffb7e6)  */
    0x64, 0x13, 0x0d, 0x4f, 0xce, 0x83, 0x9b, 0xee, /*  6558477 (0x64130d)  5230211 (0x4fce83)    -25618 (0xffff9bee)  */
    0x77, 0x26, 0xde, 0x2e, 0xc3, 0x7e, 0x88, 0xda, /*  7808734 (0x7726de)  3064702 (0x2ec37e)    -30502 (0xffff88da)  */
    0x7f, 0xa4, 0x5f, 0x09, 0x90, 0xc1, 0x80, 0x5d, /*  8365151 (0x7fa45f)   626881 (0x990c1)    -32675 (0xffff805d)  */
    0x7c, 0xca, 0x6f, 0xe3, 0x84, 0x6f, 0x83, 0x37, /*  8178287 (0x7cca6f) -1866641 (0xffe3846f)    -31945 (0xffff8337)  */
    0x6e, 0xd9, 0xeb, 0xc0, 0x00, 0x01, 0x91, 0x27, /*  7264747 (0x6ed9eb) -4194303 (0xffc00001)    -28377 (0xffff9127)  */
    0x57, 0x0f, 0xe6, 0xa2, 0x2b, 0x5c, 0xa8, 0xf1, /*  5705702 (0x570fe6) -6149284 (0xffa22b5c)    -22287 (0xffffa8f1)  */
    0x37, 0x89, 0x80, 0x8c, 0xad, 0x0e, 0xc8, 0x77, /*  3639680 (0x378980) -7557874 (0xff8cad0e)    -14217 (0xffffc877)  */
    0x13, 0x13, 0xd1, 0x81, 0x6d, 0xff, 0xec, 0xec, /*  1250257 (0x1313d1) -8294913 (0xff816dff)     -4884 (0xffffecec)  */
    0xec, 0xec, 0x2f, 0x81, 0x6d, 0xff, 0x13, 0x14, /* -1250257 (0xffecec2f) -8294913 (0xff816dff)      4884 (0x1314)  */
    0xc8, 0x76, 0x80, 0x8c, 0xad, 0x0e, 0x37, 0x89, /* -3639680 (0xffc87680) -7557874 (0xff8cad0e)     14217 (0x3789)  */
    0xa8, 0xf0, 0x1a, 0xa2, 0x2b, 0x5c, 0x57, 0x0f, /* -5705702 (0xffa8f01a) -6149284 (0xffa22b5c)     22287 (0x570f)  */
    0x91, 0x26, 0x15, 0xc0, 0x00, 0x00, 0x6e, 0xd9, /* -7264747 (0xff912615) -4194304 (0xffc00000)     28377 (0x6ed9)  */
    0x83, 0x35, 0x91, 0xe3, 0x84, 0x6f, 0x7c, 0xc9, /* -8178287 (0xff833591) -1866641 (0xffe3846f)     31945 (0x7cc9)  */
    0x80, 0x5b, 0xa1, 0x09, 0x90, 0xc1, 0x7f, 0xa3, /* -8365151 (0xff805ba1)   626881 (0x990c1)     32675 (0x7fa3)  */
    0x88, 0xd9, 0x22, 0x2e, 0xc3, 0x7e, 0x77, 0x26, /* -7808734 (0xff88d922)  3064702 (0x2ec37e)     30502 (0x7726)  */
    0x9b, 0xec, 0xf3, 0x4f, 0xce, 0x83, 0x64, 0x12, /* -6558477 (0xff9becf3)  5230211 (0x4fce83)     25618 (0x6412)  */
    0xb7, 0xe5, 0x21, 0x69, 0xc2, 0x30, 0x48, 0x1a, /* -4725471 (0xffb7e521)  6930992 (0x69c230)     18458 (0x481a)  */
    0xda, 0x45, 0x77, 0x7a, 0x50, 0x35, 0x25, 0xba  /* -2472585 (0xffda4577)  8015925 (0x7a5035)      9658 (0x25ba)  */
};

unsigned char iqlr_pktData[] = { 
	0x7f, 0x7f, 0x7f, 0,    0,    0,    0,    0, 
    0x00, 0x00, 0x7f, 0xff, 0x00, 0x00, 0x80, 0x01, /*        0 (0x00)    32767 (0x7fff)         0 (0x00)   -32767 (0xffff8001)  */
    0x25, 0xba, 0x7a, 0x4f, 0xda, 0x46, 0x85, 0xb1, /*     9658 (0x25ba)    31311 (0x7a4f)     -9658 (0xffffda46)   -31311 (0xffff85b1)  */
    0x48, 0x1a, 0x69, 0xc1, 0xb7, 0xe6, 0x96, 0x3f, /*    18458 (0x481a)    27073 (0x69c1)    -18458 (0xffffb7e6)   -27073 (0xffff963f)  */
    0x64, 0x12, 0x4f, 0xce, 0x9b, 0xee, 0xb0, 0x32, /*    25618 (0x6412)    20430 (0x4fce)    -25618 (0xffff9bee)   -20430 (0xffffb032)  */
    0x77, 0x26, 0x2e, 0xc3, 0x88, 0xda, 0xd1, 0x3d, /*    30502 (0x7726)    11971 (0x2ec3)    -30502 (0xffff88da)   -11971 (0xffffd13d)  */
    0x7f, 0xa3, 0x09, 0x91, 0x80, 0x5d, 0xf6, 0x6f, /*    32675 (0x7fa3)     2449 (0x991)    -32675 (0xffff805d)    -2449 (0xfffff66f)  */
    0x7c, 0xc9, 0xe3, 0x85, 0x83, 0x37, 0x1c, 0x7b, /*    31945 (0x7cc9)    -7291 (0xffffe385)    -31945 (0xffff8337)     7291 (0x1c7b)  */
    0x6e, 0xd9, 0xc0, 0x00, 0x91, 0x27, 0x40, 0x00, /*    28377 (0x6ed9)   -16384 (0xffffc000)    -28377 (0xffff9127)    16384 (0x4000)  */
    0x57, 0x0f, 0xa2, 0x2c, 0xa8, 0xf1, 0x5d, 0xd4, /*    22287 (0x570f)   -24020 (0xffffa22c)    -22287 (0xffffa8f1)    24020 (0x5dd4)  */
    0x37, 0x89, 0x8c, 0xae, 0xc8, 0x77, 0x73, 0x52, /*    14217 (0x3789)   -29522 (0xffff8cae)    -14217 (0xffffc877)    29522 (0x7352)  */
    0x13, 0x14, 0x81, 0x6f, 0xec, 0xec, 0x7e, 0x91, /*     4884 (0x1314)   -32401 (0xffff816f)     -4884 (0xffffecec)    32401 (0x7e91)  */
    0xec, 0xec, 0x81, 0x6f, 0x13, 0x14, 0x7e, 0x91, /*    -4884 (0xffffecec)   -32401 (0xffff816f)      4884 (0x1314)    32401 (0x7e91)  */
    0xc8, 0x77, 0x8c, 0xae, 0x37, 0x89, 0x73, 0x52, /*   -14217 (0xffffc877)   -29522 (0xffff8cae)     14217 (0x3789)    29522 (0x7352)  */
    0xa8, 0xf1, 0xa2, 0x2c, 0x57, 0x0f, 0x5d, 0xd4, /*   -22287 (0xffffa8f1)   -24020 (0xffffa22c)     22287 (0x570f)    24020 (0x5dd4)  */
    0x91, 0x27, 0xc0, 0x00, 0x6e, 0xd9, 0x40, 0x00, /*   -28377 (0xffff9127)   -16384 (0xffffc000)     28377 (0x6ed9)    16384 (0x4000)  */
    0x83, 0x37, 0xe3, 0x85, 0x7c, 0xc9, 0x1c, 0x7b, /*   -31945 (0xffff8337)    -7291 (0xffffe385)     31945 (0x7cc9)     7291 (0x1c7b)  */
    0x80, 0x5d, 0x09, 0x91, 0x7f, 0xa3, 0xf6, 0x6f, /*   -32675 (0xffff805d)     2449 (0x991)     32675 (0x7fa3)    -2449 (0xfffff66f)  */
    0x88, 0xda, 0x2e, 0xc3, 0x77, 0x26, 0xd1, 0x3d, /*   -30502 (0xffff88da)    11971 (0x2ec3)     30502 (0x7726)   -11971 (0xffffd13d)  */
    0x9b, 0xee, 0x4f, 0xce, 0x64, 0x12, 0xb0, 0x32, /*   -25618 (0xffff9bee)    20430 (0x4fce)     25618 (0x6412)   -20430 (0xffffb032)  */
    0xb7, 0xe6, 0x69, 0xc1, 0x48, 0x1a, 0x96, 0x3f, /*   -18458 (0xffffb7e6)    27073 (0x69c1)     18458 (0x481a)   -27073 (0xffff963f)  */
    0xda, 0x46, 0x7a, 0x4f, 0x25, 0xba, 0x85, 0xb1, /*    -9658 (0xffffda46)    31311 (0x7a4f)      9658 (0x25ba)   -31311 (0xffff85b1)  */
    0x00, 0x00, 0x7f, 0xff, 0x00, 0x00, 0x80, 0x01, /*        0 (0x00)    32767 (0x7fff)         0 (0x00)   -32767 (0xffff8001)  */
    0x25, 0xba, 0x7a, 0x4f, 0xda, 0x46, 0x85, 0xb1, /*     9658 (0x25ba)    31311 (0x7a4f)     -9658 (0xffffda46)   -31311 (0xffff85b1)  */
    0x48, 0x1a, 0x69, 0xc1, 0xb7, 0xe6, 0x96, 0x3f, /*    18458 (0x481a)    27073 (0x69c1)    -18458 (0xffffb7e6)   -27073 (0xffff963f)  */
    0x64, 0x12, 0x4f, 0xce, 0x9b, 0xee, 0xb0, 0x32, /*    25618 (0x6412)    20430 (0x4fce)    -25618 (0xffff9bee)   -20430 (0xffffb032)  */
    0x77, 0x26, 0x2e, 0xc3, 0x88, 0xda, 0xd1, 0x3d, /*    30502 (0x7726)    11971 (0x2ec3)    -30502 (0xffff88da)   -11971 (0xffffd13d)  */
    0x7f, 0xa3, 0x09, 0x91, 0x80, 0x5d, 0xf6, 0x6f, /*    32675 (0x7fa3)     2449 (0x991)    -32675 (0xffff805d)    -2449 (0xfffff66f)  */
    0x7c, 0xc9, 0xe3, 0x85, 0x83, 0x37, 0x1c, 0x7b, /*    31945 (0x7cc9)    -7291 (0xffffe385)    -31945 (0xffff8337)     7291 (0x1c7b)  */
    0x6e, 0xd9, 0xc0, 0x00, 0x91, 0x27, 0x40, 0x00, /*    28377 (0x6ed9)   -16384 (0xffffc000)    -28377 (0xffff9127)    16384 (0x4000)  */
    0x57, 0x0f, 0xa2, 0x2c, 0xa8, 0xf1, 0x5d, 0xd4, /*    22287 (0x570f)   -24020 (0xffffa22c)    -22287 (0xffffa8f1)    24020 (0x5dd4)  */
    0x37, 0x89, 0x8c, 0xae, 0xc8, 0x77, 0x73, 0x52, /*    14217 (0x3789)   -29522 (0xffff8cae)    -14217 (0xffffc877)    29522 (0x7352)  */
    0x13, 0x14, 0x81, 0x6f, 0xec, 0xec, 0x7e, 0x91, /*     4884 (0x1314)   -32401 (0xffff816f)     -4884 (0xffffecec)    32401 (0x7e91)  */
    0xec, 0xec, 0x81, 0x6f, 0x13, 0x14, 0x7e, 0x91, /*    -4884 (0xffffecec)   -32401 (0xffff816f)      4884 (0x1314)    32401 (0x7e91)  */
    0xc8, 0x77, 0x8c, 0xae, 0x37, 0x89, 0x73, 0x52, /*   -14217 (0xffffc877)   -29522 (0xffff8cae)     14217 (0x3789)    29522 (0x7352)  */
    0xa8, 0xf1, 0xa2, 0x2c, 0x57, 0x0f, 0x5d, 0xd4, /*   -22287 (0xffffa8f1)   -24020 (0xffffa22c)     22287 (0x570f)    24020 (0x5dd4)  */
    0x91, 0x27, 0xc0, 0x00, 0x6e, 0xd9, 0x40, 0x00, /*   -28377 (0xffff9127)   -16384 (0xffffc000)     28377 (0x6ed9)    16384 (0x4000)  */
    0x83, 0x37, 0xe3, 0x85, 0x7c, 0xc9, 0x1c, 0x7b, /*   -31945 (0xffff8337)    -7291 (0xffffe385)     31945 (0x7cc9)     7291 (0x1c7b)  */
    0x80, 0x5d, 0x09, 0x91, 0x7f, 0xa3, 0xf6, 0x6f, /*   -32675 (0xffff805d)     2449 (0x991)     32675 (0x7fa3)    -2449 (0xfffff66f)  */
    0x88, 0xda, 0x2e, 0xc3, 0x77, 0x26, 0xd1, 0x3d, /*   -30502 (0xffff88da)    11971 (0x2ec3)     30502 (0x7726)   -11971 (0xffffd13d)  */
    0x9b, 0xee, 0x4f, 0xce, 0x64, 0x12, 0xb0, 0x32, /*   -25618 (0xffff9bee)    20430 (0x4fce)     25618 (0x6412)   -20430 (0xffffb032)  */
    0xb7, 0xe6, 0x69, 0xc1, 0x48, 0x1a, 0x96, 0x3f, /*   -18458 (0xffffb7e6)    27073 (0x69c1)     18458 (0x481a)   -27073 (0xffff963f)  */
    0xda, 0x46, 0x7a, 0x4f, 0x25, 0xba, 0x85, 0xb1, /*    -9658 (0xffffda46)    31311 (0x7a4f)      9658 (0x25ba)   -31311 (0xffff85b1)  */
    0x00, 0x00, 0x7f, 0xff, 0x00, 0x00, 0x80, 0x01, /*        0 (0x00)    32767 (0x7fff)         0 (0x00)   -32767 (0xffff8001)  */
    0x25, 0xba, 0x7a, 0x4f, 0xda, 0x46, 0x85, 0xb1, /*     9658 (0x25ba)    31311 (0x7a4f)     -9658 (0xffffda46)   -31311 (0xffff85b1)  */
    0x48, 0x1a, 0x69, 0xc1, 0xb7, 0xe6, 0x96, 0x3f, /*    18458 (0x481a)    27073 (0x69c1)    -18458 (0xffffb7e6)   -27073 (0xffff963f)  */
    0x64, 0x12, 0x4f, 0xce, 0x9b, 0xee, 0xb0, 0x32, /*    25618 (0x6412)    20430 (0x4fce)    -25618 (0xffff9bee)   -20430 (0xffffb032)  */
    0x77, 0x26, 0x2e, 0xc3, 0x88, 0xda, 0xd1, 0x3d, /*    30502 (0x7726)    11971 (0x2ec3)    -30502 (0xffff88da)   -11971 (0xffffd13d)  */
    0x7f, 0xa3, 0x09, 0x91, 0x80, 0x5d, 0xf6, 0x6f, /*    32675 (0x7fa3)     2449 (0x991)    -32675 (0xffff805d)    -2449 (0xfffff66f)  */
    0x7c, 0xc9, 0xe3, 0x85, 0x83, 0x37, 0x1c, 0x7b, /*    31945 (0x7cc9)    -7291 (0xffffe385)    -31945 (0xffff8337)     7291 (0x1c7b)  */
    0x6e, 0xd9, 0xc0, 0x00, 0x91, 0x27, 0x40, 0x00, /*    28377 (0x6ed9)   -16384 (0xffffc000)    -28377 (0xffff9127)    16384 (0x4000)  */
    0x57, 0x0f, 0xa2, 0x2c, 0xa8, 0xf1, 0x5d, 0xd4, /*    22287 (0x570f)   -24020 (0xffffa22c)    -22287 (0xffffa8f1)    24020 (0x5dd4)  */
    0x37, 0x89, 0x8c, 0xae, 0xc8, 0x77, 0x73, 0x52, /*    14217 (0x3789)   -29522 (0xffff8cae)    -14217 (0xffffc877)    29522 (0x7352)  */
    0x13, 0x14, 0x81, 0x6f, 0xec, 0xec, 0x7e, 0x91, /*     4884 (0x1314)   -32401 (0xffff816f)     -4884 (0xffffecec)    32401 (0x7e91)  */
    0xec, 0xec, 0x81, 0x6f, 0x13, 0x14, 0x7e, 0x91, /*    -4884 (0xffffecec)   -32401 (0xffff816f)      4884 (0x1314)    32401 (0x7e91)  */
    0xc8, 0x77, 0x8c, 0xae, 0x37, 0x89, 0x73, 0x52, /*   -14217 (0xffffc877)   -29522 (0xffff8cae)     14217 (0x3789)    29522 (0x7352)  */
    0xa8, 0xf1, 0xa2, 0x2c, 0x57, 0x0f, 0x5d, 0xd4, /*   -22287 (0xffffa8f1)   -24020 (0xffffa22c)     22287 (0x570f)    24020 (0x5dd4)  */
    0x91, 0x27, 0xc0, 0x00, 0x6e, 0xd9, 0x40, 0x00, /*   -28377 (0xffff9127)   -16384 (0xffffc000)     28377 (0x6ed9)    16384 (0x4000)  */
    0x83, 0x37, 0xe3, 0x85, 0x7c, 0xc9, 0x1c, 0x7b, /*   -31945 (0xffff8337)    -7291 (0xffffe385)     31945 (0x7cc9)     7291 (0x1c7b)  */
    0x80, 0x5d, 0x09, 0x91, 0x7f, 0xa3, 0xf6, 0x6f, /*   -32675 (0xffff805d)     2449 (0x991)     32675 (0x7fa3)    -2449 (0xfffff66f)  */
    0x88, 0xda, 0x2e, 0xc3, 0x77, 0x26, 0xd1, 0x3d, /*   -30502 (0xffff88da)    11971 (0x2ec3)     30502 (0x7726)   -11971 (0xffffd13d)  */
    0x9b, 0xee, 0x4f, 0xce, 0x64, 0x12, 0xb0, 0x32, /*   -25618 (0xffff9bee)    20430 (0x4fce)     25618 (0x6412)   -20430 (0xffffb032)  */
    0xb7, 0xe6, 0x69, 0xc1, 0x48, 0x1a, 0x96, 0x3f, /*   -18458 (0xffffb7e6)    27073 (0x69c1)     18458 (0x481a)   -27073 (0xffff963f)  */
    0xda, 0x46, 0x7a, 0x4f, 0x25, 0xba, 0x85, 0xb1, /*    -9658 (0xffffda46)    31311 (0x7a4f)      9658 (0x25ba)   -31311 (0xffff85b1)  */
};




char *wjt_Copyright = "Copyright (C) 2009 Bill Tracey, KD5TFD (bill@ewjt.com).\nThis progam is licensed under the GUN General Public License Version 2.\n";


// Function prototypes
void ifprint(pcap_if_t *d);
char *iptos(u_long in);
char* ip6tos(struct sockaddr *sockaddr, char *address, int addrlen);



/* Print all the available information on the given interface */
void ifprint(pcap_if_t *d)
{
  pcap_addr_t *a;
  char ip6str[128];

  /* Name */
  printf("%s\n",d->name);

  /* Description */
  if (d->description)
    printf("\tDescription: %s\n",d->description);

  /* Loopback Address*/
  printf("\tLoopback: %s\n",(d->flags & PCAP_IF_LOOPBACK)?"yes":"no");

  /* IP addresses */
  for(a=d->addresses;a;a=a->next) {
    printf("\tAddress Family: #%d\n",a->addr->sa_family);
  
    switch(a->addr->sa_family)
    {
      case AF_INET:
        printf("\tAddress Family Name: AF_INET\n");
        if (a->addr)
          printf("\tAddress: %s\n",iptos(((struct sockaddr_in *)a->addr)->sin_addr.s_addr));
        if (a->netmask)
          printf("\tNetmask: %s\n",iptos(((struct sockaddr_in *)a->netmask)->sin_addr.s_addr));
        if (a->broadaddr)
          printf("\tBroadcast Address: %s\n",iptos(((struct sockaddr_in *)a->broadaddr)->sin_addr.s_addr));
        if (a->dstaddr)
          printf("\tDestination Address: %s\n",iptos(((struct sockaddr_in *)a->dstaddr)->sin_addr.s_addr));
        break;

      case AF_INET6:
        printf("\tAddress Family Name: AF_INET6\n");
        if (a->addr)
          printf("\tAddress: %s\n", ip6tos(a->addr, ip6str, sizeof(ip6str)));
       break;

      default:
        printf("\tAddress Family Name: Unknown\n");
        break;
    }
  }
  printf("\n");
}



/* From tcptraceroute, convert a numeric IP address to a string */
#define IPTOSBUFFERS    12
char *iptos(u_long in)
{
    static char output[IPTOSBUFFERS][3*4+3+1];
    static short which;
    u_char *p;

    p = (u_char *)&in;
    which = (which + 1 == IPTOSBUFFERS ? 0 : which + 1);
    sprintf(output[which], "%d.%d.%d.%d", p[0], p[1], p[2], p[3]);
    return output[which];
}

char* ip6tos(struct sockaddr *sockaddr, char *address, int addrlen)
{
    socklen_t sockaddrlen;

    #ifdef WIN32
    sockaddrlen = sizeof(struct sockaddr_in6);
    #else
    sockaddrlen = sizeof(struct sockaddr_storage);
    #endif


    if(getnameinfo(sockaddr, 
        sockaddrlen, 
        address, 
        addrlen, 
        NULL, 
        0, 
        NI_NUMERICHOST) != 0) address = NULL;

    return address;
}


void cvt32BitsIntTo4Bytes(int num, unsigned char *bytep) {
	*(bytep+3) = num & 0xff;
    num = num >> 8;
    *(bytep+2) = num & 0xff;
    num = num >> 8;
    *(bytep+1) = num & 0xff;
    num = num >> 8;
    *bytep = num & 0xff;
    return;
}


#define PACKET_LEN (20+(2*512)) 
BYTE macbuf[6] = { 0xfa, 0xaf, 0xb0, 0x0b, 0xd0, 0x0d };

char *HelpMsg = "\nusage: radioEmulator [-iqlr] [n]\n\nStreams packets emulating HPSDR datastream. [n] indicates interface to beacon on, omit n to get a list of interfaces.\n-iqlr indicates to beacon in PC -> device mode.\n\n"; 

void doHelp() { 
	printf(HelpMsg); 
	printf(wjt_Copyright); 
} 

int main(int argc, char *argv[]) {
    pcap_if_t *alldevs;
    pcap_if_t *d;
    int i=0;
    char errbuf[PCAP_ERRBUF_SIZE];
	int devidx = -1;
	char inbuf[200]; 
	char namebuf[PCAP_BUF_SIZE]; 
	char descbuf[PCAP_BUF_SIZE]; 
	int iqlr_mode = 0; 
	int pkts_sec; 
	int per_cycle_pkt_count; 
	int sleep_time; 
	


	pcap_t *fp; 
	int rc;
	unsigned int packet_num = 0; 	
	unsigned char packet[PACKET_LEN]; 
	__int64 perf_freq; 


	perf_freq = getPerfFreq(); 
#if 0 
	printf("Perf freq: %I64d\n", perf_freq); 
#endif 



	if ( argc >= 2 ) { 
		char *lcasearg;
		lcasearg = _strdup(argv[1]); 
		if ( lcasearg != NULL ) { 
			_strlwr(lcasearg); 
			if (   strstr(lcasearg, "?") != 0 ||  strstr(lcasearg, "help") != 0 ) { 
				doHelp(); 
				exit(99); 
			}
   			/* we've got some args - deal with them */
			if ( strcmp(lcasearg, "-iqlr") == 0 ) {  
				iqlr_mode = 1; 
				if ( argc >= 3 ) { 
					devidx = atoi(argv[2]); 
				} 			
			} 
			else { 
				devidx = atoi(argv[1]); 
			} 
			free(lcasearg); 
		}		
	} 

  

    /* Retrieve the device list from the local machine */
    if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL /* auth is not needed */, &alldevs, errbuf) == -1)
    {
        fprintf(stderr,"Error in pcap_findalldevs_ex: %s\n", errbuf);
        exit(1);
    }
    
    /* Print the list */
    for(d= alldevs; d != NULL; d= d->next)
    {
		++i;
		if ( devidx <= 0  ) {  /* no need to list interfaces if we have idx to use on command line */ 
			printf("%d. %s", i, d->name);
			if (d->description)  printf(" (%s)\n", d->description);
			else printf(" (No description available)\n");
		}
		/* ifprint(d);  */ 
    }
    
    if (i == 0)
    {
        printf("\nNo interfaces found! Make sure WinPcap is installed.\n");
        return;
    }
	
	if ( devidx >= 1  ) {  /* devidx given on command line */ 		    			
			;
	}
	else if ( i != 1 ) { 	
		printf("Enter number of interface to beacon on: "); fflush(stdout); 
		gets(inbuf); 
		devidx = atoi(inbuf); 
	} 
	else { 
		devidx = 1;
	}
	i = 0; 
	namebuf[0] = 0; 
	for ( d = alldevs;  d != NULL; d = d->next ) { 
		++i; 
		if ( devidx == i ) { 
			strncpy(namebuf, d->name, sizeof(namebuf)); 
			strncpy(descbuf, d->description, sizeof(descbuf)); 
			break; 
		} 
	} 

    /* We don't need any more the device list. Free it */
    pcap_freealldevs(alldevs);

	if ( namebuf[0] == 0 ) {  /* did not find device */ 
		printf("did not find device @ >%s<\n", inbuf); 
		return 8;
	} 

	printf("Using: %s (%s)\n", namebuf, descbuf); 


	/* if we get here namebuf has the name of the pcapbuf we want to play with */ 
	fp = pcap_open(namebuf, 
		           2048, /* snaplen */ 
				   0,  /* flags */
				   1000, /* read timeout */ 
				   NULL, /* authentication */ 
				   errbuf); 
	if ( fp == NULL ) { 
		printf("pcap_open of \'%s\' failed.\n", namebuf); 
		exit(97); 
	} 

	/* target mac addr */ 
	for ( i = 0; i < 6; i++ ) { 
		packet[i] = 0xff; 
	} 
	/* src mac addr */ 
	for ( i = 6; i < 12; i++ ) { 
		packet[i] = macbuf[i-6]; 
	} 
	packet[12] = 0xef; /* set ethertype to 0xeffe */ 
	packet[13] = 0xfe; 

	packet[14] = 0x01; /* subtype 1 */ 
	packet[15] = 0x04; /* ep 4 */ 

	if ( iqlr_mode ) { 
		packet[15] = 0x02; /* ep 4 */ 
		memcpy(packet+20, iqlr_pktData, 512); 
		memcpy(packet+20+512, iqlr_pktData, 512); 
		packet[24] = 0x58; /* 0101 1000 -- 0 - Janus Mic, 10 = Merc, 1 = 122.88 from Merc, 10 = Merc 10 Mhz, 00 - 48 khz */ 
		packet[512+23] = 2;  
		packet[512+25] = 0xd5; 
		packet[512+26] = 0xc6; 
		packet[512+27] = 0x90; 
	} 
	else { 
		packet[15] = 0x04; /* ep 4 */ 
		memcpy(packet+20, pktData, 512); 
		memcpy(packet+20+512, pktData, 512); 
	} 

	if ( iqlr_mode ) { 
		pkts_sec = 380; 
		per_cycle_pkt_count = 4; 
		sleep_time = 10; 
	}
	else { 
		pkts_sec = 1500; 
		per_cycle_pkt_count = 6; 
		sleep_time = 4; 
	} 

	printf("Beaconing %d packets/sec ... ctrl-c to stop...", pkts_sec); fflush(stdout); 

	while ( 1 ) { 
		int j; 
		for ( j = 0; j < per_cycle_pkt_count; j++ ) { 
			// memcpy(packet + 16, &packet_num, 4);		
			cvt32BitsIntTo4Bytes(packet_num, packet + 16);
			rc = pcap_sendpacket(fp, packet, PACKET_LEN); 
			if ( rc != 0 ) { 
				printf("i=%d, pcap_sendpacket failed: %s\n", i, pcap_geterr(fp));
				break;
			} 		
			++packet_num; 		
		}
		Sleep(sleep_time); 
		if ( iqlr_mode ) { 
			if ( sleep_time == 10 ) { 
				sleep_time = 11; 
			} 
			else { 
				sleep_time = 10; 
			} 	 
		} 
	}
	pcap_close(fp);
}
