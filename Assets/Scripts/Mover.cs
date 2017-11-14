using UnityEngine;

public class Mover : MonoBehaviour
{

	public Vector3 move;
	public float speed;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector3 (
			Mathf.Clamp (transform.position.x + move.x * speed * Time.deltaTime, -100000, 100000),
			Mathf.Clamp (transform.position.y + move.y * speed * Time.deltaTime, -100000, 100000),
			Mathf.Clamp (transform.position.z + move.z * speed * Time.deltaTime, -100000, 100000)
		);
			
	}
}
