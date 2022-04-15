using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeGrapple : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerGrapple p = collision.gameObject.GetComponent<PlayerGrapple>();
        if (p != null)
        {
            p.hasGrapple = true;
            gameObject.SetActive(false);
        }
    }
}
