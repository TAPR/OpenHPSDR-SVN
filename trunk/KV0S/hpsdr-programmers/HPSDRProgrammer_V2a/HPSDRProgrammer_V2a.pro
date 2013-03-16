#-------------------------------------------------
#
# Project created by QtCreator 2013-03-16T12:08:51
#
#-------------------------------------------------

QT       += core gui network

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = HPSDRProgrammer_V2a
TEMPLATE = app

# Included PRI files
include( ../Help/Help.pri )
include( ./hpsdr/hpsdr.pri)
include( ../Status/Status.pri )
include( ../Resources/Resources.pri )


SOURCES += main.cpp\
        mainwindow.cpp

HEADERS  += mainwindow.h \
    version.h

FORMS    += mainwindow.ui
