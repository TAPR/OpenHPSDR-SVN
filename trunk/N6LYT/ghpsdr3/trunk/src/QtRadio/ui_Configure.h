/********************************************************************************
** Form generated from reading UI file 'Configure.ui'
**
** Created: Wed Oct 13 08:34:35 2010
**      by: Qt User Interface Compiler version 4.6.2
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_CONFIGURE_H
#define UI_CONFIGURE_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QComboBox>
#include <QtGui/QDialog>
#include <QtGui/QDialogButtonBox>
#include <QtGui/QGroupBox>
#include <QtGui/QHeaderView>
#include <QtGui/QLabel>
#include <QtGui/QSpinBox>
#include <QtGui/QTabWidget>
#include <QtGui/QWidget>

QT_BEGIN_NAMESPACE

class Ui_Configure
{
public:
    QDialogButtonBox *buttonBox;
    QTabWidget *tabWidget;
    QWidget *Audio;
    QComboBox *audioDeviceComboBox;
    QLabel *label_8;
    QComboBox *sampleRateComboBox;
    QLabel *label_9;
    QLabel *label_10;
    QSpinBox *audioChannelsSpinBox;
    QLabel *label_11;
    QComboBox *encodingComboBox;
    QLabel *label_12;
    QComboBox *byteOrderComboBox;
    QWidget *Server;
    QLabel *label_6;
    QComboBox *hostComboBox;
    QLabel *label_7;
    QSpinBox *rxSpinBox;
    QWidget *Display;
    QGroupBox *groupBox;
    QLabel *label;
    QLabel *label_2;
    QSpinBox *spectrumHighSpinBox;
    QSpinBox *spectrumLowSpinBox;
    QGroupBox *groupBox_2;
    QLabel *label_3;
    QLabel *label_4;
    QSpinBox *waterfallHighSpinBox;
    QSpinBox *waterfallLowSpinBox;
    QSpinBox *fpsSpinBox;
    QLabel *label_5;
    QWidget *DSP;
    QGroupBox *groupBox_3;
    QLabel *label_13;
    QLabel *label_14;
    QLabel *label_15;
    QLabel *label_16;
    QSpinBox *nrTapsSpinBox;
    QSpinBox *nrDelaySpinBox;
    QSpinBox *nrGainSpinBox;
    QSpinBox *nrLeakSpinBox;
    QGroupBox *groupBox_4;
    QLabel *label_17;
    QLabel *label_18;
    QLabel *label_19;
    QLabel *label_20;
    QSpinBox *anfTapsSpinBox;
    QSpinBox *anfDelaySpinBox;
    QSpinBox *anfGainSpinBox;
    QSpinBox *anfLeakSpinBox;
    QGroupBox *groupBox_5;
    QLabel *label_23;
    QSpinBox *nbThresholdSpinBox;

    void setupUi(QDialog *Configure)
    {
        if (Configure->objectName().isEmpty())
            Configure->setObjectName(QString::fromUtf8("Configure"));
        Configure->resize(657, 399);
        buttonBox = new QDialogButtonBox(Configure);
        buttonBox->setObjectName(QString::fromUtf8("buttonBox"));
        buttonBox->setGeometry(QRect(290, 360, 341, 32));
        buttonBox->setOrientation(Qt::Horizontal);
        buttonBox->setStandardButtons(QDialogButtonBox::Close);
        tabWidget = new QTabWidget(Configure);
        tabWidget->setObjectName(QString::fromUtf8("tabWidget"));
        tabWidget->setGeometry(QRect(10, 10, 631, 341));
        Audio = new QWidget();
        Audio->setObjectName(QString::fromUtf8("Audio"));
        audioDeviceComboBox = new QComboBox(Audio);
        audioDeviceComboBox->setObjectName(QString::fromUtf8("audioDeviceComboBox"));
        audioDeviceComboBox->setGeometry(QRect(130, 20, 171, 29));
        label_8 = new QLabel(Audio);
        label_8->setObjectName(QString::fromUtf8("label_8"));
        label_8->setGeometry(QRect(30, 70, 91, 17));
        sampleRateComboBox = new QComboBox(Audio);
        sampleRateComboBox->setObjectName(QString::fromUtf8("sampleRateComboBox"));
        sampleRateComboBox->setGeometry(QRect(130, 60, 131, 31));
        label_9 = new QLabel(Audio);
        label_9->setObjectName(QString::fromUtf8("label_9"));
        label_9->setGeometry(QRect(30, 110, 71, 17));
        label_10 = new QLabel(Audio);
        label_10->setObjectName(QString::fromUtf8("label_10"));
        label_10->setGeometry(QRect(30, 30, 62, 17));
        audioChannelsSpinBox = new QSpinBox(Audio);
        audioChannelsSpinBox->setObjectName(QString::fromUtf8("audioChannelsSpinBox"));
        audioChannelsSpinBox->setGeometry(QRect(130, 100, 55, 27));
        audioChannelsSpinBox->setMinimum(1);
        audioChannelsSpinBox->setMaximum(2);
        label_11 = new QLabel(Audio);
        label_11->setObjectName(QString::fromUtf8("label_11"));
        label_11->setGeometry(QRect(30, 150, 71, 17));
        encodingComboBox = new QComboBox(Audio);
        encodingComboBox->setObjectName(QString::fromUtf8("encodingComboBox"));
        encodingComboBox->setGeometry(QRect(130, 140, 161, 31));
        label_12 = new QLabel(Audio);
        label_12->setObjectName(QString::fromUtf8("label_12"));
        label_12->setGeometry(QRect(30, 190, 81, 17));
        byteOrderComboBox = new QComboBox(Audio);
        byteOrderComboBox->setObjectName(QString::fromUtf8("byteOrderComboBox"));
        byteOrderComboBox->setGeometry(QRect(130, 180, 161, 31));
        tabWidget->addTab(Audio, QString());
        Server = new QWidget();
        Server->setObjectName(QString::fromUtf8("Server"));
        label_6 = new QLabel(Server);
        label_6->setObjectName(QString::fromUtf8("label_6"));
        label_6->setGeometry(QRect(40, 30, 41, 17));
        hostComboBox = new QComboBox(Server);
        hostComboBox->setObjectName(QString::fromUtf8("hostComboBox"));
        hostComboBox->setGeometry(QRect(90, 20, 221, 29));
        hostComboBox->setEditable(true);
        hostComboBox->setInsertPolicy(QComboBox::InsertAtTop);
        label_7 = new QLabel(Server);
        label_7->setObjectName(QString::fromUtf8("label_7"));
        label_7->setGeometry(QRect(40, 80, 62, 17));
        rxSpinBox = new QSpinBox(Server);
        rxSpinBox->setObjectName(QString::fromUtf8("rxSpinBox"));
        rxSpinBox->setGeometry(QRect(110, 70, 50, 25));
        rxSpinBox->setMaximum(3);
        tabWidget->addTab(Server, QString());
        Display = new QWidget();
        Display->setObjectName(QString::fromUtf8("Display"));
        groupBox = new QGroupBox(Display);
        groupBox->setObjectName(QString::fromUtf8("groupBox"));
        groupBox->setGeometry(QRect(20, 10, 291, 101));
        label = new QLabel(groupBox);
        label->setObjectName(QString::fromUtf8("label"));
        label->setGeometry(QRect(20, 30, 41, 20));
        label_2 = new QLabel(groupBox);
        label_2->setObjectName(QString::fromUtf8("label_2"));
        label_2->setGeometry(QRect(20, 60, 31, 17));
        spectrumHighSpinBox = new QSpinBox(groupBox);
        spectrumHighSpinBox->setObjectName(QString::fromUtf8("spectrumHighSpinBox"));
        spectrumHighSpinBox->setGeometry(QRect(60, 30, 81, 21));
        spectrumHighSpinBox->setMinimum(-200);
        spectrumHighSpinBox->setMaximum(200);
        spectrumHighSpinBox->setSingleStep(10);
        spectrumHighSpinBox->setValue(0);
        spectrumLowSpinBox = new QSpinBox(groupBox);
        spectrumLowSpinBox->setObjectName(QString::fromUtf8("spectrumLowSpinBox"));
        spectrumLowSpinBox->setGeometry(QRect(60, 60, 81, 21));
        spectrumLowSpinBox->setMinimum(-200);
        spectrumLowSpinBox->setMaximum(200);
        spectrumLowSpinBox->setSingleStep(10);
        spectrumLowSpinBox->setValue(-160);
        groupBox_2 = new QGroupBox(Display);
        groupBox_2->setObjectName(QString::fromUtf8("groupBox_2"));
        groupBox_2->setGeometry(QRect(20, 150, 231, 111));
        label_3 = new QLabel(groupBox_2);
        label_3->setObjectName(QString::fromUtf8("label_3"));
        label_3->setGeometry(QRect(20, 30, 41, 17));
        label_4 = new QLabel(groupBox_2);
        label_4->setObjectName(QString::fromUtf8("label_4"));
        label_4->setGeometry(QRect(20, 60, 31, 17));
        waterfallHighSpinBox = new QSpinBox(groupBox_2);
        waterfallHighSpinBox->setObjectName(QString::fromUtf8("waterfallHighSpinBox"));
        waterfallHighSpinBox->setGeometry(QRect(60, 30, 81, 21));
        waterfallHighSpinBox->setMinimum(-200);
        waterfallHighSpinBox->setMaximum(200);
        waterfallHighSpinBox->setSingleStep(5);
        waterfallHighSpinBox->setValue(-80);
        waterfallLowSpinBox = new QSpinBox(groupBox_2);
        waterfallLowSpinBox->setObjectName(QString::fromUtf8("waterfallLowSpinBox"));
        waterfallLowSpinBox->setGeometry(QRect(60, 60, 81, 21));
        waterfallLowSpinBox->setMinimum(-200);
        waterfallLowSpinBox->setMaximum(200);
        waterfallLowSpinBox->setSingleStep(5);
        waterfallLowSpinBox->setValue(-120);
        fpsSpinBox = new QSpinBox(Display);
        fpsSpinBox->setObjectName(QString::fromUtf8("fpsSpinBox"));
        fpsSpinBox->setGeometry(QRect(80, 100, 50, 21));
        fpsSpinBox->setMinimum(1);
        fpsSpinBox->setMaximum(40);
        fpsSpinBox->setValue(15);
        label_5 = new QLabel(Display);
        label_5->setObjectName(QString::fromUtf8("label_5"));
        label_5->setGeometry(QRect(40, 100, 31, 17));
        tabWidget->addTab(Display, QString());
        DSP = new QWidget();
        DSP->setObjectName(QString::fromUtf8("DSP"));
        groupBox_3 = new QGroupBox(DSP);
        groupBox_3->setObjectName(QString::fromUtf8("groupBox_3"));
        groupBox_3->setGeometry(QRect(20, 30, 181, 141));
        label_13 = new QLabel(groupBox_3);
        label_13->setObjectName(QString::fromUtf8("label_13"));
        label_13->setGeometry(QRect(20, 30, 62, 17));
        label_14 = new QLabel(groupBox_3);
        label_14->setObjectName(QString::fromUtf8("label_14"));
        label_14->setGeometry(QRect(20, 60, 62, 17));
        label_15 = new QLabel(groupBox_3);
        label_15->setObjectName(QString::fromUtf8("label_15"));
        label_15->setGeometry(QRect(20, 90, 62, 17));
        label_16 = new QLabel(groupBox_3);
        label_16->setObjectName(QString::fromUtf8("label_16"));
        label_16->setGeometry(QRect(20, 120, 62, 17));
        nrTapsSpinBox = new QSpinBox(groupBox_3);
        nrTapsSpinBox->setObjectName(QString::fromUtf8("nrTapsSpinBox"));
        nrTapsSpinBox->setGeometry(QRect(80, 20, 71, 27));
        nrTapsSpinBox->setMinimum(1);
        nrTapsSpinBox->setMaximum(9999);
        nrTapsSpinBox->setValue(16);
        nrDelaySpinBox = new QSpinBox(groupBox_3);
        nrDelaySpinBox->setObjectName(QString::fromUtf8("nrDelaySpinBox"));
        nrDelaySpinBox->setGeometry(QRect(80, 50, 71, 27));
        nrDelaySpinBox->setValue(8);
        nrGainSpinBox = new QSpinBox(groupBox_3);
        nrGainSpinBox->setObjectName(QString::fromUtf8("nrGainSpinBox"));
        nrGainSpinBox->setGeometry(QRect(80, 80, 71, 27));
        nrLeakSpinBox = new QSpinBox(groupBox_3);
        nrLeakSpinBox->setObjectName(QString::fromUtf8("nrLeakSpinBox"));
        nrLeakSpinBox->setGeometry(QRect(80, 110, 71, 27));
        nrLeakSpinBox->setMinimum(1);
        nrLeakSpinBox->setMaximum(1000);
        nrLeakSpinBox->setValue(10);
        groupBox_4 = new QGroupBox(DSP);
        groupBox_4->setObjectName(QString::fromUtf8("groupBox_4"));
        groupBox_4->setGeometry(QRect(220, 30, 181, 141));
        label_17 = new QLabel(groupBox_4);
        label_17->setObjectName(QString::fromUtf8("label_17"));
        label_17->setGeometry(QRect(20, 30, 62, 17));
        label_18 = new QLabel(groupBox_4);
        label_18->setObjectName(QString::fromUtf8("label_18"));
        label_18->setGeometry(QRect(20, 60, 62, 17));
        label_19 = new QLabel(groupBox_4);
        label_19->setObjectName(QString::fromUtf8("label_19"));
        label_19->setGeometry(QRect(20, 90, 62, 17));
        label_20 = new QLabel(groupBox_4);
        label_20->setObjectName(QString::fromUtf8("label_20"));
        label_20->setGeometry(QRect(20, 120, 62, 17));
        anfTapsSpinBox = new QSpinBox(groupBox_4);
        anfTapsSpinBox->setObjectName(QString::fromUtf8("anfTapsSpinBox"));
        anfTapsSpinBox->setGeometry(QRect(80, 20, 71, 27));
        anfTapsSpinBox->setValue(64);
        anfDelaySpinBox = new QSpinBox(groupBox_4);
        anfDelaySpinBox->setObjectName(QString::fromUtf8("anfDelaySpinBox"));
        anfDelaySpinBox->setGeometry(QRect(80, 50, 71, 27));
        anfDelaySpinBox->setValue(8);
        anfGainSpinBox = new QSpinBox(groupBox_4);
        anfGainSpinBox->setObjectName(QString::fromUtf8("anfGainSpinBox"));
        anfGainSpinBox->setGeometry(QRect(80, 80, 71, 27));
        anfGainSpinBox->setMinimum(1);
        anfGainSpinBox->setMaximum(9999);
        anfGainSpinBox->setValue(32);
        anfLeakSpinBox = new QSpinBox(groupBox_4);
        anfLeakSpinBox->setObjectName(QString::fromUtf8("anfLeakSpinBox"));
        anfLeakSpinBox->setGeometry(QRect(80, 110, 71, 27));
        anfLeakSpinBox->setMinimum(1);
        anfLeakSpinBox->setMaximum(1000);
        groupBox_5 = new QGroupBox(DSP);
        groupBox_5->setObjectName(QString::fromUtf8("groupBox_5"));
        groupBox_5->setGeometry(QRect(420, 30, 181, 141));
        label_23 = new QLabel(groupBox_5);
        label_23->setObjectName(QString::fromUtf8("label_23"));
        label_23->setGeometry(QRect(20, 30, 71, 17));
        nbThresholdSpinBox = new QSpinBox(groupBox_5);
        nbThresholdSpinBox->setObjectName(QString::fromUtf8("nbThresholdSpinBox"));
        nbThresholdSpinBox->setGeometry(QRect(100, 20, 62, 27));
        nbThresholdSpinBox->setMinimum(1);
        nbThresholdSpinBox->setMaximum(200);
        nbThresholdSpinBox->setValue(20);
        tabWidget->addTab(DSP, QString());

        retranslateUi(Configure);
        QObject::connect(buttonBox, SIGNAL(accepted()), Configure, SLOT(accept()));
        QObject::connect(buttonBox, SIGNAL(rejected()), Configure, SLOT(reject()));

        tabWidget->setCurrentIndex(0);


        QMetaObject::connectSlotsByName(Configure);
    } // setupUi

    void retranslateUi(QDialog *Configure)
    {
        Configure->setWindowTitle(QApplication::translate("Configure", "Configure", 0, QApplication::UnicodeUTF8));
        label_8->setText(QApplication::translate("Configure", "Sample Rate:", 0, QApplication::UnicodeUTF8));
        label_9->setText(QApplication::translate("Configure", "Channels:", 0, QApplication::UnicodeUTF8));
        label_10->setText(QApplication::translate("Configure", "Device:", 0, QApplication::UnicodeUTF8));
        label_11->setText(QApplication::translate("Configure", "Encoding:", 0, QApplication::UnicodeUTF8));
        label_12->setText(QApplication::translate("Configure", "Byte Order:", 0, QApplication::UnicodeUTF8));
        tabWidget->setTabText(tabWidget->indexOf(Audio), QApplication::translate("Configure", "Audio", 0, QApplication::UnicodeUTF8));
        label_6->setText(QApplication::translate("Configure", "Host:", 0, QApplication::UnicodeUTF8));
        label_7->setText(QApplication::translate("Configure", "Receiver:", 0, QApplication::UnicodeUTF8));
        tabWidget->setTabText(tabWidget->indexOf(Server), QApplication::translate("Configure", "Server", 0, QApplication::UnicodeUTF8));
        groupBox->setTitle(QApplication::translate("Configure", "Spectrum", 0, QApplication::UnicodeUTF8));
        label->setText(QApplication::translate("Configure", "High:", 0, QApplication::UnicodeUTF8));
        label_2->setText(QApplication::translate("Configure", "Low:", 0, QApplication::UnicodeUTF8));
        groupBox_2->setTitle(QApplication::translate("Configure", "Waterfall", 0, QApplication::UnicodeUTF8));
        label_3->setText(QApplication::translate("Configure", "High:", 0, QApplication::UnicodeUTF8));
        label_4->setText(QApplication::translate("Configure", "Low:", 0, QApplication::UnicodeUTF8));
        label_5->setText(QApplication::translate("Configure", "FPS:", 0, QApplication::UnicodeUTF8));
        tabWidget->setTabText(tabWidget->indexOf(Display), QApplication::translate("Configure", "Display", 0, QApplication::UnicodeUTF8));
        groupBox_3->setTitle(QApplication::translate("Configure", "NR", 0, QApplication::UnicodeUTF8));
        label_13->setText(QApplication::translate("Configure", "Taps:", 0, QApplication::UnicodeUTF8));
        label_14->setText(QApplication::translate("Configure", "Delay:", 0, QApplication::UnicodeUTF8));
        label_15->setText(QApplication::translate("Configure", "Gain:", 0, QApplication::UnicodeUTF8));
        label_16->setText(QApplication::translate("Configure", "Leak:", 0, QApplication::UnicodeUTF8));
        groupBox_4->setTitle(QApplication::translate("Configure", "ANF", 0, QApplication::UnicodeUTF8));
        label_17->setText(QApplication::translate("Configure", "Taps:", 0, QApplication::UnicodeUTF8));
        label_18->setText(QApplication::translate("Configure", "Delay:", 0, QApplication::UnicodeUTF8));
        label_19->setText(QApplication::translate("Configure", "Gain:", 0, QApplication::UnicodeUTF8));
        label_20->setText(QApplication::translate("Configure", "Leak:", 0, QApplication::UnicodeUTF8));
        groupBox_5->setTitle(QApplication::translate("Configure", "NB", 0, QApplication::UnicodeUTF8));
        label_23->setText(QApplication::translate("Configure", "Threshold:", 0, QApplication::UnicodeUTF8));
        tabWidget->setTabText(tabWidget->indexOf(DSP), QApplication::translate("Configure", "DSP", 0, QApplication::UnicodeUTF8));
    } // retranslateUi

};

namespace Ui {
    class Configure: public Ui_Configure {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_CONFIGURE_H
