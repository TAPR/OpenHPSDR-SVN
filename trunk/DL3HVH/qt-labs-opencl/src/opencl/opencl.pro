TEMPLATE = lib
TARGET = QtOpenCL
gcov {
    CONFIG += staticlib
} else {
    CONFIG += dll
}
CONFIG += warn_on
win32 { 
    DESTDIR = ../../bin
    !static:DEFINES += QT_MAKEDLL
}
else:DESTDIR = ../../lib

win32 {
    !isEmpty(QMAKE_INCDIR_OPENCL) {
        QMAKE_CXXFLAGS += -I$$QMAKE_INCDIR_OPENCL
    }
    !isEmpty(QMAKE_LIBDIR_OPENCL) {
        LIBS += -L$$QMAKE_LIBDIR_OPENCL
    }
    !isEmpty(QMAKE_LIBS_OPENCL) {
        LIBS += $$QMAKE_LIBS_OPENCL
    } else {
        LIBS += -lOpenCL
    }
}

macx:!opencl_configure {
    LIBS += -framework OpenCL
}

HEADERS += \
    qclbuffer.h \
    qclcommandqueue.h \
    qclcontext.h \
    qcldevice.h \
    qclevent.h \
    qclglobal.h \
    qclimage.h \
    qclimageformat.h \
    qclkernel.h \
    qclmemoryobject.h \
    qclplatform.h \
    qclprogram.h \
    qclsampler.h \
    qcluserevent.h \
    qclvector.h \
    qclworksize.h

SOURCES += \
    qclbuffer.cpp \
    qclcommandqueue.cpp \
    qclcontext.cpp \
    qcldevice.cpp \
    qclevent.cpp \
    qclimage.cpp \
    qclimageformat.cpp \
    qclkernel.cpp \
    qclmemoryobject.cpp \
    qclplatform.cpp \
    qclprogram.cpp \
    qclsampler.cpp \
    qcluserevent.cpp \
    qclvector.cpp \
    qclworksize.cpp

PRIVATE_HEADERS += \
    qclext_p.h

HEADERS += $$PRIVATE_HEADERS
DEFINES += QT_BUILD_CL_LIB

opencl_1_1 {
    DEFINES += QT_OPENCL_1_1
}
