using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Mech_SuitAirSystem))]

public class Controlled_Character : Deeper_Component, ICurrentable {

    public CharactersEnum thisChar;
    private int controllerNum;

    public Animator myAnim;
    public LayerMask myLM;
    public RaycastHit[] myGroundHits;

    public float groundedCheckDistance;
    public float boostingForceScalar;
    public float maxWalkSpeed;

    public Facing currentFacing;
    public Transform myLight;

    public Particle_Controller breathPart;
    public Particle_Controller boostPart;

    public Transform modelTransform;

    public SphereCollider ingressTrigger;

    [HideInInspector] public bool airAvailable;

    #region Internal Variables
    private Game_Logic _myGL;

    private bool _gnd;
    private bool _grounded
    {
        get
        {
            return _gnd;
        }
        set
        {
            if (value != _gnd)
            {
                _gnd = value;
                //if (_gnd)
                //    _fsm.TransitionTo<Standing>();
                //else
                //    _fsm.TransitionTo<Floating>();
            }
        }
    }

    private bool _b;
    private bool _boosting
    {
        get
        {
            return _b;
        }
        set
        {
            if (value)
            {
                _myMSAS.Boost();
            }
            if (value != _b)
            {
                _b = value;
                boostPart.OnOff(_b);
            }
        }
    }

    private bool _walking;

    private bool _br;
    private bool _breathing
    {
        get
        {
            return _br;
        }
        set
        {
            if (value)
                _myMSAS.Breath();
            if (value != _br)
            {
                _br = value;
                breathPart.OnOff(_br);
            }
        }
    }

    private bool _subTurning;
    private bool _canEgress
    {
        get
        {
            if (!_subTurning)
                return true;
            else
                return false;
        }
    }

    private FSM<Controlled_Character> _fsm;

    private Vector3 _leftStickInput;
    private Vector3 _rightStickInput;
    private Vector3 _actionInput;

    private PhysicMaterial _myPhysMat;

    private Quaternion _qFacingRight;
    private Quaternion _qFacingLeft;
    private Quaternion _qFacingFwd;

    private Rigidbody _myRB;

    private Vector3 _walkForceVector;
    private Vector3 _boostForceVector;

    private GameObject _subRef;
    private GameObject _model;

    private float _lightAng;

    private float _interactTimerRef;

    private Mech_SuitAirSystem _myMSAS;

    private bool isOOC;
    #endregion

    #region Deeper_Component Functions
    private void Awake()
    {
        Initialize(3000);
        Deeper_EventManager.instance.Register<Deeper_Event_ControlScheme>(OOCHandler);
        Deeper_EventManager.instance.Register<Deeper_Event_SubTurning>(SubStateHandler);
    }

    public override void _Unsub()
    {
        base._Unsub();
        Deeper_EventManager.instance.Unregister<Deeper_Event_ControlScheme>(OOCHandler);
    }

    private void Start()
    {
        _myGL = GameObject.Find("Managers_Game").GetComponent<Game_Logic>();

        _fsm = new FSM<Controlled_Character>(this);
        _fsm.TransitionTo<Inside>();
        _SetPhysMat();

        controllerNum = Deeper_ServicesLocator.instance.GetInt(thisChar);

        _qFacingFwd = Quaternion.Euler(Vector3.zero);
        _qFacingLeft = Quaternion.Euler(new Vector3(0, -91, 0));
        _qFacingRight = Quaternion.Euler(new Vector3(0, 91, 0));
        currentFacing = Facing.Forward;

        _myRB = GetComponent<Rigidbody>();

        _subRef = Deeper_ServicesLocator.instance.GetRef(CharactersEnum.DANI);
        _model = transform.Find("Model").gameObject;

        _myMSAS = GetComponent<Mech_SuitAirSystem>();
    }

    public override void NormUpdate()
    {
        //Debug.Log("NormUpdate Running for Char");
        _OnGroundCheck();
        _fsm.Update();
        _WhichAnim();
    }

    public override void PhysUpdate()
    {
        if (_grounded && !_boosting)
        {
            _myRB.AddForce(_walkForceVector);
            _myRB.velocity = Vector3.ClampMagnitude(_myRB.velocity, maxWalkSpeed);
        }
        else
            _myRB.AddForce(_boostForceVector);

        myLight.transform.localRotation = Quaternion.Slerp(myLight.transform.localRotation, Quaternion.Euler(new Vector3(_lightAng, 0, 0)), .1f);

        _myRB.AddForce(_currentForce / 10);
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

    #region Context Functions
    private void _SetPhysMat()
    {
        _myPhysMat = new PhysicMaterial();
        _myPhysMat.bounciness = 0;
        _myPhysMat.staticFriction = 1;
        _myPhysMat.dynamicFriction = 1;

        GetComponent<Collider>().material = _myPhysMat;
    }

    private void _SetPhysMatSlick()
    {
        _myPhysMat.staticFriction = 0;
        _myPhysMat.dynamicFriction = 0;
    }

    private void _SetPhysMatStick()
    {
        _myPhysMat.staticFriction = 1;
        _myPhysMat.dynamicFriction = 1;
    }

    private void _OnGroundCheck()
    {
        myGroundHits = Physics.SphereCastAll(new Ray(transform.position, Vector3.down), .25f, 1, myLM, QueryTriggerInteraction.Ignore);

        if (myGroundHits != null)
        {
            foreach (RaycastHit thisHit in myGroundHits)
            {
                if (thisHit.transform.gameObject.GetComponent<Info_Physical>() != null)
                {
                    if (thisHit.transform.gameObject.GetComponent<Info_Physical>().CanStandOn)
                    {
                        //Is the thing that the spherecast hit something that can be stood on

                        if (thisHit.distance < groundedCheckDistance)
                        {
                            _grounded = true;
                            return;
                        }
                    }
                }
            }

            //Sets standing to false if each hit is not standable
            _grounded = false;
        }
        //Sets standing to false if no hits
        else
            _grounded = false;
    }

    private void _WhichAnim()
    {
        if (_grounded)
        {
            if (!_boosting && !_walking)
                _SetAnim(CharAnimStates.Standing);
            else if (_boosting)
                _SetAnim(CharAnimStates.Boosting);
            else if (_walking)
                _SetAnim(CharAnimStates.Walking);
        }
        else
        {
            if (!_boosting)
                _SetAnim(CharAnimStates.Floating);
            else
                _SetAnim(CharAnimStates.Boosting);
        }
    }

    private void _SetAnim(CharAnimStates s)
    {
        //Debug.Log(s);
        if (myAnim.isActiveAndEnabled)
        {
            if (s == CharAnimStates.Floating)
            {
                myAnim.SetBool("Idle", true);
                myAnim.SetBool("Grounded", false);
                myAnim.SetFloat ("WalkSpeed", 0);
            }
            if (s == CharAnimStates.Boosting)
            {
                myAnim.SetBool("Idle", false);
                myAnim.SetBool("Grounded", false);
                myAnim.SetFloat("WalkSpeed", 0);
            }
            if (s == CharAnimStates.Standing)
            {
                myAnim.SetBool("Idle", true);
                myAnim.SetBool("Grounded", true);
                myAnim.SetFloat("WalkSpeed", 0);
            }
            if (s == CharAnimStates.Walking)
            {
                myAnim.SetBool("Idle", false);
                myAnim.SetBool("Grounded", true);
                myAnim.SetFloat("WalkSpeed", 1);
            }
        }

    }

    private void OOCHandler (Deeper_Event e)
    {
        Deeper_Event_ControlScheme c = e as Deeper_Event_ControlScheme;
        if (c != null)
        {
            if (c.cs == ControlStates.Doc_OOC && thisChar == CharactersEnum.Doc)
                isOOC = true;
            else if (thisChar == CharactersEnum.Doc)
                isOOC = false;

            if (c.cs == ControlStates.Ops_OOC && thisChar == CharactersEnum.Ops)
                isOOC = true;
            else if (thisChar == CharactersEnum.Ops)
                isOOC = false;
        }
    }

    private void SubStateHandler (Deeper_Event e)
    {
        Deeper_Event_SubTurning s = e as Deeper_Event_SubTurning;
        if (s != null)
        {
            if (s.isTurning)
                _subTurning = true;
            else
                _subTurning = false;
        }
    }

    public void InteractInit(bool tF)
    {
        if (tF)
            _fsm.TransitionTo<Working>();
        //else
        //    _fsm.TransitionTo<Floating>();
    }

    public void InteractComplete(bool tF)
    {
        _fsm.TransitionTo<Floating>();
    }

    private float breathingTimer;
    private float inhaleTime = 3;
    private float exhaleTime = 2;
    private void _BreathingUpdate()
    {
        if (airAvailable)
        {
            breathingTimer += Time.deltaTime;
            if (breathingTimer < inhaleTime)
                _breathing = false;
            if (breathingTimer >= inhaleTime && breathingTimer < inhaleTime + exhaleTime)
                _breathing = true;
            if (breathingTimer >= inhaleTime + exhaleTime)
                breathingTimer -= inhaleTime + exhaleTime;
        }
        else
            _breathing = false;
    }

    #endregion

    #region Private Enums
    private enum CharAnimStates { Standing, Walking, Floating, Boosting, Working }
    #endregion

    #region Public Functions
    public void IngressTaskReturn(InteractionTaskReturn r)
    {
        if (r == InteractionTaskReturn.Success)
            _fsm.TransitionTo<Inside>();
        else
            _fsm.TransitionTo<Floating>();
    }

    public void EgressTaskReturn(InteractionTaskReturn r)
    {
        if (r == InteractionTaskReturn.Success)
            _fsm.TransitionTo<Floating>();
        else
            _fsm.TransitionTo<Inside>();
    }

    public void StartWorking()
    {
        _fsm.TransitionTo<Working>();
    }

    public void DoneWorking()
    {
        _fsm.TransitionTo<Floating>();
    }
    #endregion

    #region Operating States
    private class State_Base : FSM<Controlled_Character>.State
    {
        #region State Root Functions
        public virtual void Input()
        {
            Context._leftStickInput.x = ReInput.players.GetPlayer(Context.controllerNum).GetAxis("Swim Horizontal");
            Context._leftStickInput.y = ReInput.players.GetPlayer(Context.controllerNum).GetAxis("Swim Vertical");

            Context._rightStickInput.x = ReInput.players.GetPlayer(Context.controllerNum).GetAxis("Light Horizontal");
            Context._rightStickInput.y = ReInput.players.GetPlayer(Context.controllerNum).GetAxis("Light Vertical");
        }

        public virtual void SetFaceDir()
        {
            if (Context._leftStickInput.x < 0)
                Context.currentFacing = Facing.Left;
            if (Context._leftStickInput.x > 0)
                Context.currentFacing = Facing.Right;
        }

        public virtual void Face(Facing f)
        {
            if (f == Facing.Forward)
                Context.modelTransform.transform.localRotation = Quaternion.Slerp(Context.modelTransform.transform.localRotation, Context._qFacingFwd, .02f);
            if (f == Facing.Right)
                Context.modelTransform.transform.localRotation = Quaternion.Slerp(Context.modelTransform.transform.localRotation, Context._qFacingRight, .02f);
            if (f == Facing.Left)
                Context.modelTransform.transform.localRotation = Quaternion.Slerp(Context.modelTransform.transform.localRotation, Context._qFacingLeft, .02f);
        }

        public void ControlsSetInside(CharactersEnum whoAmI)
        {
            if (whoAmI == CharactersEnum.Doc)
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_Inside));
            else
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_Inside));
        }

        public void ControlsSetOutside(CharactersEnum whoAmI)
        {
            if (whoAmI == CharactersEnum.Doc)
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_Outside));
            else
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_Outside));
        }
        
        public void ControlsSetInProgress(CharactersEnum whoAmI)
        {
            if (whoAmI == CharactersEnum.Doc)
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_IP));
            else
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_IP));
        }

        public virtual void IngressCheck()
        {
            if (Context.ingressTrigger.bounds.Contains(Context.transform.position))
                if (ReInput.players.GetPlayer(Context.controllerNum).GetButtonDown("Player Action"))
                {
                    TransitionTo<Ingressing>();
                    Debug.Log("Inside 4 and button hit");
                }
        }

        public virtual void MoveLight()
        {
            if (Context._rightStickInput.x * Context._rightStickInput.x + Context._rightStickInput.y * Context._rightStickInput.y > .7f)
            {
                if (Context.currentFacing == Facing.Right && Context._rightStickInput.x > 0)
                {
                    Context._lightAng = Mathf.Clamp(Mathf.Atan2(-Context._rightStickInput.y, Context._rightStickInput.x) * Mathf.Rad2Deg, -60, 60);
                }
                else if (Context.currentFacing == Facing.Left && Context._rightStickInput.x < 0)
                {
                    Context._lightAng = Mathf.Clamp(Mathf.Atan2(-Context._rightStickInput.y, -Context._rightStickInput.x) * Mathf.Rad2Deg, -60, 60);
                }
            }
        }

        public virtual void InteractCheck(CharactersEnum whoAmI)
        {
            if (ReInput.players.GetPlayer(Context.controllerNum).GetButtonDown("Player Action"))
            {
                Context._myGL.AttemptToInteract(Context, Context.InteractInit, Context.InteractComplete);
            }
        }

        #endregion

        public override void OnEnter()
        {
            //Debug.Log(this.GetType());
        }

        public override void Update()
        {
            Input();
            SetFaceDir();
            Face(Context.currentFacing);
        }


    }

    private class Standing : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context._SetPhysMatStick();
            if (!Context.isOOC)
                ControlsSetOutside(Context.thisChar);
        }

        public override void Update()
        {
            base.Update();

            Context._BreathingUpdate();

            IngressCheck();
            InteractCheck(Context.thisChar);
            if (Mathf.Abs(Context._leftStickInput.x) > .2f)
            {
                Context._walking = true;
                Context._SetPhysMatSlick();
                Context._walkForceVector = Vector3.right * Context.boostingForceScalar * Context._leftStickInput.x;
            }
            else
            {
                Context._walking = false;
                Context._SetPhysMatStick();
                Context._walkForceVector = Vector3.zero;
            }

            if (Context._leftStickInput.y > .75f && Context.airAvailable)
            {
                Context._boosting = true;
                Context._boostForceVector = Vector3.up * Context.boostingForceScalar * Context._leftStickInput.y;
            }
            else
            {
                Context._boosting = false;
                Context._boostForceVector = Vector3.zero;
            }

            MoveLight();

            if (!Context._gnd)
                TransitionTo<Floating>();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }

    private class Floating : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context._SetPhysMatStick();
            if (!Context.isOOC)
                ControlsSetOutside(Context.thisChar);
        }

        public override void Update()
        {
            base.Update();

            Context._BreathingUpdate();

            IngressCheck();
            InteractCheck(Context.thisChar);

            if (Mathf.Sqrt((Context._leftStickInput.y * Context._leftStickInput.y) + (Context._leftStickInput.x * Context._leftStickInput.x)) > .15f && Context.airAvailable)
            {
                Context._boosting = true;
                Context._boostForceVector = Context.boostingForceScalar * new Vector3(Context._leftStickInput.x, Context._leftStickInput.y, 0);
                //Debug.Log(Context._leftStickInput.x + " " + Context._leftStickInput.y);
            }
            else
            {
                Context._boosting = false;
                Context._boostForceVector = Vector3.zero;
            }

            MoveLight();

            if (Context._gnd)
                TransitionTo<Standing>();
        }
        public override void OnExit()
        {
            base.OnExit();
        }
    }

    private class Working : State_Base
    {
        private Vector3 holdPos;

        public override void OnEnter()
        {
            base.OnEnter();
            holdPos = Context.transform.position;
            if (!Context.isOOC)
                ControlsSetInProgress(Context.thisChar);
        }

        public override void Update()
        {
            Context._BreathingUpdate();

            Context.transform.position = holdPos;

            if (ReInput.players.GetPlayer(Context.controllerNum).GetButtonDown("Cancel"))
            {
                Context._myGL.AttemptToCancel(Context);
            }
        }
    }

    private class Inside : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Context._model.SetActive(false);
            Context.GetComponent<Collider>().enabled = false;
            ControlsSetInside(Context.thisChar);
            Context._boosting = false;
            Context._breathing = false;
        }

        public override void Update()
        {
            Context.transform.position = Context._subRef.transform.position - Vector3.right * 1.76f - Vector3.up * 3.08f;

            Context._myMSAS.Recharge();

            if (ReInput.players.GetPlayer(Context.controllerNum).GetButtonDown("Sub Egress") && Context._canEgress)
            {
                TransitionTo<Egressing>();
            }
        }
    }

    private class Ingressing : State_Base
    {
        private Vector3 relativePositionalOffset;
        private Task_Interaction_InProgress_Base t;
        private float interactionTime;

        public override void OnEnter()
        {
            Debug.Log("I'm inside ingressing");
            base.OnEnter();
            relativePositionalOffset = -Context._subRef.transform.position + Context.transform.position;
            _timer = 0;
            ControlsSetInProgress(Context.thisChar);
            interactionTime = 1.5f;
            t = new Task_Interaction_InProgress_Base(Context, interactionTime);
            Deeper_ServicesLocator.instance.TaskManager.AddTask(t);
        }

        private float _timer;
        public override void Update()
        {
            Context._BreathingUpdate();

            Context.transform.position = Context._subRef.transform.position + relativePositionalOffset;

            _timer += Time.deltaTime;
            if (_timer >= interactionTime)
            {
                TransitionTo<Inside>();
                t.SetStatus(TaskStatus.Success);
            }

            if (ReInput.players.GetPlayer(Context.controllerNum).GetButtonDown("Cancel"))
            {
                TransitionTo<Floating>();
                t.SetStatus(TaskStatus.Aborted);
            }
        }

        public override void OnExit()
        {
            Debug.Log("I'm leaving ingressing");
        }
    }

    private class Egressing : State_Base
    {
        private Task_Interaction_InProgress_Base t;
        private float interactionTime;

        public override void OnEnter()
        {
            base.OnEnter();
            _timer = 0;
            ControlsSetInProgress(Context.thisChar);
            interactionTime = .9f;
            t = new Task_Interaction_InProgress_Base(Context, interactionTime);
            Deeper_ServicesLocator.instance.TaskManager.AddTask(t);
        }

        private float _timer;
        public override void Update()
        {
            Context.transform.position = Context._subRef.transform.position - Vector3.right * 1.76f - Vector3.up * 3.08f;

            _timer += Time.deltaTime;
            if (_timer >= interactionTime)
            {
                TransitionTo<Egress_Success>();
                t.SetStatus(TaskStatus.Success);
            }

            if (ReInput.players.GetPlayer(Context.controllerNum).GetButtonDown("Cancel"))
            {
                TransitionTo<Inside>();
                t.SetStatus(TaskStatus.Aborted);
            }
        }
    }

    private class Egress_Success : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context._model.SetActive(true);
            Context.GetComponent<Collider>().enabled = true;
            ControlsSetOutside(Context.thisChar);
        }

        public override void Update()
        {
            //Debug.Log("I'm sitting in egress_success");
            TransitionTo<Floating>();
        }
    }
    #endregion
}

public enum Facing { Forward, Right, Left }