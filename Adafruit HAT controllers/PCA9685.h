#ifndef PCA9685
#define PCA9685

// PCA9685 Registers
#define MODE1 					0x00
#define MODE2 					0x01
#define SUBADR1 				0x02
#define SUBADR2 				0x03
#define SUBADR3 				0x04
#define PRESCALE 				0xFE
#define LED0_ON_L 				0x06
#define LED0_ON_H 				0x07
#define LED0_OFF_L 				0x08
#define LED0_OFF_H 				0x09
#define ALL_LED_ON_L 			0xFA
#define ALL_LED_ON_H 			0xFB
#define ALL_LED_OFF_L 			0xFC
#define ALL_LED_OFF_H 			0xFD

// PCA9685 Bits
#define RESTART 				0x80
#define SLEEP 					0x10
#define ALLCALL 				0x01
#define INVRT 					0x10
#define OUTDRV 					0x04

#define CPUCLK 					25000000.0
#define PRESCALELEVEL 			4096

void init_PCA9685(uint8_t hwAddress);
void set_PWM_frequency_PCA9685(uint8_t hwAddress, uint16_t pwmFrequency);
void set_PWM_PCA9685(uint8_t hwAddress, uint8_t pwmChannel, uint16_t onTime, uint16_t offTime);
void writeByte_PCA9685(uint8_t hwAddress, uint8_t writeAddress, uint8_t writeData);
uint8_t readByte_PCA9685(uint8_t hwAddress, uint8_t readAddress);

#endif
