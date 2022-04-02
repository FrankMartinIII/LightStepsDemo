using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrapple : MonoBehaviour
{
    [SerializeField] protected bool hasGrapple; //Whether the player has the ability to grapple or not, can use this for the upgrade
    [SerializeField] int launchDistance = 100000; //Length of grappling hook
    [SerializeField] float grappleSpeed = 1f;
    [SerializeField] float pullSpeed = 3f;
    [SerializeField] float abortPullTimer = 2f; //How long an object will be pulled towards the player on successful grapple until it gives up (should stop objects from getting stuck)

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
        while(Vector2.Distance(this.transform.position, end) > (speed * Time.deltaTime))
        {
            if(forceGrappleStop)
            {
                yield return 0;
                break;
            }
            grappling = true;
            this.transform.position = Vector2.MoveTowards(this.transform.position, end, speed * Time.deltaTime); //Move the player towards the point
            yield return 0;
        }
        grappling = false;
        forceGrappleStop = false;
        hitObj = null;
        //if(Vector2.Distance(this.transform.position, end) > 2)

        //this.transform.position = end;
    }

    protected IEnumerator PullObj(GameObject otherObj, float speed)
    {
        Debug.Log("Moving object towards player");
        StartCoroutine(PullSafetyTimer()); //This coroutine is to forcibly end the pull if the object cant reach the player after a certain amount of time.
        while (Vector2.Distance(otherObj.transform.position, this.transform.position) > (speed * Time.deltaTime))
        {
            if (forceGrappleStop)
            {
                yield return 0;
                break;
            }
            grapplingPull = true;
            otherObj.transform.position = Vector2.MoveTowards(otherObj.transform.position, this.transform.position, speed * Time.deltaTime);
            yield return 0;
        }

        grapplingPull = false;
        forceGrappleStop = false;
        hitObj = null;
        StopCoroutine(PullSafetyTimer()); //Stop the timer coroutine so that we cant accidentally stop the next pull action.
    }
    
    protected IEnumerator PullSafetyTimer()
    {
        yield return new WaitForSeconds(abortPullTimer);
        forceGrappleStop = true;
    }

}
