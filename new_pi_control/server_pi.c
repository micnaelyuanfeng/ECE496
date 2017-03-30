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
	printf("************************\n");
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
	int i;
	//uint8_t finger_number; //same as thread_id and index
	//uint8_t anker_number_1, anker_number_2, anker_number_3;
	int ip_degree;
	int pm_degree;
	int forward_displacement;
	int backward_displacement;
	int leftward_displacement;
	int rightward_displacement;
	
	int hand_x_position;
	int hand_y_position;
	int hand_z_position;

	int roll_degree; 


	//inital set up to all middle positions
    set_servo(SERVOHATADDR, PM_CHANNEL , SERVO_FREQUENCY, 90);
	set_servo(SERVOHATADDR, IP_CHANNEL , SERVO_FREQUENCY, 90);
	set_servo(SERVOHATADDR, FW_CHANNEL , SERVO_FREQUENCY, 0);
	set_servo(SERVOHATADDR, BW_CHANNEL , SERVO_FREQUENCY, 0);
	set_servo(SERVOHATADDR, RT_CHANNEL , SERVO_FREQUENCY, 0);
	set_servo(SERVOHATADDR, LF_CHANNEL , SERVO_FREQUENCY, 0);
	set_servo(SERVOHATADDR, UD_CHANNEL , SERVO_FREQUENCY, 90);
	set_servo(SERVOHATADDR, FB_CHANNEL , SERVO_FREQUENCY, 90);
	set_servo(SERVOHATADDR, LR_CHANNEL , SERVO_FREQUENCY, 90);
	set_servo(SERVOHATADDR, RO_CHANNEL , SERVO_FREQUENCY, 90);


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

				sscanf(data, "%d:%d:%d:%d:%d:%d:%d:%d:%d:%d:%d", 
						&i, &pm_degree, &ip_degree,
						&forward_displacement, &backward_displacement,
						&rightward_displacement, &leftward_displacement,
						&hand_x_position, &hand_y_position, &hand_z_position,
						&roll_degree
					   );
				/*
				printf("pm_degree: %d\n", pm_degree);
				printf("ip_degree: %d\n", ip_degree);
				printf("hand_x_position: %d\n", hand_x_position);
				printf("hand_y_position: %d\n", hand_y_position);
				printf("hand_z_position: %d\n", hand_z_position);
				printf("roll_degree: %d\n", roll_degree);
				*/

				if(hand_x_position > 90)
					hand_x_position = 90;
				else if (hand_x_position < -90)
					hand_x_position = -90;

				hand_x_position += 90;

				if(hand_y_position > 90)
					hand_y_position = 90;
				else if (hand_y_position < -90)
					hand_y_position = -90;

				hand_y_position += 90;

				if(hand_z_position > 90)
					hand_z_position = 90;
				if (hand_z_position < -90)
					hand_z_position = -90;

				hand_z_position += 90;

				if(roll_degree > 90)
					roll_degree = 90;
				else if (roll_degree < -90)
					roll_degree = -90;

				roll_degree += 90;

				if(ip_degree > 90)
					ip_degree = 90;
				else if (ip_degree < -90)
					ip_degree = -90;

				ip_degree += 90;

				if(pm_degree > 90)
					pm_degree = 90;
				else if (pm_degree < -90)
					pm_degree = -90;

				pm_degree += 90;

				/*
				printf("pm_degree: %d\n", pm_degree);
				printf("ip_degree: %d\n", ip_degree);
				printf("hand_x_position: %d\n", hand_x_position);
				printf("hand_y_position: %d\n", hand_y_position);
				printf("hand_z_position: %d\n", hand_z_position);
				printf("roll_degree: %d\n", roll_degree);
				*/

				set_servo(SERVOHATADDR, PM_CHANNEL , SERVO_FREQUENCY, pm_degree);
				set_servo(SERVOHATADDR, IP_CHANNEL , SERVO_FREQUENCY, ip_degree);
				set_servo(SERVOHATADDR, FW_CHANNEL , SERVO_FREQUENCY, 5);
				set_servo(SERVOHATADDR, BW_CHANNEL , SERVO_FREQUENCY, 5);
				set_servo(SERVOHATADDR, RT_CHANNEL , SERVO_FREQUENCY, 5);
				set_servo(SERVOHATADDR, LF_CHANNEL , SERVO_FREQUENCY, 5);
				set_servo(SERVOHATADDR, UD_CHANNEL , SERVO_FREQUENCY, hand_x_position);
				set_servo(SERVOHATADDR, FB_CHANNEL , SERVO_FREQUENCY, hand_y_position);
				set_servo(SERVOHATADDR, LR_CHANNEL , SERVO_FREQUENCY, hand_z_position);
				set_servo(SERVOHATADDR, RO_CHANNEL , SERVO_FREQUENCY, roll_degree);
				
				bzero(data, BUFFER_SIZE);
				pthread_mutex_unlock(&raw_data_lock);
			}
    	}

    	raw_curr = NULL;
    	raw_prev = NULL;
	}

	pthread_exit(NULL);
 }
