using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public List<string> sceneNames;

    public void LoadSelectedScene(int inputIndex)
    {
        SceneManager.LoadScene(sceneNames[inputIndex]);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
