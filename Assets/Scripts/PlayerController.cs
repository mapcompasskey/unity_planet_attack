using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// public references
	public LayerMask groundLayer;
	public PlayerBulletController playerBullet;
	public PlayerBombController playerBomb;
	public PlayerBombTrajectory playerBombTrajectory;
	public PlayerBulletTrajectory playerBulletTrajectory;
    public float health = 10f;
    public float maxHealth = 10;
    public LevelManagerController levelManager;

    // private references
    private Rigidbody2D rb2d;
	//private CircleCollider2D collider2d;
	private BoxCollider2D collider2d;
	private Animator anim;
	private PlayerBombTrajectory bombTrajectory;
    private PlayerBulletTrajectory bulletTrajectory;

	// vectors
	private Vector3 horizontalVelocity = Vector3.zero;
	private Vector3 verticalVelocity = Vector3.zero;

	// booleans
	private bool facingRight = true;
	private bool grounded = false;
	private bool canJump = true;
	private bool jumpButtonState = false;
	private bool canAttack = true;
	private bool canAttack2 = true;
	private bool attackButtonState = false;
	private bool attack2ButtonState = false;
	private bool hasAttackPowerup = false;
	private bool showBulletSimulation = true;
	private bool showBombSimulation = false;

	// boolean states
	private bool walking = false;
	private bool jumping = false;

	// floats
	private float moveSpeed = 8f;
	private float jumpSpeed = 16f;
	private float horizontalAxis = 0;
	private float velocityY = 0f;
	private float facingAngle = 0f;
	private float attackDelayTime = 0.3f;
	private float attackDelayTimer = 0f;
	private float attack2DelayTime = 1f;
	private float attack2DelayTimer = 0f;
	private float attackPowerupTime = 5f;
	private float attackPowerupTimer = 0f;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		//collider2d = GetComponent<CircleCollider2D>();
		collider2d = GetComponent<BoxCollider2D>();
		anim = GetComponent<Animator>();

		// add a bullet trajectory object
		bulletTrajectory = (PlayerBulletTrajectory)Instantiate(playerBulletTrajectory);
		bulletTrajectory.gameObject.SetActive(showBulletSimulation);

		// add a bomb trajectory object
		bombTrajectory = (PlayerBombTrajectory)Instantiate(playerBombTrajectory);
		bombTrajectory.gameObject.SetActive(showBombSimulation);
    }

	// called every frame
	// used for regular updates such as: moving non-physics objects, simple timers, recieving inputs
	// update interval time varies
	void Update()
	{
		UpdateFacingAngle();
		CheckInputs();
		CheckForGround();

		// update animator parameters
		anim.SetBool("Grounded", grounded);
		anim.SetBool("Walking", walking);
		anim.SetBool("Jumping", jumping);
		anim.SetFloat("Y Velocity", velocityY);
		anim.SetFloat("Facing Angle", facingAngle);

        // if the object has been killed
        if (health <= 0)
        {
            //Instantiate(deathEffect, transform.position, Quaternion.identity);
            levelManager.SetGameOver();
            Destroy(gameObject);
        }
    }

	// called every physics step
	// fixed update intervals are consistent
	// used for regular updates such as: adjusting physics (Rigidbody) objects
	void FixedUpdate ()
	{
		IsJumping();
		IsAttacking();
		IsAttacking2();
		IsWalking();

		// update the current velocity
		rb2d.velocity = horizontalVelocity + verticalVelocity;
	}

	void CheckInputs()
	{
		// left or right axis of controls
		horizontalAxis = Input.GetAxisRaw("Horizontal");
		
		// is jump button pressed
		jumpButtonState = (Input.GetAxisRaw("Vertical") == 1 ? true : false);

		// is attack button pressed
		attackButtonState = Input.GetMouseButton(0);

		// is attack 2 button pressed
		attack2ButtonState = Input.GetMouseButton(1);
	}

	void UpdateFacingAngle()
	{
		// get the angle from the player to the mouse
		Vector3 mouse = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
		float deg = Mathf.Atan2(mouse.y, mouse.x) * Mathf.Rad2Deg;
		
		// if facing right
		if ( ! facingRight && (deg > -90 && deg < 90))
		{
			facingRight = true;
			transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
		}
		// else, if facing left
		else if (facingRight && (deg > 90 || deg < -90))
		{
			facingRight = false;
			transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
		}

		// clamp the angle between 0 and 90 degrees
		if ( ! facingRight && deg > 0)
		{
			deg = Mathf.Abs(deg - 180f);
		}
		facingAngle = Mathf.Clamp(deg, 0f, 90f);
	}

	void IsJumping()
	{
		// get the current local y velocity
		// *transform.up returns this objects local "up" direction as a vector in world space
		// *transform.InverseTransformDirection converts the vector from world space to local space
		velocityY = transform.InverseTransformDirection(rb2d.velocity).y;
		verticalVelocity = transform.up * velocityY;

		// if grounded after jumping
		if (grounded && jumping)
		{
			jumping = false;
		}
		
		// prevents player hoping continuously if jump button is held down
		if (grounded && ! canJump && ! jumping && ! jumpButtonState)
		{
			canJump = true;
		}
		
		// reduce jumping acceleration
		if (jumping && velocityY > 0 && ! jumpButtonState)
		{
			verticalVelocity = verticalVelocity / 2;
		}
		
		// start jumping
		if (canJump && grounded && ! jumping && jumpButtonState)
		{
			canJump = false;
			jumping = true;

			// apply local vertical velocity
			verticalVelocity = transform.up * jumpSpeed;
		}
	}

	void IsAttacking()
	{
		// simulate the trajectory the bullet will travel
		if (bulletTrajectory && showBulletSimulation)
		{
            bulletTrajectory.transform.position = transform.position;
            bulletTrajectory.Simulate(facingRight, facingAngle, transform.rotation);
		}

		if (canAttack)
		{
			// fire a bullet
			if (attackButtonState)
			{
				canAttack = false;

				if ( ! showBulletSimulation)
				{
					showBulletSimulation = true;
					showBombSimulation = false;
					bulletTrajectory.gameObject.SetActive(showBulletSimulation);
					bombTrajectory.gameObject.SetActive(showBombSimulation);
				}

				// create a bullet
				PlayerBulletController bullet = (PlayerBulletController)Instantiate(playerBullet, transform.position, transform.rotation);
                bullet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
                bullet.OnInit(facingRight, facingAngle);
            }
		}
		else
		{
			// can attack faster with the attack powerup
			float delayTime = attackDelayTime * (hasAttackPowerup ? 0.5f : 1f);
			
			attackDelayTimer += Time.deltaTime;
			if (attackDelayTimer >= delayTime)
			{
				canAttack = true;
				attackDelayTimer = 0;
			}
		}

		// if picked up an attack powerup
		if (hasAttackPowerup)
		{
			attackPowerupTimer += Time.deltaTime;
			if (attackPowerupTimer >= attackPowerupTime)
			{
				attackPowerupTimer = 0f;
				hasAttackPowerup = false;
			}
		}
	}

	void IsAttacking2()
	{
		// simulate the trajectory the bomb will travel
		if (bombTrajectory && showBombSimulation)
		{
			bombTrajectory.transform.position = transform.position;
			bombTrajectory.Simulate(facingRight, facingAngle, transform.rotation);
		}

		if (canAttack2)
		{
			// throw a bomb
			if (attack2ButtonState)
			{
				canAttack2 = false;

				if ( ! showBombSimulation)
				{
					showBulletSimulation = false;
					showBombSimulation = true;
					bulletTrajectory.gameObject.SetActive(showBulletSimulation);
					bombTrajectory.gameObject.SetActive(showBombSimulation);
				}

				// create a bomb
				PlayerBombController bomb = (PlayerBombController)Instantiate(playerBomb, transform.position, Quaternion.identity);
                bomb.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
                bomb.OnInit(facingRight, facingAngle, transform.rotation);
            }
		}
		else
		{
			attack2DelayTimer += Time.deltaTime;
			if (attack2DelayTimer >= attack2DelayTime)
			{
				canAttack2 = true;
				attack2DelayTimer = 0;
			}
		}
	}

	void IsWalking()
	{
		walking = (horizontalAxis == 0 ? false : true);

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

	public void AttackPowerupTrigger()
	{
		hasAttackPowerup = true;
	}

    public void UpdateHealth(float amount)
    {
        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

}
