#include "Standard_Library.h"
#include "System_Library.h"
#include "PCA9685.h"
#include "BCM2835.h"


/* 	This is the driver for PCA9685 Chip (I2C Chip)
	It is designed for ADAFRUIT 16-CHANNEL PWM / SERVO HAT FOR RASPBERRY PI
	The orginal driver is implemented in python by ADAFRUIT
	The communication protocol is implemented based on BCM2835 chip
*/

void init_PCA9685(uint8_t hwAddress){
	// Reset all PWM Channels 
	writeByte_PCA9685(hwAddress, ALL_LED_ON_L, (uint8_t)(0&0xFF));
	writeByte_PCA9685(hwAddress, ALL_LED_ON_H, (uint8_t)(0>>8));
	writeByte_PCA9685(hwAddress, ALL_LED_OFF_L, (uint8_t)(0&0xFF));
	writeByte_PCA9685(hwAddress, ALL_LED_OFF_H, (uint8_t)(0>>8));
	
	// Initializing hardware
	writeByte_PCA9685(hwAddress, MODE2, OUTDRV);
    	writeByte_PCA9685(hwAddress, MODE1, ALLCALL);
	usleep(5000); // Wait for oscillator
	uint8_t mode1Status = readByte_PCA9685(hwAddress, MODE1);
	mode1Status = mode1Status & ~SLEEP;// Reset Sleep
	writeByte_PCA9685(hwAddress, MODE1, mode1Status);
	usleep(5000); // Wait for oscillator
}

void set_PWM_frequency_PCA9685(uint8_t hwAddress, uint16_t pwmFrequency){
	// Set Frequency for PCA9685
	float prescaleFreqFloat = CPUCLK;
 	prescaleFreqFloat /= PRESCALELEVEL;
	prescaleFreqFloat = prescaleFreqFloat / (float)pwmFrequency;
	prescaleFreqFloat = prescaleFreqFloat - 1.0;
	prescaleFreqFloat = floor(prescaleFreqFloat+ 0.5) ;
	int prescaleFreqInt = (int)prescaleFreqFloat;
	uint8_t mode1StatusPrev = readByte_PCA9685(hwAddress, MODE1);
	uint8_t mode1StatusNext = (mode1StatusPrev & 0x7F) | 0x10; //Put to sleep
	writeByte_PCA9685(hwAddress, MODE1, mode1StatusNext);
	writeByte_PCA9685(hwAddress, PRESCALE, (uint8_t)prescaleFreqInt);
	writeByte_PCA9685(hwAddress, MODE1, mode1StatusPrev);
	usleep(5000);
	writeByte_PCA9685(hwAddress, MODE1, mode1StatusPrev|0x80);	
}

void set_PWM_PCA9685(uint8_t hwAddress, uint8_t pwmChannel, uint16_t onTime, uint16_t offTime){
	// Set PWM for a selected channel 
	writeByte_PCA9685(hwAddress, LED0_ON_L+4*pwmChannel, (uint8_t)(onTime&0xFF));
	writeByte_PCA9685(hwAddress, LED0_ON_H+4*pwmChannel, (uint8_t)(onTime>>8));
	writeByte_PCA9685(hwAddress, LED0_OFF_L+4*pwmChannel, (uint8_t)(offTime&0xFF));
	writeByte_PCA9685(hwAddress, LED0_OFF_H+4*pwmChannel, (uint8_t)(offTime>>8));	
}
  

void writeByte_PCA9685(uint8_t hwAddress, uint8_t writeAddress, uint8_t writeData){
	// Write 8 bits data to peripheral hardware using Broadcom Serial Controller protocol (BSC0)
	// Select peripheral hardware 
	BSC0_A = hwAddress;
	// Specify the size of data (2 bytes addr + data)
	BSC0_DLEN = 2;
	// Write to peripheral in (address, data) order
	BSC0_FIFO = (uint8_t)writeAddress;
	BSC0_FIFO = (uint8_t)writeData;
	// Clear BSC0 Status register
	BSC0_S = BSC_CLEAR_STATUS_FLAG;
	// Start write on BSC0
	BSC0_C = BSC_WRITE_FLAG;
	
	// Wait BSC write complete 
	int timeoutInterval = 50;
	while(!(BSC0_S & BSC_S_DONE)) {
		usleep(1000);
		timeoutInterval --;
		if(!timeoutInterval){
			printf("Time out occured at writeByte_PCA9685. Hardware Address: %x. Write Address: %x. Write Data: %x.\n Program Exit...\n", hwAddress, writeAddress, writeData);
			exit(1);
		}
	}
}

uint8_t readByte_PCA9685(uint8_t hwAddress, uint8_t readAddress){
	// Read 8 bits data from peripheral hardware using Broadcom Serial Controller protocol (BSC0)
	// Select peripheral hardware 
	BSC0_A = hwAddress;
	// Specify the size of data (1 byte address)
	BSC0_DLEN = 1;
	// Write to peripheral in (address, data) order
	BSC0_FIFO = (uint8_t)readAddress;
	// Clear BSC0 Status register
	BSC0_S = BSC_CLEAR_STATUS_FLAG;
	// Start write on BSC0
	BSC0_C = BSC_WRITE_FLAG;
	
	// Wait BSC write complete 
	int timeoutInterval = 50;
	while(!(BSC0_S & BSC_S_DONE)) {
		usleep(1000);
		timeoutInterval --;
		if(!timeoutInterval){
			printf("Time out occured at readByte_PCA9685. Hardware Address: %x. Write Address: %x. Write Data: %x.\n Program Exit...\n", hwAddress, readAddress);
			exit(1);
		}
	}
	
	// Specify the size of data to read
	BSC0_DLEN = 1;
	// Clear BSC0 Status registera
	BSC0_S = BSC_CLEAR_STATUS_FLAG;
	// Start read on BSC0
	BSC0_C = BSC_READ_FLAG;
	
	// Wait BSC read complete 
	timeoutInterval = 50;
	while(!(BSC0_S & BSC_S_DONE)) {
		usleep(1000);
		timeoutInterval --;
		if(!timeoutInterval){
			printf("Time out occured at readByte_PCA9685. Hardware Address: %x. Write Address: %x. Write Data: %x.\n Program Exit...\n", hwAddress, readAddress);
			exit(1);
		}
	}
	return (BSC0_FIFO & 0xFF);
}
