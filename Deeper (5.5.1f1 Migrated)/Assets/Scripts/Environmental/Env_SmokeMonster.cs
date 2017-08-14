using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env_SmokeMonster : Deeper_Component {

    public Transform Ops;

    private void Awake()
    {
        Initialize(1000);
    }

    public override void NormUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, Ops.position, .5f);
    }
}
