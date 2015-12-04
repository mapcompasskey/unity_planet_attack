using UnityEngine;
using System.Collections;

public class EnemyShipController : MonoBehaviour {

	// public references
	public LayerMask groundLayer;
	public GameObject enemyBullet;
	
	// private references
	private Rigidbody2D rb2d;
	//private CircleCollider2D collider2d;
	//private Animator anim;
	private GameObject player;
	
	// vectors
	private Vector3 horizontalVelocity = Vector3.zero;
	private Vector3 verticalVelocity = Vector3.zero;
	
	// booleans
	private bool facingRight = true;
	//private bool jumpButtonState = false;
	private bool canAttack = true;
	
	// boolean states
	//private bool walking = false;
	//private bool jumping = false;
	
	// integers
	private int health = 10;
	
	// floats
	private float moveSpeed = 4f;
	//private float jumpSpeed = 16f;
	private float horizontalAxis = 0;
	private float actionTime = 0f;
	private float actionTimer = 0f;
	private float attackTime = 0.75f;
	private float attackTimer = 0f;
	private float attackDistance = 7f;
	
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		//collider2d = GetComponent<CircleCollider2D>();
		//anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update()
	{
		CheckInputs();
		
		// update animator parameters
		//anim.SetBool("Walking", walking);
	}
	
	void FixedUpdate ()
	{
		IsAttacking();
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
		//jumpButtonState = false;
	}

	void IsAttacking()
	{
		float distance = Vector3.Distance(transform.position, player.transform.position);
		if (canAttack && distance < attackDistance)
		{
			canAttack = false;
			
			// fire a bullet
			Vector3 bulletPos = transform.position - transform.up;
			GameObject.Instantiate(enemyBullet, bulletPos, Quaternion.identity);
		}

		if ( ! canAttack)
		{
			attackTimer += Time.deltaTime;
			if (attackTimer >= attackTime)
			{
				canAttack = true;
				
				// reset timer
				attackTimer = 0;
			}
		}
	}

	void IsJumping()
	{
		// get the current local y velocity
		// *transform.up returns this objects local "up" direction as a vector in world space
		// *transform.InverseTransformDirection converts the vector from world space to local space
		//verticalVelocity = transform.up * transform.InverseTransformDirection(rb2d.velocity).y;
	}
	
	void IsWalking()
	{
		//walking = (horizontalAxis == 0 ? false : true);
		
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
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "PlayerBullet")
		{
			Destroy(other.gameObject);
			TakeDamage(5);
		}
	}
	
	void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			EntitySpawner.enemyShipKillCounter++;
			EntitySpawner.enemyShipSpawnCounter--;
			Destroy(gameObject);
		}
	}

}
