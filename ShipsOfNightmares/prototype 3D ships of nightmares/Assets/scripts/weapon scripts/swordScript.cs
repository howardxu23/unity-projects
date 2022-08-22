using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class swordScript : MonoBehaviour
{
    public int swordDamage;
    public Text infoText;
    protected AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        //gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    IEnumerator TextDisappear()
    {
        infoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        infoText.gameObject.SetActive(false);
        gameObject.SetActive(false);
        //gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public void AddDamage(int damage)
    {
        if (MoneyScript.moneyValue < 75)
        {
            gameObject.SetActive(true);
            //gameObject.GetComponent<MeshRenderer>().enabled = false;
            infoText.gameObject.SetActive(true);
            infoText.text = "You do not have enough\ncorks to buy damage";
            StartCoroutine(TextDisappear());
            Debug.Log("Not enough Corks");
            return;
        }
        else
        {
            swordDamage += damage;
            MoneyScript.moneyValue -= 75;
            gameObject.SetActive(true);
            //gameObject.GetComponent<MeshRenderer>().enabled = false;
            infoText.gameObject.SetActive(true);
            infoText.text = "You bought damage for 75 corks";
            StartCoroutine(TextDisappear());
            audioManager.Play("SkeletonTalk");
            Debug.Log("Damage bought for 75 Corks");
        }
    }
    
}
