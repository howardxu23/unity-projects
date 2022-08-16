using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonActivationScript : MonoBehaviour
{
    public bool pressed=false;
    private Transform button;

    [SerializeField]
    private GameObject textDiscription;
    [SerializeField]
    private float textDisplayDuration=20.0f;
    void Start()
    {
        button = this.gameObject.transform;
        //textDiscription.SetActive(false);
    }
    
    bool isTextdisplayrunning = false;
    IEnumerator doTextDisplay()
    {
        if (isTextdisplayrunning)
        {
            yield break;
        }
        isTextdisplayrunning = true;
        textDiscription.SetActive(true);
        yield return new WaitForSeconds(textDisplayDuration);
        textDiscription.SetActive(false);
        isTextdisplayrunning = false;
    }
    IEnumerator doDepressButton(float duration)//animates the button being pressed
    {
        button.position = transform.position + new Vector3(0, -0.2f, 0);//moves button down
        yield return new WaitForSeconds(duration);
        button.position = transform.position + new Vector3(0, 0.2f, 0);//moves button to orignal position
        pressed = false;
    }
    public void depressButton(float duration)
    {
        StartCoroutine(doDepressButton(duration));
    }
    public void OnMouseDown()//sees if the player clicked on the button model
    {
        if (pressed == false)
        {
            //StartCoroutine(doTextDisplay());
            pressed = true;
            depressButton(1);
        }
    }

}
