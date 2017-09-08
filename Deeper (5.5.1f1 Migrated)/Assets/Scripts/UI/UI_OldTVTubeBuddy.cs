using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OLDTVTube))]
[RequireComponent(typeof(OLDTVScreen))]

public class UI_OldTVTubeBuddy : MonoBehaviour {

    public GameObject fuzzTarget;

    //private OLDTVTube _myT;
    private OLDTVScreen _myS;

    private bool _fuzz;
    private float _noiseMag;

    private Deeper_RolloverInt linePos;

    private FSM<UI_OldTVTubeBuddy> _fsm;

    private bool _transitionStaticInProgress;

    private bool _deathInProgress;

	void Start () {
        //_myT = GetComponent<OLDTVTube>();
        _myS = GetComponent<OLDTVScreen>();
        linePos = new Deeper_RolloverInt(0, 0, 1000);
        _fsm = new FSM<UI_OldTVTubeBuddy>(this);
        _fsm.TransitionTo<FuzzOff>();

        Deeper_EventManager.instance.Register<Deeper_Event_CamSwitch>(SwitchStaticHandler);
        Deeper_EventManager.instance.Register<Deeper_Event_Death>(DeathStaticHandler);
        Deeper_EventManager.instance.Register<Deeper_Event_LevelUnload>(Unregister);
    }

    void Update () {
        if (!_deathInProgress)
        {
            if (!_transitionStaticInProgress)
                _fsm.Update();
            else
                SwitchStaticUpdate();
        }
        else
        {
            _fsm.Update();
        }

        linePos.intVal += 2;
        _myS.staticVertical = ((float) linePos.intVal) / 1000;

        //if (Input.GetKeyDown(KeyCode.Space))
        //    _fsm.TransitionTo<FuzzPositional>();
	}

    #region Handlers
    private void Unregister(Deeper_Event e)
    {
        Deeper_EventManager.instance.Unregister<Deeper_Event_LevelUnload>(Unregister);
        Deeper_EventManager.instance.Unregister<Deeper_Event_Death>(DeathStaticHandler);
        Deeper_EventManager.instance.Unregister<Deeper_Event_CamSwitch>(SwitchStaticHandler);
    }

    private void SwitchStaticHandler(Deeper_Event e)
    {
        //Debug.Log("Static Event Fired");
        _transitionStaticInProgress = true;
        _switchStaticMagnitude = .5f;
    }

    private void DeathStaticHandler(Deeper_Event e)
    {
        _deathInProgress = true;
        _fsm.TransitionTo<FuzzDeath>();
    }
    #endregion

    private float _switchStaticMagnitude;

    private void SwitchStaticUpdate()
    {
        _switchStaticMagnitude = Mathf.Lerp(_switchStaticMagnitude, 0, .18f);
        _myS.screenSaturation = _myS.staticMagnetude = _myS.noiseMagnetude = _switchStaticMagnitude;
        //_myS.chromaticAberrationMagnetude = _switchStaticMagnitude / 2;

        if (_switchStaticMagnitude <= .005f)
            _transitionStaticInProgress = false;
    }
    private void TenSec()
    {
        for (int i = 0; i < 10; i++)
        {
            Invoke("PlayStatic", i);
        }
    }

    private void PlayStatic()
    {
        Deeper_ServicesLocator.instance.SFXManager.PlaySoundPauseable(SFX.Static, 1);
    }

    #region States

    public class State_Base : FSM<UI_OldTVTubeBuddy>.State
    {
        public float timer;
        public float rollover;
    }

    private class FuzzNormal : State_Base
    {
        public override void OnEnter()
        {
            timer = 0;
            rollover = Random.Range(.3f, .6f);
            Deeper_ServicesLocator.instance.SFXManager.PlaySoundPauseable(SFX.Static, .01f, .2f);
        }

        public override void Update()
        {
            float n = Mathf.Lerp(Context._myS.noiseMagnetude, .08f, .05f);
            Context._myS.staticMagnetude = Context._myS.noiseMagnetude = n;
            Context._myS.chromaticAberrationMagnetude = n / 2;

            timer += Time.deltaTime;
            if (timer > rollover)
                TransitionTo<FuzzOff>();
        }
    }

    public class FuzzOff : State_Base
    {
        public override void OnEnter()
        {
            timer = 0;
            rollover = Random.Range(1.5f, 20f);
        }

        public override void Update()
        {
            float n = Mathf.Lerp(Context._myS.noiseMagnetude, 0, .08f);
            Context._myS.staticMagnetude = Context._myS.noiseMagnetude = n;
            Context._myS.chromaticAberrationMagnetude = n / 2;

            timer += Time.deltaTime;
            if (timer > rollover)
                TransitionTo<FuzzNormal>();
        }
    }

    public class FuzzPositional : State_Base
    {
        private Vector3 _staticPoint;
        private float _appliedStaticStrength;
        private float _calculatedStaticStrength;
        private float _interferenceRangeMax;

        public override void OnEnter()
        {
            timer = 0;
            rollover = 2.3f;
            _staticPoint = Context.fuzzTarget.transform.position;
            _interferenceRangeMax = Random.Range(10, 30);
            Deeper_ServicesLocator.instance.SFXManager.PlaySoundPauseable(SFX.Static, .01f);
        }

        public override void Update()
        {
            _calculatedStaticStrength = .13f * Mathf.Clamp01((_interferenceRangeMax - Vector3.Distance(Context.fuzzTarget.transform.position, _staticPoint))/ _interferenceRangeMax);

            timer += Time.deltaTime;
            if (timer < rollover)
                _appliedStaticStrength = Mathf.Lerp(0, _calculatedStaticStrength, Deeper_ServicesLocator.CubicEaseIn(timer/rollover));
            else
                _appliedStaticStrength = Mathf.Lerp(_appliedStaticStrength, _calculatedStaticStrength, .01f);

            Context._myS.staticMagnetude = Context._myS.noiseMagnetude = _appliedStaticStrength;
            Context._myS.chromaticAberrationMagnetude = _appliedStaticStrength / 2;

            if (_calculatedStaticStrength <= .005f)
                TransitionTo<FuzzOff>();
        }
    }

    private class FuzzDeath : State_Base
    {
        public override void OnEnter()
        {
            Context.TenSec();
        }

        public override void Update()
        {
            float n = Mathf.Lerp(Context._myS.noiseMagnetude, .8f, .05f);
            Context._myS.staticMagnetude = Context._myS.noiseMagnetude = n;
            Context._myS.chromaticAberrationMagnetude = n / 2;
        }
    }

    #endregion
}
