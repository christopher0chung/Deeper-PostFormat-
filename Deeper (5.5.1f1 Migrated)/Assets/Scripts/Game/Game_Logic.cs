using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class Game_Logic : MonoBehaviour {

    //Coordinate pausing and unpausing
    //Modifies 

    public Controlled_Character Doc;
    public GameObject DocLight;
    public Controlled_Character Ops;
    public GameObject OpsLight;
    public GameObject Sub;

    public Deeper_MenuObject[] Menus = new Deeper_MenuObject[1];

    private FSM<Game_Logic> _fsm;

    public List<Interactable_Base> _levelInteractables = new List<Interactable_Base>();

    #region Scene Manager

    //private static Deeper_SceneManager _dsm;
    //public static Deeper_SceneManager DSM
    //{
    //    get
    //    {
    //        if (_dsm == null)
    //            _dsm = new Deeper_SceneManager();
    //        return _dsm;
    //    }
    //}

    public void SceneChangeHandler(Deeper_Event e)
    {
        Deeper_Event_LevelLoad ll = e as Deeper_Event_LevelLoad;

        Deeper_EventManager.instance.Unregister<Deeper_Event_LevelLoad>(SceneChangeHandler);

        Deeper_EventManager.instance.Fire(new Deeper_Event_LevelUnload());

        SceneManager.LoadScene(0);
    }

    #endregion

    private void Start()
    {
        for (int i = 1; i < Menus.Length; i++)
        {
            Menus[i].Unpause();
        }

        _fsm = new FSM<Game_Logic>(this);
        _fsm.TransitionTo<Paused>();

        Deeper_EventManager.instance.Register<Deeper_Event_LevelLoad>(SceneChangeHandler);
    }

    void Update() {
        _fsm.Update();
        InteractablesRefresh();
    }

    #region Context Functions

    private float DistanceToLine(Ray ray, Vector3 point)
    {
        return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
    }

    #endregion

    public int RegisterInteractable(Interactable_Base i)
    {
        int n = _levelInteractables.Count;
        _levelInteractables.Add(i);
        return n;
    }

    private Interactable_Base _DocInteractableActive = null;
    private Interactable_Base _OpsInteractableActive = null;

    private void InteractablesRefresh()
    {
        List<Interactable_Base> DocClose = new List<Interactable_Base>();
        List<Interactable_Base> OpsClose = new List<Interactable_Base>();

        _DocInteractableActive = null;
        _OpsInteractableActive = null;

        _Step1GrabAllInRange(DocClose, OpsClose, 15);

        DocClose = _Step2SortDoc(DocClose);
        OpsClose = _Step2SortOps(OpsClose);

        _DocInteractableActive = _Step3DocSelect(DocClose);
        _OpsInteractableActive = _Step3OpsSelect(OpsClose);

        _Step4Highlight(DocClose, OpsClose, _DocInteractableActive, _OpsInteractableActive);
        //Debug.Log(OpsClose.Count);
    }

    #region Interactables Substeps
    private float discoverDist = 5;

    private void _Step1GrabAllInRange(List<Interactable_Base> DList, List<Interactable_Base> OList, float range)
    {
        foreach (Interactable_Base i in _levelInteractables)
        {
            if (Vector3.Distance(i.transform.position, Doc.transform.position) <= range && (i.state == InteractableState.Available_Invisible || i.state == InteractableState.Available_Visible))
            {
                DList.Add(i);
            }
            if (Vector3.Distance(i.transform.position, Ops.transform.position) <= range && (i.state == InteractableState.Available_Invisible || i.state == InteractableState.Available_Visible))
            {
                OList.Add(i);
            }

            if (Vector3.Distance(i.transform.position, Doc.transform.position) < i.interactRange)
                i.inInteractRange = true;
            else if (Vector3.Distance(i.transform.position, Ops.transform.position) < i.interactRange)
                i.inInteractRange = true;
            else
                i.inInteractRange = false;
        }
    }

    private List<Interactable_Base> _Step2SortDoc(List<Interactable_Base> DList)
    {
        DList.Sort(delegate (Interactable_Base a, Interactable_Base b)
        {
            return (Vector3.Distance(a.transform.position, Doc.transform.position).CompareTo(Vector3.Distance(b.transform.position, Doc.transform.position)));
        });
        return DList;
    }

    private List<Interactable_Base> _Step2SortOps(List<Interactable_Base> OList)
    {
        OList.Sort(delegate (Interactable_Base a, Interactable_Base b)
        {
            return (Vector3.Distance(a.transform.position, Ops.transform.position).CompareTo(Vector3.Distance(b.transform.position, Ops.transform.position)));
        });
        return OList;
    }

    private Interactable_Base _Step3DocSelect(List<Interactable_Base> DList)
    {
        if (DList.Count > 0)
        {
            for (int i = 0; i < DList.Count; i++)
            {
                if (DistanceToLine(new Ray(DocLight.transform.position, DocLight.transform.forward), DList[i].transform.position) < 1)
                {
                    return DList[i];
                }
            }
            for (int i = 0; i < DList.Count; i++)
            {
                if (Vector3.Distance(DocLight.transform.position, DList[i].transform.position) < discoverDist)
                {
                    return DList[i];
                }
            }
        }
        return null;
    }

    private Interactable_Base _Step3OpsSelect(List<Interactable_Base> OList)
    {
        if (OList.Count > 0)
        {
            for (int i = 0; i < OList.Count; i++)
            {
                if (DistanceToLine(new Ray(OpsLight.transform.position, OpsLight.transform.forward * 14), OList[i].transform.position) < 1)
                {
                    //Debug.Log("Found one with light");
                    return OList[i];
                }
            }
            for (int i = 0; i < OList.Count; i++)
            {
                if (Vector3.Distance(OpsLight.transform.position, OList[i].transform.position) < discoverDist)
                {
                    //Debug.Log("Found one in front inside 3");
                    return OList[i];
                }
            }
        }
        //Debug.Log("Found none");
        return null;
    }

    private void _Step4Highlight(List<Interactable_Base> DList, List<Interactable_Base> OList, Interactable_Base DActive, Interactable_Base OActive)
    {
        foreach (Interactable_Base i in _levelInteractables)
        {
            if (DList.Count > 0 && DActive == i)
            {
                i.state = InteractableState.Available_Visible;
                //Debug.Log("DActive");
            }
            else if (OList.Count > 0 && OActive == i)
            {
                i.state = InteractableState.Available_Visible;
                //Debug.Log("OActive");
            }
            else if (i.state == InteractableState.Available_Visible)
            {
                i.state = InteractableState.Available_Invisible;
                //Debug.Log("Not DActive or OActive");
            }
        }
    }
    #endregion

    #region Player Initiated Interaction

    public delegate void InteractionInitCallback(bool success);
    public delegate void InteractionCompletionCallback(bool success);

    private Task _DocActiveTaskRef;
    private Task _OpsActiveTaskRef;

    public void AttemptToInteract(Controlled_Character who, InteractionInitCallback initCallback, InteractionCompletionCallback compCallback)
    {
        if (who.thisChar == CharactersEnum.Doc)
        {
            if (_DocInteractableActive != null && (_DocInteractableActive.whoCanIneract == InteractionTaskIcons.Doc || _DocInteractableActive.whoCanIneract == InteractionTaskIcons.Either) && Vector3.Distance(_DocInteractableActive.transform.position, Doc.transform.position) <= _DocInteractableActive.interactRange)
            {
                initCallback(true);
                Interactable_Linked l = _DocInteractableActive as Interactable_Linked;
                if (l!= null)
                {
                    _DocActiveTaskRef = new Task_Interaction_InProgress_Linked(compCallback, l, l.link, who, _DocInteractableActive.interactTime);
                    Deeper_ServicesLocator.instance.TaskManager.AddTask(_DocActiveTaskRef);
                    return;
                }
                else
                {
                    _DocActiveTaskRef = new Task_Interaction_InProgress_WCB(compCallback, _DocInteractableActive, who, _DocInteractableActive.interactTime);
                    Deeper_ServicesLocator.instance.TaskManager.AddTask(_DocActiveTaskRef);
                    return;
                }
            }
        }
        else if (who.thisChar == CharactersEnum.Ops)
        {
            if (_OpsInteractableActive != null && (_OpsInteractableActive.whoCanIneract == InteractionTaskIcons.Ops || _OpsInteractableActive.whoCanIneract == InteractionTaskIcons.Either) && Vector3.Distance(_OpsInteractableActive.transform.position, Ops.transform.position) <= _OpsInteractableActive.interactRange)
            {
                initCallback(true);
                Interactable_Linked l = _OpsInteractableActive as Interactable_Linked;
                if (l != null)
                {
                    _OpsActiveTaskRef = new Task_Interaction_InProgress_Linked(compCallback, l, l.link, who, _OpsInteractableActive.interactTime);
                    Deeper_ServicesLocator.instance.TaskManager.AddTask(_OpsActiveTaskRef);
                    return;
                }
                else
                {
                    _OpsActiveTaskRef = new Task_Interaction_InProgress_WCB(compCallback, _OpsInteractableActive, who, _DocInteractableActive.interactTime);
                    Deeper_ServicesLocator.instance.TaskManager.AddTask(_OpsActiveTaskRef);
                    return;
                }
            }
        }
        initCallback(false);
    }

    public void AttemptToCancel(Controlled_Character who)
    {
        if (who.thisChar == CharactersEnum.Doc)
        {
            if (_DocActiveTaskRef != null)
            {
                _DocActiveTaskRef.SetStatus(TaskStatus.Aborted);
            }
        }
        else if (who.thisChar == CharactersEnum.Ops)
        {
            if (_OpsActiveTaskRef != null)
            {
                _OpsActiveTaskRef.SetStatus(TaskStatus.Aborted);
            }
        }
    }

    #endregion

    #region States
    private class State_Base : FSM<Game_Logic>.State { }

    private class Paused : State_Base
    {
        public override void OnEnter()
        {
            Deeper_ServicesLocator.instance.Pause();
            Context.Menus[0].ExternalActivate();

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
            foreach (Deeper_MenuObject g in Context.Menus)
            {
                g.Unpause();
            }

            if (Context.Doc.gameObject.GetComponent<Collider>().enabled)
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_Outside));
            else
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_Inside));

            if (Context.Ops.gameObject.GetComponent<Collider>().enabled)
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_Outside));
            else
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_Inside));

            Deeper_EventManager.instance.Fire(new Deeper_Event_Pause(false));
        }

        public override void Update()
        {
            if (ReInput.players.GetPlayer(0).GetButtonDown("Start") || ReInput.players.GetPlayer(1).GetButtonDown("Start"))
            {
                TransitionTo<Paused>();
            }
            #region Not Used
            //if (!Context.Doc.activeSelf)
            //    Context.Doc.transform.position = Context.Sub.transform.position - Vector3.right * 1.76f - Vector3.up * 3.08f;

            //if (!Context.Ops.activeSelf)
            //    Context.Ops.transform.position = Context.Sub.transform.position - Vector3.right * 1.76f - Vector3.up * 3.08f;

            //if (ReInput.players.GetPlayer(0).GetButtonDown("Sub Egress"))
            //{
            //    if (Deeper_ServicesLocator.instance.GetCharacter(ControllersEnum.C0) == CharactersEnum.Doc)
            //    {
            //        Context.Doc.transform.position = pos;
            //        Context.Doc.SetActive(true);
            //        Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_Outside));

            //    }
            //    else
            //    {
            //        Context.Ops.transform.position = pos;
            //        Context.Ops.SetActive(true);
            //        Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_Outside));

            //    }
            //}
            //if (ReInput.players.GetPlayer(0).GetButtonDown("Player Action"))
            //{
            //    if (Deeper_ServicesLocator.instance.GetCharacter(ControllersEnum.C0) == CharactersEnum.Doc)
            //    {
            //        Context.Doc.SetActive(false);
            //        Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_Inside));
            //    }
            //    else
            //    {
            //        Context.Ops.SetActive(false);
            //        Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_Inside));
            //    }
            //}

            //if (ReInput.players.GetPlayer(1).GetButtonDown("Sub Egress"))
            //{
            //    Vector3 pos = Context.Sub.transform.position - Vector3.right * 1.76f - Vector3.up * 3.08f;
            //    if (Deeper_ServicesLocator.instance.GetCharacter(ControllersEnum.C1) == CharactersEnum.Doc)
            //    {
            //        Context.Doc.transform.position = pos;
            //        Context.Doc.SetActive(true);
            //        Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_Outside));

            //    }
            //    else
            //    {
            //        Context.Ops.transform.position = pos;
            //        Context.Ops.SetActive(true);
            //        Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_Outside));

            //    }
            //}
            //if (ReInput.players.GetPlayer(1).GetButtonDown("Player Action"))
            //{
            //    if (Deeper_ServicesLocator.instance.GetCharacter(ControllersEnum.C1) == CharactersEnum.Doc)
            //    {
            //        Context.Doc.SetActive(false);
            //        Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_Inside));
            //    }
            //    else
            //    {
            //        Context.Ops.SetActive(false);
            //        Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_Inside));
            //    }
            ////}
            #endregion
        }
    }
    #endregion
}

