#ifndef BCM2835
#define BCM2835

#define BCM2708_PERI_BASE      	0x3F000000
#define GPIO_BASE               (BCM2708_PERI_BASE + 0x200000)	// GPIO controller 
#define BSC0_BASE 				(BCM2708_PERI_BASE + 0x804000)  // I2C controller 
#define BLOCK_SIZE 				(4*1024)
 
typedef struct peripheralBCM2835 {
    unsigned long addr_p;
    int mem_fd;
    void *map;
    volatile unsigned int *addr;
}peripheralBCM2835;
  
extern peripheralBCM2835 gpio;
extern peripheralBCM2835 bsc0;	

// GPIO Registers. 
// Always use INP_GPIO(x) before using OUT_GPIO(x)
#define INP_GPIO(g)   		*(gpio.addr + ((g)/10)) &= ~(7<<(((g)%10)*3))
#define OUT_GPIO(g)   		*(gpio.addr + ((g)/10)) |=  (1<<(((g)%10)*3))
#define SET_GPIO_ALT(g,a) 	*(gpio.addr + (((g)/10))) |= (((a)<=3?(a) + 4:(a)==4?3:2)<<(((g)%10)*3))
 
#define GPIO_SET  			*(gpio.addr + 7) 
#define GPIO_CLR  			*(gpio.addr + 10)
 
#define GPIO_READ(g)  		*(gpio.addr + 13) &= (1<<(g))

// Broadcom Serial Controller Registers
#define BSC0_C        			*(bsc0.addr + 0x00)
#define BSC0_S        			*(bsc0.addr + 0x01)
#define BSC0_DLEN    			*(bsc0.addr + 0x02)
#define BSC0_A        			*(bsc0.addr + 0x03)
#define BSC0_FIFO    			*(bsc0.addr + 0x04)

// Broadcom Serial Controller Bits
#define BSC_C_I2CEN    			(1 << 15)
#define BSC_C_INTR    			(1 << 10)
#define BSC_C_INTT    			(1 << 9)
#define BSC_C_INTD    			(1 << 8)
#define BSC_C_ST    			(1 << 7)
#define BSC_C_CLEAR    			(1 << 4)
#define BSC_C_READ    			1
#define BSC_S_CLKT				(1 << 9)
#define BSC_S_ERR    			(1 << 8)
#define BSC_S_RXF    			(1 << 7)
#define BSC_S_TXE    			(1 << 6)
#define BSC_S_RXD    			(1 << 5)
#define BSC_S_TXD    			(1 << 4)
#define BSC_S_RXR    			(1 << 3)
#define BSC_S_TXW    			(1 << 2)
#define BSC_S_DONE   			(1 << 1)
#define BSC_S_TA    			1

// Broadcom Serial Controller Signal Flags
#define BSC_CLEAR_STATUS_FLAG  	BSC_S_CLKT|BSC_S_ERR|BSC_S_DONE
#define BSC_READ_FLAG    		BSC_C_I2CEN|BSC_C_ST|BSC_C_CLEAR|BSC_C_READ
#define BSC_WRITE_FLAG   		BSC_C_I2CEN|BSC_C_ST

void map_peripheral_BCM2835(struct peripheralBCM2835 *p);
void unmap_peripheral_BCM2835(struct peripheralBCM2835 *p);
void init_I2C_protocol();

#endif
