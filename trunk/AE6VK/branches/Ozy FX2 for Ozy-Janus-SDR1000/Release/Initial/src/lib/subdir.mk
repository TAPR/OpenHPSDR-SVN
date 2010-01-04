################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
C_SRCS += \
../Initial/src/lib/delay.c \
../Initial/src/lib/fx2utils.c \
../Initial/src/lib/i2c.c \
../Initial/src/lib/isr.c \
../Initial/src/lib/spi.c \
../Initial/src/lib/timer.c \
../Initial/src/lib/usb_common.c 

C_DEPS += \
./Initial/src/lib/delay.d \
./Initial/src/lib/fx2utils.d \
./Initial/src/lib/i2c.d \
./Initial/src/lib/isr.d \
./Initial/src/lib/spi.d \
./Initial/src/lib/timer.d \
./Initial/src/lib/usb_common.d 

RELS += \
./Initial/src/lib/delay.rel \
./Initial/src/lib/fx2utils.rel \
./Initial/src/lib/i2c.rel \
./Initial/src/lib/isr.rel \
./Initial/src/lib/spi.rel \
./Initial/src/lib/timer.rel \
./Initial/src/lib/usb_common.rel 


# Each subdirectory must supply rules for building sources it contributes
Initial/src/lib/%.rel: ../Initial/src/lib/%.c
	@echo 'Building file: $<'
	@echo 'Invoking: SDCC Compiler'
	sdcc -c --no-xinit-opt --xram-loc 0x2000 --xram-size 0x2000 --code-size 0x2000 -DSDR1K_CONTROL --verbose --model-small -o"$@" "$<" && \
	echo -n $(@:%.rel=%.d) $(dir $@) > $(@:%.rel=%.d) && \
	sdcc -c --no-xinit-opt --xram-loc 0x2000 --xram-size 0x2000 --code-size 0x2000 -DSDR1K_CONTROL -MM --verbose --model-small  "$<" >> $(@:%.rel=%.d)
	@echo 'Finished building: $<'
	@echo ' '


