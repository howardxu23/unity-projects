using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TurretControl : MonoBehaviourPun, IPunObservable 
{
    public GameObject Hull;
    public GameObject shell;
    public GameObject muzzlePoint;
    public bool GunFired = false;
    private Vector3 hullPos;
    public float rotateSpeed;
    private Quaternion hullAngle;
    public float turretRotation;
    private float degreesFromHullNormal=0;
    private Transform hull;
    public float ShellSpeed=50;

    
    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(GunFired);
            print("sent");
        }
        else
        {
            // Network player, receive data
            this.GunFired = (bool)stream.ReceiveNext();
            print("recived");
            print(GunFired);
        }
        
    }


    #endregion

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
    /*
    void Start()
    {
        Hull = GameObject.FindWithTag("Player");
        hull = Hull.transform.Find("turret mount");
    }
    */

    void Update()
    {
        if (Hull == null)
        {
            Hull = GameObject.FindWithTag("Player");
            hull = Hull.transform.Find("turret mount");
        }

        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)//sees if it is the correct player
        {
            return;
        }

        //locks the turret to the hull
        hullPos = hull.position;
        transform.position = hullPos;

        hullAngle = hull.rotation;
        
        if (photonView.IsMine)
        {
            //controls
            if (Input.GetKey("a"))//turn turret left
            {
                degreesFromHullNormal += rotateSpeed * Time.deltaTime;

            }
            else if (Input.GetKey("d"))//turn turret right
            {
                degreesFromHullNormal -= rotateSpeed * Time.deltaTime;

            }
            else if (Input.GetKeyDown("space")||GunFired)//fire gun
            {

                GameObject Shell = PhotonNetwork.Instantiate(this.shell.name, muzzlePoint.transform.position, muzzlePoint.transform.rotation);
                BulletScript Shellscript = Shell.GetComponent<BulletScript>();
                Shellscript.targetDirection = new Vector2(0, -turretRotation);
                GunFired = false;

            }
            /*
            else if (Input.GetKeyUp("space") || !GunFired) 
            {
                GunFired = false;

            }*/

        }

        turretRotation= hullAngle.eulerAngles.z;
        turretRotation += degreesFromHullNormal;

        Quaternion FinalturretRotation = Quaternion.identity;
        FinalturretRotation.eulerAngles = new Vector3(0,0,turretRotation);
        transform.localRotation = FinalturretRotation;
    }
}
