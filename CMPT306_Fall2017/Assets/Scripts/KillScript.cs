using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillScript : MonoBehaviour {

    public float elapsedTime;
    public float lifetime;

	// Use this for initialization
	void Start () {
        elapsedTime = 0;
	}

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > lifetime)
            Destroy(gameObject);
    }

    void setLifetime(float seconds)
    {
        lifetime = seconds;
    }
}
