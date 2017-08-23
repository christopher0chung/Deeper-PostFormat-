using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Particle_Controller))]
public class Current_Visualizer : Deeper_Component, ICurrentable {

    private ParticleSystem _myPS;
    private Rigidbody _myRB;
    private Particle_Controller _myPC;
    private Transform myP;

	void Start () {
        Initialize(4000);
        _myRB = GetComponent<Rigidbody>();
        _myPS = GetComponent<ParticleSystem>();
        _myPC = GetComponent<Particle_Controller>();
	}

    public override void PhysUpdate()
    {
        _myRB.AddForce(_currentForce);
        _currentForce = Vector3.zero;
    }

    private Vector3 _currentForce;

    public void CurrentIs (Vector3 current)
    {
        _currentForce += current;
    }
}
