QT       += core gui network

win32 {
LIBS += -lws2_32
}

SOURCES += $$PWD/discovery.cpp \
    $$PWD/error.cpp \
    $$PWD/interfaces.cpp \
    $$PWD/board.cpp 
   

    
HEADERS  += $$PWD/discovery.h \
    $$PWD/error.h \
    $$PWD/interfaces.h \
    $$PWD/board.h 
   
    
