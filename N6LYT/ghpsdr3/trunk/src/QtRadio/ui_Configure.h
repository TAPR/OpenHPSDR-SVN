/********************************************************************************
** Form generated from reading UI file 'Configure.ui'
**
** Created: Tue Aug 17 20:23:42 2010
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
    QWidget *tab;
    QLabel *label_6;
    QComboBox *hostComboBox;
    QLabel *label_7;
    QSpinBox *rxSpinBox;

    void setupUi(QDialog *Configure)
    {
        if (Configure->objectName().isEmpty())
            Configure->setObjectName(QString::fromUtf8("Configure"));
        Configure->resize(657, 399);
        buttonBox = new QDialogButtonBox(Configure);
        buttonBox->setObjectName(QString::fromUtf8("buttonBox"));
        buttonBox->setGeometry(QRect(290, 360, 341, 32));
        buttonBox->setOrientation(Qt::Horizontal);
        buttonBox->setStandardButtons(QDialogButtonBox::Cancel|QDialogButtonBox::Ok);
        tabWidget = new QTabWidget(Configure);
        tabWidget->setObjectName(QString::fromUtf8("tabWidget"));
        tabWidget->setGeometry(QRect(10, 10, 631, 341));
        Audio = new QWidget();
        Audio->setObjectName(QString::fromUtf8("Audio"));
        audioDeviceComboBox = new QComboBox(Audio);
        audioDeviceComboBox->setObjectName(QString::fromUtf8("audioDeviceComboBox"));
        audioDeviceComboBox->setGeometry(QRect(30, 20, 171, 29));
        tabWidget->addTab(Audio, QString());
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
        tab = new QWidget();
        tab->setObjectName(QString::fromUtf8("tab"));
        label_6 = new QLabel(tab);
        label_6->setObjectName(QString::fromUtf8("label_6"));
        label_6->setGeometry(QRect(40, 30, 41, 17));
        hostComboBox = new QComboBox(tab);
        hostComboBox->setObjectName(QString::fromUtf8("hostComboBox"));
        hostComboBox->setGeometry(QRect(90, 20, 221, 29));
        hostComboBox->setEditable(true);
        hostComboBox->setInsertPolicy(QComboBox::InsertAtTop);
        label_7 = new QLabel(tab);
        label_7->setObjectName(QString::fromUtf8("label_7"));
        label_7->setGeometry(QRect(40, 80, 62, 17));
        rxSpinBox = new QSpinBox(tab);
        rxSpinBox->setObjectName(QString::fromUtf8("rxSpinBox"));
        rxSpinBox->setGeometry(QRect(110, 70, 50, 25));
        rxSpinBox->setMaximum(3);
        tabWidget->addTab(tab, QString());

        retranslateUi(Configure);
        QObject::connect(buttonBox, SIGNAL(accepted()), Configure, SLOT(accept()));
        QObject::connect(buttonBox, SIGNAL(rejected()), Configure, SLOT(reject()));

        tabWidget->setCurrentIndex(0);


        QMetaObject::connectSlotsByName(Configure);
    } // setupUi

    void retranslateUi(QDialog *Configure)
    {
        Configure->setWindowTitle(QApplication::translate("Configure", "Configure", 0, QApplication::UnicodeUTF8));
        tabWidget->setTabText(tabWidget->indexOf(Audio), QApplication::translate("Configure", "Audio", 0, QApplication::UnicodeUTF8));
        groupBox->setTitle(QApplication::translate("Configure", "Spectrum", 0, QApplication::UnicodeUTF8));
        label->setText(QApplication::translate("Configure", "High:", 0, QApplication::UnicodeUTF8));
        label_2->setText(QApplication::translate("Configure", "Low:", 0, QApplication::UnicodeUTF8));
        groupBox_2->setTitle(QApplication::translate("Configure", "Waterfall", 0, QApplication::UnicodeUTF8));
        label_3->setText(QApplication::translate("Configure", "High:", 0, QApplication::UnicodeUTF8));
        label_4->setText(QApplication::translate("Configure", "Low:", 0, QApplication::UnicodeUTF8));
        label_5->setText(QApplication::translate("Configure", "FPS:", 0, QApplication::UnicodeUTF8));
        tabWidget->setTabText(tabWidget->indexOf(Display), QApplication::translate("Configure", "Display", 0, QApplication::UnicodeUTF8));
        label_6->setText(QApplication::translate("Configure", "Host:", 0, QApplication::UnicodeUTF8));
        label_7->setText(QApplication::translate("Configure", "Receiver:", 0, QApplication::UnicodeUTF8));
        tabWidget->setTabText(tabWidget->indexOf(tab), QApplication::translate("Configure", "Server", 0, QApplication::UnicodeUTF8));
    } // retranslateUi

};

namespace Ui {
    class Configure: public Ui_Configure {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_CONFIGURE_H
