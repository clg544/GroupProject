using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour {

    enum MapState
    {
        UNUSED,
        UNVISITED,
        VISITED,
        CURLEVEL,
        SHIPLEVEL
    }
    
    GameObject SceneGen;
    MapState[,] ActiveMap;
    public int centre;

    public void GenerateMap()
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = 0;
        float maxY = 0;

        Transform[] levels = SceneGen.gameObject.GetComponentsInChildren<Transform>();

        /* Decode our coor system */
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].position.x > 0 && levels[i].position.x < minX)
                minX = levels[i].position.x;

            if (levels[i].position.y > 0 && levels[i].position.y < minY)
                minY = levels[i].position.y;

            if (levels[i].position.x > maxX)
                maxX = levels[i].position.x;

            if (levels[i].position.y > maxY)
                maxY = levels[i].position.y;
        }


        ActiveMap = new MapState[(2 * (int)(maxX / minX)) + 1, (2 * (int)(maxY / minY)) + 1];

        foreach (MapState m in ActiveMap)
        {
            MapState curState = m;
            curState = MapState.UNUSED;
        }

        int x;
        int y;
        for (int i = 0; i < levels.Length; i++)
        {
            x = Mathf.RoundToInt(levels[i].position.x / minX);
            y = Mathf.RoundToInt(levels[i].position.y / minY);

            print(x + " " + y);

            ActiveMap[x, y] = MapState.UNVISITED;
        }

        centre = (Mathf.RoundToInt(maxX / maxY) / 2) + 1;
    }

	// Use this for initialization
	void Start () {
        GameObject[] managers = GameObject.FindGameObjectsWithTag("Manager");

        for(int i = 0; i < managers.Length; i++)
        {
            if(managers[i].name == "Scene Generator")
            {
                SceneGen = managers[i];
            }
        }

        GenerateMap();
	}

	// Update is called once per frame
	void Update () {


    }
}
