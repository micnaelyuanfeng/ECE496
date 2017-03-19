#include "servo.h"
#include "i2c.h"
#include <stdio.h>
#include <stdlib.h>

void raw_data_initialization(void){

	;
 }

 void servo_database_initialization(void){

	servo_head = NULL;
	servo_curr = NULL;
	servo_prev = NULL;
	printf("Servo initialization done\n");
 }
 
 void servo_setup(void){
	
	uint8_t i;

	for(i = 0; i < 16; i++){
		PCA9685_SetOutput(i, 10);
	}
}