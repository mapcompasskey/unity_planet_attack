using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	
	// public references
	public LayerMask groundLayer;
	
	// private references
	private Rigidbody2D rb2d;
	private CircleCollider2D collider2d;
	
	// booleans
	private bool facingRight = true;
	private bool walking = false;
	
	private bool grounded = false;
	private bool groundedOnSlope = false;
	private bool groundedOnSteepSlope = false;
	
	private bool jumping = false; // is jumping
	private bool canJump = true; // can jump
	private bool canJump2 = false; // can double jump
	private bool canJump3 = false; // can triple jump
	private bool canSlowJump = false; // can reduce jump acceleration
	private bool jumpButtonState = false; // is jump button up or down
	
	// floats
	private float groundedSlopeAngle = 0f;
	private float groundedSlopeNormalX = 0f;
	private float maximumSlopeAngle = 70f;
	private float moveSpeed = 8f;
	private float jumpSpeed = 16f;//8f;
	private float horizontalAxis = 0;
	private float maxVelocityX = 1f;
	private float maxVelocityY = 1f;
	private float moveSlopeFriction = 0f;
	
	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<CircleCollider2D>();
		
		// update max velocities
		maxVelocityX = moveSpeed * 2;
		maxVelocityY = jumpSpeed * 2;
		
		// set the friction applied when moving up slopes
		moveSlopeFriction = (maximumSlopeAngle / 90) / maximumSlopeAngle;
	}
	
	// called every frame
	// used for regular updates such as: moving non-physics objects, simple timers, recieving inputs
	// update interval time varies
	void Update()
	{
		// left or right axis of controls
		horizontalAxis = Input.GetAxisRaw("Horizontal");
		
		// is jump button down
		//jumpButtonState = Input.GetButton("Jump");
		jumpButtonState = Input.GetKey(KeyCode.X);
		
		checkForGround();
	}
	
	// called every physics step
	// fixed update intervals are consistent
	// used for regular updates such as: adjusting physics (Rigidbody) objects
	void FixedUpdate ()
	{
		isJumping();
		isWalking();
		
		// keep movement within velocity limits
		rb2d.velocity = new Vector2(Mathf.Clamp(rb2d.velocity.x, -maxVelocityX, maxVelocityX), Mathf.Clamp(rb2d.velocity.y, -maxVelocityY, maxVelocityY));
	}
	
	void isJumping()
	{
		if (groundedOnSteepSlope)
		{
			canJump = canJump2 = canJump3 = false;
			return;
		}
		
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
			canJump2 = false;
			canJump3 = false;
		}
		
		// reduce jumping acceleration
		if (canSlowJump && jumping && ! jumpButtonState)
		{
			canSlowJump = false;
			if (rb2d.velocity.y >= 0)
			{
				rb2d.velocity = new Vector2(rb2d.velocity.x, (rb2d.velocity.y / 2));
			}
		}
		
		// start jumping
		if (canJump && grounded && ! jumping && jumpButtonState)
		{
			canJump = false;
			canJump2 = true;
			jumping = true;
			canSlowJump = true;
			rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
		}
		
		// start double jump (after canSlowJump)
		if (canJump2 && ! canSlowJump && ! grounded && jumping && jumpButtonState)
		{
			canJump2 = false;
			canJump3 = true;
			canSlowJump = true;
			rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
		}
		
		// start triple jump (after canSlowJump)
		if (canJump3 && ! canSlowJump && ! grounded && jumping && jumpButtonState)
		{
			canJump3 = false;
			canSlowJump = true;
			rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
		}
	}
	
	void isWalking()
	{
		if (groundedOnSteepSlope)
		{
			return;
		}
		
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
		
		// move left or right
		rb2d.velocity = new Vector2(horizontalAxis * moveSpeed, rb2d.velocity.y);
		
		// apply movement friction on slopes
		if (groundedOnSlope && rb2d.velocity.x != 0)
		{
			bool movingRight = (rb2d.velocity.x > 0 ? true : false);
			bool slopeRisingRight = (groundedSlopeNormalX < 0 ? true : false);
			if ((movingRight && slopeRisingRight) || ( ! movingRight && ! slopeRisingRight))
			{
				float moveSlopeFrictionRate = 1f - (groundedSlopeAngle * moveSlopeFriction);
				rb2d.velocity = new Vector2(rb2d.velocity.x * moveSlopeFrictionRate, rb2d.velocity.y);
			}
		}
		
		// prevent the little hop that happens when moving up a slope and stopping
		if (groundedOnSlope && ! walking && ! jumping && rb2d.velocity.y > 0)
		{
			rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
		}
		
		/*
		if (groundedOnSlope)
		{
			float slopeFriction = 0.01f;
			if (Mathf.Abs(groundedSlopeNormalX) > 0.1f)
			{
				// Apply the opposite force against the slope force 
				// You will need to provide your own slopeFriction to stabalize movement
				rb2d.velocity = new Vector2(rb2d.velocity.x - (groundedSlopeNormalX * slopeFriction), rb2d.velocity.y);
				
				//Move Player up or down to compensate for the slope below them
				Vector3 pos = transform.position;
				pos.y += -groundedSlopeNormalX * Mathf.Abs(rb2d.velocity.x) * Time.deltaTime * (rb2d.velocity.x - groundedSlopeNormalX > 0 ? 1 : -1);
				transform.position = pos;
			}
		}
		*/
	}
	
	void checkForGround()
	{
		Quaternion q;
		Vector2 v;
		RaycastHit2D hit;
		float distance = collider2d.radius + 0.15f;
		float[] raycastRotations = new float[] {40f, 20f, 0f, -20f, -40f};
		
		// reset parameters
		grounded = false;
		groundedOnSlope = false;
		groundedOnSteepSlope = false;
		groundedSlopeAngle = 0;
		groundedSlopeNormalX = 0;
		
		for (int i = 0; i < raycastRotations.Length; i++)
		{
			// raycast out from the center of the circle collider
			q = Quaternion.AngleAxis(raycastRotations[i], Vector3.forward * distance);
			v = q * -Vector3.up;
			
			Debug.DrawRay(transform.position, v * distance, Color.red);
			hit = Physics2D.Raycast(transform.position, v, distance, groundLayer);
			if (hit.collider)
			{
				// prevents collision happening while inside a collider
				if (hit.distance - collider2d.radius > 0)
				{
					grounded = true;
					
					// if standing on a slope
					float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
					if (Mathf.Abs(slopeAngle) > 0)
					{
						groundedOnSlope = true;
						groundedSlopeAngle = slopeAngle;
						groundedSlopeNormalX = hit.normal.x;
						
						// if slope is to steep to stand on
						if (slopeAngle > maximumSlopeAngle)
						{
							groundedOnSteepSlope = true;
						}
					}
					
				}
			}
		}
		
	}
	
	/*
	// when touching another 2D Collider
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.tag == "MovingPlatform")
		{
			// reassign this object as a child of the moving platform
			transform.parent = other.transform;
		}
	}
	
	// when no longer touching another 2D Collider
	void OnCollisionExit2D(Collision2D other)
	{
		if (other.transform.tag == "MovingPlatform")
		{
			// reassign this object as a child of nothing
			transform.parent = null;
		}
	}
	*/
	
}
