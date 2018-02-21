using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour {

    public Transform topGen, botGen;
    public GameObject[] obstacles;
    //public float spawnMin = 1f, spawnMax=3f;
    public float distanceToTravel = 100f;
    public int nbObstaclesTotal = 10;
    public float spawnChance = 0.5f;

    float distByObsAvg;                         // average distance between obstacles
    int obsCount = 0;                           // number of obstacles spawned since the beginning
    float theoreticalObsCount = 0f;             // at our current distance, we should have spawned this number of obstacles
    float lastSpawnDist = 0f;                   // distance where last spawn took place
    const float minDistBetweenObs = 3f;         // the minimum space between 2 obstacles

    void Start()
    {
        if (nbObstaclesTotal <= 0)
            return;

        distByObsAvg = distanceToTravel / nbObstaclesTotal;
        //Invoke("Spawn", Random.Range(spawnMin, spawnMax));
        Invoke("Spawn", 1f);
    }

    void Spawn()
    {
        UpdateSpawnChance();
        if (Random.value < spawnChance)
        {
            Instantiate(obstacles[Random.Range(0, obstacles.GetLength(0))], botGen.position, Quaternion.identity);
            lastSpawnDist = botGen.position.x;
            obsCount++;
            Debug.Log("Spawn");
        }

        //Invoke("Spawn", Random.Range(spawnMin, spawnMax));
        Invoke("Spawn", 0.5f);
    }

    void UpdateSpawnChance()
    {
        if (obsCount >= nbObstaclesTotal || (transform.position.x - lastSpawnDist) < minDistBetweenObs){
            this.spawnChance = 0f;
            return;
        }

        float newSpawnChance = 0.5f;
        theoreticalObsCount = transform.position.x / distByObsAvg; // At our current distance, we should have spawned this number of obstacles

        // if we are late, increase the spawnChance
        if (theoreticalObsCount > obsCount)
            newSpawnChance = 0.5f + (theoreticalObsCount - obsCount) / nbObstaclesTotal;
        // else, reduce the spawn rate
        else
            newSpawnChance = 0.5f - (obsCount - theoreticalObsCount) * 2 / nbObstaclesTotal;

        // reduce the spawnChance if we are near the beginning (-0.2), increase it if we are near the end (+0.2)
        //float currentDistLerp = Mathf.Lerp(0, distanceToTravel, transform.position.x);
        //float distanceAdjustment = (currentDistLerp * 0.4f) - 0.2f;
        float distanceAdjustment = Mathf.Lerp(-0.2f, 0.2f, transform.position.x / distanceToTravel);

        this.spawnChance = Mathf.Clamp(newSpawnChance + distanceAdjustment,0f,1f);

        Debug.Log("spawnChance=" + spawnChance + " dist/total=" + transform.position.x + "/" + distanceToTravel + "    obsCount=" + obsCount + "   theoreticalObsCount=" + theoreticalObsCount + " distanceAdjustment=" + distanceAdjustment );
    }
}
