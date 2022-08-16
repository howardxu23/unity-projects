using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BulletScript : MonoBehaviourPun
{
    public int speed = 50;          // The speed our bullet travels
    public Vector3 targetDirection;    // the direction it travels
    public float lifetime = 10f;     // how long it lives before destroying itself
    public int damage = 10;       // how much damage this projectile causes
    Rigidbody2D Rigidbody;
    void Start()
    {
        // find our RigidBody
        Rigidbody = gameObject.GetComponentInChildren<Rigidbody2D>();
        // add force 
        //rb.AddForce(targetVector.normalized * speed);
    }


    // Update is called once per frame
    void Update()
    {
        // decrease our life timer
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            // we have ran out of life
            Destroy(gameObject);    // kill bullet
        }
        targetDirection = new Vector3(transform.up.x, transform.up.y);
        Rigidbody.velocity = targetDirection * speed * Time.deltaTime;
    }

}
