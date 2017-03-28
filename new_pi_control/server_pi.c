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

 #define block 4;
 #define MAGICNUM 2
 #define ASCII_A 65
 //Thread
 /*************************************************/
 pthread_t thread_base[2];
 int thread_fd[2];
 int thread_id[2];

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

 char* chomp(char *);
 //uint8_t hash_key(void);
 uint8_t mystoi(char *);
 
 
 struct data{
	char * data;
	struct data * next;
	struct data * prev;
 };

 struct servo{
 	uint8_t finger;
	uint8_t degree;
	uint8_t last_degree;

	struct servo * next;
	struct servo * prev;
 };

 struct servo * servo_head = NULL;
 struct servo * servo_curr = NULL;
 struct servo * servo_prev = NULL;

 struct data * raw_head = NULL;
 struct data * raw_newdata = NULL;
 struct data * raw_curr = NULL; 
/*************************************************/
//servo
 #define SERVO_FREQUENCY 50 
 #define SERVOHATADDR 0x40
 #define CHANNELTEST 0
 
//motor
 #define MOTORADDRESS 0x60
 #define CHANNELTEST 0
 #define SPEEDTEST 220
 #define DIRECTIONTEST 1
 #define MOTOR_FREQUENCY 1600
/*************************************************/

 int main(void){
 
	uint8_t thread_counter = 0;
	char user_input[50];
	
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
	
	//init_PCA9685(MOTORADDRESS);
	//set_PWM_frequency_PCA9685(MOTORADDRESS, MOTOR_FREQUENCY);
	//stop_DC_motor(MOTORADDRESS, 0);
	//stop_DC_motor(MOTORADDRESS, 1);
	//stop_DC_motor(MOTORADDRESS, 2);
	//stop_DC_motor(MOTORADDRESS, 3);
	
	printf("Set Motor Driver...\n");
  
	/*****************************************/
	printf("main Thread Creating Threads\n");
	printf("Creating servo thread %d\n", thread_counter);
	thread_id[thread_counter] = pthread_create(&thread_base[thread_counter], NULL, (void*)thread_maintain_database, (void *)&thread_counter);


	//main thread for getting control data from pi
	get_connection(&server_addr, &sockfd);
	listen(sockfd, 128);
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
		bzero(user_input, 50);
		int n = read(newconnection, user_input, 50);
		
		if(n < 0)
			 perror("error: ");
		else if (n == 0) 
			continue;
		
		
		//printf("dido\n");
		if(n < 40){    
			raw_newdata = (struct data *)malloc(sizeof(struct data));
			raw_newdata->data = (char *)malloc(sizeof(char) * 50);

			
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
					printf("old data: %s\n", raw_curr->data);
					raw_curr = raw_curr->next;
					counter++;
				}
				//printf("counter is %d\n", counter);
				printf("-----------------------\n");
			}
			else if(raw_head != NULL){
				raw_curr = raw_head;	
				while(raw_curr->next != NULL)
					raw_curr = raw_curr->next;
				

				raw_curr->next = raw_newdata;
				strcpy(raw_newdata->data, user_input);
				raw_newdata->next = NULL;
				raw_newdata->prev = raw_curr;
				printf("new data: %s\n",raw_head->data);

				printf("-----------------------\n");
				raw_curr = raw_head;
				int counter = 0;
				while(raw_curr != NULL){
					printf("old data: %s\n", raw_curr->data);
					raw_curr = raw_curr->next;
					counter++;
				}
				//printf("counter is %d\n", counter);
				printf("-----------------------\n");
				printf("%s\n",raw_newdata->data);
			}
			pthread_mutex_unlock(&raw_data_lock);
		}	
	}

	pthread_exit(NULL);
// ERROR_HANDLER:

 }


 void thread_maintain_database(void * thread_id){
	//recevie control terminal and maintan databse
	//uint16_t servo_number;
	//uint8_t id = *((uint8_t*)thread_id);
	struct servo * newservodata;
	struct data * raw_curr;
	struct data * raw_prev = NULL;
	//struct servo * servo_curr;
	printf("maintan thread is created\n");

	char *token;
	char data[50];
	uint8_t finger_number; //same as thread_id and index
	//uint8_t anker_number_1, anker_number_2, anker_number_3;
	uint8_t degree_1, degree_2, degree_3;
	uint8_t last_degree_1 = 0;
    uint8_t	last_degree_2 = 0;
    uint8_t	last_degree_3 = 0;
	uint8_t data_counter = 0;

	while(1){
		raw_curr = raw_head;
		/*
		set_servo(SERVOHATADDR, CHANNELTEST, SERVO_FREQUENCY, 0);
		sleep(1);
		set_servo(SERVOHATADDR, CHANNELTEST, SERVO_FREQUENCY, 90);
		sleep(1);
		set_servo(SERVOHATADDR, CHANNELTEST, SERVO_FREQUENCY, 180);	
		sleep(1);
		set_servo(SERVOHATADDR, CHANNELTEST, SERVO_FREQUENCY, 90);
		sleep(1);
		*/
		
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
			
				token = strtok(data, "-");
				//printf("hehe\n");
				while(token != NULL){

					if(data_counter == 0){
						finger_number = mystoi(token);
						//printf("finger %d\n", finger_number);
						data_counter ++;
					}
					else if(data_counter == 1){
						//printf("parsing anker_1 %s\n", token);
						//anker_number_1 = mystoi(token);
						//printf("parsed anker_1 %d\n", anker_number_1);
						data_counter++;
					}
					else if (data_counter == 2){
						//printf("parsing degree_1 is %s\n", token);
						degree_1 = mystoi(token);
						
						if(degree_1 > 180)
							degree_1 = last_degree_1;
						//printf("parsed degree_1 is %d\n", degree_1);
						data_counter++;
					}
					else if (data_counter == 3){
						//printf("parsing anker_2 is %s\n", token);
						//anker_number_2 = mystoi(token);
						//printf("parsed anker_2 is %d\n", anker_number_2);
						data_counter++;
					}
					else if (data_counter == 4){
						//printf("parsing degree_2 is %s\n", token);
						degree_2 = mystoi(token);
						
						if(degree_2 > 180)
							degree_2 = last_degree_2;
						//printf("parsed degree_2 is %d\n", degree_2);
						data_counter++;
					}
					else if (data_counter == 5){
						//printf("parsing anker_3 is %s\n", token);
						//anker_number_3 = mystoi(token);
						//printf("parsed anker_3 is %d\n", anker_number_3);
						data_counter++;
					}
					else if (data_counter == 6){
						//printf("parsing degree_3 is %s\n", token);
						degree_3 = mystoi(token);
						
						if(degree_3 > 180)
							degree_3 = last_degree_3;
						data_counter = 0;
					}

					token = strtok(NULL, "-");

				}
		
				newservodata = (struct servo *)malloc(sizeof(struct servo));
			

				newservodata->finger = finger_number;
				newservodata->last_degree = last_degree_1; 
				newservodata->degree = degree_1;
				
				
				last_degree_1 = degree_1;
		

				if(newservodata->degree != newservodata->last_degree){
					set_servo(SERVOHATADDR, CHANNELTEST, SERVO_FREQUENCY, degree_2);
				}

			
				free(newservodata);					
				pthread_mutex_unlock(&raw_data_lock);
			}
    	}
    	raw_curr = NULL;
    	raw_prev = NULL;
	}

	pthread_exit(NULL);
 }

 char * chomp(char * string){

	int length = strlen(string);
	int i = 0;
	char * newstring = (char *)malloc((length -1)*sizeof(char));

	for(i = 0; i < (length-1); i ++){
		newstring[i] = string[i];
	}
	
	//need to add, then there is extra '0', not 
	newstring[i] = 0;
	
	return newstring;
 }

uint8_t mystoi(char * str){

	uint8_t length = strlen(str);
	uint8_t i;
	uint8_t multiplier;
	uint8_t real_integer = 0;
	// printf("length is %d\n", length);
	for(i = 0;  i < length; i++){
		
		if(i == 0)
			multiplier = 1;
		else 
			multiplier = 10;

		real_integer = real_integer*multiplier + (str[i] - '0');
	}

	return real_integer;
 }