# This file is generated automatically. Do not edit.
# Use project properties -> Build -> Qt -> Expert -> Custom Definitions.
TEMPLATE = app
DESTDIR = dist/Debug/GNU-Linux-x86
TARGET = QtRadio
VERSION = 1.0.0
CONFIG -= debug_and_release app_bundle lib_bundle
CONFIG += release 
QT = core gui network
SOURCES += Connection.cpp FMNFilters.cpp SAMFilters.cpp main.cpp CWLFilters.cpp LSBFilters.cpp Waterfall.cpp Filters.cpp Configure.cpp DSBFilters.cpp USBFilters.cpp Band.cpp Spectrum.cpp DIGLFilters.cpp CWUFilters.cpp DIGUFilters.cpp Audio.cpp Mode.cpp BandStackEntry.cpp FiltersBase.cpp AMFilters.cpp Filter.cpp UI.cpp
HEADERS += Configure.h CWLFilters.h FiltersBase.h CWUFilters.h FMNFilters.h AMFilters.h DSBFilters.h DIGUFilters.h USBFilters.h DIGLFilters.h Band.h UI.h Audio.h BandStackEntry.h Connection.h Filter.h Filters.h Mode.h SAMFilters.h Waterfall.h Spectrum.h LSBFilters.h
FORMS += UI.ui Configure.ui
RESOURCES +=
TRANSLATIONS +=
OBJECTS_DIR = build/Debug/GNU-Linux-x86
MOC_DIR = 
RCC_DIR = 
UI_DIR = 
QMAKE_CC = gcc
QMAKE_CXX = g++
DEFINES += 
INCLUDEPATH += 
LIBS += -lQtMultimedia  
