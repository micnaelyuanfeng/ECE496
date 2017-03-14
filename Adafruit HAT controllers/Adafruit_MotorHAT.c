#include "Standard_Library.h"
#include "System_Library.h"
#include "PCA9685.h"
#include "Adafruit_MotorHAT.h"


void set_speed_DC_motor(uint8_t hwAddress, uint8_t pwmChannel, int motorSpeed){
	if(motorSpeed < 0){
		motorSpeed = 0;
	}else if (motorSpeed > 255){
		motorSpeed = 255;
	}
	
	set_PWM_PCA9685(hwAddress, pwmChannel, (uint16_t)0, (uint16_t)(motorSpeed*16));	
}

void set_direction_DC_motor(uint8_t hwAddress, uint8_t pwmChannel, int motorDirection){
	uint8_t in1Pin = PWM_channel_to_IN1_pin_converter_DC_motor(pwmChannel);
	uint8_t in2Pin = PWM_channel_to_IN2_pin_converter_DC_motor(pwmChannel);
	
	switch(motorDirection){
		case 0	: 	set_PIN_DC_motor(hwAddress, in1Pin, 0);
					set_PIN_DC_motor(hwAddress, in2Pin, 0);
					break;
		case 1	:	set_PIN_DC_motor(hwAddress, in1Pin, 0);
					set_PIN_DC_motor(hwAddress, in2Pin, 1);
					break;
		case 2	:	set_PIN_DC_motor(hwAddress, in1Pin, 1);
					set_PIN_DC_motor(hwAddress, in2Pin, 0);
					break;
		default	:	printf("set_direction_DC_motor failed to parse motor Direction %x.\n Exit...\n", motorDirection);
					exit(1);
					break;
	}
}

void stop_DC_motor(uint8_t hwAddress, uint8_t pwmChannel){
	set_direction_DC_motor(hwAddress, pwmChannel, 0);
}

void set_PIN_DC_motor(uint8_t hwAddress, uint8_t pin, int pinValue){
	
	if((pin < 0) || (pin > 15)){
		printf("set_PIN_DC_motor received incorrect pin: %d.\n Exit...\n", pin);
		exit(1);
	}else if((pinValue != 0) && (pinValue != 1)){
		printf("set_PIN_DC_motor received incorrect pin value: %d.\n Exit...\n", pinValue);
		exit(1);
	}else{
		if(pinValue == 0){
			set_PWM_PCA9685(hwAddress, pin, (uint16_t)0, (uint16_t)4096);	
		}else if (pinValue == 1){
			set_PWM_PCA9685(hwAddress, pin, (uint16_t)4096, (uint16_t)0);				
		}
	}
}


uint8_t PWM_channel_to_PWM_pin_converter_DC_motor(uint8_t pwmChannel){
	uint8_t pwmPin;
	switch(pwmChannel){
		case 0 	: 	pwmPin = 8;
					break;
		case 1 	: 	pwmPin = 13;
					break;
		case 2 	: 	pwmPin = 2;
					break;
		case 3 	: 	pwmPin = 7;
					break;
		default	:	printf("PWM_channel_to_PWM_pin_converter_DC_motor failed to parse pwm channel %x.\n Exit...\n", pwmChannel);
					exit(1);
					break;
	}
	return pwmPin;
}

uint8_t PWM_channel_to_IN1_pin_converter_DC_motor(uint8_t pwmChannel){
	uint8_t in1Pin;
	switch(pwmChannel){
		case 0 	: 	in1Pin = 10;
					break;
		case 1 	: 	in1Pin = 11;
					break;
		case 2 	: 	in1Pin = 4;
					break;
		case 3 	: 	in1Pin = 5;
					break;
		default	:	printf("PWM_channel_to_IN1_pin_converter_DC_motor failed to parse pwm channel %x.\n Exit...\n", pwmChannel);
					exit (1);
					break;
	}
	return in1Pin;
}

uint8_t PWM_channel_to_IN2_pin_converter_DC_motor(uint8_t pwmChannel){
	uint8_t in2Pin;
	switch(pwmChannel){
		case 0 	: 	in2Pin = 9;
					break;
		case 1 	: 	in2Pin = 12;
					break;
		case 2 	: 	in2Pin = 3;
					break;
		case 3 	: 	in2Pin = 6;
					break;
		default	:	printf("PWM_channel_to_IN2_pin_converter_DC_motor failed to parse pwm channel %x.\n Exit...\n", pwmChannel);
					exit (1);
					break;
	}
	return in2Pin;
}

