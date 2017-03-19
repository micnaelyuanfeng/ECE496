 #include "i2c.h"

struct bcm2835_peripheral gpio = {GPIO_BASE};
struct bcm2835_peripheral bsc0 = {BSC0_BASE};

//i2c mode register and clock

//double counter_lookup[61];

// Exposes the physical address defined in the passed structure using mmap on /dev/mem
int map_peripheral(struct bcm2835_peripheral *p)
{
   // Open /dev/mem
   if ((p->mem_fd = open("/dev/mem", O_RDWR|O_SYNC) ) < 0) {
      printf("Failed to open /dev/mem, try checking permissions.\n");
      return -1;
   }

   p->map = mmap(
      NULL,
      BLOCK_SIZE,
      PROT_READ|PROT_WRITE,
      MAP_SHARED,
      p->mem_fd,  // File descriptor to physical memory virtual file '/dev/mem'
      p->addr_p      // Address in physical map that we want this memory block to expose
   );

   if (p->map == MAP_FAILED) {
        perror("mmap");
        return -1;
   }

   p->addr = (volatile unsigned int *)p->map;

   return 0;
}

void unmap_peripheral(struct bcm2835_peripheral *p) {
	
	if(p == NULL){
		printf("No Mapped Peripheral\n");
	}
	
    munmap(p->map, BLOCK_SIZE);
    close(p->mem_fd);
}

void dump_bsc_status(void) {

    uint32_t s = BSC0_S;

    printf("BSC0_S: ERR=%d  RXF=%d  TXE=%d  RXD=%d  TXD=%d  RXR=%d  TXW=%d  DONE=%d  TA=%d\n",
        (s & BSC_S_ERR) != 0,
        (s & BSC_S_RXF) != 0,
        (s & BSC_S_TXE) != 0,
        (s & BSC_S_RXD) != 0,
        (s & BSC_S_TXD) != 0,
        (s & BSC_S_RXR) != 0,
        (s & BSC_S_TXW) != 0,
        (s & BSC_S_DONE) != 0,
        (s & BSC_S_TA) != 0 );
}

// Function to wait for the I2C transaction to complete
void wait_i2c_done(void) {
        //Wait till done, let's use a timeout just in case
        int timeout = 50;
		
        while((!((BSC0_S) & BSC_S_DONE)) && --timeout) {
            usleep(1000);
        }
		
        if(timeout == 0)
            printf("wait_i2c_done() timeout. Something went wrong.\n");
}


//write register value
void PCA9685_SetRegister(uint8_t reg_address, uint8_t reg_value ){
		
		BSC0_A = 0x40;
		
		BSC0_DLEN = 2;
		BSC0_FIFO = (uint8_t) reg_address;
		BSC0_FIFO = (uint8_t)reg_value;
		
		BSC0_S = CLEAR_STATUS;
		BSC0_C = START_WRITE;
		
		wait_i2c_done();
}

void i2c_init(void)
{
    INP_GPIO(0);
    SET_GPIO_ALT(0, 0);
    INP_GPIO(1);
    SET_GPIO_ALT(1, 0);
} 

uint16_t map_to_DutyCycle(double * head, uint8_t degree){
  
  int index;
  double dutyCycle_length;
  uint16_t dutyCycle_percentage;
 
  index = degree/5;
  //printf("index is %d\n", index);
  dutyCycle_length = head[index];
  // printf("duty length = %f\n", head[index]); 
  dutyCycle_percentage = ((dutyCycle_length + 0.00005)/ 20) * 4096;
  // printf("duty  percent = %d\n",  dutyCycle_percentage);
 
  return dutyCycle_percentage;
}

void PCA9685_SetOutput(uint8_t channel, uint8_t degree){
	
	uint16_t dutyCycle;
	
	dutyCycle = (uint16_t)(map_to_DutyCycle(counter_lookup, degree));

	//for debug purpose
	// printf("\n*********************\n");
	// printf("degree is %d\n", degree);
	// printf("duty cycle ON start @ 00\n");
	// printf("duty cycle OFF start @ %d\n", dutyCycle);
	// printf("***********************\n");
	
	if(channel == 0){
		PCA9685_SetRegister(LED0_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED0_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED0_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED0_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	else if(channel == 1){
		PCA9685_SetRegister(LED1_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED1_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED1_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED1_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	else if(channel == 2){
		PCA9685_SetRegister(LED2_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED2_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED2_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED2_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	 if(channel == 3){
		PCA9685_SetRegister(LED3_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED3_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED3_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED3_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	else if(channel == 4){
		//printf("write 4\n");
		PCA9685_SetRegister(LED4_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED4_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED4_OFF_L, (uint8_t)((dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED4_OFF_H, (uint8_t)((dutyCycle >> 8) & 0x0F));
	}
	else if(channel == 5){
		PCA9685_SetRegister(LED5_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED5_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED5_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED5_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	else if(channel == 6){
		PCA9685_SetRegister(LED6_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED6_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED6_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED6_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	 else if(channel == 7){
		//printf("write 7\n");
		PCA9685_SetRegister(LED7_ON_L, (uint8_t)0x00);
		PCA9685_SetRegister(LED7_ON_H, (uint8_t)0x00);
		PCA9685_SetRegister(LED7_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED7_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	else if(channel == 8){
		PCA9685_SetRegister(LED8_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED8_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED8_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED8_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	else if(channel == 9){
		PCA9685_SetRegister(LED9_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED9_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED9_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED9_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	else if(channel == 10){
		PCA9685_SetRegister(LED10_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED10_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED10_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED10_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	else if(channel == 11){
		PCA9685_SetRegister(LED11_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED11_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED11_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED11_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	else if(channel == 12){
		PCA9685_SetRegister(LED12_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED12_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED12_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED12_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	else if(channel == 13){
		PCA9685_SetRegister(LED13_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED13_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED13_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED13_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	else if(channel == 14){
		PCA9685_SetRegister(LED14_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED14_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED14_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED14_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	} 
	else if(channel == 15){
		PCA9685_SetRegister(LED15_ON_L, (uint8_t) 0x00);
		PCA9685_SetRegister(LED15_ON_H, (uint8_t) 0x00);
		PCA9685_SetRegister(LED15_OFF_L, (uint8_t)(( dutyCycle) & 0xFF));
		PCA9685_SetRegister(LED15_OFF_H, (uint8_t)(( dutyCycle >> 8) & 0x0F));
	}
	
}

void lookup_initialize(double *head){
 
	int i;

	for (i = 0; i < 37; i++){
		//0.1ms = 9 degrees -> 5 degrees = 0.06ms -> 3 degrees = 0.0333
		head[i] = 0.5 + i * 0.0556 ;
	}
		  
}

void PCA9685_init(unsigned short frequency){
		
		//int i;
		
		PCA9685_SetRegister(MODE1, 0x80);
		//sleep(1);
		PCA9685_SetRegister(MODE1, 0x00);
		PCA9685_SetRegister(MODE2, 0x04);
		
		//Activate address
		BSC0_A = 0x40;
		
		PCA9685_SetRegister(MODE1, (uint8_t) 0x10);
		PCA9685_SetRegister( PRE_SCALE, (uint8_t)(((CPU_CLK/4096) / frequency) -1));
		// printf("frequency is %d\n", frequency);
		// printf("frequency is %f\n", CPU_CLK);
		// printf("frequency is %f\n", CPU_CLK/4096);
		// printf("Clock is %f\n", (((CPU_CLK/4096) / frequency) -1));
		//write logic 1 to bit 7 of MODE1, restart all PWM channels
		PCA9685_SetRegister(MODE1, (uint8_t)0x80);
		
		lookup_initialize(counter_lookup);
}

// Priority 
int SetProgramPriority(int priorityLevel)
{
    struct sched_param sched;

    memset (&sched, 0, sizeof(sched));

    if (priorityLevel > sched_get_priority_max (SCHED_RR))
        priorityLevel = sched_get_priority_max (SCHED_RR);

    sched.sched_priority = priorityLevel;

    return sched_setscheduler (0, SCHED_RR, &sched);
}