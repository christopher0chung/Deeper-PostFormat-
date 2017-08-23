using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_CubeMaker : MonoBehaviour {

    public GameObject prefab;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Instantiate(prefab, new Vector3(-50 + i * 20, -50 + j * 20, 0), Quaternion.identity);
                }
            }
        }
    }

}
