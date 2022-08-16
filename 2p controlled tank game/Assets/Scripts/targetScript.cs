using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="128mm shell")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            
        }
    }
}
