using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_ReboundLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private float timer;

	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= 3)
            SceneManager.LoadScene(1);
	}
}
