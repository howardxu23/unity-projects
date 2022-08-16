using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    public float posX,posY;
    public float MoveSpeed;
    public GameObject player;
    public GameObject Bomb;
    Rigidbody2D rigidBody;
    void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;
        rigidBody=GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(0, 1);
    }

    // Update is called once per frame
    void Update()
    {

        posX = transform.position.x;
        posY = transform.position.y;
        if (Input.GetKey("w"))//go up
        {
            rigidBody.velocity = new Vector3(0, MoveSpeed, 0);
        }
        else if (Input.GetKey("s"))//go down
        {
            rigidBody.velocity = new Vector3(0, -MoveSpeed, 0);
        }
        else if (Input.GetKey("a"))//go left
        {
            rigidBody.velocity = new Vector3(-MoveSpeed,0, 0);
        }
        else if (Input.GetKey("d"))//go right
        {
            rigidBody.velocity = new Vector3(MoveSpeed, 0, 0);
        }
        else if (Input.GetKeyDown("space"))//place bomb
        {
            Vector3 playerpos = new Vector3(posX, posY);
            Instantiate(Bomb, playerpos,Quaternion.identity);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "trophy")
        {
            trophyScript trophy = collision.gameObject.GetComponent<trophyScript>();
            trophy.isTriggered = true;
        }
        
    }
}
