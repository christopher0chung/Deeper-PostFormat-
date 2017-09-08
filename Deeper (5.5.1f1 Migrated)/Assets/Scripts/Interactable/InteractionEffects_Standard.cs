using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEffects_Standard : InteractionEffects_Base
{
    public Interactable_Base[] interactablesToEnable;
    public Dialogue_Org_Conversation conversationToFire;
    public GameObject conversationToDisable;

    public bool fireBatteryEvent;
    public float batteryLevelToSet;

    public GameObject[] ObjectsToEnableAfter;

    private void Start()
    {
        foreach (GameObject g in ObjectsToEnableAfter)
            g.SetActive(false);
    }

    public override void OnInteractedSuccess()
    {
        foreach (Interactable_Base i in interactablesToEnable)
        {
            if (i.state == InteractableState.Standby)
                i.state = InteractableState.Available_Invisible;
        }

        if (fireBatteryEvent)
            Deeper_EventManager.instance.Fire(new Deeper_Event_BattLvl(batteryLevelToSet));

        foreach (GameObject g in ObjectsToEnableAfter)
            g.SetActive(true);

        if (conversationToDisable != null)
            conversationToDisable.SetActive(false);

        if (conversationToFire != null)
            conversationToFire.Fire();
    }

    public override void OnInteractedFail()
    {
        Debug.Log("Failed to interact with " + this.gameObject.name);
    }
}
