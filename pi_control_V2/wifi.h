 #include <stdio.h>
 #include <sys/socket.h>
 #include <sys/types.h>
 #include <string.h>
 #include <stdlib.h>
 #include <stdbool.h>
 #include <unistd.h>
 #include <netinet/in.h>
 #include <netdb.h>
 #include <arpa/inet.h>
 #include <ifaddrs.h>
 
 #define port_num 2050
  
  int sockfd;
  socklen_t clienlen;
  int newconnection;

  struct sockaddr_in server_addr;
  struct sockaddr_in client_addr;

  void get_connection(struct sockaddr_in *server_addr, int *sock_fd);