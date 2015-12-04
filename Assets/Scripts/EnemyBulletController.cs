using UnityEngine;
using System.Collections;

public class EnemyBulletController : MonoBehaviour {

	// private references
	private Rigidbody2D rb2d;
	
	// floats
	private float moveSpeed = 10f;
	private float killTime = 0.5f;
	
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();

		// destroy this object after some time has passed
		Destroy(gameObject, killTime);
	}
	
	void FixedUpdate()
	{
		Vector3 horizontalVelocity = Vector3.zero;
		Vector3 verticalVelocity = moveSpeed * -transform.up;
		
		// update the current velocity
		rb2d.velocity = horizontalVelocity + verticalVelocity;
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		Destroy(gameObject);
	}

}
