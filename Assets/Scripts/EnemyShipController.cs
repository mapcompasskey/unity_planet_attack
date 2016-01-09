using UnityEngine;
using System.Collections;

public class EnemyShipController : MonoBehaviour {

	// public references
	public GameObject enemyBullet;
	public GameObject deathEffect;
	
	// private references
	private Rigidbody2D rb2d;
	//private CircleCollider2D collider2d;
	//private Animator anim;
	private EnemyHealthManager healthManager;
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
	
	// floats
	private float moveSpeed = 6f;
	//private float jumpSpeed = 16f;
	private float horizontalAxis = 0;
	private float actionTime = 0f;
	private float actionTimer = 0f;
	private float attackDelayTime = 0.5f;
	private float attackDelayTimer = 0f;
	private float attackDistance = 10f;
	
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		//collider2d = GetComponent<CircleCollider2D>();
		//anim = GetComponent<Animator>();
		healthManager = GetComponent<EnemyHealthManager>();
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update()
	{
		CheckInputs();
		
		// update animator parameters
		//anim.SetBool("Walking", walking);

		// if the object has been killed
		if (healthManager.health <= 0)
		{
			EntitySpawner.enemyShipKillCounter++;
			EntitySpawner.enemyShipSpawnCounter--;
			Instantiate(deathEffect, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
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
        if ( ! player)
        {
            return;
        }

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
			attackDelayTimer += Time.deltaTime;
			if (attackDelayTimer >= attackDelayTime)
			{
				canAttack = true;
				
				// reset timer
				attackDelayTimer = 0;
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

}
