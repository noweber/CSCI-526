using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendToGoogle : MonoBehaviour
{
    // Start is called before the first frame update
    // form URL: https://docs.google.com/forms/u/0/d/e/1FAIpQLSeF7e7MzNdXpgx8imYiHNYQxzpEq7l930acFl8eMzzzBxJc3w/formResponse
    
    [SerializeField] private string URL;
    private long _sessionID;
    private int _testInt;

    public void Send()
    {
        
        _testInt = Random.Range(0, 101);
        
        
        StartCoroutine(Post(_sessionID.ToString(), _testInt.ToString()));
    }
    
    private IEnumerator Post(string sessionID, string testInt)
    {
        // Create the form and enter responses
        WWWForm form = new WWWForm();
        form.AddField("entry.1201901405", sessionID);
        form.AddField("entry.2096555585", testInt);
        
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            } 
        }
    }
    
    private void Awake()
    {
        // Assign sessionID to identify playtests
        _sessionID = System.DateTime.Now.Ticks;

        Send();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
