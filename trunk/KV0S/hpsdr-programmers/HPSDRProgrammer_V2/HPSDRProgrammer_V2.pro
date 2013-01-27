#-------------------------------------------------
#
# Project created by QtCreator 2012-12-29T10:17:39
#
#-------------------------------------------------

QT       += core gui network

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = HPSDRProgrmmer_V2
TEMPLATE = app

# Included PRI files
include( ../Help/Help.pri )
include( ../Status/Status.pri )
include( ../Programmer/Programmer.pri )
include( ../IPaddress/IPaddress.pri )
include( ../Resources/Resources.pri )


win32 {
    INCLUDEPATH += "c:/WpdPack/Include"
    LIBS += "c:/WpdPack/Lib/wpcap.lib" C:/MinGW/lib/libws2_32.a
 }

macx {
    LIBS += -framework Security -lpcap
    INCLUDEPATH += "/System/Library/Frameworks/Security.framework/Headers"
}

unix {
    LIBS += -lpcap
}



SOURCES += main.cpp\
        mainwindow.cpp


HEADERS  += mainwindow.h


FORMS    += mainwindow.ui






