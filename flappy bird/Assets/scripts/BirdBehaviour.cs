using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
    public float gravity;
    public float clickImpulse;
    public Sprite flapUpSprite;
    public Sprite flapMidSprite;
    public Sprite flapDownSprite;
    float ySpeed;
    float posX, posY, posZ;
    // Use this for initialization
    void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;
        posZ = transform.position.z;
        ySpeed = 0.0f;
    }
    void Flap()
    {
        ySpeed = clickImpulse;
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        transform.parent.GetComponent<SideScroller>().xSpeed = 0;
        transform.parent.Find("gameover").gameObject.SetActive(true);
    }

    int animCount = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
            Flap();
        // animation
        animCount = animCount + 1;
        if (ySpeed <= 0.0f)
            animCount = 2;
        if (animCount == 4)
            animCount = 0;
        if (animCount == 1)
            GetComponent<SpriteRenderer>().sprite = flapUpSprite;
        else if (animCount == 3)
            GetComponent<SpriteRenderer>().sprite = flapDownSprite;
        else
            GetComponent<SpriteRenderer>().sprite = flapMidSprite;
        float angle = 9.0f * ySpeed;
        if (angle > 30.0f)
            angle = 30.0f;
        if (angle < -30.0f)
            angle = -30.0f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    void FixedUpdate()
    {
        ySpeed -= gravity * Time.fixedDeltaTime;
        posY += ySpeed * Time.fixedDeltaTime;
        transform.localPosition = new Vector3(posX, posY, posZ);
    }
}
