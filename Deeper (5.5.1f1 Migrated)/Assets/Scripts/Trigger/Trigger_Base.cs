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
    public float timeInTrigger;

    private float timer;
    private int _numPresent;

    private InteractionEffects_Base _myIEB;

    private bool _triggered;

    private void Start()
    {
        _numPresent = 0;
        _myIEB = GetComponent<InteractionEffects_Base>();
    }

    private void _COICheckIn(Collider c)
    {
        for (int i = 0; i < rbOfInterest.Length; i++)
        {
            if (c.attachedRigidbody == rbOfInterest[i])
            {
                _numPresent++;
            }
        }
    }

    private void _CheckedTimerToTrigger()
    {
        if (rbOfInterest.Length == _numPresent)
            timer += Time.fixedDeltaTime;
        else
            timer = 0;

        if (timer >= timeInTrigger)
        {
            _triggered = true;
            _myIEB.OnInteractedSuccess();
            this.enabled = false;
        }
    }

    private void _COIClear()
    {
        _numPresent = 0;
    }

    public void OnTriggerStay(Collider other)
    {
        if (!_triggered)
            _COICheckIn(other);
    }

    public override void PhysUpdate()
    {
        _CheckedTimerToTrigger();
        _COIClear();
    }
}