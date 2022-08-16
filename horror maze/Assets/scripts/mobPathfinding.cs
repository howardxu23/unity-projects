using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobPathfinding : MonoBehaviour
{
    public int speed = 1;
    public Vector3 targetWaypoint;
    public GameObject Mob;
    private GameObject waypointObject;
    private float rotateSpeed;
    private Vector3 lookDirection;
    public string lastChoosenDirection = "s";
    private string[] direction = { "n", "s", "e", "w" };
    private bool reachedDestonation = false;

    waypointScript waypoint;
    waypointScript northPoint;
    waypointScript southPoint;
    waypointScript eastPoint;
    waypointScript westPoint;
    void Start()
    {
        rotateSpeed = 50f;

    }


    void FixedUpdate()
    {
        RaycastHit hit;
        lookDirection = transform.TransformDirection(0, -90, 0);//looks forward

        if (waypointObject == null)//search for nearest waypoint with raycast if waypoint var is null
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            //raycast to nearest wall.
            if (Physics.Raycast(transform.position, lookDirection, out hit, Mathf.Infinity))
            {
                //print("hit" + hit.collider.gameObject);
                if (hit.collider.gameObject.tag == "waypoint")
                {
                    waypointObject = hit.collider.gameObject;
                    waypoint = waypointObject.GetComponent<waypointScript>();
                    targetWaypoint = waypoint.selfWaypointPos;
                }
            }
        }
        else//patrol behaviour
        {

            waypoint = waypointObject.GetComponent<waypointScript>();

            //sets the waypoint's asigned direction
            try
            {
                northPoint = waypoint.north.GetComponent<waypointScript>();
            }
            catch
            {
                northPoint = null;
            }
            try
            {
                southPoint = waypoint.south.GetComponent<waypointScript>();
            }
            catch
            {
                southPoint = null;
            }
            try
            {
                eastPoint = waypoint.east.GetComponent<waypointScript>();
            }
            catch
            {
                eastPoint = null;
            }
            try
            {
                eastPoint = waypoint.east.GetComponent<waypointScript>();
            }
            catch
            {
                eastPoint = null;
            }
            try
            {
                westPoint = waypoint.west.GetComponent<waypointScript>();
            }
            catch
            {
                westPoint = null;
            }
            int randDirection = Random.Range(0, 4);//chooses 1 of 4 directions

            //sets destonation to target waypoint
            if (reachedDestonation)
            {
                if (northPoint != null && direction[randDirection] == "n" && lastChoosenDirection != "s")
                {
                    targetWaypoint = northPoint.selfWaypointPos;
                    lastChoosenDirection = "n";
                    reachedDestonation = false;
                    waypointObject = waypoint.north;
                    Quaternion rot = new Quaternion();
                    rot.eulerAngles = new Vector3(-90,0,180);
                    transform.rotation = rot;
                }
                else if (southPoint != null && direction[randDirection] == "s" && lastChoosenDirection != "n")
                {
                    targetWaypoint = southPoint.selfWaypointPos;
                    lastChoosenDirection = "s";
                    reachedDestonation = false;
                    waypointObject = waypoint.south;
                    Quaternion rot = new Quaternion();
                    rot.eulerAngles = new Vector3(-90, 0, 0);
                    transform.rotation = rot;
                }
                else if (eastPoint != null && direction[randDirection] == "e" && lastChoosenDirection != "w")
                {
                    targetWaypoint = eastPoint.selfWaypointPos;
                    lastChoosenDirection = "e";
                    reachedDestonation = false;
                    waypointObject = waypoint.east;
                    Quaternion rot = new Quaternion();
                    rot.eulerAngles = new Vector3(-90, 0, -90);
                    transform.rotation = rot;
                }
                else if (westPoint != null && direction[randDirection] == "w" && lastChoosenDirection != "e")
                {
                    targetWaypoint = westPoint.selfWaypointPos;
                    lastChoosenDirection = "w";
                    reachedDestonation = false;
                    waypointObject = waypoint.west;
                    Quaternion rot = new Quaternion();
                    rot.eulerAngles = new Vector3(-90, 0, 90);
                    transform.rotation = rot;
                }
            }
            float step = speed * Time.deltaTime;//caculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, step);//moves to waypoint
            //check if arrived at the target
            if (Vector3.Distance(transform.position, targetWaypoint) < 0.001f)
            {
                reachedDestonation = true;
                
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position =new Vector3(15,1,23);
        }
    }
}

