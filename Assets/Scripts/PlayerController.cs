using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// public references
	public LayerMask groundLayer;
	public PlayerBulletController playerBullet;
	public PlayerBulletTrajectory playerBulletTrajectory;
	public GameObject bulletTrajectory;
	
	// private references
	private Rigidbody2D rb2d;
	//private CircleCollider2D collider2d;
	private BoxCollider2D collider2d;
	private Animator anim;
	private GameObject[] trajectory;

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
	private float facingAngle = 0f;
	private float attackDelayTime = 0.3f;
	private float attackDelayTimer = 0f;
	private float attackPowerupTime = 5f;
	private float attackPowerupTimer = 0f;
	private float bulletTrajectoryDelayTime = 0.1f;
	private float bulletTrajectoryDelayTimer = 0f;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		//collider2d = GetComponent<CircleCollider2D>();
		collider2d = GetComponent<BoxCollider2D>();
		anim = GetComponent<Animator>();

		/**/
		// create 20 instances of the bullet trajectory object
		trajectory = new GameObject[20];
		for (int i = 0; i < 20; i++)
		{
			GameObject obj = Instantiate(bulletTrajectory) as GameObject;
			trajectory[i] = obj.gameObject;
		}
		/**/
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
		jumpButtonState = (Input.GetAxisRaw("Vertical") == 1 ? true : false);

		// is attack button pressed
		attackButtonState = Input.GetMouseButton(0);
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
		/**/
		// draw the trajectory the bullet will take
		bulletTrajectoryDelayTimer += Time.deltaTime;
		if (bulletTrajectoryDelayTimer >= bulletTrajectoryDelayTime)
		{
			PlayerBulletTrajectory bulletTrajectory = (PlayerBulletTrajectory)Instantiate(playerBulletTrajectory, transform.position, transform.rotation);
			bulletTrajectory.OnInit(facingRight, facingAngle);
			bulletTrajectoryDelayTimer = 0;
		}
		/**/

		/** /
		Vector3 hPos = Vector3.zero;
		Vector3 vPos = Vector3.zero;
		for (int i = 0; i < trajectory.Length; i++)
		{
			hPos = transform.right * Mathf.Cos(facingAngle * Mathf.Deg2Rad) * (i * 0.5f) * (facingRight ? 1f : -1f);
			vPos = transform.up * Mathf.Sin(facingAngle * Mathf.Deg2Rad) * (i * 0.5f);
			trajectory[i].transform.position = transform.position + hPos + vPos;

			//Vector2 targetDir = (body.position - transform.position).normalized;
			//float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
			
			//body.rotation = Quaternion.AngleAxis(angle, body.forward);
			//rb2d.AddForce(targetDir * gravity * rb2d.mass * gravityScale); <- dont need for bullets since they have no gravity

			//transform.position Vector3.zero
			//gravity -50
			
			Vector3 targetDir = (trajectory[i].transform.position - Vector3.zero).normalized;
			float theta = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;

			trajectory[i].transform.rotation = Quaternion.AngleAxis(theta, trajectory[i].transform.forward);
			//Debug.Log(targetDir * -50 * Time.fixedDeltaTime);
			//trajectory[i].transform.position += targetDir * -50 * Time.fixedDeltaTime;

			hPos = trajectory[i].transform.right * Mathf.Cos(theta * Mathf.Deg2Rad) * (facingRight ? 1f : -1f);
			vPos = trajectory[i].transform.up * Mathf.Sin(theta * Mathf.Deg2Rad);
			trajectory[i].transform.position += hPos + vPos;
		}
		/**/

		/*
		var theta = -this.gun.rotation;
		var x = 0, y = 0;
		for(var t = 0 + this.timeOffset/(1000*MARCH_SPEED/60); t < 3; t += 0.03) {
			x = this.BULLET_SPEED * t * Math.cos(theta) * correctionFactor;
			y = this.BULLET_SPEED * t * Math.sin(theta) * correctionFactor - 0.5 * this.GRAVITY * t * t;
			this.bitmap.context.fillRect(x + this.gun.x, this.gun.y - y, 3, 3);
			if (y < -15) break;
		}
		*/

		/**/
		// this draws the trajectory but doesn't account for the planet's curvature
		float xPos = 0;
		float yPos = 0;
		float time = Time.deltaTime;
		float theta = facingAngle * Mathf.Deg2Rad;
		for (int i = 0; i < trajectory.Length; i++)
		{
			float t = i * time;
			xPos = 30f * t * Mathf.Cos(theta) * (facingRight ? 1f : -1f);
			yPos = 30f * t * Mathf.Sin(theta) - 0.5f * 50f * t * t;
			trajectory[i].transform.position = transform.position + new Vector3(xPos, yPos, 0);
		}
		/**/

		/*
		Vector2 targetDir = (body.position - transform.position).normalized;
		float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
		Rigidbody2D rb2d = body.GetComponent<Rigidbody2D>();
		float gravityScale = body.GetComponent<GravityBody>().gravityScale;

		body.rotation = Quaternion.AngleAxis(angle, body.forward);
		rb2d.AddForce(targetDir * gravity * rb2d.mass * gravityScale);
		*/

		/*
		float angle;
		Vector3 targetDir;
		Vector3 targetRight = transform.right;
		Vector3 targetForward = transform.forward;
		Vector3 targetPos = transform.position;
		for (int i = 0; i < trajectory.Length; i++)
		{
			targetDir = (targetPos - Vector3.zero).normalized;
			angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;

			trajectory[i].transform.rotation = Quaternion.AngleAxis(angle, targetForward);
			trajectory[i].transform.position = targetPos + targetRight + targetDir;

			targetRight = trajectory[i].transform.right;
			targetForward = trajectory[i].transform.forward;
			targetPos = trajectory[i].transform.position;
		}
		*/


		if (canAttack)
		{
			// fire a bullet
			if (attackButtonState)
			{
				canAttack = false;

				// create a bullet
				PlayerBulletController bullet = (PlayerBulletController)Instantiate(playerBullet, transform.position, transform.rotation);
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

}
