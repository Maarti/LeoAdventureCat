using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CityTutorial : MonoBehaviour {

    public SpeechBubbleController bubble;
    public GameObject tutoImgCrouch, tutoImgJump, tutoImgDoubleJump, tutoImgAttack, obsGen;
    public SkateController skateCtrlr;
    public GameObject screenMask, endSign, distanceBar;

    void Start() {
        obsGen.SetActive(false);
        distanceBar.SetActive(false);
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
        screenMask.SetActive(true);
        yield return new WaitForSeconds(2f);

        // Fade screen to black
        Image mask = screenMask.GetComponent<Image>();
        Color newColor = mask.color;
        for (float t = 0.0f; t <= .5f; t += Time.deltaTime) {
            newColor.a = Mathf.Lerp(0, 1, t*2);
            mask.color = newColor;
            yield return null;
        }
        newColor.a = 1f;
        mask.color = newColor;
        yield return null;

        // Reset game
        Vector3 pos = skateCtrlr.gameObject.transform.position;
        pos.x = 0f;
        skateCtrlr.gameObject.transform.position = pos;
        skateCtrlr.Speed = 1f;
        obsGen.SetActive(true);
        distanceBar.SetActive(true);
        endSign.GetComponent<CityEndSign>().Init();
        CityGameController.gc.levelTimer = 0f;
        yield return null;
        yield return new WaitForSeconds(.25f);

        // Fade out screen
        for (float t = 0.0f; t <= .5f; t += Time.deltaTime) {
            newColor.a = Mathf.Lerp(1,0, t*2);
            mask.color = newColor;
            yield return null;
        }
        Destroy(screenMask);
        yield return null;

        Destroy(this.gameObject);
    }
}
