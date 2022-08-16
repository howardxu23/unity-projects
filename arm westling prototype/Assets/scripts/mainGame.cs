
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class mainGame : MonoBehaviour
{
    public GameObject goodBar;
    public GameObject legendaryBar;

    public GameObject arrow;
    public GameObject armModel;

    public GameObject mainGameScript;

    public GameObject redWinTag;
    public GameObject BlueWinTag;

    public int player1Score=1;
    public int player2Score=1;
    private int MAXSCORE = 100;
    public float gameTimerMAX = 20f;//timer in s
    private float gametimer = 0f;
    private int direction;//direction of arrow, there are 4: 0=up,1=left,2=down,3=right
    private bool Isplayer1side;//top row or bottom row, top=player1



    //stops arrows from spawning on top of each other
    private float SPAWNARROWDELAY = 0.5f;
    private float timerspawntop = 0.0f;
    private float timerspawnbottom = 0.0f;
    private bool canSpawnArrow = false;
    private Vector3 topRowPos;
    private Vector3 bottomrowpos;

    private bool gameover = false;
    Arrows arrowScript;
    private void Start()
    {
        topRowPos = new Vector3(5.03f,0f,-10f);//sets the spawn location of the arrows
        bottomrowpos=new Vector3(5.03f, -1.2f, -10f);
        redWinTag.SetActive(false);
        BlueWinTag.SetActive(false);
    }
    void Update()
    {
        timerspawntop += Time.deltaTime;
        timerspawnbottom += Time.deltaTime;

        direction = Random.Range(0, 4);
        Isplayer1side = Random.Range(1, 3)==1;

        canSpawnArrow= Random.Range(0, 10) == 5;

        //arrow spawner
        if (canSpawnArrow && !gameover)//checks if can spawn a arrow
        {
            if (Isplayer1side && timerspawntop > SPAWNARROWDELAY)//spawn in top row
            {
                
                GameObject arrowClone = Instantiate(arrow);
                arrowClone.transform.Translate(topRowPos);
                timerspawntop = 0;
                arrowScript = arrowClone.GetComponent<Arrows>();
                arrowScript.direction = direction;
                arrowScript.whichrow = "top";
                arrowScript.MainScript = mainGameScript;
            }
            else if (!Isplayer1side && timerspawnbottom > SPAWNARROWDELAY) 
            {
                
                GameObject arrowClone = Instantiate(arrow);
                arrowClone.transform.Translate(bottomrowpos);
                timerspawnbottom = 0;
                arrowScript = arrowClone.GetComponent<Arrows>();
                arrowScript.direction = direction;
                arrowScript.whichrow = "bottom";
                arrowScript.MainScript = mainGameScript;
            }
        }

        //decides which player has won by seeing who has more score, determined by the angle of the arms to see who is closer
        if (!gameover)
        {
            if (player1Score > player2Score)
            {
                Quaternion rot = new Quaternion();
                rot.eulerAngles = new Vector3(-62, 0, 0);
                armModel.transform.rotation = rot;
            }
            else if (player1Score < player2Score)
            {
                Quaternion rot = new Quaternion();
                rot.eulerAngles = new Vector3(-125, 0, 0);
                armModel.transform.rotation = rot;
            }
        }
        //end game condition, time up or who reached score limit first

        gametimer += Time.deltaTime;
        print(gametimer);
        if (gametimer >= gameTimerMAX || player1Score >= MAXSCORE || player2Score >= MAXSCORE) 
        {
            gameover = true;
        }
        if (gameover)
        {
            if (player1Score > player2Score)
            {
                Quaternion rot = new Quaternion();
                rot.eulerAngles = new Vector3(0, 0, 0);
                armModel.transform.rotation = rot;
                redWinTag.SetActive(true);
            }
            else if (player1Score < player2Score)
            {
                Quaternion rot = new Quaternion();
                rot.eulerAngles = new Vector3(-189, 0, 0);
                armModel.transform.rotation = rot;
                BlueWinTag.SetActive(true);
            }
        }
    }
    
}
