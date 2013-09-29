
QT       += core gui network

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets



SOURCES += $$PWD/addressdialogprog.cpp \
           $$PWD/board.cpp \
           $$PWD/interfaces.cpp \
           $$PWD/writeboard.cpp

HEADERS  += $$PWD/addressdialogprog.h \
	    $$PWD/board.h \
            $$PWD/interfaces.h \
            $$PWD/writeboard.h \
    hpsdr/timeouts.h

FORMS    += $$PWD/addressdialogprog.ui 
            


    
