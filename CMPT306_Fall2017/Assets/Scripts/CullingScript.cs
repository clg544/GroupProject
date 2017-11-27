using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingScript : MonoBehaviour {

    GameObject[] AllEnemies;
    GameObject[] AllPower;

    LinkedList<GameObject> cullingList;
    LinkedListNode<GameObject> curObj;

    bool ready = false;

    public float cullingDistance;
    public GameObject cameraTarget;

    
	// Use this for initialization
    // Set to run last of all start scripts
	void Start ()
    {
        cullingList = new LinkedList<GameObject>();

        AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        AllPower = GameObject.FindGameObjectsWithTag("Power");

        for(int i = 0; i < AllEnemies.Length; i++)
        {
            cullingList.AddFirst(AllEnemies[i]);

            if(Vector3.Distance(AllEnemies[i].transform.position, cameraTarget.transform.position) > cullingDistance)
            {
                AllPower[i].SetActive(false);
            }
        }
        for (int i = 0; i < AllPower.Length; i++)
        {
            cullingList.AddFirst(AllPower[i]);

            if (Vector3.Distance(AllPower[i].transform.position, cameraTarget.transform.position) > cullingDistance)
            {
                AllPower[i].SetActive(false);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        curObj = cullingList.First;
        while(curObj != null)
        {
            if (curObj.Value == null)
                cullingList.Remove(curObj);

            if (Vector3.Distance(curObj.Value.transform.position, cameraTarget.transform.position) < cullingDistance)
            {
				Debug.Log (curObj);
                curObj.Value.SetActive(true);
            }
            else
            {
                curObj.Value.SetActive(false);
            }

            curObj = curObj.Next;
        }
    }
}
