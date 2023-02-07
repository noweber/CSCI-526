using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Analytics : MonoBehaviour
{   

    [SerializeField] private string URL;
    private long _sessionID;
    private int _totalMoves;
    public static Analytics Instance;
    public int flag=0;

    public Analytics()
    {
        Instance = this;
    }

    public void Send(string sessionID,int totalmovesfromGMC) 
    {   
        Debug.Log("Analytics Send method started");
        //_sessionID = Random.Range(15000,100000);
        //_totalMoves = Random.Range(1,40);

        StartCoroutine(Post( sessionID, totalmovesfromGMC.ToString()));
    }

    private IEnumerator Post(string sessionID, string totalMoves)
    {   
        Debug.Log("Post Coroutine started");
        WWWForm form = new WWWForm();
        form.AddField("entry.1379325124", sessionID);
        form.AddField("entry.1367608849", totalMoves);
        
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
// 
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
// Google FORM URL "https://docs.google.com/forms/u/1/d/e/1FAIpQLScbdeMbYwWKzLKgAWeWEHL94ruFqymF66zYgzN2pEjgAFcrCg/formResponse" 

