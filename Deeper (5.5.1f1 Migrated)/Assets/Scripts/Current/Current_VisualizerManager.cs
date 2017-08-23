using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current_VisualizerManager : Deeper_Component {

    public GameObject prefab;

    public List<GameObject> myObjs;
    public List<float> myTimes;

    public int num;
    public int range;
    public float lifeTime;
    public float fMag;
    public float tMag;

	void Start () {
        Initialize(5000);

        for (int i = 0; i < num; i++)
        {
            GameObject g = Instantiate(prefab, Where(), Quaternion.identity);
            g.GetComponent<Particle_Controller>().OnOff(true);
            g.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-fMag, fMag), Random.Range(-fMag, fMag), 0), ForceMode.Impulse);
            g.GetComponent<Rigidbody>().AddTorque(Vector3.forward * Random.Range(-tMag, tMag), ForceMode.Impulse);
            myObjs.Add(g);
            myTimes.Add(((float)i / num) * lifeTime * 2);
        }
	}

    private float timer;

	public override void PostUpdate () {

        //Debug.DrawLine(transform.position, ProjectedLoc());

        for (int i = 0; i < myTimes.Count; i++)
        {
            myTimes[i] += Time.deltaTime;

            if (myTimes[i] > lifeTime)
            {
                myObjs[i].GetComponent<Particle_Controller>().OnOff(false);
            }
            if (myTimes[i] > lifeTime * 2)
            {
                //myTimes.Remove(myTimes[i]);

                //GameObject toDie = myObjs[i];
                //myObjs.Remove(toDie);
                //toDie.GetComponent<Deeper_Component>().Cleanup();

                myTimes[i] = 0;
                myObjs[i].transform.position = Where();
                myObjs[i].GetComponent<Particle_Controller>().OnOff(true);
                myObjs[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                myObjs[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                myObjs[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-fMag, fMag), Random.Range(-fMag, fMag), 0), ForceMode.Impulse);
                myObjs[i].GetComponent<Rigidbody>().AddTorque(Vector3.forward * Random.Range(-tMag, tMag), ForceMode.Impulse);
            }
        }

        //if (myObjs.Count <= num)
        //{
        //    GameObject g = Instantiate(prefab, Where(), Quaternion.identity);
        //    g.GetComponent<Particle_Controller>().OnOff(true);
        //    g.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-fMag, fMag), Random.Range(-fMag, fMag), 0), ForceMode.Impulse);
        //    g.GetComponent<Rigidbody>().AddTorque(Vector3.forward * Random.Range(-tMag, tMag), ForceMode.Impulse);
        //    myObjs.Add(g);
        //    myTimes.Add(0);
        //}

	}

    private Vector3 Where()
    {
        Vector3 pos = (transform.position + Vector3.up * Random.Range(-range, range) + Vector3.right * Random.Range(-range, range));
        pos.z = 0;
        return pos;
    }

    private Vector3 ProjectedLoc()
    {
        return transform.position + transform.forward * (-transform.position.z / Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad));
    }
}
