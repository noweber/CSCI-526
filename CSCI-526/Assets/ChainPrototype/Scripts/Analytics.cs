using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Google FORM URL "https://docs.google.com/forms/u/1/d/e/1FAIpQLScbdeMbYwWKzLKgAWeWEHL94ruFqymF66zYgzN2pEjgAFcrCg/formResponse" 
/// Week 7 Form URL: https://docs.google.com/forms/d/e/1FAIpQLSfvesyHfUZdYlkKeJvxUR5VH5F165VoJnUx3Q8FB6beTribkA/formResponse
/// </summary>
public class Analytics : MonoBehaviour
{

    [SerializeField] private string URL;

    public static Analytics Instance;

    public Analytics()
    {
        Instance = this;
    }

    public void Send(string sessionID, float timePlayed, string levelName, int levelWidth, int levelHeight, int totalMovesMadeThisLevel, string movesMadeJson)
    {
        Debug.Log("Analytics Send method started");

        StartCoroutine(
            Post(sessionID,
            timePlayed.ToString(),
            levelName,
            levelWidth,
            levelHeight,
            totalMovesMadeThisLevel.ToString(),
            movesMadeJson));
    }

    private IEnumerator Post(string sessionID, string timePlayed, string levelName, int levelWidth, int levelHeight, string totalMoves, string movesMadeJson)
    {
        Debug.Log("Post Coroutine started");
        WWWForm form = new WWWForm();
        form.AddField("entry.1379325124", sessionID);
        form.AddField("entry.589367142", timePlayed);
        form.AddField("entry.1700549212", levelName);
        form.AddField("entry.846791691", levelWidth);
        form.AddField("entry.1378155138", levelHeight);
        form.AddField("entry.1367608849", totalMoves);
        form.AddField("entry.812093032", movesMadeJson);
        // TODO: form.AddField("entry.1687230251", circleMovesMade); // TODO: Add these for the rest of week 7's analytics.
        // TODO: form.AddField("entry.1816303285", diamondMovesMade); // TODO: Add these for the rest of week 7's analytics.

        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload done");
            }

        }
    }
}

