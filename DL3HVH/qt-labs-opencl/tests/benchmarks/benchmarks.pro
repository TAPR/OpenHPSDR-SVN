TEMPLATE = subdirs
SUBDIRS += mandelbrot overhead
contains(QT_CONFIG, private_tests): SUBDIRS += blur
