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

	}
	
	void FixedUpdate()
	{
		ApplyGravity();

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
	}

	private void ApplyGravity()
	{
		Vector2 targetDir = (transform.position - planet.transform.position).normalized;
		Vector2 bodyUp = transform.up;

		// rotate the transform so it is always pointing away from the planet
		transform.rotation = Quaternion.FromToRotation(bodyUp, targetDir) * transform.rotation;

		// apply the faux gravity force
		rb2d.AddForce(targetDir * gravity);
	}
}
