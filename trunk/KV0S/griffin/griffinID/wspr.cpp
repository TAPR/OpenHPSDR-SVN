/**
* \file wspr.cpp
* \brief Code files for the WSPR functions as part of the Griffin ID program
* \author David R. Larsen
* \version 1.0.0
* \date August 14, 2011
*/



/* Copyright (C) 2011 - David R. Larsen
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/



#include "wspr.h"
#include <QDebug>

WSPR::WSPR()
{
    poly = new long;
    poly[0] = 0xf2d05351L;
    poly[1] = 0xe4613c47L;
    loadByte();

}

void WSPR::packing(char *c, char* loc, int dbm)
{
    unsigned long n1;
    n1=(c[0]>='0'&&c[0]<='9'?c[0]-'0':c[0]==' '?36:c[0]-'A'+10);
    n1=36*n1+(c[1]>='0'&&c[1]<='9'?c[1]-'0':c[1]==' '?36:c[1]-'A'+10);
    n1=10*n1+c[2]-'0';
    n1=27*n1+(c[3]==' '?26:c[3]-'A');
    n1=27*n1+(c[4]==' '?26:c[4]-'A');
    n1=27*n1+(c[5]==' '?26:c[5]-'A');

    //grid2deg( loc, 4 );
    //int ng = ( (dlong + 180) / 2 ) * 180 + (dlat + 90);
    //qDebug() << loc;
    int nloc1 = int(loc[0]) - int('A');
    int nloc2 = int(loc[1]) - int('A');
    int nloc3 = int(loc[2]) - int('0');
    int nloc4 = int(loc[3]) - int('0');
    int ng = (179 - 10 * nloc1 - nloc3) * 180 + 10 * nloc2 + nloc4;

    unsigned long n2 = ng * 128 + dbm + 64;

    //qDebug() << n1 << ng << n2 << dbm;

    //qDebug() << QString("%1").arg(n1,10,10) << QString("%1").arg(n2,10,10);
    QString n = QString("%1%2").arg(n1,28,2).arg(n2,22,2);
    //char* nn = (QString("%1%2").arg(n1,28,2).arg(n2,22,2)).toLocal8Bit().data();
    //qDebug() << nn;

    for( int i=0; i < 11; i++ )
    {
       int idx = i * 8;
       //qDebug() << n.mid(idx,8) << QString("%1").arg( binaryByteLookup( n.mid(idx,8)),2,16);
       packed[i] = binaryByteLookup( n.mid(idx,8) );
       //qDebug() << packed[i];
    }

}

void WSPR::convolution()
{
    int k = 0;
    int nstate = 0;
    int jmax = sizeof( packed );
    //qDebug() << "jmax" << jmax;
    for( int j=0; j!= sizeof( packed ); j++ )
    {
      for( int i=7; i>=0; i-- )
      {
         nstate = (nstate<<1) | ((packed[j]>>i)&1);
         for( int p=0; p<2; p++ )
         {
            //convolve
            unsigned long n = nstate & poly[p];
            // even := parity(n)
            int even = 0;
            while(n)
            {
               even = 1 - even;
               n = n & (n - 1);
            }
            //symb[k] = even;
            symbol[k] = even;
            k++;
            //qDebug() << "k "<< k;
         }
       }
    }
}

void WSPR::interleave()
{
    for(int i=0; i!=162; i++ )
    {
       // j0 := bit reversed_values_smaller_than_161[i]
       unsigned char j0;
       int p=-1;
       for(int k=0; p!=i; k++ )
       {
          for(int j=0; j!=8; j++ )   // j0:=bit_reverse(k)
             j0 = ((k>>j)&1) | (j0<<1);
          if(j0<162)
           p++;
       }
       symbol2[j0]=symbol[i]; //interleave
    }
    char npr3[162] = {
          1,1,0,0,0,0,0,0,1,0,0,0,1,1,1,0,0,0,1,0,0,1,0,1,1,1,1,0,0,0,0,0,
          0,0,1,0,0,1,0,1,0,0,0,0,0,0,1,0,1,1,0,0,1,1,0,1,0,0,0,1,1,0,1,0,
          0,0,0,1,1,0,1,0,1,0,1,0,1,0,0,1,0,0,1,0,1,1,0,0,0,1,1,0,1,0,1,0,
          0,0,1,0,0,0,0,0,1,0,0,1,0,0,1,1,1,0,1,1,0,0,1,1,0,1,0,0,0,1,1,1,
          0,0,0,0,0,1,0,1,0,0,1,1,0,0,0,0,0,0,0,1,1,0,1,0,1,1,0,0,0,1,1,0,
          0,0 };
    for(int i=0; i!=162; i++ )
    {
          //qDebug() << QString("symbol[%1]=%2").arg( i ).arg( symbol2[i],1,2);
          symbol2[i] = npr3[i] | symbol2[i]<<1;
    }

}

char* WSPR::getSymbol()
{
    return( symbol2 );
}

char* WSPR::getGrid()
{
    return( grid );
}

float WSPR::getLat()
{
    return( dlat );
}

float WSPR::getLong()
{
    return( dlong );
}

void WSPR::deg2grid( double dlat, double dlong, int len )
{
    char str[6];
    float nlong, nlat;

    // Convert to units of 5 min of longitude, working east from 180 deg.
    nlong = (60.0 * (180.0-dlong) / 5.0);
    int n1 = nlong / 240;
    int n2 = (nlong-240*n1)/24;
    int n3 = nlong-240*n1-24*n2;

    str[0] = char(65+n1);
    str[2] = char(48+n2);
    if( len > 4 )   //Select 4 or 6 character strings
    {
        str[4] = char(97+n3);
    }

    //  Convert to units of 2.5 min of latitude, working north from -90 deg.
    nlat = (60.0 * (dlat+90.0) / 2.5);
    int l1 = nlat / 240;
    int l2 = (nlat - 240 * l1) / 24;
    int l3 = nlat - 240 * l1 - 24 * l2;

    if( len > 4 )   //Select 4 or 6 character strings
    {
        str[1] = char(65+l1);
        str[3] = char(48+l2);
        str[5] = char(97+l3);
        strncpy( grid, str, 6 );
    }else{
        str[1] = char(65+l1);
        str[3] = char(48+l2);
        strncpy( grid, str, 4 );
    }
    //qDebug() << str;

 }

void WSPR::grid2deg( char* gridsq, int len )
{
    float nlong, n20d, xminlong;
    float nlat, xminlat;
    char grid2[6];
    strcpy(grid2,gridsq);
    int tupper1 = int('A');
    int tupper2 = int('a');
    int tupper = tupper1 - tupper2;

    if( len > 4 )
    {
       int val = int(grid2[5]);
       if( grid2[4] == ' ' || val < 64 || val > 128) grid2[4] = 'm';
       if( grid2[4] == ' ' || val < 64 || val > 128) grid2[5] = 'm';
       if( grid2[4] > 'A' && grid2[4] < 'Z') grid2[4] = grid[4] - tupper;
       if( grid2[5] > 'A' && grid2[5] < 'Z') grid2[5] = grid[5] - tupper;
    }
    if( grid2[0] > 'a' && grid2[0] < 'z') grid2[0] = gridsq[0] + tupper;
    if( grid2[1] > 'a' && grid2[1] < 'z') grid2[1] = gridsq[1] + tupper;

    nlong = 180 - 20 * (int(grid2[0]) -int('A'));
    n20d = 2 * (int(grid2[2]) -int('0'));
    if( len > 4 )
    {
       xminlong = 5 * (int(grid2[4]) - int('a'));
       dlong = nlong - n20d - xminlong/60.0;
    }else{
       dlong = nlong - n20d;
    }

    nlat = -90 + 10 * (int(grid2[1]) - int('A')) + int(grid2[3]) - int('0');
    if( len > 4 )
    {
       xminlat = 2.5 * (int(grid2[5]) - int('a') + 0.5);
       dlat = nlat + xminlat / 60.0;
       strncpy(grid,grid2,6);
    }else{
       dlat = nlat;
       strncpy(grid,grid2,4);
    }

    //qDebug() << grid << dlong << dlat;

}

void WSPR::loadByte()
{

    byte["00000000"] = 0x00;
    byte["00000001"] = 0x01;
    byte["00000010"] = 0x02;
    byte["00000011"] = 0x03;
    byte["00000100"] = 0x04;
    byte["00000101"] = 0x05;
    byte["00000110"] = 0x06;
    byte["00000111"] = 0x07;
    byte["00001000"] = 0x08;
    byte["00001001"] = 0x09;
    byte["00001010"] = 0x0a;
    byte["00001011"] = 0x0b;
    byte["00001100"] = 0x0c;
    byte["00001101"] = 0x0d;
    byte["00001110"] = 0x0e;
    byte["00001111"] = 0x0f;
    byte["00010000"] = 0x10;
    byte["00010001"] = 0x11;
    byte["00010010"] = 0x12;
    byte["00010011"] = 0x13;
    byte["00010100"] = 0x14;
    byte["00010101"] = 0x15;
    byte["00010110"] = 0x16;
    byte["00010111"] = 0x17;
    byte["00011000"] = 0x18;
    byte["00011001"] = 0x19;
    byte["00011010"] = 0x1a;
    byte["00011011"] = 0x1b;
    byte["00011100"] = 0x1c;
    byte["00011101"] = 0x1d;
    byte["00011110"] = 0x1e;
    byte["00011111"] = 0x1f;
    byte["00100000"] = 0x20;
    byte["00100001"] = 0x21;
    byte["00100010"] = 0x22;
    byte["00100011"] = 0x23;
    byte["00100100"] = 0x24;
    byte["00100101"] = 0x25;
    byte["00100110"] = 0x26;
    byte["00100111"] = 0x27;
    byte["00101000"] = 0x28;
    byte["00101001"] = 0x29;
    byte["00101010"] = 0x2a;
    byte["00101011"] = 0x2b;
    byte["00101100"] = 0x2c;
    byte["00101101"] = 0x2d;
    byte["00101110"] = 0x2e;
    byte["00101111"] = 0x2f;
    byte["00110000"] = 0x30;
    byte["00110001"] = 0x31;
    byte["00110010"] = 0x32;
    byte["00110011"] = 0x33;
    byte["00110100"] = 0x34;
    byte["00110101"] = 0x35;
    byte["00110110"] = 0x36;
    byte["00110111"] = 0x37;
    byte["00111000"] = 0x38;
    byte["00111001"] = 0x39;
    byte["00111010"] = 0x3a;
    byte["00111011"] = 0x3b;
    byte["00111100"] = 0x3c;
    byte["00111101"] = 0x3d;
    byte["00111110"] = 0x3e;
    byte["00111111"] = 0x3f;
    byte["01000000"] = 0x40;
    byte["01000001"] = 0x41;
    byte["01000010"] = 0x42;
    byte["01000011"] = 0x43;
    byte["01000100"] = 0x44;
    byte["01000101"] = 0x45;
    byte["01000110"] = 0x46;
    byte["01000111"] = 0x47;
    byte["01001000"] = 0x48;
    byte["01001001"] = 0x49;
    byte["01001010"] = 0x4a;
    byte["01001011"] = 0x4b;
    byte["01001100"] = 0x4c;
    byte["01001101"] = 0x4d;
    byte["01001110"] = 0x4e;
    byte["01001111"] = 0x4f;
    byte["01010000"] = 0x50;
    byte["01010001"] = 0x51;
    byte["01010010"] = 0x52;
    byte["01010011"] = 0x53;
    byte["01010100"] = 0x54;
    byte["01010101"] = 0x55;
    byte["01010110"] = 0x56;
    byte["01010111"] = 0x57;
    byte["01011000"] = 0x58;
    byte["01011001"] = 0x59;
    byte["01011010"] = 0x5a;
    byte["01011011"] = 0x5b;
    byte["01011100"] = 0x5c;
    byte["01011101"] = 0x5d;
    byte["01011110"] = 0x5e;
    byte["01011111"] = 0x5f;
    byte["01100000"] = 0x60;
    byte["01100001"] = 0x61;
    byte["01100010"] = 0x62;
    byte["01100011"] = 0x63;
    byte["01100100"] = 0x64;
    byte["01100101"] = 0x65;
    byte["01100110"] = 0x66;
    byte["01100111"] = 0x67;
    byte["01101000"] = 0x68;
    byte["01101001"] = 0x69;
    byte["01101010"] = 0x6a;
    byte["01101011"] = 0x6b;
    byte["01101100"] = 0x6c;
    byte["01101101"] = 0x6d;
    byte["01101110"] = 0x6e;
    byte["01101111"] = 0x6f;
    byte["01110010"] = 0x70;
    byte["01110001"] = 0x71;
    byte["01110010"] = 0x72;
    byte["01110011"] = 0x73;
    byte["01110100"] = 0x74;
    byte["01110101"] = 0x75;
    byte["01110110"] = 0x76;
    byte["01110111"] = 0x77;
    byte["01111000"] = 0x78;
    byte["01111001"] = 0x79;
    byte["01111010"] = 0x7a;
    byte["01111011"] = 0x7b;
    byte["01111100"] = 0x7c;
    byte["01111101"] = 0x7d;
    byte["01111110"] = 0x7e;
    byte["10000000"] = 0x80;
    byte["10000001"] = 0x81;
    byte["10000010"] = 0x82;
    byte["10000011"] = 0x83;
    byte["10000100"] = 0x84;
    byte["10000101"] = 0x85;
    byte["10000110"] = 0x86;
    byte["10000111"] = 0x87;
    byte["10001000"] = 0x88;
    byte["10001001"] = 0x89;
    byte["10001010"] = 0x8a;
    byte["10001011"] = 0x8b;
    byte["10001100"] = 0x8c;
    byte["10001101"] = 0x8d;
    byte["10001110"] = 0x8e;
    byte["10001111"] = 0x8f;
    byte["10010000"] = 0x90;
    byte["10010001"] = 0x91;
    byte["10010010"] = 0x92;
    byte["10010011"] = 0x93;
    byte["10010100"] = 0x94;
    byte["10010101"] = 0x95;
    byte["10010110"] = 0x96;
    byte["10010111"] = 0x97;
    byte["10011000"] = 0x98;
    byte["10011001"] = 0x99;
    byte["10011010"] = 0x9a;
    byte["10011011"] = 0x9b;
    byte["10011100"] = 0x9c;
    byte["10011101"] = 0x9d;
    byte["10011110"] = 0x9e;
    byte["10011111"] = 0x9f;
    byte["10100000"] = 0xa0;
    byte["10100001"] = 0xa1;
    byte["10100010"] = 0xa2;
    byte["10100011"] = 0xa3;
    byte["10100100"] = 0xa4;
    byte["10100101"] = 0xa5;
    byte["10100110"] = 0xa6;
    byte["10100111"] = 0xa7;
    byte["10101000"] = 0xa8;
    byte["10101001"] = 0xa9;
    byte["10101010"] = 0xaa;
    byte["10101011"] = 0xab;
    byte["10101100"] = 0xac;
    byte["10101101"] = 0xad;
    byte["10101110"] = 0xae;
    byte["10101111"] = 0xaf;
    byte["10110000"] = 0xb0;
    byte["10110001"] = 0xb1;
    byte["10110010"] = 0xb2;
    byte["10110011"] = 0xb3;
    byte["10110100"] = 0xb4;
    byte["10110101"] = 0xb5;
    byte["10110110"] = 0xb6;
    byte["10110111"] = 0xb7;
    byte["10111000"] = 0xb8;
    byte["10111001"] = 0xb9;
    byte["10111010"] = 0xba;
    byte["10111011"] = 0xbb;
    byte["10111100"] = 0xbc;
    byte["10111101"] = 0xbd;
    byte["10111110"] = 0xbe;
    byte["10111111"] = 0xbf;
    byte["11000000"] = 0xc0;
    byte["11000001"] = 0xc1;
    byte["11000010"] = 0xc2;
    byte["11000011"] = 0xc3;
    byte["11000100"] = 0xc4;
    byte["11000101"] = 0xc5;
    byte["11000110"] = 0xc6;
    byte["11000111"] = 0xc7;
    byte["11001000"] = 0xc8;
    byte["11001001"] = 0xc9;
    byte["11001010"] = 0xca;
    byte["11001011"] = 0xcb;
    byte["11001100"] = 0xcc;
    byte["11001101"] = 0xcd;
    byte["11001110"] = 0xce;
    byte["11001111"] = 0xcf;
    byte["11010000"] = 0xd0;
    byte["11010001"] = 0xd1;
    byte["11010010"] = 0xd2;
    byte["11010011"] = 0xd3;
    byte["11010100"] = 0xd4;
    byte["11010101"] = 0xd5;
    byte["11010110"] = 0xd6;
    byte["11010111"] = 0xd7;
    byte["11011000"] = 0xd8;
    byte["11011001"] = 0xd9;
    byte["11011010"] = 0xda;
    byte["11011011"] = 0xdb;
    byte["11011100"] = 0xdc;
    byte["11011101"] = 0xdd;
    byte["11011110"] = 0xde;
    byte["11011111"] = 0xdf;
    byte["11100000"] = 0xe0;
    byte["11100001"] = 0xe1;
    byte["11100010"] = 0xe2;
    byte["11100011"] = 0xe3;
    byte["11100100"] = 0xe4;
    byte["11100101"] = 0xe5;
    byte["11100110"] = 0xe6;
    byte["11100111"] = 0xe7;
    byte["11101000"] = 0xe8;
    byte["11101001"] = 0xe9;
    byte["11101010"] = 0xea;
    byte["11101011"] = 0xeb;
    byte["11101100"] = 0xec;
    byte["11101101"] = 0xed;
    byte["11101110"] = 0xee;
    byte["11101111"] = 0xef;
    byte["11110000"] = 0xf0;
    byte["11110001"] = 0xf1;
    byte["11110010"] = 0xf2;
    byte["11110011"] = 0xf3;
    byte["11110100"] = 0xf4;
    byte["11110101"] = 0xf5;
    byte["11110110"] = 0xf6;
    byte["11110111"] = 0xf7;
    byte["11111000"] = 0xf8;
    byte["11111001"] = 0xf9;
    byte["11111010"] = 0xfa;
    byte["11111011"] = 0xfb;
    byte["11111100"] = 0xfc;
    byte["11111101"] = 0xfd;
    byte["11111110"] = 0xfe;
    byte["11111111"] = 0xff;
    byte["0000"] = 0x0;
    byte["0001"] = 0x1;
    byte["0010"] = 0x2;
    byte["0011"] = 0x3;
    byte["0100"] = 0x4;
    byte["0101"] = 0x5;
    byte["0110"] = 0x6;
    byte["0111"] = 0x7;
    byte["1000"] = 0x8;
    byte["1001"] = 0x9;
    byte["1010"] = 0xa;
    byte["1011"] = 0xb;
    byte["1100"] = 0xc;
    byte["1101"] = 0xd;
    byte["1110"] = 0xe;
    byte["1111"] = 0xf;
    byte["00"] = 0x0;    // if one bit set least bit to 00
    byte["01"] = 0x4;
    byte["10"] = 0x8;
    byte["11"] = 0xc;
}

int WSPR::binaryByteLookup( QString bytestr )
{
    return( byte[bytestr] );
}



