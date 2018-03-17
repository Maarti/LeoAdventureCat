using UnityEngine;

public class BackButtonBehaviour : MonoBehaviour
{
    public GameObject backPanel;
    public AudioSource audioS;
    public AudioClip audioClic;
    public bool closeCurrentPanel = true;

    void Update ()
	{
        // If Android back button is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
            OnBackPressed(); 
	}

    // If Android back button is pressed, go back or exit
    void OnBackPressed() {
        if (backPanel != null) {
            backPanel.SetActive(true);
            if(closeCurrentPanel)
                this.gameObject.SetActive(false);
        }
        if (audioS != null && audioClic !=null)
            audioS.PlayOneShot(audioClic);
    }

    public void QuitApplication() {
        Application.Quit();
    }
}
