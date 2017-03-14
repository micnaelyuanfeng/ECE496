#include "Standard_Library.h"
#include "System_Library.h"
#include "BCM2835.h"
#include "PCA9685.h"
#include "Adafruit_MotorHAT.h"
#include "Adafruit_ServoHAT.h"

#define FREQUENCY 300 
#define SERVOHATADDR 0x40
#define CHANNELTEST 0
int main(){
	map_peripheral_BCM2835(&gpio);
	map_peripheral_BCM2835(&bsc0);
	init_I2C_protocol();
	init_PCA9685(SERVOHATADDR);
	set_PWM_frequency_PCA9685(SERVOHATADDR, FREQUENCY);
	int middle = pulse_length_to_tick_converter(FREQUENCY, 1500);
	int min = pulse_length_to_tick_converter(FREQUENCY, 500);
	int max = pulse_length_to_tick_converter(FREQUENCY, 2500);
	printf("min: %d, middle: %d, max: %d\n", min, middle, max);
	init_angle_to_pulse_length_lookup_table();
	while(1){
		//set_PWM_PCA9685(SERVOHATADDR, CHANNELTEST, 0, min);	
		set_servo(SERVOHATADDR, CHANNELTEST, FREQUENCY, 0);
		sleep(5);
		set_servo(SERVOHATADDR, CHANNELTEST, FREQUENCY, 90);
		sleep(5);
		set_servo(SERVOHATADDR, CHANNELTEST, FREQUENCY, 180);	
		sleep(5);
		set_servo(SERVOHATADDR, CHANNELTEST, FREQUENCY, 90);
		sleep(5);
	}
	return 0;
}
