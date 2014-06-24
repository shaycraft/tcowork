__author__ = 'shaycraft'

def generate_lndkey(township, tdir, rnge, rdir, meridian, section, qq, state):
    tmp_lndkey = []
    tmp_lndkey.append(state)
    tmp_lndkey.append('{0:02d}'.format(meridian))
    tmp_lndkey.append('{0:03d}0{1}'.format(township, tdir))
    tmp_lndkey.append('{0:03d}0{1}'.format(rnge, rdir))
    tmp_lndkey.append('0SN{0:02d}0'.format(section))

    qq = qq.replace('/', '').replace('2', '').replace('4', '')
    if any(qq in x for x in ['L1', 'L2', 'L3', 'L4', 'L5', 'L6']):
        tmp_lndkey.append(qq)
    else:
        tmp_lndkey.append('A{0}'.format(qq))

    return ''.join(tmp_lndkey)