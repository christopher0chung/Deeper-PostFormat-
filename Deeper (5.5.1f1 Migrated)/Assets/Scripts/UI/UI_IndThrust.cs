using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_IndThrust : Deeper_Component
{

    [Header("References")]
    public Transform ind;
    public Transform p1;
    public Transform p2;

    [Header("Values")]
    public Vector3 max;
    public Vector3 min;

    private Controlled_Sub _sub;

    private Vector3 _p1Offset;
    private Vector3 _p2Offset;

    private float _lerpPerc;


    void Awake()
    {
        Initialize(4900);
    }
    void Start()
    {
        _sub = GetComponent<Controlled_Sub>();
        _p1Offset = p1.transform.localPosition - Vector3.Lerp(min, max, .5f);
        _p2Offset = p2.transform.localPosition - Vector3.Lerp(min, max, .5f);
        _lerpPerc = .5f;
    }

    public override void PostUpdate()
    {
        p1.transform.localPosition = Vector3.Lerp(min, max, (_sub._maneuveringFloats[0] + 1) / 2) + _p1Offset;
        p2.transform.localPosition = Vector3.Lerp(min, max, (_sub._maneuveringFloats[1] + 1) / 2) + _p2Offset;

        _lerpPerc = Mathf.Lerp(_lerpPerc, (((_sub._maneuveringFloats[0] + _sub._maneuveringFloats[1]) / 2) + 1) / 2, .02f);
        ind.transform.localPosition = Vector3.Lerp(min, max, _lerpPerc);
    }
}
