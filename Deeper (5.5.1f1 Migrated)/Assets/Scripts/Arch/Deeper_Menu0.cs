using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMPro.TextMeshPro))]
public class Deeper_Menu0 : Deeper_Component {
    [Header ("Set Layer to UI")]
    public GameObject cameraVisible;
    public Deeper_Menu0 myPathUp;

    protected TMPro.TextMeshPro _title;

    protected virtual void Awake () {
        Initialize(1000);
	}

    protected virtual void Start()
    {
        transform.parent.SetParent(cameraVisible.transform);
        _title = GetComponent<TMPro.TextMeshPro>();
    }

    public virtual void TurnOn()
    {

    }
}
