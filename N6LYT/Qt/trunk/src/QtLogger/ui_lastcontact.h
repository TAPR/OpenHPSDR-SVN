/********************************************************************************
** Form generated from reading UI file 'lastcontact.ui'
**
** Created: Tue Mar 20 21:55:51 2012
**      by: Qt User Interface Compiler version 4.8.0
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_LASTCONTACT_H
#define UI_LASTCONTACT_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QDialog>
#include <QtGui/QGridLayout>
#include <QtGui/QHBoxLayout>
#include <QtGui/QHeaderView>
#include <QtGui/QLabel>
#include <QtGui/QPushButton>
#include <QtGui/QSpacerItem>
#include <QtGui/QTreeView>

QT_BEGIN_NAMESPACE

class Ui_lastContact
{
public:
    QGridLayout *gridLayout;
    QTreeView *treeView;
    QHBoxLayout *horizontalLayout;
    QLabel *label;
    QSpacerItem *horizontalSpacer;
    QPushButton *closeButton;

    void setupUi(QDialog *lastContact)
    {
        if (lastContact->objectName().isEmpty())
            lastContact->setObjectName(QString::fromUtf8("lastContact"));
        lastContact->resize(601, 274);
        gridLayout = new QGridLayout(lastContact);
        gridLayout->setObjectName(QString::fromUtf8("gridLayout"));
        treeView = new QTreeView(lastContact);
        treeView->setObjectName(QString::fromUtf8("treeView"));

        gridLayout->addWidget(treeView, 0, 0, 1, 1);

        horizontalLayout = new QHBoxLayout();
        horizontalLayout->setObjectName(QString::fromUtf8("horizontalLayout"));
        label = new QLabel(lastContact);
        label->setObjectName(QString::fromUtf8("label"));

        horizontalLayout->addWidget(label);

        horizontalSpacer = new QSpacerItem(40, 20, QSizePolicy::Expanding, QSizePolicy::Minimum);

        horizontalLayout->addItem(horizontalSpacer);

        closeButton = new QPushButton(lastContact);
        closeButton->setObjectName(QString::fromUtf8("closeButton"));

        horizontalLayout->addWidget(closeButton);


        gridLayout->addLayout(horizontalLayout, 1, 0, 1, 1);


        retranslateUi(lastContact);

        QMetaObject::connectSlotsByName(lastContact);
    } // setupUi

    void retranslateUi(QDialog *lastContact)
    {
        lastContact->setWindowTitle(QApplication::translate("lastContact", "Last contact dialog", 0, QApplication::UnicodeUTF8));
        label->setText(QApplication::translate("lastContact", "Search Call:", 0, QApplication::UnicodeUTF8));
        closeButton->setText(QApplication::translate("lastContact", "Close", 0, QApplication::UnicodeUTF8));
    } // retranslateUi

};

namespace Ui {
    class lastContact: public Ui_lastContact {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_LASTCONTACT_H
