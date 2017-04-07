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

 //Thread
 /*************************************************/
 pthread_t thread_base;
 int thread_id;

/*************conditional variable********************/
pthread_mutex_t m = PTHREAD_MUTEX_INITIALIZER;
pthread_cond_t c = PTHREAD_COND_INITIALIZER;
/******************************************************/

/*************************lock*************************/
pthread_mutex_t raw_data_lock = PTHREAD_MUTEX_INITIALIZER;
//pthread_mutex_t control_data_lock = PTHREAD_MUTEX_INITIALIZER;
/******************************************************/

 /***************function block***********************/
 void thread_maintain_database(void *);
 
 int main(void){
	char user_input[BUFFER_SIZE];
	uint8_t tid = 0;
	bzero((char *)&server_addr, sizeof(server_addr));
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
	
	init_PCA9685(MOTORADDRESS);
	set_PWM_frequency_PCA9685(MOTORADDRESS, MOTOR_FREQUENCY);
	stop_DC_motor(MOTORADDRESS, 0);
	stop_DC_motor(MOTORADDRESS, 1);
	stop_DC_motor(MOTORADDRESS, 2);
	stop_DC_motor(MOTORADDRESS, 3); 
	printf("Set Motor Driver...\n");
  
	/*****************************************/
	printf("main Thread Creating Threads\n");
	printf("Creating servo thread...\n");
	thread_id = pthread_create(&thread_base, NULL, (void*)thread_maintain_database, (void *)&tid);


	//main thread for getting control data from pi
	get_connection(&server_addr, &sockfd);
	listen(sockfd, 5);
	bzero((char *)&client_addr, sizeof(client_addr));
	clienlen = sizeof(client_addr);
	newconnection = accept(sockfd, (struct sockaddr*)&client_addr, &clienlen);

	if(newconnection < 0){
		perror("Accept Error: ");
		exit(1);
	}

	printf("Connected to a Client\n");
	//printf("************************\n");
	printf("Main thread listen to data port\n" );
	
	//main thread reading data from client
	//********************************************
	
	

	while(1){
		//printf("haha\n");
		bzero(user_input, BUFFER_SIZE);
		int n = read(newconnection, user_input, BUFFER_SIZE);
		
		if(n < 0)
			 perror("error: ");
		else if (n == 0) 
			continue;
		
		
		//printf("dido\n");
		if(n < BUFFER_SIZE){    
			raw_newdata = (struct data *)malloc(sizeof(struct data));
			raw_newdata->data = (char *)malloc(sizeof(char) * BUFFER_SIZE);

			
			pthread_mutex_lock(&raw_data_lock);
			if(raw_head == NULL){
				raw_head = raw_newdata;
				raw_head->next = NULL;
				raw_head->prev = raw_head;
				strcpy(raw_head->data, user_input);
				printf("new data: %s\n",raw_head->data);
				raw_curr = raw_head;

				printf("-----------------------\n");
				raw_curr = raw_head;
				int counter = 0;

				while(raw_curr != NULL){
					//printf("old data: %s\n", raw_curr->data);
					raw_curr = raw_curr->next;
					counter++;
				}
				//printf("counter is %d\n", counter);
				//printf("-----------------------\n");
			}
			else if(raw_head != NULL){
				raw_curr = raw_head;	
				while(raw_curr->next != NULL)
					raw_curr = raw_curr->next;
				

				raw_curr->next = raw_newdata;
				strcpy(raw_newdata->data, user_input);
				raw_newdata->next = NULL;
				raw_newdata->prev = raw_curr;
				//printf("new data: %s\n",raw_head->data);

				//printf("-----------------------\n");
				raw_curr = raw_head;
				int counter = 0;
				while(raw_curr != NULL){
					//printf("old data: %s\n", raw_curr->data);
					raw_curr = raw_curr->next;
					counter++;
				}
				//printf("counter is %d\n", counter);
				//printf("-----------------------\n");
				//printf("%s\n",raw_newdata->data);
			}
			pthread_mutex_unlock(&raw_data_lock);
		}	   
	}

	pthread_exit(NULL);
// ERROR_HANDLER:

 }


 void thread_maintain_database(void * thread_id){
	//recevie control terminal and maintan databse
	struct data * raw_curr;
	struct data * raw_prev = NULL;
	//struct servo * servo_curr;
	printf("maintan thread is created\n");

	char data[BUFFER_SIZE];

	//inital set up to all middle positions
	set_servo(SERVOHATADDR, 6 , SERVO_FREQUENCY, 90);
					set_servo(SERVOHATADDR, 2 , SERVO_FREQUENCY, 155);
					set_servo(SERVOHATADDR, 0 , SERVO_FREQUENCY, 155);
					set_servo(SERVOHATADDR, 9, SERVO_FREQUENCY, 155);
					set_servo(SERVOHATADDR, 4 , SERVO_FREQUENCY, 180);
	set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, 255);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, 0);
						set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, 255);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, 0);

	while(1){
		
		raw_curr = raw_head;
		
		if(raw_curr != NULL){
			pthread_mutex_lock(&raw_data_lock);
			if(raw_head != NULL){
				raw_curr = raw_head;	
				while(raw_curr->next != NULL){
					raw_curr = raw_curr->next;
					raw_prev = raw_curr;
					
				}

				if(raw_prev == NULL){
					strcpy(data, raw_curr->data);
					free(raw_curr->data);
					free(raw_curr);
					raw_head = NULL;
				}
				else{
					strcpy(data, raw_curr->data);
					raw_curr->prev->next = NULL;

					free(raw_curr->data);
					free(raw_curr);
				}


				char *array[10];
				int i=0;

				array[i] = strtok(data,":");

				while(array[i]!=NULL && i < 10)
				{
				   array[++i] = strtok(NULL,":");
				}
				if (!strcmp(array[0], "1")){
					  
                	int baseServo = atoi(array[1]);
                	int shoulderServo = atoi(array[2]);
                	int elbowServo = atoi (array[3]);
                	int handRoll = atoi(array[4]);
                	int rollServo = 155+handRoll;
                	if(rollServo > 250){
						rollServo = 250;
					}else if (rollServo < 20){
						rollServo = 20;
					}
                	double claw = atof(array[5]);
                	int clawServo;
                	if(claw < 0.5)
                	{
                		clawServo = 50;
                	}else if (claw == 0.5){
                		clawServo = 125;
                	}else{
                		clawServo = 180;
                	}
                	printf("baseServo: %d, shoulderServo: %d, elbowServo: %d, rollServo: %d\n", baseServo, shoulderServo, elbowServo, rollServo);
                	set_servo(SERVOHATADDR, 6 , SERVO_FREQUENCY, baseServo);
					set_servo(SERVOHATADDR, 2 , SERVO_FREQUENCY, shoulderServo);
					set_servo(SERVOHATADDR, 0 , SERVO_FREQUENCY, elbowServo);
					set_servo(SERVOHATADDR, 9, SERVO_FREQUENCY, rollServo);
					set_servo(SERVOHATADDR, 4 , SERVO_FREQUENCY, clawServo);


				}else if (!strcmp(array[0], "2")){
					int handRoll = atoi(array[1]);
                	int rollServo = 155+handRoll;
                	double claw = atof(array[2]);
                	int clawServo;
                	if(claw < 0.5){
                		clawServo = 50;
                	}else if (claw == 0.5){
                		clawServo = 125;
                	}else{
                		clawServo = 180;
                	}
                	if(rollServo > 250){
						rollServo = 250;
					}else if (rollServo < 20){
						rollServo = 20;
					}
                	printf("rollServo: %d, clawServo: %d\n", rollServo, clawServo);
                	set_servo(SERVOHATADDR, 9 , SERVO_FREQUENCY, rollServo);
                	set_servo(SERVOHATADDR, 4 , SERVO_FREQUENCY, clawServo);
				}else if (!strcmp(array[0], "3")){
					int M1Direction = atoi(array[1]);
					int M2Direction = atoi(array[2]);
					if(M1Direction == 0){
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, DIRECTION_ST);
						printf("M1 Stop");
					}else if(M1Direction == 1){
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, DIRECTION_FW);
						printf("M1 Forward");
					}else if(M1Direction == 2){
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, DIRECTION_BW);
						printf("M1 Backward");
					}
					
					if(M2Direction == 0){
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, DIRECTION_ST);
						printf("M2 Stop");
					}else if(M2Direction == 1){
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, DIRECTION_FW);
						printf("M2 Forward");
					}else if(M2Direction == 2){
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, DIRECTION_BW);
						printf("M2 Backward");
					}
				}
						



				/*
				if(hand_x_position > 90)
					hand_x_position = 90;
				else if(hand_x_position < -90)
					hand_x_position = -90;

				if(hand_y_position > 90)
					hand_y_position = 90;
				else if (hand_y_position < -90)
					hand_y_position = -90;

				hand_y_position += 90;

				if(hand_z_position > 70)
					hand_z_position = 70;
				if (hand_z_position < -60)
					hand_z_position = -50;

				hand_z_position += 80;
				
				printf("pm_degree: %d\n", pm_degree);
				printf("ip_degree: %d\n", ip_degree);
				printf("hand_x_position: %d\n", hand_x_position);
				printf("hand_y_position: %d\n", hand_y_position);
				printf("hand_z_position: %d\n", hand_z_position);
				printf("roll_degree: %d\n", roll_degree);
				printf("direction: %d\n", direction);

			    if(hand_y_position >= 30 && hand_y_position <= 180){
					if(hand_z_position >= 0 && hand_z_position < 80){
						knee_2 = 95;
						knee_1 = 100;
					}
					else if(hand_z_position >= 80 && hand_z_position <= 150){
						knee_2 = 120;
						knee_1 = 120;
					}
				}
				else if(hand_y_position >= 0 && hand_y_position < 30){
					if(hand_z_position >= 0 && hand_z_position < 80){
						knee_2 = 105;
						knee_1 = 90;
					}
					else if(hand_z_position >= 80 && hand_z_position <= 150){
						knee_2 = 135;//120
						knee_1 = 120;
					}
				}

				if(hand_x_position >= 90){
					hand_x_position = 90;
				}
				else if (hand_x_position < 0){
					if(hand_x_position < - 90)
						hand_x_position = -90;
				}
				hand_x_position = hand_x_position + 90;



				if(roll_degree > 90)
					roll_degree = 90;
				else if (roll_degree < -90)
					roll_degree = -90;
 
				roll_degree += 90;


				if(pm_degree >= 10)
					pm_degree += 20;
				else if(pm_degree >= 20)
					pm_degree += 30;
				else if(pm_degree >= 30)
					pm_degree += 40;
				else if(pm_degree >= 40)
					pm_degree += 50;
				else if(pm_degree >= 50)
					pm_degree += 60;
				else if(pm_degree >= 60)
					pm_degree += 70;
				else if(pm_degree >= 70)
					pm_degree += 80;
				else if (pm_degree >= 80)
					pm_degree += 90;
				else if(pm_degree <= 0)
					pm_degree += 10;
 				*/
 				//printf("knee1=%d,knee2=%d,hand_z=%d\n",knee_1,knee_2,hand_z_position);
 				//set_servo(SERVOHATADDR, 0 , SERVO_FREQUENCY, knee_1);
				//set_servo(SERVOHATADDR, 1 , SERVO_FREQUENCY, knee_2);
				//set_servo(SERVOHATADDR, 2 , SERVO_FREQUENCY, hand_z_position);
			/*
				
				switch(direction){
					case 1:
						printf("case1/n");
						set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, 255);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, DIRECTION_FW);
						set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, 255);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, DIRECTION_FW);
						break;
					case 2:
						set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, 255);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, DIRECTION_BW);
						set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, 255);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, DIRECTION_BW);
						break;
					case 3:
						set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, 255);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, DIRECTION_BW);
						set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, 255);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, DIRECTION_FW);
						break;
					case 4:
						set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, 255);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, DIRECTION_FW);
						set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, 255);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, DIRECTION_BW);
						break;
					case 5:
						set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, 0);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, DIRECTION_ST);
						set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, 0);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, DIRECTION_ST);
						break;
					default:
						set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, 0);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_1, DIRECTION_ST);
						set_speed_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, 0);
						set_direction_DC_motor(MOTORADDRESS, MOTOR_CHANNEL_2, DIRECTION_ST);
						break;

				}*/

				bzero(data, BUFFER_SIZE);
				pthread_mutex_unlock(&raw_data_lock);
			}
    	}

    	raw_curr = NULL;
    	raw_prev = NULL;
	}

	pthread_exit(NULL);
 }
 

