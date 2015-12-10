using UnityEngine;
using System.Collections;

public class PlayerBulletTrajectory : MonoBehaviour {

	// private references
	private Rigidbody2D rb2d;
	
	// vectors
	private Vector3 horizontalVelocity = Vector3.zero;
	private Vector3 verticalVelocity = Vector3.zero;
	
	// booleans
	private bool facingRight = true;
	
	// float
	private float angle = 0f;
	private float moveSpeed = 10f;
	private float killTime = 1f;
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
		// update the directional velocity as the object moves around the planet
		horizontalVelocity = transform.right * Mathf.Cos(angle * Mathf.Deg2Rad) * (facingRight ? 1 : -1) * moveSpeed;
		verticalVelocity = transform.up * Mathf.Sin(angle * Mathf.Deg2Rad) * moveSpeed;
		rb2d.velocity = horizontalVelocity + verticalVelocity;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Planet")
		{
			Destroy(gameObject);
		}
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
