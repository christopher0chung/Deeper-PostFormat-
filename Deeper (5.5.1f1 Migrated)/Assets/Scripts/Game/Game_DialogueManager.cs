using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

[RequireComponent(typeof(AudioSource))]

public class Game_DialogueManager : Deeper_Component {

    private AudioSource _myAS;
    private AudioClip _activeLineClip;

    private bool singleFalseSplitTrue;

    private FSM<Game_DialogueManager> _fsm;

    public TMPro.TextMeshPro myTranscipt;
    public TMPro.TextMeshPro myP1Choices;
    public TMPro.TextMeshPro myP2Choices;

    public SpriteRenderer[] DPads;
    public SpriteRenderer[] DPads_Up;
    public SpriteRenderer[] DPads_Down;

    #region Deeper_Component
    void Awake()
    {
        Initialize(5000);
        Deeper_EventManager.instance.Register<Deeper_Event_CamSingleSplit>(_CamEvents);
        Deeper_EventManager.instance.Register<Deeper_Event_Pause>(_PauseHandler);
    }

    void Start()
    {
        _GetRefs();
        _fsm = new FSM<Game_DialogueManager>(this);
        _fsm.TransitionTo<Standby>();
        _UIOff();
    }

    public override void PostUpdate()
    {
        _fsm.Update();
    }

    public override void Unsub(Deeper_Event e)
    {
        base.Unsub(e);
        Deeper_EventManager.instance.Unregister<Deeper_Event_CamSingleSplit>(_CamEvents);
        Deeper_EventManager.instance.Unregister<Deeper_Event_Pause>(_PauseHandler);
    }
    #endregion

    #region Internal Functions
    private void _GetRefs()
    {
        _myAS = GetComponent<AudioSource>();
    }

    private void _CamEvents(Deeper_Event e)
    {
        Deeper_Event_CamSingleSplit c = e as Deeper_Event_CamSingleSplit;
        if (c != null)
        {
            //Do something
        }
    }
    private void _PauseHandler(Deeper_Event e)
    {
        Deeper_Event_Pause p = e as Deeper_Event_Pause;
        if (p != null)
        {
            if (p.isPaused)
                _myAS.Pause();
            else
                _myAS.UnPause();
        }
    }

    private void _UIOff()
    {
        foreach (SpriteRenderer r in DPads)
        {
            r.color = new Color(r.color.r, r.color.g, r.color.b, 0);
        }
        foreach (SpriteRenderer r in DPads_Up)
        {
            r.color = new Color(r.color.r, r.color.g, r.color.b, 0);
        }
        foreach (SpriteRenderer r in DPads_Down)
        {
            r.color = new Color(r.color.r, r.color.g, r.color.b, 0);
        }
    }

    private void _BeginDialogue()
    {
        _fsm.TransitionTo<Load>();
    }

    #endregion

    #region External Functions

    private Dialogue_Org_Conversation _c;
    private Dialogue_Org_Chain activeChain;
    private Dialogue activeLine;

    public void RegisterDialogue(Dialogue_Org_Conversation conversation)
    {
        _c = null;

        _c = conversation;

        Debug.Assert(_c.FirstChain != null, "No chain loaded");
        activeChain = _c.FirstChain;

        activeLine = null;

        _BeginDialogue();

        //Debug.Log("Dialogue registered");
    }

    #endregion

    #region States

    public class State_Base : FSM<Game_DialogueManager>.State
    {
        public float timer;
        public float transitionTime;
        public int wordCount;
        public int wordCounter;
        public int letterNumForWordCounter;

        public float loadDelayTime;

        public void _Clear()
        {
            Context.myTranscipt.text = "";
            Context.myP1Choices.text = "";
            Context.myP2Choices.text = "";
            Context._UIOff();
        }

        public void _LoadAndPlayAudio()
        {
            Context._activeLineClip = (AudioClip)Resources.Load("Dialogue/" + Context.activeLine.gameObject.name);
            Context._myAS.Stop();
            Context._myAS.loop = false;
            Context._myAS.clip = Context._activeLineClip;
            Context._myAS.Play();
        }

        public void _ParseAndTransition()
        {
            Dialogue_Line l = Context.activeLine as Dialogue_Line;
            Dialogue_LineTerminal t = Context.activeLine as Dialogue_LineTerminal;
            Dialogue_Choice c = Context.activeLine as Dialogue_Choice;

            if (l != null)
            {
                //Debug.Log("It's a LINE");

                TransitionTo<PrintLine>();
            }
            else if (t != null)
            {
                //Debug.Log("It's a TERMINAL LINE");

                TransitionTo<PrintTerminalLine>();
            }
            else if (c != null)
            {
                //Debug.Log("It's a CHOICE");

                TransitionTo<PrintChoice>();
            }
        }

        public void PullTime()
        {
            if (Context.activeLine == null)
            {
                //loadDelayTime = Context.activeChain.LinesAndChoices[0].delayTime;
                loadDelayTime = 0;
            }

            else
            {
                for (int i = 0; i <= Context.activeChain.LinesAndChoices.Count; i++)
                {
                    if (Context.activeLine != Context.activeChain.LinesAndChoices[i])
                    {
                        continue;
                    }
                    else
                    {
                        if (i + 1 < Context.activeChain.LinesAndChoices.Count)
                        {
                            loadDelayTime = Context.activeChain.LinesAndChoices[i + 1].delayTime;
                            return;
                        }

                        else
                        {
                            return;
                        }

                    }
                }
            }
        }

        public void LoadAndTransition()
        {
            // if this is the first line
            if (Context.activeLine == null)
            {
                //Debug.Log("First Line");
                Context.activeLine = Context.activeChain.LinesAndChoices[0];
                _ParseAndTransition();
            }
            // if this is not the first
            else
            {
                //Debug.Log("Not first line");
                for (int i = 0; i <= Context.activeChain.LinesAndChoices.Count; i++)
                {
                    // search for the next line in the chain
                    if (Context.activeLine != Context.activeChain.LinesAndChoices[i])
                    {
                        //Debug.Log("Haven't found it yet");
                        continue;
                    }
                    else
                    {
                        //Debug.Log("Found the line I was on.");
                        // set the next line to active

                        if (i + 1 < Context.activeChain.LinesAndChoices.Count)
                        {
                            Context.activeLine = Context.activeChain.LinesAndChoices[i + 1];
                            _ParseAndTransition();
                            return;
                        }

                        // if there is no next line
                        else
                        {
                            Context.activeLine = null;
                            TransitionTo<Standby>();
                            return;
                        }

                    }
                }
            }
        }

        public int WordCount(string l)
        {
            int counter = 0;
            for (int i = 0; i < l.Length; i++)
            {
                //Debug.Log(l.Substring(i, 1));
                if (l.Substring(i, 1) ==  " ")
                {
                    //Debug.Log("Found a space");
                    counter++;
                }
            }
            //Debug.Log("Word count = " + counter.ToString());
            return counter;
        }
    }

    private class Standby : State_Base
    {
        public override void OnEnter()
        {
            _Clear();
        }
    }

    private class Load : State_Base
    {
        public override void OnEnter()
        {
            //Debug.Log("In Load");
            _Clear();
            timer = 0;
            //PullTime();
        }

        public override void Update()
        {
            //Debug.Log("Updating Load");
            timer += Time.deltaTime;
            if (timer > .6f + loadDelayTime)
                LoadAndTransition();
        }
    }

    private class PrintLine : State_Base
    {
        Dialogue_Line l;

        public override void OnEnter()
        {
            //Debug.Log("Now in PrintLine");

            Debug.Assert(Context.activeLine as Dialogue_Line, "Dialogue_Line not sent to expected state");
            l = Context.activeLine as Dialogue_Line;

            Context.myTranscipt.text = "<color=#FFFFFFFF>" + l.speaker.ToString() + ": <color=#FFFFFF00>" + l.line;
            wordCount = WordCount(l.line);

            _LoadAndPlayAudio();
            timer = 0;
            transitionTime = Context._activeLineClip.length + .1f;
        }

        int spaceCounter;

        public override void Update()
        {
            timer += Time.deltaTime;
            spaceCounter = 0;

            wordCounter = (int)(wordCount * (timer / (transitionTime - 2)));

            for (int i = 0; i < l.line.Length; i++)
            {
                if (l.line.Substring(i, 1) == " ")
                {
                    //Debug.Log("space counter has registered that it has found a space");
                    spaceCounter++;
                }

                if (spaceCounter == wordCounter)
                {
                    letterNumForWordCounter = i;
                    break;
                }
            }

            Context.myTranscipt.text = "<color=#FFFFFFFF>" + l.speaker.ToString() + ": " + l.line.Substring(0, letterNumForWordCounter) + "<color=#FFFFFF00>" + l.line.Substring(letterNumForWordCounter + 1, l.line.Length - letterNumForWordCounter - 1);

            if (timer >= transitionTime)
            {
                TransitionTo<Load>();
            }
        }
    }

    private class PrintTerminalLine : State_Base
    {
        Dialogue_LineTerminal t;

        public override void OnEnter()
        {

            Debug.Assert(Context.activeLine as Dialogue_LineTerminal, "Dialogue_Line not sent to expected state");
            t = Context.activeLine as Dialogue_LineTerminal;

            Context.myTranscipt.text = "<color=#FFFFFFFF>" + t.speaker.ToString() + ": <color=#FFFFFF00>" + t.line;
            wordCount = WordCount(t.line);

            _LoadAndPlayAudio();
            Debug.Log("Now in PrintTerminalLine");
            timer = 0;
            transitionTime = Context._activeLineClip.length + .1f;
        }

        int spaceCounter;

        public override void Update()
        {
            timer += Time.deltaTime;
            spaceCounter = 0;

            wordCounter = (int)(wordCount * (timer / (transitionTime - 2)));

            for (int i = 0; i < t.line.Length; i++)
            {
                if (t.line.Substring(i, 1) == " ")
                {
                    //Debug.Log("space counter has registered that it has found a space");
                    spaceCounter++;
                }

                if (spaceCounter == wordCounter)
                {
                    letterNumForWordCounter = i;
                    break;
                }
            }

            Context.myTranscipt.text = "<color=#FFFFFFFF>" + t.speaker.ToString() + ": " + t.line.Substring(0, letterNumForWordCounter) + "<color=#FFFFFF00>" + t.line.Substring(letterNumForWordCounter + 1, t.line.Length - letterNumForWordCounter - 1);

            if (timer >= transitionTime)
            {
                Context.activeChain = t.ChainLink;
                Context.activeLine = null;
                TransitionTo<Load>();
            }
        }
    }

    public class PrintChoice : State_Base
    {
        Dialogue_Choice c;
        TMPro.TextMeshPro tMesh;

        private float timerResetMax;

        #region State Functions

        public override void OnEnter()
        {
            //Debug.Log("Now in PrintLine");
            timer = 0;
            transitionTime = 10;

            Debug.Assert(Context.activeLine as Dialogue_Choice, "Dialogue_Choice not sent to expected state");
            c = Context.activeLine as Dialogue_Choice;

            if (c.choice == CharactersEnum.Ops)
            {
                playerInt = 0;
                tMesh = Context.myP1Choices;
                
            }
            else if (c.choice == CharactersEnum.Doc)
            {
                playerInt = 1;
                tMesh = Context.myP2Choices;
            }
            Context.DPads[playerInt].color = new Color(Context.DPads[playerInt].color.r, Context.DPads[playerInt].color.g, Context.DPads[playerInt].color.b, 1);

            _selInt = 0;

            tMesh.text = "<color=#FFFFFFFF><b><u>" + c.choice1 + "</u></b>\n\n" + c.choice2;

            StartDialogueTimer(0);
        }

        int playerInt;

        public override void Update()
        {
            //Debug.Log("This is frame number: " + debugFrameCount);
            //Debug.Log(ReInput.players.GetPlayer(playerInt).GetAxis("Dialogue Select"));
            timer += Time.deltaTime;
            timerResetMax = timer / 4f;

            _DPadInd();
            _DPadSel();
        }

        #endregion

        #region DPad
        private void _DPadInd()
        {
            if (ReInput.players.GetPlayer(playerInt).GetAxis("Dialogue Select") > 0)
            {
                //Context.activeChain = c.ChainLink1;
                //Context.activeLine = null;
                //TransitionTo<Load>();
                Context.DPads_Up[playerInt].color = new Color(Context.DPads_Up[playerInt].color.r, Context.DPads_Up[playerInt].color.g, Context.DPads_Up[playerInt].color.b, 1);
            }
            else if (ReInput.players.GetPlayer(playerInt).GetAxis("Dialogue Select") < 0)
            {
                //Context.activeChain = c.ChainLink2;
                //Context.activeLine = null;
                //TransitionTo<Load>();
                Context.DPads_Down[playerInt].color = new Color(Context.DPads_Down[playerInt].color.r, Context.DPads_Down[playerInt].color.g, Context.DPads_Down[playerInt].color.b, 1);
            }
            else
            {
                Context.DPads_Up[playerInt].color = new Color(Context.DPads_Up[playerInt].color.r, Context.DPads_Up[playerInt].color.g, Context.DPads_Up[playerInt].color.b, 0);
                Context.DPads_Down[playerInt].color = new Color(Context.DPads_Down[playerInt].color.r, Context.DPads_Down[playerInt].color.g, Context.DPads_Down[playerInt].color.b, 0);
            }
        }

        private bool __dPadSelFlag;
        private bool _dPadSelFlag
        {
            get
            {
                return __dPadSelFlag;
            }
            set
            {
                if (__dPadSelFlag != value)
                {
                    __dPadSelFlag = value;
                    //Debug.Log("I am " + value + " on frame number " + debugFrameCount);
                }
            }
        }
        private int _selInt;

        private void _DPadSel()
        {
            if (ReInput.players.GetPlayer(playerInt).GetAxis("Dialogue Select") > 0 && !_dPadSelFlag)
            {
                _dPadSelFlag = true;
                _selInt = 0;
                activeTask.SetStatus(TaskStatus.Aborted);
                StartDialogueTimer(timerResetMax);
            }
            else if (ReInput.players.GetPlayer(playerInt).GetAxis("Dialogue Select") < 0 && !_dPadSelFlag)
            {
                _dPadSelFlag = true;
                _selInt = 1;
                activeTask.SetStatus(TaskStatus.Aborted);
                StartDialogueTimer(timerResetMax);
            }
            else if (ReInput.players.GetPlayer(playerInt).GetAxis("Dialogue Select") == 0)
            {
                _dPadSelFlag = false;
            }

            if (_selInt == 0)
            {
                tMesh.text = "<color=#FFFFFFFF><b><u>" + c.choice1 + "</u></b>\n\n" + c.choice2;
            }
            else
            {
                tMesh.text = "<color=#FFFFFFFF>" + c.choice1 + "\n\n<b><u>" + c.choice2 + "</u></b>";
            }

            if (ReInput.players.GetPlayer(playerInt).GetButtonDown("Dialogue Button"))
            {
                SelectDialogueChoice();
                activeTask.SetStatus(TaskStatus.Success);
            }
        }
        #endregion

        #region Task

        public delegate void InteractionInitCallback();
        public Task_Interaction_DialogueChoice activeTask;
        private Vector3 where;

        private void StartDialogueTimer (float progressTime)
        {
            where = Context.DPads[playerInt].transform.position;

            activeTask = new Task_Interaction_DialogueChoice(where, SelectDialogueChoice, 5, progressTime, 1);

            Deeper_ServicesLocator.instance.TaskManager.AddTask(activeTask);
        }

        #endregion

        public void SelectDialogueChoice ()
        {
            Debug.Log("Time's up!");

            if (_selInt == 0)
                Context.activeChain = c.ChainLink1;
            else
                Context.activeChain = c.ChainLink2;

            Context.activeLine = null;
            TransitionTo<Load>();
        }
    }

    #endregion
}
