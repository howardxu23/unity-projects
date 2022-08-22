using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{


    [SerializeField]
    private Transform TargetObjectTransform;


    // Start is called before the first frame update


    public void FixedUpdate()
    {

        gameObject.transform.LookAt(TargetObjectTransform);
    }

}
