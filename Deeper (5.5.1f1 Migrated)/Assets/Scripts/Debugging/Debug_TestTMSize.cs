using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Debug_TestTMSize : MonoBehaviour {

    public TextMeshPro myTMP;
    private string myTestString;

	void Start () {
		for (int i = 0; i < 99; i++)
        {
            if (i <= 9)
                myTestString += "[0" + i + "] ";
            else
                myTestString += "[" + i + "] ";
        }
        myTMP.text = myTestString;
	}

}
