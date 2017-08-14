using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_Framerate : MonoBehaviour {

    public Text t;
    private List<float> dT = new List<float>();
    private float total;
    private float f;

	void Update () {

        total = 0;
        dT.Add(Time.deltaTime);
        if (dT.Count > 100)
            dT.RemoveAt(0);

        foreach (float a in dT)
            total += a;

        t.text = "Frames: " + 1 / (total / dT.Count);	
	}
}
