#-------------------------------------------------
#
# Project created by QtCreator 2012-12-29T10:17:39
#
#-------------------------------------------------

QT       += core gui network

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = HPSDRProgrmmer_V2
TEMPLATE = app

win32 {
    INCLUDEPATH += "c:\WpdPack\Include"
    LIBS += "c:\WpdPack\Lib\wpcap.lib" /Qt/2010.05/mingw/lib/libws2_32.a
 }

macx {
    LIBS += -framework Security -lpcap
    INCLUDEPATH += "/System/Library/Frameworks/Security.framework/Headers"
}

unix {
    LIBS += -lpcap
}

SOURCES += main.cpp\
        mainwindow.cpp \
    aboutdialog.cpp \
    Interfaces.cpp \
    Discovery.cpp \
    ReceiveThread.cpp \
    RawReceiveThread.cpp \
    Metis.cpp

HEADERS  += mainwindow.h \
    aboutdialog.h \
    Interfaces.h \
    Discovery.h \
    ReceiveThread.h \
    RawReceiveThread.h \
    Metis.h \
    Version.h

FORMS    += mainwindow.ui \
    aboutdialog.ui

RESOURCES += \
    HPSDRProgrammer_Res.qrc
