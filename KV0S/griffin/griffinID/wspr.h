/**
* \file wspr.h
* \brief Header files for the WSPR functions as part of the Griffin ID program
* \author David R. Larsen, KV0S
* \version 1.0.0
* \date August 14, 2011
*/



/* Copyright (C) 2011 - David R. Larsen, KV0S
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



#ifndef WSPR_H
#define WSPR_H

#include <QString>
#include <QHash>
#include <QList>
#include <QFileDialog>
#include <QFile>
#include <string.h>

/**
 *  \brief the WSPR class for the griffinID program
 *
 *  These fuction encode the text string to transmit the WSPR beacon.  This is used to encode your
 *  output beacon using the openhpsdr.org : griffin board
 *
 *  for more informations on WSPR see  Joe Taylor's site http://physics.princeton.edu/pulsar/K1JT/
 *  also in coding these function I use the nice summary bu Andy Talbot G4JNT description
 *  at  http://www.g4jnt.com/JTModesBcns.htm
 *
 */


class WSPR
{

public:
    WSPR();
    void packing(char * message, char* loc, int dbm );
    void convolution();
    void interleave();
    char* getSymbol();
    void deg2grid( double dlat, double dlong, int len );
    void grid2deg( char* gridsq, int len );
    char* getGrid();
    float getLat();
    float getLong();
    int binaryByteLookup( QString byte );


private:
    long* poly;
    char symbol[176];
    char packed[11];
    char symbol2[162];
    char grid[6];
    double dlong;
    double dlat;
    QHash <QString, int> byte;
    void loadByte();

};

#endif // WSPR_H
