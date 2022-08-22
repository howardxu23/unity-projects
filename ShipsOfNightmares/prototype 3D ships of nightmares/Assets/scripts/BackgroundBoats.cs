using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBoats : MonoBehaviour
{
    private GameObject boat0;
    private Vector3 boat0StartPos;
    //private Quaternion boat0StartRot;
    //private Quaternion boat0TargetRot;

    private GameObject boat1;
    private Vector3 boat1StartPos;

    public float sailTime = 5f;
    //public float waitTime = 1f;

    private bool boat0Sail;
    private bool boat1Sail;
    float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        boat0 = transform.GetChild(0).gameObject;
        boat0StartPos = boat0.transform.position;
        //boat0StartRot = boat0.transform.rotation;
        //boat0TargetRot = Quaternion.Euler(0f, 90f, 0f);

        boat1 = transform.GetChild(1).gameObject;
        boat1StartPos = boat1.transform.position;

        //BoatSailing();
        boat0Sail = true;
    }

    void Update()
    {
        if (boat0Sail)
        {
            if (elapsedTime < sailTime)
            {
                boat0.transform.position = Vector3.Lerp(boat0StartPos, boat1StartPos, (elapsedTime / sailTime));

                elapsedTime += Time.deltaTime;
            }
            else
            {
                boat0.transform.position = boat0StartPos;
                elapsedTime = 0;
                boat0Sail = false;
                boat1Sail = true;
            }
        }

        if (boat1Sail)
        {
            if (elapsedTime < sailTime)
            {
                boat1.transform.position = Vector3.Lerp(boat1StartPos, boat0StartPos, (elapsedTime / sailTime));

                elapsedTime += Time.deltaTime;
            }
            else
            {
                boat1.transform.position = boat1StartPos;
                elapsedTime = 0;
                boat1Sail = false;
                boat0Sail = true;
            }
        }
    }

    /*void BoatSailing()
    {
        StartCoroutine(SailingSequence());
    }

    IEnumerator SailingSequence()
    {
        StartCoroutine(MoveOverTime(boat0, boat1StartPos, sailTime));// Move boat0 to boat1's start pos (across the screen). ~ SK
        yield return new WaitForSeconds(sailTime);// Wait. ~ SK
        boat0.transform.position = boat0StartPos;// Reset boat position. ~ SK
        yield return new WaitForSeconds(waitTime);

        StartCoroutine(MoveOverTime(boat1, boat0StartPos, sailTime));// Move boat1 to boat0's start pos (across the screen). ~ SK
        yield return new WaitForSeconds(sailTime);
        boat1.transform.position = boat1StartPos;// Reset boat position. ~ SK
        yield return new WaitForSeconds(waitTime);

        BoatSailing();// Loop. ~ SK
    }

    public IEnumerator MoveOverTime(GameObject objectToMove, Vector3 endPos, float moveTime)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;

        while (elapsedTime < moveTime)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, endPos, (elapsedTime / moveTime));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = endPos;
    }
    */

    /*
    // Was useful code. ~ SK
    public IEnumerator RotateOverTime(GameObject objectToRot, Quaternion endRot, float rotTime)
    {
        float elapsedTime = 0;
        Quaternion startingRot = objectToRot.transform.rotation;

        while (elapsedTime < rotTime)
        {
            objectToRot.transform.rotation = Quaternion.Lerp(startingRot, endRot, (elapsedTime / rotTime));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToRot.transform.rotation = endRot;
    }

    public IEnumerator OrbitOverTime(GameObject orbital, float turn, float radius, float orbitTime)
    {
        float elapsedTime = 0;
        float angle = 0f;
        Vector3 startingPos = orbital.transform.position;
        float turnSpeed = turn / (orbitTime * 60);

        while (elapsedTime < orbitTime)
        {
            angle += turnSpeed * Time.deltaTime;
            var offset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius;
            transform.position = startingPos + offset;

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    */
}
