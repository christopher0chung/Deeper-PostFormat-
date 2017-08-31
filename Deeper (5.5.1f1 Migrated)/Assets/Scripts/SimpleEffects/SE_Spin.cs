using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_Spin : MonoBehaviour {

    public Axis myAxis;
    public float rate;

    private float x;

	void Update () {

        x += rate * Time.deltaTime;
        if (myAxis == Axis.X)
            transform.localRotation = Quaternion.Euler(new Vector3(x, 0, 0));
        else if (myAxis == Axis.Y)
            transform.localRotation = Quaternion.Euler(new Vector3(0, x, 0));
        else
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, x));
    }
}

public enum Axis { X, Y, Z }
