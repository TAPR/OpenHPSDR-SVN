/********************************************************************************
** Form generated from reading UI file 'UI.ui'
**
** Created: Mon Aug 16 18:19:37 2010
**      by: Qt User Interface Compiler version 4.6.2
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_UI_H
#define UI_UI_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QComboBox>
#include <QtGui/QGroupBox>
#include <QtGui/QHeaderView>
#include <QtGui/QLabel>
#include <QtGui/QMainWindow>
#include <QtGui/QMenu>
#include <QtGui/QMenuBar>
#include <QtGui/QPushButton>
#include <QtGui/QSlider>
#include <QtGui/QSpinBox>
#include <QtGui/QStatusBar>
#include <QtGui/QWidget>
#include "Spectrum.h"
#include "Waterfall.h"

QT_BEGIN_NAMESPACE

class Ui_UI
{
public:
    QAction *action160;
    QAction *action80;
    QAction *action60;
    QAction *action40;
    QAction *action30;
    QAction *action20;
    QAction *action17;
    QAction *action15;
    QAction *action12;
    QAction *action10;
    QAction *action6;
    QAction *actionGen;
    QAction *actionWWV;
    QAction *actionLSB;
    QAction *actionUSB;
    QAction *actionDSB;
    QAction *actionCWL;
    QAction *actionCWU;
    QAction *actionAM;
    QAction *actionFMN;
    QAction *actionFilter_0;
    QAction *actionFilter_1;
    QAction *actionFilter_2;
    QAction *actionFilter_3;
    QAction *actionFilter_4;
    QAction *actionFilter_5;
    QAction *actionFilter_6;
    QAction *actionFilter_7;
    QAction *actionFilter_8;
    QAction *actionFilter_9;
    QAction *actionANF;
    QAction *actionNR;
    QAction *actionNB;
    QAction *actionSlow;
    QAction *actionMedium;
    QAction *actionFast;
    QAction *actionLong;
    QAction *actionSAM;
    QAction *actionDIGL;
    QAction *actionDIGU;
    QWidget *centralwidget;
    Spectrum *spectrumFrame;
    Waterfall *waterfallFrame;
    QGroupBox *groupBox;
    QComboBox *audioComboBox;
    QLabel *label;
    QSlider *afGainHorizontalSlider;
    QGroupBox *groupBox_2;
    QPushButton *subRxPushButton;
    QLabel *label_3;
    QSlider *subRxAfGainHorizontalSlider;
    QGroupBox *groupBox_3;
    QComboBox *serverHostComboBox;
    QLabel *label_5;
    QLabel *label_6;
    QSpinBox *serverRxSpinBox;
    QPushButton *serverConnectPushButton;
    QMenuBar *menubar;
    QMenu *menuBand;
    QMenu *menuMode;
    QMenu *menuFilter;
    QMenu *menuNoise_Reduction;
    QMenu *menuAGC;
    QStatusBar *statusbar;

    void setupUi(QMainWindow *UI)
    {
        if (UI->objectName().isEmpty())
            UI->setObjectName(QString::fromUtf8("UI"));
        UI->resize(963, 604);
        action160 = new QAction(UI);
        action160->setObjectName(QString::fromUtf8("action160"));
        action160->setCheckable(true);
        action80 = new QAction(UI);
        action80->setObjectName(QString::fromUtf8("action80"));
        action80->setCheckable(true);
        action60 = new QAction(UI);
        action60->setObjectName(QString::fromUtf8("action60"));
        action60->setCheckable(true);
        action40 = new QAction(UI);
        action40->setObjectName(QString::fromUtf8("action40"));
        action40->setCheckable(true);
        action30 = new QAction(UI);
        action30->setObjectName(QString::fromUtf8("action30"));
        action30->setCheckable(true);
        action20 = new QAction(UI);
        action20->setObjectName(QString::fromUtf8("action20"));
        action20->setCheckable(true);
        action17 = new QAction(UI);
        action17->setObjectName(QString::fromUtf8("action17"));
        action17->setCheckable(true);
        action15 = new QAction(UI);
        action15->setObjectName(QString::fromUtf8("action15"));
        action15->setCheckable(true);
        action12 = new QAction(UI);
        action12->setObjectName(QString::fromUtf8("action12"));
        action12->setCheckable(true);
        action10 = new QAction(UI);
        action10->setObjectName(QString::fromUtf8("action10"));
        action10->setCheckable(true);
        action6 = new QAction(UI);
        action6->setObjectName(QString::fromUtf8("action6"));
        action6->setCheckable(true);
        actionGen = new QAction(UI);
        actionGen->setObjectName(QString::fromUtf8("actionGen"));
        actionGen->setCheckable(true);
        actionWWV = new QAction(UI);
        actionWWV->setObjectName(QString::fromUtf8("actionWWV"));
        actionWWV->setCheckable(true);
        actionLSB = new QAction(UI);
        actionLSB->setObjectName(QString::fromUtf8("actionLSB"));
        actionLSB->setCheckable(true);
        actionUSB = new QAction(UI);
        actionUSB->setObjectName(QString::fromUtf8("actionUSB"));
        actionUSB->setCheckable(true);
        actionDSB = new QAction(UI);
        actionDSB->setObjectName(QString::fromUtf8("actionDSB"));
        actionDSB->setCheckable(true);
        actionCWL = new QAction(UI);
        actionCWL->setObjectName(QString::fromUtf8("actionCWL"));
        actionCWL->setCheckable(true);
        actionCWU = new QAction(UI);
        actionCWU->setObjectName(QString::fromUtf8("actionCWU"));
        actionCWU->setCheckable(true);
        actionAM = new QAction(UI);
        actionAM->setObjectName(QString::fromUtf8("actionAM"));
        actionAM->setCheckable(true);
        actionFMN = new QAction(UI);
        actionFMN->setObjectName(QString::fromUtf8("actionFMN"));
        actionFMN->setCheckable(true);
        actionFilter_0 = new QAction(UI);
        actionFilter_0->setObjectName(QString::fromUtf8("actionFilter_0"));
        actionFilter_0->setCheckable(true);
        actionFilter_1 = new QAction(UI);
        actionFilter_1->setObjectName(QString::fromUtf8("actionFilter_1"));
        actionFilter_1->setCheckable(true);
        actionFilter_2 = new QAction(UI);
        actionFilter_2->setObjectName(QString::fromUtf8("actionFilter_2"));
        actionFilter_2->setCheckable(true);
        actionFilter_3 = new QAction(UI);
        actionFilter_3->setObjectName(QString::fromUtf8("actionFilter_3"));
        actionFilter_3->setCheckable(true);
        actionFilter_4 = new QAction(UI);
        actionFilter_4->setObjectName(QString::fromUtf8("actionFilter_4"));
        actionFilter_4->setCheckable(true);
        actionFilter_5 = new QAction(UI);
        actionFilter_5->setObjectName(QString::fromUtf8("actionFilter_5"));
        actionFilter_5->setCheckable(true);
        actionFilter_6 = new QAction(UI);
        actionFilter_6->setObjectName(QString::fromUtf8("actionFilter_6"));
        actionFilter_6->setCheckable(true);
        actionFilter_7 = new QAction(UI);
        actionFilter_7->setObjectName(QString::fromUtf8("actionFilter_7"));
        actionFilter_7->setCheckable(true);
        actionFilter_8 = new QAction(UI);
        actionFilter_8->setObjectName(QString::fromUtf8("actionFilter_8"));
        actionFilter_8->setCheckable(true);
        actionFilter_9 = new QAction(UI);
        actionFilter_9->setObjectName(QString::fromUtf8("actionFilter_9"));
        actionFilter_9->setCheckable(true);
        actionANF = new QAction(UI);
        actionANF->setObjectName(QString::fromUtf8("actionANF"));
        actionNR = new QAction(UI);
        actionNR->setObjectName(QString::fromUtf8("actionNR"));
        actionNB = new QAction(UI);
        actionNB->setObjectName(QString::fromUtf8("actionNB"));
        actionSlow = new QAction(UI);
        actionSlow->setObjectName(QString::fromUtf8("actionSlow"));
        actionMedium = new QAction(UI);
        actionMedium->setObjectName(QString::fromUtf8("actionMedium"));
        actionFast = new QAction(UI);
        actionFast->setObjectName(QString::fromUtf8("actionFast"));
        actionLong = new QAction(UI);
        actionLong->setObjectName(QString::fromUtf8("actionLong"));
        actionSAM = new QAction(UI);
        actionSAM->setObjectName(QString::fromUtf8("actionSAM"));
        actionSAM->setCheckable(true);
        actionDIGL = new QAction(UI);
        actionDIGL->setObjectName(QString::fromUtf8("actionDIGL"));
        actionDIGL->setCheckable(true);
        actionDIGU = new QAction(UI);
        actionDIGU->setObjectName(QString::fromUtf8("actionDIGU"));
        actionDIGU->setCheckable(true);
        centralwidget = new QWidget(UI);
        centralwidget->setObjectName(QString::fromUtf8("centralwidget"));
        spectrumFrame = new Spectrum(centralwidget);
        spectrumFrame->setObjectName(QString::fromUtf8("spectrumFrame"));
        spectrumFrame->setGeometry(QRect(0, 0, 961, 231));
        spectrumFrame->setFrameShape(QFrame::StyledPanel);
        spectrumFrame->setFrameShadow(QFrame::Raised);
        waterfallFrame = new Waterfall(centralwidget);
        waterfallFrame->setObjectName(QString::fromUtf8("waterfallFrame"));
        waterfallFrame->setGeometry(QRect(0, 230, 961, 231));
        waterfallFrame->setFrameShape(QFrame::StyledPanel);
        waterfallFrame->setFrameShadow(QFrame::Raised);
        groupBox = new QGroupBox(centralwidget);
        groupBox->setObjectName(QString::fromUtf8("groupBox"));
        groupBox->setGeometry(QRect(0, 460, 231, 91));
        audioComboBox = new QComboBox(groupBox);
        audioComboBox->setObjectName(QString::fromUtf8("audioComboBox"));
        audioComboBox->setGeometry(QRect(60, 10, 161, 29));
        label = new QLabel(groupBox);
        label->setObjectName(QString::fromUtf8("label"));
        label->setGeometry(QRect(0, 50, 62, 17));
        afGainHorizontalSlider = new QSlider(groupBox);
        afGainHorizontalSlider->setObjectName(QString::fromUtf8("afGainHorizontalSlider"));
        afGainHorizontalSlider->setGeometry(QRect(60, 50, 160, 18));
        afGainHorizontalSlider->setValue(30);
        afGainHorizontalSlider->setOrientation(Qt::Horizontal);
        groupBox_2 = new QGroupBox(centralwidget);
        groupBox_2->setObjectName(QString::fromUtf8("groupBox_2"));
        groupBox_2->setGeometry(QRect(230, 460, 231, 91));
        subRxPushButton = new QPushButton(groupBox_2);
        subRxPushButton->setObjectName(QString::fromUtf8("subRxPushButton"));
        subRxPushButton->setGeometry(QRect(70, 10, 91, 27));
        label_3 = new QLabel(groupBox_2);
        label_3->setObjectName(QString::fromUtf8("label_3"));
        label_3->setGeometry(QRect(0, 50, 62, 17));
        subRxAfGainHorizontalSlider = new QSlider(groupBox_2);
        subRxAfGainHorizontalSlider->setObjectName(QString::fromUtf8("subRxAfGainHorizontalSlider"));
        subRxAfGainHorizontalSlider->setGeometry(QRect(60, 50, 160, 18));
        subRxAfGainHorizontalSlider->setValue(30);
        subRxAfGainHorizontalSlider->setOrientation(Qt::Horizontal);
        groupBox_3 = new QGroupBox(centralwidget);
        groupBox_3->setObjectName(QString::fromUtf8("groupBox_3"));
        groupBox_3->setGeometry(QRect(470, 460, 401, 101));
        serverHostComboBox = new QComboBox(groupBox_3);
        serverHostComboBox->setObjectName(QString::fromUtf8("serverHostComboBox"));
        serverHostComboBox->setGeometry(QRect(40, 50, 191, 21));
        serverHostComboBox->setEditable(true);
        label_5 = new QLabel(groupBox_3);
        label_5->setObjectName(QString::fromUtf8("label_5"));
        label_5->setGeometry(QRect(0, 50, 62, 17));
        label_6 = new QLabel(groupBox_3);
        label_6->setObjectName(QString::fromUtf8("label_6"));
        label_6->setGeometry(QRect(250, 50, 62, 17));
        serverRxSpinBox = new QSpinBox(groupBox_3);
        serverRxSpinBox->setObjectName(QString::fromUtf8("serverRxSpinBox"));
        serverRxSpinBox->setGeometry(QRect(310, 50, 50, 25));
        serverConnectPushButton = new QPushButton(groupBox_3);
        serverConnectPushButton->setObjectName(QString::fromUtf8("serverConnectPushButton"));
        serverConnectPushButton->setGeometry(QRect(70, 10, 91, 27));
        UI->setCentralWidget(centralwidget);
        menubar = new QMenuBar(UI);
        menubar->setObjectName(QString::fromUtf8("menubar"));
        menubar->setGeometry(QRect(0, 0, 963, 27));
        menuBand = new QMenu(menubar);
        menuBand->setObjectName(QString::fromUtf8("menuBand"));
        menuMode = new QMenu(menubar);
        menuMode->setObjectName(QString::fromUtf8("menuMode"));
        menuFilter = new QMenu(menubar);
        menuFilter->setObjectName(QString::fromUtf8("menuFilter"));
        menuNoise_Reduction = new QMenu(menubar);
        menuNoise_Reduction->setObjectName(QString::fromUtf8("menuNoise_Reduction"));
        menuAGC = new QMenu(menubar);
        menuAGC->setObjectName(QString::fromUtf8("menuAGC"));
        UI->setMenuBar(menubar);
        statusbar = new QStatusBar(UI);
        statusbar->setObjectName(QString::fromUtf8("statusbar"));
        UI->setStatusBar(statusbar);

        menubar->addAction(menuBand->menuAction());
        menubar->addAction(menuMode->menuAction());
        menubar->addAction(menuFilter->menuAction());
        menubar->addAction(menuNoise_Reduction->menuAction());
        menubar->addAction(menuAGC->menuAction());
        menuBand->addAction(action160);
        menuBand->addAction(action80);
        menuBand->addAction(action60);
        menuBand->addAction(action40);
        menuBand->addAction(action30);
        menuBand->addAction(action20);
        menuBand->addAction(action17);
        menuBand->addAction(action15);
        menuBand->addAction(action12);
        menuBand->addAction(action10);
        menuBand->addAction(action6);
        menuBand->addAction(actionGen);
        menuBand->addAction(actionWWV);
        menuMode->addAction(actionLSB);
        menuMode->addAction(actionUSB);
        menuMode->addAction(actionDSB);
        menuMode->addAction(actionCWU);
        menuMode->addAction(actionCWL);
        menuMode->addAction(actionAM);
        menuMode->addAction(actionSAM);
        menuMode->addAction(actionFMN);
        menuMode->addAction(actionDIGL);
        menuMode->addAction(actionDIGU);
        menuFilter->addAction(actionFilter_0);
        menuFilter->addAction(actionFilter_1);
        menuFilter->addAction(actionFilter_2);
        menuFilter->addAction(actionFilter_3);
        menuFilter->addAction(actionFilter_4);
        menuFilter->addAction(actionFilter_5);
        menuFilter->addAction(actionFilter_6);
        menuFilter->addAction(actionFilter_7);
        menuFilter->addAction(actionFilter_8);
        menuFilter->addAction(actionFilter_9);
        menuNoise_Reduction->addAction(actionANF);
        menuNoise_Reduction->addAction(actionNR);
        menuNoise_Reduction->addAction(actionNB);
        menuAGC->addAction(actionSlow);
        menuAGC->addAction(actionMedium);
        menuAGC->addAction(actionFast);
        menuAGC->addAction(actionLong);

        retranslateUi(UI);

        QMetaObject::connectSlotsByName(UI);
    } // setupUi

    void retranslateUi(QMainWindow *UI)
    {
        UI->setWindowTitle(QApplication::translate("UI", "QtRadio", 0, QApplication::UnicodeUTF8));
        action160->setText(QApplication::translate("UI", "160", 0, QApplication::UnicodeUTF8));
        action80->setText(QApplication::translate("UI", "80", 0, QApplication::UnicodeUTF8));
        action60->setText(QApplication::translate("UI", "60", 0, QApplication::UnicodeUTF8));
        action40->setText(QApplication::translate("UI", "40", 0, QApplication::UnicodeUTF8));
        action30->setText(QApplication::translate("UI", "30", 0, QApplication::UnicodeUTF8));
        action20->setText(QApplication::translate("UI", "20", 0, QApplication::UnicodeUTF8));
        action17->setText(QApplication::translate("UI", "17", 0, QApplication::UnicodeUTF8));
        action15->setText(QApplication::translate("UI", "15", 0, QApplication::UnicodeUTF8));
        action12->setText(QApplication::translate("UI", "12", 0, QApplication::UnicodeUTF8));
        action10->setText(QApplication::translate("UI", "10", 0, QApplication::UnicodeUTF8));
        action6->setText(QApplication::translate("UI", "6", 0, QApplication::UnicodeUTF8));
        actionGen->setText(QApplication::translate("UI", "Gen", 0, QApplication::UnicodeUTF8));
        actionWWV->setText(QApplication::translate("UI", "WWV", 0, QApplication::UnicodeUTF8));
        actionLSB->setText(QApplication::translate("UI", "LSB", 0, QApplication::UnicodeUTF8));
        actionUSB->setText(QApplication::translate("UI", "USB", 0, QApplication::UnicodeUTF8));
        actionDSB->setText(QApplication::translate("UI", "DSB", 0, QApplication::UnicodeUTF8));
        actionCWL->setText(QApplication::translate("UI", "CWL", 0, QApplication::UnicodeUTF8));
        actionCWU->setText(QApplication::translate("UI", "CWU", 0, QApplication::UnicodeUTF8));
        actionAM->setText(QApplication::translate("UI", "AM", 0, QApplication::UnicodeUTF8));
        actionFMN->setText(QApplication::translate("UI", "FMN", 0, QApplication::UnicodeUTF8));
        actionFilter_0->setText(QApplication::translate("UI", "Filter 0", 0, QApplication::UnicodeUTF8));
        actionFilter_1->setText(QApplication::translate("UI", "Filter 1", 0, QApplication::UnicodeUTF8));
        actionFilter_2->setText(QApplication::translate("UI", "Filter 2", 0, QApplication::UnicodeUTF8));
        actionFilter_3->setText(QApplication::translate("UI", "Filter 3", 0, QApplication::UnicodeUTF8));
        actionFilter_4->setText(QApplication::translate("UI", "Filter 4", 0, QApplication::UnicodeUTF8));
        actionFilter_5->setText(QApplication::translate("UI", "Filter 5", 0, QApplication::UnicodeUTF8));
        actionFilter_6->setText(QApplication::translate("UI", "Filter 6", 0, QApplication::UnicodeUTF8));
        actionFilter_7->setText(QApplication::translate("UI", "Filter 7", 0, QApplication::UnicodeUTF8));
        actionFilter_8->setText(QApplication::translate("UI", "Filter 8", 0, QApplication::UnicodeUTF8));
        actionFilter_9->setText(QApplication::translate("UI", "Filter 9", 0, QApplication::UnicodeUTF8));
        actionANF->setText(QApplication::translate("UI", "ANF", 0, QApplication::UnicodeUTF8));
        actionNR->setText(QApplication::translate("UI", "NR", 0, QApplication::UnicodeUTF8));
        actionNB->setText(QApplication::translate("UI", "NB", 0, QApplication::UnicodeUTF8));
        actionSlow->setText(QApplication::translate("UI", "Slow", 0, QApplication::UnicodeUTF8));
        actionMedium->setText(QApplication::translate("UI", "Medium", 0, QApplication::UnicodeUTF8));
        actionFast->setText(QApplication::translate("UI", "Fast", 0, QApplication::UnicodeUTF8));
        actionLong->setText(QApplication::translate("UI", "Long", 0, QApplication::UnicodeUTF8));
        actionSAM->setText(QApplication::translate("UI", "SAM", 0, QApplication::UnicodeUTF8));
        actionDIGL->setText(QApplication::translate("UI", "DIGL", 0, QApplication::UnicodeUTF8));
        actionDIGU->setText(QApplication::translate("UI", "DIGU", 0, QApplication::UnicodeUTF8));
        groupBox->setTitle(QApplication::translate("UI", "Audio:", 0, QApplication::UnicodeUTF8));
        label->setText(QApplication::translate("UI", "AF Gain", 0, QApplication::UnicodeUTF8));
        groupBox_2->setTitle(QApplication::translate("UI", "Sub RX", 0, QApplication::UnicodeUTF8));
        subRxPushButton->setText(QApplication::translate("UI", "Switch On", 0, QApplication::UnicodeUTF8));
        label_3->setText(QApplication::translate("UI", "AF Gain", 0, QApplication::UnicodeUTF8));
        groupBox_3->setTitle(QApplication::translate("UI", "Server", 0, QApplication::UnicodeUTF8));
        label_5->setText(QApplication::translate("UI", "Host", 0, QApplication::UnicodeUTF8));
        label_6->setText(QApplication::translate("UI", "Receiver", 0, QApplication::UnicodeUTF8));
        serverConnectPushButton->setText(QApplication::translate("UI", "Connect", 0, QApplication::UnicodeUTF8));
        menuBand->setTitle(QApplication::translate("UI", "Band", 0, QApplication::UnicodeUTF8));
        menuMode->setTitle(QApplication::translate("UI", "Mode", 0, QApplication::UnicodeUTF8));
        menuFilter->setTitle(QApplication::translate("UI", "Filter", 0, QApplication::UnicodeUTF8));
        menuNoise_Reduction->setTitle(QApplication::translate("UI", "Noise Reduction", 0, QApplication::UnicodeUTF8));
        menuAGC->setTitle(QApplication::translate("UI", "AGC", 0, QApplication::UnicodeUTF8));
    } // retranslateUi

};

namespace Ui {
    class UI: public Ui_UI {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_UI_H
