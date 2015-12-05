using UnityEngine;
using System.Collections;

public class BuildingController : MonoBehaviour {

	// public references
	public LayerMask groundLayer;
	public GameObject deathEffect;
	
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
			Instantiate(deathEffect, transform.position - transform.up, Quaternion.identity);
			Destroy(gameObject);
		}
	}

}
