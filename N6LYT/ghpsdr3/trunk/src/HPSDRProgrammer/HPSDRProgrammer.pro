#-------------------------------------------------
#
# Project created by QtCreator 2010-12-14T14:22:05
#
#-------------------------------------------------

QT       += core gui network

TARGET = HPSDRProgrammer
TEMPLATE = app

win32 {
    LIBS += wpcap.lib /Qt/2010.04/mingw/lib/libws2_32.a
} else {
    LIBS += -lpcap
}

SOURCES += main.cpp\
        mainwindow.cpp \
    Interfaces.cpp \
    RawReceiveThread.cpp \
    DiscoveryThread.cpp \
    ReceiveThread.cpp \
    Metis.cpp

HEADERS  += mainwindow.h \
    Interfaces.h \
    RawReceiveThread.h \
    DiscoveryThread.h \
    ReceiveThread.h \
    Metis.h

FORMS    += mainwindow.ui
