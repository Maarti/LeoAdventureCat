using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilationGameController : MonoBehaviour {

    public Vector3 cameraPosition = new Vector3(5.6f,18.2f,-11f);
    public GameObject catBoundary, ratBoundary;
    GameObject cam;
    bool isPlaying = false, isCameraInit=false;
    Rigidbody2D ratRb;

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
    }

    public void EndGame()
    {
        // camera
        StopCoroutine("MoveCamera");
        cam.GetComponent<CameraFloorController>().enabled = true;
        // boundaries
        Destroy(catBoundary);
        Destroy(ratBoundary);
        // rat
        ratRb.isKinematic = true;
        ratRb.gravityScale = 1f;
    }

    public void InitCamera()
    {
        cam.GetComponent<CameraFloorController>().enabled = false; // deactivate camera following
        StopCoroutine("MoveCamera");
        StartCoroutine(MoveCamera(cameraPosition));
    }

    IEnumerator MoveCamera(Vector3 target)
    {
        while (cam.transform.position != cameraPosition)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, cameraPosition, Time.deltaTime);
            yield return null;
        }
        isCameraInit = true;
    }
}
