using UnityEngine;
using System.Collections;

public class FauxGravity2D : MonoBehaviour {

	private float gravity = -600f;
	private float moveSpeed = 15f;
	private Vector3 moveDirection;
	private Rigidbody2D rb2d;
	private GameObject planet;

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
		moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
	}
	
	void FixedUpdate()
	{
		Vector3 pos = new Vector3(rb2d.position.x, rb2d.position.y, 0);
		rb2d.MovePosition(pos + transform.TransformDirection(moveDirection) * moveSpeed * Time.fixedDeltaTime);
		ApplyGravity();
	}

	private void ApplyGravity()
	{
		Vector2 targetDir = (transform.position - planet.transform.position).normalized;
		Vector2 bodyUp = transform.up;
		
		transform.rotation = Quaternion.FromToRotation(bodyUp, targetDir) * transform.rotation;
		rb2d.AddForce(targetDir * gravity);
	}
}
