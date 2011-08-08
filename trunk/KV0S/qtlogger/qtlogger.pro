#-------------------------------------------------
#
# Project created by QtCreator 2011-04-23T13:36:54
#
#-------------------------------------------------

QT       += core gui xml network

TARGET = qtlogger
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    adif.cpp \
    about.cpp \
    loaddata.cpp \
    savedata.cpp \
    adddialog.cpp \
    adifhandler.cpp \
    loghandler.cpp

HEADERS  += mainwindow.h \
    adif.h \
    about.h \
    loaddata.h \
    savedata.h \
    adddialog.h \
    adifhandler.h \
    loghandler.h

FORMS    += mainwindow.ui \
    addDialog.ui \

OTHER_FILES += \
    adif_codes_2.2.7.xml \
    qtlogger.xsl
