using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EntitySpawner : MonoBehaviour {

	// public variables
	public Text spawnerText;
	public GameObject attackPowerup;
    public GameObject bossObject;

    public int enemySpawnsPublic = 0;
    public int enemySpawnLimit = 20;
    public GameObject enemyObject;
	public LayerMask enemyTriggerLayerMask;

	public int enemyShipSpawnsPublic = 0;
    public int enemyShipSpawnLimit = 20;
    public GameObject enemyShipObject;
	public LayerMask enemyShipTriggerLayerMask;

	public int buildingSpawnsPublic = 0;
    public int buildingSpawnLimit = 10;
    public GameObject buildingObject;
	public LayerMask buildingTriggerLayerMask;

	// private references
	private GameObject player;
	private GameObject planet;
	private CircleCollider2D planetCollider2d;

	// vectors
	private Vector3 planetPosition = Vector3.zero;
	private Vector3 groundPosition = Vector3.zero;
	private Vector3 airPosition = Vector3.zero;

    // bools
    private bool bossSpawned = false;
    public static bool bossKilled = false;

	// integers
	private int enemySpawns = 0;
	public static int enemyKillCounter = 0;
    public static int enemySpawnCounter = 0;
    private int enemySpawnTotalCounter = 0;

	private int enemyShipSpawns = 0;
	public static int enemyShipKillCounter = 0;
	public static int enemyShipSpawnCounter = 0;
    private int enemyShipSpawnTotalCounter = 0;

    private int buildingSpawns = 0;
	public static int buildingKillCounter = 0;
	public static int buildingSpawnCounter = 0;
    private int buildingSpawnTotalCounter = 0;

    private int attackPowerupSpawns = 3;
	public static int attackPowerupSpawnCounter = 0;

	// floats
	private float planetRadius = 0f;
	private float enemySpawnRadius = 2f;
	private float enemyShipSpawnRadius = 4f;
	private float buildingSpawnRadius = 4f;
	private float attackPowerupTime = 5f;
	private float attackPowerupTimer = 6f;

	// strings
	private string strSpawnerText;
	private Collider2D[] results;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		planet = GameObject.FindGameObjectWithTag("Planet");

		// the planet's radius
		planetCollider2d = planet.GetComponent<CircleCollider2D>();
		planetRadius = planetCollider2d.radius * planet.transform.localScale.x;

		enemySpawns = enemySpawnsPublic;
		enemyShipSpawns = enemyShipSpawnsPublic;
		buildingSpawns = buildingSpawnsPublic;
	}

	void Update()
	{
        if ( ! player)
        {
            return;
        }

		// planet positions
		planetPosition = new Vector3(planet.transform.position.x, planet.transform.position.y, -10);

		// the planet's radius
		//planetCollider2d = planet.GetComponent<CircleCollider2D>();
		//planetRadius = planetCollider2d.radius * planet.transform.localScale.x;

		// draw a line to the opposite side of the player from the center of planet
		groundPosition = player.transform.up * -planetRadius;
		groundPosition = new Vector3(groundPosition.x, groundPosition.y, -10);
		Debug.DrawLine(planetPosition, groundPosition, Color.yellow, 0, false);

		airPosition = player.transform.up * -planetRadius * 1.5f;
		airPosition = new Vector3(airPosition.x, airPosition.y, -10);
		//Debug.DrawLine(planetPosition, groundPosition, Color.magenta, 0, false);

		strSpawnerText = "";
        strSpawnerText += " Enemies Destroyed: " + enemyKillCounter.ToString("n0") + " / " + enemySpawnLimit.ToString("n0");
		strSpawnerText += "\n Enemy Ships Destroyed: " + enemyShipKillCounter.ToString("n0") + " / " + enemyShipSpawnLimit.ToString("n0");
        strSpawnerText += "\n Buildings Destroyed: " + buildingKillCounter.ToString("n0") + " / " + buildingSpawnLimit.ToString("n0");

        // spawn entities
        EnemySpawner();
		EnemyShipSpawner();
		BuildingSpawner();
        BossSpawner();
        Restart();

        spawnerText.text = strSpawnerText;
	}

	void FixedUpdate()
	{
		// spawn an attack powerup
		AttackPowerupSpawner();
	}

	void EnemySpawner()
	{
        if (enemySpawnTotalCounter >= enemySpawnLimit)
        {
            return;
        }

		// test if there is anything from the layer mask inside the area opposite the player
		if ( ! Physics2D.OverlapCircle(groundPosition, enemySpawnRadius, enemyTriggerLayerMask))
		{
			if (enemySpawnCounter < enemySpawns)
			{
				Vector3 newPos = groundPosition - (player.transform.up * 0.5f);
				newPos = new Vector3(newPos.x, newPos.y, enemyObject.transform.position.z);

				Instantiate(enemyObject, newPos, Quaternion.identity);
				enemySpawnCounter++;
                enemySpawnTotalCounter++;
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

			Debug.DrawLine(groundPosition + v1, groundPosition + v2, Color.yellow, 0, false);
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
			Debug.DrawRay(groundPosition, vX * enemySpawnRadius, Color.cyan);
		}
		*/

	}

	void EnemyShipSpawner()
	{
        if (enemyShipSpawnTotalCounter >= enemyShipSpawnLimit)
        {
            return;
        }

        // test if there is anything from the layer mask inside the area opposite the player
        if ( ! Physics2D.OverlapCircle(airPosition, enemyShipSpawnRadius, enemyShipTriggerLayerMask))
		{
			if (enemyShipSpawnCounter < enemyShipSpawns)
			{
				Vector3 newPos = airPosition - player.transform.up * Random.Range(-2, 2);
				newPos = new Vector3(newPos.x, newPos.y, enemyShipObject.transform.position.z);
				
				Instantiate(enemyShipObject, newPos, Quaternion.identity);
				enemyShipSpawnCounter++;
                enemyShipSpawnTotalCounter++;
			}
		}
		
		// draw a circle where the overlap circle should be
		Quaternion q1, q2;
		Vector3 v1, v2;
		for (int i = 0; i < 360; i += 30)
		{
			q1 = Quaternion.AngleAxis(i, transform.forward * enemyShipSpawnRadius);
			v1 = q1 * transform.up * enemyShipSpawnRadius;
			
			q2 = Quaternion.AngleAxis(i + 30, transform.forward * enemyShipSpawnRadius);
			v2 = q2 * transform.up * enemyShipSpawnRadius;
			
			Debug.DrawLine(airPosition + v1, airPosition + v2, Color.magenta, 0, false);
		}
		
	}

	void BuildingSpawner()
	{
        if (buildingSpawnTotalCounter >= buildingSpawnLimit)
        {
            return;
        }

        // test if there is anything from the layer mask inside the area opposite the player
        if ( ! Physics2D.OverlapCircle(groundPosition, buildingSpawnRadius, buildingTriggerLayerMask))
		{
			if (buildingSpawnCounter < buildingSpawns)
			{
				Vector3 newPos = new Vector3(groundPosition.x, groundPosition.y, 5) - player.transform.up * 2f;
				newPos = new Vector3(newPos.x, newPos.y, buildingObject.transform.position.z);

				Instantiate(buildingObject, newPos, Quaternion.identity);
				buildingSpawnCounter++;
                buildingSpawnTotalCounter++;

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
			
			Debug.DrawLine(groundPosition + v1, groundPosition + v2, Color.cyan, 0, false);
		}
	}

    void BossSpawner()
    {
        if (bossSpawned)
        {
            return;
        }
        
        if (enemyKillCounter < enemySpawnLimit)
        {
            return;
        }

        if (enemyShipKillCounter < enemyShipSpawnLimit)
        {
            return;
        }

        if (buildingKillCounter < buildingSpawnLimit)
        {
            return;
        }
        
        bossSpawned = true;

        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, 5) - player.transform.up * 2f;
        newPos = new Vector3(newPos.x, newPos.y, bossObject.transform.position.z);

        Instantiate(bossObject, newPos, Quaternion.identity);
    }

    void AttackPowerupSpawner()
	{
		// spawn an attack powerup
		if (attackPowerupSpawnCounter < attackPowerupSpawns)
		{
			attackPowerupTimer += Time.deltaTime;
			if (attackPowerupTimer >= attackPowerupTime)
			{
				// randomly drop from anywhere above the planet
				float distance = (planetRadius * 2f);
				Vector3 v = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward) * Vector3.up * distance;
				GameObject.Instantiate(attackPowerup, v, Quaternion.identity);
				
				attackPowerupTimer = 0f;
				attackPowerupSpawnCounter++;
			}
		}
	}

    void Restart()
    {
        if ( ! bossKilled)
        {
            return;
        }
        Debug.Log("restart");
        bossKilled = false;

        enemyKillCounter = 0;
        enemySpawnTotalCounter = 0;

        enemyShipKillCounter = 0;
        enemyShipSpawnTotalCounter = 0;

        buildingKillCounter = 0;
        buildingSpawnTotalCounter = 0;

        bossSpawned = false;
    }

}
