################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
C_SRCS += \
../Initial/src/board_specific.c \
../Initial/src/eeprom_io.c \
../Initial/src/fpga_load.c \
../Initial/src/hpsdr_common.c \
../Initial/src/hpsdr_main.c \
../Initial/src/sdr1kctl.c 

C_DEPS += \
./Initial/src/board_specific.d \
./Initial/src/eeprom_io.d \
./Initial/src/fpga_load.d \
./Initial/src/hpsdr_common.d \
./Initial/src/hpsdr_main.d \
./Initial/src/sdr1kctl.d 

RELS += \
./Initial/src/board_specific.rel \
./Initial/src/eeprom_io.rel \
./Initial/src/fpga_load.rel \
./Initial/src/hpsdr_common.rel \
./Initial/src/hpsdr_main.rel \
./Initial/src/sdr1kctl.rel 


# Each subdirectory must supply rules for building sources it contributes
Initial/src/%.rel: ../Initial/src/%.c
	@echo 'Building file: $<'
	@echo 'Invoking: SDCC Compiler'
	sdcc -c --no-xinit-opt --xram-loc 0x2000 --xram-size 0x2000 --code-size 0x2000 -DSDR1K_CONTROL --verbose --model-small -o"$@" "$<" && \
	echo -n $(@:%.rel=%.d) $(dir $@) > $(@:%.rel=%.d) && \
	sdcc -c --no-xinit-opt --xram-loc 0x2000 --xram-size 0x2000 --code-size 0x2000 -DSDR1K_CONTROL -MM --verbose --model-small  "$<" >> $(@:%.rel=%.d)
	@echo 'Finished building: $<'
	@echo ' '


