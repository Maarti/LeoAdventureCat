using System.Collections;
using UnityEngine;

public class ObstaclesDestroyer : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Debug.Break();

        Destroy(collision.gameObject);
    }
}
