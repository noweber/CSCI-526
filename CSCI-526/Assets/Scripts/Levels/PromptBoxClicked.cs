using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptBoxClicked : MonoBehaviour
{
    [SerializeField] private GameObject promptObject;

    private void OnMouseDown()
    {
        Debug.Log("CLICKED");
        promptObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
