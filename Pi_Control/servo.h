 #include <stdbool.h>
 #include <stdint.h>
 /*************************************************/
 //share data_blocks
 /************************************************/

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

 struct servo * servo_head;
 struct servo * servo_curr;
 struct servo * servo_prev;


 void raw_data_initialization(void);
 void servo_database_initialization(void);
 void servo_setup(void);