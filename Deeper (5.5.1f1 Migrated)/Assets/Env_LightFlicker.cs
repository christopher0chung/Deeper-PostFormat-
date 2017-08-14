using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]

public class Env_LightFlicker : Deeper_Component {

    private Light _l;
    private float _timer;
    private float _offTime;
    private float _onTime;

    private void Awake()
    {
        Initialize(1000);
    }

    void Start () {
        _l = GetComponent<Light>();
        _timer = Random.Range(0, 3.0f);
        _offTime = Random.Range(.05f, .2f);
        _onTime = Random.Range(.05f, 3f);
	}

    public override void NormUpdate()
    {
        _timer += Time.deltaTime;
        if(_timer <= _onTime)
        {
            _l.enabled = true;
        }
        else if (_timer > _onTime && _timer <= _offTime + _onTime)
        {
            _l.enabled = false;
        }
        else
        {
            _timer = 0;
            _offTime = Random.Range(.05f, .2f);
            _onTime = Random.Range(.05f, 3f);
        }
    }
}
