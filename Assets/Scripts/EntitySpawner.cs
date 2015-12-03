using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EntitySpawner : MonoBehaviour {

	// public references
	public Text spawnerText;
	public GameObject enemyObject;
	public LayerMask enemyTriggerLayerMask;
	public GameObject buildingObject;
	public LayerMask buildingTriggerLayerMask;

	// private references
	private GameObject player;
	private GameObject planet;
	private CircleCollider2D planetCollider2d;

	// vectors
	private Vector3 playerPos = Vector3.zero;
	private Vector3 planetPos = Vector3.zero;
	private Vector3 playerPos2 = Vector3.zero;

	// integers
	private int enemySpawns = 0;
	public static int enemyKillCounter = 0;
	public static int enemySpawnCounter = 0;
	private int buildingSpawns = 0;
	public static int buildingKillCounter = 0;
	public static int buildingSpawnCounter = 0;

	public int enemySpawnsPublic = 0;
	public int buildingSpawnsPublic = 0;

	// floats
	private float planetRadius = 0f;
	private float enemySpawnRadius = 2f;
	private float buildingSpawnRadius = 4f;

	// strings
	private string strSpawnerText;
	private Collider2D[] results;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		planet = GameObject.FindGameObjectWithTag("Planet");
		enemySpawns = enemySpawnsPublic;
		buildingSpawns = buildingSpawnsPublic;
	}

	void Update()
	{
		// player and planet positions
		playerPos = new Vector3(player.transform.position.x, player.transform.position.y, -10);
		planetPos = new Vector3(planet.transform.position.x, planet.transform.position.y, -10);

		// the planet's radius
		planetCollider2d = planet.GetComponent<CircleCollider2D>();
		planetRadius = planetCollider2d.radius * planet.transform.localScale.x;

		// draw a line to the opposite side of the player from the center of planet
		playerPos2 = player.transform.up * -planetRadius;
		playerPos2 = new Vector3(playerPos2.x, playerPos2.y, -10);
		Debug.DrawLine(planetPos, playerPos2, Color.yellow, 0, false);

		strSpawnerText = "";
		strSpawnerText += " Enemies Destroyed: " + enemyKillCounter.ToString("n0");
		strSpawnerText += "\n Buildings Destroyed: " + buildingKillCounter.ToString("n0");
		
		// spawn entities
		EnemySpawner();
		BuildingSpawner();

		spawnerText.text = strSpawnerText;
	}

	void EnemySpawner()
	{
		// test if there is anything from the layer mask inside the area opposite the player
		if ( ! Physics2D.OverlapCircle(playerPos2, enemySpawnRadius, enemyTriggerLayerMask))
		{
			if (enemySpawnCounter < enemySpawns)
			{
				Vector3 newPos = playerPos2 - (player.transform.up * 0.5f);
				newPos = new Vector3(newPos.x, newPos.y, enemyObject.transform.position.z);

				Instantiate(enemyObject, newPos, Quaternion.identity);
				enemySpawnCounter++;
			}
		}

		// draw a circle where the overlap circle should be
		Quaternion q1, q2;
		Vector3 v1, v2;
		for (int i = 0; i < 360; i += 30)
		{
			q1 = Quaternion.AngleAxis(i, transform.forward * enemySpawnRadius);
			v1 = q1 * transform.up * enemySpawnRadius;
			
			q2 = Quaternion.AngleAxis(i + 30, transform.forward * enemySpawnRadius);
			v2 = q2 * transform.up * enemySpawnRadius;

			Debug.DrawLine(playerPos2 + v1, playerPos2 + v2, Color.yellow, 0, false);
		}

		/*
		// draw a radial burst of lines out from the opposite point to represent the OverlapCircle
		Quaternion qX;
		Vector2 vX;
		for (int i = 0; i < 360; i += 30)
		{
			// raycast out from the center of the circle collider
			qX = Quaternion.AngleAxis(i, transform.forward * enemySpawnRadius);
			vX = qX * player.transform.up;
			Debug.DrawRay(playerPos2, vX * enemySpawnRadius, Color.cyan);
		}
		*/

	}

	void BuildingSpawner()
	{
		// test if there is anything from the layer mask inside the area opposite the player
		if ( ! Physics2D.OverlapCircle(playerPos2, buildingSpawnRadius, buildingTriggerLayerMask))
		{
			if (buildingSpawnCounter < buildingSpawns)
			{
				Vector3 newPos = new Vector3(playerPos2.x, playerPos2.y, 5) + (-player.transform.up);
				newPos = new Vector3(newPos.x, newPos.y, buildingObject.transform.position.z);

				Instantiate(buildingObject, newPos, Quaternion.identity);
				buildingSpawnCounter++;

				// change spawn radius
				buildingSpawnRadius = Random.Range(4F, 8F);
			}
		}
		
		// draw a circle where the overlap circle should be
		Quaternion q1, q2;
		Vector3 v1, v2;
		for (int i = 0; i < 360; i += 30)
		{
			q1 = Quaternion.AngleAxis(i, transform.forward * buildingSpawnRadius);
			v1 = q1 * transform.up * buildingSpawnRadius;
			
			q2 = Quaternion.AngleAxis(i + 30, transform.forward * buildingSpawnRadius);
			v2 = q2 * transform.up * buildingSpawnRadius;
			
			Debug.DrawLine(playerPos2 + v1, playerPos2 + v2, Color.yellow, 0, false);
		}
	}

}
