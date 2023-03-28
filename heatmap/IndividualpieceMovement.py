import gspread
import json
import time
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
    filter_data_sheet = gc.open('Multilevel_branch_analytics_test').worksheet('Week9_Cleaned_Data')

    ##### AUTH CODE END ##  DO NOT TOUCH ####
    #PieceMoves_per_level = { "TutorialLevel": [Circle,Diamond,Scout] }

    PieceMoves_per_level = { "TutorialLevel": [[0],[0],[0]],
                             "TutorialFogOfWar":[[0],[0],[0]],
                             "Level_One": [[0],[0],[0]],
                             "Level_Two": [[0],[0],[0]],
                            "Challenge_Circle" : [[0],[0],[0]],
                            "Challenge_Scout" : [[0],[0],[0]]}
    
    Circle_percentages_per_level = { "TutorialLevel": [],
                             "TutorialFogOfWar":[] ,
                             "Level_One": [],
                             "Level_Two": [],
                            "Challenge_Circle" : [],
                            "Challenge_Scout" : []}
    
    Diamond_percentages_per_level = { "TutorialLevel": [],
                             "TutorialFogOfWar":[] ,
                             "Level_One": [],
                             "Level_Two": [],
                            "Challenge_Circle" : [],
                             "Challenge_Scout": [] }
    
    

    data = EOLgsheet.get_all_values()
    #  USE ONLY ONCE : Loop to fix the { Circle Diamond Triangle Scout } wrong counts in original form data. 
    # for row_index,row_data in enumerate(data[1:]):
    #     move_initialize = {"Circle":0,"Diamond":0,"Triangle":0,"Scout":0}
    #     hashmap_allmoves = json.loads(row_data[13])
    #     movesmade = hashmap_allmoves['MovesMade']
    #     for move in movesmade:
    #         move_initialize[move['Piece']] += 1
    #     final_js = json.dumps(move_initialize)
    #     EOLgsheet.update( f'M{row_index+2}' , final_js)
    #     time.sleep(2)

    
        


    for row_data in data[1:]:
        hashmap = json.loads(row_data[12])
        PieceMoves_per_level[row_data[3]][0][0] += hashmap['Circle']
        PieceMoves_per_level[row_data[3]][1][0] += hashmap['Diamond']
        PieceMoves_per_level[row_data[3]][2][0] += hashmap['Scout']

        hashmap_allmoves = json.loads(row_data[13])
        total_moves = len(hashmap_allmoves['MovesMade'])
        if total_moves == 0:
            continue
        temp = (int(hashmap['Circle']) *100)//(int(total_moves))
        Circle_percentages_per_level[row_data[3]].append([temp])
        temp = (int(hashmap['Diamond']) *100)//(int(total_moves))
        Diamond_percentages_per_level[row_data[3]].append([temp])

    # print(Circle_percentages_per_level)
    # print(Diamond_percentages_per_level)

    filter_data_sheet.update('J3:J5' , PieceMoves_per_level['TutorialLevel'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('K3:K5' , PieceMoves_per_level['TutorialFogOfWar'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('L3:L5' , PieceMoves_per_level['Level_One'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('M3:M5' , PieceMoves_per_level['Level_Two'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('N3:N5' , PieceMoves_per_level['Challenge_Circle'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('O3:O5' , PieceMoves_per_level['Challenge_Scout'] , value_input_option='USER_ENTERED')
    
    n = len(Circle_percentages_per_level['TutorialFogOfWar'])
    filter_data_sheet.update(f'I11:I{10+n}' , Circle_percentages_per_level['TutorialFogOfWar'] , value_input_option='USER_ENTERED')
    n = len(Circle_percentages_per_level['Level_One'])
    filter_data_sheet.update(f'K11:K{10+n}' , Circle_percentages_per_level['Level_One'] , value_input_option='USER_ENTERED')
    n = len(Circle_percentages_per_level['Level_Two'])
    filter_data_sheet.update(f'M11:M{10+n}' , Circle_percentages_per_level['Level_Two'] , value_input_option='USER_ENTERED')
    n = len(Circle_percentages_per_level['Challenge_Circle'])
    filter_data_sheet.update(f'O11:O{10+n}' , Circle_percentages_per_level['Challenge_Circle'] , value_input_option='USER_ENTERED')

    n = len(Diamond_percentages_per_level['TutorialFogOfWar'])
    filter_data_sheet.update(f'J11:J{10+n}' , Diamond_percentages_per_level['TutorialFogOfWar'] , value_input_option='USER_ENTERED')
    n = len(Diamond_percentages_per_level['Level_One'])
    filter_data_sheet.update(f'L11:L{10+n}' , Diamond_percentages_per_level['Level_One'] , value_input_option='USER_ENTERED')
    n = len(Diamond_percentages_per_level['Level_Two'])
    filter_data_sheet.update(f'N11:N{10+n}' , Diamond_percentages_per_level['Level_Two'] , value_input_option='USER_ENTERED')
    n = len(Diamond_percentages_per_level['Challenge_Circle'])
    filter_data_sheet.update(f'P11:P{10+n}' , Diamond_percentages_per_level['Challenge_Circle'] , value_input_option='USER_ENTERED')


      
if __name__ == "__main__":
    main()
