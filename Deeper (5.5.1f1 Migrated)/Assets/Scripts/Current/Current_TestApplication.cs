using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current_TestApplication : MonoBehaviour {

    public Current_Test currentSource;
    public Vector3 current;

	void Start () {
		
	}
	
	void FixedUpdate () {
        current = currentSource.GetForce(transform.position);
        GetComponent<Rigidbody>().AddForce(current);
    }
}
