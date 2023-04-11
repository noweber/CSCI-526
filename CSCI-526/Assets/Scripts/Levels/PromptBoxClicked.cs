using UnityEngine;

public class PromptBoxClicked : MonoBehaviour
{
    [SerializeField] private GameObject promptObject;

    private void OnMouseDown()
    {
        Debug.Log("CLICKED");
        promptObject.SetActive(false);
    }
}
