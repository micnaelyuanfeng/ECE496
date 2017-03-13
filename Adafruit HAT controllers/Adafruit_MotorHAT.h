#ifndef ADAFRUIT_MOTORHAT
#define ADAFRUIT_MOTORHAT
#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>

void set_speed_DC_motor(uint8_t hwAddress, uint8_t pwmChannel, int motorSpeed);
void set_direction_DC_motor(uint8_t hwAddress, uint8_t pwmChannel, int motorDirection);
void stop_DC_motor(uint8_t hwAddress, uint8_t pwmChannel);
void set_PIN_DC_motor(uint8_t hwAddress, uint8_t pin, int pinValue);
uint8_t PWM_channel_to_PWM_pin_converter_DC_motor(uint8_t pwmChannel);
uint8_t PWM_channel_to_IN1_pin_converter_DC_motor(uint8_t pwmChannel);
uint8_t PWM_channel_to_IN2_pin_converter_DC_motor(uint8_t pwmChannel);

/*
	Usage:
		1. Initalize the borad according to its address	
			init_PCA9685(hwAddress)
			set_PWM_frequency_PCA9685(hwAddress, pwmFrequency)
		2. Stop all DC motors by default
			stop_DC_motor(hwAddress, pwmChannel)
		3. Set DC motor speed. Speed ranges from 0 to 255
			set_speed_DC_motor(hwAddress, pwmChannel, motorSpeed)
		4. Start DC motor. Direction = 0 => stop. Direction = 1 => forward. Direction = 2 => backward
			set_direction_DC_motor(hwAddress, pwmChannel, motorDirection)
*/

