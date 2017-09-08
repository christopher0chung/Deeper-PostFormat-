using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Battery : Deeper_Component {

    public float startVal;

    private float _battPerc;
    private float _battIndPerc;
    private float _battIndPercApplied;
    private Vector3 _localScale;

    public Transform bar;
    public Controlled_Sub sub;

    private bool _b;
    private bool bottomedOut
    {
        get
        {
            return _b;
        }
        set
        {
            if (value != _b)
            {
                _b = value;
                if (_b)
                    sub.canDrive = false;
                else
                    sub.canDrive = true;
            }
        }
    }

	void Awake () {
        Initialize(2000);
        _localScale = startVal * Vector3.one / 2;
        NewChargeLevel(startVal);
        Deeper_EventManager.instance.Register<Deeper_Event_BattLvl>(BatteryHandler);
	}

    public override void _Unsub()
    {
        base._Unsub();
        Deeper_EventManager.instance.Unregister<Deeper_Event_BattLvl>(BatteryHandler);
    }

    public override void NormUpdate()
    {
        _battIndPercApplied = Mathf.Lerp(_battIndPercApplied, _battIndPerc, .005f);
        _localScale.y = _battIndPercApplied;
        bar.transform.localScale = _localScale;
        ChargeBasedEffects();
    }

    private void NewChargeLevel(float levelZeroToOne)
    {
        _battPerc = Mathf.Clamp01(levelZeroToOne);
        _battIndPerc = Mathf.Lerp(.05f, .5f, _battPerc);
    }

    private void BatteryHandler(Deeper_Event e)
    {
        Deeper_Event_BattLvl b = e as Deeper_Event_BattLvl;
        if (b != null)
            NewChargeLevel(b.level);
    }

    private void ChargeBasedEffects()
    {
        if (_battIndPercApplied <= .051f)
            bottomedOut = true;

        if (bottomedOut && _battIndPercApplied >= .499f)
            bottomedOut = false;
    }
}
