#-------------------------------------------------
#
# Project created by QtCreator 2012-12-29T10:17:39
#
#-------------------------------------------------

QT       += core gui network

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = HPSDRProgrammer_V2
TEMPLATE = app

# Included PRI files
include( ../Help/Help.pri )
include( ../Status/Status.pri )
include( ../Programmer/Programmer.pri )
include( ../IPaddress/IPaddressProg.pri )
include( ../Resources/Resources.pri )


SOURCES += main.cpp\
        mainwindow.cpp


HEADERS  += mainwindow.h \
            version.h


FORMS    += mainwindow.ui








