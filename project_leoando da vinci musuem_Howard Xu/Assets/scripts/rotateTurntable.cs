using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class rotateTurntable : MonoBehaviour
{
    public GameObject button;
    public float spinDuration=60;
    public int spinSpeed = 10;

    private float timer = 0.0f;
    public bool turntableSpin = false;
    private Transform turntable;
    private buttonActivationScript ButtonScript;

    void Start()
    {
        ButtonScript = button.GetComponentInChildren<buttonActivationScript>();
        turntable = this.gameObject.transform;

    }
   
    // Update is called once per frame
    void Update()
    {
        
        if (ButtonScript.pressed==true)//if button pressed spin turntable
        {
            turntableSpin = true;
            timer = 0.0f;//resets the timer
        }
        if (turntableSpin == true)
        {
            turntable.Rotate(0, spinSpeed * Time.deltaTime, 0);//rotate turntable at a set speed
            timer += Time.deltaTime;//counts up to the time

            if (timer > spinDuration)//once it spins for a set duation, stop spinning
            {
                timer -= spinDuration;
                turntableSpin = false;
            }
        }
    }
}
