using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(Rigidbody))] 

public class Controlled_Sub : Deeper_Component {

    private FSM<Controlled_Sub> _fsm;

    public float moveForceScalar;
    public float attitudeSpeedScalar;
    public float buoyancyForceScalar;
    public float turnNormalizedTime;

    public bool canDrive;

    private Rigidbody _rigidbody;

    private Vector3 _p0MoveVector;
    private Vector3 _p1MoveVector;

    private float[] _maneuveringFloats = new float[4];
    // 0 - p0 LR Thrust
    // 1 - p1 LR Thrust
    // 2 - p0 UD Input
    // 3 - p1 UD Input
    private float _attitudeAngTarget;
    [HideInInspector] public float _attitudeAngActual;

    private float _deltaBuoyancyForce;
    private float _deltaBuoyancyForceApplied;
    private float _linearThrust;
    private Vector3 totalForce;
    private float angClamp = 25;

    public GameObject propPort;
    public GameObject propStbd;
    private Particle_Controller _portPS;
    private Particle_Controller _stbdPS;

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

        _portPS = propPort.GetComponentInChildren<Particle_Controller>();
        _stbdPS = propStbd.GetComponentInChildren<Particle_Controller>();
	}

    private Vector3 _rotAng;
    private float _spinRate;

    public override void NormUpdate()
    {
        //Debug.Log("NormUpdate Running");
        _ReadInputFloats();
        _fsm.Update();

        _PropAnim();

    }

    public override void PhysUpdate()
    {
        //_rigidbody.AddForce(moveForceScalar * (_p0MoveVector + _p1MoveVector));
        _ApplyClamps();
        _LerpToTargets();
        _VelocityCompBuoyancy();

        if (canDrive)
            _rigidbody.AddForce(Quaternion.Euler(0, 0, _attitudeAngActual) * Vector3.right * _linearThrust + _deltaBuoyancyForceApplied * Vector3.up);
    }
    #endregion

#region Bubbles and Animation

    private void _PropAnim()
    {
        _spinRate = Mathf.Lerp(_spinRate, _linearThrust, .05f);
        _rotAng.z -= _spinRate / 20;
        _rotAng.z = (_rotAng.z + 360) % 360;
        propPort.transform.localRotation = (Quaternion.Euler(_rotAng));
        propStbd.transform.localRotation = (Quaternion.Euler(_rotAng));

        int bubbleRand = Random.Range(0, 3000);
        if (_spinRate > 100)
        {
            _portPS.transform.localRotation = Quaternion.Euler(0, 180, 0);
            _portPS.OnOff(true);
            _stbdPS.transform.localRotation = Quaternion.Euler(0, 180, 0);
            _stbdPS.OnOff(true);
        }
        else if (_spinRate < -100)
        {
            _portPS.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _portPS.OnOff(true);
            _stbdPS.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _stbdPS.OnOff(true);
        }
        else
        {
            _portPS.OnOff(false);
            _stbdPS.OnOff(false);
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
#endregion

#region States

    private class State_Base : FSM<Controlled_Sub>.State
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("In " + this.GetType().Name);
        }
    }

    private class FacingRight : State_Base
    {
        private float backupTimer;
        public override void OnEnter()
        {
            base.OnEnter();
            Context._deltaBuoyancyForce = 0;
        }       
        public override void Update()
        {
            Context._linearThrust = Context.moveForceScalar * (Context._maneuveringFloats[0] + Context._maneuveringFloats[1]);

            if (Mathf.Abs(Context._maneuveringFloats[2]) > .15f || Mathf.Abs(Context._maneuveringFloats[3]) > .15f)
            Context._attitudeAngTarget += Context.attitudeSpeedScalar * (Context._maneuveringFloats[2] + Context._maneuveringFloats[3]);

            if ((Mathf.Abs(Context._maneuveringFloats[2]) >.7f || Mathf.Abs(Context._maneuveringFloats[3]) >.7f) && Mathf.Abs(Context._attitudeAngActual) > Context.angClamp - 1)
                Context._deltaBuoyancyForce += (Context._maneuveringFloats[2] + Context._maneuveringFloats[3]) * Context.buoyancyForceScalar;
        }
    }

    private class FacingLeft : State_Base
    {
        private float backupTimer;
        public override void OnEnter()
        {
            base.OnEnter();
            Context._deltaBuoyancyForce = 0;
        }
        public override void Update()
        {
            Context._linearThrust = -(Context._maneuveringFloats[0] + Context._maneuveringFloats[1]);

            Context._attitudeAngTarget -=  Context.attitudeSpeedScalar * (Context._maneuveringFloats[2] + Context._maneuveringFloats[3]);

            Context._deltaBuoyancyForce += (Context._maneuveringFloats[2] + Context._maneuveringFloats[3]);

            if (Context._maneuveringFloats[0] > -.5f || Context._maneuveringFloats[1] > -.5f)
                backupTimer += Time.deltaTime;
            if (Context._maneuveringFloats[0] > .75f || Context._maneuveringFloats[1] > .75f)
                backupTimer = 0;

            if (backupTimer >= 3)
                TransitionTo<TurnToRight>();

            //Debug.Log(backupTimer);

            backupTimer -= .2f * Time.deltaTime;
            backupTimer = Mathf.Clamp(backupTimer, 0, 4);
        }
    }

    private class TurnToRight : State_Base
    {
        private float turnTimer;
        public override void OnEnter()
        {
            base.OnEnter();
            turnTimer = 0;
        }
        public override void Update()
        {
            turnTimer += Time.deltaTime / Context.turnNormalizedTime;

            if (turnTimer >= 2)
                TransitionTo<FacingRight>();
        }
    }

    private class TurnToLeft : State_Base
    {
        private float turnTimer;
        public override void OnEnter()
        {
            base.OnEnter();
            turnTimer = 0;
        }
        public override void Update()
        {
            turnTimer += Time.deltaTime / Context.turnNormalizedTime;

            if (turnTimer >= 2)
                TransitionTo<FacingLeft>();
        }
    }

    #endregion
}
