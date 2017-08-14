using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deeper_RewiredAid {

    private static Deeper_RewiredAid _instance;
    public static Deeper_RewiredAid instance
    {
        get
        {
            if (_instance == null)
                _instance = new Deeper_RewiredAid();
            return _instance;
        }
    }

    public int GetMap (ControlMaps m)
    {
        if (m == ControlMaps.Default)
            return 0;
        else if (m == ControlMaps.Menu)
            return 3;
        else if (m == ControlMaps.Character)
            return 10;
        else
            return 11;
    }
}

public enum ControlMaps { Default, Menu, Character, Sub }
