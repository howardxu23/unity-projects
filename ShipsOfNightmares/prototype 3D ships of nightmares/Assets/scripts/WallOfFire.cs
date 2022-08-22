using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfFire : MonoBehaviour
{
    private bool burnReady;

    private void Start()
    {
        burnReady = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && burnReady)
        {
            StartCoroutine(BurnCooldown(other));
        }
    }

    private IEnumerator BurnCooldown(Collider other)// Gives time for burn sound to play. ~ SK
    {
        burnReady = false;
        other.GetComponent<playerScript>().TakeDamage(30, "BurnDamage");
        yield return new WaitForSeconds(1f);// Wait. ~ SK

        burnReady = true;
    }
}
