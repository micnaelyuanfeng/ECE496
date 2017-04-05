 #include "wifi.h"
 #include <pthread.h>
 #include <signal.h>
 #include "Adafruit_MotorHAT.h"
 #include "Adafruit_ServoHAT.h"
 #include <time.h>
 #include <sys/time.h>
 #include <math.h>
 #include "BCM2835.h"
 #include "Standard_Library.h"
 #include "System_Library.h"
 #include "PCA9685.h"
 #include "Data_Macro.h"
 
 
 int main()
 {
	 	/******************************************
	Peripheral initialization
	gpio and i2c PCA9685
	Gain access to peripheral memory structures
	******************************************/
	map_peripheral_BCM2835(&gpio);
	map_peripheral_BCM2835(&bsc0);
	init_I2C_protocol();
	
	// Board setup 
	init_PCA9685(SERVOHATADDR);
	set_PWM_frequency_PCA9685(SERVOHATADDR, SERVO_FREQUENCY);
	init_angle_to_pulse_length_lookup_table();
	
	printf("Set Servo Driver...\n");
	
	//set_servo(SERVOHATADDR, 0 , SERVO_FREQUENCY, 120);
	//set_servo(SERVOHATADDR, 1 , SERVO_FREQUENCY, 120);
	set_servo(SERVOHATADDR, 2 , SERVO_FREQUENCY, 150);
	 return 0;
 }
