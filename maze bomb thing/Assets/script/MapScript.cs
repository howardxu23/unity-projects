using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public GameObject metalWall;
    public GameObject armoredWall;
    public GameObject woodWall;

    public GameObject player;
    public GameObject goal;
    public GameObject winsprite;
    public GameObject losesprite;

    private Vector3 currentPos;
    private float MOVRIGHTAMOUNT = 1f;
    private float MOVEDOWNAMOUNT = -1f;

    public int[,] Maparray = new int[,]   { { 1, 0, 1, 1, 1, 1, 1, 1 },
                                            { 1, 0, 0, 1, 0, 0, 0, 1 },
                                            { 1, 0, 1, 0, 0, 0, 0, 1 },
                                            { 1, 0, 0, 0, 0, 1, 1, 1 },
                                            { 1, 1, 0, 1, 0, 0, 0, 1 },
                                            { 1, 2, 1, 1, 2, 1, 2, 1 },
                                            { 1, 0, 2, 0, 0, 0, 0, 1 },
                                            { 1, 1, 1, 1, 1, 1, 1, 1 }};//0=empty space,1=metal wall,2=unarmored wall, 30=armoed up,31=armoed right,32=armoed down, 33==armoed left
    // Start is called before the first frame update
    void Start()
    {
        float originalX = -3.52f;
        float Ymap = 3.42f;
        float Xmap = originalX; 
        currentPos = new Vector3(Xmap, Ymap, 0);

        for (int y= 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (Maparray[y, x] == 0)//empty space
                {
                    //do nothing, move one square to the right

                }
                else if (Maparray[y, x] == 1)//spawns metal wall
                {
                    GameObject wall = Instantiate(metalWall);
                    wall.transform.position = currentPos;
                }
                else if (Maparray[y,x]==2)//unaromed wall
                {
                    GameObject wall = Instantiate(woodWall);
                    wall.transform.position = currentPos;
                }
                /*
                else if (Maparray[y, x] == 30)//armoed up code
                {
                    
                }
                else if (Maparray[y, x] == 31)//armoed right code
                {
                    
                }
                else if (Maparray[y, x] == 32)//armoed down code
                {

                }
                else if (Maparray[y, x] == 33)//armoed left code
                {

                }
                */

                Xmap += MOVRIGHTAMOUNT;
                currentPos = new Vector3(Xmap, Ymap, -2);
            }
           Xmap = originalX;
            Ymap += MOVEDOWNAMOUNT;
            currentPos = new Vector3(Xmap, Ymap, -2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.activeSelf == false)//player kills himself
        {
            losesprite.SetActive(true);
        }
        if (goal.activeSelf==false)//exit successful, game is won
        {
            winsprite.SetActive(true);
        }
    }
}
