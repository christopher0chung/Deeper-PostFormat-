using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Actual : Deeper_Component {

    [SerializeField] private List<Transform> OOIs = new List<Transform>();
    [SerializeField] private float distance;

    private Vector3 dest;

    private void Awake()
    {
        Initialize(4500);
    }

    private Vector3 hold;

    private void Calculate()
    {
        hold = Vector3.zero;
        float lowest = 10000;
        foreach (Transform t in OOIs)
        {
            hold += t.position;
            if (t.position.y < lowest) lowest = t.position.y;
        }
        dest = (hold / OOIs.Count) - Vector3.forward * distance;
        dest.y = lowest - 5;
    }

    public override void PhysUpdate()
    {
        if (OOIs.Count > 0)
        {
            Calculate();
            transform.position = Vector3.Lerp(transform.position, dest, .1f);
        }
    }

    public void UpdateList(List<Transform> o)
    {
        OOIs.Clear();
        OOIs = o;
    }
}
