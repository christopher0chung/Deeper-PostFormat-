using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Virtual : Deeper_Component {

    public float distance;
    public float range;

    private void Awake()
    {
        Initialize(4000);
    }

    void Start () {
        _nextInterval = Random.Range(.8f, 1.1f);
        _offsetOld = transform.localPosition;
	}

    private float _timer;
    private Vector3 _offsetOld;
    private Vector3 _offset;
    private float _nextInterval;

    private void TimedOffsetCalc()
    {
        _timer += Time.deltaTime;
        if (_timer >= _nextInterval)
        {
            _nextInterval = Random.Range(.8f, 2.5f);
            _timer = 0;
            _easeTimer = 0;
            _offsetOld = _offset;
            _offset = Random.insideUnitSphere * range + Vector3.forward * -distance;
        }
    }

    private float _easeTimer;

    public override void PostUpdate()
    {
        TimedOffsetCalc();
        _easeTimer += Time.deltaTime / (_nextInterval);
        transform.localPosition = Vector3.Lerp(_offsetOld, _offset, (Deeper_ServicesLocator.SineEaseInOut(_easeTimer)));
    }
}
