/**
* @file  cusdr_alexAntennaWidget.cpp
* @brief Alexiares antenna settings widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-06-09
*/

/*
 *   
 *   Copyright 2012 Hermann von Hasseln, DL3HVH
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

#define LOG_ALEX_WIDGET


#include <QtGui>
#include <QPen>
#include <QDebug>
#include <QScopedPointer>

#include "cusdr_alexAntennaWidget.h"

#define	btn_height		15
#define	btn_width		16//20
#define	btn_width2		24//28
#define	btn_width3		45

AlexAntennaWidget::AlexAntennaWidget(QWidget *parent)
	: QWidget(parent)
	, set(Settings::instance())
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(0)
	, m_numberOfBands(12)
{
	setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(2, 2, 12, 0);
	setMouseTracking(true);

	fonts = new CFonts(this);
	m_fonts = fonts->getFonts();

//	//QFont titleFont;
//	m_normalFont.setStyleStrategy(QFont::PreferQuality);
//	m_normalFont.setFixedPitch(true);
//	#ifdef Q_OS_MAC
//		m_titleFont.setPixelSize(11);
//		m_titleFont.setFamily("Arial");
//		//m_smallFont.setBold(true);
//	#endif
//	#ifdef Q_OS_WIN
//		m_normalFont.setPixelSize(11);
//		m_normalFont.setFamily("Arial");
//		//m_smallFont.setBold(true);
//		//m_smallFont.setItalic(true);
//	#endif

//	m_smallFont.setStyleStrategy(QFont::PreferAntialias);
//	m_smallFont.setFixedPitch(true);
//	#ifdef Q_OS_MAC
//		m_smallFont.setPixelSize(9);
//		m_smallFont.setFamily("Arial");
//		//m_smallFont.setBold(true);
//	#endif
//	#ifdef Q_OS_WIN
//		m_smallFont.setPixelSize(9);
//		m_smallFont.setFamily("Arial");
//		//m_smallFont.setBold(true);
//		//m_smallFont.setItalic(true);
//	#endif

	createAntennasGroup();

	// set main layout
	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(0);
	hbox1->setContentsMargins(4, 0, 4, 0);
	hbox1->addWidget(antennaGroup);

	mainLayout->addLayout(hbox1);
	mainLayout->addStretch();
		
	setLayout(mainLayout);

	setAlexValues();
	setupConnections();
}

AlexAntennaWidget::~AlexAntennaWidget() {

	// disconnect all signals
	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

void AlexAntennaWidget::setupConnections() {

	/*
	CHECKED_CONNECT(
		set,
		SIGNAL(alexStateChanged(int, int)),
		this,
		SLOT(alexStateChanged(int, int)));
*/
}

void AlexAntennaWidget::createAntennasGroup() {

	QLabel *emptyLabel1 = new QLabel("  ", this);
    emptyLabel1->setFrameStyle(QFrame::Box | QFrame::Raised);
	emptyLabel1->setStyleSheet(set->getLabelStyle());

	QLabel *emptyLabel2 = new QLabel(" ", this);
    emptyLabel2->setFrameStyle(QFrame::Box | QFrame::Raised);
	emptyLabel2->setStyleSheet(set->getLabelStyle());

	QLabel *emptyLabel3 = new QLabel(" ", this);
	emptyLabel3->setFrameStyle(QFrame::Box | QFrame::Raised);
	emptyLabel3->setStyleSheet(set->getLabelStyle());

	QLabel *antRxLabel = new QLabel("Rx Ant", this);
	antRxLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	antRxLabel->setStyleSheet(set->getLabelStyle());

	QLabel *rxAuxLabel = new QLabel("Rx Aux", this);
    rxAuxLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	rxAuxLabel->setStyleSheet(set->getLabelStyle());

	QLabel *antTxLabel = new QLabel("Tx Ant", this);
	antTxLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	antTxLabel->setStyleSheet(set->getLabelStyle());

	// band label
	QStringList bandNames;
	bandNames << "160 m" << "80 m" << "60 m" << "40 m" << "30 m" << "20 m" << "17 m" << "15 m" << "12 m" << "10 m" << "6 m" << "gen";

	QList<QLabel *> bandLabelList;

	for (int i = 0; i < m_numberOfBands; i++) {

		QLabel *label = new QLabel(bandNames.at(i), this);
		label->setFrameStyle(QFrame::Box | QFrame::Raised);
		label->setStyleSheet(set->getLabelStyle());

		bandLabelList << label;
	}


	// antennas 1,2, and 3 button lists
	for (int i = 0; i < m_numberOfBands; i++) {

		AeroButton *btn = new AeroButton("1", this);
		btn->setRoundness(0);
		btn->setFixedSize (btn_width, btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());

		antenna1BtnList << btn;

		CHECKED_CONNECT(btn, SIGNAL(clicked()), this, SLOT(rxAntBtnClicked()));

		btn = new AeroButton("2", this);
		btn->setRoundness(0);
		btn->setFixedSize (btn_width, btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());

		antenna2BtnList << btn;

		CHECKED_CONNECT(btn, SIGNAL(clicked()), this, SLOT(rxAntBtnClicked()));

		btn = new AeroButton("3", this);
		btn->setRoundness(0);
		btn->setFixedSize (btn_width, btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());

		antenna3BtnList << btn;

		CHECKED_CONNECT(btn, SIGNAL(clicked()), this, SLOT(rxAntBtnClicked()));
	}

	for (int i = 0; i < m_numberOfBands; i++) {

		QList<AeroButton *> btnList;
		btnList << antenna1BtnList.at(i) << antenna2BtnList.at(i) << antenna3BtnList.at(i);

		bandBtnMatrix << btnList;
	}

	// Rx Aux 1,2, and XV button lists
	for (int i = 0; i < m_numberOfBands; i++) {

		AeroButton *btn = new AeroButton("1", this);
		btn->setRoundness(0);
		btn->setFixedSize (btn_width, btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());

		rx1BtnList << btn;

		CHECKED_CONNECT(btn, SIGNAL(clicked()), this, SLOT(rxAuxBtnClicked()));

		btn = new AeroButton("2", this);
		btn->setRoundness(0);
		btn->setFixedSize (btn_width, btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());

		rx2BtnList << btn;

		CHECKED_CONNECT(btn, SIGNAL(clicked()), this, SLOT(rxAuxBtnClicked()));

		btn = new AeroButton("XV", this);
		btn->setRoundness(0);
		btn->setFixedSize (btn_width, btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());

		xvBtnList << btn;

		CHECKED_CONNECT(btn, SIGNAL(clicked()), this, SLOT(rxAuxBtnClicked()));
	}

	for (int i = 0; i < m_numberOfBands; i++) {

		QList<AeroButton *> btnList;
		btnList << rx1BtnList.at(i) << rx2BtnList.at(i) << xvBtnList.at(i);

		bandBtnRxMatrix << btnList;
	}

	// Tx 1,2, and 3 button lists
	for (int i = 0; i < m_numberOfBands; i++) {

		AeroButton *btn = new AeroButton("1", this);
		btn->setRoundness(0);
		btn->setFixedSize (btn_width, btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());

		tx1BtnList << btn;

		CHECKED_CONNECT(btn, SIGNAL(clicked()), this, SLOT(txAntBtnClicked()));

		btn = new AeroButton("2", this);
		btn->setRoundness(0);
		btn->setFixedSize (btn_width, btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());

		tx2BtnList << btn;

		CHECKED_CONNECT(btn, SIGNAL(clicked()), this, SLOT(txAntBtnClicked()));

		btn = new AeroButton("3", this);
		btn->setRoundness(0);
		btn->setFixedSize (btn_width, btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());

		tx3BtnList << btn;

		CHECKED_CONNECT(btn, SIGNAL(clicked()), this, SLOT(txAntBtnClicked()));
	}

	for (int i = 0; i < m_numberOfBands; i++) {

		QList<AeroButton *> btnList;
		btnList << tx1BtnList.at(i) << tx2BtnList.at(i) << tx3BtnList.at(i);

		bandBtnTxMatrix << btnList;
	}

	QGridLayout* grid = new QGridLayout(this);
	grid->setVerticalSpacing(1);
	grid->setHorizontalSpacing(1);
	grid->setContentsMargins(3, 7, 5, 7);

	grid->addWidget(emptyLabel1, 0, 0, 1, 2, Qt::AlignLeft);
	grid->addWidget(antRxLabel, 0, 2, 1, 3, Qt::AlignCenter);
	grid->addWidget(emptyLabel2, 0, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(rxAuxLabel, 0, 6, 1, 3, Qt::AlignCenter);
	grid->addWidget(emptyLabel3, 0, 9, 1, 1, Qt::AlignCenter);
	grid->addWidget(antTxLabel, 0, 10, 1, 3, Qt::AlignCenter);


	for (int i = 0; i < m_numberOfBands; i++) {

		grid->addWidget(bandLabelList.at(i),   i+1, 0, 1, 2, Qt::AlignLeft);
		grid->addWidget(antenna1BtnList.at(i), i+1, 2, 1, 1, Qt::AlignCenter);
		grid->addWidget(antenna2BtnList.at(i), i+1, 3, 1, 1, Qt::AlignCenter);
		grid->addWidget(antenna3BtnList.at(i), i+1, 4, 1, 1, Qt::AlignCenter);
		grid->addWidget(emptyLabel2,            i+1, 5, 1, 1, Qt::AlignCenter);
		grid->addWidget(rx1BtnList.at(i),      i+1, 6, 1, 1, Qt::AlignCenter);
		grid->addWidget(rx2BtnList.at(i),      i+1, 7, 1, 1, Qt::AlignCenter);
		grid->addWidget(xvBtnList.at(i),       i+1, 8, 1, 1, Qt::AlignCenter);
		grid->addWidget(emptyLabel3,            i+1, 9, 1, 1, Qt::AlignCenter);
		grid->addWidget(tx1BtnList.at(i),      i+1, 10, 1, 1, Qt::AlignCenter);
		grid->addWidget(tx2BtnList.at(i),      i+1, 11, 1, 1, Qt::AlignCenter);
		grid->addWidget(tx3BtnList.at(i),      i+1, 12, 1, 1, Qt::AlignCenter);
	}

	antennaGroup = new QGroupBox(tr(""), this);
	antennaGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	antennaGroup->setLayout(grid);
	antennaGroup->setStyleSheet(set->getWidgetStyle());
	antennaGroup->setFont(QFont("Arial", 8));
}

//*****************
void AlexAntennaWidget::rxAntBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());

	int btnHit;
	int antenna;
	int btnHit1 = antenna1BtnList.indexOf(button);
	int btnHit2 = antenna2BtnList.indexOf(button);
	int btnHit3 = antenna3BtnList.indexOf(button);

	if (btnHit1 >= 0) {

		btnHit = btnHit1;
		antenna = 1;
	}
	else if (btnHit2 >= 0) {

		btnHit = btnHit2;
		antenna = 2;
	}
	else if (btnHit3 >= 0) {

		btnHit = btnHit3;
		antenna = 3;
	}
	else
		return;

	if (btnHit >= 0) {

		foreach(AeroButton *btn, bandBtnMatrix.at(btnHit)) {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
		button->setBtnState(AeroButton::ON);
		button->update();
	}

	m_alexStates[btnHit] &= 0x1FC; // 1 1 1 1 1 1 1 0 0
	m_alexStates[btnHit] |= antenna;

	set->setAlexState(this, btnHit, m_alexStates[btnHit]);
}

void AlexAntennaWidget::rxAuxBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());

	int btnHit;
	int aux;
	int btnHit1 = rx1BtnList.indexOf(button);
	int btnHit2 = rx2BtnList.indexOf(button);
	int btnHit3 = xvBtnList.indexOf(button);

	if (btnHit1 >= 0) {

		btnHit = btnHit1;
		aux = 1;
	}
	else if (btnHit2 >= 0) {

		btnHit = btnHit2;
		aux = 2;
	}
	else if (btnHit3 >= 0) {

		btnHit = btnHit3;
		aux = 3;
	}
	else
		return;

	if (button->btnState() == AeroButton::ON) {

		button->setBtnState(AeroButton::OFF);
		button->update();

		m_alexStates[btnHit] &= 0x1E3; // 1 1 1 1 0 0 0 1 1
	}
	else {

		if (btnHit >= 0) {

			foreach(AeroButton *btn, bandBtnRxMatrix.at(btnHit)) {

				btn->setBtnState(AeroButton::OFF);
				btn->update();
			}
			button->setBtnState(AeroButton::ON);
			button->update();
		}
		m_alexStates[btnHit] &= 0x1E3; // 1 1 1 1 0 0 0 1 1
		m_alexStates[btnHit] |= aux << 2;
	}
	set->setAlexState(this, btnHit, m_alexStates[btnHit]);
}

void AlexAntennaWidget::txAntBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());

	int btnHit;
	int antenna;
	int btnHit1 = tx1BtnList.indexOf(button);
	int btnHit2 = tx2BtnList.indexOf(button);
	int btnHit3 = tx3BtnList.indexOf(button);

	if (btnHit1 >= 0) {

		btnHit = btnHit1;
		antenna = 1;
	}
	else if (btnHit2 >= 0) {

		btnHit = btnHit2;
		antenna = 2;
	}
	else if (btnHit3 >= 0) {

		btnHit = btnHit3;
		antenna = 3;
	}
	else
		return;

	if (btnHit >= 0) {

		foreach(AeroButton *btn, bandBtnTxMatrix.at(btnHit)) {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
		button->setBtnState(AeroButton::ON);
		button->update();
	}

	m_alexStates[btnHit] &= 0x19F; // 1 1 0 0 1 1 1 1 1
	m_alexStates[btnHit] |= antenna << 5;

	set->setAlexState(this, btnHit, m_alexStates[btnHit]);
}

void AlexAntennaWidget::setAlexValues() {

	//m_alexConfiguration = set->getAlexConfiguration();
	m_alexConfig = set->getAlexConfig();
	m_alexStates = set->getAlexStates();//m_alexParameters.state;

	for (int i = 0; i < m_numberOfBands; i++) {

		int rxAnt = m_alexStates.at(i) & 0x03;
		if (rxAnt > 0)
			bandBtnMatrix.at(i).at(rxAnt-1)->setBtnState(AeroButton::ON);

		int rxAux = (m_alexStates.at(i) >> 2) & 0x07;
		if (rxAux > 0)
			bandBtnRxMatrix.at(i).at(rxAux-1)->setBtnState(AeroButton::ON);
		else {

			bandBtnRxMatrix.at(i).at(0)->setBtnState(AeroButton::OFF);
			bandBtnRxMatrix.at(i).at(1)->setBtnState(AeroButton::OFF);
			bandBtnRxMatrix.at(i).at(2)->setBtnState(AeroButton::OFF);
		}

		int txAnt = (m_alexStates.at(i) >> 5) & 0x03;
		if (txAnt > 0)
			bandBtnTxMatrix.at(i).at(txAnt-1)->setBtnState(AeroButton::ON);
	}
}
