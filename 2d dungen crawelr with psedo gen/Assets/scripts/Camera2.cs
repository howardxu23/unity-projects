﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2: MonoBehaviour
{
    public Transform attachedPlayer;
    Camera thisCamera;
    public float blendAmount = 0.05f;
    // Use this for initialization
    void Start()
    {
        thisCamera = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 player = attachedPlayer.transform.position;
        Vector3 newCamPos = player * blendAmount
       + transform.position * (1.0f - blendAmount);
        transform.position = new Vector3(newCamPos.x, 2.75f,
        transform.position.z);
    }

}
