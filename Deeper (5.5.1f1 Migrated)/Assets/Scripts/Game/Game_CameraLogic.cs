using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_CameraLogic : MonoBehaviour {

    private Camera thisCam;
    private GameObject thisPS;

    private void Start()
    {
        thisCam = GetComponent<Camera>();
        if (transform.GetChild(0) != null)
            thisPS = transform.GetChild(0).gameObject;
    }

    public void TurnOn()
    {
        thisCam.enabled = true;
        if (thisPS != null)
            thisPS.SetActive(true);
    }

    public void TurnOff()
    {
        thisCam.enabled = false;
        if (thisPS != null)
            thisPS.SetActive(false);
    }
}
