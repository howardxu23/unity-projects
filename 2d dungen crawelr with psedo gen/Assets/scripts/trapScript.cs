using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapScript : MonoBehaviour
{
    Rigidbody2D platformBody;
    public GameObject platform;
    Rigidbody2D spikeBody;
    public bool isTriggered=false;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(0, 9);
        Physics2D.IgnoreLayerCollision(8, 9);
        platformBody =platform.GetComponent<Rigidbody2D>();
        platformBody.bodyType = RigidbodyType2D.Static;

        foreach(Transform child in platform.transform)
        {
            if (child.gameObject.tag == "spike")
            {
                spikeBody = child.gameObject.GetComponent<Rigidbody2D>();
                spikeBody.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (isTriggered == true)
        {
            platformBody.bodyType = RigidbodyType2D.Dynamic;
            foreach (Transform child in platform.transform)
            {
                if (child.gameObject.tag == "spike")
                {
                    spikeBody = child.gameObject.GetComponent<Rigidbody2D>();
                    spikeBody.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
    }
}
