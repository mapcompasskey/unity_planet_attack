using UnityEngine;
using System.Collections;

public class PlayerBulletController : MonoBehaviour {

	// private references
	private Rigidbody2D rb2d;
	
	// booleans
	private bool facingRight = true;
	
	// floats
	private float moveSpeed = 20f;
	private float horizontalAxis = 0;
	private float killTime = 0.5f;

	private string direction = "forward";

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();

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
		Vector3 horizontalVelocity = Vector3.zero;
		Vector3 verticalVelocity = Vector3.zero;
		
		if (direction == "forward")
		{
			horizontalVelocity = transform.right * horizontalAxis * moveSpeed;
		}
		else if (direction == "up")
		{
			verticalVelocity = transform.up * moveSpeed;
		}
		
		// update the current velocity
		rb2d.velocity = horizontalVelocity + verticalVelocity;
	}

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
