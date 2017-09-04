using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlled_SubVis : Deeper_Component {

    void Awake()
    {
        Initialize(3000);
    }

    public Controlled_Sub myCS;

    public override void NormUpdate()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, myCS._headingAngActual, myCS._attitudeAngActual));
    }
}
