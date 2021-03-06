﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav_Repeater : Deeper_Component {

    public GameObject pointer;
    public bool navigating;

    public Transform sub;

    private Vector3 whereTo;

    private float ang;

    public SpriteRenderer[] pointerSprites;

    private Color destColor = new Color(176f / 255f, 1, 203f / 255f, .04f);
    //private Color hitColor = new Color(176f / 255f, 1, 203f / 255f, 1);
    private Color hitColor = Color.white;

    void Awake () {
        Initialize(4500);
        pointer.SetActive(false);
        Deeper_EventManager.instance.Register<Deeper_Event_Nav>(NavHandler);
        Deeper_EventManager.instance.Register<Deeper_Event_NavBeaconPing>(NavHandler);
	}
    private void Start()
    {
    }

    public override void NormUpdate()
    {
        if (navigating)
        {
            ang = Mathf.Atan2(whereTo.y - sub.position.y, whereTo.x - sub.position.x) * Mathf.Rad2Deg;
            pointer.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, ang));

            foreach (SpriteRenderer sr in pointerSprites)
            {
                sr.color = Color.Lerp(sr.color, destColor, .04f);
            }
        }
    }

    private void NavHandler (Deeper_Event e)
    {
        Deeper_Event_Nav n = e as Deeper_Event_Nav;
        if (n != null)
        {
            if (n.navActive)
            {
                whereTo = n.toWhere;
                navigating = true;
            }
            else
            {
                whereTo = n.toWhere;
                navigating = false;
            }

            pointer.SetActive(navigating);
        }

        Deeper_Event_NavBeaconPing b = e as Deeper_Event_NavBeaconPing;
        if (b != null)
            foreach (SpriteRenderer sr in pointerSprites)
            {
                sr.color = hitColor;
            }
    }

    public override void _Unsub()
    {
        base._Unsub();
        Deeper_EventManager.instance.Unregister<Deeper_Event_Nav>(NavHandler);
        Deeper_EventManager.instance.Unregister<Deeper_Event_NavBeaconPing>(NavHandler);
    }
}
