#include "Standard_Library.h"
#include "System_Library.h"
#include "PCA9685.h"
#include "Adafruit_ServoHAT.h"


int angleToPulseLengthLookupTable[271];

void init_angle_to_pulse_length_lookup_table(){
	int i;
	for (i = 0; i < 271; i++){
		// 0 -> 500, 270 -> 2500, 135 -> 1500
		angleToPulseLengthLookupTable[i] = (int)(500 + i * 7.4);
	}
	printf("******Angle To Duty Cycle Lookup Table******\n");
	printf("Index\tAngle\tPulse Length\t\n");
	for (i = 0; i < 271; i++){
		printf("%d\t%d\t%d\n", i, i, angleToPulseLengthLookupTable[i]);
	}		  
}



int pulse_length_to_tick_converter(int frequency, int pulseLength){
	double periodLength = (1.0 / frequency) * 1000000;
	double timePerTick = periodLength / 4096.0;
	double tick = pulseLength / timePerTick;
	return ((int)tick);
}

void set_servo(uint8_t hwAddress, uint8_t pwmChannel, int frequency, int angleDegree){
	
	if(angleDegree < 0 || angleDegree > 271){
		printf("angle %d is out of servo range. Exit....\n", angleDegree);
		exit(1);
	}
	int pulseLength = angleToPulseLengthLookupTable[angleDegree];
	uint16_t offTime = (uint16_t)pulse_length_to_tick_converter(frequency, pulseLength);
	set_PWM_PCA9685(hwAddress, pwmChannel, 0, offTime);	
}
