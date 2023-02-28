using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Google FORM URL "https://docs.google.com/forms/u/1/d/e/1FAIpQLScbdeMbYwWKzLKgAWeWEHL94ruFqymF66zYgzN2pEjgAFcrCg/formResponse" 
/// Week 7 Form URL: https://docs.google.com/forms/d/e/1FAIpQLSfvesyHfUZdYlkKeJvxUR5VH5F165VoJnUx3Q8FB6beTribkA/formResponse
/// </summary>
public class Analytics : MonoBehaviour
{
    [SerializeField] private string StartOfLevelUrl;
    [SerializeField] private string EndOfLevelUrl;

    public static Analytics Instance;

    private void Awake()
    {
        if (string.IsNullOrEmpty(StartOfLevelUrl))
        {
            StartOfLevelUrl = "https://docs.google.com/forms/d/e/1FAIpQLSerply3kFSQjKMtiMbBGhCrQ-OtvP-Oa1fGMKo5OtTe6G5omg/formResponse";
        }
        if (string.IsNullOrEmpty(EndOfLevelUrl))
        {
            EndOfLevelUrl = "https://docs.google.com/forms/d/e/1FAIpQLSdvn64Wxc0jOofVxK7r-tztWiLzJq31PnJCJZNMSsPzwczG4Q/formResponse";
        }
    }

    public Analytics()
    {
        Instance = this;
    }

    public void SendStartOfLevelData(string sessionID, string levelName, int levelWidth, int levelHeight)
    {
        StartCoroutine(
            PostStartOfLevelData(sessionID,
            levelName,
            levelWidth.ToString(),
            levelHeight.ToString()));
    }

    public void SendEndOfLevelData(string sessionID, float timePlayedInMinutes, string levelName, int levelWidth, int levelHeight, int totalNumberOfPiecesMoved, string tilesOccupiedHeatmapJson, string pieceMovementHeatmapJson, int countOfCirclePiecesMoved, int countOfDiamondPiecesMoved, int countOfScoutPiecesMoved, string countOfPiecesMovedByTypeJson)
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
            countOfPiecesMovedByTypeJson));
    }

    private IEnumerator PostStartOfLevelData(string sessionID, string levelName, string levelWidth, string levelHeight)
    {
        Debug.Log("Post Coroutine started");
        WWWForm form = new WWWForm();
        form.AddField("entry.1379325124", sessionID);
        form.AddField("entry.1700549212", levelName);
        form.AddField("entry.846791691", levelWidth);
        form.AddField("entry.1378155138", levelHeight);
        return PostAnalyticsForm(form);
    }

    private IEnumerator PostEndOfLevelData(string sessionID, string timePlayed, string levelName, string levelWidth, string levelHeight, string totalNumberOfPiecesMoved, string tilesOccupiedHeatmapJson, string pieceMovementHeatmapJson, string countOfCirclePiecesMoved, string countOfDiamondPiecesMoved, string countOfScoutPiecesMoved, string countOfPiecesMovedByTypeJson)
    {
        Debug.Log("Post Coroutine started");
        WWWForm form = new WWWForm();
        form.AddField("entry.1379325124", sessionID);
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
        return PostAnalyticsForm(form);
    }

    private IEnumerator PostAnalyticsForm(WWWForm analyticsForm)
    {
        if (analyticsForm == null)
        {
            throw new ArgumentNullException(nameof(analyticsForm));
        }

        using (UnityWebRequest www = UnityWebRequest.Post(EndOfLevelUrl, analyticsForm))
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

