using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blip : MonoBehaviour
{
    public Transform target; //What the blip is following

    MiniMapManager miniMap;
    RectTransform rectTrans;

    float minScale = 1f;

    bool lockScale = false;

    void Start()
    {
        miniMap = GetComponentInParent<MiniMapManager>();
        rectTrans = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        Vector2 newPosition = miniMap.TransformPosition(target.position);

        if (!lockScale)
        {
            float scale = Mathf.Max(minScale, miniMap.zoomLevel);
            rectTrans.localScale = new Vector3(scale, scale, 1);
        }

        rectTrans.localPosition = newPosition;
    }
}
