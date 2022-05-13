using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : PhysicsObject
{
    private HealthBar healthBar;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    //animation
    public Animator animator;


    private PlayerControllerInput controls;
    private PlayerGrapple grappleScript;
    //private PlayerCrates crates;

    Vector2 p_move = Vector2.zero;

    protected GameManager gm; //Reference to the GameManager in the scene

    //Upgrade stuff
    [SerializeField] public bool hasDoubleJump = false;


    [SerializeField] int maxPlayerHealth = 100;
    [SerializeField] int currPlayerHealth;
    bool isDead = false;
    
    protected void Awake()
    {
        grappleScript = gameObject.GetComponent<PlayerGrapple>();
        controls = new PlayerControllerInput();
        currPlayerHealth = maxPlayerHealth;
        gm = GameObject.FindObjectOfType<GameManager>(); 
        if(gm == null)
        {
            Debug.LogError("Player cannot find GameManager. Make sure one is present in the scene.");
        }
    }

    protected new void Start()
    {
        base.Start();
        healthBar = FindObjectOfType<HealthBar>();
        if(healthBar == null)
        {
            Debug.LogError("Player cannot find HealthBar. Make sure one is present in the scene.");
        }
        else
        {
            healthBar.SetSize(currPlayerHealth / maxPlayerHealth);
        }
    }

    public void changePlayerHealth(int amount)
    {
        currPlayerHealth = currPlayerHealth + amount;
        Debug.Log("Player health modified by " + amount);
        
        if(currPlayerHealth <= 0)
        {
            isDead = true;
            gm.playerDied();
            //gameObject.SetActive(false);
            grappleScript.Reset();
            transform.parent.gameObject.SetActive(false);
        }

        if(currPlayerHealth >= maxPlayerHealth)
        {
            Debug.Log("Player exceeded max health, setting current health to max");
            currPlayerHealth = maxPlayerHealth;
        }

        if (healthBar != null)
        {
            healthBar.SetSize((float)currPlayerHealth / maxPlayerHealth);
        }
    }










    
    protected new void OnEnable() //add corresponding OnDisable
    {
        base.OnEnable();
        controls.PlayerControls.Move.performed += HandleMove;
        controls.PlayerControls.Move.canceled += ctx =>  p_move = Vector2.zero;

        controls.PlayerControls.Jump.performed += HandleJump;
        controls.PlayerControls.Jump.canceled += CutJump;
        controls.PlayerControls.Move.Enable();
        controls.PlayerControls.Jump.Enable();
    }

    private void CutJump(InputAction.CallbackContext obj)
    {
        if (velocity.y > 0)
        {
            velocity.y = velocity.y * 0.5f;
        }
    }

    private void HandleJump(InputAction.CallbackContext obj)
    {
        //Debug.Log("Jump pressed");

        if(isGrounded == true || (hasDoubleJump && jumpsPerformed == 1)) //Check if jumping now is valid
        {
            velocity.y = jumpTakeOffSpeed;
            jumpsPerformed += 1;
            //animate jump
            StartCoroutine(AnimateJump());
        }
        
    }
    
    private IEnumerator AnimateJump()
    {
        yield return new WaitForSeconds(0.01f);
        animator.SetBool("Jumping", true);
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        p_move = context.ReadValue<Vector2>();
        
    }
    
    
    protected override void ComputeVelocity()
    {
        //Debug.Log("move axis " + p_move);
        //
        Vector2 move = Vector2.zero;
        //move.x = p_move.x;

        targetVelocity = p_move * maxSpeed;
        //p_move = Vector2.zero;

        //animation
        animator.SetFloat("Walking", Mathf.Abs(p_move.x) );
        transform = gameObject.GetComponent<Transform>();
        
        if (p_move.x < 0)
        {
            //if moving in -x direction, face left
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (p_move.x > 0) 
        {
            //if moving in +x direction, face right
            transform.localScale = new Vector3(1, 1, 1);
        }
        else 
        {
            //if xvelocity = 0 then don't change current x orientation.
        }
        
        if (isGrounded)
        {
            //when player is grounded.
            animator.SetBool("Jumping", false);
        }
        
    }

    public bool GetDeathStatus()
    {
        return isDead;
    }

    public void CalledOnReloadRespawn() //Called from GameManager when a save is reloaded.
    {
        //Debug.Log("Called on res");
        currPlayerHealth = maxPlayerHealth;
        if (healthBar != null)
        {
            healthBar.SetSize((float)currPlayerHealth / maxPlayerHealth);
        }
    }
    

}
