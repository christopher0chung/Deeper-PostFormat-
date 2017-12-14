using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InteractionEffects_ChangeScene : InteractionEffects_Base
{
    private void Start()
    {
       
    }

    public override void OnInteractedSuccess()
    {
        Deeper_ServicesLocator.instance.SFXManager.PlaySoundPauseable(SFX.Checkpoint);
        Deeper_EventManager.instance.Fire(new Deeper_Event_LevelUnload());
        SceneManager.LoadScene(3);
    }

    public override void OnInteractedFail()
    {
        Debug.Log("Failed to interact with " + this.gameObject.name);
    }
}
