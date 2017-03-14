#include "Standard_Library.h"
#include "System_Library.h"
#include "PCA9685.h"
#include "Adafruit_ServoHAT.h"


int angleToPulseLengthLookupTable[181];

void init_angle_to_pulse_length_lookup_table(){
	int i;
	for (i = 0; i < 181; i++){
		// 0 -> 500, 180 -> 2500, 90 -> 1500
		angleToPulseLengthLookupTable[i] = (int)(500 + i * 11.1111111+0.5);
	}
	printf("******Angle To Duty Cycle Lookup Table******\n");
	printf("Index\tAngle\tPulse Length\t\n");
	for (i = 0; i < 181; i++){
		printf("%d\t%d\t%d\n", i, i, angleToPulseLengthLookupTable[i]);
	}		  
}



int pulse_length_to_tick_converter(int frequency, int pulseLength){
	double periodLength = (1.0 / frequency) * 1000000;
	double timePerTick = periodLength / 4096.0;
	double tick = pulseLength / timePerTick;
printf("hi %d %f %f %f\n", frequency, periodLength, timePerTick, tick);
	return ((int)tick);
}

void set_servo(uint8_t hwAddress, uint8_t pwmChannel, int frequency, int angleDegree){
	if(angleDegree < 0 || angleDegree > 180){
		printf("angle %d is out of servo range. Exit....\n", angleDegree);
		exit(1);
	}
	int pulseLength = angleToPulseLengthLookupTable[angleDegree];
	uint16_t offTime = (uint16_t)pulse_length_to_tick_converter(frequency, pulseLength);
	set_PWM_PCA9685(hwAddress, pwmChannel, 0, offTime);	
}