using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_ReboundLevel : MonoBehaviour {

    public float reloadTime;

	// Use this for initialization
	void Start () {
		
	}

    private float timer;

	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= reloadTime)
            SceneManager.LoadScene(1);
	}
}
