using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectiveStates { Inactive, Active, Complete }

public class Deeper_Objective
{
    public ObjectiveStates currentState;
    public List<Deeper_Objective> activationCondition;
}

public class Deeper_Objective_Level1_GetToPlace : Deeper_Objective
{
    public Deeper_Objective_Level1_GetToPlace (List<Deeper_Objective> activationCond)
    {
        currentState = ObjectiveStates.Inactive;
        activationCondition = activationCond;
    }
}
