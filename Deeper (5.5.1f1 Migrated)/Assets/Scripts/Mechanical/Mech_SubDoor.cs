using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_SubDoor : Deeper_Component {

    public SphereCollider mySC;
    public Transform ops;
    public GameObject opsModel;
    public Transform doc;
    public GameObject docModel;
    public Transform door;
    public Light doorLight;

    private FSM<Mech_SubDoor> _fsm;

    void Awake()
    {
        Initialize(3000);
    }

    private void Start()
    {
        _fsm = new FSM<Mech_SubDoor>(this);
        _fsm.TransitionTo<Standby>();
    }

    public override void NormUpdate()
    {
        _fsm.Update();
    }

#region States

    private class State_Base : FSM<Mech_SubDoor>.State
    {

    }

    private class Standby : State_Base
    {
        public override void Update()
        {
            if ((Context.mySC.bounds.Contains(Context.ops.position) && Context.opsModel.activeSelf) || (Context.mySC.bounds.Contains(Context.doc.position) && Context.docModel.activeSelf))
            {
                TransitionTo<Opening>();
            }
        }
    }

    private class Closing : State_Base
    {
        public override void Update()
        {
            Context.door.rotation = Quaternion.Euler(Context.door.eulerAngles.x, Context.door.eulerAngles.y, Mathf.MoveTowards(Context.door.eulerAngles.z, 90, 1));

            if (Context.door.eulerAngles.z > 80)
                Context.doorLight.enabled = false;

            if (Context.door.eulerAngles.z >= 90)
                TransitionTo<Standby>();

            if ((Context.mySC.bounds.Contains(Context.ops.position) && Context.opsModel.activeSelf) || (Context.mySC.bounds.Contains(Context.doc.position) && Context.docModel.activeSelf))
            {
                TransitionTo<Opening>();
            }
        }

    }

    private class Opening : State_Base
    {
        public override void Update()
        {
            Context.door.rotation = Quaternion.Euler(Context.door.eulerAngles.x, Context.door.eulerAngles.y, Mathf.MoveTowards(Context.door.eulerAngles.z, 0, 1));

            if (Context.door.eulerAngles.z < 90)
                Context.doorLight.enabled = true;

            if (Context.door.eulerAngles.z <= 0)
                TransitionTo<Open>();
        }

    }

    private class Open : State_Base
    {
        public override void Update()
        {
            if (!(Context.mySC.bounds.Contains(Context.ops.position) && Context.opsModel.activeSelf) && !(Context.mySC.bounds.Contains(Context.doc.position) && Context.docModel.activeSelf))
            {
                TransitionTo<Closing>();
            }
        }
    }

    #endregion
}
