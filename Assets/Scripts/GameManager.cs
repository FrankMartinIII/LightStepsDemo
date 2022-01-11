using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelSegment currentSegment;

    public List<LevelSegment> loadedNeighbors = new List<LevelSegment>();
    // Start is called before the first frame update
    void Start()
    {
        //changeCurrentSegment(currentSegment);
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
}
