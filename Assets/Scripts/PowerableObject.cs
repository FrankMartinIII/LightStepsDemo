using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableObject : ColoredObject
{
    [SerializeField] protected int sourcesPowering = 0;
    public virtual void receivePower()
    {
        //Debug.Log("Received power");
        sourcesPowering++;
    }

    public virtual void removePower()
    {
        //Debug.Log("Lost power");
        sourcesPowering--;
    }
}
