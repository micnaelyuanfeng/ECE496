#include "Standard_Library.h"
#include "System_Library.h"
#include "BCM2835.h"
#include "PCA9685.h"
#include "Adafruit_MotorHAT.h"
#include "Adafruit_ServoHAT.h"

#define MOTORADDRESS 0x60
#define CHANNELTEST 0
#define SPEEDTEST 220
#define DIRECTIONTEST 1
#define FREQUENCY 1600

int main(){
	// Generic setup
	map_peripheral_BCM2835(&gpio);
	map_peripheral_BCM2835(&bsc0);
	init_I2C_protocol();
	// Board setup
	init_PCA9685(MOTORADDRESS);
	set_PWM_frequency_PCA9685(MOTORADDRESS, FREQUENCY);
	stop_DC_motor(MOTORADDRESS, 0);
	stop_DC_motor(MOTORADDRESS, 1);
	stop_DC_motor(MOTORADDRESS, 2);
	stop_DC_motor(MOTORADDRESS, 3);
	
	// DC motor control 
	set_speed_DC_motor(MOTORADDRESS, CHANNELTEST, SPEEDTEST);
	set_direction_DC_motor(MOTORADDRESS, CHANNELTEST, DIRECTIONTEST);

	return 0;
}
