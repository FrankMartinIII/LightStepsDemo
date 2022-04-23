using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public LevelSegment currentSegment;

    public List<LevelSegment> loadedNeighbors = new List<LevelSegment>();

    public ColorSystem.Colors curColor = ColorSystem.Colors.BLUE;
    private PlayerControllerInput tempcontrols;

    public CameraFollow pCam; //Player camera

    protected TMP_Text centerTextBox;
    protected GameObject uiMap;


    public bool loadGameOnStart = false;
    // Start is called before the first frame update
    void Start()
    {
        //changeCurrentSegment(currentSegment);
        pCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>(); //Find the Camera in the scene
        centerTextBox = GameObject.FindGameObjectWithTag("CenterTextBoxTMP").GetComponent<TMP_Text>();
        uiMap = GameObject.FindObjectOfType<MiniMapManager>().gameObject;
        uiMap.SetActive(false);
    }

    void Awake()
    {
        
        InitializerScript init = null;
        if (GameObject.FindGameObjectWithTag("Initializer") != null)
        {
            init = GameObject.FindGameObjectWithTag("Initializer").GetComponent<InitializerScript>();
        }
        if(init != null)
        {
            loadGameOnStart = init.loadGame; //Find the initializer and if it is telling us to load a game, we will set our flag to load the save
            //GameObject player = GameObject.FindGameObjectWithTag("Player");
            Destroy(init.gameObject); //Uncomment this line after testing
        }
        
        tempcontrols = new PlayerControllerInput();
    }

    // Update is called once per frame
    void Update()
    {
        if(loadGameOnStart)
        {
            loadGameOnStart = false;
            LoadSave();
        }
    }

    public void changeCurrentSegment(LevelSegment newSeg)
    {
        currentSegment = newSeg;
        unloadPrevNeighbors();
        loadNeighbors(currentSegment);
        List<Vector2> bounds = currentSegment.getSegCorners(); //Set camera bounds to follow in the new segment
        pCam.changeCameraBounds(bounds[0], bounds[1]);
        if (loadGameOnStart == false)
        {
            CreateSave();
        }

    }

    public void unloadPrevNeighbors()
    {
        foreach(LevelSegment n in loadedNeighbors)
        {
            Debug.Log("curr seg is: " + currentSegment);
            if (n != currentSegment)
            {
                n.gameObject.SetActive(false);
                Debug.Log(n.gameObject.name + " unloaded");
            }
            
        }
        loadedNeighbors.Clear();
    }

    public void loadNeighbors(LevelSegment seg)
    {
        List<LevelSegment> neighbors = seg.getNeighbors();
        foreach(LevelSegment n in neighbors)
        {
            Vector3 vec = new Vector3(n.xPos, n.yPos, 0);
            n.gameObject.SetActive(true);
            n.toggleObjects(curColor); //Set newly loaded segments to the correct color
            loadedNeighbors.Add(n);
            Debug.Log(n.gameObject.name + " loaded");
        }
    }

    public void playerDied()
    {
        if(centerTextBox != null)
        {
            centerTextBox.text = "SYSTEMS DAMAGED";
        }

        StartCoroutine("respawnOnTimer");

    }

    IEnumerator respawnOnTimer()
    {
        yield return new WaitForSeconds(3f);
        centerTextBox.text = "";
        if(ES3.FileExists("SaveFile.es3")) //Check if a save exists. If it does, load it.
        {
            Debug.Log("Save found");
            ES3AutoSaveMgr.Current.Load();
        }
        else
        {
            Debug.LogError("NO PREVIOUS SAVE");
        }
    }


    


    protected void OnEnable()
    {
        updateSegmentColor();
        tempcontrols.PlayerControls.InputColorRotateForward.performed += colorRotateForward;
        tempcontrols.PlayerControls.InputColorRotateForward.Enable();
        tempcontrols.PlayerControls.InputColorRotateBackward.performed += colorRotateBackward;
        tempcontrols.PlayerControls.InputColorRotateBackward.Enable();
        tempcontrols.PlayerControls.Save.performed += SaveGame;
        tempcontrols.PlayerControls.Save.Enable();
        tempcontrols.PlayerControls.Load.performed += LoadGame;
        tempcontrols.PlayerControls.Load.Enable();
        tempcontrols.PlayerControls.Map.performed += OpenCloseMap;
        tempcontrols.PlayerControls.Map.Enable();
    }

    private void colorRotateForward(InputAction.CallbackContext ctx)
    {
        //Debug.Log("CHANGE COLOR");
        //Cast color to int for math
        int col = (int)curColor;
        col = ((col + 1) % 3) + 1; //Should go 1, 2, 3, 1, 2, 3
        //Cast back to a color
        curColor = (ColorSystem.Colors)col;
        Debug.Log("CHANGE COLOR " + curColor);
        updateSegmentColor();
    }

    private void colorRotateBackward(InputAction.CallbackContext ctx)
    {
        int col = (int)curColor;
        col = col + 1;
        if(col > 3) //Yellow (3) is the last color
        {
            col = 1; 
        }
        //Cast back to a color
        curColor = (ColorSystem.Colors)col;
        Debug.Log("CHANGE2 COLOR " + curColor);
        updateSegmentColor();
    }


    private void updateSegmentColor()
    {
        //Alert all loaded segments that they must update their color
        currentSegment.toggleObjects(curColor);
        foreach(LevelSegment seg in loadedNeighbors)
        {
            seg.toggleObjects(curColor);
        }
    }

    private void OpenCloseMap(InputAction.CallbackContext ctx)
    {
        if(uiMap.activeSelf) //If map is active, toggle off
        {
            uiMap.SetActive(false);
        }
        else
        {
            uiMap.SetActive(true);
        }
    }

    private void SaveGame(InputAction.CallbackContext ctx)
    {
        CreateSave();
    }

    private void CreateSave()
    {
        Debug.Log("saving");
        ES3AutoSaveMgr.Current.Save();
    }

    private void LoadGame(InputAction.CallbackContext ctx)
    {
        Scene curScene = SceneManager.GetActiveScene();
        string sceneName = curScene.name;
        //SceneManager.LoadScene(sceneName);
        LoadSave();
    }


    private void LoadSave()
    {
        Debug.Log("reloading previous save");
        ES3AutoSaveMgr.Current.Load();
    }


}
