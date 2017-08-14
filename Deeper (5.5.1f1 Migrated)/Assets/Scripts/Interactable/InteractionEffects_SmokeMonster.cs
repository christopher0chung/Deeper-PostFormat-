using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEffects_SmokeMonster : InteractionEffects_Base
{
    public GameObject smokeMonster;

    private void Start()
    {
        smokeMonster.SetActive(false);
    }

    public override void OnInteractedSuccess()
    {
        smokeMonster.SetActive(true);
    }

    public override void OnInteractedFail()
    {
        Debug.Log("Failed to interact with " + this.gameObject.name);
    }
}
