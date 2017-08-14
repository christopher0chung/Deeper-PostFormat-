using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugging_AttitudeVis : MonoBehaviour {

    public Controlled_Sub myCS;

    void Update () {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, myCS._attitudeAngActual));
    }
}
