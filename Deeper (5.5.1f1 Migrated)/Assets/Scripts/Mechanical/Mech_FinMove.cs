﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_FinMove : Deeper_Component {

    public Vector3 minEuler;
    public Vector3 maxEuler;

    private void Awake()
    {
        Initialize(3000);
    }

    private float timer;
    private float rollover;
    private float xVal;
    private float yVal;
    private float zVal;

    public override void NormUpdate()
    {
        timer += Time.deltaTime;
        if (timer > rollover)
        {
            timer = 0;
            rollover = Random.Range(.5f, 3);
            xVal = Random.Range(minEuler.x, maxEuler.x);
            yVal = Random.Range(minEuler.y, maxEuler.y);
            zVal = Random.Range(minEuler.z, maxEuler.z);
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(new Vector3(xVal, yVal, zVal)), .05f);
    }
}
