using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Scroller : MonoBehaviour
{
    public float offsetFactor=0.2f;
    float startPosX;
    // Use this for initialization
    void Start()
    {
        startPosX = transform.parent.position.x;
    }
    // Update is called once per frame
    void Update()
    {
        float xPos = transform.parent.position.x - startPosX;
        float texOffset = xPos * offsetFactor;
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", new
       Vector2(texOffset, 0.0f));
    }
}
