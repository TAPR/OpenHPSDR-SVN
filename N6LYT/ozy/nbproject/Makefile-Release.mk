#
# Generated Makefile - do not edit!
#
# Edit the Makefile in the project folder instead (../Makefile). Each target
# has a -pre and a -post target defined where you can add customized code.
#
# This makefile implements configuration specific macros and targets.


# Environment
MKDIR=mkdir
CP=cp
CCADMIN=CCadmin
RANLIB=ranlib
CC=gcc
CCC=g++
CXX=g++
FC=

# Macros
PLATFORM=GNU-Linux-x86

# Include project Makefile
include Makefile

# Object Directory
OBJECTDIR=build/Release/${PLATFORM}

# Object Files
OBJECTFILES= \
	${OBJECTDIR}/jack_to_ozy.o \
	${OBJECTDIR}/ozy_buffers.o \
	${OBJECTDIR}/jack_client.o \
	${OBJECTDIR}/spectrum_thread.o \
	${OBJECTDIR}/ozyio.o \
	${OBJECTDIR}/main.o \
	${OBJECTDIR}/command_thread.o \
	${OBJECTDIR}/ozy_client.o \
	${OBJECTDIR}/ozy_to_jack.o \
	${OBJECTDIR}/jack_ringbuffer.o \
	${OBJECTDIR}/spectrum_buffers.o \
	${OBJECTDIR}/jack_buffers.o \
	${OBJECTDIR}/ozy_ringbuffer.o

# C Compiler Flags
CFLAGS=

# CC Compiler Flags
CCFLAGS=
CXXFLAGS=

# Fortran Compiler Flags
FFLAGS=

# Link Libraries and Options
LDLIBSOPTIONS=-lusb -ljack

# Build Targets
.build-conf: ${BUILD_SUBPROJECTS}
	${MAKE}  -f nbproject/Makefile-Release.mk dist/Release/${PLATFORM}/ozy

dist/Release/${PLATFORM}/ozy: ${OBJECTFILES}
	${MKDIR} -p dist/Release/${PLATFORM}
	${LINK.c} -o dist/Release/${PLATFORM}/ozy ${OBJECTFILES} ${LDLIBSOPTIONS} 

${OBJECTDIR}/jack_to_ozy.o: jack_to_ozy.c 
	${MKDIR} -p ${OBJECTDIR}
	${RM} $@.d
	$(COMPILE.c) -O2 -MMD -MP -MF $@.d -o ${OBJECTDIR}/jack_to_ozy.o jack_to_ozy.c

${OBJECTDIR}/ozy_buffers.o: ozy_buffers.c 
	${MKDIR} -p ${OBJECTDIR}
	${RM} $@.d
	$(COMPILE.c) -O2 -MMD -MP -MF $@.d -o ${OBJECTDIR}/ozy_buffers.o ozy_buffers.c

${OBJECTDIR}/jack_client.o: jack_client.c 
	${MKDIR} -p ${OBJECTDIR}
	${RM} $@.d
	$(COMPILE.c) -O2 -MMD -MP -MF $@.d -o ${OBJECTDIR}/jack_client.o jack_client.c

${OBJECTDIR}/spectrum_thread.o: spectrum_thread.c 
	${MKDIR} -p ${OBJECTDIR}
	${RM} $@.d
	$(COMPILE.c) -O2 -MMD -MP -MF $@.d -o ${OBJECTDIR}/spectrum_thread.o spectrum_thread.c

${OBJECTDIR}/ozyio.o: ozyio.c 
	${MKDIR} -p ${OBJECTDIR}
	${RM} $@.d
	$(COMPILE.c) -O2 -MMD -MP -MF $@.d -o ${OBJECTDIR}/ozyio.o ozyio.c

${OBJECTDIR}/main.o: main.c 
	${MKDIR} -p ${OBJECTDIR}
	${RM} $@.d
	$(COMPILE.c) -O2 -MMD -MP -MF $@.d -o ${OBJECTDIR}/main.o main.c

${OBJECTDIR}/command_thread.o: command_thread.c 
	${MKDIR} -p ${OBJECTDIR}
	${RM} $@.d
	$(COMPILE.c) -O2 -MMD -MP -MF $@.d -o ${OBJECTDIR}/command_thread.o command_thread.c

${OBJECTDIR}/ozy_client.o: ozy_client.c 
	${MKDIR} -p ${OBJECTDIR}
	${RM} $@.d
	$(COMPILE.c) -O2 -MMD -MP -MF $@.d -o ${OBJECTDIR}/ozy_client.o ozy_client.c

${OBJECTDIR}/ozy_to_jack.o: ozy_to_jack.c 
	${MKDIR} -p ${OBJECTDIR}
	${RM} $@.d
	$(COMPILE.c) -O2 -MMD -MP -MF $@.d -o ${OBJECTDIR}/ozy_to_jack.o ozy_to_jack.c

${OBJECTDIR}/jack_ringbuffer.o: jack_ringbuffer.c 
	${MKDIR} -p ${OBJECTDIR}
	${RM} $@.d
	$(COMPILE.c) -O2 -MMD -MP -MF $@.d -o ${OBJECTDIR}/jack_ringbuffer.o jack_ringbuffer.c

${OBJECTDIR}/spectrum_buffers.o: spectrum_buffers.c 
	${MKDIR} -p ${OBJECTDIR}
	${RM} $@.d
	$(COMPILE.c) -O2 -MMD -MP -MF $@.d -o ${OBJECTDIR}/spectrum_buffers.o spectrum_buffers.c

${OBJECTDIR}/jack_buffers.o: jack_buffers.c 
	${MKDIR} -p ${OBJECTDIR}
	${RM} $@.d
	$(COMPILE.c) -O2 -MMD -MP -MF $@.d -o ${OBJECTDIR}/jack_buffers.o jack_buffers.c

${OBJECTDIR}/ozy_ringbuffer.o: ozy_ringbuffer.c 
	${MKDIR} -p ${OBJECTDIR}
	${RM} $@.d
	$(COMPILE.c) -O2 -MMD -MP -MF $@.d -o ${OBJECTDIR}/ozy_ringbuffer.o ozy_ringbuffer.c

# Subprojects
.build-subprojects:

# Clean Targets
.clean-conf:
	${RM} -r build/Release
	${RM} dist/Release/${PLATFORM}/ozy

# Subprojects
.clean-subprojects:

# Enable dependency checking
.dep.inc: .depcheck-impl

include .dep.inc
