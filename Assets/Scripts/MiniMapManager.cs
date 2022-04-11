using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{
    public Transform target;

    public float zoomLevel = 10f;

    //Takes a position and returns its relative position for the minimap
    public Vector2 TransformPosition(Vector3 blipPosition)
    {
        Vector3 offset = blipPosition - target.position;
        Vector2 newPosition = new Vector2(offset.x, offset.y);
        newPosition *= zoomLevel;

        return newPosition;
    }
}
