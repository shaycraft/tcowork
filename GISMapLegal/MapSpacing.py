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

#print header
print 'section_id, legal, unit_size, formation, link, lkey'

count = 1
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
        try:
            #print 'DEBUG, line = ' + str(count)
            #print ','.join(row)
            section = int(row[1])
            townregex = re.match('(\d+)([S|N|E|W])', row[2])
            township = int(townregex.group(1))
            tdir = townregex.group(2)
            rangeregex = re.match('(\d+)([S|N|E|W])', row[3])
            rnge = int(rangeregex.group(1))
            rdir = rangeregex.group(2)
            #meridian = int(row[4])
        except ValueError:
            sys.stderr.write("ValueError on line {0}\n".format(count))
            sys.stderr.write(row)
        except AttributeError:
            sys.stderr.write("NoneError on line {0}\n".format(count))

        tmp_legal = row[5]
        if tmp_legal == 'ALL':
            if 1 <= section < 6:
                tmp_legal = 'L1,L2,L3,L4,S/2N/2,S/2'
            elif section == 6:
                tmp_legal = 'L1,L2,L3,L4,L5,L6,L7,SE/4NW/4,E/2SW/4,S/2NE/4,SE/4'
            elif section in [7, 18, 19, 30]:
                tmp_legal = 'L1,L2,L3,L4,E/2W/2,E/2'
            elif section == 31:
                tmp_legal = 'L1,L2,L3,L4,L5,L6,L7,NE/4SW/4,E/2NW/4,NE/4,N/2SE/4'
            elif 32 <= section <= 36:
                tmp_legal = 'L1,L2,L3,L4,N/2S/2,N/2'

        legal_exploded = explode(tmp_legal)

        for x in legal_exploded:
            lkey = generate_lndkey(township, tdir, rnge, rdir, 6, section, x, 'CO')
            print '{0},"{1}","{2}","{3}","{4}",{5}'.format(section_id, legal, unit_size, formation, link, lkey)

        count += 1

    print generate_lndkey(10, 'N', 5, 'W', 6, 5, 'L2', 'CO')