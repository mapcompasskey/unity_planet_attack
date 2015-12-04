using UnityEngine;
using System.Collections;

public class BuildingController : MonoBehaviour {

	// public references
	public LayerMask groundLayer;
	
	// private references
	//private Rigidbody2D rb2d;
	//private BoxCollider2D collider2d;
	
	// booleans
	private bool grounded = false;
	
	// integers
	private int health = 10;
	
	void Start()
	{
		//rb2d = GetComponent<Rigidbody2D>();
		//collider2d = GetComponent<BoxCollider2D>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "PlayerBullet")
		{
			Destroy(other.gameObject);
			TakeDamage(4);
		}
	}
	
	void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			EntitySpawner.buildingKillCounter++;
			EntitySpawner.buildingSpawnCounter--;
			Destroy(gameObject);
		}
	}

}
