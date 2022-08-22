using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScript : MonoBehaviour
{
    public GameObject brokenVersion;

    protected AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)// Arif
    {
        if (other.gameObject.layer == 10)
        {
            audioManager.Play("BarrelBreak");
            Instantiate(brokenVersion, transform.position, transform.rotation);
            Debug.Log("Barrel destroyed!");
            Destroy(gameObject);
            MoneyScript.moneyValue += 5;
        }
    }
}
