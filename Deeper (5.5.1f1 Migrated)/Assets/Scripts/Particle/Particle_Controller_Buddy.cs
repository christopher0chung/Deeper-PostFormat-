using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Particle_Controller))]

public class Particle_Controller_Buddy : Deeper_Component {

    private AudioSource _myAS;
    private Particle_Controller _myPC;

    private bool _i;
    private bool _isOn
    {
        get { return _i; }
        set
        {
            if (value != _i)
            {
                _i = value;
                if (_i)
                {
                    _myAS.Play();
                    _myAS.volume = .5f;
                }
                else
                    _myAS.volume = 0;
            }
        }
    }

    private void Awake()
    {
        Initialize(2000);
    }
	void Start () {
        _myAS = GetComponent<AudioSource>();
        _myPC = GetComponent<Particle_Controller>();
        Deeper_EventManager.instance.Register<Deeper_Event_Pause>(PauseHandler);
        _myAS.volume = 0;
	}

    public override void _Unsub()
    {
        base._Unsub();
        Deeper_EventManager.instance.Unregister<Deeper_Event_Pause>(PauseHandler);
    }

    private void PauseHandler(Deeper_Event e)
    {
        Deeper_Event_Pause p = e as Deeper_Event_Pause;
        if (p.isPaused)
            _myAS.Pause();
        else
            _myAS.UnPause();
    }

    public override void NormUpdate()
    {
        _isOn = _myPC.on;
    }
}
