using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDebris : MonoBehaviour
{
    private int children;// Track number of child objects (debris pieces). ~ SK
    private bool cleanUp;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            children++;
        }

        children += -1;// Otherwise OoB exception. ~ SK

        StartCoroutine(DelayCleanUp());
    }

    IEnumerator DelayCleanUp()
    {
        yield return new WaitForSeconds(2f);// Wait 2 seconds. ~ SK

        cleanUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (cleanUp)
        {
            if (children >= 0)
            {
                transform.GetChild(children).gameObject.SetActive(false);// SetActive(false) is less instense than Destroy(). ~ SK
                children--;
            }
            else// Once all debris pieces have been deactivated. ~ SK
            {
                Destroy(gameObject);
            }
        }
    }
}
