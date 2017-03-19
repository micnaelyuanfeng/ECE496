#ifndef _INC_PJ_GPIO_H
#define _INC_PJ_GPIO_H

#include <stdio.h>

#include <string.h>
#include <stdlib.h>
#include <stdint.h>
#include <dirent.h>
#include <fcntl.h>
#include <assert.h>

#include <sched.h>		// To set the priority on linux

#include <sys/mman.h>
#include <sys/types.h>
#include <sys/stat.h>

#include <unistd.h>

// Define which Raspberry Pi board are you using. Take care to have defined only one at time.
#define RPI
//#define RPI2

#ifdef RPI
#define BCM2708_PERI_BASE       0x20000000
#define GPIO_BASE               (BCM2708_PERI_BASE + 0x200000)	// GPIO controller 
//#define BSC0_BASE 		(BCM2708_PERI_BASE + 0x205000)	// I2C controller	
#define BSC0_BASE 		(BCM2708_PERI_BASE + 0x804000)	// I2C controller	
#endif

#ifdef RPI2
#define BCM2708_PERI_BASE       0x3F000000
#define GPIO_BASE               (BCM2708_PERI_BASE + 0x200000)	// GPIO controller. Maybe wrong. Need to be tested.
#define BSC0_BASE 		(BCM2708_PERI_BASE + 0x804000)	// I2C controller	
#endif	

#define PAGE_SIZE 		(4*1024)
#define BLOCK_SIZE 		(4*1024)

// GPIO setup macros. Always use INP_GPIO(x) before using OUT_GPIO(x) or SET_GPIO_ALT(x,y)
#define INP_GPIO(g) 	*(gpio.addr + ((g)/10)) &= ~(7<<(((g)%10)*3))
#define OUT_GPIO(g) 	*(gpio.addr + ((g)/10)) |=  (1<<(((g)%10)*3))
#define SET_GPIO_ALT(g,a) *(gpio.addr + (((g)/10))) |= (((a)<=3?(a) + 4:(a)==4?3:2)<<(((g)%10)*3))

#define GPIO_SET 	*(gpio.addr + 7)  // sets   bits which are 1 ignores bits which are 0
#define GPIO_CLR 	*(gpio.addr + 10) // clears bits which are 1 ignores bits which are 0

#define GPIO_READ(g) 	*(gpio.addr + 13) &= (1<<(g))

// I2C macros
#define BSC0_C        	*(bsc0.addr + 0x00)
#define BSC0_S        	*(bsc0.addr + 0x01)
#define BSC0_DLEN    	*(bsc0.addr + 0x02)
#define BSC0_A        	*(bsc0.addr + 0x03)
#define BSC0_FIFO    	*(bsc0.addr + 0x04)

#define BSC_C_I2CEN    	(1 << 15)
#define BSC_C_INTR    	(1 << 10)
#define BSC_C_INTT    	(1 << 9)
#define BSC_C_INTD    	(1 << 8)
#define BSC_C_ST    	(1 << 7)
#define BSC_C_CLEAR    	(1 << 4)
#define BSC_C_READ    	1

#define START_READ    	BSC_C_I2CEN|BSC_C_ST|BSC_C_CLEAR|BSC_C_READ
#define START_WRITE   	BSC_C_I2CEN|BSC_C_ST

#define BSC_S_CLKT	(1 << 9)
#define BSC_S_ERR    	(1 << 8)
#define BSC_S_RXF    	(1 << 7)
#define BSC_S_TXE    	(1 << 6)
#define BSC_S_RXD    	(1 << 5)
#define BSC_S_TXD    	(1 << 4)
#define BSC_S_RXR    	(1 << 3)
#define BSC_S_TXW    	(1 << 2)
#define BSC_S_DONE   	(1 << 1)
#define BSC_S_TA    	1

#define CLEAR_STATUS    BSC_S_CLKT|BSC_S_ERR|BSC_S_DONE

//PWM output address table
// LED0
#define LED0_ON_L		0x06
#define LED0_ON_H		0x07
#define LED0_OFF_L	0x08
#define LED0_OFF_H	0x09
// LED1
#define LED1_ON_L		0x0A
#define LED1_ON_H		0x0B
#define LED1_OFF_L	0x0C
#define LED1_OFF_H	0x0D
// LED2
#define LED2_ON_L		0x0E
#define LED2_ON_H		0x0F
#define LED2_OFF_L	0x10
#define LED2_OFF_H	0x11
// LED3
#define LED3_ON_L		0x12
#define LED3_ON_H		0x13
#define LED3_OFF_L	0x14
#define LED3_OFF_H	0x15
// LED4
#define LED4_ON_L		0x16
#define LED4_ON_H		0x17
#define LED4_OFF_L	0x18
#define LED4_OFF_H	0x19
// LED5
#define LED5_ON_L		0x1A
#define LED5_ON_H     0x1B
#define LED5_OFF_L	0x1C
#define LED5_OFF_H	0x1D
// LED6
#define LED6_ON_L		0x1E
#define LED6_ON_H		0x1F
#define LED6_OFF_L	0x20
#define LED6_OFF_H	0x21
// LED7
#define LED7_ON_L		0x22
#define LED7_ON_H		0x23
#define LED7_OFF_L		0x24
#define LED7_OFF_H		0x25
// LED8
#define LED8_ON_L 		0x26
#define LED8_ON_H		0x27
#define LED8_OFF_L	0x28
#define LED8_OFF_H	0x29
// LED9
#define LED9_ON_L		0x2A
#define LED9_ON_H		0x2B
#define LED9_OFF_L	0x2C
#define LED9_OFF_H	0x2D
// LED10
#define LED10_ON_L		0x2E
#define LED10_ON_H		0x2F
#define LED10_OFF_L	0x30
#define LED10_OFF_H	0x31
// LED11
#define LED11_ON_L		0x32
#define LED11_ON_H		0x33
#define LED11_OFF_L	0x34
#define LED11_OFF_H	0x35
// LED12
#define LED12_ON_L		0x36
#define LED12_ON_H		0x37
#define LED12_OFF_L	0x38
#define LED12_OFF_H	0x39
// LED13
#define LED13_ON_L		0x3A
#define LED13_ON_H		0x3B
#define LED13_OFF_L	0x3C
#define LED13_OFF_H	0x3D
// LED14
#define LED14_ON_L		0x3E
#define LED14_ON_H		0x3F
#define LED14_OFF_L	0x40
#define LED14_OFF_H	0x41
// LED15
#define LED15_ON_L		0x42
#define LED15_ON_H		0x43
#define LED15_OFF_L	0x44
#define LED15_OFF_H	0x45

// IO Acces
struct bcm2835_peripheral {
    unsigned long addr_p;
    int mem_fd;
    void *map;
    volatile unsigned int *addr;
};

extern struct bcm2835_peripheral gpio; 	// They have to be found somewhere, but can't be in the header
extern struct bcm2835_peripheral bsc0;	// so use extern!!


// Function prototypes
int map_peripheral(struct bcm2835_peripheral *p);
void unmap_peripheral(struct bcm2835_peripheral *p);

// I2C
void dump_bsc_status();
void wait_i2c_done();
void i2c_init();
// Priority
int SetProgramPriority(int priorityLevel);

#endif

/*
Raspberry Pi Pin# :       Signal:                Default Function
-----------------------------------------------------------------
P1-1                      3.3V                   3.3V DC Power
P1-2                      5V                     5V DC Power
P1-3                      GPIO00                 I2C0_SDA
P1-4                      --                     No Connection
P1-5                      GPIO01                 I2C0_SCL
P1-6                      0V                     0V
P1-7                      GPIO04                 D7
P1-8                      GPIO14                 UART TX
P1-9                      --                     No Connection
P1-10                     GPIO15                 UART RX
P1-11                     GPIO17                 D0
P1-12                     GPIO18                 D1
P1-13                     GPIO21                 D2
P1-14                     --                     No Connection
P1-15                     GPIO22                 D3
P1-16                     GPIO23                 D4
P1-17                     --                     No Connection
P1-18                     GPIO24                 D5
P1-19                     GPIO10                 SPI MOSI
P1-20                     --                     No Connection
P1-21                     GPIO09                 SPI MISO
P1-22                     GPIO25                 D6
P1-23                     GPIO11                 SPI SCLK
P1-24                     GPIO08                 SPI CE0 N
P1-25                     --                     No Connection
P1-26                     GPIO07                 SPI CE1 N
*/


