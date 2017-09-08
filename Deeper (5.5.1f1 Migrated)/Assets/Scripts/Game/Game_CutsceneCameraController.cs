using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_CutsceneCameraController : MonoBehaviour
{
    [Header("Camera_Menu")]
    public GameObject cam_menu;

    [Header("Camera_Combined")]
    public GameObject cam_com;

    [Header("Camera_UI")]
    public GameObject cam_UI;

    private FSM<Game_CutsceneCameraController> _fsm;

    private void Awake()
    {
        Deeper_EventManager.instance.Register<Deeper_Event_Pause>(_PauseHandler);
        Deeper_EventManager.instance.Register<Deeper_Event_LevelUnload>(_UnloadHandler);
    }

    void Start()
    {
        _fsm = new FSM<Game_CutsceneCameraController>(this);
        _fsm.TransitionTo<SingleCam>();
    }

    private void Update()
    {
        _fsm.Update();
    }

    #region Handlers
    private void _PauseHandler(Deeper_Event e)
    {
        Deeper_Event_Pause p = e as Deeper_Event_Pause;
        if (p != null)
        {
            if (p.isPaused)
                Pause();
            else
                UnPause();
        }
    }

    private void _UnloadHandler(Deeper_Event e)
    {
        Deeper_Event_LevelUnload u = e as Deeper_Event_LevelUnload;
        if (u != null)
        {
            Deeper_EventManager.instance.Unregister<Deeper_Event_Pause>(_PauseHandler);
            Deeper_EventManager.instance.Unregister<Deeper_Event_LevelUnload>(_UnloadHandler);
        }
    }
    #endregion

    #region Context Functions
    public void Pause()
    {
        _fsm.TransitionTo<Paused>();
    }

    public void UnPause()
    {
            _fsm.TransitionTo<SingleCam>();
    }
    #endregion

    #region FSM States
    private class State_Base : FSM<Game_CutsceneCameraController>.State
    {

    }

    private class SingleCam : State_Base
    {
        public override void OnEnter()
        {
            Context.cam_com.GetComponent<Game_CameraLogic>().TurnOn();
            Context.cam_menu.GetComponent<Game_CameraLogic>().TurnOff();
            Context.cam_UI.GetComponent<Game_CameraLogic>().TurnOn();

            Deeper_EventManager.instance.Fire(new Deeper_Event_CamSwitch());
            Deeper_EventManager.instance.Fire(new Deeper_Event_CamSingleSplit(false));

            Deeper_ServicesLocator.instance.SFXManager.PlaySoundPauseable(SFX.Static, .1f, .1f);
        }
    }

    private class Paused : State_Base
    {
        public override void OnEnter()
        {
            Context.cam_menu.GetComponent<Game_CameraLogic>().TurnOn();
            Context.cam_com.GetComponent<Game_CameraLogic>().TurnOff();
            Context.cam_UI.GetComponent<Game_CameraLogic>().TurnOff();

            Deeper_EventManager.instance.Fire(new Deeper_Event_CamSwitch());

            Deeper_ServicesLocator.instance.SFXManager.PlaySoundPauseable(SFX.Static, .1f, .1f);
        }
    }
    #endregion
}
