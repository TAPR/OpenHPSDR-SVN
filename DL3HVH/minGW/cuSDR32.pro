QT += core gui multimedia network opengl

TARGET = cuSDR32
TEMPLATE = app

CONFIG += qt warn_on
#CONFIG += debug
#CONFIG += console
CONFIG += release

#message(CONFIG: $$CONFIG)

DEFINES += _WINDOWS
DEFINES += _CRT_SECURE_NO_WARNINGS
DEFINES += QT_LARGEFILE_SUPPORT 
DEFINES += QT_DLL 
DEFINES += QT_MULTIMEDIA_LIB 
DEFINES += QT_OPENGL_LIB 
DEFINES += QT_NETWORK_LIB 
DEFINES += QT_HAVE_MMX 
DEFINES += QT_HAVE_3DNOW 
DEFINES += QT_HAVE_SSE 
DEFINES += QT_HAVE_MMXEXT 
DEFINES += QT_HAVE_SSE2

INCLUDEPATH += \
    $(NVSDKCOMPUTE_ROOT)/OpenCL/common/inc \
    ./src/CL
	
LIBS += \
	-L"$$(NVSDKCOMPUTE_ROOT)/OpenCL/common/lib" \
    -L"./lib/Win32" \
	-lwsock32 \
	-lVersion \
	-lPsapi \
	-lQtOpenCL \
	-lopengl32 \
    -llibfftw \
    -llibfftw3f-3 \
    -lgdi32 \
    -luser32 \
    -lKernel32
	
QCLEANFILES = \
	Makefile \
	Makefile.Debug \
	Makefile.Release \
	object_script.cuSDR32.Debug \
	object_script.cuSDR32.Release
	
win32:QCLEANFILES += bin/cuSDR32.exe

#DEPENDPATH += .

DESTDIR = bin
OBJECTS_DIR = bld/o
MOC_DIR = bld/moc
UI_DIR = bld/ui
RCC_DIR = bld/rcc

include(cuSDR32.pri)

#QMAKE_CXXFLAGS_MP = -MP
