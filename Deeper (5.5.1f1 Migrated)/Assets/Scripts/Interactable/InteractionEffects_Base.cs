using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEffects_Base : MonoBehaviour, IInteractable {

    public virtual void OnInteractedSuccess() { }

    public virtual void OnInteractedFail() { }
}
