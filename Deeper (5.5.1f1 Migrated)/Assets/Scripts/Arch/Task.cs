using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public enum TaskStatus : byte { Detached, Pending, Working, Success, Fail, Aborted }

public class Task
{
    public TaskStatus Status { get; private set; }
    public Task NextTask { get; private set; }

    public TaskCanBeInterrupted CanBeInterrupted;
    public TaskDoesInterrupt DoesInterrupt;

    public bool IsDetached { get { return Status == TaskStatus.Detached; } }
    public bool IsAttached { get { return Status != TaskStatus.Detached; } }
    public bool IsPending { get { return Status == TaskStatus.Pending; } }
    public bool IsWorking { get { return Status == TaskStatus.Working; } }
    public bool IsSuccessful { get { return Status == TaskStatus.Success; } }
    public bool IsFailed { get { return Status == TaskStatus.Fail; } }
    public bool IsAborted { get { return Status == TaskStatus.Aborted; } }
    public bool IsFinished { get { return (Status == TaskStatus.Fail || Status == TaskStatus.Success || Status == TaskStatus.Aborted); } }

    internal void SetStatus(TaskStatus newStatus)
    {
        if (Status == newStatus) return;

        Status = newStatus;

        switch (newStatus)
        {
            case TaskStatus.Working:
                Init();
                break;

            case TaskStatus.Success:
                OnSuccess();
                CleanUp();
                break;

            case TaskStatus.Aborted:
                OnAbort();
                CleanUp();
                break;

            case TaskStatus.Fail:
                OnFail();
                CleanUp();
                break;

            case TaskStatus.Detached:
            case TaskStatus.Pending:
                break;
            default:
                Debug.Log("Task error:" + Status);
                break;
        }
    }

    public virtual void Init() { }

    public virtual void TaskUpdate() { }
    public virtual void TaskDeeperEarlyUpdate() { }
    public virtual void TaskDeeperNormUpdate() { }
    public virtual void TaskDeeperPostUpdate() { }

    public virtual void CleanUp() { }

    public virtual void OnSuccess() { }

    public virtual void OnFail() { }

    public virtual void OnAbort() { }

    public virtual void Abort()
    {
        SetStatus(TaskStatus.Aborted);
    }

    public Task Then(Task task)
    {
        Debug.Assert(!task.IsAttached);
        NextTask = task;
        return task;
    }
}

public class TestTask : Task
{
    private GameObject gO;
    private TMPro.TextMeshPro myTMP;

    public override void Init()
    {
        base.Init();
        Debug.Log("Init was run");

        gO = new GameObject();
        gO.transform.position = new Vector3(0, 0, 20);
        myTMP = gO.AddComponent<TMPro.TextMeshPro>();
        myTMP.SetText("Garblk" + (int)UnityEngine.Random.Range(10, 101209));
    }

    private float timer;


    public override void TaskUpdate()
    {
        base.TaskUpdate();
        Debug.Log("TaskUpdate is running");

        timer += Time.unscaledDeltaTime;
        myTMP.color = new Color(1, 0, 0, 1 - timer);

        if (timer > 1)
            SetStatus(TaskStatus.Success);
    }

    public override void OnSuccess()
    {
        base.OnSuccess();
        Debug.Log("Success!!");
        GameObject.Destroy(gO);
    }
}

#region Menu Tasks
//-------------------------------------------------
// Menu Tasks
//-------------------------------------------------

public class Task_MenuTasks: Task
{
    public Deeper_MenuItem context;
    public TMPro.TextMeshPro myTMP;

    public float scaleSpeed = 120;
    public float sizeIdle = 10;
    public float sizeHighlight = 11;

    public Color colorHighlight = new Color(1, 0, 0, 1);
    public Color colorVis = new Color(1, 1, 1, 1);
    public Color colorInvis = new Color(1, 1, 1, 0);
    public float colorNormalizedTime = .15f;
}

public class Task_MenuAnimation_Highlight : Task_MenuTasks
{
    public Task_MenuAnimation_Highlight (Deeper_MenuItem c)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = TaskCanBeInterrupted.Yes;
        DoesInterrupt = TaskDoesInterrupt.No;
    }

    public Task_MenuAnimation_Highlight(Deeper_MenuItem c, TaskCanBeInterrupted cI, TaskDoesInterrupt dI)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = cI;
        DoesInterrupt = dI;
    }

    public override void Init()
    {
        myTMP.fontSize = sizeIdle;
    }
    
    public override void TaskUpdate()
    {
        base.TaskUpdate();
        //Debug.Log("Task_MenuAnimation_Highlight is running");
        myTMP.fontSize += Time.unscaledDeltaTime * scaleSpeed;
        myTMP.color = Color.Lerp(myTMP.color, colorHighlight, .1f);
        if (myTMP.fontSize >= sizeHighlight)
            SetStatus(TaskStatus.Success);
    }

    public override void OnFail()
    {
        base.OnFail();
        myTMP.fontSize = sizeIdle;
        myTMP.color = colorVis;
    }

    public override void OnAbort()
    {
        OnFail();
    }

    public override void OnSuccess()
    {
        base.OnSuccess();
        myTMP.fontSize = sizeHighlight;
        myTMP.color = colorHighlight;
    }
}

public class Task_MenuAnimation_Unhighlight : Task_MenuTasks
{
    public Task_MenuAnimation_Unhighlight(Deeper_MenuItem c)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = TaskCanBeInterrupted.Yes;
        DoesInterrupt = TaskDoesInterrupt.No;
    }

    public Task_MenuAnimation_Unhighlight(Deeper_MenuItem c, TaskCanBeInterrupted cI, TaskDoesInterrupt dI)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = cI;
        DoesInterrupt = dI;
    }

    public override void Init()
    {
        myTMP.fontSize = sizeHighlight;
    }

    public override void TaskUpdate()
    {
        base.TaskUpdate();
        //Debug.Log("Task_MenuAnimation_Unhighlight is running");
        myTMP.fontSize -= Time.unscaledDeltaTime * scaleSpeed;
        myTMP.color = Color.Lerp(myTMP.color, colorVis, .1f);
        if (myTMP.fontSize <= sizeIdle)
            SetStatus(TaskStatus.Success);
    }

    public override void OnFail()
    {
        base.OnFail();
        myTMP.fontSize = sizeHighlight;
        myTMP.color = colorHighlight;
    }

    public override void OnAbort()
    {
        OnFail();
    }

    public override void OnSuccess()
    {
        base.OnSuccess();
        myTMP.fontSize = sizeIdle;
        myTMP.color = colorVis;
    }
}

public class Task_MenuAnimation_Visible : Task_MenuTasks
{
    public Task_MenuAnimation_Visible(Deeper_MenuItem c)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = TaskCanBeInterrupted.Yes;
        DoesInterrupt = TaskDoesInterrupt.No;
    }

    public Task_MenuAnimation_Visible(Deeper_MenuItem c, TaskCanBeInterrupted cI, TaskDoesInterrupt dI)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = cI;
        DoesInterrupt = dI;
    }

    private float timer;

    public override void Init()
    {
        timer = 0;
    }

    public override void TaskUpdate()
    {
        base.TaskUpdate();
        //Debug.Log("Task_MenuAnimation_Visible is running");
        timer += Time.unscaledDeltaTime;
        myTMP.color = Color.Lerp(myTMP.color, new Color (myTMP.color.r, myTMP.color.g, myTMP.color.b, 1), timer / colorNormalizedTime);
        if (timer / colorNormalizedTime >= 1)
            SetStatus(TaskStatus.Success);
    }

    public override void OnFail()
    {
        base.OnFail();
        myTMP.color = new Color(myTMP.color.r, myTMP.color.g, myTMP.color.b, 0);
    }

    public override void OnAbort()
    {
        OnFail();
    }

    public override void OnSuccess()
    {
        base.OnSuccess();
        myTMP.color = new Color(myTMP.color.r, myTMP.color.g, myTMP.color.b, 1);
    }
}

public class Task_MenuAnimation_Invisible : Task_MenuTasks
{
    public Task_MenuAnimation_Invisible(Deeper_MenuItem c)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = TaskCanBeInterrupted.Yes;
        DoesInterrupt = TaskDoesInterrupt.No;
    }

    public Task_MenuAnimation_Invisible(Deeper_MenuItem c, TaskCanBeInterrupted cI, TaskDoesInterrupt dI)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = cI;
        DoesInterrupt = dI;
    }

    private float timer;

    public override void Init()
    {
        timer = 0;
    }

    public override void TaskUpdate()
    {
        base.TaskUpdate();
        //Debug.Log("Task_MenuAnimation_Visible is running");
        timer += Time.unscaledDeltaTime;
        myTMP.color = Color.Lerp(myTMP.color, new Color(myTMP.color.r, myTMP.color.g, myTMP.color.b, 0), timer / colorNormalizedTime);
        if (timer / colorNormalizedTime >= 1)
            SetStatus(TaskStatus.Success);
    }

    public override void OnFail()
    {
        base.OnFail();
        myTMP.color = new Color(myTMP.color.r, myTMP.color.g, myTMP.color.b, 1);
    }

    public override void OnAbort()
    {
        OnFail();
    }

    public override void OnSuccess()
    {
        base.OnSuccess();
        myTMP.color = new Color(myTMP.color.r, myTMP.color.g, myTMP.color.b, 0);
    }
}

#endregion

#region Interaction Tasks
//-------------------------------------------------
// Interaction Tasks
//-------------------------------------------------

public enum InteractionTaskReturn { Success, Fail, Abort }
public enum InteractionTaskIcons { Doc, Ops, Either }

public class Task_Interaction : Task
{
    public Controlled_Character i;

    public GameObject progressRing;
    public float tNorm;
    public RectTransform rt;
    public Image img;

    public GameObject iconBorder;
    public GameObject iconSymbol;
    public RectTransform rtIconBorder;
    public RectTransform rtIconSymbol;
    public Image imgIconBorder;
    public Image imgIconSymbol;

    public void CreateRing()
    {
        progressRing = new GameObject();

        progressRing.AddComponent<Canvas>();
        progressRing.AddComponent<CanvasRenderer>();
        progressRing.AddComponent<Image>();

        rt = progressRing.GetComponent<RectTransform>();
        img = progressRing.GetComponent<Image>();

        Sprite s = Resources.Load("UI/Ring", typeof(Sprite)) as Sprite;
        Debug.Assert(s != null, "Ring cannot be found");
        img.sprite = s;
        img.raycastTarget = false;
        img.type = Image.Type.Filled;
        img.fillMethod = Image.FillMethod.Radial360;
        img.fillClockwise = true;

        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 2);
        rt.localScale = Vector3.one;
        rt.rotation = Quaternion.identity;
    }

    //public void CreateIcon(InteractionTaskIcons i)
    //{
    //    iconBorder = new GameObject();
    //    iconBorder.name = "IconBorder";

    //    iconBorder.AddComponent<Canvas>();
    //    iconBorder.AddComponent<CanvasRenderer>();
    //    iconBorder.AddComponent<Image>();

    //    rtIconBorder = iconBorder.GetComponent<RectTransform>();
    //    imgIconBorder = iconBorder.GetComponent<Image>();

    //    Sprite s = Resources.Load("UI/II_Border", typeof(Sprite)) as Sprite;
    //    imgIconBorder.sprite = s;

    //    rtIconBorder.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2);
    //    rtIconBorder.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 2);
    //    rtIconBorder.localScale = Vector3.one;
    //    rtIconBorder.rotation = Quaternion.identity;


    //    iconSymbol = new GameObject();
    //    iconSymbol.name = "IconSymbol";

    //    iconSymbol.AddComponent<Canvas>();
    //    iconSymbol.AddComponent<CanvasRenderer>();
    //    iconSymbol.AddComponent<Image>();

    //    rtIconSymbol = iconSymbol.GetComponent<RectTransform>();
    //    imgIconSymbol = iconSymbol.GetComponent<Image>();

    //    Sprite sy;
    //    if (i == InteractionTaskIcons.Doc)
    //        sy = Resources.Load("UI/II_Doc", typeof(Sprite)) as Sprite;
    //    else if (i == InteractionTaskIcons.Ops)
    //        sy = Resources.Load("UI/II_Ops", typeof(Sprite)) as Sprite;
    //    else
    //        sy = Resources.Load("UI/II_Either", typeof(Sprite)) as Sprite;

    //    imgIconSymbol.sprite = sy;
    //    rtIconSymbol.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2);
    //    rtIconSymbol.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 2);
    //    rtIconSymbol.localScale = Vector3.one;
    //    rtIconSymbol.rotation = Quaternion.identity;

    //    iconSymbol.transform.SetParent(iconBorder.transform);
    //    rtIconSymbol.localPosition = Vector3.forward * -.05f;
    //}

    public override void CleanUp()
    {
        if (progressRing != null)
            GameObject.Destroy(progressRing);
        if (iconBorder != null)
            GameObject.Destroy(iconBorder);
    }
}

public class Task_Interaction_InProgress_Base : Task_Interaction
{
    public Task_Interaction_InProgress_Base (Controlled_Character interactor, float interactionTime)
    {
        i = interactor;
        tNorm = interactionTime;
    }

    public float timer;

    public override void Init()
    {
        timer = 0;
        CreateRing();
    }

    public override void TaskDeeperNormUpdate()
    {
        timer += Time.deltaTime;
        rt.position = i.transform.position + (-5 * Vector3.forward);
        img.fillAmount = Mathf.Clamp01(timer / tNorm);
    }

    public override void OnAbort()
    {
        Deeper_ServicesLocator.instance.TaskManager.AddTask(new Task_Interaction_Fail_Base(rt.position, img.fillAmount));
    }

    public override void OnSuccess()
    {
        Deeper_ServicesLocator.instance.TaskManager.AddTask(new Task_Interaction_Success_Base(rt.position));
    }

    public override void CleanUp()
    {
        base.CleanUp();
    }
}

public class Task_Interaction_InProgress_WCB : Task_Interaction_InProgress_Base
{
    private Game_Logic.InteractionCompletionCallback cb;
    private Interactable_Base _interactee;

    public Task_Interaction_InProgress_WCB (Game_Logic.InteractionCompletionCallback callback, Interactable_Base interactee, Controlled_Character interactor, float interactionTime) : base(interactor, interactionTime)
    {
        cb = callback;
        _interactee = interactee;
    }

    public override void TaskDeeperNormUpdate()
    {
        timer += Time.deltaTime;
        rt.position = _interactee.transform.position + (-5.2f * Vector3.forward);
        img.fillAmount = Mathf.Clamp01(timer / tNorm);

        if (timer > tNorm)
        {
            SetStatus(TaskStatus.Success);
        }
    }

    public override void OnAbort()
    {
        base.OnAbort();
        cb(false);
        if (_interactee.GetComponent<IInteractable>() != null)
        {
            _interactee.GetComponent<IInteractable>().OnInteractedFail();
        }
    }

    public override void OnSuccess()
    {
        Debug.Log("Sucessful Interaction");
        base.OnSuccess();
        cb(true);
        _interactee.state = InteractableState.Interacted;
        if (_interactee.GetComponent<IInteractable>() != null)
        {
            _interactee.GetComponent<IInteractable>().OnInteractedSuccess();
        }
    }
}

public class Task_Interaction_InProgress_Linked : Task_Interaction_InProgress_Base
{
    private Game_Logic.InteractionCompletionCallback cb;
    private Interactable_Linked _interactee;
    private Interactable_Linked _comparator;

    public Task_Interaction_InProgress_Linked(Game_Logic.InteractionCompletionCallback callback, Interactable_Linked interactee, Interactable_Linked linkComparator, Controlled_Character interactor, float interactionTime) : base(interactor, interactionTime)
    {
        cb = callback;
        _interactee = interactee;
        _comparator = linkComparator;
    }

    public override void Init()
    {
        base.Init();
        Deeper_EventManager.instance.Register<Deeper_Event_LinkedInteractionInitiated>(InteractionTaskEventHandler);
        Deeper_EventManager.instance.Fire(new Deeper_Event_LinkedInteractionInitiated(_interactee));
    }

    public void InteractionTaskEventHandler (Deeper_Event e)
    {
        Deeper_Event_LinkedInteractionInitiated hold = e as Deeper_Event_LinkedInteractionInitiated;
        if (hold != null)
        {
            if (hold.callingInteractable == _comparator)
            {
                SetStatus(TaskStatus.Success);
                Deeper_EventManager.instance.Fire(new Deeper_Event_LinkedInteractionInitiated(_interactee));
            }
        }
    }

    public override void TaskDeeperNormUpdate()
    {
        timer += Time.deltaTime;
        rt.position = _interactee.transform.position + (-5.2f * Vector3.forward);
        img.fillAmount = Mathf.Clamp01(timer / .25f);

        if (timer > .25f)
        {
            timer = 0;
        }
    }



    public override void OnAbort()
    {
        base.OnAbort();
        cb(false);
        if (_interactee.GetComponent<IInteractable>() != null)
        {
            _interactee.GetComponent<IInteractable>().OnInteractedFail();
        }
    }

    public override void OnSuccess()
    {
        Debug.Log("Sucessful Interaction");
        base.OnSuccess();

        Deeper_ServicesLocator.instance.TaskManager.AddTask(new Task_Interaction_InProgress_WCB(cb, _interactee, i, tNorm));
        //cb(true);
        //_interactee.state = InteractableState.Interacted;
        //if (_interactee.GetComponent<IInteractable>() != null)
        //{
        //    _interactee.GetComponent<IInteractable>().OnInteractedSuccess();
        //}
    }

    public override void CleanUp()
    {
        base.CleanUp();
        Deeper_EventManager.instance.Unregister<Deeper_Event_LinkedInteractionInitiated>(InteractionTaskEventHandler);
    }
}

public class Task_Interaction_Success_Base : Task_Interaction
{
    private Vector3 sL;

    private float timer;

    private float normTime;

    public Task_Interaction_Success_Base(Vector3 staticLocation)
    {
        sL = staticLocation;
    }

    public override void Init()
    {
        timer = 0;
        CreateRing();
        img.fillAmount = 1;
        img.color = Color.green;
        rt.position = sL;
        normTime = 1.2f;

    }

    public override void TaskDeeperNormUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= normTime)
            this.SetStatus(TaskStatus.Success);

        float v = ((1 - ((1 - timer) * (1 - timer) * (1 - timer) * (1 - timer) * (1 - timer) * (1 - timer))) / normTime);
        //Debug.Log(v);
        img.color = Color.Lerp(img.color, new Color(0, 1, 0, 0), .08f);
        rt.localScale = Vector3.one * (1 + v);
    }
    public override void CleanUp()
    {
        base.CleanUp();
    }
}

public class Task_Interaction_Success_DialogueTimerComplete : Task_Interaction
{
    private Vector3 sL;

    private float timer;

    private float normTime;

    public Task_Interaction_Success_DialogueTimerComplete(Vector3 staticLocation)
    {
        sL = staticLocation;
    }

    public override void Init()
    {
        timer = 0;
        CreateRing();
        img.fillAmount = 1;
        img.color = Color.green;
        rt.position = sL;
        normTime = 1.2f;
        progressRing.layer = LayerMask.NameToLayer("UI");
    }

    public override void TaskDeeperNormUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= normTime)
            this.SetStatus(TaskStatus.Success);

        float v = ((1 - ((1 - timer) * (1 - timer) * (1 - timer) * (1 - timer) * (1 - timer) * (1 - timer))) / normTime);
        //Debug.Log(v);
        img.color = Color.Lerp(img.color, new Color(0, 1, 0, 0), .08f);
        rt.localScale = Vector3.one * (1 + v);
    }
    public override void CleanUp()
    {
        base.CleanUp();
    }
}

public class Task_Interaction_Fail_Base : Task_Interaction
{
    private Vector3 sL;
    private float cF;

    private float timer;

    private float normTime;

    public Task_Interaction_Fail_Base(Vector3 staticLocation, float cancelFloat)
    {
        sL = staticLocation;
        cF = cancelFloat;
    }

    public override void Init()
    {
        timer = 0;
        CreateRing();
        img.fillAmount = cF;
        img.color = Color.red;
        rt.position = sL;
        normTime = 1.2f;
    }

    public override void TaskDeeperNormUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= normTime)
            this.SetStatus(TaskStatus.Success);

        float v = ((1 - ((1 - timer) * (1 - timer) * (1 - timer) * (1 - timer) * (1 - timer) * (1 - timer))) / normTime);
        //Debug.Log(v);
        img.color = Color.Lerp(img.color, new Color(1, 0, 0, 0), .08f);
        rt.localScale = Vector3.one * (1 + v);
    }

    public override void CleanUp()
    {
        base.CleanUp();
    }
}

public class Task_Interaction_Fail_DialogueReset : Task_Interaction
{
    private Vector3 sL;
    private float cF;

    private float timer;

    private float normTime;

    private Color altColor;

    public Task_Interaction_Fail_DialogueReset(Vector3 staticLocation, float cancelFloat, Color desiredColor)
    {
        sL = staticLocation;
        cF = cancelFloat;
        altColor = desiredColor;
    }

    public override void Init()
    {
        timer = 0;
        CreateRing();
        img.fillAmount = cF;
        img.color = altColor;
        rt.position = sL;
        normTime = 1.2f;
        progressRing.layer = LayerMask.NameToLayer("UI");
    }

    public override void TaskDeeperNormUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= normTime)
            this.SetStatus(TaskStatus.Success);

        float v = ((1 - ((1 - timer) * (1 - timer) * (1 - timer) * (1 - timer) * (1 - timer) * (1 - timer))) / normTime);
        //Debug.Log(v);
        img.color = Color.Lerp(img.color, new Color(1, 0, 0, 0), .08f);
        rt.localScale = Vector3.one * (1 + v);
    }

    public override void CleanUp()
    {
        base.CleanUp();
    }
}

public class Task_Interaction_DialogueChoice : Task_Interaction
{
    private Vector3 where;
    private Game_DialogueManager.PrintChoice.InteractionInitCallback myCB;
    private float progressT;
    private float tuneScale;

    public Task_Interaction_DialogueChoice (Vector3 location, Game_DialogueManager.PrintChoice.InteractionInitCallback callback, float maxTime, float progressTime, float scaleOffset)
    {
        where = location;
        myCB = callback;
        tNorm = maxTime;
        timer = progressTime;
        tuneScale = scaleOffset;
    }

    private float timer;

    public override void Init()
    {
        CreateRing();
        progressRing.layer = LayerMask.NameToLayer("UI");
    }

    public override void TaskDeeperNormUpdate()
    {
        timer += Time.deltaTime;
        rt.position = where;
        img.fillAmount = Mathf.Clamp01(timer / tNorm);

        if (timer / tNorm >= 1)
        {
            SetStatus(TaskStatus.Success);
        }
    }

    public override void OnAbort()
    {
        Deeper_ServicesLocator.instance.TaskManager.AddTask(new Task_Interaction_Fail_DialogueReset(where, img.fillAmount, Color.green));
    }

    public override void OnSuccess()
    {
        Deeper_ServicesLocator.instance.TaskManager.AddTask(new Task_Interaction_Success_DialogueTimerComplete(where));
        myCB();
    }

    public override void CleanUp()
    {
        base.CleanUp();
    }
}

//public class Task_Interactable_FadeIn : Task_Interaction
//{
//    private InteractionTaskIcons who;
//    private Vector3 loc;

//    private Color borderFinalColor;
//    private Color symbolFinalColor;

//    private Color borderInvisColor;
//    private Color symbolInvisColor;

//    private float prox;
//    private bool done;

//    public Task_Interactable_FadeIn(InteractionTaskIcons whoCanInteract, Vector3 where)
//    {
//        who = whoCanInteract;
//        loc = where;

//        if (whoCanInteract == InteractionTaskIcons.Doc)
//        {
//            borderInvisColor = borderFinalColor = new Color(0, 0, 1, 1);
//            borderInvisColor.a = 0;

//            symbolInvisColor = symbolFinalColor = new Color(1, 1, 1, 1);
//            symbolInvisColor.a = 0;
//        }
//        else if (whoCanInteract == InteractionTaskIcons.Ops)
//        {
//            borderInvisColor = borderFinalColor = new Color(1, 0, 0, 1);
//            borderInvisColor.a = 0;

//            symbolInvisColor = symbolFinalColor = new Color(1, 1, 1, 1);
//            symbolInvisColor.a = 0;
//        }
//        else
//        {
//            borderInvisColor = borderFinalColor = new Color(.4f, 0, 1, 1);
//            borderInvisColor.a = 0;

//            symbolInvisColor = symbolFinalColor = new Color(1, 1, 1, 1);
//            symbolInvisColor.a = 0;
//        }
//    }

//    public override void Init()
//    {
//        CreateIcon(who);
//        rtIconBorder.position = loc + Vector3.forward * -5;
//        imgIconBorder.color = borderInvisColor;
//        imgIconSymbol.color = symbolInvisColor;
//        prox = 1;
//    }

//    public override void TaskDeeperNormUpdate()
//    {
//        if (done)
//            return;

//        imgIconBorder.color = Color.Lerp(imgIconBorder.color, borderFinalColor, .1f);
//        imgIconSymbol.color = Color.Lerp(imgIconSymbol.color, symbolFinalColor, .1f);
//        prox = Mathf.Lerp(prox, 0, .1f);

//        if (prox <= .005f)
//        {
//            done = true;
//            imgIconBorder.color = borderFinalColor;
//            imgIconSymbol.color = symbolFinalColor;
//        }
//    }

//    public override void CleanUp()
//    {
//        base.CleanUp();
//    }
//}

//public class Task_Interactable_FadeOut : Task_Interaction
//{
//    private InteractionTaskIcons who;
//    private Vector3 loc;

//    private Color borderFinalColor;
//    private Color symbolFinalColor;

//    private Color borderInvisColor;
//    private Color symbolInvisColor;

//    private float prox;
//    private bool done;

//    public Task_Interactable_FadeOut(InteractionTaskIcons whoCanInteract, Vector3 where)
//    {
//        who = whoCanInteract;
//        loc = where;

//        if (whoCanInteract == InteractionTaskIcons.Doc)
//        {
//            borderInvisColor = borderFinalColor = new Color(0, 0, 1, 1);
//            borderInvisColor.a = 0;

//            symbolInvisColor = symbolFinalColor = new Color(1, 1, 1, 1);
//            symbolInvisColor.a = 0;
//        }
//        else if (whoCanInteract == InteractionTaskIcons.Ops)
//        {
//            borderInvisColor = borderFinalColor = new Color(1, 0, 0, 1);
//            borderInvisColor.a = 0;

//            symbolInvisColor = symbolFinalColor = new Color(1, 1, 1, 1);
//            symbolInvisColor.a = 0;
//        }
//        else
//        {
//            borderInvisColor = borderFinalColor = new Color(.4f, 0, 1, 1);
//            borderInvisColor.a = 0;

//            symbolInvisColor = symbolFinalColor = new Color(1, 1, 1, 1);
//            symbolInvisColor.a = 0;
//        }
//    }

//    public override void Init()
//    {
//        CreateIcon(who);
//        rtIconBorder.position = loc + Vector3.forward * -5;
//        imgIconBorder.color = borderFinalColor;
//        imgIconSymbol.color = symbolFinalColor;
//        prox = 1;
//    }

//    public override void TaskDeeperNormUpdate()
//    {
//        if (done)
//            return;

//        imgIconBorder.color = Color.Lerp(imgIconBorder.color, borderInvisColor, .1f);
//        imgIconSymbol.color = Color.Lerp(imgIconSymbol.color, symbolInvisColor, .1f);
//        prox = Mathf.Lerp(prox, 0, .1f);

//        if (prox <= .005f)
//        {
//            done = true;
//            imgIconBorder.color = borderInvisColor;
//            imgIconSymbol.color = symbolInvisColor;
//            Abort();
//        }
//    }

//    public override void CleanUp()
//    {
//        base.CleanUp();
//    }
//}

#endregion

#region Death Tasks

public class DeathTask_Drown : Task
{
    public override void Init()
    {
        Deeper_EventManager.instance.Fire(new Deeper_Event_Death(DeathTypes.Drown));
        timer = 0;
        Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Interrupted));
    }

    private float timer;

    public override void TaskDeeperNormUpdate()
    {
        timer += Time.deltaTime;

        if (timer > 3)
        {
            Deeper_EventManager.instance.Fire(new Deeper_Event_LevelLoad(0));
        }
    }
}

#endregion

#region SmokeMonster Task

public class Task_SmokeMonster : Task
{
    Material onMat;
    Material offMat;
    GameObject monster;

    GameObject playerOfConcern;

    GameObject[] l;
    GameObject[] h;

    bool lightOnOff;
    bool smokeOnOff;

    public override void Init()
    {
        onMat = (Material) Resources.Load("HallwayMaterials/Glow");
        offMat = (Material)Resources.Load("HallwayMaterials/NoGlow");
        monster = GameObject.Find("SmokeMonster");

        if (GameObject.Find("Ops").transform.position.x > GameObject.Find("Doc").transform.position.x)
            playerOfConcern = GameObject.Find("Doc");
        else
            playerOfConcern = GameObject.Find("Ops");

        Debug.Assert(onMat != null, "onMat is missing");
        Debug.Assert(offMat != null, "offMat is missing");
        Debug.Assert(monster != null, "monster is missing");

        l = GameObject.FindGameObjectsWithTag("HallLight");
        h = GameObject.FindGameObjectsWithTag("Hall");

        monster.transform.position = new Vector3(-38, -59, 66.2f);

        if (playerOfConcern.gameObject.name == "Doc")
            Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_OOC));
        else
            Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_OOC));

        rollOver = 1;
    }

    private float timer;
    private float rollOver;
    private int flickCounter;

    private float flyAwayTimer;
    bool flyAway;

    bool appliedNarcYet;

    public override void TaskDeeperNormUpdate()
    {
        if (flyAway)
        {
            flyAwayTimer += Time.deltaTime;

            if (flyAwayTimer >= 1.3f)
            {
                if (!appliedNarcYet)
                {
                    appliedNarcYet = true;
                    if (playerOfConcern.gameObject.name == "Doc")
                        Deeper_EventManager.instance.Fire(new Deeper_Event_Narc(CharactersEnum.Doc));
                    else
                        Deeper_EventManager.instance.Fire(new Deeper_Event_Narc(CharactersEnum.Ops));
                }

                monster.transform.position += new Vector3(10, 10, 10) * Time.deltaTime;
                if (Vector3.Distance(monster.transform.position, playerOfConcern.transform.position) > 100)
                    this.SetStatus(TaskStatus.Success);
                return;
            }
        }


        if (flickCounter >= 15)
        {
            monster.SetActive(true);
            flickCounter++;
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, playerOfConcern.transform.position, 30 * Time.deltaTime);

            if (Vector3.Distance(monster.transform.position, playerOfConcern.transform.position) < .01f)
                flyAway = true;

            return;
        }


        timer += Time.deltaTime;
        if (timer > rollOver)
        {
            timer -= rollOver;
            rollOver = Random.Range(0.1f, .6f);

            lightOnOff = !lightOnOff;

            if (lightOnOff)
            {
                flickCounter++;

                int i = Random.Range(0, 4);
                if (i != 0)
                    smokeOnOff = true;
                else
                    smokeOnOff = false;
                monster.SetActive(smokeOnOff);
            }
            else
            {
                monster.SetActive(false);
            }

            monster.transform.position += Vector3.forward * -.25f;


            foreach (GameObject x in l)
            {
                x.SetActive(lightOnOff);
            }

            foreach (GameObject x in h)
            {
                if (lightOnOff)
                    x.GetComponent<MeshRenderer>().material = onMat;
                else
                    x.GetComponent<MeshRenderer>().material = offMat;
            }
        }
    }

    public override void CleanUp()
    {
        if (playerOfConcern.gameObject.name == "Ops")
            Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_RTS));
        else
            Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_RTS));
    }
}

#endregion