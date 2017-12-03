using UnityEngine;

public class FallingLampController : MonoBehaviour
{
	public float lineOfSight = 2f;
	public LayerMask playerLayer;
	public int damageOnCollision = 1;
	public Vector2 bumpOnCollision = new Vector2 (-2, 0);
	Transform leftStartLoS, rightStartLoS;
	public bool isFalling = false, hasFallen = false;
	Animator animator;

	// Use this for initialization
	void Start ()
	{
		animator = transform.parent.gameObject.GetComponent<Animator> ();
		leftStartLoS = transform.Find ("LeftStartLoS");
		rightStartLoS = transform.Find ("RightStartLoS");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!hasFallen && !isFalling && CheckLoS ()) {
			animator.SetBool ("switch", false);
			isFalling = true;
		}
	}

	GameObject CheckLoS ()
	{
		// Detecting player in line of sight
		if (lineOfSight > 0) {
			RaycastHit2D ray = Physics2D.Raycast (leftStartLoS.position, Vector2.down, lineOfSight, playerLayer);
			Debug.DrawRay (leftStartLoS.position, Vector2.down * lineOfSight, Color.red);
			if (ray)
				return ray.collider.gameObject;
			else {
				ray = Physics2D.Raycast (rightStartLoS.position, Vector2.down, lineOfSight, playerLayer);
				Debug.DrawRay (rightStartLoS.position, Vector2.down * lineOfSight, Color.red);
				if (ray)
					return ray.collider.gameObject;
			}
		}
		return null;
	}

	void OnCollisionStay2D (Collision2D other)
	{
		if (isFalling && other.transform.tag == "Player")
			other.gameObject.GetComponent<PlayerController> ().Defend (this.gameObject, damageOnCollision, bumpOnCollision, 0.5f);
		else if (isFalling && LayerMask.LayerToName (other.gameObject.layer) == "Ground") {
			isFalling = false;
			hasFallen = true;
		}
	}
}
