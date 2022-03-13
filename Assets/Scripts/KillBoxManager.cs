using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBoxManager : MonoBehaviour
{

    ColoredObject coloredObject;
    public int damageStartAmount;
    int damageAmount = 0;
    public bool recentlyDamagedPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        coloredObject = GetComponentInParent<ColoredObject>();
        damageAmount = 5000;
    }



    public void OnTriggerStay2D(Collider2D collision)
    {

        //Debug.Log("rec" + recentlyDamagedPlayer);
        //Debug.Log("dmg amount " + damageAmount);
        //Debug.Log("Collided with player");
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null && coloredObject.GetCurrentVisibility() && recentlyDamagedPlayer == false)
        {
            recentlyDamagedPlayer = true;
            StartCoroutine("playerDamageTimer");
            //Debug.Log("Damaged player");
            //Destroy(player.gameObject);
            player.changePlayerHealth(-damageAmount);
        }
    }



    IEnumerator playerDamageTimer()
    {
        
        yield return new WaitForSeconds(1f);
        recentlyDamagedPlayer = false;
        //Debug.Log("Player can be damaged again");
    }
    
}
