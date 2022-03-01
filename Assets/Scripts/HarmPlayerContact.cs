using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmPlayerContact : MonoBehaviour
{
    public int amount;
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collided with player");
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            Debug.Log("Collided with player");
            //Destroy(player.gameObject);
            player.changePlayerHealth(-amount);
        }
    }
    /*
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with player");
    }
    */
}
