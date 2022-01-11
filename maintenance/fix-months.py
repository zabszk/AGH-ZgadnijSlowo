import glob
import shutil
g = glob.glob("/path/to/webroot/GameLogs/*.log")
for f in g:
    month = ''
    with open(f) as ff:
        month = ff.readline()[22:24]
    sp = f.split("-")
    sp[3] = month
    sp = '-'.join(sp)
    shutil.move(f, sp)
