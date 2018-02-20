using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour {

    public Transform topGen, botGen;
    public GameObject[] obstacles;
    public float spawnMin = 1f, spawnMax=3f;
	
	void Start () {
        Invoke("Spawn", Random.Range(spawnMin, spawnMax));
    }
	
	void Spawn()
    {
        Instantiate(obstacles[Random.Range(0, obstacles.GetLength(0))], botGen.position, Quaternion.identity);
        Invoke("Spawn", Random.Range(spawnMin, spawnMax));
    }
}
