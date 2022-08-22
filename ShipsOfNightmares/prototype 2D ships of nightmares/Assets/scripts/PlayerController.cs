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
    public GameObject swordHitbox;
    [SerializeField]
    [Tooltip("hitbox is what interactes with munitions and weapons, rather then coliding with enviroment objects.")]
    private GameObject playerCollison;
    [SerializeField]
    [Tooltip("how long the sword hitbox should linger in seconds")]
    private float attackLingerDuration = 0.25f;
    //float playerhitbox = 0f;// Necessary? ~SK
    Animator animator;
    Vector3 boxExtents;
    // Use this for initialization
    void Start()
    {

        rigidBody = GetComponent<Rigidbody2D>();
        // get the extent of the collision box
        boxExtents = GetComponent<BoxCollider2D>().bounds.extents;
        animator = GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(7, 8);
        Physics2D.IgnoreLayerCollision(6, 9);
        Physics2D.IgnoreLayerCollision(7, 9);
        Physics2D.IgnoreLayerCollision(6, 7);
    }
    // Update is called once per frame
    void Update()
    {
        if (rigidBody.velocity.x * transform.localScale.x < 0.0f)
        
            transform.localScale = new Vector3(-transform.localScale.x,
            transform.localScale.y, transform.localScale.z);
        
        float xSpeed = Mathf.Abs(rigidBody.velocity.x);
        //animator.SetFloat("Xspeed", xSpeed);
        float ySpeed = Mathf.Abs(rigidBody.velocity.y);
        //animator.SetFloat("yspeed", ySpeed);

        if (xSpeed > 0 || ySpeed > 0)// If player is moving... ~ SK
        {
            animator.SetBool("Movement", true);
        }
        else
        {
            animator.SetBool("Movement", false);
        }

        bool attack = Input.GetMouseButtonDown(0);//LMB pressed?
        //if pressed animate attack animation and enable attack hitbox

        animator.SetBool("Sword", false);// Set to false (assumes player didn't attack). ~ SK
        if (attack==true)
        {
            swordAttackHitboxDuration(attackLingerDuration);

            animator.SetBool("Sword", true);// Play sword attack animation. ~ SK
        }

    }
    void swordAttackHitboxDuration(float duration)
    {
        StartCoroutine(doswordAttackHitboxDuration( duration));
    }
    IEnumerator doswordAttackHitboxDuration(float duration)
    {
        swordHitbox.SetActive(true);
        yield return new WaitForSeconds(duration);
        swordHitbox.SetActive(false);
    }
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        /*
        // check if we are on the ground
        Vector2 bottom = new Vector2(transform.position.x, transform.position.y - boxExtents.y);

        Vector2 hitBoxSize = new Vector2(boxExtents.x * 2.0f, 0.05f);

        RaycastHit2D result = Physics2D.BoxCast(bottom, hitBoxSize, 0.0f, new Vector3(0.0f, -1.0f), 0.0f, 1 << LayerMask.NameToLayer("Ground"));

        bool grounded = result.collider != null && result.normal.y > 0.9f;
        */
        /*
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
        }*/
        rigidBody.velocity = new Vector2(speed * h, speed*v);//moves the player

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Ouch!");
        //Destroy(gameObject);
        //gameObject.SetActive(false);// Less intense than Destroy(gameObject); ~ SK
    }
}