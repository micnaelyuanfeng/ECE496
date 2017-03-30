#ifndef DATA_MACRO_H
#define DATA_MACRO_H

#define block 			4
#define MAGICNUM 		2
#define ASCII_A 		65
#define BUFFER_SIZE 	50

 struct data{
	char * data;
	struct data * next;
	struct data * prev;
 };

 struct data * raw_head = NULL;
 struct data * raw_newdata = NULL;
 struct data * raw_curr = NULL; 

//servo
 #define SERVO_FREQUENCY 	50 
 #define SERVOHATADDR 		0x40
 #define PM_CHANNEL			0
 #define IP_CHANNEL			1
 #define FW_CHANNEL			2
 #define BW_CHANNEL			3
 #define RT_CHANNEL			4
 #define LF_CHANNEL			5
 #define UD_CHANNEL			6
 #define FB_CHANNEL			7
 #define LR_CHANNEL			8
 #define RO_CHANNEL			9
//motor
 #define MOTORADDRESS 		0x60
 #define SPEEDTEST 			220
 #define DIRECTIONTEST 		1
 #define MOTOR_FREQUENCY 	1600

 #define MOUDLATION			5



#endif