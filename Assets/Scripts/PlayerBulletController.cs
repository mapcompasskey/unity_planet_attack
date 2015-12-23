using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GravityBody))]
public class PlayerBulletController : MonoBehaviour {

	// public variables
	public GameObject impactEffect;
	public float damage = 1f;

	// private references
	private Rigidbody2D rb2d;
	private GravityBody gravityBody;

	// vectors
	private Vector3 horizontalVelocity = Vector3.zero;
	private Vector3 verticalVelocity = Vector3.zero;

	// booleans
	private bool facingRight = true;

	// float
	private float angle = 0f;
	private float moveSpeed = 30f;
	private float killTime = 0.4f;
	private float killTimer = 0f;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		gravityBody = GetComponent<GravityBody>();
	}

	void Update()
	{
		// increment the kill timer
		killTimer += Time.deltaTime;
		if (killTimer >= killTime)
		{
			Destroy(gameObject);
		}
	}
	
	void FixedUpdate()
	{
		// apply rotation from gravity attractor
		transform.rotation = gravityBody.rotation;

		// update the directional velocity as the object moves around the planet
		horizontalVelocity = transform.right * Mathf.Cos(angle * Mathf.Deg2Rad) * (facingRight ? 1 : -1) * moveSpeed;
		verticalVelocity = transform.up * Mathf.Sin(angle * Mathf.Deg2Rad) * moveSpeed;
		rb2d.velocity = horizontalVelocity + verticalVelocity;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Planet")
		{
			OnImpact();
		}
		else if (other.tag == "Enemy" || other.tag == "Building")
		{
			other.gameObject.GetComponent<EnemyHealthManager>().UpdateHealth(-damage);
			OnImpact();
		}
	}

	public void OnImpact()
	{
		Instantiate(impactEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	// called by the player when object is created
	public void OnInit(bool facingRight, float angle)
	{
		// update direction and angle
		this.facingRight = facingRight;
		this.angle = angle;

		// reposition away from the player by 1 unit
		Vector3 horizontalPosition = transform.right * Mathf.Cos(angle * Mathf.Deg2Rad) * (facingRight ? 1 : -1);
		Vector3 verticalPosition = transform.up * Mathf.Sin(angle * Mathf.Deg2Rad);
		transform.position += horizontalPosition + verticalPosition;
	}

}
