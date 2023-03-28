using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Analytics : MonoBehaviour
{
    [SerializeField] private string StartOfLevelUrl;
    [SerializeField] private string EndOfLevelUrl;

    public static Analytics Instance;

    public Analytics()
    {
        Instance = this;
    }

    void Awake()
    {
        // TODO: Take these URLs from a config file and swap it on build for production instances.
        if (string.IsNullOrEmpty(StartOfLevelUrl))
        {
            StartOfLevelUrl = "https://docs.google.com/forms/d/e/1FAIpQLScx2srJ-CuNMi6lSInGdIulNEICS_A4DGzPgInOSyEDVkD_-A/formResponse";
        }
        if (string.IsNullOrEmpty(EndOfLevelUrl))
        {
            EndOfLevelUrl = "https://docs.google.com/forms/d/e/1FAIpQLSdpsatzp5ucBpTpuz2khbtjAWZ9OOz7baGN7fWJDW20l8aNYQ/formResponse";
        }
    }



    public void SendStartOfLevelData(string sessionID, string levelName, int levelWidth, int levelHeight)
    {
        StartCoroutine(
            PostStartOfLevelData(sessionID,
            levelName,
            levelWidth.ToString(),
            levelHeight.ToString()));
    }

    public void SendEndOfLevelData(string sessionID, float timePlayedInMinutes, string levelName, int levelWidth, int levelHeight, int totalNumberOfPiecesMoved, string tilesOccupiedHeatmapJson, string pieceMovementHeatmapJson, int countOfCirclePiecesMoved, int countOfDiamondPiecesMoved, int countOfScoutPiecesMoved, string countOfPiecesMovedByTypeJson, string replayDataJson)
    {
        Debug.Log("Analytics Send method started");

        StartCoroutine(
            PostEndOfLevelData(sessionID,
            timePlayedInMinutes.ToString(),
            levelName,
            levelWidth.ToString(),
            levelHeight.ToString(),
            totalNumberOfPiecesMoved.ToString(),
            tilesOccupiedHeatmapJson,
            pieceMovementHeatmapJson,
            countOfCirclePiecesMoved.ToString(),
            countOfDiamondPiecesMoved.ToString(),
            countOfScoutPiecesMoved.ToString(),
            countOfPiecesMovedByTypeJson,
            replayDataJson));
    }

    private IEnumerator PostStartOfLevelData(string sessionID, string levelName, string levelWidth, string levelHeight)
    {
        Debug.Log("Post Coroutine started");
        WWWForm form = new WWWForm();
        form.AddField("entry.1379325124", sessionID);
        form.AddField("entry.1700549212", levelName);
        form.AddField("entry.846791691", levelWidth);
        form.AddField("entry.1378155138", levelHeight);
        return PostAnalyticsForm(form, StartOfLevelUrl);
    }

    private IEnumerator PostEndOfLevelData(string sessionID, string timePlayed, string levelName, string levelWidth, string levelHeight, string totalNumberOfPiecesMoved, string tilesOccupiedHeatmapJson, string pieceMovementHeatmapJson, string countOfCirclePiecesMoved, string countOfDiamondPiecesMoved, string countOfScoutPiecesMoved, string countOfPiecesMovedByTypeJson, string replayDataJson)
    {
        Debug.Log("Post Coroutine started");
        WWWForm form = new WWWForm();
        form.AddField("entry.1379325124", sessionID);
        form.AddField("entry.589367142", timePlayed);
        form.AddField("entry.1700549212", levelName);
        form.AddField("entry.846791691", levelWidth);
        form.AddField("entry.1378155138", levelHeight);
        form.AddField("entry.1367608849", totalNumberOfPiecesMoved);
        form.AddField("entry.812093032", tilesOccupiedHeatmapJson);
        form.AddField("entry.1687230251", pieceMovementHeatmapJson);
        form.AddField("entry.346810973", countOfCirclePiecesMoved);
        form.AddField("entry.1119668788", countOfDiamondPiecesMoved);
        form.AddField("entry.1498362014", countOfScoutPiecesMoved);
        form.AddField("entry.1649735813", countOfPiecesMovedByTypeJson); 
        form.AddField("entry.617707412", replayDataJson);
        return PostAnalyticsForm(form, EndOfLevelUrl);
    }

    private IEnumerator PostAnalyticsForm(WWWForm analyticsForm, string formUrl)
    {
        if (analyticsForm == null)
        {
            throw new ArgumentNullException(nameof(analyticsForm));
        }

        using (UnityWebRequest www = UnityWebRequest.Post(formUrl, analyticsForm))
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete.");
            }

        }
    }
}

