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

    Synergy_per_level = { "TutorialLevel": [],
                             "TutorialFogOfWar":[] ,
                             "Level_One": [],
                             "Level_Two": [],
                            "Challenge_Circle" : [],
                            "Challenge_Scout" : []}
    
    data = EOLgsheet.get_all_values()
    synergy_list = []
    for row in data[1:]:
        # if row[3] == 'Level_Two':
        count = 0
        hashmap = json.loads(row[13])
        list_of_Moves = hashmap['MovesMade']
        for move in (list_of_Moves):
            if move['Player'] == 'Human' and move['Piece']== 'Circle' and ( abs(int(move['Position']['Item1'])-int(move['Destination']['Item1']))>1 or abs(int(move['Position']['Item2'])- int(move['Destination']['Item2']))>1):
                count +=1
            
        Synergy_per_level[row[3]].append([count])
    
    #print(Synergy_per_level)

    n = len(Synergy_per_level['TutorialLevel'])
    drange = f'V3:V{n+3}'
    filter_data_sheet.update(drange , Synergy_per_level['TutorialLevel'] , value_input_option='USER_ENTERED')

    n = len(Synergy_per_level['TutorialFogOfWar'])
    drange = f'W3:W{n+3}'
    filter_data_sheet.update(drange , Synergy_per_level['TutorialFogOfWar'] , value_input_option='USER_ENTERED')

    n = len(Synergy_per_level['Level_One'])
    drange = f'X3:X{n+3}'
    filter_data_sheet.update(drange , Synergy_per_level['Level_One'] , value_input_option='USER_ENTERED')

    n = len(Synergy_per_level['Level_Two'])
    drange = f'Y3:Y{n+3}'
    filter_data_sheet.update(drange , Synergy_per_level['Level_Two'] , value_input_option='USER_ENTERED')

    n = len(Synergy_per_level['Challenge_Circle'])
    drange = f'Z3:Z{n+3}'
    filter_data_sheet.update(drange , Synergy_per_level['Challenge_Circle'] , value_input_option='USER_ENTERED')

    n = len(Synergy_per_level['Challenge_Scout'])
    drange = f'AA3:AA{n+3}'
    filter_data_sheet.update(drange , Synergy_per_level['Challenge_Scout'] , value_input_option='USER_ENTERED')



if __name__ == "__main__":
    main()