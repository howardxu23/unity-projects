using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks,IPunObservable
{
    public float speed = 0.03f;
    public static GameObject localPlayerInstance;
    public bool activated=false;
    
    void Update()
    {
        // photonView.IsMine to check that this is actually our player to control.
        // If it isn't, we return, to drop out of the update method and do no further operations on this instance of the Player.

        // PhotonNetwork.IsConnected - this makes debugging a little easier for us!
        // If we're testing the player controller in an offline environment, we can control the player ourselves.
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {


            activated = Input.GetButton("Fire1");//checks to see if fire button is pressed
            this.gameObject.transform.position = new Vector3(//player controller
                this.gameObject.transform.position.x + Input.GetAxis("Horizontal") * speed,
                 this.gameObject.transform.position.y,
                  this.gameObject.transform.position.z + Input.GetAxis("Vertical") * speed);
        }
        if (activated == true)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.gray;
        }
    }
    private void Awake(){//We are going to keep a static reference to the player that refers to this client's player GameObject
                        //this will be really helpful if we need to check whether this client has been given a Player yet, and if
                        //we need to quickly find the GameObject being used as this client's player.  
        if (photonView.IsMine)
        {
            PlayerManager.localPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    //his procedure will be called when this class is serialized across the network (that's just a fancy word for sent/received),
    //which happens several times per second.
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //In here, we can write code to tell Photon what variables we'd like to send, and also, assign back the value of the variables we received.
        if (stream.IsWriting)//We poll the value of stream.IsWriting - if this is true, it means that the client running this code is the owner of this object, and should prepare any variables that need sending.
        {
            stream.SendNext(activated);
        }
        else//If stream.IsWriting is false, it means that the client running this code is not the owner of this object, and we should prepare to receive any variables that have been sent.
        {
            this.activated = (bool)stream.ReceiveNext();
        }
    }
}
