using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureScroller : MonoBehaviour
{
    public float offsetFactorX = 1.76f;
    public float offsetFactorY = 1f;
    float startPosX;
    float texOffsetX = 0;
    float texOffsetY = 0;
    // Use this for initialization
    void Start()
    {
        startPosX = transform.parent.position.x;
    }
    // Update is called once per frame


    public void FixedUpdate()
    {

        texOffsetX += offsetFactorX / 1000;
        texOffsetY += offsetFactorY / 1000;
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", new Vector2(texOffsetX, texOffsetY));
    }

}
