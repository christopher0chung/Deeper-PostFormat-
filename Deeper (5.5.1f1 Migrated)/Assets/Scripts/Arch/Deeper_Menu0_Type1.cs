using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Deeper_Menu0_Type1 : Deeper_Menu0 {

    [SerializeField] protected List<GameObject> _infoFrames;
    protected Deeper_RolloverInt _selectNum;
    protected FSM<Deeper_Menu0_Type1> _fsm;
    protected Game_Logic _thisSceneGameLogic;
    protected TMPro.TextMeshPro _label;

    private Vector3 _activeLoc;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        _activeLoc = transform.localPosition;

        _thisSceneGameLogic = GameObject.Find("Managers_Game").GetComponent<Game_Logic>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Debug.Assert(transform.GetChild(i).GetComponent<Deeper_Menu0_Element>() != null, gameObject.name + " has a non-element child");
            Debug.Assert(transform.GetChild(i).GetComponent<Deeper_Menu0_Element>().myType == MenuElementType.Info ||
                transform.GetChild(i).GetComponent<Deeper_Menu0_Element>().myType == MenuElementType.Label, gameObject.name + " has a non-info type child");

            if (transform.GetChild(i).GetComponent<Deeper_Menu0_Element>().myType == MenuElementType.Info)
                _infoFrames.Add(transform.GetChild(i).gameObject);
            else
                _label = transform.GetChild(i).GetComponent<TMPro.TextMeshPro>();
        }
        _selectNum = new Deeper_RolloverInt(0, 0, _infoFrames.Count - 1);

        _fsm = new FSM<Deeper_Menu0_Type1>(this);
        _fsm.TransitionTo<StartState>();
    }

    void Update()
    {
        _fsm.Update();
    }

    public override void TurnOn()
    {
        _fsm.TransitionTo<Activate>();
    }

    private class State_Base : FSM<Deeper_Menu0_Type1>.State
    {
        public override void OnEnter()
        {
            Debug.Log(Context.gameObject.name + " is in " + Context._fsm.CurrentState.ToString());
        }

        protected void _Label()
        {
            Context._label.text = Context._selectNum.intVal + 1 + " / " + Context._infoFrames.Count;
        }
    }

    private class StartState : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context.transform.localPosition = Context._activeLoc + Vector3.right * -40;
            _Label();
        }
    }

    private class Inactive : State_Base
    {
        public override void Update()
        {
            Context.transform.localPosition = Vector3.Lerp(Context.transform.localPosition, Context._activeLoc + Vector3.right * -40, .1f);
        }
    }

    private class Activate : State_Base
    {
        public override void OnEnter()
        {
            Context._selectNum.intVal = 0;
            foreach (GameObject g in Context._infoFrames)
            {
                g.GetComponent<SpriteRenderer>().color = new Color(g.GetComponent<SpriteRenderer>().color.r, g.GetComponent<SpriteRenderer>().color.g, g.GetComponent<SpriteRenderer>().color.b, 0);
            }
            Context._infoFrames[Context._selectNum.intVal].GetComponent<SpriteRenderer>().color = new Color(Context._infoFrames[Context._selectNum.intVal].GetComponent<SpriteRenderer>().color.r, Context._infoFrames[Context._selectNum.intVal].GetComponent<SpriteRenderer>().color.g, Context._infoFrames[Context._selectNum.intVal].GetComponent<SpriteRenderer>().color.b, 1);
            _Label();
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
                if (Mathf.Abs(ReInput.players.GetPlayer(i).GetAxis("Menu Select Horizontal")) > .25f && axisFlag[i] == false)
                {
                    axisFlag[i] = true;
                    if (ReInput.players.GetPlayer(i).GetAxis("Menu Select Horizontal") > 0)
                        Context._selectNum.intVal++;
                    else
                        Context._selectNum.intVal--;
                }
                else if (Mathf.Abs(ReInput.players.GetPlayer(i).GetAxis("Menu Select Horizontal")) <= .25f)
                {
                    axisFlag[i] = false;
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
                        }
                    }
                }

                if (ReInput.players.GetPlayer(i).GetButtonDown("Start"))
                {
                    TransitionTo<Inactive>();
                }
            }
        }

        private void _ColorSet()
        {
            for (int i = 0; i < Context._infoFrames.Count; i++)
            {
                if (i == Context._selectNum.intVal)
                {
                    Context._infoFrames[i].GetComponent<SpriteRenderer>().color = Color.Lerp(Context._infoFrames[i].GetComponent<SpriteRenderer>().color, 
                        new Color(Context._infoFrames[i].GetComponent<SpriteRenderer>().color.r, Context._infoFrames[i].GetComponent<SpriteRenderer>().color.g, Context._infoFrames[i].GetComponent<SpriteRenderer>().color.b, 1), 
                        .08f);
                }
                else
                {
                    Context._infoFrames[i].GetComponent<SpriteRenderer>().color = Color.Lerp(Context._infoFrames[i].GetComponent<SpriteRenderer>().color,
                        new Color(Context._infoFrames[i].GetComponent<SpriteRenderer>().color.r, Context._infoFrames[i].GetComponent<SpriteRenderer>().color.g, Context._infoFrames[i].GetComponent<SpriteRenderer>().color.b, 0),
                        .08f);
                }
            }
        }



        public override void Update()
        {
            _Controls();
            _ColorSet();
            _Label();
        }
    }
}
