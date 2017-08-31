using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_TailMove : MonoBehaviour {

    private float timer;
    private float rollover;
    private float xVal;
    private float yVal;

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer > rollover)
        {
            timer = 0;
            rollover = Random.Range(.5f, 3);
            xVal = Random.Range(-10f, 10f);
            yVal = Random.Range(-10f, 10f);
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(new Vector3(xVal, yVal, 0)), .05f);
    }
}
