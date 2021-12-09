using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : ColoredObject
{

    protected bool isAffectedByGravity = true;

    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f; //How hard does gravity affect this object. Probably dont change.

    protected Vector2 targetVelocity;
    protected bool isGrounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2D;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter; //What colliders will the physics collide with
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float MINMOVEDISTANCE = 0.001f;
    protected const float SHELLRADIUS = 0.01f; //Buffer to make sure object cant get stuck inside of another. May have to come up with a different solution for if objects switch color while Player is inside of them

    protected new Transform transform;

    protected void Start()
    {
        transform = gameObject.GetComponent<Transform>();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;

        //ChangeGravityDirection("left");
    }

    
    protected virtual void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected void OnEnable()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (isAffectedByGravity) //Downward force applied
        {
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        }

        velocity.x = targetVelocity.x;

        isGrounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);

    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if(distance > MINMOVEDISTANCE)
        {
            int count = rb2D.Cast(move, contactFilter, hitBuffer, distance + SHELLRADIUS); //Check in the direction that we are moving to see if there is a collider in the way.
            hitBufferList.Clear();

            for(int i = 0; i < count; i++) //take raycast2d objects frim hitbuffer and copy into the list
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal; //check normal of each raycast2d and compare to minimum value

                //Is this grounded?

                if (currentNormal.y > minGroundNormalY)
                {
                    isGrounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                //Debug.Log("projection: " + projection);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal; //Cancel out part of velocity that would be stopped by collision 
                }

                float modifiedDistance = hitBufferList[i].distance - SHELLRADIUS;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        rb2D.position = rb2D.position + move.normalized * distance;

    }


    protected virtual void ComputeVelocity()
    {

    }
}
