using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(Rigidbody))] 
[RequireComponent(typeof(AudioSource))]

public class Controlled_Sub : Deeper_Component, ICurrentable {

    private FSM<Controlled_Sub> _fsm;

    private AudioSource _myAS;

    public float moveForceScalar;
    public float attitudeSpeedScalar;
    public float buoyancyForceScalar;
    public float turnNormalizedTime;

    public float lightWheelAngSpeed;

    public bool canDrive;

    public GameObject prop;
    public Transform lightWheel;

    public Particle_Controller fwdBallast;
    public Particle_Controller aftBallast;

    #region Private Variables
    private Rigidbody _rigidbody;

    private Vector3 _p0MoveVector;
    private Vector3 _p1MoveVector;

    public float[] _maneuveringFloats = new float[4];
    // 0 - p0 LR Thrust
    // 1 - p1 LR Thrust
    // 2 - p0 UD Input
    // 3 - p1 UD Input
    private float _attitudeAngTarget;
    [HideInInspector] public float _attitudeAngActual;
    [HideInInspector] public float _headingAngActual;

    private float _deltaBuoyancyForce;
    [HideInInspector] public float _deltaBuoyancyForceApplied { get; private set; }
    private float _linearThrust;
    private Vector3 totalForce;
    private float angClamp = 25;

    private Particle_Controller _PSCFwd;
    private Particle_Controller _PSCAft;

    private float _lightWheelAng;
    private float _lightFloatUpDownP1;
    private float _lightFloatUpDownP2;

    private bool _fwdBallastOpen;
    private bool _aftBallastOpen;
    private bool _negBuoyancy;
    private bool _pitchUp;
    private bool _pitchDown;
    #endregion

    #region Deeper_Component Functions
    private void Awake()
    {
        Initialize(3000);
    }

    void Start () {
        //Debug.Log("Sub Start Run");
        _fsm = new FSM<Controlled_Sub>(this);
        _fsm.TransitionTo<FacingRight>();

        _rigidbody = GetComponent<Rigidbody>();

        _PSCFwd = prop.transform.Find("Fwd").GetComponent<Particle_Controller>();
        _PSCAft = prop.transform.Find("Aft").GetComponent<Particle_Controller>();

        _myAS = GetComponent<AudioSource>();
        _myAS.volume = 0;
    }

    private Vector3 _rotAng;
    private float _spinRate;

    public override void NormUpdate()
    {
        //Debug.Log("NormUpdate Running");
        _ReadInputFloats();
        _fsm.Update();

        _PropAnim();

        //if (Input.GetKeyDown(KeyCode.Space))
        //    _fsm.TransitionTo<TurnToLeft>();
    }

    public override void PhysUpdate()
    {
        //_rigidbody.AddForce(moveForceScalar * (_p0MoveVector + _p1MoveVector));
        _ApplyClamps();
        _LerpToTargets();
        _VelocityCompBuoyancy();
        _AssessMotorAudio();

        if (canDrive)
            _rigidbody.AddForce(Quaternion.Euler(0, _headingAngActual, _attitudeAngActual) * Vector3.right * _linearThrust + _deltaBuoyancyForceApplied * Vector3.up);

        _rigidbody.AddForce(_currentForce);
        //Debug.Log(_currentForce);
        _currentForce = Vector3.zero;
    }
    #endregion

#region Interface

    private Vector3 _currentForce;

    public void CurrentIs(Vector3 current)
    {
        _currentForce += current;
    }

#endregion

#region Bubbles and Animation

    private void _PropAnim()
    {
        _spinRate = Mathf.Lerp(_spinRate, _linearThrust, .05f);
        _rotAng.z -= _spinRate / 20;
        _rotAng.z = (_rotAng.z + 360) % 360;
        prop.transform.localRotation = Quaternion.Euler(_rotAng);

        if (_spinRate > 100)
        {
            _PSCFwd.OnOff(true);
            _PSCAft.OnOff(false);
        }
        else if (_spinRate < -100)
        {
            _PSCFwd.OnOff(false);
            _PSCAft.OnOff(true);
        }
        else
        {
            _PSCFwd.OnOff(false);
            _PSCAft.OnOff(false);
        }
    }
    
    #endregion

#region Context Functions
    private void _ReadInputFloats()
    {
        _maneuveringFloats[0] = ReInput.players.GetPlayer(0).GetAxis("Sub Horizontal");
        _maneuveringFloats[1] = ReInput.players.GetPlayer(1).GetAxis("Sub Horizontal");
        _maneuveringFloats[2] = ReInput.players.GetPlayer(0).GetAxis("Sub Vertical");
        _maneuveringFloats[3] = ReInput.players.GetPlayer(1).GetAxis("Sub Vertical");

        _lightFloatUpDownP1 = ReInput.players.GetPlayer(0).GetAxis("Sub Light Vertical");
        _lightFloatUpDownP2 = ReInput.players.GetPlayer(1).GetAxis("Sub Light Vertical");
    }

    private void _ApplyClamps()
    {
        _attitudeAngTarget = Mathf.Clamp(_attitudeAngTarget, -angClamp, angClamp);
        _deltaBuoyancyForce = Mathf.Clamp(_deltaBuoyancyForce, -600, 600);
    }

    private void _LerpToTargets()
    {
        _attitudeAngActual = Mathf.Lerp(_attitudeAngActual, _attitudeAngTarget, .04f);
        _deltaBuoyancyForce *= .99f;
    }

    private void _VelocityCompBuoyancy()
    {
        Vector3 velocityNet = GetComponent<Rigidbody>().velocity;
        float velocityForeAft = Vector3.Dot(velocityNet, Quaternion.Euler(0, 0, _attitudeAngActual) * Vector3.right) * Vector3.Magnitude(velocityNet);

        float velocityMax = 173f;

        float linearVelComp = 1 - Mathf.Clamp01(velocityForeAft / velocityMax);
        //Debug.Log("Velocity fore or aft is: " + velocityForeAft + " and the max is: " + velocityMax + " and the k is" + linearVelComp);

        _deltaBuoyancyForceApplied = _deltaBuoyancyForce * linearVelComp;
    }

    private void _CheckToEmit()
    {
        //Debug.Log(_deltaBuoyancyForceApplied);
        if (_deltaBuoyancyForceApplied < -300)
            _negBuoyancy = true;
        else
            _negBuoyancy = false;

        if (_attitudeAngTarget > _attitudeAngActual + 1)
            _pitchUp = true;
        else
            _pitchUp = false;

        if (_attitudeAngTarget < _attitudeAngActual - 1)
            _pitchDown = true;
        else
            _pitchDown = false;

        if (_negBuoyancy || _pitchDown)
            _fwdBallastOpen = true;
        else
            _fwdBallastOpen = false;

        if (_negBuoyancy || _pitchUp)
            _aftBallastOpen = true;
        else
            _aftBallastOpen = false;

        fwdBallast.OnOff(_fwdBallastOpen);
        aftBallast.OnOff(_aftBallastOpen);
    }

    private float _motorPerc;

    private void _AssessMotorAudio()
    {
        _motorPerc = Mathf.Lerp(_motorPerc, ((_maneuveringFloats[0] + _maneuveringFloats[1]) / 2), .02f);
        if (canDrive)
        {
            _myAS.volume = (Mathf.Abs(_motorPerc) / 5);
            _myAS.pitch = (Mathf.Lerp(1.2f, 1.6f, Mathf.Abs(_motorPerc)));
        }
        else
        {
            _myAS.volume = (Mathf.Lerp(0, .1f, Mathf.Abs(_motorPerc)));
            _myAS.pitch = (Mathf.Lerp(.5f, .6f, Mathf.Abs(_motorPerc)));
        }
    }
#endregion

#region States

    private class State_Base : FSM<Controlled_Sub>.State
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("In " + this.GetType().Name);
        }

        protected float p1backuptimer;
        protected float p2backupTimer;

        protected void TestToTurn (bool trueLessThanFalseGreatherThan, float stickValLRThreshold, float howLongTillTrigger)
        {
            if (trueLessThanFalseGreatherThan)
            {
                if (Context._maneuveringFloats[0] < stickValLRThreshold)
                    p1backuptimer += Time.deltaTime;
                else
                    p1backuptimer = 0;

                if (Context._maneuveringFloats[1] < stickValLRThreshold)
                    p2backupTimer += Time.deltaTime;
                else
                    p2backupTimer = 0;
            }
            else
            {
                if (Context._maneuveringFloats[0] > stickValLRThreshold)
                    p1backuptimer += Time.deltaTime;
                else
                    p1backuptimer = 0;

                if (Context._maneuveringFloats[1] > stickValLRThreshold)
                    p2backupTimer += Time.deltaTime;
                else
                    p2backupTimer = 0;
            }

            if (p1backuptimer >= howLongTillTrigger || p2backupTimer >= howLongTillTrigger)
            {
                if (trueLessThanFalseGreatherThan)
                {
                    TransitionTo<TurnToLeft>();
                }
                else
                {
                    TransitionTo<TurnToRight>();
                }
            }
        }

        protected void ChangeLightAng()
        {
            Context._lightWheelAng += (Context._lightFloatUpDownP1 + Context._lightFloatUpDownP2) * -Context.lightWheelAngSpeed * Time.deltaTime;
            Context._lightWheelAng = Mathf.Clamp(Context._lightWheelAng, -60f, 110f);
            Context.lightWheel.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Context._lightWheelAng));
        }
    }

    private class FacingRight : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context._deltaBuoyancyForce = 0;
            p1backuptimer = p2backupTimer = 0;
        }       
        public override void Update()
        {
            Context._linearThrust = Context.moveForceScalar * (Context._maneuveringFloats[0] + Context._maneuveringFloats[1]);

            if (Mathf.Abs(Context._maneuveringFloats[2]) > .15f || Mathf.Abs(Context._maneuveringFloats[3]) > .15f)
            Context._attitudeAngTarget += Context.attitudeSpeedScalar * (Context._maneuveringFloats[2] + Context._maneuveringFloats[3]);

            if ((Mathf.Abs(Context._maneuveringFloats[2]) >.7f || Mathf.Abs(Context._maneuveringFloats[3]) >.7f) && Mathf.Abs(Context._attitudeAngActual) > Context.angClamp - 1)
                Context._deltaBuoyancyForce += (Context._maneuveringFloats[2] + Context._maneuveringFloats[3]) * Context.buoyancyForceScalar;

            TestToTurn(true, -.7f, 5f);
            ChangeLightAng();
            Context._CheckToEmit();
        }
    }

    private class FacingLeft : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context._deltaBuoyancyForce = 0;
            p1backuptimer = p2backupTimer = 0;
        }
        public override void Update()
        {
            Context._linearThrust = Context.moveForceScalar * -(Context._maneuveringFloats[0] + Context._maneuveringFloats[1]);

            if (Mathf.Abs(Context._maneuveringFloats[2]) > .15f || Mathf.Abs(Context._maneuveringFloats[3]) > .15f)
                Context._attitudeAngTarget += Context.attitudeSpeedScalar * (Context._maneuveringFloats[2] + Context._maneuveringFloats[3]);

            if ((Mathf.Abs(Context._maneuveringFloats[2]) > .7f || Mathf.Abs(Context._maneuveringFloats[3]) > .7f) && Mathf.Abs(Context._attitudeAngActual) > Context.angClamp - 1)
                Context._deltaBuoyancyForce += (Context._maneuveringFloats[2] + Context._maneuveringFloats[3]) * Context.buoyancyForceScalar;

            TestToTurn(false, .7f, 5f);
            ChangeLightAng();
            Context._CheckToEmit();
        }
    }

    private class TurnToRight : State_Base
    {
        private float turnTimer;
        public override void OnEnter()
        {
            base.OnEnter();
            turnTimer = 0;
            Deeper_EventManager.instance.Fire(new Deeper_Event_SubTurning(true));
        }

        public override void Update()
        {
            turnTimer += Time.deltaTime / Context.turnNormalizedTime;

            Context._headingAngActual = Mathf.Lerp(180f, 360f, Deeper_ServicesLocator.SineEaseInOut(turnTimer));

            ChangeLightAng();

            if (turnTimer >= 1)
                TransitionTo<FacingRight>();
        }

        public override void OnExit()
        {
            base.OnExit();
            Context._headingAngActual = 0;
            Deeper_EventManager.instance.Fire(new Deeper_Event_SubTurning(false));
        }
    }

    private class TurnToLeft : State_Base
    {
        private float turnTimer;
        public override void OnEnter()
        {
            base.OnEnter();
            turnTimer = 0;
            Deeper_EventManager.instance.Fire(new Deeper_Event_SubTurning(true));
        }

        public override void Update()
        {
            turnTimer += Time.deltaTime / Context.turnNormalizedTime;

            Context._headingAngActual = Mathf.Lerp(0f, 180f, Deeper_ServicesLocator.SineEaseInOut(turnTimer));

            ChangeLightAng();

            if (turnTimer >= 1)
                TransitionTo<FacingLeft>();
        }

        public override void OnExit()
        {
            base.OnExit();
            Deeper_EventManager.instance.Fire(new Deeper_Event_SubTurning(false));
        }
    }

    #endregion
}
