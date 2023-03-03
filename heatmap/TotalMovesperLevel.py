
import gspread
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

    player_totalmoves_per_level = { "TutorialLevel": [],
                             "TutorialFogOfWar":[] ,
                             "Level_One": [],
                             "Level_Two": [],
                            "Challenge_Circle" : []}
    
    data = EOLgsheet.get_all_values()
    for row in data[1:]:
        player_totalmoves_per_level[row[3]].append([row[6]])

    #BatchUpdate

    n = len(player_totalmoves_per_level['TutorialLevel'])
    drange = f'A3:A{n+3}'
    filter_data_sheet.update(drange , player_totalmoves_per_level['TutorialLevel'] , value_input_option='USER_ENTERED')

    n = len(player_totalmoves_per_level['TutorialFogOfWar'])
    drange = f'B3:B{n+3}'
    filter_data_sheet.update(drange , player_totalmoves_per_level['TutorialFogOfWar'] , value_input_option='USER_ENTERED')

    n = len(player_totalmoves_per_level['Level_One'])
    drange = f'C3:C{n+3}'
    filter_data_sheet.update(drange , player_totalmoves_per_level['Level_One'] , value_input_option='USER_ENTERED')

    n = len(player_totalmoves_per_level['Level_Two'])
    drange = f'D3:D{n+3}'
    filter_data_sheet.update(drange , player_totalmoves_per_level['Level_Two'] , value_input_option='USER_ENTERED')

    n = len(player_totalmoves_per_level['Challenge_Circle'])
    drange = f'E3:E{n+3}'
    filter_data_sheet.update(drange , player_totalmoves_per_level['Challenge_Circle'] , value_input_option='USER_ENTERED')
    
    
if __name__ == "__main__":
    main()




