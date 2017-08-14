using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable_Base : Deeper_Component {

    public InteractionTaskIcons whoCanIneract;

    public InteractableState startingState;

    public float interactTime;
    public float interactRange;

    [HideInInspector] public bool inInteractRange;

    protected InteractableState _state = InteractableState.Standby;
    public InteractableState state
    {
        get { return _state; }
        set
        {
            if (value != _state)
            {
                _state = value;
                if (_state == InteractableState.Standby)
                    _fsm.TransitionTo<Standby>();
                else if (_state == InteractableState.Available_Invisible)
                    _fsm.TransitionTo<Available_Invisible>();
                else if (_state == InteractableState.Available_Visible)
                    _fsm.TransitionTo<Available_Visible>();
                else
                    _fsm.TransitionTo<Interacted>();
            }
        }
    }

    protected Game_Logic _thisSceneGameLogic;

    protected FSM<Interactable_Base> _fsm;

    protected void Awake()
    {
        Initialize(1000);
    }

    protected void Start()
    {
        _thisSceneGameLogic = GameObject.Find("Managers_Game").GetComponent<Game_Logic>();
        _thisSceneGameLogic.RegisterInteractable(this);

        _fsm = new FSM<Interactable_Base>(this);
        if (startingState == InteractableState.Standby)
            _fsm.TransitionTo<Standby>();
        else
            state = startingState;

        CreateIcon(whoCanIneract);
    }

    public override void NormUpdate()
    {
        _fsm.Update();
    }

    [HideInInspector] public Task t;

    protected GameObject iconBorder;
    protected GameObject iconSymbol;
    protected RectTransform rtIconBorder;
    protected RectTransform rtIconSymbol;
    protected Image imgIconBorder;
    protected Image imgIconSymbol;

    protected void CreateIcon(InteractionTaskIcons i)
    {
        iconBorder = new GameObject();
        iconBorder.name = "IconBorder";

        iconBorder.AddComponent<Canvas>();
        iconBorder.AddComponent<CanvasRenderer>();
        iconBorder.AddComponent<Image>();

        rtIconBorder = iconBorder.GetComponent<RectTransform>();
        imgIconBorder = iconBorder.GetComponent<Image>();

        Sprite s = Resources.Load("UI/II_Border", typeof(Sprite)) as Sprite;
        imgIconBorder.sprite = s;

        rtIconBorder.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2);
        rtIconBorder.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 2);
        rtIconBorder.localScale = Vector3.one;
        rtIconBorder.rotation = Quaternion.identity;


        iconSymbol = new GameObject();
        iconSymbol.name = "IconSymbol";

        iconSymbol.AddComponent<Canvas>();
        iconSymbol.AddComponent<CanvasRenderer>();
        iconSymbol.AddComponent<Image>();

        rtIconSymbol = iconSymbol.GetComponent<RectTransform>();
        imgIconSymbol = iconSymbol.GetComponent<Image>();

        Sprite sy;
        if (i == InteractionTaskIcons.Doc)
            sy = Resources.Load("UI/II_Doc", typeof(Sprite)) as Sprite;
        else if (i == InteractionTaskIcons.Ops)
            sy = Resources.Load("UI/II_Ops", typeof(Sprite)) as Sprite;
        else
            sy = Resources.Load("UI/II_Either", typeof(Sprite)) as Sprite;

        imgIconSymbol.sprite = sy;
        rtIconSymbol.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2);
        rtIconSymbol.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 2);
        rtIconSymbol.localScale = Vector3.one;
        rtIconSymbol.rotation = Quaternion.identity;

        iconSymbol.transform.SetParent(iconBorder.transform);
        rtIconSymbol.localPosition = Vector3.forward * -.05f;
    }

    #region Not In Use
    //private bool _display;
    //private bool display
    //{
    //    get
    //    {
    //        return _display;
    //    }

    //    set
    //    {
    //        if (value != _display)
    //        {
    //            _display = value;

    //            if (t != null)
    //                t.Abort();

    //            if (_display)
    //            {
    //                t = new Task_Interactable_FadeIn(whoCanIneract, transform.position);
    //                Deeper_ServicesLocator.instance.TaskManager.AddTask(t);
    //            }
    //            else
    //            {
    //                t = new Task_Interactable_FadeOut(whoCanIneract, transform.position);
    //                Deeper_ServicesLocator.instance.TaskManager.AddTask(t);
    //            }
    //        }
    //    }
    //}


    //public void SetDisplay(bool shouldBeVisible)
    //{
    //    if (_state == InteractableState.Available)
    //        display = shouldBeVisible;
    //    else
    //        display = false;
    //}

    //public override void NormUpdate()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha0))
    //        display = !display;
    //}
    #endregion

    #region States

    public class State_Base : FSM<Interactable_Base>.State
    {
        public void RefreshIcon(InteractionTaskIcons i )
        {
            Sprite sy;
            if (i == InteractionTaskIcons.Doc)
                sy = Resources.Load("UI/II_Doc", typeof(Sprite)) as Sprite;
            else if (i == InteractionTaskIcons.Ops)
                sy = Resources.Load("UI/II_Ops", typeof(Sprite)) as Sprite;
            else
                sy = Resources.Load("UI/II_Either", typeof(Sprite)) as Sprite;
            Context.imgIconSymbol.sprite = sy;
        }

        public Color borderVisColor;
        public Color symbolVisColor;

        public Color borderHalfVisColor;
        public Color symbolHalfVisColor;

        public Color borderInvisColor;
        public Color symbolInvisColor;

        public void SelectColor(InteractionTaskIcons i )
        {
            if (Context.whoCanIneract == InteractionTaskIcons.Doc)
            {
                borderInvisColor = borderVisColor = borderHalfVisColor = new Color(0, 0, 1, 1);
                borderInvisColor.a = 0;
                borderHalfVisColor.a = .25f;

                symbolInvisColor = symbolVisColor = symbolHalfVisColor = new Color(1, 1, 1, 1);
                symbolInvisColor.a = 0;
                symbolHalfVisColor.a = .25f;
            }
            else if (Context.whoCanIneract == InteractionTaskIcons.Ops)
            {
                borderInvisColor = borderVisColor = borderHalfVisColor = new Color(1, 0, 0, 1);
                borderInvisColor.a = 0;
                borderHalfVisColor.a = .25f;

                symbolInvisColor = symbolVisColor = symbolHalfVisColor = new Color(1, 1, 1, 1);
                symbolInvisColor.a = 0;
                symbolHalfVisColor.a = .25f;
            }
            else
            {
                borderInvisColor = borderVisColor = borderHalfVisColor = new Color(.4f, 0, 1, 1);
                borderInvisColor.a = 0;
                borderHalfVisColor.a = .25f;

                symbolInvisColor = symbolVisColor = symbolHalfVisColor = new Color(1, 1, 1, 1);
                symbolInvisColor.a = 0;
                symbolHalfVisColor.a = .25f;
            }
        }

        public override void OnEnter()
        {
            //Context.CreateIcon(Context.whoCanIneract);
            SelectColor(Context.whoCanIneract);
            Context.rtIconBorder.position = Context.transform.position + Vector3.forward * -5;
            Context.imgIconBorder.color = borderInvisColor;
            Context.imgIconSymbol.color = symbolInvisColor;
        }

    }

    protected class Standby : State_Base
    {

    }

    protected class Available_Invisible : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            RefreshIcon(Context.whoCanIneract);
        }

        public override void Update()
        {
            Context.imgIconBorder.color = Color.Lerp(Context.imgIconBorder.color, borderInvisColor, .1f);
            Context.imgIconSymbol.color = Color.Lerp(Context.imgIconSymbol.color, symbolInvisColor, .1f);
        }
    }

    protected class Available_Visible : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            RefreshIcon(Context.whoCanIneract);
        }

        public override void Update()
        {
            if (Context.inInteractRange)
            {
                Context.imgIconBorder.color = Color.Lerp(Context.imgIconBorder.color, borderVisColor, .1f);
                Context.imgIconSymbol.color = Color.Lerp(Context.imgIconSymbol.color, symbolVisColor, .1f);
            }
            else
            {
                Context.imgIconBorder.color = Color.Lerp(Context.imgIconBorder.color, borderHalfVisColor, .1f);
                Context.imgIconSymbol.color = Color.Lerp(Context.imgIconSymbol.color, symbolHalfVisColor, .1f);
            }
        }
        //public override void OnEnter()
        //{
        //    if (Context.t != null)
        //        Context.t.Abort();

        //    Context.t = new Task_Interactable_FadeIn(Context.whoCanIneract, Context.transform.position);
        //    Deeper_ServicesLocator.instance.TaskManager.AddTask(Context.t);
        //}

        //public override void OnExit()
        //{
        //    if (Context.t != null)
        //        Context.t.Abort();

        //    Context.t = new Task_Interactable_FadeOut(Context.whoCanIneract, Context.transform.position);
        //    Deeper_ServicesLocator.instance.TaskManager.AddTask(Context.t);
        //}
    }

    public class Interacted : State_Base
    {
        public override void OnEnter()
        {
            Context.imgIconBorder.color = borderInvisColor;
            Context.imgIconSymbol.color = symbolInvisColor;
        }

        //public override void OnEnter()
        //{
        //    if (Context.t != null)
        //        Context.t.Abort();

        //    Context.t = new Task_Interactable_FadeOut(Context.whoCanIneract, Context.transform.position);
        //    Deeper_ServicesLocator.instance.TaskManager.AddTask(Context.t);
        //}
    }
    
    #endregion
}

public enum InteractableState { Standby, Available_Invisible, Available_Visible, Interacted }
