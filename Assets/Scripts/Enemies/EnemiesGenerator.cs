using System.Collections;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour {

    public Transform[] enemies;
    public float waitMin = 2f, waitMax=4f;
    public bool popOnStart = false;

    Coroutine coroutine;
	
	void Start () {
        if (popOnStart)
            StartPoping();
	}

    public void StartPoping()
    {
        coroutine = StartCoroutine(GenerateEnemies());
    }

    public void StopPoping()
    {
        if(coroutine!=null)
            StopCoroutine(coroutine);
    }
	
	IEnumerator GenerateEnemies()
    {
        while (true)
        {
            GenerateEnemy();
            yield return new WaitForSeconds(Random.Range(waitMin, waitMax));
        }
    }

    void GenerateEnemy()
    {
        Transform enemy = enemies[Random.Range(0, enemies.Length)];
        Instantiate(enemy, this.transform.position, this.transform.rotation);
    }
}
