using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelowDeckWallsTint : MonoBehaviour
{
    private Color darkTint = new Color(0.5f, 0.5f, 0.5f);

    //public Material darkMat;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)// "child" is each wall tile of "below deck walls back". ~ SK
        {
            child.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", darkTint);

            //child.GetChild(0).GetComponent<MeshRenderer>().material = darkMat;
        }
    }
}
