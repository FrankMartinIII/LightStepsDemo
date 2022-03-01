using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : PowerSource
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            isActive = true; //Apply power and the destruct
            foreach (PowerableObject o in linkedObjects)
            {
                o.receivePower();
                Debug.Log("Sending power to: " + o.gameObject.name);
            }
            gameObject.SetActive(false);
        }
    }
}
