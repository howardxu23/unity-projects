using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrows : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int direction=0;//0=up,1=left,2=down,3=right
    public float movespeed = 3f;
    public int colorState = 0;//0=normal,1=good,2=bad,3=legendary;
    public int addStrengthGood = 1;
    public int addStrengthLegendary = 5;
    public string whichrow = "top";//top or bottom row
    public GameObject MainScript;
    private mainGame mainscript;
    private float arrowPosX;

    public GameObject arrowGroup;
    public GameObject normalArrow;
    public GameObject goodArrow;
    public GameObject badArrow;
    public GameObject legendaryArrow;

    private void Start()
    {

        if (direction == 0)//up
        {
            transform.Rotate(0,0,-90);
        }
        else if (direction == 1)//left
        {
            transform.Rotate(0, 0, 0);
        }
        else if (direction == 2)//down
        {
            transform.Rotate(0, 0, 90);
        }
        else if (direction == 3)//right
        {
            transform.Rotate(0, 0, -180);
        }
        mainscript = MainScript.GetComponent<mainGame>();
    }
    void Update()
    {

        //moves arrow to the left
        if (direction == 0)
        {
            transform.Translate(0, 1 * Time.deltaTime * -movespeed, 0);
        }
        else if (direction == 2)
        {
            transform.Translate(0, 1 * Time.deltaTime * movespeed, 0);
        }
        else if (direction == 1)
        {
            transform.Translate(1 * Time.deltaTime * -movespeed, 0, 0);
        }
        else if (direction == 3)
        {
            transform.Translate(1 * Time.deltaTime * movespeed, 0, 0);
        }

        arrowPosX = transform.position.x;//get arrow pos
              //left boundary     //right boundary

        if (-3.78 < arrowPosX && arrowPosX < -3.66)//legendary area scoring
        {
            if (whichrow == "top")//for player 1
            {
                if (Input.GetKeyDown(KeyCode.W) && direction == 0 && colorState == 0)//up
                {
                    colorState = 3;
                    mainscript.player1Score += addStrengthLegendary;
                }
                else if (Input.GetKeyDown(KeyCode.A) && direction == 1 && colorState == 0)//left
                {
                    colorState = 3;
                    mainscript.player1Score += addStrengthLegendary;
                }
                else if (Input.GetKeyDown(KeyCode.S) && direction == 2 && colorState == 0)//down
                {
                    colorState = 3;
                    mainscript.player1Score += addStrengthLegendary;
                }
                else if (Input.GetKeyDown(KeyCode.D) && direction == 3 && colorState == 0)//right
                {
                    colorState = 3;
                    mainscript.player1Score += addStrengthLegendary;
                }
                else if (Input.anyKeyDown && colorState == 0 && !Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.RightArrow)) //wrong key pressed
                {
                    colorState = 2;
                }
            }
            else if (whichrow == "bottom")//for player 2
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && direction == 0 && colorState == 0)//up
                {
                    colorState = 3;
                    mainscript.player2Score += addStrengthLegendary;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction == 1 && colorState == 0)//left
                {
                    colorState = 3;
                    mainscript.player2Score += addStrengthLegendary;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && direction == 2 && colorState == 0)//down
                {
                    colorState = 3;
                    mainscript.player2Score += addStrengthLegendary;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) && direction == 3 && colorState == 0)//right
                {
                    colorState = 3;
                    mainscript.player2Score += addStrengthLegendary;
                }
                else if (Input.anyKeyDown && colorState == 0 && !Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.D)) //wrong key pressed
                {
                    colorState = 2;
                }

            }
        }           //left boundary                                   //right boundary
        else if ((-4.42 < arrowPosX && arrowPosX < -3.78) || (-3.66 < arrowPosX && arrowPosX < -3))//good area scoring
        {
            if (whichrow == "top")//for player 1
            {
                if (Input.GetKeyDown(KeyCode.W) && direction == 0 && colorState == 0)//up
                {
                    colorState = 1;
                    mainscript.player1Score += addStrengthGood;
                }
                else if (Input.GetKeyDown(KeyCode.A) && direction == 1 && colorState == 0)//left
                {
                    colorState = 1;
                    mainscript.player1Score += addStrengthGood;
                }
                else if (Input.GetKeyDown(KeyCode.S) && direction == 2 && colorState == 0)//down
                {
                    colorState = 1;
                    mainscript.player1Score += addStrengthGood;
                }
                else if (Input.GetKeyDown(KeyCode.D) && direction == 3 && colorState == 0)//right
                {
                    colorState = 1;
                    mainscript.player1Score += addStrengthGood;
                }
                else if (Input.anyKeyDown && colorState == 0 && !Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.RightArrow)) //wrong key pressed
                {
                    colorState = 2;
                }
            }
            else if (whichrow == "bottom")//for player 2
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && direction == 0 && colorState == 0)//up
                {
                    colorState = 1;
                    mainscript.player2Score += addStrengthGood;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction == 1 && colorState == 0)//left
                {
                    colorState = 1;
                    mainscript.player2Score += addStrengthGood;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && direction == 2 && colorState == 0)//down
                {
                    colorState = 1;
                    mainscript.player2Score += addStrengthGood;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) && direction == 3 && colorState == 0)//right
                {
                    colorState = 1;
                    mainscript.player2Score += addStrengthGood;
                }
                else if (Input.anyKeyDown && colorState == 0 && !Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.D)) //wrong key pressed
                {
                    colorState = 2;
                }
            }
        }else if(arrowPosX <-4.42 && colorState==0)//flys off the bar without doing anything
        {
            colorState = 2;
        }
        if(arrowPosX< -5.61)//destroy once it goes off screen
        {
            Destroy(arrowGroup);
        }
        //updates arrow color
        if (colorState == 0)//normal
        {
            normalArrow.SetActive(true);
            badArrow.SetActive(false);
            goodArrow.SetActive(false);
            legendaryArrow.SetActive(false);
        }
        else if (colorState == 1)//good
        {
            normalArrow.SetActive(false);
            badArrow.SetActive(false);
            goodArrow.SetActive(true);
            legendaryArrow.SetActive(false);
        }
        else if (colorState == 2)//bad
        {
            normalArrow.SetActive(false);
            badArrow.SetActive(true);
            goodArrow.SetActive(false);
            legendaryArrow.SetActive(false);
        }
        else if (colorState == 3)//legendary
        {
            normalArrow.SetActive(false);
            badArrow.SetActive(false);
            goodArrow.SetActive(false);
            legendaryArrow.SetActive(true);
        }
    }
}
