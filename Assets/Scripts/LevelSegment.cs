using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSegment : MonoBehaviour
{
    [SerializeField] protected List<ColoredObject> segmentObjects = new List<ColoredObject>();
    [SerializeField] protected List<LevelSegment> myNeighbors = new List<LevelSegment>();
    public float xPos, yPos; //Hold xpos and ypos of segment in the level (maybe use later for resetSegment()?)
    GameManager gameManager;
    [SerializeField] protected bool hasPlayer = false;
    //These exist to find the boundaries to fit the camera into the segment
    BoxCollider2D segBoundary;
    //public float upperLeftCamBound, lowerRightCamBound;
    [SerializeField] protected List<Vector2> boundaryCorners; //Holds the boundaries

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    // Change this to some load function later, which will be called by game manager
    void Start()
    {
        findColoredObjects();
        toggleObjects(gameManager.curColor); //On initial load of a segment, needs to set the correct color for itself (because this needs to happen after the LevelSegment is initialized

        segBoundary = GetComponent<BoxCollider2D>(); //Get this segment's collider
        boundaryCorners = calculateCameraBounds(); //Find the bounds of the segment, only needs to happen once unless we are going to be changing seg sizes dynamically
        //Debug.Log(boundaryCorners[0] + " " + boundaryCorners[1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void findColoredObjects()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<ColoredObject>() == null)
            {
                Debug.Log("Picked up non ColoredObject");

            }
            else
            {
                segmentObjects.Add(child.GetComponent<ColoredObject>());
            }
        }
    }

    protected List<Vector2> calculateCameraBounds()
    {
        float leftX = transform.position.x + (segBoundary.offset.x - (segBoundary.size.x) / 2) + 5.5f;
        float rightX = transform.position.x + (segBoundary.offset.x + (segBoundary.size.x) / 2) - 5.5f;
        float upY = segBoundary.offset.y + (segBoundary.size.y) - 1;
        float downY = segBoundary.offset.y - (segBoundary.size.y) + 1;

        Vector2 upperLeft = new Vector2(leftX, upY);
        Vector2 lowerRight = new Vector2(rightX, downY);
        List<Vector2> vecList = new List<Vector2>();
        vecList.Add(upperLeft);
        vecList.Add(lowerRight);
        return vecList;
    }

    public List<LevelSegment> getNeighbors()
    {
        return myNeighbors;
    }

    //Return list of corners as calculated above
    public List<Vector2> getSegCorners()
    {
        return boundaryCorners;
    }

    //Function called by GameManager to change colors of stuff
    public void toggleObjects(ColorSystem.Colors gmColor)
    {
        //Debug.Log("toggleObjects to " + gmColor);
        foreach(ColoredObject colObj in segmentObjects)
        {
            if(colObj.GetColor() == ColorSystem.Colors.BLACK)
            {
                //DO NOTHING
            }
            else if(colObj.GetColor() == gmColor)
            {
                colObj.MakeVisible();
            }
            else if(colObj.GetColor() != gmColor)
            {
                colObj.MakeInvisible();
            }
        }
    }









    //Functions to detect when Player enters or exits a segment. Will alert GameManager of Player's position.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            Debug.Log("Player entered segment, telling GameManager");
            /*foreach(LevelSegment n in myNeighbors)
            {
                Debug.Log(n.gameObject.name);
            }*/
            gameManager.changeCurrentSegment(this);
            hasPlayer = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            Debug.Log("Player left segment");
            hasPlayer = false;
        }
    }
}
