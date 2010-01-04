################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
ASM_SRCS += \
../Initial/src/asm/_startup.asm \
../Initial/src/asm/usb_descriptors_X1.asm \
../Initial/src/asm/vectors.asm 

RELS += \
./Initial/src/asm/_startup.rel \
./Initial/src/asm/usb_descriptors_X1.rel \
./Initial/src/asm/vectors.rel 


# Each subdirectory must supply rules for building sources it contributes
Initial/src/asm/%.rel: ../Initial/src/asm/%.asm
	@echo 'Building file: $<'
	@echo 'Invoking: SDCC Assembler'
	sh ../tools/asx8051-eclipse.zsh  "$@"  -plosgff  "$<"
	@echo 'Finished building: $<'
	@echo ' '


