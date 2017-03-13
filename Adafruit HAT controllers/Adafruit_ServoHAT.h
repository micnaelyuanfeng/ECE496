#ifndef ADAFRUIT_SERVOHAT
#define ADAFRUIT_SERVOHAT
#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>

void init_angle_to_duty_cycle_lookup_table(double *ptLookupTable);
uint16_t angle_to_duty_cycle_percentage_converter(double * ptLookupTable, uint8_t angleDegree);
void set_servo(uint8_t hwAddress, uint8_t pwmChannel, uint8_t angleDegree);

/*
	Usage:
		1. Initalize the borad according to its address	
			init_PCA9685(hwAddress)
			set_PWM_frequency_PCA9685(hwAddress, pwmFrequency)
		2. Set all servos to default position. Different servos may have different positoins
			set_servo(hwAddress, pwmChannel, angleDegree)
		3. Set servo position upon reuqest 
			set_servo(hwAddress, pwmChannel, angleDegree)
*/