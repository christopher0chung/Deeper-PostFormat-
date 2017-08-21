using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [ExecuteInEditMode]
public class Current_Test : MonoBehaviour {

    public Vector2 lowerLeftBound;
    public Vector2 upperRightBound;

    public Texture2D c1;
    public Texture2D c2;

    public float mag; 

    public int x;
    public int y;

    [SerializeField] private Vector3 output;

    public void Update()
    {
        x = Mathf.Clamp(x, 0, c1.width - 1);
        y = Mathf.Clamp(y, 0, c1.height - 1);
        output.x = c1.GetPixel(x, y).r - .5f;
        output.y = c2.GetPixel(x, y).r - .5f;
        output.z = 0;
        output = output * mag;
    }

    public Vector3 GetForce (Vector3 position)
    {
        //Vector2 convertedPos.x = position.x 
        return Vector3.zero;
    }
}
