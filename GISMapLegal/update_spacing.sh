#!/bin/bash

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
