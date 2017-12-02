using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTreasureOnDeath : MonoBehaviour {

    int count;

    public int min;
    public int max;
    public float maxForce;

    public GameObject treasurePrefab;

    void OnDestroy()
    {
        GameObject curObj;

        for(int i = 0; i < count; i++)
        {
            curObj = Instantiate(treasurePrefab, this.transform.position, Quaternion.identity, this.transform.parent);
            curObj.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * Random.Range(0, maxForce));
        }
    }


	// Use this for initialization
	void Start () {
        count = Random.Range(min, max);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
