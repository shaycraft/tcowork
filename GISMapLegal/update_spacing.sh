#!/bin/bash

export TMP=./tmp

if [ -d tmp ]; then
	rm -Rf $TMP/*
else
	mkdir $TMP
fi

wget --directory=$TMP http://cogcc.state.co.us/COGIS_Help/CauseOrderTable_Download.zip
unzip -d $TMP $TMP/CauseOrderTable_Download.zip
mdb-export $TMP/COGCC_OrdersTable_April_28_2014.mdb COGCC_Spacing_Download | sed -e 's/\s\+//g' > $TMP/shit.csv
cat $TMP/shit.csv
