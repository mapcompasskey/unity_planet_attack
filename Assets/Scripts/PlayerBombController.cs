using UnityEngine;
using System.Collections;

public class PlayerBombController : MonoBehaviour {

	// public variables
	//public GameObject impactEffect;
	public float damage = 1f;
	
	// private references
	private Rigidbody2D rb2d;
	
	// vectors
	private Vector3 horizontalVelocity = Vector3.zero;
	private Vector3 verticalVelocity = Vector3.zero;
	
	// booleans
	private bool facingRight = true;
	private bool jumpButtonState = true;
	
	// float
	private float angle = 0f;
	private float moveSpeed = 20f;
	private float jumpSpeed = 30f;
	private float velocityY = 0f;
	private float killTime = 2f;
	private float killTimer = 0f;
	
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
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
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Planet")
		{
			// add friction
			moveSpeed = moveSpeed * 0.7f;

			// bounce
			if (jumpSpeed > 5f)
			{
				jumpButtonState = true;
				jumpSpeed = jumpSpeed * 0.5f;
			}
			//OnImpact();
		}
		else if (other.tag == "Enemy" || other.tag == "Building")
		{
			other.gameObject.GetComponent<EnemyHealthManager>().UpdateHealth(-damage);
			OnImpact();
		}
	}
	
	public void OnImpact()
	{
		//Instantiate(impactEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
	
	// called by the player when this object is created
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
