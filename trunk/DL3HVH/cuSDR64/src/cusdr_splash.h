/**
* @file  cusdr_splash.h
* @brief splash screen header file for cuSDR
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

#ifndef _CUSDR_SLASHSCREEN_H
#define _CUSDR_SLASHSCREEN_H
 
#include <QFrame>
 
class CSplashScreen : public QFrame {

public:
	CSplashScreen(const QPixmap& pixmap);
	
	void clearMessage();
	void showMessage(const QString& theMessage, int theAlignment = Qt::AlignLeft, const QColor& theColor = Qt::black);
 
private:
	virtual void paintEvent(QPaintEvent* pe);
	
	QPixmap  itsPixmap;
	QString  itsMessage;
	int   itsAlignment;
	QColor  itsColor;
};
 
 
#endif // _CUSDR_SLASHSCREEN_H