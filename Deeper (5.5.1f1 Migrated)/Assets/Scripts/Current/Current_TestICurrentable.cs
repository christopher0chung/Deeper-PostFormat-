using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current_TestICurrentable : MonoBehaviour, ICurrentable {

    public void CurrentIs(Vector3 current)
    {
        Debug.Log(current);
        Debug.DrawLine(transform.position, transform.position + current);
    }
}
