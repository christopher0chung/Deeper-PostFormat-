using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deeper_Component : MonoBehaviour
{
    //-----------------------------------------------------------------------------
    // 1000 - 
    // 2000 - 
    // 3000 - controls
    // 4000 - 
    // 5000 - player objects
    //-----------------------------------------------------------------------------

    [HideInInspector] public int priority;

    public virtual void Initialize(int p)
    {
        priority = p;
        Deeper_GameUpdateManager.instance.Subscribe(this);
        Deeper_EventManager.instance.Register<Deeper_Event_LevelUnload>(Unsub);
        //Debug.Log(this.gameObject.name);
    }

    public virtual void Unsub (Deeper_Event e)
    {
        _Unsub();
    }

    public virtual void _Unsub()
    {
        Deeper_GameUpdateManager.instance.Unsubscribe(this);
        Deeper_EventManager.instance.Unregister<Deeper_Event_LevelUnload>(Unsub);
    }

    public virtual void EarlyUpdate() { }
    public virtual void NormUpdate() { }
    public virtual void PostUpdate() { }

    public virtual void PhysUpdate() { }

    public virtual void Cleanup()
    {
        Deeper_Component[] d_C = GetComponents<Deeper_Component>();
        foreach (Deeper_Component d in d_C)
        {
            d._Unsub();
        }
        Destroy(this.gameObject);
    }
}
