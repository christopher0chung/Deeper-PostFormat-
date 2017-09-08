using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Game_CutsceneLogic : MonoBehaviour
{
    public Deeper_Menu0_Type0 menuBase;

    private FSM<Game_CutsceneLogic> _fsm;

    private void Start()
    {
        //for (int i = 1; i < Menus.Length; i++)
        //{
        //    Menus[i].Unpause();
        //}

        _fsm = new FSM<Game_CutsceneLogic>(this);
        _fsm.TransitionTo<Playing>();
    }

    void Update()
    {
        _fsm.Update();
    }

    #region Context Functions

    private float DistanceToLine(Ray ray, Vector3 point)
    {
        return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
    }

    #endregion

   #region Public Functions for Menu

    public void UnpauseGameLogic()
    {
        _fsm.TransitionTo<Playing>();
    }

    #endregion

    #region States
    private class State_Base : FSM<Game_CutsceneLogic>.State { }

    private class Paused : State_Base
    {
        public override void OnEnter()
        {
            Deeper_ServicesLocator.instance.Pause();
            //Context.Menus[0].ExternalActivate();

            Context.menuBase.TurnOn();

            Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Menu));

            Deeper_EventManager.instance.Fire(new Deeper_Event_Pause(true));
        }

        public override void Update()
        {
            if (ReInput.players.GetPlayer(0).GetButtonDown("Start") || ReInput.players.GetPlayer(1).GetButtonDown("Start"))
            {
                TransitionTo<Playing>();
            }
        }
    }

    private class Playing : State_Base
    {
        public override void OnEnter()
        {
            Deeper_ServicesLocator.instance.Unpause();

            Deeper_EventManager.instance.Fire(new Deeper_Event_Pause(false));
        }

        public override void Update()
        {
            if (ReInput.players.GetPlayer(0).GetButtonDown("Start") || ReInput.players.GetPlayer(1).GetButtonDown("Start"))
            {
                TransitionTo<Paused>();
            }
        }
    }
    #endregion
}

