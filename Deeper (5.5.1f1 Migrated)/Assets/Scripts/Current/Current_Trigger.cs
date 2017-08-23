using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Current_Trigger : Deeper_Component {

    protected Collider myCol;

    protected virtual void Start () {
        Initialize(1000);
        myCol = GetComponent<Collider>();
        myCol.isTrigger = true;
	}

    protected virtual Vector3 GetForce(Vector3 otherPost, Vector3 triggerPos)
    {
        return Vector3.zero;
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.transform.root.gameObject.GetComponent<ICurrentable>() != null)
        {
            other.transform.root.gameObject.GetComponent<ICurrentable>().CurrentIs(GetForce(other.transform.root.position, transform.position));
        }
    }
}
