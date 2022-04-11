using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredLaser : ColoredHazard
{
    public override void OnCollisionStay2D(Collision2D collision)
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
        else if (!player) //If object is not a player, let it pass thru
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), obj.GetComponent<Collider2D>());
        }

    }
}
