using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerGrapple : MonoBehaviour
{
    [SerializeField] public bool hasGrapple; //Whether the player has the ability to grapple or not, can use this for the upgrade
    [SerializeField] int launchDistance = 100000; //Length of grappling hook
    [SerializeField] float grappleSpeed = 1f;
    [SerializeField] float pullSpeed = 3f;
    [SerializeField] float abortPullTimer = 2f; //How long an object will be pulled towards the player on successful grapple until it gives up (should stop objects from getting stuck)
    [SerializeField] LineRenderer lr;
    [SerializeField] GameObject hand; //Hand prefab to place
    private GameObject placedHand; //The hand currently in the pull animation (if one exists)

    private PlayerController playerController;

    Vector2 launchDirection;

    private PlayerControllerInput controls;

    private Rigidbody2D rb;
    [SerializeField] private bool grappling = false;
    [SerializeField] private bool forceGrappleStop = false;
    [SerializeField] private bool grapplingPull = false;

    private GameObject hitObj;

    protected void Awake()
    {
        controls = new PlayerControllerInput();
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerController = gameObject.GetComponent<PlayerController>();
    }

    protected void OnEnable() //add corresponding OnDisable
    {
        controls.PlayerControls.Move.performed += ctx => launchDirection = ctx.ReadValue<Vector2>(); //Get last direction moved by the player
        controls.PlayerControls.Grapple.performed += LaunchGrapple;

        controls.PlayerControls.Move.Enable();
        controls.PlayerControls.Grapple.Enable();
    }


    protected void LaunchGrapple(InputAction.CallbackContext ctx)
    {
        if ((hasGrapple) && (grappling == false) && (grapplingPull == false)) //Grapple can only occur if the player has the grapple upgrade and is not already grappling
        {
            forceGrappleStop = false;
            Debug.DrawRay(transform.position, launchDirection, Color.red, 5); //Debug ray to show grapple
            int layermask = LayerMask.GetMask("Player", "Ignore Raycast"); //This layermask will make the raycast only hit the player
            layermask = ~layermask; //Invert the layermask to hit everything besides the player
            Debug.Log("Raycast launched");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, launchDirection, Mathf.Infinity, layermask); //Launch the raycast outward from the front of the player

            if (hit.collider != null) //If the raycast hit something
            {
                hitObj = hit.collider.gameObject;
                Debug.Log("Raycast hit obj: " + hitObj.name);

                //If we hit a GrapplePoint
                if (hitObj.tag == "GrapplePoint")
                {
                    Debug.Log("Hit grapple point");
                    //rb.MovePosition(hitObj.transform.position);
                    StartCoroutine(GrappleTo(hitObj.transform.position, grappleSpeed)); //Coroutine to pull the player to the grapple point
                }

                else if(hitObj.tag == "Crate") //If we hit a crate
                {
                    Debug.Log("Hit crate");
                    StartCoroutine(PullObj(hitObj, pullSpeed)); //Coroutine to pull an object to the player
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (grappling == true) //If the player is mid grapple and hits something, then force the grapple to stop
        {
            GameObject other = collision.gameObject;
            Debug.Log("Player collided with something");

            if(other.GetComponent<TilemapCollider2D>() != null)
            {
                return;
            }
            forceGrappleStop = true;

        }

        else if(grapplingPull == true) //If the player is pulling the object and it hits them, force the grapple to stop
        {
            GameObject other = collision.gameObject;
            Debug.Log("Player collided with something");

            if(other == hitObj)
            {
                forceGrappleStop = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //I added a trigger to the GrapplePoint gameobject that is larger than the point itself.
        //This is to stop the player from trying to go into the point. We stop the player when they hit the trigger box.
        if (grappling == true) 
        {
            GameObject other = collision.gameObject;
            if ((hitObj != null) && (other.transform.parent == hitObj.transform)) 
            {
                Debug.Log("Player hit grappleTrigger");
                forceGrappleStop = true;
            }

        }
    }

    protected IEnumerator GrappleTo(Vector2 end, float speed)
    {
        Debug.Log("Moving player towards point");
        placedHand = CreateHand(this.transform.position, end);
        while(Vector2.Distance(this.transform.position, end) > (speed * Time.deltaTime))
        {
            lr.SetPosition(0, end);
            lr.SetPosition(1, this.transform.position);
            if(forceGrappleStop)
            {
                yield return 0;
                break;
            }
            grappling = true;
            this.transform.position = Vector2.MoveTowards(this.transform.position, end, speed * Time.deltaTime); //Move the player towards the point
            yield return 0;
        }
        lr.SetPosition(1, lr.GetPosition(0));
        Destroy(placedHand);
        grappling = false;
        forceGrappleStop = false;
        hitObj = null;
        //if(Vector2.Distance(this.transform.position, end) > 2)

        //this.transform.position = end;
    }

    protected IEnumerator PullObj(GameObject otherObj, float speed)
    {
        Debug.Log("Moving object towards player");
        placedHand = CreateHand(this.transform.position, otherObj.transform.position); //Create a new hand sprite
        StopCoroutine(PullSafetyTimer());
        Coroutine lastRoutine = StartCoroutine(PullSafetyTimer()); //This coroutine is to forcibly end the pull if the object cant reach the player after a certain amount of time.
        while (Vector2.Distance(otherObj.transform.position, this.transform.position) > (speed * Time.deltaTime))
        {
            lr.SetPosition(0, this.transform.position);
            lr.SetPosition(1, otherObj.transform.position); //Change the position of the line renderer's end points for the player's arm
            placedHand.transform.position = otherObj.transform.position; //Move the hand with the object
            if (forceGrappleStop)
            {
                yield return 0;
                break;
            }
            grapplingPull = true;
            otherObj.transform.position = Vector2.MoveTowards(otherObj.transform.position, this.transform.position, speed * Time.deltaTime);
            yield return 0;
        }
        StopCoroutine(lastRoutine); //Stop the timer coroutine so that we cant accidentally stop the next pull action.
        lr.SetPosition(0, lr.GetPosition(1));
        Destroy(placedHand);
        grapplingPull = false;
        forceGrappleStop = false;
        hitObj = null;

    }
    
    protected IEnumerator PullSafetyTimer()
    {
        yield return new WaitForSeconds(abortPullTimer);
        Debug.Log("PullSafetyTimer triggered");
        forceGrappleStop = true;
    }

    public void Reset() //Reset the grapple script (called when player dies to prevent bugs with save)
    {
        grappling = false;
        grapplingPull = false;
        lr.SetPosition(0, lr.GetPosition(1));
        Destroy(placedHand);
    }



    IEnumerator LineDraw() //Not used anywhere, was a test function
    {
        float t = 0;
        float time = 2;
        Vector3 orig = lr.GetPosition(0);
        Vector3 orig2 = lr.GetPosition(1);
        lr.SetPosition(1, orig);
        Vector3 newpos;
        for (; t < time; t += Time.deltaTime)
        {
            newpos = Vector3.Lerp(orig, orig2, t / time);
            lr.SetPosition(1, newpos);
            yield return null;
        }
        lr.SetPosition(1, orig2);
    }

    protected GameObject CreateHand(Vector2 pos1, Vector2 pos2) //Creates a hand rotated to align to the two positions
    {
        float angle = Mathf.Atan2(pos2.x - pos1.x, pos2.y - pos1.y) * Mathf.Rad2Deg;
        Quaternion tmp = new Quaternion();
        GameObject placedHand = Instantiate(hand, pos2, tmp);
        placedHand.transform.Rotate(0, 0, angle);
        return placedHand;
    }

}
