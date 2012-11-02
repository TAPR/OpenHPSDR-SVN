QT += core gui multimedia network opengl

TARGET = cuSDR32
TEMPLATE = app

CONFIG += qt warn_on
#CONFIG += console

include(cuSDR.pri)
	
RESOURCES += res/cusdr.qrc

win32:RC_FILE += res/cusdr.rc

INCLUDEPATH += \
    $(CUDA_INC_PATH) \
    src/ \
    src/CL
	
LIBS += \
	-L"$$(CUDA_LIB_PATH)/Win32" \
    -L"./lib" \
	-lwsock32 \
	-lVersion \
	-lPsapi \
	#-lQtOpenCL \
	-lopengl32 \
    -llibfftw3f-3 \
    -lgdi32 \
    -luser32 \
    -lKernel32
	

debug {

	OBJECTS_DIR = ./bld/debug/o
	MOC_DIR = ./bld/debug/moc
	#UI_DIR = ./bld/debug/ui
	RCC_DIR = ./bld/debug/rcc
	DESTDIR = ./bin/debug
}

release {

	OBJECTS_DIR = ./bld/release/o
	MOC_DIR = ./bld/release/moc
	#UI_DIR = ./bld/release/ui
	RCC_DIR = ./bld/release/rcc
	DESTDIR = ./bin/release	
}	
	


