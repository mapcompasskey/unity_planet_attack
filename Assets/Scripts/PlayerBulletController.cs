using UnityEngine;
using System.Collections;

public class PlayerBulletController : MonoBehaviour {

	// public variables
	public GameObject impactEffect;
	public float damage = 1f;

	// private references
	private Rigidbody2D rb2d;
	
	// booleans
	private bool facingRight = true;
	
	// floats
	private float moveSpeed = 20f;
	private float horizontalAxis = 0;
	private float killTime = 0.5f;
	private float killTimer = 0f;

	private string direction = "forward";

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();

		// destroy this object after some time has passed
		//Destroy(gameObject, killTime);
	}

	void Update()
	{
		// set horizontal input
		horizontalAxis = (facingRight ? 1 : -1);

		// increment the kill timer
		killTimer += Time.deltaTime;
		if (killTimer >= killTime)
		{
			Destroy(gameObject);
		}
	}
	
	void FixedUpdate()
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

	/*void OnCollisionEnter2D(Collision2D other)
	{
		//Instantiate(impactEffect, transform.position, Quaternion.identity);
		//Destroy(gameObject);
		OnImpact();
	}*/

	/*void OnTriggerEnter2D(Collider2D other)
	{
		Debug.LogFormat("bullet: {0}, {1}", other, other.tag);
	}*/

	/*void OnDestroy()
	{
		if (enabled)
		{
			if (impactEffect && killTimer < killTime)
			{
				Instantiate(impactEffect, transform.position, Quaternion.identity);
			}
		}
	}*/

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

	public void SetFacingRight(bool val)
	{
		facingRight = val;
	}

	public void SetDirection(string val)
	{
		direction = val;
	}

}
