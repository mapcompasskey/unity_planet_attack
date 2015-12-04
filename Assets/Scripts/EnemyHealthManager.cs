using UnityEngine;
using System.Collections;

public class EnemyHealthManager : MonoBehaviour {

	// public variables
	public float health = 10;
	public float maxHealth = 10;

	public void UpdateHealth(float amount)
	{
		health += amount;

		if (health > maxHealth)
		{
			health = maxHealth;
		}
	}
	
}
