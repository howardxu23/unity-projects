using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoneCam : MonoBehaviour
{
    //public Transform zoneCamera;
    public GameObject zoneCamera;
    public cameraTracker ct;
    public float speed = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            float camSpeed = speed * Time.deltaTime;
            ct.transform.position = Vector3.MoveTowards(ct.transform.position, zoneCamera.transform.position, camSpeed);
            zoneCamera.gameObject.SetActive(true);
            ct.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ct.gameObject.SetActive(true);
            zoneCamera.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
