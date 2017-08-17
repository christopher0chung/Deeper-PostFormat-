using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ParticleSystem))]

public class Particle_Controller : Deeper_Component {

    public float particlesPerSecond;
    private float t;

	void Awake () {
        Initialize(1000);
	}

    private ParticleSystem _myPS;

    private void Start()
    {
        _myPS = GetComponent<ParticleSystem>();
    }

    private float timer;
    private bool _on;

    public override void NormUpdate()
    {
        if (_on)
        {
            timer += Time.deltaTime;

            t = 1 / (particlesPerSecond * .5f);

            if (timer > t)
            {
                _myPS.Emit(2);
                timer -= t;
            }
        }
    }

    public void OnOff(bool onTrueOffFalse)
    {
        _on = onTrueOffFalse;
    }
}
