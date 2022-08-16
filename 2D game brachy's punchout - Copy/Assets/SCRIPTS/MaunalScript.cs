using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaunalScript : MonoBehaviour
{

    [SerializeField] GameObject manualUI;
    [SerializeField] Button manualButton;
    [SerializeField] Button ReturnButton;
    bool manualCLicked=false;
    bool returnclicked = false;
    // Start is called before the first frame update
    void Start()
    {
        manualUI.SetActive(false);
        manualButton.onClick.AddListener(manualButtonPressed);
        ReturnButton.onClick.AddListener(returnButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {
        if (manualCLicked == true)
        {

            manualCLicked = false;
            manualUI.SetActive(true);//enables help manual
        }
        if (returnclicked == true)
        {
            returnclicked = false;
            manualUI.SetActive(false);//diables help manual
        }
    }
    void manualButtonPressed()
    {
        manualCLicked = true;
    }
    void returnButtonPressed()
    {
        returnclicked = true;
    }
}
