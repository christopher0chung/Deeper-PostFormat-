using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current_Test : MonoBehaviour {

    public Vector2 lowerLeftBound;
    public Vector2 upperRightBound;

    public Texture2D c1;
    public Texture2D c2;

    public float mag;

    [HideInInspector] public int xOffset;
    [HideInInspector] public float _xOffset;
    public float xSpeed;

    [HideInInspector] public int yOffset;
    [HideInInspector] public float _yOffset;
    public float ySpeed;

    public float xSetpoint;
    public float ySetpoint;

    private float timer;

    public void Update()
    {
        _xOffset += xSpeed;
        _yOffset += ySpeed;

        xOffset = (int)_xOffset;
        yOffset = (int)_yOffset;

        timer += Time.deltaTime;
        if (timer >= 1)
        {
            _xOffset += Random.Range(1.0f, 42.0f);
            _yOffset += Random.Range(1.0f, 42.0f);
            timer -= Random.Range(.01f, 1f);
        }
    }

    public Vector3 GetForce (Vector3 position)
    {
        float x = c1.width * (((position.x + xOffset + (upperRightBound.x - lowerLeftBound.x)) % (upperRightBound.x - lowerLeftBound.x))- lowerLeftBound.x) / (upperRightBound.x - lowerLeftBound.x);
        float y = c1.height * (((position.y + yOffset + (upperRightBound.y - lowerLeftBound.y)) % (upperRightBound.y - lowerLeftBound.y)) - lowerLeftBound.y) / (upperRightBound.y - lowerLeftBound.y);
        //Debug.Log(mag * new Vector3(c1.GetPixel((int)x, (int)y).r - .5f, c2.GetPixel((int)x, (int)y).r - .5f, 0));
        return mag * new Vector3(c1.GetPixel((int)x, (int)y).r - xSetpoint, c2.GetPixel((int)x, (int)y).r - ySetpoint, 0);
    }
}
