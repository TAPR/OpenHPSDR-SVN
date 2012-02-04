QT = core network
TARGET = ../../bin/QtDSPServer
TEMPLATE = app
CONFIG += CONSOLE
CONFIG += debug
LIBS += -lfftw3f

QMAKE_CXXFLAGS += -fpermissive

mac:CONFIG-=app_bundle

HEADERS = \
    static.h \
    startup.h \
    dttsp.h \
    hpsdr/error.h \
    hpsdr/connection.h \
    hpsdr/data.h \
    client/client.h \
    client/clientlistener.h \
    client/g711a.h \
    client/g711u.h \
    client/g721.h

SOURCES = main.cpp \
    static.cpp \
    startup.cpp \
    hpsdr/error.cpp \
    hpsdr/connection.cpp \
    hpsdr/data.cpp \
    client/client.cpp \
    client/clientlistener.cpp \
    client/g711a.cpp \
    client/g711u.cpp \
    client/g721.cpp


include(../DttSP/DttSP.pri)
