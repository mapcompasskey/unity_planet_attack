using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	// public references
	public LayerMask groundLayer;
	
	// private references
	private Rigidbody2D rb2d;
	private CircleCollider2D collider2d;
	
	// vectors
	private Vector3 horizontalVelocity = Vector3.zero;
	private Vector3 verticalVelocity = Vector3.zero;
	
	// booleans
	private bool facingRight = true;
	private bool grounded = false;
	private bool jumpButtonState = false;
	
	// boolean states
	private bool walking = false; // is walking
	private bool jumping = false; // is jumping

	// integers
	private int health = 10;

	// floats
	private float moveSpeed = 4f;
	private float jumpSpeed = 16f;
	private float horizontalAxis = 0;
	private float maxVelocityX = 1f;
	private float maxVelocityY = 1f;
	private float actionTime = 0f;
	private float actionTimer = 0f;
	
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<CircleCollider2D>();
		
		// update max velocities
		maxVelocityX = moveSpeed * 2;
		maxVelocityY = jumpSpeed * 2;
	}
	
	// called every frame
	// used for regular updates such as: moving non-physics objects, simple timers, recieving inputs
	// update interval time varies
	void Update()
	{
		CheckInputs();
		CheckForGround();
	}

	// called every physics step
	// fixed update intervals are consistent
	// used for regular updates such as: adjusting physics (Rigidbody) objects
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
		
		// is jump button down
		jumpButtonState = false;
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
			// raycast out from the center of the circle collider
			q = Quaternion.AngleAxis(raycastRotations[i], transform.forward * distance);
			v = q * -transform.up;
			
			// draw rays on screen
			Debug.DrawRay(transform.position, v * distance, Color.red);
			
			// check for hits against the "ground layer"
			hit = Physics2D.Raycast(transform.position, v, distance, groundLayer);
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
	
	void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case "Player":
				//Debug.LogFormat("gameObject: {0}, other name: {1}, other tag: {2}", gameObject.name, other.name, other.tag);
				break;

			case "PlayerBullet":
				Destroy(other.gameObject);
				TakeDamage(10);
				break;
		}
	}

	void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			EntitySpawner.enemySpawnCounter--;
			Destroy(gameObject);
		}
	}

}
