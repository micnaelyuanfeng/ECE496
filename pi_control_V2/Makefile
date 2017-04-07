CC = gcc
LD = gcc

OBJS = Adafruit_MotorHAT.o Adafruit_ServoHAT.o wifi.o server_pi.o BCM2835.o PCA9685.o

CFLAGS = -Wall
LDFLAGS =

PROG = server



$(PROG):$(OBJS)
	$(LD) $(LDFLAGS) $(OBJS) -pthread -lm -o $(PROG)

%.o:%.c
	$(CC) $(CFLAGS) -c $<
