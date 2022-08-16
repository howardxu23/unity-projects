using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trophyScript : MonoBehaviour
{

    public GameObject triggeredObject;
    public bool isTriggered = false;
    public GameObject trophy;

    void Update()
    {
        if (isTriggered)
        {
            triggeredObject.SetActive(false);
            trophy.SetActive(false);
        }
    }
}
