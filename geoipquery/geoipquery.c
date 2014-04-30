#include<stdio.h>
#include<stdlib.h>
#include<GeoIP.h>

int main(int argc, char **argv)
{
	char *line;
	GeoIP *gi;
	const char *returned_country;

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
		gi = GeoIP_open("/usr/local/share/GeoIP/GeoIP.dat", GEOIP_STANDARD | GEOIP_CHECK_CACHE);
		returned_country = GeoIP_country_code_by_addr(gi, line);	
		printf("ip = %s, country = %s\n", line, returned_country);
	}

	fclose(fp);

	return 0;
}
