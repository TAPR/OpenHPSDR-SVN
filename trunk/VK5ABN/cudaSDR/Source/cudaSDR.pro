QT += core gui network multimedia opengl

TARGET = cudaSDR
TEMPLATE = app

CONFIG += qt warn_on
CONFIG += console
CONFIG += mobility
CONFIG += $$QMAKE_HOST.arch
message(CONFIG: $$QMAKE_HOST.arch)

MOBILITY += multimedia

include(cudaSDR.pri)

RESOURCES += res/cusdr.qrc

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

unix {
    CUDA_DIR = $$system(which nvcc | sed 's,/bin/nvcc$,,')

x86_64 { HOST_ARCH = x86_64-linux }
arm7   { HOST_ARCH = armv7-linux-gnueabihf }

    INCLUDEPATH += \
        ./ \
        src/ \
        src/AudioEngine \
        src/CL \
        src/Cuda \
        src/DataEngine \
        src/GL \
        src/QtDSP \
        src/Util \
        $$CUDA_DIR/targets/$$HOST_ARCH/include

    LIBS += \
        -lcudart \
        -lcufftw \
        -lcufft \
        -L$$CUDA_DIR/targets/$$HOST_ARCH/lib
}

# ********************************************************
# The WIN32 configuration needs to be checked and updated
# ********************************************************
win32 {
    RC_FILE += res/cusdr.rc

    INCLUDEPATH += \
	./ \
	src/ \
	src/AudioEngine \
	src/CL \
        src/Cuda \
	src/DataEngine \
	src/GL \
	src/QtDSP \
	src/Util \
        $(CUDA_PATH_V6_5)/include
	
    LIBS += \
        -L"$$(CUDA_PATH_V6_5)/lib/x64" \
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
}

OBJECTS_DIR = ./bld/o
MOC_DIR = ./bld/moc
#UI_DIR = ./bld/ui
RCC_DIR = ./bld/rcc
DESTDIR = ./bin
