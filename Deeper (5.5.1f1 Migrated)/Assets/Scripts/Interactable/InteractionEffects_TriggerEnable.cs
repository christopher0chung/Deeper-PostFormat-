using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEffects_TriggerEnable : InteractionEffects_Base {

    public Trigger_Base myT;

    public bool fireBatteryEvent;
    public float batteryLevelToSet;

    void Start () {
        myT.enabled = false;
	}

    public override void OnInteractedSuccess()
    {
        myT.enabled = true;
        Deeper_ServicesLocator.instance.SFXManager.PlaySoundOneHit(SFX.Checkpoint);

        if (fireBatteryEvent)
            Deeper_EventManager.instance.Fire(new Deeper_Event_BattLvl(batteryLevelToSet));
    }
}
