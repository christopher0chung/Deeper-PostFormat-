using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Rewired;

public enum MenuItemTypes { Basic, Toggle }

public class Deeper_MenuItem
{
    public Deeper_MenuItem(string l)
    {
        visibleLabel = l;
        itemGameObject = new GameObject();
        itemTMP = itemGameObject.AddComponent<TMPro.TextMeshPro>();
        itemTMP.text = visibleLabel;

        itemTMP.fontSize = 10;
        itemTMP.alignment = TextAlignmentOptions.MidlineLeft;
    }

    #region Internal

    public GameObject itemGameObject;
    public TMPro.TextMeshPro itemTMP;
    public string visibleLabel;

    [HideInInspector] public bool _hidden;
    public bool hidden
    {
        get { return _hidden; }
        set
        {
            if (value != _hidden)
            {
                _hidden = value;
                if (_hidden)
                    Deeper_ServicesLocator.instance.TaskManager.AddTask(new Task_MenuAnimation_Invisible(this, TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
                else
                    Deeper_ServicesLocator.instance.TaskManager.AddTask(new Task_MenuAnimation_Visible(this, TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
            }
        }
    }

    [HideInInspector] public bool _highlighted;
    public bool highlighted
    {
        get { return _highlighted; }
        set
        {
            if (value != _highlighted)
            {
                _highlighted = value;
                if (_highlighted)
                    Deeper_ServicesLocator.instance.TaskManager.AddTask(new Task_MenuAnimation_Highlight(this, TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
                else
                    Deeper_ServicesLocator.instance.TaskManager.AddTask(new Task_MenuAnimation_Unhighlight(this, TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
            }
        }
    }

    #endregion
}

public class Deeper_MenuItem_Toggle : Deeper_MenuItem
{
    [HideInInspector] public bool _trueFalse;
    public bool trueFalse
    {
        get { return _trueFalse; }
        set
        {
            if (value != _trueFalse)
            {
                _trueFalse = value;
                if (_trueFalse)
                    itemTMP.text = visibleLabelAlt;
                else
                    itemTMP.text = visibleLabel;
            }
        }
    }
    public string visibleLabelAlt;

    public Deeper_MenuItem_Toggle (string l, string lAlt) : base(l)
    {
        visibleLabelAlt = lAlt;
    }
}

public class Deeper_Menu : MonoBehaviour
{
    public string title;
    private Deeper_MenuItem _title;
    public Deeper_Menu parentPath;

    [Header("The number of names must equal the number of paths")]
    public List<MenuItemTypes> testItemType = new List<MenuItemTypes>();
    public List<string> testItemsNames = new List<string>();
    public List<string> testItemsNamesAlt = new List<string>();
    public List<Deeper_Menu> testPaths = new List<Deeper_Menu>();
    public List<MenuItemActions> testItemAction = new List<MenuItemActions>();

    [HideInInspector] public List<Deeper_MenuItem> _testItems;


    [HideInInspector] public Transform _mCam;

    [HideInInspector] public Vector3 _posActive = Vector3.zero;
    [HideInInspector] public Vector3 _posDeactive = Vector3.right * 25;
    [HideInInspector] public Vector3 _posOpen = Vector3.right * -25;

    [HideInInspector] public FSM<Deeper_Menu> _fsm;
    [HideInInspector] public Deeper_RolloverInt _highlightedItemIndex;

    public int highlightedItemIndex
    {
        get
        {
            if (_highlightedItemIndex == null)
            {
                _highlightedItemIndex = new Deeper_RolloverInt(0, 0, testItemsNames.Count - 1);
                _testItems[_highlightedItemIndex.intVal].highlighted = true;
            }
            return _highlightedItemIndex.intVal;
        }
        set
        {
            if (_highlightedItemIndex == null) _highlightedItemIndex = new Deeper_RolloverInt(0, 0, testItemsNames.Count - 1);

            _testItems[_highlightedItemIndex.intVal].highlighted = false;
            _highlightedItemIndex.intVal = value;
            _testItems[_highlightedItemIndex.intVal].highlighted = true;
        }
    }

    #region Intialization
    public virtual void Awake()
    {
        Debug.Assert(testItemsNames.Count == testPaths.Count, "Deeper_MenuObject on " + this.gameObject.name + ": Items and paths mismatch!");
        Debug.Assert(testItemsNames.Count == testItemAction.Count, "Deeper_MenuObject on " + this.gameObject.name + ": Items and actions mismatch!");
        Debug.Assert(testItemsNames.Count == testItemType.Count, "Deeper_MenuObject on " + this.gameObject.name + ": Items and types mismatch!");
        Debug.Assert(testItemsNames.Count == testItemsNamesAlt.Count, "Deeper_MenuObject on " + this.gameObject.name + ": Items and alt names mismatch!");

        _fsm = new FSM<Deeper_Menu>(this);
    }

    public virtual void Start()
    {
        RewiredRefs();
        _fsm.TransitionTo<Active>();
    }

    public virtual void OnEnable()
    {
        CamReference();
        if (_testItems == null)
        {
            PlaceElements();
        }
    }

    public virtual void PlaceElements()
    {
        transform.parent = _mCam;
        transform.position = _mCam.position;

        float spread = 1;
        float upperHeight = 2f;

        _title = new Deeper_MenuItem(title);
        _title.itemGameObject.transform.position = _mCam.position + _mCam.forward * 20 + _mCam.right * 1 + _mCam.up * 3;
        _title.itemGameObject.transform.SetParent(this.transform);

        _testItems = new List<Deeper_MenuItem>();

        for (int i = 0; i < testItemsNames.Count; i++)
        {
            if (testItemType[i] == MenuItemTypes.Basic)
                _testItems.Add(new Deeper_MenuItem(testItemsNames[i]));
            else if (testItemType[i] == MenuItemTypes.Toggle)
                _testItems.Add(new Deeper_MenuItem_Toggle(testItemsNames[i], testItemsNamesAlt[i]));
            _testItems[i].itemGameObject.transform.position = _mCam.position + _mCam.forward * 20 + _mCam.right * 2f + _mCam.up * (upperHeight - spread * i);
            _testItems[i].itemGameObject.transform.SetParent(this.transform);
        }

        highlightedItemIndex = 0;
    }

    public virtual void CamReference()
    {
        if (_mCam == null)
            _mCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    [HideInInspector] public Rewired.Player[] players = new Rewired.Player[2];

    [HideInInspector] public Rewired.Player player0 { get { return ReInput.isReady ? ReInput.players.GetPlayer(0) : null; } }
    [HideInInspector] public Rewired.Player player1 { get { return ReInput.isReady ? ReInput.players.GetPlayer(1) : null; } }

    public virtual void RewiredRefs()
    {
        players[0] = player0;
        players[1] = player1;
    }

    #endregion

    public virtual void Update()
    {
        _fsm.Update();
        CamReference();
    }

    public virtual void ExternalActivate()
    {
        //Debug.Log("external activate called");
        this.enabled = true;
        //Debug.Log("enabled = " + this.isActiveAndEnabled);
        _fsm.TransitionTo<Activate>();
    }

    public virtual void Unpause()
    {
        _fsm.TransitionTo<Deactivate>();
    }

    #region States for FSM

    public class State_Base : FSM<Deeper_Menu>.State
    {

    }

    public class Active : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            //Debug.Log("The ActiveState is running in " + Context.gameObject.name);
        }

        private bool[] _iFlag = new bool[2];

        public override void Update()
        {
            for (int i = 0; i <= 1; i++)
            {
                if (Mathf.Abs(Context.players[i].GetAxis("Menu Select Vertical")) > .75f && !_iFlag[i])
                {
                    //Debug.Log("Before the set, _iFlag[" + i + "] is set to " + _iFlag[i]);
                    _iFlag[i] = true;
                    //Debug.Log("After the set, _iFlag[" + i + "] is set to " + _iFlag[i]);

                    if (Context.players[i].GetAxis("Menu Select Vertical") > 0)
                    {
                        Context.highlightedItemIndex--;
                    }
                    else if (Context.players[i].GetAxis("Menu Select Vertical") < 0)
                    {
                        Context.highlightedItemIndex++;
                    }
                }
                else if (Mathf.Abs(Context.players[i].GetAxis("Menu Select Vertical")) <= .15f)
                {
                    _iFlag[i] = false;
                }

                if (Context.players[i].GetButtonDown("Menu Accept"))
                {
                    if (Context.testPaths[Context.highlightedItemIndex] != null)
                    {
                        Context.testPaths[Context.highlightedItemIndex].ExternalActivate();
                        TransitionTo<Open>();
                    }

                    if (Context.testItemType[Context.highlightedItemIndex] == MenuItemTypes.Toggle)
                    {
                        Deeper_MenuItem_Toggle item = Context._testItems[Context.highlightedItemIndex] as Deeper_MenuItem_Toggle;
                        if (item != null)
                        {
                            item.trueFalse = !item.trueFalse;
                        }

                    }

                    Deeper_MenuAid.instance.GetAction(Context.testItemAction[Context.highlightedItemIndex]).Invoke();
                }

                if (Context.players[i].GetButtonDown("Cancel"))
                {
                    if (Context.parentPath != null)
                    {
                        Context.parentPath.ExternalActivate();
                        TransitionTo<Deactivate>();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Context.highlightedItemIndex++;
            }
        }
    }

    public class Activate : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            //Debug.Log("The ActivateState is running in " + Context.gameObject.name);

            Context._title.hidden = false;

            foreach (Deeper_MenuItem m in Context._testItems)
            {
                m.hidden = false;
            }
        }

        public override void Update()
        {
            Context.transform.localPosition = Vector3.Lerp(Context.transform.localPosition, Context._posActive, .08f);
            if (Vector3.Distance(Context.transform.localPosition, Context._posActive) < .01f)
            {
                TransitionTo<Active>();
            }
        }

        public override void OnExit()
        {
            Context.transform.localPosition = Context._posActive;
        }
    }

    public class Deactivate : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            //Debug.Log("The DectivateState is running in " + Context.gameObject.name);

            Context._title.hidden = true;

            foreach (Deeper_MenuItem m in Context._testItems)
            {
                m.hidden = true;
            }
        }

        public override void Update()
        {
            Context.transform.localPosition = Vector3.Lerp(Context.transform.localPosition, Context._posDeactive, .08f);

            if (Vector3.Distance(Context.transform.localPosition, Context._posDeactive) < .01f)
            {
                TransitionTo<State_Base>();
            }
        }

        public override void OnExit()
        {
            Context.transform.localPosition = Context._posDeactive;
            //Way to up a level of menu
            //if (!Context.parentPath.isActiveAndEnabled)
            //    Context.parentPath.ExternalActivate();
            Context.enabled = false;
        }
    }

    public class Open : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            //Debug.Log("The DectivateState is running in " + Context.gameObject.name);

            Context._title.hidden = true;

            foreach (Deeper_MenuItem m in Context._testItems)
            {
                m.hidden = true;
            }
        }

        public override void Update()
        {
            Context.transform.localPosition = Vector3.Lerp(Context.transform.localPosition, Context._posOpen, .08f);
            if (Vector3.Distance(Context.transform.localPosition, Context._posOpen) < .01f)
            {
                TransitionTo<State_Base>();
            }
        }

        public override void OnExit()
        {
            Context.transform.localPosition = Context._posOpen;
            Context.enabled = false;
        }
    }

    #endregion
}

public class Deeper_MenuAid
{

    #region Instance
    private static Deeper_MenuAid _instance;
    public static Deeper_MenuAid instance
    {
        get { return (_instance != null)? _instance :_instance = new Deeper_MenuAid(); }
    }

    private Deeper_MenuAid()
    {
        _menuActionsDict = new Dictionary<MenuItemActions, Action>();

        _menuActionsDict.Add(MenuItemActions.None, DoNothing);
        _menuActionsDict.Add(MenuItemActions.SetP1Doc, SetP1Doc);
        _menuActionsDict.Add(MenuItemActions.ChangeCamTests, Test);
    }
    #endregion

    #region Storage and Functions
    private Dictionary<MenuItemActions, Action> _menuActionsDict;

    private void DoNothing()
    {
        Debug.Log("Do Nothing");
    }

    private void SetP1Doc ()
    {
        Debug.Log("This is going to set P1");
    }

    private void Test ()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
    }
    #endregion

    public Action GetAction (MenuItemActions mIM)
    {
        Action a;
        _menuActionsDict.TryGetValue(mIM, out a);
        return a;
    }
}

public enum MenuItemActions { None, SetP1Doc, ChangeCamTests }