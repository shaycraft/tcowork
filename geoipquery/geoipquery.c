#include<stdio.h>
#include<stdlib.h>
#include<GeoIP.h>

int main(int argc, char **argv)
{
	char *line;

	line = malloc(2000*sizeof(char));	

	if (argc != 2)
	{
		printf("Usage: %s <filename>\n", argv[0]);
		exit(0);
	}

	printf ("Reading file...\n");
	
	FILE *fp = fopen(argv[1], "r");

	if (fp == NULL)
	{
		fprintf(stderr, "Something fucked up\n");
		exit(1);
	}

	while (fscanf(fp, "%s", line) != EOF)
	{
		
	}

	fclose(fp);

	return 0;
}
