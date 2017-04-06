#include "Standard_Library.h"
#include "System_Library.h"
#include "BCM2835.h"
#include "PCA9685.h"
#include "Adafruit_MotorHAT.h"
#include "Adafruit_ServoHAT.h"
#include "Inverse_Kinematics.h"

#define FREQUENCY 300 
#define SERVOHATADDR 0x40

int main(){
	
	map_peripheral_BCM2835(&gpio);
	map_peripheral_BCM2835(&bsc0);
	init_I2C_protocol();
	init_PCA9685(SERVOHATADDR);
	set_PWM_frequency_PCA9685(SERVOHATADDR, FREQUENCY);
	init_angle_to_pulse_length_lookup_table();
	/*while(1){
		//set_PWM_PCA9685(SERVOHATADDR, CHANNELTEST, 0, min);	
		set_servo(SERVOHATADDR, CHANNELTEST, FREQUENCY, 0);
		sleep(5);
		set_servo(SERVOHATADDR, CHANNELTEST, FREQUENCY, 90);
		sleep(5);
		set_servo(SERVOHATADDR, CHANNELTEST, FREQUENCY, 180);	
		sleep(5);
		set_servo(SERVOHATADDR, CHANNELTEST, FREQUENCY, 90);
		sleep(5);
	}*/
	inverseKinematics(0,30,25,t,angles);
	double baseServo = angles[1] * (180.0 / M_PI);
	if (baseServo < 30 )
		baseServo = 30;
	else if(baseServo > 150)
		baseServo = 150;
	double joint3 = angles[2] * (180.0 / M_PI);
	if(joint3 < 0) 
		joint3 = 0;
	else if (joint3 > 90)
		joint3 = 90;
	joint3 = 155 - joint3;
	
	double joint2 = angles[3] * (180.0 / M_PI);
	if(joint2 < 0) 
		joint2 = 0;
	else if (joint2 > 90)
		joint2 = 90;
	joint2 = 155 - joint2;
	
	double joint1 = angles[4] * (180.0 / M_PI);
	if(joint1 < 0) 
		joint1 = 0;
	else if (joint1 > 90)
		joint1 = 90;
	joint1 = 155 - joint1;
		printf("%f, %f, %f, %f, %f, %f, %f", angles[0], angles[1], angles[2], angles[3], angles[4], angles[5], angles[6]);

	
	
	set_servo(SERVOHATADDR, 6, FREQUENCY, (int)baseServo);
	set_servo(SERVOHATADDR, 0, FREQUENCY, (int)joint1);.
	set_servo(SERVOHATADDR, 1, FREQUENCY, (int)joint2);
	set_servo(SERVOHATADDR, 2, FREQUENCY, (int)joint3);



	
	
	return 0;
    

    
   
}
