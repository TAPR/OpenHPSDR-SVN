#                                               -*- Autoconf -*-
# Process this file with autoconf to produce a configure script.

AC_PREREQ(2.60)
AC_INIT(dttsp, new, [ab2kt@arrl.net> or <rwmcgwier@gmail.com])
#AM_INIT_AUTOMAKE([-Wall -Werror foreign])
#AM_INIT_AUTOMAKE([-Werror foreign])
AM_INIT_AUTOMAKE([])
AC_CONFIG_SRCDIR([am_demod.c])
#AC_CONFIG_HEADER([config.h])

AC_CANONICAL_HOST

# Checks for programs.
AC_PROG_CC
AC_PROG_RANLIB

dnl XXX we need to cover Intel as well as PowerPC
if test $host_cpu = "powerpc" && test $host_vendor = "apple"; then

	dnl set specific macosx paths (fink)
	CFLAGS="$CFLAGS -I/sw/include"
	LDFLAGS="$LDFLAGS -L/sw/lib"
	CPPFLAGS="$CPPFLAGS -I/sw/include"
fi

# Checks for libraries.
AC_CHECK_LIB([fftw3f], [fftwf_execute], ,
	[AC_MSG_ERROR("Could not find library fftw3f.")])
AC_CHECK_LIB([jack], [jack_connect], ,
	[AC_MSG_ERROR("Could not find library jack.")])
AC_CHECK_LIB([m], [pow])
AC_CHECK_LIB([pthread], [pthread_create])
AC_CHECK_LIB([lo], [lo_address_new], ,
	[AC_MSG_ERROR("Could not find library liblo.")])

# Checks for header files.
AC_HEADER_STDC
AC_HEADER_SYS_WAIT
AC_CHECK_HEADERS([fcntl.h stdlib.h string.h sys/ioctl.h sys/param.h sys/socket.h sys/time.h unistd.h])

# Checks for typedefs, structures, and compiler characteristics.
AC_C_CONST
AC_C_INLINE
AC_TYPE_SIZE_T
AC_HEADER_TIME
AC_HEADER_STDBOOL
AC_TYPE_UID_T

# Checks for library functions.
AC_PROG_GCC_TRADITIONAL
AC_FUNC_STAT
AC_CHECK_FUNCS([floor gettimeofday memset pow sqrt])

AC_CONFIG_FILES([Makefile])
AC_OUTPUT
