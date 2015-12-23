using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GravityBody))]
public class PlayerBombController : MonoBehaviour {

	// public variables
	public GameObject bombExplosion;
	
	// private references
	private Rigidbody2D rb2d;
	private GravityBody gravityBody;

	// booleans
	private bool facingRight = true;
	
	// float
	private float speed = 20f;
	private float moveSpeed = 10f;
	private float jumpSpeed = 15f;
	private float killTime = 2f;
	private float killTimer = 0f;
	private float timeElapsed = 0f;
	
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
			OnImpact();
		}
	}
	
	void FixedUpdate()
	{
		/*
		horizontalVelocity = transform.right * Mathf.Cos(angle * Mathf.Deg2Rad) * (facingRight ? 1 : -1) * moveSpeed;

		velocityY = transform.InverseTransformDirection(rb2d.velocity).y;
		verticalVelocity = transform.up * velocityY;
		
		// apply vertical velocity
		if (jumpButtonState)
		{
			jumpButtonState = false;
			verticalVelocity = transform.up * Mathf.Sin(angle * Mathf.Deg2Rad) * jumpSpeed;
		}

		// update the directional velocity as the object moves around the planet
		rb2d.velocity = horizontalVelocity + verticalVelocity;
		*/

		timeElapsed += Time.fixedDeltaTime;

		Vector3 horizontalVelocity = transform.right * (facingRight ? 1 : -1) * moveSpeed;
		Vector3 verticalVelocity = transform.up * (jumpSpeed + (gravityBody.gravity * gravityBody.gravityScale * timeElapsed));

		rb2d.velocity = gravityBody.rotation * (horizontalVelocity + verticalVelocity);
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Planet")
		{
			// add friction
			//moveSpeed = moveSpeed * 0.7f;

			// bounce
			if (jumpSpeed > 5f)
			{
				timeElapsed = 0f;
				jumpSpeed = jumpSpeed * 0.5f;
			}
		}
		else if (other.tag == "Enemy" || other.tag == "Building")
		{
			OnImpact();
		}
	}
	
	public void OnImpact()
	{
		Instantiate(bombExplosion, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
	
	// called by the player when this object is created
	public void OnInit(bool facingRight, float angle, Quaternion playerRotation)
	{
		// update direction and angle
		this.facingRight = facingRight;

		moveSpeed = speed * Mathf.Cos(angle * Mathf.Deg2Rad);
		jumpSpeed = speed * Mathf.Sin(angle * Mathf.Deg2Rad);

		// reposition away from the player by 1 unit
		Vector3 horizontalPosition = transform.right * Mathf.Cos(angle * Mathf.Deg2Rad) * (facingRight ? 1 : -1);
		Vector3 verticalPosition = transform.up * Mathf.Sin(angle * Mathf.Deg2Rad);
		transform.position += playerRotation * (horizontalPosition + verticalPosition);
	}

}
