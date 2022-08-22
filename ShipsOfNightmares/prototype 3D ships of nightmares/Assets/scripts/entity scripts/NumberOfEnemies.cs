using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberOfEnemies : MonoBehaviour
{
    public int enemiesDead;
    private GameObject hatchPlace;
    public GameObject hatch;
    public GameObject railing;
    public GameObject belowShop;
    public GameObject shopUpper;
    public GameObject railing2;

    private Vector3 hatchTarget = new Vector3(4f, -0.1f, 2f);
    private float step = 0.05f;

    private void Start()
    {
        hatchPlace = GameObject.Find("Hatch place");
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesDead == 3)
        {
            hatchPlace.SetActive(false);
            hatch.transform.position = Vector3.MoveTowards(hatch.transform.position, hatchTarget, step);// Slowly move hatch. ~ SK
            railing.SetActive(false);
        }
        else if (enemiesDead == 8)
        {
            belowShop.SetActive(true);
            shopUpper.SetActive(true);
            railing2.SetActive(false);
        }
    }
}
