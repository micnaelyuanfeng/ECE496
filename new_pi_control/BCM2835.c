#include "Standard_Library.h"
#include "System_Library.h"
#include "BCM2835.h"


/* 	This is the driver for BCM2835 Chip 
	The orginal code is implemented by Pieter-Jan Van de Maele and posted on http://www.pieter-jan.com
	BCM2835 is responsible for peripheral management and communication (GPIO and I2C)
	GPIO protocol is implemented here. I2C protocol is initialized here but implemented by PCA9685 chip driver
*/

peripheralBCM2835 gpio = {GPIO_BASE};
peripheralBCM2835 bsc0 = {BSC0_BASE};


void map_peripheral_BCM2835(struct peripheralBCM2835 *p){
	// Open /dev/mem
	if ((p->mem_fd = open("/dev/mem", O_RDWR|O_SYNC) ) < 0) {
		printf("Error occured at map_peripheral_BCM2835.\nFailed to open /dev/mem, try checking permissions.Exit...\n");
		exit(1);
	}
 
	p->map = mmap(
		NULL,
		BLOCK_SIZE,
		PROT_READ|PROT_WRITE,
		MAP_SHARED,
		p->mem_fd,      // File descriptor to physical memory virtual file '/dev/mem'
		p->addr_p       // Address in physical map that we want this memory block to expose
	);
 
	if (p->map == MAP_FAILED) {
		printf("Error occured at map_peripheral_BCM2835.\n mmap failed.Exit...\n");
        perror("mmap");
		exit(1);
	}
	p->addr = (volatile unsigned int *)p->map;
}
 
void unmap_peripheral_BCM2835(struct peripheralBCM2835 *p){
    munmap(p->map, BLOCK_SIZE);
    close(p->mem_fd);
}

void init_I2C_protocol(){
    INP_GPIO(0);
    SET_GPIO_ALT(0, 0);
    INP_GPIO(1);
    SET_GPIO_ALT(1, 0);
}  