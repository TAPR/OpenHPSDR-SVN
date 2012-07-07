TEMPLATE = app
TARGET = cuSDR64
QT += core gui multimedia network opengl

CONFIG += qt warn_on
#CONFIG += debug
CONFIG += console
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
	$(QTDIR)/mkspecs/win32-msvc2010 \
    $(NVSDKCOMPUTE_ROOT)/OpenCL/common/inc \
    ./src/CL \
    ./bld/moc
	
LIBS += \
	-L"$(NVSDKCOMPUTE_ROOT)/OpenCL/common/lib/x64" \
    -L"./lib/x64/Debug" \
	-lwsock32 \
	-lVersion \
	-lPsapi \
	-lQtOpenCL \
	-lqtmaind \
	-lQtCored4 \
	-lQtGuid4 \
	-lQtMultimediad4 \
	-lQtOpenGLd4 \
	-lQtNetworkd4 \
	-lopengl32 \
    -llibfftw \
    -llibfftw3f-3_x64 \
    -lgdi32 \
    -luser32 \
    -lKernel32
	
DEPENDPATH += .

DESTDIR = ./bin
OBJECTS_DIR = ./bld/o
MOC_DIR = ./bld/moc
UI_DIR = ./bld/ui
RCC_DIR = ./bld/rcc

include(cuSDR64Test.pri)

win32:RC_FILE = ./res/cuSDR.rc

QMAKE_CXXFLAGS_MP = -MP
