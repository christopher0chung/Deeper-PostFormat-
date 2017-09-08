using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_CameraLogic : MonoBehaviour {

    private Camera thisCam;

    private void Start()
    {
        thisCam = GetComponent<Camera>();
    }

    public void TurnOn()
    {
        thisCam.enabled = true;
    }

    public void TurnOff()
    {
        thisCam.enabled = false;
    }
}
