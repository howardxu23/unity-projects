using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypointScript : MonoBehaviour
{
    //links the waypoints together to form a path
    public GameObject north;
    public bool IsOnewayNorth;
    public bool isDoorNorth;
    public GameObject south;
    public bool IsOnewaySouth;
    public bool isDoorSouth;
    public GameObject east;
    public bool IsOnewayEast;
    public bool isDoorEast;
    public GameObject west;
    public bool IsOnewayWest;
    public bool isDoorWest;

    public Vector3 selfWaypointPos;
    void Start()
    {
        //sets selfwaypoint pos to it current location on the map
        selfWaypointPos = transform.position;
    }

    // Update is called once per frame

}
