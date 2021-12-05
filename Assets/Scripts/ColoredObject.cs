using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredObject : MonoBehaviour
{

    public enum Colors { BLACK, RED, BLUE, YELLOW, PURPLE, GREEN, ORANGE};

    [SerializeField] Colors curColor = Colors.BLACK;

    bool isVisible;

    [SerializeField]
    Sprite onSprite;

    [SerializeField]
    Sprite offSprite;

    bool isDestructible = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColor(Colors newColor)
    {
        curColor = newColor;
    }

    public Colors GetColor()
    {
        return curColor;
    }

    public void MakeVisible()
    {
        isVisible = true;
        //Have to implement rest of logic here. What does it mean to be invisible? Outline shown? No collision?
    }


    public void MakeInvisible()
    {
        isVisible = false;

        //Same deal here
    }

}
