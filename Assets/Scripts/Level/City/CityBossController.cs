using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBossController : MonoBehaviour
{

    public float distanceToTriggerBoss = 0f, distanceToEndBoss = 100f, timeBetweenAttacks = 3f, wheelForce = 5f;
    public SkateController playerCtrlr;
    public GameObject introVan, van, vanContainer, wheelPrefab;
    public Transform vanRightPos, wheelPos, vanOutPos;
    public AudioClip accidentSound;

    int state = -1;
    Animator bossAnim, introVanAnim, vanAnim;
    bool waitingTimeIsSet = false;
    float startWaitingTime = 0f;
    AudioSource audioSource;

    private void Start() {
        introVanAnim = introVan.GetComponent<Animator>();
        introVan.SetActive(false);
        vanAnim = van.GetComponent<Animator>();
        van.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        bossAnim = GetComponent<Animator>();
    }

    private void Update() {
        // End boss
        if (state != -99 && playerCtrlr.transform.position.x >= distanceToEndBoss)
            EndBoss();

        // Intro van goes from left to right
        if (state == -1 && playerCtrlr.transform.position.x >= distanceToTriggerBoss) {
            introVan.SetActive(true);
            introVanAnim.SetTrigger("startRolling");
            bossAnim.SetTrigger("introVan");
            van.SetActive(true);
            vanAnim.SetTrigger("startRolling");
            state++;
        }
        // Van appears on the right then swich driver
        else if (state == 1) {
            if (vanContainer.transform.position != vanRightPos.position) {
                vanContainer.transform.position = Vector3.MoveTowards(vanContainer.transform.position, vanRightPos.position, Time.deltaTime * 1);
            } else {
                bossAnim.SetTrigger("switchDriver");
                state++;
            }
        }
        // Open rear door
        else if (state == 3) {
            vanAnim.SetTrigger("openDoorRolling");
            bossAnim.SetTrigger("rearBoss");
            state++;
        }

        // Attack
        else if (state == 4) {
            if (!waitingTimeIsSet) {
                startWaitingTime = Time.time;
                waitingTimeIsSet = true;
            } else {
                if ((Time.time - startWaitingTime) < timeBetweenAttacks)
                    return;
                else {
                    bossAnim.SetTrigger("attack");
                    waitingTimeIsSet = false;
                }
            }
        }

    }

    // Called by animator when to begin next state
    void NextState() {
        state++;
    }

    void InstantiateWheel() {
        GameObject wheel = Instantiate(wheelPrefab, wheelPos.transform.position, wheelPrefab.transform.rotation) as GameObject;
        if (Random.value < 0.5f) {
            Vector2 force = new Vector2(0f, wheelForce);
            wheel.GetComponent<Rigidbody2D>().velocity = force;
            audioSource.Play();
        }
    }

    void EndBoss() {
        StartCoroutine(VanGoAway());
        state = -99;
    }

    IEnumerator VanGoAway() {
        audioSource.Stop();
        audioSource.PlayOneShot(accidentSound);
        while (vanContainer.transform.position != vanOutPos.position) {
            vanContainer.transform.position = Vector3.MoveTowards(vanContainer.transform.position, vanOutPos.position, Time.deltaTime * 4);
            yield return null;
        }
    }
}
