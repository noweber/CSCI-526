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
    filter_data_sheet = gc.open('Multilevel_branch_analytics_test').worksheet('Week9_Cleaned_Data')

    ##### AUTH CODE END ##  DO NOT TOUCH ####

    # WL_per_level = { "TutorialLevel": [0, 0],
    #                          "TutorialFogOfWar":[0,0],
    #                          "Level_One": [0,0],
    #                          "Level_Two": [0,0],
    #                         "Challenge_Circle" : [0,0],
    #                         "Challenge_Scout" : [0,0],
    #                         "Level_Three" : [0,0]}
    
    WL_new_per_level = { "Tutorial_Circle": [0, 0],
                             "Tutorial_Circle_Ability":[0,0],
                             "Tutorial_Diamond": [0,0],
                             "Tutorial_Diamond_Ability": [0,0],
                            "Tutorial_Triangle" : [0,0],
                            "Tutorial_Triangle_Ability" : [0,0],
                            "Tutorial_Scout" : [0,0],
                            "Tutorial_Scout_Ability" : [0,0],
                            "Level_Three" : [0,0],
                            "Level_One" : [0,0],
                            "Level_Two" : [0,0],
                            "Level_Three" : [0,0],
                             "TutorialLevel": [0, 0],
                             "TutorialFogOfWar":[0,0],
                            "Challenge_Circle" : [0,0],
                            "Challenge_Scout" : [0,0]
                            }
    
    data = EOLgsheet.get_all_values()
    for row in data[1:]:

        hashmap = json.loads(row[13])
        if(hashmap['VictoriousPlayerName'] == 'AI' ):
            WL_new_per_level[row[3]][1] += 1
        else:
            WL_new_per_level[row[3]][0] += 1

    print(WL_new_per_level)
    
    #print(Synergy_per_level)

    # n = len(Synergy_per_level['TutorialLevel'])
    # drange = f'V3:V{n+3}'
    # filter_data_sheet.update(drange , Synergy_per_level['TutorialLevel'] , value_input_option='USER_ENTERED')

    # n = len(Synergy_per_level['TutorialFogOfWar'])
    # drange = f'W3:W{n+3}'
    # filter_data_sheet.update(drange , Synergy_per_level['TutorialFogOfWar'] , value_input_option='USER_ENTERED')

    # n = len(Synergy_per_level['Level_One'])
    # drange = f'X3:X{n+3}'
    # filter_data_sheet.update(drange , Synergy_per_level['Level_One'] , value_input_option='USER_ENTERED')

    # n = len(Synergy_per_level['Level_Two'])
    # drange = f'Y3:Y{n+3}'
    # filter_data_sheet.update(drange , Synergy_per_level['Level_Two'] , value_input_option='USER_ENTERED')

    # n = len(Synergy_per_level['Challenge_Circle'])
    # drange = f'Z3:Z{n+3}'
    # filter_data_sheet.update(drange , Synergy_per_level['Challenge_Circle'] , value_input_option='USER_ENTERED')

    # n = len(Synergy_per_level['Challenge_Scout'])
    # drange = f'AA3:AA{n+3}'
    # filter_data_sheet.update(drange , Synergy_per_level['Challenge_Scout'] , value_input_option='USER_ENTERED')



if __name__ == "__main__":
    main()