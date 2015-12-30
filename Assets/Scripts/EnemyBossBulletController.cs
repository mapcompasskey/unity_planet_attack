using UnityEngine;
using System.Collections;

public class EnemyBossBulletController : MonoBehaviour {
	
	// public variables
	public GameObject impactEffect;
	
	// private references
	private Rigidbody2D rb2d;

	// booleans
	private bool facingRight = true;
	
	// float
	private float angle = 0f;
	private float moveSpeed = 7f;
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
		//angle += 1f;

		// update the directional velocity as the object moves around the planet
		Vector3 horizontalVelocity = transform.right * Mathf.Cos(angle * Mathf.Deg2Rad) * (facingRight ? 1 : -1) * moveSpeed;
		Vector3 verticalVelocity = transform.up * Mathf.Sin(angle * Mathf.Deg2Rad) * moveSpeed;
		rb2d.velocity = horizontalVelocity + verticalVelocity;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Planet")
		{
			OnImpact();
		}
		else if (other.tag == "Player")
		{
			OnImpact();
		}
	}
	
	void OnImpact()
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
		Vector3 horizontalPosition = (transform.right * 1.5f) * Mathf.Cos(angle * Mathf.Deg2Rad) * (facingRight ? 1 : -1);
		Vector3 verticalPosition = (transform.up * 1.5f) * Mathf.Sin(angle * Mathf.Deg2Rad);
		transform.position += horizontalPosition + verticalPosition;

        transform.rotation = Quaternion.identity;
	}
	
}
