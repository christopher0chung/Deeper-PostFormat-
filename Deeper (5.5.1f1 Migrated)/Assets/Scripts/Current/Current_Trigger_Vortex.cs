using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Current_Trigger_Vortex : Current_Trigger {

    [SerializeField] private bool CCW;
    private float radMax;
    private float radThresh;
    [SerializeField] private float magMax;
    [SerializeField] private Vector3 vortexTuningAng;

    private float magnitude;
    private Vector3 F;
    private int ccwInt;

    protected override void Start () {
        base.Start();
        radMax = (myCol as SphereCollider).radius;
        radThresh = radMax * .4f;
	}

    protected override Vector3 GetForce(Vector3 otherPos, Vector3 triggerPos)
    {
        float dist = Vector3.Distance(otherPos, triggerPos);
        if (dist >= radThresh)
            magnitude = Mathf.Lerp(magMax, 0, ((dist - radThresh) / (radMax - radThresh)));
        else
            magnitude = 0;

        if (CCW)
            ccwInt = 1;
        else
            ccwInt = -1;

        F = Vector3.Normalize(Quaternion.Euler(ccwInt * vortexTuningAng) * (otherPos - triggerPos));

        return magnitude * F;
    }
}
