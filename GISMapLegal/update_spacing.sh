#!/bin/bash

read -s -p "Enter db password for $USER: " pgpass
export PGPASSWORD=$pgpass

cd /home/shaycraft/src/github-shaycraft/tcowork/GISMapLegal

export TMP=./tmp
export OUT_FILE=./tmp/COGCC_spacing.csv

if [ -d tmp ]; then
	rm -Rf $TMP/*
else
	mkdir $TMP
fi

wget --directory=$TMP http://cogcc.state.co.us/COGIS_Help/CauseOrderTable_Download.zip
unzip -d $TMP $TMP/CauseOrderTable_Download.zip
mdb-export $TMP/COGCC_OrdersTable_April_28_2014.mdb COGCC_Spacing_Download | sed -e 's/\s\+//g' > $OUT_FILE 

sed -i -e 's/NW/NW\/4/g' $OUT_FILE
sed -i -e 's/SW/SW\/4/g' $OUT_FILE
sed -i -e 's/NE/NE\/4/g' $OUT_FILE
sed -i -e 's/SE/SE\/4/g' $OUT_FILE
sed -i -e 's/N2/N\/2/g' $OUT_FILE
sed -i -e 's/S2/S\/2/g' $OUT_FILE
sed -i -e 's/W2/W\/2/g' $OUT_FILE
sed -i -e 's/E2/E\/2/g' $OUT_FILE

python ./MapSpacing.py $OUT_FILE > $TMP/spacing_exploded.csv

psql -d cogcc -c "DELETE FROM staging.spacing"
psql -d cogcc -c "\copy staging.spacing(section_id,legal,unit_size,formation,link,lkey) FROM '$TMP/spacing_exploded.csv' WITH DELIMITER AS ',' CSV HEADER;"
psql -d cogcc -c "vacuum analyze staging.spacing"

pgsql2shp -f $TMP/co_spacing.shp -P samcool -g geom cogcc "`cat joinquery.sql`"
zip $TMP/co_spacing.zip $TMP/co_spacing.*

echo "Please see attached file" | mail -s "COGCC spacing file" -a $TMP/co_spacing.zip "`whoami`@tcolandservices.com"
