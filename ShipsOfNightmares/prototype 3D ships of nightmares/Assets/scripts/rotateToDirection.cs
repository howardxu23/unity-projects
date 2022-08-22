using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateToDirection : MonoBehaviour
{
    [SerializeField]
    private GameObject ParentNode;//parent node
    private Transform transformparentNode;
    private Rigidbody ParentRigidbody;
    Vector3 movmentDirection;
    //[SerializeField]
    //private Transform modelTransform;//the actual model

    // Start is called before the first frame update
    void Start()
    {
        ParentRigidbody = ParentNode.GetComponent<Rigidbody>();
        //modelTransform = this.transform;
    }

    public void FixedUpdate()
    {
        movmentDirection = new Vector3(-ParentRigidbody.velocity.x, 0, -ParentRigidbody.velocity.z);
        //movmentDirection = ParentRigidbody.velocity;
        if (movmentDirection != Vector3.zero)//checks if it is moving
        {
            transform.forward = movmentDirection;//points player in direction of travel
        }
    }
}
