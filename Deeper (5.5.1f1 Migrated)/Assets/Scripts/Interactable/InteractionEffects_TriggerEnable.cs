using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEffects_TriggerEnable : InteractionEffects_Base {

    public Trigger_Base myT;
	void Start () {
        myT.enabled = false;
	}

    public override void OnInteractedSuccess()
    {
        myT.enabled = true;
    }
}
