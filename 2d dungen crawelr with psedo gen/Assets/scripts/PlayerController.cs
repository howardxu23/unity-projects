using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidBody;
    public float speed = 5.0f;
    public float jumpForce = 8.0f;
    public float airControlForce = 10.0f;
    public float airControlMax = 1.5f;
    float playerhitbox = 0f;
    bool grounded = false;
    Animator animator;
    Vector3 boxExtents;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        // get the extent of the collision box
        boxExtents = GetComponent<CapsuleCollider2D>().bounds.extents;
        animator = GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(0,9);
    }
    // Update is called once per frame
    void Update()
    {
        /*
        float blinkVal = Random.Range(0.0f, 200.0f);
        if (blinkVal < 1.0f)
            animator.SetTrigger("blinktrigger");
        */
        if (rigidBody.velocity.x * transform.localScale.x < 0.0f)
        
            transform.localScale = new Vector3(-transform.localScale.x,
            transform.localScale.y, transform.localScale.z);
        
        float xSpeed = Mathf.Abs(rigidBody.velocity.x);
        animator.SetFloat("Xspeed", xSpeed);
        float ySpeed = rigidBody.velocity.y;
        animator.SetFloat("yspeed", ySpeed);

    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        // check if we are on the ground


 
        print(grounded);
        if (grounded)
        {
            if (Input.GetAxis("Jump") > 0.0f)
                rigidBody.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
            else 
                rigidBody.velocity = new Vector2(speed * h, rigidBody.velocity.y);
        }
        else
        {
            // allow a small amount of movement in the air
            float vx = rigidBody.velocity.x;
            if (h * vx < airControlMax)
                rigidBody.AddForce(new Vector2(h * airControlForce, 0));
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.layer == 8)
        {
            grounded = true;
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            grounded = false;
        }
    }
}