import numpy as np
import matplotlib.pyplot as plt
import matplotlib.colors as colors
import gspread
import json
from oauth2client.service_account import ServiceAccountCredentials


def main():


    ##### AUTH CODE START ##  DO NOT TOUCH ####
    scope = ['https://spreadsheets.google.com/feeds',
         'https://www.googleapis.com/auth/spreadsheets',
         'https://www.googleapis.com/auth/drive.file',
         'https://www.googleapis.com/auth/drive']
   
    credentials = ServiceAccountCredentials.from_json_keyfile_name('gamedev_key.json', scope)            
    gc = gspread.authorize(credentials)
    EOLgsheet = gc.open('Midterm End Analytics').worksheet('Form Responses 1')
    ##### AUTH CODE END ##  DO NOT TOUCH ####

    data = EOLgsheet.get_all_values()
    player_movement = {}

    for row_data in data[1:]:
        if(row_data[3]== 'Level_One'):
            hashmap = json.loads(row_data[13])
            if(hashmap['VictoriousPlayerName'] == 'AI' or hashmap['VictoriousPlayerName'] == 'Human' ):    #hashmap['VictoriousPlayerName'] == 'Human'
                move_array = hashmap['MovesMade']
                for move in move_array:
                    if(move['Player'] == "Human" ):
                        x = move['Destination']['Item1']
                        y = move['Destination']['Item2']
                        if (x,y) in player_movement:
                            player_movement[(x,y)] += 1
                        else:
                            player_movement[(x,y)] = 1
    
    # for i in player_movement:
    #     print(i , ":", player_movement[i])



    heatmap_cc = np.zeros((10,5), dtype=int)
    for i in player_movement:
        x = int(i[0])
        y = int(i[1])
        heatmap_cc[10-y-1,x] += int(player_movement[i])

    print(heatmap_cc)
    

    xlabel = ["0", "1" , "2" , "3" , "4", "5", "6", "7", "8","9", "10"]
    ylabel = [ "9" , "8" , "7" , "6", "5", "4", "3", "2", "1", "0"]
    
    #Challenge_Circle  heatmap specifics
    fig, ax = plt.subplots()
    # cmap_bounds = [0,6,15,25,45,70,100,150,350]
    # cmap_norm = colors.BoundaryNorm(cmap_bounds, 8)
    im = ax.imshow(heatmap_cc, cmap='coolwarm')
    ax.set_yticks(np.arange(10), labels = ylabel )
    ax.set_xticks(np.arange(5), labels = xlabel[0:5])
    ax.set_title("FogofWar Player HeatMap")
    ax.spines[:].set_visible(False)
    ax.set_xticks(np.arange(6)-0.5, minor = True)
    ax.set_yticks(np.arange(11)-0.5, minor = True)
    ax.grid(which="minor", color="w", linestyle='-', linewidth=3)
    ax.tick_params(which="minor", bottom=False, left=False)
    fig.tight_layout()
    cbar = ax.figure.colorbar(im, ax=ax)
    #cbar.ax.set_ylabel(cbarlabel, rotation=-90, va="bottom")
    plt.show()




if __name__ == "__main__":
    main()