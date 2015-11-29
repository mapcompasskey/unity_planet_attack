using UnityEngine;
using System.Collections;

public class BuildingController : MonoBehaviour {

	// public references
	public LayerMask groundLayer;
	
	// private references
	private Rigidbody2D rb2d;
	private BoxCollider2D collider2d;
	
	// booleans
	private bool grounded = false;
	
	// integers
	private int health = 10;
	
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<BoxCollider2D>();
	}

	void Update()
	{
		CheckForGround();
	}
	
	void CheckForGround()
	{
		/*
		Quaternion q;
		Vector2 v;
		RaycastHit2D hit;
		float distance = collider2d.radius + 0.15f;
		float[] raycastRotations = new float[] {40f, 20f, 0f, -20f, -40f};
		
		// reset parameters
		grounded = false;
		
		for (int i = 0; i < raycastRotations.Length; i++)
		{
			// raycast out from the center of the circle collider
			q = Quaternion.AngleAxis(raycastRotations[i], transform.forward * distance);
			v = q * -transform.up;
			
			// draw rays on screen
			Debug.DrawRay(transform.position, v * distance, Color.red);
			
			// check for hits against the "ground layer"
			hit = Physics2D.Raycast(transform.position, v, distance, groundLayer);
			if (hit.collider)
			{
				// prevents collision happening while inside a collider
				if (hit.distance - collider2d.radius > 0)
				{
					grounded = true;
				}
			}
		}
		*/
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case "PlayerBullet":
				Destroy(other.gameObject);
				TakeDamage(10);
				break;
		}
	}
	
	void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			EntitySpawner.enemySpawnCounter--;
			Destroy(gameObject);
		}
	}

}
