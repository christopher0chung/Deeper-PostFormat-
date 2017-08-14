using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env_KelpRandomScale : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float scale = Random.Range(3.00f, 5.00f);
        transform.localScale = Vector3.one * scale;
	}
	
}
