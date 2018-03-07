using System.Collections;
using UnityEngine;

public class CityTutorial : MonoBehaviour {

    public SpeechBubbleController bubble;
    public GameObject tutoImgCrouch, tutoImgJump, tutoImgDoubleJump, tutoImgAttack, obsGen;
    public SkateController skateCtrlr;

    void Start() {
        obsGen.SetActive(false);
        Invoke("DisplayCrouchTuto",2f);
    }

    void DisplayCrouchTuto() {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        tutoImgCrouch.SetActive(true);
#endif
        bubble.SetText("CITY_TUTO_CROUCH", 10f);
        // Subscribe to player crouch event
        skateCtrlr.OnCrouch += OnCrouch;
    }

    void OnCrouch() {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        tutoImgCrouch.SetActive(false);
#endif
        skateCtrlr.OnCrouch -= OnCrouch;
        DisplayJumpTuto();
    }

    void DisplayJumpTuto() {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        tutoImgJump.SetActive(true);
#endif
        bubble.SetText("CITY_TUTO_JUMP", 10f);
        skateCtrlr.OnJump += OnJump;
    }

    void OnJump() {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        tutoImgJump.SetActive(false);
#endif
        skateCtrlr.OnJump -= OnJump;
        DisplayAttackTuto();
    }

    void DisplayAttackTuto() {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        tutoImgAttack.SetActive(true);
#endif
        bubble.SetText("CITY_TUTO_ATTACK", 10f);
        skateCtrlr.OnAttack += OnAttack;
    }

    void OnAttack() {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        tutoImgAttack.SetActive(false);
#endif
        skateCtrlr.OnAttack -= OnAttack;
        DisplayDoubleJumpTuto();
    }

    void DisplayDoubleJumpTuto() {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        tutoImgDoubleJump.SetActive(true);
#endif
        bubble.SetText("CITY_TUTO_DOUBLEJUMP", 10f);
        skateCtrlr.OnDoubleJump += OnDoubleJump;
    }

    void OnDoubleJump() {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        tutoImgDoubleJump.SetActive(false);
#endif
        bubble.SetText("CITY_TUTO_AWESOME", 1f);
        skateCtrlr.OnDoubleJump -= OnDoubleJump;
        StartCoroutine(EndTuto());
    }

    IEnumerator EndTuto() {
        yield return new WaitForSeconds(1f);
        // agrandir cercle noir
        Vector3 pos = skateCtrlr.gameObject.transform.position;
        pos.x = 0f;
        skateCtrlr.gameObject.transform.position = pos;
        obsGen.SetActive(true);
        yield return null;
        // retrecir cercle noir
        yield return null;
        Destroy(this.gameObject);
    }
}
