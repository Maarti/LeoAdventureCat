using UnityEngine;

public class CameraCityController : MonoBehaviour
{
	public float xMin = 6;
	public Vector3 offset = new Vector3 (5f, 0f, -10f);
    public GameObject player;
    public Vector2 shakeOffset = new Vector2(0.25f, 0.25f);

    float originY;
    float trauma = 0f;      // trauma represents shake intensity   
    float Trauma {
        get { return trauma; }
        set { trauma = Mathf.Clamp(value, 0f, 1f); }
    }

    void Start() {
        player.GetComponent<SkateController>().OnInjured += OnPlayerInjured;
        originY = transform.position.y;
    }

    void Update() {
        if (Trauma > 0f)
            Trauma -= 0.02f;
    }

    // LateUpdate is called after Update
    void LateUpdate ()
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		Vector3 newPosition = new Vector3(player.transform.position.x, originY, player.transform.position.z) + offset;
  
        // Borders managing
		if (newPosition.x < xMin)
			newPosition.x = xMin;

        // Move camera smoothly
        this.transform.position = newPosition;

        ShakeCamera();
    }

    void ShakeCamera() {
        if (shakeOffset == Vector2.zero || Trauma <= 0)
            return;
        float offsetX = shakeOffset.x * Trauma * Random.Range(-1f, 1f);
        float offsetY = shakeOffset.y * Trauma * Random.Range(-1f, 1f);
        this.transform.position = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);
    }

    void OnPlayerInjured() {
        Trauma += 0.5f;
    }

}