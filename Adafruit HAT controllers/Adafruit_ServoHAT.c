#include "Standard_Library.h"
#include "System_Library.h"
#include "PCA9685.h"
#include "Adafruit_ServoHAT.h"


double angleToDutyCycleLookupTable[37];

void init_angle_to_duty_cycle_lookup_table(){
	int i;
	for (i = 0; i < 37; i++){
		//0.1ms = 9 degrees -> 5 degrees = 0.06ms -> 3 degrees = 0.0333
		angleToDutyCycleLookupTable[i] = 0.5 + i * 0.0556 ;
	}
	printf("******Angle To Duty Cycle Lookup Table******\n");
	printf("Index\tAngle\tDuty Cycle Length\t\n");
	for (i = 0; i < 37; i++){
		printf("%d\t%d\t%f\n", i, i*5, angleToDutyCycleLookupTable[i]);
	}		  
}

uint16_t angle_to_duty_cycle_percentage_converter(double * ptLookupTable, uint8_t angleDegree){
	double dutyCycleLength = ptLookupTable[angleDegree/5];
	double dutyCyclePercentage = ((dutyCycleLength + 0.00005)/ 20) * 4096;
	return (uint16_t)dutyCyclePercentage;

}

void set_servo(uint8_t hwAddress, uint8_t pwmChannel, uint8_t angleDegree){
	uint16_t offTime = angle_to_duty_cycle_percentage_converter(angleToDutyCycleLookupTable, angleDegree);
	set_PWM_PCA9685(hwAddress, pwmChannel, 0, offTime);	
}

