using UnityEngine;

[RequireComponent(typeof(Flare))]
public class CityFlareController : MonoBehaviour {
    
    public float localStart = 5f, localEnd = -5f;
    public ObstaclesGenerator obsGen;
    float distanceToTravel;
    
	void Start () {
        distanceToTravel = obsGen.distanceToTravel;
	}
	
	void Update () {
        Vector3 localPos = this.transform.localPosition;
        localPos.x = Mathf.Lerp(localStart, localEnd, transform.parent.position.x / distanceToTravel);
        this.transform.localPosition = localPos;
    }
}
