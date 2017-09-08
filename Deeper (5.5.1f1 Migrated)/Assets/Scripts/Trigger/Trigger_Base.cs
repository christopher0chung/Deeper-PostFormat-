using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionEffects_Base))]
[RequireComponent(typeof(Rigidbody))]

public class Trigger_Base : Deeper_Component {

	void Awake () {
        Initialize(1000);
	}

    public Rigidbody[] rbOfInterest;
    public List<bool> rbPresent;

    public float timeInTrigger;

    private float timer;
    //private int _numPresent;

    private InteractionEffects_Base _myIEB;

    //private bool _triggered;

    private void Start()
    {
        //_numPresent = 0;
        _myIEB = GetComponent<InteractionEffects_Base>();
        rbPresent = new List<bool>();
        foreach (Rigidbody rb in rbOfInterest)
        {
            rbPresent.Add(false);
        }
    }

    public override void PhysUpdate()
    {
        // assess who's inside
        for (int i = 0; i < rbOfInterest.Length; i++)
        {
            foreach(Collider c in GetComponentsInChildren<Collider>())
            {
                if (c.bounds.Contains(rbOfInterest[i].transform.position))
                {
                    rbPresent[i] = true;
                }
            }
        }
        
        // kick out of physUpdate if any not present
        for (int i = 0; i < rbPresent.Count; i++)
        {
            if (rbPresent[i] == false)
            {
                for (int j = 0; j < rbPresent.Count; j++)
                {
                    rbPresent[j] = false;
                }
                return;
            }
        }

        timer += Time.fixedDeltaTime;

        if (timer >= timeInTrigger)
        {
            //_triggered = true;
            _myIEB.OnInteractedSuccess();
            this.enabled = false;
        }
    }
}