using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// public references
	public LayerMask groundLayer;
	public PlayerBulletController playerBullet;
	
	// private references
	private Rigidbody2D rb2d;
	//private CircleCollider2D collider2d;
	private BoxCollider2D collider2d;
	private Animator anim;

	// vectors
	private Vector3 horizontalVelocity = Vector3.zero;
	private Vector3 verticalVelocity = Vector3.zero;

	// booleans
	private bool facingRight = true;
	private bool grounded = false;
	private bool canJump = true;
	private bool jumpButtonState = false;
	private bool canAttack = true;
	private bool attackButtonState = false;
	private bool hasAttackPowerup = false;

	// boolean states
	private bool walking = false;
	private bool jumping = false;

	// floats
	private float moveSpeed = 8f;
	private float jumpSpeed = 16f;
	private float horizontalAxis = 0;
	private float velocityY = 0f;
	private float anglePointing = 0f;
	private float attackDelayTime = 0.3f;
	private float attackDelayTimer = 0f;
	private float attackPowerupTime = 5f;
	private float attackPowerupTimer = 0f;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		//collider2d = GetComponent<CircleCollider2D>();
		collider2d = GetComponent<BoxCollider2D>();
		anim = GetComponent<Animator>();
	}

	// called every frame
	// used for regular updates such as: moving non-physics objects, simple timers, recieving inputs
	// update interval time varies
	void Update()
	{
		CheckInputs();
		CheckForGround();

		// update animator parameters
		anim.SetBool("Grounded", grounded);
		anim.SetBool("Walking", walking);
		anim.SetBool("Jumping", jumping);
		anim.SetFloat("Y Velocity", velocityY);
		anim.SetFloat("Angle Pointing", anglePointing);
	}

	// called every physics step
	// fixed update intervals are consistent
	// used for regular updates such as: adjusting physics (Rigidbody) objects
	void FixedUpdate ()
	{
		IsJumping();
		IsAttacking();
		IsWalking();
		
		// update the current velocity
		rb2d.velocity = horizontalVelocity + verticalVelocity;
	}

	void CheckInputs()
	{
		// left or right axis of controls
		horizontalAxis = Input.GetAxisRaw("Horizontal");
		
		// is jump button pressed
		//jumpButtonState = Input.GetButton("Jump");
		jumpButtonState = Input.GetKey(KeyCode.X);

		// is attack button pressed
		//attackButtonState = Input.GetKey(KeyCode.Z);
		attackButtonState = Input.GetMouseButton(0);

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

		if ( ! facingRight && deg > 0)
		{
			deg = Mathf.Abs(deg - 180f);
		}
		anglePointing = Mathf.Clamp(deg, 0f, 90f);
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
		if (canAttack)
		{
			// fire a bullet
			if (attackButtonState)
			{
				canAttack = false;

				/*
				Vector3 bulletPos = transform.position + (transform.right * (facingRight ? 1 : -1));
				string direction = "forward";
				
				if (Input.GetKey(KeyCode.UpArrow))
				{
					bulletPos = transform.position + transform.up;
					direction = "up";
				}
				
				GameObject bullet = GameObject.Instantiate(playerBullet, bulletPos, transform.rotation) as GameObject;
				bullet.GetComponent<PlayerBulletController>().SetFacingRight(facingRight);
				bullet.GetComponent<PlayerBulletController>().SetDirection(direction);
				*/
				/*
				Vector3 bulletPos = transform.position;
				GameObject bullet = GameObject.Instantiate(playerBullet, bulletPos, transform.rotation) as GameObject;
				bullet.GetComponent<PlayerBulletController>().SetFacingRight(facingRight);
				bullet.GetComponent<PlayerBulletController>().SetAngle(anglePointing);
				*/

				PlayerBulletController bullet = (PlayerBulletController)Instantiate(playerBullet, transform.position, transform.rotation);
				bullet.OnInit(facingRight, anglePointing);
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

	void IsWalking()
	{
		walking = (horizontalAxis == 0 ? false : true);

		/*
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
		*/

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

}
