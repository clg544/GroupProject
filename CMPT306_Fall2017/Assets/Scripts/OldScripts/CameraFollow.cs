using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform lookAt;

	private Vector3 positionOffSet = new Vector3(0, 0, -6.5f);

	private void Start () {
	}
	
	// Update is called once per frame
	private void Update () {
		transform.position = lookAt.transform.position + positionOffSet;
	}
}
