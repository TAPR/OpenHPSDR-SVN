/********************************************************************************
** Form generated from reading UI file 'Bandscope.ui'
**
** Created: Tue Oct 12 16:23:49 2010
**      by: Qt User Interface Compiler version 4.6.2
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_BANDSCOPE_H
#define UI_BANDSCOPE_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QFrame>
#include <QtGui/QHeaderView>

QT_BEGIN_NAMESPACE

class Ui_Bandscope
{
public:

    void setupUi(QFrame *Bandscope)
    {
        if (Bandscope->objectName().isEmpty())
            Bandscope->setObjectName(QString::fromUtf8("Bandscope"));
        Bandscope->resize(600, 92);
        Bandscope->setFrameShape(QFrame::StyledPanel);
        Bandscope->setFrameShadow(QFrame::Raised);

        retranslateUi(Bandscope);

        QMetaObject::connectSlotsByName(Bandscope);
    } // setupUi

    void retranslateUi(QFrame *Bandscope)
    {
        Bandscope->setWindowTitle(QApplication::translate("Bandscope", "Frame", 0, QApplication::UnicodeUTF8));
    } // retranslateUi

};

namespace Ui {
    class Bandscope: public Ui_Bandscope {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_BANDSCOPE_H
