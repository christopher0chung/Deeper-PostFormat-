using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deeper_ObjectiveManager {

    static private Deeper_ObjectiveManager _instance;
    static public Deeper_ObjectiveManager instance
    {
        get
        {
            if (_instance == null)
                return _instance = new Deeper_ObjectiveManager();
            else
                return _instance;
        }
    }

    private List<Deeper_Objective> registeredObjectives;

    public void Register(Deeper_Objective o)
    {
        Debug.Assert(!registeredObjectives.Contains(o), "Registered Objective already exists.");
        registeredObjectives.Add(o);
    }

    //public void UpdateObjective<T>()
}
