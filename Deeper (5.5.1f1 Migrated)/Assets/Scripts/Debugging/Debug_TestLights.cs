using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_TestLights : MonoBehaviour {

    public bool onOff;

    public GameObject[] l;
    public GameObject[] h;

    public Material onMat;
    public Material offMat;

    void Start()
    {
        l = GameObject.FindGameObjectsWithTag("HallLight");
        h = GameObject.FindGameObjectsWithTag("Hall");
    }


    void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            onOff = !onOff;
            foreach (GameObject x in l)
            {
                x.SetActive(onOff);
            }

            foreach (GameObject x in h)
            {
                if (onOff)
                    x.GetComponent<MeshRenderer>().material = onMat;
                else
                    x.GetComponent<MeshRenderer>().material = offMat;
            }
        }
	}
}
