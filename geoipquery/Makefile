geoipquery: geoipquery.o
	gcc -Wall geoipquery.o -o geoipquery -lGeoIP
geoipquery.o: geoipquery.c
	gcc -Wall -c geoipquery.c -lGeoIP

clean:
	rm -f *.o geoipquery
