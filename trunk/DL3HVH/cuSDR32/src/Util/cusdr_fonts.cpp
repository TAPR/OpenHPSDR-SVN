/**
* @file  cusdr_fonts.cpp
* @brief Fonts class for cuSDR
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

#include "cusdr_fonts.h"


CFonts::CFonts(QObject *parent)
	: QObject(parent)
{
	m_fonts.tinyFont.setStyleStrategy(QFont::PreferAntialias);
	m_fonts.tinyFont.setFixedPitch(true);
	#if defined(Q_OS_MAC)
		m_tinyFont.setPixelSize(8);
		m_tinyFont.setFamily("Arial");
		//#endif
	#elif defined(Q_OS_WIN32)
		m_fonts.tinyFont.setPixelSize(8);
		m_fonts.tinyFont.setFamily("Arial");
	#elif defined(Q_OS_LINUX)
		m_fonts.tinyFont.setPixelSize(7);
		m_fonts.tinyFont.setFamily("Arial");
	#endif

	m_fonts.tinyFontMetrics = new QFontMetrics(m_fonts.tinyFont);
	m_fonts.fontHeightTinyFont = m_fonts.tinyFontMetrics->tightBoundingRect("M").height();

	m_fonts.smallFont.setStyleStrategy(QFont::PreferAntialias);
	m_fonts.smallFont.setStyleHint(QFont::SansSerif, QFont::PreferAntialias);
	m_fonts.smallFont.setBold(true);
	m_fonts.smallFont.setFixedPitch(true);
	#if defined(Q_OS_MAC)
		m_fonts.smallFont.setPixelSize(10);
		m_fonts.smallFont.setFamily("Arial");
	#elif defined(Q_OS_WIN32)
		m_fonts.smallFont.setPixelSize(10);
		m_fonts.smallFont.setFamily("Arial");
	#elif defined(Q_OS_LINUX)
		m_fonts.smallFont.setPixelSize(9);
		m_fonts.smallFont.setFamily("Arial");
	#endif

	m_fonts.smallFontMetrics = new QFontMetrics(m_fonts.smallFont);
	m_fonts.fontHeightSmallFont = m_fonts.smallFontMetrics->tightBoundingRect("M").height();


	m_fonts.normalFont.setStyleStrategy(QFont::PreferAntialias);
	m_fonts.normalFont.setStyleHint(QFont::SansSerif, QFont::PreferAntialias);
	m_fonts.normalFont.setBold(true);
	m_fonts.normalFont.setFixedPitch(true);
	#if defined(Q_OS_MAC)
		m_fonts.normalFont.setPixelSize(11);
		m_fonts.normalFont.setFamily("Arial");
	#elif defined(Q_OS_WIN32)
		m_fonts.normalFont.setPixelSize(11);
		m_fonts.normalFont.setFamily("Arial");
	#elif defined(Q_OS_LINUX)
		m_fonts.normalFont.setPixelSize(10);
		m_fonts.normalFont.setFamily("Arial");
	#endif

	m_fonts.normalFontMetrics = new QFontMetrics(m_fonts.normalFont);
	m_fonts.fontHeightNormalFont = m_fonts.normalFontMetrics->tightBoundingRect("M").height();

	m_fonts.bigFont1.setStyleStrategy(QFont::PreferAntialias);
	m_fonts.bigFont1.setStyleHint(QFont::SansSerif, QFont::PreferAntialias);
	m_fonts.bigFont1.setBold(true);
	m_fonts.bigFont1.setFixedPitch(true);
	#if defined(Q_OS_MAC)
		m_fonts.bigFont1.setPixelSize(12);
		m_fonts.bigFont1.setFamily("Arial");
	#elif defined(Q_OS_WIN32)
		m_fonts.bigFont1.setPixelSize(12);
		m_fonts.bigFont1.setFamily("Arial");
	#elif defined(Q_OS_LINUX)
		m_fonts.bigFont1.setPixelSize(12);
		m_fonts.bigFont1.setFamily("Arial");
	#endif

	m_fonts.bigFont1Metrics = new QFontMetrics(m_fonts.bigFont1);
	m_fonts.fontHeightBigFont1 = m_fonts.bigFont1Metrics->tightBoundingRect("M").height();


	m_fonts.bigFont2.setStyleStrategy(QFont::PreferAntialias);
	m_fonts.bigFont2.setStyleHint(QFont::SansSerif, QFont::PreferAntialias);
	m_fonts.bigFont2.setBold(true);
	m_fonts.bigFont2.setFixedPitch(true);
	#if defined(Q_OS_MAC)
		m_fonts.bigFont2.setPixelSize(10);
		m_fonts.bigFont2.setFamily("Arial");
	#elif defined(Q_OS_WIN32)
		m_fonts.bigFont2.setPixelSize(10);
		m_fonts.bigFont2.setFamily("Arial");
	#elif defined(Q_OS_LINUX)
		m_fonts.bigFont2.setPixelSize(10);
		m_fonts.bigFont2.setFamily("Arial");
	#endif

	m_fonts.bigFont2Metrics = new QFontMetrics(m_fonts.bigFont2);
	m_fonts.fontHeightBigFont2 = m_fonts.bigFont2Metrics->tightBoundingRect("M").height();


	m_fonts.freqFont1.setStyleStrategy(QFont::PreferAntialias);
	m_fonts.freqFont1.setStyleHint(QFont::SansSerif, QFont::PreferAntialias);
	m_fonts.freqFont1.setBold(true);
	m_fonts.freqFont1.setFixedPitch(true);
	#if defined(Q_OS_MAC)
		m_fonts.freqFont1.setPixelSize(36);
		m_fonts.freqFont1.setFamily("Arial");
	#elif defined(Q_OS_WIN32)
		m_fonts.freqFont1.setPixelSize(36);
		m_fonts.freqFont1.setFamily("Arial");
	#elif defined(Q_OS_LINUX)
		m_fonts.freqFont1.setPixelSize(36);
		m_fonts.freqFont1.setFamily("Arial");
	#endif

	m_fonts.freqFont1Metrics = new QFontMetrics(m_fonts.freqFont1);
	m_fonts.fontHeightFreqFont1 = m_fonts.freqFont1Metrics->tightBoundingRect("M").height();


	m_fonts.freqFont2.setStyleStrategy(QFont::PreferAntialias);
	m_fonts.freqFont2.setStyleHint(QFont::SansSerif, QFont::PreferAntialias);
	m_fonts.freqFont2.setBold(true);
	m_fonts.freqFont2.setFixedPitch(true);
	#if defined(Q_OS_MAC)
		m_fonts.freqFont2.setPixelSize(24);
		m_fonts.freqFont2.setFamily("Arial");
	#elif defined(Q_OS_WIN32)
		m_fonts.freqFont2.setPixelSize(24);
		m_fonts.freqFont2.setFamily("Arial");
	#elif defined(Q_OS_LINUX)
		m_fonts.freqFont2.setPixelSize(24);
		m_fonts.freqFont2.setFamily("Arial");
	#endif

	m_fonts.freqFont2Metrics = new QFontMetrics(m_fonts.freqFont2);
	m_fonts.fontHeightFreqFont2 = m_fonts.freqFont2Metrics->tightBoundingRect("M").height();


	m_fonts.impactFont.setStyleStrategy(QFont::PreferAntialias);
	m_fonts.impactFont.setItalic(true);
	m_fonts.impactFont.setFixedPitch(true);
	#if defined(Q_OS_MAC)
		m_fonts.impactFont.setPixelSize(12);
		m_fonts.impactFont.setFamily("Impact");
	#elif defined(Q_OS_WIN32)
		m_fonts.impactFont.setPixelSize(12);
		m_fonts.impactFont.setFamily("Impact");
	#elif defined(Q_OS_LINUX)
		m_fonts.impactFont.setPixelSize(18);
		m_fonts.impactFont.setFamily("Impact");
	#endif

	m_fonts.impactFontMetrics = new QFontMetrics(m_fonts.impactFont);
	m_fonts.fontHeightImpactFont = m_fonts.impactFontMetrics->tightBoundingRect("M").height();

}

CFonts::~CFonts() {
	
}

