#include "Standard_Library.h"
#include "System_Library.h"
#include "BCM2835.h"
#include "PCA9685.h"
#include "Adafruit_MotorHAT.h"
#include "Adafruit_ServoHAT.h"

int main(){
	map_peripheral_BCM2835(&gpio);
	map_peripheral_BCM2835(&bsc0);
	init_I2C_protocol();
	init_PCA9685(0x60);
	set_PWM_frequency_PCA9685(0x60, 1600);
	stop_DC_motor(0x60, 0);
	stop_DC_motor(0x60, 1);
	stop_DC_motor(0x60, 2);
	stop_DC_motor(0x60, 3);

	set_speed_DC_motor(0x60, 0, 220);
	set_direction_DC_motor(0x60, 0, 1);

	return 0;
}
