/********************************************************************************
** Form generated from reading UI file 'help.ui'
**
** Created: Tue Mar 20 21:55:51 2012
**      by: Qt User Interface Compiler version 4.8.0
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_HELP_H
#define UI_HELP_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QDialog>
#include <QtGui/QGridLayout>
#include <QtGui/QHeaderView>
#include <QtGui/QPushButton>
#include <QtGui/QSpacerItem>
#include <QtGui/QTextBrowser>

QT_BEGIN_NAMESPACE

class Ui_Help
{
public:
    QGridLayout *gridLayout_2;
    QGridLayout *gridLayout;
    QPushButton *homeButton;
    QPushButton *backButton;
    QPushButton *quitButton;
    QTextBrowser *textBrowser;
    QSpacerItem *horizontalSpacer;

    void setupUi(QDialog *Help)
    {
        if (Help->objectName().isEmpty())
            Help->setObjectName(QString::fromUtf8("Help"));
        Help->resize(590, 485);
        gridLayout_2 = new QGridLayout(Help);
        gridLayout_2->setObjectName(QString::fromUtf8("gridLayout_2"));
        gridLayout = new QGridLayout();
        gridLayout->setObjectName(QString::fromUtf8("gridLayout"));
        homeButton = new QPushButton(Help);
        homeButton->setObjectName(QString::fromUtf8("homeButton"));

        gridLayout->addWidget(homeButton, 0, 0, 1, 1);

        backButton = new QPushButton(Help);
        backButton->setObjectName(QString::fromUtf8("backButton"));

        gridLayout->addWidget(backButton, 0, 2, 1, 1);

        quitButton = new QPushButton(Help);
        quitButton->setObjectName(QString::fromUtf8("quitButton"));

        gridLayout->addWidget(quitButton, 0, 4, 1, 1);

        textBrowser = new QTextBrowser(Help);
        textBrowser->setObjectName(QString::fromUtf8("textBrowser"));

        gridLayout->addWidget(textBrowser, 2, 0, 1, 5);

        horizontalSpacer = new QSpacerItem(40, 20, QSizePolicy::Expanding, QSizePolicy::Minimum);

        gridLayout->addItem(horizontalSpacer, 0, 3, 1, 1);


        gridLayout_2->addLayout(gridLayout, 0, 0, 1, 1);


        retranslateUi(Help);

        QMetaObject::connectSlotsByName(Help);
    } // setupUi

    void retranslateUi(QDialog *Help)
    {
        Help->setWindowTitle(QApplication::translate("Help", "Help", 0, QApplication::UnicodeUTF8));
        homeButton->setText(QApplication::translate("Help", "Home", 0, QApplication::UnicodeUTF8));
        backButton->setText(QApplication::translate("Help", "Back", 0, QApplication::UnicodeUTF8));
        quitButton->setText(QApplication::translate("Help", "Quit", 0, QApplication::UnicodeUTF8));
    } // retranslateUi

};

namespace Ui {
    class Help: public Ui_Help {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_HELP_H
