LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)
LOCAL_MODULE := fftw3
include $(LOCAL_PATH)/fftw3/api/sources.mk
include $(LOCAL_PATH)/fftw3/dft/sources.mk
include $(LOCAL_PATH)/fftw3/dft/scalar/sources.mk
include $(LOCAL_PATH)/fftw3/dft/scalar/codelets/sources.mk
include $(LOCAL_PATH)/fftw3/kernel/sources.mk
include $(LOCAL_PATH)/fftw3/rdft/sources.mk
include $(LOCAL_PATH)/fftw3/rdft/scalar/sources.mk
include $(LOCAL_PATH)/fftw3/rdft/scalar/r2cb/sources.mk
include $(LOCAL_PATH)/fftw3/rdft/scalar/r2cf/sources.mk
include $(LOCAL_PATH)/fftw3/rdft/scalar/r2r/sources.mk
include $(LOCAL_PATH)/fftw3/reodft/sources.mk
include $(LOCAL_PATH)/fftw3/simd-support/sources.mk
LOCAL_C_INCLUDES := $(LOCAL_PATH)/fftw3 \
    $(LOCAL_PATH)/fftw3/api \
    $(LOCAL_PATH)/fftw3/dft \
    $(LOCAL_PATH)/fftw3/dft/scalar \
    $(LOCAL_PATH)/fftw3/dft/scalar/codelets \
    $(LOCAL_PATH)/fftw3/dft/simd \
    $(LOCAL_PATH)/fftw3/dft/simd/altivec \
    $(LOCAL_PATH)/fftw3/dft/simd/avx \
    $(LOCAL_PATH)/fftw3/dft/simd/common \
    $(LOCAL_PATH)/fftw3/dft/simd/neon \
    $(LOCAL_PATH)/fftw3/dft/simd/sse2 \
    $(LOCAL_PATH)/fftw3/kernel \
    $(LOCAL_PATH)/fftw3/rdft \
    $(LOCAL_PATH)/fftw3/rdft/scalar \
    $(LOCAL_PATH)/fftw3/rdft/scalar/r2cb \
    $(LOCAL_PATH)/fftw3/rdft/scalar/r2cf \
    $(LOCAL_PATH)/fftw3/rdft/scalar/r2r \
    $(LOCAL_PATH)/fftw3/rdft/simd \
    $(LOCAL_PATH)/fftw3/rdft/simd/altivec \
    $(LOCAL_PATH)/fftw3/rdft/simd/avx \
    $(LOCAL_PATH)/fftw3/rdft/simd/common \
    $(LOCAL_PATH)/fftw3/rdft/simd/neon \
    $(LOCAL_PATH)/fftw3/rdft/simd/sse2 \
    $(LOCAL_PATH)/fftw3/reodft \
    $(LOCAL_PATH)/fftw3/simd-support
LOCAL_CFLAGS := -g
LOCAL_LDLIBS := -llog
include $(BUILD_SHARED_LIBRARY)

include $(CLEAR_VARS)
LOCAL_SHARED_LIBRARIES := fftw3
LOCAL_MODULE := wdsp
include $(LOCAL_PATH)/wdsp/sources.mk
LOCAL_CFLAGS := -g
LOCAL_LDLIBS := -llog
include $(BUILD_SHARED_LIBRARY)

