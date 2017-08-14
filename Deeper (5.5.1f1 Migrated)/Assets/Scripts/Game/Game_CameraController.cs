using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_CameraController : MonoBehaviour {

    [Header("Camera_Menu")]
    public GameObject cam_menu;

    [Header("Camera_Combined")]
    public GameObject cam_com;

    [Header("Cameras_Individual")]
    public GameObject cam_c1;
    public GameObject cam_c2;

    [Header("Camera_UI")]
    public GameObject cam_UI;

    [Header("References")]
    [SerializeField] private GameObject Sub;
    [SerializeField] private GameObject Ops;
    [SerializeField] private GameObject Doc;

    private FSM<Game_CameraController> _fsm;
    private CameraModes _lastMode;

    [Header("Tuning Values (OneToTwo must be greater than TwoToOne")]
    public float OneToTwo;
    public float TwoToOne;

    private void Awake()
    {
        Deeper_EventManager.instance.Register<Deeper_Event_Pause>(_PauseHandler);
        Deeper_EventManager.instance.Register<Deeper_Event_LevelUnload>(_UnloadHandler);
    }

    void Start () {

        _fsm = new FSM<Game_CameraController>(this);
        _fsm.TransitionTo<SingleCam>();
    }
	
	private void Update () {
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
        if (_lastMode == CameraModes.Single)
            _fsm.TransitionTo<SingleCam>();
        else
            _fsm.TransitionTo<DualCam>();
    }
#endregion

    #region FSM States
    private class State_Base : FSM<Game_CameraController>.State
    {

    }

    private class SingleCam : State_Base
    {
        public override void OnEnter()
        {
            Context.cam_com.GetComponent<Game_CameraLogic>().TurnOn();
            Context.cam_c1.GetComponent<Game_CameraLogic>().TurnOff();
            Context.cam_c2.GetComponent<Game_CameraLogic>().TurnOff();
            Context.cam_menu.GetComponent<Game_CameraLogic>().TurnOff();
            Context.cam_UI.GetComponent<Game_CameraLogic>().TurnOn();

            Context._lastMode = CameraModes.Single;

            Deeper_EventManager.instance.Fire(new Deeper_Event_CamSwitch());
            Deeper_EventManager.instance.Fire(new Deeper_Event_CamSingleSplit(false));
        }

        public override void Update()
        {
            if (Vector3.Distance(Context.Ops.transform.position, Context.Doc.transform.position) >= Context.OneToTwo)
                TransitionTo<DualCam>();
        }
    }

    private class DualCam : State_Base
    {
        public override void OnEnter()
        {
            Context.cam_c1.GetComponent<Game_CameraLogic>().TurnOn();
            Context.cam_c2.GetComponent<Game_CameraLogic>().TurnOn();
            Context.cam_com.GetComponent<Game_CameraLogic>().TurnOff();
            Context.cam_menu.GetComponent<Game_CameraLogic>().TurnOff();
            Context.cam_UI.GetComponent<Game_CameraLogic>().TurnOn();

            Context._lastMode = CameraModes.Split;

            Deeper_EventManager.instance.Fire(new Deeper_Event_CamSwitch());
            Deeper_EventManager.instance.Fire(new Deeper_Event_CamSingleSplit(true));
        }

        public override void Update()
        {
            if (Vector3.Distance(Context.Ops.transform.position, Context.Doc.transform.position) <= Context.TwoToOne)
                TransitionTo<SingleCam>();
        }
    }

    private class Paused : State_Base
    {
        public override void OnEnter()
        {
            Context.cam_menu.GetComponent<Game_CameraLogic>().TurnOn();
            Context.cam_c1.GetComponent<Game_CameraLogic>().TurnOff();
            Context.cam_c2.GetComponent<Game_CameraLogic>().TurnOff();
            Context.cam_com.GetComponent<Game_CameraLogic>().TurnOff();
            Context.cam_UI.GetComponent<Game_CameraLogic>().TurnOff();

            Deeper_EventManager.instance.Fire(new Deeper_Event_CamSwitch());
        }
    }
    
    #endregion
}

public enum CameraModes { Single, Split }
