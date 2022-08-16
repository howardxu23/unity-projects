using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIscript : MonoBehaviour
{
    public Canvas UiStatus;
    public bool gameOver = false;
    public GameObject Player;
    brachycontrol player;
    public GameObject ai;
    Basic_ai AI;
    public GameObject camera;
    Camera mainCamera;
    public Slider playerHealthBar;
    public Slider AiHealthBar;
    public Text playerStaminaDisplay;
    public Text playerStarpunchDisplay;
    public Text AiStaminaDisplay;
    
    public GameObject startScreenUI;
    public GameObject faintTag;
    public GameObject WinTag;
    public GameObject blackScreen;
    public GameObject whiteScreen;
    SpriteRenderer blackscreen;
    SpriteRenderer whitescreen;

    int playerhealth;
    int playerstamina;
    int playerstarpunch;

    int initalPlayerHP;
    int initalAIHP;
    int AIhealth;
    int AIstamina;
    float fadeSpeed = 0.2f;

    bool StartTransitionProgressdone = false;
    bool StartHalfFade = false;
    bool endHalffade = false;
    bool endTrasnsitionprogressdone = false;
    float YOUWINDISPLAYDELAY = 5;
    float youWinDisaplayDelay;

    // Start is called before the first frame update
    void Start()
    {
        AI = ai.GetComponent<Basic_ai>();
        player = Player.GetComponent<brachycontrol>();
        initalPlayerHP = player.health;
        initalAIHP = AI.HEALTH;
        blackscreen = blackScreen.GetComponent<SpriteRenderer>();
        whitescreen = whiteScreen.GetComponent<SpriteRenderer>();
        youWinDisaplayDelay = YOUWINDISPLAYDELAY;
    }

    // Update is called once per frame
    void Update()
    {

        if (gameOver == true)
        {
            UiStatus.gameObject.SetActive(false);//disables the UI
            startScreenUI.gameObject.SetActive(true);//enables the start button
            player.gameOver = true;//stops them from running in the background
            AI.gameOver = true;
            youWinDisaplayDelay = YOUWINDISPLAYDELAY;
            return;
        }
        else if (gameOver==false&&StartTransitionProgressdone==false)
        {

            StartToGameTransition();
            AI.reset = true;
            player.reset = true;
            if (StartHalfFade==true)
            {
                camera.transform.position = new Vector3(-0.08f, 0.29f, -20f);
                startScreenUI.gameObject.SetActive(false);
            }
            if (StartTransitionProgressdone==true)
            {
                AI.gameOver = false;
                player.gameOver = false;
                UiStatus.gameObject.SetActive(true);

            }

        }
        //setting varibles for player statuses
        playerhealth = player.health;
        playerstamina = player.stamina;
        playerstarpunch = player.starpunch;
        playerHealthBar.maxValue = initalPlayerHP;
        //setting varibles for AI statuses
        AIhealth = AI.health;
        AIstamina = AI.stamina;
        AiHealthBar.maxValue = initalAIHP;

        playerStaminaDisplay.text = playerstamina.ToString();
        playerStarpunchDisplay.text = playerstarpunch.ToString();
        playerHealthBar.value = playerhealth;

        AiStaminaDisplay.text = AIstamina.ToString();
        AiHealthBar.value = AIhealth;

        if (player.knockout==true)
        {
            endRoundTransition(false);
        }
        else if (AI.knockout==true)
        {
            endRoundTransition(true);
        }
        else
        {
            WinTag.gameObject.SetActive(false);
        }

    }


    public void endRoundTransition(bool win)
    {
        if (win == false)
        {

            if (endHalffade == false && fadeBlack(true) == false){
                fadeBlack(true);
                faintTag.gameObject.SetActive(true);
            }
            else
            {
                camera.transform.position = new Vector3(-20.79f, 0.27f, -20f);//displays the start screen
                endHalffade = true;
                faintTag.gameObject.SetActive(false);
                endTrasnsitionprogressdone = fadeBlack(false);
            }
            if (endTrasnsitionprogressdone == true)
            {
                gameOver = true;
                StartHalfFade = false;
                StartTransitionProgressdone = false;
            }
        }
        else if (win == true)
        {
            if (endHalffade == false && fadeWhite(true) == false)
            {
                fadeWhite(true);
            }
            else
            {
                if (youWinDisaplayDelay > 0&&AI.knockout==true)
                {
                    WinTag.gameObject.SetActive(true);
                    UiStatus.gameObject.SetActive(false);
                    youWinDisaplayDelay -= Time.deltaTime;
                }
                else
                {
                    WinTag.gameObject.SetActive(false);
                    camera.transform.position = new Vector3(-20.79f, 0.27f, -20f);//displays the start screen
                    endHalffade = true;
                    endTrasnsitionprogressdone = fadeWhite(false);

                    if (endTrasnsitionprogressdone == true)
                    {
                        gameOver = true;
                        StartHalfFade = false;
                        StartTransitionProgressdone = false;
                    }
                }
            }
            /*
            if (endTrasnsitionprogressdone == true)
            {
                gameOver = true;
                StartHalfFade = false;
                StartTransitionProgressdone = false;
            }*/
        }
    }
    public void StartToGameTransition()//fades in the screen, then out again, for the start transition.
    {
        if (StartHalfFade == false&&fadeBlack(true)==false)
        {
            fadeBlack(true);
        }
        else
        {

            StartHalfFade = true;
            StartTransitionProgressdone = fadeBlack(false);
            endHalffade = false;
            endTrasnsitionprogressdone = false;
        }
    }
    public bool fadeBlack(bool In)//fade transition black in and out, returns true when done
    {
        if (In == false)//fades out to clear
        {

            Color screencolor=blackscreen.color;
            float fadeamount = screencolor.a - (fadeSpeed*Time.deltaTime);
            screencolor = new Color(screencolor.r, screencolor.g, screencolor.b, fadeamount);
            blackscreen.color = screencolor;

            if (screencolor.a <= 0)//tells main code that fading is done
            {
                return true;
            }
     
        }else if (In == true)//fades in to black
        {
            Color screencolor = blackscreen.color;
            float fadeamount = screencolor.a + (fadeSpeed * Time.deltaTime);
            screencolor = new Color(screencolor.r, screencolor.g, screencolor.b, fadeamount);
            blackscreen.color = screencolor;

            if (screencolor.a >= 1)//tells main code that fading is done
            {
                return true;
            }
        }

        return false;
    }
    public bool fadeWhite(bool In)//fade transition for white screen,returns true when done
    {
        if (In == false)//fades out to clear
        {
            Color screencolor = whitescreen.color;
            float fadeamount = screencolor.a - (fadeSpeed*Time.deltaTime);
            screencolor = new Color(screencolor.r, screencolor.g, screencolor.b, fadeamount);
            whitescreen.color = screencolor;

            if (screencolor.a <= 0)//tells main code that fading is done
            {
                return true;
            }
        }
        else if (In == true)//fades in to white
        {
            Color screencolor = whitescreen.color;
            float fadeamount = screencolor.a + (fadeSpeed * Time.deltaTime);
            screencolor = new Color(screencolor.r, screencolor.g, screencolor.b, fadeamount);
            whitescreen.color = screencolor;
          
            if (screencolor.a >=1)//tells main code that fading is done
            {
                return true;
            }

        }
        return false;
    }
}
