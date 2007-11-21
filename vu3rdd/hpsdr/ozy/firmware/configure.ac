#                                               -*- Autoconf -*-
# Process this file with autoconf to produce a configure script.

AC_PREREQ(2.61)
AC_INIT(hpsdr_ozy, 0.1, vu3rdd@gmail.com)
AC_CONFIG_SRCDIR([include/spi.h])
AC_CONFIG_HEADER([config.h])

AC_CANONICAL_BUILD
AC_CANONICAL_HOST
AC_CANONICAL_TARGET

# Checks for programs.
AC_PROG_CC

# Checks for libraries.

# Checks for header files.
AC_HEADER_STDC
AC_CHECK_HEADERS([string.h])

# Checks for typedefs, structures, and compiler characteristics.
AC_C_CONST
AC_C_VOLATILE

# specific checks
build_dirs="config"
HPSDR_OZY_CHECKS

# Each component is now either to be built, was skipped, or failed dependencies
AC_SUBST([build_dirs], [$build_dirs])

AC_CONFIG_FILES([Makefile src/Makefile lib/Makefile])
AM_INIT_AUTOMAKE(hpsdr,0.1svn)

# Checks for library functions.
AC_OUTPUT

