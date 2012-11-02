/**
* @file  cusdr_fonts.h
* @brief Fonts header for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-11-01
*/

/*   
 *   Copyright (C) 2012 Hermann von Hasseln, DL3HVH
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Library General Public License version 2 as
 *   published by the Free Software Foundation
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details
 *
 *   You should have received a copy of the GNU Library General Public
 *   License along with this program; if not, write to the
 *   Free Software Foundation, Inc.,
 *   51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

#ifndef _CUSDR_FONTS_H
#define	_CUSDR_FONTS_H

#include <QObject>
#include <QFont>
#include <QFontMetrics>


typedef struct _fonts {

	QFont	tinyFont;
	QFont	smallFont;
	QFont	normalFont;
	QFont	bigFont1;
	QFont	bigFont2;
	QFont	freqFont1;
	QFont	freqFont2;
	QFont	impactFont;
	QFont	hugeFont;

	QFontMetrics 	*tinyFontMetrics;
	QFontMetrics 	*smallFontMetrics;
	QFontMetrics 	*normalFontMetrics;
	QFontMetrics 	*bigFont1Metrics;
	QFontMetrics 	*bigFont2Metrics;
	QFontMetrics 	*freqFont1Metrics;
	QFontMetrics 	*freqFont2Metrics;
	QFontMetrics 	*impactFontMetrics;
	QFontMetrics 	*hugeFontMetrics;

	int		fontHeightTinyFont;
	int		fontHeightSmallFont;
	int		fontHeightNormalFont;
	int		fontHeightBigFont1;
	int		fontHeightBigFont2;
	int		fontHeightFreqFont1;
	int		fontHeightFreqFont2;
	int		fontHeightImpactFont;
	int		fontHeightHugeFont;

} TFonts;

class CFonts : public QObject {

	Q_OBJECT

public:
	CFonts(QObject *parent = 0);
    ~CFonts();

    TFonts getFonts()	{ return m_fonts; }

public slots:

private:
	TFonts			m_fonts;

};

#endif	// _CUSDR_FONTS_H
