using UnityEngine;

public class CityEndSign : EndSign
{

    public GameObject obstaclesGenerator;
    public float endOffsetPosition = 10f;
    CityUIController guictrl;

    // Use this for initialization
    protected override void Start() {
        guictrl = GameObject.Find("Canvas/GameUI").GetComponent<CityUIController>();
        transform.position = new Vector3(obstaclesGenerator.GetComponent<ObstaclesGenerator>().distanceToTravel+endOffsetPosition, transform.position.y);
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
            guictrl.EndGame();

    }
}