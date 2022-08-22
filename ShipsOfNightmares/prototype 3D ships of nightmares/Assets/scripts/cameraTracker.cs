using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTracker : MonoBehaviour
{
    private Transform attachedPlayer;
    private Vector3 cameraOffset;

    // Use this for initialization
    void Start()
    {
        attachedPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        cameraOffset = transform.position;// Offset is based on camera starting pos.

        Physics.IgnoreLayerCollision(7, 10);
        Physics.IgnoreLayerCollision(6, 11);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        transform.position = attachedPlayer.position + cameraOffset + new Vector3(0, 0, -3f);

        //if (attachedPlayer.position.y > 0) StartCoroutine(MoveOverTime(gameObject, attachedPlayer.position + cameraOffset + new Vector3(0, 0, -3f), 2f));
        //else StartCoroutine(MoveOverTime(gameObject, attachedPlayer.position + cameraOffset, 2f));

        if (attachedPlayer.position.y < 0) StartCoroutine(MoveOverTime(cameraOffset, new Vector3(0, 0, 3f), 2f));
        */
        if (attachedPlayer.position.y > 0) transform.position = attachedPlayer.position + cameraOffset;
        else transform.position = attachedPlayer.position + cameraOffset + new Vector3(0, 0, +3f);
    }

    /*public IEnumerator MoveOverTime(GameObject objectToMove, Vector3 endPos, float moveTime)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;

        while (elapsedTime < moveTime)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, endPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = endPos;
    }*/
}
