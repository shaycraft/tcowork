def explodetoarray(str_legal):
    legalout = {}
    legallist = str_legal.split(',')
    print legallist
    for i in range(0, len(legallist)):
        print i


def explode_single_array(s):
    land_grid = {}  # this should be a 4x4 multi-array
    divs = []  # can use lists as stacks, see https://docs.python.org/2/tutorial/datastructures.html
    quarters = {}
    sb_legal_out = {}

    idx = 0
    for i in s:
        print i
        if i == '/':
            divs.append(s[idx+1])
            print s[idx+1]
        idx = idx + 1

    print divs


explode_single_array('NW/4')
