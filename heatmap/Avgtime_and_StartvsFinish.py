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
    SOLgsheet = gc.open('Midterm_Start_Analytics').worksheet('Form Responses 1')
    filter_data_sheet = gc.open('Multilevel_branch_analytics_test').worksheet('Week9_Cleaned_Data')

    ##### AUTH CODE END ##  DO NOT TOUCH ####
    
    # Average_Time_Code
    dict_time_per_level = {"TutorialLevel": [],
                             "TutorialFogOfWar": [],
                             "Level_One": [],
                             "Level_Two":[],
                             "Challenge_Circle":[],
                             "Challenge_Scout":[]
                             }

    all_val = EOLgsheet.get_all_values()
    for row in all_val[1:]:
        dict_time_per_level[row[3]].append(float(row[2]))

    avg_time_per_level = { "TutorialLevel": 0,
                             "TutorialFogOfWar":0 ,
                             "Level_One": 0,
                             "Level_Two": 0 ,
                             "Challenge_Circle":0,
                             "Challenge_Scout":0}
    
    for key in dict_time_per_level:
        temp = (sum(dict_time_per_level[key])/len(dict_time_per_level[key]) )* 60
        avg_time_per_level[key] = round(temp,2)
    
    filter_data_sheet.update('R2' , avg_time_per_level['TutorialLevel'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('R3' , avg_time_per_level['TutorialFogOfWar'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('R4' , avg_time_per_level['Level_One'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('R5' , avg_time_per_level['Level_Two'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('R6' , avg_time_per_level['Challenge_Circle'] , value_input_option='USER_ENTERED')
    filter_data_sheet.update('R7' , avg_time_per_level['Challenge_Scout'] , value_input_option='USER_ENTERED')



    #Start vs Finish Player per Level
    player_start_per_level = { "TutorialLevel": 0,
                             "TutorialFogOfWar":0 ,
                             "Level_One": 0,
                             "Level_Two": 0 ,
                             "Challenge_Circle":0,
                             "Challenge_Scout": 0}
    
    player_end_per_level = { "TutorialLevel": 0,
                             "TutorialFogOfWar":0 ,
                             "Level_One": 0,
                             "Level_Two": 0 ,
                             "Challenge_Circle":0,
                             "Challenge_Scout": 0}

    start = SOLgsheet.get_all_values()
    for row in start[1:]:
        player_start_per_level[row[2]] += 1

    end = EOLgsheet.get_all_values()
    for row in end[1:]:
        player_end_per_level[row[3]] += 1

    filter_data_sheet.update('T3' , int(player_start_per_level["TutorialLevel"]),value_input_option='USER_ENTERED' )
    filter_data_sheet.update('T4' , int(player_end_per_level["TutorialLevel"]) ,value_input_option='USER_ENTERED')
    filter_data_sheet.update('T6' , int(player_start_per_level["TutorialFogOfWar"]),value_input_option='USER_ENTERED' )
    filter_data_sheet.update('T7' , int(player_end_per_level["TutorialFogOfWar"]), value_input_option='USER_ENTERED' )
    filter_data_sheet.update('T9' , int(player_start_per_level["Level_One"]),value_input_option='USER_ENTERED' )
    filter_data_sheet.update('T10' , int(player_end_per_level["Level_One"]) ,value_input_option='USER_ENTERED')
    filter_data_sheet.update('T12' , int(player_start_per_level["Level_Two"]),value_input_option='USER_ENTERED' )
    filter_data_sheet.update('T13' , int(player_end_per_level["Level_Two"]),value_input_option='USER_ENTERED' )
    filter_data_sheet.update('T15' , int(player_end_per_level["Challenge_Circle"]),value_input_option='USER_ENTERED' )
    filter_data_sheet.update('T16' , int(player_end_per_level["Challenge_Circle"]),value_input_option='USER_ENTERED' )
    filter_data_sheet.update('T18' , int(player_end_per_level["Challenge_Scout"]),value_input_option='USER_ENTERED' )
    filter_data_sheet.update('T19' , int(player_end_per_level["Challenge_Scout"]),value_input_option='USER_ENTERED' )

    
          
if __name__ == "__main__":
    main()
