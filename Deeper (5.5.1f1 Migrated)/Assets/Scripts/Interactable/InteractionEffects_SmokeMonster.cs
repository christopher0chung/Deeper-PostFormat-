using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEffects_SmokeMonster : InteractionEffects_Base
{
    private void Start()
    {
    }

    public override void OnInteractedSuccess()
    {
        Deeper_ServicesLocator.instance.TaskManager.AddTask(new Task_SmokeMonster());
    }

    public override void OnInteractedFail()
    {
        Debug.Log("Failed to interact with " + this.gameObject.name);
    }
}
