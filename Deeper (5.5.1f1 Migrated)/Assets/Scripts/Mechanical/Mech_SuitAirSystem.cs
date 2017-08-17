using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controlled_Character))]

public class Mech_SuitAirSystem : Deeper_Component
{
    public float tempStartingAirPercent;

    public float breathRate;
    public float boostRate;
    public float rechargeRate;

    private float airPerc;

    private Controlled_Character myCC;

    private FSM<Mech_SuitAirSystem> _fsm;

    public TMPro.TextMeshPro myTM;
    public GameObject warningLight;

    private void Awake()
    {
        Initialize(4000);
    }

    void Start()
    {
        airPerc = tempStartingAirPercent;
        _fsm = new FSM<Mech_SuitAirSystem>(this);
        _fsm.TransitionTo<AirAvailable>();
        warningLight.SetActive(false);
        myCC = GetComponent<Controlled_Character>();
    }

    public void Breath ()
    {
        airPerc -= breathRate * Time.deltaTime;
    }

    public void Boost ()
    {
        airPerc -= boostRate * Time.deltaTime;
    }

    public void Recharge ()
    {
        airPerc += rechargeRate * Time.deltaTime;
    }

    public override void NormUpdate()
    {
        airPerc = Mathf.Clamp(airPerc, 0, 100);
        myTM.text = "Air: " + airPerc.ToString("0.##") + "%";

        _fsm.Update();
    }

    #region States
    
    private class State_Base : FSM<Mech_SuitAirSystem>.State
    {

    }

    private class AirAvailable : State_Base
    {
        public override void OnEnter()
        {
            Context.myCC.airAvailable = true;
        }

        public override void Update()
        {
            if (Context.airPerc <= 0)
                TransitionTo<AirEmpty>();
        }
    }

    private class AirEmpty: State_Base
    {
        private float timer;
        private bool onOff;

        private float drowningTimer;

        public override void OnEnter()
        {
            timer = 0;
            drowningTimer = 0;
            Context.myCC.airAvailable = false;
            Context.warningLight.SetActive(true);
            onOff = true;
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            drowningTimer += Time.deltaTime;

            if (timer > .5f)
            {
                onOff = !onOff;
                Context.warningLight.SetActive(onOff);
                timer -= .5f;
            }

            if (Context.airPerc > 0)
                TransitionTo<AirAvailable>();

            if (drowningTimer >= 10)
                Debug.Log(Context.myCC.thisChar.ToString() + " has drowned.");
        }

        public override void OnExit()
        {
            Context.warningLight.SetActive(false);
        }
    }
    
    #endregion
}