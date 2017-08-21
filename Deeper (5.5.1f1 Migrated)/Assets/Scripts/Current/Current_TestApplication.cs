using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current_TestApplication : MonoBehaviour {

    public Vector3 current;

	void Start () {
		
	}
	
	void FixedUpdate () {
        //current = GetComponent<Current_Test>().GetForce(transform.position, GetComponent<Rigidbody>().velocity);
        //GetComponent<Rigidbody>().AddForce(current);
	}
}
