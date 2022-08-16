using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class playerListDisplayItem : MonoBehaviourPunCallbacks,IPunObservable
{
    [SerializeField]
    private Text playerName;
    private bool isMaster = false;

    public static GameObject localPlayerInstance;
    private Image background;

    private void Awake()
    {
        
        playerName.text = this.photonView.Owner.NickName;

        if (photonView.IsMine)
        {
            playerListDisplayItem.localPlayerInstance = this.gameObject;
        }
        gameObject.transform.parent = GameObject.Find("player list Panel").transform;
        background = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if (photonView.IsMine||!PhotonNetwork.IsConnected)
        {
            isMaster = PhotonNetwork.IsMasterClient;
            
        }
        if (isMaster == true)
        {
            this.background.color = Color.green;
        }
        else
        {
            this.background.color = Color.grey;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//We poll the value of stream.IsWriting - if this is true, it means that the client running this code is the owner of this object, and should prepare any variables that need sending.
        {
            stream.SendNext(isMaster);
        }
        else//If stream.IsWriting is false, it means that the client running this code is not the owner of this object, and we should prepare to receive any variables that have been sent.
        {
            this.isMaster = (bool)stream.ReceiveNext();
        }
    }
}
