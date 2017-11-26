using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTreasureOnDeath : MonoBehaviour {

    public int numMin;
    public int numMax;

    public float spawnRadius;
    public float spawnVelocity;

    public GameObject treasure;

    public void OnDestroy()
    {
        GameObject curObject;
        int rand = Random.Range(numMin, numMax + 1);

        for(int i = numMin; i < numMax; i++)
        {
            curObject = Instantiate(treasure, gameObject.transform.position + (Random.insideUnitSphere * spawnRadius), Quaternion.identity);
            curObject.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitSphere * spawnVelocity);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
