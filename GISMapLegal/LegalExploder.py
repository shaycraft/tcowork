quarter_names = [[]]


def explode_to_array(str_legal):
    legalout = []
    legal_list = str_legal.split(',')
    for i in range(0, len(legal_list)):
        legalout.append(explode_single_array(legal_list[i]))

def set_grid(grid, org_x, org_y, len_x, len_y):
    for i in range(org_x, org_x + len_x):
        for j in range(org_y, org_y + len_y):
            grid[i][j] = True


def grid_calc(grid, org_x, org_y, len_x, len_y, divs, quarters):
    # STRATEGY:  examine the top elements of the stacks divs and quarters
    # call recursively until empty
    # Based upon these values, call recursively, but shift the length and origin accordingly

    # do we have work to do?  If so process, if not we're done
    if len(divs) != 0 and len(quarters) != 0:
        norg_x = -1
        norg_y = -1
        nlen_x = -1
        nlen_y = -1
        cur_quarter = quarters.pop()
        cur_div = divs.pop()

        # set current subGrid according to values
        if cur_div == '2':
            if cur_quarter == 'N':
                norg_x = org_x
                norg_y = org_y + len_y / 2
                nlen_x = len_x
                nlen_y = len_y / 2
            elif cur_quarter == 'S':
                norg_x = org_x
                norg_y = org_y
                nlen_x = len_x
                nlen_y = len_y / 2
            elif cur_quarter == 'W':
                norg_x = org_x
                norg_y = org_y
                nlen_y = len_y
                nlen_x = len_x / 2
            elif cur_quarter == 'E':
                norg_x = org_x + len_x / 2
                norg_y = org_y
                nlen_x = len_x / 2
                nlen_y = len_y
        elif cur_div == '4':
            if cur_quarter == 'NW':
                norg_x = org_x
                norg_y = org_y + len_y / 2
                nlen_x = len_x / 2
                nlen_y = len_y / 2
            elif cur_quarter == 'SW':
                norg_x = org_x
                norg_y = org_y
                nlen_x = len_x / 2
                nlen_y = len_y / 2
            elif cur_quarter == 'NE':
                norg_x = org_x + len_x / 2
                norg_y = org_y + len_y / 2
                nlen_x = len_x / 2
                nlen_y = len_y / 2
            elif cur_quarter == 'SE':
                norg_x = org_x + len_x / 2
                norg_y = org_y
                nlen_x = len_x / 2
                nlen_y = len_y / 2
        grid_calc(grid, norg_x, norg_y, nlen_x, nlen_y, divs, quarters)
    else:
        #print ' org_x = {0}, org_y = {1}, len_x = {2}, len_y = {3}'.format(org_x,org_y, len_x, len_y)
        set_grid(grid, org_x, org_y, len_x, len_y)


def explode_single_array(s):
    land_grid = [[]]  # this should be a 4x4 multi-array
    land_grid = [[0 for x in xrange(4)] for x in xrange(4)]
    divs = []  # can use lists as stacks, see https://docs.python.org/2/tutorial/datastructures.html
    quarters = []

    idx = 0
    for i in s:
        if i == '/':
            divs.append(s[idx+1])

            # half or quarter?
            if divs[-1] == '2':
                quarters.append(s[idx-1: idx])
            elif divs[-1] == '4':
                quarters.append(s[idx-2: idx])
        idx += 1

    grid_calc(land_grid, 0, 0, 4, 4, divs, quarters)
    print land_grid
    #quarter_names = [['' for x in xrange(4)] for x in xrange(4)]
    #print quarter_names
    #print land_grid

    initialize_quarter_names(0, 0, 4, 4, [])
    print quarter_names


def initialize_quarter_names(org_x, org_y, len_x, len_y, quarters):
    if len_x == 1 and len_y == 1:
        sb = ''
        while len(quarters) > 0:
            sb = sb + quarters.pop()
        quarter_names[org_x][org_y] = sb
    else:
        nw = list(quarters)  # the colon is needed to slice the list, as otherwise it would be an assignment by reference
        ne = list(quarters)
        sw = list(quarters)
        se = list(quarters)

        nw.append('NW')
        ne.append('NE')
        sw.append('SW')
        se.append('SE')

        len_x /= 2
        len_y /= 2
        
        initialize_quarter_names(org_x, org_y + len_y, len_x, len_y, nw)
        initialize_quarter_names(org_x + len_x, org_y + len_y, len_x, len_y, ne)
        initialize_quarter_names(org_x, org_y, len_x, len_y, sw)
        initialize_quarter_names(org_x + len_x, org_y, len_x, len_y, se)


quarter_names = [['' for z in xrange(4)] for z in xrange(4)]
explode_single_array('NW/4')