#ifndef PCA9685
#define PCA9685
#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <unistd.h>
#include <assert.h>

// Broadcom Serial Controller Registers
#define BSC0_C        	*(bsc0.addr + 0x00)
#define BSC0_S        	*(bsc0.addr + 0x01)
#define BSC0_DLEN    	*(bsc0.addr + 0x02)
#define BSC0_A        	*(bsc0.addr + 0x03)
#define BSC0_FIFO    	*(bsc0.addr + 0x04)

// Broadcom Serial Controller Bits
#define BSC_C_I2CEN    	(1 << 15)
#define BSC_C_INTR    	(1 << 10)
#define BSC_C_INTT    	(1 << 9)
#define BSC_C_INTD    	(1 << 8)
#define BSC_C_ST    	(1 << 7)
#define BSC_C_CLEAR    	(1 << 4)
#define BSC_C_READ    	1
#define BSC_S_CLKT	(1 << 9)
#define BSC_S_ERR    	(1 << 8)
#define BSC_S_RXF    	(1 << 7)
#define BSC_S_TXE    	(1 << 6)
#define BSC_S_RXD    	(1 << 5)
#define BSC_S_TXD    	(1 << 4)
#define BSC_S_RXR    	(1 << 3)
#define BSC_S_TXW    	(1 << 2)
#define BSC_S_DONE   	(1 << 1)
#define BSC_S_TA    	1

// Broadcom Serial Controller Signal Flags
#define BSC_CLEAR_STATUS_FLAG    BSC_S_CLKT|BSC_S_ERR|BSC_S_DONE
#define BSC_READ_FLAG    	BSC_C_I2CEN|BSC_C_ST|BSC_C_CLEAR|BSC_C_READ
#define BSC_WRITE_FLAG   	BSC_C_I2CEN|BSC_C_ST

// PCA9685 Registers
#define MODE1 0x00
#define MODE2 0x01
#define SUBADR1 0x02
#define SUBADR2 0x03
#define SUBADR3 0x04
#define PRESCALE 0xFE
#define LED0_ON_L 0x06
#define LED0_ON_H 0x07
#define LED0_OFF_L 0x08
#define LED0_OFF_H 0x09
#define ALL_LED_ON_L 0xFA
#define ALL_LED_ON_H 0xFB
#define ALL_LED_OFF_L 0xFC
#define ALL_LED_OFF_H 0xFD

// PCA9685 Bits
#define RESTART 0x80
#define SLEEP 0x10
#define ALLCALL 0x01
#define INVRT 0x10
#define OUTDRV 0x04

#define CPUCLK 25000000.0
#define PRESCALELEVEL 4096

void init_PCA9685(uint8_t hwAddress);
void set_PWM_frequency_PCA9685(uint8_t hwAddress, uint16_t pwmFrequency);
void set_PWM_PCA9685(uint8_t hwAddress, uint8_t pwmChannel, uint16_t onTime, uint16_t offTime);
void writeByte_PCA9685(uint8_t hwAddress, uint8_t writeAddress, uint8_t writeData);
uint8_t readByte_PCA9685(uint8_t hwAddress, uint8_t readAddress);