using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_SuitAirSystem : MonoBehaviour
{
    public float tempStartingAirPercent;

    public float breathRate;
    public float boostRate;

    private float airPerc;

    void Start()
    {
        airPerc = tempStartingAirPercent;
    }

    public void Breath ()
    {
        airPerc -= breathRate * Time.deltaTime;
    }

    public void Boost ()
    {
        airPerc -= boostRate * Time.deltaTime;
    }
}