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
	init_PCA9685(0x40);
	set_PWM_frequency_PCA9685(0x40, 60);

	while(1){
		set_PWM_PCA9685(0x40, 0, 0, 150);	
		sleep(5);
		set_PWM_PCA9685(0x40, 0, 0, 600);	
		sleep(5);
	}
	return 0;
}
