__author__ = 'shaycraft'
import csv
from sys import argv
import sys
import re
from LegalExploder import *
from Lndkey import *

if len(argv) != 2:
    sys.stderr.write('Usage: {0} <file>\n'.format(argv[0]))
    sys.exit(1)

#sample line
# 8744,"18","8N","68W","6","E/2,SE/4SW/4","84","5","",2581,"JSND","http://cogcc.state.co.us/orders/orders/84/5.html"

with open(argv[1]) as csvfile:
    reader = csv.reader(csvfile, delimiter=',', quotechar='"')
    # skip header
    next(reader, None)

    for row in reader:
        section_id = row[0]
        legal = row[5]
        unit_size = row[8]
        formation = row[10]
        link = row[11]

        # lndkey
        section = int(row[1])
        townregex = re.match('(\d+)([S|N|E|W])', row[2])
        township = int(townregex.group(1))
        tdir = townregex.group(2)
        rangeregex = re.match('(\d+)([S|N|E|W])', row[3])
        rnge = int(rangeregex.group(1))
        rdir = rangeregex.group(2)
        meridian = int(row[4])

        legal_exploded = explode(row[5])
        for x in legal_exploded:
            lkey = generate_lndkey(township, tdir, rnge, rdir, meridian, section, x, 'CO')
            print '{0},{1},{2},{3},"{4}",{5}'.format(section_id, legal, unit_size, formation, link, x)
