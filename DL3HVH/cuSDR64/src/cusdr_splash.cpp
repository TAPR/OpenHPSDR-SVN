/**
* @file  cusdr_splash.cpp
* @brief splash screen class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-05-21
*/

/*
 *	Taken from: Qt Developer Network; QSplashScreen replacement for semitransparent images
 *
 *	http://developer.qt.nokia.com/wiki/QSplashScreen_replacement_for_semitransparent_images
 *
 *  License: http://creativecommons.org/licenses/by-sa/2.5/legalcode
 */

#include <QPainter>
#include "cusdr_splash.h"
 
CSplashScreen::CSplashScreen(const QPixmap& thePixmap)
	//: QFrame(0, Qt::FramelessWindowHint|Qt::WindowStaysOnTopHint)
	: QFrame(0, Qt::FramelessWindowHint)
	, itsPixmap(thePixmap)
{
	setAttribute(Qt::WA_TranslucentBackground);
	setAttribute(Qt::WA_TransparentForMouseEvents);
	setFixedSize(itsPixmap.size());
};
  
void CSplashScreen::clearMessage() {
	
	itsMessage.clear();
	repaint();
}
 
void CSplashScreen::showMessage(const QString& theMessage, int theAlignment/* = Qt::AlignLeft*/, const QColor& theColor/* = Qt::black*/) {
	
	itsMessage  = theMessage;
	itsAlignment = theAlignment;
	itsColor  = theColor;
	repaint();
}
 
void CSplashScreen::paintEvent(QPaintEvent* pe) {
	
	Q_UNUSED(pe)

	QRect aTextRect(rect());
	//aTextRect.setRect(aTextRect.x() + 5, aTextRect.y() + 5, aTextRect.width() - 10, aTextRect.height() - 10);
	aTextRect.setRect(aTextRect.x() - 5, aTextRect.y() - 5, aTextRect.width() - 10, aTextRect.height() - 10);
	
	QPainter aPainter(this);
	aPainter.drawPixmap(rect(), itsPixmap);
	aPainter.setPen(itsColor);
	aPainter.drawText(aTextRect, itsAlignment, itsMessage);
}