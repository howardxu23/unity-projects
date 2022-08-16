using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateObject : MonoBehaviour
{

    Animator animation;
    rotateTurntable turntableScript;

    // Start is called before the first frame update
    void Start()
    {
        animation = gameObject.GetComponent<Animator>();
        animation.enabled=false;//disables animation
        turntableScript = gameObject.GetComponentInParent(typeof( rotateTurntable)) as rotateTurntable;
    }

    // Update is called once per frame
    void Update()
    {
        if (turntableScript.turntableSpin == true)//checks to see if turntable is spinning
        {
            animation.enabled = true;//play animation
        }
        else
        {
            animation.enabled = false;//stops animation
        }
    }
}
