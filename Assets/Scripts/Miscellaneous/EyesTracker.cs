using UnityEngine;

public class EyesTracker : MonoBehaviour {

    public GameObject[] eyes;    
    public float eyeRadius = 0.06f;
    public bool hit = false;

    Transform player;
    Transform[] pupils;
    Vector3[] originPupils;
    
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        int index = 0;
        originPupils = new Vector3[eyes.Length];
        pupils = new Transform[eyes.Length];
        foreach(GameObject eye in eyes) {
            pupils[index] = eye.transform.Find("Pupil");
            originPupils[index] = pupils[index].position;
            index++;
        }        
    }

    void Update() {
        Vector3 lookDir;
        int index = 0;
        foreach (GameObject eye in eyes) {
            // If behind hit, update the origin because  it's moving
            if (hit) {
                Vector3 origin = pupils[index].parent.position;
                origin.z = originPupils[index].z;
                originPupils[index] = origin;

            }

            lookDir = (player.position - originPupils[index]).normalized;
            pupils[index].position = originPupils[index] + (lookDir * eyeRadius);
            index++;
        }
    }
}
