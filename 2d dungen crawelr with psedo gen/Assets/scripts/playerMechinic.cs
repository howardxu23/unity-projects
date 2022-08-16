using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMechinic : MonoBehaviour
{

    public int lives = 3;
    public bool hardcoreMode = false;
    public  bool hurt = false;
    public  bool dead = false;
    private bool isImmune = false;
    Rigidbody2D rigidBody;
    SpriteRenderer playerSprite;
    public int impulseForce;
    public float immumeDuration = 3f;
    public float immumeFlash = 0.5f;
    private float IMMUNEFLASH;
    private float IMMUNEDURATION;
    private float temp;
    public GameObject gameMamanger;
    levelGen gameController;
    public GameObject livesLeftText;
    private Text textbox;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        IMMUNEDURATION = immumeDuration;
        IMMUNEFLASH = immumeFlash;
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
        temp = IMMUNEFLASH;
        gameController = gameMamanger.GetComponent<levelGen>();
        textbox = livesLeftText.GetComponent<Text>();
        if (hardcoreMode)
        {
            lives = 1;
        }
    }


    void Update()
    {
        textbox.text = "lives: " + lives;
        if(hurt)//launches player up, makes it blink for a while and short immunity frames
        {
            rigidBody.AddForce(new Vector2(0.0f, impulseForce),ForceMode2D.Impulse);
            hurt = false;
            lives -= 1;
            isImmune = true;
        }
        if (isImmune)//makes plauer flash when immunity frame is up
        {
            if (immumeDuration  <= 0)
            {
                isImmune = false;
                immumeDuration = IMMUNEDURATION;
                immumeFlash = IMMUNEFLASH;
                temp = IMMUNEFLASH;
            }
            else
            {
                immumeDuration -= Time.deltaTime;

                immumeFlash -= Time.deltaTime;

                if (playerSprite.enabled == true && immumeFlash <= 0)
                {
                    playerSprite.enabled = false;
                    immumeFlash = temp;
                    temp=temp / 2;
                }
                else if (playerSprite.enabled == false && immumeFlash <= 0)
                {
                    playerSprite.enabled = true;
                    immumeFlash = temp;
                    temp = temp / 2;
                }
            }
        }
        else
        {
            playerSprite.enabled = true;
        }
        if(dead)
        {
            transform.position = new Vector3(0.9f, 0.64f, 0);
            dead = false;
            lives -= 1;
        }
        if (lives == 0)
        {
            gameController.reachedEnd = true;
            lives = 2;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "spike" && hurt == false &&isImmune==false)//sees if touches spike
        {
            hurt = true;
        }
        else if (other.gameObject.tag == "pitfall")
        {
            dead = true;
        }
        else if(other.gameObject.tag =="trigger trap")
        {

            var trapscript = other.gameObject.GetComponent<trapScript>();
            trapscript.isTriggered = true;
        }
        if (other.gameObject.tag == "exit")
        {
            gameController.reachedEnd = true;
        }
    }



}
