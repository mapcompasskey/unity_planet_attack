using UnityEngine;
using System.Collections;

public class PlayerBulletController : MonoBehaviour {

	// private references
	private Rigidbody2D rb2d;
	
	// vectors
	private Vector3 horizontalVelocity = Vector3.zero;
	private Vector3 verticalVelocity = Vector3.zero;
	
	// booleans
	private bool facingRight = true;
	
	// floats
	private float moveSpeed = 20f;
	private float jumpSpeed = 0f;
	private float horizontalAxis = 0;
	private float maxVelocityX = 1f;
	private float maxVelocityY = 1f;
	private float killTime = 0.5f;

	private string direction = "forward";

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		
		// update max velocities
		maxVelocityX = moveSpeed;
		maxVelocityY = jumpSpeed;

		// destroy this object after some time has passed
		Destroy(gameObject, killTime);
	}

	void Update()
	{
		// set horizontal input
		horizontalAxis = (facingRight ? 1 : -1);
	}
	
	void FixedUpdate ()
	{
		IsMoving();
		
		// update the current velocity
		rb2d.velocity = horizontalVelocity + verticalVelocity;
	}

	void IsMoving()
	{
		if (direction == "forward")
		{
			horizontalVelocity = transform.right * horizontalAxis * moveSpeed;
			verticalVelocity = Vector3.zero;
		}
		else if (direction == "up")
		{
			horizontalVelocity = Vector3.zero;
			verticalVelocity = transform.up * moveSpeed;
		}
	}

	/*
	void IsMoving()
	{
		// if just moved right
		if (horizontalAxis > 0 && ! facingRight)
		{
			facingRight = true;
			transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
		}
		// else, if just moved left
		else if (horizontalAxis < 0 && facingRight)
		{
			facingRight = false;
			transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
		}
		
		// update local horizontal velocity
		// *transform.right returns this objects local "right" direction as a vector in world space
		horizontalVelocity = transform.right * horizontalAxis * moveSpeed;
	}
	*/
	
	void OnCollisionEnter2D(Collision2D other)
	{
		Destroy(gameObject);
	}

	/*void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Enemy")
		{
			Destroy(gameObject);
		}
	}*/

	public void SetFacingRight(bool val)
	{
		facingRight = val;
	}

	public void SetDirection(string val)
	{
		direction = val;
	}

}
