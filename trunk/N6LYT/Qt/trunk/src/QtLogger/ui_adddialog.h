/********************************************************************************
** Form generated from reading UI file 'adddialog.ui'
**
** Created: Mon Jan 30 17:14:50 2012
**      by: Qt User Interface Compiler version 4.7.4
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_ADDDIALOG_H
#define UI_ADDDIALOG_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QComboBox>
#include <QtGui/QDateEdit>
#include <QtGui/QDialog>
#include <QtGui/QFormLayout>
#include <QtGui/QGridLayout>
#include <QtGui/QHeaderView>
#include <QtGui/QLabel>
#include <QtGui/QLineEdit>
#include <QtGui/QPushButton>
#include <QtGui/QSpacerItem>
#include <QtGui/QSpinBox>
#include <QtGui/QTabWidget>
#include <QtGui/QWidget>

QT_BEGIN_NAMESPACE

class Ui_addDialog
{
public:
    QGridLayout *gridLayout_8;
    QTabWidget *tabWidget;
    QWidget *tab;
    QGridLayout *gridLayout;
    QGridLayout *gridLayout_7;
    QFormLayout *formLayout;
    QLabel *label;
    QLineEdit *dateEdit;
    QLabel *label_2;
    QLineEdit *timeEdit;
    QLabel *label_3;
    QLineEdit *callEdit;
    QLabel *label_9;
    QComboBox *txrstBox;
    QLabel *label_10;
    QComboBox *rxrstBox;
    QLabel *label_7;
    QLineEdit *nameEdit;
    QLabel *label_8;
    QLineEdit *qthEdit;
    QLabel *label_23;
    QLineEdit *gridsqEdit;
    QFormLayout *formLayout_2;
    QLabel *label_5;
    QLineEdit *freqEdit;
    QLabel *label_6;
    QLabel *label_11;
    QLabel *label_12;
    QLabel *label_13;
    QComboBox *eventBox;
    QLineEdit *checkEdit;
    QLabel *label_14;
    QLineEdit *modeEdit;
    QLineEdit *countryEdit;
    QLineEdit *subdivisionEdit;
    QSpinBox *contactBox;
    QLabel *label_4;
    QLineEdit *countyEdit;
    QLabel *label_24;
    QGridLayout *gridLayout_2;
    QLabel *label_15;
    QLineEdit *commentEdit;
    QWidget *tab_2;
    QFormLayout *formLayout_3;
    QGridLayout *gridLayout_4;
    QFormLayout *formLayout_8;
    QLabel *label_21;
    QLineEdit *operatorEdit;
    QLabel *label_20;
    QLineEdit *homeqthEdit;
    QFormLayout *formLayout_4;
    QLabel *label_16;
    QLineEdit *stationEdit;
    QLineEdit *homegridEdit;
    QLabel *label_17;
    QWidget *tab_3;
    QGridLayout *gridLayout_6;
    QGridLayout *gridLayout_5;
    QFormLayout *formLayout_6;
    QLabel *label_18;
    QLabel *label_19;
    QDateEdit *qslrcvdEdit;
    QComboBox *qslrcvdviaBox;
    QFormLayout *formLayout_7;
    QLabel *label_22;
    QLabel *label_26;
    QDateEdit *qslsentEdit;
    QComboBox *qslsentviaBox;
    QGridLayout *gridLayout_3;
    QPushButton *addButton;
    QSpacerItem *horizontalSpacer;
    QPushButton *resetButton;
    QPushButton *cancelButton;

    void setupUi(QDialog *addDialog)
    {
        if (addDialog->objectName().isEmpty())
            addDialog->setObjectName(QString::fromUtf8("addDialog"));
        addDialog->resize(576, 412);
        gridLayout_8 = new QGridLayout(addDialog);
        gridLayout_8->setObjectName(QString::fromUtf8("gridLayout_8"));
        tabWidget = new QTabWidget(addDialog);
        tabWidget->setObjectName(QString::fromUtf8("tabWidget"));
        tab = new QWidget();
        tab->setObjectName(QString::fromUtf8("tab"));
        gridLayout = new QGridLayout(tab);
        gridLayout->setObjectName(QString::fromUtf8("gridLayout"));
        gridLayout_7 = new QGridLayout();
        gridLayout_7->setObjectName(QString::fromUtf8("gridLayout_7"));
        formLayout = new QFormLayout();
        formLayout->setObjectName(QString::fromUtf8("formLayout"));
        formLayout->setFieldGrowthPolicy(QFormLayout::AllNonFixedFieldsGrow);
        label = new QLabel(tab);
        label->setObjectName(QString::fromUtf8("label"));

        formLayout->setWidget(0, QFormLayout::LabelRole, label);

        dateEdit = new QLineEdit(tab);
        dateEdit->setObjectName(QString::fromUtf8("dateEdit"));
        QFont font;
        font.setPointSize(9);
        dateEdit->setFont(font);

        formLayout->setWidget(0, QFormLayout::FieldRole, dateEdit);

        label_2 = new QLabel(tab);
        label_2->setObjectName(QString::fromUtf8("label_2"));

        formLayout->setWidget(1, QFormLayout::LabelRole, label_2);

        timeEdit = new QLineEdit(tab);
        timeEdit->setObjectName(QString::fromUtf8("timeEdit"));
        timeEdit->setFont(font);

        formLayout->setWidget(1, QFormLayout::FieldRole, timeEdit);

        label_3 = new QLabel(tab);
        label_3->setObjectName(QString::fromUtf8("label_3"));

        formLayout->setWidget(2, QFormLayout::LabelRole, label_3);

        callEdit = new QLineEdit(tab);
        callEdit->setObjectName(QString::fromUtf8("callEdit"));
        QFont font1;
        font1.setPointSize(15);
        callEdit->setFont(font1);

        formLayout->setWidget(2, QFormLayout::FieldRole, callEdit);

        label_9 = new QLabel(tab);
        label_9->setObjectName(QString::fromUtf8("label_9"));

        formLayout->setWidget(3, QFormLayout::LabelRole, label_9);

        txrstBox = new QComboBox(tab);
        txrstBox->setObjectName(QString::fromUtf8("txrstBox"));
        txrstBox->setEditable(true);

        formLayout->setWidget(3, QFormLayout::FieldRole, txrstBox);

        label_10 = new QLabel(tab);
        label_10->setObjectName(QString::fromUtf8("label_10"));

        formLayout->setWidget(4, QFormLayout::LabelRole, label_10);

        rxrstBox = new QComboBox(tab);
        rxrstBox->setObjectName(QString::fromUtf8("rxrstBox"));
        rxrstBox->setFocusPolicy(Qt::WheelFocus);
        rxrstBox->setEditable(true);

        formLayout->setWidget(4, QFormLayout::FieldRole, rxrstBox);

        label_7 = new QLabel(tab);
        label_7->setObjectName(QString::fromUtf8("label_7"));

        formLayout->setWidget(5, QFormLayout::LabelRole, label_7);

        nameEdit = new QLineEdit(tab);
        nameEdit->setObjectName(QString::fromUtf8("nameEdit"));

        formLayout->setWidget(5, QFormLayout::FieldRole, nameEdit);

        label_8 = new QLabel(tab);
        label_8->setObjectName(QString::fromUtf8("label_8"));

        formLayout->setWidget(6, QFormLayout::LabelRole, label_8);

        qthEdit = new QLineEdit(tab);
        qthEdit->setObjectName(QString::fromUtf8("qthEdit"));

        formLayout->setWidget(6, QFormLayout::FieldRole, qthEdit);

        label_23 = new QLabel(tab);
        label_23->setObjectName(QString::fromUtf8("label_23"));

        formLayout->setWidget(7, QFormLayout::LabelRole, label_23);

        gridsqEdit = new QLineEdit(tab);
        gridsqEdit->setObjectName(QString::fromUtf8("gridsqEdit"));

        formLayout->setWidget(7, QFormLayout::FieldRole, gridsqEdit);


        gridLayout_7->addLayout(formLayout, 0, 0, 1, 1);

        formLayout_2 = new QFormLayout();
        formLayout_2->setObjectName(QString::fromUtf8("formLayout_2"));
        label_5 = new QLabel(tab);
        label_5->setObjectName(QString::fromUtf8("label_5"));

        formLayout_2->setWidget(1, QFormLayout::LabelRole, label_5);

        freqEdit = new QLineEdit(tab);
        freqEdit->setObjectName(QString::fromUtf8("freqEdit"));

        formLayout_2->setWidget(1, QFormLayout::FieldRole, freqEdit);

        label_6 = new QLabel(tab);
        label_6->setObjectName(QString::fromUtf8("label_6"));

        formLayout_2->setWidget(2, QFormLayout::LabelRole, label_6);

        label_11 = new QLabel(tab);
        label_11->setObjectName(QString::fromUtf8("label_11"));

        formLayout_2->setWidget(3, QFormLayout::LabelRole, label_11);

        label_12 = new QLabel(tab);
        label_12->setObjectName(QString::fromUtf8("label_12"));

        formLayout_2->setWidget(4, QFormLayout::LabelRole, label_12);

        label_13 = new QLabel(tab);
        label_13->setObjectName(QString::fromUtf8("label_13"));

        formLayout_2->setWidget(6, QFormLayout::LabelRole, label_13);

        eventBox = new QComboBox(tab);
        eventBox->setObjectName(QString::fromUtf8("eventBox"));
        eventBox->setEditable(true);

        formLayout_2->setWidget(6, QFormLayout::FieldRole, eventBox);

        checkEdit = new QLineEdit(tab);
        checkEdit->setObjectName(QString::fromUtf8("checkEdit"));

        formLayout_2->setWidget(8, QFormLayout::FieldRole, checkEdit);

        label_14 = new QLabel(tab);
        label_14->setObjectName(QString::fromUtf8("label_14"));

        formLayout_2->setWidget(8, QFormLayout::LabelRole, label_14);

        modeEdit = new QLineEdit(tab);
        modeEdit->setObjectName(QString::fromUtf8("modeEdit"));

        formLayout_2->setWidget(2, QFormLayout::FieldRole, modeEdit);

        countryEdit = new QLineEdit(tab);
        countryEdit->setObjectName(QString::fromUtf8("countryEdit"));

        formLayout_2->setWidget(3, QFormLayout::FieldRole, countryEdit);

        subdivisionEdit = new QLineEdit(tab);
        subdivisionEdit->setObjectName(QString::fromUtf8("subdivisionEdit"));

        formLayout_2->setWidget(4, QFormLayout::FieldRole, subdivisionEdit);

        contactBox = new QSpinBox(tab);
        contactBox->setObjectName(QString::fromUtf8("contactBox"));
        contactBox->setFont(font);
        contactBox->setMinimum(1);
        contactBox->setMaximum(9999);

        formLayout_2->setWidget(9, QFormLayout::FieldRole, contactBox);

        label_4 = new QLabel(tab);
        label_4->setObjectName(QString::fromUtf8("label_4"));

        formLayout_2->setWidget(9, QFormLayout::LabelRole, label_4);

        countyEdit = new QLineEdit(tab);
        countyEdit->setObjectName(QString::fromUtf8("countyEdit"));

        formLayout_2->setWidget(5, QFormLayout::FieldRole, countyEdit);

        label_24 = new QLabel(tab);
        label_24->setObjectName(QString::fromUtf8("label_24"));

        formLayout_2->setWidget(5, QFormLayout::LabelRole, label_24);


        gridLayout_7->addLayout(formLayout_2, 0, 1, 1, 1);

        gridLayout_2 = new QGridLayout();
        gridLayout_2->setObjectName(QString::fromUtf8("gridLayout_2"));
        label_15 = new QLabel(tab);
        label_15->setObjectName(QString::fromUtf8("label_15"));

        gridLayout_2->addWidget(label_15, 0, 0, 1, 1);

        commentEdit = new QLineEdit(tab);
        commentEdit->setObjectName(QString::fromUtf8("commentEdit"));

        gridLayout_2->addWidget(commentEdit, 0, 1, 1, 1);


        gridLayout_7->addLayout(gridLayout_2, 1, 0, 1, 2);


        gridLayout->addLayout(gridLayout_7, 0, 0, 1, 1);

        tabWidget->addTab(tab, QString());
        tab_2 = new QWidget();
        tab_2->setObjectName(QString::fromUtf8("tab_2"));
        formLayout_3 = new QFormLayout(tab_2);
        formLayout_3->setObjectName(QString::fromUtf8("formLayout_3"));
        gridLayout_4 = new QGridLayout();
        gridLayout_4->setObjectName(QString::fromUtf8("gridLayout_4"));
        formLayout_8 = new QFormLayout();
        formLayout_8->setObjectName(QString::fromUtf8("formLayout_8"));
        label_21 = new QLabel(tab_2);
        label_21->setObjectName(QString::fromUtf8("label_21"));

        formLayout_8->setWidget(0, QFormLayout::LabelRole, label_21);

        operatorEdit = new QLineEdit(tab_2);
        operatorEdit->setObjectName(QString::fromUtf8("operatorEdit"));

        formLayout_8->setWidget(0, QFormLayout::FieldRole, operatorEdit);

        label_20 = new QLabel(tab_2);
        label_20->setObjectName(QString::fromUtf8("label_20"));

        formLayout_8->setWidget(1, QFormLayout::LabelRole, label_20);

        homeqthEdit = new QLineEdit(tab_2);
        homeqthEdit->setObjectName(QString::fromUtf8("homeqthEdit"));

        formLayout_8->setWidget(1, QFormLayout::FieldRole, homeqthEdit);


        gridLayout_4->addLayout(formLayout_8, 0, 0, 1, 1);

        formLayout_4 = new QFormLayout();
        formLayout_4->setObjectName(QString::fromUtf8("formLayout_4"));
        formLayout_4->setFieldGrowthPolicy(QFormLayout::AllNonFixedFieldsGrow);
        label_16 = new QLabel(tab_2);
        label_16->setObjectName(QString::fromUtf8("label_16"));

        formLayout_4->setWidget(0, QFormLayout::LabelRole, label_16);

        stationEdit = new QLineEdit(tab_2);
        stationEdit->setObjectName(QString::fromUtf8("stationEdit"));

        formLayout_4->setWidget(0, QFormLayout::FieldRole, stationEdit);

        homegridEdit = new QLineEdit(tab_2);
        homegridEdit->setObjectName(QString::fromUtf8("homegridEdit"));

        formLayout_4->setWidget(1, QFormLayout::FieldRole, homegridEdit);

        label_17 = new QLabel(tab_2);
        label_17->setObjectName(QString::fromUtf8("label_17"));

        formLayout_4->setWidget(1, QFormLayout::LabelRole, label_17);


        gridLayout_4->addLayout(formLayout_4, 0, 1, 1, 1);


        formLayout_3->setLayout(0, QFormLayout::LabelRole, gridLayout_4);

        tabWidget->addTab(tab_2, QString());
        tab_3 = new QWidget();
        tab_3->setObjectName(QString::fromUtf8("tab_3"));
        gridLayout_6 = new QGridLayout(tab_3);
        gridLayout_6->setObjectName(QString::fromUtf8("gridLayout_6"));
        gridLayout_5 = new QGridLayout();
        gridLayout_5->setObjectName(QString::fromUtf8("gridLayout_5"));
        formLayout_6 = new QFormLayout();
        formLayout_6->setObjectName(QString::fromUtf8("formLayout_6"));
        label_18 = new QLabel(tab_3);
        label_18->setObjectName(QString::fromUtf8("label_18"));

        formLayout_6->setWidget(0, QFormLayout::LabelRole, label_18);

        label_19 = new QLabel(tab_3);
        label_19->setObjectName(QString::fromUtf8("label_19"));

        formLayout_6->setWidget(1, QFormLayout::LabelRole, label_19);

        qslrcvdEdit = new QDateEdit(tab_3);
        qslrcvdEdit->setObjectName(QString::fromUtf8("qslrcvdEdit"));

        formLayout_6->setWidget(0, QFormLayout::FieldRole, qslrcvdEdit);

        qslrcvdviaBox = new QComboBox(tab_3);
        qslrcvdviaBox->setObjectName(QString::fromUtf8("qslrcvdviaBox"));

        formLayout_6->setWidget(1, QFormLayout::FieldRole, qslrcvdviaBox);


        gridLayout_5->addLayout(formLayout_6, 0, 0, 1, 1);

        formLayout_7 = new QFormLayout();
        formLayout_7->setObjectName(QString::fromUtf8("formLayout_7"));
        label_22 = new QLabel(tab_3);
        label_22->setObjectName(QString::fromUtf8("label_22"));

        formLayout_7->setWidget(0, QFormLayout::LabelRole, label_22);

        label_26 = new QLabel(tab_3);
        label_26->setObjectName(QString::fromUtf8("label_26"));

        formLayout_7->setWidget(1, QFormLayout::LabelRole, label_26);

        qslsentEdit = new QDateEdit(tab_3);
        qslsentEdit->setObjectName(QString::fromUtf8("qslsentEdit"));

        formLayout_7->setWidget(0, QFormLayout::FieldRole, qslsentEdit);

        qslsentviaBox = new QComboBox(tab_3);
        qslsentviaBox->setObjectName(QString::fromUtf8("qslsentviaBox"));

        formLayout_7->setWidget(1, QFormLayout::FieldRole, qslsentviaBox);


        gridLayout_5->addLayout(formLayout_7, 0, 1, 1, 1);


        gridLayout_6->addLayout(gridLayout_5, 0, 0, 1, 1);

        tabWidget->addTab(tab_3, QString());

        gridLayout_8->addWidget(tabWidget, 0, 0, 1, 1);

        gridLayout_3 = new QGridLayout();
        gridLayout_3->setObjectName(QString::fromUtf8("gridLayout_3"));
        gridLayout_3->setSizeConstraint(QLayout::SetDefaultConstraint);
        addButton = new QPushButton(addDialog);
        addButton->setObjectName(QString::fromUtf8("addButton"));

        gridLayout_3->addWidget(addButton, 0, 0, 1, 1);

        horizontalSpacer = new QSpacerItem(40, 20, QSizePolicy::Expanding, QSizePolicy::Minimum);

        gridLayout_3->addItem(horizontalSpacer, 0, 1, 1, 1);

        resetButton = new QPushButton(addDialog);
        resetButton->setObjectName(QString::fromUtf8("resetButton"));

        gridLayout_3->addWidget(resetButton, 0, 2, 1, 1);

        cancelButton = new QPushButton(addDialog);
        cancelButton->setObjectName(QString::fromUtf8("cancelButton"));

        gridLayout_3->addWidget(cancelButton, 0, 3, 1, 1);


        gridLayout_8->addLayout(gridLayout_3, 1, 0, 1, 1);

        QWidget::setTabOrder(txrstBox, rxrstBox);
        QWidget::setTabOrder(rxrstBox, nameEdit);
        QWidget::setTabOrder(nameEdit, qthEdit);
        QWidget::setTabOrder(qthEdit, gridsqEdit);
        QWidget::setTabOrder(gridsqEdit, commentEdit);
        QWidget::setTabOrder(commentEdit, countryEdit);
        QWidget::setTabOrder(countryEdit, subdivisionEdit);
        QWidget::setTabOrder(subdivisionEdit, countyEdit);
        QWidget::setTabOrder(countyEdit, checkEdit);
        QWidget::setTabOrder(checkEdit, addButton);
        QWidget::setTabOrder(addButton, resetButton);
        QWidget::setTabOrder(resetButton, cancelButton);
        QWidget::setTabOrder(cancelButton, freqEdit);
        QWidget::setTabOrder(freqEdit, modeEdit);
        QWidget::setTabOrder(modeEdit, eventBox);
        QWidget::setTabOrder(eventBox, contactBox);
        QWidget::setTabOrder(contactBox, dateEdit);
        QWidget::setTabOrder(dateEdit, timeEdit);
        QWidget::setTabOrder(timeEdit, operatorEdit);
        QWidget::setTabOrder(operatorEdit, stationEdit);
        QWidget::setTabOrder(stationEdit, qslrcvdEdit);
        QWidget::setTabOrder(qslrcvdEdit, qslrcvdviaBox);
        QWidget::setTabOrder(qslrcvdviaBox, qslsentEdit);
        QWidget::setTabOrder(qslsentEdit, qslsentviaBox);
        QWidget::setTabOrder(qslsentviaBox, tabWidget);
        QWidget::setTabOrder(tabWidget, callEdit);
        QWidget::setTabOrder(callEdit, homeqthEdit);
        QWidget::setTabOrder(homeqthEdit, homegridEdit);

        retranslateUi(addDialog);

        tabWidget->setCurrentIndex(0);


        QMetaObject::connectSlotsByName(addDialog);
    } // setupUi

    void retranslateUi(QDialog *addDialog)
    {
        addDialog->setWindowTitle(QApplication::translate("addDialog", "Dialog", 0, QApplication::UnicodeUTF8));
        label->setText(QApplication::translate("addDialog", "Date", 0, QApplication::UnicodeUTF8));
        label_2->setText(QApplication::translate("addDialog", "Time", 0, QApplication::UnicodeUTF8));
        label_3->setText(QApplication::translate("addDialog", "Callsign", 0, QApplication::UnicodeUTF8));
        label_9->setText(QApplication::translate("addDialog", "TXrst", 0, QApplication::UnicodeUTF8));
        txrstBox->clear();
        txrstBox->insertItems(0, QStringList()
         << QApplication::translate("addDialog", "59", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "58", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "57", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "56", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "55", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "54", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "53", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "52", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "51", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "50", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "49", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "48", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "47", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "46", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "45", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "44", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "43", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "42", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "41", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "40", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "39", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "38", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "37", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "36", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "35", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "29", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "28", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "27", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "26", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "25", 0, QApplication::UnicodeUTF8)
        );
        label_10->setText(QApplication::translate("addDialog", "RXrst", 0, QApplication::UnicodeUTF8));
        rxrstBox->clear();
        rxrstBox->insertItems(0, QStringList()
         << QApplication::translate("addDialog", "59", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "58", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "57", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "56", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "55", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "54", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "53", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "52", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "51", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "50", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "49", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "48", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "47", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "46", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "45", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "44", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "43", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "42", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "41", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "40", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "39", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "38", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "37", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "36", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "35", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "29", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "28", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "27", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "26", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "25", 0, QApplication::UnicodeUTF8)
        );
        label_7->setText(QApplication::translate("addDialog", "Name", 0, QApplication::UnicodeUTF8));
        label_8->setText(QApplication::translate("addDialog", "QTH", 0, QApplication::UnicodeUTF8));
        label_23->setText(QApplication::translate("addDialog", "Grid Sq", 0, QApplication::UnicodeUTF8));
        label_5->setText(QApplication::translate("addDialog", "Frequency", 0, QApplication::UnicodeUTF8));
        label_6->setText(QApplication::translate("addDialog", "Mode", 0, QApplication::UnicodeUTF8));
        label_11->setText(QApplication::translate("addDialog", "Country", 0, QApplication::UnicodeUTF8));
        label_12->setText(QApplication::translate("addDialog", "State", 0, QApplication::UnicodeUTF8));
        label_13->setText(QApplication::translate("addDialog", "Event", 0, QApplication::UnicodeUTF8));
        eventBox->clear();
        eventBox->insertItems(0, QStringList()
         << QApplication::translate("addDialog", "Home", 0, QApplication::UnicodeUTF8)
        );
        label_14->setText(QApplication::translate("addDialog", "Check", 0, QApplication::UnicodeUTF8));
        label_4->setText(QApplication::translate("addDialog", "Contact", 0, QApplication::UnicodeUTF8));
        label_24->setText(QApplication::translate("addDialog", "County", 0, QApplication::UnicodeUTF8));
        label_15->setText(QApplication::translate("addDialog", "Comment", 0, QApplication::UnicodeUTF8));
        tabWidget->setTabText(tabWidget->indexOf(tab), QApplication::translate("addDialog", "Contact", 0, QApplication::UnicodeUTF8));
        label_21->setText(QApplication::translate("addDialog", "Operator", 0, QApplication::UnicodeUTF8));
        label_20->setText(QApplication::translate("addDialog", "Home QTH", 0, QApplication::UnicodeUTF8));
        label_16->setText(QApplication::translate("addDialog", "Station_Call", 0, QApplication::UnicodeUTF8));
        label_17->setText(QApplication::translate("addDialog", "Grid square", 0, QApplication::UnicodeUTF8));
        tabWidget->setTabText(tabWidget->indexOf(tab_2), QApplication::translate("addDialog", "Owner", 0, QApplication::UnicodeUTF8));
        label_18->setText(QApplication::translate("addDialog", "QSL_RCVD", 0, QApplication::UnicodeUTF8));
        label_19->setText(QApplication::translate("addDialog", "QSL_RCVD_VIA", 0, QApplication::UnicodeUTF8));
        qslrcvdviaBox->clear();
        qslrcvdviaBox->insertItems(0, QStringList()
         << QApplication::translate("addDialog", "None", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "Direct", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "Bureau", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "Electronic", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "Manager", 0, QApplication::UnicodeUTF8)
        );
        label_22->setText(QApplication::translate("addDialog", "QSL_SENT", 0, QApplication::UnicodeUTF8));
        label_26->setText(QApplication::translate("addDialog", "QSL_SENT_VIA", 0, QApplication::UnicodeUTF8));
        qslsentviaBox->clear();
        qslsentviaBox->insertItems(0, QStringList()
         << QApplication::translate("addDialog", "None", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "Direct", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "Bureau", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "Electronic", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("addDialog", "Manager", 0, QApplication::UnicodeUTF8)
        );
        tabWidget->setTabText(tabWidget->indexOf(tab_3), QApplication::translate("addDialog", "QSL", 0, QApplication::UnicodeUTF8));
        addButton->setText(QApplication::translate("addDialog", "Add", 0, QApplication::UnicodeUTF8));
        resetButton->setText(QApplication::translate("addDialog", "Reset", 0, QApplication::UnicodeUTF8));
        cancelButton->setText(QApplication::translate("addDialog", "Cancel", 0, QApplication::UnicodeUTF8));
    } // retranslateUi

};

namespace Ui {
    class addDialog: public Ui_addDialog {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_ADDDIALOG_H
