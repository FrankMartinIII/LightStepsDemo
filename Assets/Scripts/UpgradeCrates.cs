using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCrates : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController p = collision.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            //p.hasCrate = true;
            gameObject.SetActive(false);
        }
    }
}
