using UnityEngine;
using System.Collections;

public class PlayerBombExplosionController : MonoBehaviour {

	// public variables
	public float damage = 1f;

	void Start()
	{
		Destroy(gameObject, GetComponent<ParticleSystem>().duration);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Enemy" || other.tag == "Building")
		{
			other.gameObject.GetComponent<EnemyHealthManager>().UpdateHealth(-damage);
		}
	}

}
