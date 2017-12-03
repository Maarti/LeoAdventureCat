using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilationGameController : MonoBehaviour {

    public Vector3 cameraPosition = new Vector3(5.6f,18.2f,-11f);
    GameObject cam;
    bool isPlaying = false, isCameraInit=false;

	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera");
        cameraPosition.z = cam.transform.position.z;
    }
	
	void Update () {
		
	}

    public void StartGame()
    {
        if (!isCameraInit)
            InitCamera();
    }

    public void EndGame()
    {
        StopCoroutine("MoveCamera");
        cam.GetComponent<CameraFloorController>().enabled = true;
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
