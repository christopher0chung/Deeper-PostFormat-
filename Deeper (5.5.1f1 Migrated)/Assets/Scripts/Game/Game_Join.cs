using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;
using UnityEngine.SceneManagement;

public class Game_Join : MonoBehaviour {

    public GameObject cube;

    private bool _p1Connected
    {
        get
        {
            IList<Joystick> joysticks = ReInput.controllers.Joysticks;
            if (joysticks.Count > 0)
                return ReInput.controllers.IsControllerAssigned(joysticks[0].type, joysticks[0].id);
            else
                return false;
        }
    }

    private GameObject _cube1;
    private bool _p1S;
    private bool _p1Show
    {
        get
        {
            return _p1S;
        }
        set
        {
            if (value != _p1S)
            {
                _p1S = value;
                if (_p1S)
                    _cube1 = Instantiate(cube, new Vector3(-5, 0, 0), Quaternion.identity);
                else
                    Destroy(_cube1);
            }
        }
    }

    private bool _p2Connected
    {
        get
        {
            IList<Joystick> joysticks = ReInput.controllers.Joysticks;
            if (joysticks.Count > 1)
                return ReInput.controllers.IsControllerAssigned(joysticks[1].type, joysticks[1].id);
            else
                return false;
        }
    }

    private GameObject _cube2;
    private bool _p2S;
    private bool _p2Show
    {
        get
        {
            return _p2S;
        }
        set
        {
            if (value != _p2S)
            {
                _p2S = value;
                if (_p2S)
                    _cube2 = Instantiate(cube, new Vector3(5, 0, 0), Quaternion.identity);
                else
                    Destroy(_cube2);
            }
        }
    }

    private bool _bothConnected
    {
        get
        {
            if (_p1Connected && _p2Connected)
                return true;
            else
                return false;
        }
    }

    void Update()
    {
        _p1Show = _p1Connected;
        _p2Show = _p2Connected;

        if (_bothConnected)
        {
            Debug.Log("Both connected");
            if (ReInput.players.GetPlayer(0).GetButtonDown("Start") || ReInput.players.GetPlayer(1).GetButtonDown("Start"))
            {
                SceneManager.LoadScene(1);
                Debug.Log("Ready to play");
            }
        }
    }
}
