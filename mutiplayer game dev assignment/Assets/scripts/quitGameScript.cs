using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using Photon.Realtime;

public class quitGameScript : MonoBehaviourPunCallbacks
{
    public void leaveRoom()//when quit button is pressed, dissconnect from server
    {
        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)//due to a delay in sending the quit to server, loading the level needs to be done once
                                                              //it recives confirmation
    {
        base.OnDisconnected(cause);
        PhotonNetwork.LoadLevel("Launcher");
    }
}
