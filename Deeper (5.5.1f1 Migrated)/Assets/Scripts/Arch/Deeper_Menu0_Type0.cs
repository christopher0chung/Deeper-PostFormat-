using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Deeper_Menu0_Type0 : Deeper_Menu0 {

    [SerializeField] protected List<GameObject> _options;
    protected Deeper_RolloverInt _selectNum;
    protected FSM<Deeper_Menu0_Type0> _fsm;
    protected Game_Logic _thisSceneGameLogic;

    private Vector3 _activeLoc;

    protected override void Awake() {
        base.Awake();
    }

    protected override void Start () {
        base.Start();

        _activeLoc = transform.localPosition;

        _thisSceneGameLogic = GameObject.Find("Managers_Game").GetComponent<Game_Logic>();

        _fsm = new FSM<Deeper_Menu0_Type0>(this);
        _fsm.TransitionTo<StartState>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Debug.Assert(transform.GetChild(i).GetComponent<Deeper_Menu0_Element>() != null, gameObject.name + " has a non-element child");
            Debug.Assert(transform.GetChild(i).GetComponent<Deeper_Menu0_Element>().myType == MenuElementType.Action
                || transform.GetChild(i).GetComponent<Deeper_Menu0_Element>().myType == MenuElementType.Nav, gameObject.name + " has a info type child");

            _options.Add(transform.GetChild(i).gameObject);
        }
        _selectNum = new Deeper_RolloverInt(0, 0, _options.Count - 1);
	}

    void Update () {
        _fsm.Update();
	}

    public override void TurnOn()
    {
        _fsm.TransitionTo<Activate>();
    }

    private class State_Base : FSM<Deeper_Menu0_Type0>.State
    {
        public override void OnEnter()
        {
            Debug.Log(Context.gameObject.name + " is in " + Context._fsm.CurrentState.ToString());
        }
    }

    private class StartState : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context.transform.localPosition = Context._activeLoc + Vector3.right * -25;
        }
    }

    private class Inactive : State_Base
    {
        public override void Update()
        {
            Context.transform.localPosition = Vector3.Lerp(Context.transform.localPosition, Context._activeLoc + Vector3.right * -25, .1f);
        }
    }

    private class Activate : State_Base
    {
        public override void OnEnter()
        {
            Context._selectNum.intVal = 0;
            Context._options[Context._selectNum.intVal].GetComponent<TMPro.TextMeshPro>().color = Color.red;
        }

        public override void Update()
        {
            Context.transform.localPosition = Vector3.Lerp(Context.transform.localPosition, Context._activeLoc, .1f);

            if (Vector3.Distance(Context.transform.localPosition, Context._activeLoc) <= .1f)
                TransitionTo<Active>();
        }
    }

    private class Active : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        private bool[] axisFlag = new bool[2];

        private void _Controls()
        {
            for (int i = 0; i <= 1; i++)
            {
                if (Mathf.Abs(ReInput.players.GetPlayer(i).GetAxis("Menu Select Vertical")) > .25f && axisFlag[i] == false)
                {
                    axisFlag[i] = true;
                    if (ReInput.players.GetPlayer(i).GetAxis("Menu Select Vertical") > 0)
                    {
                        Context._selectNum.intVal--;
                        Deeper_ServicesLocator.instance.SFXManager.PlaySoundOneHit(SFX.Toggle);
                    }
                    else
                    {
                        Context._selectNum.intVal++;
                        Deeper_ServicesLocator.instance.SFXManager.PlaySoundOneHit(SFX.Toggle);
                    }
                }
                else if (Mathf.Abs(ReInput.players.GetPlayer(i).GetAxis("Menu Select Vertical")) <= .25f)
                {
                    axisFlag[i] = false;
                }

                if (ReInput.players.GetPlayer(i).GetButtonDown("Menu Accept"))
                {
                    Deeper_ServicesLocator.instance.SFXManager.PlaySoundOneHit(SFX.Select);

                    if (Context._options[Context._selectNum.intVal].GetComponent<Deeper_Menu0_Element>().myType == MenuElementType.Nav)
                        TransitionTo<Inactive>();

                    Context._options[Context._selectNum.intVal].GetComponent<Deeper_Menu0_Element>().DoSomething();
                }

                if (ReInput.players.GetPlayer(i).GetButtonDown("Cancel"))
                {
                    if (Context.myPathUp != null)
                    {
                        Deeper_Menu0_Type0 up = Context.myPathUp as Deeper_Menu0_Type0;
                        Debug.Assert(up != null, "Something is wrong");
                        if (up != null)
                        {
                            up.TurnOn();
                            TransitionTo<Inactive>();
                            Deeper_ServicesLocator.instance.SFXManager.PlaySoundOneHit(SFX.Cancel);
                        }
                    }
                    else
                    {
                        Debug.Log(Context.gameObject.name + " called unpause");
                        Context._thisSceneGameLogic.UnpauseGameLogic();
                    }
                }
            }
        }

        private void _ColorSet()
        {
            for (int i = 0; i < Context._options.Count; i++)
            {
                if (i == Context._selectNum.intVal)
                {
                    Context._options[i].GetComponent<TMPro.TextMeshPro>().color = Color.Lerp(Context._options[i].GetComponent<TMPro.TextMeshPro>().color, Color.red, .08f);
                }
                else
                {
                    Context._options[i].GetComponent<TMPro.TextMeshPro>().color = Color.Lerp(Context._options[i].GetComponent<TMPro.TextMeshPro>().color, Color.white, .08f);
                }
            }
        }

        public override void Update()
        {
            _Controls();
            _ColorSet();
        }
    }
}
