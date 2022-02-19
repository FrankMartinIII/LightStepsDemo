using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public LevelSegment currentSegment;

    public List<LevelSegment> loadedNeighbors = new List<LevelSegment>();

    public ColorSystem.Colors curColor = ColorSystem.Colors.BLUE;
    private PlayerControllerInput tempcontrols;

    public CameraFollow pCam; //Player camera
    // Start is called before the first frame update
    void Start()
    {
        //changeCurrentSegment(currentSegment);
        pCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>(); //Find the Camera in the scene

    }

    void Awake()
    {
        tempcontrols = new PlayerControllerInput();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeCurrentSegment(LevelSegment newSeg)
    {
        currentSegment = newSeg;
        unloadPrevNeighbors();
        loadNeighbors(currentSegment);
        List<Vector2> bounds = currentSegment.getSegCorners(); //Set camera bounds to follow in the new segment
        pCam.changeCameraBounds(bounds[0], bounds[1]);

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

    


    protected void OnEnable()
    {
        updateSegmentColor();
        tempcontrols.PlayerControls.InputColorRotateForward.performed += colorRotateForward;
        tempcontrols.PlayerControls.InputColorRotateForward.Enable();
        tempcontrols.PlayerControls.InputColorRotateBackward.performed += colorRotateBackward;
        tempcontrols.PlayerControls.InputColorRotateBackward.Enable();
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
}
