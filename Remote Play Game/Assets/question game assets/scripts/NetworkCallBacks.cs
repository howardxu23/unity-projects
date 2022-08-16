using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using UnityEngine.UI;

namespace Bolt.Samples.Photon.Lobby.Drawgame
{
    public class NetworkCallBacks : GlobalEventListener
    {

        public GameObject[] playerPrefabs;
        public GameObject[] spawnPoints;

        [System.Obsolete]
        public override void SceneLoadLocalDone(string scene)
        {
            //Instantiate players
            int randomNumber = Random.Range(0, spawnPoints.Length - 1);
            Vector3 spawnPoint = spawnPoints[randomNumber].GetComponent<Transform>().position;

            int randomPrefab = Random.Range(0, playerPrefabs.Length - 1);

            if (BoltNetwork.IsClient)
            {
                var entity = BoltNetwork.Instantiate(playerPrefabs[randomPrefab], spawnPoint, Quaternion.identity);
            }
            //instantiate the quetion display
            BoltNetwork.Instantiate(BoltPrefabs.Quetion_game_display);

        }
    }
}
