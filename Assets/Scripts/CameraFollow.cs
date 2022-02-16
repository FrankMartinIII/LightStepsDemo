using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    protected Vector3 startPos;
    public Vector2 upperLeftBound;
    public Vector2 lowerRightBound;

    public float followSpeed;
    private float step;
    public Transform targetTransform; //What we are following
    private Vector3 moveToPosition;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }


    void Update()
    {
        
        moveToPosition.z = transform.position.z;

        if(targetTransform == null)
        {
            Debug.Log("No target. Must acquire player.");
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                targetTransform = player.transform;
            }
        }

        if(targetTransform != null) //We are tracking an object
        {
            if(targetTransform.position.x <= upperLeftBound.x || targetTransform.position.x >= lowerRightBound.x) //Target is out of bounds in the x direction
            {
                if(targetTransform.position.x <= upperLeftBound.x) //Too far left
                {
                    moveToPosition.x = upperLeftBound.x;
                }
                else if(targetTransform.position.x >= lowerRightBound.x) //Too far right
                {
                    moveToPosition.x = lowerRightBound.x;
                }
            }

            else //Otherwise, follow the player
            {
                moveToPosition.x = targetTransform.position.x;
            }

            if (targetTransform.position.y >= upperLeftBound.y || targetTransform.position.y <= lowerRightBound.y)
            {
                if(targetTransform.position.y >= upperLeftBound.y)
                {
                    moveToPosition.y = upperLeftBound.y;

                }
                else if (targetTransform.position.y <= lowerRightBound.y)
                {
                    moveToPosition.y = lowerRightBound.y;
                }

            }
                
            else
            {
                moveToPosition.y = targetTransform.position.y;
            }


                
            moveToPosition.y = targetTransform.position.y;
            step = followSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, moveToPosition, step);
        }

        

    }
    public void changeCameraBounds(Vector2 upperL, Vector2 lowerR)
    {
        upperLeftBound = upperL;
        lowerRightBound = lowerR;
    }
}
