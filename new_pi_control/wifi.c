#include "wifi.h"

 void get_connection(struct sockaddr_in *server_addr, int *sock_fd){

	*sock_fd = socket(AF_INET, SOCK_STREAM, 0);

	if(*sock_fd < 0){
		perror("Socket Building Error: ");
		exit(1);
	}

	bzero((char *)server_addr, sizeof(*server_addr));
	// bzero((char *)&client_addr, sizeof(client_addr));

	server_addr->sin_family = AF_INET;
	server_addr->sin_addr.s_addr = INADDR_ANY;
	server_addr->sin_port = htons(port_num);

	char ip_addr[INET_ADDRSTRLEN];
	inet_ntop(AF_INET, &server_addr[0], ip_addr, INET_ADDRSTRLEN);
	//printf("Local Server IP Address is: <%s>\n", ip_addr);

	int bind_fd = bind(*sock_fd, (struct sockaddr*)server_addr, sizeof(*server_addr));

	if(bind_fd < 0){

		perror("Binding Error: ");
		exit(1);
	}

 }
