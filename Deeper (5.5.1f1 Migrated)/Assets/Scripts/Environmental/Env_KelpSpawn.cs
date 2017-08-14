using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env_KelpSpawn : MonoBehaviour {

    public GameObject kelp;

    public Vector3 minCoord;
    public Vector3 maxCoord;

    public int numberCluster;

    public int minClusterSize;
    public int maxClusterSize;

    public float minClusterRad;
    public float maxClusterRad;


    void Start () {
        for (int i = 0; i < numberCluster; i++)
        {
            Vector3 clusterCoord = new Vector3(Random.Range(minCoord.x, maxCoord.x), 0, Random.Range(minCoord.z, maxCoord.z));

            int thisCluster = Random.Range(minClusterSize, maxClusterSize);

            for (int j = 0; j < thisCluster; j++)
            {
                float range = Random.Range(minClusterRad, maxClusterRad);
                Vector3 loc = clusterCoord + new Vector3(Mathf.Sin(2 * Mathf.PI * j / thisCluster) * range, 0, Mathf.Cos(2 * Mathf.PI * j / thisCluster) * range);
                GameObject k = Instantiate(kelp, loc, Quaternion.identity, this.transform);
                k.transform.localScale = Vector3.one * Random.Range(1.00f, 3.00f);
            }
        }
	}
}
