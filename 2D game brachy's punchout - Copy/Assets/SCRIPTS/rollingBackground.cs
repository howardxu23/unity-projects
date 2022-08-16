using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class rollingBackground : MonoBehaviour
{
    public float xSpeed=1.76f;
    public float ySpeed=1f;
    float posX, posY;

    // Use this for initialization
    void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;

    }
    // Update is called once per frame
    void Update()
    {
        posX += Time.fixedDeltaTime * xSpeed;
        //posY += Time.fixedDeltaTime * ySpeed;
        transform.position = new Vector2(posX, posY);
    }
}
