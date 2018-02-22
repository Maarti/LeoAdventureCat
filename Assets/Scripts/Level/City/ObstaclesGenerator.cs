using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObstaclesGenerator : MonoBehaviour {

    public Transform topGen, botGen;
    public Obstacle[] obstaclesParam;
    public GameObject kittyzPrefab, player;
    public float distanceToTravel = 100f, kittyzChance = 0f, kittyzOffset = -1f;
    public int nbObstaclesTotal = 10, kittyzTotal, kittyzCount = 0;
    public float spawnChance = 0.5f;

    float distByObsAvg;                         // average distance between obstacles
    int obsCount = 0;                           // number of obstacles spawned since the beginning
    float theoreticalObsCount = 0f;             // at our current distance, we should have spawned this number of obstacles
    float lastSpawnDist = 0f;                   // distance where last spawn took place
    const float minDistBetweenObs = 2f;         // the minimum space between 2 obstacles
    SkateController pc;
    Stack<GameObject> obstaclesStack;

    [System.Serializable]
    public struct Obstacle {
        public GameObject prefab;
        public int number;
    }


    void Start()
    {
        if (nbObstaclesTotal <= 0)
            return;
        pc = player.GetComponent<SkateController>();
        InitObstaclesStack();
        distByObsAvg = distanceToTravel / nbObstaclesTotal;
        Invoke("Spawn", 1f);
    }

    void InitObstaclesStack() {
        nbObstaclesTotal = 0;
        /*foreach (Obstacle obs in obstaclesParam) {
            nbObstaclesTotal += obs.number;
        }*/
        obstaclesStack = new Stack<GameObject>();
        //int index = 0;
        foreach (Obstacle obs in obstaclesParam) {
            nbObstaclesTotal += obs.number;
            for (int i = 0; i < obs.number; i++) {
                obstaclesStack.Push(obs.prefab);
                //obstaclesArray[index] = obs.prefab;
                //index++;
            }
        }

        System.Random rnd = new System.Random();
        obstaclesStack = new Stack<GameObject>(obstaclesStack.OrderBy(x => rnd.Next()));
    }

    void Spawn()
    {
        UpdateSpawnChance();
        if (Random.value < spawnChance)
        {
            //Instantiate(obstacles[Random.Range(0, obstacles.GetLength(0))].prefab, botGen.position, Quaternion.identity);
            Instantiate(obstaclesStack.Pop(), botGen.position, Quaternion.identity);
            lastSpawnDist = botGen.position.x;
            obsCount++;
            Debug.Log("Spawn");
        } else {
            UpdateKittyzChance();
            if (Random.value < kittyzChance) {
                // spawn a kittyz with an x offset and random y
                Instantiate(kittyzPrefab, new Vector3(botGen.position.x + kittyzOffset, botGen.position.y + Random.Range(0.25f, 1.5f)), Quaternion.identity);
                kittyzCount++;
                Debug.Log("Kittyz spawned");
            }
        }

        Invoke("Spawn", 0.5f);
    }

    void UpdateSpawnChance()
    {
        if (obsCount >= nbObstaclesTotal || (transform.position.x - lastSpawnDist) < (minDistBetweenObs+pc.speed)){
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
        float distanceAdjustment = Mathf.Lerp(-0.2f, 0.2f, transform.position.x / distanceToTravel);

        this.spawnChance = Mathf.Clamp(newSpawnChance + distanceAdjustment,0f,1f);

        Debug.Log("spawnChance=" + spawnChance + " dist/total=" + transform.position.x + "/" + distanceToTravel + "    obsCount=" + obsCount + "   theoreticalObsCount=" + theoreticalObsCount + " distanceAdjustment=" + distanceAdjustment );
    }

    void UpdateKittyzChance() {
        if (kittyzCount >= kittyzTotal || ((botGen.position.x + kittyzOffset - lastSpawnDist) < (minDistBetweenObs+pc.speed))){
            this.kittyzChance = 0f;
            return;
        }

        float newKittyzChance = 0.05f, theoreticalKittyzCount;
        theoreticalKittyzCount = transform.position.x / ((distanceToTravel-10) / kittyzTotal); // At our current distance, we should have spawned this number of kittyz

        // if we are late, increase the chance
        if (theoreticalKittyzCount > kittyzCount)
            newKittyzChance += (theoreticalKittyzCount - kittyzCount);
        
        // reduce the chance if we are near the beginning, chances are normal if we reached to middle
        float distanceAdjustment = Mathf.Clamp(botGen.position.x / (distanceToTravel / 2),0f,1f);

        this.kittyzChance = Mathf.Clamp(newKittyzChance * distanceAdjustment, 0f, 1f);

        Debug.Log("kittyzChance=" + kittyzChance + "    theoreticalKittyzCount=" + theoreticalKittyzCount + " distanceAdjustment=" + distanceAdjustment);
    }

}
