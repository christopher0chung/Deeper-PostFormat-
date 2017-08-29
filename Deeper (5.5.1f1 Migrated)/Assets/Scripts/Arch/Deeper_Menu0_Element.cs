using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MenuElementType { Action, Nav, Info, Label }

public class Deeper_Menu0_Element : MonoBehaviour {

    public MenuElementType myType;
    public MenuItemActions myAction;
    public Deeper_Menu0 myLink;

    public virtual void DoSomething ()
    {
        if (myType == MenuElementType.Nav)
        {
            Debug.Assert(myLink != null, gameObject.name + " is a Link without a link");
            myLink.TurnOn();
        }
        else if (myType == MenuElementType.Action)
        {
            Deeper_MenuAid.instance.GetAction(myAction).Invoke();
        }
    }
}

