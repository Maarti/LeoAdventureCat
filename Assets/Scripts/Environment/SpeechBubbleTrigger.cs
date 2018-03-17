using UnityEngine;

public class SpeechBubbleTrigger : MonoBehaviour {

    public string stringId;
    public SpeechBubbleController bubble;
    public float textTime = 5f;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            bubble.SetText(stringId, textTime);
            Destroy(this.gameObject);
        }
    }
}
