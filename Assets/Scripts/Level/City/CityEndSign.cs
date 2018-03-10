using UnityEngine;

public class CityEndSign : EndSign
{

    public GameObject obstaclesGenerator, boss;
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
        if(boss==null)
            transform.position = new Vector3(obstaclesGenerator.GetComponent<ObstaclesGenerator>().distanceToTravel + endOffsetPosition, transform.position.y);
        else
            transform.position = new Vector3(boss.GetComponent<CityBossController>().distanceToEndBoss + endOffsetPosition, transform.position.y);
    }
}