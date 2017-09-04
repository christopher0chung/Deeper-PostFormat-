using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_Line : MonoBehaviour {

    public Vector3 loci;

    public Vector3[] unitCircle;

    public int points;
    private float radius;
    public float radiusRate;

    private FSM<Debug_Line> _fsm;

    void Start()
    {
        _fsm = new FSM<Debug_Line>(this);
        _fsm.TransitionTo<State_Base>();
    }

	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeCircle();
            _fsm.TransitionTo<Grow>();
        }
        _fsm.Update();
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

    private class State_Base : FSM<Debug_Line>.State
    {

    }

    private class Grow : State_Base
    {
        Vector3[] circle;

        public override void OnEnter()
        {
            circle = new Vector3[Context.unitCircle.Length];
            Context.GetComponent<LineRenderer>().numPositions = circle.Length;
            Context.radius = 0;
        }

        public override void Update()
        {
            Context.radius += Context.radiusRate * Time.deltaTime;
            for (int i = 0; i < Context.unitCircle.Length; i++)
            {
                circle[i] = Context.loci + Context.radius * Context.unitCircle[i];
            }
            Context.GetComponent<LineRenderer>().SetPositions(circle);

            if (Context.radius >= 200)
            {
                TransitionTo<State_Base>();
            }
        }

        public override void OnExit()
        {
            Context.radius = 0;
        }
    }
}
