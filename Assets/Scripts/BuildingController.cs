using UnityEngine;
using System.Collections;

public class BuildingController : MonoBehaviour {

	// public references
	public LayerMask groundLayer;
	public GameObject buildingBlip;
	
	// private references
	//private Rigidbody2D rb2d;
	//private BoxCollider2D collider2d;
	private GameObject minimap;
	private GameObject blip;
	
	// booleans
	private bool grounded = false;
	
	// integers
	private int health = 10;
	
	void Start()
	{
		//rb2d = GetComponent<Rigidbody2D>();
		//collider2d = GetComponent<BoxCollider2D>();

		// add a blip to the minimap
		minimap = GameObject.Find("Minimap").gameObject;
		if (minimap)
		{
			blip = Instantiate(buildingBlip, Vector3.zero, Quaternion.identity) as GameObject;
			blip.transform.SetParent(minimap.transform, false);
		}
	}

	void FixedUpdate()
	{
		// update blip in minimap
		if (blip)
		{
			blip.transform.localPosition = transform.position;
			blip.transform.rotation = transform.rotation;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case "PlayerBullet":
				Destroy(other.gameObject);
				TakeDamage(4);
				break;
		}
	}
	
	void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			EntitySpawner.buildingKillCounter++;
			EntitySpawner.buildingSpawnCounter--;
			Destroy(blip);
			Destroy(gameObject);
		}
	}

}
