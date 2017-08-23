using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current_Wander : Deeper_Component {

    public Vector3 lowerLeft;
    public Vector3 upperRight;
    public float speed;

    private Vector3 _dest;

    public void Awake()
    {
        Initialize(3000);
        _dest = NewPoint();
    }

    public override void NormUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _dest, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _dest) <= 5)
            _dest = NewPoint();
    }

    private Vector3 NewPoint()
    {
        if(Random.Range(0, 2) == 0) //0-topbottom 1-lefright
        {
            if(Random.Range(0, 2) == 0) //0-lower 1-higher
                return new Vector3(Random.Range(lowerLeft.x, upperRight.x), lowerLeft.y, 0);
            else
                return new Vector3(Random.Range(lowerLeft.x, upperRight.x), upperRight.y, 0);
        }
        else
        {
            if (Random.Range(0, 2) == 0) //0-lower 1-higher
                return new Vector3(lowerLeft.x, Random.Range(lowerLeft.y, upperRight.y), 0);
            else
                return new Vector3(upperRight.x, Random.Range(lowerLeft.y, upperRight.y), 0);
        }
    }
}
