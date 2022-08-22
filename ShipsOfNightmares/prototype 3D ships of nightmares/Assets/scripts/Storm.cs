using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    private Light lightning;
    protected AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        lightning = GameObject.Find("Lightning").GetComponent<Light>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        audioManager.Play("Rain");
        audioManager.Play("Wind");
        Thunder();
    }


    void Thunder()
    {
        StartCoroutine(DoThunder(Random.Range(10, 20), Random.Range(1, 6)));
        //StartCoroutine(DoThunder(Random.Range(3, 6), Random.Range(1, 6)));
        //StartCoroutine(DoThunder(3, Random.Range(1, 6)));
    }

    IEnumerator DoThunder(float delay, int track)
    {
        yield return new WaitForSeconds(delay);// Wait 10-20 seconds between thunder strikes. ~ SK

        //Debug.Log("THUNDER!!!");
        lightning.enabled = true;// Enable an additional light source. ~ SK
        audioManager.Play("Thunder" + track);// Play a random Thunder track (Thunder1, Thunder2, ...). ~ SK
        yield return new WaitForSeconds(0.2f);// Wait 0.2 seconds so for the light to stay on. ~ SK

        lightning.enabled = false;// Turn off the additional light source (creating a 'flash' effect). ~ SK
        Thunder();// Recursion. ~ SK
    }
}
