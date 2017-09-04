using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav_NavBeacon : Deeper_Component
{
    public Transform sub;
    public Vector3 loci;

    public Vector3[] unitCircle;

    public int points;
    private float radius;
    public float radiusRate;

    private FSM<Nav_NavBeacon> _fsm;

    private bool _toggleLightOnYet;

    private float _pingTime = 4f;
    private float _navPingCompleteDist = 10f;

    void Start()
    {
        _fsm = new FSM<Nav_NavBeacon>(this);
        _fsm.TransitionTo<State_Base>();
    }

    private float timer;
    private bool _lastPingFire;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= _pingTime && !_lastPingFire)
        //if (Input.GetKeyDown(KeyCode.Space))
        {
            timer -= _pingTime;
            MakeCircle();
            _fsm.TransitionTo<Grow>();
        }
        _fsm.Update();

        if (Vector3.Distance(transform.position, sub.position) <= _navPingCompleteDist && !_lastPingFire)
        {
            _lastPingFire = true;
            _fsm.TransitionTo<LastPing>();
        }
    }

    public void MakeCircle()
    {
        //loci = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
        loci = transform.position;

        unitCircle = new Vector3[points + 1];

        for (int i = 0; i < points; i++)
        {
            unitCircle[i] = new Vector3(Mathf.Cos(i * (360 / points) * Mathf.Deg2Rad), Mathf.Sin(i * (360 / points) * Mathf.Deg2Rad), 0);
        }

        unitCircle[points] = unitCircle[0];
    }

    private class State_Base : FSM<Nav_NavBeacon>.State
    {

    }

    private class Grow : State_Base
    {
        protected Vector3[] circle;

        public override void OnEnter()
        {
            circle = new Vector3[Context.unitCircle.Length];
            Context.GetComponent<LineRenderer>().numPositions = circle.Length;
            Context.radius = 0;
            Context._toggleLightOnYet = false;
        }

        public override void Update()
        {
            Context.radius += Context.radiusRate * Time.deltaTime;
            for (int i = 0; i < Context.unitCircle.Length; i++)
            {
                circle[i] = Context.loci + Context.radius * Context.unitCircle[i];
            }
            Context.GetComponent<LineRenderer>().SetPositions(circle);

            if (Context.radius >= Vector3.Distance(Context.sub.transform.position, Context.transform.position) && !Context._toggleLightOnYet)
            {
                Context.sub.GetComponent<Mech_NavPingLight>().PingHit();
                Context._toggleLightOnYet = true;
            }

            if (Context.radius >= 200)
            {
                TransitionTo<State_Base>();
            }
        }

        public override void OnExit()
        {
            Context.radius = 0;
            for (int i = 0; i < Context.unitCircle.Length; i++)
            {
                circle[i] = Context.loci + Context.radius * Context.unitCircle[i];
            }
            Context.GetComponent<LineRenderer>().SetPositions(circle);
        }
    }

    private class LastPing : State_Base
    {
        protected Vector3[] circle;

        public override void OnEnter()
        {
            Debug.Log("In Last Ping");
            circle = new Vector3[Context.unitCircle.Length];
            Context.GetComponent<LineRenderer>().numPositions = circle.Length;
            Context.radius = 0;
            Context._toggleLightOnYet = false;
        }

        public override void Update()
        {
            Context.radius += Context.radiusRate * Time.deltaTime;
            Debug.Log("radius is now " + Context.radius);
            for (int i = 0; i < Context.unitCircle.Length; i++)
            {
                circle[i] = Context.loci + Context.radius * Context.unitCircle[i];
            }
            Context.GetComponent<LineRenderer>().SetPositions(circle);

            if (Context.radius >= Vector3.Distance(Context.sub.transform.position, Context.transform.position) && !Context._toggleLightOnYet)
            {
                Context.sub.GetComponent<Mech_NavPingLight>().BigPingHit();
                Context._toggleLightOnYet = true;
            }

            if (Context.radius >= 200)
            {
                Debug.Log("Radius is greater than 200");
                TransitionTo<State_Base>();
            }
        }

        public override void OnExit()
        {
            Debug.Log("On Exit");
            Context.radius = 0;
            for (int i = 0; i < Context.unitCircle.Length; i++)
            {
                circle[i] = Context.loci + Context.radius * Context.unitCircle[i];
            }
            Context.GetComponent<LineRenderer>().SetPositions(circle);
            base.OnExit();
            Context.gameObject.SetActive(false);
        }
    }
}
