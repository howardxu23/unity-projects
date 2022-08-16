using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerControldynamic : MonoBehaviourPun
{
    //static playercontrol
    // Start is called before the first frame update
    public float posX,posY;
    public float MoveSpeed;
    public float rotateSpeed=2;
    public GameObject player;

    Rigidbody2D rigidBody;

    #region Public Fields
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    #endregion

    private void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerControldynamic.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;
        rigidBody=GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)//sees if it is the correct player
        {
            return;
        }

        posX = transform.position.x;
        posY = transform.position.y;

        Vector3 direction = new Vector3(transform.up.x, transform.up.y);
        if (photonView.IsMine)
        {
            if (Input.GetKey("w"))//go forwards
            {
                rigidBody.velocity = direction * Time.deltaTime * MoveSpeed;
            }
            else if (Input.GetKey("s"))//go back
            {
                rigidBody.velocity = direction * Time.deltaTime * -MoveSpeed;
            }
            else if (Input.GetKey("a"))//turn left
            {
                rigidBody.angularVelocity = rotateSpeed;
            }
            else if (Input.GetKey("d"))//turn right
            {
                rigidBody.angularVelocity = -rotateSpeed;
            }
        }
    }
}
