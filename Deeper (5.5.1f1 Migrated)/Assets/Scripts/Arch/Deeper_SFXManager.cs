using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Deeper_SFXManager : MonoBehaviour {

    private AudioSource _myAS;
    private Dictionary<SFX, AudioClip> sFXToClip = new Dictionary<SFX, AudioClip>();

    private bool awake;

    private void Awake()
    {
        if (!awake)
        {
            awake = true;
            DontDestroyOnLoad(this);
            this.gameObject.name = ("SFX");
            sFXToClip.Add(SFX.Cancel, (AudioClip)Resources.Load("SFX/TEMPO0-N-Click Cancel UN"));
            sFXToClip.Add(SFX.Select, (AudioClip)Resources.Load("SFX/TEMPO0-N-Click Confirm 2 UN"));
            sFXToClip.Add(SFX.Toggle, (AudioClip)Resources.Load("SFX/TEMPO0-N-Click Confirm UN"));
            sFXToClip.Add(SFX.Checkpoint, (AudioClip)Resources.Load("SFX/TEMPO0-N-Checkpoint Chime UN"));
            sFXToClip.Add(SFX.CrashDeath, (AudioClip)Resources.Load("SFX/TEMPO0-N-Sub Death UN"));
            sFXToClip.Add(SFX.Narc, (AudioClip)Resources.Load("SFX/TEMPOA-N-Narced A"));
            sFXToClip.Add(SFX.SmokeMonster_Music, (AudioClip)Resources.Load("SFX/TEMPO0-N-Smoke Monster music UN"));
            sFXToClip.Add(SFX.SmokeMonster_Ring, (AudioClip)Resources.Load("SFX/TEMPO0-N-Smoke Monster ringing UN"));
            sFXToClip.Add(SFX.SmokeMonster_Screech, (AudioClip)Resources.Load("SFX/TEMPO0-N-Smoke Monster screech UN"));

            sFXToClip.Add(SFX.Squeak_Hi1, (AudioClip)Resources.Load("SFX/TEMPO0-N-Metal Squeak hi 1"));
            sFXToClip.Add(SFX.Squeak_Hi2, (AudioClip)Resources.Load("SFX/TEMPO0-N-Metal Squeak hi 2"));
            sFXToClip.Add(SFX.Squeak_Hi3, (AudioClip)Resources.Load("SFX/TEMPO0-N-Metal Squeak hi 3"));
            sFXToClip.Add(SFX.Squeak_Low1, (AudioClip)Resources.Load("SFX/TEMPO0-N-Metal Squeak low 1"));
            sFXToClip.Add(SFX.Squeak_Low2, (AudioClip)Resources.Load("SFX/TEMPO0-N-Metal Squeak low 2"));
            sFXToClip.Add(SFX.Squeak_Low3, (AudioClip)Resources.Load("SFX/TEMPO0-N-Metal Squeak low 3"));
            sFXToClip.Add(SFX.Static, (AudioClip)Resources.Load("SFX/TEMPO0-L-Static 1s"));

            sFXToClip.Add(SFX.Breath_Ops, (AudioClip)Resources.Load("SFX/TEMPO0-N-Breathing 2 low"));
            sFXToClip.Add(SFX.Breath_Doc, (AudioClip)Resources.Load("SFX/TEMPO0-N-Breathing 3 high"));
            sFXToClip.Add(SFX.Breath_Temp, (AudioClip)Resources.Load("SFX/Breath_Temp"));

            sFXToClip.Add(SFX.Alarm, (AudioClip)Resources.Load("SFX/TEMPO0-N-Oxygen Warning slow"));
            sFXToClip.Add(SFX.Ping, (AudioClip)Resources.Load("SFX/TEMPO0-N-Radar Ping"));

            _myAS = GetComponent<AudioSource>();
        }
    }

    public void PlaySoundOneHit(SFX s)
    {
        Awake();
        AudioClip toPlay;
        sFXToClip.TryGetValue(s, out toPlay);
        Debug.Assert(toPlay != null, "SFX Manager - PlaySoundOneHit has a null 'toPlay'");
        _myAS.volume = 1;
        _myAS.PlayOneShot(toPlay);
    }

    public void PlaySoundOneHit(SFX s, float volume)
    {
        Awake();
        AudioClip toPlay;
        sFXToClip.TryGetValue(s, out toPlay);
        Debug.Assert(toPlay != null, "SFX Manager - PlaySoundOneHit has a null 'toPlay'");
        _myAS.volume = volume;
        _myAS.PlayOneShot(toPlay);
    }

    public void PlaySoundPauseable(SFX s)
    {
        Awake();
        AudioClip toPlay;
        sFXToClip.TryGetValue(s, out toPlay);
        Debug.Assert(toPlay != null, "SFX Manager - PlaySoundPauseable has a null 'toPlay'");

        GameObject sfx = new GameObject();
        Deeper_SFX_Pausable p = sfx.AddComponent<Deeper_SFX_Pausable>();
        _myAS.volume = 1;
        p.Initialize(toPlay);
    }

    public void PlaySoundPauseable(SFX s, float volume)
    {
        Awake();
        AudioClip toPlay;
        sFXToClip.TryGetValue(s, out toPlay);
        Debug.Assert(toPlay != null, "SFX Manager - PlaySoundPauseable has a null 'toPlay'");

        GameObject sfx = new GameObject();
        Deeper_SFX_Pausable p = sfx.AddComponent<Deeper_SFX_Pausable>();
        p.GetComponent<AudioSource>().volume = volume;
        p.Initialize(toPlay);
    }

    public void PlaySoundPauseable(SFX s, float volume, float time)
    {
        Awake();
        AudioClip toPlay;
        sFXToClip.TryGetValue(s, out toPlay);
        Debug.Assert(toPlay != null, "SFX Manager - PlaySoundPauseable has a null 'toPlay'");

        GameObject sfx = new GameObject();
        Deeper_SFX_Pausable p = sfx.AddComponent<Deeper_SFX_Pausable>();
        p.GetComponent<AudioSource>().volume = volume;
        p.Initialize(toPlay, time);
    }
}

public enum SFX { Toggle, Select, Cancel, CrashDeath, Narc, SmokeMonster_Music, SmokeMonster_Ring, SmokeMonster_Screech, Checkpoint, Squeak_Hi1, Squeak_Hi2, Squeak_Hi3, Squeak_Low1, Squeak_Low2, Squeak_Low3, Static, Breath_Ops, Breath_Doc, Ping, Alarm, Breath_Temp }