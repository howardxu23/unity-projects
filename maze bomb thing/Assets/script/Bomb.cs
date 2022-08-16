using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bombObject;
    public GameObject leftShockwave;
    public GameObject rightShockwave;
    public GameObject upShockwave;
    public GameObject downShockwave;
    private SpriteRenderer bombrenderer;
    public float bombFuse = 5;
    private SpriteRenderer Shockrender;
    public AudioClip BoomSound;
    private AudioSource kaboomsound;
    void Start()
    {
        leftShockwave.SetActive(false);
        rightShockwave.SetActive(false);
        upShockwave.SetActive(false);
        downShockwave.SetActive(false);
        bombrenderer = GetComponent<SpriteRenderer>();
        Shockrender = upShockwave.GetComponent<SpriteRenderer>();
        kaboomsound = GetComponent<AudioSource>();
    }


    void Update()
    {
        bombFuse -= Time.deltaTime;//waits 5 s, then bomb explodes
        if (bombFuse <= 0)
        {
            leftShockwave.SetActive(true);
            rightShockwave.SetActive(true);
            upShockwave.SetActive(true);
            downShockwave.SetActive(true);
            bombrenderer.enabled = false;
            kaboomsound.PlayOneShot(BoomSound);
        }
        if (Shockrender.enabled == false)
        {
            Destroy(bombObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "pressure plate")
        {

                PressurePlate pressureplate = collision.gameObject.GetComponent<PressurePlate>();
                pressureplate.isTriggered = true;
            
        }
    }
    void playsound()
    {

    }
}
