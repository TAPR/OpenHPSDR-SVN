QT += core gui network opengl

TARGET = cuSDR64
TEMPLATE = app

QT_VERSION = $$[QT_VERSION]
QT_VERSION = $$split(QT_VERSION, ".")
QT_VER_MAJ = $$member(QT_VERSION, 0)
QT_VER_MIN = $$member(QT_VERSION, 1)
lessThan(QT_VER_MAJ, 4) | lessThan(QT_VER_MIN, 7) {
   error(cuSDR requires Qt 4.7 or newer but Qt $$[QT_VERSION] was detected.)
}

CONFIG += qt warn_on
CONFIG += console
CONFIG += mobility

MOBILITY += multimedia

#message(CONFIG: $$CONFIG)

include(cuSDR.pri)

RESOURCES += res/cusdr.qrc

win32:RC_FILE += res/cusdr.rc

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
