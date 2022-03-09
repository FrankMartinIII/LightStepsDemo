using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSave : MonoBehaviour
{
    private PlayerControllerInput tempcontrols;

    void Awake()
    {
        tempcontrols = new PlayerControllerInput();
    }


    protected void OnEnable()
    {
        tempcontrols.PlayerControls.Save.performed += SaveGame;
        tempcontrols.PlayerControls.Save.Enable();
        tempcontrols.PlayerControls.Load.performed += LoadGame;
        tempcontrols.PlayerControls.Load.Enable();
    }

    private void SaveGame(InputAction.CallbackContext ctx)
    {
        ES3AutoSaveMgr.Current.Save();
    }

    private void LoadGame(InputAction.CallbackContext ctx)
    {
        ES3AutoSaveMgr.Current.Load();
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
