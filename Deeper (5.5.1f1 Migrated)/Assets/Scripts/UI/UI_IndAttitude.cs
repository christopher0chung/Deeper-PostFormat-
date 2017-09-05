using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_IndAttitude : Deeper_Component {

    [Header("References")]
    public Transform ind;
    public TextMeshPro tm;
    public GameObject p1Up;
    public GameObject p1Down;
    public GameObject p2Up;
    public GameObject p2Down;

    [Header("Values")]
    public Vector3 max;
    public Vector3 min;

    private Controlled_Sub _sub;

    private float _valActual;
    private float _valTrunc;
    private string _valPrint; 

    private float _lerpPerc;


    void Awake ()
    {
        Initialize(4900);
    }
    void Start()
    {
        _sub = GetComponent<Controlled_Sub>();
        p1Up.SetActive(false);
        p1Down.SetActive(false);
        p2Up.SetActive(false);
        p2Down.SetActive(false);
    }

    public override void PostUpdate()
    {
        _valActual = _sub._attitudeAngActual;
        _valTrunc = (float) System.Math.Round(_valActual, 1);
        _valPrint = _valTrunc.ToString();
        tm.text = _valPrint + "°";

        AssessInputs();

        _lerpPerc = (_valActual + 30) / 60f;
        ind.localEulerAngles = Vector3.Lerp(min, max, _lerpPerc);
    }

    private void AssessInputs()
    {
        if (_sub._maneuveringFloats[2] > .15f)
        {
            p1Up.SetActive(true);
            p1Down.SetActive(false);
        }
        else if (_sub._maneuveringFloats[2] < -.15f)
        {
            p1Up.SetActive(false);
            p1Down.SetActive(true);
        }
        else
        {
            p1Up.SetActive(false);
            p1Down.SetActive(false);
        }

        if (_sub._maneuveringFloats[3] > .15f)
        {
            p2Up.SetActive(true);
            p2Down.SetActive(false);
        }
        else if (_sub._maneuveringFloats[3] < -.15f)
        {
            p2Up.SetActive(false);
            p2Down.SetActive(true);
        }
        else
        {
            p2Up.SetActive(false);
            p2Down.SetActive(false);
        }
    }
}
