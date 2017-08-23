using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current_VisualizerManager : Deeper_Component {

    public GameObject prefab;

    public List<GameObject> myObjs;
    public List<float> myTimes;

    public int num;
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
            myTimes.Add(Random.Range(0.0f, 5.0f));
        }
	}

    private float timer;

	public override void PostUpdate () {
        for (int i = 0; i < myTimes.Count; i++)
        {
            myTimes[i] += Time.deltaTime;

            if (myTimes[i] > 5)
            {
                myObjs[i].GetComponent<Particle_Controller>().OnOff(false);
            }
            if (myTimes[i] > 10)
            {
                myTimes.Remove(myTimes[i]);

                GameObject toDie = myObjs[i];
                myObjs.Remove(toDie);
                toDie.GetComponent<Deeper_Component>().Cleanup();
            }
        }

        if (myObjs.Count <= num)
        {
            GameObject g = Instantiate(prefab, Where(), Quaternion.identity);
            g.GetComponent<Particle_Controller>().OnOff(true);
            g.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-fMag, fMag), Random.Range(-fMag, fMag), 0), ForceMode.Impulse);
            g.GetComponent<Rigidbody>().AddTorque(Vector3.forward * Random.Range(-tMag, tMag), ForceMode.Impulse);
            myObjs.Add(g);
            myTimes.Add(Random.Range(0.0f, 2.0f));
        }

	}

    private Vector3 Where()
    {
        return (transform.position + Vector3.up * Random.Range(-20, 20) + Vector3.right * Random.Range(-20, 20));
    }
}
