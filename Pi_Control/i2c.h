 #include "peri.h"

#define PRE_SCALE 0xFE
#define MODE1 0x00
#define MODE2 0x01

#define CPU_CLK 25000000.0
#define RESOLUTION 4096

double counter_lookup[37];

int map_peripheral(struct bcm2835_peripheral *p);
void unmap_peripheral(struct bcm2835_peripheral *p);
void dump_bsc_status(void);
void wait_i2c_done(void);
void PCA9685_SetRegister(uint8_t reg_address, uint8_t reg_value );
void i2c_init(void);
uint16_t map_to_DutyCycle(double * head, uint8_t degree);
void PCA9685_SetOutput(uint8_t channel, uint8_t degree);
void lookup_initialize(double *head);
void PCA9685_init(unsigned short frequency);
int SetProgramPriority(int priorityLevel);