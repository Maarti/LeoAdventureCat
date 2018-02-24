using System.Collections;
using UnityEngine;

public class ObstaclesDestroyer : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Destroy " + collision.gameObject.name);

        if(collision.transform.parent)
            Destroy(collision.transform.parent.gameObject);
        else
            Destroy(collision.gameObject);
    }
}
