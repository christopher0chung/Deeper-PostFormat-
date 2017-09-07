using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;
using UnityEngine.SceneManagement;
using TMPro;

public class Game_Join : MonoBehaviour {

    public GameObject cube;

    public TMPro.TextMeshPro field;

    private string connected0 = "Standing by... Press ANY BUTTON to Connect";
    private string connected1 = "Terminal 1 Connected. Terminal 2: Press ANY BUTTON";
    private string connected2 = "Terminal 2 Connected. Terminal 1: Press ANY BUTTON";
    private string connectedBoth = "Terminals 1 and 2 Connected. Press 'Start' to begin";

    private FSM<Game_Join> _fsm;

    private bool _p1Connected
    {
        get
        {
            IList<Joystick> joysticks = ReInput.controllers.Joysticks;
            if (joysticks.Count > 0)
                return ReInput.controllers.IsControllerAssigned(joysticks[0].type, joysticks[0].id);
            else
                return false;
        }
    }

    //private GameObject _cube1;
    //private bool _p1S;
    //private bool _p1Show
    //{
    //    get
    //    {
    //        return _p1S;
    //    }
    //    set
    //    {
    //        if (value != _p1S)
    //        {
    //            _p1S = value;
    //            if (_p1S)
    //                _cube1 = Instantiate(cube, new Vector3(-5, 0, 0), Quaternion.identity);
    //            else
    //                Destroy(_cube1);
    //        }
    //    }
    //}

    private bool _p2Connected
    {
        get
        {
            IList<Joystick> joysticks = ReInput.controllers.Joysticks;
            if (joysticks.Count > 1)
                return ReInput.controllers.IsControllerAssigned(joysticks[1].type, joysticks[1].id);
            else
                return false;
        }
    }

    //private GameObject _cube2;
    //private bool _p2S;
    //private bool _p2Show
    //{
    //    get
    //    {
    //        return _p2S;
    //    }
    //    set
    //    {
    //        if (value != _p2S)
    //        {
    //            _p2S = value;
    //            if (_p2S)
    //                _cube2 = Instantiate(cube, new Vector3(5, 0, 0), Quaternion.identity);
    //            else
    //                Destroy(_cube2);
    //        }
    //    }
    //}

    private bool _bothConnected
    {
        get
        {
            if (_p1Connected && _p2Connected)
                return true;
            else
                return false;
        }
    }

    private int index;

    private void Start()
    {
        _fsm = new FSM<Game_Join>(this);
        _fsm.TransitionTo<P0>();

        if (GameObject.Find("Managers") == null)
        {
            Instantiate(Resources.Load("Managers/Managers"));
        }
    }

    void Update()
    {
        //_p1Show = _p1Connected;
        //_p2Show = _p2Connected;

        if (_bothConnected)
        {
            //Debug.Log("Both connected");
            if (ReInput.players.GetPlayer(0).GetButtonDown("Start") || ReInput.players.GetPlayer(1).GetButtonDown("Start"))
            {
                SceneManager.LoadScene(1);
                //Debug.Log("Ready to play");
            }
        }

        _fsm.Update();
    }

    private class State_Base : FSM<Game_Join>.State
    {
        protected float timer;

        protected float cursorTimer;
        protected bool cursorOn;

        protected string stringHold;

        protected float typeTime = .05f;

        protected void Print (string s)
        {
            cursorTimer += Time.deltaTime;
            if (cursorTimer >= .15f)
            {
                cursorTimer = 0;
                cursorOn = !cursorOn;
            }

            ClampIndex();

            if (cursorOn)
                Context.field.text = s.Substring(0, Context.index) + "[]";
            else
                Context.field.text = s.Substring(0, Context.index);
        }

        protected void PrintOut ()
        {
            timer += Time.deltaTime;
            if (timer >= typeTime)
            {
                timer -= typeTime;
                Context.index++;

                ClampIndex();
                Print(stringHold);
            }
        }

        protected void ClampIndex()
        {
            Context.index = Mathf.Clamp(Context.index, 0, stringHold.Length);
        }
    }

    private class Clear : State_Base
    {
        public override void Init()
        {
            timer = 0;
            stringHold = Context.field.text;
            if (stringHold.Contains("[]"))
            {
                stringHold = stringHold.Substring(0, stringHold.Length - 2);
                Debug.Log("Contained curson");
            }
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            if (timer >= typeTime / 2)
            {
                timer -= typeTime / 2;
                Context.index--;

                Print(stringHold);

                if (Context.index == 0)
                {
                    if (Context._bothConnected)
                        TransitionTo<PBoth>();
                    else if (Context._p1Connected)
                        TransitionTo<P1>();
                    else if (Context._p2Connected)
                        TransitionTo<P2>();
                    else
                        TransitionTo<P0>();
                }
            }
        }
    }

    private class P0 : State_Base
    {
        public override void Init()
        {
            timer = 0;
            stringHold = Context.connected0;
        }

        public override void Update()
        {
            PrintOut();

            if (Context._p1Connected || Context._p2Connected || Context._bothConnected)
                TransitionTo<Clear>();
        }
    }

    private class P1 : State_Base
    {
        public override void Init()
        {
            timer = 0;
            stringHold = Context.connected1;
        }

        public override void Update()
        {
            PrintOut();

            if (!Context._p1Connected || Context._p2Connected || Context._bothConnected)
                TransitionTo<Clear>();
        }
    }

    private class P2 : State_Base
    {
        public override void Init()
        {
            timer = 0;
            stringHold = Context.connected2;
        }

        public override void Update()
        {
            PrintOut();
            if (Context._p1Connected || !Context._p2Connected || Context._bothConnected)
                TransitionTo<Clear>();
        }
    }

    private class PBoth : State_Base
    {
        public override void Init()
        {
            timer = 0;
            stringHold = Context.connectedBoth;
        }

        public override void Update()
        {
            PrintOut();
            if (!Context._bothConnected)
                TransitionTo<Clear>();
        }
    }
}
