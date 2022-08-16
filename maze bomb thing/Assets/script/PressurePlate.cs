using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject triggeredObject;
    public bool isTriggered = false;

    // Update is called once per frame
    void Update()//waits till bomb is placed on it
    {
        if (isTriggered)
        {
            triggeredObject.SetActive(false);
        }
    }
}
