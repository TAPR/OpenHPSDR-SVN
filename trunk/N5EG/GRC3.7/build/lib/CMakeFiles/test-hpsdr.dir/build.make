# CMAKE generated file: DO NOT EDIT!
# Generated by "Unix Makefiles" Generator, CMake Version 2.8

#=============================================================================
# Special targets provided by cmake.

# Disable implicit rules so canonical targets will work.
.SUFFIXES:

# Remove some rules from gmake that .SUFFIXES does not remove.
SUFFIXES =

.SUFFIXES: .hpux_make_needs_suffix_list

# Suppress display of executed commands.
$(VERBOSE).SILENT:

# A target that is always out of date.
cmake_force:
.PHONY : cmake_force

#=============================================================================
# Set environment variables for the build.

# The shell in which to execute make rules.
SHELL = /bin/sh

# The CMake executable.
CMAKE_COMMAND = /usr/bin/cmake

# The command to remove a file.
RM = /usr/bin/cmake -E remove -f

# Escaping for special characters.
EQUALS = =

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = /home/tom/gr-hpsdr

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = /home/tom/gr-hpsdr/build

# Include any dependencies generated for this target.
include lib/CMakeFiles/test-hpsdr.dir/depend.make

# Include the progress variables for this target.
include lib/CMakeFiles/test-hpsdr.dir/progress.make

# Include the compile flags for this target's objects.
include lib/CMakeFiles/test-hpsdr.dir/flags.make

lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o: lib/CMakeFiles/test-hpsdr.dir/flags.make
lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o: ../lib/test_hpsdr.cc
	$(CMAKE_COMMAND) -E cmake_progress_report /home/tom/gr-hpsdr/build/CMakeFiles $(CMAKE_PROGRESS_1)
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Building CXX object lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o"
	cd /home/tom/gr-hpsdr/build/lib && /usr/bin/c++   $(CXX_DEFINES) $(CXX_FLAGS) -o CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o -c /home/tom/gr-hpsdr/lib/test_hpsdr.cc

lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.i"
	cd /home/tom/gr-hpsdr/build/lib && /usr/bin/c++  $(CXX_DEFINES) $(CXX_FLAGS) -E /home/tom/gr-hpsdr/lib/test_hpsdr.cc > CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.i

lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.s"
	cd /home/tom/gr-hpsdr/build/lib && /usr/bin/c++  $(CXX_DEFINES) $(CXX_FLAGS) -S /home/tom/gr-hpsdr/lib/test_hpsdr.cc -o CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.s

lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o.requires:
.PHONY : lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o.requires

lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o.provides: lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o.requires
	$(MAKE) -f lib/CMakeFiles/test-hpsdr.dir/build.make lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o.provides.build
.PHONY : lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o.provides

lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o.provides.build: lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o

lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o: lib/CMakeFiles/test-hpsdr.dir/flags.make
lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o: ../lib/qa_hpsdr.cc
	$(CMAKE_COMMAND) -E cmake_progress_report /home/tom/gr-hpsdr/build/CMakeFiles $(CMAKE_PROGRESS_2)
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Building CXX object lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o"
	cd /home/tom/gr-hpsdr/build/lib && /usr/bin/c++   $(CXX_DEFINES) $(CXX_FLAGS) -o CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o -c /home/tom/gr-hpsdr/lib/qa_hpsdr.cc

lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.i"
	cd /home/tom/gr-hpsdr/build/lib && /usr/bin/c++  $(CXX_DEFINES) $(CXX_FLAGS) -E /home/tom/gr-hpsdr/lib/qa_hpsdr.cc > CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.i

lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.s"
	cd /home/tom/gr-hpsdr/build/lib && /usr/bin/c++  $(CXX_DEFINES) $(CXX_FLAGS) -S /home/tom/gr-hpsdr/lib/qa_hpsdr.cc -o CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.s

lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o.requires:
.PHONY : lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o.requires

lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o.provides: lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o.requires
	$(MAKE) -f lib/CMakeFiles/test-hpsdr.dir/build.make lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o.provides.build
.PHONY : lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o.provides

lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o.provides.build: lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o

# Object files for target test-hpsdr
test__hpsdr_OBJECTS = \
"CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o" \
"CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o"

# External object files for target test-hpsdr
test__hpsdr_EXTERNAL_OBJECTS =

lib/test-hpsdr: lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o
lib/test-hpsdr: lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o
lib/test-hpsdr: lib/CMakeFiles/test-hpsdr.dir/build.make
lib/test-hpsdr: /usr/lib/libboost_filesystem.so
lib/test-hpsdr: /usr/lib/libboost_system.so
lib/test-hpsdr: /usr/lib/libcppunit.so
lib/test-hpsdr: lib/libgnuradio-hpsdr.so
lib/test-hpsdr: /usr/lib/libboost_filesystem.so
lib/test-hpsdr: /usr/lib/libboost_system.so
lib/test-hpsdr: lib/CMakeFiles/test-hpsdr.dir/link.txt
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --red --bold "Linking CXX executable test-hpsdr"
	cd /home/tom/gr-hpsdr/build/lib && $(CMAKE_COMMAND) -E cmake_link_script CMakeFiles/test-hpsdr.dir/link.txt --verbose=$(VERBOSE)

# Rule to build all files generated by this target.
lib/CMakeFiles/test-hpsdr.dir/build: lib/test-hpsdr
.PHONY : lib/CMakeFiles/test-hpsdr.dir/build

lib/CMakeFiles/test-hpsdr.dir/requires: lib/CMakeFiles/test-hpsdr.dir/test_hpsdr.cc.o.requires
lib/CMakeFiles/test-hpsdr.dir/requires: lib/CMakeFiles/test-hpsdr.dir/qa_hpsdr.cc.o.requires
.PHONY : lib/CMakeFiles/test-hpsdr.dir/requires

lib/CMakeFiles/test-hpsdr.dir/clean:
	cd /home/tom/gr-hpsdr/build/lib && $(CMAKE_COMMAND) -P CMakeFiles/test-hpsdr.dir/cmake_clean.cmake
.PHONY : lib/CMakeFiles/test-hpsdr.dir/clean

lib/CMakeFiles/test-hpsdr.dir/depend:
	cd /home/tom/gr-hpsdr/build && $(CMAKE_COMMAND) -E cmake_depends "Unix Makefiles" /home/tom/gr-hpsdr /home/tom/gr-hpsdr/lib /home/tom/gr-hpsdr/build /home/tom/gr-hpsdr/build/lib /home/tom/gr-hpsdr/build/lib/CMakeFiles/test-hpsdr.dir/DependInfo.cmake --color=$(COLOR)
.PHONY : lib/CMakeFiles/test-hpsdr.dir/depend

