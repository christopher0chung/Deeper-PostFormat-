using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_TailMove : Deeper_Component {

    void Awake()
    {
        Initialize(3000);
    }

    private float timer;
    private float rollover;
    private float xVal;
    private float yVal;

    public override void NormUpdate()
    {
        timer += Time.deltaTime;
        if (timer > rollover)
        {
            timer = 0;
            rollover = Random.Range(.5f, 3);
            xVal = Random.Range(-10f, 10f);
            yVal = Random.Range(-10f, 10f);

            PlayASound();
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(new Vector3(xVal, yVal, 0)), .05f);
    }

    private void PlayASound()
    {
        int which = Random.Range(0, 12);
        if (which == 0)
            Deeper_ServicesLocator.instance.SFXManager.PlaySoundPauseable(SFX.Squeak_Low1, .001f);
        else if (which == 1)
            Deeper_ServicesLocator.instance.SFXManager.PlaySoundPauseable(SFX.Squeak_Low2, .001f);
        else
            Deeper_ServicesLocator.instance.SFXManager.PlaySoundPauseable(SFX.Squeak_Low3, .001f);
    }
}
