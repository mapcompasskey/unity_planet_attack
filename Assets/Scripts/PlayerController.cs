using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// public references
	public LayerMask groundLayer;
	public GameObject playerBullet;
	//public PhysicsMaterial2D materialDefault;
	//public PhysicsMaterial2D materialNoFriction;
	
	// private references
	private Rigidbody2D rb2d;
	private CircleCollider2D collider2d;
	private Animator anim;

	// vectors
	private Vector3 horizontalVelocity = Vector3.zero;
	private Vector3 verticalVelocity = Vector3.zero;

	// booleans
	private bool facingRight = true;
	private bool grounded = false;
	private bool canJump = true; // can jump
	private bool canSlowJump = false; // can reduce jump acceleration
	private bool jumpButtonState = false; // is jump button up or down
	//private bool noFriction = false;

	// boolean states
	private bool walking = false; // is walking
	private bool jumping = false; // is jumping

	// floats
	private float moveSpeed = 8f;
	private float jumpSpeed = 16f;
	private float horizontalAxis = 0;
	private float maxVelocityX = 1f;
	private float maxVelocityY = 1f;
	private float velocityY = 0f;

	//private Vector3 moveDirection;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<CircleCollider2D>();
		anim = GetComponent<Animator>();
		
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

		// update animator parameters
		anim.SetBool("Grounded", grounded);
		anim.SetBool("Walking", walking);
		anim.SetBool("Jumping", jumping);
		anim.SetFloat("Y Velocity", velocityY);
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
		// left or right axis of controls
		horizontalAxis = Input.GetAxisRaw("Horizontal");
		
		// is jump button down
		//jumpButtonState = Input.GetButton("Jump");
		jumpButtonState = Input.GetKey(KeyCode.X);

		// fire a bullet
		if (Input.GetKeyDown(KeyCode.Z))
		{
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
		}
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
			canSlowJump = false;
		}
		
		// prevents player hoping continuously if jump button is held down
		if (grounded && ! canJump && ! jumping && ! jumpButtonState)
		{
			canJump = true;
		}
		
		// reduce jumping acceleration
		if (canSlowJump && jumping && ! jumpButtonState)
		{
			canSlowJump = false;
			if (verticalVelocity.y > 0)
			{
				verticalVelocity = verticalVelocity / 2;
			}
		}
		
		// start jumping
		if (canJump && grounded && ! jumping && jumpButtonState)
		{
			canJump = false;
			jumping = true;
			canSlowJump = true;

			// apply local vertical velocity
			verticalVelocity = transform.up * jumpSpeed;
		}
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

		//Debug.LogFormat("position: {0} - collider: {1} - offset {2} - center: {3}", transform.position, collider2d.transform.position, collider2d.offset, collider2d.bounds.center);

		// reset parameters
		grounded = false;
		
		for (int i = 0; i < raycastRotations.Length; i++)
		{
			/*
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
			*/

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

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Enemy")
		{
			//Debug.LogFormat("gameObject: {0}, other name: {1}, other tag: {2}", gameObject.name, other.name, other.tag);
		}
	}

}
