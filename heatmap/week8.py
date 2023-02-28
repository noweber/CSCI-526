import numpy as np
import matplotlib.pyplot as plt
import gspread
import json
from oauth2client.service_account import ServiceAccountCredentials


heatmap_5_10 = np.zeros((10,5), dtype=int)
#Duplicated code for now only
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

    HeatMapSheet = gc.open('Heatmap_Test (Responses)')
    sheet3 = gc.open('Week 8 - End of Level Analytics Form (Responses)')
    sheet2 = gc.open('Week 8 - Start of Level Analytics Form (Responses)')
    filter_data_sheet = gc.open('Multilevel_branch_analytics_test').worksheet('Filter_Data_DND')

    ##### AUTH CODE END ##  DO NOT TOUCH ####
    try:
        worksheet1 = HeatMapSheet.get_worksheet(0)
    except:
        print('Some Error Occurred while fetching the Week7 response sheet')

    try:
        Week8StartSheet = sheet2.get_worksheet(0)
    except:
        print('Some Error Occurred while fetching the Week 8 response sheet')

    try:
        Week8EndSheet = sheet3.get_worksheet(0)
    except:
        print('Some Error Occurred while fetching the Week 8 response sheet')



    # Average_Time_Code
    dict_time_per_level = {"TutorialLevel": [],
                             "TutorialFogOfWar": [],
                             "Level_One": [],
                             "Level_Two":[],
                             "ChainPrototype":[]
                             }

    all_val = worksheet1.get_all_values()
    for row in all_val[1:]:
        dict_time_per_level[row[3]].append(float(row[4]))
    
    all_val2 = Week8EndSheet.get_all_values()
    for row in all_val2[1:]:
        dict_time_per_level[row[3]].append(float(row[2]))

    avg_time_per_level = { "TutorialLevel": 0,
                             "TutorialFogOfWar":0 ,
                             "Level_One": 0,
                             "Level_Two": 0 ,
                             "ChainPrototype":0}
    for key in dict_time_per_level:
        avg = (sum(dict_time_per_level[key])/len(dict_time_per_level[key]) )* 60
        avg_time_per_level[key] = round(avg,2)
    

    
    filter_data_sheet.update('J3' , float(avg_time_per_level["TutorialLevel"]) )
    filter_data_sheet.update('J4' , float(avg_time_per_level["TutorialFogOfWar"]) )
    filter_data_sheet.update('J5' , float(avg_time_per_level["Level_One"]) )
    filter_data_sheet.update('J6' , float(avg_time_per_level["Level_Two"]) )


    #Start vs Finish Player per Level
    player_start_per_level = { "TutorialLevel": 0,
                             "TutorialFogOfWar":0 ,
                             "Level_One": 0,
                             "Level_Two": 0 ,
                             "ChainPrototype":0}
    
    player_end_per_level = { "TutorialLevel": 0,
                             "TutorialFogOfWar":0 ,
                             "Level_One": 0,
                             "Level_Two": 0 ,
                             "ChainPrototype":0}

    start = Week8StartSheet.get_all_values()
    for row in start[1:]:
        player_start_per_level[row[2]] += 1

    end = Week8EndSheet.get_all_values()
    for row in end[1:]:
        player_end_per_level[row[3]] += 1

    filter_data_sheet.update('L3' , int(player_start_per_level["TutorialLevel"]) )
    filter_data_sheet.update('L4' , int(player_end_per_level["TutorialLevel"]) )
    filter_data_sheet.update('L6' , int(player_start_per_level["TutorialFogOfWar"]) )
    filter_data_sheet.update('L7' , int(player_end_per_level["TutorialFogOfWar"]) )
    filter_data_sheet.update('L9' , int(player_start_per_level["Level_One"]) )
    filter_data_sheet.update('L10' , int(player_end_per_level["Level_One"]) )
    filter_data_sheet.update('L12' , int(player_start_per_level["Level_Two"]) )
    filter_data_sheet.update('L13' , int(player_end_per_level["Level_Two"]) )
    
    
    print(player_start_per_level,player_end_per_level)


    #Heatmap2 from EndForm
    listofall_5_10_maps=[]
    matching_cells = Week8EndSheet.findall('TutorialFogOfWar', in_column=4)
    matching_rows = []
    for cell in matching_cells:
        row = Week8EndSheet.row_values(cell.row)
        matching_rows.append(row)

    for row in matching_rows:
        if row[7] != "{}":
            listofall_5_10_maps.append(row[7])

    processthisshit(listofall_5_10_maps)
    print(heatmap_5_10)

    # plot the heatmap
    fig, ax = plt.subplots()
    im = ax.imshow(heatmap_5_10, cmap='Pastel1')
    # set the colorbar
    cbar = ax.figure.colorbar(im, ax=ax)
    # show the plot
    plt.show()

if __name__ == "__main__":
    main()