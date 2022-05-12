using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabController : MonoBehaviour
{
    public Transform grabDetect;
    public Transform crateHolder;
    public float rayDist;
    public Transform level;
    private GameManager gm;

    
    void Start(){
        gm = GameObject.FindObjectOfType<GameManager>(); 
    }

    void Update()
    {
        RaycastHit2D grabCheck = Physics2D.Raycast(grabDetect.position, Vector2.right * transform.localScale, rayDist);
        
        if (grabCheck.collider != null && grabCheck.collider.tag == "Crate") //if the object is a crate
        {
            GameObject crate = grabCheck.collider.gameObject; //easier reference. note it is NOT of crate class
            
            if (crate.transform.parent != crateHolder) {
                level = grabCheck.collider.gameObject.transform.parent;
            } // this makes sure the crate will stay in the hierarchy

            if ((gm.curColor == crate.GetComponent<Crate>().curColor) && Input.GetKey(KeyCode.O))
            {
                crate.transform.parent = crateHolder;
                crate.transform.position = crateHolder.position;
                crate.GetComponent<Rigidbody2D>().isKinematic = true;
            } // player picks up crate and crate follows them

            else {
                crate.transform.parent = level;
                crate.transform.position = crate.transform.position;
                crate.GetComponent<Rigidbody2D>().isKinematic = false;
            } //player drops crate and it returns to the scene
        }
    }
}
