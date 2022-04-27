using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    [SerializeField] GameManager gm;

    [SerializeField] GameObject pauseMenu;

    private PlayerControllerInput ctrls;

    private void Awake()
    {
        ctrls = new PlayerControllerInput();
        GameObject tmp = GameObject.FindGameObjectWithTag("GameManager");
        if (tmp != null)
        {
            gm = tmp.GetComponent<GameManager>();
        }

    }
    void OnEnable()
    {
        ctrls.PlayerControls.PauseGame.performed += HandlePause ;
        ctrls.PlayerControls.PauseGame.Enable();
    }

    private void HandlePause(InputAction.CallbackContext ctx)
    {
        Debug.Log("Pressed PAUSE");
        if (!isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Map()
    {
        gm.OpenCloseMap();
    }

    public void ExitAndSave()
    {
        GameManager.CreateSave();
        Application.Quit();
    }
}
