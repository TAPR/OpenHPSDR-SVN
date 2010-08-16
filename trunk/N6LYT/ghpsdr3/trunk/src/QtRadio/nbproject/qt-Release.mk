#############################################################################
# Makefile for building: dist/Release/GNU-Linux-x86/QtRadio
# Generated by qmake (2.01a) (Qt 4.6.2) on: Mon Aug 16 18:19:51 2010
# Project:  nbproject/qt-Release.pro
# Template: app
# Command: /usr/bin/qmake -unix VPATH=. -o qttmp-Release.mk nbproject/qt-Release.pro
#############################################################################

####### Compiler, tools and options

CC            = gcc
CXX           = g++
DEFINES       = -DQT_NO_DEBUG -DQT_GUI_LIB -DQT_NETWORK_LIB -DQT_CORE_LIB -DQT_SHARED
CFLAGS        = -pipe -O2 -Wall -W -D_REENTRANT $(DEFINES)
CXXFLAGS      = -pipe -O2 -Wall -W -D_REENTRANT $(DEFINES)
INCPATH       = -I/usr/share/qt4/mkspecs/linux-g++ -Inbproject -I/usr/include/qt4/QtCore -I/usr/include/qt4/QtNetwork -I/usr/include/qt4/QtGui -I/usr/include/qt4 -I. -I. -Inbproject -I.
LINK          = g++
LFLAGS        = -Wl,-O1
LIBS          = $(SUBLIBS)  -L/usr/lib -Wl,-rpath /usr/lib /usr/lib/libQtMultimedia.so.4.6.2 -lQtGui -lQtNetwork -lQtCore -lpthread 
AR            = ar cqs
RANLIB        = 
QMAKE         = /usr/bin/qmake
TAR           = tar -cf
COMPRESS      = gzip -9f
COPY          = cp -f
SED           = sed
COPY_FILE     = $(COPY)
COPY_DIR      = $(COPY) -r
STRIP         = strip
INSTALL_FILE  = install -m 644 -p
INSTALL_DIR   = $(COPY_DIR)
INSTALL_PROGRAM = install -m 755 -p
DEL_FILE      = rm -f
SYMLINK       = ln -f -s
DEL_DIR       = rmdir
MOVE          = mv -f
CHK_DIR_EXISTS= test -d
MKDIR         = mkdir -p

####### Output directory

OBJECTS_DIR   = build/Release/GNU-Linux-x86/

####### Files

SOURCES       = Connection.cpp \
		FMNFilters.cpp \
		SAMFilters.cpp \
		main.cpp \
		CWLFilters.cpp \
		LSBFilters.cpp \
		Waterfall.cpp \
		Filters.cpp \
		DSBFilters.cpp \
		USBFilters.cpp \
		Band.cpp \
		Spectrum.cpp \
		DIGLFilters.cpp \
		CWUFilters.cpp \
		DIGUFilters.cpp \
		Audio.cpp \
		Mode.cpp \
		BandStackEntry.cpp \
		FiltersBase.cpp \
		AMFilters.cpp \
		Filter.cpp \
		UI.cpp moc_FiltersBase.cpp \
		moc_Band.cpp \
		moc_UI.cpp \
		moc_Audio.cpp \
		moc_BandStackEntry.cpp \
		moc_Connection.cpp \
		moc_Filter.cpp \
		moc_Filters.cpp \
		moc_Mode.cpp \
		moc_Spectrum.cpp \
		moc_Waterfall.cpp
OBJECTS       = build/Release/GNU-Linux-x86/Connection.o \
		build/Release/GNU-Linux-x86/FMNFilters.o \
		build/Release/GNU-Linux-x86/SAMFilters.o \
		build/Release/GNU-Linux-x86/main.o \
		build/Release/GNU-Linux-x86/CWLFilters.o \
		build/Release/GNU-Linux-x86/LSBFilters.o \
		build/Release/GNU-Linux-x86/Waterfall.o \
		build/Release/GNU-Linux-x86/Filters.o \
		build/Release/GNU-Linux-x86/DSBFilters.o \
		build/Release/GNU-Linux-x86/USBFilters.o \
		build/Release/GNU-Linux-x86/Band.o \
		build/Release/GNU-Linux-x86/Spectrum.o \
		build/Release/GNU-Linux-x86/DIGLFilters.o \
		build/Release/GNU-Linux-x86/CWUFilters.o \
		build/Release/GNU-Linux-x86/DIGUFilters.o \
		build/Release/GNU-Linux-x86/Audio.o \
		build/Release/GNU-Linux-x86/Mode.o \
		build/Release/GNU-Linux-x86/BandStackEntry.o \
		build/Release/GNU-Linux-x86/FiltersBase.o \
		build/Release/GNU-Linux-x86/AMFilters.o \
		build/Release/GNU-Linux-x86/Filter.o \
		build/Release/GNU-Linux-x86/UI.o \
		build/Release/GNU-Linux-x86/moc_FiltersBase.o \
		build/Release/GNU-Linux-x86/moc_Band.o \
		build/Release/GNU-Linux-x86/moc_UI.o \
		build/Release/GNU-Linux-x86/moc_Audio.o \
		build/Release/GNU-Linux-x86/moc_BandStackEntry.o \
		build/Release/GNU-Linux-x86/moc_Connection.o \
		build/Release/GNU-Linux-x86/moc_Filter.o \
		build/Release/GNU-Linux-x86/moc_Filters.o \
		build/Release/GNU-Linux-x86/moc_Mode.o \
		build/Release/GNU-Linux-x86/moc_Spectrum.o \
		build/Release/GNU-Linux-x86/moc_Waterfall.o
DIST          = /usr/share/qt4/mkspecs/common/g++.conf \
		/usr/share/qt4/mkspecs/common/unix.conf \
		/usr/share/qt4/mkspecs/common/linux.conf \
		/usr/share/qt4/mkspecs/qconfig.pri \
		/usr/share/qt4/mkspecs/features/qt_functions.prf \
		/usr/share/qt4/mkspecs/features/qt_config.prf \
		/usr/share/qt4/mkspecs/features/exclusive_builds.prf \
		/usr/share/qt4/mkspecs/features/default_pre.prf \
		/usr/share/qt4/mkspecs/features/release.prf \
		/usr/share/qt4/mkspecs/features/default_post.prf \
		/usr/share/qt4/mkspecs/features/warn_on.prf \
		/usr/share/qt4/mkspecs/features/qt.prf \
		/usr/share/qt4/mkspecs/features/unix/thread.prf \
		/usr/share/qt4/mkspecs/features/moc.prf \
		/usr/share/qt4/mkspecs/features/resources.prf \
		/usr/share/qt4/mkspecs/features/uic.prf \
		/usr/share/qt4/mkspecs/features/yacc.prf \
		/usr/share/qt4/mkspecs/features/lex.prf \
		/usr/share/qt4/mkspecs/features/include_source_dir.prf \
		nbproject/qt-Release.pro
QMAKE_TARGET  = QtRadio
DESTDIR       = dist/Release/GNU-Linux-x86/
TARGET        = dist/Release/GNU-Linux-x86/QtRadio

first: all
####### Implicit rules

.SUFFIXES: .o .c .cpp .cc .cxx .C

.cpp.o:
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o "$@" "$<"

.cc.o:
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o "$@" "$<"

.cxx.o:
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o "$@" "$<"

.C.o:
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o "$@" "$<"

.c.o:
	$(CC) -c $(CFLAGS) $(INCPATH) -o "$@" "$<"

####### Build rules

all: qttmp-Release.mk $(TARGET)

$(TARGET): ui_UI.h $(OBJECTS)  
	@$(CHK_DIR_EXISTS) dist/Release/GNU-Linux-x86/ || $(MKDIR) dist/Release/GNU-Linux-x86/ 
	$(LINK) $(LFLAGS) -o $(TARGET) $(OBJECTS) $(OBJCOMP) $(LIBS)

qttmp-Release.mk: nbproject/qt-Release.pro  /usr/share/qt4/mkspecs/linux-g++/qmake.conf /usr/share/qt4/mkspecs/common/g++.conf \
		/usr/share/qt4/mkspecs/common/unix.conf \
		/usr/share/qt4/mkspecs/common/linux.conf \
		/usr/share/qt4/mkspecs/qconfig.pri \
		/usr/share/qt4/mkspecs/features/qt_functions.prf \
		/usr/share/qt4/mkspecs/features/qt_config.prf \
		/usr/share/qt4/mkspecs/features/exclusive_builds.prf \
		/usr/share/qt4/mkspecs/features/default_pre.prf \
		/usr/share/qt4/mkspecs/features/release.prf \
		/usr/share/qt4/mkspecs/features/default_post.prf \
		/usr/share/qt4/mkspecs/features/warn_on.prf \
		/usr/share/qt4/mkspecs/features/qt.prf \
		/usr/share/qt4/mkspecs/features/unix/thread.prf \
		/usr/share/qt4/mkspecs/features/moc.prf \
		/usr/share/qt4/mkspecs/features/resources.prf \
		/usr/share/qt4/mkspecs/features/uic.prf \
		/usr/share/qt4/mkspecs/features/yacc.prf \
		/usr/share/qt4/mkspecs/features/lex.prf \
		/usr/share/qt4/mkspecs/features/include_source_dir.prf \
		/usr/lib/libQtGui.prl \
		/usr/lib/libQtNetwork.prl \
		/usr/lib/libQtCore.prl
	$(QMAKE) -unix VPATH=. -o qttmp-Release.mk nbproject/qt-Release.pro
/usr/share/qt4/mkspecs/common/g++.conf:
/usr/share/qt4/mkspecs/common/unix.conf:
/usr/share/qt4/mkspecs/common/linux.conf:
/usr/share/qt4/mkspecs/qconfig.pri:
/usr/share/qt4/mkspecs/features/qt_functions.prf:
/usr/share/qt4/mkspecs/features/qt_config.prf:
/usr/share/qt4/mkspecs/features/exclusive_builds.prf:
/usr/share/qt4/mkspecs/features/default_pre.prf:
/usr/share/qt4/mkspecs/features/release.prf:
/usr/share/qt4/mkspecs/features/default_post.prf:
/usr/share/qt4/mkspecs/features/warn_on.prf:
/usr/share/qt4/mkspecs/features/qt.prf:
/usr/share/qt4/mkspecs/features/unix/thread.prf:
/usr/share/qt4/mkspecs/features/moc.prf:
/usr/share/qt4/mkspecs/features/resources.prf:
/usr/share/qt4/mkspecs/features/uic.prf:
/usr/share/qt4/mkspecs/features/yacc.prf:
/usr/share/qt4/mkspecs/features/lex.prf:
/usr/share/qt4/mkspecs/features/include_source_dir.prf:
/usr/lib/libQtGui.prl:
/usr/lib/libQtNetwork.prl:
/usr/lib/libQtCore.prl:
qmake:  FORCE
	@$(QMAKE) -unix VPATH=. -o qttmp-Release.mk nbproject/qt-Release.pro

dist: 
	@$(CHK_DIR_EXISTS) nbproject/build/Release/GNU-Linux-x86/QtRadio1.0.0 || $(MKDIR) nbproject/build/Release/GNU-Linux-x86/QtRadio1.0.0 
	$(COPY_FILE) --parents $(SOURCES) $(DIST) nbproject/build/Release/GNU-Linux-x86/QtRadio1.0.0/ && $(COPY_FILE) --parents CWLFilters.h FiltersBase.h CWUFilters.h FMNFilters.h AMFilters.h DSBFilters.h DIGUFilters.h USBFilters.h DIGLFilters.h Band.h UI.h Audio.h BandStackEntry.h Connection.h Filter.h Filters.h Mode.h SAMFilters.h Spectrum.h Waterfall.h LSBFilters.h nbproject/build/Release/GNU-Linux-x86/QtRadio1.0.0/ && $(COPY_FILE) --parents Connection.cpp FMNFilters.cpp SAMFilters.cpp main.cpp CWLFilters.cpp LSBFilters.cpp Waterfall.cpp Filters.cpp DSBFilters.cpp USBFilters.cpp Band.cpp Spectrum.cpp DIGLFilters.cpp CWUFilters.cpp DIGUFilters.cpp Audio.cpp Mode.cpp BandStackEntry.cpp FiltersBase.cpp AMFilters.cpp Filter.cpp UI.cpp nbproject/build/Release/GNU-Linux-x86/QtRadio1.0.0/ && $(COPY_FILE) --parents UI.ui nbproject/build/Release/GNU-Linux-x86/QtRadio1.0.0/ && (cd `dirname nbproject/build/Release/GNU-Linux-x86/QtRadio1.0.0` && $(TAR) QtRadio1.0.0.tar QtRadio1.0.0 && $(COMPRESS) QtRadio1.0.0.tar) && $(MOVE) `dirname nbproject/build/Release/GNU-Linux-x86/QtRadio1.0.0`/QtRadio1.0.0.tar.gz . && $(DEL_FILE) -r nbproject/build/Release/GNU-Linux-x86/QtRadio1.0.0


clean:compiler_clean 
	-$(DEL_FILE) $(OBJECTS)
	-$(DEL_FILE) *~ core *.core


####### Sub-libraries

distclean: clean
	-$(DEL_FILE) $(TARGET) 
	-$(DEL_FILE) qttmp-Release.mk


mocclean: compiler_moc_header_clean compiler_moc_source_clean

mocables: compiler_moc_header_make_all compiler_moc_source_make_all

compiler_moc_header_make_all: moc_FiltersBase.cpp moc_Band.cpp moc_UI.cpp moc_Audio.cpp moc_BandStackEntry.cpp moc_Connection.cpp moc_Filter.cpp moc_Filters.cpp moc_Mode.cpp moc_Spectrum.cpp moc_Waterfall.cpp
compiler_moc_header_clean:
	-$(DEL_FILE) moc_FiltersBase.cpp moc_Band.cpp moc_UI.cpp moc_Audio.cpp moc_BandStackEntry.cpp moc_Connection.cpp moc_Filter.cpp moc_Filters.cpp moc_Mode.cpp moc_Spectrum.cpp moc_Waterfall.cpp
moc_FiltersBase.cpp: Filter.h \
		FiltersBase.h
	/usr/bin/moc-qt4 $(DEFINES) $(INCPATH) FiltersBase.h -o moc_FiltersBase.cpp

moc_Band.cpp: BandStackEntry.h \
		Band.h
	/usr/bin/moc-qt4 $(DEFINES) $(INCPATH) Band.h -o moc_Band.cpp

moc_UI.cpp: ui_UI.h \
		Spectrum.h \
		Waterfall.h \
		Audio.h \
		Connection.h \
		Band.h \
		BandStackEntry.h \
		Mode.h \
		Filters.h \
		FiltersBase.h \
		Filter.h \
		CWLFilters.h \
		CWUFilters.h \
		LSBFilters.h \
		USBFilters.h \
		DSBFilters.h \
		AMFilters.h \
		SAMFilters.h \
		FMNFilters.h \
		DIGLFilters.h \
		DIGUFilters.h \
		UI.h
	/usr/bin/moc-qt4 $(DEFINES) $(INCPATH) UI.h -o moc_UI.cpp

moc_Audio.cpp: Audio.h
	/usr/bin/moc-qt4 $(DEFINES) $(INCPATH) Audio.h -o moc_Audio.cpp

moc_BandStackEntry.cpp: BandStackEntry.h
	/usr/bin/moc-qt4 $(DEFINES) $(INCPATH) BandStackEntry.h -o moc_BandStackEntry.cpp

moc_Connection.cpp: Connection.h
	/usr/bin/moc-qt4 $(DEFINES) $(INCPATH) Connection.h -o moc_Connection.cpp

moc_Filter.cpp: Filter.h
	/usr/bin/moc-qt4 $(DEFINES) $(INCPATH) Filter.h -o moc_Filter.cpp

moc_Filters.cpp: FiltersBase.h \
		Filter.h \
		Filters.h
	/usr/bin/moc-qt4 $(DEFINES) $(INCPATH) Filters.h -o moc_Filters.cpp

moc_Mode.cpp: Mode.h
	/usr/bin/moc-qt4 $(DEFINES) $(INCPATH) Mode.h -o moc_Mode.cpp

moc_Spectrum.cpp: Spectrum.h
	/usr/bin/moc-qt4 $(DEFINES) $(INCPATH) Spectrum.h -o moc_Spectrum.cpp

moc_Waterfall.cpp: Waterfall.h
	/usr/bin/moc-qt4 $(DEFINES) $(INCPATH) Waterfall.h -o moc_Waterfall.cpp

compiler_rcc_make_all:
compiler_rcc_clean:
compiler_image_collection_make_all: qmake_image_collection.cpp
compiler_image_collection_clean:
	-$(DEL_FILE) qmake_image_collection.cpp
compiler_moc_source_make_all:
compiler_moc_source_clean:
compiler_uic_make_all: ui_UI.h
compiler_uic_clean:
	-$(DEL_FILE) ui_UI.h
ui_UI.h: UI.ui
	/usr/bin/uic-qt4 UI.ui -o ui_UI.h

compiler_yacc_decl_make_all:
compiler_yacc_decl_clean:
compiler_yacc_impl_make_all:
compiler_yacc_impl_clean:
compiler_lex_make_all:
compiler_lex_clean:
compiler_clean: compiler_moc_header_clean compiler_uic_clean 

####### Compile

build/Release/GNU-Linux-x86/Connection.o: Connection.cpp Connection.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/Connection.o Connection.cpp

build/Release/GNU-Linux-x86/FMNFilters.o: FMNFilters.cpp FMNFilters.h \
		FiltersBase.h \
		Filter.h \
		Filters.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/FMNFilters.o FMNFilters.cpp

build/Release/GNU-Linux-x86/SAMFilters.o: SAMFilters.cpp SAMFilters.h \
		FiltersBase.h \
		Filter.h \
		Filters.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/SAMFilters.o SAMFilters.cpp

build/Release/GNU-Linux-x86/main.o: main.cpp UI.h \
		ui_UI.h \
		Spectrum.h \
		Waterfall.h \
		Audio.h \
		Connection.h \
		Band.h \
		BandStackEntry.h \
		Mode.h \
		Filters.h \
		FiltersBase.h \
		Filter.h \
		CWLFilters.h \
		CWUFilters.h \
		LSBFilters.h \
		USBFilters.h \
		DSBFilters.h \
		AMFilters.h \
		SAMFilters.h \
		FMNFilters.h \
		DIGLFilters.h \
		DIGUFilters.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/main.o main.cpp

build/Release/GNU-Linux-x86/CWLFilters.o: CWLFilters.cpp CWLFilters.h \
		FiltersBase.h \
		Filter.h \
		Filters.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/CWLFilters.o CWLFilters.cpp

build/Release/GNU-Linux-x86/LSBFilters.o: LSBFilters.cpp LSBFilters.h \
		FiltersBase.h \
		Filter.h \
		Filters.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/LSBFilters.o LSBFilters.cpp

build/Release/GNU-Linux-x86/Waterfall.o: Waterfall.cpp Waterfall.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/Waterfall.o Waterfall.cpp

build/Release/GNU-Linux-x86/Filters.o: Filters.cpp Filters.h \
		FiltersBase.h \
		Filter.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/Filters.o Filters.cpp

build/Release/GNU-Linux-x86/DSBFilters.o: DSBFilters.cpp DSBFilters.h \
		FiltersBase.h \
		Filter.h \
		Filters.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/DSBFilters.o DSBFilters.cpp

build/Release/GNU-Linux-x86/USBFilters.o: USBFilters.cpp USBFilters.h \
		FiltersBase.h \
		Filter.h \
		Filters.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/USBFilters.o USBFilters.cpp

build/Release/GNU-Linux-x86/Band.o: Band.cpp Band.h \
		BandStackEntry.h \
		Mode.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/Band.o Band.cpp

build/Release/GNU-Linux-x86/Spectrum.o: Spectrum.cpp Spectrum.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/Spectrum.o Spectrum.cpp

build/Release/GNU-Linux-x86/DIGLFilters.o: DIGLFilters.cpp DIGLFilters.h \
		FiltersBase.h \
		Filter.h \
		Filters.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/DIGLFilters.o DIGLFilters.cpp

build/Release/GNU-Linux-x86/CWUFilters.o: CWUFilters.cpp CWUFilters.h \
		FiltersBase.h \
		Filter.h \
		Filters.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/CWUFilters.o CWUFilters.cpp

build/Release/GNU-Linux-x86/DIGUFilters.o: DIGUFilters.cpp DIGUFilters.h \
		FiltersBase.h \
		Filter.h \
		Filters.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/DIGUFilters.o DIGUFilters.cpp

build/Release/GNU-Linux-x86/Audio.o: Audio.cpp Audio.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/Audio.o Audio.cpp

build/Release/GNU-Linux-x86/Mode.o: Mode.cpp Mode.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/Mode.o Mode.cpp

build/Release/GNU-Linux-x86/BandStackEntry.o: BandStackEntry.cpp BandStackEntry.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/BandStackEntry.o BandStackEntry.cpp

build/Release/GNU-Linux-x86/FiltersBase.o: FiltersBase.cpp FiltersBase.h \
		Filter.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/FiltersBase.o FiltersBase.cpp

build/Release/GNU-Linux-x86/AMFilters.o: AMFilters.cpp AMFilters.h \
		FiltersBase.h \
		Filter.h \
		Filters.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/AMFilters.o AMFilters.cpp

build/Release/GNU-Linux-x86/Filter.o: Filter.cpp Filter.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/Filter.o Filter.cpp

build/Release/GNU-Linux-x86/UI.o: UI.cpp UI.h \
		ui_UI.h \
		Spectrum.h \
		Waterfall.h \
		Audio.h \
		Connection.h \
		Band.h \
		BandStackEntry.h \
		Mode.h \
		Filters.h \
		FiltersBase.h \
		Filter.h \
		CWLFilters.h \
		CWUFilters.h \
		LSBFilters.h \
		USBFilters.h \
		DSBFilters.h \
		AMFilters.h \
		SAMFilters.h \
		FMNFilters.h \
		DIGLFilters.h \
		DIGUFilters.h
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/UI.o UI.cpp

build/Release/GNU-Linux-x86/moc_FiltersBase.o: moc_FiltersBase.cpp 
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/moc_FiltersBase.o moc_FiltersBase.cpp

build/Release/GNU-Linux-x86/moc_Band.o: moc_Band.cpp 
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/moc_Band.o moc_Band.cpp

build/Release/GNU-Linux-x86/moc_UI.o: moc_UI.cpp 
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/moc_UI.o moc_UI.cpp

build/Release/GNU-Linux-x86/moc_Audio.o: moc_Audio.cpp 
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/moc_Audio.o moc_Audio.cpp

build/Release/GNU-Linux-x86/moc_BandStackEntry.o: moc_BandStackEntry.cpp 
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/moc_BandStackEntry.o moc_BandStackEntry.cpp

build/Release/GNU-Linux-x86/moc_Connection.o: moc_Connection.cpp 
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/moc_Connection.o moc_Connection.cpp

build/Release/GNU-Linux-x86/moc_Filter.o: moc_Filter.cpp 
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/moc_Filter.o moc_Filter.cpp

build/Release/GNU-Linux-x86/moc_Filters.o: moc_Filters.cpp 
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/moc_Filters.o moc_Filters.cpp

build/Release/GNU-Linux-x86/moc_Mode.o: moc_Mode.cpp 
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/moc_Mode.o moc_Mode.cpp

build/Release/GNU-Linux-x86/moc_Spectrum.o: moc_Spectrum.cpp 
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/moc_Spectrum.o moc_Spectrum.cpp

build/Release/GNU-Linux-x86/moc_Waterfall.o: moc_Waterfall.cpp 
	$(CXX) -c $(CXXFLAGS) $(INCPATH) -o build/Release/GNU-Linux-x86/moc_Waterfall.o moc_Waterfall.cpp

####### Install

install:   FORCE

uninstall:   FORCE

FORCE:

