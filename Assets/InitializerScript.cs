using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializerScript : MonoBehaviour
{
    public bool loadGame = true;
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}
