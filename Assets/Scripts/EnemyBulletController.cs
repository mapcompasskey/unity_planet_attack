using UnityEngine;
using System.Collections;

public class EnemyBulletController : MonoBehaviour {

	// public variables
	public GameObject impactEffect;

	// private references
	private Rigidbody2D rb2d;
	
	// floats
	private float moveSpeed = 10f;
	private float killTime = 2f;
	private float killTimer = 0f;
	
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();

		// change the color of the particles
		impactEffect.GetComponent<ParticleSystem>().startColor = new Color(0, 1f, 0, 1f);
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
		Vector3 horizontalVelocity = Vector3.zero;
		Vector3 verticalVelocity = moveSpeed * -transform.up;
		
		// update the current velocity
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

	public void OnImpact()
	{
		Instantiate(impactEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

}
