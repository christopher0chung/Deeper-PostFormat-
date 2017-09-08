using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Deeper_SFX_Pausable : MonoBehaviour {

    private AudioSource _myAS;
    private float timer;
    private float trip;

	public void Initialize (AudioClip clip) {
        DontDestroyOnLoad(this.gameObject);
        _myAS = GetComponent<AudioSource>();
        _myAS.clip = clip;
        _myAS.Play();
        Deeper_EventManager.instance.Register<Deeper_Event_Pause>(PauseHandler);
        trip = clip.length;
    }

    public void Initialize(AudioClip clip, float time)
    {
        DontDestroyOnLoad(this.gameObject);
        _myAS = GetComponent<AudioSource>();
        _myAS.clip = clip;
        _myAS.Play();
        Deeper_EventManager.instance.Register<Deeper_Event_Pause>(PauseHandler);
        trip = time;
    }

    private void PauseHandler(Deeper_Event e)
    {
        Deeper_Event_Pause p = e as Deeper_Event_Pause;
        if (p != null)
        {
            if (p.isPaused)
                _myAS.Pause();
            else
                _myAS.UnPause();
        }
    }
	
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= trip + .1f)
        {
            Deeper_EventManager.instance.Unregister<Deeper_Event_Pause>(PauseHandler);
            Destroy(this.gameObject);
        }
    }
}
