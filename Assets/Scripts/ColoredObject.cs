using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColoredObject : MonoBehaviour
{

    //public enum Colors { BLACK, RED, BLUE, YELLOW, PURPLE, GREEN, ORANGE};

    [SerializeField] ColorSystem.Colors curColor = ColorSystem.Colors.BLACK;

    bool isVisible = true;

    [SerializeField]
    Sprite onSprite;

    [SerializeField]
    Sprite offSprite;

    Collider2D thisCollider;
    SpriteRenderer spriteRenderer;

    
    bool isDestructible = false;

    // Start is called before the first frame update
    void Awake()
    {
        thisCollider = gameObject.GetComponent<Collider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //Awake2();
    }


    public void ChangeColor(ColorSystem.Colors newColor)
    {
        curColor = newColor;
    }

    public ColorSystem.Colors GetColor()
    {
        return curColor;
    }

    public void MakeVisible()
    {
        if (curColor != ColorSystem.Colors.BLACK)
        {
            isVisible = true;
            //Have to implement rest of logic here. What does it mean to be invisible? Outline shown? No collision?
            thisCollider.enabled = true;
            spriteRenderer.sprite = onSprite;
        }
    }


    public void MakeInvisible()
    {
        if (curColor != ColorSystem.Colors.BLACK)
        {
            isVisible = false;
            //Same deal here
            thisCollider.enabled = false;
            spriteRenderer.sprite = offSprite;
        }
    }
    /*
    private PlayerControllerInput tempcontrols;
    //TEMP CODE
    private void Awake2()
    {
        tempcontrols = new PlayerControllerInput();
    }
    protected void OnEnable()
    {
        tempcontrols.PlayerControls.ChangeColor.performed += ColorOnOff;
        tempcontrols.PlayerControls.ChangeColor.Enable();
    }

    private void ColorOnOff(InputAction.CallbackContext ctx)
    {
        Debug.Log("Color swap");
        if(isVisible)
        {
            MakeInvisible();
        }
        else
        {
            MakeVisible();
        }
    }
    */
}
