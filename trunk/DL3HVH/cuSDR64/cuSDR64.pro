QT += core gui multimedia network opengl

TARGET = cuSDR64
TEMPLATE = app

CONFIG += qt warn_on
#CONFIG += console

#message(CONFIG: $$CONFIG)

include(cuSDR.pri)

RESOURCES += res/cusdr.qrc

win32:RC_FILE += res/cusdr.rc

#DEFINES += _WINDOWS
#DEFINES += _CRT_SECURE_NO_WARNINGS
#DEFINES += QT_LARGEFILE_SUPPORT 
#DEFINES += QT_DLL 
#DEFINES += QT_MULTIMEDIA_LIB 
#DEFINES += QT_OPENGL_LIB 
#DEFINES += QT_NETWORK_LIB 
#DEFINES += QT_HAVE_MMX 
#DEFINES += QT_HAVE_3DNOW 
#DEFINES += QT_HAVE_SSE 
#DEFINES += QT_HAVE_MMXEXT 
#DEFINES += QT_HAVE_SSE2

unix:INCLUDEPATH += \
	./ \
	src/ \
	src/AudioEngine \
	src/CL \
	src/DataEngine \
	src/GL \
	src/QtDSP \
	src/Util

win32:INCLUDEPATH += \
	./ \
	src/ \
	src/CL \
	$(CUDA_PATH_V5_0)/include
	
unix:LIBS += -lfftw3f

win32:LIBS += \
	-L"$$(CUDA_PATH_V5_0)/lib/x64" \
	-L"./lib" \
	-lwsock32 \
	-lVersion \
	-lPsapi \
	-lQtOpenCL \
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
