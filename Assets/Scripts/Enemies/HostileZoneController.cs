using UnityEngine;

public class HostileZoneController : MonoBehaviour
{

	public int damage = 1;
	public Vector2 bumpVelocity = new Vector2 (1, 2);
	public float bumpDuration = 0.5f;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<IDefendable> ().Defend (this.gameObject, damage, bumpVelocity, bumpDuration);
		}

	}
}
