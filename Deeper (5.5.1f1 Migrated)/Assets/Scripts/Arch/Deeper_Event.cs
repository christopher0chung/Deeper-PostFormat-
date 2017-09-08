using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Deeper_Event
{
    public delegate void Handler(Deeper_Event e);
}

//--------------------------------------------
// Control Event Messages
//--------------------------------------------

public enum ControllersEnum { C0, C1 }
public enum CharactersEnum { Doc, Ops, DANI }
public class Deeper_Event_ControlAssignment: Deeper_Event
{
    public ControllersEnum controller;
    public CharactersEnum character;

    public Deeper_Event_ControlAssignment (ControllersEnum co, CharactersEnum ch)
    {
        controller = co;
        character = ch;
    }
}

public class Deeper_Event_ControlScheme: Deeper_Event
{
    public ControlStates cs;
    public Deeper_Event_ControlScheme (ControlStates state)
    {
        cs = state;
    }
}

//--------------------------------------------
// Level Event Messages
//--------------------------------------------

public enum Levels { Splash, OpeningCutscene }

public class Deeper_Event_LevelUnload : Deeper_Event { }

public class Deeper_Event_LevelLoad : Deeper_Event
{
    public Levels level;
    public Deeper_Event_LevelLoad(Levels l)
    {
        level = l;
    }
}

//--------------------------------------------
// Pause
//--------------------------------------------

public class Deeper_Event_Pause : Deeper_Event
{
    public bool isPaused;
    public Deeper_Event_Pause(bool truePausedFalseUnpaused)
    {
        isPaused = truePausedFalseUnpaused;
    }
}

//--------------------------------------------
// Camera
//--------------------------------------------

public class Deeper_Event_CamSwitch : Deeper_Event { }

public class Deeper_Event_CamSingleSplit : Deeper_Event
{
    public bool c;

    public Deeper_Event_CamSingleSplit (bool singleFalseSplitTrue)
    {
        c = singleFalseSplitTrue;
    }
}

//--------------------------------------------
// Interaction
//--------------------------------------------

public class Deeper_Event_LinkedInteractionInitiated : Deeper_Event
{
    public Interactable_Linked callingInteractable;

    public Deeper_Event_LinkedInteractionInitiated (Interactable_Linked callingLinkedInteractable)
    {
        callingInteractable = callingLinkedInteractable;
    }
}

//--------------------------------------------
// Death
//--------------------------------------------

public enum DeathTypes { Drown }

public class Deeper_Event_Death : Deeper_Event
{
    public DeathTypes dT;

    public Deeper_Event_Death (DeathTypes typeOfDeath)
    {
        dT = typeOfDeath;
    }
}

//--------------------------------------------
// Ship Handling and Status
//--------------------------------------------

public class Deeper_Event_SubTurning : Deeper_Event
{
    public bool isTurning;

    public Deeper_Event_SubTurning (bool isTheSubTurning)
    {
        isTurning = isTheSubTurning;
    }
}

//--------------------------------------------
// Narc
//--------------------------------------------

public class Deeper_Event_Narc : Deeper_Event
{
    public CharactersEnum whoIsNarced;

    public Deeper_Event_Narc(CharactersEnum who)
    {
        whoIsNarced = who;
    }
}

//--------------------------------------------
// Navigation
//--------------------------------------------

public class Deeper_Event_Nav : Deeper_Event
{
    public bool navActive;
    public Vector3 toWhere;

    public Deeper_Event_Nav (bool startUpTShutDownF, Vector3 location)
    {
        navActive = startUpTShutDownF;
        toWhere = location;
    }
}

public class Deeper_Event_NavBeaconPing : Deeper_Event { }

//--------------------------------------------
// UI
//--------------------------------------------

public class Deeper_Event_BattLvl : Deeper_Event
{
    public float level;
    public Deeper_Event_BattLvl (float chargeLevel)
    {
        level = chargeLevel;
    }
}