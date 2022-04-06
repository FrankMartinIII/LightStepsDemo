using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredHazard : ColoredObject
{
    public int damageAmount = 10;
    public float knockbackForce = 1.0f;
    public bool recentlyDamagedPlayer = false;

    public float damageTimer = 1f;

    public new void Start()
    {
        gameObject.layer = 2;
    }
    public virtual void OnCollisionStay2D(Collision2D collision)
    {
        
        GameObject obj = collision.gameObject;
        PlayerController player = getPlayerController(obj);
        if (player != null && (recentlyDamagedPlayer == false))
        {
            recentlyDamagedPlayer = true;
            StartCoroutine(playerDamageTimer());
            player.changePlayerHealth(-damageAmount);
            //knockback(obj);
        }

    }


    protected PlayerController getPlayerController(GameObject obj)
    {
        PlayerController p;
        if(p = obj.GetComponent<PlayerController>())
        {
            return p;
        }
        else
        {
            return null;
        }
    }

    protected void knockback(GameObject otherObj)
    {
        Vector2 vec = this.transform.position - otherObj.transform.position;
        vec.Normalize();
        Debug.Log(vec);
        Rigidbody2D rb;
        
        if(rb = otherObj.GetComponent<Rigidbody2D>())
        {
            Debug.Log("Knock");
            //Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), otherObj.GetComponent<Collider2D>());
            rb.AddForce(-vec * knockbackForce);
        }
    }

    protected IEnumerator playerDamageTimer()
    {

        yield return new WaitForSeconds(damageTimer);
        recentlyDamagedPlayer = false;
        Debug.Log("Player can be damaged again");
    }
}
