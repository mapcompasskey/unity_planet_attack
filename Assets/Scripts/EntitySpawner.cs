using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EntitySpawner : MonoBehaviour {

	public Text spawnerText;
	public LayerMask layerMask;
	public GameObject enemyObject;

	private GameObject player;
	private GameObject planet;
	private Collider2D[] results;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		planet = GameObject.FindGameObjectWithTag("Planet");
	}

	void Update()
	{
		/*
		Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, -10);
		Vector3 planetPos = new Vector3(planet.transform.position.x, planet.transform.position.y, -10);

		// draw a line from the player to the center of the planet
		float distance1 = Vector3.Distance(playerPos, planetPos);
		Vector2 v1 = -player.transform.up;
		Debug.DrawRay(playerPos, v1 * distance1, Color.cyan);

		// draw a line from the center of the planet at an angle from the player
		CircleCollider2D planetCollider2d = planet.GetComponent<CircleCollider2D>();
		float distance2 = planetCollider2d.radius * planet.transform.localScale.x;

		Quaternion q2 = Quaternion.AngleAxis(40f, planet.transform.forward * distance2);
		Vector2 v2 = q2 * -player.transform.up;
		Debug.DrawRay(planetPos, v2 * distance2, Color.cyan);

		Quaternion q3 = Quaternion.AngleAxis(-40f, planet.transform.forward * distance2);
		Vector2 v3 = q3 * -player.transform.up;
		Debug.DrawRay(planetPos, v3 * distance2, Color.cyan);

		// draw a line oppsite the player from the center of the planet
		Vector2 v4 = -player.transform.up;
		Debug.DrawRay(planetPos, v4 * distance2 / 2, Color.yellow);

		Quaternion qX;
		Vector2 vX;
		for (int i = 0; i < 360; i+=5)
		{
			// raycast out from the center of the circle collider
			qX = Quaternion.AngleAxis(i, transform.forward * distance2);
			vX = qX * -player.transform.up;
			Debug.DrawRay(v4 * distance2 / 2, vX * distance2, Color.cyan);
		}


		//if (Physics2D.OverlapCircle(planetPos, distance2+10, layerMask))
		if (Physics2D.OverlapCircle(v4 * distance2 / 2, distance2, layerMask))
		{
			spawnerText.text = "true";
		}
		else
		{
			spawnerText.text = "false";
		}
		*/

		Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, -10);
		Vector3 planetPos = new Vector3(planet.transform.position.x, planet.transform.position.y, -10);

		CircleCollider2D planetCollider2d = planet.GetComponent<CircleCollider2D>();
		float planetRadius = planetCollider2d.radius * planet.transform.localScale.x;
		float planetWidth = planetRadius * 2;

		//CircleCollider2D playerCollider2d = player.GetComponent<CircleCollider2D>();
		//float playerRadius = playerCollider2d.radius;

		// draw a line from the player to the other side of the plane
		//Vector2 v1 = -player.transform.up;
		//Debug.DrawRay(playerPos, v1 * (planetWidth + playerRadius), Color.cyan);

		// draw a line from the player to the center of the planet
		//float distance1 = Vector3.Distance(playerPos, planetPos);
		//Debug.DrawRay(playerPos, player.transform.up * -distance1, Color.cyan);

		// draw a line to the opposite side of the player from the center of planet
		Vector3 playerPos2 = player.transform.up * -planetRadius;
		Debug.DrawRay(planetPos, playerPos2, Color.yellow);

		// test if there is anything from the layer mask inside the area opposite the player
		float dist = 2;
		if (Physics2D.OverlapCircle(playerPos2, dist, layerMask))
		{
			spawnerText.text = "true";
		}
		else
		{
			spawnerText.text = "false";
			Vector3 newPos = playerPos2 + (-player.transform.up);
			Instantiate(enemyObject, newPos, Quaternion.identity);
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
