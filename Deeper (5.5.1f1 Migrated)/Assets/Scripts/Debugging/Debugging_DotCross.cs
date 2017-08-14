using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Debugging_DotCross : MonoBehaviour {

    [Header("Input")]
    public Vector3 v1;
    public Vector3 v2;

    public float X;
    public float Y;

    [Header("Output")]
    [SerializeField] private float _dot;
    [SerializeField] private Vector3 _cross;
    [SerializeField] private float _aTan;

	void Update () {

        _dot = Vector3.Dot(Vector3.Normalize(v1), Vector3.Normalize(v2));
        _cross = Vector3.Cross(v1, v2);

        _aTan = ((Mathf.Atan2(Y, X) * Mathf.Rad2Deg) + 360) % 360;
	}
}
