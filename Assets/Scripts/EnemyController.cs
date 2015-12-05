using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	// public references
	public LayerMask groundLayer;
	public GameObject impactEffect;
	
	// private references
	private Rigidbody2D rb2d;
	//private CircleCollider2D collider2d;
	private BoxCollider2D collider2d;
	private Animator anim;
	private EnemyHealthManager healthManager;

	// vectors
	private Vector3 horizontalVelocity = Vector3.zero;
	private Vector3 verticalVelocity = Vector3.zero;
	
	// booleans
	private bool facingRight = true;
	private bool grounded = false;
	
	// boolean states
	private bool walking = false;

	// integers
	private int health = 10;

	// floats
	private float moveSpeed = 4f;
	private float horizontalAxis = 0;
	private float actionTime = 0f;
	private float actionTimer = 0f;
	
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		//collider2d = GetComponent<CircleCollider2D>();
		collider2d = GetComponent<BoxCollider2D>();
		anim = GetComponent<Animator>();
		healthManager = GetComponent<EnemyHealthManager>();
	}

	void Update()
	{
		CheckInputs();
		CheckForGround();

		// update animator parameters
		anim.SetBool("Grounded", grounded);
		anim.SetBool("Walking", walking);

		// if the object has been killed
		if (healthManager.health <= 0)
		{
			EntitySpawner.enemyKillCounter++;
			EntitySpawner.enemySpawnCounter--;
			Destroy(gameObject);
		}
	}
	
	void FixedUpdate ()
	{
		IsJumping();
		IsWalking();
		
		// update the current velocity
		rb2d.velocity = horizontalVelocity + verticalVelocity;
	}

	void CheckInputs()
	{
		actionTimer += Time.deltaTime;
		if (actionTimer >= actionTime)
		{
			// reset timer
			actionTimer = 0;

			// choose random alert
			actionTime = Random.Range(2f, 5f);

			// random horizontal input
			//horizontalAxis = Random.Range(-1, 1);
			horizontalAxis = (Random.value < 0.5 ? -1 : 1);
		}
	}

	void IsJumping()
	{
		// get the current local y velocity
		// *transform.up returns this objects local "up" direction as a vector in world space
		// *transform.InverseTransformDirection converts the vector from world space to local space
		verticalVelocity = transform.up * transform.InverseTransformDirection(rb2d.velocity).y;
	}

	void IsWalking()
	{
		walking = (horizontalAxis == 0 ? false : true);
		
		// if just moved right
		if (horizontalAxis > 0 && ! facingRight)
		{
			facingRight = true;
			transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
		}
		// else, if just moved left
		else if (horizontalAxis < 0 && facingRight)
		{
			facingRight = false;
			transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
		}
		
		// update local horizontal velocity
		// *transform.right returns this objects local "right" direction as a vector in world space
		horizontalVelocity = transform.right * horizontalAxis * moveSpeed;
	}

	/*
	void CheckForGround()
	{
		Quaternion q;
		Vector2 v;
		RaycastHit2D hit;
		float distance = collider2d.radius + 0.15f;
		float[] raycastRotations = new float[] {40f, 20f, 0f, -20f, -40f};
		
		// reset parameters
		grounded = false;
		
		for (int i = 0; i < raycastRotations.Length; i++)
		{
			// raycast out from the center of the circle collider - taking into account the collider's position
			q = Quaternion.AngleAxis(raycastRotations[i], transform.forward * distance);
			v = q * -transform.up;
			
			// draw rays on screen
			//Debug.DrawRay(collider2d.bounds.center, v * distance, Color.red);
			
			// check for hits against the "ground layer"
			hit = Physics2D.Raycast(collider2d.bounds.center, v, distance, groundLayer);
			if (hit.collider)
			{
				// prevents collision happening while inside a collider
				if (hit.distance - collider2d.radius > 0)
				{
					grounded = true;
				}
			}
		}
	}
	*/

	void CheckForGround()
	{
		// raycasting variables
		RaycastHit2D hit;
		int rays = 5;
		float skinWidth = 0.05f;
		float distance = 0.15f;
		float spacing = (collider2d.size.x - skinWidth * 2) / (rays - 1);
		Vector3 offsetX = Vector3.zero;
		Vector3 offsetY = Vector3.zero;
		Vector3 start = Vector3.zero;
		
		// reset parameters
		grounded = false;
		
		for (int i = 0; i < rays; i++)
		{
			// rayout out across the bottom of the collider
			offsetX = collider2d.transform.right * ((collider2d.size.x / 2) - collider2d.offset.x - skinWidth - (spacing * i));
			offsetY = collider2d.transform.up    * ((collider2d.size.y / 2) - collider2d.offset.y - skinWidth);
			start = collider2d.transform.position - (offsetX + offsetY);
			
			// draw rays on screen
			Debug.DrawRay(start, -collider2d.transform.up * distance, Color.red);
			
			// check for hits against the "ground layer"
			hit = Physics2D.Raycast(start, -collider2d.transform.up, distance, groundLayer);
			if (hit.collider)
			{
				grounded = true;
			}
		}
	}
	
	/*void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "PlayerBullet")
		{
			other.gameObject.GetComponent<PlayerBulletController>().OnImpact();
			TakeDamage(10);
		}
	}*/

	/*void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			EntitySpawner.enemyKillCounter++;
			EntitySpawner.enemySpawnCounter--;
			Destroy(gameObject);
		}
	}*/

}
