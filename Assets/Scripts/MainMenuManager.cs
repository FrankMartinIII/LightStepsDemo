using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string sceneToLoad;

    public GameObject initializerObject;
    public void StartNewGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ContinuePrevGame()
    {
        Instantiate(initializerObject);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
