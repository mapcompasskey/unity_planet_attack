using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EntitySpawner : MonoBehaviour {

	// public references
	public Text spawnerText;
	public LayerMask layerMask;
	public GameObject enemyObject;

	// private references
	private GameObject player;
	private GameObject planet;

	// integers
	private int enemySpawns = 10;
	public static int enemySpawnCounter = 0;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		planet = GameObject.FindGameObjectWithTag("Planet");
	}

	void Update()
	{
		// player and planet positions
		Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, -10);
		Vector3 planetPos = new Vector3(planet.transform.position.x, planet.transform.position.y, -10);

		// the planet's radius
		CircleCollider2D planetCollider2d = planet.GetComponent<CircleCollider2D>();
		float planetRadius = planetCollider2d.radius * planet.transform.localScale.x;

		// draw a line to the opposite side of the player from the center of planet
		Vector3 playerPos2 = player.transform.up * -planetRadius;
		Debug.DrawRay(planetPos, playerPos2, Color.yellow);

		// test if there is anything from the layer mask inside the area opposite the player
		float dist = 2;
		if (Physics2D.OverlapCircle(playerPos2, dist, layerMask))
		{
			spawnerText.text = "Overlapping";
		}
		else
		{
			spawnerText.text = "No Overlap";

			if (enemySpawnCounter < enemySpawns)
			{
				Vector3 newPos = playerPos2 + (-player.transform.up);
				Instantiate(enemyObject, newPos, Quaternion.identity);
				enemySpawnCounter++;
			}
		}

		// draw a radial burst of lines out from the opposite point to represent the OverlapCircle
		Quaternion qX;
		Vector2 vX;
		for (int i = 0; i < 360; i += 30)
		{
			// raycast out from the center of the circle collider
			qX = Quaternion.AngleAxis(i, transform.forward * dist);
			vX = qX * -player.transform.up;
			Debug.DrawRay(playerPos2, vX * dist, Color.cyan);
		}
	}

}
