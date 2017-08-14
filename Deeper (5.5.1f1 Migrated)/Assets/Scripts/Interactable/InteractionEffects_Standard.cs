﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEffects_Standard : InteractionEffects_Base
{
    public Interactable_Base[] interactablesToEnable;
    public Dialogue_Org_Conversation conversationToFire;

    public override void OnInteractedSuccess()
    {
        foreach (Interactable_Base i in interactablesToEnable)
        {
            if (i.state == InteractableState.Standby)
                i.state = InteractableState.Available_Invisible;
        }
        if (conversationToFire != null)
            conversationToFire.Fire();
    }

    public override void OnInteractedFail()
    {
        Debug.Log("Failed to interact with " + this.gameObject.name);
    }
}