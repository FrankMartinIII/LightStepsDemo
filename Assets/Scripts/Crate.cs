using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    //minimized version of colored object.
    public ColorSystem.Colors curColor = ColorSystem.Colors.BLACK;

    public Sprite onSprite;

    public Sprite offSprite;
    
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    void Start()
    {   
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindObjectOfType<GameManager>(); 
    }
    // Update is called once per frame
    void Update()
    {
        if (curColor == gameManager.curColor)
        {
            spriteRenderer.sprite = onSprite;
        }

        else //(curColor != gameManager.curColor)
        {
            spriteRenderer.sprite = offSprite;
        }
    }
}
