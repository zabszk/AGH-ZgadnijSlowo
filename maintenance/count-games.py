import glob
import shutil
g = glob.glob("/path/to/webroot/GameLogs/*.log")
players = {}
for f in g:
    with open(f) as ff:
        l = "."
        while l:
            l = ff.readline()
            if l.__contains__("has been accepted into the game"):
                username = l.split()[2]
                if username in players:
                    players[username] += 1
                else:
                    players[username] = 1

print(players)
