/********************************************************************************
** Form generated from reading UI file 'preferences.ui'
**
** Created: Tue Mar 20 21:55:51 2012
**      by: Qt User Interface Compiler version 4.8.0
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_PREFERENCES_H
#define UI_PREFERENCES_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QCheckBox>
#include <QtGui/QDialog>
#include <QtGui/QGridLayout>
#include <QtGui/QGroupBox>
#include <QtGui/QHBoxLayout>
#include <QtGui/QHeaderView>
#include <QtGui/QPushButton>
#include <QtGui/QSpacerItem>

QT_BEGIN_NAMESPACE

class Ui_Preferences
{
public:
    QGridLayout *gridLayout_3;
    QGridLayout *gridLayout_2;
    QGroupBox *groupBox;
    QGridLayout *gridLayout;
    QCheckBox *radio_checkBox;
    QCheckBox *location_checkBox;
    QCheckBox *event_checkBox;
    QCheckBox *owner_checkBox;
    QCheckBox *qsl_checkBox;
    QGroupBox *groupBox_2;
    QCheckBox *callfilter_checkBox;
    QHBoxLayout *horizontalLayout;
    QPushButton *okButton;
    QSpacerItem *horizontalSpacer;
    QPushButton *cancelButton;

    void setupUi(QDialog *Preferences)
    {
        if (Preferences->objectName().isEmpty())
            Preferences->setObjectName(QString::fromUtf8("Preferences"));
        Preferences->resize(440, 225);
        gridLayout_3 = new QGridLayout(Preferences);
        gridLayout_3->setObjectName(QString::fromUtf8("gridLayout_3"));
        gridLayout_2 = new QGridLayout();
        gridLayout_2->setObjectName(QString::fromUtf8("gridLayout_2"));
        groupBox = new QGroupBox(Preferences);
        groupBox->setObjectName(QString::fromUtf8("groupBox"));
        gridLayout = new QGridLayout(groupBox);
        gridLayout->setObjectName(QString::fromUtf8("gridLayout"));
        radio_checkBox = new QCheckBox(groupBox);
        radio_checkBox->setObjectName(QString::fromUtf8("radio_checkBox"));
        radio_checkBox->setChecked(false);

        gridLayout->addWidget(radio_checkBox, 0, 0, 1, 1);

        location_checkBox = new QCheckBox(groupBox);
        location_checkBox->setObjectName(QString::fromUtf8("location_checkBox"));
        location_checkBox->setChecked(false);

        gridLayout->addWidget(location_checkBox, 1, 0, 1, 1);

        event_checkBox = new QCheckBox(groupBox);
        event_checkBox->setObjectName(QString::fromUtf8("event_checkBox"));
        event_checkBox->setChecked(false);

        gridLayout->addWidget(event_checkBox, 2, 0, 1, 1);

        owner_checkBox = new QCheckBox(groupBox);
        owner_checkBox->setObjectName(QString::fromUtf8("owner_checkBox"));
        owner_checkBox->setChecked(false);

        gridLayout->addWidget(owner_checkBox, 3, 0, 1, 1);

        qsl_checkBox = new QCheckBox(groupBox);
        qsl_checkBox->setObjectName(QString::fromUtf8("qsl_checkBox"));
        qsl_checkBox->setChecked(false);

        gridLayout->addWidget(qsl_checkBox, 4, 0, 1, 1);


        gridLayout_2->addWidget(groupBox, 0, 0, 1, 1);

        groupBox_2 = new QGroupBox(Preferences);
        groupBox_2->setObjectName(QString::fromUtf8("groupBox_2"));
        callfilter_checkBox = new QCheckBox(groupBox_2);
        callfilter_checkBox->setObjectName(QString::fromUtf8("callfilter_checkBox"));
        callfilter_checkBox->setGeometry(QRect(30, 30, 151, 22));

        gridLayout_2->addWidget(groupBox_2, 0, 1, 1, 1);

        horizontalLayout = new QHBoxLayout();
        horizontalLayout->setObjectName(QString::fromUtf8("horizontalLayout"));
        okButton = new QPushButton(Preferences);
        okButton->setObjectName(QString::fromUtf8("okButton"));

        horizontalLayout->addWidget(okButton);

        horizontalSpacer = new QSpacerItem(40, 20, QSizePolicy::Expanding, QSizePolicy::Minimum);

        horizontalLayout->addItem(horizontalSpacer);

        cancelButton = new QPushButton(Preferences);
        cancelButton->setObjectName(QString::fromUtf8("cancelButton"));

        horizontalLayout->addWidget(cancelButton);


        gridLayout_2->addLayout(horizontalLayout, 1, 0, 1, 2);


        gridLayout_3->addLayout(gridLayout_2, 0, 0, 1, 1);


        retranslateUi(Preferences);

        QMetaObject::connectSlotsByName(Preferences);
    } // setupUi

    void retranslateUi(QDialog *Preferences)
    {
        Preferences->setWindowTitle(QApplication::translate("Preferences", "Dialog", 0, QApplication::UnicodeUTF8));
        groupBox->setTitle(QApplication::translate("Preferences", "QtLogger View Preferences", 0, QApplication::UnicodeUTF8));
        radio_checkBox->setText(QApplication::translate("Preferences", "Radio items", 0, QApplication::UnicodeUTF8));
        location_checkBox->setText(QApplication::translate("Preferences", "Location items", 0, QApplication::UnicodeUTF8));
        event_checkBox->setText(QApplication::translate("Preferences", "Event Items", 0, QApplication::UnicodeUTF8));
        owner_checkBox->setText(QApplication::translate("Preferences", "Owner items", 0, QApplication::UnicodeUTF8));
        qsl_checkBox->setText(QApplication::translate("Preferences", "QSL items", 0, QApplication::UnicodeUTF8));
        groupBox_2->setTitle(QApplication::translate("Preferences", "Callsign", 0, QApplication::UnicodeUTF8));
#ifndef QT_NO_TOOLTIP
        callfilter_checkBox->setToolTip(QApplication::translate("Preferences", "Turn off the callsign filter for odd special callsigns", 0, QApplication::UnicodeUTF8));
#endif // QT_NO_TOOLTIP
        callfilter_checkBox->setText(QApplication::translate("Preferences", "Callsign Filter", 0, QApplication::UnicodeUTF8));
        okButton->setText(QApplication::translate("Preferences", "OK", 0, QApplication::UnicodeUTF8));
        cancelButton->setText(QApplication::translate("Preferences", "Cancel", 0, QApplication::UnicodeUTF8));
    } // retranslateUi

};

namespace Ui {
    class Preferences: public Ui_Preferences {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_PREFERENCES_H
