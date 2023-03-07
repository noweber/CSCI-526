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


    EOLgsheet = gc.open('Week 8 - End of Level Analytics Form (Responses)').worksheet('Form Responses 1')
    filter_data_sheet = gc.open('Multilevel_branch_analytics_test').worksheet('Week9_Cleaned_Data')

    ##### AUTH CODE END ##  DO NOT TOUCH ####
    #PieceMoves_per_level = { "TutorialLevel": [Circle,Diamond,Scout] }

    PieceMoves_per_level = { "TutorialLevel": [[0],[0],[0]],
                             "TutorialFogOfWar":[[0],[0],[0]],
                             "Level_One": [[0],[0],[0]],
                             "Level_Two": [[0],[0],[0]],
                            "Challenge_Circle" : [[0],[0],[0]] }
    
    Circle_percentages_per_level = { "TutorialLevel": [],
                             "TutorialFogOfWar":[] ,
                             "Level_One": [],
                             "Level_Two": [],
                            "Challenge_Circle" : []}
    
    Diamond_percentages_per_level = { "TutorialLevel": [],
                             "TutorialFogOfWar":[] ,
                             "Level_One": [],
                             "Level_Two": [],
                            "Challenge_Circle" : [] }
    

    data = EOLgsheet.get_all_values()
    for row in data[5:]:
        hashmap = json.loads(row[12])
        PieceMoves_per_level[row[3]][0][0] += hashmap['Circle']
        PieceMoves_per_level[row[3]][1][0] += hashmap['Diamond']
        PieceMoves_per_level[row[3]][2][0] += hashmap['Scout']

        if row[6] == '0':
            continue
        temp = (int(hashmap['Circle']) *100)//(int(row[6]))
        Circle_percentages_per_level[row[3]].append([temp])
        temp = (int(hashmap['Diamond']) *100)//(int(row[6]))
        Diamond_percentages_per_level[row[3]].append([temp])

    # print(Circle_percentages_per_level)
    # print(Diamond_percentages_per_level)

    filter_data_sheet.update('J3:J5' , PieceMoves_per_level['TutorialLevel'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('K3:K5' , PieceMoves_per_level['TutorialFogOfWar'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('L3:L5' , PieceMoves_per_level['Level_One'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('M3:M5' , PieceMoves_per_level['Level_Two'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('N3:N5' , PieceMoves_per_level['Challenge_Circle'] , value_input_option='USER_ENTERED')
    
    n = len(Circle_percentages_per_level['TutorialFogOfWar'])
    filter_data_sheet.update(f'I11:I{10+n}' , Circle_percentages_per_level['TutorialFogOfWar'] , value_input_option='USER_ENTERED')
    n = len(Circle_percentages_per_level['Level_One'])
    filter_data_sheet.update(f'K11:K{10+n}' , Circle_percentages_per_level['Level_One'] , value_input_option='USER_ENTERED')
    n = len(Circle_percentages_per_level['Level_Two'])
    filter_data_sheet.update(f'M11:M{10+n}' , Circle_percentages_per_level['Level_Two'] , value_input_option='USER_ENTERED')

    n = len(Diamond_percentages_per_level['TutorialFogOfWar'])
    filter_data_sheet.update(f'J11:J{10+n}' , Diamond_percentages_per_level['TutorialFogOfWar'] , value_input_option='USER_ENTERED')
    n = len(Diamond_percentages_per_level['Level_One'])
    filter_data_sheet.update(f'L11:L{10+n}' , Diamond_percentages_per_level['Level_One'] , value_input_option='USER_ENTERED')
    n = len(Diamond_percentages_per_level['Level_Two'])
    filter_data_sheet.update(f'N11:N{10+n}' , Diamond_percentages_per_level['Level_Two'] , value_input_option='USER_ENTERED')


      
if __name__ == "__main__":
    main()
