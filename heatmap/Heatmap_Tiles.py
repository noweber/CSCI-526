import numpy as np
import matplotlib.pyplot as plt
import gspread
import json
from oauth2client.service_account import ServiceAccountCredentials


heatmap_fow = np.zeros((10,5), dtype=int)
heatmap_level1 = np.zeros((10,5), dtype=int)
heatmap_level2 = np.zeros((10,8), dtype=int)

list_heatmap_fow = []
list_heatmap_level1 = []
list_heatmap_level2 = []



def add_to_heatmap(hashmap,k):
    global heatmap_fow
    global heatmap_level1
    global heatmap_level2
    for i in hashmap:
        n = i.strip('()').split(',')
        x = int(n[0])
        y = int(n[1])
        if k == 1:
            heatmap_fow[10-y-1,x] += int(hashmap[i])
        elif k==2:
            heatmap_level1[10-y-1,x] += int(hashmap[i])
        elif k==3:
            heatmap_level2[10-y-1,x] += int(hashmap[i])


def processthematrix(listofmaps,k):
    for item in listofmaps:
        hashmap = json.loads(item)
        add_to_heatmap(hashmap,k)

    return 


def main():

    ##### AUTH CODE START ##  DO NOT TOUCH ####
    scope = ['https://spreadsheets.google.com/feeds',
         'https://www.googleapis.com/auth/spreadsheets',
         'https://www.googleapis.com/auth/drive.file',
         'https://www.googleapis.com/auth/drive']
   
    credentials = ServiceAccountCredentials.from_json_keyfile_name('gamedev_key.json', scope)            
    gc = gspread.authorize(credentials)
    EOLgsheet = gc.open('Week 8 - End of Level Analytics Form (Responses)').worksheet('Form Responses 1')
    ##### AUTH CODE END ##  DO NOT TOUCH ####

    data = EOLgsheet.get_all_values()
    for row in data[1:]:
        if row[3] == 'TutorialFogOfWar' and row[7] != '{}':
            list_heatmap_fow.append(row[7])
        elif row[3] == 'Level_One' and row[7] != '{}':
            list_heatmap_level1.append(row[7])
        elif row[3] == 'Level_Two' and row[7] != '{}':
            list_heatmap_level2.append(row[7])

    processthematrix(list_heatmap_fow,1)
    processthematrix(list_heatmap_level1,2)
    processthematrix(list_heatmap_level2,3)

    print(heatmap_fow)
    print(heatmap_level1)
    print(heatmap_level2)

    xlabel = ["0", "1" , "2" , "3" , "4", "5", "6", "7", "8","9", "10"]
    ylabel = [ "9" , "8" , "7" , "6", "5", "4", "3", "2", "1", "0"]
    
    #FogofWar Tutorial heatmap specifics
    fig, ax = plt.subplots()
    im = ax.imshow(heatmap_fow, cmap='Pastel2_r')
    ax.set_yticks(np.arange(10), labels = ylabel )
    ax.set_xticks(np.arange(5), labels = xlabel[0:5])
    ax.set_title("FogofWar(Tutorial) HeatMap")
    ax.spines[:].set_visible(False)
    ax.set_xticks(np.arange(6)-0.5, minor = True)
    ax.set_yticks(np.arange(11)-0.5, minor = True)
    ax.grid(which="minor", color="w", linestyle='-', linewidth=3)
    ax.tick_params(which="minor", bottom=False, left=False)
    fig.tight_layout()
    cbar = ax.figure.colorbar(im, ax=ax)
    #cbar.ax.set_ylabel(cbarlabel, rotation=-90, va="bottom")
    plt.show()

    #Level_1 heatmap specifics
    fig, ax = plt.subplots()
    im = ax.imshow(heatmap_level1, cmap='Pastel2_r')
    ax.set_yticks(np.arange(10), labels = ylabel )
    ax.set_xticks(np.arange(5), labels = xlabel[0:5])
    ax.set_title("LevelOne HeatMap")
    ax.spines[:].set_visible(False)
    ax.set_xticks(np.arange(6)-0.5, minor = True)
    ax.set_yticks(np.arange(11)-0.5, minor = True)
    ax.grid(which="minor", color="w", linestyle='-', linewidth=3)
    ax.tick_params(which="minor", bottom=False, left=False)
    fig.tight_layout()
    cbar = ax.figure.colorbar(im, ax=ax)
    #cbar.ax.set_ylabel(cbarlabel, rotation=-90, va="bottom")
    plt.show()

    #Level_2 heatmap specifics
    fig, ax = plt.subplots()
    im = ax.imshow(heatmap_level2, cmap='Pastel2_r')
    ax.set_yticks(np.arange(10), labels = ylabel )
    ax.set_xticks(np.arange(8), labels = xlabel[0:8])
    ax.set_title("LevelTwo HeatMap")
    ax.spines[:].set_visible(False)
    ax.set_xticks(np.arange(9)-0.5, minor = True)
    ax.set_yticks(np.arange(11)-0.5, minor = True)
    ax.grid(which="minor", color="w", linestyle='-', linewidth=3)
    ax.tick_params(which="minor", bottom=False, left=False)
    fig.tight_layout()
    cbar = ax.figure.colorbar(im, ax=ax)
    #cbar.ax.set_ylabel(cbarlabel, rotation=-90, va="bottom")
    plt.show()




if __name__ == "__main__":
    main()
