using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBoxManager : MonoBehaviour
{

    ColoredObject coloredObject;
    public int damageAmount;

    // Start is called before the first frame update
    void Start()
    {
        coloredObject = GetComponentInParent<ColoredObject>();

    }



    public void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("Collided with player");
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null && coloredObject.GetCurrentVisibility())
        {
            Debug.Log("Damaged player");
            //Destroy(player.gameObject);
            player.changePlayerHealth(-damageAmount);
        }
    }
}
