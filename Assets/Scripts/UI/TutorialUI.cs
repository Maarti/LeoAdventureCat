using UnityEngine;

public class TutorialUI : MonoBehaviour
{

	public GameObject uiElementOn, uiElementOff;
	public bool fadeControllerUi = false;
    public SpeechBubbleController bubble;
    public float bubbleTimeToLive = 0f;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			SetHalo ();
			if (fadeControllerUi)
				FadeOutControllerUI ();
            if (bubble != null && bubbleTimeToLive > 0f)
                bubble.StartWaitThenFadeOut(bubbleTimeToLive);
			Destroy (this.gameObject);
		}
	}

	void SetHalo ()
	{
		if (uiElementOn)
			uiElementOn.GetComponent<Animator> ().SetBool ("halo", true);
		if (uiElementOff)
			uiElementOff.GetComponent<Animator> ().SetBool ("halo", false);
	}

	void FadeOutControllerUI ()
	{
		GameObject mc = GameObject.Find ("Canvas/GameUI/MobileController");
		if (mc)
			mc.GetComponent<DestroyIfNotMobile> ().FadeOutUI ();
	}

}
