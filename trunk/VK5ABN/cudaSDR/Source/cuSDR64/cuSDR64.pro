QT += core gui network multimedia opengl

TARGET = cuSDR64
TEMPLATE = app


CONFIG += debug
CONFIG += qt warn_on
CONFIG += console
CONFIG += mobility
	
MOBILITY += multimedia
#message(CONFIG: $$CONFIG)

include(cuSDR.pri)

RESOURCES += res/cusdr.qrc

win32:RC_FILE += res/cusdr.rc

#DEFINES += AUDIO_ENGINE_DEBUG
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
	src/AudioEngine \
	src/CL \
	src/DataEngine \
	src/GL \
	src/QtDSP \
	src/Util \
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

OBJECTS_DIR = ./bld/o
MOC_DIR = ./bld/moc
#UI_DIR = ./bld/ui
RCC_DIR = ./bld/rcc
DESTDIR = ./bin
