using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MessageDisplay : MonoBehaviour
{
    public Transform messageUI;
    Text textObject;
    string itemName;
    // Use this for initialization
    void Start()
    {
        textObject = messageUI.GetChild(1).GetComponent<Text>();
    }
    IEnumerator DoMessage(string message, float seconds)
    {
        messageUI.gameObject.SetActive(true);
        textObject.text = message;
        yield return new WaitForSeconds(seconds);
        messageUI.gameObject.SetActive(false);
    }
    public void ShowMessage(string message, float seconds)
    {
        StartCoroutine(DoMessage(message, seconds));
        MessageDisplay disp =GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
        disp.ShowMessage("You picked up a " + itemName, 2.0f);
    }
}