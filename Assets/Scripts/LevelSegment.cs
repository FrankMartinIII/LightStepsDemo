using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSegment : MonoBehaviour
{
    [SerializeField] protected List<ColoredObject> segmentObjects = new List<ColoredObject>();
    [SerializeField] protected List<LevelSegment> myNeighbors = new List<LevelSegment>();
    GameManager gameManager;
    [SerializeField] protected bool hasPlayer = false;
    public float xPos, yPos;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    // Change this to some load function later, which will be called by game manager
    void Start()
    {
        foreach(Transform child in transform)
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<LevelSegment> getNeighbors()
    {
        return myNeighbors;
    }
    //Function called by GameManager to change colors of stuff
    public void toggleObjects(ColorSystem.Colors gmColor)
    {
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
