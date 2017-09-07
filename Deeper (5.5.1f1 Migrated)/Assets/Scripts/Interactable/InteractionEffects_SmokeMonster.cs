using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEffects_SmokeMonster : InteractionEffects_Base
{
    public Dialogue_Org_Conversation OpsVersion;
    public Dialogue_Org_Conversation DocVersion;

    private void Start()
    {
    }

    public override void OnInteractedSuccess()
    {
        Deeper_ServicesLocator.instance.TaskManager.AddTask(new Task_SmokeMonster());
        Invoke("FireDialogue", 5);
    }

    public override void OnInteractedFail()
    {
        Debug.Log("Failed to interact with " + this.gameObject.name);
    }

    private void FireDialogue()
    {
        if (GameObject.Find("Ops").transform.position.x > GameObject.Find("Doc").transform.position.x)
            DocVersion.Fire();
        else
            OpsVersion.Fire();
    }
}
