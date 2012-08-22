TEMPLATE = lib
TARGET = QtOpenCLGL
QT += opengl
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

LIBS += -L../../lib -L../../bin
win32 {
    LIBS += -lQtOpenCL
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
} else {
    LIBS += -lQtOpenCL
}
macx:!opencl_configure {
    LIBS += -framework OpenCL
}

no_cl_gl {
    DEFINES += QT_NO_CL_OPENGL
}

INCLUDEPATH += $$PWD/../opencl

HEADERS += \
    qclcontextgl.h

SOURCES += \
    qclcontextgl.cpp

PRIVATE_HEADERS += \
    qcl_gl_p.h

HEADERS += $$PRIVATE_HEADERS
DEFINES += QT_BUILD_CLGL_LIB
