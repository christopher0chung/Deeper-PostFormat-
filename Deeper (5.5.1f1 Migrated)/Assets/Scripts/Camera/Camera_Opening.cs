﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Opening : MonoBehaviour {

    [SerializeField] private Transform pivot;
    [SerializeField] private Transform tgt;

    private float xRot;
    private float yRot;

    private float xRate;
    private float yRate;

    private float xRateTgt;
    private float yRateTgt;

    private float dist;
    private float distRate;
    private float distRateTgt;

    private float timer;

    private void Start()
    {
        xRateTgt = Random.Range(-5.0f, 5.0f);
        yRateTgt = Random.Range(-5.0f, 5.0f);
        dist = Random.Range(11.7f, 17.7f);
        distRateTgt = Random.Range(11.7f, 17.7f);
    }

    void Update () {

        timer += Time.deltaTime;
        if (timer >= 10)
        {
            timer = 0;
            xRateTgt = Random.Range(-5.0f, 5.0f);
            yRateTgt = Random.Range(-15.0f, 15.0f);
            distRateTgt = Random.Range(11.7f, 17.7f);
        }

        xRate = Mathf.Lerp(xRate, xRateTgt, .03f);
        yRate = Mathf.Lerp(yRate, yRateTgt, .03f);

        xRot += xRate * Time.deltaTime;
        xRot = Mathf.Clamp(xRot, -30, 30);
        yRot += yRate * Time.deltaTime;

        transform.LookAt(tgt);
        pivot.localRotation = Quaternion.Euler(new Vector3(0, yRot, xRot));

        distRate = Mathf.Lerp(distRate, distRateTgt, .03f);
        dist += distRate;
        dist = Mathf.Clamp(dist, 11.7f, 17.7f);

        transform.localPosition = new Vector3(dist, 0, 0);
	}
}
