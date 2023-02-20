import numpy as np
import matplotlib.pyplot as plt
import gspread
import json
from oauth2client.service_account import ServiceAccountCredentials

# !!! Current code makes a heatmap for ONLY 5X10 TutorialFogOfWar level. 

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
    try:
        worksheet = sheet.get_worksheet(0)
    except:
        print('Some Error Occurred while fetching the response sheet')

    # ##### Google Sheet heatmap data fetch and filter START ####
    # listofall_5_10_maps=[]
    # matching_cells = worksheet.findall('TutorialFogOfWar', in_column=4)
    # matching_rows = []
    # for cell in matching_cells:
    #     row = worksheet.row_values(cell.row)
    #     matching_rows.append(row)

    # for row in matching_rows:
    #     listofall_5_10_maps.append(row[6])
    # processthisshit(listofall_5_10_maps)
    # print(heatmap_5_10)
    # ##### Google Sheet heatmap data fetch and filter  END ####


    #### Google Sheet % of moves by Circle START###
    circle_Moves = worksheet.col_values(8) 
    total_Moves = worksheet.col_values(3) 
    percentage_circle = [0]*len(total_Moves)

    for i in range(1,len(total_Moves)):
        percentage_circle[i] = [str (int(circle_Moves[i])/int(total_Moves[i])*100)]


    filter_data_sheet = gc.open('Multilevel_branch_analytics_test').worksheet('Filter_Data_DND')
    n = len(percentage_circle)+4
    srange = f'D4:D{n}'
    filter_data_sheet.update(srange,percentage_circle[1:])
    ### Google Sheet % of moves by Circle END ###

    #### Google Sheet % of moves by diamond START###
    diamond_Moves = worksheet.col_values(9) 
    total_Moves = worksheet.col_values(3) 
    percentage_diamond = [0]*len(total_Moves)

    for i in range(1,len(total_Moves)):
        percentage_diamond[i] = [str (int(diamond_Moves[i])/int(total_Moves[i])*100)]


    filter_data_sheet = gc.open('Multilevel_branch_analytics_test').worksheet('Filter_Data_DND')
    n = len(percentage_diamond)+4
    srange = f'F4:F{n}'
    filter_data_sheet.update(srange,percentage_diamond[1:])
    ### Google Sheet % of moves by Diamond END ###

    

    # # plot the heatmap
    # fig, ax = plt.subplots()
    # im = ax.imshow(heatmap_5_10, cmap='coolwarm')
    # # set the colorbar
    # cbar = ax.figure.colorbar(im, ax=ax)
    # # show the plot
    # plt.show()



if __name__ == "__main__":
    main()