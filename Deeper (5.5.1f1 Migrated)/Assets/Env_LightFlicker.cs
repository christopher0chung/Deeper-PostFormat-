using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env_LightFlicker : Deeper_Component {

    public Light[] _l;
    private MeshRenderer _mAssign;
    public Material mOff;
    private Material _mOff;
    public Material mOn;
    private Material _mOn;
    private float _timer;
    private float _offTime;
    private float _onTime;

    private void Awake()
    {
        Initialize(1000);
    }

    void Start () {
        _timer = Random.Range(0, 3.0f);
        _offTime = Random.Range(.05f, .2f);
        _onTime = Random.Range(.05f, 3f);

        _mAssign = GetComponent<MeshRenderer>();
        _mOn = new Material(mOn);
        _mOff = new Material(mOff);
	}

    public override void NormUpdate()
    {
        _timer += Time.deltaTime;
        if(_timer <= _onTime)
        {
            foreach (Light l in _l)
            {
                l.enabled = true;
            }
            _mAssign.material = _mOn;
        }
        else if (_timer > _onTime && _timer <= _offTime + _onTime)
        {
            foreach (Light l in _l)
            {
                l.enabled = false;
            }
            _mAssign.material = _mOff;
        }
        else
        {
            _timer = 0;
            _offTime = Random.Range(.05f, .2f);
            _onTime = Random.Range(.05f, 3f);
        }
    }
}
