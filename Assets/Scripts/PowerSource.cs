using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : ColoredObject
{
    [SerializeField] List<PowerableObject> linkedObjects;
    public bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            foreach (PowerableObject o in linkedObjects)
            {
                o.recievePower();
            }
        }
    }
}
