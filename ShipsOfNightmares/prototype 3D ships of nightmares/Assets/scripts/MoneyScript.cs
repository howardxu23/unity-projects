using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyScript : MonoBehaviour
{
    public static int moneyValue = 0;
    Text money;

    void Start()
    {
        moneyValue = 0;
        money = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        money.text = "" + moneyValue;
    }
}
