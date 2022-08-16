using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_ai : MonoBehaviour
{
    public bool gameOver = false;
    // Start is called before the first frame update
    public GameObject Player;
    Animator animator;
    brachycontrol player;

    //game balnce stuff
    public int HEALTH=1000;
    public float MISSDELAY = 50;
    public float PUNCHDELAYANIMATION = 0;
    public float PUNCHIMPACTDELAY = 0.1f;
    public int stamina = 30;
    public int DAMAGE = 50;
    public int LOWHEALTHANIMMATIONLIMIT;
    public float MINPUNCHDELAY = 0;
    public float MAXPUNCHDELAY = 0;
    //checks to see if animations are there
    public bool EVADEANIMATION;
    public bool BLOCKANIMATION;
    public bool LOWHEALTHSTATEANIMATION;
    float knockoutDelay;
    public float KNOCKOUTDELAY;
    //don't change these
    public bool reset=false;
    bool PlayinglowHealthAnimation=false;
    public int health;
    public bool knockout = false;
    float anidelay = 0;
    public bool leftpunch = false;
    public bool rightpunch = false;
    float missdelay = 0;
    bool block = false;
    bool evadeleft = false;
    bool evaderight = false;
    public bool missed = false;
    float stun=0.0f;
    bool doneAnimation = false;
    float countdownToNextPunch = 0;
    void Start()
    {
        knockoutDelay = KNOCKOUTDELAY;
        health = HEALTH;
        animator = GetComponent<Animator>();
        player = Player.GetComponent<brachycontrol>();
        countdownToNextPunch = 5;
        PUNCHIMPACTDELAY = PUNCHDELAYANIMATION - PUNCHIMPACTDELAY;
    }

    // Update is called once per frame
    public void Update()
    {
        if (reset == true)
        {
            knockout = false;
            health = HEALTH;
            animator.enabled = true;
            reset = false;
            animator.enabled = true;
            knockoutDelay = KNOCKOUTDELAY;
            animator.SetBool("KO left", false);
            animator.SetBool("KO right", false);
            doneAnimation = false;
        }
        if (gameOver)
        {
            return;
        }
        if (knockout == false && PlayinglowHealthAnimation==false)
        {

            if (missed == false)//once the missed delay cools off
            {
                countdownToNextPunch -= Time.deltaTime;//waits random time beween punches

                if (countdownToNextPunch<=0 && anidelay <= 0)//attempts new punch
                {
                    countdownToNextPunch = Random.Range(MINPUNCHDELAY, MAXPUNCHDELAY); //set a random time between punches

                    if (Random.Range(0, 3) == 1)
                    {
                        leftpunch = true;
                        animator.SetTrigger("left punch");
                        anidelay = PUNCHDELAYANIMATION;
                    }
                    else
                    {
                        rightpunch = true;
                        animator.SetTrigger("right punch");
                        anidelay = PUNCHDELAYANIMATION;
                    }

                }
                if (anidelay <= PUNCHIMPACTDELAY)
                {
                    if (leftpunch)
                    {
                        if (player.isEvade == true)//sees if player is evading, then it freezes the animation
                        {
                            leftpunch = false;
                            missed = true;
                            missdelay = MISSDELAY;
                            stun = MISSDELAY / 2;
                            animator.SetBool("missed", true);

                            if (player.stamina <= 0)
                            {
                                player.stamina += 15;
                            }
                            else
                            {
                                player.stamina += 5;
                            }
                        }
                        else if (player.isEvade == false)
                        {
                            leftpunch = false;
                            player.hitleft = true;
                        }
                    }
                    else if (rightpunch)
                    {
                        if (player.isEvade == true)//sees if player is evading, then it freezes the animation
                        {
                            rightpunch = false;
                            missed = true;
                            missdelay = MISSDELAY;
                            stun = MISSDELAY / 2;
                            animator.SetBool("missed", true);

                            if (player.stamina <= 0)
                            {
                                player.stamina += 15;
                            }
                            else
                            {
                                player.stamina += 5;
                            }
                        }
                        else if (player.isEvade == false)
                        {
                            rightpunch = false;
                            player.hitright = true;
                        }
                    }

                }
                if ((player.lowleftpunch || player.lowrightpunch || player.highleftpunch || player.highrightpunch||player.starPunched) && BLOCKANIMATION == true)//block animation
                {
                    animator.SetTrigger("block");
                    anidelay = 0.1f;
                    if (player.starPunched == true)
                    {
                        health -= player.STARPUNCHDAMAGE / 4;
                    }
                }
            }
            else if (missed == true && missdelay > 0)//opening for the player to land the blows
            {
                //high punches have high damage and can KO, but lower stun potential
                if (player.highleftpunch == true)
                {

                    health -= player.PUNCHDAMAGEHIGH;
                    animator.SetBool("missed", false);
                    missdelay += stun;
                    stun = stun / 3;
                    player.blowslanded += 1;
                    if (health <= 0)
                    {
                        knockout = true;
                        animator.SetBool("KO left", true);
                    }
                    else
                    {
                        animator.SetTrigger("hit left");
                    }
                }
                else if (player.highrightpunch == true)
                {

                    health -= player.PUNCHDAMAGEHIGH;
                    animator.SetBool("missed", false);
                    missdelay += stun;
                    stun = stun / 3;
                    player.blowslanded += 1;
                    if (health <= 0)
                    {
                        knockout = true;
                        animator.SetBool("KO right", true);
                    }
                    else {
                        animator.SetTrigger("hit right");
                    }
                }
                else if (player.lowleftpunch == true || player.lowrightpunch == true)//low punchs do less damage and unable to KO, but have a lengthed "stun" effect"
                {
                    animator.SetTrigger("hit low");
                    health -= player.PUNCHDAMAGELOW;
                    animator.SetBool("missed", false);
                    missdelay += stun;
                    stun = stun / 2;
                    player.blowslanded += 1;
                    if (health <= 0)
                    {
                        health = 2;
                    }
                }
                else if (player.starPunched == true)//checks to see if the player did a star punch
                {
                    health -= player.STARPUNCHDAMAGE;
                    animator.SetBool("missed", false);
                    if (health <= 0)
                    {
                        knockout = true;
                        animator.SetBool("KO right", true);
                    }
                    else
                    {
                        animator.SetTrigger("hit right");
                    }
                }
            }


            block = false;
            evadeleft = false;
            evaderight = false;
        }
        if (knockout == true && knockoutDelay <= 0)
        {
            animator.enabled=false;
        }
        else if(knockout == true && knockoutDelay >= 0)
        {
            knockoutDelay -= Time.deltaTime;
        }
        if (LOWHEALTHSTATEANIMATION==true && (health < LOWHEALTHANIMMATIONLIMIT)&& missdelay<=0 &&doneAnimation==false)
        {
            animator.SetTrigger("lowHP animation trigger");
            doneAnimation = true;
        }
        if (anidelay > 0)//decrease the timer before blow conects
        {
            anidelay -= Time.deltaTime;

        }
        
        if (missdelay > 0)//resets the missed punch delay
        {
            missdelay -= Time.deltaTime;
        }
        else if (missdelay <= 0)
        {
            missed = false;
            animator.SetBool("missed", false);
            
        }
    }
    void SetPlayinglowHealthAnimation()
    {
        PlayinglowHealthAnimation = true;
    }
    void UnsetPlayinglowHealthAnimation()
    {
        PlayinglowHealthAnimation = false;
    }
}
