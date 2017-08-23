using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Actual : Deeper_Component {

    [SerializeField] private List<Transform> OOIs = new List<Transform>();

    [SerializeField] private Transform sub_Ref;

    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float zoomRange;

    [SerializeField] private float minVertOffset;
    [SerializeField] private float maxVertOffset;


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
        dest = (hold / OOIs.Count) - Vector3.forward * minDistance;

        dest.y = lowest - Mathf.Lerp(-maxVertOffset, -minVertOffset, Vector3.Distance(sub_Ref.position, ProjectedLoc()) / zoomRange); ;
        dest.z = Mathf.Lerp(-maxDistance, -minDistance, Vector3.Distance(sub_Ref.position, ProjectedLoc()) / zoomRange);
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

    private Vector3 ProjectedLoc()
    {
        return transform.position + transform.forward * (-transform.position.z / Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad));
    }
}
