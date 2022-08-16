using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//script by https://forum.unity.com/threads/how-do-i-detect-when-a-button-is-being-pressed-held-on-eventtype.352368/
public class startButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject userInterface;
    UIscript UI;
    public bool buttonPressed;
    // Start is called before the first frame update
    void Start()
    {

        UI = userInterface.GetComponent<UIscript>();
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        UI.gameOver = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}
