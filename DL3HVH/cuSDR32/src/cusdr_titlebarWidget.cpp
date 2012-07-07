/**
* @file  cusdr_titlebarWidget.cpp
* @brief hpsdr server titlebar widget
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-09-21
*/

/*
 *   
 *   Copyright 2010 Hermann von Hasseln, DL3HVH
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

#include <QtGui>
#include <QDebug>

#include "cusdr_titlebarWidget.h"
#include "cusdr_settings.h"


TitleBarWidget::TitleBarWidget(QWidget *parent)
	: QWidget(parent)
	, m_mouseOver(false)
{
	setMouseTracking(true);
	
	setContentsMargins(0, 0, 0, 0);
	
	/*titleText = 
		QApplication::applicationName() + "   " + 
		QApplication::applicationVersion() + "    " + 
		Settings::instance()->callsign();*/

	m_appName = QApplication::applicationName();
	m_appVersion = QApplication::applicationVersion();
	m_callsign = Settings::instance()->callsign();
	
	m_appFont.setStyleStrategy(QFont::PreferAntialias);
	m_appFont.setFixedPitch(true);

	m_titleFont.setStyleStrategy(QFont::PreferAntialias);
	m_titleFont.setFixedPitch(true);
	
	#ifdef Q_OS_MAC
		m_appFont.setPixelSize(11);
		m_appFont.setFamily("Arial");
		m_appFont.setBold(true);
	#else if Q_OS_WIN
		m_appFont.setPixelSize(12);
		m_appFont.setFamily("Arial");
		m_appFont.setBold(true);
	#endif

	#ifdef Q_OS_MAC
		m_titleFont.setPixelSize(11);
		m_titleFont.setFamily("Arial");
		m_titleFont.setBold(true);
	#else if Q_OS_WIN
		m_titleFont.setPixelSize(10);
		m_titleFont.setFamily("Arial");
		m_titleFont.setBold(true);
	#endif

	QFontMetrics tfm(m_appFont);
	m_appNameFontWidth = tfm.width(m_appName);
	m_appNameFontHeight = tfm.height();
	m_titlebarHeight = m_appNameFontHeight + 12;

	QFontMetrics afm(m_titleFont);
	m_appVersionFontWidth = afm.width(m_appVersion);
	m_callsignFontWidth = afm.width(m_callsign);
	m_appVersionFontHeight = afm.height();

	//icon = QPixmap::fromImage(QImage(QLatin1String(":/img/icon.png")), Qt::ColorOnly);
	//m_icon = QPixmap::fromImage(QImage(QLatin1String(":/img/hpsdr5.ico")), Qt::ColorOnly);
	//m_icon = QPixmap::fromImage(QImage(QLatin1String(":/img/cusdr20x20.ico")), Qt::ColorOnly);
	m_icon = QPixmap::fromImage(QImage(QLatin1String(":/img/cusdr4.png")), Qt::ColorOnly);
	
	m_quitIcon.addFile(QString::fromUtf8(":/img/close_high.png"), QSize(), QIcon::Normal, QIcon::Off);
	m_minimizeIcon.addFile(QString::fromUtf8(":/img/minimize_high.png"), QSize(), QIcon::Normal, QIcon::Off);
	window.addFile(QString::fromUtf8(":/img/maximize_high.png"), QSize(), QIcon::Normal, QIcon::Off);
	m_hideIcon.addFile(QString::fromUtf8(":/img/hide_high.png"), QSize(), QIcon::Normal, QIcon::Off);
	
	closeBtn = new AeroButton(m_quitIcon, "", this);
	maxBtn = new AeroButton(window, "", this);
	hideBtn = new AeroButton(m_hideIcon, "", this);

	CHECKED_CONNECT(
		closeBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(mainWindowClose()));

	CHECKED_CONNECT(
		maxBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(maximizeMainWindow()));

	CHECKED_CONNECT(
		hideBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(hideTitlebar()));
}

TitleBarWidget::~TitleBarWidget() {

}

void TitleBarWidget::mainWindowClose() {

	emit mainClose();
}

void TitleBarWidget::maximizeMainWindow() {

	
	emit mainMaximize();
}

void TitleBarWidget::hideTitlebar() {

	emit mainMaximizeHideTitlebar();
}

QSize TitleBarWidget::minimumSizeHint() const {

	return QSize(width(), m_titlebarHeight);
}

QSize TitleBarWidget::sizeHint() const {

	return QSize(width(), m_titlebarHeight);
}

void TitleBarWidget::setPen(const QPen &pen) {

     this->pen = pen;
     update();
}

void TitleBarWidget::setBrush(const QBrush &brush) {

     this->brush = brush;
     update();
}

void TitleBarWidget::setAntialiased(bool antialiased) {

     this->antialiased = antialiased;
     update();
}

void TitleBarWidget::resizeEvent(QResizeEvent *event) {

	QWidget::resizeEvent(event);
}

void TitleBarWidget::enterEvent(QEvent *event) {

	m_mouseOver = true;
	QWidget::enterEvent(event);
}

void TitleBarWidget::leaveEvent(QEvent *event) {

	m_mouseOver = false;

	/*switch (region) {
		case TitleBar:
			break;

		case TitleText:
			break;

		case CloseIcon:
			closeIcon = m_quitIcon;
			update();
			break;

		case MaximizeIcon:
			break;

		case HideIcon:
			break;

		case HPSDRIcon:
			break;
	}*/
	QWidget::leaveEvent(event);
}

void TitleBarWidget::mouseMoveEvent(QMouseEvent *event) {

	QPoint mousePos = event->pos();
	
	if (mousePos.y() < 4)
		setCursor(Qt::SizeVerCursor);
	else
		setCursor(Qt::ArrowCursor);

	QWidget::mouseMoveEvent(event);
}

void TitleBarWidget::mousePressEvent(QMouseEvent *event) {

	//m_mouseDownPos = QCursor::pos();
	//m_mouseDownWindowPos = pos();
	
	QWidget::mousePressEvent(event);
}

void TitleBarWidget::mouseReleaseEvent(QMouseEvent *event) {

	//setCursor(Qt::OpenHandCursor);
	QWidget::mouseReleaseEvent(event);
}

void TitleBarWidget::paintEvent(QPaintEvent *) {

	QImage image = QImage(QSize(18, 18), QImage::Format_ARGB32_Premultiplied);
	if (image.isNull()) return;

	image.fill(QColor(0, 0, 0, 0).rgba());

	QPainter p(this);
	p.setRenderHints(QPainter::SmoothPixmapTransform | QPainter::Antialiasing | QPainter::TextAntialiasing, true);

	QRect titlebar_rect(0, 0, width(), m_titlebarHeight);

	QLinearGradient titlebarGrad(0, 0, 0, 1);
	titlebarGrad.setCoordinateMode(QGradient::ObjectBoundingMode);
	titlebarGrad.setSpread(QGradient::PadSpread);
	titlebarGrad.setColorAt(0, QColor(110, 110, 110));
	titlebarGrad.setColorAt(0.45, QColor(80, 80, 80));
	titlebarGrad.setColorAt(0.55, QColor(56, 56, 65));
	titlebarGrad.setColorAt(1, QColor(40, 40, 40));

	// draw background rect
	p.setPen(Qt::NoPen);
	p.setBrush(QBrush(titlebarGrad));
	p.drawRect(titlebar_rect);
	p.setPen(QColor(255, 255, 255, 140));
	p.drawLine(1, titlebar_rect.top(), width() - 2, titlebar_rect.top());
	p.setPen(QColor(255, 255, 255, 30));
	p.drawLine(1, titlebar_rect.bottom() - 2, width() - 2, titlebar_rect.bottom() - 2);
	p.setPen(QColor(0, 0, 0, 255));
	p.drawLine(0, titlebar_rect.bottom(), width(), titlebar_rect.bottom());

	// draw text
	p.setFont(m_appFont);
	p.setPen(Qt::black);

	// application name
	/*p.drawText(
		30, 6, 
		m_appNameFontWidth, m_appNameFontHeight, 
		Qt::TextSingleLine | Qt::TextDontClip | Qt::AlignVCenter | Qt::AlignLeft, 
		m_appName);

	p.setPen(QColor(255, 255, 255, 255));
	p.drawText(
		29, 5, 
		m_appNameFontWidth, m_appNameFontHeight, 
		Qt::TextSingleLine | Qt::TextDontClip | Qt::AlignVCenter | Qt::AlignLeft, 
		m_appName);*/

	// application version
	p.setFont(m_titleFont);
	p.setPen(Qt::black);
	p.drawText(
		//m_appNameFontWidth + 27, 7, 
		62, 7, 
		m_appVersionFontWidth, m_appVersionFontHeight, 
		Qt::TextSingleLine | Qt::TextDontClip | Qt::AlignVCenter | Qt::AlignLeft, 
		m_appVersion);

	p.setPen(QColor(255, 255, 255, 205));
	p.drawText(
		//m_appNameFontWidth + 28, 6, 
		63, 6, 
		m_appVersionFontWidth, m_appVersionFontHeight,
		Qt::TextSingleLine | Qt::TextDontClip | Qt::AlignVCenter | Qt::AlignLeft, 
		m_appVersion);

	// callsign
	p.setFont(m_titleFont);
	p.setPen(Qt::black);
	p.drawText(
		//m_appNameFontWidth + m_appNameFontWidth + 27, 6, 
		122, 6, 
		m_appVersionFontWidth, m_appVersionFontHeight, 
		Qt::TextSingleLine | Qt::TextDontClip | Qt::AlignVCenter | Qt::AlignLeft, 
		m_callsign);

	p.setPen(QColor(255, 255, 255, 205));
	p.drawText(
		//m_appNameFontWidth + m_appNameFontWidth + 26, 6, 
		123, 6, 
		m_appVersionFontWidth, m_appVersionFontHeight,
		Qt::TextSingleLine | Qt::TextDontClip | Qt::AlignVCenter | Qt::AlignLeft, 
		m_callsign);

	

	if (!m_icon.isNull())
		//p.drawTiledPixmap(5, 3, 17, 16, m_icon, 0, 0);
		//p.drawTiledPixmap(3, 3, 20, 20, m_icon, 0, 0);
		p.drawTiledPixmap(3, 0, 55, 25, m_icon, 0, 0);

	closeBtn->setFixedSize(15, 15);
	closeBtn->move(width() - 18, 5);

	maxBtn->setFixedSize(15, 15);
	maxBtn->move(width() - 36, 5);

	hideBtn->setFixedSize(15, 15);
	hideBtn->move(width() - 54, 5);

	p.end();
}
