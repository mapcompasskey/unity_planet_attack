using UnityEngine;
using System.Collections;

public class PersonController2D : MonoBehaviour {

	private float moveSpeed = 5f;
	private Vector3 moveDirection;
	public Rigidbody2D rb2d;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
	}
	
	void FixedUpdate()
	{
		Vector3 pos = new Vector3(rb2d.position.x, rb2d.position.y, 0);
		rb2d.MovePosition(pos + transform.TransformDirection(moveDirection) * moveSpeed * Time.fixedDeltaTime);
	}
}
