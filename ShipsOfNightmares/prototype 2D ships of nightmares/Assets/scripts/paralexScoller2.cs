using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paralexScoller2 : MonoBehaviour
{
    /*requirements for working correctly:
    have a repeating background, thats is at leastthe length of the display
    duplicate the background twice, and move them to each end of the original
    make the dupliates the child of the original, so it looks like this in the inspector:
    orignial background
        orignial background(front)
        orignial background(back)
    attach this script to the parent background, and make the parent background the child of the camera
    */
    private float length, startpos;
    public GameObject cam;
    [Tooltip("0 is static, 1 is following the camera")]
    public float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));//how far moved relative to camera

        float dist = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);//parrallax effect

        if (temp > startpos + length) {
            startpos += length; 
        }else if (temp < startpos - length)
        {
            startpos -= length;
        }
    }
}
