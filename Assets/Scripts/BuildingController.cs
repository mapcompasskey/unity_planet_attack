using UnityEngine;
using System.Collections;

public class BuildingController : MonoBehaviour {

	// public references
	public LayerMask groundLayer;
	
	// private references
	//private Rigidbody2D rb2d;
	//private BoxCollider2D collider2d;
	private EnemyHealthManager healthManager;
	
	// integers
	private int health = 10;
	
	void Start()
	{
		//rb2d = GetComponent<Rigidbody2D>();
		//collider2d = GetComponent<BoxCollider2D>();
		healthManager = GetComponent<EnemyHealthManager>();
	}

	void Update()
	{
		// if the object has been killed
		if (healthManager.health <= 0)
		{
			EntitySpawner.buildingKillCounter++;
			EntitySpawner.buildingSpawnCounter--;
			Destroy(gameObject);
		}
	}

	/*void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "PlayerBullet")
		{
			other.gameObject.GetComponent<PlayerBulletController>().OnImpact();
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
	}*/

}
