using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableObject : ColoredObject
{
    [SerializeField] protected int sourcesPowering = 0;
    public virtual void recievePower()
    {
        sourcesPowering++;
    }

    public virtual void removePower()
    {
        sourcesPowering--;
    }
}
