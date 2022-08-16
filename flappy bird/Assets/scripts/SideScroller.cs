using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SideScroller : MonoBehaviour
{
    public float xSpeed=2;
    float posX, posY, posZ;
    // Use this for initialization
    void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;
        posZ = transform.position.z;
    }
    void FixedUpdate()
    {
        posX += Time.fixedDeltaTime * xSpeed;
        transform.position = new Vector3(posX, posY, posZ);
    }
}
