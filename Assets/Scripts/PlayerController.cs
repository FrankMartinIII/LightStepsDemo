using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : PhysicsObject
{

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private PlayerControllerInput controls;


    Vector2 p_move = Vector2.zero;



    //Upgrade stuff
    [SerializeField] bool hasDoubleJump = false;


    [SerializeField] int maxPlayerHealth = 100;
    [SerializeField] int currPlayerHealth;
    private bool isDead = false;
    
    protected void Awake()
    {
        controls = new PlayerControllerInput();
        currPlayerHealth = maxPlayerHealth;
    }

    public void changePlayerHealth(int amount)
    {
        currPlayerHealth = currPlayerHealth + amount;
        Debug.Log("Player health modified by " + amount);
        if(currPlayerHealth <= 0)
        {
            isDead = true;
            gameObject.SetActive(false);
        }

        if(currPlayerHealth >= maxPlayerHealth)
        {
            Debug.Log("Player exceeded max health, setting current health to max");
            currPlayerHealth = maxPlayerHealth;
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
        Debug.Log("Jump pressed");

        if(isGrounded == true || (hasDoubleJump && jumpsPerformed == 0)) //Check if jumping now is valid
        {
            velocity.y = jumpTakeOffSpeed;
            jumpsPerformed += 1;
        }
        
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
    }
    

}
