import numpy as np
import matplotlib.pyplot as plt
import gspread
import json
from oauth2client.service_account import ServiceAccountCredentials

# !!! DO NOT USE CURRENT CODE for 5X8 level data - ONLY 5X10

heatmap_5_10 = np.zeros((10,5), dtype=int)

def add_to_5x10_heatmap(hashmap):
    global heatmap_5_10

    for i in hashmap:
        n = i.strip('()').split(',')
        x = int(n[0])
        y = int(n[1])
        heatmap_5_10[10-y-1,x] += int(hashmap[i])



def processthisshit(listofall_5_10_maps):

    for item in listofall_5_10_maps:
        hashmap = json.loads(item)
        add_to_5x10_heatmap(hashmap)

    return 


def main():

    ##### AUTH CODE START ##  DO NOT TOUCH ####
    scope = ['https://spreadsheets.google.com/feeds',
         'https://www.googleapis.com/auth/spreadsheets',
         'https://www.googleapis.com/auth/drive.file',
         'https://www.googleapis.com/auth/drive']
   
    credentials = ServiceAccountCredentials.from_json_keyfile_name('gamedev_key.json', scope)            
    gc = gspread.authorize(credentials)
    sheet = gc.open('Heatmap_Test (Responses)')
    ##### AUTH CODE END ##  DO NOT TOUCH ####

    ##### Google Sheet fetch and filter START ####
    listofall_5_10_maps=[]
    try:
        worksheet = sheet.get_worksheet(0)
    except:
        print('Some Error Occurred while fetching the response sheet')

    matching_cells = worksheet.findall('TutorialFogOfWar', in_column=4)
    matching_rows = []
    for cell in matching_cells:
        row = worksheet.row_values(cell.row)
        matching_rows.append(row)

    for row in matching_rows:
        listofall_5_10_maps.append(row[6])
    
    ##### Google Sheet fetch and filter  END ####


    processthisshit(listofall_5_10_maps)
    print(heatmap_5_10)

    # plot the heatmap
    fig, ax = plt.subplots()
    im = ax.imshow(heatmap_5_10, cmap='coolwarm')
    # set the colorbar
    cbar = ax.figure.colorbar(im, ax=ax)
    # show the plot
    plt.show()



if __name__ == "__main__":
    main()
    

    