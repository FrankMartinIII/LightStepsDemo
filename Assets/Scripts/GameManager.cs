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

    // Start is called before the first frame update
    void Start()
    {
        //changeCurrentSegment(currentSegment);
        
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
            loadedNeighbors.Add(n);
            Debug.Log(n.gameObject.name + " loaded");
        }
    }

    


    protected void OnEnable()
    {
        updateSegmentColor();
        tempcontrols.PlayerControls.InputColorRotateForward.performed += colorRotateForward;
        tempcontrols.PlayerControls.InputColorRotateForward.Enable();
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
