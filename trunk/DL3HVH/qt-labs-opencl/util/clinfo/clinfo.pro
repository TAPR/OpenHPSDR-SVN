TARGET = clinfo

SOURCES += \
    clinfo.cpp \

LIBS += -L../../lib -L../../bin

include(../../src/opencl/opencl_dep.pri)
