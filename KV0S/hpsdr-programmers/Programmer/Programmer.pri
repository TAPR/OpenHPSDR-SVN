#-------------------------------------------------
#
# Project created by QtCreator 2012-12-29T10:17:39
#
#-------------------------------------------------

QT       += core gui network

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

win32 {
    INCLUDEPATH += "c:/WpdPack/Include"
    LIBS += "c:/WpdPack/Lib/wpcap.lib"  "C:/Qt/4.8.4/tools/MinGW-gcc-4.4.0-2/mingw/lib/libws2_32.a"
 }

macx {
    LIBS += -framework Security -lpcap
    INCLUDEPATH += "/System/Library/Frameworks/Security.framework/Headers"
}

unix {
    LIBS += -lpcap
}

SOURCES += $$PWD/board.cpp \
    $$PWD/discovery.cpp \
    $$PWD/interfaces.cpp \
    $$PWD/rawreceivethread.cpp \
    $$PWD/receivethread.cpp
   
HEADERS  += $$PWD/board.h \
    $$PWD/discovery.h \
    $$PWD/interfaces.h \
    $$PWD/rawreceivethread.h \
    $$PWD/receivethread.h

