using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brachycontrol : MonoBehaviour
{
    public bool gameOver = false;
    public GameObject opponet;
    Basic_ai AI;
    Animator animator;

    //balance stuff
    public int HEALTH = 1000;

    int star_punch = 0;
    public float IFRAME = 20;

    public int PUNCHDAMAGEHIGH = 50;
    public int PUNCHDAMAGELOW = 30;
    public int STARPUNCHDAMAGE = 1000;
    public int STAMINA = 25;

    //don't change these
    public bool reset = false;
    public int stamina;
    public int health;
    public bool knockout = false;
    int HEALTHLOSS;
    public int starpunch = 0;
    int starpunchBuildUp = 0;
    public int blowslanded=0;
    public bool starPunched = false;
    public bool isBlocking = false;
    public bool isEvade = false;
    bool evadeleft = false;
    bool evaderight = false;
    public bool lowleftpunch=false;
    public bool lowrightpunch = false;
    public bool highleftpunch = false;
    public bool highrightpunch = false;
    public bool hitright = false;
    public bool hitleft = false;
    float iframes = 0;
    float knockoutDelay = 75;
    // Start is called before the first frame update
    void Start()
    {
        stamina = STAMINA;
        health = HEALTH;
        animator = GetComponent<Animator>();
        AI = opponet.GetComponent<Basic_ai>();
        HEALTHLOSS = AI.DAMAGE;
    }

    // Update is called once per frame

    public void Update()
    {
        if (reset == true)//resets the stats after presing start again
        {
            knockout = false;
            health = HEALTH;
            animator.enabled = true;
            reset = false;
            stamina = STAMINA;
            animator.SetBool("knockout", false);
            starpunch = 0;
            knockoutDelay = 75;
            hitleft = false;
            hitright = false;
        }
        if (gameOver)
        {
            return;
        }

        if (knockout == false)
        {
            if (stamina < 0)
            {
                stamina = 0;
            }
            if (Input.GetKeyDown("a") && stamina > 0)
            {
                //print("low left punch");
                lowleftpunch = true;
                animator.SetTrigger("left low punch");
                stamina -= 1;
            }
            else if (Input.GetKeyDown("q") && stamina > 0)
            {
                //print("high left punch");
                highleftpunch = true;
                animator.SetTrigger("left high punch");
                stamina -= 1;
            }
            else if (Input.GetKeyDown("d") && stamina > 0)
            {
                //print("low right punch");
                lowrightpunch = true;
                animator.SetTrigger("right low punch");
                stamina -= 1;
            }
            else if (Input.GetKeyDown("e") && stamina > 0)
            {
                //print("high right punch");
                highrightpunch = true;
                animator.SetTrigger("right high punch");
                stamina -= 1;
            }
            else if (Input.GetKeyDown("z"))
            {
                // print("evade left");
                evadeleft = true;
                isEvade = true;
                animator.SetTrigger("evade left");
                iframes = IFRAME;
                if (stamina > 0)
                {
                    stamina -= 2;
                }

            }
            else if (Input.GetKeyDown("c"))
            {
                //print("evade right");
                evaderight = true;
                isEvade = true;
                animator.SetTrigger("evade right");
                iframes = IFRAME;
                if (stamina > 0)
                {
                    stamina -= 2;
                }
            }
            else if (Input.GetKeyDown("w") && stamina > 0)
            {
                //print("block");
                isBlocking = true;
                animator.SetBool("block", true);
            }
            else if (Input.GetKeyDown("s") && starpunch > 0)
            {
                // print("star punch");
                starPunched = true;
                animator.SetTrigger("star punch");
                starpunch -= 1;

            }
            else if (Input.GetKeyUp("w"))
            {
                isBlocking = false;
                animator.SetBool("block", false);
            }
            else
            {
                //resets states
                evadeleft = false;
                evaderight = false;
                lowleftpunch = false;
                lowrightpunch = false;
                highleftpunch = false;
                highrightpunch = false;
                starPunched = false;

            }
            if (blowslanded == 3)//increments starpucnh build up by 1
            {
                blowslanded = 0;
                starpunchBuildUp += 1;

            }
            else if (AI.missed == false)
            {
                blowslanded = 0;
            }
            if (starpunchBuildUp == 3)//3 combos consectivly without getting hit in return and it gives you 1 star punch in clip, but if you get hit it resets the build up
            {
                starpunch += 1;
                starpunchBuildUp = 0;
            }
            if (isEvade == true && iframes > 0)
            {//resets evade after specic amount of time
                iframes -= Time.deltaTime;
            }
            else
            {
                isEvade = false;
            }

            if (hitleft == true || hitright == true)//sees if it gets hit by the AI
            {
                if (isEvade == false && isBlocking == false)//if both are false then the player takes damage
                {
                    starpunchBuildUp = 0;//if get hit the star punch buildup gets resetted
                    if (hitleft == true)
                    {
                        animator.SetTrigger("hit left");
                        hitleft = false;
                        health -= HEALTHLOSS;
                    }
                    else if (hitright == true)
                    {
                        animator.SetTrigger("hit right");
                        hitright = false;
                        health -= HEALTHLOSS;
                    }
                }
                else if (isBlocking == true)//blocking will still take some damage
                {
                    health -= HEALTHLOSS / 3;
                    animator.SetTrigger("block hit");
                    hitright = false;
                    hitleft = false;
                }
            }
        }
        if (health <= 0)
        {//checks to see if player is knocked out
            knockout = true;
            animator.SetBool("knockout", true);
            knockoutDelay -= Time.deltaTime;
        }

        if (knockout == true && knockoutDelay <= 0)
        {
            animator.enabled = false;
        }
    }
}
