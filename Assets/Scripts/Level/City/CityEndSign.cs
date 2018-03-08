using UnityEngine;

public class CityEndSign : EndSign
{

    public GameObject obstaclesGenerator;
    public float endOffsetPosition = 10f;
    public bool isTutorial = false;
    CityUIController guictrl;

    // Use this for initialization
    protected override void Start() {
        guictrl = GameObject.Find("Canvas/GameUI").GetComponent<CityUIController>();
        if(isTutorial)
            transform.position = new Vector3(-100, transform.position.y);
        else
            Init();
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
            guictrl.EndGame();

    }

    public void Init() {
        transform.position = new Vector3(obstaclesGenerator.GetComponent<ObstaclesGenerator>().distanceToTravel + endOffsetPosition, transform.position.y);
    }
}