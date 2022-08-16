using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bolt.Samples.Photon.Lobby.Drawgame
{
    public class QuetionGameDIspayPos : Bolt.GlobalEventListener
    {
        public void Update()
        {
            gameObject.transform.parent= GameObject.Find("MainCanvas").transform;
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}