using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject mob;
    void Start()
    {
        //check if this client has a player setup
        if (PlayerManager.localPlayerInstance == null)
        {
            //if not, we need to make them one, spawn a char for the local player
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 10, 0), Quaternion.identity,0);
        }
        else
        {
            Debug.LogFormat("ignoring scence load for {0}", SceneManagerHelper.ActiveSceneName);
        }
        /*
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(this.mob.name, new Vector3(7.92f, 1.76f, 9.27f), Quaternion.Euler(90,0,0), 0); ;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
