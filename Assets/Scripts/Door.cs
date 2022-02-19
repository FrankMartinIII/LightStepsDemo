using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : PowerableObject
{
    [SerializeField] GameObject closedDoor;
    public int sourcesNeededToOpen;

    public bool startingStateIsLocked = true;
    public bool isLocked;
    /*
    {
        get
        {
            return isLocked;
        }
        set
        {
            isLocked = value;
            
            if(isLocked)
            {
                closedDoor.SetActive(true);
            }
            else
            {
                closedDoor.SetActive(false);
            }
            
        }
    }*/

    void Start()
    {
        isLocked = startingStateIsLocked;
    }

    // Update is called once per frame
    void Update()
    {
        if(isLocked)
        {
            if(sourcesPowering == sourcesNeededToOpen)
            {
                isLocked = false;
                closedDoor.SetActive(false);
            }
        }
    }
}
