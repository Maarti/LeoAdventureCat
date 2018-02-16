using UnityEngine;

public class ActivateOnEnter : MonoBehaviour {

    public GameObject[] objs;
    
	void Start () {
        foreach (GameObject obj in objs)
        {
            obj.SetActive(false);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            foreach (GameObject obj in objs)
            {
                obj.SetActive(true);
            }
        }
    }
}
