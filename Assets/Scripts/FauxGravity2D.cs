using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FauxGravity2D : MonoBehaviour {

	// objects
	public Text debugUpdate;
	public Text debugFixedUpdate;
	private Rigidbody2D rb2d;
	private GameObject planet;

	// floats
	private float gravity = -100f;
	private float moveSpeed = 8f;
	private float jumpSpeed = 16f;

	// strings
	private string strMoveDirection;
	private string strTransDirection;
	private string strTransRotation;
	private string strTargetDirection;
	private string strVelocity;

	// vectors
	private Vector3 moveDirection;

	void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.gravityScale = 0;
		rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	void Start()
	{
		planet = GameObject.FindGameObjectWithTag("Planet");
	}
	
	void Update()
	{
		debugUpdate.text = "";

		/*
		// the direction (in local space) to move
		moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
		if (moveDirection != Vector3.zero)
		{
			strMoveDirection = "moveDirection: " + moveDirection.ToString();
		}
		*/

		// the direction (in local space) to move
		moveDirection = new Vector3(Input.GetAxisRaw("Horizontal") * 5f, Input.GetAxisRaw("Vertical") * 15f, 0);
		if (moveDirection != Vector3.zero)
		{
			strMoveDirection = "moveDirection: " + moveDirection.ToString();
		}

		debugUpdate.text += strMoveDirection;
	}
	
	void FixedUpdate()
	{
		debugFixedUpdate.text = "";

		ApplyGravity();

		/*
		// convert transform from local space to world space since it is being rotated
		Vector3 transDir = transform.TransformDirection(moveDirection);

		// need to convert the current Vector2 to a Vector3 for calculations
		Vector3 pos = new Vector3(rb2d.position.x, rb2d.position.y, 0);

		// move rigid body to position
		rb2d.MovePosition(pos + transDir * moveSpeed * Time.deltaTime);

		if (transDir != Vector3.zero)
		{
			strTransDirection = "transDir: " + transDir;
		}

		debugFixedUpdate.text += strTransDirection + "\n" + strTransRotation;
		*/

		/*
		// convert transform from local space to world space since it is being rotated
		Vector3 transDir = transform.TransformDirection(moveDirection);
		
		// need to convert the current Vector2 to a Vector3 for calculations
		Vector3 currentPos = new Vector3(rb2d.position.x, rb2d.position.y, 0);

		Vector3 targetDir = (transDir - currentPos).normalized;
		rb2d.AddForce(targetDir);
		*/

		/*
		rb2d.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb2d.velocity.y);
		if (Input.GetKeyDown(KeyCode.Space))
		{
			rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
		}
		*/


		//rb2d.AddRelativeForce(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed, Input.GetAxisRaw("Vertical") * 200f));

		/** /
		Vector3 horizontalVel = transform.right * Input.GetAxisRaw("Horizontal") * moveSpeed;
		Vector3 forwardVel = transform.up * Input.GetAxisRaw("Vertical") * jumpSpeed;
		rb2d.velocity = forwardVel + horizontalVel;
		strVelocity = "velocity: " + rb2d.velocity.ToString();
		/**/

		// transform.right returns this objects local "right" direction as a vector in world space
		// transform.up returns this objects local "up" direction as a vector in world space

		// apply local horizontal velocity
		Vector3 horizontalVel = transform.right * Input.GetAxisRaw("Horizontal") * moveSpeed;

		// get the local y velocity
		Vector3 verticalVel = transform.up * transform.InverseTransformDirection(rb2d.velocity).y;

		// if the JUMP key was pressed
		if (Input.GetKeyDown(KeyCode.Space))
		{
			// apply local vertical velocity
			verticalVel = transform.up * jumpSpeed;
		}

		// set the current velocity
		rb2d.velocity = horizontalVel + verticalVel;
		strVelocity = "velocity: " + rb2d.velocity.ToString();
		
		debugFixedUpdate.text += strTransDirection + "\n" + strTargetDirection + "\n" + strTransRotation + "\n" + strVelocity;
	}

	private void ApplyGravity()
	{
		Vector2 targetDir = (transform.position - planet.transform.position).normalized;
		strTargetDirection = "target direction: " + targetDir.ToString();

		Vector2 bodyUp = transform.up;

		// rotate the transform so it is always pointing away from the planet
		transform.rotation = Quaternion.FromToRotation(bodyUp, targetDir) * transform.rotation;
		strTransRotation = "rotation: " + transform.rotation.eulerAngles;

		// apply the faux gravity force
		rb2d.AddForce(targetDir * gravity);
	}
}
