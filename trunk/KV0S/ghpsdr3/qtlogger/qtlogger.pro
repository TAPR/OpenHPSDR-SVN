#-------------------------------------------------
#
# Project created by QtCreator 2011-08-21T11:31:13
#
#-------------------------------------------------

QT       += core gui network xml

TARGET = QtLogger
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    data.cpp \
    about.cpp \
    help.cpp \
    adddialog.cpp \
    adifhandler.cpp \
    range.cpp \
    loghandler.cpp \
    adif.cpp

HEADERS  += mainwindow.h \
    about.h \
    data.h \
    help.h \
    adddialog.h \
    adifhandler.h \
    range.h \
    loghandler.h \
    adif.h

FORMS    += mainwindow.ui \
    help.ui \
    adddialog.ui \

OTHER_FILES += \
    help/daveStyle.css \
    help/QtLogger.html

RESOURCES += \
    resources.qrc











