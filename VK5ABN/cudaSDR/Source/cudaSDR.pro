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
    CUDA_ARCH = sm_30

x86_64 { HOST_ARCH = x86_64-linux }
armv7l { HOST_ARCH = armv7-linux-gnueabihf }

    INCLUDEPATH += \
        ./ \
        $$PWD/src/ \
        $$PWD/src/AudioEngine \
        $$PWD/src/CL \
        $$PWD/src/Cuda \
        $$PWD/src/DataEngine \
        $$PWD/src/GL \
        $$PWD/src/QtDSP \
        $$PWD/src/Util \
        $$CUDA_DIR/targets/$$HOST_ARCH/include \
        $$CUDA_DIR/include

    LIBS += \
        -lcudart \
        -lcufftw \
        -lcufft \
        -lasound \
        -L$$CUDA_DIR/targets/$$HOST_ARCH/lib \
        -L$$CUDA_DIR/lib64

    CUDA_INC = $$join(INCLUDEPATH,' -I','-I',' ')

    NVCCFLAGS = --compiler-options -fno-strict-aliasing -use_fast_math --ptxas-options=-v

    cuda.commands = $$CUDA_DIR/bin/nvcc -O3 -arch=$$CUDA_ARCH -c $$NVCCFLAGS \
                    $$CUDA_INC $$LIBS  ${QMAKE_FILE_NAME} -o ${QMAKE_FILE_OUT}

    cuda.dependency_type = TYPE_C
    cuda.depend_command = $$CUDA_DIR/bin/nvcc -O3 -M $$CUDA_INC $$NVCCFLAGS ${QMAKE_FILE_NAME}
    cuda.input = CUDA_SOURCES
    cuda.output = ${OBJECTS_DIR}${QMAKE_FILE_BASE}.o

    QMAKE_EXTRA_COMPILERS += cuda
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
