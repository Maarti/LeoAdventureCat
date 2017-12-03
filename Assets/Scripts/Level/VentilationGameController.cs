using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilationGameController : MonoBehaviour {

    public GameObject[] switches;
    public Vector3 cameraPosition = new Vector3(5.6f,18.2f,-11f);
    public GameObject catBoundary, ratBoundary, finalBlower;
    GameObject cam;
    bool isPlaying = false, isCameraInit=false;
    Rigidbody2D ratRb;
    Coroutine camRoutine = null;

	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera");
        cameraPosition.z = cam.transform.position.z;
        ratRb = GameObject.FindGameObjectWithTag("Rat").GetComponent<Rigidbody2D>();
    }
	
	void Update () {
		
	}

    public void StartGame()
    {
        // camera
        if (!isCameraInit)
            InitCamera();
        // boundaries
        catBoundary.SetActive(true);
        ratBoundary.SetActive(true);
        // rat
        ratRb.isKinematic = false;
        ratRb.gravityScale = 0f;
        // activate all switches
        EnableSwitches(true);
    }

    public void EndGame()
    {
        // camera
        if (camRoutine != null)
            StopCoroutine(camRoutine);
        cam.GetComponent<CameraFloorController>().enabled = true;
        // boundaries
        Destroy(catBoundary);
        Destroy(ratBoundary);
        // rat
        //ratRb.isKinematic = true;
        ratRb.gravityScale = 1f;
        ratRb.velocity = Vector2.zero;
        ratRb.gameObject.GetComponent<Animator>().SetBool("isFloating", false);
        // deactivate all switches
        EnableSwitches(false);
        // activate final blower
        finalBlower.GetComponent<BlowerController>().damage = 1;
        finalBlower.transform.Find("Burner/RightFire").gameObject.SetActive(true);
        finalBlower.transform.Find("Burner/LeftFire").gameObject.SetActive(true);
        finalBlower.GetComponent<BlowerController>().StartBlowing();
    }

    public void InitCamera()
    {
        cam.GetComponent<CameraFloorController>().enabled = false; // deactivate camera following
        if (camRoutine != null)
            StopCoroutine(camRoutine);
        camRoutine = StartCoroutine(MoveCamera(cameraPosition));
    }

    IEnumerator MoveCamera(Vector3 target)
    {
        float i = 0f;
        while (cam.transform.position != cameraPosition)
        {
            i = Mathf.Clamp(i+Time.deltaTime/50, 0f, 1f);
            cam.transform.position = Vector3.Lerp(cam.transform.position, cameraPosition, i);
            yield return null;
        }
        isCameraInit = true;
    }

    void EnableSwitches(bool activate = true)
    {
        VentilationSwitchController ctrlr;
        foreach (GameObject ventil in switches)
        {
            ctrlr = ventil.GetComponent<VentilationSwitchController>();
            ctrlr.Activate(activate);
        }
    }
}