using UnityEngine;

public class CityEndSign : EndSign
{

    public GameObject obstaclesGenerator;
    new protected CityUIController guic;

    // Use this for initialization
    protected override void Start() {
        guic = GameObject.Find("Canvas/GameUI").GetComponent<CityUIController>();
        transform.position = new Vector3(obstaclesGenerator.GetComponent<ObstaclesGenerator>().distanceToTravel, transform.position.y);
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
            guic.EndGame();

    }
}