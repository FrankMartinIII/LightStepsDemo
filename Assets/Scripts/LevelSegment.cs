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
