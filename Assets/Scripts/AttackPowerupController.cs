using UnityEngine;
using System.Collections;

public class AttackPowerupController : MonoBehaviour {

	// public variables
	public GameObject deathEffect;

	// floats
	private float killTime = 20f;
	private float killTimer = 0f;

	void FixedUpdate()
	{
		killTimer += Time.deltaTime;
		if (killTimer >= killTime)
		{
			EntitySpawner.attackPowerupSpawnCounter--;
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<PlayerController>().AttackPowerupTrigger();

			Instantiate(deathEffect, transform.position, Quaternion.identity);
			EntitySpawner.attackPowerupSpawnCounter--;
			Destroy(gameObject);
		}
	}

}
