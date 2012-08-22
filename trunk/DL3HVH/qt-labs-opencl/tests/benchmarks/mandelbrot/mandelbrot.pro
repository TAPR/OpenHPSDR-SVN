TEMPLATE=app
QT += testlib
CONFIG += unittest warn_on

SOURCES += tst_mandelbrot.cpp
RESOURCES += mandelbrot.qrc

LIBS += -L../../../lib -L../../../bin

include(../../../src/opencl/opencl_dep.pri)
