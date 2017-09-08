using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_NavPingLight : Deeper_Component {

    public Light pingLight;

    private bool _pingStart;

    private bool _bigPingStart;

    private float _intensityStart;
    private float _intensityApplied;

    private void Awake()
    {
        Initialize(3000);
    }

    private void Start()
    {
        _intensityStart = pingLight.intensity;
        pingLight.intensity = _intensityApplied;
    }

    public override void NormUpdate()
    {
        if (_pingStart)
        {
            _intensityApplied = Mathf.Lerp(_intensityApplied, 0, .15f);
            pingLight.intensity = _intensityApplied;
            if (_intensityApplied <= .01f)
            {
                _intensityApplied = 0;
                _pingStart = false;
            }
        }

        if (_bigPingStart)
        {
            _intensityApplied = Mathf.Lerp(_intensityApplied, 0, .05f);
            pingLight.intensity = _intensityApplied;
            if (_intensityApplied <= .01f)
            {
                _intensityApplied = 0;
                _bigPingStart = false;
            }
        }

    }

    public void PingHit()
    {
        Debug.Log("Ping Hit");
        Deeper_EventManager.instance.Fire(new Deeper_Event_NavBeaconPing());
        _pingStart = true;
        _bigPingStart = false;
        _intensityApplied = _intensityStart;
        pingLight.intensity = _intensityApplied;
    }

    public void BigPingHit()
    {
        Debug.Log("Ping Hit");
        Deeper_EventManager.instance.Fire(new Deeper_Event_NavBeaconPing());
        _pingStart = false;
        _bigPingStart = true;
        _intensityApplied = _intensityStart;
        pingLight.intensity = _intensityApplied;
    }
}
