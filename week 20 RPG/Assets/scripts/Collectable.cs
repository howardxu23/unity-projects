using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // Start is called before the first frame update
    public string itemName;
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<Inventory>().Add(itemName, 1);

            Destroy(gameObject);
        }
        MessageDisplay disp = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
        disp.ShowMessage("You picked up a " + itemName, 2.0f);
    }
}
