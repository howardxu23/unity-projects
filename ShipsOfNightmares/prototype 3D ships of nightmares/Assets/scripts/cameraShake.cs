using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour
{
    IEnumerator CamShake(float force)
    {
        bool elapsed = true;

        while (elapsed)
        {
            float x = Random.Range(-1f, 1f) * force;
            float y = Random.Range(-1f, 1f) * force;

            transform.localPosition = new Vector3(x, y, -2f);

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CamShake(0.05f));
    }
}
