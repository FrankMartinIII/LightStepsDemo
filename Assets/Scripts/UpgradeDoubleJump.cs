using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDoubleJump : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController p = collision.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            p.hasDoubleJump = true;
            gameObject.SetActive(false);
        }
    }
}
