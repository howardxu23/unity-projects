using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonScript : MonoBehaviour
{
    [SerializeField]
    private GameObject firingSystem;
    public bool firing = false;
    //private static Vector3 originalPos;// Static causes all cannons to move to same position. ~ SK
    private Vector3 originalPos;

    public void Start()
    {
        originalPos = gameObject.transform.position;
    }
    public void Update()
    {
        if (firing)//recoils and fires a projectile
        {
            firingSystem.GetComponent<gunScript>().FireGun = true;
            transform.Translate(new Vector3(0, 0, -0.7f));
            firing = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, originalPos, 1 * Time.deltaTime);
    }
}
