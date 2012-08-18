#-------------------------------------------------
#
# Project created by QtCreator 2012-01-28T15:10:13
#
#-------------------------------------------------

QT       += core gui network

TARGET = ../../bin/QtHPSDRServer
TEMPLATE = app

win32 {
LIBS += -lws2_32
}

SOURCES += main.cpp\
        mainwindow.cpp \
    client/client.cpp \
    hpsdr/server.cpp \
    client/clientlistener.cpp \
    hpsdr/alex.cpp \
    hpsdr/discovery.cpp \
    hpsdr/error.cpp \
    hpsdr/interfaces.cpp \
    hpsdr/metis.cpp \
    hpsdr/receiver.cpp \
    hpsdr/xvtr.cpp \
    about.cpp

HEADERS  += mainwindow.h \
    client/client.h \
    hpsdr/server.h \
    client/clientlistener.h \
    hpsdr/alex.h \
    hpsdr/discovery.h \
    hpsdr/error.h \
    hpsdr/interfaces.h \
    hpsdr/metis.h \
    hpsdr/receiver.h \
    hpsdr/xvtr.h \
    about.h

FORMS    += mainwindow.ui
