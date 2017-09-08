using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_SubCollisionSystem : Deeper_Component {

    private FSM<Mech_SubCollisionSystem> _fsm;
    public GameObject warningLight;
    public float collisionThresholdVel;

    private void Awake()
    {
        Initialize(3000);
    }

    private void Start()
    {
        _fsm = new FSM<Mech_SubCollisionSystem>(this);
        _fsm.TransitionTo<Standby>();
    }

    public float preCollVel;

    public override void NormUpdate()
    {
        _fsm.Update();

        preCollVel = Vector3.Magnitude(GetComponent<Rigidbody>().velocity);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<Info_Physical>() == null)
            return;
        else if (other.transform.GetComponent<Info_Physical>().IsPlayer)
            return;
        else
        {
            if (preCollVel >= collisionThresholdVel)
                _fsm.TransitionTo<CollisionState>();
        }
    }

    #region States

    private class State_Base : FSM<Mech_SubCollisionSystem>.State
    {

    }

    private class Standby : State_Base
    {
        public override void OnEnter()
        {
            Context.warningLight.SetActive(false);
        }
    }

    private class CollisionState : State_Base
    {
        public override void OnEnter()
        {
            Context.warningLight.SetActive(true);
            Context.warningLight.GetComponent<Light>().intensity = 8;
            timer = 0;
            normTime = .75f;
            Deeper_ServicesLocator.instance.SFXManager.PlaySoundOneHit(SFX.CrashDeath);
        }

        private float timer;
        private float normTime;

        public override void Update()
        {
            timer += Time.deltaTime;

            Context.warningLight.GetComponent<Light>().intensity = 8 - 8 * (timer / normTime);

            if (timer >= normTime)
                TransitionTo<Standby>();
        }
    }

#endregion
}
