#-------------------------------------------------
#
# Project created by QtCreator 2012-12-29T10:17:39
#
#-------------------------------------------------

QT       += core gui network opengl

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = HPSDRBootloader
TEMPLATE = app

# Included PRI files
include( ../Help/Help.pri )
include( ../Status/Status.pri )
include( ../Programmer/Programmer.pri )
include( ../IPaddress/IPaddressBoot.pri )
include( ../Resources/Resources.pri )


win32 {
    INCLUDEPATH += "C:/src/WpdPack/Include"
    LIBS += "C:/src/WpdPack/Lib/wpcap.lib" "C:/Qt/Tools/mingw491_32/i686-w64-mingw32/lib/libwsock32.a"
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


HEADERS  += mainwindow.h \
    version.h


FORMS    += mainwindow.ui






