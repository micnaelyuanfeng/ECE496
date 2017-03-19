 #include "peri.h"
 #include <sys/ioctl.h>
 #include <linux/i2c-dev.h>
 
 int map_gpio (struct bcm2835_peripheral *p){
	if((p->mem_fd = open ("/dev/mem", O_RDWR|O_SYNC)) < 0 ){
		printf ("Failed to open /dev/mem");
		return -1;
	}
	
	p->map = (void *) mmap(NULL, BLOCK_SIZE, PROT_READ|PROT_WRITE, MAP_SHARED, p->mem_fd, p->addr_p);
	
	if(p->map == MAP_FAILED){
		perror("mmap");
		return -1;
	}
	
	p->addr = (volatile unsigned int *)p->map;
	
	return 0;
 }
 
 void unmap_gpio (struct bcm2835_peripheral *p){
	munmap(p->map, BLOCK_SIZE);
	close(p->mem_fd);
 }
 
void gpio_in (struct bcm2835_peripheral *p, int pin_num){
	volatile unsigned int * pin_address = p->addr + (pin_num/10);
	*pin_address &= ~(7 << (pin_num%10)*3);
	// return 0;
 }
 
 void gpio_out (struct bcm2835_peripheral *p, int pin_num){
	volatile unsigned int *pin_address = p->addr + (pin_num/10);
	*pin_address |= (1 << (pin_num%10)*3);
	// return 0;
 }
 
 void gpio_set (struct bcm2835_peripheral *p, int pin_num){
	volatile unsigned int *pin_set = p->addr + 7;
	*pin_set = (1<<pin_num); 
	// return 0;
 }
 
 void gpio_clear(struct bcm2835_peripheral *p, int pin_num){
	volatile unsigned int *pin_clear = p->addr + 10;
	*pin_clear = (1<<pin_num);
	// return 0;
 }
 
 int gpio_read(struct bcm2835_peripheral *p, int pin_num){
	volatile unsigned int *pin_status_reg = p-> addr + 13;
	return (*pin_status_reg & (1<<pin_num));
 }
 
 // void pin_func_sel(struct bcm2835_peripheral *p, int pin_num, int func_num){
	// volatile unsigned int *pin_address = p->addr + (pin_num/10);
	// //6 alternative function to choose
	// *pin_address |= (func_num <= 3 ? func_num + 4:func_num ==4?3:2)<<((pin_num%10) * 3);
 // }