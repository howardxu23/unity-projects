using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGen : MonoBehaviour
{
    //world gen
    public GameObject emptyBlock;
    public GameObject startBlock;
    public GameObject endBlock;
    public GameObject pitfallBlock;
    public GameObject spikeBlock;
    public GameObject platformSpikeBlock;
    public GameObject trappedBlock;
    public GameObject spacer;
    public GameObject player;

    playerMechinic playerScript;
    public Vector3 playerStartPos;
    public bool reachedEnd = false;
    public Vector3 currentBlockPos;
    private  float cube_width =0.23f;
    public bool spawningMap = true;

    private int randomNum;
    void Start()
    {
        playerStartPos = new Vector3(0.9f, 0.64f, 0);
        player.transform.position = playerStartPos;
        currentBlockPos = new Vector3(0, 0, 0);
        randomNum = Random.Range(1, 100);

        playerScript = player.GetComponent<playerMechinic>();
        //spawnStartBlock(ref currentBlockPos);
        //spawnspacer(ref currentBlockPos);
    }

   
    void Update()
    {
        if (!reachedEnd)
        {
            if (spawningMap)//creates the level
            {
                
                spawnStartBlock(ref currentBlockPos);
                while (randomNum > 5)
                {
                    randomNum = Random.Range(1, 100);
                    var blockIndex = Random.Range(1, 6);
                    
                    switch (blockIndex)
                    {
                        case 1:
                            spawnEmptyBlock(ref currentBlockPos);
                            spawnspacer(ref currentBlockPos);
                            break;
                        case 2:
                            spawnpitfallblock(ref currentBlockPos);
                            spawnspacer(ref currentBlockPos);
                            break;
                        case 3:
                            spawnplatformspikeblock(ref currentBlockPos);
                            spawnspacer(ref currentBlockPos);
                            break;
                        case 4:
                            spawnspikeblock(ref currentBlockPos);
                            spawnspacer(ref currentBlockPos);
                            break;
                        case 5:
                            spawntrappedblock(ref currentBlockPos);
                            spawnspacer(ref currentBlockPos);
                            break;
                        default:
                            print("something went wrong");
                            break;
                    }
                }
                spawnEndBlock(ref currentBlockPos);
                spawningMap = false;                
            }           
        }
        else if (reachedEnd)//player reaches exit, generates new level
        {
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("blocks");
            foreach (var block in blocks)
            {
                Destroy(block);
            }
            reachedEnd = false;
            spawningMap = true;
            player.transform.position = playerStartPos;
            randomNum= Random.Range(1, 100);
            playerScript.lives += 1;
        }
    }
    void spawnStartBlock(ref Vector3 spawnpos)
    {
        Instantiate(startBlock, spawnpos,Quaternion.identity);

        spawnpos = spawnpos + new Vector3(6.0f*cube_width,0,0);
        
    }
    void spawnEndBlock(ref Vector3 spawnpos)
    {
        Instantiate(endBlock, spawnpos, Quaternion.identity);
        spawnpos = new Vector3(0, 0, 0);
    }
    void spawnEmptyBlock(ref Vector3 spawnpos)
    {
        Instantiate(emptyBlock, spawnpos, Quaternion.identity);
        spawnpos = spawnpos+new Vector3(5.0f * cube_width, 0, 0);
    }
    void spawnpitfallblock(ref Vector3 spawnpos)
    {
        Instantiate(pitfallBlock, spawnpos, Quaternion.identity);
        spawnpos = spawnpos + new Vector3(10.0f * cube_width, 0, 0);
    }
    void spawnspikeblock(ref Vector3 spawnpos)
    {
        Instantiate(spikeBlock, spawnpos, Quaternion.identity);
        spawnpos = spawnpos + new Vector3(5.0f * cube_width, 0, 0);
    }
    void spawnplatformspikeblock(ref Vector3 spawnpos)
    {
        Instantiate(platformSpikeBlock, spawnpos, Quaternion.identity);
        spawnpos = spawnpos + new Vector3(10.0f * cube_width, 0, 0);
    }
    void spawntrappedblock(ref Vector3 spawnpos)
    {
        Instantiate(trappedBlock, spawnpos, Quaternion.identity);
        spawnpos = spawnpos + new Vector3(10.0f * cube_width, 0, 0);
    }
    void spawnspacer(ref Vector3 spawnpos)
    {
        Instantiate(spacer, spawnpos, Quaternion.identity);
        spawnpos = spawnpos + new Vector3(3.0f * cube_width, 0, 0);
    }
}

