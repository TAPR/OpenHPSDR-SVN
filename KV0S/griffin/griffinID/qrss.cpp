/**
* \file qrss.cpp
* \brief Code files for the QRSS functions as part of the Griffin ID program
* \author David R. Larsen
* \version 1.0.0
* \date August 14, 2011
*/



/* Copyright (C) 2010 - David R. Larsen
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



#include "qrss.h"

/**
 *  \brief the QRSS class for the griffinID program
 *
 *  load the QHash of the morse code symbols
 */

QRSS::QRSS()
{

    morse['A'] = "10111";
    morse['B'] = "111010101";
    morse['C'] = "11101011101";
    morse['D'] = "1110101";
    morse['E'] = "1";
    morse['F'] = "101011101";
    morse['G'] = "111011101";
    morse['H'] = "1010101";
    morse['I'] = "101";
    morse['J'] = "1011101110111";
    morse['K'] = "111010111";
    morse['L'] = "101110101";
    morse['M'] = "1110111";
    morse['N'] = "11101";
    morse['O'] = "11101110111";
    morse['P'] = "10111011101";
    morse['Q'] = "1110111010111";
    morse['R'] = "1011101";
    morse['S'] = "10101";
    morse['T'] = "111";
    morse['U'] = "1010111";
    morse['V'] = "101010111";
    morse['W'] = "101110111";
    morse['X'] = "11101010111";
    morse['Y'] = "11101110101";
    morse['Z'] = "11101110101";
    morse['1'] = "10111011101110111";
    morse['2'] = "101011101110111";
    morse['3'] = "1010101110111";
    morse['4'] = "10101010111";
    morse['5'] = "101010101";
    morse['6'] = "11101010101";
    morse['7'] = "1110111010101";
    morse['8'] = "111011101110101";
    morse['9'] = "11101110111011101";
    morse['0'] = "1110111011101110111";
    morse[' '] = "000";


}

/**
 *  \brief morsechar function
 *
 */


QString QRSS::morsechar( QChar ch )
{
    return( morse[ch] );
}

/**
 *  \brief encodeCW function
 *
 *
 */




QString QRSS::encodeCW( QString mess )
{
    message = mess;
    QString output;
    output = "";
    for (int i = 0; i < message.size(); ++i)
    {
        //qDebug() << message.at(i) << endl;
        QString value = (message.at(i)).toUpper();
        //qDebug() << value;
        for( int j = 0; j < value.size(); ++j )
        {
            //qDebug() << value[j];
            output.append( morsechar( value[j] ));
            output.append( morsechar( ' ' ) );
        }
    }
    output.append( morsechar( ' ' ) );

    //foreach( QChar c, output ){
    //    qDebug() << c << "=" << c.unicode() << "=" << QString( "%1" ).arg( c.unicode(), 0 , 2 );
    //}
    return(  output );
}

/**
 *  \brief diplayCW function
 *
 *
 */


QString QRSS::displayCW( QString message )
{
    QString display;
    for( int i = 0; i < message.size(); ++i )
    {
        if( message.at(i) == '1' )
        {
            display.append('#');
        }else{
            display.append(' ');
        }
    }
    return( display );
}

